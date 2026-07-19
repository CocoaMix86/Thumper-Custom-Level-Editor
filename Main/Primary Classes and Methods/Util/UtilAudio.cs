using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass.Misc;
using Un4seen.Bass;
using Fmod5Sharp.FmodTypes;
using Fmod5Sharp;
using NAudio.Wave;
using Thumper_Custom_Level_Editor.Editor_Panels;

namespace Thumper_Custom_Level_Editor.Primary_Classes_and_Methods.Util
{
    public static class UtilAudio
    {
        public static Dictionary<int, int> Frequencies = new() {
            { 1, 8000 },
            { 2, 11_000 },
            { 3, 11_025 },
            { 4, 16_000 },
            { 5, 22_050 },
            { 6, 24_000 },
            { 7, 32_000 },
            { 8, 44_100 },
            { 9, 48_000 },
            { 10,96_000 }
        };

        public static Dictionary<string, byte[]> SoundEffects = new() {
            ["UIaddrandom"] = Properties.Resources.UIaddrandom,
            ["UIbeetleclick1"] = Properties.Resources.UIbeetleclick1,
            ["UIbeetleclick2"] = Properties.Resources.UIbeetleclick2,
            ["UIbeetleclick3"] = Properties.Resources.UIbeetleclick3,
            ["UIbeetleclick4"] = Properties.Resources.UIbeetleclick4,
            ["UIbeetleclick5"] = Properties.Resources.UIbeetleclick5,
            ["UIbeetleclick6"] = Properties.Resources.UIbeetleclick6,
            ["UIbeetleclick7"] = Properties.Resources.UIbeetleclick7,
            ["UIbeetleclick8"] = Properties.Resources.UIbeetleclick8,
            ["UIbeetleclickGOLD"] = Properties.Resources.UIbeetleclickGOLD,
            ["UIboot"] = Properties.Resources.UIboot,
            ["UIcolorapply"] = Properties.Resources.UIcolorapply,
            ["UIcoloropen"] = Properties.Resources.UIcoloropen,
            ["UIdataerase"] = Properties.Resources.UIdataerase,
            ["UIdelete"] = Properties.Resources.UIdelete,
            ["UIdock"] = Properties.Resources.UIdock,
            ["UIdockun"] = Properties.Resources.UIdockun,
            ["UIfolderclose"] = Properties.Resources.UIfolderclose,
            ["UIfolderopen"] = Properties.Resources.UIfolderopen,
            ["UIinterpolate"] = Properties.Resources.UIinterpolate,
            ["UIinterpolatewindow"] = Properties.Resources.UIinterpolatewindow,
            ["UIkcopy"] = Properties.Resources.UIkcopy,
            ["UIkpaste"] = Properties.Resources.UIkpaste,
            ["UIleafsplit"] = Properties.Resources.UIleafsplit,
            ["UIobjectadd"] = Properties.Resources.UIobjectadd,
            ["UIobjectremove"] = Properties.Resources.UIobjectremove,
            ["UIrefresh"] = Properties.Resources.UIrefresh,
            ["UIrevertchanges"] = Properties.Resources.UIrevertchanges,
            ["UIrevertnew"] = Properties.Resources.UIrevertnew,
            ["UIsave"] = Properties.Resources.UIsave,
            ["UIselect"] = Properties.Resources.UIselect,
            ["UItunneladd"] = Properties.Resources.UItunneladd,
            ["UItunnelremove"] = Properties.Resources.UItunnelremove,
            ["UIwindowclose"] = Properties.Resources.UIwindowclose,
            ["UIwindowopen"] = Properties.Resources.UIwindowopen,
            ["duck"] = Properties.Resources.duck
        };

        public static void CalculateSampleRuntimes()
        {
            foreach (SampleData samp in TCLE.ProjectSamples.Where(x => x.time == 0)) {
                byte[] _bytes;
                //get the hash of this filename. This will be used to locate the sample's .PC file
                string _hashedname = UtilMath.HashPCName($"A{samp.path}");
                //check if sample is custom or not. This changes where we load audio from
                string filetoread;
                try {
                    if (samp.path.Contains("custom"))
                        filetoread = $@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc";
                    else
                        filetoread = $@"{Properties.Settings.Default.game_dir}\cache\{_hashedname}.pc";

                    using (BinaryReader reader = new(new FileStream(filetoread, FileMode.Open, FileAccess.Read, FileShare.Read))) {
                        reader.ReadUInt32(); //pc header
                        reader.ReadUInt32(); //fsb5 header
                        reader.ReadUInt32(); //version
                        reader.ReadUInt32(); //# of tracks
                        reader.ReadUInt32(); //size of sample header
                        reader.ReadUInt32(); //size of header table
                        reader.ReadUInt32(); //sample bytes
                        reader.ReadUInt32(); //audio type
                        reader.ReadUInt32(); //unknown
                        reader.ReadUInt32(); //flags
                        reader.ReadUInt64(); //hash1
                        reader.ReadUInt64(); //hash2
                        reader.ReadUInt64(); //hash3
                        UInt64 metadata = reader.ReadUInt64(); //metadata

                        UInt64 freqid = (metadata & 0b11110) >> 1;
                        UInt64 samples = metadata >> 34;
                        int freq = Frequencies[(int)freqid];
                        samp.time = (double)(samples) / (double)freq;
                    }
                } catch (Exception ex) {
                    samp.time = -1;
                }
            }
        }

        public static void StopAudio()
        {
            Playback.StopPlayback();
            Bass.BASS_Free();
            TCLE.alzheimer();
            PlayingChannels.Clear();
            foreach (EditorSample samp in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorSample))) {
                samp.sampleList.Refresh();
            }
            foreach (EditorLvl lvl in TCLE.Documents.Values.Where(x => x.GetType() == typeof(EditorLvl))) {
                lvl.lvlLoopTracks.Refresh();
            }
            // Initialize Sound library
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_LATENCY, TCLE.Instance.Handle);
        }

        public static string PCtoAudioFile(SampleData _samp)
        {
            if (_samp == null || _samp.obj_name == ".samp")
                return null;
            //check if the gamedir has been set so the method can find the .pc files
            if (Properties.Settings.Default.game_dir == "none") {
                UtilImport.GetThumperCacheFolder();
            }

            byte[] _bytes;
            //get the hash of this filename. This will be used to locate the sample's .PC file
            string _hashedname = UtilMath.HashPCName($"A{_samp.path}");

            //check if sample is custom or not. This changes where we load audio from
            if (_samp.path.Contains("custom")) {
                //attempt to locate file. But error and return safely if nothing found
                try {
                    _bytes = File.ReadAllBytes($@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc");
                    _bytes = _bytes.Skip(4).ToArray();
                } catch {
                    _samp.message = $@"Unable to locate file {TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc for sample {_samp.obj_name}. Is the file in the project's ""extras"" folder? You may need to re-import the file.";
                    return null;
                }
            }
            else {
                try {
                    _bytes = File.ReadAllBytes($@"{Properties.Settings.Default.game_dir}\cache\{_hashedname}.pc");
                    _bytes = _bytes.Skip(4).ToArray();
                } catch {
                    _samp.message = $@"Unable to locate file {Properties.Settings.Default.game_dir}\cache\{_hashedname}.pc for sample {_samp.obj_name}. This is a non-custom sample supplied by the game. If you need to change your Game Directory, go to the the Help menu. Otherwise you may need to repair your Thumper installation.";
                    return null;
                }
            }
            if (_bytes.Length == 0) {
                _samp.message = $@"Unable to properly parse {TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc to play sample {_samp.obj_name}. You may need to re-import the file.";
                return null;
            }
            //check if file has been converted already. Ready the path if true
            string existingFile = Directory.GetFiles($@"temp\", $"{_samp.obj_name}.*", SearchOption.AllDirectories).FirstOrDefault();
            if (existingFile != null) {
                _samp.TempFile = existingFile;
                return _samp.TempFile;
            }
            ///
            // credit to https://github.com/SamboyCoding/Fmod5Sharp
            FmodSoundBank bank = FsbLoader.LoadFsbFromByteArray(_bytes);
            List<FmodSample> samples = bank.Samples;
            /*byte 24 of FSB files contains the data type of the audio
            PCM8 = 1,
            PCM16 = 2,
            PCM24 = 3,
            PCM32 = 4,
            PCMFLOAT = 5,
            GCADPCM = 6,
            IMAADPCM = 7,
            VAG = 8,
            HEVAG = 9,
            XMA = 10,
            MPEG = 11,
            CELT = 12,
            AT9 = 13,
            XWMA = 14,
            VORBIS = 15,*/
            int type = _bytes[24];
            byte[] dataBytes = null;
            string fileExtension = "";
            //PCM types
            if (type is 1 or 2 or 3 or 4) {
                try {
                    //My reimplementation of the RebuildAsStandardFileFormat() function, to support PCM24
                    dataBytes = RebuildWav(samples[0], bank.Header.AudioType);
                    fileExtension = "wav";
                } catch (Exception) {
                    _samp.message = $@"Unable to properly parse {TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc to play sample. You may need to re-import the file.";
                    return null;
                }
            }
            //Vorbis (ogg)
            else if (type is 15) {
                samples[0].RebuildAsStandardFileFormat(out dataBytes, out fileExtension);
            }
            else {
                _samp.message = $@"{TCLE.WorkingFolder.FullName}\extras\{_hashedname}.pc for {_samp.obj_name} is an unsupported audio type. Not PCM or Vorbis.";
                return null;
            }

            string finalfilename = $@"temp\{_samp.obj_name}.{fileExtension}";
            using (var stream = File.Open(finalfilename, FileMode.Create)) {
                using (BinaryWriter bw = new(stream)) {
                    bw.Write(dataBytes);
                }
            }
            //File.WriteAllBytes(finalfilename, dataBytes);
            _samp.TempFile = finalfilename;
            return _samp.TempFile;
        }

        public static byte[] RebuildWav(FmodSample sample, FmodAudioType type)
        {
            int width = type switch {
                FmodAudioType.PCM8 => 1,
                FmodAudioType.PCM16 => 2,
                FmodAudioType.PCM24 => 3,
                FmodAudioType.PCM32 => 4,
                _ => 0
                //_ => throw new($"FmodPcmRebuilder does not support encoding of type {type}"),
            };

            int numChannels = sample.Metadata.IsStereo ? 2 : 1;
            WaveFormat format = WaveFormat.CreateCustomFormat(
                WaveFormatEncoding.Pcm,
                sample.Metadata.Frequency,
                numChannels,
                sample.Metadata.Frequency * numChannels * width,
                numChannels * width,
                width * 8
            );
            using MemoryStream stream = new();
            using WaveFileWriter writer = new(stream, format);

            writer.Write(sample.SampleBytes, 0, sample.SampleBytes.Length);

            return stream.GetBuffer();
        }
        public static void PlaySound(string audiofile)
        {
            if (Properties.Settings.Default.muteapplication)
                return;
            if (!Properties.Settings.Default.muteduck && TCLE.rng.Next(1000) == 0) 
                PlaySampleOneOff("duck", SoundEffects["duck"], out _);            
            else
                PlaySampleOneOff(audiofile, SoundEffects[audiofile], out _);
            TCLE.alzheimer();
        }

        public static List<Tuple<DataGridView, string, int>> PlayingChannels = new();
        public static int LastChannel;
        public static SYNCPROC EndingProc = new(OnEnding);
        public static bool PlaySampleOneOff(DataGridViewCell cell, SampleData _samp, out int SampChannel)
        {
            string _sampleName = cell.DataGridView[1, cell.RowIndex].Value.ToString();
            if (Bass.BASS_ChannelIsActive(PlayingChannels.FirstOrDefault(x => x.Item1 == cell.DataGridView && x.Item2 == _sampleName)?.Item3 ?? 0) == BASSActive.BASS_ACTIVE_STOPPED) {
                string SampleToPlay = PCtoAudioFile(_samp);
                if (String.IsNullOrEmpty(SampleToPlay)) {
                    SampChannel = 0;
                    return false;
                }

                float initialfreq = 0;
                //initialize the player and load the sample
                SampChannel = Bass.BASS_StreamCreateFile($@"{SampleToPlay}", 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                _ = Bass.BASS_ChannelSetSync(SampChannel, BASSSync.BASS_SYNC_END, 0, EndingProc, 0);
                //pitch shift and pan
                Bass.BASS_ChannelGetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_FREQ, ref initialfreq);
                Bass.BASS_ChannelSetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_FREQ, initialfreq * (float)_samp.pitch);
                Bass.BASS_ChannelSetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_PAN, (float)_samp.pan);
                Bass.BASS_ChannelSetAttribute(SampChannel, BASSAttribute.BASS_ATTRIB_VOL, (float)Properties.Settings.Default.VolKey99 / 100f);
                Bass.BASS_ChannelSetPosition(SampChannel, (double)_samp.offset / 1000d);
                if (_samp.wave == null) {
                    _samp.CalculateRuntime(SampChannel, false);
                    _samp.UpdateRuntime();
                }
                //play the sample
                if (SampChannel != 0 && Bass.BASS_ChannelPlay(SampChannel, false)) {
                    PlayingChannels.Add(new Tuple<DataGridView, string, int>(cell.DataGridView, _sampleName, SampChannel));
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                Tuple<DataGridView, string, int> ItemToRemove = PlayingChannels.First(x => x.Item1 == cell.DataGridView && x.Item2 == _sampleName);
                SampChannel = ItemToRemove.Item3;
                Bass.BASS_ChannelStop(ItemToRemove.Item3);
                Bass.BASS_ChannelFree(ItemToRemove.Item3);
                PlayingChannels.Remove(ItemToRemove);
                return false;
            }
        }
        public static int PlaySampleOneOff(string samplename, byte[] stream, out int SampChannel)
        {
            //initialize the player and load the sample
            SampChannel = Bass.BASS_SampleLoad(stream, 0, stream.Length, 10, BASSFlag.BASS_SAMPLE_FLOAT);
            SampChannel = Bass.BASS_SampleGetChannel(SampChannel, BASSFlag.BASS_SAMPLE_FLOAT);
            _ = Bass.BASS_ChannelSetSync(SampChannel, BASSSync.BASS_SYNC_END, 0, EndingProc, IntPtr.Zero);
            //play the sample
            if (SampChannel != 0 && Bass.BASS_ChannelPlay(SampChannel, false)) {
                return SampChannel;
            }
            else {
                return SampChannel = 0;
            }
        }

        private static void OnEnding(int handle, int channel, int data, IntPtr user)
        {
            bool free1 = Bass.BASS_ChannelStop(channel);
            bool free2 = Bass.BASS_ChannelFree(channel);
            Tuple<DataGridView, string, int>? ItemToRemove = PlayingChannels.FirstOrDefault(x => x.Item3 == channel);
            if (ItemToRemove != null) {
                ItemToRemove.Item1.InvalidateColumn(0);
                PlayingChannels.Remove(ItemToRemove);
                if (PlayingChannels.Count > 0)
                    LastChannel = PlayingChannels.Last().Item3;
            }
            TCLE.alzheimer();
        }

        public static void GenerateSampWave(SampleData samp, int channel)
        {
            WaveForm wave = new(samp.TempFile) {
                DrawWaveForm = WaveForm.WAVEFORMDRAWTYPE.DualMono
            };
            //math to figure out how long the sample is, in seconds and dimensions
            long len = Bass.BASS_ChannelGetLength(channel, BASSMode.BASS_POS_BYTE);
            samp.time = Bass.BASS_ChannelBytes2Seconds(channel, len);/* - ((double)samp.offset / 1000d)) / (double)samp.pitch;*/
            //render wave
            wave.RenderStart(false, BASSFlag.BASS_SAMPLE_FLOAT);
            samp.wave = wave;
        }
    }
}
