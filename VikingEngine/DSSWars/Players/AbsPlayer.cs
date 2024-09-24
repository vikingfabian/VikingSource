using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players
{
    abstract class AbsPlayer
    {
        public const int AggressionLevel0_Passive = 0;
        protected const int AggressionLevel1_RevengeOnly = 1;
        protected const int AggressionLevel2_RandomAttacks = 2;
        protected const int AggressionLevel3_FocusedAttacks = 3;

        public bool IsPlayerNeighbor = false;
        public Faction faction;
        public int aggressionLevel = AggressionLevel0_Passive;
        public bool protectedPlayer = false;
        protected bool ignorePlayerCapture = false;

        public List<AbsOrder> orders = new List<AbsOrder>();
        abstract public void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType buildType, out bool intelligent);
        public AbsPlayer(Faction faction)
        {
            this.faction = faction;
            faction.SetStartOwner(this);

            faction.mainCity?.createStartupBarracks();
        }

        public bool orderConflictingSubTile(IntVector2 subTilePos)
        {
            for (int i = 0; i < orders.Count; ++i)
            {
                if (orders[i].IsBuildOnSubTile(subTilePos))//order.IsConflictingOrder(orders[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public void addOrder(AbsOrder order)
        {
            lock (orders)
            {
                //Check for conflicting
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (order.IsConflictingOrder(orders[i]))
                    {
                        orders[i].DeleteMe();
                        orders.RemoveAt(i);
                    }
                }
                orders.Add(order);
            }
        }

        public AbsOrder GetFromId(int id)
        {
            lock (orders)
            {
                foreach (AbsOrder order in orders)
                { 
                    if (order.id == id)
                        return order;
                }
            }
            return null;
        }

        public AbsOrder StartOrderId(int id)
        {
            lock (orders)
            {
                foreach (AbsOrder order in orders)
                {
                    if (order.id == id)
                    { 
                        order.orderStatus = OrderStatus.Started;
                        break;
                    }
                }
            }
            return null;
        }

        public void CompleteOrderId(int id)
        {
            lock (orders)
            {
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (orders[i].id == id)
                    {
                        Ref.update.AddSyncAction(new SyncAction(orders[i].DeleteMe));
                        orders.RemoveAt(i);
                    }
                }
            }
        }

        virtual public void Update()
        { }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {

        }
        virtual public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
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
            if (player.IsAi())
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
