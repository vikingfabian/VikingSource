using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DataStream;

namespace VikingEngine.PJ
{
    class Storage
    {
        const int Version = 3;

        public List<GamerData> joinedGamersSetup = null;
        public List<GamerData> startingRemoteGamers = null;
        public GamerData previousVictor = null;
        PartyGameMode mode;
        
        public GameModeSettings modeSettings;
        public Network.LobbyPublicity lobbyPublicity = Network.LobbyPublicity.FriendsOnly;

        public Storage()
        {
            PjRef.storage = this;
            Mode = PartyGameMode.Jousting;
#if PCGAME
            saveLoad(false, false);
#endif
        }

        public void saveLoad(bool save, bool startThread)
        {
            FilePath filePath = new FilePath(
                null,
                "PartyJoustingSettings", ".sav",
                true,
                false);
            
            new FileIO_2(save, filePath, write, read, startThread);
        }

        void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            Ref.gamesett.writeEmbeddedSettingsAndVersion(w);

            w.Write((byte)mode);
            DataStream.SafeStream safeStream = new DataStream.SafeStream(w);

            var sw = safeStream.beginWriteChunk();
            if (joinedGamersSetup == null)
            {
                sw.Write((int)0);
            }
            else
            {
                sw.Write(joinedGamersSetup.Count);
                foreach (var m in joinedGamersSetup)
                {
                    m.write(sw);
                }
            }
            safeStream.endWriteChunk();

            w.Write((byte)lobbyPublicity);
        }

        void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            if (version >= 3)
            {
                Ref.gamesett.readEmbeddedSettingsAndVersion(r);
            }

            mode = (PartyGameMode)r.ReadByte();
            modeSettings = new GameModeSettings(mode);
            DataStream.SafeStream safeStream = new DataStream.SafeStream(r);

            safeStream.beginReadChunk();
            int joinedGamersCount = r.ReadInt32();
            for (int i = 0; i < joinedGamersCount; ++i)
            {
                if (joinedGamersSetup == null)
                {
                    joinedGamersSetup = new List<GamerData>();
                }

                joinedGamersSetup.Add(new GamerData(r, version));
            }
            safeStream.endReadChunk(PlatformSettings.DevBuild);

            if (version >= 1)
            {
                lobbyPublicity = (Network.LobbyPublicity)r.ReadByte();
            }
        }

        public PartyGameMode Mode
        {
            get { return mode; }
            set { mode = value; modeSettings = new GameModeSettings(value); }
        }
    }
}
