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
            unitType = UnitType.ConscriptCavalry;

            //modelScale = DssConst.Men_StandardModelScale * 1.5f;
            boundRadius = DssVar.StandardBoundRadius;

            idleFrame = 0;
            attackFrame = 1;
            
            //ArmySpeedBonusLand = 0.8;
            //rotationSpeed = SoldierGroupStandardRotatingSpeed * 2f;
            targetSpotRange = StandardTargetSpotRange;
                        
            

            goldCost = MathExt.MultiplyInt(2, DssLib.GroupDefaultCost);

           
            
            modelAdjY = 0.1f;
            //hasBannerMan = false;

            description = DssRef.lang.UnitType_Description_Knight;
        }

        public override AbsSoldierUnit CreateUnit()
        {
            return new Knight();
        }
    }
    class Knight : BaseSoldier
    {
        public Knight()
            : base()
        {    
            
        }

        protected override DetailUnitModel initModel()
        {
            updateGroudY(true);
            if (this.myIndex == 11)
            {
                return new KnightBannerModel(this);
            }
            else
            {
                return new KnightModel(this);
            }
        }
    }

    class KnightModel : SoldierUnitAdvancedModel
    {
        
        Graphics.VoxelModelInstance horsemodel;
        public KnightModel(AbsSoldierUnit soldier)
           : base(soldier)
        {
          
           horsemodel = DssRef.models.ModelInstance_drawbatch(Ref.rnd.Chance(0.2)? VoxelModelName.horse_white : VoxelModelName.horse_brown, DssConst.Men_StandardModelScale * 1.5f);
           //horsemodel.AddToRender(DrawGame.UnitDetailLayer);

           walkingAnimation = new WalkingAnimation(1, 6, WalkingAnimation.StandardMoveFrames*2f);
        }

        public override void update(AbsSoldierUnit soldier)
        {
            base.update(soldier);

            model.position.Y += 0.03f;
        }

        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            if (soldier.inAttackAnimation())
            {
                model.Frame = soldier.Profile().attackFrame;
            }
            else
            {
                model.Frame = soldier.Profile().idleFrame;
            }

            if (soldier.state.walking)
            {
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);

                walkingAnimation.update(move, horsemodel);
            }
            else 
            {
                horsemodel.Frame = 0;
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);
            
            horsemodel.position = model.position;
            horsemodel.Rotation = model.Rotation;
        }
        //protected override void updateShipAnimation(AbsSoldierUnit soldier)
        //{
        //    base.updateShipAnimation(soldier);
        //    horsemodel.position = model.position;
        //    horsemodel.Rotation = model.Rotation;
        //}

        public override void DeleteMe()
        {
            base.DeleteMe();
            //horsemodel.DeleteMe();
            //DssRef.models.recycle(ref horsemodel, true);
            horsemodel.preRemoveFromDrawBatch();
        }
    }

    class KnightBannerModel : KnightModel
    {
        HorseBanner banner;

        public KnightBannerModel(AbsSoldierUnit soldier)
            : base(soldier)
        {
            banner = new HorseBanner(soldier.GetFaction(), soldier.soldierData.modelScale);
        }

        //protected override void updateShipAnimation(AbsSoldierUnit soldier)
        //{
        //    base.updateShipAnimation(soldier);
        //    banner.update(soldier);
        //}
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
               modelName(), soldierScale * 1f);
            diff = new Vector3(-0.12f, 0.15f, -0.05f) * soldierScale;
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
