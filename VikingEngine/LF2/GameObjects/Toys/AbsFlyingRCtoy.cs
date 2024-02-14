//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    abstract class AbsFlyingRCtoy : AbsRC
//    {
//        protected Voxels.Voxel firstBlockBelow;
//        protected Vector3 dir;
//        public Vector3 Dir { get { return dir; } }
//        Graphics.Mesh helpLine;
//        const float MaxYpos = Map.WorldPosition.ChunkHeight + 20;
//        const float HelpLinePosAdj = -20;
//        public float BehindPos = 8;

//        public AbsFlyingRCtoy(Players.Player player)
//            :base(player)
//        {

//        }
//        public AbsFlyingRCtoy(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }
//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);
//            if (localMember)
//            {
//                physics.ObsticleCollision();
//                updateFlyingToy();
//            }
//            else
//            {
                
//                const float RotationAjdSpeed = 0.1f;

//                Vector3 rot = image.Rotation.RotationV3;
//                if (rot.X < 0)
//                    rot.X += MathHelper.TwoPi;
//                if (rot.Y < 0)
//                    rot.Y += MathHelper.TwoPi;
//                if (rot.Z < 0)
//                    rot.Z += MathHelper.TwoPi;

//                rot.X += Rotation1D.AngleDifference(rot.X, goalRotation.X) * RotationAjdSpeed;
//                rot.Y += Rotation1D.AngleDifference(rot.Y, goalRotation.Y) * RotationAjdSpeed;
//                rot.Z += Rotation1D.AngleDifference(rot.Z, goalRotation.Z) * RotationAjdSpeed;
//                image.Rotation.RotationV3 = rot;
//            }
            
            
//        }
//        //public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
//        //{
//        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
//        //    const float RotationAjdSpeed = 0.1f;

//        //    Vector3 rot = image.Rotation.RotationV3;
//        //    if (rot.X < 0)
//        //        rot.X += MathHelper.TwoPi;
//        //    if (rot.Y < 0)
//        //        rot.Y += MathHelper.TwoPi;
//        //    if (rot.Z < 0)
//        //        rot.Z += MathHelper.TwoPi;

//        //    rot.X += Rotation1D.AngleDifference(rot.X, goalRotation.X) * RotationAjdSpeed;
//        //    rot.Y += Rotation1D.AngleDifference(rot.Y, goalRotation.Y) * RotationAjdSpeed;
//        //    rot.Z += Rotation1D.AngleDifference(rot.Z, goalRotation.Z) * RotationAjdSpeed;
//        //    image.Rotation.RotationV3 = rot;
//        //}
//        Vector3 goalRotation;
//        protected override ByteVector3 LowResRotation3d
//        {
//            get
//            {
//                return new Rotation3D(image.Rotation.RotationV3).LowRes;
//            }
//            set
//            {
//                //image.Rotation.RotationV3
//                goalRotation = new Vector3(
//                    Rotation1D.ByteToRadians(value.X), Rotation1D.ByteToRadians(value.Y), Rotation1D.ByteToRadians(value.Z));
//            }
//        }

//        protected void updateFlyingToy()
//        {
//            firstBlockBelow = LfRef.chunks.GetScreen(WorldPosition.ChunkGrindex).GetFirstBlockBelow(WorldPosition);
//            if (image.Position.Y - firstBlockBelow.Position.Y < 10)
//            {
//                //terrain effect
//                switch ((Data.MaterialType)firstBlockBelow.Material)
//                {
//                    case Data.MaterialType.iron:
//                        if (fireTime > wepFireRate)
//                        {
//                            fireTime = wepFireRate;
//                        }
//                        break;
//                    case Data.MaterialType.white:
//                        HealUp();
//                        break;
//                    case Data.MaterialType.gold:
//                        //boost
//                        boostUp();
//                        break;
//                }
//            }

//            if (image.Position.Y > MaxYpos)
//            { image.Position.Y = MaxYpos; }

//            helpLine.Position = image.Position;
//            helpLine.Y += HelpLinePosAdj;
//        }
        
//        protected void collisionDamage(Vector3 posDiff)
//        {
//            float length = posDiff.Length();
//            if (length > 0.2)
//            {
//                TakeDamage(new WeaponAttack.DamageData(length * 1f), true);
//            }
//        }
//        protected override Vector3 ProjectileStartDir
//        {
//            get
//            {
//                return image.Rotation.TranslateAlongAxis(Vector3.Backward, Vector3.Zero);
//            }
//        }
//        protected override void basicInit()
//        {
//            base.basicInit();
//            helpLine = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, new Graphics.TextureEffect(TextureEffectType.Flat, TileName.WhiteArea), 0.12f);
//            helpLine.ScaleY = MaxYpos;
//            helpLine.Transparentsy = 0.4f;
//            UpdateShowHelpLine();
//        }
//        public void UpdateShowHelpLine()
//        {
//#if CMODE
//            helpLine.Visible = LfRef.gamestate.LocalHostingPlayer.settings.FlyingRCHelpLine;
//#endif
//        }
//        public override ObjPhysicsType PhysicsType
//        {
//            get
//            {
//                return ObjPhysicsType.FlyingObj;
//            }
//        }
//        protected override NetworkClientRotationUpdateType NetRotationType
//        {
//            get
//            {
//                return NetworkClientRotationUpdateType.Full3D;
//            }
//        }
//        public override void DeleteMe(bool local)
//        {
//            helpLine.DeleteMe();
//            base.DeleteMe(local);
//        }
//        override public bool FlyingToy { get { return true; } }

//        public override void HandleColl3D(Game1.Physics.Bound3dIntersect collData, GameObjects.AbsUpdateObj ObjCollision)
//        {
//            base.HandleColl3D(collData, ObjCollision);
//        }

//        //virtual public void Collide(TerrainColl collSata, Vector3 oldPos)
//        //{
//        //}
//    }
    
//}
