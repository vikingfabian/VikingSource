using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace;
using VikingEngine.Network;

namespace VikingEngine.Network
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

        /// <summary>
        /// Distance between players
        /// </summary>
        public int PlayerSpacing = 0;

        StructList<StoredNetworkGamer> storedGamers = new StructList<StoredNetworkGamer>(8);

        public StoredNetworkGamer getStoredGamer(ulong id)
        {
            for (int i = 0; i < storedGamers.Count; i++)
            {
                if (storedGamers.array[i].id == id)
                { 
                    return storedGamers.array[i];
                }
            }

            StoredNetworkGamer gamer = new StoredNetworkGamer(id);
            gamer.index = storedGamers.Count;
            storedGamers.Add(gamer);

            return gamer;
        }

        public NetworkSettings()
        {
            Ref.netsett = this;
        }
    }
}
