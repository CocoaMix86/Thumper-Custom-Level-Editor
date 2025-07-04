using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static Un4seen.Bass.Misc.WaveForm.WaveBuffer;

namespace Thumper_Custom_Level_Editor.Shared_Classes_and_Methods
{
    public static class BuildObjlib
    {
        static public JsonLoadSettings jo = new() { CommentHandling = CommentHandling.Load };

        static List<string> file_types = new() { ".gate", ".leaf", ".lvl", ".master", ".xfm", ".config" };
        static List<string> file_special = new() { ".spn", ".samp" };
        static List<string> trait_types = new() {
            "kTraitInt",
            "kTraitBool",
            "kTraitFloat",
            "kTraitColor",
            "kTraitObj",
            "kTraitVec3",
            "kTraitPath",
            "kTraitEnum",
            "kTraitAction",
            "kTraitObjVec",
            "kTraitString",
            "kTraitCue",
            "kTraitEvent",
            "kTraitSym",
            "kTraitList",
            "kTraitTraitPath",
            "kTraitQuat",
            "kTraitChildLib",
            "kTraitComponent",
            "kNumTraitTypes"
        };

        static List<string> obj_types = new() {
            "SequinLeaf",
            "SequinLevel",
            "SequinGate",
            "SequinMaster",
            "EntitySpawner",
            "Sample",
            "Xfmer"
        };

        ///
        /// The primary code block that turns the custom level data into a format the game can read
        ///
        public static void Make_Custom_Level(ProjectProperties LevelExport)
        {
            dynamic? level_config = null;
            List<dynamic> objs = new();
            int obj_count = 0;
            dynamic? new_objs = null;
            string errorlist = "";

            //if pyramid exists... delete it
            if (LevelExport.WorkingFolder.GetFiles("pyramid_outro.leaf", SearchOption.AllDirectories).FirstOrDefault() is FileInfo pyramid) {
                pyramid.Delete();
            }
            //load pyramid leaf
            new_objs = JsonConvert.DeserializeObject(Properties.Resources.leaf_pyramid_outro);
            objs.Add(new_objs);
            obj_count++;
            //iterate over each file in the custom level directory
            //filter out files that do not match the <file_types> list
            foreach (FileInfo FileInProject in LevelExport.WorkingFolder.GetFiles("*.*", SearchOption.AllDirectories)) {
                if (file_types.Contains(FileInProject.Extension.ToLower())) {
                    //read file and store JSON in dynamic object
                    try {
                        new_objs = LoadFileLock(FileInProject.FullName);
                    } catch (Exception ex) {
                        errorlist += $"error parsing:\n{ex.Message} in file \"{FileInProject.Name}\"\n\n";
                        continue;
                    }
                    objs.Add(new_objs);
                    obj_count++;
                }
                //these file types require different processing to get the data
                else if (file_special.Contains(FileInProject.Extension.ToLower())) {
                    try {
                        new_objs = LoadFileLock(FileInProject.FullName);
                    } catch (Exception ex) {
                        errorlist += $"error parsing:\n{ex.Message} in file \"{FileInProject.Name}\"\n\n";
                        continue;
                    }
                    //spn_ and samp_ files contain multiple entries, inside the "multi":[] list
                    foreach (var _v in new_objs.items) {
                        objs.Add(_v);
                        obj_count++;
                    }
                }
                else if (FileInProject.Extension.Equals(".tcl", StringComparison.OrdinalIgnoreCase)) {
                    level_config = LoadFileLock(FileInProject.FullName);
                }
            }
            
            //if errors exist, show the error, then return to stop further processing
            if (errorlist.Contains("error parsing")) {
                MessageBox.Show("PROJECT FAILED TO EXPORT\n\n" + errorlist, "Thumper Custom Level Editor");
                return;
            }

            byte[] bytes;
            #region Write Objlib
            var cache_filename = $@"{TCLE.AppLocation}\temp\{LevelExport.projectname}.objlib";
            using (FileStream f = File.Open(cache_filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
                //write header information to level .pc file
                ///bytes = File.ReadAllBytes(@"lib/header.objlib");
                bytes = Properties.Resources.header;
                f.Write(bytes, 0, bytes.Length);

                //write objlib path to level .pc file
                Write_String(f, $"levels/custom/{LevelExport.projectname}.objlib");

                //write "basic list of objects #1" information to level .pc file
                ///bytes = File.ReadAllBytes(@"lib/obj_list_1.objlib");
                bytes = Properties.Resources.obj_list_1;
                f.Write(bytes, 0, bytes.Length);
                //write to file the amount of objects that exists (after this number)
                //this includes everything in "obj_list_2.objlib" (63) and obj_count
                Write_Int(f, 63 + obj_count);
                //write "basic list of objects #2" information to level .pc file
                ///bytes = File.ReadAllBytes(@"lib/obj_list_2.objlib");
                bytes = Properties.Resources.obj_list_2;
                f.Write(bytes, 0, bytes.Length);
                //write every object to the .pc file, hashing its name
                foreach (var obj in objs) {
                    if (obj_types.Contains((string)obj["obj_type"])) {
                        if ((string)obj["obj_type"] != "Xfmer") {
                            Write_Hash(f, (string)obj["obj_type"]);
                            Write_String(f, (string)obj["obj_name"]);
                        }
                        else {
                            Write_Hash(f, (string)obj["obj_type"]);
                            Write_String(f, $"levels/custom/{LevelExport.projectname}.xfm");
                        }
                    }
                }

                ///bytes = File.ReadAllBytes($@"lib/obj_def_customlevel.objlib");
                bytes = Properties.Resources.obj_def_customlevel;
                f.Write(bytes, 0, bytes.Length);
                //iterate over every loaded object, and write its data to .pc file in specific formats.
                //format is different per object. I myself am not exactly sure how it works, but this is how it's done
                foreach (var obj in objs) {
                    if (obj["obj_type"] == "SequinLeaf")
                        Write_Leaf(f, obj);
                    else if (obj["obj_type"] == "SequinLevel") {
                        Write_Lvl_Header(f);
                        Write_Approach_Anim_Comp(f, obj);
                        Write_Lvl_Comp(f, obj);
                        Write_Lvl_Footer(f, obj);
                    }
                    else if (obj["obj_type"] == "SequinGate")
                        Write_Gate(f, obj);
                    else if (obj["obj_type"] == "SequinMaster")
                        Write_Master(f, obj);
                    else if (obj["obj_type"] == "EntitySpawner")
                        Write_Spn(f, obj);
                    else if (obj["obj_type"] == "Sample")
                        Write_Samp(f, obj);
                    else if (obj["obj_type"] == "Xfmer") {
                        Write_Xfm_Header(f);
                        Write_Xfm_Comp(f, obj);
                    }
                }

                //close out file with footer info #1
                ///bytes = File.ReadAllBytes(@"lib/footer_1.objlib");
                bytes = Properties.Resources.footer_1;
                f.Write(bytes, 0, bytes.Length);
                //write file bpm
                Write_Float(f, (float)level_config["bpm"]);
                //close out file with footer info #2
                ///bytes = File.ReadAllBytes(@"lib/footer_2.objlib");
                bytes = Properties.Resources.footer_2;
                f.Write(bytes, 0, bytes.Length);
            }
            #endregion
            #region Write Sec
            var config_cache_filename = $@"{TCLE.AppLocation}\temp\{LevelExport.projectname}.sec";
            using (FileStream f = File.Open(config_cache_filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
                Write_Int(f, 9);
                Write_Int(f, level_config["level_sections"].Count);
                foreach (var level_section in level_config["level_sections"]) {
                    Write_String(f, (string)level_section);
                }
                Write_Color(f, level_config["rails_color"]);
                Write_Color(f, level_config["rails_glow_color"]);
                Write_Color(f, level_config["path_color"]);
                Write_Color(f, level_config["joy_color"]);
            }
            #endregion
        }


        /// 
        /// Methods below are used by the main block to manipulate the level data into the correct forms
        /// And to write data to files
        ///
        private static void Write_Param_Path(FileStream f, string param_path, string param_path_hash)
        {
            Write_Int(f, 1);
            string param;
            string param_name;
            string param_idx;

            if (!string.IsNullOrEmpty(param_path))
                param = param_path;
            else
                param = param_path_hash;
            //a few specific param paths have a ',' followed by a number. In these special cases, split on ','
            //[0] is the param_name and [1] is the value
            if (param.Contains(",")) {
                var _p = param.Split(',');
                param_name = _p[0];
                param_idx = _p[1];
            }
            //if the param_path does not have a ',', idx is -1
            else {
                param_name = param;
                param_idx = "-1";
            }
            //depending if the string is plain text or hex-hash, write it to .pc file differently
            if (!string.IsNullOrEmpty(param_path))
                Write_Hash(f, param_name);
            else
                Write_Hex_Reverse(f, param_name);

            Write_Int(f, int.Parse(param_idx));
        }

        private static void Write_Data_Point_Value(FileStream f, string val, string trait_type)
        {
            if (trait_type == "kTraitInt")
                Write_Int(f, int.Parse(val));
            else if (trait_type == "kTraitBool" || trait_type == "kTraitAction")
                Write_Bool(f, val);
            else if (trait_type == "kTraitFloat")
                Write_Float(f, float.Parse(val));
            else if (trait_type == "kTraitColor") {
                Color _c = Color.FromArgb(int.Parse(val));
                Write_Float(f, _c.R != 0 ? _c.R / 255f : 0);
                Write_Float(f, _c.G != 0 ? _c.G / 255f : 0);
                Write_Float(f, _c.B != 0 ? _c.B / 255f : 0);
                Write_Float(f, _c.A != 0 ? _c.A / 255f : 0);
                //Write_Float(f, 4278190335);
            }
        }

        static string interpv3 = "kTraitInterpLinear";
        static string easev3 = "kEaseInOut";
        private static void Write_Sequencer_Objects(FileStream f, dynamic obj)
        {
            int beat_cnt = obj["beat_cnt"] ?? 0;
            dynamic seq_objs = obj["seq_objs"];
            //write amount of seq_objs (different tracks) to .pc file
            Write_Int(f, seq_objs.Count);

            foreach (var _obj in seq_objs) {
                Write_Sequencer_Object_v3(f, _obj, beat_cnt);
                Write_Int(f, 0);

                ///footer of object
                JArray _footer = _obj["footer"].GetType() == typeof(JArray) ? _obj["footer"] : JArray.FromObject(((string)_obj["footer"]).Replace("[", "").Replace("]", "").Replace("'", "").Split(','));
                Write_Int(f, (int)_footer[0]);
                Write_Int(f, (int)_footer[1]);
                Write_Int(f, (int)_footer[2]);
                Write_Int(f, (int)_footer[3]);
                Write_Int(f, (int)_footer[4]);
                Write_String(f, (string)_footer[5]!);
                Write_String(f, (string)_footer[6]!);
                Write_Bool(f, (string)_footer[7]!);
                Write_Bool(f, (string)_footer[8]!);
                Write_Int(f, (int)_footer[9]);
                Write_Float(f, (float)_footer[10]);
                Write_Float(f, (float)_footer[11]);
                Write_Float(f, (float)_footer[12]);
                Write_Float(f, (float)_footer[13]);
                Write_Float(f, (float)_footer[14]);
                Write_Bool(f, (string)_footer[15]!);
                Write_Bool(f, (string)_footer[16]!);
                Write_Bool(f, (string)_footer[17]!);
            }
        }

        private static void Write_Sequencer_Object_v2(FileStream f, dynamic _obj, int beat_cnt)
        {
            ///header of object
            Write_String(f, (string)_obj["obj_name"]);
            Write_Param_Path(f, (string)_obj["param_path"], (string)_obj["param_path_hash"]);
            Write_Int(f, trait_types.IndexOf((string)_obj["trait_type"]));

            ///data points of object
            //
            var interp = _obj.ContainsKey("default_interp") ? (string)_obj["default_interp"] : "kTraitInterpLinear";
            if (interp == null)
                interp = "kTraitInterpLinear";
            var ease = _obj.ContainsKey("default_ease") ? (string)_obj["default_ease"] : "kEaseInOut";
            //Data points written different depending on STEP
            if (_obj["step"] == "True") {
                //STEP true = value updates every beat, and if no value is set for a beat, it'll use _obj.default
                Write_Int(f, beat_cnt);
                for (int i = 0; i < beat_cnt; i++) {
                    Write_Float(f, i);
                    //check if data_points contains an entry for beat `i`. If yes, write it
                    if (_obj["data_points"].ContainsKey(i.ToString()))
                        Write_Data_Point_Value(f, (string)_obj["data_points"][i.ToString()], (string)_obj["trait_type"]);
                    else
                        Write_Data_Point_Value(f, (string)_obj["default"], (string)_obj["trait_type"]);
                    //write these after every beat for some reason
                    Write_String(f, interp);
                    Write_String(f, ease);
                }
            }
            else {
                //STEP false = value interpolates between values set on beats. Default is ignored.
                Write_Int(f, Enumerable.Count<dynamic>(_obj["data_points"]));
                int _iii = 0;
                foreach (var _v in _obj["data_points"]) {
                    JProperty _p = _v;
                    Write_Float(f, float.Parse(_p.Name)); _iii++;
                    Write_Data_Point_Value(f, (string)_v, (string)_obj["trait_type"]);
                    Write_String(f, interp);
                    Write_String(f, ease);
                }
            }
        }

        private static void Write_Sequencer_Object_v3(FileStream f, dynamic _obj, int beat_cnt)
        {
            ///header of object
            Write_String(f, (string)_obj["obj_name"]);
            Write_Param_Path(f, (string)_obj["param_path"], (string)_obj["param_path_hash"]);
            Write_Int(f, trait_types.IndexOf((string)_obj["trait_type"]));

            string traittype = (string)_obj["trait_type"];
            string default_value = (string)_obj["default"];

            ///data points of object
            //
            //Data points written different depending on STEP
            if (_obj["step"] == "True") {
                ///STEP true = value updates every beat, and if no value is set for a beat, it'll use _obj.default
                Write_Int(f, beat_cnt);
                int indexofwrittenbeat = 0;
                for (int i = 0; i < beat_cnt; i++) {
                    Write_Float(f, i);
                    if (_obj["data_points"].Count > indexofwrittenbeat && (int)_obj["data_points"][indexofwrittenbeat]["beat"] == i) {
                        Write_Data_Point_Value(f, (string)_obj["data_points"][indexofwrittenbeat]["value"], traittype);
                        Write_String(f, (string)_obj["data_points"][indexofwrittenbeat]["interp"]);
                        Write_String(f, (string)_obj["data_points"][indexofwrittenbeat]["ease"]);
                        indexofwrittenbeat++;
                    }
                    else {
                        Write_Data_Point_Value(f, default_value, (string)_obj["trait_type"]);
                        Write_String(f, interpv3);
                        Write_String(f, easev3);
                    }
                }
            }
            else {
                ///STEP false = value interpolates between values set on beats. Default is ignored.
                Write_Int(f, Enumerable.Count<dynamic>(_obj["data_points"]));
                foreach (dynamic dp in _obj["data_points"]) {
                    Write_Float(f, (float)dp["beat"]);
                    Write_Data_Point_Value(f, (string)dp["value"], traittype);
                    Write_String(f, (string)dp["interp"]);
                    Write_String(f, (string)dp["ease"]);
                }
            }
        }

        private static void Write_Anim_Comp(FileStream f)
        {
            Write_Hash(f, "AnimComp");
            Write_Int(f, 1);
            Write_Float(f, 0);
            Write_String(f, "kTimeBeats");
        }

        private static void Write_Approach_Anim_Comp(FileStream f, dynamic obj)
        {
            Write_Hash(f, "ApproachAnimComp");
            Write_Int(f, 1);
            Write_Float(f, 0);
            Write_String(f, "kTimeBeats");
            Write_Int(f, 0);
            Write_Int(f, (int)obj["approach_beats"]);
        }

        private static void Write_Xfm_Header(FileStream f)
        {
            Write_Int(f, 4);
            Write_Int(f, 4);
            Write_Int(f, 1);
        }

        private static void Write_Xfm_Comp(FileStream f, dynamic obj)
        {
            Write_Hash(f, "XfmComp");
            Write_Int(f, 1);
            Write_String(f, (string)obj["xfm_name"]);
            Write_String(f, (string)obj["constraint"]);
            Write_Vec3(f, obj["pos"]);
            Write_Vec3(f, obj["rot_x"]);
            Write_Vec3(f, obj["rot_y"]);
            Write_Vec3(f, obj["rot_z"]);
            Write_Vec3(f, obj["scale"]);
        }

        private static void Write_Leaf(FileStream f, dynamic obj)
        {
            ///header
            //honestly don't know what these values do
            Write_Int(f, 34);
            Write_Int(f, 33);
            Write_Int(f, 4);
            Write_Int(f, 2);

            ///Anim_Comp
            Write_Anim_Comp(f);

            ///comp
            Write_Hash(f, "EditStateComp");
            Write_Sequencer_Objects(f, obj);

            ///footer
            int beat_cnt = obj["beat_cnt"];
            Write_Int(f, 0);
            Write_Int(f, beat_cnt);
            for (int i = 0; i < beat_cnt * 3; i++)
                Write_Int(f, 0);
            Write_Int(f, 0);
            Write_Int(f, 0);
            Write_Int(f, 0);
        }

        private static void Write_Lvl_Header(FileStream f)
        {
            //honestly don't know what these values do
            Write_Int(f, 51);
            Write_Int(f, 33);
            Write_Int(f, 4);
            Write_Int(f, 2);
        }

        private static void Write_Lvl_Comp(FileStream f, dynamic obj)
        {
            Write_Hash(f, "EditStateComp");
            Write_Sequencer_Objects(f, obj);

            //.leaf sequence
            Write_Int(f, 0);
            Write_String(f, "kMovePhaseRepeatChild");
            Write_Int(f, 0);
            int last_beat_cnt = 0;
            //iterate over each leaf in the lvl file and write data to file
            foreach (var leaf in obj["leaf_seq"]) {
                Write_Bool(f, "True");
                Write_Int(f, 0);
                Write_Int(f, (int)leaf["beat_cnt"]);
                Write_Bool(f, "False");
                Write_String(f, (string)leaf["leaf_name"]);
                Write_String(f, (string)leaf["main_path"]);
                Write_Int(f, leaf["sub_paths"].Count);
                foreach (var sub_path in leaf["sub_paths"]) {
                    Write_String(f, (string)sub_path);
                    Write_Int(f, 0);
                }
                Write_String(f, "kStepGameplay");
                Write_Int(f, last_beat_cnt);
                Write_Vec3(f, leaf["pos"]);
                Write_Vec3(f, leaf["rot_x"]);
                Write_Vec3(f, leaf["rot_y"]);
                Write_Vec3(f, leaf["rot_z"]);
                Write_Vec3(f, leaf["scale"]);
                Write_Hex(f, "0000");
                last_beat_cnt = (int)leaf["beat_cnt"];
            }

            Write_Bool(f, "False");
            //write loops
            Write_Int(f, (int)obj["loops"].Count);
            foreach (var loop in obj["loops"]) {
                Write_String(f, (string)loop["samp_name"]);
                Write_Int(f, (int)loop["beats_per_loop"]);
                Write_Int(f, 0);
            }
        }

        private static void Write_Lvl_Footer(FileStream f, dynamic obj)
        {
            Write_Bool(f, "False");
            Write_Float(f, (float)obj["volume"]);
            Write_Int(f, 0);
            Write_Int(f, 0);
            Write_String(f, "kNumTraitTypes");
            Write_Bool(f, (string)obj["input_allowed"]);
            Write_String(f, (string)obj["tutorial_type"]);
            Write_Vec3(f, obj["start_angle_fracs"]);
        }

        private static void Write_Gate(FileStream f, dynamic obj)
        {
            ///header
            Write_Int(f, 26);
            Write_Int(f, 4);
            Write_Int(f, 1);

            ///comp
            Write_Hash(f, "EditStateComp");
            Write_String(f, (string)obj["spn_name"]);
            Write_Param_Path(f, (string)obj["param_path"], (string)obj["param_path_hash"]);

            Write_Int(f, obj["boss_patterns"].Count);
            foreach (var boss_pattern in obj["boss_patterns"]) {
                if (boss_pattern.ContainsKey("node_name"))
                    Write_Hash(f, (string)boss_pattern["node_name"]);
                else
                    Write_Hex_Reverse(f, (string)boss_pattern["node_name_hash"]);
                Write_String(f, (string)boss_pattern["lvl_name"]);
                Write_Bool(f, "True");
                Write_String(f, (string)boss_pattern["sentry_type"]);
                Write_Hex(f, "00000000");
                Write_Int(f, (int)boss_pattern["bucket_num"]);
            }

            ///footer
            Write_String(f, (string)obj["pre_lvl_name"]);
            Write_String(f, (string)obj["post_lvl_name"]);
            Write_String(f, (string)obj["restart_lvl_name"]);
            Write_Int(f, 0);
            Write_String(f, (string)obj["section_type"]);
            Write_Float(f, 9);
            Write_String(f, (string)obj["random_type"]);
        }

        private static void Write_Master(FileStream f, dynamic obj)
        {
            ///header
            Write_Int(f, 33);
            Write_Int(f, 33);
            Write_Int(f, 4);
            Write_Int(f, 2);

            ///Anim_Comp
            Write_Anim_Comp(f);

            ///comp
            Write_Hash(f, "EditStateComp");
            Write_Int(f, 0);
            Write_Float(f, (float)300);
            Write_String(f, (string)obj["skybox_name"]);
            Write_String(f, (string)obj["intro_lvl_name"]);

            //lvl/.gate groupings
            int isolated = 0;
            foreach (var grouping in obj["groupings"]) {
                if (grouping["isolate"] == obj["isolate_tracks"])
                    isolated++;
            }
            //Write_Int(f, obj["groupings"].Count);
            Write_Int(f, isolated);
            foreach (var grouping in obj["groupings"]) {
                //If track isolation is enabled, only add the isolated tracks to the level.
                //If it's off, isolate_tracks will be False, and so will all instances grouping["isolate"]
                if (grouping["isolate"] == obj["isolate_tracks"]) {
                    Write_String(f, (string)grouping["lvl_name"]);
                    Write_String(f, (string)grouping["gate_name"]);
                    Write_Bool(f, (string)grouping["checkpoint"]);
                    Write_String(f, (string)grouping["checkpoint_leader_lvl_name"]);
                    Write_String(f, (string)grouping["rest_lvl_name"]);
                    Write_Hex(f, "01000100000001");
                    Write_Bool(f, (string)grouping["play_plus"]);
                }
            }

            ///footer
            Write_Bool(f, "False");
            Write_Bool(f, "True");
            Write_Int(f, 3);
            Write_Int(f, 50);
            Write_Int(f, 8);
            Write_Int(f, 1);
            Write_Float(f, 0.6F);
            Write_Float(f, 0.5F);
            Write_Float(f, 0.5F);
            Write_String(f, (string)obj["checkpoint_lvl_name"]);
            Write_String(f, "path.gameplay");
        }

        private static void Write_Spn(FileStream f, dynamic obj)
        {
            ///header
            Write_Int(f, 1);
            Write_Int(f, 4);
            Write_Int(f, 2);

            ///comp
            Write_Hash(f, "EditStateComp");

            ///xfm comp
            Write_Xfm_Comp(f, obj);

            ///footer
            Write_Int(f, 0);
            Write_String(f, (string)obj["objlib_path"]);
            Write_String(f, (string)obj["bucket"]);
        }

        private static void Write_Samp(FileStream f, dynamic obj)
        {
            ///header
            Write_Int(f, 12);
            Write_Int(f, 4);
            Write_Int(f, 1);

            ///comp
            Write_Hash(f, "EditStateComp");
            Write_String(f, (string)obj["mode"]);
            Write_Int(f, 0);
            Write_String(f, (string)obj["path"]);
            Write_Hex(f, "0000000000");
            Write_Float(f, (float)obj["volume"]);
            Write_Float(f, (float)obj["pitch"]);
            Write_Float(f, (float)obj["pan"]);
            Write_Float(f, (float)obj["offset"]);
            Write_String(f, (string)obj["channel_group"]);
        }

        #region Common Methods
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static uint Hash32(string s)
        {
            //this hashes stuff. Don't know why it does it this why.
            //this is ripped directly from the game's code
            uint h = 0x811c9dc5;
            foreach (char c in s)
                h = ((h ^ c) * 0x1000193) & 0xffffffff;
            h = (h * 0x2001) & 0xffffffff;
            h = (h ^ (h >> 0x7)) & 0xffffffff;
            h = (h * 0x9) & 0xffffffff;
            h = (h ^ (h >> 0x11)) & 0xffffffff;
            h = (h * 0x21) & 0xffffffff;

            return h;
        }

        private static void Write_Int(FileStream f, int val)
        {
            //convert int to bytes and append to file
            byte[] bytes = BitConverter.GetBytes((int)val);
            f.Write(bytes, 0, bytes.Length);
        }

        private static void Write_Bool(FileStream f, string val)
        {
            byte bytes = val == "1" || val == "True" ? (byte)1 : (byte)0;
            f.WriteByte(bytes);
        }

        private static void Write_Bool(FileStream f, bool val)
        {
            byte bytes = val ? (byte)1 : (byte)0;
            f.WriteByte(bytes);
        }

        private static void Write_Float(FileStream f, float val)
        {
            byte[] bytes = BitConverter.GetBytes((float)val);
            f.Write(bytes, 0, bytes.Length);
        }

        private static void Write_Color(FileStream f, dynamic val)
        {
            Write_Float(f, (float)val[0]);
            Write_Float(f, (float)val[1]);
            Write_Float(f, (float)val[2]);
            Write_Float(f, (float)val[3]);
        }

        private static void Write_Vec3(FileStream f, dynamic val)
        {
            Write_Float(f, (float)val[0]);
            Write_Float(f, (float)val[1]);
            Write_Float(f, (float)val[2]);
        }

        private static void Write_String(FileStream f, string val)
        {
            //In the .pc file, strings are preceeded by their length
            Write_Int(f, val.Length);
            f.Write(Encoding.ASCII.GetBytes(val), 0, val.Length);
        }

        private static void Write_Hash(FileStream f, string val)
        {
            //pass the string to the hash function first before writing to file
            byte[] bytes = BitConverter.GetBytes((uint)Hash32(val));
            f.Write(bytes, 0, bytes.Length);
        }

        private static void Write_Hex(FileStream f, string val)
        {
            byte[] bytes = StringToByteArray(val);
            f.Write(bytes, 0, bytes.Length);
        }
        private static void Write_Hex(FileStream f, byte[] val)
        {
            f.Write(val, 0, val.Length);
        }

        private static void Write_Hex_Reverse(FileStream f, string val)
        {
            byte[] bytes = StringToByteArray(val);
            bytes = bytes.Reverse().ToArray();
            f.Write(bytes, 0, bytes.Length);
        }

        public static string DateTime_Ago(DateTime dt)
        {
            // lazy copy paste from stackoverflow
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH) {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 month ago" : months + " months ago";
            }
            else {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "1 year ago" : years + " years ago";
            }
        }

        public static dynamic LoadFileLock(string _selectedfilename)
        {
            dynamic _load;
            ///reference:
            ///https://stackoverflow.com/questions/1389155/easiest-way-to-read-text-file-which-is-locked-by-another-application
            using (var fileStream = new FileStream(_selectedfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream)) {
                _load = JsonConvert.DeserializeObject(Regex.Replace(textReader.ReadToEnd(), "#.*", ""));
            }

            return _load;
        }

        public static List<int> Search(byte[] src, byte[] pattern)
        {
            List<int> indexes = new List<int>();
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            if (pattern.Length == 1) {
                for (int i = 0; i < maxFirstCharSlot; i++) {
                    if (src[i] == pattern[0])
                        indexes.Add(i);
                }
            }
            else {
                for (int i = 0; i < maxFirstCharSlot; i++) {
                    if (src[i] != pattern[0]) // compare only first byte
                        continue;

                    for (int j = pattern.Length - 1; j > 0; j--) {
                        if (src[i + j] != pattern[j])
                            break;
                        if (j == 1) {
                            indexes.Add(i);
                        }
                    }
                }
            }
            return indexes;
        }
        #endregion
    }
}
