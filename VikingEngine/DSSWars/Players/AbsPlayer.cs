using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players
{
    abstract class AbsPlayer
    {
        public const int AggressionLevel0_Passive = 0;
        protected const int AggressionLevel1_RevengeOnly = 1;
        protected const int AggressionLevel2_RandomAttacks = 2;
        protected const int AggressionLevel3_FocusedAttacks = 3;

        public List<AbsPlayer> opponents = new List<AbsPlayer>(2);
        public bool IsPlayerNeighbor = false;
        public Faction faction;
        public int aggressionLevel = AggressionLevel0_Passive;
        public bool protectedPlayer = false;
        protected bool ignorePlayerCapture = false;

        public AbsPlayer(Faction faction)
        {
            this.faction = faction;
            faction.SetStartOwner(this);
        }

        virtual public void Update()
        { }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {

        }
        virtual public void readGameState(System.IO.BinaryReader r, int version)
        {

        }

        virtual public void writeNet(System.IO.BinaryWriter w)
        {

        }
        virtual public void readNet(System.IO.BinaryReader r)
        {

        }

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
            if (ignorePlayerCapture)
                return;

            ignorePlayerCapture = true;

            if (aggressionLevel == AggressionLevel0_Passive)
            {
                if (DssRef.difficulty.aiAggressivity == AiAggressivity.Medium)
                {
                    if (Ref.rnd.Chance(0.35))
                    {
                        aggressionLevel = AggressionLevel2_RandomAttacks;
                    }
                    else if (Ref.rnd.Chance(0.06))
                    {
                        aggressionLevel = AggressionLevel3_FocusedAttacks;
                    }
                }
                else if (DssRef.difficulty.aiAggressivity == AiAggressivity.High)
                {
                    if (Ref.rnd.Chance(0.6))
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
                    if (Ref.rnd.Chance(0.05))
                    {
                        aggressionLevel = AggressionLevel3_FocusedAttacks;
                    }
                }
                else if (DssRef.difficulty.aiAggressivity == AiAggressivity.High)
                {
                    if (Ref.rnd.Chance(0.7))
                    {
                        aggressionLevel = AggressionLevel3_FocusedAttacks;
                    }
                }
            }

            var relation = DssRef.diplomacy.GetOrCreateRelation(faction, player.faction);
            relation.SetWorseSpeakTerms(DssRef.diplomacy.SpeakTermsOnNeigbor_BadChance, DssRef.diplomacy.SpeakTermsOnNeigbor_NoneChance);

            if (IsAi() && faction.Size() >= FactionSize.Big)
            {
                protectedPlayer = true;
            }
        }

        virtual public void createStartUnits()
        {   
        }

        virtual public void onGameStart(bool newGame) { }

        abstract public bool IsLocal { get; }

        abstract public bool IsAi();

        abstract public bool IsPlayer();

        virtual public LocalPlayer GetLocalPlayer()
        {
            return null;
        }
        virtual public AiPlayer GetAiPlayer()
        {
            return null;
        }

        abstract public string Name { get; }

        virtual public void OnCityCapture(City city)
        {}
    }

    
}
