using System.ComponentModel;
using System.Reflection;

namespace Thumper_Custom_Level_Editor
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SettingNameAttribute : Attribute
    {
        public string Name { get; }

        public SettingNameAttribute(string name)
        {
            Name = name;
        }
    }

    public class SettingsUITheme
    {
        public SettingsUITheme()
        {
            Load();
            /*
            mainmenubar = Properties.Settings.Default.ColorMainMenuBar;
            mainsubmenubar = Properties.Settings.Default.ColorMainSubMenubar;
            mainbg = Properties.Settings.Default.ColorMainBG;
            leafbg = Properties.Settings.Default.ColorLeafBG;
            leafseqbg = Properties.Settings.Default.ColorLeafSeqBG;
            leaftimesig1 = Properties.Settings.Default.ColorLeafTimeSig1;
            leaftimesig2 = Properties.Settings.Default.ColorLeafTimeSig2;
            leafrawbg = Properties.Settings.Default.ColorLeafRawBG;
            leafrawtext = Properties.Settings.Default.ColorLeafRawText;
            lvlbg = Properties.Settings.Default.ColorLvlBG;
            lvlleafbg = Properties.Settings.Default.ColorLvlLeafBG;
            lvltunnelsbg = Properties.Settings.Default.ColorLvlTunnelBG;
            lvlloopsbg = Properties.Settings.Default.ColorLvlLoopsBG;
            gatebg = Properties.Settings.Default.ColorGateBG;
            gatelvlbg = Properties.Settings.Default.ColorGateLvlBG;
            masterbg = Properties.Settings.Default.ColorMasterBG;
            masterlvlbg = Properties.Settings.Default.ColorMasterLvlBG;
            samplebg = Properties.Settings.Default.ColorSampleBG;
            samplelistbg = Properties.Settings.Default.ColorSampleListBG;
            samplewaveformbg = Properties.Settings.Default.ColorWaveformBG;
            projectexplorerbg = Properties.Settings.Default.ColorProjectExplorerBG;
            projectexplorerhighlight = Properties.Settings.Default.ColorProjExpHighlight;
            projectexplorertext = Properties.Settings.Default.ColorProjExpText;
            rawbg = Properties.Settings.Default.ColorRawBG;
            rawtext = Properties.Settings.Default.ColorRawText;
            tuningbg = Properties.Settings.Default.ColorTuningBG;
            tuningline = Properties.Settings.Default.ColorTuningLine;
            tuningpoint = Properties.Settings.Default.ColorTuningPoint;
            tuningmaxmin = Properties.Settings.Default.ColorTuningMaxMin;
            tuningfont = Properties.Settings.Default.ColorTuningFont;
            basiceditorbg = Properties.Settings.Default.ColorLeafBasicBG;
            basiceditorgrid = Properties.Settings.Default.ColorLeafBasicGrid;
            */
        }

        public void Load()
        {
            var settings = Properties.Settings.Default;

            foreach (var property in GetType().GetProperties()) {
                var attr = property.GetCustomAttribute<SettingNameAttribute>();
                if (attr == null)
                    continue;

                property.SetValue(this, settings[attr.Name]);
            }
        }

        public void Save()
        {
            var settings = Properties.Settings.Default;

            foreach (var property in GetType().GetProperties()) {
                var attr = property.GetCustomAttribute<SettingNameAttribute>();
                if (attr == null)
                    continue;

                settings[attr.Name] = property.GetValue(this);
            }
            settings.Save();
        }
        /*
        public void SaveSettings()
        {
            Properties.Settings.Default.ColorMainMenuBar = mainmenubar;
            Properties.Settings.Default.ColorMainSubMenubar = mainsubmenubar;
            Properties.Settings.Default.ColorMainBG = mainbg;
            Properties.Settings.Default.ColorLeafBG = leafbg;
            Properties.Settings.Default.ColorLeafSeqBG = leafseqbg;
            Properties.Settings.Default.ColorLeafTimeSig1 = leaftimesig1;
            Properties.Settings.Default.ColorLeafTimeSig2 = leaftimesig2;
            Properties.Settings.Default.ColorLeafRawBG = leafrawbg;
            Properties.Settings.Default.ColorLeafRawText = leafrawtext;
            Properties.Settings.Default.ColorLvlBG = lvlbg;
            Properties.Settings.Default.ColorLvlLeafBG = lvlleafbg;
            Properties.Settings.Default.ColorLvlTunnelBG = lvltunnelsbg;
            Properties.Settings.Default.ColorLvlLoopsBG = lvlloopsbg;
            Properties.Settings.Default.ColorGateBG = gatebg;
            Properties.Settings.Default.ColorGateLvlBG = gatelvlbg;
            Properties.Settings.Default.ColorMasterBG = masterbg;
            Properties.Settings.Default.ColorMasterLvlBG = masterlvlbg;
            Properties.Settings.Default.ColorSampleBG = samplebg;
            Properties.Settings.Default.ColorSampleListBG = samplelistbg;
            Properties.Settings.Default.ColorWaveformBG = samplewaveformbg;
            Properties.Settings.Default.ColorProjectExplorerBG = projectexplorerbg;
            Properties.Settings.Default.ColorProjExpHighlight = projectexplorerhighlight;
            Properties.Settings.Default.ColorProjExpText = projectexplorertext;
            Properties.Settings.Default.ColorRawBG = rawbg;
            Properties.Settings.Default.ColorRawText = rawtext;
            Properties.Settings.Default.ColorTuningBG = tuningbg;
            Properties.Settings.Default.ColorTuningLine = tuningline;
            Properties.Settings.Default.ColorTuningPoint = tuningpoint;
            Properties.Settings.Default.ColorTuningMaxMin = tuningmaxmin;
            Properties.Settings.Default.ColorTuningFont = tuningfont;
            Properties.Settings.Default.ColorLeafBasicBG = basiceditorbg;
            Properties.Settings.Default.ColorLeafBasicGrid = basiceditorgrid;
        }*/

        ///
        [CategoryAttribute("Main Menu")]
        [DisplayName("Menu Bar")]
        [SettingName(nameof(Properties.Settings.Default.ColorMainMenuBar))]
        public Color mainmenubar { get; set; }

        [CategoryAttribute("Main Menu")]
        [DisplayName("Submenu Bar")]
        [SettingName(nameof(Properties.Settings.Default.ColorMainSubMenubar))]
        public Color mainsubmenubar { get; set; }

        [CategoryAttribute("Main Menu")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorMainBG))]
        public Color mainbg { get; set; }
        ///
        ///
        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafBG))]
        public Color leafbg { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Sequencer Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafSeqBG))]
        public Color leafseqbg { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Sequencer Time Sig. 1")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafTimeSig1))]
        public Color leaftimesig1 { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Sequencer Time Sig. 2")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafTimeSig2))]
        public Color leaftimesig2 { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Raw Text Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafRawBG))]
        public Color leafrawbg { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Raw Text")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafRawText))]
        public Color leafrawtext { get; set; }
        ///
        ///
        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLvlBG))]
        public Color lvlbg { get; set; }

        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Leaf List Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLvlLeafBG))]
        public Color lvlleafbg { get; set; }

        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Tunnels Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLvlTunnelBG))]
        public Color lvltunnelsbg { get; set; }

        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Loop Tracks Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLvlLoopsBG))]
        public Color lvlloopsbg { get; set; }
        ///
        ///
        [CategoryAttribute("Gate Editor")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorGateBG))]
        public Color gatebg { get; set; }

        [CategoryAttribute("Gate Editor")]
        [DisplayName("Lvl List Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorGateLvlBG))]
        public Color gatelvlbg { get; set; }
        ///
        ///
        [CategoryAttribute("Master Editor")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorMasterBG))]
        public Color masterbg { get; set; }

        [CategoryAttribute("Master Editor")]
        [DisplayName("Lvl List Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorMasterLvlBG))]
        public Color masterlvlbg { get; set; }
        ///
        ///
        [CategoryAttribute("Sample Editor")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorSampleBG))]
        public Color samplebg { get; set; }

        [CategoryAttribute("Sample Editor")]
        [DisplayName("Samp List Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorSampleListBG))]
        public Color samplelistbg { get; set; }

        [CategoryAttribute("Sample Editor")]
        [DisplayName("Waveforms Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorWaveformBG))]
        public Color samplewaveformbg { get; set; }
        ///
        ///
        [CategoryAttribute("Project Explorer")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorProjectExplorerBG))]
        public Color projectexplorerbg { get; set; }

        [CategoryAttribute("Project Explorer")]
        [DisplayName("Text Color")]
        [SettingName(nameof(Properties.Settings.Default.ColorProjExpText))]
        public Color projectexplorertext { get; set; }

        [CategoryAttribute("Project Explorer")]
        [DisplayName("Highlight Color")]
        [SettingName(nameof(Properties.Settings.Default.ColorProjExpHighlight))]
        public Color projectexplorerhighlight { get; set; }
        ///
        ///
        [CategoryAttribute("Raw Text Editor")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorRawBG))]
        public Color rawbg { get; set; }

        [CategoryAttribute("Raw Text Editor")]
        [DisplayName("Text")]
        [SettingName(nameof(Properties.Settings.Default.ColorRawText))]
        public Color rawtext { get; set; }
        ///
        ///
        [CategoryAttribute("Tuning Layers")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorTuningBG))]
        public Color tuningbg { get; set; }

        [CategoryAttribute("Tuning Layers")]
        [DisplayName("Graph Line")]
        [SettingName(nameof(Properties.Settings.Default.ColorTuningLine))]
        public Color tuningline { get; set; }

        [CategoryAttribute("Tuning Layers")]
        [DisplayName("Points")]
        [SettingName(nameof(Properties.Settings.Default.ColorTuningPoint))]
        public Color tuningpoint { get; set; }

        [CategoryAttribute("Tuning Layers")]
        [DisplayName("Max and Min Lines")]
        [SettingName(nameof(Properties.Settings.Default.ColorTuningMaxMin))]
        public Color tuningmaxmin { get; set; }

        [CategoryAttribute("Tuning Layers")]
        [DisplayName("Text")]
        [SettingName(nameof(Properties.Settings.Default.ColorTuningFont))]
        public Color tuningfont { get; set; }
        ///
        ///
        [CategoryAttribute("Basic Editor")]
        [DisplayName("Background")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafBasicBG))]
        public Color basiceditorbg { get; set; }

        [CategoryAttribute("Basic Editor")]
        [DisplayName("Grid Color")]
        [SettingName(nameof(Properties.Settings.Default.ColorLeafBasicGrid))]
        public Color basiceditorgrid { get; set; }
        ///
    }
}
