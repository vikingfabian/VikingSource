﻿using Microsoft.Xna.Framework;
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
            DSSWars.Faction.SetNewMaster(name, unit.Data().modelName, model, master);
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
            model = soldier.group.army.faction.AutoLoadModelInstance(
                soldier.data.RandomModelName(), soldier.data.modelScale, true);

            model.position = soldier.position;

            shadowPlane = new Graphics.Mesh(LoadedMesh.plane, soldier.position,
                 soldier.data.ShadowModelScale(), Graphics.TextureEffectType.Flat,
                 SpriteName.LittleUnitShadow, Color.Black);

            shadowPlane.Opacity = 0.5f;

            bound = new Physics.CircleBound(Vector2.Zero, soldier.data.boundRadius);
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
        //Graphics.Mesh ship = null;
        
        

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

            //model = soldier.group.army.faction.AutoLoadModelInstance(
            //    soldier.data.RandomModelName(), soldier.data.modelScale,  true);

            //model.position = soldier.position;

            //shadowPlane = new Graphics.Mesh(LoadedMesh.plane, soldier.position,
            //     soldier.data.ShadowModelScale(), Graphics.TextureEffectType.Flat,
            //     SpriteName.LittleUnitShadow, Color.Black);
            
            //shadowPlane.Opacity = 0.5f;

            //bound = new Physics.CircleBound(Vector2.Zero, soldier.data.boundRadius);
            //selectionArea = new Circle(Vector2.Zero, 1.2f);

            //if (soldier.group.isShip)
            //{
            //    setShip(true);
            //}
        }

        override public void update(AbsSoldierUnit soldier)
        {
            base.update(soldier);
            //model.position = soldier.position;

            //shadowPlane.Position = model.position + shadowOffset;
            //shadowPlane.Rotation = model.Rotation;

            //bound.Center = VectorExt.V3XZtoV2(model.position);
            //selectionArea.Center = bound.Center;
            //selectionArea.Center.Y -= 0.5f;

            //if (ship == null)
            //{
                updateAnimation(soldier);
            //}
            //else
            //{
            //    ship.Position = model.position;
            //    ship.Rotation = model.Rotation;

            //    updateShipAnimation(soldier);
            //}
        }



        //virtual protected void updateShipAnimation(AbsSoldierUnit soldier)
        //{
        //    WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

        //    if (soldier.inAttackAnimation())
        //    {
        //        model.Frame = soldier.data.attackFrame;
        //    }
        //    else
        //    {
        //        model.Frame = soldier.data.idleFrame;
        //    }
        //}

        virtual protected void updateAnimation(AbsSoldierUnit soldier)
        {
            if (soldier.state.walking)
            {
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);

                moveJiggle.Add(move * 50f);
                walkingAnimation.update(move, model);

                float jiggleAdd = 0f;
                if (soldier.data.walkingWaggleAngle > 0)
                {
                    jiggleAdd = moveJiggle.Direction(soldier.data.walkingWaggleAngle).X;
                }
                WP.Rotation1DToQuaterion(model, soldier.rotation.Radians + jiggleAdd);
            }
            else
            {
                if (soldier.inAttackAnimation())
                {
                    model.Frame =soldier.data.attackFrame;
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


                    model.Frame = inBlinkFrame ? soldier.data.idleBlinkFrame : soldier.data.idleFrame;
                }

                WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);
            }
        }

        //virtual public void setShip(bool hasShip)
        //{
        //    if (hasShip)
        //    {
        //        if (ship == null)
        //        {
        //            ship = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero,
        //                new Vector3(1f, 0.5f, 2f) * AbsDetailUnitData.StandardModelScale,
        //                Graphics.TextureEffectType.Flat,
        //                SpriteName.WhiteArea, Color.Brown, false);
        //            ship.AddToRender(DrawGame.UnitDetailLayer);
        //        }
        //    }
        //    else
        //    {
        //        ship?.DeleteMe();
        //    }
        //}

        public override SoldierUnitAdvancedModel Adv()
        {
            return this;
        }

        //override public void DeleteMe()
        //{
        //    base.DeleteMe();
        //    ship?.DeleteMe();
        //}
    }
}
