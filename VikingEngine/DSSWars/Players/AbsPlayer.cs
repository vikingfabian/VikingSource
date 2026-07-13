using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players
{
    abstract partial class AbsPlayer
    {
        public const int AggressionLevel0_Passive = 0;
        public const int AggressionLevel1_RevengeOnly = 1;
        public const int AggressionLevel2_RandomAttacks = 2;
        public const int AggressionLevel3_FocusedAttacks = 3;

        public bool IsPlayerNeighbor = false;
        //public Faction faction;
        public PFaction pfaction;
        public int aggressionLevel = AggressionLevel0_Passive;
        public bool protectedFromBotAttacks = false;
        protected bool ignorePlayerCapture = false;
        public bool personality_loner = false;
        public bool protectedFromDelete = false;
        public bool mayAttackPlayer = true;

        public Orders.Orders orders;
        abstract public void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType buildType, out bool intelligent);

        public PlayerProfile profile;
        public Texture2D flagTexture;

        //public Faction pfaction.GetFaction();
        public AbsPlayer()
        { }

        public void SetProfile(PlayerProfile profile)
        {
            this.profile = profile;
            flagTexture = profile.flag.flagDesign.CreateTexture(profile.flag);
        }

        virtual public void AssignFaction(Faction faction)
        {
            this.pfaction = faction.Pointer();
            //faction.player = this;
            faction.SetStartOwner(this);
            faction.onNewPlayerModels();
            DssRef.world.BordersUpdated = true;
        }

        virtual public void SetColor(Color selected, bool netShare)
        {
            //if (IsLocalPlayer())
            //{
            //    var clone = profile.flag.Clone();
            //    profile.flag = clone;
            //}
            profile.flag.col0_Main = selected;
            refreshFlag();

            if (netShare)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DssReColor, Network.PacketReliability.Reliable);
                pfaction.write(w);//Net.ObjectId.WriteFaction(w, faction);
                StreamLib.WriteColorStream_3B(w, selected);
            }
        }

        virtual public void refreshFlag()
        {
            flagTexture = profile.flag.flagDesign.CreateTexture(profile.flag);
            pfaction.GetFaction()?.onNewPlayerModels();
            
            DssRef.world.BordersUpdated = true;
        }

        public AbsPlayer(Faction faction, bool newGame)
        {
            this.pfaction = faction.Pointer();
            faction.SetStartOwner(this);

            if (newGame)
            {
                createStartupBarracks();

                int startGold = DssRef.difficulty.setting_gameMode == GameModeMainType.Sandbox ? 500 : 200;
                if (DssRef.storage.ruleset.factionStartSize == FactionStartSize.Settler)
                {
                    startGold = 6000;
                }

                faction.addGold_factionWide(startGold);
            }
        }

        /// <summary>
        /// Casual: upkeep in copper, Normal: upkeep in energy
        /// </summary>
        public float ConvertUpkeep(float upkeep, out bool casual)
        {
            casual = profile.casualControls;
            if (profile.casualControls)
            {
                return upkeep * DssConst.CasualSoldierDefaultCost_Copp;
            }
            else
            {
                return upkeep * DssConst.ManDefaultEnergyCost;
            }
        }

        public void createStartupBarracks()
        { 
            pfaction.GetFaction().mainCity?.createStartupBarracks();
        }

        virtual public void Update()
        { }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {

        }

        

        protected void readAiPlayerGameState(BinaryReader r, int subversion)
        {
            if (subversion < 72)
            {
                IsPlayerNeighbor = r.ReadBoolean();
                aggressionLevel = r.ReadByte();
                protectedFromBotAttacks = r.ReadBoolean();
            }
            else
            {
                aggressionLevel = r.ReadByte();
                var bools = new EightBit(r);
                bools.Get(out IsPlayerNeighbor, out protectedFromBotAttacks, out personality_loner, out protectedFromDelete, out mayAttackPlayer);
                
            }
        }

        virtual public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {

        }

        //virtual public void writeNet(System.IO.BinaryWriter w)
        //{

        //}
        //virtual public void readNet(System.IO.BinaryReader r)
        //{

        //}

        virtual public void oneSecUpdate()
        { }
       
        virtual public void aiPlayerAsynchUpdate(float time)
        { }

        //virtual public void onNewRelation(bool isActuator, Faction otherFaction, Communication.DiplomaticRelation rel, RelationType previousRelation)
        virtual public void onNewRelation(bool isActuator, PFaction otherPFaction, Communication.DiplomaticRelation rel, RelationType previousRelation, bool localAction)
        {
            //On peace, stop all attacking armies
            bool fromWar = Diplomacy.IsWar(previousRelation);
            bool toWar = Diplomacy.IsWar(rel.Relation);
            var faction = pfaction.GetFaction();
            //var otherFaction = otherPFaction.GetFaction();

            if (fromWar != toWar)
            {
                if (toWar)
                {
                    if (localAction)
                    {
                        faction.tradeAllianceWars(isActuator, otherPFaction);
                    }
                }
                else
                {
                    faction.stopAllAttacksAgainst(otherPFaction);
                }
            }

            if (rel.Relation == RelationType.RelationType3_Ally &&
                !rel.secret)
            {
                if (localAction)
                {
                    faction.tradeAllianceWars(isActuator, otherPFaction);
                }
            }
        }

        public void onPlayerNeighborCapture(LocalPlayer player)
        {
            if (player.IsBot())
            {
                if (ignorePlayerCapture)
                    return;

                ignorePlayerCapture = true;

                if (aggressionLevel == AggressionLevel0_Passive)
                {
                    if (DssRef.difficulty.aiAggressivity == AiAggressivity.Medium)
                    {
                        if (Ref.peRnd.Chance(0.35))
                        {
                            aggressionLevel = AggressionLevel2_RandomAttacks;
                        }
                        else if (Ref.peRnd.Chance(0.06))
                        {
                            aggressionLevel = AggressionLevel3_FocusedAttacks;
                        }
                    }
                    else if (DssRef.difficulty.aiAggressivity == AiAggressivity.High)
                    {
                        if (Ref.peRnd.Chance(0.6))
                        {
                            aggressionLevel = AggressionLevel2_RandomAttacks;
                        }
                        else
                        {
                            aggressionLevel = AggressionLevel3_FocusedAttacks;
                        }
                    }
                }
                else if (aggressionLevel == AggressionLevel2_RandomAttacks)
                {
                    if (DssRef.difficulty.aiAggressivity == AiAggressivity.Medium)
                    {
                        if (Ref.peRnd.Chance(0.05))
                        {
                            aggressionLevel = AggressionLevel3_FocusedAttacks;
                        }
                    }
                    else if (DssRef.difficulty.aiAggressivity == AiAggressivity.High)
                    {
                        if (Ref.peRnd.Chance(0.7))
                        {
                            aggressionLevel = AggressionLevel3_FocusedAttacks;
                        }
                    }
                }

                //player.GetAiPlayer().refreshAggression();

                ref var relation = ref DssRef.world.diplomacy.GetRefRelation(pfaction, player.pfaction);
                relation.SetWorseSpeakTerms(DssRef.world.diplomacy.SpeakTermsOnNeigbor_BadChance, DssRef.world.diplomacy.SpeakTermsOnNeigbor_NoneChance);

                if (pfaction.TryGetFaction(out var faction) && faction.Size() >= FactionSize.Big)
                {
                    protectedFromBotAttacks = true;
                }
            }
        }

        protected bool quickMatchUnits(bool checkIfParticipant)
        {
            if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
            {
                if (!checkIfParticipant || IsLocalPlayer() || DssRef.world.quickMatchFactions.Contains(pfaction))
                {
                    var faction = pfaction.GetFaction();
                    IntVector2 onTile = faction.mainCity.ArmySpawnTilePos();
                    Army mainArmy = faction.NewArmy(onTile);

                    for (int i = 0; i < 2; ++i)
                    {
                        new SoldierGroup(mainArmy, DssLib.SoldierProfile_StandardArcher, mainArmy.position);
                    }
                    for (int i = 0; i < 1; ++i)
                    {
                        new SoldierGroup(mainArmy, DssLib.SoldierProfile_Swordsman, mainArmy.position);
                    }

                    if (IsLocalPlayer() && DssRef.difficulty.honorGuard)
                    {
                        for (int i = 0; i < 1; ++i)
                        {
                            new SoldierGroup(mainArmy, DssLib.SoldierProfile_HonorGuard, mainArmy.position);
                        }
                    }

                    mainArmy.setAsStartArmy();

                    return true;
                }
            }
            return false;
        }

        protected void settlerGuardUnits()
        {
            var faction = pfaction.GetFaction();
            IntVector2 onTile = faction.mainCity.ArmySpawnTilePos();
            Army mainArmy = faction.NewArmy(onTile);

            if (IsHumanPlayer() && DssRef.difficulty.honorGuard)
            {
                new SoldierGroup(mainArmy, DssLib.SoldierProfile_HonorGuard, mainArmy.position);
            }
            else
            {
                new SoldierGroup(mainArmy, DssLib.SoldierProfile_Swordsman, mainArmy.position);
            }

            mainArmy.setAsStartArmy();
        }


        virtual public void createStartUnits(double unitCountMulti, bool settlerGuard)
        {   
        }

        virtual public void onGameStart(bool newGame) { }

        abstract public bool IsLocal { get; }

        abstract public bool IsBot();

        abstract public bool IsLocalPlayer();
        abstract public bool IsHumanPlayer();

        virtual public bool IsRemotePlayer() { return false; }

        virtual public LocalPlayer GetLocalPlayer()
        {
            return null;
        }
        virtual public RemotePlayer GetRemotePlayer()
        {
            return null;
        }
        virtual public AbsHumanPlayer GetHumanPlayer()
        {
            return null;
        }
        virtual public AiPlayer GetAiPlayer()
        {
            return null;
        }

        abstract public string Name { get; }

        virtual public void OnCityCapture(City city)
        {
            
        }
        public void setAggression(int agg)
        {
            if (aggressionLevel != agg)
            {
                aggressionLevel = agg;
                //GetAiPlayer()?.refreshAggression();
            }
        }
        public void setMinimumAggression(int minAgg)
        {
            if (aggressionLevel <= minAgg)
            {
                setAggression(minAgg);
            }
        }

        public override string ToString()
        {
            return "Player (" + Name + ")";
        }
    }

    
}
