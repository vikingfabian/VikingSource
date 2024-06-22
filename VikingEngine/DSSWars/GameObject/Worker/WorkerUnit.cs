using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    class WorkerUnit: AbsGameObject
    {
        WorkerStatus status;
        int statusIndex;
        public Graphics.AbsVoxelObj model;

        bool hasGoal = false;
        Vector3 goalPos;
        Vector3 walkDir;
        City city;
        public WorkerUnit(City city, WorkerStatus status, int statusIndex)
        {
            this.city = city;
            this.status = status;
            this.statusIndex = statusIndex;
            model = city.faction.AutoLoadModelInstance(
                 LootFest.VoxelModelName.war_worker, AbsDetailUnitData.StandardModelScale * 0.9f, true);

            model.position = WP.WorldPosFromSubtile(status.subTileStart);

            checkForGoal(true);

            updateGroudY(true);
        }

        public void update()
        {
            if (hasGoal)
            {
                float speed = AbsDetailUnitData.StandardWalkingSpeed * Ref.DeltaGameTimeMs;
                model.position += walkDir * speed;

                //WalkingAnimation.Standard.update(speed, model);
                updateGroudY(false);

                if (VectorExt.PlaneXZDistance(ref model.position, ref goalPos) < WorldData.SubTileWidth)
                {
                    status.WorkComplete();
                    city.setWorkerStatus(statusIndex, ref status);
                    hasGoal = false;
                }
            }
            else
            {
                city.getWorkerStatus(statusIndex, ref status);
                checkForGoal(false);                
            }
        }

        void checkForGoal(bool onInit)
        { 
            if (status.work != WorkType.Idle)
            {   
                goalPos = WP.WorldPosFromSubtile(status.subTileEnd);
                walkDir = VectorExt.SafeNormalizeV3(goalPos - model.position);
                WP.Rotation1DToQuaterion(model, lib.V2ToAngle(VectorExt.V3XZtoV2(walkDir)));

                if (onInit)
                {
                    float timePassed = Ref.TotalGameTimeSec - status.processTimeStartStampSec;
                    float perc = timePassed / status.processTimeLengthSec;
                    model.position = model.position * (1 - perc) + goalPos * perc;
                }

                hasGoal = true;
            }
        }

        const float ModelGroundYAdj = 0.02f;
        void updateGroudY(bool set)
        {
            if (DssRef.world.unitBounds.IntersectPoint(model.position.X, model.position.Z))//position.X > 0 && position.Z>0)
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

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.Worker;
        }

        public override Faction GetFaction()
        {
            return city.faction;
        }
    }
}
