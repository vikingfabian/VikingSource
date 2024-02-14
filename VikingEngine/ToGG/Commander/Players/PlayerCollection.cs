using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.LevelSetup;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.Players
{
    class PlayerCollection : AbsPlayerCollection
    {
        public ListWithSelection<Players.AbsCmdPlayer> allPlayers = new ListWithSelection<AbsCmdPlayer>();
        public Players.LocalPlayer localHost = null;        

        public PlayerCollection() 
            : base()
        {
            Commander.cmdRef.players = this;
        }

        public void createPlayers(GameSetup gameSetup)
        {
            //gameSetup.initRelations();
            allPlayers.list = new List<Players.AbsCmdPlayer>(gameSetup.lobbyMembers.Count);

            for (int i = 0; i < gameSetup.lobbyMembers.Count; ++i)
            {
                addPlayer(gameSetup.lobbyMembers[i].createPlayer(i, gameSetup));
            }
            //Engine.XGuide.LocalHost.view.Camera = camera;
        }

        void addPlayer(Players.AbsCmdPlayer p)
        {
            allPlayers.Add(p, p.pData.globalPlayerIndex == 0);
            if (p is Players.LocalPlayer && localHost == null)
            {
                localHost = p as Players.LocalPlayer;
            }
        }

         

        public override AbsGenericPlayer readGenericPlayer(System.IO.BinaryReader r)
        {
            int number = r.ReadByte();
            return allPlayers.list[number];
        }

        public LocalPlayer ActiveLocalPlayer()
        {
            if (allPlayers.Selected() is LocalPlayer)
            {
                return allPlayers.Selected() as LocalPlayer;
            }
            else
            {
                return localHost;
            }
        }

        public List<AbsCmdPlayer> opponentPlayers(AbsCmdPlayer toPlayer)
        {
            List<AbsCmdPlayer> opponents = new List<AbsCmdPlayer>(1);

            foreach (var m in allPlayers.list)
            {
                if (m.pData.globalPlayerIndex != toPlayer.pData.globalPlayerIndex)
                {
                    opponents.Add(m);
                }
            }

            return opponents;
        }

        public SpottedArrayCounter<AbsUnit> CollectEnemyUnits(int friendlyGlobalIndex)
        {
            Players.AbsCmdPlayer opponentPlayer = null;
            for (int i = 0; i < PlayerCount; ++i)
            {
                if (i != friendlyGlobalIndex)
                {
                    opponentPlayer =Player(i);
                }
            }

            return (SpottedArrayCounter<AbsUnit>)opponentPlayer.unitsColl.unitsCounter.IClone();
        }

        public SpottedArrayCounter<AbsUnit> CollectFriendlyUnits(int friendlyGlobalIndex)
        {
            var friendlyUnits = Player(friendlyGlobalIndex).unitsColl.unitsCounter;

            return friendlyUnits;
        }

        public override void clearUnits()
        {
            foreach (var m in allPlayers.list)
            {
                m.unitsColl.clearUnits();
            }
        }

        public override AbsGenericPlayer getGenericPlayer(int ix)
        {
            return allPlayers.list[ix];
        }
        public override int PlayerCount
        {
            get
            {
                return allPlayers.list.Count;
            }
        }

        public override void OnOpenCloseMenu(bool open)
        {
            localHost.onOpenMenu(open);
        }

        public Players.AbsCmdPlayer Player(int number) { return allPlayers.list[number]; }
        public override AbsGenericPlayer GenPlayer(int globalIndex)
        {
            return allPlayers.list[globalIndex];
        }
        //public int PlayerCount { get { return allPlayers.list.Count; } }

        public override bool IsOpponent(AbsUnit u1, AbsUnit u2)
        {
            return u1.globalPlayerIndex != u2.globalPlayerIndex;
        }
        public override bool IsOpponent(int p1Index, int p2Index)
        {
            return true;
        }
        public override List<AbsGenericPlayer> allTeamPlayers(int team)
        {
            //throw new NotImplementedException();
            List<AbsGenericPlayer> result = new List<AbsGenericPlayer>(4);

            foreach (var m in allPlayers.list)
            {
                if (m.pData.teamIndex == team)
                {
                    result.Add(m);
                }
            }

            return result;
        }

        public override void OnUnitsCountChange()
        {
            //throw new NotImplementedException();
        }

        public override AbsGenericPlayer LocalHost()
        {
            return localHost;
        }
    }
}
