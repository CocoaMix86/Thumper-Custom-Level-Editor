using Newtonsoft.Json.Linq;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilMath
    {
        public static void OutputStopwatch(Stopwatch sw, string reason)
        {
            Debug.WriteLine($"{reason}: {sw.ElapsedTicks} ticks");
            sw.Restart();
        }

        /// This also works for negative numbers
        public static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
        public static decimal mod(decimal x, int m)
        {
            decimal r = x % m;
            return r < 0 ? r + m : r;
        }

        /// <summary>Blends the specified colors together.</summary>
        /// <param name="color">Color to blend onto the background color.</param>
        /// <param name="backColor">Color to blend the other color onto.</param>
        /// <param name="amount">How much of <paramref name="color"/> to keep,
        /// “on top of” <paramref name="backColor"/>.</param>
        /// <returns>The blended colors.</returns>
        public static Color Blend(Color color, Color backColor, double amount)
        {
            byte r = (byte)((color.R * amount) + (backColor.R * (1 - amount)));
            byte g = (byte)((color.G * amount) + (backColor.G * (1 - amount)));
            byte b = (byte)((color.B * amount) + (backColor.B * (1 - amount)));
            return Color.FromArgb(r, g, b);
        }

        /// https://stackoverflow.com/questions/3143657/truncate-two-decimal-places-without-rounding#answer-43639947
        public static decimal? TruncateDecimal(decimal? d, byte decimals)
        {
            if (d == null)
                return null;
            decimal r = Math.Round((decimal)d, decimals);

            if (d > 0 && r > d) {
                return r - new decimal(1, 0, 0, false, decimals);
            }
            else if (d < 0 && r < d) {
                return r + new decimal(1, 0, 0, false, decimals);
            }

            return r;
        }

        public static uint Hash32(string s)
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

        public static string HashPCName(string StringToHash)
        {
            string _hashedname = "";
            byte[] hashbytes = BitConverter.GetBytes(Hash32(StringToHash));
            Array.Reverse(hashbytes);
            foreach (byte b in hashbytes)
                _hashedname += b.ToString("X").PadLeft(2, '0').ToLower();
            //if the hashed name starts with a '0', remove it
            if (_hashedname[0] == '0')
                _hashedname = _hashedname[1..];
            return _hashedname;
        }

        public static int ByteSearch(byte[] src, byte[] pattern)
        {
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            for (int i = 0; i < maxFirstCharSlot; i++) {
                if (src[i] != pattern[0]) // compare only first byte
                    continue;

                // found a match on first byte, now try to match rest of the pattern
                for (int j = pattern.Length - 1; j >= 1; j--) {
                    if (src[i + j] != pattern[j]) break;
                    if (j == 1) return i;
                }
            }
            return -1;
        }
        /*
        public static int CalculateSublevelRuntime(MasterLvlData _masterlvl)
        {
            int _beatcount = 0;
            if (_masterlvl.Type == "lvl") {
                if (!ProjectExplorer.TryGetFile(_masterlvl.NameSplitter, out FileInfo lvl))
                    return -1;
                _beatcount += CalculateLvlRuntime(lvl);
            }
            //this section handles gate
            else {
                int gatebeats = CalculateGateRuntimeFromFile(_masterlvl);
                if (gatebeats == -1)
                    return -1;
                else
                    _beatcount += gatebeats;
            }
            if (ProjectExplorer.TryGetFile(_masterlvl.RestLvl, out FileInfo lvlrest))
                _beatcount += CalculateLvlRuntime(lvlrest);

            return _beatcount;
        }

        public static int CalculateGateRuntimeFromFile(MasterLvlData _masterlvl)
        {
            dynamic _load;
            int _beatcount = 0;
            List<int> bucketscounted = new();
            bool israndom;
            //load the gate to then loop through all lvls in it
            if (!ProjectExplorer.TryGetFile(_masterlvl.NameSplitter, out FileInfo gate))
                return -1;
            if (!string.IsNullOrEmpty(_masterlvl.gatesectiontype) && TCLE.CachedRuntimes.TryGetValue(gate.Name, out int runtime)) {
                if (runtime != 0)
                    return runtime;
            }

            _load = UtilFile.LoadFileLock(gate);
            //if gate not found, _load is null. Return -1 to denote this
            if (_load == null)
                return -1;
            _masterlvl.gatesectiontype = (string)_load["section_type"];
            //check if random is enabled on this gate
            israndom = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET";
            //loop through each lvl in gate
            foreach (dynamic _lvl in _load["boss_patterns"]) {
                //attempt to load lvl
                if (ProjectExplorer.TryGetFile((string)_lvl["lvl_name"], out FileInfo lvl)) { 
                    //if random is enabled, count only the first entry in each bucket
                    if (israndom) {
                        if (!bucketscounted.Contains((int)_lvl["bucket_num"])) {
                            bucketscounted.Add((int)_lvl["bucket_num"]);
                            _beatcount += CalculateLvlRuntime(lvl);
                        }
                    }
                    //otherwise count each lvl
                    else
                        _beatcount += CalculateLvlRuntime(lvl);
                }
            }
            //need to also count pre and post lvl
            if (ProjectExplorer.TryGetFile((string)_load["pre_lvl_name"], out FileInfo prelvl))
                _beatcount += CalculateLvlRuntime(prelvl);
            if (ProjectExplorer.TryGetFile((string)_load["post_lvl_name"], out FileInfo postlvl))
                _beatcount += CalculateLvlRuntime(postlvl);            

            TCLE.CachedRuntimes[gate.Name] = _beatcount;
            return _beatcount;
        }

        public static int CalculateLvlRuntime(FileInfo lvl, bool GetApproach = false)
        {
            if (lvl is null)
                return 0;
            if (!GetApproach && TCLE.CachedRuntimes.TryGetValue(lvl.Name, out int runtime)) {
                if (runtime != 0)
                    return runtime;
            }

            int _beatcount = 0;
            //load the lvl and then loop through its leafs to get beat counts
            JObject _load = UtilFile.LoadFileLock(lvl);
            if (_load == null)
                return 0;
            foreach (JObject leaf in _load["leaf_seq"]) {
                if (ProjectExplorer.TryGetFile((string)leaf["leaf_name"], out FileInfo _leaf) && _leaf.Exists) {
                    JObject _loadleaf = UtilFile.LoadFileLock(_leaf);
                    if (_loadleaf != null)
                        _beatcount += (int)_loadleaf["beat_cnt"];
                }
            }
            //every lvl has an approach beats to consider too
            //_beatcount += (int)_load["approach_beats"];
            TCLE.CachedRuntimes[lvl.Name] = _beatcount;
            return _beatcount + (GetApproach ? (int)_load["approach_beats"] : 0);
        }

        public static int CalculateLeafRuntime(FileInfo leaf)
        {
            if (leaf is null)
                return 0;
            JObject _loadleaf = UtilFile.LoadFileLock(leaf);
            int runtime = 0;
            if (_loadleaf is null) {
                TCLE.CachedRuntimes[leaf.Name] = -1;
                runtime = -1;
            }
            else {
                runtime = (int)_loadleaf["beat_cnt"];
                TCLE.CachedRuntimes[leaf.Name] = runtime;
            }
            return runtime;
        }
        */
        public static void RecalculateAllRuntimes()
        {
            foreach (FileInfo leaf in TCLE.WorkingFolder.EnumerateFiles("*.leaf", SearchOption.AllDirectories)) {
                UtilMath.CalculateLeafRuntimeStartup(leaf);
            }
            foreach (FileInfo lvl in TCLE.WorkingFolder.EnumerateFiles("*.lvl", SearchOption.AllDirectories)) {
                UtilMath.CalculateLvlRuntimeStartup(lvl);
            }
            foreach (FileInfo gate in TCLE.WorkingFolder.EnumerateFiles("*.gate", SearchOption.AllDirectories)) {
                UtilMath.CalculateGateRuntimeStartup(gate);
            }
            foreach (FileInfo gate in TCLE.WorkingFolder.EnumerateFiles("*.master", SearchOption.AllDirectories)) {
                UtilMath.CalculateMasterRuntimeStartup(gate);
            }
        }

        public static void CalculateLeafRuntimeStartup(FileInfo leaf)
        {
            JObject _load = UtilFile.LoadFileLock(leaf);
            if (_load is null) {
                ProjectExplorer.Files[leaf.Name].Runtime = -1;
                return;
            }
            ProjectExplorer.Files[leaf.Name].Data = _load;
            ProjectExplorer.Files[leaf.Name].Runtime = (int)_load["beat_cnt"];
        }
        public static void CalculateLvlRuntimeStartup(FileInfo lvl)
        {
            //load the lvl and then loop through its leafs to get beat counts
            JObject _load = UtilFile.LoadFileLock(lvl);
            if (_load == null) {
                ProjectExplorer.Files[lvl.Name].Runtime = -1;
                return;
            }
            ProjectItem Item = ProjectExplorer.Files[lvl.Name];

            foreach (JObject leaf in _load["leaf_seq"]) {
                Item.AddChild((string)leaf["leaf_name"]);
                if (ProjectExplorer.TryGetFile((string)leaf["leaf_name"], out ProjectItem _run)) {
                    if (_run.Runtime != (int)leaf["beat_cnt"])
                        leaf["beat_cnt"] = _run.Runtime;
                }
            }
            Item.Data = _load;
            Item.GetRuntime();
        }
        public static void CalculateGateRuntimeStartup(FileInfo gate)
        {
            JObject _load = UtilFile.LoadFileLock(gate);
            List<int> bucketscounted = new();
            bool israndom;
            //if gate not found, _load is null. Return -1 to denote this
            if (_load == null) {
                ProjectExplorer.Files[gate.Name].Runtime = -1;
                return;
            }
            ProjectItem Item = ProjectExplorer.Files[gate.Name];
            //check if random is enabled on this gate
            israndom = (string)_load["random_type"] == "LEVEL_RANDOM_BUCKET";
            //loop through each lvl in gate
            foreach (JObject _lvl in _load["boss_patterns"]) {
                Item.AddChild((string)_lvl["lvl_name"]);
                //attempt to load lvl
                if (ProjectExplorer.TryGetFile((string)_lvl["lvl_name"], out ProjectItem _run)) {
                    //if random is enabled, count only the first entry in each bucket
                    /*if (israndom) {
                        if (!bucketscounted.Contains((int)_lvl["bucket_num"])) {
                            bucketscounted.Add((int)_lvl["bucket_num"]);
                            _beatcount += _run.Runtime;
                        }
                    }
                    //otherwise count each lvl
                    else
                        _beatcount += _run.Runtime;*/
                }
            }
            //need to also count pre and post lvl
            Item.AddChild((string)_load["pre_lvl_name"]);
            if (ProjectExplorer.TryGetFile((string)_load["pre_lvl_name"], out ProjectItem _pre)) {
                //_beatcount += _pre.Runtime;
            }
            Item.AddChild((string)_load["post_lvl_name"]);
            if (ProjectExplorer.TryGetFile((string)_load["post_lvl_name"], out ProjectItem _post)) {
                //_beatcount += _post.Runtime;
            }

            Item.Data = _load;
            Item.GetRuntime();
        }
        public static void CalculateMasterRuntimeStartup(FileInfo master)
        {
            JObject _load = UtilFile.LoadFileLock(master);
            if (_load == null) {
                ProjectExplorer.Files[master.Name].Runtime = -1;
                return;
            }
            ProjectItem Item = ProjectExplorer.Files[master.Name];
            //int _beatcount = 0;
            foreach (JObject _sublevel in _load["groupings"]) {
                Item.AddChild((string)_sublevel["lvl_name"]);
                if (ProjectExplorer.TryGetFile((string)_sublevel["lvl_name"], out ProjectItem _run)) {
                    //_beatcount += _run.Runtime;
                }
                Item.AddChild((string)_sublevel["gate_name"]);
                if (ProjectExplorer.TryGetFile((string)_sublevel["gate_name"], out _run)) {
                    //_beatcount += _run.Runtime;
                }
                Item.AddChild((string)_sublevel["rest_lvl_name"]);
                if (ProjectExplorer.TryGetFile((string)_sublevel["rest_lvl_name"], out _run)) {
                    //_beatcount += _run.Runtime;
                }
            }
            Item.AddChild((string)_load["intro_lvl_name"]);
            if (ProjectExplorer.TryGetFile((string)_load["intro_lvl_name"], out ProjectItem _run2)) {
                //_beatcount += _run2.Runtime;
            }

            Item.Data = _load;
            Item.GetRuntime();
        }

        public static int GetTrackOffset(DataGridView trackEditor)
        {
            return (trackEditor.Columns[3].Width - trackEditor.FirstDisplayedScrollingColumnHiddenWidth) + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3) + 4;
        }

        public static void CalculateTuning(double[] interp, string Type)
        {
            switch (Type) {
                case "Linear Ease In":
                case "Linear Ease Out":
                case "Linear Ease In Out":
                    break;
                case "Step Ease In":
                case "Step Ease Out":
                case "Step Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 0;
                    }
                    interp[^1] = 1;
                    break;
                case "Quadratic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x];
                    }
                    break;
                case "Quadratic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - (1 - interp[x]) * (1 - interp[x]);
                    }
                    break;
                case "Quadratic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (2 * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 2) / 2));
                    }
                    break;
                case "Cubic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x] * interp[x];
                    }
                    break;
                case "Cubic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Pow(1 - interp[x], 3);
                    }
                    break;
                case "Cubic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (4 * interp[x] * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 3) / 2));
                    }
                    break;
                case "Quartic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x] * interp[x] * interp[x];
                    }
                    break;
                case "Quartic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Pow(1 - interp[x], 4);
                    }
                    break;
                case "Quartic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (8 * interp[x] * interp[x] * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 4) / 2));
                    }
                    break;
                case "Quintic Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] * interp[x] * interp[x] * interp[x] * interp[x];
                    }
                    break;
                case "Quintic Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Pow(1 - interp[x], 5);
                    }
                    break;
                case "Quintic Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = interp[x] < 0.5 ? (16 * interp[x] * interp[x] * interp[x] * interp[x]) : (1 - (Math.Pow(-2 * interp[x] + 2, 5) / 2));
                    }
                    break;
                case "Sine Ease In":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = 1 - Math.Cos((interp[x] * Math.PI) / 2);
                    }
                    break;
                case "Sine Ease Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = Math.Sin((interp[x] * Math.PI) / 2);
                    }
                    break;
                case "Sine Ease In Out":
                    for (int x = 0; x < interp.Length; x++) {
                        interp[x] = -(Math.Cos(Math.PI * interp[x]) - 1) / 2;
                    }
                    break;
            }
        }
    }
}
