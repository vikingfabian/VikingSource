using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map;


namespace VikingEngine.LootFest.GO
{
    abstract class AbsVoxelObj : AbsUpdateObj
    {
        /* Static readonlies */
        //protected static readonly Data.TempVoxelReplacementSett TempSwordImage = new Data.TempVoxelReplacementSett(VoxelModelName.Sword1, false);
        protected static readonly Effects.BouncingBlockColors StandardDamageColors = new Effects.BouncingBlockColors(
           Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

        /* Static fields */
        static Graphics.AbsVoxelModelInstance tempInstance;

        /* Static methods */
        public static void MakeTempImage()
        {
            tempInstance = new Graphics.VoxelModelInstance(LfRef.Images.StandardModel_TempBlockAnimated);//LfRef.Images.getInstance_TempblockAnim();
        }

        /* Constants */
        const float MinShadowLayer = 3.02f;

        /* Properties */
        public override RotationQuarterion RotationQuarterion
        {
            get { return image.Rotation; }
        }
        public override float LightSourceRadius { get { return image.scale.X * 14; } }
        
        public override RotationQuarterion FireDir3D(GameObjectType weaponType)
        {
            return image.Rotation;
        }

        public override Vector3 LightSourcePosition
        {
            get
            {
                if (LightSourceFallToGround)
                {
                    Vector3 result = image.position;
                    result.Y = lightSourceY;
                    return result;
                }
                return image.position;
            }
        }        
        public override Vector2 PlanePos
        {
            get
            {
                Vector2 ret = Vector2.Zero;
                ret.X = image.position.X;
                ret.Y = image.position.Z;
                return ret;
            }
            set
            {
                image.position.X = value.X;
                image.position.Z = value.Y;
            }
        }

        public virtual Vector3 HandWeaponPosition
        {
            get { return image.position; }
        }

        public override Vector3 Position
        {
            get { return image.position; }
            set { image.position = value; }
        }
        public override float X
        {
            get { return image.position.X; }
            set { image.position.X = value; }
        }
        public override float Y
        {
            get { return image.position.Y; }
            set { image.position.Y = value; }
        }
        public override float Z
        {
            get { return image.position.Z; }
            set { image.position.Z = value; }
        }

        public virtual Effects.BouncingBlockColors DamageColors
        {
            get { return StandardDamageColors; }
        }

        public float PlanePosX
        {
            get
            {
                return image.position.X;
            }
            set
            {
                image.position.X = value;

            }
        }
        public float PlanePosY
        {
            get
            {
                return image.position.Z;
            }
            set
            {
                image.position.Z = value;

            }
        }
        public bool Visible
        {
            get
            {
                return image.Visible;
            }
            set
            {
                image.Visible = value;
            }
        }

        protected virtual bool ClientPhysics { get { return false; } }
        protected virtual bool autoMoveImage { get { return true; } }
        protected virtual bool autoDeleteImage { get { return true; } }
        protected virtual bool LightSourceFallToGround { get { return true; } }

        /* Member fields */
        public Graphics.AbsVoxelObj image = tempInstance;
        public AnimationsSettings animSettings = AnimationsSettings.OneFrame;

        //public SubLevel subLevel = null;

        protected bool Collide = false; 
        protected Time immortalityTime = Time.Zero;
        protected float lightSourceY;

        /* C-tors */
        public AbsVoxelObj(GoArgs args)
            : base(args)
        {
            if (args.LocalMember)
            {
                addPhysics();
                addLevelCollider();
            }
            else
            {
                if (ClientPhysics)
                    addPhysics();
            }
        }

        override public Graphics.AbsVoxelObj getModel()
        {
            return image;
        }
        
        /* Member methods */
        public override void Time_LasyUpdate(ref float time)
        {
            
        }
        public override void Time_Update(UpdateArgs args)
        {
            if (autoMoveImage && (localMember || !NetworkShareSettings.Update))
            {
                if (!Velocity.ZeroPlaneSpeed && !(physics != null && physics.PhysicsStatusFalling))
                {
                    oldVelocity = Velocity;
                    moveImage(Velocity, args.time);
                }
            }

            UpdateBound();
            base.Time_Update(args);
        }
        public override void setImageDirFromRotation()
        {
            if (image != null)
            {
                Map.WorldPosition.Rotation1DToQuaterion(image, rotation.Radians + adjustRotation);
            }
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (autoDeleteImage)
                image.DeleteMe();
            if (PlatformSettings.ViewCollisionBounds)
            {
                lib.SafeDelete(CollisionAndDefaultBound);
                lib.SafeDelete(TerrainInteractBound);
                lib.SafeDelete(DamageBound);

            }
        }
        public override void setImageDirFromSpeed()
        {
            if (!Velocity.ZeroPlaneSpeed)
            { //set angle
                rotation.Radians = Velocity.Radians();//lib.V2ToAngle(Speed);
                setImageDirFromRotation();
            }
        }
        public override void clientStartPosition()
        {
            base.clientStartPosition();
            if (image.position.X == 0)
                image.position = WorldPos.PositionV3;
        }
        public override bool VisibleInCam(int camIx)
        {
            return image.VisibleInCamera(camIx);
        }
        public override bool UpdateWorldPos()
        {
            if (base.UpdateWorldPos())
            {
                refreshLightY();
                return true;
            }
            return false;
        }

        protected void refreshLightY()
        {
            if (WorldPos.CorrectPos)
            {
                var c = WorldPos.Screen;
                if (c.openstatus >= Map.ScreenOpenStatus.Detail_2)
                {
                    lightSourceY = this.WorldPos.Screen.GetFirstBlockBelow(WorldPos);
                }
                else
                {
                    lightSourceY = WorldPos.WorldGrindex.Y;
                }
            }
        }

        public virtual void SetStartY()
        {
            image.position.Y = LfRef.chunks.GetHighestYpos(WorldPos) + 1;
        }

        public void RotateImage(float add)
        {
            Vector3 rot = Vector3.Zero;
            rot.X = add;
            image.Rotation.RotateWorld(rot);
        }
        public void SetRotation(float angle)
        {
            rotation.Radians = angle;
            setImageDirFromRotation();
        }
        public void UnthreadDeleteMe()
        {
            new Timer.UnthreadedDelete(this);
        }

        public override void BlockSplatter()
        {
            //int numDummies = 1;
            int numBlocks = 3;
            
            if (Alive)
            {
                new Effects.DamageFlash(image, immortalityTime.MilliSeconds);
            }
            else
            {
                numBlocks = 40;
                Music.SoundManager.PlaySound(LoadedSound.deathpop, image.position);
            }

            Effects.EffectLib.DamageBlocks(numBlocks, image, DamageColors);
        }

        protected virtual void moveImage(Velocity speed, float time)
        {
            speed.Update(time, image);
        }
        protected virtual void onHitCharacter(AbsUpdateObj character)
        { }

        protected bool CheckOutSideWorldsBounds()
        {
            bool collision = false;

            const int MaxPos = Map.WorldPosition.WorldBlocksWidth - Map.WorldPosition.WorldEdgeSafetyDistance;

            if (image.position.X < Map.WorldPosition.WorldEdgeSafetyDistance)
            {
                image.position.X = Map.WorldPosition.WorldEdgeSafetyDistance;
                collision = true;
            }
            else if (image.position.X > MaxPos)
            {
                image.position.X = MaxPos;
                collision = true;
            }

            if (image.position.Z < Map.WorldPosition.WorldEdgeSafetyDistance)
            {
                image.position.Z = Map.WorldPosition.WorldEdgeSafetyDistance;
                collision = true;
            }
            else if (image.position.Z > MaxPos)
            {
                image.position.Z = MaxPos;
                collision = true;
            }
            return collision;
        }
        protected void moveTowardsObject2(AbsUpdateObj otherObj, float speed)
        {
            Vector3 diff = PositionDiff3D(otherObj);
            float l = diff.Length();
            
            float moveDist = speed * VikingEngine.Ref.DeltaTimeMs;
            if (l < moveDist)
            {
                image.position = otherObj.Position;
            }
            else
            {
                diff.Normalize();
                image.position += diff * moveDist;
            }
        }
        //protected void ImageSetup(Vector3 position, VoxelModelName image, Data.IReplacementImage replaceMent, float scale)
        //{
        //    ImageSetup(position, Graphics.AnimationsSettings.OneFrame, image, replaceMent, scale, 0.16f);
        //}
        //protected void ImageSetup(Vector3 position, Graphics.AnimationsSettings animSettings,
        //    VoxelModelName imageName, Data.IReplacementImage replaceMent, float scale, float scaleToBound)
        //{
        //    image = LfRef.modelLoad.AutoLoadModelInstance(imageName, replaceMent, scale, 1, false, animSettings);

        //    if (scaleToBound > 0)
        //        CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(scaleToBound * scale);
        //    image.Position = position;
        //}
        protected void SolidBodyCheck(ISpottedArrayCounter<AbsUpdateObj> active)
        {
            active.Reset();
            while (active.Next())
            {
                if (active.GetSelection.SolidBody && active.GetSelection != this)
                {
                    if (LootFest.AbsObjBound.SolidBodyIntersect(this, active.GetSelection))
                        return;
                }
            }
        }
        protected GoArgs LootDropPos()
        {
            Vector3 pos = image.position;
            pos.Y += ExspectedHeight * Ref.rnd.Float(0.4f, 1.2f);
            pos.X += Ref.rnd.Plus_MinusF(1f);
            pos.Z += Ref.rnd.Plus_MinusF(1f);
            return new GoArgs(pos, characterLevel);
        }

        public override float Scale1D
        {
            get
            {
                if (modelScale > 0)
                {
                    return modelScale;
                }
                return image.Scale1D;
            }
        }
        public override void sleep(bool setToSleep)
        {
            image.Visible = !setToSleep;
            base.sleep(setToSleep);
        }
    }
}
