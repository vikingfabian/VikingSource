//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    abstract class AbsRCVeihcle : AbsRC
//    {

//        protected TerrainSpeed badTerrain = TerrainSpeed.Normal;
//        protected float goalXtilt = 0;
//        protected float goalZtilt = 0;
//        float xtilt = 0;
//        float ztilt = 0;
//        protected float velocity = 0;
//        const float Gravity = -0.06f;

//        protected static readonly WeaponAttack.DamageData LavaDamage = new WeaponAttack.DamageData(1f);
//        protected static readonly WeaponAttack.DamageData WatereDamage = new WeaponAttack.DamageData(0.5f);


//        public AbsRCVeihcle(Characters.Hero parent)
//            :base(parent.Player)
//        {
            
//            physics.Gravity = Gravity;
            
//        }
//        public AbsRCVeihcle(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }
//        //static readonly NetworkShare NetShare = new NetworkShare(true, false, false);
//        override public NetworkShare NetworkShareSettings 
//        { 
//            get {
//                if (localMember)
//                {
//                    NetworkShare NetShare = GameObjects.NetworkShare.FullExceptClientDel;
//                    NetShare.Update = velocity != 0 || updatePositionToNewbie > 0;
//                    return NetShare;
//                }
//                else
//                {
//                    return GameObjects.NetworkShare.FullExceptClientDel;
//                }
//            } 
//        }
//        //public override bool NetUpdateable
//        //{
//        //    get
//        //    {
                
//        //    }
//        //}
//        protected override void BasicInit(Players.Player player)
//        {
//            base.BasicInit(player);
           
            
            

//        }
//        //abstract protected VoxelModelName imageName
//        //{ get; }
//        override protected void setImageDirFromSpeed()
//        {
//            //do nothing
//        }

        
//        public override void Time_Update(UpdateArgs args)
//        {
            
//            //base.Time_Update(args);
//            //physics.Update(args.time);
//            //if (physics.ObsticleCollision())
//            //{
//            //    image.Position.Y += 0.1f;
//            //}
//            //else
//            //{
//            //    GroundWithSlopesData groundData = physics.GetGroundY();
//            //    image.Position.Y = groundData.slopeY;
//            //    if (!physics.PhysicsStatusFalling)
//            //    {
//            //        const float TiltMulti = 0.7f;
//            //        goalXtilt = groundData.Xtilt * TiltMulti;
//            //        goalZtilt = groundData.Ztilt * TiltMulti;
//            //        badTerrain = TerrainSpeed.Normal;
//            //        terrainEffect((Data.MaterialType)groundData.material);
                    
//            //    }
                
//            //}

//            //if (!physics.PhysicsStatusFalling)
//            //{
//            //    const float TiltAdjustSpeed = 0.3f;
//            //    xtilt += (goalXtilt - xtilt) * TiltAdjustSpeed;
//            //    ztilt += (goalZtilt - ztilt) * TiltAdjustSpeed;
//            //}
           
//            //setImageDirFromRotation();
            
//        }

//        bool gotBadTerrainWarning = false;
//        virtual protected void terrainEffect(Data.MaterialType material)
//        {
//            switch (material)
//            {
//                case Data.MaterialType.gold:
//                    //boost
//                    boostUp();
//                    break;
//                case Data.MaterialType.dirt:
//                    badTerrain = TerrainSpeed.Bad;
//                    break;
//                case Data.MaterialType.sand:
//                    badTerrain = TerrainSpeed.Bad;
//                    break;
//                case Data.MaterialType.forest:
//                    badTerrain = TerrainSpeed.VeryBad;
//                    break;
//                case Data.MaterialType.burnt_ground:
//                    badTerrain = TerrainSpeed.VeryBad;
//                    break;
//                case Data.MaterialType.lava:
//                    TakeDamage(LavaDamage, true);
//                    break;
//                case Data.MaterialType.water:
//                    TakeDamage(WatereDamage, true);
//                    break;
//                case Data.MaterialType.iron:
//                    if (fireTime > wepFireRate)
//                    {
//                        fireTime = wepFireRate;
//                    }
//                    break;
//                case Data.MaterialType.leather:
//                    jump(0.4f);
//                    break;
//                case Data.MaterialType.purple_skin:
//                    jump(0.8f);
//                    break;
//                case Data.MaterialType.white:
//                    HealUp();
//                    break;
//            }
//        }

//        void jump(float speed)
//        {
//            image.Position.Y += 1f;
//            physics.SpeedY = speed;
//        }

//        static readonly LootFest.ObjSingleBound StandardBound = LootFest.ObjSingleBound.QuickBoundingBox(0.6f);
//        override protected LootFest.ObjSingleBound bound
//        { get { return StandardBound; } }

//        const float BadTerrainEffect = 0.6f;
//        virtual protected float badTerrainEffect
//        { get { return BadTerrainEffect; } }

//        const float BoostEffect = 1.6f;
//        virtual protected float boostEffect
//        { get { return BoostEffect; } }

//        protected void addTerrainEffect(ref float acceleration, ref float maxVelocity)
//        {
//            if (boostTime > 0)
//            {
//               // const float BoostEffect = 1.6f;
//                acceleration *= boostEffect;
//                maxVelocity *= boostEffect;
//            }
//            if (badTerrain == TerrainSpeed.Bad)
//            {
//                badTerrainWarning();
//                acceleration *= badTerrainEffect;
//                maxVelocity *= badTerrainEffect;
//            }
//            else if (badTerrain == TerrainSpeed.VeryBad)
//            {
//                badTerrainWarning();
//                const float VeryBadEffect = 0.3f;
//                acceleration *= VeryBadEffect;
//                maxVelocity *= VeryBadEffect;
//            }
//        }

//        void badTerrainWarning()
//        {
//            if (!gotBadTerrainWarning)
//            {
//                gotBadTerrainWarning = true;
//                if (player != null)
//                {
//                    player.Print("Bad terrain");
//                }
//            }
//        }
//        //const float Yadj = 0.0f;
//        //public override float CollisionYposWithAdjust
//        //{
//        //    get
//        //    {
//        //        return image.Position.Y + Yadj; //base.CollisionYposWithAdjust;
//        //    }
//        //}

//        public override ObjPhysicsType PhysicsType
//        {
//            get
//            {
//                return ObjPhysicsType.Character2;
//            }
//        }
//        protected override Vector3 ProjectileStartDir
//        {
//            get 
//            {
//                const float SpeedY = 0.04f;
//                return Map.WorldPosition.V2toV3(rotation.Direction(1), SpeedY);
//            }
//        }
//        public override void HandleColl3D(Game1.Physics.Bound3dIntersect collData, GameObjects.AbsUpdateObj ObjCollision)
//        {
//            base.HandleColl3D(collData, ObjCollision);
//            velocity = 0;
//        }
//        //public override void HandleTerrainColl3D(TerrainColl collSata, Vector3 oldPos)
//        //{
//        //    base.HandleTerrainColl3D(collSata, oldPos);
//        //}
//        protected override NetworkClientRotationUpdateType NetRotationType
//        {
//            get
//            {
//                return NetworkClientRotationUpdateType.Plane1D;
//            }
//        }

//        public override void setImageDirFromRotation()
//        {
//            image.Rotation.QuadRotation = Quaternion.Identity;
//            Vector3 rot = Vector3.Zero;
//            rot.X = MathHelper.TwoPi - rotation.Radians + adjustRotation;
            
//            image.Rotation.RotateWorld(new Vector3(0, -ztilt, xtilt));
//            image.Rotation.RotateAxis(rot);
//        }
//    }
//    enum TerrainSpeed
//    {
//        Normal,
//        Bad,
//        VeryBad,
//    }
//}
