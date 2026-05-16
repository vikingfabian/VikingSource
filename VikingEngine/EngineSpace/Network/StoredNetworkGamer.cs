using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Network
{
    //internal class StoredNetworkGamer
    //{
    //}
    struct GamerCommunicationSetting
    {
        public bool muteVoice, muteText, mutePins, muteInGameCommunications, muteCreations;
    }

    struct StoredNetworkGamer
    {
        public int index;
        public ulong id;
        public string name;

        public BanStatus ban;
        public GamerCommunicationSetting communicationSetting;
        public float voiceVolume;

        public StoredNetworkGamer(ulong id)
        { 
            this.id = id;
            voiceVolume = 1;
        }
    }

    enum BanStatus
    { 
        None,
        Warning,
        Banned,
    }
}
