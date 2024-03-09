﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players
{
    class DarkLordPlayer : AiPlayer
    {
        List<Faction> darkLordAllies;
        int maxDiplomacy;
        int diplomacyPoints = 0;
        bool hasEntered = false;

        public int factoriesLeft;

        public DarkLord darkLordUnit = null;

        public DarkLordPlayer(Faction faction)
            : base(faction)
        {
            switch (DssRef.storage.bossSize)
            {
                case BossSize.Small:
                    factoriesLeft = 2;
                    maxDiplomacy = DssLib.HeadCityMaxWorkForce * 24;
                    break;
                case BossSize.Medium:
                    factoriesLeft = 3;
                    maxDiplomacy = DssLib.HeadCityMaxWorkForce * 30;
                    break;
                case BossSize.Large:
                    factoriesLeft = 3;
                    maxDiplomacy = DssLib.HeadCityMaxWorkForce * 36;
                    break;
                case BossSize.Huge:
                    factoriesLeft = 4;
                    maxDiplomacy = DssLib.HeadCityMaxWorkForce * 42;
                    break;
            }
        }

        public void EnterMap(Faction takeOverFaction, List<Faction> darkLordAllies)
        {
            faction.gold = DssLib.HeadCityMaxWorkForce * 10;

            this.darkLordAllies = darkLordAllies;

            Faction greenwood = DssRef.world.factions.Array[DssRef.Faction_GreenWood];
           

            foreach (var ally in darkLordAllies)
            {
                DssRef.diplomacy.SetRelationType(faction, ally, RelationType.RelationType3_Ally);
                foreach (var p in DssRef.state.localPlayers)
                {
                    DssRef.diplomacy.SetRelationType(p.faction, ally, RelationType.RelationTypeN4_TotalWar).SpeakTerms = SpeakTerms.SpeakTermsN2_None;
                }

                if (greenwood != null)
                {
                    DssRef.diplomacy.SetRelationType(greenwood, ally, RelationType.RelationTypeN4_TotalWar);
                }
            }

            darkLordAllies.Remove(faction);
            darkLordAllies.Remove(takeOverFaction);

            makeServant(takeOverFaction, true);

            foreach (var f in darkLordAllies)
            {
                if (f.factiontype == FactionType.DefaultAi)
                {
                    makeServant(f, false);
                }
            }

            diplomacyPoints /= 4;

            hasEntered = true;
        }

        public override void Update()
        {
            base.Update();

            if (hasEntered)
            { 
                
            }
        }

        public override void aiPlayerAsynchUpdate(float time)
        {
            base.aiPlayerAsynchUpdate(time);


            if (hasEntered)
            {
                var city = faction.cities.GetRandomUnsafe(Ref.rnd);
                if (city != null)
                {
                    foreach (var n in city.neighborCities)
                    {
                        var nFaction = DssRef.world.cities[n].faction;
                        if (nFaction != faction &&
                            !DssRef.diplomacy.PositiveRelationWithPlayer(nFaction))
                        {
                            lock (darkLordAllies)
                            {
                                if (!darkLordAllies.Contains(nFaction))
                                {
                                    darkLordAllies.Add(nFaction);
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
                if (maxDiplomacy > 0)
                {
                    diplomacyPoints += DssLib.HeadCityMaxWorkForce / 20;

                    if (diplomacyPoints >= 0)
                    {
                        Faction ally = null;
                        lock (darkLordAllies)
                        {
                            ally = arraylib.RandomListMemberPop(darkLordAllies);
                        }

                        if (ally != null && ally.cities.Count > 0)
                        {
                            makeServant(ally, true);
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
            int cost = takeOverFaction.cityIncome;
            diplomacyPoints -= cost;
            maxDiplomacy -= cost;

            takeOverFaction.mergeTo(faction);

            if (factory && factoriesLeft > 0 && takeOverFaction.mainCity != null)
            {
                --factoriesLeft;
                takeOverFaction.mainCity.setFactoryType(true);
            }
        }
    }

}
