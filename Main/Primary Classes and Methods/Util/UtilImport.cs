using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thumper_Custom_Level_Editor.Properties;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilImport
    {
        public static void ImportInit()
        {
            ImportQuickValues();
            ImportObjects();
            ImportDefaultColors();
            GetThumperCacheFolder(true);
        }

        public static void ImportQuickValues()
        {
            string path = Path.Combine(TCLE.AppLocation, "settings", "quickvalues.txt");
            if (!File.Exists(path))
                return;

            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < TCLE.LeafQuickValues.Length; i++) {
                TCLE.LeafQuickValues[i] = i < lines.Length && decimal.TryParse(lines[i], System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value) ? value : 1.000m;
            }
        }

        public static void ImportObjects()
        {
            TCLE.LeafObjects.Clear();
            //check if the track_objects exists or not, but do not overwrite it
            string _trackobjectspath = Path.Combine(TCLE.AppLocation, "settings", "track_objects_v4.txt");
            if (!File.Exists(_trackobjectspath)) {
                using (StreamWriter sw = File.CreateText(_trackobjectspath)) {
                    sw.Write(Properties.Resources.trackobjects_v4);
                }
            }
            //import selectable objects from file and parse them into lists for manipulation
            string[] _importedObjects = File.ReadAllLines(_trackobjectspath);
            TCLE.LeafObjects = _importedObjects.Select(x => x.Split(';'))
                                        .Select(x => new KeyValuePair<string, DefaultSequencerObject>(x[1] + ";" + x[3], new DefaultSequencerObject {
                                            Category = x[0],
                                            Name = x[1],
                                            ParamDisplayName = x[2],
                                            ParamPath = x[3],
                                            TraitType = DefaultSequencerObject.TraitLookup[x[4]],
                                            Step = x[5] == "True",
                                            DefaultValue = decimal.TryParse(x[6], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal _result) ? _result : 0,
                                            Footer = x[7].Trim('[', ']'),
                                            DefaultColor = Color.Purple
                                        })).ToDictionary();

            TCLE.LeafObjects.Add("_TuningLayerX;⮝ Tuning Layer X", new DefaultSequencerObject {
                Category = "",
                Name = "_TuningLayerX",
                ParamDisplayName = "⮝ Tuning Layer X",
                ParamPath = "⮝ Tuning Layer X",
                TraitType = DefaultSequencerObject.TraitLookup[""],
                Step = false,
                DefaultValue = 0m,
                Footer = "",
                DefaultColor = Color.FromArgb(40, 40, 40)
            });
            //import default colors per object
            ImportDefaultColors();
            //import favorites
            if (Properties.Settings.Default.SequencerFavorites != null) {
                foreach (string key in Properties.Settings.Default.SequencerFavorites)
                    TCLE.LeafObjects[key].Favorite = true;
            }
            //ObjectFavorites = LeafObjects.Where(x => Properties.Settings.Default.SequencerFavorites.Contains(x.Key)).ToDictionary();
        }

        public static void ImportDefaultColors()
        {
            String _pathdefaultcolors = Path.Combine(TCLE.AppLocation, "settings", "objects_defaultcolors_v3.txt");
            Dictionary<string, Color> ObjectColors = new();
            if (!File.Exists(_pathdefaultcolors)) {
                File.WriteAllText(_pathdefaultcolors, Resources.objects_defaultcolors);
            }
            ObjectColors = File.ReadAllLines(_pathdefaultcolors).Select(line => line.Split(';')).ToDictionary(part => part[0], part => Color.FromArgb(int.Parse(part[1])));

            ///colorDialog1.CustomColors = Properties.Settings.Default.colordialogcustomcolors?.ToArray() ?? new[] { 1 };
            //once all the colors are processed, assign them directly to the objects
            foreach (DefaultSequencerObject obj in TCLE.LeafObjects.Select(x => x.Value)) {
                obj.DefaultColor = ObjectColors.TryGetValue(obj.ParamDisplayName, out Color value) ? value : Color.Purple;
                string _colorkey = obj.DefaultColor.ToArgb().ToString();
                if (!TCLE.ColorIcons.ContainsKey(_colorkey)) {
                    Bitmap color = new(16, 16);
                    using (Graphics g = Graphics.FromImage(color)) {
                        g.Clear(obj.DefaultColor);
                    }
                    TCLE.ColorIcons[_colorkey] = color;
                }
            }

            foreach (KeyValuePair<string, Bitmap> _ColorIcon in TCLE.ColorIcons) {
                TCLE.Instance.imageListCategoryIcons.Images.Add(_ColorIcon.Key, _ColorIcon.Value);
            }
        }

        public static void GetThumperCacheFolder(bool init = false)
        {
            if (init && Settings.Default?.game_dir != "none")
                return;

            CommonOpenFileDialog cfd_lvl = new() {
                IsFolderPicker = true,
                Multiselect = false,
                Title = "Select the folder where Thumper is installed (NOT the cache folder)"
            };
            //check if the game_dir has been set before. It'll be empty if starting for the first time
            if (Settings.Default?.game_dir == "none")
                cfd_lvl.InitialDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\Thumper";
            else
                //if it's not empty, initialize the FolderBrowser to be whatever was selected last
                cfd_lvl.InitialDirectory = Settings.Default?.game_dir;
            //show FolderBrowser, and then set "game_dir" to whatever is chosen
            if (cfd_lvl.ShowDialog() == CommonFileDialogResult.Ok)
                Settings.Default?.game_dir = cfd_lvl.FileName;

            Settings.Default?.Save();
        }
    }
}
