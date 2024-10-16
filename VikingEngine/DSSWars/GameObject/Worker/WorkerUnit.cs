using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    class WorkerUnit : AbsGameObject
    {
        WalkingAnimation walkingAnimation;
        protected WorkerStatus status;

        public Graphics.AbsVoxelObj model;
        Graphics.Mesh resourceModel;

        WorkerUnitState state = WorkerUnitState.None;
        Vector3 goalPos;
        Vector3 walkDir;
        AbsMapObject parentMapObject;
        float finalizeWorkTime;
        GameTimer workAnimation = new GameTimer(1f, true, true);
        bool isShip = false;
        int prevX, prevZ;
        float walkDist_beforeRefresh = 0f;

        public WorkerUnit(AbsMapObject mapObject, WorkerStatus status, int statusIndex)
        {
            this.parentMapObject = mapObject;
            this.status = status;
            this.parentArrayIndex = statusIndex;
            model = mapObject.GetFaction().AutoLoadModelInstance(
                 LootFest.VoxelModelName.war_worker, DssConst.Men_StandardModelScale * 0.9f, true);

            model.position = WP.SubtileToWorldPosXZ(status.subTileStart);

            checkForGoal(true, mapObject.GetCity());

            updateGroudY(true);
        }

        public void update(City city)
        {
            if (parentArrayIndex == 6)
            {
                lib.DoNothing();
            }

            switch (state)
            {
                case WorkerUnitState.HasGoal:

                    float speed = DssConst.Men_StandardWalkingSpeed * Ref.DeltaGameTimeMs;
                    walkDist_beforeRefresh += speed;

                    if (walkDist_beforeRefresh > 0.5f)
                    {
                        refreshGoalDir();
                    }

                    if (VectorExt.PlaneXZDistance(ref model.position, ref goalPos) < speed * 4)
                    {
                        model.position.X = goalPos.X;
                        model.position.Z = goalPos.Z;
                        WP.Rotation1DToQuaterion(model, 2.8f);
                        state = WorkerUnitState.FinalizeWork;
                        model.Frame = 0;
                        updateGroudY(true);
                    }
                    else
                    {
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
                                if (isShip)
                                {
                                    model.Frame = status.carry.amount > 0 ? 4 : 3;
                                }
                            }
                        }

                        if (!isShip)
                        {
                            walkingAnimation.update(speed, model);
                        }

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
                                    case TerrainSubFoilType.DryWood:
                                    case TerrainSubFoilType.TreeSoft:
                                    case TerrainSubFoilType.TreeHard:
                                        SoundLib.woodcut.Play(model.position);
                                        break;
                                    case TerrainSubFoilType.WheatFarm:
                                        SoundLib.scythe.Play(model.position);
                                        break;
                                    case TerrainSubFoilType.StoneBlock:
                                        SoundLib.pickaxe.Play(model.position);
                                        break;
                                    case TerrainSubFoilType.BogIron:
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
                                    case TerrainBuildingType.Brewery:
                                    case TerrainBuildingType.Work_Bench:
                                    case TerrainBuildingType.Work_Cook:
                                        SoundLib.genericWork.Play(model.position);
                                        break;
                                    case TerrainBuildingType.Work_Smith:
                                        SoundLib.anvil.Play(model.position);
                                        break;
                                }
                            }

                            if (resourceModel != null)
                            {
                                resourceModel.Rotation.RotateAxis(new Vector3(0, 0, Ref.DeltaTimeMs * 0.0014f));
                            }
                            else
                            { 
                                refreshCarryModel();
                                //updateGroudY(false);
                            }
                            break;
                        case WorkType.Build:
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
                                    case TerrainSubFoilType.DryWood:
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
                                SoundLib.drop_item.Play(model.position);
                                break;
                            case WorkType.DropOff:
                                SoundLib.drop_item.Play(model.position);

                                
                                break;
                            case WorkType.LocalTrade:
                                SoundLib.buy.Play(model.position);
                                break;
                            case WorkType.PickUpResource:
                            case WorkType.PickUpProduce:
                                SoundLib.pickup.Play(model.position);
                                break;
                            case WorkType.Starving:
                            case WorkType.Exit:
                                DeleteMe();
                                break;
                        }

                        status.WorkComplete(parentMapObject, true);
                        parentMapObject.setWorkerStatus(parentArrayIndex, ref status);
                        state = WorkerUnitState.None;
                        refreshCarryModel();
                    }
                    break;

                case WorkerUnitState.None:
                    parentMapObject.getWorkerStatus(parentArrayIndex, ref status);
                    checkForGoal(false, city);
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

        protected void checkForGoal(bool onInit, City city)
        {
            if (status.work > WorkType.Idle)
            {
                if (!model.Visible)
                {
                    //remove hidden status
                    model.Visible = true;
                    model.position = WP.SubtileToWorldPosXZ(status.subTileStart);
                }

                if (status.subTileEnd == status.subTileStart)
                {
                    finalizeWorkTime = status.finalizeWorkTime(city);
                    state = WorkerUnitState.FinalizeWork;
                    //refreshCarryModel();
                }
                else
                {
                    //lib.DoNothing();

                    refreshGoalDir();

                    finalizeWorkTime = status.finalizeWorkTime(city);

                    if (onInit)
                    {
                        float timePassed = Ref.TotalGameTimeSec - status.processTimeStartStampSec;
                        float perc = Bound.Set(timePassed / (status.processTimeLengthSec - finalizeWorkTime), 0, 1);
                        model.position = model.position * (1 - perc) + goalPos * perc;
                    }

                    switch (status.work)
                    {
                        case WorkType.TrossReturnToArmy:
                        case WorkType.DropOff:
                            walkingAnimation = WalkingAnimation.WorkerCarry;
                            break;
                        case WorkType.TrossCityTrade:
                        case WorkType.LocalTrade:
                            walkingAnimation = WalkingAnimation.WorkerTrading;
                            break;
                        default:
                            walkingAnimation = WalkingAnimation.WorkerWalking;
                            break;
                    }
                    state = WorkerUnitState.HasGoal;
                }

                refreshCarryModel();
            }
            else if (status.work == WorkType.IsDeleted)
            {
                model.position = Vector3.Zero;
                model.Visible = false;
            }
        }

        void refreshGoalDir()
        {
            walkDist_beforeRefresh = 0;
            goalPos = WP.SubtileToWorldPosXZ(status.subTileEnd);
            goalPos.X += WorldData.SubTileWidth * 0.25f;
            goalPos.Z += WorldData.SubTileWidth * 0.5f;

            walkDir = VectorExt.SafeNormalizeV3(goalPos - model.position);
            WP.Rotation1DToQuaterion(model, lib.V2ToAngle(VectorExt.V3XZtoV2(walkDir)));
        }

        void refreshCarryModel()
        {
            SpriteName sprite = SpriteName.NO_IMAGE;
            bool hasImage;
            if (status.carry.amount > 0)
            {
                hasImage = true;
                sprite = Resource.ResourceLib.Icon(status.carry.type);
            }
            else if (status.work == WorkType.Craft && state == WorkerUnitState.FinalizeWork)
            {
                hasImage = true;
                ItemResourceType item = (ItemResourceType)status.workSubType;
                sprite = ResourceLib.Icon(item);
            }
            else
            { 
                hasImage = false;
            }

            if (hasImage)
            {
                if (resourceModel == null)
                {
                    resourceModel = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero,
                        new Vector3(DssConst.Men_StandardModelScale * 0.6f), Graphics.TextureEffectType.Flat, SpriteName.NO_IMAGE, Color.White, false);
                    resourceModel.AddToRender(DrawGame.UnitDetailLayer);
                    resourceModel.Rotation = DssLib.FaceCameraRotation;
                }
                updateResourceModel();
                resourceModel.SetSpriteName(sprite);
            }
            else
            {
                resourceModel?.DeleteMe();
                resourceModel = null;
            }
        }

        const float ModelGroundYAdj = 0.02f;
        protected void updateGroudY(bool set)
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

            if (resourceModel != null)
            {
                updateResourceModel();
            }
        }

        void updateResourceModel()
        {
            resourceModel.position = model.Rotation.TranslateAlongAxis(
                DssVar.WorkerUnit_ResourcePosDiff, model.position);
        }

        
        public override void toHud(ObjectHudArgs args)
        {
            
            args.content.h2(Name()).overrideColor = Color.LightYellow;
            args.content.text(string.Format(DssRef.lang.WorkerHud_WorkType, status.workString()));

            if (status.carry.amount > 0)
            {
                args.content.text(string.Format(DssRef.lang.WorkerHud_Carry, status.carry.amount, LangLib.Item( status.carry.type)));
            }

            args.content.text(string.Format(DssRef.lang.WorkerHud_Energy, TextLib.OneDecimal(status.energy)));
        }
        
        public override void selectionFrame(bool hover, Selection selection)
        {
            Vector3 scale = new Vector3(DssVar.StandardBoundRadius * 2f);
            selection.BeginGroupModel(true);
            selection.setGroupModel(0, model.position, scale, hover, true, false);

        }

        public void DeleteMe()
        {
            model.DeleteMe();
            resourceModel?.DeleteMe();
        }

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.Worker;
        }

        public override Faction GetFaction()
        {
            return parentMapObject.GetFaction();
        }

        public override Vector3 WorldPos()
        {
            return model.position;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            return faction == parentMapObject.GetFaction();
        }

        public override WorkerUnit GetWorker()
        {
            return this;
        }

        public override string Name()
        {
            return parentMapObject.TypeName() + " " +  DssRef.lang.UnitType_Worker +" (" + parentArrayIndex.ToString() + ")";
        }

        enum WorkerUnitState
        {
            None,
            HasGoal,
            FinalizeWork,
        }
    }
}
