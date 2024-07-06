using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    class WorkerUnit: AbsGameObject
    {
        WalkingAnimation walkingAnimation;
        WorkerStatus status;
        int statusIndex;
        public Graphics.AbsVoxelObj model;

        //bool hasGoal = false;
        WorkerUnitState state = WorkerUnitState.None;
        Vector3 goalPos;
        Vector3 walkDir;
        City city;
        float finalizeWorkTime;
        GameTimer workAnimation = new GameTimer(0.5f, true, true);

        public WorkerUnit(City city, WorkerStatus status, int statusIndex)
        {
            this.city = city;
            this.status = status;
            this.statusIndex = statusIndex;
            model = city.faction.AutoLoadModelInstance(
                 LootFest.VoxelModelName.war_worker, AbsDetailUnitData.StandardModelScale * 0.9f, true);

            model.position = WP.SubtileToWorldPos(status.subTileStart);

            checkForGoal(true);

            updateGroudY(true);
        }

        public void update()
        {
            switch (state)
            { 
                case WorkerUnitState.HasGoal:
                    float speed = AbsDetailUnitData.StandardWalkingSpeed * Ref.DeltaGameTimeMs;
                    model.position += walkDir * speed;

                    walkingAnimation.update(speed, model);
                    updateGroudY(false);

                    if (VectorExt.PlaneXZDistance(ref model.position, ref goalPos) < 0.02f)
                    {
                        model.position.X = goalPos.X;
                        model.position.Z = goalPos.Z;
                        WP.Rotation1DToQuaterion(model, MathExt.TauOver8);
                        state = WorkerUnitState.FinalizeWork;
                    }
                    break;

                case WorkerUnitState.FinalizeWork:
                    if (status.work == WorkType.GatherFoil || status.work == WorkType.Mine)
                    {
                        if (workAnimation.timeOut())
                        { 
                            model.Frame = model.Frame == 1? 2 : 1;  
                        }
                    }

                    finalizeWorkTime -= Ref.DeltaGameTimeSec;
                    if (finalizeWorkTime <= 0)
                    {
                        status.WorkComplete(city);
                        city.setWorkerStatus(statusIndex, ref status);
                        //hasGoal = false;
                        state = WorkerUnitState.None;
                    }
                    break;
                    
                case WorkerUnitState.None:
                    city.getWorkerStatus(statusIndex, ref status);
                    checkForGoal(false);
                    break;
            }
            
        }

        void checkForGoal(bool onInit)
        { 
            if (status.work != WorkType.Idle)
            {   
                goalPos = WP.SubtileToWorldPos(status.subTileEnd);
                goalPos.X += WorldData.SubTileWidth * 0.25f;
                goalPos.Z += WorldData.SubTileWidth;

                walkDir = VectorExt.SafeNormalizeV3(goalPos - model.position);
                WP.Rotation1DToQuaterion(model, lib.V2ToAngle(VectorExt.V3XZtoV2(walkDir)));
                finalizeWorkTime = status.finalizeWorkTime();

                if (onInit)
                {
                    float timePassed = Ref.TotalGameTimeSec - status.processTimeStartStampSec;
                    float perc = timePassed / (status.processTimeLengthSec - finalizeWorkTime);
                    model.position = model.position * (1 - perc) + goalPos * perc;
                }

                if (status.work == WorkType.DropOff)
                {
                    walkingAnimation = WalkingAnimation.WorkerCarry;
                }
                else
                {
                    walkingAnimation = WalkingAnimation.Standard;
                }
                state = WorkerUnitState.HasGoal;
            }
        }

        const float ModelGroundYAdj = 0.02f;
        void updateGroudY(bool set)
        {
            if (DssRef.world.unitBounds.IntersectPoint(model.position.X, model.position.Z))
            {
                float y = DssRef.world.SubTileHeight(model.position) + ModelGroundYAdj;

                if (y < Map.Tile.UnitMinY)
                {
                    y = Map.Tile.UnitMinY;
                }

                if (y != model.position.Y)
                {
                    if (set)
                    {
                        model.position.Y = y;
                    }
                    else
                    {
                        float diff = y - model.position.Y;
                        if (Math.Abs(diff) < 0.01f)
                        {
                            model.position.Y = y;
                        }
                        else
                        {
                            model.position.Y += diff * 0.06f;
                        }
                    }
                }
            }
        }

        public void DeleteMe()
        { 
            model.DeleteMe();
        }

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.Worker;
        }

        public override Faction GetFaction()
        {
            return city.faction;
        }

        enum WorkerUnitState
        {
            None,
            HasGoal,
            FinalizeWork,
        }
    }
}
