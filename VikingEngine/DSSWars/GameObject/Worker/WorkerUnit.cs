using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    class WorkerUnit: AbsGameObject
    {
        WorkerStatus status;
        int statusIndex;
        public Graphics.AbsVoxelObj model;

        Vector3 goalPos;
        Vector3 walkDir;

        public WorkerUnit(City city, WorkerStatus status, int statusIndex)
        {
            this.status = status;
            this.statusIndex = statusIndex;
            model = city.faction.AutoLoadModelInstance(
                 LootFest.VoxelModelName.war_worker, AbsDetailUnitData.StandardModelScale * 0.9f, true);

            model.position = WP.WorldPosFromSubtile(status.subTileStart);

            if (status.work != WorkType.Idle)
            {
                float timePassed = Ref.TotalGameTimeSec - status.processTimeStartStampSec;
                float perc = timePassed /status.processTimeLengthSec;

                goalPos = WP.WorldPosFromSubtile(status.subTileEnd);
                walkDir = VectorExt.SafeNormalizeV3(goalPos - model.position);
                model.position = model.position * (1-perc) + goalPos * perc;
            }

            updateGroudY(true);
        }

        public void update()
        {
            if (status.work != WorkType.Idle)
            {
                model.position += walkDir * AbsDetailUnitData.StandardWalkingSpeed * Ref.DeltaGameTimeMs;

                updateGroudY(false);

                if (VectorExt.PlaneXZDistance(ref model.position, ref goalPos) < WorldData.SubTileWidth)
                {
                    status.WorkComplete();
                }
            }
            //
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
            throw new NotImplementedException();
        }
    }
}
