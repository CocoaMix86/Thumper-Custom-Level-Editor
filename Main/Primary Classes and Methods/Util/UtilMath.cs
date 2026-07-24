namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilMath
    {
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

        public static int CalculateSublevelRuntime(MasterLvlData _masterlvl)
        {
            int _beatcount = 0;
            if (_masterlvl.Type == "lvl") {
                if (!ProjectExplorer.TryGetFile(_masterlvl.name, out FileInfo lvl))
                    return -1;
                _beatcount += CalculateLvlRuntime(lvl);
            }
            //this section handles gate
            else {
                int gatebeats = CalculateGateRuntimeFromFile(_masterlvl.name);
                if (gatebeats == -1)
                    return -1;
                else
                    _beatcount += gatebeats;
            }
            if (ProjectExplorer.TryGetFile(_masterlvl.rest, out FileInfo lvlrest))
                _beatcount += CalculateLvlRuntime(lvlrest);

            return _beatcount;
        }

        public static int CalculateGateRuntimeFromFile(string gatename)
        {
            dynamic _load;
            int _beatcount = 0;
            List<int> bucketscounted = new();
            bool israndom;
            //load the gate to then loop through all lvls in it
            if (!ProjectExplorer.TryGetFile(gatename, out FileInfo gate))
                return -1;

            _load = UtilFile.LoadFileLock(gate);
            //if gate not found, _load is null. Return -1 to denote this
            if (_load == null)
                return -1;
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

            return _beatcount;
        }

        public static int CalculateLvlRuntime(FileInfo lvl)
        {
            int _beatcount = 0;

            //load the lvl and then loop through its leafs to get beat counts
            dynamic _load = UtilFile.LoadFileLock(lvl);
            if (_load == null)
                return 0;
            foreach (dynamic leaf in _load["leaf_seq"]) {
                if (ProjectExplorer.TryGetFile((string)leaf["leaf_name"], out FileInfo _leaf) && _leaf.Exists) {
                    dynamic _loeadleaf = UtilFile.LoadFileLock(_leaf);
                    if (_loeadleaf != null)
                        _beatcount += (int)_loeadleaf["beat_cnt"];
                }
            }
            //every lvl has an approach beats to consider too
            //_beatcount += (int)_load["approach_beats"];

            return _beatcount;
        }

        public static int GetTrackOffset(DataGridView trackEditor)
        {
            return (trackEditor.Columns[3].Width - trackEditor.FirstDisplayedScrollingColumnHiddenWidth) + trackEditor.RowHeadersWidth + (trackEditor.Columns[0].Width * 3) + 4;
        }
    }
}
