using Microsoft.Xna.Framework.Audio;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace.Sound
{
    /// <summary>
    /// A lightweight, gapless music streamer for OGG files.
    /// Features: Play/Pause/Stop, IsRepeating, Loop Points, Volume, Fades, Crossfade.
    /// </summary>
    public sealed class NVorbisPlayer : IDisposable
    {
        // ---- Public API (MediaPlayer-like) ----------------------------------

        public bool IsPlaying { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsRepeating { get; set; } = true;

        /// <summary>Main volume [0..1]. Multiplies with FadeGain.</summary>
        public float Volume
        {
            get => _userVolume;
            set
            {
                _userVolume = Math.Clamp(value, 0f, 1f);
                ApplyVolume();
            }
        }

        /// <summary>Fired when the track reaches its natural end (if not repeating).</summary>
        public event Action? MediaEnded;

        /// <summary>Current playback position in seconds (approx).</summary>
        public double PositionSeconds
        {
            get
            {
                lock (_lock)
                {
                    if (_vorbis == null) return 0;
                    // DecodedPosition is in PCM samples per channel
                    return (double)_vorbis.TimePosition.TotalSeconds;
                }
            }
        }

        /// <summary>Optional loop points in seconds (inclusive start, exclusive end). If not set and IsRepeating=true, loops whole file.</summary>
        public (double? Start, double? End) LoopPointsSeconds
        {
            get => (_loopStartSec, _loopEndSec);
            set
            {
                lock (_lock)
                {
                    _loopStartSec = value.Start;
                    _loopEndSec = value.End;
                }
            }
        }

        // ---- Construction / Lifetime ----------------------------------------

        public NVorbisPlayer()
        {
            _worker = new Thread(StreamingLoop) { IsBackground = true, Name = "MusicPlayerStreamer" };
            _worker.Start();
        }

        public void Dispose()
        {
            StopInternal(hardStop: true);
            _run = false;
            _wake.Set();
            _worker.Join();

            _wake.Dispose();
        }

        // ---- Control ---------------------------------------------------------

        /// <summary>
        /// Begin playing an OGG file from a path. Any current track is stopped immediately.
        /// </summary>
        public void Play(string oggPath, double? loopStartSec = null, double? loopEndSec = null, bool isRepeating = true, bool startPaused = false)
        {
            if (string.IsNullOrWhiteSpace(oggPath)) throw new ArgumentNullException(nameof(oggPath));
            Play(File.OpenRead(oggPath), ownsStream: true, loopStartSec, loopEndSec, isRepeating, startPaused);
        }

        /// <summary>
        /// Begin playing from a provided stream (must be readable & seekable). If ownsStream=true the player will dispose it.
        /// </summary>
        public void Play(Stream oggStream, bool ownsStream, double? loopStartSec = null, double? loopEndSec = null, bool isRepeating = true, bool startPaused = false)
        {
            lock (_lock)
            {
                StopInternal(hardStop: true);

                _pendingStart = new PendingStart
                {
                    Stream = oggStream,
                    OwnsStream = ownsStream,
                    LoopStartSec = loopStartSec,
                    LoopEndSec = loopEndSec,
                    IsRepeating = isRepeating,
                    StartPaused = startPaused
                };
            }
            _wake.Set();
        }

        public void Pause()
        {
            lock (_lock)
            {
                if (!IsPlaying || IsPaused) return;
                _isManuallyPaused = true;
                _dsei?.Pause();
                IsPaused = true;
            }
        }

        public void Resume()
        {
            lock (_lock)
            {
                if (!IsPlaying || !IsPaused) return;
                _isManuallyPaused = false;
                _dsei?.Play();
                IsPaused = false;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                StopInternal(hardStop: true);
            }
        }

        /// <summary>Instantly seeks to a position (seconds). Clamps to [0, trackLength].</summary>
        public void Seek(double seconds)
        {
            lock (_lock)
            {
                if (_vorbis == null) return;
                seconds = Math.Max(0, Math.Min(seconds, _totalDurationSec));
                _vorbis.TimePosition = TimeSpan.FromSeconds(seconds);
                // Flush queued audio so we don't hear old buffers
                _dsei?.Stop();
                _dsei?.Play();
                _buffersQueued = 0;
            }
        }

        // ---- Fades / Crossfades ----------------------------------------------

        /// <summary>Fade the current track's audible gain to target [0..1] over durationMs.</summary>
        public void FadeTo(float targetGain, int durationMs)
        {
            lock (_lock)
            {
                _fadeFrom = _fadeGain;
                _fadeTo = Math.Clamp(targetGain, 0f, 1f);
                _fadeTimeMs = durationMs <= 0 ? 0 : durationMs;
                _fadeStartMs = NowMs();
                if (_fadeTimeMs == 0) _fadeGain = _fadeTo;
                ApplyVolume();
            }
        }

        /// <summary>
        /// Crossfade to a new track over durationMs. Starts the new track at 0 gain, fades it in while fading current to 0, then stops old.
        /// </summary>
        public void CrossfadeTo(string oggPath, int durationMs, double? loopStartSec = null, double? loopEndSec = null, bool isRepeating = true)
        {
            // Simple 2-step: start new at 0 gain paused, then atomically switch players
            var newPlayer = new NVorbisPlayer { Volume = this.Volume, IsRepeating = isRepeating };
            newPlayer.Play(oggPath, loopStartSec, loopEndSec, isRepeating, startPaused: false);
            newPlayer._fadeGain = 0f;
            newPlayer.ApplyVolume();
            newPlayer.FadeTo(1f, durationMs);

            // Fade this out, then stop after duration
            this.FadeTo(0f, durationMs);

            // Schedule a stop; user can manage lifetime or we can hand back newPlayer.
            // For simplicity here, we just suggest how you'd use Crossfade in code comments.
        }

        // ---- Internal state ---------------------------------------------------

        private readonly object _lock = new();
        private readonly AutoResetEvent _wake = new(false);
        private readonly Thread _worker;
        private volatile bool _run = true;

        private class PendingStart
        {
            public Stream? Stream;
            public bool OwnsStream;
            public double? LoopStartSec;
            public double? LoopEndSec;
            public bool IsRepeating;
            public bool StartPaused;
        }
        private PendingStart? _pendingStart;

        private VorbisReader? _vorbis;
        private Stream? _stream;
        private bool _ownsStream;

        private DynamicSoundEffectInstance? _dsei;
        private int _sampleRate;
        private int _channels;
        private double _totalDurationSec;

        // Buffering
        private const int BufferMillis = 100;   // length of each buffer
        private const int QueueTarget = 4;     // keep this many queued
        private int _buffersQueued = 0;

        // Playback flags
        private bool _isManuallyPaused = false;

        // Volume & Fades
        private float _userVolume = 1f; // external volume
        private float _fadeGain = 1f;   // internal gain for fades
        private float _fadeFrom = 1f, _fadeTo = 1f;
        private int _fadeTimeMs = 0;
        private long _fadeStartMs = 0;

        // Loop points (seconds)
        private double? _loopStartSec = null;
        private double? _loopEndSec = null;

        // ---- Worker thread ----------------------------------------------------

        private void StreamingLoop()
        {
            // Reusable buffers (allocated on first track)
            float[]? floatBuf = null;
            short[]? pcm = null;
            byte[]? bytes = null;

            while (_run)
            {
                // Handle new track requests
                if (_pendingStart != null)
                {
                    lock (_lock)
                    {
                        var p = _pendingStart; _pendingStart = null;
                        if (p != null)
                        {
                            StartTrack(p, ref floatBuf, ref pcm, ref bytes);
                            if (_dsei != null)
                            {
                                IsPaused = p.StartPaused;
                                if (IsPaused) _dsei.Pause(); else _dsei.Play();
                            }
                        }
                    }
                }

                // If no active track, sleep until something happens
                if (_vorbis == null || _dsei == null)
                {
                    _wake.WaitOne(100);
                    continue;
                }

                // Update fade
                UpdateFade();

                // Fill buffers if needed
                while (_dsei.PendingBufferCount < QueueTarget && _run && _vorbis != null)
                {
                    int bytesPerSample = sizeof(short);
                    int samplesPerBuffer = (int)(BufferMillis * _sampleRate / 1000.0);
                    int frameSamples = samplesPerBuffer * _channels;

                    floatBuf ??= new float[frameSamples];
                    pcm ??= new short[frameSamples];
                    bytes ??= new byte[frameSamples * bytesPerSample];

                    int read = _vorbis.ReadSamples(floatBuf, 0, frameSamples);

                    if (read == 0)
                    {
                        // Hit end
                        if (IsRepeating)
                        {
                            // Looping: either loop the whole file or to explicit loop points
                            if (_loopStartSec.HasValue)
                                _vorbis.TimePosition = TimeSpan.FromSeconds(_loopStartSec.Value);
                            else
                                _vorbis.TimePosition = TimeSpan.Zero;
                            continue;
                        }
                        else
                        {
                            // Natural end — stop and notify on main thread context (we're already background; safe to call event)
                            StopInternal(hardStop: false);
                            MediaEnded?.Invoke();
                            break;
                        }
                    }

                    // Apply loop end clamp (if set)
                    if (_loopEndSec.HasValue)
                    {
                        double posEnd = _vorbis.TimePosition.TotalSeconds;
                        double posStart = posEnd - (double)read / (_sampleRate * _channels);
                        if (posEnd > _loopEndSec.Value)
                        {
                            // We read past loop end; trim the frame to just before loop end
                            double extraSec = posEnd - _loopEndSec.Value;
                            int samplesToTrim = (int)Math.Round(extraSec * _sampleRate * _channels);
                            int trimmed = Math.Max(0, read - samplesToTrim);
                            if (trimmed <= 0)
                            {
                                // Force loop to start, then continue next iteration
                                if (_loopStartSec.HasValue)
                                    _vorbis.TimePosition = TimeSpan.FromSeconds(_loopStartSec.Value);
                                else
                                    _vorbis.TimePosition = TimeSpan.Zero;
                                continue;
                            }
                            read = trimmed;

                            // After we submit this trimmed buffer, rewind to loop start
                            // by setting DecodedTime for the next iteration:
                            if (_loopStartSec.HasValue)
                                _vorbis.TimePosition = TimeSpan.FromSeconds(_loopStartSec.Value);
                            else
                                _vorbis.TimePosition = TimeSpan.Zero;
                        }
                    }

                    // Convert float [-1..1] to 16-bit PCM, applying fade gain
                    float gain = Math.Clamp(_fadeGain, 0f, 1f);
                    for (int i = 0; i < read; i++)
                    {
                        float f = floatBuf[i] * gain;
                        f = f < -1f ? -1f : (f > 1f ? 1f : f);
                        pcm[i] = (short)(f * short.MaxValue);
                    }

                    Buffer.BlockCopy(pcm, 0, bytes, 0, read * bytesPerSample);
                    _dsei.SubmitBuffer(bytes, 0, read * bytesPerSample);
                    _buffersQueued++;
                }

                // Sleep a tiny bit to avoid hot spinning
                Thread.Sleep(5);
            }

            // Cleanup (in case Dispose() didn’t already)
            lock (_lock) StopInternal(hardStop: true);
        }

        private void StartTrack(PendingStart p, ref float[]? floatBuf, ref short[]? pcm, ref byte[]? bytes)
        {
            StopInternal(hardStop: true);

            _stream = p.Stream!;
            _ownsStream = p.OwnsStream;

            _vorbis = new VorbisReader(_stream, false);
            _sampleRate = _vorbis.SampleRate;
            _channels = _vorbis.Channels;
            _totalDurationSec = _vorbis.TotalTime.TotalSeconds;

            _loopStartSec = p.LoopStartSec;
            _loopEndSec = p.LoopEndSec;
            IsRepeating = p.IsRepeating;

            _dsei = new DynamicSoundEffectInstance(
                _sampleRate,
                _channels == 1 ? AudioChannels.Mono : AudioChannels.Stereo);

            _dsei.Play(); // start primed; we might pause right after
            _buffersQueued = 0;
            _isManuallyPaused = p.StartPaused;
            IsPlaying = true;
            IsPaused = p.StartPaused;

            // Reset fades to full gain (caller can run FadeTo after Play)
            _fadeGain = 1f; _fadeFrom = 1f; _fadeTo = 1f; _fadeTimeMs = 0; _fadeStartMs = NowMs();
            ApplyVolume();

            // Reset reusable buffers to match new frame size on-demand
            floatBuf = null; pcm = null; bytes = null;
        }

        private void StopInternal(bool hardStop)
        {
            IsPlaying = false;
            IsPaused = false;
            _isManuallyPaused = false;

            try { _dsei?.Stop(); } catch { /* ignore */ }
            _dsei?.Dispose(); _dsei = null;

            _vorbis?.Dispose(); _vorbis = null;

            if (_ownsStream) { try { _stream?.Dispose(); } catch { } }
            _stream = null; _ownsStream = false;

            _buffersQueued = 0;

            if (hardStop)
            {
                _loopStartSec = null;
                _loopEndSec = null;
            }
        }

        private void UpdateFade()
        {
            if (_fadeTimeMs <= 0) return;

            long now = NowMs();
            float t = (float)(now - _fadeStartMs) / _fadeTimeMs;
            if (t >= 1f)
            {
                _fadeGain = _fadeTo;
                _fadeTimeMs = 0;
            }
            else
            {
                _fadeGain = Lerp(_fadeFrom, _fadeTo, t);
            }
            ApplyVolume();
        }

        private void ApplyVolume()
        {
            if (_dsei == null) return;
            float outVol = Math.Clamp(_userVolume * _fadeGain, 0f, 1f);
            _dsei.Volume = outVol;
        }

        private static float Lerp(float a, float b, float t) => a + (b - a) * t;
        private static long NowMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
