//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.Characters
//{
//    //hela skadade orcher, om inte hero är för nära
//    //fly om för nära
//    //kasta knockade stenar

//    class Grunt : AbsEnemy
//    {
//        //CarryType carry;
//        //Vector3 carryRelPos;
//        Graphics.AbsVoxelObj carryImg;
//        const float WalkingSpeed = 0.007f;
//        const float CarryingSpeed = WalkingSpeed * 0.6f;
//        float walkingSpeed
//        {
//            get { return WalkingSpeed; }
//        }

//        static readonly IntervalF BasicScale = new IntervalF(0.08f, 0.09f);
//        float stateTime = 1000;
//        Map.WalkingPath walkingPath;
//        float reloadStoneTime = 0;

//        public Grunt(GoArgs goArgs)
//            : base(goArgs)
//        {
//            //this.areaLevel = level;
//            WorldPos = goArgs.startWp;

//            //int rnd = Ref.rnd.Int(100);
//            //if (rnd < 10)
//            //{
//            //    carry = CarryType.Chest;
//            //}
//            //else
//            //{
//            //    carry = CarryType.Barrel;
//            //}

//            gruntBaicInit();

//            image.Position = goArgs.startPos;
//            image.Position.Y = Map.WorldPosition.ChunkStandardHeight;
//            Health = LfLib.StandardEnemyHealth;

//            NetworkShareObject();
//        }

//        public override void ObjToNetPacket(System.IO.BinaryWriter w)
//        {
//            base.ObjToNetPacket(w);
//           // w.Write((byte)carry);
//            w.Write((byte)characterLevel);
//        }

//        //public Grunt(System.IO.BinaryReader r)
//        //    : base(r)
//        //{
//        //    //carry = (CarryType)r.ReadByte();
//        //    characterLevel = r.ReadByte();
//        //    gruntBaicInit();
//        //}

//        void gruntBaicInit()
//        {
//            float scale = BasicScale.GetRandom();
//            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(5.5f * scale, 12 * scale, 0f);
//            image =
//                LfRef.modelLoad.AutoLoadModelInstance(characterLevel == 0 ? VoxelModelName.grunt : VoxelModelName.grunt2,
//                TempCharacterImage,
//                0, 1, false, new Graphics.AnimationsSettings(7, 1.1f));

//            image.Scale = Vector3.One * scale;

//            //if (carry == CarryType.Barrel)
//            //{
//            //    carryImg = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.barrelX, GO.WeaponAttack.ItemThrow.Barrel.BarrelTempImage,
//            //        WeaponAttack.ItemThrow.Barrel.BarrelScale, 0);
//            //    carryRelPos = new Vector3(0, 0, -1f);
//            //}
//            //else
//            //{
//            //    carryImg = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.chest_closed, GO.EnvironmentObj.Chest.ChestTempImage,
//            //        WeaponAttack.ItemThrow.Barrel.BarrelScale, 0);
//            //    carryImg.Scale = carryImg.OneScale * 2 * Vector3.One;
//            //    carryRelPos = new Vector3(0, 0, -1f);
//            //}

//            //updateCarryImg();
//        }

//        bool barrelThrow = false;
//        bool fireStone = false;

//        static readonly List<int> FollowTypes = new List<int> { (int)GameObjectType.Humanioid, };
//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            if (localMember)
//            {
//                SolidBodyCheck(args.allMembersCounter);

//                stateTime -= args.time;
//                if (stateTime <= 0)
//                {
//                    //first check if he is to close to a hero
//                    target = getClosestCharacter(32, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
//                    if (target == null)
//                    {
//                        //walk towards closest friendly
//                        GO.AbsUpdateObj friendly = null;// = GetClosestObjectType(args.allMembersCounter, GameObjectType.Character, FollowTypes, 64);
//                        if (friendly == null)
//                        {
//                            removePath();

//                            if (Ref.rnd.RandomChance(60))
//                            {
//                                walkRandomDir();
//                            }
//                            stateTime = 400 + Ref.rnd.Int(800);

//                        }
//                        else
//                        {
//                            addWalkingPath(walkingPathTowardsObject(friendly));
//                        }
//                        stateTime = 2000;
//                    }
//                    else
//                    {
//                        if (distanceToObject(target) < 10)
//                        {
//                            //flee
//                            addWalkingPath(walkingPathAwayFromObject(target, 0.3f));
//                            stateTime = 1000 + Ref.rnd.Int(2000);
//                        }
//                        //else if (carry == CarryType.Barrel && Ref.rnd.RandomChance(4))
//                        //{
//                        //    //throw barrel
//                        //    barrelThrow = true;
//                        //    stateTime = 500;
//                        //}
//                        //else if (carry == CarryType.NON && Ref.rnd.RandomChance(20) && reloadStoneTime <= 0)
//                        //{
//                        //    fireStone = true;
//                        //    reloadStoneTime = 4000;
//                        //    stateTime = 500;
//                        //}
//                        else
//                        {
//                            //walk random dir
//                            walkRandomDir();
//                            stateTime = 1000 + Ref.rnd.Int(2000);
//                        }
//                    }
//                }
//            }
//        }
//        public override void Time_Update(UpdateArgs args)
//        {
//            image.UpdateAnimation(Velocity.PlaneLength(), args.time);
//            if (localMember)
//            {
//                if (fireStone)
//                {
//                    fireStone = false;
//                    new WeaponAttack.GruntStone(target, image.Position);
//                }
//                if (barrelThrow)
//                {
//                    barrelThrow = false;
//                    //new WeaponAttack.ItemThrow.Barrel(true, new Rotation1D(AngleDirToObject(target) + Ref.rnd.Plus_MinusF(0.4f)), carryImg, this);
//                    carryImg = null;
//                    //carry = CarryType.NON;
//                }


//                base.Time_Update(args);
//                if (walkingPath == null)
//                {
//                    Velocity.SetZeroPlaneSpeed();
//                }
//                else
//                {
//                    Velocity.PlaneValue = walkingPath.WalkTowardsNode(image.Position, rotation, 0.0005f * args.time, walkingSpeed);
//                    setImageDirFromSpeed();
//                    oldVelocity = Velocity;
//                    moveImage(Velocity, args.time);

//                   // updateCarryImg();
//                }
//                reloadStoneTime -= args.time;
//            }
//            else
//            {
//                base.Time_Update(args);
//              //  updateCarryImg();
//            }
//        }
//        void walkRandomDir()
//        {
//            addWalkingPath(LfRef.chunks.PathFindRandomDir(image.Position));
//        }

//        void addWalkingPath(Map.WalkingPath path)
//        {
//            //The charcter move some before the path is calculated
//            bool movedOn = removePath();

//            walkingPath = path;
//        }
//        bool removePath()
//        {
//            Velocity.SetZeroPlaneSpeed();
//            if (walkingPath != null)
//            {
//                walkingPath.DeleteMe();
//                walkingPath = null;
//                return true;
//            }
//            return false;
//        }



//        //void updateCarryImg()
//        //{
//        //    if (carryImg != null)
//        //    {
//        //        carryImg.Position = image.Rotation.TranslateAlongAxis(carryRelPos, image.Position);
//        //        carryImg.Rotation = image.Rotation;
//        //    }
//        //}


//        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
//        { 

//            if (localMember)
//            {
//                //if (carry == CarryType.Chest)
//                //{
//                //   // new PickUp.GruntChest(carryImg.Position, EnemyValueLevel);
//                //}
//                //else if (carry == CarryType.Barrel)
//                //{
//                //    new WeaponAttack.ItemThrow.Barrel(false, Rotation1D.Random(), carryImg, this);
//                //    carryImg = null;
//                //}
//                if (Ref.rnd.RandomChance(80))
//                    new PickUp.Coin(LootDropPos(), EnemyValueLevel);
//            }
//            else
//            {
//                carryImg.DeleteMe();
//            }
            

//            base.DeathEvent(local, damage);
//        }
        
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.Grunt; }
//        }
//        public override void DeleteMe(bool local)
//        {
//            base.DeleteMe(local);
//            removePath();
//            if (carryImg != null)
//            {
//                carryImg.DeleteMe();
//            }
//        }
//        //public override void WeaponAttackFeedback(WeaponAttack.WeaponTrophyType weaponType, int numHits, int numKills)
//        //{
//        //    //if (numKills >= LootfestLib.Trophy_BarrelKill)
//        //    //{
//        //    //    LfRef.gamestate.BarrelKillTrophy(image.Position);
//        //    //}
//        //    //base.WeaponAttackFeedback(weaponType, numHits, numKills);
//        //}
//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
//        }
//        //grunt
//        //gray, blue, r brown
//        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.white,Data.MaterialType.white,Data.MaterialType.white);
//        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return characterLevel == 0? DamageColorsLvl1 : DamageColorsLvl2;
//            }
//        }
//    }

//    enum GruntState
//    {

//    }
//    enum CarryType
//    {
//        NON,
//        Barrel,
//        Chest,
//        NUM
//    }
//}
