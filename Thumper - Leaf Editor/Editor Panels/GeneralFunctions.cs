﻿using ControlManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using NAudio.Wave;
using NAudio.Vorbis;

namespace Thumper_Custom_Level_Editor
{
    public partial class FormLeafEditor : Form
    {

        Color backcolor = Color.FromArgb(40, 40, 40);

        public void ImportObjects()
        {
            _objects.Clear();
            //check if the track_objects exists or not, but do not overwrite it
            if (!File.Exists(@"templates\track_objects.txt")) {
                File.WriteAllText(@"templates\track_objects.txt", Properties.Resources.track_objects);
            }

            //import default colors per object
            ImportDefaultColors();
            //

            ///import selectable objects from file and parse them into lists for manipulation
            //splits input at "###". Each section is a collection of param_paths
            var import = (File.ReadAllText(@"templates\track_objects.txt")).Replace("\r\n", "\n").Split(new string[] { "###\n" }, StringSplitOptions.None).ToList();
            for (int x = 0; x < import.Count; x++) {
                //split each section into individual lines
                var import2 = import[x].Split('\n').ToList();
                //initialise class so we can add to it

                for (int y = 2; y < import2.Count - 1; y++) {
                    //split each line by ';'. Now each property is separated
                    var import3 = import2[y].Split(';');
                    try {
                        Object_Params objpar = new Object_Params() {
                            category = import2[0],
                            obj_name = import3[0],
                            param_displayname = import3[1],
                            param_path = import3[2],
                            trait_type = import3[3],
                            step = import3[4],
                            def = import3[5],
                            footer = import3[6].Replace("[", "").Replace("]", ""),
                        };
                        //finally, add complete object and values to list
                        _objects.Add(objpar);
                    }
                    catch {
                        _errorlog += "failed to import all properties of param_path " + import3[0] + " of object " + import2[0] + ".\n";
                    }
                }
            }
            //show errors to user if any imports failed
            if (_errorlog.Length > 1) {
                MessageBox.Show(_errorlog);
            }

            _errorlog = "";
            //customize combobox to display the correct content
            dropObjects.DataSource = _objects.Select(x => x.category).Distinct().ToList();
            dropParamPath.DataSource = _objects.Where(obj => obj.category == dropObjects.Text).Select(obj => obj.param_displayname).ToList();
            dropParamPath.Enabled = false;
        }

        ///Color elements based on set properties
        private void ColorFormElements()
        {
            this.BackColor = Properties.Settings.Default.custom_bgcolor;
            menuStrip.BackColor = Properties.Settings.Default.custom_menucolor;
            panelLeaf.BackColor = Properties.Settings.Default.custom_leafcolor;
            panelLevel.BackColor = Properties.Settings.Default.custom_lvlcolor;
            panelGate.BackColor = Properties.Settings.Default.custom_gatecolor;
            panelMaster.BackColor = Properties.Settings.Default.custom_mastercolor;
            panelSample.BackColor = Properties.Settings.Default.custom_samplecolor;
        }

        /// <summary>Blends the specified colors together.</summary>
        /// <param name="color">Color to blend onto the background color.</param>
        /// <param name="backColor">Color to blend the other color onto.</param>
        /// <param name="amount">How much of <paramref name="color"/> to keep,
        /// “on top of” <paramref name="backColor"/>.</param>
        /// <returns>The blended colors.</returns>
        public Color Blend(Color color, Color backColor, double amount)
        {
            byte r = (byte)(color.R * amount + backColor.R * (1 - amount));
            byte g = (byte)(color.G * amount + backColor.G * (1 - amount));
            byte b = (byte)(color.B * amount + backColor.B * (1 - amount));
            return Color.FromArgb(r, g, b);
        }

        private void ClearPanels()
        {
            //clear lists used for storing level data
            _tracks.Clear();
            _lvlleafs.Clear();
            _gatelvls.Clear();
            _masterlvls.Clear();
            _samplelist.Clear();
            //set paths to nothing
            _loadedleaf = null;
            _loadedlvl = null;
            _loadedgate = null;
            _loadedmaster = null;
            _loadedsample = null;
        }

        public static void PlaySound(string audiofile)
        {
            if (Properties.Settings.Default.muteapplication)
                return;
            Stream stream = new MemoryStream((byte[])Properties.Resources.ResourceManager.GetObject(audiofile));
            var vorbisStream = new VorbisWaveReader(stream);
            var waveOut = new WaveOut();
            waveOut.Init(vorbisStream);
            waveOut.Volume = 1;
            waveOut.Play();
        }

        /// Used to allow only numbers and a single decimal during input
        private void NumericInputSanitize(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-') {
                e.Handled = true;
            }

            //only allow `-` at beginning
            if (e.KeyChar == '-' && (sender as TextBox).SelectionStart != 0)
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) {
                e.Handled = true;
            }
        }

        private void Read_Config()
        {
            cfd_lvl.Title = "Select the folder where Thumper is installed (NOT the cache folder)";
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

        public void ImportDefaultColors()
        {
            objectcolors.Clear();
            if (!File.Exists(@"templates\objects_defaultcolors.txt")) {
                File.WriteAllText(@"templates\objects_defaultcolors.txt", Properties.Resources.objects_defaultcolors);
            }
            string[] importcolors = File.Exists($@"templates\objects_defaultcolors.txt") ? File.ReadAllLines($@"templates\objects_defaultcolors.txt") : null;
            foreach (string line in importcolors) {
                var items = line.Split(';');
                objectcolors.Add(new Tuple<string, string>(items[0], items[1]));
            }
        }

        public DataGridViewRow CloneRow(DataGridViewRow dgvr, int cellstocopy)
        {
            DataGridViewRow newdgvr = (DataGridViewRow)dgvr.Clone();
            newdgvr.Cells.Clear();
            for (int x = 0; x < cellstocopy && x < dgvr.Cells.Count; x++) {
                newdgvr.Cells.Add((DataGridViewCell)dgvr.Cells[x].Clone());
                newdgvr.Cells[x].Value = dgvr.Cells[x].Value;
            }
            return newdgvr;
        }


        private void RandomizeRowValues(DataGridViewRow dgvr)
        {
            int idx = dgvr.Index;
            foreach (DataGridViewCell dgvc in dgvr.Cells) {
                trackEditor.CellValueChanged -= trackEditor_CellValueChanged;
                dgvc.Value = null;
                if (_tracks[idx].trait_type == "kTraitBool" || _tracks[idx].trait_type == "kTraitAction" || _tracks[idx].param_path == "visibla01" || _tracks[idx].param_path == "visibla02" || _tracks[idx].param_path == "visible" || _tracks[idx].param_path == "visiblz01" || _tracks[idx].param_path == "visiblz02") {
                    if (_tracks[idx].obj_name == "sentry.spn")
                        dgvc.Value = rng.Next(0, 40) == 39 ? 1 : null;
                    else
                        dgvc.Value = rng.Next(0, 10) == 9 ? 1 : null;
                }
                else if (_tracks[idx].trait_type == "kTraitColor") {
                    dgvc.Value = rng.Next(0, 10) >= 5 ? Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256)).ToArgb() : null;
                }
                else {
                    if (_tracks[idx].param_path == "sequin_speed")
                        dgvc.Value = rng.Next(0, 10) >= 9 ? (Math.Truncate(rng.NextDouble() * 10000) / 100) % 4 : null;
                    else if (_tracks[idx].obj_name == "fade.pp")
                        dgvc.Value = rng.Next(0, 10) >= 9 ? rng.NextDouble() : null;
                    else
                        dgvc.Value = rng.Next(0, 10) >= 9 ? (Math.Truncate(rng.NextDouble() * 10000) / 100) * (rng.Next(0, 1) == 0 ? 1 : -1) : null;
                }

                trackEditor.CellValueChanged += trackEditor_CellValueChanged;
            }

            TrackUpdateHighlighting(trackEditor.CurrentRow);
        }

        private ToolStripDropDown CreateUndoMenu(List<SaveState> undolist)
        {
            ToolStripDropDown menu = new ToolStripDropDown();
            menu.BackColor = Color.FromArgb(40, 40, 40);
            foreach (SaveState s in undolist) {
                ToolStripMenuItem tmsi = new ToolStripMenuItem();
                tmsi.Text = s.reason;
                tmsi.MouseEnter += undoMenu_MouseEnter;
                tmsi.BackColor = Color.FromArgb(40, 40, 40);
                tmsi.ForeColor = Color.White;
                menu.Items.Add(tmsi);
            }
            return menu;
        }
        private void undoMenu_MouseEnter(object sender, EventArgs e)
        {
            backcolor = Color.FromArgb(40, 40, 40);
            var parent = ((ToolStripMenuItem)sender).Owner;
            for (int x = parent.Items.Count - 1; x >= 0; x--) {
                parent.Items[x].BackColor = backcolor;
                if (parent.Items[x] == sender)
                    backcolor = Color.Maroon;
            }
        }
    }

    public static class ExtensionMethodClass
    {
        public static string FirstLetterToUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string ToTitleCase(this string str)
        {
            var firstword = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
            //str = str.Replace(str.Split(' ')[0], firstword);
            return firstword;
        }
    }
}