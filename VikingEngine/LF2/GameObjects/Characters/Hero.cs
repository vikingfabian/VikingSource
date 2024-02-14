using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Voxels;
//

using VikingEngine.LF2;
using VikingEngine.LootFest;

namespace VikingEngine.LF2.GameObjects.Characters
{
    class Hero : AbsCharacter, IFirstPerson, Process.ILoadImage, Timer.IEventTriggerCallBack, IMiniMapLocation
    {
        #region MINI_MAP
        public IntVector2 MapLocationChunk { get { return WorldPosition.ChunkGrindex; } }
        public IntVector2 TravelEntrance { get { return WorldPosition.ChunkGrindex; } }
        public SpriteName MiniMapIcon { get {
            return (clientPlayer != null && clientPlayer.Host) ?
                SpriteName.IconHost : SpriteName.IconClient;
            }
        }
        public string MapLocationName 
        { 
            get {
                if (clientPlayer != null)
                    return clientPlayer.Name;
                else
                    return player.Name;
            }
        }
        public bool VisibleOnMiniMap { get { return false; } }
        public bool TravelTo { get { return true; } }
        #endregion

        static readonly Vector3 HeroImagePosAdj = Vector3.Up * -3.5f;
        List<Characters.Condition.AbsHeroCondition> modifiers = new List<Condition.AbsHeroCondition>();
        public bool Immortal = false;
        //Edge jump
        //float edgePressure = 0;
        //Vector3 edgePressurePos;
        HeroPhysics heroPhysics;

        const float MaxAutoAim = 1.6f;
        const float TestHalfSpeed = 0.6f;
        const float HALT_DIST = 50;

        TextG actionButtonText;
        Image actionButtonImage;

        public bool LockControls = false; //made by modifiers like spear rush
        bool testMoveX = true;
        
        float currentFireReloadtime = float.MaxValue;
        Gadgets.WeaponGadget.AbsWeaponGadget2 currentFire = null;
        bool currentAttackKeyDown = false;
        public bool IsAttacking
        {
            get { return currentFire != null; }
        }
        Gadgets.WeaponGadget.AbsWeaponGadget2 usingWeapon = null;
        
        Players.EquipSlot currentFireButtonIx;
        Gadgets.GadgetAlternativeUseType currentFireAltUse;

        static readonly Vector3 ButtonRotationSpeed = new Vector3(0.07f, 0, 0);
        
        WeaponAttack.Shield shield = null;
        Effects.VisualBow visualBow;

        float timeSinceMelee = 0;
        float timeSinceProjectile = 0;
        int killsWithoutBeingHit = 0;
        public float TimeSinceMelee
        {
            get {  return timeSinceMelee; }
        }
        public float TimeSinceProjectile
        {
            get { return timeSinceProjectile; }
        }


        public void UpdateShield()
        {
            if (shield != null)
                shield.DeleteMe();
            GameObjects.Gadgets.Shield data;
            if (player != null)
                data= player.Progress.Shield();
            else
                data = clientPlayer.Progress.Shield();
            if (data != null)
                shield = new WeaponAttack.Shield(data, this);
        }
        public Vector3 ShieldPos
        {
            get
            {
                if (shield == null)
                    return image.position;
                return shield.Position;
            }
        }
        public bool WeaponShieldCheck(AbsUpdateObj weapon)
        {
            if (shield != null)
            {
                return shield.WeaponShieldCheck(weapon);
            }
            return false;
        }
        Timer.Basic flyingPetResetTimer = new Timer.Basic(lib.SecondsToMS(FlyingPet.ResetPetTimeSec), true);
        FlyingPet flyingPet = null;

        float animationTime = 0;
        CirkleCounterUp currentFrame = new CirkleCounterUp(5);
        Image redBorder = null;

        public bool DebugMode = false;
        
        FloatInBound health = new FloatInBound(LootfestLib.HeroHealth, new IntervalF(0, LootfestLib.HeroHealth));
        FloatInBound magic = new FloatInBound(LootfestLib.HeroMagicBar, new IntervalF(0, LootfestLib.HeroMagicBar));
        Health healthBar;

        public float Health
        {
            get { return health.Value; }
            set
            {
                health.Value = value;
                updateBar();
            }
        }
        public bool FullMagic
        {
            get { return magic.IsMax; }
        }

        void updateBar()
        {
            healthBar.UpdateValue(new Percent(health.Percent), new Percent(magic.Percent), 
                player.Progress.Items.GetItemAmount(Gadgets.GoodsType.Water_bottle), drinkTimer.Active);
            updateRedBorder();
        }

        public float PercentHealth
        {
            get { return health.Percent; }
        }

        void updateRedBorder()
        {
            const float ViewRedBorderHealthPercent = 0.35f;
            if (PercentHealth <= ViewRedBorderHealthPercent)
            {
                showRedBorder(true);
                redBorder.Transparentsy = 1 - (PercentHealth / ViewRedBorderHealthPercent);
            }
            else
                showRedBorder(false);
        }

        static readonly float DrinkWaterTime = lib.SecondsToMS(6);
        Timer.Basic drinkTimer;
        public bool SpendMagic(float amount)
        {

            if (!drinkTimer.Active &&
                (magic.Value > 0 || player.Progress.Items.GetItemAmount(Gadgets.GoodsType.Water_bottle) > 0))
            {
                if (amount >= magic.Value)
                {
                    DrinkWaterBottle();
                    magic.Value = 0;
                }
                else
                {
                    magic.Value -= amount;

                }
                updateBar();
                return true;
            }

            Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
            return false;
        }

        public bool DrinkWaterBottle()
        {
            if (player.Progress.Items.RemoveItem(Gadgets.GoodsType.Water_bottle))
            {
                //Drank one bottle
                drinkTimer = new Timer.Basic(DrinkWaterTime, false);
                new Effects.DrinkingWater(this, DrinkWaterTime);
                return true;    
            }
            return false;
        }

        public bool FullHealth
        {
            get { return health.Value == health.Bounds.Max; }
        }


        EnvironmentObj.IInteractionObj interactingWith;
        public EnvironmentObj.IInteractionObj InteractingWith
        {
            get
            {
                return interactingWith;
            }
            set
            {
#if WINDOWS
                Debug.DebugLib.CrashIfThreaded();
#endif
                interactingWith = value;
                if (interactingWith == null)
                    RemoveActionPrompt();
            }
        }

        int hawks = 0;
        public int Hawks
        { get { return hawks; } }
        int pigeons = 2;
        public int Pidgins
        { get { return pigeons; } }

        public int Money
        { 

            get { return player.Progress.Items.Money; }
            set { player.Progress.Items.Money = value; }

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
            base.DeleteMe(local);

            if (redBorder != null)
                redBorder.DeleteMe();
        }

        public int SwordLevel = 0;

        public IntVector2 ChunkUpdateCenter
        {
            get { return player.ChunkUpdateCenter; }
        }

        Players.Player player;
        public Players.ClientPlayer clientPlayer;
        
        Players.AI ai = null;
        public Players.AI AI
        {
            set { ai = value; }
        }
        public bool IsBot
        { get { return ai != null; } }
        public Players.Player Player
        { get { return player; } }
        
        
        IntVector2 currentScreen = IntVector2.Zero;
        IntVector2 currentArea = IntVector2.Zero;
        float busyAttackingTime = 0;

        static List<AbsCharacter> heroes = new List<AbsCharacter>();

        protected override void EnteredNewTile()
        {

            LfRef.gamestate.Progress.SetVisitedArea(WorldPosition.ChunkGrindex, null, false);

        }

        public Hero(System.IO.BinaryReader r, Players.ClientPlayer parent)
            : base(r)
        {
            //ObjOwnerAndId = ByteVector2.FromStream(r);

            clientPlayer = parent;
            //basicInit();
            UpdateAppearance(false);
        }
        
        public Hero(Players.Player p)
            : base()
        {
            HasNetworkClient = true;
            heroPhysics = (HeroPhysics)physics;
            player = p;

            image.position = new Vector3(200, Map.WorldPosition.ChunkStandardHeight, 200);
            WorldPosition = new Map.WorldPosition(image.position);

            new UpdateApperanceTimer(this);
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            //ObjOwnerAndId.WriteStream(w);
        }
       
        protected override void basicInit()
        {
            base.basicInit();
            originalMesh = LootfestLib.Images.StandardAnimatedVoxelObjects[VoxelModelName.Character];

            image = new Graphics.VoxelModelInstance(originalMesh);
            
            image.Rotation.RotateWorld(new Vector3(MathHelper.Pi, 0, 0));
            image.scale = lib.V3(HeroSize);
            this.CollisionBound = HeroCollisionBound();
            this.TerrainInteractBound = HeroTerrainBound();

            UpdateBound();

            //LfRef.gamestate.RefreshAllHeroesList();
        }
        
        public static LF2.ObjSingleBound HeroCollisionBound()
        {
            const float CollRadius = 0.5f;
            const float BoundHeight = 2f * CollRadius;
            return new LF2.ObjSingleBound(
              new BoundData2(new Physics.CylinderBound(
                    new CylinderVolume(Vector3.Zero, BoundHeight, CollRadius)),
                    new Vector3(0, BoundHeight * 0.5f, 0)));
        }
        public static LF2.ObjSingleBound HeroTerrainBound()
        {
            const float CollRadius = 0.6f;
            const float BoundHeight = 2.0f * CollRadius;
            return new LF2.ObjSingleBound(
              new BoundData2(new Physics.StaticBoxBound(
                    new VectorVolume(Vector3.Zero, new Vector3(CollRadius, BoundHeight, CollRadius))),
                    new Vector3(0, BoundHeight * 0.6f, 0)));//offset
        }

        public void JumpTo(IntVector2 chunkIx)
        {

            LfRef.gamestate.Progress.SetVisitedArea(chunkIx, null, false);

            this.JumpTo(Map.WorldPosition.V2toV3(chunkIx.Multiply(Map.WorldPosition.ChunkWidth).Vec + Vector2.One * Map.WorldPosition.ChunkHalfWidth, image.position.Y));
            System.Diagnostics.Debug.WriteLine("----------------JUMP TRAVEL--------------------");
        }
        public void JumpTo(Vector3 pos)
        {
            image.position = pos;
            WorldPosition = new Map.WorldPosition(image.position);
        }

        IntVector2 homePos
        {
            get
            {
                return LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.HomeBase][0].position;
            }
        }

        public void Restart(bool stayInLocation)
        {
            showRedBorder(false);
            
            Health = health.Bounds.Max;
            magic.Value = magic.Bounds.Max;
            DeathAnimation(false);
            if (!stayInLocation)
            {

                IntVector2 restartPos;
                if (Player.Settings.UnlockedPrivateHome && Player.Settings.SpawnAtPrivateHome)
                {
                    restartPos = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.PrivateHome][Player.PrivateHomeIndex].position;
                }
                else
                {
                    restartPos = homePos;
                    restartPos.X += 1;
                }

                JumpTo(restartPos);        

            }
            
        }
        
        /// <summary>
        /// How much to pay for a continue
        /// </summary>
        public int DeathTax()
        {
            return (int)(Money * 0.5f);
        }

        Players.PlayerSettings settings
        {
            get
            {
                return localMember ? player.Settings : clientPlayer.Settings;
            }
        }

        public List<Process.AddImageToCustom> ImageAddOns(bool faceExpression)
        {
            settings.HatType = (Players.HatType)Bound.Set((int)settings.HatType, 0, (int)Players.HatType.NUM - 1);
            settings.BeardType = (Players.BeardType)Bound.Set((int)settings.BeardType, 0, (int)Players.BeardType.NUM - 1);
            settings.MouthType = (Players.MouthType)Bound.Set((int)settings.MouthType, 0, (int)Players.MouthType.NUM - 1);
            settings.EyesType = (Players.EyeType)Bound.Set((int)settings.EyesType, 0, (int)Players.EyeType.NUM - 1);
            settings.BodyType = (Players.BodyType)Bound.Set((int)settings.BodyType, 0, (int)Players.BodyType.NUM - 1);
            settings.BeltType = (Players.BeltType)Bound.Set((int)settings.BeltType, 0, (int)Players.BeltType.NUM - 1);

            VoxelModelName mouthObjName = VoxelModelName.NUM_NON;
            VoxelModelName eyesObjName = VoxelModelName.NUM_NON;
            VoxelModelName beardObjName = VoxelModelName.NUM_NON;
            VoxelModelName helmetObjName = VoxelModelName.NUM_NON;
            List<Process.AddImageToCustom> addItems = new List<Process.AddImageToCustom>();
            if (settings.BeardType != Players.BeardType.Shaved)
            {
                switch (settings.BeardType)
                {
                    default: //small beard
                        beardObjName = VoxelModelName.BeardSmall;
                        break;
                    case Players.BeardType.BeardLarge:
                        beardObjName = VoxelModelName.BeardLarge;
                        break;
                    case Players.BeardType.MustacheBiker:
                        beardObjName = VoxelModelName.MustacheBikers;
                        break;
                    case Players.BeardType.MustachePlummer:
                        beardObjName = VoxelModelName.Mustache;
                        break;

                }
                addItems.Add(new Process.AddImageToCustom(beardObjName, (byte)Data.MaterialType.dirt, settings.BeardColor, false));
            }

            if (faceExpression)
            {
                switch (settings.MouthType)
                {
                    case Players.MouthType.BigSmile:
                        mouthObjName = VoxelModelName.MouthBigSmile;
                        break;
                    case Players.MouthType.Hmm:
                        mouthObjName = VoxelModelName.MouthHmm;
                        break;
                    case Players.MouthType.Loony:
                        mouthObjName = VoxelModelName.MouthLoony;
                        break;
                    case Players.MouthType.OMG:
                        mouthObjName = VoxelModelName.MouthOMG;
                        break;
                    case Players.MouthType.Orc:
                        mouthObjName = VoxelModelName.MouthOrch;
                        break;
                    case Players.MouthType.Smile:
                        mouthObjName = VoxelModelName.MouthSmile;
                        break;
                    case Players.MouthType.Sour:
                        mouthObjName = VoxelModelName.MouthSour;
                        break;
                    case Players.MouthType.Straight:
                        mouthObjName = VoxelModelName.MouthStraight;
                        break;
                    case Players.MouthType.WideSmile:
                        mouthObjName = VoxelModelName.MouthWideSmile;
                        break;
                    case Players.MouthType.Laugh:
                        mouthObjName = VoxelModelName.mouth_laugh;
                        break;
                    case Players.MouthType.Girly1:
                        mouthObjName = VoxelModelName.MouthGirly1;
                        break;
                    case Players.MouthType.Girly2:
                        mouthObjName = VoxelModelName.MouthGirly2;
                        break;
                    case Players.MouthType.Gasp:
                        mouthObjName = VoxelModelName.MouthGasp;
                        break;
                    case Players.MouthType.Pirate:
                        mouthObjName = VoxelModelName.MouthPirate;
                        break;

                }
                switch (settings.EyesType)
                {
                    case Players.EyeType.Cross:
                        eyesObjName = VoxelModelName.EyeCross;
                        break;
                    case Players.EyeType.Evil:
                        eyesObjName = VoxelModelName.EyeEvil;
                        break;
                    case Players.EyeType.Frown:
                        eyesObjName = VoxelModelName.EyeFrown;
                        break;
                    case Players.EyeType.Loony:
                        eyesObjName = VoxelModelName.EyeLoony;
                        break;
                    case Players.EyeType.Normal:
                        eyesObjName = VoxelModelName.EyeNormal;
                        break;
                    case Players.EyeType.Red:
                        eyesObjName = VoxelModelName.EyeRed;
                        break;
                    case Players.EyeType.Sleepy:
                        eyesObjName = VoxelModelName.EyeSleepy;
                        break;
                    case Players.EyeType.Slim:
                        eyesObjName = VoxelModelName.EyeSlim;
                        break;
                    case Players.EyeType.Sunshine:
                        eyesObjName = VoxelModelName.EyeSunshine;
                        break;
                    case Players.EyeType.Crossed1:
                        eyesObjName = VoxelModelName.EyesCrossed1;
                        break;
                    case Players.EyeType.Crossed2:
                        eyesObjName = VoxelModelName.EyesCrossed2;
                        break;
                    case Players.EyeType.Cyclops:
                        eyesObjName = VoxelModelName.EyesCyclops;
                        break;

                    case Players.EyeType.Girly1:
                        eyesObjName = VoxelModelName.EyesGirly1;
                        break;
                    case Players.EyeType.Girly2:
                        eyesObjName = VoxelModelName.EyesGirly2;
                        break;
                    case Players.EyeType.Girly3:
                        eyesObjName = VoxelModelName.EyesGirly3;
                        break;
                    case Players.EyeType.Pirate:
                        eyesObjName = VoxelModelName.EyesPirate;
                        break;

                }
                addItems.Add(new Process.AddImageToCustom(eyesObjName, (byte)Data.MaterialType.skin, settings.SkinColor, false));
                addItems.Add(new Process.AddImageToCustom(mouthObjName, (byte)Data.MaterialType.skin, settings.SkinColor, false));
            }
            if (settings.HatType != Players.HatType.None)
            {


                switch (settings.HatType)
                {
                    default: //vendel
                        helmetObjName = VoxelModelName.HelmetVendel;
                        break;
                    case Players.HatType.Viking1:
                        helmetObjName = VoxelModelName.HelmetHorned1;
                        break;
                    case Players.HatType.Viking2:
                        helmetObjName = VoxelModelName.HelmetHorned2;
                        break;
                    case Players.HatType.Viking3:
                        helmetObjName = VoxelModelName.HelmetHorned3;
                        break;
                    case Players.HatType.Viking4:
                        helmetObjName = VoxelModelName.HelmetHorned4;
                        break;
                    case Players.HatType.Knight:
                        helmetObjName = VoxelModelName.HelmetKnight;
                        break;
                    case Players.HatType.Cap:
                        helmetObjName = VoxelModelName.HatCap;
                        break;
                    case Players.HatType.Football:
                        helmetObjName = VoxelModelName.HatFootball;
                        break;
                    case Players.HatType.Spartan:
                        helmetObjName = VoxelModelName.HatSpartan;
                        break;
                    case Players.HatType.Witch:
                        helmetObjName = VoxelModelName.HatWitch;
                        break;
                    case Players.HatType.Pirate1:
                        helmetObjName = VoxelModelName.HatPirate1;
                        break;
                    case Players.HatType.Pirate2:
                        helmetObjName = VoxelModelName.HatPirate2;
                        break;
                    case Players.HatType.Pirate3:
                        helmetObjName = VoxelModelName.HatPirate3;
                        break;
                    case Players.HatType.Girly1:
                        helmetObjName = VoxelModelName.HairGirly1;
                        break;
                    case Players.HatType.Girly2:
                        helmetObjName = VoxelModelName.HairGirly2;
                        break;


                }
                addItems.Add(new Process.AddImageToCustom(helmetObjName, new List<ByteVector2>
                {
                   new ByteVector2( (byte)Data.MaterialType.iron,settings.HatMainColor),
                    new ByteVector2((byte)Data.MaterialType.gold, settings.HatDetailColor),
                }, false));
            }

            if (settings.BeltType != Players.BeltType.No_belt)
            {
                VoxelModelName beltName = VoxelModelName.NUM_Empty;
                switch (settings.BeltType)
                {
                    case Players.BeltType.Slim:
                        beltName = VoxelModelName.belt_slim;
                        break;
                    case Players.BeltType.Thick:
                        beltName = VoxelModelName.belt_thick;
                        break;
                }
                addItems.Add(new Process.AddImageToCustom(beltName, new List<ByteVector2>
                {
                   new ByteVector2( (byte)Data.MaterialType.leather,settings.BeltColor),
                    new ByteVector2((byte)Data.MaterialType.iron, settings.BeltBuckleColor),
                }, false));
            }

            if (settings.UseCape)
            {
                addItems.Add(new Process.AddImageToCustom(VoxelModelName.cape, new List<ByteVector2>
                {
                   new ByteVector2((byte)Data.MaterialType.red, settings.CapeColor),
                }, true));
            }

            return addItems;
        }

        List<ByteVector2> ImageColorReplace()
        {
            Players.PlayerSettings sett = settings;
            return new List<ByteVector2>
                    {
                        new ByteVector2((byte)Data.MaterialType.blue_gray, sett.ClothColor), //tunic
                        new ByteVector2((byte)Data.MaterialType.skin, sett.SkinColor), //skinn 
                        new ByteVector2((byte)Data.MaterialType.brown, sett.hairColor), //hair
                        new ByteVector2((byte)Data.MaterialType.leather, sett.ShoeColor), 
                        new ByteVector2((byte)Data.MaterialType.yellow, sett.PantsColor), 
                    };
        }

        public void UpdateAppearance(bool netShare)
        {            
            new Process.ModifiedImage(this, VoxelModelName.Character,
                ImageColorReplace(), ImageAddOns(true), HeroImagePosAdj);
            damageColors = new Effects.BouncingBlockColors((Data.MaterialType)settings.ClothColor, (Data.MaterialType)settings.SkinColor, (Data.MaterialType)settings.hairColor);
            if (netShare)
            {
                player.NetShareAppearance();
            }
        }
        
        Graphics.VoxelModel originalMesh;
        
#region EXPRESSION
        Graphics.VoxelModel expressionMesh;
        VoxelModelName lastExpression = VoxelModelName.NUM_Empty;
        bool bisyExpressing = false;
        public void Express(VoxelModelName expression, bool local)
        {
            if (!bisyExpressing)
            {
                //if (local)
                //{//net share
                //    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.Express, 
                //        Network.PacketReliability.RelyableLasy, Player.Index);
                //    w.Write((byte)expression);
                //}

                bisyExpressing = true;
                if (lastExpression == expression)
                {
                    startExpression();
                }
                else
                {//load expression
                    List<Process.AddImageToCustom> addons = ImageAddOns(false);
                    VoxelModelName mouthObjName = VoxelModelName.NUM_Empty;
                    VoxelModelName eyesObjName = VoxelModelName.NUM_Empty;
                    switch (expression)
                    {
                        case VoxelModelName.express_anger:
                            eyesObjName = VoxelModelName.EyeFrown;
                            mouthObjName = VoxelModelName.MouthSour;
                            break;
                        case VoxelModelName.express_hi:
                            eyesObjName = VoxelModelName.EyeSunshine;
                            mouthObjName = VoxelModelName.MouthGasp;
                            break;
                        case VoxelModelName.express_laugh:
                            eyesObjName = VoxelModelName.EyeSunshine;
                            mouthObjName = VoxelModelName.mouth_laugh;
                            break;
                        case VoxelModelName.express_teasing:
                            eyesObjName = VoxelModelName.EyeSlim;
                            mouthObjName = VoxelModelName.MouthLoony;
                            break;
                        case VoxelModelName.express_thumbup:
                            eyesObjName = VoxelModelName.EyeSunshine;
                            mouthObjName = VoxelModelName.MouthHmm;
                            break;

                    }

                    addons.Add(new Process.AddImageToCustom(eyesObjName, (byte)Data.MaterialType.skin, settings.SkinColor, false));
                    addons.Add(new Process.AddImageToCustom(mouthObjName, (byte)Data.MaterialType.skin, settings.SkinColor, false));

                    new Process.ModifiedImage(this, expression,
                        ImageColorReplace(), addons, HeroImagePosAdj, (int)expression);
                }
            }
        }

        void startExpression()
        {
            image.SetMaster(expressionMesh);
            new Timer.TimedEventTrigger(this, 1000, TriggerEndExpression);
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
                        smoke.Add(new ParticleInitData(lib.RandomV3(startPos, 1)));
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
                        shiny.Add(new ParticleInitData(lib.RandomV3(startPos, 1), lib.RandomV3(startSpeed, 1)));
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

            }

            //int p = LfRef.gamestate.LocalHostingPlayer.Index;
            //if (Player != null)
            //{
            //    p = Player.Index;
            //}
            Music.SoundManager.PlaySound(sound, image.position);
        }

        const int TriggerEndExpression = 0;
        public void EventTriggerCallBack(int type)
        {
            if (type == TriggerEndExpression)
            {
                image.SetMaster(originalMesh);
                bisyExpressing = false;
            }
        }
        //express_anger, express_hi, express_laugh, express_teasing, express_thumbup,
#endregion
        
        const float HeroSize = 0.125f;
        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            if (link == 0)
            {
                this.originalMesh = original;
                image.SetMaster(original);
                image.scale = new Vector3(HeroSize);
                lastExpression = VoxelModelName.NUM_NON;
            }
            else
            {
                lastExpression = (VoxelModelName)link;
                expressionMesh = original;
                startExpression();
            }
        }

        public void SetHUD(Health health, Money moneyCounter, Spears spearsCounter)
        {

            healthBar = health;
        }

        
        public void ResetModifiers()
        {
            LockControls = false;
            modifiers.Clear();
            foreach (Magic.MagicRingSkill skill in player.Progress.Skills)
            {
                switch (skill)
                {
                    case Magic.MagicRingSkill.First_swing:
                        modifiers.Add(new Condition.FirstBlood(this, true));
                        break;
                    case Magic.MagicRingSkill.First_string:
                        modifiers.Add(new Condition.FirstBlood(this, false));
                        break;
                    case Magic.MagicRingSkill.Evil_aura:
                        modifiers.Add(new Condition.EvilAura(this));
                        break;
                }
            }

        }
        void showRedBorder(bool show)
        {
            if (show)
            {
                if (redBorder == null)
                {
                    redBorder = new Image(SpriteName.LFEdge, player.ScreenArea.Position, player.ScreenArea.Size, ImageLayers.Background7);
                    redBorder.Color = Color.Red;
                }
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

       // int numHealthPrompts = 4;
        int numBowHitsInARow = 0;
        int numJavelinHitsInARow = 0;

        override public void WeaponAttackFeedback(WeaponAttack.WeaponTrophyType weaponType, int numHits, int numKills)
        {
            if (localMember)
            {
                
                killsWithoutBeingHit += numKills;
                if (killsWithoutBeingHit >= LootfestLib.Trophy_KillWithoutHit)
                {
                    player.UnlockThrophy(Trophies.Kill25WithoutBeingHit);
                }

                if (weaponType == WeaponAttack.WeaponTrophyType.Sword
                    &&
                    numHits >= LootfestLib.Trophy_HitEnemiesWithSword)
                {
                    player.UnlockThrophy(Trophies.Hit4EnemiesInOneSwordAttack);
                }
                else if (weaponType == WeaponAttack.WeaponTrophyType.Axe &&
                    numHits >= LootfestLib.Trophy_HitEnemiesWithAxe)
                {
                    player.UnlockThrophy(Trophies.Hit3EnemiesInOneAxeAttack);
                }
                else if (weaponType == WeaponAttack.WeaponTrophyType.Spear &&
                    numHits >= LootfestLib.Trophy_HitEnemiesWithSpear)
                {
                    player.UnlockThrophy(Trophies.Hit2EnemiesInOneSpearAttack);
                }
                else if (weaponType == WeaponAttack.WeaponTrophyType.Arrow_Slingstone)
                {
                    if (numHits > 0)
                    {
                        numBowHitsInARow++;
                        if (numBowHitsInARow >= LootfestLib.Trophy_HitWithBow)
                        {
                            player.UnlockThrophy(Trophies.Hit10BowShots);
                        }
                    }
                    else
                    {
                        numBowHitsInARow = 0;
                    }
                }
                else if (weaponType == WeaponAttack.WeaponTrophyType.Javelin)
                {
                    if (numHits > 0)
                    {
                        numJavelinHitsInARow++;
                        if (numJavelinHitsInARow >= LootfestLib.Trophy_HitWithJavelin)
                        {
                            player.UnlockThrophy(Trophies.Hit6Javelins);
                        }
                    }
                    else
                    {
                        numJavelinHitsInARow = 0;
                    }
                }
                if (numKills >= LootfestLib.Trophy_KillEnemiesWithAttack && 
                    (weaponType == WeaponAttack.WeaponTrophyType.Sword || 
                    weaponType == WeaponAttack.WeaponTrophyType.Axe || 
                    weaponType == WeaponAttack.WeaponTrophyType.Spear))
                {
                    player.UnlockThrophy(Trophies.Kill3EnemiesInOneAttack);
                }
            }
        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (localMember && (!Immortal && immortalityTime.TimeOut && !DebugMode && Alive))
            {
                return true;
            }
            return false;
        }
        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            immortalityTime.MilliSeconds = 500;
            if (localMember)
            {
                if (health.Value > 0)
                {
                    if (PlatformSettings.ViewErrorWarnings && damage.Damage <= 0)
                    {
                        Debug.LogError("Zero damage");
                    }

                    Debug.CrashIfThreaded();//.DebugLib.CrashIfThreaded();

                    damage = player.Progress.DamageReduce(damage);
                    if (damage.Push != WeaponAttack.WeaponPush.NON)
                        new PushForce(damage, this.heroPhysics);

                    killsWithoutBeingHit = 0;
                    Health -= damage.Damage;
                    if (ai != null)
                    {
                        ai.TakeDamage(health.Value);
                    }

                    BlockSplatter();

                    
                    if (health.Value <= 0)
                    {
                        DeathEvent(local, damage);
                        Health = 0;
                    }
                    else
                    {
                        //updateRedBorder();
                        player.inputMap.Vibrate(300, 0.4f, 0.4f);
                        //new Timer.Vibration(Player.Index, 300, 0.8f, Pan.Center);
                    }
                    
                    new Process.TakeDamage(Player);

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
            //player.DeathEvent(local);
        }
        public override Rotation1D FireDir
        {
            get
            {
                if (player != null)
                {
                    Graphics.AbsCamera cam = Engine.XGuide.GetPlayer(player.Index).view.Camera;
                    if (cam.CamType == Graphics.CameraType.FirstPerson)
                    {
                        return new Rotation1D(cam.TiltX - MathHelper.PiOver2);
                    }
                }
                return base.FireDir;
            }
        }
        public override RotationQuarterion FireDir3D
        {
            get
            {
                if (player != null)
                {
                    Graphics.AbsCamera cam = Engine.XGuide.GetPlayer(player.Index).view.Camera;
                    if (cam.CamType == Graphics.CameraType.FirstPerson)
                    {
                        RotationQuarterion result = RotationQuarterion.Identity;
                        result.RotateWorldX(-cam.TiltX - MathHelper.PiOver2);
                        return result;
                    }
                }
                return base.FireDir3D;
            }
        }

        public void ChangeEquipSetup()
        {
            UpdateShield();            
            busyAttackingTime = 600;
        }

        public void Attack(Gadgets.WeaponGadget.AbsWeaponGadget2 weapon, bool keyDown, Players.EquipSlot fireButtonIx, Gadgets.GadgetAlternativeUseType altUse)
        {
            if (keyDown)
            {
                currentFireAltUse = altUse;
                currentFireButtonIx = fireButtonIx;
                currentFire = weapon;
                currentFireReloadtime = currentFire.Reloadtime(this);
                usingWeapon = weapon;
                currentAttackKeyDown = true;
            }
            else
                currentAttackKeyDown = false;
        }
        
        public bool TriggerInteraction()
        {
            if (InteractingWith != null)
            {
                InteractingWith.InteractEvent(this, true);
                RemoveActionPrompt();
                //bring up the shopping menu
                return true;
            }
            return false;
        }
        void heroCollitionCheck2(float time)
        {
            Vector3 savePos = image.position;
            bool xfirst = Ref.rnd.Plus_MinusF(1) > 0;

            testMove(xfirst, time);
            testMove(!xfirst, time);

            moveImage(Velocity.Zero, time);

        }

        public void PickUpCollect(Gadgets.IGadget item)
        {
            player.AddItem(item, true);
            pickupEffeckt();
            
        }
        public void PickUpSmallBoost(bool health_notMagic)
        {
            const float HealthPercentBoost = 0.2f;
            const float MagicPercentBoost = 0.2f;

            if (health_notMagic)
                health.Value += health.Bounds.Max * HealthPercentBoost;
            else
                magic.Value += magic.Bounds.Max * MagicPercentBoost;
            
            updateBar();
            pickupEffeckt();
            new Effects.HealUp(this, health_notMagic, true);
        }
        public void pickupEffeckt()
        {
            Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
            Engine.ParticleHandler.AddExpandingParticleArea(ParticleSystemType.GoldenSparkle, image.position + Vector3.Up * 2, 1, 8, 2);
        }
        
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Player;
            }
        }

        public void addHealth(float amount)
        {
            if (!FullHealth)
            {
                new Effects.HealUp(this, true, false);
                Health += amount;
                showRedBorder(false);
                Music.SoundManager.PlayFlatSound(LoadedSound.HealthUp);
            }
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
            //don nothing
        }

        protected override void clientSpeed(float speed)
        {
            walkAnimation(speed);
        }



        void walkAnimation(float speed)
        {
            if (busyAttackingTime >= currentFireReloadtime)
            {
                originalMesh.Frame = 1;
            }
            else if (speed == 0)
            {
                originalMesh.Frame = 0;
            }
            else
            {
                const float TimePerFrameAndSpeed = 1.1f;
                animationTime += speed * Ref.DeltaTimeMs;
                if (animationTime >= TimePerFrameAndSpeed)
                {
                    animationTime -= TimePerFrameAndSpeed;
                    currentFrame.Next();
                }
                originalMesh.Frame = currentFrame.Value + 2;
            }
        }

        public static readonly GameObjects.NetworkShare NetShare = new NetworkShare(false, true, false, true);
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
        
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {

                timeSinceProjectile += args.time;
                timeSinceMelee += args.time;

                UpdateWorldPos();
                updateMovement(args);

                if (visualBow != null)
                {
                    if (visualBow.Time_Update(args.time))
                    {
                        visualBow = null;
                    }
                }
                //--Update attack--
                UpdateAttack(args);

                updatePhysics(args);

                for (int i = modifiers.Count - 1; i >= 0; i--)
                {
                    if (!modifiers[i].Update(args))
                    {
                        modifiers.RemoveAt(i);
                    }
                }
                //lazy update
                if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part2)
                {
                    if (player.Settings.UseFlyingPet)
                    {

                        if (flyingPet == null)
                        {
                            if (flyingPetResetTimer.Update(Ref.update.LazyUpdateTime))
                            {
                                flyingPet = new FlyingPet(this, player.Settings.FlyingPetType);
                            }
                        }
                        else if (!flyingPet.Alive)
                            flyingPet = null;
                    }
                    
                }

                if (drinkTimer.Active)
                {
                    drinkTimer.Update();
                    magic.Percent = drinkTimer.PercentTimePassed;
                    updateBar();
                }
            }
            else //clientMember
            {
                UpdateWorldPos();
                base.Time_Update(args);
            }
        }

        private void updatePhysics(UpdateArgs args)
        {
        }

        Vector2 runningDir = Vector2.Zero;
        const float WalkingSpeed = 0.014f;//0.02f;
        const float RunningSpeed = WalkingSpeed * 1.3f;
        /// <summary>
        /// When carrying to much
        /// </summary>
        const float OverweightWalkingSpeed = WalkingSpeed * 0.12f;
        /// <summary>
        /// Calc speed from input, add varius speed modifiers and acceleration
        /// </summary>
        void updateMoveVelocity()
        {
            const float DebugSpeed = 0.08f;
            
            Vector2 moveDir = player.HeroMoveDir();
            runningDir *= 0.9f;
            runningDir += moveDir;

            bool weaponHalt = usingWeapon != null && usingWeapon.HaltUser && busyAttackingTime > usingWeapon.HideShieldTime;

            Velocity.MultiplyPlane(0.4f);

            if (moveDir != Vector2.Zero && !weaponHalt)
            {//add speed
                const float AcceleratePerc = 0.64f;
                
                float maxSpeed = WalkingSpeed;
                if (DebugMode) maxSpeed = DebugSpeed;
                else if (runningDir.Length() >= 8.6f)
                {
                    maxSpeed = RunningSpeed;
                }

                Velocity.Add(moveDir * maxSpeed * AcceleratePerc);

                //debug
                //System.Diagnostics.Debug.WriteLine("Move % of max: " + (Velocity.PlaneLength() / maxSpeed));

                Velocity.SetMaxPlaneSpeed(maxSpeed);

            }
            else
            {//slow down
                if (Velocity.PlaneLength() < 0.001f)
                    Velocity.SetZeroPlaneSpeed();
            }
        }

        private void updateMovement(UpdateArgs args)
        {
            updateMoveVelocity();


            if (Velocity.ZeroPlaneSpeed)
            {
                walkAnimation(0);
            }
            else
            {
                float LTbreak = Bound.Min(1 - MathExt.Square(player.inputMap.holdMovement.Value), 0.02f);
                if (LTbreak > 0.9f) LTbreak = 1;
                Velocity move = Velocity * LTbreak;
                move.Value.Y = 0;

                moveImage(move, args.time);
                walkAnimation(move.PlaneLength());
                setImageDirFromSpeed();
            }

            image.position.Y = physics.GetGroundY().slopeY;

            UpdateBound();
        }

        Map.WorldPosition previousCollisionPos;
        float collectedJumpForce = 0;

        protected override void moveImage(Velocity velocity, float time)
        {
            //First make sure he's not inside a collision already
            Physics.Collision3D collData1 = physics.ObsticleCollision();
            if (collData1 != null)
                HandleColl3D(collData1, null);


            Vector3 oldPos = image.position;
            velocity.Update(time, image);

            physics.Update(time);
            Physics.Collision3D collData = physics.ObsticleCollision();

            if (collData != null)
            {
                Map.WorldPosition collisionPos = new Map.WorldPosition(collData.OtherBound.Center);
                if (jumpableLedge(collisionPos))
                {
                    collectedJumpForce += time;
                    if (collectedJumpForce >= 90)
                    {
                        heroPhysics.Jump();
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
            float groundY = collisionPos.GetGroundY();
            float ledgeHeight = groundY - WorldPosition.WorldGrindex.Y;
            
            bool jumpable = samePos && notFalling && ledgeHeight < 2f;

            if (jumpable)
            {
                collisionPos.WorldGrindex.Y = (int)groundY + 1;
                if (LfRef.chunks.GetScreen(collisionPos).HasFreeSpaceUp(collisionPos, 3) &&
                    LfRef.chunks.GetScreen(WorldPosition).HasFreeSpaceUp(WorldPosition, 5))
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

        static readonly Vector3 VisualBowPosDiff = new Vector3(0.8f, -0.3f, 0.7f);
        private void UpdateAttack(UpdateArgs args)
        {
            if (busyAttackingTime <= 0)
            {
                if (currentFire != null)
                {
                    currentFire.SetupDamage(this, modifiers);

                    Vector3 target = currentFire.UsesTargeting ? GetBowTarget(args.allMembersCounter) : Vector3.Zero;
                    if (currentFire.UsesAmmo)
                    {//bow
                        timeSinceProjectile = 0;
                        Vector2 attackTime = currentFire.Use(this,
                            target, currentFireAltUse, true);
                        busyAttackingTime = attackTime.X + attackTime.Y;

                        Time bowVisualTime = new Time(busyAttackingTime + 100);
                        if (visualBow != null)
                        {
                            visualBow.ResetTime(bowVisualTime);
                        }
                        else
                        {
                            visualBow = new Effects.VisualBow(image, currentFire.VisualBowImage, bowVisualTime, VisualBowPosDiff);
                        }
                    }
                    else
                    {
                        timeSinceMelee = 0;
                        Vector2 attackTime = currentFire.Use(this, target, currentFireAltUse, true);
                        busyAttackingTime = attackTime.X + attackTime.Y;
                        if (currentFireAltUse == Gadgets.GadgetAlternativeUseType.Rush)
                        {
                            modifiers.Add(new Condition.RushAttackLock(this, attackTime.X));
                        }

                        System.IO.BinaryWriter wattack = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_StartAttack,
                            Network.PacketReliability.Reliable, player.Index);
                        //TwoHalfByte attackButton = new TwoHalfByte(0, (byte)currentFireButtonIx);
                        //attackButton.WriteStream(wattack);
                        wattack.Write((byte)currentFireButtonIx);
                    }
                    HideShieldFromAttack(currentFire);

                    if (player.Mode != Players.PlayerMode.Play)
                    {
                        currentAttackKeyDown = false;
                        //currentFire = null;
                    }
                    
                }

            }//end if busyAttackingTime <= 0
            else
            {
                busyAttackingTime -= args.time;
            }
            if (!currentAttackKeyDown)
                currentFire = null;
            
        }

        public void HideShieldFromAttack(Gadgets.WeaponGadget.AbsWeaponGadget2 wep)
        {
            if (shield != null)
                shield.Hide(wep.HideShieldTime, wep.Hands);
        }

        public void RemovePet()
        {
            if (flyingPet != null)
            {
                flyingPet.TakeDamage(new WeaponAttack.DamageData(100, WeaponAttack.WeaponUserType.Neutral, ByteVector2.Zero), true);
                flyingPet = null;
            }
        }

        static readonly WeaponAttack.DamageData EvilTouchDamage = new WeaponAttack.DamageData(LootfestLib.EvilTouchDamage, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Evil);

        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, AbsUpdateObj ObjCollision)
        {
            if (localMember)
            {
                obsticlePushBack(collData, 0.9f);
                if (ObjCollision != null
                    && player.Progress.GotSkill(Magic.MagicRingSkill.Evil_touch)
                    && ObjCollision.Type == ObjectType.Character)
                {
                    //damage character with evil touch
                    if (ObjCollision.TakeDamage(EvilTouchDamage, true) &&
                        !WeaponAttack.WeaponLib.IsFoeTarget(this, ObjCollision, false))
                    {
                        this.TakeDamage(EvilTouchDamage, true);
                    }
                }
            }
            
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
        }
        public Vector3 GetBowTarget(ISpottedArrayCounter<GameObjects.AbsUpdateObj> active)
        {
            AbsUpdateObj closest = null;
            float closestScore = float.MaxValue;
            const float MaxAngle = 1.2f;
            const float MaxLength = 35;
            const float AngleScore = 0.4f;
            const float LengthScore = 0.6f;

            Rotation1D fireDir = this.FireDir;
            active.Reset();
            while (active.Next())
            {
                if (active.GetMember.IsWeaponTarget &&
                    WeaponAttack.WeaponLib.IsFoeTarget(this, active.GetMember, false))//active.Member.WeaponTargetType == WeaponAttack.WeaponUserType.Enemy)
                {
                    float angle = fireDir.AngleDifference(AngleDirToObject(active.GetMember));
                    if (Math.Abs(angle) <= MaxAngle)
                    {
                        float length = PositionDiff3D(active.GetMember).Length();
                        if (length <= MaxLength)
                        {
                            float score = length / MaxLength * LengthScore +
                                angle / MaxAngle * AngleScore;
                            if (score < closestScore)
                            {
                                closest = active.GetMember;
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
                result += Map.WorldPosition.V2toV3(fireDir.Direction(NoTargetAimLength));
                return result;
            }

            return closest.Position;
        }

        


        EnvironmentObj.IInteractionObj lastInteractObj = null;

        public void GotChatMessage()
        {//to avoid see everyone talk at the same time
            openingPhraseTime = 0;
        }
        float openingPhraseTime = 0;
        public void InteractPrompt(EnvironmentObj.IInteractionObj talkingTo)
        {
#if WINDOWS 
            Debug.DebugLib.CrashIfThreaded();
#endif
            if (player.Mode == Players.PlayerMode.Play)
            {
                //When standing next to a salesman, Xtalk? 

                this.InteractingWith = talkingTo;
                if (lastInteractObj != InteractingWith && openingPhraseTime > 4000)
                {
                    //print opening text
                    openingPhraseTime = 0;
                    if (interactingWith.InteractType == InteractType.SpeakDialogue)
                    {
                        HUD.DialogueData phrase = interactingWith.Interact_OpeningPhrase(this);
                        if (phrase != null)
                            player.PrintChat(new ChatMessageData( phrase.Pages[0], interactingWith.ToString()), LoadedSound.Dialogue_Neutral);
                    }
                    lastInteractObj = interactingWith;
                }

            }
            else
            {
                openingPhraseTime = 0;
            }
        }

        void showActionPrompt(string text)
        {
            const ImageLayers Layer = ImageLayers.Background5;

            RemoveActionPrompt();
            actionButtonText = new TextG(LoadedFont.Lootfest, player.ScreenArea.Center,
                new Vector2(0.8f), Align.CenterAll, text, Color.Yellow, Layer);
            actionButtonImage = new Image(SpriteName.ButtonY,
                actionButtonText.Position, new Vector2(32), Layer, true);
            actionButtonImage.Xpos -= actionButtonText.MesureText().X * PublicConstants.Half + 20;
            const float ImageYadj = -4;
            actionButtonImage.Ypos += ImageYadj;
        }
        public void RemoveActionPrompt()
        {
            if (actionButtonText != null)
            {
                actionButtonText.DeleteMe();
                actionButtonImage.DeleteMe();
                actionButtonText = null;
                actionButtonImage = null;
            }
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.Hero;
            }
        }

        float runOffWarningTime = 0;
        int numWarnings = 0;
        public override void Time_LasyUpdate(ref float time)
        {
            openingPhraseTime += time;
            immortalityTime.CountDown(time);
            
            if (WorldPosition.ChunkGrindex.X != currentScreen.X || WorldPosition.ChunkGrindex.Y != currentScreen.Y)
            {
                currentScreen = WorldPosition.ChunkGrindex;
            }

            if (InteractingWith != null && player.Mode == Players.PlayerMode.Play)
            {
                if (!InteractingWith.InRange(this))
                {
                    RemoveActionPrompt();
                    InteractingWith = null;
                }
                else
                {
                    showActionPrompt(InteractingWith.InteractionText);
                }
            }
           
            if (CheckOutSideWorldsBounds())
            {
                player.Print("Invisible wall!");
            }
            //base.Time_LasyUpdate(ref time);
            updatePositionToNewbie--;

            if (PlatformSettings.HelpAndTutorials && Map.World.RunningAsHost && !player.Progress.TakenGifts && numWarnings < 2)
            {
                runOffWarningTime -= time;
                if (runOffWarningTime <= 0)
                {
                    if ((WorldPosition.ChunkGrindex - homePos).SideLength() > 1)
                    {
                        //tell the player to not run off
                        string message;
                        switch (Ref.rnd.Int(2))
                        {
                            default:
                                message = "Don't run off without your starting gear!";
                                break;
                            case 1:
                                message = "Go back and talk to your father";
                                break;

                        }

                        player.beginButtonTutorial( new ButtonTutorialArgs( SpriteName.ButtonA, numBUTTON.A, null,
                            message, player.ScreenArea));
                        runOffWarningTime = lib.SecondsToMS(20);
                        new CompassPulse(player);
                        numWarnings++;
                    }
                }
            }
        }
        public IntVector2 AreaPos
        {
            get { return currentArea; }
        }
        public override int UnderType
        {
            get { return (int)CharacterUtype.Player; }
        }
        public override Characters.CharacterUtype CharacterType
        {
            get { return Characters.CharacterUtype.Player; }
        }
        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.FromSpeed;
            }
        }
        override protected Color damageTextCol { get { return Color.Red; } }


        Effects.BouncingBlockColors damageColors = StandardDamageColors;//new Effects.BouncingBlockColors(Data.MaterialType.red,,
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return damageColors;
            }
        }

        public bool HasFlyingPet
        {
            get { return flyingPet != null; }
        }

        public override Graphics.LightSourcePrio LightSourcePrio
        {
            get
            {
                return Graphics.LightSourcePrio.High;
            }
        }
        public override bool HasNetId
        {
            get
            {
                return true;
            }
        }
    }

    class UpdateApperanceTimer : LazyUpdate
    {
        Hero h;
        float wait = 4000;
        public UpdateApperanceTimer(Hero h)
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
