//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.Characters
//{
//    class EvilSpider : AbsMonster
//    {
//        public EvilSpider(Vector3 startPos)
//            : base(args)
//        {
//            aiState = AiState.Walking;
//            aiStateTimer.MilliSeconds = 400 + Ref.rnd.Int(1000);
//            Velocity = new VikingEngine.Velocity(Rotation1D.Random(), WalkingSpeed);
//            setImageDirFromSpeed();
//            image.Position.Y = startPos.Y + 1;
//            setHealth( LfLib.StandardEnemyHealth);
//            NetworkShareObject();
//        }
//         public EvilSpider(System.IO.BinaryReader r)
//            : base(r)
//        {
//        }
//         override protected void createBound(float imageScale)
//         {
//             CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.3f * imageScale, 0.5f * imageScale, imageScale * 0.5f));
//             TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
//         }
//        protected override VoxelModelName imageName
//        {
//            get { return VoxelModelName.evil_spider; }
//        }
        
//        static readonly Graphics.AnimationsSettings AnimSettings = new Graphics.AnimationsSettings(6, 0.5f);
//        protected override Graphics.AnimationsSettings animSettings
//        {
//            get { return AnimSettings; }
//        }

//        protected override Monster2Type monsterType
//        {
//            get { return Monster2Type.EvilSpider; }
//        }

//        bool lifeTimeOut = false;
//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            basicAIupdate(args);
//            if (localMember)
//            {
//                if (aiStateTimer.MilliSeconds <= 0 && !lifeTimeOut)
//                {
//                        aiStateTimer.MilliSeconds = 3000 + Ref.rnd.Int(600);
                        
//                        target = getClosestCharacter(float.MaxValue, args.allMembersCounter, WeaponAttack.WeaponUserType.NON);
//                        //aiState = AiState.Attacking;
//                        lifeTimeOut = true;
//                }
//            }
//        }
//        static readonly IntervalF ScaleRange = new IntervalF(3f, 4f);
//        protected override IntervalF scaleRange
//        {
//            get { return ScaleRange; }
//        }


//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);
//            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, new Graphics.ParticleInitData(image.Position));

//            if (target != null)
//            {
//                const float RotationsSpeed = 0.005f;
//                rotateTowardsObject(target, RotationsSpeed, WalkingSpeed);
//                setImageDirFromRotation();

//            }
//            if (lifeTimeOut && aiStateTimer.MilliSeconds <= 0)
//            {
//                lifeTimeOut = false;
//                DeleteMe();
//                const float BlockScale = 0.3f;
//                new Effects.BouncingBlock2(image.Position, Data.MaterialType.black, BlockScale);
//                new Effects.BouncingBlock2(image.Position, Data.MaterialType.black, BlockScale);
//                new Effects.BouncingBlock2Dummie(image.Position, Data.MaterialType.black, BlockScale);
//                new Effects.BouncingBlock2Dummie(image.Position, Data.MaterialType.black, BlockScale);
//            }
            
//        }

//        public override void DeleteMe(bool local)
//        {
//            base.DeleteMe(local);
//        }

//        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Magic.MagicElement.Evil);
//        protected override WeaponAttack.DamageData contactDamage
//        {
//            get
//            {
//                return Damage;
//            }
//        }
//        protected override void HitCharacter(AbsUpdateObj character)
//        {
//            Health -= LfLib.StandardEnemyHealth * 0.5f;
//        }



//        const float WalkingSpeed = 0.016f;

//        protected override float walkingSpeed
//        {
//            get { return WalkingSpeed; }
//        }
//        protected override float runningSpeed
//        {
//            get { return WalkingSpeed; }
//        }

//        public override WeaponAttack.WeaponUserType WeaponTargetType
//        {
//            get
//            {
//                return WeaponAttack.WeaponUserType.NON;
//            }
//        }
//        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return DamageColorsLvl1;
//            }
//        }
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.EvilSpider; }
//        }

        

//        public override Graphics.LightParticleType LightSourceType
//        {
//            get
//            {
//                return Graphics.LightParticleType.NUM_NON;
//            }
//        }
//    }
//}
