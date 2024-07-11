using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;
using VikingEngine.Timer;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    class WorkerUnit: AbsGameObject
    {
        public const float StandardBoundRadius = AbsSoldierData.StandardBoundRadius * 4f;

        WalkingAnimation walkingAnimation;
        WorkerStatus status;

        public Graphics.AbsVoxelObj model;

        WorkerUnitState state = WorkerUnitState.None;
        Vector3 goalPos;
        Vector3 walkDir;
        City city;
        float finalizeWorkTime;
        GameTimer workAnimation = new GameTimer(1f, true, true);
        bool isShip = false;
        int prevX, prevZ;

        public WorkerUnit(City city, WorkerStatus status, int statusIndex)
        {
            this.city = city;
            this.status = status;
            this.parentArrayIndex = statusIndex;
            model = city.faction.AutoLoadModelInstance(
                 LootFest.VoxelModelName.war_worker, AbsDetailUnitData.StandardModelScale * 0.9f, true);

            model.position = WP.SubtileToWorldPos(status.subTileStart);

            checkForGoal(true);

            updateGroudY(true);
        }

        public void update()
        {
            if (parentArrayIndex == 29)
            {
                lib.DoNothing();
            }

            switch (state)
            { 
                case WorkerUnitState.HasGoal:
                   
                    float speed = AbsDetailUnitData.StandardWalkingSpeed * Ref.DeltaGameTimeMs;
                    model.position += walkDir * speed;
                    updateGroudY(false);

                    if (Convert.ToInt32(model.position.X) != prevX || Convert.ToInt32(model.position.Z) != prevZ)
                    {
                        prevX = Convert.ToInt32(model.position.X);
                        prevZ = Convert.ToInt32(model.position.Z);
                        //Tile tile;
                        if (DssRef.world.tileGrid.TryGet(prevX, prevZ, out Tile tile))
                        {
                            isShip = tile.IsWater();
                            model.Frame = status.carry.amount > 0 ? 4 : 3;
                        }
                    }

                    if (!isShip)
                    {
                        walkingAnimation.update(speed, model);
                    }

                    if (VectorExt.PlaneXZDistance(ref model.position, ref goalPos) < 0.02f)
                    {
                        model.position.X = goalPos.X;
                        model.position.Z = goalPos.Z;
                        WP.Rotation1DToQuaterion(model, MathExt.TauOver8);
                        state = WorkerUnitState.FinalizeWork;
                    }

                    break;

                case WorkerUnitState.FinalizeWork:

                    switch (status.work)
                    {
                        case WorkType.GatherFoil:
                            if (workAnimation_soundframe())
                            {
                                SubTile subTile = DssRef.world.subTileGrid.Get(status.subTileEnd);

                                switch ((TerrainSubFoilType)subTile.subTerrain)
                                {
                                    case TerrainSubFoilType.TreeSoft:
                                    case TerrainSubFoilType.TreeHard:
                                        SoundLib.woodcut.Play(model.position);
                                        break;
                                    case TerrainSubFoilType.FarmCulture:
                                        SoundLib.scythe.Play(model.position);
                                        break;
                                    case TerrainSubFoilType.Stones:
                                        SoundLib.dig.Play(model.position);
                                        break;
                                }
                            }
                            break;
                        case WorkType.Mine:
                            if (workAnimation_soundframe())
                            {
                                SoundLib.pickaxe.Play(model.position);
                            }
                            break;
                        case WorkType.Plant:
                            if (workAnimation_soundframe())
                            {
                                SoundLib.genericWork.Play(model.position);
                            }
                            break;
                        case WorkType.Till:
                            if (workAnimation_soundframe())
                            {
                                SoundLib.dig.Play(model.position);
                            }
                            break;
                        case WorkType.Craft:
                            if (workAnimation_soundframe())
                            {
                                SubTile subTile = DssRef.world.subTileGrid.Get(status.subTileEnd);
                                var building = (TerrainBuildingType)subTile.subTerrain;

                                switch (building)
                                {
                                    case TerrainBuildingType.Work_Cook:
                                        SoundLib.genericWork.Play(model.position);
                                        break;
                                    case TerrainBuildingType.Work_Smith:
                                        SoundLib.anvil.Play(model.position);
                                        break;
                                }
                            }
                            break;
                        case WorkType.Building:
                            if (workAnimation_soundframe())
                            {
                                SoundLib.hammer.Play(model.position);
                            }
                            break;
                    }

                    finalizeWorkTime -= Ref.DeltaGameTimeSec;
                    if (finalizeWorkTime <= 0)
                    {
                        switch (status.work)
                        {
                            case WorkType.GatherFoil:
                                SubTile subTile = DssRef.world.subTileGrid.Get(status.subTileEnd);
                                switch ((TerrainSubFoilType)subTile.subTerrain)
                                {
                                    case TerrainSubFoilType.TreeSoft:
                                    case TerrainSubFoilType.TreeHard:
                                        SoundLib.tree_falling.Play(model.position);
                                        break;

                                    case TerrainSubFoilType.Stones:
                                        SoundLib.pickup.Play(model.position);
                                        break;
                                }
                                break;
                            case WorkType.Plant:
                            case WorkType.DropOff:
                                SoundLib.drop_item.Play(model.position);
                                break;
                            case WorkType.PickUpResource:
                            case WorkType.PickUpProduce:
                                SoundLib.pickup.Play(model.position);
                                break;
                        }

                        status.WorkComplete(city);
                        city.setWorkerStatus(parentArrayIndex, ref status);
                        state = WorkerUnitState.None;
                    }
                    break;

                case WorkerUnitState.None:
                    city.getWorkerStatus(parentArrayIndex, ref status);
                    checkForGoal(false);
                    break;
            }

            

        }

        bool workAnimation_soundframe()
        {
            if (workAnimation.timeOut())
            {
                model.Frame = model.Frame == 1 ? 2 : 1;
                return model.Frame == 2;
            }

            return false;
        }

        void checkForGoal(bool onInit)
        { 
            if (status.work != WorkType.Idle)
            {
                if (status.subTileEnd == status.subTileStart)
                {
                    finalizeWorkTime = status.finalizeWorkTime();
                    state = WorkerUnitState.FinalizeWork;
                }
                else 
                {
                    //lib.DoNothing();

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
                        walkingAnimation = WalkingAnimation.WorkerWalking;
                    }
                    state = WorkerUnitState.HasGoal;
                }

                

                
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

        public void Tooltip(RichBoxContent content)
        {
            const string WorkType = "Work: {0}";
            content.text(string.Format(WorkType, status.work));

            const string Carry = "Carry: {0} {1}";

            if (status.carry.amount > 0)
            { 
                content.text(string.Format(Carry, status.carry.amount, status.carry.type));
            }

            const string Energy = "Energy: {0}";
            content.text(string.Format(Energy, TextLib.OneDecimal(status.energy)));
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

        public override Vector3 WorldPos()
        {
            return model.position;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            return faction == city.faction;
        }

        public override string Name()
        {
            return "Worker (" + parentArrayIndex.ToString() + ")";
        }

        public override WorkerUnit GetWorker()
        {
            return this;
        }

        enum WorkerUnitState
        {
            None,
            HasGoal,
            FinalizeWork,
        }
    }
}
