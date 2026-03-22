using Microsoft.Xna.Framework;
using Sentry.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.LootFest;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class WagonBuilder : ConscriptedSoldierBuilder
    {
        public WagonBuilder() 
            :base()
        { 
            unitBuildType = UnitBuildType.ConscriptWagon;            
        }

        public override AbsSoldierUnit CreateUnit(bool bannerman)
        {
            return new WagonSoldier();
        }
    }

    class WagonSoldier : BaseSoldier
    {
        public WagonSoldier()
            : base()
        {

        }

        protected override DetailUnitModel initModel(bool bannerman)
        {
            updateGroudY(true);

            return new WagonRiderModel(this);
        }
    }

    class WagonRiderModel : AbsDetailUnitAdvancedModel
    {
        int firstUpdate = 10;
        float wagonGoalDistance;
        Vector3 wagonPos;
        float wagonY;
        Graphics.VoxelModelInstance animalmodel_left, animalmodel_right;
        Vector3 leftAnimalPosDiff, rightAnimalPosDiff;
        WalkingAnimation horseWalkingAnimation;
        WalkingAnimation wagonRollAnimation;

        WagonManType manType;

        Graphics.VoxelModelInstance soldierLeft, soldierRight, soldierBackLeft, soldierBackRight;
        Vector3 leftSoldierPosDiff, rightSoldierPosDiff, backleftSoldierPosDiff, backrightSoldierPosDiff;
        public WagonRiderModel(AbsSoldierUnit soldier)
        {
            AnimalModelData modelData = CavalryModel.AnimalModel(soldier.group.soldierConscript.conscript.animal);
            horseWalkingAnimation = modelData.animation;
            animalmodel_left = DssRef.models.ModelInstance_drawbatch(modelData.modelName, modelData.scale);
            animalmodel_right = DssRef.models.ModelInstance_drawbatch(modelData.modelName, modelData.scale);

            
            LootFest.VoxelModelName wagonModelName;
            float wagonMScale = 2f;
            float MGoalDistance = 0.65f;
            switch (soldier.group.soldierConscript.conscript.vehicle)
            {
                default:
                case Resource.ItemResourceType.Wagon2Wheel:
                    switch (soldier.group.soldierConscript.conscript.weapon)
                    {
                        default:
                            wagonModelName = VoxelModelName.wagon_light;
                            //chariot = true;
                            MGoalDistance = 0.5f;
                            manType = WagonManType.Chariot;
                            break;
                        case Resource.ItemResourceType.ManCannonIron:
                            wagonMScale = 2.7f;
                            wagonModelName = VoxelModelName.cannonwagon_maniron;
                            manType = WagonManType.Riding;
                            break;
                        case Resource.ItemResourceType.ManCannonBronze:
                            wagonMScale = 2.7f;
                            wagonModelName = VoxelModelName.cannonwagon_manbronze;
                            manType = WagonManType.Riding;
                            break;
                        case Resource.ItemResourceType.Ballista:
                            wagonMScale = 2.7f;
                            wagonModelName = VoxelModelName.cannonwagon_ballista;
                            manType = WagonManType.Riding;
                            break;
                        case Resource.ItemResourceType.Manuballista:
                            wagonMScale = 2.7f;
                            wagonModelName = VoxelModelName.cannonwagon_manuballista;
                            manType = WagonManType.Riding;
                            break;
                        case Resource.ItemResourceType.Catapult:
                            wagonMScale = 2.7f;
                            wagonModelName = VoxelModelName.cannonwagon_catapult;
                            manType = WagonManType.Riding;
                            break;
                       

                    }
                    break;
                case Resource.ItemResourceType.Wagon4Wheel:
                    switch (soldier.group.soldierConscript.conscript.weapon)
                    {
                        default:
                            wagonModelName = VoxelModelName.wagon_light4;
                            manType = WagonManType.ManTransport;
                            break;
                        case Resource.ItemResourceType.ManCannonIron:
                            wagonMScale = 3.5f;
                            MGoalDistance = 0.9f;
                            wagonModelName = VoxelModelName.cannon4wagon_maniron;
                            manType = WagonManType.Coach;
                            break;
                        case Resource.ItemResourceType.ManCannonBronze:
                            wagonMScale = 3.5f;
                            MGoalDistance = 0.9f;
                            wagonModelName = VoxelModelName.cannon4wagon_maniron;
                            manType = WagonManType.Coach;
                            break;
                        case Resource.ItemResourceType.Ballista:
                            wagonMScale = 3.5f;
                            MGoalDistance = 0.9f;
                            wagonModelName = VoxelModelName.cannon4wagon_maniron;
                            manType = WagonManType.Coach;
                            break;
                        case Resource.ItemResourceType.Manuballista:
                            wagonMScale = 3.5f;
                            MGoalDistance = 0.9f;
                            wagonModelName = VoxelModelName.cannon4wagon_maniron;
                            manType = WagonManType.Coach;
                            break;
                        case Resource.ItemResourceType.Catapult:
                            wagonMScale = 3.5f;
                            MGoalDistance = 0.9f;
                            wagonModelName = VoxelModelName.cannon4wagon_maniron;
                            manType = WagonManType.Coach;
                            break;


                    }
                    break;
                case Resource.ItemResourceType.WagonClosed:
                    switch (soldier.group.soldierConscript.conscript.weapon)
                    {
                        default:
                            wagonModelName = VoxelModelName.wagon_coach;
                            manType = WagonManType.ManTransport;
                            break;
                        case Resource.ItemResourceType.ManCannonIron:
                            wagonModelName = VoxelModelName.cannoncoach_maniron;
                            manType = WagonManType.Gunner;
                            break;
                        case Resource.ItemResourceType.ManCannonBronze:
                            wagonModelName = VoxelModelName.cannoncoach_manbronze;
                            manType = WagonManType.Gunner;
                            break;
                        case Resource.ItemResourceType.SiegeCannonIron:
                            wagonModelName = VoxelModelName.cannoncoach_siegeiron;
                            manType = WagonManType.Gunner;
                            break;
                    }
                    break;
                case Resource.ItemResourceType.WagonIron:
                case Resource.ItemResourceType.WagonSteel:
                    switch (soldier.group.soldierConscript.conscript.weapon)
                    {
                        default:
                            wagonModelName = VoxelModelName.wagon_ironcoach;
                            manType = WagonManType.ManTransport;
                            break;
                        case Resource.ItemResourceType.ManCannonIron:
                            wagonModelName = VoxelModelName.cannoncoach_maniron;
                            manType = WagonManType.Gunner;
                            break;
                        case Resource.ItemResourceType.ManCannonBronze:
                            wagonModelName = VoxelModelName.cannoncoach_manbronze;
                            manType = WagonManType.Gunner;
                            break;
                        case Resource.ItemResourceType.SiegeCannonIron:
                            wagonModelName = VoxelModelName.cannoncoach_siegeiron;
                            manType = WagonManType.Gunner;
                            break;
                    }
                    break;

            }

            wagonRollAnimation = new WalkingAnimation(0, 3, WalkingAnimation.StandardMoveFrames * 1f);

            leftAnimalPosDiff = new Vector3(-0.24f, 0, 0f) * modelData.scale;
            rightAnimalPosDiff = leftAnimalPosDiff;
            rightAnimalPosDiff.X = -rightAnimalPosDiff.X;

            float wagonScale = DssConst.Men_StandardModelScale * wagonMScale;
            model = DssRef.models.ModelInstance_drawbatch(wagonModelName, wagonScale);

            wagonGoalDistance = modelData.scale * (MGoalDistance + modelData.wagonPullDistance);

            wagonPos = VectorExt.AddXZ(soldier.position, -soldier.rotation.Direction(wagonGoalDistance));
            wagonY = 0.02f * wagonScale;

            var faction = soldier.GetFaction_NoChecks();
           
            switch (manType)
            {
                case WagonManType.Riding:
                    rightSoldierPosDiff = new Vector3(0, modelData.riderY, 0);
                    soldierRight = createSoldier();
                    break;
                case WagonManType.Coach:
                    rightSoldierPosDiff = new Vector3(0.02f, 0.08f, 0.025f) * wagonScale;
                    soldierRight = createSoldier();
                    break;
                case WagonManType.Gunner:
                    rightSoldierPosDiff = new Vector3(0.02f, 0.08f, -0.3f) * wagonScale;
                    soldierRight = createSoldier();
                    break;
                case WagonManType.Chariot:
                    {
                        soldierLeft = createSoldier();
                        soldierRight = createSoldier();

                        leftSoldierPosDiff = new Vector3(-0.08f, 0.1f, -0.12f) * wagonScale;
                        rightSoldierPosDiff = leftSoldierPosDiff;
                        rightSoldierPosDiff.X = -rightSoldierPosDiff.X;
                        rightSoldierPosDiff.Z *= 0.6f;
                    }
                    break;
                case WagonManType.ManTransport:
                    {
                        soldierLeft = createSoldier();
                        soldierRight = createSoldier();

                        soldierBackLeft = createSoldier();
                        soldierBackRight = createSoldier();

                        Vector3 offset = new Vector3(0.03f, 0.1f, 0.03f) * wagonScale;
                        float zStep = -0.12f * wagonScale;

                        rightSoldierPosDiff = offset;

                        offset.Z += zStep;
                        leftSoldierPosDiff = VectorExt.FlipX(offset);

                        offset.Z += zStep;
                        backrightSoldierPosDiff = offset;

                        offset.Z += zStep;
                        backleftSoldierPosDiff = VectorExt.FlipX(offset);
                    }
                    break;
            }
            update(soldier);

            VoxelModelInstance_Pooled createSoldier()
            {
                return faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);
            }
        }

        public override void DeleteMe()
        {
            //base.DeleteMe();
            model.preRemoveFromDrawBatch();
            animalmodel_left.preRemoveFromDrawBatch();
            animalmodel_right.preRemoveFromDrawBatch();
            
            soldierRight.preRemoveFromDrawBatch();

            if (soldierLeft != null)
            {
                soldierLeft.preRemoveFromDrawBatch();
                if (soldierBackLeft != null)
                {
                    soldierBackLeft.preRemoveFromDrawBatch();
                    soldierBackRight.preRemoveFromDrawBatch();
                }
            }
        }

        public override void update(AbsSoldierUnit soldier)
        {
            if (soldier.state.walking || firstUpdate > 0)
            {
                firstUpdate--;
                Vector3 center = soldier.position;

                WP.Rotation1DToQuaterion(animalmodel_left, soldier.rotation.Radians);
                animalmodel_left.position = animalmodel_left.Rotation.TranslateAlongAxis(
                    leftAnimalPosDiff, center);

                animalmodel_right.Rotation = animalmodel_left.Rotation;
                animalmodel_right.position = animalmodel_left.Rotation.TranslateAlongAxis(
                    rightAnimalPosDiff, center);

                if (firstUpdate > 0)
                {
                    wagonPos = animalmodel_left.Rotation.TranslateAlongAxis(
                        new Vector3(0, 0, -wagonGoalDistance), center);
                    model.Rotation = animalmodel_left.Rotation;
                }
                else
                {
                    Vector3 offset = wagonPos - center;
                    offset.Y = 0;

                    offset.Normalize();
                    wagonPos = offset * wagonGoalDistance + center;
                    wagonPos.Y = center.Y + wagonY;
                    WP.Rotation1DToQuaterion(model, lib.V3XZToAngle(-offset));
                }
                model.position = wagonPos;
                
                

                switch (manType)
                {
                    
                    case WagonManType.ManTransport:
                        soldierLeft.position = model.Rotation.TranslateAlongAxis(leftSoldierPosDiff, wagonPos);
                        soldierRight.position = model.Rotation.TranslateAlongAxis(rightSoldierPosDiff, wagonPos);

                        soldierBackLeft.position = model.Rotation.TranslateAlongAxis(backleftSoldierPosDiff, wagonPos);
                        soldierBackRight.position = model.Rotation.TranslateAlongAxis(backrightSoldierPosDiff, wagonPos);

                        const float RotateSoldier = 0.5f;
                        RotationQuarterion left = model.Rotation;
                        left.RotateWorldX(RotateSoldier);
                        RotationQuarterion right = model.Rotation;
                        right.RotateWorldX(-RotateSoldier);

                        soldierLeft.Rotation = left;
                        soldierRight.Rotation = right;
                        soldierBackLeft.Rotation = left;
                        soldierBackRight.Rotation = right;
                        break;

                    case WagonManType.Riding:
                        soldierRight.position = animalmodel_right.position + rightSoldierPosDiff;
                        soldierRight.Rotation = animalmodel_right.Rotation;
                        break;
                    
                    case WagonManType.Chariot:
                        soldierLeft.position = model.Rotation.TranslateAlongAxis(leftSoldierPosDiff, wagonPos);
                        soldierRight.position = model.Rotation.TranslateAlongAxis(rightSoldierPosDiff, wagonPos);

                        soldierLeft.Rotation = model.Rotation;
                        soldierRight.Rotation = model.Rotation;
                        break;
                    case WagonManType.Gunner:
                    case WagonManType.Coach:
                        soldierRight.position = model.Rotation.TranslateAlongAxis(rightSoldierPosDiff, wagonPos);
                        soldierRight.Rotation = model.Rotation;
                        break;
                }

                //Animation
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);
                horseWalkingAnimation.update(move, animalmodel_left);
                animalmodel_right.Frame = animalmodel_left.Frame;
                wagonRollAnimation.update(move, model);
            }
            else if (soldier.inAttackAnimation())
            {
                soldierRight.Frame = CharacterModelBuilder.AttackFrame;
            }
            else
            {

                //soldierLeft.Frame = CharacterModelBuilder.IdleFrame;
                
                soldierRight.Frame = CharacterModelBuilder.IdleFrame;
                animalmodel_left.Frame = 0;
                animalmodel_right.Frame = 0;
                
            }
        }

        enum WagonManType
        { 
            ManTransport,
            Chariot,
            Riding,
            Coach,
            Gunner,
        }
    }
}
