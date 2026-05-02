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
        byte[] _compressedVoiceBuffer = new byte[1024 * 8];
        byte[] _uncompressedVoiceBuffer = new byte[1024 * 22]; // Max size of an uncompressed voice chunk

        public void InitChat()
        {
            // Steam optimally records at 11025, 22050, or 44100 Hz depending on the user's hardware/settings.
            uint sampleRate = SteamUser.GetVoiceOptimalSampleRate();

            // Initialize MonoGame's DynamicSoundEffectInstance to accept PCM data matching Steam's sample rate
            _audioPlayback = new DynamicSoundEffectInstance((int)sampleRate, AudioChannels.Mono);
            _audioPlayback.Play();
        }

        public void UpdateChat()
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

        public void readChat(System.IO.BinaryReader r)
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

        public void StartRecording()
        {
            SteamUser.StartVoiceRecording();
        }

        public void StopRecording()
        {
            SteamUser.StopVoiceRecording();
        }

        public void DisposeChat()
        {
            StopRecording();
            _audioPlayback?.Stop();
            _audioPlayback?.Dispose();
        }
    }

//    using System;
//using Steamworks;
//using Microsoft.Xna.Framework.Audio;

    //class SteamVoiceManager : IDisposable
    //{
    //    private DynamicSoundEffectInstance _audioPlayback;

    //    // Buffers to hold our audio data
    //    private byte[] _compressedVoiceBuffer = new byte[1024 * 8];
    //    private byte[] _uncompressedVoiceBuffer = new byte[1024 * 22]; // Max size of an uncompressed voice chunk

    //    public SteamVoiceManager()
    //    {
    //        // Steam optimally records at 11025, 22050, or 44100 Hz depending on the user's hardware/settings.
    //        uint sampleRate = SteamUser.GetVoiceOptimalSampleRate();

    //        // Initialize MonoGame's DynamicSoundEffectInstance to accept PCM data matching Steam's sample rate
    //        _audioPlayback = new DynamicSoundEffectInstance((int)sampleRate, AudioChannels.Mono);
    //        _audioPlayback.Play();
    //    }

    //    public void StartRecording()
    //    {
    //        SteamUser.StartVoiceRecording();
    //    }

    //    public void StopRecording()
    //    {
    //        SteamUser.StopVoiceRecording();
    //    }

    //    // Call this in your Game.Update() loop
    //    public void Update()
    //    {
    //        //uint compressedBytesAvailable;
    //        //uint uncompressedBytesAvailable;

    //        // 1. Check if Steam has captured any voice data from the microphone
    //        EVoiceResult availableResult = SteamUser.GetAvailableVoice(out uint compressedBytesAvailable);

    //        if (availableResult == EVoiceResult.k_EVoiceResultOK && compressedBytesAvailable > 0)
    //        {
    //            //uint bytesWritten;
    //            //uint uncompressedBytesWritten;

    //            // 2. Retrieve the COMPRESSED voice data.
    //            EVoiceResult getVoiceResult = SteamUser.GetVoice(true, _compressedVoiceBuffer, (uint)_compressedVoiceBuffer.Length, out uint bytesWritten);
    //            //    bWantCompressed: true,
    //            //    pDestBuffer: _compressedVoiceBuffer,
    //            //    cbDestBufferSize: (uint)_compressedVoiceBuffer.Length,
    //            //    nBytesWritten: out bytesWritten,
    //            //    bWantUncompressed: false,
    //            //    pUncompressedDestBuffer: null,
    //            //    cbUncompressedDestBufferSize: 0,
    //            //    nUncompressBytesWritten: out uncompressedBytesWritten,
    //            //    nUncompressedVoiceDesiredSampleRate: 0
    //            //);

    //            if (getVoiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
    //            {
    //                // ----------------------------------------------------------------------
    //                // MULTIPLAYER NOTE: 
    //                // At this exact point in a real game, you would take `_compressedVoiceBuffer` 
    //                // (up to `bytesWritten` in length) and send it over your network packet 
    //                // (e.g., using SteamNetworkingSockets) to other players.
    //                // ----------------------------------------------------------------------

    //                // 3. Decompress the data (The RECEIVING client does this in a real game)
    //                //uint bytesDecompressed;
    //                uint optimalRate = SteamUser.GetVoiceOptimalSampleRate();

    //                EVoiceResult decompressResult = SteamUser.DecompressVoice(_compressedVoiceBuffer, bytesWritten, _uncompressedVoiceBuffer, (uint)_uncompressedVoiceBuffer.Length, out uint nBytesWritten, optimalRate);
    //                //    pCompressed: _compressedVoiceBuffer,
    //                //    cbCompressed: bytesWritten,
    //                //    pDestBuffer: _uncompressedVoiceBuffer,
    //                //    cbDestBufferSize: (uint)_uncompressedVoiceBuffer.Length,
    //                //    nBytesWritten: out bytesDecompressed,
    //                //    nDesiredSampleRate: optimalRate
    //                //);

    //                // 4. Submit the decompressed raw PCM audio to MonoGame for playback
    //                if (decompressResult == EVoiceResult.k_EVoiceResultOK && nBytesWritten > 0)
    //                {
    //                    // SubmitBuffer expects standard little-endian PCM wave data, which Steam kindly provides
    //                    _audioPlayback.SubmitBuffer(_uncompressedVoiceBuffer, 0, (int)nBytesWritten);
    //                }
    //            }
    //        }


    //    }

    //    public void Dispose()
    //    {
    //        StopRecording();
    //        _audioPlayback?.Stop();
    //        _audioPlayback?.Dispose();
    //    }
    //}
}
