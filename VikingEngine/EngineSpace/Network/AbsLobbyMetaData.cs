using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Network
{
    abstract class AbsLobbyMetaData
    {
        public const string LobbyTimeDataKey = "TIME";
        public const string NameKey = "NAME";

        public bool MatchingVersion;
        public string Version;
        public bool alive = true;
        public string name;
        public LobbyPublicity lobbyPublicity;

        //public const string LobbyTimeKey = "TIME";
        public const string LobbyAliveDataKey = "ALIVE";
        protected const string LobbyPublicityDataKey = "PUBLIC";
        protected const string VersionDataKey = "VER";
        

        abstract public string[] GetKeys();
        public string[] Values;


        abstract public void CollectValues();
        abstract public void OnDataRecieved();

        //protected string PublicityString()
        //{
        //    return ((int)lobbyPublicity).ToString();
        //}
    }
}
