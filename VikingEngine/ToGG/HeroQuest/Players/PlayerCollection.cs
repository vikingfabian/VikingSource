using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Players
{
    class PlayerCollection : AbsPlayerCollection
    {
        public bool needsRefresh = true;
        public const int HeroTeam = 0;
        public const int DungeonMasterTeam = 1;

        public int currentTeam = -1;

        public const int MaxPlayerCount = OldNetLobby.MaxPlayers + 1;

        public AiPlayer dungeonMaster;
        public LocalPlayer localHost;
        public BoardDesignPlayerHQ editorHost;

        public List<AbsHQPlayer> allPlayersUnsorted = new List<AbsHQPlayer>(MaxPlayerCount);
        public SpottedArray<AbsHQPlayer> allPlayers;
        //public SpottedArrayCounter<AbsHQPlayer> allPlayersCounter;
        //SpottedArrayCounter<AbsHQPlayer> allPlayersAsynchCounter;
        //public SpottedArrayTypeCounter<AbsHQPlayer> remotePlayersCounter;
        
        public PlayerCollection() 
            : base()
        {
            hqRef.players = this;
            allPlayers = new SpottedArray<AbsHQPlayer>(MaxPlayerCount);
            //allPlayersCounter = allPlayers.counter();
            //allPlayersAsynchCounter = allPlayers.counter();
            //remotePlayersCounter = new SpottedArrayTypeCounter<AbsHQPlayer>(allPlayers, typeof(RemotePlayer));

            new AiPlayer(new Engine.AiPlayerData());
            if (toggRef.InEditor)
            {
                editorHost = new BoardDesignPlayerHQ(Engine.XGuide.LocalHost);
            }
            else
            {
                new LocalPlayer(Engine.XGuide.LocalHost);
            }
            //sortPlayers();
        }

        public void update_asynch(float time)
        {
            var allPlayersAsynchCounter = allPlayers.counter();
            while (allPlayersAsynchCounter.Next())
            {
                allPlayersAsynchCounter.sel.update_asynch(time);
            }
        }

        public void addPlayer(AbsHQPlayer player)
        {
            if (player is LocalPlayer)
            {
                localHost = (LocalPlayer)player;
            }
            else if (player is AiPlayer)
            {
                dungeonMaster = (AiPlayer)player;
            }

            player.assignPlayerIx(allPlayersUnsorted.Count);
            allPlayersUnsorted.Add(player);
            
        }

        public void sortPlayers()
        {
            foreach (var p in allPlayersUnsorted)
            {
                allPlayers.HardSet(p, p.pData.globalPlayerIndex);
            }

            allPlayersUnsorted = null;
        }

        public SpottedArrayCounter<AbsUnit> CollectEnemyUnits(ToGG.AbsGenericPlayer player)
        {
            if (player.pData.teamIndex == HeroTeam) 
            {
                return teamUnits(DungeonMasterTeam);
            }
            else
            {
                return teamUnits(HeroTeam);
            }
        }

        public SpottedArrayCounter<AbsUnit> CollectFriendlyUnits(AbsUnit unit)
        {
            return teamUnits(unit.Player.pData.teamIndex);
        }

        public SpottedArrayCounter<AbsUnit> CollectFriendlyUnits(int team)
        {
            return teamUnits(team);
        }

        public void unitsWithProperty(UnitPropertyType property, List<AbsUnit> result, int? teamFilter)
        {
          var allPlayersCounter =  allPlayers.counter();
            while (allPlayersCounter.Next())
            {
                if (teamFilter == null || allPlayersCounter.sel.pData.teamIndex == teamFilter)
                {
                    allPlayersCounter.sel.hqUnits.unitsWithProperty(property, result);
                }
            }
        }

        public SpottedArrayCounter<AbsUnit> teamUnits(int team)
        {
            if (team == HeroTeam)
            {
                SpottedArray<AbsUnit> units = new SpottedArray<AbsUnit>(16);
                units.Add(localHost.unitsColl.units);

                var remotePlayers = new SpottedArrayTypeCounter<AbsHQPlayer>(hqRef.players.allPlayers, typeof(RemotePlayer));

                while (remotePlayers.Next())
                {
                    units.Add(remotePlayers.GetSelection.unitsColl.units);
                }

                return units.counter();
            }
            else
            {
                return dungeonMaster.unitsColl.unitsCounter.Clone();
            }
        }

        public bool isOpponent(AbsGenericPlayer p1, AbsGenericPlayer p2)
        {
            return p1.pData.teamIndex != p2.pData.teamIndex;
        }

        public override AbsGenericPlayer getGenericPlayer(int ix)
        {
            if (ix < allPlayers.Count)
            {
                return allPlayers[ix];
            }
            return null;
        }
        public override int PlayerCount
        {
            get
            {
                if (allPlayersUnsorted == null)
                {
                    return allPlayers.Count;
                }
                else
                {
                    return allPlayersUnsorted.Count;
                }
            }
        }

        public override void clearUnits()
        {
            var allPlayersCounter = allPlayers.counter();
            while(allPlayersCounter.Next())
            {
                allPlayersCounter.sel.unitsColl.clearUnits();
            }
        }

        public AbsHQPlayer player(int globalIndex)
        {
            return allPlayers[globalIndex];
        }

        public override AbsGenericPlayer GenPlayer(int globalIndex)
        {
            return allPlayers[globalIndex];
        }

        public override AbsGenericPlayer readGenericPlayer(BinaryReader r)
        {
            int playerIx = r.ReadByte();
            AbsGenericPlayer result;
            arraylib.TryGet(allPlayers.Array, playerIx, out result);

            if (result == null)
            {
                hqRef.netManager.historyAdd("Get player fail ix" + playerIx.ToString());
            }

            return result;
        }

        public override void OnOpenCloseMenu(bool open)
        {
        }

        public AbsHQPlayer PlayerFromNetId(ulong id)
        {
            if (allPlayersUnsorted != null)
            {
                foreach (var m in allPlayersUnsorted)
                {
                    if (m.pData.Type != Engine.PlayerType.Ai && 
                        id == m.pData.netPeer().FullId)
                    {
                        return m;
                    }
                }
            }
            else
            {
                var allPlayersCounter = allPlayers.counter();
                while (allPlayersCounter.Next())
                {
                    if (allPlayersCounter.sel.pData.Type != Engine.PlayerType.Ai &&
                        id == allPlayersCounter.sel.pData.netPeer().FullId)
                    {
                        return allPlayersCounter.sel;
                    }
                }
            }
            return null;
        }


        public void netWritePlayer(System.IO.BinaryWriter w, AbsHQPlayer p)
        {
            w.Write((byte)p.pData.globalPlayerIndex);            
        }

        public void netWritePlayer(System.IO.BinaryWriter w, Unit unitOwner)
        {
            w.Write((byte)unitOwner.globalPlayerIndex);
        }

        public AbsHQPlayer netReadPlayer(System.IO.BinaryReader r)
        {
            return allPlayers[r.ReadByte()];
        }

        public override AbsGenericPlayer LocalHost()
        {
            return localHost;
        }

        public int HeroPlayersCount
        {
            get { return PlayerCount - 1; }
        }

        public bool PetsFillout
        {
            get
            {
                return hqRef.players.HeroPlayersCount == hqLib.AddPetsToPlayerCount;
            }
        }
        public override bool IsOpponent(AbsUnit u1, AbsUnit u2)
        {
            return allPlayers[u1.globalPlayerIndex].pData.teamIndex != allPlayers[u2.globalPlayerIndex].pData.teamIndex;
        }

        public bool isAlly(AbsUnit u1, AbsUnit u2)
        {
            return !IsOpponent(u1, u2);
        }

        public override bool IsOpponent(int p1Index, int p2Index)
        {
            return allPlayers[p1Index].pData.teamIndex != allPlayers[p2Index].pData.teamIndex;
        }
        public override List<AbsGenericPlayer> allTeamPlayers(int team)
        {
            List<AbsGenericPlayer> result = new List<AbsGenericPlayer>(4);

            var allPlayersCounter = allPlayers.counter();
            while (allPlayersCounter.Next())
            {
                if (allPlayersCounter.sel.pData.teamIndex == team)
                {
                    result.Add(allPlayersCounter.sel);
                }
            }

            return result;
        }

        public List<AbsGenericPlayer> activePlayers()
        {
            return hqRef.players.allTeamPlayers(currentTeam);
        }

        public AbsHQPlayer activePlayer()
        {
            return currentTeam == HeroTeam ? (AbsHQPlayer)localHost : (AbsHQPlayer)dungeonMaster;
        }

        public bool IsDungeonMasterTurn => currentTeam == DungeonMasterTeam;

        public bool IsHeroesTurn => currentTeam == HeroTeam;

        public override void OnUnitsCountChange()
        {
            needsRefresh = true;            
        }

        public override void OnMapPan(Vector2 screenPan)
        {
            base.OnMapPan(screenPan);
            var allPlayersCounter = allPlayers.counter();
            while (allPlayersCounter.Next())
            {
                allPlayersCounter.sel.OnMapPan(screenPan);
            }
        }
 
        public void Refresh()
        {
            needsRefresh = false;

            var players = allPlayers.counter();
            while (players.Next())
            {
                players.sel.hqUnits.enemies = CollectEnemyUnits(players.sel).array.toList();
                players.sel.hqUnits.friendly = CollectFriendlyUnits(players.sel.pData.teamIndex).array.toList();
            }
        }

        public int NextTeam()
        {
            int next;

            if (hqRef.setup.conditions.HasDungeonMaster)
            {
                next = lib.AlternateBetweenTwoValues(currentTeam,
                    HeroTeam, DungeonMasterTeam);
            }
            else
            {
                next = HeroTeam;
            }

            return next;
        }
    }
}
