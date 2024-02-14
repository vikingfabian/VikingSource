//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
////xna
//namespace VikingEngine.LootFest.GO.Characters
//{
//    class Magician : AbsEnemy
//    { 
//    //    -standard
//    //    -fungerar som leader
//    //    -aggresiva svärds hugg
//    //    -rotera med svärd
//    //    -svärd boomerang
//    //    -sköld enchant på egna
//    //    -eld spår attack
//    //    -dör i eldhav
//    //-boss magiker (3st som slutboss)
//    //    -flyger och landar med stunn attack
//    //    -teleport förflyttning mellan attacker
//    //    -eld beam, stråle av attacker
//    //    -eld cirkel
//    //    -springa i cirklar
//    //    -välj nån typ av magi som favoritiseras
//    //        -eld
//    //        -light
//        //        -poision

//#region BATTLEAREA
//        VectorRect battleArea;
//        List<Hero> capturedHeroes = new List<Hero>();
//        Graphics.Mesh[] walls;
//        void initBattleArea()
//        {
//            const float BattleAreaRadius = Map.WorldPosition.ChunkWidth * 1.5f;
//            //Map.WorldPosition wp = new Map.WorldPosition(battleCenter);
//            battleArea = new VectorRect(new Vector2((battleCenter.X + PublicConstants.Half) * Map.WorldPosition.ChunkWidth - BattleAreaRadius, (battleCenter.Y + PublicConstants.Half) * Map.WorldPosition.ChunkWidth - BattleAreaRadius),  //wp.ToV3() - Vector2.One * BattleAreaRadius + battleCenter.Vec + new Vector2(Map.WorldPosition.ChunkHalfWith), 
//                Vector2.One * BattleAreaRadius * PublicConstants.Twice);
//            //raise four walls
//            const float WallHeight = 9;
//            const float WallThickness = 1;
//            Color wallCol = Color.Black;
//             Graphics.TextureEffect texture =  new Graphics.TextureEffect( Graphics.TextureEffectType.Flat, SpriteName.WhiteArea);
//            walls = new Graphics.Mesh[4];

//            Graphics.Mesh north = new Graphics.Mesh(LoadedMesh.cube_repeating,
//                new Vector3(battleArea.Center.X, Map.WorldPosition.ChunkStandardHeight, battleArea.Position.Y), texture, WallThickness);
//            north.ScaleY = WallHeight;
//            north.ScaleX = battleArea.Width;
//            north.Color = wallCol;

//            Graphics.Mesh south = new Graphics.Mesh(LoadedMesh.cube_repeating,
//                            new Vector3(battleArea.Center.X, Map.WorldPosition.ChunkStandardHeight, battleArea.Bottom), texture, WallThickness);
//            south.ScaleY = WallHeight;
//            south.ScaleX = battleArea.Width;
//            south.Color = wallCol;

//            Graphics.Mesh west = new Graphics.Mesh(LoadedMesh.cube_repeating,
//                new Vector3(battleArea.Position.X, Map.WorldPosition.ChunkStandardHeight, battleArea.Center.Y), texture, WallThickness);
//            west.ScaleY = WallHeight;
//            west.ScaleZ = battleArea.Width;
//            west.Color = wallCol;

//            Graphics.Mesh east = new Graphics.Mesh(LoadedMesh.cube_repeating,
//                new Vector3(battleArea.Right, Map.WorldPosition.ChunkStandardHeight, battleArea.Center.Y), texture, WallThickness);
//            east.ScaleY = WallHeight;
//            east.ScaleZ = battleArea.Width;
//            east.Color = wallCol;

//            walls = new Graphics.Mesh[]
//            {
//                north, south, west, east,
//            };
//        }

//        void updateBattleArea()
//        {
//            //List<Hero> heroes = LfRef.LocalHeroes;// .AllHeroes();
//            //foreach (Hero h in heroes)
//            for (int i = 0; i < LfRef.LocalHeroes.Count; i++)
//            {
//                Hero h = LfRef.LocalHeroes[i];
//                if (capturedHeroes.Contains(h))
//                {//make sure he wont leave
//                    if (h.Alive)
//                    {
//                        if (!battleArea.IntersectPoint(h.PlanePos))
//                        {
//                            h.TakeDamage(new WeaponAttack.DamageData(10, WeaponAttack.WeaponUserType.Neutral, ByteVector2.Zero, Magic.MagicElement.Evil), true);
//                            h.PlanePos = battleArea.KeepPointInsideBound_Position(h.PlanePos);
//                        }
//                    }
//                    else
//                    {
//                        capturedHeroes.Remove(h);
//                    }
//                }
//                else if (h.Alive)
//                {//check if he is close enough to be captured
//                    if (battleArea.IntersectPoint(h.PlanePos))
//                    {
//                        capturedHeroes.Add(h);
//                        if (i == 0)
//                        {
//                            LfRef.gamestate.MusicDirector.BossFight(true);
//                        }
//                    }
//                }
//            }

//            if (!battleArea.IntersectPoint(this.PlanePos))
//                PlanePos = battleArea.KeepPointInsideBound_Position(this.PlanePos);
//        }
//        public override void DeleteMe(bool local)
//        {
//            LfRef.gamestate.MusicDirector.BossFight(false);
//            foreach (Graphics.Mesh mesh in walls)
//            {
//                mesh.DeleteMe();
//            }
//            base.DeleteMe(local);
//        }
//#endregion
//        const float WalkingSpeed = 0.009f;
//        const float LvlWalkingSpeedAdd = 0.001f;
//        const float RunningSpeedMulti = 2f;
//        float circleWindSpeed { get { return walkingSpeed * 0.8f; } }
//        float walkingSpeed { get { return WalkingSpeed + LvlWalkingSpeedAdd * data.BossLevel; } }
//        float runningSpeed { get { return walkingSpeed * RunningSpeedMulti; } }
        
//        AbsCharacter target;
//        AiState state = AiState.Waiting;
//        float stateRefreshTime = 0;
//        WeaponAttack.DamageData wepDamage;
//        float magicDamage;
//        float swordScale;
//        Data.Characters.MagicianData data;
//        IntVector2 battleCenter;

//        public Magician(Map.WorldPosition startPos, Data.Characters.MagicianData data)
//            : base(0)
//        {
//            battleCenter = startPos.ChunkGrindex;
//            this.data = data;
//            WorldPosition = startPos;
//            magicianInit();

//            armor = LootfestLib.HumanoidStandardArmor;
//            target = null;
//            Health = data.BossLevel * LootfestLib.MagicianLvlHealthAdd + LootfestLib.MagicianBaseHealth;
            
//            magicDamage = MagicDamage(data.BossLevel);

//            NetworkShareObject();
//        }

//        public override void ObjToNetPacket(System.IO.BinaryWriter w)
//        {
//            base.ObjToNetPacket(w);

//            w.Write((byte)data.BossLevel);
//            //data.NetworkShare(w);
//            Map.WorldPosition.WriteChunkGrindex_Static(battleCenter, w); //battleCenter.WriteChunkGrindex(w);
//        }

//        public Magician(System.IO.BinaryReader r)
//            : base(r)
//        {
//            //data = new Data.Characters.MagicianData(r);
//            data = LfRef.worldOverView.Boss(r.ReadByte());
//            battleCenter = Map.WorldPosition.ReadChunkGrindex_Static(r);
//            magicianInit();
//        }



//        void magicianInit()
//        {
//            float scale = 3.8f + 0.2f * data.BossLevel;
//            float boundW = scale * 0.22f;
//            CollisionBound = LootFest.ObjSingleBound.QuickBoundingBox(new Vector3(boundW, boundW * 2f, boundW), 1.5f);
//            image = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.magician, TempCharacterImage, scale, 1, 
//                new Graphics.AnimationsSettings(8, 1.1f, 2));
//            image.Position = WorldPosition.ToV3();
//            image.Position.Y = Map.WorldPosition.ChunkStandardHeight;
            
//            Data.ImageAutoLoad.PreLoadImage(VoxelObjName.magician_sword, false, 0);

//            swordScale = image.Scale.X * 1.4f;
//            wepDamage = new WeaponAttack.DamageData(WeaponDamage(data.BossLevel),
//                WeaponAttack.WeaponUserType.Enemy, this.ObjOwnerAndId, Gadgets.GoodsType.Iron, data.AttackMagic);

//            initBattleArea();
//        }


//        public static float WeaponDamage(int bossLvl)
//        {
//            return LootfestLib.MagicianSwordBaseDamage + LootfestLib.MagicianSwordLvlDamageAdd * bossLvl;//data.BossLevel
//        }
//        public static float MagicDamage(int bossLvl)
//        {
//            return WeaponDamage(bossLvl) * 0.6f;
//        }

//        const int NetShareStateTime = 10;

//        void setState(AiState state, ISpottedArrayCounter<GO.AbsUpdateObj> active)
//        {
//            this.state = state;
//            switch (state)
//            {
//                case AiState.Waiting:
//                    Velocity.SetZeroPlaneSpeed();
//                    stateRefreshTime = 200 + Ref.rnd.Int(300);
//                    break;
//                case AiState.MoveTowardsTarget:
//                    target = closestGoodGuy(active);//ClosestHero(image.Position);
//                    stateRefreshTime = 800 + Ref.rnd.Int(800);
//                    break;
//                case AiState.Walking:
//                    rotation = Rotation1D.Random();
//                    Velocity.Set(rotation, walkingSpeed);
//                    stateRefreshTime = 600 + Ref.rnd.Int(1000);
//                    break;
//                case AiState.MagicAttack:
//                    Velocity.SetZeroPlaneSpeed();
//                    rotation.Radians = AngleDirToObject(closestGoodGuy(active));
//                    setImageDirFromRotation();
//                    attack = MagicianAttack.MagicAttack;
//                    stateRefreshTime = 600;
//                    break;
//                case AiState.SwordCirkleWind:
//                    moveTowardsObject(closestGoodGuy(active), 2, circleWindSpeed);
//                    stateRefreshTime = 1400 + Ref.rnd.Int(800);
//                    attack = MagicianAttack.Sword ;

//                    new Timer.Action0ArgTrigger(netSendStateAndTime);
//                    //System.IO.BinaryWriter w = NetworkWriteObjectState((int)state);
//                    //w.Write((byte)(stateRefreshTime / NetShareStateTime));
//                    break;
//                case AiState.Attacking:
//                    Velocity.SetZeroPlaneSpeed();
//                    stateRefreshTime = 400;
//                    attack = MagicianAttack.Sword ;

//                    new Timer.Action0ArgTrigger(netSendStateAndTime);
//                    //System.IO.BinaryWriter w = NetworkWriteObjectState((int)state);
//                    //w.Write((byte)(stateRefreshTime / NetShareStateTime));
//                    break;
//                case AiState.Swordrush:
//                    moveTowardsObject(closestGoodGuy(active), 0, runningSpeed);
//                    stateRefreshTime = 600 + Ref.rnd.Int(800);
//                    attack = MagicianAttack.Sword;

//                    new Timer.Action0ArgTrigger(netSendStateAndTime);
//                    break;
//                case AiState.ThrowSword:
//                    Velocity.SetZeroPlaneSpeed();
//                    stateRefreshTime = 2000;
//                    attack = MagicianAttack.ThrowingSword;

//                    System.IO.BinaryWriter writeThrowSword = BeginWriteObjectStateAsynch(state);
//                    writeThrowSword.Write(rotation.ByteDir);
//                    EndWriteObjectStateAsynch(writeThrowSword);
//                    break;

//            }
//        }

       


//        void netSendStateAndTime()
//        {
//            System.IO.BinaryWriter w = NetworkWriteObjectState(state);
//            w.Write((byte)(stateRefreshTime / NetShareStateTime));
//            Velocity.WriteByteDir(w);
//            //w.Write( Rotation1D.FromDirection(Speed).ByteDir);
//        }

//        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
//        {
//            this.state = state;
//            if (this.state == AiState.SwordCirkleWind || this.state == AiState.Swordrush || this.state == AiState.Attacking)
//            {
//                stateRefreshTime = r.ReadByte() * NetShareStateTime;
//                rotation.ByteDir = r.ReadByte();
//                switch (this.state)
//                {
//                    case AiState.SwordCirkleWind:
//                        Velocity.Set(rotation, circleWindSpeed);
//                        break;
//                    case AiState.Swordrush:
//                        Velocity.Set(rotation, runningSpeed);
//                        break;
//                    case AiState.Attacking:
//                        Velocity.SetZeroPlaneSpeed();
//                        break;
//                }
                
//                createSword();
//            }
//            else if (this.state == AiState.ThrowSword)
//            {
//                rotation.ByteDir = r.ReadByte();
//                throwSword();
//            }
//        }

//        public override void AIupdate(GO.UpdateArgs args)
//        {
//            if (localMember)
//            {
//                switch (state)
//                {
//                    case AiState.MoveTowardsTarget:
//                        if (moveTowardsObject(target, 2, walkingSpeed))
//                        {
//                            stateRefreshTime = 0;
//                        }
//                        break;
//                }

//                //check area bound
//                Velocity.PlaneValue = battleArea.KeepPointInsideBound_Speed(PlanePos, Velocity.PlaneValue);

//                if (stateRefreshTime <= 0)
//                {
//                    AI.SelectRandomState rndState = new AI.SelectRandomState();
//                    switch (state)
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
//            bool immune = data.Immune.DamageSensitive(damage);
//            if (immune && clinkSoundTime <= 0)
//            {
//                Music.SoundManager.PlaySound(LoadedSound.weaponclink, image.Position);
//                clinkSoundTime = 600;
//            }
//            return !immune && base.willReceiveDamage(damage);
//        }
//        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
//        {
//            if (data.Weakness.DamageSensitive(damage))
//                damage.Damage *= LootfestLib.MagicianWeaknessMultiply;
//            base.handleDamage(damage, local);
//        }


//        MagicianAttack attack = MagicianAttack.NONE;
//        public override void Time_LasyUpdate(ref float time)
//        {
//            base.Time_LasyUpdate(ref time);
//            //throwSword();
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            switch (state)
//            {
//                case AiState.SwordCirkleWind:
//                    rotation.Radians += 0.02f * args.time;
//                    break;
//            }
//            stateRefreshTime -= args.time;
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
//                        //if (Tweaking.Debug == BuildDebugLevel.DeveloperDebug_1)
//                        //    data.AttackMagic = Magic.MagicElement.Lightning;

//                        switch (data.AttackMagic)
//                        {
//                            case Magic.MagicElement.Fire:
//                                new Magic.MagicianFireBall(data.BossLevel,
//                                   image.Position + Vector3.Up * 1.5f, rotation);
//                                break;
//                            case Magic.MagicElement.Lightning:
//                                const float Spread = 0.4f;
//                                Rotation1D fireDir = rotation;
//                                fireDir.Add(-Spread);
//                                for (int i = 0; i < 3; i++)
//                                {
//                                    new WeaponAttack.MagicianLightSpark(image.Position, fireDir, areaLevel);
//                                    fireDir.Add(Spread);
//                                }
//                                break;
//                            case Magic.MagicElement.Poision:
//                                new WeaponAttack.ItemThrow.MagicianPoisonBomb(this, areaLevel);
//                                break;
//                            default:
//                                throw new NotImplementedException("Magician fire");

//                        }



//                    }
//                    else if (attack == MagicianAttack.ThrowingSword)
//                    {
//                        throwSword();
//                    }
//                    if (state != AiState.SwordCirkleWind)
//                        stateRefreshTime += 400;
//                    attack = MagicianAttack.NONE;
//                }

//                animationSpeed = Velocity.PlaneLength();
//                image.Position = physics.UpdateMovement();

//                if (state == AiState.SwordCirkleWind)
//                {
//                    setImageDirFromRotation();
//                }
//                else
//                {
//                    setImageDirFromSpeed();
//                }
//                if (!IsDeleted)
//                {
//                    if (!LfRef.chunks.ScreenDataGridLoadingComplete(battleCenter))
//                    {
//                        DeleteMe();
//                    }
//                }
//            }
//            else //clientMember
//            {
//                if (stateRefreshTime <= 0)
//                    state = AiState.Waiting;
//                if (state == AiState.Swordrush || state == AiState.SwordCirkleWind)
//                {
//                    Velocity.PlaneUpdate(args.time, image);

//                    if (state == AiState.Swordrush)
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
            

//            updateBattleArea();
//            image.UpdateAnimation(animationSpeed, args.time);
//            characterCritiqalUpdate(false);
//        }

//        void createSword()
//        {
//            new WeaponAttack.HandWeaponAttack(stateRefreshTime, this,
//                LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.magician_sword, TempSwordImage, swordScale, 0), swordScale,
//                wepDamage, Data.Gadgets.BluePrint.EnchantedSword, localMember);
//        }

//        void throwSword()
//        {
//            new WeaponAttack.MagicianThrowSword(this, runningSpeed * 1.4f, wepDamage, swordScale);
//        }

//        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
//        {

//            //if (localMember)
//            //{
//            //    int numCoins = 4 + Ref.rnd.Int(3);
//            //    int numLoot = 2 + Ref.rnd.Int(3);
//            //    //Vector3 itemPos = image.Position + Vector3.Up * 2;

//            //    for (int i = 0; i < numCoins; i++)
//            //    {
//            //        new PickUp.Coin(LootDropPos(), EnemyValueLevel);
//            //    }
//            //    for (int i = 0; i < numLoot; i++)
//            //    {
//            //        new PickUp.HumanoidLoot(LootDropPos(), EnemyValueLevel);
//            //    }
//            //    new Effects.BossDeathItem(LootDropPos());
//            //}
//            //LfRef.gamestate.Progress.DefeatedBoss(data.BossLevel);

            
//            //if (data.BossLevel == LootfestLib.BossCount - 1)
//            //{
//            //    if (local)
//            //    {
//            //        LfRef.gamestate.GameCompleted(true, battleCenter, LfRef.gamestate.Progress.NumHeroDeaths == 0);
//            //    }
//            //}
//            //else
//            //{
//            //    bool gotHeroInBattle = false;
//            //    for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
//            //    {
//            //        GO.PlayerCharacter.AbsHero h = LfRef.LocalHeroes[i];
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

//        public override float ExspectedHeight
//        {
//            get
//            {
//                return 6;
//            }
//        }
//    }

//    enum MagicianAttack
//    {
//        NONE,
//        Sword,
//        ThrowingSword,
//        MagicAttack,
//    }

//}
