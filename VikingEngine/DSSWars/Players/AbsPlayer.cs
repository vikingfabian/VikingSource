using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
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
        public Faction faction;
        public int aggressionLevel = AggressionLevel0_Passive;
        public bool protectedPlayer = false;
        protected bool ignorePlayerCapture = false;

        //public List<AbsOrder> orders = new List<AbsOrder>();
        public Orders.Orders orders;
        abstract public void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType buildType, out bool intelligent);

        public PlayerProfile profile;
        public Texture2D flagTexture;

        public AbsPlayer()
        { }

        public void SetProfile(PlayerProfile profile)
        {
            this.profile = profile;
            flagTexture = profile.flag.flagDesign.CreateTexture(profile.flag);
        }

        public AbsPlayer(Faction faction, bool newGame)
        {
            this.faction = faction;
            faction.SetStartOwner(this);

            if (newGame)
            {
                createStartupBarracks();
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
            faction.mainCity?.createStartupBarracks();
        }

        virtual public void Update()
        { }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {

        }

        protected void readAiPlayerGameState(BinaryReader r, int version)
        {
            IsPlayerNeighbor = r.ReadBoolean();
            aggressionLevel = r.ReadByte();
            protectedPlayer = r.ReadBoolean();
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

        virtual public void onNewRelation(Faction otherFaction, DiplomaticRelation rel, RelationType previousRelation)
        {
            //On peace, stop all attacking armies
            bool fromWar = Diplomacy.IsWar(rel.Relation);
            bool toWar = Diplomacy.IsWar(rel.Relation);

            if (fromWar != toWar)
            {
                if (toWar)
                {
                    faction.tradeAllianceWars(otherFaction);
                }
                else
                {
                    faction.stopAllAttacksAgainst(otherFaction);
                }
            }

            if (rel.Relation == RelationType.RelationType3_Ally &&
                !rel.secret)
            {
                faction.tradeAllianceWars(otherFaction);
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

                player.GetAiPlayer().refreshAggression();

                var relation = DssRef.diplomacy.GetOrCreateRelation(faction, player.faction);
                relation.SetWorseSpeakTerms(DssRef.diplomacy.SpeakTermsOnNeigbor_BadChance, DssRef.diplomacy.SpeakTermsOnNeigbor_NoneChance);

                if (faction.Size() >= FactionSize.Big)
                {
                    protectedPlayer = true;
                }
            }
        }

        virtual public void createStartUnits()
        {   
        }

        virtual public void onGameStart(bool newGame) { }

        abstract public bool IsLocal { get; }

        abstract public bool IsBot();

        abstract public bool IsLocalPlayer();

        virtual public LocalPlayer GetLocalPlayer()
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
                GetAiPlayer()?.refreshAggression();
            }
        }
        public void setMinimumAggression(int minAgg)
        {
            if (aggressionLevel <= minAgg)
            {
                setAggression(minAgg);
            }
        }
    }

    
}
