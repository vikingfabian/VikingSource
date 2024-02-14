//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////xna
//using Microsoft.Xna.Framework;

//namespace VikingEngine.LootFest.GO.Characters
//{
//    class EndMagician : AbsEnemy
//    {
//        const float WalkingSpeed = 0.009f;
//        const float RunningSpeed = WalkingSpeed * 2f;
//        float circleWindSpeed { get { return walkingSpeed * 0.8f; } }
//        float walkingSpeed { get { return WalkingSpeed; } }
//        float runningSpeed { get { return RunningSpeed; } }
        
//        AbsCharacter target;
//        //AiState aiState = AiState.Waiting;
//        //float aiStateTimer.MilliSeconds = 0;
       
//        public const float Scale = 5;
//        const float SwordScale = Scale * 0.02f;

//        public EndMagician(Vector3 startPos, Rotation1D dir)
//            : base(0)
//        {

//            WorldPosition = new Map.WorldPosition(startPos);
//            magicianInit(startPos, dir);

//            armor = LootfestLib.HumanoidStandardArmor;
//            target = null;
//            Health = LootfestLib.EndMagicianHealth;
            
//            NetworkShareObject();
//        }

//        public override void ObjToNetPacket(System.IO.BinaryWriter w)
//        {
//            base.ObjToNetPacket(w);
//            w.Write(rotation.ByteDir);
//            w.Write(image.Position);
//        }

//        public EndMagician(System.IO.BinaryReader r)
//            : base(r)
//        {
//            rotation.ByteDir = r.ReadByte();
//            Vector3 pos = r.ReadVector3();
//            magicianInit(pos, rotation);
//        }



//        void magicianInit(Vector3 startPos, Rotation1D dir)
//        {
//            CollisionBound = LootFest.ObjSingleBound.QuickBoundingBox(Scale * 0.22f);
//            image = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.end_magician, TempCharacterImage, Scale, 1,
//                new Graphics.AnimationsSettings(8, 1.1f, 2));
//            image.Position = startPos;
            
//            Data.ImageAutoLoad.PreLoadImage(VoxelObjName.magician_sword, false, 0);
//            rotation = dir;
//        }


//        const int NetShareStateTime = 10;

//        void setState(AiState state, ISpottedArrayCounter<GO.AbsUpdateObj> active)
//        {
//            this.aiState = state;
//            switch (state)
//            {
//                case AiState.Waiting:
//                    Velocity.SetZeroPlaneSpeed();
//                    aiStateTimer.MilliSeconds = 200 + Ref.rnd.Int(300);
//                    break;
//                case AiState.MoveTowardsTarget:
//                    target = closestGoodGuy(active);//ClosestHero(image.Position);
//                    aiStateTimer.MilliSeconds = 800 + Ref.rnd.Int(800);
//                    break;
//                case AiState.Walking:
//                    rotation = Rotation1D.Random();
//                    Velocity.Set(rotation, walkingSpeed);
//                    aiStateTimer.MilliSeconds = 600 + Ref.rnd.Int(1000);
//                    break;
//                case AiState.MagicAttack:
//                    Velocity.SetZeroPlaneSpeed();
//                    rotation.Radians = AngleDirToObject(closestGoodGuy(active));
//                    setImageDirFromRotation();
//                    attack = MagicianAttack.MagicAttack;
//                    aiStateTimer.MilliSeconds = 600;
//                    break;
//                case AiState.SwordCirkleWind:
//                    moveTowardsObject(closestGoodGuy(active), 2, circleWindSpeed);
//                    aiStateTimer.MilliSeconds = 1400 + Ref.rnd.Int(800);
//                    attack = MagicianAttack.Sword ;

//                    netSendStateAndTime();
//                    //System.IO.BinaryWriter w = NetworkWriteObjectState((int)state);
//                    //w.Write((byte)(stateRefreshTime / NetShareStateTime));
//                    break;
//                case AiState.Attacking:
//                    Velocity.SetZeroPlaneSpeed();
//                    aiStateTimer.MilliSeconds = 400;
//                    attack = MagicianAttack.Sword ;

//                    netSendStateAndTime();
//                    //System.IO.BinaryWriter w = NetworkWriteObjectState((int)state);
//                    //w.Write((byte)(stateRefreshTime / NetShareStateTime));
//                    break;
//                case AiState.Swordrush:
//                    moveTowardsObject(closestGoodGuy(active), 0, runningSpeed);
//                    aiStateTimer.MilliSeconds = 600 + Ref.rnd.Int(800);
//                    attack = MagicianAttack.Sword;

//                    netSendStateAndTime();
//                    break;
//                case AiState.ThrowSword:
//                    Velocity.SetZeroPlaneSpeed();
//                    aiStateTimer.MilliSeconds = 2000;
//                    attack = MagicianAttack.ThrowingSword;

//                    System.IO.BinaryWriter writeThrowSword = BeginWriteObjectStateAsynch(state);
//                    writeThrowSword.Write(rotation.ByteDir);
//                    EndWriteObjectStateAsynch(writeThrowSword);

//                    break;

//            }
//        }

       

//        void netSendStateAndTime()
//        {
//            System.IO.BinaryWriter w = BeginWriteObjectStateAsynch(aiState);
//            w.Write((byte)(aiStateTimer.MilliSeconds / NetShareStateTime));
//            w.Write(Velocity.Rotation1D().ByteDir);

//            EndWriteObjectStateAsynch(w);
//        }

//        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
//        {
//            this.aiState = state;
//            if (this.aiState == AiState.SwordCirkleWind || this.aiState == AiState.Swordrush || this.aiState == AiState.Attacking)
//            {
//                aiStateTimer.MilliSeconds = r.ReadByte() * NetShareStateTime;
//                rotation.ByteDir = r.ReadByte();
//                switch (this.aiState)
//                {
//                    case AiState.SwordCirkleWind:
//                        Velocity.Set(rotation, circleWindSpeed);// = rotation.Direction(circleWindSpeed);
//                        break;
//                    case AiState.Swordrush:
//                        Velocity.Set(rotation, runningSpeed);// = rotation.Direction(runningSpeed);
//                        break;
//                    case AiState.Attacking:
//                        Velocity.SetZeroPlaneSpeed();
//                        break;
//                }
                
//                createSword();
//            }
//            else if (this.aiState == AiState.ThrowSword)
//            {
//                rotation.ByteDir = r.ReadByte();
//                throwSword();
//            }
//        }

//        public override void AIupdate(GO.UpdateArgs args)
//        {
//            if (localMember)
//            {
//                switch (aiState)
//                {
//                    case AiState.MoveTowardsTarget:
//                        if (moveTowardsObject(target, 2, walkingSpeed))
//                        {
//                            aiStateTimer.MilliSeconds = 0;
//                        }
//                        break;
//                }

//                //check area bound
//                //speed = battleArea.KeepPointInsideBound_Speed(PlanePos, speed);

//                if (aiStateTimer.MilliSeconds <= 0)
//                {
//                    AI.SelectRandomState rndState = new AI.SelectRandomState();
//                    switch (aiState)
//                    {
//                        default:
//                            rndState.AddState((int)AiState.Waiting, 10);
//                            break;
//                        case AiState.Waiting:
//                            rndState.AddState((int)AiState.MoveTowardsTarget, 10);
//                            rndState.AddState((int)AiState.Walking, 5);
//                            rndState.AddState((int)AiState.MagicAttack, 10);
//                            break;
//                        case AiState.Walking:
//                            rndState.AddState((int)AiState.Waiting, 40);
//                            rndState.AddState((int)AiState.Walking, 10);
//                            rndState.AddState((int)AiState.Attacking, 10);
//                            break;
//                        case AiState.MoveTowardsTarget:
//                            rndState.AddState((int)AiState.Waiting, 10);
//                            rndState.AddState((int)AiState.Attacking, 40);
//                            break;
//                        case AiState.MagicAttack:
//                            rndState.AddState((int)AiState.MagicAttack, 40);
//                            rndState.AddState((int)AiState.Walking, 5);
//                            rndState.AddState((int)AiState.Waiting, 10);
//                            break;
//                        case AiState.Attacking:
//                            rndState.AddState((int)AiState.MoveTowardsTarget, 10);
//                            rndState.AddState((int)AiState.ThrowSword, 20);
//                            rndState.AddState((int)AiState.Swordrush, 20);
//                            rndState.AddState((int)AiState.SwordCirkleWind, 20);
//                            rndState.AddState((int)AiState.Waiting, 5);
//                            break;

//                    }
//                    setState((AiState)rndState.GetRandom().State, args.allMembersCounter);

//                }
//            }
//        }

//        float clinkSoundTime = 0;
//        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
//        {
//            bool immune = damage.Magic == Magic.MagicElement.Evil;
//            if (immune && clinkSoundTime <= 0)
//            {
//                Music.SoundManager.PlaySound(LoadedSound.weaponclink, image.Position);
//                clinkSoundTime = 600;
//            }
//            return !immune && base.willReceiveDamage(damage);
//        }
//        //protected override void handleDamage(Weapons.DamageData damage, bool local)
//        //{
//        //    if (data.Weakness.DamageSensitive(damage))
//        //        damage.Damage *= LootfestLib.MagicianWeaknessMultiply;
//        //    base.handleDamage(damage, local);
//        //}


//        MagicianAttack attack = MagicianAttack.NONE;
//        public override void Time_LasyUpdate(ref float time)
//        {
//            base.Time_LasyUpdate(ref time);
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            switch (aiState)
//            {
//                case AiState.SwordCirkleWind:
//                    rotation.Radians += 0.02f * args.time;
//                    break;
//            }
//            aiStateTimer.MilliSeconds -= args.time;
//            clinkSoundTime -= args.time;
//            immortalityTime.CountDown();
//            float animationSpeed;

//            if (localMember)
//            {
//                if (attack != MagicianAttack.NONE)
//                {
//                    NetworkUpdatePacket(Network.PacketReliability.Reliable);

//                    if (attack == MagicianAttack.Sword)
//                    {
//                        createSword();
//                    }
//                    else if (attack == MagicianAttack.MagicAttack)
//                    {
//                        //switch (data.AttackMagic)
//                        //{
//                        //    case Magic.MagicType.Fire:
//                        //        new Magic.MagicianFireBall(data.BossLevel,
//                        //           image.Position + Vector3.Up * 1.5f, rotation);
//                        //        break;
//                        //    case Magic.MagicType.Lightning:
//                        //        const float Spread = 0.4f;
//                        //        Rotation1D fireDir = rotation;
//                        //        fireDir.Add(-Spread);
//                        //        for (int i = 0; i < 3; i++)
//                        //        {
//                        //            new Weapons.MagicianLightSpark(image.Position, fireDir, areaLevel);
//                        //            fireDir.Add(Spread);
//                        //        }
//                        //        break;
//                        //    case Magic.MagicType.Poision:
//                        //        new Weapons.ItemThrow.MagicianPoisonBomb(this, areaLevel);
//                        //        break;
//                        //    default:
//                        //        throw new NotImplementedException("Magician fire");

//                        //}



//                    }
//                    else if (attack == MagicianAttack.ThrowingSword)
//                    {
//                        throwSword();
//                    }
//                    if (aiState != AiState.SwordCirkleWind)
//                        aiStateTimer.MilliSeconds += 400;
//                    attack = MagicianAttack.NONE;
//                }

//                animationSpeed = Velocity.PlaneLength();
//                if (!Velocity.ZeroPlaneSpeed)
//                {
//                    oldVelocity = Velocity;
//                    moveImage(Velocity, args.time);
//                }
//                physics.Update(args.time);
//                physics.ObsticleCollision();

//                if (aiState == AiState.SwordCirkleWind)
//                {
//                    setImageDirFromRotation();
//                }
//                else
//                {
//                    setImageDirFromSpeed();
//                }
//                //if (this.myUpdateStatus != UpdateStatus.Deleted)
//                //{
//                //    //Map.Screen s = 
//                //    //if (!LfRef.chunks.ScreenDataGridLoadingComplete(battleCenter))
//                //    //{
//                //        //DeleteMe();
//                //    //}
//                //}
//            }
//            else //clientMember
//            {
//                if (aiStateTimer.MilliSeconds <= 0)
//                    aiState = AiState.Waiting;
//                if (aiState == AiState.Swordrush || aiState == AiState.SwordCirkleWind)
//                {
//                    //image.Position.X += Speed.X * args.time;
//                    //image.Position.Z += Speed.Y * args.time;

//                    Velocity.PlaneUpdate(args.time, image);

//                    if (aiState == AiState.Swordrush)
//                    {
//                        animationSpeed = runningSpeed;
//                    }
//                    else
//                    {
//                        animationSpeed = circleWindSpeed;
//                    }
//                    setImageDirFromRotation();
//                }
//                else
//                {
//                    updateClientDummieMotion(args.time);
//                    animationSpeed = clientSpeedLength;
//                }
//            }
            

//            //updateBattleArea();
//            image.UpdateAnimation(animationSpeed, args.time);
//        }


//        WeaponAttack.DamageData SwordDamage = new WeaponAttack.DamageData(LootfestLib.EndMagicianSwordDamage);
//        void createSword()
//        {
//            new WeaponAttack.HandWeaponAttack(aiStateTimer.MilliSeconds, this,
//                LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.magician_sword, TempSwordImage, SwordScale, 0), SwordScale,
//                SwordDamage, Data.Gadgets.BluePrint.EnchantedSword, localMember);
//        }

//        void throwSword()
//        {
//            //if (!localMember)
//            //{
//            //    image.Position.Y += 1;
//            //}
//            new WeaponAttack.MagicianThrowSword(this, runningSpeed * 1.4f, SwordDamage, SwordScale);
//        }

//        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
//        {
//            //if (localMember)
//            //{
//            //    int numCoins = 4 + Ref.rnd.Int(3);
//            //    int numLoot = 2 + Ref.rnd.Int(3);
//            //    Vector3 itemPos = image.Position + Vector3.Up * 2;

//            //    for (int i = 0; i < numCoins; i++)
//            //    {
//            //        new PickUp.Coin(itemPos, EnemyValueLevel);
//            //    }
//            //    for (int i = 0; i < numLoot; i++)
//            //    {
//            //        new PickUp.HumanoidLoot(itemPos, EnemyValueLevel);
//            //    }
//            //    new Effects.BossDeathItem(image.Position);
//            //}
//            //LfRef.gamestate.Progress.DefeatedBoss(data.BossLevel);

            
//            //if (data.BossLevel == LootfestLib.NumBosses - 1)
//            //{
//            //    if (local)
//            //    {
//            //        LfRef.gamestate.GameCompleted(true, battleCenter, LfRef.gamestate.Progress.NumHeroDeaths == 0);
//            //    }
//            //}
//            //else
//            //{
//            //    bool gotHeroInBattle = false;
//            //    foreach (GO.PlayerCharacter.AbsHero h in LfRef.AbsHeroes)
//            //    {
//            //        //check distance
//            //        if ((h.ScreenPos - battleCenter).SideLength() <= 5)
//            //        {
//            //            gotHeroInBattle = true;
//            //        }
//            //    }
//            //    if (gotHeroInBattle)
//            //    {
//            //        LfRef.gamestate.MusicDirector.SuccessSong();
//            //    }
//            //}
//            base.DeathEvent(local, damage);

//        }

//        static readonly LootFest.ObjSingleBound ProjectileBound = LootFest.ObjSingleBound.QuickBoundingBox(0.6f);
        
        
//        public override GameObjectType Type
//        {
//            get
//            {
//                return GameObjectType.Magician;
//            }
//        }

//        protected override NetworkClientRotationUpdateType NetRotationType
//        {
//            get
//            {
//                return NetworkClientRotationUpdateType.Plane1D;
//            }
//        }
//    }

    
//}
