using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects
{
    abstract class AbsVoxelObj : AbsUpdateObj
    {
        protected static readonly Data.TempVoxelReplacementSett TempSwordImage = new Data.TempVoxelReplacementSett(VoxelModelName.Sword1, false);

        static Graphics.AbsVoxelModelInstance tempInstance;
        public static void MakeTempImage()
        {
            tempInstance = LootfestLib.Images.StandardAnimObjInstance(VoxelModelName.temp_block_animated, new Graphics.AnimationsSettings(3, 0.1f));
        }

        protected Graphics.AbsVoxelObj image = tempInstance;
        protected bool Collide = false;

        public RotationQuarterion RotationQuarterion
        {
            get { return image.Rotation; }
        }

        public AbsVoxelObj(System.IO.BinaryReader r)
            : base(r)
        { 
             if (ClientPhysics)
                addPhysics();
        }

        public AbsVoxelObj()
            :base()
        {
            addPhysics();
        }

        virtual protected bool ClientPhysics { get { return false; } }
        
        protected void ImageSetup(Vector3 position, VoxelModelName image, Data.IReplacementImage replaceMent, float scale)
        {
            ImageSetup(position, Graphics.AnimationsSettings.OneFrame, image, replaceMent, scale, 0.16f);
        }
        protected void ImageSetup(Vector3 position, Graphics.AnimationsSettings animSettings,
            VoxelModelName imageName, Data.IReplacementImage replaceMent, float scale, float scaleToBound)
        {
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(imageName, replaceMent, scale, 1, animSettings);
            
            if (scaleToBound > 0)
                CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(scaleToBound * scale);
            image.position = position;

           
        }
        protected static readonly Effects.BouncingBlockColors StandardDamageColors = new Effects.BouncingBlockColors(
            Data.MaterialType.red, Data.MaterialType.red_brown, Data.MaterialType.orange);
        virtual public Effects.BouncingBlockColors DamageColors
        {
            get { return StandardDamageColors; }
        }
        override protected void BlockSplatter()
        {
            
            int numDummies = 1;
            int numBlocks = 2;
            
            if (!Alive)
            {
                numDummies = 20;
                numBlocks = 20;
                Music.SoundManager.PlaySound(LoadedSound.deathpop, image.position);
            }
            if (Engine.Update.IsRunningSlow)
            {
                numBlocks = 0;
            }
            

            Vector3 pos = image.position;
            pos.Y += image.scale.Y * 8f;
            float scale = image.scale.X * 1.6f;
            for (int i = 0; i < numBlocks; i++)
            {
                new Effects.BouncingBlock2(pos, DamageColors.GetRandom(), scale);
            }
            for (int i = 0; i < numDummies; i++)
            {
                new Effects.BouncingBlock2Dummie(pos, DamageColors.GetRandom(), scale);
            }

        }
        protected bool CheckOutSideWorldsBounds()
        {
            bool collision = false;

            const int MaxPos = Map.WorldPosition.WorldSizeX - Map.World.InvisibleWallRadius;

            if (image.position.X < Map.World.InvisibleWallRadius)
            {
                image.position.X = Map.World.InvisibleWallRadius;
                collision = true;
            }
            else if (image.position.X > MaxPos)
            {
                image.position.X = MaxPos;
                collision = true;
            }

            if (image.position.Z < Map.World.InvisibleWallRadius)
            {
                image.position.Z = Map.World.InvisibleWallRadius;
                collision = true;
            }
            else if (image.position.Z > MaxPos)
            {
                image.position.Z = MaxPos;
                collision = true;
            }
            return collision;
        }

        virtual protected void HitCharacter(AbsUpdateObj character)
        { }
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
        override public Vector2 PlanePos
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


        override public Vector3 Position
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

        virtual protected bool autoMoveImage { get { return true; } }

        const float MinShadowLayer = 3.02f;
        static float shadowLayer = MinShadowLayer;

        public bool Visible
        {
            set
            {
                image.Visible = value;
            }
        }

        override public void setImageDirFromRotation()
        {
            if (image != null)
            {
                Map.WorldPosition.Rotation1DToQuaterion(image, rotation.Radians + adjustRotation);
            }
        }

        public void RotateImage(float add)
        {
            Vector3 rot = Vector3.Zero;
            rot.X = add;
            image.Rotation.RotateWorld(rot);
        }
        virtual protected void moveImage(Velocity speed, float time)
        {
            //image.Position.X += speed.X * time;
            //image.Position.Z += speed.Y * time;
            speed.Update(time, image);

            //Speed.SetZeroPlaneSpeed();

        }

        
        
        virtual public void SetStartY()
        {
            image.position.Y = LfRef.chunks.GetHighestYpos(WorldPosition) + 1;
        }
        override protected void setImageDirFromSpeed()
        {
            if (!Velocity.ZeroPlaneSpeed)
            { //set angle
                rotation.Radians = Velocity.Radians();//lib.V2ToAngle(Speed);
                setImageDirFromRotation();
            }
        }

        public void SetRotation(float angle)
        {
            rotation.Radians = angle;
            setImageDirFromRotation();
        }
        protected void SolidBodyCheck(ISpottedArrayCounter<AbsUpdateObj> active)
        {
            active.Reset();
            while (active.Next())
            {
                if (active.GetMember.SolidBody && active.GetMember != this)
                {
                    if (LF2.AbsObjBound.SolidBodyIntersect(this, active.GetMember))
                        return;
                }
                
            }
        }

        virtual protected bool autoDeleteImage { get { return true; } }
        public override void DeleteMe(bool local)
        {
 	        base.DeleteMe(local);
            if (autoDeleteImage)
                image.DeleteMe();
            if (PlatformSettings.ViewCollisionBounds)
            {
                lib.SafeDelete(CollisionBound);
                lib.SafeDelete(TerrainInteractBound);
            }
        }
        public void UnthreadedDeleteMe()
        {
            new Timer.UnthreadedDelete(this);
        }

        public override void clientStartPosition()
        {
            base.clientStartPosition();
            if (image.position.X == 0)
                image.position = WorldPosition.ToV3();
        }

        public override bool VisibleInCam(int camIx)
        {
            return image.VisibleInCamera(camIx);
        }
        public override RotationQuarterion FireDir3D
        {
            get
            {
                return image.Rotation;
            }
        }

        override public Vector3 LightSourcePosition { get { return image.position; } }
        override public float LightSourceRadius { get { return image.scale.X * 14; } }

        protected Vector3 LootDropPos()
        {
            Vector3 pos = image.position;
            pos.Y += ExspectedHeight  * lib.RandomFloat(0.4f, 1.2f);
            pos.X += Ref.rnd.Plus_MinusF(1f);
            pos.Z += Ref.rnd.Plus_MinusF(1f);
            return pos;
        }

        /// <summary>
        /// cheaper version, and will avoid spasm out
        /// </summary>
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
    }
}
