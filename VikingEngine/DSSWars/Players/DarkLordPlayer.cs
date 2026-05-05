using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players
{
    class DarkLordPlayer : AiPlayer
    {
        List<Faction> servantFactions;
        int maxDiplomacy;
        
        bool hasEntered = false;

        public int factoriesLeft;

        public AbsSoldierUnit darkLordUnit = null;

        int servantCountdown = 20;


        public DarkLordPlayer(Faction faction, bool newGame)
            : base(faction, newGame)
        {
            DssRef.settings.darkLordPlayer = this;

            if (DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory)
            {
                switch (DssRef.difficulty.bossSize)
                {
                    case BossSize.Small:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 60;
                        break;
                    case BossSize.Medium:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 120;
                        break;
                    case BossSize.Large:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 200;
                        break;
                    case BossSize.Huge:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 300;
                        break;
                }
            }
            else
            {
                switch (DssRef.difficulty.bossSize)
                {
                    case BossSize.Small:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 30;
                        break;
                    case BossSize.Medium:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 60;
                        break;
                    case BossSize.Large:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 100;
                        break;
                    case BossSize.Huge:
                        maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 140;
                        break;
                }
                
            }
        }

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);

            w.Write(arraylib.SafeCount(servantFactions));
            if (servantFactions != null)
            {
                foreach (var ally in servantFactions)
                {
                    w.Write((ushort)ally.myIndex);
                }
            }

            w.Write(maxDiplomacy);
            w.Write(diplomacyPoints);
            w.Write(hasEntered);
            w.Write(factoriesLeft);

            Debug.WriteCheck(w);
        }

        public override void readGameState(BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            base.readGameState(r, version, pointers);

            int darkLordAlliesCount = r.ReadInt32();
            if (darkLordAlliesCount > 0)
            {
                servantFactions = new List<Faction>(darkLordAlliesCount);
                for (int i = 0; i < darkLordAlliesCount; i++)
                {
                    var f = DssRef.world.factions.GetIndex_Safe(r.ReadUInt16());
                    servantFactions.Add(f);
                }
            }

            maxDiplomacy = r.ReadInt32();
            diplomacyPoints = r.ReadInt32();
            hasEntered = r.ReadBoolean();
            factoriesLeft = r.ReadInt32();

            Debug.ReadCheck(r);
        }

        public void EnterMap(/*Faction takeOverFaction, */List<Faction> servantFactions, List<Faction> darkLordAllies)
        {
            
            faction.money.copper = DssConst.HeadCityStartMaxWorkForce * 1000000;

            //this.servantFactions = darkLordAllies;
            Faction greenwood = DssRef.world.faction(DssRef.settings.Faction_GreenWood);
           
            foreach (var ally in darkLordAllies)
            {
                DssRef.world.diplomacy.SetRelationType(faction, ally, RelationType.RelationType3_Ally);//.secret = false;

                foreach (var p in DssRef.state.localPlayers)
                {
                    DssRef.world.diplomacy.SetRelationType(p.faction, ally, RelationType.RelationTypeN4_TotalWar);
                }

                if (greenwood != null)
                {
                    DssRef.world.diplomacy.SetRelationType(greenwood, ally, RelationType.RelationTypeN4_TotalWar);
                }                
            }

            //darkLordAllies.Remove(faction);
            //darkLordAllies.Remove(takeOverFaction);

            //makeServant(takeOverFaction, true);
            
            //TEMP
            //maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 64;

            diplomacyPoints = maxDiplomacy;
            //foreach (var f in darkLordAllies)
            while (servantFactions.Count > 0 &&
                diplomacyPoints > 0)
            {
                var f = arraylib.RandomListMemberPop(servantFactions);
                makeServant(f, false);
            }
            this.servantFactions = servantFactions;

            //diplomacyPoints /= 4;

            hasEntered = true;
            protectedFromDelete = false;
        }

        public override void aiPlayerAsynchUpdate(float time)
        {
            base.aiPlayerAsynchUpdate(time);


            if (hasEntered)
            {
                var city = faction.cities.GetRandom(Ref.rnd, DssRef.world.cities);
                if (city != null)
                {
                    EcsStaticArrayCounter neighbors = city.CityNeighbors();
                    while (neighbors.Next(DssRef.world.cities, out City nCity))
                    {
                        var nFaction = nCity.GetFaction();
                        if (nFaction != null &&
                            nFaction != faction &&
                            nFaction.diplomaticSide != DiplomaticSide.Light &&
                            !DssRef.world.diplomacy.PositiveRelationWithPlayer(nFaction))
                        {
                            if (servantFactions == null)
                            {
                                servantFactions = new List<Faction>(8);
                            }

                            lock (servantFactions)
                            {
                                if (!servantFactions.Contains(nFaction))
                                {
                                    servantFactions.Add(nFaction);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();

            if (hasEntered)
            {
                --servantCountdown;

                if (maxDiplomacy > 0 && servantCountdown <= 0)
                {
                    diplomacyPoints += 5;

                    if (diplomacyPoints >= 0)
                    {
                        Faction ally = null;
                        lock (servantFactions)
                        {
                            ally = arraylib.RandomListMemberPop(servantFactions);
                        }

                        if (ally != null && ally.cities.Count > 0)
                        {
                            makeServant(ally, true);
                            servantCountdown = Ref.rnd.Int(5, 40);
                        }
                    }
                }

                if (faction.cities.Count == 0)
                {
                    DssRef.state.events.onAllDarkCitiesDestroyed();
                }
            }
        }

        void makeServant(Faction takeOverFaction, bool factory)
        {
            int cost = takeOverFaction.totalWorkForce;// .citiesEconomy.workerCount;

            diplomacyPoints -= cost;
            maxDiplomacy -= cost;

            takeOverFaction.mergeTo(faction);

            //if (factory && factoriesLeft > 0 && takeOverFaction.mainCity != null)
            //{
            //    --factoriesLeft;
            //    takeOverFaction.mainCity.setFactoryType(true);
            //}
        }

        //protected override bool buySoldiers(City city, bool aggresive, bool commit)
        //{
        //    bool result = base.buySoldiers(city, aggresive, commit);

        //    //if (commit && DssRef.state.events.CurrentEvent()?.StoryEventType() == EventType.DarkLordInPerson)
        //    //{
        //    //    city.conscriptArmy(DssLib.SoldierProfile_HonorGuard.conscript, city.defaultConscriptPos(), 4);

        //    //    ConscriptProfile profile = new ConscriptProfile();
        //    //    profile.specialization = SpecializationType.DarkLord;
        //    //    city.conscriptArmy(profile, city.defaultConscriptPos(), 1);

        //    //    DssRef.state.events.addStoryEvent(new List<AbsStoryEvent>
        //    //        {
        //    //            new StoryEvent_KillTheDarkLord()
        //    //        }, true);
        //    //    //DssRef.state.events.nextEvent = EventType.KillTheDarkLord;
        //    //}

        //    return result;
        //}
    }

}
