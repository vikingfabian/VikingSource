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

            "mode",
            "difficulty",
        };

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
            };
        }
        public override void OnDataRecieved()
        {
            name = Values[0];
            bool.TryParse(Values[1], out alive);
            MatchingVersion = Engine.LoadContent.EngineVersion == Values[2];

            if (int.TryParse(Values[3], out int publicity))
            {
                lobbyPublicity = (LobbyPublicity)publicity;
            }
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
