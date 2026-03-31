using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.ToGG.MoonFall.GO;

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
            AnimalProfile modelData = AnimalModel(soldier.group.soldierConscript.conscript.animal);
            walkSound = modelData.walkSoundType;
            animalNoiseType = modelData.noiseType;
            animalmodel = DssRef.models.ModelInstance_drawbatch(modelData.modelName, modelData.scale);
            riderY = modelData.riderY;
            

            walkingAnimation = modelData.animation;
            var soldierProfile = soldier.Profile();
            walkingAnimation.attackframe = CharacterModelBuilder.AttackFrame;
            walkingAnimation.idleframe = CharacterModelBuilder.IdleFrame;
            walkingAnimation.idleblinkframe = CharacterModelBuilder.IdleBlinkFrame;

            resetAnimlNoise();
        }

        public static AnimalProfile AnimalModel(Resource.ItemResourceType animal/*, out float riderY, out float wagonPullDistance*/)
        {
            AnimalProfile modelData;
            switch (animal)
            {
                case Resource.ItemResourceType.Pony:
                    modelData = DssVar.ponyModel;
                    break;

                default:
                    modelData = DssVar.horseModel;
                    if (Ref.rnd.Chance(0.2))
                    {
                        modelData.modelName = VoxelModelName.horse_white;
                    }
                    //modelName = Ref.rnd.Chance(0.2) ? VoxelModelName.horse_white : VoxelModelName.horse_brown;
                    //modelScale = DssConst.Men_StandardModelScale * 1.1f;
                    //walkingAnimation = HorseAnimation;//new WalkingAnimation(1, 6, WalkingAnimation.StandardMoveFrames * 1f);
                    //riderY = 0.018f;
                    break;

                case Resource.ItemResourceType.WarHorse:
                    modelData = DssVar.warHorseModel;
                    break;

                case Resource.ItemResourceType.DraftHorse:
                    modelData = DssVar.draftHorseModel;
                    break;

                case Resource.ItemResourceType.Oxen:
                    modelData = DssVar.oxenModel;
                    break;
                case Resource.ItemResourceType.KineOxen:
                    modelData = DssVar.kineOxenModel;
                    break;

                case Resource.ItemResourceType.WildPig:
                case Resource.ItemResourceType.WildHog:
                case Resource.ItemResourceType.WarHog:
                case Resource.ItemResourceType.StagHog:
                    modelData = DssVar.hogModel;
                    //wagonPullDistance = 0.2f;
                    //modelName = VoxelModelName.hog1;
                    //modelScale = HogScale;
                    //walkingAnimation = HogAnimation;//new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1f);
                    //riderY = 0.013f;
                    break;

                case Resource.ItemResourceType.Wolf:
                    modelData = DssVar.wolfModel;
                    break;

                case Resource.ItemResourceType.Warg:
                    modelData = DssVar.wargModel;
                    break;

                case Resource.ItemResourceType.AlphaWarg:
                    modelData = DssVar.alphaWargModel;
                    break;
                    //modelName = VoxelModelName.wolf1;
                    //modelScale = WolfScale;
                    //walkingAnimation = WolfAnimation;//new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f);
                    //riderY = 0.018f;
                    //break;

                case Resource.ItemResourceType.WildCat:
                case Resource.ItemResourceType.Lion:
                case Resource.ItemResourceType.WarLion:
                    modelData = DssVar.lionModel;
                    //modelName = VoxelModelName.lion1;
                    //modelScale = LionScale;
                    //walkingAnimation = LionAnimation;// new WalkingAnimation(2, 6, WalkingAnimation.StandardMoveFrames * 0.9f);
                    //riderY = 0.018f;
                    break;

                case Resource.ItemResourceType.Elephant:
                    modelData = DssVar.elephantModel;
                    break;

                case Resource.ItemResourceType.WarElephant:
                    modelData = DssVar.warElephantModel;
                    break;

                case Resource.ItemResourceType.Oliphant:
                    modelData = DssVar.oliphantModel;
                    break;
                    //modelScale = ElephantScale;
                    //modelName = VoxelModelName.Elephant_default;
                    //walkingAnimation = ElephantAnimation;//new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames * 2);
                    //riderY = 0.38f * modelScale;
                    //break;
            }
            return modelData;
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

                walkingAnimation.update(move, animalmodel, out bool enterEvenFrame);
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
                animalmodel.Frame = 0;
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);
            
            animalmodel.position = model.position;
            animalmodel.Rotation = model.Rotation;

            updateAnimalNoise();
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
