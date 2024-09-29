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
    class KnightData : AbsSoldierProfile
    {
        public KnightData() 
        {
            unitType = UnitType.Knight;

            modelScale = DssConst.Men_StandardModelScale * 1.8f;
            boundRadius = DssVar.StandardBoundRadius;

            idleFrame = 0;
            attackFrame = 1;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed * 2f;
            ArmySpeedBonusLand = 0.8;
            rotationSpeed = StandardRotatingSpeed * 2f;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.06f;
            basehealth = DssConst.Soldier_DefaultHealth * 3;
            mainAttack = AttackType.Melee;
            attackDamage = 120;
            attackDamageStructure = 30;
            attackDamageSea = 20;
            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;

            rowWidth = 4;
            columnsDepth = 3;
            groupSpacing = DssVar.DefaultGroupSpacing * 1.4f;

            goldCost = MathExt.MultiplyInt(2, DssLib.GroupDefaultCost);
            workForcePerUnit = 2;
            upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            recruitTrainingTimeSec = MathExt.MultiplyInt(1.5, DssLib.DefalutRecruitTrainingTimeSec);

            modelAdjY = 0.1f;
            modelName = LootFest.VoxelModelName.war_knight;
            modelVariationCount = 3;
            hasBannerMan = false;

            description = DssRef.lang.UnitType_Description_Knight;
            icon = SpriteName.WarsUnitIcon_Knight;

            energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 3;
        }


        public override AbsDetailUnit CreateUnit()
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
            if (this.parentArrayIndex == 11)
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
        
        Graphics.AbsVoxelObj horsemodel;
        public KnightModel(AbsSoldierUnit soldier)
           : base(soldier)
        {
          
           horsemodel = DssRef.models.ModelInstance(Ref.rnd.Chance(0.2)? VoxelModelName.horse_white : VoxelModelName.horse_brown, DssConst.Men_StandardModelScale * 1.6f,false);
           horsemodel.AddToRender(DrawGame.UnitDetailLayer);

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
                model.Frame = soldier.data.attackFrame;
            }
            else
            {
                model.Frame = soldier.data.idleFrame;
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
            horsemodel.DeleteMe();
        }
    }

    class KnightBannerModel : KnightModel
    {
        HorseBanner banner;

        public KnightBannerModel(AbsSoldierUnit soldier)
            : base(soldier)
        {
            banner = new HorseBanner(soldier.GetFaction(), soldier.data.modelScale);
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

    class HorseBanner : AbsModelAttachment
    {
        public HorseBanner(Faction faction, float soldierScale)
        {
            model = faction.AutoLoadModelInstance(
               modelName(), soldierScale * 1f, true);
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
