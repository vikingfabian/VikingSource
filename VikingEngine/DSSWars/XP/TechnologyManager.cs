using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.XP
{
    class TechnologyManager
    {
        /// <remark>
        /// Players must be fully initialized
        /// </remark>
        public void initGame(bool newGame)
        {
            if (newGame)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var factionsCounter = DssRef.world.factions.counter();
                        while (factionsCounter.Next())
                        {
                            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                            while (citiesC.Next(ref factionsCounter.sel.cities, DssRef.world.cities, out City citySel))
                            {
                                citySel.technology.addFactionUnlocked(factionsCounter.sel.technology, true, true);
                            }
                        }

                        asyncOneMinuteUpdate(false);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                    
                });
            }
        }

        public void asyncOneMinuteUpdate(bool runSpread)
        {
            //Collect tech spread

            if (runSpread)
            {
                foreach (var city in DssRef.world.cities)
                {
                    if (city.IsNetHosted)
                    {
                        if (city.debugTagged || city.myIndex == 192)
                        {
                            lib.DoNothing();
                        }
                        EcsStaticArrayCounter neighbors = city.CityNeighbors();
                        while (neighbors.Next(DssRef.world.cities, out City nCity))//foreach (var ni in city.neighborCities)
                        {
                            
                            //var nCity = DssRef.world.cities[ni];
                            if (city.factionIndex == nCity.factionIndex)
                            {
                                TechnologyTemplate.GainTechSpread(city, nCity.technology, DssConst.TechnologyGain_CitySpread, TechnologyGainReason.CityToCitySpread);
                                //city.technology.gainTechSpread(nCity.technology, DssConst.TechnologyGain_CitySpread);
                            }
                            else
                            {
                                switch (DssRef.world.diplomacy.GetRelation(city.GetFaction(), nCity.GetFaction()).Relation)
                                {
                                    case RelationType.RelationType2_Good:
                                        TechnologyTemplate.GainTechSpread(city, nCity.technology, DssConst.TechnologyGain_GoodRelation_PerMin, TechnologyGainReason.FactionToFactionSpread);

                                        //city.technology.gainTechSpread(nCity.technology, DssConst.TechnologyGain_GoodRelation_PerMin);
                                        break;
                                    case RelationType.RelationType3_Ally:
                                        TechnologyTemplate.GainTechSpread(city, nCity.technology, DssConst.TechnologyGain_AllyRelation_PerMin, TechnologyGainReason.FactionToFactionSpread);

                                        //city.technology.gainTechSpread(nCity.technology, DssConst.TechnologyGain_AllyRelation_PerMin);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //Faction tech overview
            //Apply unlock to work
            var factionsC = DssRef.world.factions.counter();
            while (factionsC.Next())
            {
                if (factionsC.sel.IsNetHosted())
                {
                    TechnologyTemplate factionTech = new TechnologyTemplate();
                    factionTech.zero();
                    factionTech.addFactionUnlocked(factionsC.sel.technology, false, false);

#if DEBUG
                    if (StartupSettings.UnlockAllProgress && factionsC.sel.player.IsLocalPlayer())
                    {
                        SpottedPointerArrayCounter unlockCities = new SpottedPointerArrayCounter();
                        while (unlockCities.Next(ref factionsC.sel.cities, DssRef.world.cities, out City citySel))
                        {
                            citySel.technology.unlockAll_debug();
                        }
                    }
#endif

                    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                    while (citiesC.Next(ref factionsC.sel.cities, DssRef.world.cities, out City citySel))
                    {
                        var unlocks = citySel.technology.GetUnlocks(false);
                        citySel.workTemplate.applyUnlock(unlocks);
                        factionTech.countUnlocks(citySel.technology);
                        if (unlocks.allUnlocked && factionsC.sel.player.IsLocalPlayer())
                        {
                            DssRef.achieve.UnlockAchievement_async(AchievementIndex.techtree);
                        }
                    }

                    factionsC.sel.technology = factionTech;
                    factionsC.sel.workTemplate.applyUnlock(factionTech.GetUnlocks(true));
                }
            }
        }
    }
}
