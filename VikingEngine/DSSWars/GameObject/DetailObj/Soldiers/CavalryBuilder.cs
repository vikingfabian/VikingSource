using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.GameObject
{
    
    class CavalryBuilder : ConscriptedSoldierBuilder
    {
        public CavalryBuilder()
            :base()
        {
            unitBuildType = UnitBuildType.ConscriptCavalry;
            //boundRadius = DssVar.StandardBoundRadius;

            //idleFrame = 0;
            targetSpotRange = StandardTargetSpotRange;
            
            goldCost = MathExt.MultiplyInt(2, DssLib.GroupDefaultCost);

            modelAdjY = 0.1f;
        }

        public override AbsSoldierUnit CreateUnit(bool bannerman)
        {
            return new CavalrySoldier();
        }
    }

    class CavalrySoldier : BaseSoldier
    {
        public CavalrySoldier()
            : base()
        {    
            
        }

        protected override DetailUnitModel initModel(bool bannerman)
        {
            updateGroudY(true);
            if (bannerman)
            {
                return new CavalryBannerModel(this);
            }
            else
            {
                return new CavalryModel(this);
            }
        }
    }

    class CavalryModel : SoldierUnitAdvancedModel
    {
        
        Graphics.VoxelModelInstance animalmodel;
        float riderY;
        public CavalryModel(AbsSoldierUnit soldier)
           : base(soldier)
        {

            switch (soldier.group.soldierConscript.conscript.animal)
            {
                default:
                    animalmodel = DssRef.models.ModelInstance_drawbatch(Ref.rnd.Chance(0.2) ? VoxelModelName.horse_white : VoxelModelName.horse_brown, DssConst.Men_StandardModelScale * 1.1f);
                    walkingAnimation = new WalkingAnimation(1, 6, WalkingAnimation.StandardMoveFrames * 2f);
                    riderY = 0.018f;
                    break;
                
                case Resource.ItemResourceType.WildPig:
                case Resource.ItemResourceType.WildHog:
                case Resource.ItemResourceType.StagHog:
                    animalmodel = DssRef.models.ModelInstance_drawbatch(VoxelModelName.hog1, DssConst.Men_StandardModelScale * 1.1f);
                    walkingAnimation = new WalkingAnimation(1, 5, WalkingAnimation.StandardMoveFrames * 2f);
                    riderY = 0.013f;
                    break;

                case Resource.ItemResourceType.Wolf:
                case Resource.ItemResourceType.Warg:
                case Resource.ItemResourceType.AlphaWarg:
                    animalmodel = DssRef.models.ModelInstance_drawbatch(VoxelModelName.wolf1, DssConst.Men_StandardModelScale * 1.1f);
                    walkingAnimation = new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f);
                    riderY = 0.018f;
                    break;

                case Resource.ItemResourceType.WildCat:
                case Resource.ItemResourceType.Lion:
                case Resource.ItemResourceType.WarLion:
                    animalmodel = DssRef.models.ModelInstance_drawbatch(VoxelModelName.lion1, DssConst.Men_StandardModelScale * 1.1f);
                    walkingAnimation = new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f);
                    riderY = 0.018f;
                    break;

                case Resource.ItemResourceType.Elephant:
                case Resource.ItemResourceType.WarElephant:
                case Resource.ItemResourceType.Oliphant:
                    float scale = DssConst.Men_StandardModelScale * 1.9f;
                    animalmodel = DssRef.models.ModelInstance_drawbatch(VoxelModelName.Elephant1, scale);
                    walkingAnimation = new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames * 2);
                    riderY = 0.38f * scale;
                    break;
            }

            var soldierProfile = soldier.Profile();
            walkingAnimation.attackframe = CharacterModelBuilder.AttackFrame;
            walkingAnimation.idleframe = CharacterModelBuilder.IdleFrame;
            walkingAnimation.idleblinkframe = CharacterModelBuilder.IdleBlinkFrame;
        }

        public override void update(AbsSoldierUnit soldier)
        {
            base.update(soldier);

            model.position.Y += riderY;
        }

        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            if (soldier.inAttackAnimation())
            {
                model.Frame = walkingAnimation.attackframe;
            }
            else
            {
                model.Frame = walkingAnimation.idleframe;
            }

            if (soldier.state.walking)
            {
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);

                walkingAnimation.update(move, animalmodel);
            }
            else 
            {
                animalmodel.Frame = 0;
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);
            
            animalmodel.position = model.position;
            animalmodel.Rotation = model.Rotation;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            animalmodel.preRemoveFromDrawBatch();
        }
    }

    class CavalryBannerModel : CavalryModel
    {
        HorseBanner banner;

        public CavalryBannerModel(AbsSoldierUnit soldier)
            : base(soldier)
        {
            banner = new HorseBanner(soldier.GetFaction(), soldier.soldierData.modelScale);
        }

        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            base.updateAnimation(soldier);
            banner.update(soldier);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            banner.DeleteMe();
        }

        public override void onNewModel(VoxelModelName name, VoxelModel master, AbsDetailUnit unit)
        {
            base.onNewModel(name, master, unit);
            banner.onNewModel_asynch(name, master);
        }
    }

    class HorseBanner : AbsModelAttachment_Batched
    {
        public HorseBanner(Faction faction, float soldierScale)
        {
            model = faction.AutoLoadModelInstance_batched(
               modelName(), soldierScale * 0.8f);
            diff = new Vector3(-0.12f, 0.16f, -0.05f) * soldierScale;
        }

        public override void update(AbsSoldierUnit parent)
        {
            base.update(parent);
            model.Rotation.RotateWorldX(MathExt.TauOver4);
        }

        protected override VoxelModelName modelName()
        {
            return VoxelModelName.horsebanner;
        }
    }
}
