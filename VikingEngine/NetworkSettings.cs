using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;

namespace VikingEngine
{
    class NetworkSettings
    {
        public bool hostNetwork = true;
        public bool findNetwork = true;

        public Network.NetworkCanJoinType hostSessionJoinType = Network.NetworkCanJoinType.Friends;
        public Network.NetworkCanJoinType findSessionJoinType = Network.NetworkCanJoinType.Open_for_all;

        float netVoiceVolume = 1f;
        public float NetVoiceVol() { return MathHelper.Clamp(netVoiceVolume * Ref.gamesett.MasterVolume, 0.0f, 1.0f); }
        public bool NetVoiceMuted() { return netVoiceVolume * Ref.gamesett.MasterVolume <= 0; }

        public VoiceOption voiceOption = VoiceOption.ButtonHold;
    }
}
