using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;

namespace VikingEngine.SteamWrapping
{
    partial class SteamManager
    {
        DynamicSoundEffectInstance _audioPlayback;

        // Buffers to hold our audio data
        private const int MAX_PAYLOAD_SIZE = 1024;
        private byte[] _compressedVoiceBuffer = new byte[MAX_PAYLOAD_SIZE];
        byte[] _uncompressedVoiceBuffer = new byte[1024 * 22]; // Max size of an uncompressed voice chunk

        public void InitVoice()
        {
            // Steam optimally records at 11025, 22050, or 44100 Hz depending on the user's hardware/settings.
            uint sampleRate = SteamUser.GetVoiceOptimalSampleRate();

            // Initialize MonoGame's DynamicSoundEffectInstance to accept PCM data matching Steam's sample rate
            _audioPlayback = new DynamicSoundEffectInstance((int)sampleRate, AudioChannels.Mono);
            _audioPlayback.Play();
        }

        public void UpdateVoice()
        {
            EVoiceResult availableResult = SteamUser.GetAvailableVoice(out uint compressedBytesAvailable);

            if (availableResult == EVoiceResult.k_EVoiceResultOK && compressedBytesAvailable > 0)
            {
                EVoiceResult getVoiceResult = SteamUser.GetVoice(true, _compressedVoiceBuffer, (uint)_compressedVoiceBuffer.Length, out uint bytesWritten);
                
                if (getVoiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(PacketType.VoiceChat, PacketReliability.Unrelyable);
                    //Add to writer
                    // Add to writer: First the size of the payload, then the payload itself
                    w.Write((ushort)bytesWritten);
                    w.Write(_compressedVoiceBuffer, 0, (int)bytesWritten);
                }
            }
        }

        public void readVoice(System.IO.BinaryReader r)
        {
            if (!Ref.netsett.NetVoiceMuted())
            {
                //read here
                ushort bytesWritten = r.ReadUInt16();

                // Read the actual compressed payload directly into our buffer
                int bytesRead = r.Read(_compressedVoiceBuffer, 0, bytesWritten);

                // Ensure we actually read the data we expected
                if (bytesRead > 0 && bytesRead == bytesWritten)
                {
                    uint optimalRate = SteamUser.GetVoiceOptimalSampleRate();

                    EVoiceResult decompressResult = SteamUser.DecompressVoice(_compressedVoiceBuffer, bytesWritten, _uncompressedVoiceBuffer, (uint)_uncompressedVoiceBuffer.Length, out uint nBytesWritten, optimalRate);

                    // 4. Submit the decompressed raw PCM audio to MonoGame for playback
                    if (decompressResult == EVoiceResult.k_EVoiceResultOK && nBytesWritten > 0)
                    {
                        // SubmitBuffer expects standard little-endian PCM wave data, which Steam kindly provides
                        _audioPlayback.SubmitBuffer(_uncompressedVoiceBuffer, 0, (int)nBytesWritten);
                    }
                }
            }
        }

        public void StartRecording()
        {
            SteamUser.StartVoiceRecording();
        }

        public void StopRecording()
        {
            SteamUser.StopVoiceRecording();
        }

        public void UpdateVolume()
        {
            // Clamp the value just to be safe, ensuring it stays between 0 and 1
            if (_audioPlayback != null)
            {
                _audioPlayback.Volume = Ref.netsett.NetVoiceVol();
            }
        }

        public void DisposeVoice()
        {
            StopRecording();
            _audioPlayback?.Stop();
            _audioPlayback?.Dispose();
        }
    }

}
