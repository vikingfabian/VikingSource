using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        bool chariot = false;
        int firstUpdate = 10;
        float wagonGoalDistance;
        Vector3 wagonPos;
        float wagonY;
        Graphics.VoxelModelInstance animalmodel_left, animalmodel_right;
        Vector3 leftAnimalPosDiff, rightAnimalPosDiff;
        WalkingAnimation horseWalkingAnimation;
        WalkingAnimation wagonRollAnimation;

        Graphics.VoxelModelInstance soldierLeft, soldierRight, soldierBackLeft, soldierBackRight;
        Vector3 leftSoldierPosDiff, rightSoldierPosDiff, backleftSoldierPosDiff, backrightSoldierPosDiff;
        public WagonRiderModel(AbsSoldierUnit soldier)
        {
            CavalryModel.AnimalModel(soldier.group.soldierConscript.conscript.animal, out VoxelModelName modelName, out float modelScale, out horseWalkingAnimation, out float riderY);
            animalmodel_left = DssRef.models.ModelInstance_drawbatch(modelName, modelScale);
            animalmodel_right = DssRef.models.ModelInstance_drawbatch(modelName, modelScale);

            
            LootFest.VoxelModelName wagonModelName;
            switch (soldier.group.soldierConscript.conscript.vehicle)
            {
                default:
                case Resource.ItemResourceType.Wagon2Wheel:
                    wagonModelName = VoxelModelName.wagon_light;
                    chariot = true;
                    break;
                case Resource.ItemResourceType.Wagon4Wheel:
                    wagonModelName = VoxelModelName.wagon_light4;
                    break;
                case Resource.ItemResourceType.WagonClosed:
                    wagonModelName = VoxelModelName.wagon_coach;
                    break;
                case Resource.ItemResourceType.WagonIron:
                case Resource.ItemResourceType.WagonSteel:
                    wagonModelName = VoxelModelName.wagon_ironcoach;
                    break;

            }

            wagonRollAnimation = new WalkingAnimation(0, 3, WalkingAnimation.StandardMoveFrames * 1f);

            leftAnimalPosDiff = new Vector3(-0.24f, 0, 0f) * modelScale;
            rightAnimalPosDiff = leftAnimalPosDiff;
            rightAnimalPosDiff.X = -rightAnimalPosDiff.X;

            float wagonScale = DssConst.Men_StandardModelScale * 2f;
            model = DssRef.models.ModelInstance_drawbatch(wagonModelName, wagonScale);

            wagonGoalDistance = modelScale * (chariot? 0.5f : 0.65f);

            wagonPos = VectorExt.AddXZ(soldier.position, -soldier.rotation.Direction(wagonGoalDistance));
            wagonY = 0.02f * wagonScale;

            var faction = soldier.GetFaction_NoChecks();
            soldierLeft = faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);
            soldierRight = faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);

            if (chariot)
            {
                leftSoldierPosDiff = new Vector3(-0.08f, 0.1f, -0.12f) * wagonScale;
                rightSoldierPosDiff = leftSoldierPosDiff;
                rightSoldierPosDiff.X = -rightSoldierPosDiff.X;
                rightSoldierPosDiff.Z *= 0.6f;
            }
            else 
            {
                soldierBackLeft = faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);
                soldierBackRight = faction.AutoLoadModelInstance_character(
                            soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);

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

            update(soldier);
        }

        public override void DeleteMe()
        {
            //base.DeleteMe();
            model.preRemoveFromDrawBatch();
            animalmodel_left.preRemoveFromDrawBatch();
            animalmodel_right.preRemoveFromDrawBatch();
            soldierLeft.preRemoveFromDrawBatch();
            soldierRight.preRemoveFromDrawBatch();

            if (soldierBackLeft != null)
            {
                soldierBackLeft.preRemoveFromDrawBatch();
                soldierBackRight.preRemoveFromDrawBatch();
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
                
                soldierLeft.position = model.Rotation.TranslateAlongAxis(leftSoldierPosDiff, wagonPos);
                soldierRight.position = model.Rotation.TranslateAlongAxis(rightSoldierPosDiff, wagonPos);
                
                if (chariot)
                {
                    soldierLeft.Rotation = model.Rotation;
                    soldierRight.Rotation = model.Rotation;
                }
                else
                {
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
                soldierLeft.Frame = CharacterModelBuilder.IdleFrame;
                soldierRight.Frame = CharacterModelBuilder.IdleFrame;
                animalmodel_left.Frame = 0;
                animalmodel_right.Frame = 0;
                
            }
        }
    }
}
