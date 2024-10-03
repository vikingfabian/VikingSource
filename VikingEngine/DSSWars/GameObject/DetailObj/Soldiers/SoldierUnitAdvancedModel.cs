using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.DSSWars.GameObject
{
    class DetailUnitModel : IDeleteable
    {
        public Graphics.AbsVoxelObj model;
        public Physics.AbsBound2D bound;

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
        protected Graphics.Mesh shadowPlane;
        protected Vector3 shadowOffset = new Vector3(-0.005f, 0, -0.0058f);
        public Circle selectionArea;
        override public void update(AbsSoldierUnit soldier)
        {
            model.position = soldier.position;

            shadowPlane.Position = model.position + shadowOffset;
            shadowPlane.Rotation = model.Rotation;

            bound.Center = VectorExt.V3XZtoV2(model.position);
            selectionArea.Center = bound.Center;
            selectionArea.Center.Y -= 0.5f;
        }

        public AbsDetailUnitAdvancedModel()
        { }

        public AbsDetailUnitAdvancedModel(AbsSoldierUnit soldier)
        {
            if (soldier.group.typeCurrentData.IsShip())
            { 
                    lib.DoNothing();
            }
            model = soldier.group.army.faction.AutoLoadModelInstance(
                soldier.soldierData.RandomModelName(), soldier.SoldierProfile().modelScale, true);

            model.position = soldier.position;

            shadowPlane = new Graphics.Mesh(LoadedMesh.plane, soldier.position,
                 soldier.SoldierProfile().ShadowModelScale(), Graphics.TextureEffectType.Flat,
                 SpriteName.LittleUnitShadow, Color.Black);

            shadowPlane.Opacity = 0.5f;

            bound = new Physics.CircleBound(Vector2.Zero, soldier.SoldierProfile().boundRadius);
            selectionArea = new Circle(Vector2.Zero, 1.2f);

        }

        override public void DeleteMe()
        {
            base.DeleteMe();
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
                walkingAnimation.update(move, model);

                float jiggleAdd = 0f;
                if (soldier.SoldierProfile().walkingWaggleAngle > 0)
                {
                    jiggleAdd = moveJiggle.Direction(soldier.SoldierProfile().walkingWaggleAngle).X;
                }
                WP.Rotation1DToQuaterion(model, soldier.rotation.Radians + jiggleAdd);
            }
            else
            {
                if (soldier.inAttackAnimation())
                {
                    model.Frame =soldier.SoldierProfile().attackFrame;
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


                    model.Frame = inBlinkFrame ? soldier.SoldierProfile().idleBlinkFrame : soldier.SoldierProfile().idleFrame;
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
