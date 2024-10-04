using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;


namespace VikingEngine.DSSWars.GameObject
{
    partial class Army
    {
        //public List<WorkerUnit> workerUnits = null;
        //List<WorkerStatus> workerStatuses = new List<WorkerStatus>();

        float foodBackOrderTimeSec = 0;

        public void setMaxFood()
        {
            float energy = DssLib.SoldierDefaultEnergyUpkeep * DssConst.SoldierGroup_DefaultCount * groups.Count;
            float bufferGoalFood = friendlyAreaFoodBuffer_minutes * TimeExt.MinuteInSeconds * energy;
            food = bufferGoalFood;
        }

        public void async_workUpdate(float seconds)
        {
            if (seconds > 0)
            {
                //float newFoodCosts = 0;

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
                        
                        if (food < 0)
                        {
                            //black market trade
                            var cost = (int)Math.Ceiling(DssConst.FoodGoldValue_BlackMarket * -food);

                            if (faction.payMoney(cost, false))
                            {
                                foodCosts_blackmarket.add(cost);
                                food = 0;                                
                            }
                        }

                        float bufferGoalFood = bufferGoal_minutes * TimeExt.MinuteInSeconds * foodUpkeep;

                        if (bufferGoal_minutes > 0 && food < bufferGoalFood)
                        {
                            int statusIx = getOrCreateFreeWorker();
                            var status = workerStatuses[statusIx];
                            status.createWorkOrder(WorkType.TrossCityTrade, -1, -1, WP.ToSubTilePos_Centered(city.tilePos), null);
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
                            //}
                            //else if (perc > 0.5)
                            //{
                            //    foodBackOrderTimeSec += status.processTimeLengthSec * 0.5f;
                            //}
                            //else if (perc > 0.2)
                            //{
                            //    foodBackOrderTimeSec += status.processTimeLengthSec * 0.2f;
                            //}
                            //else if (perc > 0.05)
                            //{
                            //    foodBackOrderTimeSec += status.processTimeLengthSec * 0.2f;
                            //}
                        }
                    }
                }

                //foodCosts = newFoodCosts;
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

