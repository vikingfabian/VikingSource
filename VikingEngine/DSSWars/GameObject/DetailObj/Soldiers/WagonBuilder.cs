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
        int firstUpdate = 10;
        float wagonGoalDistance;
        Vector3 wagonPos;
        float wagonY;
        Graphics.VoxelModelInstance animalmodel_left, animalmodel_right;
        Vector3 leftAnimalPosDiff, rightAnimalPosDiff;
        WalkingAnimation horseWalkingAnimation;
        WalkingAnimation wagonRollAnimation;

        Graphics.VoxelModelInstance soldierLeft, soldierRight;
        Vector3 leftSoldierPosDiff, rightSoldierPosDiff;
        public WagonRiderModel(AbsSoldierUnit soldier)
        {
            CavalryModel.AnimalModel(soldier.group.soldierConscript.conscript.animal, out VoxelModelName modelName, out float modelScale, out horseWalkingAnimation, out float riderY);
            animalmodel_left = DssRef.models.ModelInstance_drawbatch(modelName, modelScale);
            animalmodel_right = DssRef.models.ModelInstance_drawbatch(modelName, modelScale);

            wagonRollAnimation = new WalkingAnimation(0, 3, WalkingAnimation.StandardMoveFrames * 1f);

            leftAnimalPosDiff = new Vector3(-0.24f, 0, 0f) * modelScale;
            rightAnimalPosDiff = leftAnimalPosDiff;
            rightAnimalPosDiff.X = -rightAnimalPosDiff.X;

            float wagonScale = DssConst.Men_StandardModelScale * 2f;
            model = DssRef.models.ModelInstance_drawbatch( LootFest.VoxelModelName.wagon_light, wagonScale);

            wagonGoalDistance = modelScale * 0.5f;

            wagonPos = VectorExt.AddXZ(soldier.position, -soldier.rotation.Direction(wagonGoalDistance));
            wagonY = 0.02f * wagonScale;

            var faction = soldier.GetFaction_NoChecks();
            soldierLeft = faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);
            soldierRight = faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);

            leftSoldierPosDiff = new Vector3(-0.08f, 0.1f, -0.12f) * wagonScale;
            rightSoldierPosDiff = leftSoldierPosDiff;
            rightSoldierPosDiff.X = -rightSoldierPosDiff.X;
            rightSoldierPosDiff.Z *= 0.6f;

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
                soldierLeft.Rotation = model.Rotation;
                soldierRight.Rotation = model.Rotation;

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
