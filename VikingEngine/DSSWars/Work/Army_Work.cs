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
            conservedFood *= 10;
        }

        public void setMaxFood()
        {
            //float energy = DssConst.ManDefaultEnergyCost / DssRef.difficulty.FoodEnergySett * DssConst.SoldierGroup_DefaultCount * Bound.Min(groups.Count, 1);
            //float minuteEnergy = TimeExt.MinuteInSeconds * energy;
            //float bufferGoalFood = friendlyAreaFoodBuffer_minutes * minuteEnergy;
            //float bufferGoalConservedFood = friendlyAreaConservedFoodBuffer_minutes * minuteEnergy;
            getFoodGoalBuffer(out float bufferGoalFood, out float bufferGoalConservedFood);
            //#if DEBUG
            //            if (Debug.CorruptValue(food))
            //            {
            //                lib.DoNothing();
            //            }
            //#endif
            food = bufferGoalFood;
            conservedFood = bufferGoalConservedFood;
        }

        void getFoodGoalBuffer(out float bufferGoalFood, out float bufferGoalConservedFood)
        {
            float energy = DssConst.ManDefaultEnergyCost / DssRef.difficulty.FoodEnergySett * DssConst.SoldierGroup_DefaultCount * Bound.Min(groups.Count, 1);
            float minuteEnergy = TimeExt.MinuteInSeconds * energy;
            bufferGoalFood = friendlyAreaFoodBuffer_minutes * minuteEnergy;
            bufferGoalConservedFood = friendlyAreaConservedFoodBuffer_minutes * minuteEnergy;
        }

        public void async_workUpdate(Faction faction, float seconds)
        {
            if ( factionIndex >= 0)
            {
                bool casual = GetCasual();

                if (seconds > 0)
                {
                    
                    goldUpkeepUpdate_async(seconds);

                    if (!casual)
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
            //float foodUpkeep = //ManUpkeepToFoodUpkeep(totalCopperUpkeep);

            if (foodBackOrderTimeSec > 0)
            {
                foodBackOrderTimeSec -= seconds;
            }
            else
            {
                //Order new food
                City city = DssRef.world.tileGrid.Get(tilePos).City();
                if (city != null && city.HasFaction())
                {
                    float bufferGoal_minutes = -1;
                    if (city.factionIndex == factionIndex)
                    {
                        bufferGoal_minutes = friendlyAreaFoodBuffer_minutes;
                    }
                    else if (!DssRef.diplomacy.GetRelation(city.factionIndex, factionIndex).InWar())
                    {
                        bufferGoal_minutes = foodBuffer_minutes;
                    }

                    //float bufferGoalFood = bufferGoal_minutes * TimeExt.MinuteInSeconds * totalUpkeep.food;
                    if (bufferGoal_minutes > 0)
                    {
                        getFoodGoalBuffer(out float bufferGoalFood, out float bufferGoalConservedFood);

                        if (Ref.peRnd.ChanceF(0.6f))
                        {
                            orderMissingFood(food, bufferGoalFood, city.resourceAmount(EntityComponent.CityResoureIndex.food), DssConst.FoodGoldValue, ItemResourceType.Food_G);
                        }
                        else
                        {
                            orderMissingFood(conservedFood, bufferGoalConservedFood, city.resourceAmount(EntityComponent.CityResoureIndex.ConservedFood), DssConst.ConservedFoodGoldValue, ItemResourceType.ConservedFood);
                        }

                        void orderMissingFood(float hasAmount, float goalAmount, int cityAmount, int goldValue, ItemResourceType foodType)
                        {
                            if (bufferGoal_minutes > 0 &&
                                (food + conservedFood) < bufferGoalFood &&
                                cityAmount >= ItemPropertyColl.CarryFood &&
                                faction.hasGold(goldValue * ItemPropertyColl.CarryFood, this))
                            {
                                int statusIx = getOrCreateFreeWorker();
                                var status = workerStatuses[statusIx];
                                status.createWorkOrder(WorkType.TrossCityTrade, (int)foodType, 0, XP.WorkExperienceType.NUM_NONE, -1, WP.ToSubTilePos_Centered(city.tilePos), null);
                                if (city.factionIndex != factionIndex)
                                {
                                    foodCosts_import.add(status.carry.amount);
                                }
                                workerStatuses[statusIx] = status;

                                //Calc backorder 
                                float perc = (ItemPropertyColl.ArmyFoodOrderSize + hasAmount) / goalAmount;

                                if (perc > 0)
                                {
                                    foodBackOrderTimeSec += status.processTimeLengthSec * perc * 0.8f;
                                }
                            } }
                    } }


            }

            float minBuffer = totalUpkeep.food * 2;

            if (food + conservedFood < minBuffer)
            {
                bool allowDept = false;

                if (GetPlayer().IsLocalPlayer())
                {
                    //goNegative = false;
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        GetPlayer().GetLocalPlayer().hud.messages.armyLowFoodMessage(this);
                    }));
                }
                else if (!DssRef.storage.gameRuleset.centralGold && money.copper > -soldiersCount * DssConst.FoodGoldValue_BlackMarket * 100)
                {
                    allowDept = true;
                }
                //black market trade

                //if (localPlayer || (!DssRef.storage.gameRuleset.centralGold && 

                var cost = (int)Math.Ceiling(DssConst.FoodGoldValue_BlackMarket * (minBuffer - food));
                
                if (payGold(cost, allowDept))
                {
                    foodCosts_blackmarket.add(cost);
                    food = minBuffer;
                }
            }

            int getOrCreateFreeWorker()
            {
                var worker = new WorkerStatus(true) { subTileEnd = WP.ToSubTilePos_Centered(tilePos) };
                for (int i = 0; i < workerStatuses.Count; i++)
                {
                    if (workerStatuses.array[i].work == WorkType.IsDeleted)
                    {
                        workerStatuses.array[i] = worker;
                        return i;
                    }
                }
                workerStatuses.Add(worker);
                return workerStatuses.Count - 1;
            }
        }

        protected override void onWorkComplete_async(ref WorkerStatus status)
        {
            status.WorkComplete(this, false);
        }
    }
}

