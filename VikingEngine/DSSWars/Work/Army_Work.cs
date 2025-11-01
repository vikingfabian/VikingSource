using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.PJ.Joust;


namespace VikingEngine.DSSWars.GameObject
{
    partial class Army
    {
        float foodBackOrderTimeSec = 0;

        public void setAsStartArmy()
        {
           
            refreshGroupPlacements2(tilePos, false, true, false);
            setMaxFood();
        }

        public void setMassiveFood()
        {
            setMaxFood();
            food *= 10;
        }

        public void setMaxFood()
        {
            float energy = DssConst.ManDefaultEnergyCost / DssRef.difficulty.FoodEnergySett * DssConst.SoldierGroup_DefaultCount * Bound.Min(groups.Count, 1);
            float bufferGoalFood = friendlyAreaFoodBuffer_minutes * TimeExt.MinuteInSeconds * energy;
            food = bufferGoalFood;
        }

        public void async_workUpdate(Faction faction, float seconds)
        {
            if ( factionIndex >= 0)
            {
                bool casual = GetCasual();

                if (seconds > 0)
                {
                    if (casual)
                    {
                        goldUpkeepUpdate_async(seconds);
                    }
                    else
                    {
                        foodUpkeepUpdate_async(faction, seconds);
                    }
                }

                if (!casual && !inRender_detailLayer)
                {
                    processAsynchWork(ref workerStatuses);
                }
            }        
        }

        void goldUpkeepUpdate_async(float seconds)
        {

        }

        public static float ManUpkeepToFoodUpkeep(float manUpkeep)
        {
            float energyUpkeep = manUpkeep * DssConst.ManDefaultEnergyCost;
            float foodUpkeep = energyUpkeep / DssRef.difficulty.FoodEnergySett;
            return foodUpkeep;
        }

        void foodUpkeepUpdate_async(Faction faction, float seconds)
        {

            if (debugTagged)
            {
                lib.DoNothing();
            }

            //float energyUpkeep = totalUpkeep * DssConst.ManDefaultEnergyCost;
            //float foodUpkeep = energyUpkeep * DssRef.difficulty.FoodEnergySett;
            float foodUpkeep = ManUpkeepToFoodUpkeep(totalUpkeep);

            if (foodBackOrderTimeSec > 0)
            {
                foodBackOrderTimeSec -= seconds;
            }
            else
            {
                //Order new food
                City city = DssRef.world.tileGrid.Get(tilePos).City();
                if (city != null)
                {
                    float bufferGoal_minutes = -1;
                    if (city.factionIndex == factionIndex)
                    {
                        bufferGoal_minutes = friendlyAreaFoodBuffer_minutes;
                    }
                    else if (!DssRef.diplomacy.InWar(city.factionIndex, factionIndex))
                    {
                        bufferGoal_minutes = foodBuffer_minutes;
                    }

                    float bufferGoalFood = bufferGoal_minutes * TimeExt.MinuteInSeconds * foodUpkeep;

                    if (bufferGoal_minutes > 0 && food < bufferGoalFood && 
                        city.res_food.amount >= ItemPropertyColl.CarryFood &&
                         faction.hasGold(city.SellCost(ItemResourceType.Food_G) * ItemPropertyColl.CarryFood, this))
                    {
                        int statusIx = getOrCreateFreeWorker();
                        var status = workerStatuses[statusIx];
                        status.createWorkOrder(WorkType.TrossCityTrade, -1, 0, XP.WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(city.tilePos), null);
                        if (city.factionIndex != factionIndex)
                        {
                            foodCosts_import.add(status.carry.amount);
                        }
                        workerStatuses[statusIx] = status;

                        //Calc backorder 
                        //float foodOrderSize = ItemPropertyColl.CarryFood * DssConst.Worker_TrossWorkerCarryWeight;
                        float perc = (ItemPropertyColl.ArmyFoodOrderSize + food) / bufferGoalFood;

                        if (perc > 0)
                        {
                            foodBackOrderTimeSec += status.processTimeLengthSec * perc * 0.8f;
                        }
                    }
                }


            }

            float minBuffer = foodUpkeep * 2;

            if (food < minBuffer)
            {
                if (GetPlayer().IsLocalPlayer())
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        GetPlayer().GetLocalPlayer().hud.messages.armyLowFoodMessage(this);
                    }));
                }

                //black market trade
                var cost = (int)Math.Ceiling(DssConst.FoodGoldValue_BlackMarket * (minBuffer - food));

                if (payGold(cost))
                {
                    foodCosts_blackmarket.add(cost);
                    food = minBuffer;
                }
            }

            int getOrCreateFreeWorker()
            {
                for (int i = 0; i < workerStatuses.Count; i++)
                {
                    if (workerStatuses[i].work == WorkType.IsDeleted)
                    {
                        return i;
                    }
                }
                workerStatuses.Add(new WorkerStatus() { subTileEnd = WP.ToSubTilePos_Centered(tilePos) });
                return workerStatuses.Count - 1;
            }
        }

        protected override void onWorkComplete_async(ref WorkerStatus status)
        {
            status.WorkComplete(this, false);
        }
    }
}

