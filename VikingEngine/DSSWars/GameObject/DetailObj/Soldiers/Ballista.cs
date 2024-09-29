using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    class BallistaData : AbsSoldierProfile
    {
        public BallistaData()
        {
            unitType = UnitType.Ballista;

            modelScale = DssConst.Men_StandardModelScale * 2f;
            boundRadius =DssVar.StandardBoundRadius * 2.2f;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            ArmySpeedBonusLand = -0.5;
            rotationSpeed = StandardRotatingSpeed * 0.04f;
            attackRange = 3f;
            targetSpotRange = attackRange + StandardTargetSpotRange;
            basehealth = MathExt.MultiplyInt(0.5, DssConst.Soldier_DefaultHealth);
            mainAttack = AttackType.Ballista;
            attackDamage = 300;
            attackDamageStructure = 1500;
            attackDamageSea = 400;
            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 16f;

            maxAttackAngle = 0.07f;
            
            rowWidth = 3;
            columnsDepth = 2;
            workForcePerUnit = 2;
            goldCost = MathExt.MultiplyInt(0.9, DssLib.GroupDefaultCost);
            upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;

            modelName = LootFest.VoxelModelName.war_ballista;
            modelVariationCount = 2;
            hasBannerMan = false;
            ArmyFrontToBackPlacement = ArmyPlacement.Back;

            description =DssRef.lang.UnitType_Description_Ballista;
            icon = SpriteName.WarsUnitIcon_Ballista;

            energyPerSoldier  = DssLib.SoldierDefaultEnergyUpkeep * 2;
        }

        public override AbsDetailUnit CreateUnit()
        {
            return new Ballista();
        }
    }

    class Ballista : BaseSoldier
    {
        public Ballista()
            : base()
        { }

        protected override DetailUnitModel initModel()
        {
            updateGroudY(true);
            return new BallistaModel(this);
        }
    }

    class BallistaModel : SoldierUnitAdvancedModel
    {
        WarmashineWorkerCollection workers;
        int loadedFrame = 3;

        public BallistaModel(AbsSoldierUnit soldier)
           : base(soldier)
        {
            const float Xdiff = 0.2f;
            const float Zdiff = -0.37f;

            workers = new WarmashineWorkerCollection();

            workers.Add(soldier.GetFaction(),
                soldier.data.modelScale * Xdiff, soldier.data.modelScale * Zdiff);
            workers.Add(soldier.GetFaction(),
                soldier.data.modelScale * -Xdiff, soldier.data.modelScale * Zdiff);
        }

        public override void onNewModel(VoxelModelName name, VoxelModel master, AbsDetailUnit unit)
        {
            base.onNewModel(name, master, unit);

            workers.onNewModel_asynch(name, master);
        }

        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            updateAnimationBoth(soldier);
        }
        //protected override void updateShipAnimation(AbsSoldierUnit soldier)
        //{
        //    updateAnimationBoth(soldier);
        //}

        void updateAnimationBoth(AbsSoldierUnit soldier)
        {
            if (soldier.state.walking)
            {
                model.Frame = loadedFrame;
            }
            else
            {
                if (soldier.attackCooldownTime.HasTime)
                {
                    float percReloadTime = soldier.attackCooldownTime.MilliSeconds / soldier.prevAttackTime;
                    model.Frame = (int)(1 + (1f - percReloadTime) * 3);
                }
                else
                {
                    model.Frame = loadedFrame;
                }
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

            workers.update(soldier);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            workers.DeleteMe();
        }
    }

    class WarmashineWorkerCollection
    {
        List<WarmashineWorker> members = new List<WarmashineWorker>(2);

        public void Add(Faction player, float xDiff, float zDiff)
        {
            members.Add(new WarmashineWorker(player, new Vector3(xDiff, 0, zDiff)));
        }

        public void update(AbsSoldierUnit parent)
        {
            foreach (var m in members)
            {
                m.update(parent);
            }
        }

        public void onNewModel_asynch(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            if (name == VoxelModelName.war_worker)
            {
                foreach (var m in members)
                {
                    m.onNewModel_asynch(name, master);
                }
            }
        }

        public void DeleteMe()
        {
            foreach (var m in members)
            {
                m.DeleteMe();
            }
        }
    }

    class WarmashineWorker
    {
        WalkingAnimation walkingAnimation = WalkingAnimation.Standard;
        Graphics.AbsVoxelObj model;
        Vector3 diff;

        public WarmashineWorker(Faction faction, Vector3 diff)
        {
            this.diff = diff;
            
            model = faction.AutoLoadModelInstance(
               LootFest.VoxelModelName.war_worker, DssConst.Men_StandardModelScale * 0.9f, true);
        }

        public void onNewModel_asynch(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            DSSWars.Faction.SetNewMaster(name, LootFest.VoxelModelName.war_worker, model, master);
        }

        public void update(AbsSoldierUnit parent)
        {
            if (parent.state.walking || parent.state.rotating)
            {
                float move = parent.walkingSpeedWithModifiers(Ref.DeltaTimeMs);
                walkingAnimation.update(move, model);
            }
            else
            {
                model.Frame = 0;
            }
            var pModel = parent.model;
            if (pModel != null)
            {
                model.Rotation = pModel.model.Rotation;
                model.position = pModel.model.Rotation.TranslateAlongAxis(diff, parent.position);
            }
        }

        public void DeleteMe()
        {
            model.DeleteMe();
        }
    }
}
