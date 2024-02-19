using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;
//xna
using VikingEngine.LootFest;
using Microsoft.Xna.Framework.Input;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.Input;

namespace VikingEngine.LootFest.GO.PlayerCharacter
{
    abstract class AbsHero : Characters.AbsCharacter, IFirstPerson
    {
        const float DamageImmortalyTimeMs = 500;
        public Players.InputMap inputMap;
        
        Timer.Basic moveHistoryTimer = new Timer.Basic(500, true);
        Vector3 moveHistory1, moveHistory2, moveHistory3;
        public Vector3 NextToHeroPos { get { return moveHistory3; } }

        protected static readonly Vector3 HeroImagePosAdj = Vector3.Up * -3f;
        public bool Immortal = false;
        public HeroPhysics heroPhysics;

        const float MaxAutoAim = 1.6f;
        const float TestHalfSpeed = 0.6f;
        const float HALT_DIST = 50;
        Image interactHudButton;

        public bool LockControls = false; //made by modifiers like spear rush
        bool testMoveX = true;
        
        static readonly Vector3 ButtonRotationSpeed = new Vector3(0.07f, 0, 0);
        
        WeaponAttack.Shield shield = null;
        public VikingEngine.LootFest.GO.EnvironmentObj.AbsCarryObject carryObject = null;
        
        float timeSinceMelee = 0;
        float timeSinceProjectile = 0;
        public Vector3 previousGameObjectGeneratingPos;
        public ForXYLoop gameObjectGeneratingLoop;
        public bool gameObjectGeneratingComplete;

        
        public float TimeSinceMelee
        {
            get {  return timeSinceMelee; }
        }
        public float TimeSinceProjectile
        {
            get { return timeSinceProjectile; }
        }

        protected int CarryIdleFrame = 1;
        protected int CarryJumpFrame = 12;
        protected int CarryWalkStartFrame = 11;
        protected int AttackFrame = 2;
        protected int JumpFrame = 3;
        protected int FoundItemFrame = 4;
        protected int IdleFramesCount = 5;

        protected int WalkingFramesCount = 6;

        float animationTime = 0;
        protected CirkleCounterUp currentWalkingFrame;
        Image redBorder = null;

        public bool DebugMode = false;
        
        protected FloatInBound health;
        protected float storedPlayerHealth;
        protected int armorValue = 0;

        public SpawnDirectorHeroData spawnDirectorHeroData = new SpawnDirectorHeroData();
        public Time slowedByEnemyTrap = Time.Zero;
        public IntVector2? checkPoint = null;
        public VikingEngine.LootFest.Players.AbsPlayerAction mainAction = null, secondaryAction = null; 

        new public float Health
        {
            get { return health.Value; }
            set
            {
                health.Value = value;
                updateBar();
            }
        }

        public int MaxHealth
        {
            get { return (int)health.Bounds.Max; }
        }

        void updateBar()
        {
            if (isMounted)
            {
                if (player.statusDisplay.mountHealthbar != null)
                { player.statusDisplay.mountHealthbar.UpdateHealth(health.Value); }
            }
            else
            {
                player.statusDisplay.healthBar.UpdateHealth(health.Value);
                updateRedBorder();
            }
        }

        public float PercentHealth
        {
            get { return health.Percent; }
        }

        public void updateRedBorder()
        {
            const float ViewRedBorderHealthPercent = 0.35f;
            if (PercentHealth <= ViewRedBorderHealthPercent)
            {
                showRedBorder(true);
                redBorder.Opacity = 1 - (PercentHealth / ViewRedBorderHealthPercent);
            }
            else
                showRedBorder(false);
        }

        public bool FullHealth
        {
            get { return health.Value == health.Bounds.Max; }
        }

        public int Money
        {

            get { return player.Storage.Coins; }
            set {
                player.Storage.Coins = value;
                player.statusDisplay.coinsHud.UpdateAmount(player.Storage.Coins);
            }

        }
        
        public override bool Alive
        {
            get
            {
                return health.Value > 0;
            }
        }

        public override void DeleteMe(bool local)
        {
            health.Value = 0;
            if (shield != null)
                shield.DeleteMe();

            deleteInteractPrompt();
            base.DeleteMe(local);
            showRedBorder(false);
        }

        public int SwordLevel = 0;

        public IntVector2 ChunkUpdateCenter
        {
            get { return ScreenPos; }
        }

        public Players.Player player;
        public Players.ClientPlayer clientPlayer;
        
       
        //public Players.Player Player
        //{ get { return player; } }

        public Players.AbsPlayer absPlayer
        {
            get
            {
                if (localMember)
                    return player;
                else
                    return clientPlayer;
            }
        }
        
        
        IntVector2 currentScreen = IntVector2.Zero;
        IntVector2 currentArea = IntVector2.Zero;

        static List<AbsCharacter> heroes = new List<AbsCharacter>();

        protected override void EnteredNewTile()
        {
        }

        public AbsHero(System.IO.BinaryReader r, Players.ClientPlayer parent)
            : base(new GoArgs(r))
        {
            localMember = false;
            clientPlayer = parent;
            heroBasicInit();
            UpdateAppearance(false);
        }
        
        public AbsHero(Players.Player p)
            : base(GoArgs.Empty)
        {
            player = p;
            heroBasicInit();
            inputMap = p.localPData.inputMap as Players.InputMap;

            HasNetworkClient = true;
            heroPhysics = (HeroPhysics)physics;
            

            image.position = new Vector3(200, Map.WorldPosition.ChunkStandardHeight, 200);
            WorldPos = new Map.WorldPosition(image.position);
            
            
            player.Gear.itemSlot = new Gadgets.Item(this);
        }

        public void InheritPreviousPlayerChar(AbsHero prevChar)
        {
            this.Position = prevChar.Position;
            this.WorldPos = prevChar.WorldPos;
            this.rotation = prevChar.rotation;
            this.Velocity = prevChar.Velocity;
            this.runningDir = prevChar.runningDir;
            if (this.isMounted)
            {
                storedPlayerHealth = prevChar.health.Value;
                storedOriginalMesh = prevChar.originalMesh;
            }
            else if (prevChar.isMounted)
            {
                this.health.Value = prevChar.storedPlayerHealth;
                if (prevChar.IsKilled)
                {
                    this.immortalityTime.MilliSeconds = DamageImmortalyTimeMs;
                    new Effects.DamageFlash(image, immortalityTime.MilliSeconds);
                }
            }
            else
            {
                this.health = prevChar.health;
            }

            setImageDirFromRotation();

            if (prevChar.isMounted && prevChar.storedOriginalMesh != null)
            {
                setModel(prevChar.storedOriginalMesh, VoxelModelName.NUM_NON);
            }
            else
            {
                setModel(prevChar.originalMesh, VoxelModelName.NUM_NON);
            }
        }


        public override void NetWriteUpdate(System.IO.BinaryWriter w)
        {
            base.NetWriteUpdate(w);
            player.netWriteUpdate(w);
        }
        public override void NetReadUpdate(System.IO.BinaryReader r)
        {
            base.NetReadUpdate(r);
            clientPlayer.netReadUpdate(r);
        }
       
        void heroBasicInit()
        {
            float maxHp = this is HorseRidingHero ? LfLib.MountHealth : LfLib.HeroHealth;
            health = new FloatInBound(maxHp, new IntervalF(0, maxHp));

            originalMesh = LfRef.Images.StandardModel_Character;

            image = new Graphics.VoxelModelInstance(originalMesh);
            
            image.Rotation.RotateWorld(new Vector3(MathHelper.Pi, 0, 0));
            image.scale = VectorExt.V3(HeroSize);

            createBounds();
            UpdateBound();
        }

        virtual protected void createBounds()
        {
            this.CollisionAndDefaultBound = HeroCollisionBound();
            this.TerrainInteractBound = HeroTerrainBound();
        }

        Time foundItemAnimationTimer = 0;
        public void foundItemAnimation(VoxelModelName imageType, float imageScale, bool longAnimation)
        {
            Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
            if (longAnimation)
            {
                foundItemAnimationTimer.Seconds = 1.6f;
            }
            else
            {
                foundItemAnimationTimer.Seconds = 0.4f;
            }
            new Effects.FoundItemAnimation(imageType, imageScale, this, foundItemAnimationTimer);
            walkAnimation(0);

            if (player.Gear.suit.shield != null)
            { player.Gear.suit.shield.Hide(foundItemAnimationTimer.MilliSeconds); }
        }

        virtual public GO.Bounds.ObjectBound HeroCollisionBound()
        {
            const float CollRadius = 0.5f * LfLib.ModelsScaleUp;
            const float BoundHeight = 2f * CollRadius;
            return new GO.Bounds.ObjectBound(
              new BoundData2(new VikingEngine.Physics.CylinderBound(
                    new CylinderVolume(Vector3.Zero, BoundHeight, CollRadius)),
                    new Vector3(0, BoundHeight * 0.5f, 0)));
        }
        virtual public GO.Bounds.ObjectBound HeroTerrainBound()
        {
            const float CollRadius = 0.6f * LfLib.ModelsScaleUp;
            const float BoundHeight = 2.0f * CollRadius;
            var bound = new GO.Bounds.ObjectBound(
              new BoundData2(new VikingEngine.Physics.StaticBoxBound(
                    new VectorVolumeC(Vector3.Zero, new Vector3(CollRadius, BoundHeight, CollRadius))),
                    new Vector3(0, BoundHeight * 0.6f, 0)));//offset

            bound.DebugBoundColor(Color.Pink);

            return bound;
        }

        public Vector3 fpsPosition
        {
            get
            {
                return image.position;
            }
        }
        virtual public void setVisibleForFpsCam(bool visible)
        {
            image.Visible = visible;
        }

        public void PickUpCarryObject(VikingEngine.LootFest.GO.EnvironmentObj.AbsCarryObject carryObj)
        {
            this.carryObject = carryObj;
            carryObj.isCarried = true;
            AddChildObject(carryObject);
            player.Gear.suit.gotKeyDown = false;
        }

        public void ThrowCarryObject()
        {
            carryObject.Velocity.Set(FireDir(GameObjectType.ExplodingBarrel), 0.01f);
            carryObject.Velocity.Y = 0.005f;
            carryObject.Velocity.Value += this.Velocity.Value * 0.6f;
            carryObject.physics.WakeUp();

            carryObject.isCarried = false;
            carryObject.isThrown = true;
            carryObject = null;
        }

        public TeleportReason teleportReason;
        public void TeleportFromNowhereTo(IntVector2 worldXZ, TeleportReason reason)
        {
            this.TeleportTo(new WorldPosition(worldXZ, Map.WorldPosition.ChunkStandardHeight), reason);
        }
        public void TeleportFromNowhereTo(WorldPosition wp, TeleportReason reason)
        {
            this.TeleportTo(wp, reason);
        }

        public void TeleportFromTeleportTo(IntVector2 worldXZ, Dir4 enterDirection, TeleportReason reason)
        {
            this.TeleportFromTeleportTo(new WorldPosition(worldXZ, Map.WorldPosition.ChunkStandardHeight), enterDirection, reason);
        }
        public void TeleportFromTeleportTo(WorldPosition wp, Dir4 enterDirection, TeleportReason reason)
        {
            //player.mode = Players.PlayerMode.WaitForTeleport;
            player.startCinematicMode(new Time(10f, TimeUnit.Seconds));
            Visible = false;

        }

        public void TeleportTo(Map.WorldPosition wp, TeleportReason reason)
        {
            teleportReason = reason;
            wp.SetFromHeightMap(1);
            WorldPos = wp;
            CollisionAndDefaultBound.UpdatePosition2(this);

            Position = wp.PositionV3;

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.EnteredLevel, Network.PacketReliability.Reliable);
            ObjOwnerAndId.write(w);
            w.Write((byte)LevelEnum);
        }

        void OnTeleport(LoadingScreen loadingScreen)
        //void OnTeleport(WorldPosition wp)
        {
            //player.mode = Players.PlayerMode.WaitForTeleport;
            player.startCinematicMode(new Time(10f, TimeUnit.Seconds));
            Visible = true;
            Dir4 exitDirection = Dir4.N;

            // move
            WorldPos = loadingScreen.teleportingToWp;
            //WorldPos = wp;
            image.position = WorldPos.PositionV3;
            refreshLightY();
            Position += new Vector3(0, 0, 10);

            // rotate
            Velocity.Value = Vector3.Zero;
            rotation = Rotation1D.FromDir4(exitDirection);
            setImageDirFromRotation();

            // make cinematic script
            //AbsWorldLevel level = LfRef.levels.GetLevelUnsafe(loadingScreen.teleportingToWp.PlaneCoordinates);
            //{
            //    AbsCamera cam = player.pData.view.Camera;
            //    CinematicScriptHandler cc = cam.cinematicControls;
            //    cc.ClearQueue();

            //    // setup start
            //    cc.Enqueue(new CamCmd_UsePosFromRot(false));
            //    cc.Enqueue(new CamCmd_MoveCam(Position, new OnceCriterion()));
            //    cc.Enqueue(new CamCmd_UsePosFromRot(true),
            //               new CamCmd_YawRot(CamCmd_YawRot.DirToAngle(exitDirection), new OnceCriterion()),
            //               new CamCmd_PitchRot(CamCmd_PitchRot.AngleFromSide, new OnceCriterion()),
            //               new CamCmd_Zoom(2, new OnceCriterion()));
            //    cc.Enqueue(new CamCmd_FOV(cam.FieldOfView, new EventCriterion(player.NotInLoadingScreen)));

            //    // set cam over head and move player forward
            //    TimerCriterion timer = new TimerCriterion(0.8f);
            //    AbsCinematicCmd lastCmd = new CamCmd_Zoom(30, timer);
            //    cc.Enqueue(
            //        new CamCmd_YawRot(CamCmd_YawRot.DirToAngle(lib.OppositeDir(exitDirection)), timer),
            //        new CamCmd_PitchRot(MathHelper.Pi / 4, timer),
            //        lastCmd);

            //    lastCmd.OnComplete += lastCmd_OnComplete;
            //}

            Debug.Log("----------------Teleport--------------------");

            //var newSubLvl = LfRef.levels.GetSubLevelUnsafe(WorldPos);
            //if (newSubLvl != subLevel)
            //{
            //    subLevel = LfRef.levels.GetSubLevelUnsafe(WorldPos);

            //    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.EnteredSubLevel, Network.PacketReliability.Reliable);
            //    ObjOwnerAndId.write(w);
            //    AbsWorldLevel.writeSubLevel(subLevel, w);
            //}

            //if (subLevel != null && player != null)
            //{
            //    WorldLevelEnum lvlEnum = subLevel.WorldLevel.LevelEnum;
            //    Debug.Log(lvlEnum.ToString());

            //    if (lvlEnum == Map.WorldLevelEnum.Lobby || lvlEnum == WorldLevelEnum.HappyNpcs)
            //    {
            //        player.Gear.SpecialAttackAmmo = player.Gear.suit.MaxSpecialAmmo;
            //        addHealth(LfLib.HeroHealth, false);
            //    }
            //    else if (lvlEnum > Map.WorldLevelEnum.Lobby)
            //    {
                   
            //    }


            //    if (subLevel.WorldLevel.SuitEnforcement != SuitType.NUM_NON)
            //    {
            //        player.Gear.dressInSuit(subLevel.WorldLevel.SuitEnforcement);
            //    }
            //}
        }

        void lastCmd_OnComplete()
        {
            //if (player.inMenu)
            //    player.mode = Players.PlayerMode.InMenu;
            //else
            //    player.mode = Players.PlayerMode.Play;

            AbsCamera cam = player.localPData.view.Camera;
            //cam.LookTarget = Position;
            player.endCinematicMode();
        }

        public void restartFromKnockout()
        {
            Velocity.SetZero();
            showRedBorder(false);

            Health = health.Bounds.Max;
            DeathAnimation(false);

            //Restart close to friend if any is available
            //for (int i = 0; i < LfRef.AllHeroes.Count; ++i)
            //{
            //    var h = LfRef.AllHeroes[i];
            //    //if (h != this && h.Alive && h.worldLevel == this.worldLevel)
            //    //if (h != this && h.Alive && h.subLevel != null && h.subLevel.WorldLevel == this.subLevel.WorldLevel)
            //    //{
            //    //    //Restart at this hero location
            //    //    WorldPosition restartPos = new Map.WorldPosition(h.NextToHeroPos);
            //    //    TeleportFromNowhereTo(restartPos, TeleportReason.RestartFromDeath);
            //    //    return;
            //    //}
            //}

            //Found no hero, restart at level
            RestartTeleport();
        }


        TeleportLocationId restartLocation = TeleportLocationId.TutorialStart;

        public void RestartTeleport()
        {
            if (checkPoint == null)
            {
                teleportToLocation(restartLocation);
            }
            else
            {
                TeleportTo(new WorldPosition(checkPoint.Value, true), TeleportReason.RestartFromDeath);
            }
        }

        public void teleportToPlayer(AbsHero hero)
        {
            player.CloseMenu();

            var lvl = LfRef.levels2.GetLevelUnsafe(hero.WorldPos.ChunkGrindex);

            if (lvl != null)
            {
                if (lvl != Level)
                {
                    Level = lvl;

                    foreach (var m in LfLib.TeleportLocations)
                    {
                        if (m.level == lvl.LevelEnum && m.levelDefaultRepawn)
                        {
                            restartLocation = m.location;
                            break;
                        }
                    }
                }

                TeleportTo(hero.WorldPos, TeleportReason.Transport);
            }
        }

       

        public void teleportToLocation(TeleportLocationId to)
        {
            player.CloseMenu();
            checkPoint = null;

            var loc = LfLib.Location(to);
            if (loc.setRespawnTo != TeleportLocationId.NUM_NON)
            {
                restartLocation = loc.setRespawnTo;
            }

            if (loc.level == BlockMap.LevelEnum.Lobby)
            {
                LfRef.progress.UnlockProgressPoint(Players.ProgressPoint.MainLobby);
            }

            LfRef.levels2.GetLevel(loc.level, this, startInLocation, to);
        }

        public void teleportInteraction(TeleportLocationId toLocation, bool displayBound, VikingEngine.LootFest.GO.EnvironmentObj.Teleport sender)
        {
            bool locked = player.Storage.progress.teleportIsLocked(toLocation);
            
            if (displayBound)
            {
                if (player.interactDisplay == null ||
                    player.interactDisplay is Display.LevelProgressLabel == false)
                {
                    player.deleteInteractDisplay();
                    player.interactDisplay = new Display.LevelProgressLabel(player, sender);
                }
                else
                {
                    player.interactDisplay.refresh(player, sender);
                }
            }
            else
            {
                if (locked == false)
                {
                    teleportToLocation(toLocation);
                }
            }
        }

        public void teleportWithinLevel(Map.WorldPosition toWp)
        {
            TeleportTo(toWp, TeleportReason.Transport);
        }

        

        //public Players.PlayerCurrentLevelStatus GetLevelProgress()
        //{
        //    Players.PlayerCurrentLevelStatus result = new Players.PlayerCurrentLevelStatus();

        //    if (subLevel != null && subLevel.WorldLevel.LevelEnum > Map.WorldLevelEnum.Lobby)
        //    {
        //        result.continueLevel = subLevel.WorldLevel.LevelEnum;
        //    }
        //    else
        //    {
        //        result.continueLevel = Map.WorldLevelEnum.NUM_NON;
        //    }
        //    result.continueSuit = player.Gear.suit.Type;

        //    return result;
        //}

        public void SetHeroDataFromLevelProgress(SuitType continueSuit)//Players.PlayerCurrentLevelStatus progress)
        {
            //Map.WorldLevelEnum startLevel = Map.WorldLevelEnum.NUM_NON;
            //Debug.CrashIfThreaded();
            //if (progress.CorrectlyLoaded)
            //{
            //    //if (progress.continueLevel != Map.WorldLevelEnum.NUM_NON)
            //    //{
            //    //    startLevel = progress.continueLevel;
            //    //    //if (worldLevel != null)
            //    //    AbsWorldLevel worldLevel = LfRef.worldOverView.GetLevel(startLevel);
            //    //    if (worldLevel != null)
            //    //    {
            //    //        subLevel = worldLevel.subLevels[0];
            //    //    }
            //    //}

                player.Gear.dressInSuit(continueSuit);
                player.Gear.SpecialAttackAmmo = player.Gear.suit.MaxSpecialAmmo;

                TeleportLocationId startAt = TeleportLocationId.Creative;

            //if (player.Storage.progress.StoredProgressPoints.Get(VikingEngine.LootFest.Players.ProgressPoint.TutorialLobby) == false)
            //{
            //    startAt = TeleportLocationId.TutorialStart;
            //}
            //else if (player.Storage.progress.StoredProgressPoints.Get(VikingEngine.LootFest.Players.ProgressPoint.MainLobby) == false)
            //{
            //    startAt = TeleportLocationId.TutorialLobby;
            //}

            //if (PlatformSettings.DevBuild && DebugSett.StartLocation != null)
            //{
            //    startAt = DebugSett.StartLocation.Value;
            //}

            if (startAt != TeleportLocationId.NUM_NON)
            {
                teleportToLocation(startAt);
            }
            //}


            //if (startLevel == Map.WorldLevelEnum.NUM_NON)
            //{
            //    if (player.Storage.completedLevels[(int)Map.WorldLevelEnum.Tutorial].completed)
            //    {
            //        startLevel = Map.WorldLevelEnum.Lobby;
            //    }
            //    else
            //    {
            //        startLevel = Map.WorldLevelEnum.Tutorial;
            //    }
            //}

            //if (DebugSett.StartArea != null && PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
            //{
            //    startLevel = DebugSett.StartArea.Value;
            //}


            //if (startLevel == Map.WorldLevelEnum.Lobby)
            //{
            //Restart(false);
            //}
            //else
            //{
            //    //startInLevel(LfRef.worldOverView.GetLevel(startLevel), true);
            //    LfRef.levels.GetLevel(startLevel, this, startInLevel, true);
            //}
        }

        public void WriteLevelStatus(System.IO.BinaryWriter w, bool inHostileLevel)
        {
            w.Write((byte)player.Gear.suit.Type);
            if (inHostileLevel)
            {
                w.Write(player.Gear.SpecialAttackAmmo);
                w.Write(health.Value);
            }
        }
        public void ReadLevelStatus(System.IO.BinaryReader r, int version, bool inHostileLevel, out SuitType continueSuit)
        {
            continueSuit = (SuitType)r.ReadByte();
            if (inHostileLevel)
            {
                player.Gear.SpecialAttackAmmo = r.ReadInt32();
                health.Value = r.ReadInt32();
            }
        }

        public void startInLocation(VikingEngine.LootFest.BlockMap.AbsLevel lvl, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero, object args)
        {
            TeleportLocationId id = (TeleportLocationId)args;
            var loc = LfLib.Location(id); 
            Level = lvl;
            TeleportTo(loc.wp, TeleportReason.StartPosition);//new WorldPosition(lvl.playerSpawn, 0), TeleportReason.StartPosition);

            if (loc.unlockIds != null)
            {
                arraylib.ListAddIfNotExist(lvl.unlockedAreas, loc.unlockIds);
            }

            if (id == TeleportLocationId.Lobby)//lvl.LevelEnum == BlockMap.LevelEnum.Lobby)
            {
                addHealth(health.Bounds.Max, false);
            }
        }
        //public void startInLevel(Map.AbsWorldLevel lvl, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero, object args)
        //{
        //    // use this to make supply info to teleport cinematics on what direction we start in
        //    //AbsRoomAttr[] attrs = lvl.subLevels[0].Tree.GetRoot().data.Attributes;
        //    //foreach(AbsRoomAttr attr in attrs)
        //    //{
        //    //    if (attr is PlayerSpawnAttribute)
        //    //    {
        //    //        attr.forward;
        //    //    }
        //    //}

        //    bool isRestart = (bool)args;

        //    if (lvl.LevelEnum == Map.WorldLevelEnum.Tutorial)
        //    { player.Gear.SpecialAttackAmmo = 0; }

        //    IntVector2 restartPos = isRestart ? lvl.ReStartWorldPositionXZ : lvl.StartWorldPositionXZ;
        //    TeleportFromNowhereTo(restartPos, isRestart ? TeleportReason.RestartFromDeath : TeleportReason.StartPosition);
        //}
        
        /// <summary>
        /// How much to pay for a continue
        /// </summary>
        public int DeathTax()
        {
            return (int)(Money * 0.5f);
        }

        protected Players.PlayerStorage storage
        {
            get
            {
                return localMember ? player.Storage : clientPlayer.Storage;
            }
        }


        virtual public void UpdateAppearance(bool netShare)
        {
            currentWalkingFrame = new CirkleCounterUp(WalkingFramesCount - 1);

            Players.SuitAppearance suitAppear;
            //if (localMember)
            suitAppear = absPlayer.SuitAppearance;
            //else
            //    suitAppear = new Players.SuitAppearance();

            new HeroAppearance(VoxelModelName.CharacterHD, true, HeroImagePosAdj, storage, suitAppear, setModel, this.Type);
            //new Process.ModifiedImage(this, VoxelModelName.Character,
            //    ImageColorReplace(), ImageAddOns(true), HeroImagePosAdj);
            damageColors = new Effects.BouncingBlockColors((Data.MaterialType)storage.ClothColor, (Data.MaterialType)storage.SkinColor, 
                (Data.MaterialType)storage.hairColor);
            if (netShare)
            {
                player.NetShareAppearance();
            }
        }
        
        protected Graphics.VoxelModel originalMesh;
        protected Graphics.VoxelModel storedOriginalMesh = null;
        
#region EXPRESSION
        Graphics.VoxelModel expressionMesh;
        VoxelModelName lastExpression = VoxelModelName.NUM_NON;
        bool bisyExpressing = false;
        public void Express(VoxelModelName expression)
        {
            if (!bisyExpressing)
            {
                if (localMember)
                {//net share
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.Express,
                        Network.PacketReliability.Reliable, player.PlayerIndex);
                    
                    w.Write((ushort)expression);
                }

                bisyExpressing = true;
                if (lastExpression == expression)
                {
                    startExpression();
                }
                else
                {//load expression

                    new HeroAppearance(expression, false, HeroImagePosAdj, storage, absPlayer.SuitAppearance, setExpressionModel, this.Type); 

                    //List<Process.AddImageToCustom> addons = ImageAddOns(false);
                    //VoxelModelName mouthObjName = VoxelModelName.NUM_NON;
                    //VoxelModelName eyesObjName = VoxelModelName.NUM_NON;
                    //switch (expression)
                    //{
                    //    case VoxelModelName.express_anger:
                    //        eyesObjName = VoxelModelName.EyeFrown;
                    //        mouthObjName = VoxelModelName.MouthSour;
                    //        break;
                    //    case VoxelModelName.express_hi:
                    //        eyesObjName = VoxelModelName.EyeSunshine;
                    //        mouthObjName = VoxelModelName.MouthGasp;
                    //        break;
                    //    case VoxelModelName.express_laugh:
                    //        eyesObjName = VoxelModelName.EyeSunshine;
                    //        mouthObjName = VoxelModelName.mouth_laugh;
                    //        break;
                    //    case VoxelModelName.express_teasing:
                    //        eyesObjName = VoxelModelName.EyeSlim;
                    //        mouthObjName = VoxelModelName.MouthLoony;
                    //        break;
                    //    case VoxelModelName.express_thumbup:
                    //        eyesObjName = VoxelModelName.EyeSunshine;
                    //        mouthObjName = VoxelModelName.MouthHmm;
                    //        break;

                    //}

                    //addons.Add(new Process.AddImageToCustom(eyesObjName, (byte)Data.MaterialType.pale_skin, storage.SkinColor, false));
                    //addons.Add(new Process.AddImageToCustom(mouthObjName, (byte)Data.MaterialType.pale_skin, storage.SkinColor, false));

                    //new Process.ModifiedImage(this, expression,
                    //    ImageColorReplace(), addons, HeroImagePosAdj, (int)expression);
                }

                
            }
        }

        void endExpression()
        {
            image.SetMaster(originalMesh);
            bisyExpressing = false;
        }

        public const float ExpressionTime = 1000;

        void startExpression()
        {
            

            image.SetMaster(expressionMesh);
            new Timer.TimedAction0ArgTrigger(endExpression, ExpressionTime);
            //new Timer.TimedEventTrigger(this, ExpressionTime, TriggerEndExpression);
            Vector3 startPos = image.position;
            startPos.Y += 2;
            //add sound and particles
            LoadedSound sound = LoadedSound.Coin1;

            switch (lastExpression)
            {
                case VoxelModelName.express_anger:
                    sound = LoadedSound.express_anger;
                    const int NumSmokeParticles = 8;
                    List<ParticleInitData> smoke = new List<ParticleInitData>();
                    for (int i = 0; i < NumSmokeParticles; i++)
                    {
                        smoke.Add(new ParticleInitData(Ref.rnd.Vector3_Sq(startPos, 1)));
                    }
                    Engine.ParticleHandler.AddParticles(ParticleSystemType.Smoke, smoke);
                    break;
                case VoxelModelName.express_thumbup:
                    sound = LoadedSound.express_thumbup1;
                    const int NumShinyParticles = 8;
                    List<ParticleInitData> shiny = new List<ParticleInitData>();
                    Vector3 startSpeed = Vector3.Up * 2;
                    for (int i = 0; i < NumShinyParticles; i++)
                    {
                        shiny.Add(new ParticleInitData(Ref.rnd.Vector3_Sq(startPos, 1), Ref.rnd.Vector3_Sq(startSpeed, 1)));
                    }
                    Engine.ParticleHandler.AddParticles(ParticleSystemType.GoldenSparkle, shiny);
                    break;
                case VoxelModelName.express_hi:
                    sound = LoadedSound.express_hi1;
                    break;
                case VoxelModelName.express_laugh:
                    sound = LoadedSound.express_laugh;
                    break;
                case VoxelModelName.express_teasing:
                    sound = LoadedSound.express_teasing1;
                    break;
                case VoxelModelName.express_loot:
                    sound = LoadedSound.NON;
                    new Effects.CoinExpression(this);
                    //sound = LoadedSound.express_teasing1;
                    break;

            }

            
            if (sound != LoadedSound.NON)
            {
                Music.SoundManager.PlaySound(sound, image.position);
            }

            if (absPlayer.Gear.suit.shield != null)
            { absPlayer.Gear.suit.shield.Hide(ExpressionTime); }
        }

        //const int TriggerEndExpression = 0;
        //public void EventTriggerCallBack(int type)
        //{
        //    if (type == TriggerEndExpression)
        //    {
        //        image.SetMaster(originalMesh);
        //        bisyExpressing = false;
        //    }
        //}
        //express_anger, express_hi, express_laugh, express_teasing, express_thumbup,
#endregion

        public const float StandardHeroSize = 0.14f * LfLib.ModelsScaleUp;
        virtual protected float HeroSize { get { return StandardHeroSize; } }

        protected void setModel(Graphics.VoxelModel original, VoxelModelName modelName)
        {
            this.originalMesh = original;
            image.SetMaster(original);
            image.scale = VectorExt.V3(HeroSize);
            lastExpression = VoxelModelName.NUM_NON;
        }

        void setExpressionModel(Graphics.VoxelModel original, VoxelModelName modelName)
        {
            lastExpression = modelName;
            expressionMesh = original;
            startExpression();
        }
        
       
        public void ResetModifiers()
        {
            LockControls = false;
        }
        void showRedBorder(bool show)
        {
            if (show)
            {
                if (redBorder == null)
                {
                    redBorder = new Image(SpriteName.LFEdge, Vector2.Zero, Vector2.Zero, ImageLayers.Background7);
                    redBorder.Color = Color.Red;
                }
                redBorder.Position = player.ScreenArea.Position;
                redBorder.Size = player.ScreenArea.Size;
            }
            else
            {
                if (redBorder != null)
                {
                    redBorder.DeleteMe();
                    redBorder = null;
                }
            }
        }

     

        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (localMember)
            {
                if (mainAction != null && mainAction.BlocksDamage)//player.Gear.suit.specialAttackTimer.MilliSeconds > 0 && player.Gear.suit.Type == SuitType.SpearMan)
                {
                    return false;
                }
                if (foundItemAnimationTimer.TimeOut &&
                    !Immortal &&
                    immortalityTime.TimeOut &&
                    !DebugMode &&
                    Alive &&
                    !player.InLoadingScene)
                {
                    return true;
                }
            }
            return false;
        }

       
        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            if (localMember)
            {
                if (damage.Magic == Magic.MagicElement.Stunn)
                {
                    aiStateTimer.MilliSeconds = 1500;
                    addStunnEffect();
                }
                if (damage.Special == WeaponAttack.SpecialDamage.TerrainDamageFloor)
                { //shoot hero upwards
                    Velocity.Y = 0;
                    physics.Jump(1.6f, image);
                    image.position.Y += 2f;
                }

                if (health.Value > 0 && foundItemAnimationTimer.TimeOut)
                {
                    if (PlatformSettings.ViewErrorWarnings && damage.Damage <= 0)
                    {
                        Debug.LogError("Zero damage");
                    }

                    Debug.CrashIfThreaded();

                    if (damage.Push != WeaponAttack.WeaponPush.NON)
                        new PushForce(damage, this.heroPhysics);

                    if (armorValue > 0)
                    {
                        armorValue--;
                    }
                    else
                    {
                        Health -= damage.Damage;
                    }
                    if (damage.Damage > LfLib.MinVisualDamage)
                    {
                        immortalityTime.MilliSeconds = DamageImmortalyTimeMs;
                        BlockSplatter();
                    }
                    
                    if (health.Value <= 0)
                    {
                        DeathEvent(local, damage);
                        Health = 0;
                    }
                    else
                    {
                        //new Timer.Vibration((int)Player.inputMap.controllerIndex, 300, 0.8f, Pan.Center);
                    }
                    
                    new Process.TakeDamage(player);

                }
            }
            else
            {
                base.handleDamage(damage, local);
            }
            
        }

        public void DeathAnimation(bool set)
        {
            if (set)
            {
                BlockSplatter();
                BlockSplatter();
            }

            image.Visible = !set;
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            IsKilled = true;
            //player.DeathEvent(local);
        }
        public override Rotation1D FireDir(GameObjectType weaponType)
        {
            //get
            //{
                if (player != null)
                {
                    Graphics.AbsCamera cam = Engine.XGuide.GetPlayer(player.PlayerIndex).view.Camera;
                    if (cam.CamType == Graphics.CameraType.FirstPerson)
                    {
                        return new Rotation1D(cam.TiltX - MathHelper.PiOver2);
                    }
                }
                return base.FireDir(weaponType);
            //}
        }
        public override RotationQuarterion FireDir3D(GameObjectType weaponType)
        {
            //get
            //{
                if (player != null)
                {
                    Graphics.AbsCamera cam = Engine.XGuide.GetPlayer(player.PlayerIndex).view.Camera;
                    if (cam.CamType == Graphics.CameraType.FirstPerson)
                    {
                        RotationQuarterion result = RotationQuarterion.Identity;
                        result.RotateWorldX(-cam.TiltX - MathHelper.PiOver2);
                        return result;
                    }
                }
                return base.FireDir3D(weaponType);
            //}
        }

        void heroCollitionCheck2(float time)
        {
            Vector3 savePos = image.position;
            bool xfirst = Ref.rnd.Plus_MinusF(1) > 0;

            testMove(xfirst, time);
            testMove(!xfirst, time);

            moveImage(Velocity.Zero, time);

        }

        virtual public bool PickUpCollect(PickUp.AbsHeroPickUp pickupObj, bool playerSelection, bool collect)
        {
            if (Dead)
            { return false; }

            bool pickup = false;

            switch (pickupObj.Type)
            {
                case GameObjectType.ItemForSale:
                    {
                        var item = (GO.PickUp.ItemForSale)pickupObj;
                        if (item.cost <= Money)
                        {
                            Gadgets.ItemType itype = Gadgets.ItemType.NUM_NON;

                            if (item.saleType == PickUp.ForSaleType.HealthRefill)
                            {
                                pickup = !FullHealth;
                            }
                            else if (item.saleType == PickUp.ForSaleType.SpecialAmmoRefill)
                            {
                                pickup = player.Gear.SpecialAttackAmmo < player.Gear.suit.MaxSpecialAmmo;
                            }
                            else
                            { //Item box
                                
                                switch (item.saleType)
                                {
                                    case PickUp.ForSaleType.ItemApple: itype = Gadgets.ItemType.Apple; break;
                                    case PickUp.ForSaleType.ItemPie: itype = Gadgets.ItemType.ApplePie; break;
                                    case PickUp.ForSaleType.Cards: itype = Gadgets.ItemType.Card; break;
                                    case PickUp.ForSaleType.PickAxe: itype = Gadgets.ItemType.PickAxe; break;
                                }

                                if (player.Gear.itemSlot.Type != itype || player.Gear.itemSlot.amount < player.Gear.itemSlot.MaxAmount)
                                {
                                    pickup = true;
                                }
                            }

                            if (pickup)
                            {
                                if (playerSelection) //player selected to pick item, drop old one
                                {
                                    Money -= item.cost;
                                    player.statusDisplay.coinsHud.UpdateAmount(Money);

                                    switch (item.saleType)
                                    {
                                        case PickUp.ForSaleType.Cards:
                                        case PickUp.ForSaleType.PickAxe:
                                        case PickUp.ForSaleType.ItemPie:
                                        case PickUp.ForSaleType.ItemApple:
                                            
                                            player.Gear.itemSlot.AddItem(itype, true);
                                            break;

                                        

                                        case PickUp.ForSaleType.HealthRefill:
                                            addHealth(health.Bounds.Max, true);
                                            break;

                                        case PickUp.ForSaleType.SpecialAmmoRefill:
                                            player.Gear.SpecialAttackAmmo = player.Gear.suit.MaxSpecialAmmo;
                                            break;
                                    }
                                    //Create box with old stuff
                                    pickupEffeckt();
                                }
                                else
                                {
                                    InteractPrompt_ver2(pickupObj);
                                    pickup = false;
                                }
                            }
                        }

                        
                    }
                    break;

                case GameObjectType.Coin:
                    if (collect)
                    {
                        Money += pickupObj.amount;
                        player.statusDisplay.coinsHud.OnPickUp();
                    }
                    pickup = true;
                    break;
                case GameObjectType.MiningMithril:
                    if (collect)
                    {
                        Level.collectAdd(1);
                        //Level.collect.collectedCount++;
                        //player.refreshLevelCollectItem();
                        //Money += pickupObj.amount;
                        //player.statusDisplay.coinsHud.OnPickUp();
                    }
                    pickup = true;
                    break;
                case GameObjectType.HealUp:
                    if (!FullHealth)
                    {
                        if (collect)
                        {
                            addHealth(pickupObj.amount, true);
                        }
                        pickup = true;
                    }
                    break;
                case GameObjectType.SpecialAmmo1:
                    if (player.Gear.SpecialAttackAmmo < player.Gear.suit.MaxSpecialAmmo)
                    {
                        if (collect)
                        {
                            player.Gear.SpecialAttackAmmo = Bound.Max(player.Gear.SpecialAttackAmmo + pickupObj.amount, player.Gear.suit.MaxSpecialAmmo);
                        }
                        pickup = true;
                    }
                    break;
                case GameObjectType.SpecialAmmoFull:
                    if (player.Gear.SpecialAttackAmmo < player.Gear.suit.MaxSpecialAmmo)
                    {
                        if (collect)
                        {
                            player.Gear.SpecialAttackAmmo = player.Gear.suit.MaxSpecialAmmo;
                        }
                        pickup = true;
                    }
                    break;
                case GameObjectType.ItemBox:
                    {
                        //TODO valbar pickup
                        var itemType = ((PickUp.ItemBox)pickupObj).item;
                        if (player.Gear.itemSlot.Type == itemType)// && player.Gear.itemSlot.amount == player.Gear.itemSlot.MaxAmount)
                        {
                            if (player.Gear.itemSlot.amount == player.Gear.itemSlot.MaxAmount)
                            {
                                //Full of the same items
                                pickup = false;
                            }
                            else //if (player.Gear.itemSlot.amount < player.Gear.itemSlot.MaxAmount || player.Gear.itemSlot.Type == itemType)
                            {
                                //auto pick
                                if (collect)
                                {
                                    player.Gear.itemSlot.AddItem(itemType, true);
                                }
                                pickup = true;
                            }
                        }
                        else
                        {
                            //promt
                            if (playerSelection) //player selected to pick item, drop old one
                            {
                                //Create box with old stuff
                                //throwAwayOldItem(true);

                                player.Gear.itemSlot.AddItem(itemType, true);
                                pickup = true;
                            }
                            else
                            {
                                InteractPrompt_ver2(pickupObj);
                            }
                        }

                        if (pickup && pickupObj.isUnlockItem)
                        {
                            LfRef.progress.UnlockEvent(player, VikingEngine.LootFest.Players.UnlockType.Cards);
                        }
                    }
                    break;
                case GameObjectType.SuitBox:
                    var suitType = ((PickUp.SuitBox)pickupObj).suit;
                    if (player.Gear.suit.Type == SuitType.Basic)
                    {//Auto pick
                        if (collect)
                        {
                           player.Gear.dressInSuit(suitType);
                        }
                        pickup = true;
                        
                    }
                    else
                    {
                        if (playerSelection) //player selected to pick item, drop old one
                        {
                            throwAwayOldItem(false);

                            pickup = true;
                            player.Gear.dressInSuit(suitType);
                        }
                        else
                        {
                            InteractPrompt_ver2(pickupObj);
                        }
                    }
                    break;
                case GameObjectType.CardCapture:
                    if (collect)
                    {
                        CardType capture = pickupObj.CardCaptureType;

                        new Effects.CardCaptureHUDEffect(player, capture);

                        if (storage.progress.cardCollection[(int)capture].TotalCount == 0)
                        {
                            //player.Print("New card: " + capture.ToString());

                            bool gotAllCards = true;
                            for (int i = 0; i < storage.progress.cardCollection.Length; ++i)
                            {
                                if (storage.progress.cardCollection[i].TotalCount <= 0 && i != (int)capture)
                                {
                                    gotAllCards = false;
                                    break;
                                }
                            }
                            if (gotAllCards)
                            {
                                Data.Achievements.UnlockAchievement(Data.AchievementIndex.CaptureAllCardTypes, player);
                                LfRef.progress.UnlockEvent(player, VikingEngine.LootFest.Players.UnlockType.Cape);
                            }
                        }
                        storage.progress.cardCollection[(int)capture] = storage.progress.cardCollection[(int)capture].AddOne();


                    }
                    pickup = true;
                    break;
            }

            if (pickup && collect)
                pickupEffeckt();

            return pickup;
        }

        public void pickMoney(int count)
        {
            Money += count;
            player.statusDisplay.coinsHud.OnPickUp();

            pickupEffeckt();
        }

        public void throwAwayOldItem(bool itemNotSuit)
        {
            Vector3 boxPos = image.position;
            boxPos.Y += 2f;
            boxPos.X += Ref.rnd.Plus_MinusF(1f);
            boxPos.Z += Ref.rnd.Plus_MinusF(1f);


            PickUp.AbsHeroPickUp box = null;
            if (itemNotSuit)
            {
                if (player.Gear.itemSlot.amount > 0)
                {
                    box = new PickUp.ItemBox(new GoArgs(boxPos), player.Gear.itemSlot.Type, player.Gear.itemSlot.amount);
                }
            }
            else
            { box = new PickUp.SuitBox(new GoArgs(boxPos), player.Gear.suit.Type); }

            if (box != null)
            {
                //box.amount = itemSlot.amount;
                box.Velocity = new VikingEngine.Velocity(rotation + Rotation1D.D180, 0.004f);
            }
        }

        


        public void PickUpSmallBoost(bool health_notMagic)
        {
            const float HealthPercentBoost = 0.2f;
            
            if (health_notMagic)
                health.Value += health.Bounds.Max * HealthPercentBoost;
            
            
            updateBar();
            pickupEffeckt();
            new Effects.HealUp(this, health_notMagic, true);
        }

        public void pickupEffeckt()
        {
            Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
            Engine.ParticleHandler.AddExpandingParticleArea(ParticleSystemType.GoldenSparkle, image.position + Vector3.Up * 2, 1, 8, 2);
        }

        public void weaponReadyEffeckt()
        {
            Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);

            const int PCount = 8;
            var particles = new List< ParticleInitData>(PCount);
            //Vector3 center = HeadPosition;
            ParticleInitData p = new ParticleInitData(HeadPosition + Vector3.Up * 2f, Vector3.Up * 5f);

            for (int i = 0; i < PCount; ++i)
            {
                ParticleInitData addP = p;
                addP.Position.X += Ref.rnd.Plus_MinusF(0.4f);
                addP.Position.Y += Ref.rnd.Plus_MinusF(0.2f);
                addP.Position.Z += Ref.rnd.Plus_MinusF(0.4f);

                particles.Add(addP);
            }

            Engine.ParticleHandler.AddParticles(ParticleSystemType.GoldenSparkle, particles);
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Player;
            }
        }

        public bool addHealth(float amount, bool healEffect)
        {
            if (!FullHealth)
            {
                
                Health += amount;
                showRedBorder(false);
                if (healEffect)
                {
                    new Effects.HealUp(this, true, false);
                    Music.SoundManager.PlayFlatSound(LoadedSound.HealthUp);
                }
                return true;
            }
            return false;
        }

        public bool SpendSpecialsAmmo(int cost)
        {
            if (player.Gear.SpecialAttackAmmo >= cost)
            {
                player.Gear.SpecialAttackAmmo -= cost;
                return true;
            }
            return false;
        }

        public void refreshAllHUD()
        {
            player.Gear.itemSlot.refreshHud();
        }
        

        void testMove(bool xdir, float time)
        {
            testMoveX = xdir;
            if (xdir)
            { image.position.X += Velocity.PlaneX * time; }
            else
            { image.position.Z += Velocity.PlaneY * time; }
        }

        public override void OutSideActiveArea()
        {
            //do nothing
        }

        protected override void clientSpeed(float speed)
        {
            walkAnimation(speed);
        }

        virtual protected void walkAnimation(float speed)
        {
            if (foundItemAnimationTimer.MilliSeconds > 0)
            {
                image.Frame = FoundItemFrame;
            }
            else if (inAttackFrame)
            {
                image.Frame = AttackFrame;
            }
            else if (player != null && player.hero.isJumping)
            {
                if (carryObject == null)
                    image.Frame = JumpFrame;
                else
                    image.Frame = CarryJumpFrame;
            }
            else if (speed == 0)
            {
                if (carryObject == null)
                    image.Frame = 0;
                else
                    image.Frame = CarryIdleFrame;
            }
            else
            {
                const float TimePerFrameAndSpeed = 4.2f;
                animationTime += speed * Ref.DeltaTimeMs;
                if (animationTime >= TimePerFrameAndSpeed)
                {
                    animationTime -= TimePerFrameAndSpeed;
                    currentWalkingFrame.Next();
                }
                image.Frame = currentWalkingFrame.Value;
                if (carryObject == null)
                    image.Frame += IdleFramesCount;
                else
                    image.Frame += CarryWalkStartFrame;
            }
        }

        virtual protected bool inAttackFrame
        {
            get { return player == null? false : ((mainAction != null && mainAction.HeroAttackAnimation) || player.Gear.suit.attackAnimationFrame); }
        }

        public static readonly GO.NetworkShare NetShare = new NetworkShare(false, false, false, true);
        override public NetworkShare NetworkShareSettings
        {
            get
            {
                return NetShare;
            }
        }

        public void CheckIfUnderGround()
        {
            physics.CollectObsticles();
            while (physics.ObsticleCollision() != null)
            {
                image.position.Y += 4;
                physics.CollectObsticles();
            }
        }

        virtual public void UpdateInput()
        {
            UpdateWorldPos();
            updateActions();

            if (foundItemAnimationTimer.CountDown())
            {
                //if (player.Gear.suit.specialAttackTimer.MilliSeconds > 0)
                //{
                //    //Special movement
                //    if (player.Gear.suit.Type == SuitType.BarbarianDual)
                //    { updateDualAxeWhirrWind(); }
                //    else
                //    { updateDashAttack(); }
                //}
                if (mainAction == null || !mainAction.OverridesMovement)
                {
                    updateMovement();

                    if (carryObject == null)
                    {
                        if (playerCharacterUsesSuit)
                        { player.Gear.suit.Update(inputMap); }

                        if (inputMap.useItem.DownEvent)//.DownEvent(ButtonActionType.GameUseItem))
                        {
                            if (!player.Gear.itemSlot.Use())
                            {
                                player.statusDisplay.itemHud.emptyBump();
                                LootFest.Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                            }
                        }
                    }
                    else
                    {
                        if (inputMap.mainAttack.DownEvent)//.DownEvent(ButtonActionType.GameMainAttack))
                        {
                            ThrowCarryObject();
                        }
                    }
                }

                UpdateAllChildObjects();

                //DEBUG
                //if (inputMap.useItem.DownEvent)//.DownEvent(ButtonActionType.GameUseItem))
                //{
                //    if (PlatformSettings.DebugOptions && inputMap.IsDown(ButtonActionType.GameAltButton))
                //    {
                //        addHealth(100, false);
                //        player.Gear.SpecialAttackAmmo = 10;

                //        Money += 100;
                //    }
                //}
            }
            else
            {//locked in animation
                Velocity.SetZeroPlaneSpeed();
            }

        }

        void updateActions()
        {
            if (mainAction != null)
            {
                if (mainAction.Update())
                {
                    mainAction.OnPlayerActionComplete();
                    mainAction = null;
                    player.Gear.suit.onActionComplete(true);
                }
            }
            if (secondaryAction != null)
            {
                if (secondaryAction.Update())
                {
                    secondaryAction.OnPlayerActionComplete();
                    secondaryAction = null;
                    player.Gear.suit.onActionComplete(false);
                }
            }
        }
        public bool canPerformAction(bool main)
        {
            if (main)
            {
                return mainAction == null;
            }
            else
            {
                return secondaryAction == null && (mainAction == null || !mainAction.BlocksSecondaryActions);
            }
        }
        public void setTimedMainAction(Time time, bool heroAttackAnimation)
        {
            //new Graphics.Mesh(LoadedMesh.cube_repeating, VectorExt.AddY( HeadPosition, 10), new TextureEffect(TextureEffectType.FixedLight, SpriteName.WhiteArea, Color.DarkGray), 20f);
            BlockSplatter();

            mainAction = new Players.PlayerActionTimer(time, false, heroAttackAnimation);
        }
        

        public void updateDualAxeWhirrWind()
        {
            Vector2 moveinput = player.HeroMoveDir();
            if (moveinput != Vector2.Zero)
            {
                Rotation1D inputDir = Rotation1D.FromDirection(moveinput);
                float angleDiff = player.Gear.suit.forwardDir.AngleDifference(inputDir.Radians);
                if (angleDiff != 0)
                {
                    float maxRotSpeed = 5f * Ref.DeltaTimeSec;
                    if (Math.Abs(angleDiff) <= maxRotSpeed)
                    {
                        player.Gear.suit.forwardDir = inputDir;
                    }
                    else
                    {
                        player.Gear.suit.forwardDir.Add(lib.ToLeftRight(angleDiff) * maxRotSpeed);
                    }
                }
            }

            Velocity.Set(player.Gear.suit.forwardDir, BarbarianDualSuit.WhirrWindMoveSpeed);
            moveImage(Velocity, Ref.DeltaTimeMs);

            player.Gear.suit.Update(inputMap);

            rotation.Add(25f * Ref.DeltaTimeSec);
            setImageDirFromRotation();

            image.position.Y = physics.GetGroundY().slopeY;

            UpdateBound();
            updateWorldBounds();
        }

        public void updateDashAttackMove()
        {
            if (player.Gear.suit.Type == SuitType.Emo)
            {
                Velocity.Set(rotation, EmoSuit.DashMoveSpeed);
            }
            else
            {
                Velocity.Set(rotation, SpearManSuit.DashMoveSpeed);
            }
            moveImage(Velocity, Ref.DeltaTimeMs);

            player.Gear.suit.Update(inputMap);

            image.position.Y = physics.GetGroundY().slopeY;

            UpdateBound();
            updateWorldBounds();
        }

        virtual protected bool playerCharacterUsesSuit { get { return true; } }


        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {

                timeSinceProjectile += args.time;
                timeSinceMelee += args.time;

                updateInteractPrompt_ver2();
            }
            else //clientMember
            {
                UpdateWorldPos();
                base.Time_Update(args);
                UpdateAllChildObjects();
            }

            if (aiStateTimer.CountDown())
            {
                aiState = AiState.NUM_None;
            }


            if (moveHistoryTimer.Update())
            {
                if (image.position != moveHistory1)
                {
                    moveHistory3 = moveHistory2;
                    moveHistory2 = moveHistory1;
                    moveHistory1 = image.position;
                }
            }

            slowedByEnemyTrap.CountDown();
        }
        public void updateMovement()
        {
            if (Ref.TimePassed16ms)
            { updateMoveVelocity(); }

            if (MovementAccPerc == 0)
            {
                lib.DoNothing();
            }
            if (Velocity.ZeroPlaneSpeed)
            {
                walkAnimation(0);
            }
            else
            {
                Velocity move;

                if (aiState == AiState.IsStunned)
                {
                    move = Velocity.Zero;
                }
                else
                {

                    move = movementPercent() * Velocity;
                }

                Velocity.PlaneValue = move.PlaneValue;
                move.Value.Y = 0;

                moveImage(move, Ref.DeltaTimeMs);
                walkAnimation(move.PlaneLength());
                setImageDirFromSpeed();
            }

            image.position.Y = physics.GetGroundY().slopeY;

            UpdateBound();
            updateWorldBounds();
            updateJumping();
        }

        float jumpHoldTimeLeft = 0;
        public bool isJumping = false;
        float prevJumpUpdate = float.MinValue;

        void updateJumping()
        {
            if (heroPhysics.jumpableGround)
            {
                isJumping = false;
                if (inputMap.jump.DownEvent && slowedByEnemyTrap.TimeOut)
                {
                    heroPhysics.Jump(initialJumpForce);
                    jumpHoldTimeLeft = holdJumpMaxTime;
                    isJumping = true;
                }
            }
            else
            {
                if (jumpHoldTimeLeft > 0)
                {
                    if (inputMap.jump.IsDown)
                    {
                        jumpHoldTimeLeft -= Ref.DeltaTimeMs;
                        if (Ref.TimePassed16ms)
                        { Velocity.Y += Engine.Update.Time16msInSeconds * holdJumpForcePerSec; }

                        if (PlatformSettings.DevBuild && prevJumpUpdate == Ref.TotalTimeSec)
                        {
                            //throw new Exception("Jumping gets double update");
                        }
                        prevJumpUpdate = Ref.TotalTimeSec;

                    }
                    else
                    {
                        jumpHoldTimeLeft = 0;
                    }
                }
            }
        }

        virtual protected float movementPercent()
        {
            float LTbreak = Bound.Min(1 - MathExt.Square(inputMap.holdMovement.Value), 0.02f);
            if (LTbreak > 0.9f) LTbreak = 1;

            float result = LTbreak * player.Gear.suit.MovementPerc;

            if (slowedByEnemyTrap.MilliSeconds > 0)
            {
                result *= 0.2f;
            }
            return result;
        }

        virtual protected ButtonActionType[] jumpButtons
        {
            get { return new ButtonActionType[] { ButtonActionType.GameJump }; }
        }

        virtual protected float initialJumpForce { get { return player.Gear.suit.initialJumpForce; } }
        virtual protected float holdJumpMaxTime { get { return player.Gear.suit.holdJumpMaxTime; } }
        virtual protected float holdJumpForcePerSec { get { return player.Gear.suit.holdJumpForcePerSec; } }

        virtual protected float WalkingSpeed { get { return player.Gear.suit.WalkingSpeed; } }
        virtual protected float RunningSpeed { get { return player.Gear.suit.RunningSpeed; } }

        virtual protected float MovementAccPerc { get { return player.Gear.suit.MovementAccPerc; } }
        virtual protected float RunLengthDecrease { get { return player.Gear.suit.RunLengthDecrease; } }
        virtual protected float RunLengthGoal { get { return player.Gear.suit.RunLengthGoal; } }
        virtual protected float KeepOldVelocity { get { return player.Gear.suit.KeepOldVelocity; } }

        void updateWorldBounds()
        {
            levelCollider.updateCollisions();
            //if (subLevel != null)
            //{
            //    subLevel.KeepGOInsidePlayerUnlockedLevelBounds(this);
            //}
        }
        
       

        //private void updatePhysics(UpdateArgs args)
        //{
        //}

        Vector2 runningDir = Vector2.Zero;
        
        /// <summary>
        /// Calc speed from input, add varius speed modifiers and acceleration
        /// </summary>
        void updateMoveVelocity()
        {
            const float DebugSpeed = 0.08f;
            
            Vector2 moveDir = VectorExt.SafeNormalizeV2(player.HeroMoveDir());

            runningDir *= RunLengthDecrease;
            runningDir += moveDir;

            if (float.IsInfinity(runningDir.X) || float.IsInfinity(runningDir.Y))
            {
                throw new Exception();
            }

            bool weaponHalt = false;

            Velocity.MultiplyPlane(KeepOldVelocity); //behåll gamla hastighet

            if (moveDir != Vector2.Zero && !weaponHalt)
            {//add speed
                float maxSpeed = WalkingSpeed;
                if (DebugMode) maxSpeed = DebugSpeed;
                else if (runningDir.Length() >= RunLengthGoal)
                {
                    //is running
                    maxSpeed = RunningSpeed;

                    if ( Ref.rnd.Chance(0.6f))
                    {
                        Engine.ParticleHandler.AddParticles(ParticleSystemType.RunningSmoke, 
                            new ParticleInitData(Ref.rnd.Vector3_Sq(image.position, 0.3f), Vector3.Zero));
                    }
                }
                if (player.Gear.suit.MovementAccPerc == 0)
                {
                    lib.DoNothing();
                }
                Velocity.Add(moveDir * maxSpeed * MovementAccPerc);


                Velocity.SetMaxPlaneSpeed(maxSpeed);

            }
            else
            {//slow down
                if (Velocity.PlaneLength() < 0.001f)
                    Velocity.SetZeroPlaneSpeed();
            }
        }

        

        Map.WorldPosition previousCollisionPos;
        float collectedJumpForce = 0;

        float prevMoveUpdate = float.MinValue;
        protected override void moveImage(Velocity velocity, float time)
        {
            if (PlatformSettings.DevBuild && prevMoveUpdate == Ref.TotalTimeSec)
            {
                Debug.LogError("Moving player gets double update");//throw new Exception("Moving player gets double update");
            }
            prevMoveUpdate = Ref.TotalTimeSec;

            //First make sure he's not inside a collision already
            var collData1 = physics.ObsticleCollision();
            if (collData1 != null)
                HandleColl3D(collData1, null);


            Vector3 oldPos = image.position;
            velocity.Update(time, image);

            physics.Update(time);
            var collData = physics.ObsticleCollision();

            if (collData != null)
            {
                Map.WorldPosition collisionPos = new Map.WorldPosition(collData.OtherBound.center);
                if (jumpableLedge(collisionPos))
                {
                    collectedJumpForce += time;
                    if (collectedJumpForce >= 90)
                    {
                        heroPhysics.Jump(1f);
                        collectedJumpForce = 0;
                    }
                }
                else
                {
                    collectedJumpForce = 0;
                    previousCollisionPos = collisionPos;
                }
                

                image.position = oldPos;
                //Try walking in only one dimention
                bool startDirX = velocity.PlaneX < velocity.PlaneY;
                if (testRunDir(velocity, startDirX, time))
                {
                    image.position = oldPos;
                    if (testRunDir(velocity, !startDirX, time))
                    {
                        //Still colliding, dont move at all
                        image.position = oldPos;
                    }
                }
            }
        }

        bool jumpableLedge(Map.WorldPosition collisionPos)
        {
            bool samePos = previousCollisionPos.WorldGrindex.X == collisionPos.WorldGrindex.X && previousCollisionPos.WorldGrindex.Z == collisionPos.WorldGrindex.Z;
            bool notFalling = Math.Abs(Velocity.Value.Y) < 0.02f;
            float groundY = collisionPos.GetClosestFreeYFloat();
            float ledgeHeight = groundY - WorldPos.WorldGrindex.Y;
            
            bool jumpable = samePos && notFalling && ledgeHeight < 2f;

            if (jumpable)
            {
                collisionPos.WorldGrindex.Y = (int)groundY + 1;
                if (LfRef.chunks.GetScreen(collisionPos).HasFreeSpaceUp(collisionPos, 3) &&
                    LfRef.chunks.GetScreen(WorldPos).HasFreeSpaceUp(WorldPos, 5))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool testRunDir(Velocity v, bool Xdir, float time)
        {
            const float TestStraifSpeed = 0.8f;

            if (Xdir)
            {
                v.Value.Z = 0;
            }
            else
            {
                v.Value.X = 0;
            }
            //base.moveImage(TestStraifSpeed * v, time);
            (v * TestStraifSpeed).Update(time, image);

            return physics.ObsticleCollision() != null;
        }

        //static readonly Vector3 VisualBowPosDiff = new Vector3(0.8f, -0.3f, 0.7f);

        static readonly WeaponAttack.DamageData EvilTouchDamage = new WeaponAttack.DamageData(LfLib.EvilTouchDamage, WeaponAttack.WeaponUserType.NON, NetworkId.Empty, Magic.MagicElement.Evil);

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            if (localMember)
            {
                obsticlePushBack(collData, 0.9f);
            }
            if (ObjCollision != null)
            {
                if (physics != null && ObjCollision is GO.Characters.AbsCharacter && WeaponAttack.WeaponLib.IsFoeTarget(this, ObjCollision, false))
                {
                    //Jumping on enemy head should stun him
                    if (physics.PhysicsStatusFalling && Velocity.Y < 0)
                    {
                        float headY = collData.OtherBound.CenterScale.Center.Y + collData.OtherBound.CenterScale.HalfSize.Y;
                        if (image.position.Y >= headY - 0.6f)
                        {
                            new Timer.Action1ArgTrigger<AbsUpdateObj>(headStomp, ObjCollision);
                        }
                    }
                }
            }
        }

        void headStomp(AbsUpdateObj obj)
        {
            obj.stunForce(1, LfLib.HeroStunDamage, true, true);
            Velocity.Y = Bound.Min(Velocity.Y * -0.8f, 0.05f);

            {//Particles
                Vector3 particleCenter = image.position;
                const int ParticleCount = 16;
                Rotation1D rot = Rotation1D.Random();
                float radStep = MathHelper.TwoPi / ParticleCount;

                Graphics.ParticleInitData p = new Graphics.ParticleInitData();
                var particles = new List<Graphics.ParticleInitData>(ParticleCount * 4);
                for (int i = 0; i < ParticleCount; ++i)
                {
                    Vector3 dir = VectorExt.V2toV3XZ(rot.Direction(1));
                    p.StartSpeed = dir * 16f;//new Vector2toV3(rot.Direction(16));

                    p.Position = particleCenter + dir * 0.1f;
                    particles.Add(p);

                    rot.Add(radStep);
                }
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.BulletTrace, particles);
            }

            player.localPData.inputMap.Vibrate(200, 0f, 1f);
        }

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
        }


        virtual public Vector3 BowFirePos()
        {
            Vector3 firePos = getHeroModel().position;//image.Position;
            firePos.Y += 1.2f;
            firePos += VectorExt.V2toV3XZ(FireDir(GameObjectType.GravityArrow).Direction(1.2f));
            return firePos;
        }

        public Vector3 GetBowTarget()
        {
            AbsUpdateObj closest = null;
            float closestScore = float.MaxValue;
            const float MaxAngle = 1.2f;
            const float MaxLength = 35;
            const float AngleScore = 0.4f;
            const float LengthScore = 0.6f;

            Rotation1D fireDir = this.FireDir(GameObjectType.GravityArrow);

            ISpottedArrayCounter<GO.AbsUpdateObj> active = LfRef.gamestate.GameObjCollection.AllMembersUpdateCounter;
            
            while (active.Next())
            {
                if (active.GetSelection.IsWeaponTarget &&
                    WeaponAttack.WeaponLib.IsFoeTarget(this, active.GetSelection, false))
                {
                    float angle = fireDir.AngleDifference(AngleDirToObject(active.GetSelection));
                    if (Math.Abs(angle) <= MaxAngle)
                    {
                        float length = PositionDiff3D(active.GetSelection).Length();
                        if (length <= MaxLength)
                        {
                            float score = length / MaxLength * LengthScore +
                                angle / MaxAngle * AngleScore;
                            if (score < closestScore)
                            {
                                closest = active.GetSelection;
                                closestScore = score;
                            }
                        }
                    }
                }
            }

            if (closest == null)
            {
                const float NoTargetAimLength = 25;
                Vector3 result = image.position;
                result += VectorExt.V2toV3XZ(fireDir.Direction(NoTargetAimLength));
                return result;
            }

            return closest.HeadPosition;
        }

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
            }
        }


        public void GotChatMessage()
        {//to avoid see everyone talk at the same time
            openingPhraseTime = 0;
        }
        float openingPhraseTime = 0;

#region INTERACT_VERSION2
        AbsUpdateObj currentInteractObj_ver2 = null;
        const float InteractPromptTime = 300;
        Time interactTimer_ver2 = 0;
        Image interactHudIcon;

        public void InteractPrompt_ver2(AbsUpdateObj interactingWith)
        {
            if (Alive &&
                (player.interactDisplay == null || !player.interactDisplay.overrideInteractInput))
            {
                if (interactingWith.Interact_AutoInteract)
                {
                    interactingWith.InteractVersion2_interactEvent(this, interactingWith != currentInteractObj_ver2);
                    this.currentInteractObj_ver2 = interactingWith;
                }
                else
                {
                    if (interactingWith != currentInteractObj_ver2)
                    {
                        viewInteractHUD(interactingWith);
                    }
                    interactTimer_ver2.MilliSeconds = InteractPromptTime;
                }
            }
        }

        void viewInteractHUD(AbsUpdateObj interactingWith)
        {
            const ImageLayers Layer = ImageLayers.Background5;

            deleteInteractPrompt();

            Vector2 center = player.ScreenArea.Center;
            Vector2 iconScale = new Vector2(Engine.Screen.IconSize);
            float halfspacing = iconScale.X * 0.5f;

            interactHudButton = new Image(inputMap.interact.Icon, center, iconScale, Layer, true, true);//inputMap.ButtonIcon( ButtonActionType.GameInteract)
            interactHudButton.Xpos -= halfspacing;
            if (!interactingWith.Interact_Enabled)
            {
                interactHudButton.Color = Color.DarkGray;
            }

            interactHudIcon = new Image(interactingWith.InteractVersion2_interactIcon,
                center, iconScale, Layer, true);
            interactHudIcon.Xpos += halfspacing;

            this.currentInteractObj_ver2 = interactingWith;
        }

        void deleteInteractPrompt()
        {
            if (interactHudIcon != null)
            {
                interactHudIcon.DeleteMe();
                interactHudButton.DeleteMe();
                interactHudIcon = null;
                interactHudButton = null;
            }
        }

        void updateInteractPrompt_ver2()
        {
            if (currentInteractObj_ver2 != null)
            { 

                if (inputMap.interact.DownEvent)//.DownEvent(ButtonActionType.GameInteract))//controller.KeyDownEvent(Buttons.Y))
                {
                    currentInteractObj_ver2.InteractVersion2_interactEvent(this, true);
                }

                if (interactTimer_ver2.CountDown() || !currentInteractObj_ver2.Alive)
                {
                    deleteInteractPrompt();
                    currentInteractObj_ver2 = null;
                }
            }
        }


#endregion

        
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.Hero;
            }
        }

        public override void Time_LasyUpdate(ref float time)
        {
            openingPhraseTime += time;
            immortalityTime.CountDown(time);
            
            if (WorldPos.ChunkGrindex.X != currentScreen.X || WorldPos.ChunkGrindex.Y != currentScreen.Y)
            {
                currentScreen = WorldPos.ChunkGrindex;
            }

            if (CheckOutSideWorldsBounds())
            {
                player.Print("Invisible wall!");
            }
            //base.Time_LasyUpdate(ref time);
            updatePositionToNewbie--;

            

        }
        public IntVector2 AreaPos
        {
            get { return currentArea; }
        }

        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.Plane1D;//.FromSpeed;
            }
        }
        override protected Color damageTextCol { get { return Color.Red; } }


        protected Effects.BouncingBlockColors damageColors = StandardDamageColors;//new Effects.BouncingBlockColors(Data.MaterialType.red,,
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return damageColors;
            }
        }

        public override Graphics.LightSourcePrio LightSourcePrio
        {
            get
            {
                return Graphics.LightSourcePrio.High;
            }
        }
        public override Vector3 LightSourcePosition
        {
            get
            {
                return base.LightSourcePosition;
            }
        }

        public override float LightSourceRadius
        {
            get
            {
                return 5f;
            }
        }
        public override bool HasNetId
        {
            get
            {
                return true;
            }
        }

        public override Vector3 HeadPosition
        {
            get
            {
                Vector3 result = image.position;
                result.Y += 1f;
                return result;
            }
        }


        static readonly Vector3 FireProjectileOffset = new Vector3(0.4f, 0, 0);
        virtual public Vector3 FireProjectilePosition
        {
            get
            {
                return this.FireDir3D(GameObjectType.NUM_NON).TranslateAlongAxis(FireProjectileOffset, image.position);
            }
        }

        virtual public Graphics.AbsVoxelObj getHeroModel()
        {
            return image;
        }

        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, 3, 0);
            }
        }

        public bool isInLevel(BlockMap.LevelEnum lvl)
        {
            return LevelEnum == lvl;
        }
        //public bool isInLevel(Map.AbsWorldLevel lvl)
        //{
        //    return subLevel != null && subLevel.WorldLevel == lvl;
        //}

        public override void onLevelDestroyed(BlockMap.AbsLevel level)
        {
            //do nothing
        }

        public void returnToHumanForm()
        {
            if (player.statusDisplay.mountHealthbar != null)
            {
                player.statusDisplay.mountHealthbar.DeleteMe();
                player.statusDisplay.mountHealthbar = null;
            }
            player.setNewHero(new Hero(player), true);
        }

        virtual public void dismount()
        {
            throw new NotImplementedException();
        }

        virtual public bool isMounted { get { return false; } }

        virtual public void setCamera()
        { }

        override public bool IsHero()
        {
            return true;
        }

        
    }

    class UpdateApperanceTimer : LazyUpdate
    {
        AbsHero h;
        float wait = 4000;
        public UpdateApperanceTimer(AbsHero h)
            :base(true)
        {
            this.h = h;
        }

        public override void Time_Update(float time)
        {
            wait -= time;
            if (wait <= 0)
            {
                h.UpdateAppearance(true);
                DeleteMe();
            }

        }
    }
}
