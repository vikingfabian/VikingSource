using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.ToGG.Commander.GO;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class BalkongBuilder : ConscriptedSoldierBuilder
    {
        public BalkongBuilder()
            : base()
        {
            unitBuildType = UnitBuildType.ConscriptBalkong;

            targetSpotRange = StandardTargetSpotRange;

            goldCost = MathExt.MultiplyInt(2, DssLib.GroupDefaultCost);

            modelAdjY = 0.1f;
        }

        public override AbsSoldierUnit CreateUnit(bool bannerman)
        {
            return new BalkongSoldier();
        }
    }

    class BalkongSoldier : BaseSoldier
    {
        public BalkongSoldier()
            : base()
        {

        }

        protected override DetailUnitModel initModel(bool bannerman)
        {
            updateGroudY(true);
            if (bannerman)
            {
                return new BalkongBannerModel(this);
            }
            else
            {
                return new BalkongModel(this);
            }
        }
    }

    class BalkongModel : SoldierUnitAdvancedModel
    {
        Graphics.VoxelModelInstance soldier1, soldier2;
        Vector3 Soldier1PosDiff, Soldier2PosDiff;
        protected float riderY;
        public BalkongModel(AbsSoldierUnit soldier, bool firstupdate = true)
        {
            AnimalProfile modelData = CavalryModel.AnimalModel(soldier.group.soldierConscript.conscript.animal);

            walkSound = modelData.walkSoundType;
            animalNoiseType = modelData.noiseType;
            riderY = modelData.riderY;

            model = ElephantModelBuilder.GetInstance(new ElephantModelData(soldier.group.soldierConscript.conscript), modelData.scale);

            walkingAnimation = modelData.animation;
            walkingAnimation.attackframe = CharacterModelBuilder.AttackFrame;
            walkingAnimation.idleframe = 0;
            walkingAnimation.idleblinkframe = 0;

            resetAnimalNoise();

            var faction = soldier.GetFaction_NoChecks();

            Soldier1PosDiff = new Vector3(0.02f, 0, -0.04f) * modelData.scale;
            Soldier1PosDiff.Y += modelData.riderY;
            soldier1 = createSoldier();
            soldier2 = createSoldier();

            Soldier2PosDiff = Soldier1PosDiff;
            Soldier2PosDiff.Z -= 0.16f * modelData.scale;

            if (firstupdate)
            {
                update(soldier);
            }

            VoxelModelInstance_Pooled createSoldier()
            {
                return faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, DssConst.Men_ModCharacterScale * faction.player.profile.character.soldierScale);
            }
        }

        public override void update(AbsSoldierUnit soldier)
        {
            const float Rotation = 0.2f;

            base.update(soldier);
            soldier1.position = model.Rotation.TranslateAlongAxis(Soldier1PosDiff, model.position);
            soldier1.Rotation = model.Rotation;
            soldier1.Rotation.RotateWorldX(Rotation);

            soldier2.position = model.Rotation.TranslateAlongAxis(Soldier2PosDiff, model.position);
            soldier2.Rotation = model.Rotation;
            soldier2.Rotation.RotateWorldX(-Rotation);


        }

        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            if (soldier.inAttackAnimation())
            {
                soldier1.Frame = walkingAnimation.attackframe;
            }
            else
            {
                soldier1.Frame = walkingAnimation.idleframe;
            }

            if (soldier.state.walking)
            {
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);

                walkingAnimation.update(move, model, out bool enterEvenFrame);
                if (enterEvenFrame && Ref.peRnd.ChanceF(0.1f))
                {
                    SoundLib.WalkSounds[(int)walkSound].Play(model.position);
                }

                if (Ref.peRnd.Chance(0.5 / Ref.UpdateTimes60FPS))
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Dust, Ref.peRnd.Vector3_SqXZ(model.position, 0.02f));
                }
            }
            else
            {
                model.Frame = 0;
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

            updateAnimalNoise();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            soldier1.preRemoveFromDrawBatch();
            soldier2.preRemoveFromDrawBatch();
        }
    }


    class BalkongBannerModel : BalkongModel
    {
        HorseBanner banner;

        public BalkongBannerModel(AbsSoldierUnit soldier)
            : base(soldier, false)
        {
            banner = new HorseBanner(soldier.GetFaction(), soldier.soldierData.modelScale, riderY);
            update(soldier);
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
}
