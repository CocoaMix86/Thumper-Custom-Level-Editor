using System.ComponentModel;

namespace Thumper_Custom_Level_Editor
{
    public class SettingsUITheme
    {
        public SettingsUITheme()
        {
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
        }

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
        }

        ///
        [CategoryAttribute("Main Menu")]
        [DisplayName("Menu Bar")]
        public Color mainmenubar { get; set; }

        [CategoryAttribute("Main Menu")]
        [DisplayName("Submenu Bar")]
        public Color mainsubmenubar { get; set; }

        [CategoryAttribute("Main Menu")]
        [DisplayName("Background")]
        public Color mainbg { get; set; }
        ///
        ///
        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Background")]
        public Color leafbg { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Sequencer Background")]
        public Color leafseqbg { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Sequencer Time Sig. 1")]
        public Color leaftimesig1 { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Sequencer Time Sig. 2")]
        public Color leaftimesig2 { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Raw Text Background")]
        public Color leafrawbg { get; set; }

        [CategoryAttribute("Leaf Editor")]
        [DisplayName("Raw Text")]
        public Color leafrawtext { get; set; }
        ///
        ///
        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Background")]
        public Color lvlbg { get; set; }

        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Leaf List Background")]
        public Color lvlleafbg { get; set; }

        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Tunnels Background")]
        public Color lvltunnelsbg { get; set; }

        [CategoryAttribute("Lvl Editor")]
        [DisplayName("Loop Tracks Background")]
        public Color lvlloopsbg { get; set; }
        ///
        ///
        [CategoryAttribute("Gate Editor")]
        [DisplayName("Background")]
        public Color gatebg { get; set; }

        [CategoryAttribute("Gate Editor")]
        [DisplayName("Lvl List Background")]
        public Color gatelvlbg { get; set; }
        ///
        ///
        [CategoryAttribute("Master Editor")]
        [DisplayName("Background")]
        public Color masterbg { get; set; }

        [CategoryAttribute("Master Editor")]
        [DisplayName("Lvl List Background")]
        public Color masterlvlbg { get; set; }
        ///
        ///
        [CategoryAttribute("Sample Editor")]
        [DisplayName("Background")]
        public Color samplebg { get; set; }

        [CategoryAttribute("Sample Editor")]
        [DisplayName("Samp List Background")]
        public Color samplelistbg { get; set; }

        [CategoryAttribute("Sample Editor")]
        [DisplayName("Waveforms Background")]
        public Color samplewaveformbg { get; set; }
        ///
        ///
        [CategoryAttribute("Project Explorer")]
        [DisplayName("Background")]
        public Color projectexplorerbg { get; set; }

        [CategoryAttribute("Project Explorer")]
        [DisplayName("Text Color")]
        public Color projectexplorertext { get; set; }

        [CategoryAttribute("Project Explorer")]
        [DisplayName("Highlight Color")]
        public Color projectexplorerhighlight { get; set; }
        ///
        ///
        [CategoryAttribute("Raw Text Editor")]
        [DisplayName("Background")]
        public Color rawbg { get; set; }

        [CategoryAttribute("Raw Text Editor")]
        [DisplayName("Text")]
        public Color rawtext { get; set; }
        ///
    }
}
