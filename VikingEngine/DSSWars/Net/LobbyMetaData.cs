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
            LobbyAliveDataKey,
            VersionDataKey,
            LobbyPublicityDataKey,

            "mode",//4
            "difficulty",//5
            "allow_casual",//6
            "pvp",//7
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
                alive.ToString(),
                Engine.LoadContent.EngineVersion,
                ((int)lobbyPublicity).ToString(),

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
            bool.TryParse(Values[1], out alive);
            Version = Values[2];
            MatchingVersion = Engine.LoadContent.EngineVersion == Version;

            if (int.TryParse(Values[3], out int publicity))
            {
                lobbyPublicity = (LobbyPublicity)publicity;
            }

            if (int.TryParse(Values[4], out int intmode))
            {
                mode = (GameModeMainType)intmode;
            }
            int.TryParse(Values[5], out difficulty);            
            bool.TryParse(Values[6], out allowCasual);
            bool.TryParse(Values[7], out hasPvp);

            int.TryParse(Values[8], out playerCount);
            int.TryParse(Values[9], out maxPlayerCount);
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
