using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            VorbisWaveReader input = new VorbisWaveReader(fileName);
            AddMixerInput(input);
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

        public static readonly AudioPlaybackEngine Instance = new(44100, 2);
    }

    public class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string audioFileName)
        {
            if (audioFileName.Contains(".ogg")) {
                using (VorbisWaveReader vorbisWaveReader = new VorbisWaveReader(audioFileName)) {
                    // TODO: could add resampling in here if required
                    WaveFormat = vorbisWaveReader.WaveFormat;
                    List<float> wholeFile = new List<float>((int)(vorbisWaveReader.Length / 4));
                    float[] readBuffer = new float[vorbisWaveReader.WaveFormat.SampleRate * vorbisWaveReader.WaveFormat.Channels];
                    int samplesRead;
                    while ((samplesRead = vorbisWaveReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
                        wholeFile.AddRange(readBuffer.Take(samplesRead));
                    }
                    AudioData = wholeFile.ToArray();
                }
            }

            else if (audioFileName.Contains(".wav")) {
                using (AudioFileReader wavWaveReader = new AudioFileReader(audioFileName)) {
                    //need to resample wav to 44100 sample rate
                    ///https://markheath.net/post/how-to-resample-audio-with-naudio
                    ///https://markheath.net/post/convert-16-bit-pcm-to-ieee-float
                    WaveFormat outFormat = new WaveFormat(44100, wavWaveReader.WaveFormat.Channels);
                    WdlResamplingSampleProvider resampler = new WdlResamplingSampleProvider(wavWaveReader, 44100);
                    WaveFormat = resampler.WaveFormat;
                    List<float> wholeFile = new List<float>((int)(wavWaveReader.Length / 4));
                    float[] readBuffer = new float[resampler.WaveFormat.SampleRate * resampler.WaveFormat.Channels];
                    int samplesRead;
                    while ((samplesRead = resampler.Read(readBuffer, 0, readBuffer.Length)) > 0) {
                        wholeFile.AddRange(readBuffer.Take(samplesRead));
                    }
                    AudioData = wholeFile.ToArray();
                }
            }

        }
        public CachedSound(Stream audioFileName)
        {
            using (VorbisWaveReader vorbisWaveReader = new VorbisWaveReader(audioFileName)) {
                // TODO: could add resampling in here if required
                WaveFormat = vorbisWaveReader.WaveFormat;
                List<float> wholeFile = new List<float>((int)(vorbisWaveReader.Length / 4));
                float[] readBuffer = new float[vorbisWaveReader.WaveFormat.SampleRate * vorbisWaveReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = vorbisWaveReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
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
            long availableSamples = cachedSound.AudioData.Length - position;
            long samplesToCopy = Math.Min(availableSamples, count);
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
}
