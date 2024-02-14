//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
////xna

//namespace VikingEngine.LootFest.GO.Characters
//{
//    class FlyingPet : AbsEnemy
//    {
//        const float FlyingMoveSpeed = 0.02f;
//        public const int ResetPetTimeSec = 10;
//        const float FireDistance = 32;
//        const float FireAngle = 0.2f;
//        const float WantedDistance = 8;

//        GO.PlayerCharacter.AbsHero master;
//        new GO.AbsUpdateObj target;
//        Timer.Basic fireRate = new Timer.Basic(3000, true);
//        float goalY;
        
//        float wantedHeightAboveHero = 4;
//        Rotation1D goalDir = Rotation1D.Random();
//        int moveDirAddDir = Ref.rnd.Dir();
//        FlyingPetType petType;

//        public FlyingPet(GO.PlayerCharacter.AbsHero master, FlyingPetType petType)
//            : base(0)
//        {
//            this.petType = petType;
//            goalDir = Rotation1D.Random();
            
//            Velocity = new VikingEngine.Velocity( goalDir,FlyingMoveSpeed);
//            this.master = master;
//            WorldPos = master.WorldPos;
//            Health = 3;
//            basicInit();
//            NetworkShareObject();
//        }

//        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
//        {
//            base.ObjToNetPacket(writer);
//            writer.Write((byte)petType);
//        }

//        public FlyingPet(System.IO.BinaryReader r)
//            : base(r)
//        {
//            petType = (FlyingPetType)r.ReadByte();
//            basicInit();
//        }



//        protected override void basicInit()
//        {
//            float size = 2f;
//            VoxelModelName imageName;
//            Graphics.AnimationsSettings animation;
//            switch (petType)
//            {
//                default:
//                    imageName = VoxelModelName.pet_pig;
//                    animation = new Graphics.AnimationsSettings(4, 1.5f, false);
//                    size = 1.6f;
//                    break;
//                case FlyingPetType.Angel:
//                    imageName = VoxelModelName.pet_angel;
//                    animation = new Graphics.AnimationsSettings(2, 1.6f, false);
//                    break;
//                case FlyingPetType.Dragon:
//                    imageName = VoxelModelName.pet_dragon;
//                    animation = new Graphics.AnimationsSettings(5, 1.6f, false);
//                    size = 2.8f;
//                    break;
//                case FlyingPetType.Falcon:
//                    imageName = VoxelModelName.pet_falcon;
//                    animation = new Graphics.AnimationsSettings(5, 1.6f, false);
//                    size = 2.6f;
//                    break;
//                case FlyingPetType.Bird:
//                    imageName = VoxelModelName.pet_bird;
//                    animation = new Graphics.AnimationsSettings(2, 3f, false);
//                    size = 1.4f;
//                    break;

//            }
//            image = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(imageName, TempMonsterImage, size, 0, animation);
//            image.Position = WorldPos.PositionV3;
//            image.Position.Y = LfRef.chunks.GetScreen(WorldPos).GetGroundY(WorldPos) + 2;
            
//            CollisionBound = LootFest.ObjSingleBound.QuickBoundingBox(0.24f * size);
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            image.UpdateAnimation(Velocity.PlaneLength(), args.time);
//            if (localMember)
//            {
//                goalY = master.Position.Y + wantedHeightAboveHero;
//                Vector2 diff = PositionDiff(master);
//                heroPosDiffLength = diff.Length();
//                if (heroPosDiffLength > WantedDistance)
//                {
//                    //boost towards player
//                    diff.Normalize();
//                    Velocity.Add(diff * 0.002f);
//                    goalDir = Rotation1D.FromDirection(diff);
//                }



//                //if (!Velocity.ZeroPlaneSpeed) //!= Vector2.Zero)
//                //{
//                    oldVelocity = Velocity;
//                    Velocity.PlaneUpdate(args.time, image);
//                //}
//                //else
//                //{
//                //    lib.DoNothing();
//                //}

//                float ydiff = goalY - image.Position.Y;
//                image.Position.Y += ydiff * 0.05f;




//                //speed decline
//                Velocity *= 0.84f;

//                goalDir += Ref.rnd.Float(0.06f) * moveDirAddDir;
//                if (Ref.rnd.RandomChance(0.004f))
//                {
//                    moveDirAddDir = -moveDirAddDir;
//                    wantedHeightAboveHero = 3 + Ref.rnd.Float(3);
//                }
//                Velocity.Add(goalDir, 0.001f);

//                setImageDirFromSpeed();
                


//                //fire projectiles

//                if (target != null)
//                {
//                    GO.AbsUpdateObj currentTarget = target;
//                    target = null;
//                    new WeaponAttack.FlyingPetBullet(image.Position, currentTarget.Position, petType);
//                }

//                if (heroPosDiffLength > 80)
//                {
//                    //jump
//                    image.Position = master.Position;
//                }
//            }
//            else
//            {
//                base.Time_Update(args);
//            }
//            //setImageDirFromSpeed();
//        }

//        float heroPosDiffLength = 0;

//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {

//            base.AsynchGOUpdate(args);

//            if (localMember)
//            {
//                SolidBodyCheck(args.allMembersCounter);
                
//                if (fireRate.Update(args.time))
//                {
//                    LowestValueFinder lowestDist = new LowestValueFinder(true);

//                    args.allMembersCounter.Reset();
//                    while (args.allMembersCounter.Next())
//                    //for (int i = 0; i < active.Count; i++)
//                    {
//                        if (args.allMembersCounter.GetMember.Type == ObjectType.Character && 
//                            WeaponAttack.WeaponLib.IsFoeTarget(args.allMembersCounter.GetMember.WeaponTargetType, this.WeaponTargetType, true))
//                        {
//                            float dist = distanceToObject(args.allMembersCounter.GetMember);
//                            if (dist <= FireDistance && angleDiff(args.allMembersCounter.GetMember) <= FireAngle)
//                            {
//                                lowestDist.Next(dist, args.allMembersCounter.CurrentIndex);
//                            }
//                        }
//                    }

//                    if (lowestDist.hasValue)
//                    {
//                        target = args.allMembersCounter.GetFromIndex(lowestDist.LowestMemberIndex);

//                    }
//                }
//            }
//        }

//        protected override void clientSpeed(float speed)
//        {
//            image.UpdateAnimation(speed, Ref.DeltaTimeMs);
//        }

//        protected override LoadedSound HurtSound
//        {
//            get
//            {
//                return LoadedSound.MonsterHit1;
//            }
//        }
//        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
//        {
//            return true;//do nothing
//        }
//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
//            Vector3 depth = CollisionBound.IntersectionDepthAndDir(collData);
//            goalDir = Rotation1D.FromDirection( VectorExt.V3XZtoV2(depth));
            
//        }

//        public override ObjPhysicsType PhysicsType
//        {
//            get
//            {
//                return  ObjPhysicsType.Projectile;;
//            }
//        }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.FlyingPet; }
//        }
//        //protected override bool ViewHealthBar
//        //{
//        //    get
//        //    {
//        //        return false;
//        //    }
//        //}
//        public override Graphics.LightParticleType LightSourceType
//        {
//            get
//            {
//                return Graphics.LightParticleType.NUM_NON;
//            }
//        }
//        public override WeaponAttack.WeaponUserType  WeaponTargetType
//        {
//            get 
//            { 
//                 return WeaponAttack.WeaponUserType.Friendly;
//            }
//        }
//        protected override bool pushable
//        {
//            get
//            {
//                return false;
//            }
//        }

       

//        //Pig,
//        //frames:4
//        //color:l brown, white, red
//        //Dragon,
//        //frames:5
//        //color:green, y gren, bone
//        //Angel,
//        //frames:2
//        //color:blue, skin, yell
//        //Falcon,
//        //frames:5
//        //color:gray, whit, black

//        Effects.BouncingBlockColors DamageColPig = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        Effects.BouncingBlockColors DamageColDragon = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        Effects.BouncingBlockColors DamageColAngel = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        Effects.BouncingBlockColors DamageColFalcon = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);


//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                switch (petType)
//                {
//                    default:
//                        return DamageColPig;
//                    case FlyingPetType.Angel:
//                        return DamageColAngel;
//                    case FlyingPetType.Dragon:
//                        return DamageColDragon;
//                    case FlyingPetType.Falcon:
//                        return DamageColFalcon;

//                }
//            }
//        }
//    }
//    enum FlyingPetType
//    {
//        Pig,
//        Dragon,
//        Angel,
//        Falcon,
//        Bird,
//        NUM
//    }
//}
