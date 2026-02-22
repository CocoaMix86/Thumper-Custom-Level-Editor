using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilImport
    {
        public static void ImportQuickValues()
        {
            if (!File.Exists($@"{TCLE.AppLocation}\settings\quickvalues.txt"))
                return;
            string[] _load = File.ReadAllLines($@"{TCLE.AppLocation}\settings\quickvalues.txt");

            TCLE.LeafQuickValue0 = decimal.TryParse(_load[0], out decimal result) ? result : 1.000m;
            TCLE.LeafQuickValue1 = decimal.TryParse(_load[1], out result) ? result : 1.000m;
            TCLE.LeafQuickValue2 = decimal.TryParse(_load[2], out result) ? result : 1.000m;
            TCLE.LeafQuickValue3 = decimal.TryParse(_load[3], out result) ? result : 1.000m;
            TCLE.LeafQuickValue4 = decimal.TryParse(_load[4], out result) ? result : 1.000m;
            TCLE.LeafQuickValue5 = decimal.TryParse(_load[5], out result) ? result : 1.000m;
            TCLE.LeafQuickValue6 = decimal.TryParse(_load[6], out result) ? result : 1.000m;
            TCLE.LeafQuickValue7 = decimal.TryParse(_load[7], out result) ? result : 1.000m;
            TCLE.LeafQuickValue8 = decimal.TryParse(_load[8], out result) ? result : 1.000m;
            TCLE.LeafQuickValue9 = decimal.TryParse(_load[9], out result) ? result : 1.000m;
        }

        public static void ImportObjects()
        {
            TCLE.LeafObjects.Clear();
            //check if the track_objects exists or not, but do not overwrite it
            if (!File.Exists($@"{TCLE.AppLocation}\settings\track_objects_v4.txt")) {
                using (StreamWriter sw = File.CreateText($@"{TCLE.AppLocation}\settings\track_objects_v4.txt")) {
                    sw.Write(Properties.Resources.trackobjects_v4);
                }
            }
            //import selectable objects from file and parse them into lists for manipulation
            string[] _importedObjects = File.ReadAllLines($@"{TCLE.AppLocation}\settings\track_objects_v4.txt");
            TCLE.LeafObjects = _importedObjects.Select(x => x.Split(';'))
                                        .Select(x => new KeyValuePair<string, Object_Params>(x[1] + ";" + x[3], new Object_Params {
                                            category = x[0],
                                            obj_name = x[1],
                                            param_displayname = x[2],
                                            param_path = x[3],
                                            trait_type = x[4],
                                            step = x[5] == "True",
                                            default_value = decimal.TryParse(x[6], out decimal _result) ? _result : 0,
                                            footer = x[7].Replace("[", "").Replace("]", ""),
                                            defaultcolor = Color.Purple
                                        })).ToDictionary();

            TCLE.LeafObjects.Add("_TuningLayerX;⮝ Tuning Layer X", new Object_Params {
                category = "",
                obj_name = "_TuningLayerX",
                param_displayname = "⮝ Tuning Layer X",
                param_path = "⮝ Tuning Layer X",
                trait_type = "",
                step = false,
                default_value = 0m,
                footer = "",
                defaultcolor = Color.FromArgb(40, 40, 40)
            });
            //import default colors per object
            ImportDefaultColors();
            //import favorites
            if (Properties.Settings.Default.SequencerFavorites != null) {
                foreach (string key in Properties.Settings.Default.SequencerFavorites)
                    TCLE.LeafObjects[key].favorite = true;
            }
            //ObjectFavorites = LeafObjects.Where(x => Properties.Settings.Default.SequencerFavorites.Contains(x.Key)).ToDictionary();
        }

        public static void ImportDefaultColors()
        {
            Dictionary<string, Color> ObjectColors = new();
            if (!File.Exists($@"{TCLE.AppLocation}\settings\objects_defaultcolors_v3.txt")) {
                File.WriteAllText($@"{TCLE.AppLocation}\settings\objects_defaultcolors_v3.txt", Properties.Resources.objects_defaultcolors);
            }
            ObjectColors = File.ReadAllLines($@"{TCLE.AppLocation}\settings\objects_defaultcolors_v3.txt").ToDictionary(g => g.Split(';')[0], g => Color.FromArgb(int.Parse(g.Split(';')[1])));

            ///colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //once all the colors are processed, assign them directly to the objects
            foreach (Object_Params obj in TCLE.LeafObjects.Select(x => x.Value)) {
                obj.defaultcolor = ObjectColors.TryGetValue(obj.param_displayname, out Color value) ? value : Color.Purple;
                Bitmap color = new(16, 16);
                using (Graphics g = Graphics.FromImage(color)) {
                    g.Clear(value);
                }
                TCLE.ColorIcons.TryAdd(value.ToArgb().ToString(), color);
            }
        }

        public static void GetThumperCacheFolder()
        {
            CommonOpenFileDialog cfd_lvl = new() {
                IsFolderPicker = true,
                Multiselect = false,
                Title = "Select the folder where Thumper is installed (NOT the cache folder)"
            };
            //check if the game_dir has been set before. It'll be empty if starting for the first time
            if (Properties.Settings.Default.game_dir == "none")
                cfd_lvl.InitialDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\Thumper";
            else
                //if it's not empty, initialize the FolderBrowser to be whatever was selected last
                cfd_lvl.InitialDirectory = Properties.Settings.Default.game_dir;
            //show FolderBrowser, and then set "game_dir" to whatever is chosen
            if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok)
                Properties.Settings.Default.game_dir = cfd_lvl.FileName;

            Properties.Settings.Default.Save();
        }
    }
}
