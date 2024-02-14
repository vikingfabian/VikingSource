#if PCGAME
using VikingEngine.LootFest;
using VikingEngine.LootFest.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    class AudioStreamer
    {
        /* Fields */
        uint sampleRate;
        AudioChannels channels;
        DynamicSoundEffectInstance sfx;

        /* Constructors */
        public AudioStreamer(uint sampleRate, AudioChannels channels)
        {
            this.sampleRate = sampleRate;
            this.channels = channels;

            sfx = new DynamicSoundEffectInstance((int)sampleRate, channels);
            FrameworkDispatcher.Update(); // make sure this has been called at least once, or Play() will raise an exception.
            sfx.Play();
        }

        /* Novelty Methods */
        /// <summary>
        /// Data must be submitted in 16 bit signed integer format
        /// </summary>
        /// <param name="data"></param>
        public void SubmitBuffer(byte[] data, uint relevantDataSize, bool littleEndian)
        {
            bool convertEndianness = (BitConverter.IsLittleEndian != littleEndian);

            // we need the number of samples to be a multiple of ten
            if (relevantDataSize % 10 != 0)
            {
                // so we truncate trailing bytes
                relevantDataSize = (relevantDataSize / 10) * 10;
            }

            if (relevantDataSize > 0)
            {
                byte[] relevantData = new byte[relevantDataSize];

                for (int i = 0; i < (int)relevantDataSize - 1; i += 2)
                {
                    if (convertEndianness)
                    {
                        relevantData[i] = data[i + 1];
                        relevantData[i + 1] = data[i];
                    }
                    else
                    {
                        relevantData[i] = data[i];
                        relevantData[i + 1] = data[i + 1];
                    }
                }

                sfx.SubmitBuffer(relevantData);
            }
        }
    }

    class SteamVOIP
    {
        /* Constants */
        const uint SAMPLE_RATE = 11025;
        const uint DECOMPRESSED_BUFFER_SIZE = 22000; // steam suggests size >= 22kb
        const uint COMPRESSED_INPUT_BUFFER_SIZE = 8000; // steam suggests size >= 8kb

        /* Static */
        static byte[] decompressedBuffer;
        static byte[] compressedMicInputBuffer;

        /* Fields */
        bool currentlyRecording;
        Player player;
        AudioStreamer audioStream;

        /* Constructors */
        public SteamVOIP()
        {
            currentlyRecording = false;
            player = null;
            audioStream = new AudioStreamer(SAMPLE_RATE, AudioChannels.Mono);

            decompressedBuffer = new byte[DECOMPRESSED_BUFFER_SIZE];
            compressedMicInputBuffer = new byte[COMPRESSED_INPUT_BUFFER_SIZE];
        }

        public void readVoice(Network.ReceivedPacket packet)
        {
            bool remoteUsesLittleEndian = packet.r.ReadBoolean();
            uint length = packet.r.ReadUInt32();

            byte[] voiceData = packet.r.ReadBytes((int)length);

            OnReceiveVoicePacket(voiceData, length, remoteUsesLittleEndian);
        }

        /* Novelty Methods */
        public void OnReceiveVoicePacket(byte[] voiceData, uint receivedVoiceDataSize, bool remoteUsesLittleEndian)
        {
            uint uncWritten;

            // 5. When the target users receive the voice data, they call SteamUser()->DecompressVoice() to turn the compressed data back into audio.
            EVoiceResult result = SteamAPI.SteamUser().DecompressVoice(voiceData, receivedVoiceDataSize, decompressedBuffer, DECOMPRESSED_BUFFER_SIZE, out uncWritten, SAMPLE_RATE);

            Debug.Log(result.ToString());
            //bool remoteUsesLittleEndian = true; // TODO(Martin): Fabian, make sure endianness is correct.
            audioStream.SubmitBuffer(voiceData, uncWritten, remoteUsesLittleEndian);
        }

        public void Update()
        {
            FrameworkDispatcher.Update(); // necessary for audio streaming

            if (player == null)
            {
                if (LfRef.gamestate != null)
                    player = LfRef.gamestate.LocalHostingPlayer;
                else
                    return;
            }

            //if (!currentlyRecording && player.pData.inputMap.DownEvent(Input.ButtonActionType.GameAltButton))
            {
    
                //// 1. Call SteamUser()->StartVoiceRecording() to begin recording.
                //SteamAPI.SteamUser().StartVoiceRecording();
                //// 2. If you are recording for push-to-talk voice chat, call SteamFriends()->SetInGameVoiceSpeaking() to mute any voice chat the user may be doing through the Steam friends UI.
                //SteamAPI.SteamFriends().SetInGameVoiceSpeaking(player.StaticNetworkId, true);

                //currentlyRecording = true;
            }

            if (currentlyRecording)
            {
                //if (player.pData.inputMap.UpEvent(Input.ButtonActionType.GameAltButton))
                {
                    // 6. Call SteamUser()->StopVoiceRecording() when it is time to stop recording.
                    SteamAPI.SteamUser().StopVoiceRecording();
                    // 7. If you used SteamFriends()->SetInGameVoiceSpeaking() when you started recording, call it again to unmute voice chat in Steam.
                    SteamAPI.SteamFriends().SetInGameVoiceSpeaking(player.pData.netId(), false);

                    // NOTE(Martin): But we are not done until GetVoice returns k_EVoiceResultNotRecording, so currentlyRecording is still true.
                }

                uint receivedCompressedVoiceDataSize;
                uint receivedUncompresseVoiceDataSize; // we don't care

                // 3. Call SteamUser()->GetVoice() regularly to get the latest recorded data.
                EVoiceResult result = SteamAPI.SteamUser().GetVoice(true, compressedMicInputBuffer, COMPRESSED_INPUT_BUFFER_SIZE, out receivedCompressedVoiceDataSize, false, null, 0, out receivedUncompresseVoiceDataSize, SAMPLE_RATE);
                if (result != EVoiceResult.k_EVoiceResultOK)
                {
                    if (result == EVoiceResult.k_EVoiceResultNotRecording)
                    {
                        currentlyRecording = false; // this is when we are ACTUALLY done.
                        return;
                    }
                    else if (result != EVoiceResult.k_EVoiceResultNoData)
                    {
                        PrintError(result);
                    }
                }

                // 4. Relay the data to the destination users. This can be accomplished via our peer-to-peer networking API.

                // TODO(Martin): Fabian, send voiceData over P2P!
                if (result == EVoiceResult.k_EVoiceResultOK)
                {
                    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.VoiceChat, Network.PacketReliability.Reliable);
                    w.Write(System.BitConverter.IsLittleEndian);
                    w.Write((uint)compressedMicInputBuffer.Length);
                    w.Write(compressedMicInputBuffer);
                }
            }
        }

        void PrintError(EVoiceResult result)
        {
            throw new InvalidOperationException("VOIP call returned " + result.ToString());
        }
    }
}
#endif