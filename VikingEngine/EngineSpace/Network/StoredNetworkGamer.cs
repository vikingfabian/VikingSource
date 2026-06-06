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
        public bool muteVoice, muteText, mutePins, muteInGameCommunications, muteCreations, muteErrors;
        public float voiceVolume;

        public GamerCommunicationSetting()
        {
            voiceVolume = 1;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(voiceVolume);

            new EightBit(muteVoice, muteText, mutePins, muteInGameCommunications, muteCreations, muteErrors).write(w);
        }

        public void read(System.IO.BinaryReader r, int storageVersion)
        {
            voiceVolume = r.ReadSingle();

            var bits = EightBit.FromStream(r);
            bits.Get(out muteVoice, out muteText, out mutePins, out muteInGameCommunications, out muteCreations, out muteErrors);
        }
    }

    struct StoredNetworkGamer
    {
        public int index;
        public ulong id;
        public string name;

        public BanStatus ban;
        public GamerCommunicationSetting communicationSetting;
        

        public StoredNetworkGamer(ulong id)
        { 
            this.id = id;
            communicationSetting = new GamerCommunicationSetting();
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(id);

            StreamLib.WriteString(w, name);

            w.Write((byte)ban);
            communicationSetting.write(w);

            
        }

        public void read(System.IO.BinaryReader r, int storageVersion)
        {
            id = r.ReadUInt64();

            name = StreamLib.ReadString(r);

            ban = (BanStatus)r.ReadByte();
            communicationSetting.read(r, storageVersion);
        }
    }

     

    enum BanStatus
    { 
        None,
        Warning,
        Banned,
    }
}
