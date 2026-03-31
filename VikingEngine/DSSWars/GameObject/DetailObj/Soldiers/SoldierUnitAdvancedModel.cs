using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.Sound;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    class DetailUnitModel : IDeleteable
    {
        public Graphics.AbsVoxelObj model;

        virtual public void DeleteMe()
        {
            model.DeleteMe();
        }

        virtual public void onNewModel(LootFest.VoxelModelName name,
            Graphics.VoxelModel master, AbsDetailUnit unit)
        {
            DSSWars.Faction.SetNewMaster(name, unit.soldierData.modelName, model, master);
        }

        virtual public void update(AbsSoldierUnit soldier)
        {
        
        }

        virtual public void RotateVector(Vector3 forward, ref Vector3 pos)
        {
            pos = model.Rotation.TranslateAlongAxis(forward, model.position);
        }

        virtual public void displayHealth(float percHealth)
        { }

        virtual public SoldierUnitAdvancedModel Adv() 
        {
            return null;
        }

        public bool IsDeleted
        {
            get { return model.IsDeleted; }
        }
    }

    abstract class AbsDetailUnitAdvancedModel : DetailUnitModel
    {
        protected WalkSoundType walkSound;
        
        protected Graphics.Mesh shadowPlane;
        protected Vector3 shadowOffset = new Vector3(-0.005f, 0, -0.0058f);
        public Circle selectionArea;
        override public void update(AbsSoldierUnit soldier)
        {
            model.position = soldier.position;

            if (shadowPlane != null)
            {
                shadowPlane.Position = model.position + shadowOffset;
                shadowPlane.Rotation = model.Rotation;
            }

            selectionArea.Center = soldier.posXZ();//bound.Center;
            selectionArea.Center.Y -= 0.5f;
        }

        public AbsDetailUnitAdvancedModel()
        { }

        public AbsDetailUnitAdvancedModel(AbsSoldierUnit soldier)
        {
            if (soldier.soldierData.factionColoredModel)
            {
                var faction = soldier.GetFaction_NoChecks();

                if (soldier.soldierData.modelData.modelType == ModelType.Soldier)
                {
                    model = faction.AutoLoadModelInstance_character(
                        soldier.soldierData.modelData, soldier.soldierData.modelScale * faction.player.profile.character.soldierScale);
                }
                else
                {
                    model = faction.AutoLoadModelInstance_batched(
                        soldier.soldierData.RandomModelName(), soldier.soldierData.modelScale);
                }
            }
            else
            {
                model = DssRef.models.ModelInstance_drawbatch(soldier.soldierData.modelName, soldier.soldierData.modelScale);
            }
            model.position = soldier.position;

            createShadow(soldier);

            selectionArea = new Circle(Vector2.Zero, 1.2f);
        }

        protected AnimalNoiseType animalNoiseType = AnimalNoiseType.NUM_NONE;
        protected static readonly IntervalF AnimalNoiseFrequecy = new IntervalF(5, 40);
        protected TimeInGameCountdown nextAnimalNoise;
        protected void resetAnimlNoise()
        {
            nextAnimalNoise.start(AnimalNoiseFrequecy);
        }

        protected void updateAnimalNoise()
        {
            if (nextAnimalNoise.TimeOut())
            {
                if (Ref.peRnd.Chance(0.5) && animalNoiseType != AnimalNoiseType.NUM_NONE && SoundStackManager.RareAvailable())
                {
                    SoundLib.AnimalNoises[(int)animalNoiseType].Play(model.position);
                }
                resetAnimlNoise();
            }
        }

        protected void createShadow(AbsSoldierUnit soldier)
        {
            if (!Ref.gamesett.modelShadow)
            {
                shadowPlane = new Graphics.Mesh(LoadedMesh.plane, soldier.position,
                     soldier.soldierData.ShadowModelScale(), Graphics.TextureEffectType.Flat,
                     SpriteName.LittleUnitShadow, Color.Black);
                shadowPlane.Opacity = 0.5f;
            }
        }

        override public void DeleteMe()
        {
            //base.DeleteMe();
            model?.preRemoveFromDrawBatch();
            shadowPlane?.DeleteMe();
        }
    }

    
    class SoldierUnitAdvancedModel: AbsDetailUnitAdvancedModel
    {
        
        protected WalkingAnimation walkingAnimation;
        
        Rotation1D moveJiggle = Rotation1D.Random();
        
        bool inBlinkFrame = true;
        Time blinkTimer;

        public SoldierUnitAdvancedModel()
        { }

        public SoldierUnitAdvancedModel(AbsSoldierUnit soldier)
            :base(soldier)
        {
            walkingAnimation = WalkingAnimation.Standard;
        }

        

        override public void update(AbsSoldierUnit soldier)
        {
            base.update(soldier);
            updateAnimation(soldier);
        }

        virtual protected void updateAnimation(AbsSoldierUnit soldier)
        {
            if (soldier.state.walking)
            {
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);

                moveJiggle.Add(move * 50f);
                walkingAnimation.update(move, model, out bool enterEvenFrame);
                if (enterEvenFrame && Ref.peRnd.ChanceF_Low(0.04f))
                {
                    SoundLib.footstep.Play(model.position);
                }

                float jiggleAdd = 0f;
                if (soldier.SoldierProfile().walkingWaggleAngle > 0)
                {
                    jiggleAdd = moveJiggle.Direction(soldier.SoldierProfile().walkingWaggleAngle).X;
                }
                WP.Rotation1DToQuaterion(model, soldier.rotation.Radians + jiggleAdd);


                if (Ref.peRnd.Chance(0.5 / Ref.UpdateTimes60FPS))
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Dust, Ref.peRnd.Vector3_SqXZ( soldier.position, 0.02f));
                    
                }
            }
            else
            {
                if (soldier.inAttackAnimation())
                {
                    model.Frame =CharacterModelBuilder.AttackFrame;
                }
                else
                {
                    if (blinkTimer.CountDownGameTime())
                    {
                        lib.Invert(ref inBlinkFrame);
                        if (inBlinkFrame)
                        {
                            blinkTimer.MilliSeconds = 200;
                        }
                        else
                        {
                            blinkTimer.MilliSeconds = Ref.rnd.Float(600, 10000);
                        }
                    }


                    model.Frame = inBlinkFrame ? CharacterModelBuilder.IdleBlinkFrame : CharacterModelBuilder.IdleFrame;
                }

                WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);
            }
        }

        public override SoldierUnitAdvancedModel Adv()
        {
            return this;
        }
    }
}
