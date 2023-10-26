using ControlManager;
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
    }
}