using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.Sound;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.Work
{
    class WorkerUnit : AbsGameObject
    {
        WalkingAnimation walkingAnimation;
        //protected WorkerStatus status;

        public Graphics.AbsVoxelObj model;
        Graphics.Mesh resourceModel;

        WorkerUnitState state = WorkerUnitState.None;
        Vector3 goalPos;
        Vector3 walkDir;
        AbsArmy parentMapObject;
        float finalizeWorkTime;
        GameTimer workAnimation = new GameTimer(1f, true, true);
        bool isShip = false;
        int prevX, prevZ;
        float walkDist_beforeRefresh = 0f;
        AbsWorkEffect workEffect = null;

        public WorkerUnit(AbsArmy mapObject, WorkerStatus status, int statusIndex)
        {
            parentMapObject = mapObject;
            factionIndex = mapObject.factionIndex;
            //this.status = status;
            myIndex = statusIndex;
            model = mapObject.GetFaction_NoChecks().AutoLoadModelInstance_batched(
                 DssLib.WorkerModel, DssConst.Men_StandardModelScale * 0.9f);

            model.position = WP.SubtileToWorldPosXZ(status.subTileStart);

            checkForGoal(true, mapObject.GetCity());

            updateGroudY(true);
            refreshCarryModel();
        }

        public bool update(City city)
        {

            //if (myIndex == 6)
            //{
            //    lib.DoNothing();
            //}

            ref WorkerStatus status = ref parentMapObject.getRefWorkerStatus(myIndex);

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
                        //state = WorkerUnitState.FinalizeWork;
                       
                        model.Frame = 0;
                        updateGroudY(true);

                        if ((status.work == WorkType.Build || status.work == WorkType.Upgrade || status.work == WorkType.Demolish) &&
                            !status.orderIsActive(city))
                        {
                            state = WorkerUnitState.None;
                            status.cancelWork();
                            parentMapObject.setWorkerStatus(myIndex, ref status);
                        }
                        else
                        {
                            beginWork();
                        }
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

                        if (isShip)
                        {
                            if (/*Ref.TimePassed16ms &&*/ Ref.peRnd.ChanceF(0.3f/Ref.UpdateTimes60FPS))
                            {
                                Engine.ParticleHandler.AddParticleAreaFlat(Graphics.ParticleSystemType.WaterFoam, VectorExt.SetY(model.position, Tile.WaterSurfaceY),
                                    DssConst.Men_StandardModelScale * 0.2f, 4);
                            }
                        }
                        else
                        {
                            walkingAnimation.update(speed, model, out _);
                           
                        }

                    }

                    break;

                case WorkerUnitState.FinalizeWork:
                    workEffect?.update();
                    switch (status.work)
                    {
                        case WorkType.GatherFoil:
                            if (workAnimation_soundframe())
                            {
                                workEffect?.onSoundAnimation();
                                if (DssRef.world.subTileGrid.TryGet(status.subTileEnd, out SubTile subTile))
                                {
                                    switch ((TerrainSubFoilType)subTile.subTerrain)
                                    {
                                        case TerrainSubFoilType.DryWood:
                                        case TerrainSubFoilType.TreeSoft:
                                        case TerrainSubFoilType.TreeHard:
                                            SoundLib.woodcut.Play(model.position);
                                            break;
                                        case TerrainSubFoilType.TreeApple:
                                        case TerrainSubFoilType.TreeBanana:
                                        case TerrainSubFoilType.WheatFarm:
                                        case TerrainSubFoilType.WheatFarmUpgraded:
                                        case TerrainSubFoilType.LinenFarm:
                                        case TerrainSubFoilType.LinenFarmUpgraded:
                                        case TerrainSubFoilType.RapeSeedFarm:
                                        case TerrainSubFoilType.RapeSeedFarmUpgraded:
                                        case TerrainSubFoilType.HempFarm:
                                        case TerrainSubFoilType.HempFarmUpgraded:
                                            if (SoundStackManager.RareAvailable())
                                            {
                                                SoundLib.scythe.Play(model.position);
                                            }
                                            break;
                                        case TerrainSubFoilType.StoneBlock:
                                            SoundLib.pickaxe.Play(model.position);
                                            break;
                                        case TerrainSubFoilType.ClayPit:
                                        case TerrainSubFoilType.BogIron:
                                        case TerrainSubFoilType.Stones:
                                            if (SoundStackManager.RareAvailable())
                                            {
                                                SoundLib.dig.Play(model.position);
                                            }
                                            break;
                                    }
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
                            if (workAnimation_soundframe() && SoundStackManager.RareAvailable())
                            {
                                SoundLib.dig.Play(model.position);
                            }
                            break;
                        
                        case WorkType.Craft:
                            if (workAnimation_soundframe())
                            {
                                if (DssRef.world.subTileGrid.TryGet(status.subTileEnd, out SubTile subTile))
                                {
                                    var building = (TerrainBuildingType)subTile.subTerrain;

                                    switch (building)
                                    {
                                        case TerrainBuildingType.Brewery:
                                        case TerrainBuildingType.Work_Bench:
                                        case TerrainBuildingType.Work_Cook:
                                            if (SoundStackManager.RareAvailable())
                                            {
                                                SoundLib.genericWork.Play(model.position);
                                            }
                                            break;
                                        case TerrainBuildingType.Work_Smith:
                                            if (SoundStackManager.RareAvailable())
                                            {
                                                SoundLib.anvil.Play(model.position);
                                            }
                                            break;
                                    }
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
                        case WorkType.Upgrade:
                        case WorkType.Demolish:
                        case WorkType.School:
                            if (workAnimation_soundframe() && SoundStackManager.RareAvailable())
                            {
                                SoundLib.hammer.Play(model.position);
                            }
                            break;
                    }

                    finalizeWorkTime -= Ref.DeltaGameTimeSec;
                    if (finalizeWorkTime <= 0)
                    {
                        workEffect = null;

                        switch (status.work)
                        {
                            case WorkType.GatherFoil:
                                if (DssRef.world.subTileGrid.TryGet(status.subTileEnd, out SubTile subTile))
                                {
                                    switch ((TerrainSubFoilType)subTile.subTerrain)
                                    {
                                        case TerrainSubFoilType.DryWood:
                                        case TerrainSubFoilType.TreeSoft:
                                        case TerrainSubFoilType.TreeHard:
                                            SoundLib.tree_falling.Play(model.position);
                                            break;

                                        case TerrainSubFoilType.Stones:
                                            if (SoundStackManager.RareAvailable())
                                            {
                                                SoundLib.pickup.Play(model.position);
                                            }
                                            break;
                                    }
                                    EditSubTile.OntileChange(WP.SubtileToTilePos(status.subTileEnd));
                                }
                                break;
                            case WorkType.Plant:
                                int waterCost;
                                switch ((TerrainSubFoilType)DssRef.world.subTileGrid.Get(status.subTileEnd).subTerrain)
                                {
                                    case TerrainSubFoilType.TreeApple:
                                    case TerrainSubFoilType.TreeBanana:
                                        waterCost = DssConst.OrchardWaterCost;
                                        break;
                                    default:
                                        waterCost = DssConst.PlantWaterCost;
                                        break;
                                }
                                SoundLib.drop_item.Play(model.position);
                                /*new ResourceEffect*/
                                SpriteText3D.GetOrCreate().init(ItemResourceType.Water_G, -waterCost, model.position, ResourceEffectType.Add);
                                EditSubTile.OntileChange(WP.SubtileToTilePos(status.subTileEnd));
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

                            case WorkType.Demolish:
                                SoundLib.breaking.Play(model.position);
                                break;

                            case WorkType.Starving:
                            case WorkType.Exit:
                                DeleteMe();
                                break;
                        }

//#if !DEBUG
//                        try
//                        {
//#endif
                            status.WorkComplete(parentMapObject, true);
//#if !DEBUG
//                        }
//                        catch
//                        {
//                            //muted
//                            lib.DoNothing();
//                        }
//#endif
                        //parentMapObject.setWorkerStatus(myIndex, ref status);
                        state = WorkerUnitState.None;
                        refreshCarryModel();
                    }
                    break;

                case WorkerUnitState.None:
                    //parentMapObject.getWorkerStatus(myIndex, ref status);
                    checkForGoal(false, city);
                    break;
            }

            return model.IsDeleted;
        }

        bool workAnimation_soundframe()
        {
            if (workAnimation.timeOut())
            {
                model.Frame = model.Frame == 0 ? 2 : 0;
                return model.Frame == 2;
            }

            return false;
        }

        void beginWork()
        {
            ref WorkerStatus status = ref parentMapObject.getRefWorkerStatus(myIndex);
            state = WorkerUnitState.FinalizeWork;

            switch (status.work)
            {
                case WorkType.Craft:
                    if (DssRef.world.subTileGrid.TryGet(status.subTileEnd, out SubTile subTile))
                    {
                        var building = (TerrainBuildingType)subTile.subTerrain;

                        switch (building)
                        {
                            case TerrainBuildingType.Work_Cook:
                                workEffect = new CookingWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Work_CoalPit:
                                workEffect = new CoalPitWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Work_Smith:
                                workEffect = new SmithWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Smelter:
                                workEffect = new SmelterWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Foundry:
                                workEffect = new FoundryWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Brewery:
                                workEffect = new BreweryWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Pottery:
                                workEffect = new PotteryWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Butcher:
                                workEffect = new ButcherWorkEffect(status.subTileEnd);
                                break;
                            case TerrainBuildingType.Smoker:
                                workEffect = new SmokingWorkEffect(status.subTileEnd);
                                break;

                        }
                        
                    }
                    break;
            }
        }

        protected void checkForGoal(bool onInit, City city)
        {
            ref WorkerStatus status = ref parentMapObject.getRefWorkerStatus(myIndex);
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
                    beginWork();
                }
                else
                {
                    refreshGoalDir();

                    finalizeWorkTime = status.finalizeWorkTime(city);

                    if (onInit)
                    {
                        float timePassed = Ref.TotalGameTimeSec - status.processTimeStartStampSec;
                        float walkingPerc = timePassed / (status.processTimeLengthSec - finalizeWorkTime);

                        if (walkingPerc >= 1)
                        {
                            model.position = goalPos;
                            finalizeWorkTime = status.processTimeLengthSec - timePassed;
                        }
                        else
                        {
                            model.position = model.position * (1 - walkingPerc) + goalPos * walkingPerc;
                        }
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
                resourceModel?.DeleteMe();
                resourceModel = null;
            }
        }

        void refreshGoalDir()
        {
            ref WorkerStatus status = ref parentMapObject.getRefWorkerStatus(myIndex);
            walkDist_beforeRefresh = 0;
            goalPos = WP.SubtileToWorldPosXZ(status.subTileEnd);
            goalPos.X += WorldData.SubTileWidth * 0.25f;
            goalPos.Z += WorldData.SubTileWidth * 0.1f;

            walkDir = VectorExt.SafeNormalizeV3(goalPos - model.position);
            WP.Rotation1DToQuaterion(model, lib.V2ToAngle(VectorExt.V3XZtoV2(walkDir)));
        }

        void refreshCarryModel()
        {
            ref WorkerStatus status = ref parentMapObject.getRefWorkerStatus(myIndex);
            SpriteName sprite = SpriteName.NO_IMAGE;
            bool hasImage;
            if (status.carry.amount > 0)
            {
                hasImage = true;
                IconName.Item(status.carry.type, out sprite, out var name);
                //sprite = Resource.ResourceLib.Icon(status.carry.type);
            }
            else if (status.work == WorkType.Craft && state == WorkerUnitState.FinalizeWork)
            {
                hasImage = true;
                ItemResourceType item = (ItemResourceType)status.workSubType;
                IconName.Item(item, out sprite, out var name);
                //sprite = ResourceLib.Icon(item);
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
#if DEBUG
                    resourceModel.DebugName = "resourceModel";
#endif
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

        const float ModelGroundYAdj = 0.01f;
        protected void updateGroudY(bool set)
        {
            if (DssRef.world.unitBounds.IntersectPoint(model.position.X, model.position.Z))
            {
                float y = DssRef.world.SubTileHeight(model.position) + 0.01f;//ModelGroundYAdj;

                if (y < Tile.UnitMinY)
                {
                    y = Tile.UnitMinY;
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

        void WorkerPresentationHud(ObjectHudArgs args, bool tooltip)
        {
            args.content.Add(new RbBeginTitle(tooltip ? 2 : 1));
            args.content.Add(GetFaction().FlagTextureToHud());
            args.content.space(0.5f);
            args.content.Add(new RbImage(SpriteName.WarsWorker));
            args.content.space(0.5f);
            args.content.Add(new RbText(DssRef.lang.UnitType_Worker, tooltip ? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));

            args.content.space();
            args.content.Add(new RbText(string.Format(DssRef.lang.UnitId, myIndex), HudLib.SecondaryTextColor));

            ownerToHud(args, false);
        }
        public override void toHud(ObjectHudArgs args)
        {
            WorkerStatus status = parentMapObject.getWorkerStatus(myIndex);
            WorkerPresentationHud(args, false);
            //args.content.h2(Name(out _)).overrideColor = Color.LightYellow;
            args.content.text(string.Format(DssRef.lang.WorkerHud_WorkType, status.workString()));
            
            args.content.Add(new RbSeperationLine());
            status.xpToHud(args.content);

            if (status.carry.amount > 0)
            {
                args.content.newLine();
                args.content.Add(new RbImage(SpriteName.WarsWorkMove));
                args.content.space();
                IconName.Item(status.carry.type, out var icon, out var name);
                args.content.Add(new RbText(string.Format(DssRef.lang.WorkerHud_Carry, status.carry.amount, name)));
            }

            args.content.text(string.Format(DssRef.lang.WorkerHud_Energy, TextLib.OneDecimal(status.energy)));

            if (DssRef.difficulty.GodPowers() && GetCity() != null)
            {
                args.content.newParagraph();
                Color? fontColor = DssRef.difficulty.GodPowers()? HudLib.GodPower_Color : null;
                foreach (var exp in XpLib.ExperienceTypes)
                {
                    LangLib.ExperienceType(exp, out string text, out SpriteName icon);
                    

                    var buttonContent = new List<AbsRichBoxMember>()
                    {
                        new RbImage(icon),
                        new RbSpace(),
                        new RbText(text, fontColor),
                    };

                    args.content.Add(new ArtButton(RbButtonStyle.GodPower, buttonContent, new RbAction1Arg<WorkExperienceType>(
                        (WorkExperienceType xp) =>
                    {
                        var current = status.getXpFor(xp);
                        int maxAdd = DssConst.WorkLevel_Master - current.xp;

                        if (maxAdd > 0)
                        {
                            status.addExperience(xp, args.player.gameControls.map.selection.obj.GetCity(), (byte)Bound.Max(DssConst.WorkLevel_Master, maxAdd));
                        }
                    }, exp)));
                    args.content.space();
                    // var button = new ArtOption(exp == currentStatus.learnExperience, buttonContent,
                    //    new RbAction1Arg<WorkExperienceType>(experienceClick, exp, RbSoundType.Option),
                    //new RbTooltip(expTooltip, exp));
                    // //button.setGroupSelectionColor(HudLib.RbSettings, );
                    // content.Add(button);
                    //content.space();
                }

                //args.content.newLine();
                //HudLib.Label(args.content, DssRef.lang.GeneralSetting_SetAll);
                //args.content.space();
                //args.content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> {
                //    new RbImage(SpriteName.WarsUnitLevelMinimal),  new RbSpace(), new RbText(DssRef.lang.ExperienceLevel_1, HudLib.GodPower_Color),  
                //},
                //    new RbAction(() => {
                        
                //        status.xp1 = 0;
                //        status.xp2 = 0;
                //        status.xp3 = 0;

                //    })));
            }

            args.content.text("status ix: " + myIndex.ToString(), HudLib.SecondaryTextColor);
            args.content.text("xp ix: " + status.XpEntityIndex.ToString(), HudLib.SecondaryTextColor);

//#if DEBUG
//            args.content.text(string.Format("XP1: {0} {1}", status.xpType1, status.xp1));
//            args.content.text(string.Format("XP2: {0} {1}", status.xpType2, status.xp2));
//            args.content.text(string.Format("XP3: {0} {1}", status.xpType3, status.xp3));
//#endif
        }

        //public void toolTip(RichBoxContent content)
        //{
        //    WorkerPresentationHud(content, true);
        //    status.xpToHud(content);
        //}
        public override void toTooltip(ObjectHudArgs args)
        {
            WorkerStatus status = parentMapObject.getWorkerStatus(myIndex);
            WorkerPresentationHud(args, true);
            status.xpToHud(args.content);
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            Vector3 scale = new Vector3(DssVar.StandardBoundRadius * 2f);
            selection.groupModels_detail.BeginGroupModel();
            selection.groupModels_detail.setGroupModel(0, model.position, scale, hover, true, false);

        }

        public void DeleteMe()
        {
            model.preRemoveFromDrawBatch();//.DeleteMe();
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
        public override City GetCity()
        {
            return parentMapObject.GetCity();
        }
        //public override Faction GetFaction()
        //{
        //    return parentMapObject.GetFaction();
        //}

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

        public override string Name(out bool mayEdit)
        {
            mayEdit = false;
            return parentMapObject.TypeName() + " " + DssRef.lang.UnitType_Worker + " (" + myIndex.ToString() + ")";
        }

        enum WorkerUnitState
        {
            None,
            HasGoal,
            FinalizeWork,
        }
    }
}
