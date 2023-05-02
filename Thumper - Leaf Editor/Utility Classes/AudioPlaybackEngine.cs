using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Thumper_Custom_Level_Editor
{
    /// 
    /// http://mark-dot-net.blogspot.com/2014/02/fire-and-forget-audio-playback-with.html
    ///
    class AudioPlaybackEngine : IDisposable
    {
        private readonly IWavePlayer outputDevice;
        private readonly MixingSampleProvider mixer;

        public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
        {
            outputDevice = new WaveOutEvent();
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            mixer.ReadFully = true;
            outputDevice.Volume = 0.5f;
            outputDevice.Init(mixer);
            outputDevice.Play();
        }

        public void PlaySound(string fileName)
        {
            var input = new VorbisWaveReader(fileName);
            AddMixerInput(input);
        }

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == mixer.WaveFormat.Channels) {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2) {
                return new MonoToStereoSampleProvider(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        public void PlaySound(CachedSound sound)
        {
            AddMixerInput(new CachedSoundSampleProvider(sound));
        }

        private void AddMixerInput(ISampleProvider input)
        {
            mixer.AddMixerInput(/*ConvertToRightChannelCount(input)*/input);
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }

        public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);
    }

    public class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string audioFileName)
        {
            if (audioFileName.Contains(".ogg")) {
                using (var vorbisWaveReader = new VorbisWaveReader(audioFileName)) {
                    // TODO: could add resampling in here if required
                    WaveFormat = vorbisWaveReader.WaveFormat;
                    var wholeFile = new List<float>((int)(vorbisWaveReader.Length / 4));
                    var readBuffer = new float[vorbisWaveReader.WaveFormat.SampleRate * vorbisWaveReader.WaveFormat.Channels];
                    int samplesRead;
                    while ((samplesRead = vorbisWaveReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
                        wholeFile.AddRange(readBuffer.Take(samplesRead));
                    }
                    AudioData = wholeFile.ToArray();
                }
            }
            
            else if (audioFileName.Contains(".wav")) {
                using (var wavWaveReader = new AudioFileReader(audioFileName)) {
                    //need to resample wav to 44100 sample rate
                    ///https://markheath.net/post/how-to-resample-audio-with-naudio
                    ///https://markheath.net/post/convert-16-bit-pcm-to-ieee-float
                    var outFormat = new WaveFormat(44100, wavWaveReader.WaveFormat.Channels);
                    var resampler = new WdlResamplingSampleProvider(wavWaveReader, 44100);
                    WaveFormat = resampler.WaveFormat;
                    var wholeFile = new List<float>((int)(wavWaveReader.Length / 4));
                    var readBuffer = new float[resampler.WaveFormat.SampleRate * resampler.WaveFormat.Channels];
                    int samplesRead;
                    while ((samplesRead = resampler.Read(readBuffer, 0, readBuffer.Length)) > 0) {
                        wholeFile.AddRange(readBuffer.Take(samplesRead));
                    }
                    AudioData = wholeFile.ToArray();
                }
            }

        }
    }

    class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }
    }

    class AutoDisposeFileReader : ISampleProvider
    {
        private readonly AudioFileReader reader;
        private bool isDisposed;
        public AutoDisposeFileReader(AudioFileReader reader)
        {
            this.reader = reader;
            this.WaveFormat = reader.WaveFormat;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (isDisposed)
                return 0;
            int read = reader.Read(buffer, offset, count);
            if (read == 0) {
                reader.Dispose();
                isDisposed = true;
            }
            return read;
        }

        public WaveFormat WaveFormat { get; private set; }
    }
    /*
    /// <summary>
    /// Converts 16 bit PCM to IEEE float, optionally adjusting volume along the way
    /// </summary>
    public class Wave16toIeeeProvider : IWaveProvider
    {
        private IWaveProvider sourceProvider;
        private readonly WaveFormat waveFormat;
        private volatile float volume;
        private byte[] sourceBuffer;

        /// <summary>
        /// Creates a new Wave16toIeeeProvider
        /// </summary>
        /// <param name="sourceStream">the source stream</param>
        /// <param name="volume">stream volume (1 is 0dB)</param>
        /// <param name="pan">pan control (-1 to 1)</param>
        public Wave16toIeeeProvider(IWaveProvider sourceProvider)
        {
            if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
                throw new ApplicationException("Only PCM supported");
            if (sourceProvider.WaveFormat.BitsPerSample != 16)
                throw new ApplicationException("Only 16 bit audio supported");

            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sourceProvider.WaveFormat.SampleRate, sourceProvider.WaveFormat.Channels);

            this.sourceProvider = sourceProvider;
            this.volume = 1.0f;
        }

        /// <summary>
        /// Helper function to avoid creating a new buffer every read
        /// </summary>
        byte[] GetSourceBuffer(int bytesRequired)
        {
            if (sourceBuffer == null || sourceBuffer.Length < bytesRequired) {
                sourceBuffer = new byte[bytesRequired];
            }
            return sourceBuffer;
        }

        /// <summary>
        /// Reads bytes from this wave stream
        /// </summary>
        /// <param name="destBuffer">The destination buffer</param>
        /// <param name="offset">Offset into the destination buffer</param>
        /// <param name="numBytes">Number of bytes read</param>
        /// <returns>Number of bytes read.</returns>
        public int Read(byte[] destBuffer, int offset, int numBytes)
        {
            int sourceBytesRequired = numBytes / 2;
            byte[] sourceBuffer = GetSourceBuffer(sourceBytesRequired);
            int sourceBytesRead = sourceProvider.Read(sourceBuffer, offset, sourceBytesRequired);
            WaveBuffer sourceWaveBuffer = new WaveBuffer(sourceBuffer);
            WaveBuffer destWaveBuffer = new WaveBuffer(destBuffer);

            int sourceSamples = sourceBytesRead / 2;
            int destOffset = offset / 4;
            for (int sample = 0; sample < sourceSamples; sample++) {
                destWaveBuffer.FloatBuffer[destOffset++] = (sourceWaveBuffer.ShortBuffer[sample] / 32768f) * volume;
            }

            return sourceSamples * 4;
        }

        /// <summary>
        /// <see cref="IWaveProvider.WaveFormat"/>
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        /// <summary>
        /// Volume of this channel. 1.0 = full scale
        /// </summary>
        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }*/
}
