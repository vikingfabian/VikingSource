using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Net
{
    class LobbyMetaData : AbsLobbyMetaData
    {
        static readonly string[] KEYS = {
            NameKey,
            HostIdKey,
            LobbyAliveDataKey,
            VersionDataKey,
            LobbyPublicityDataKey,

            "mode",//5
            "difficulty",//6
            "allow_casual",//7
            "pvp",//8
            "playerCount",
            "maxPlayerCount",
        };

        public GameModeMainType mode;
        public int difficulty;
        public bool allowCasual;
        public bool hasPvp;
        public int playerCount;
        public int maxPlayerCount;
        public override string[] GetKeys()
        {
            return KEYS;
        }

        public override void CollectValues()
        {
            Values = new string[] {
                SteamFriends.GetPersonaName(),
                SteamUser.GetSteamID().ToString(),
                alive.ToString(),
                Engine.LoadContent.EngineVersion,
                ((int)Ref.netsett.lobbyPublicity).ToString(),

                ((int)DssRef.difficulty.setting_gameMode).ToString(),
                DssRef.difficulty.TotalDifficulty().ToString(),
                Ref.netsett.hostSettings.allowCasualControls.ToString(),
                Ref.netsett.HasPvp.ToString(),
                (Ref.netSession.RemoteGamersCount +1).ToString(),
                Ref.netsett.maxPlayerCount.ToString(),
            };
        }
        public override void OnDataRecieved()
        {
            name = Values[0];
            if (ulong.TryParse(Values[1], out var id))
            {
                host = new CSteamID(id);
            }
            bool.TryParse(Values[2], out alive);
            Version = Values[3];
            MatchingVersion = Engine.LoadContent.EngineVersion == Version;

            if (int.TryParse(Values[4], out int publicity))
            {
                lobbyPublicity = (LobbyPublicity)publicity;
            }
            

            const int CustomStartIx = 5;
            if (int.TryParse(Values[CustomStartIx], out int intmode))
            {
                mode = (GameModeMainType)intmode;
            }
            int.TryParse(Values[CustomStartIx +1], out difficulty);            
            bool.TryParse(Values[CustomStartIx +2], out allowCasual);
            bool.TryParse(Values[CustomStartIx +3], out hasPvp);

            int.TryParse(Values[CustomStartIx +4], out playerCount);
            int.TryParse(Values[CustomStartIx + 5], out maxPlayerCount);
        }

        public GameModeMainType GameMode()
        {
            GameModeMainType gameMode = GameModeMainType.NUM;
            if (int.TryParse(Values[4], out int value))
            {
                gameMode = (GameModeMainType)value;
            }
            return gameMode;
        }

        public int TotalDifficulty()
        {
            if (int.TryParse(Values[5], out int value))
            {
                return value;
            }
            return -1;
        }

    }
}
