using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;


namespace VikingEngine.DSSWars.GameObject
{
    partial class Army
    {
        float foodBackOrderTimeSec = 0;

        public void setMaxFood()
        {
            float energy = DssLib.SoldierDefaultEnergyUpkeep / DssRef.difficulty.FoodEnergySett * DssConst.SoldierGroup_DefaultCount * Bound.Min(groups.Count, 1);
            float bufferGoalFood = friendlyAreaFoodBuffer_minutes * TimeExt.MinuteInSeconds * energy;
            food = bufferGoalFood;
        }

        public void async_workUpdate(float seconds)
        {
            if (seconds > 0)
            {
                if (debugTagged)
                {
                    lib.DoNothing();
                }

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
                        if (city.faction == faction)
                        {
                            bufferGoal_minutes = friendlyAreaFoodBuffer_minutes;
                        }
                        else if (!DssRef.diplomacy.InWar(city.faction, faction))
                        {
                            bufferGoal_minutes = foodBuffer_minutes;
                        }

                        float bufferGoalFood = bufferGoal_minutes * TimeExt.MinuteInSeconds * foodUpkeep;

                        if (bufferGoal_minutes > 0 && food < bufferGoalFood && city.res_food.amount >= ItemPropertyColl.CarryFood)
                        {
                            int statusIx = getOrCreateFreeWorker();
                            var status = workerStatuses[statusIx];
                            status.createWorkOrder(WorkType.TrossCityTrade, -1, 0, WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(city.tilePos), null);
                            if (city.faction != faction)
                            {
                                foodCosts_import.add(status.carry.amount);
                            }
                            workerStatuses[statusIx] = status;

                            //Calc backorder 
                            float foodOrderSize = ItemPropertyColl.CarryFood * DssConst.Worker_TrossWorkerCarryWeight;
                            float perc = (foodOrderSize + food) / bufferGoalFood;

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
                    if (faction.player.IsPlayer())
                    {
                        Ref.update.AddSyncAction(new SyncAction(() =>
                        {
                            faction.player.GetLocalPlayer().hud.messages.armyLowFoodMessage(this);
                        }));
                    }

                    //black market trade
                    var cost = (int)Math.Ceiling(DssConst.FoodGoldValue_BlackMarket * (minBuffer - food));

                    if (faction.payMoney(cost, false))
                    {
                        foodCosts_blackmarket.add(cost);
                        food = minBuffer;
                    }
                }
            }

            if (!inRender_detailLayer)
            {
                processAsynchWork(workerStatuses);
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
                workerStatuses.Add(new WorkerStatus() { subTileEnd = WP.ToSubTilePos_Centered(tilePos)});
                return workerStatuses.Count -1;
            }
        }

        protected override void onWorkComplete_async(ref WorkerStatus status)
        {
            status.WorkComplete(this, false);
        }
    }
}

