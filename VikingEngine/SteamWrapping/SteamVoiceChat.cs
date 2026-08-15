using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
        

        public const float VoiceDisplayTimeMs = 300;
        // Buffers to hold our audio data
        private const int MAX_PAYLOAD_SIZE = 1024;
        private byte[] _compressedVoiceBuffer = new byte[MAX_PAYLOAD_SIZE];
        byte[] _uncompressedVoiceBuffer = new byte[1024 * 22]; // Max size of an uncompressed voice chunk
        public bool recordingOn = false;
        //public void InitVoice()
        //{
        //    // Steam optimally records at 11025, 22050, or 44100 Hz depending on the user's hardware/settings.
            

        //    // Initialize MonoGame's DynamicSoundEffectInstance to accept PCM data matching Steam's sample rate
            
        //}

        public void UpdateVoice()
        {
            
            EVoiceResult availableResult = SteamUser.GetAvailableVoice(out uint compressedBytesAvailable);

            if (availableResult == EVoiceResult.k_EVoiceResultOK && compressedBytesAvailable > 0)
            {
                EVoiceResult getVoiceResult = SteamUser.GetVoice(true, _compressedVoiceBuffer, (uint)_compressedVoiceBuffer.Length, out uint bytesWritten);
                
                if (getVoiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
                {
                    P2PManager.localPeer?.lastvoice.setNow();

                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(PacketType.VoiceChat, PacketReliability.Unrelyable);
                    //Add to writer
                    // Add to writer: First the size of the payload, then the payload itself
                    w.Write((ushort)bytesWritten);
                    w.Write(_compressedVoiceBuffer, 0, (int)bytesWritten);
                }
            }
        }

        public void readVoice(AbsNetworkPeer peer, System.IO.BinaryReader r)
        {
            float volume = Ref.gamesett.NetVoiceVol() * peer.storedData.communicationSetting.voiceVolume;

            if (volume > 0)
            {
                //read here
                peer.isRecording = true;
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
                        if (peer._audioPlayback == null)
                        {
                            uint sampleRate = SteamUser.GetVoiceOptimalSampleRate();
                            peer._audioPlayback = new DynamicSoundEffectInstance((int)sampleRate, AudioChannels.Mono);
                            peer._audioPlayback.Play();
                        }

                        peer.lastvoice.setNow();
                        // SubmitBuffer expects standard little-endian PCM wave data, which Steam kindly provides
                        peer._audioPlayback.SubmitBuffer(_uncompressedVoiceBuffer, 0, (int)nBytesWritten);
                        peer._audioPlayback.Volume = Bound.Set(volume, 0, 1);
                    }
                }
            }
        }

        public void StartRecording()
        {
            if (!recordingOn)
            {
                recordingOn = true;
                if (P2PManager.localPeer != null)
                {
                    P2PManager.localPeer.isRecording = recordingOn;
                }
                SteamUser.StartVoiceRecording();
            }
        }

        public void StopRecording()
        {
            recordingOn = false;
            if (P2PManager != null && P2PManager.localPeer != null)
            {
                P2PManager.localPeer.isRecording = recordingOn;
            }
            SteamUser.StopVoiceRecording();
        }

        public void ToggleRecording()
        {
            if (recordingOn)
            {
                StopRecording();
            }
            else
            { 
                StartRecording();
            }
        }

        //public void UpdateVolume()
        //{
        //    // Clamp the value just to be safe, ensuring it stays between 0 and 1
        //    if (_audioPlayback != null)
        //    {
        //        _audioPlayback.Volume = Ref.gamesett.NetVoiceVol();
        //    }
        //}

        public void DisposeVoice()
        {
            StopRecording();
            //_audioPlayback?.Stop();
            //_audioPlayback?.Dispose();
        }
    }

}
