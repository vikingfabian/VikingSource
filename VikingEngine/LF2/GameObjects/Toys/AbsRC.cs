//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;
//using Game1.Voxels;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    abstract class AbsRC : AbsVoxelObj
//    {
//        Effects.BouncingBlockColors damageColor = StandardDamageColors;
//        protected float immortalityTime = 0;
//        protected float boostTime = 0;
//        List<float> anglesHistory;
//        List<Vector3> positionHistory;
//        float mediumAngle;
//        public float MediumAngle
//        {
//            get { return mediumAngle; }
//        }

//        public bool LockControls = false;
//        public bool InMenu = false;
//#if CMODE
//        RaceStarter race = null;
//        public RaceStarter Race
//        { 
//            set 
//            {
//                if (value == null)
//                {
//                    LockControls = false;
//                    if (race != null && !race.Owner.Local)
//                    {
//                        Print("Race ended");
//                    }
//                }
//                race = value;
//                positionHistory = new List<Vector3>();
//            } 
//        }
//#endif
//        protected Players.RCcolorScheme colors;
//        IntVector2 currentScreen = IntVector2.Zero;
//        public Players.Player player;
//        Network.AbsNetworkGamer sender;
//        protected bool fireKeyDown = false;
//        int numBullets;
//        protected float fireTime = 0;
//        protected const float StartHealth = 20;

//        public AbsRC(Players.Player player)
//            : base()
//        {
//#if CMODE
//            anglesHistory = new List<float> { player.hero.Rotation.Radians };
//            this.player = player;

//            health = StartHealth;
//            numBullets = wepNumBullets;
//            Players.RCcolorScheme colors = player.settings.GetRCcolorScheme(RcCategory);
//            BasicInit(player);
//            NetworkShare();

//            rotation = player.hero.Rotation;
//            setImageDirFromRotation();
//#endif
//        }

//        public AbsRC(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(r)
//        {
//            this.sender = sender;
//            colors = new Players.RCcolorScheme();
//            UpdateImage(System.IO.BinaryReader);
//            BasicInit(null);
//        }
//        public void UpdateImage(System.IO.BinaryReader r)
//        {
//            colors.ReadStream(r);
//            Vector3 pos = Vector3.Zero;
//            if (image != null)
//                pos = image.Position;
//            UpdateImage(pos);
//        }
//        public void CheckPointFeedback()
//        {
//#if CMODE
//            if (player != null)
//            {
//                new Timer.Vibration(player.Index, 260, 0.8f, Pan.Center);
//                ParticleInitData particle = new ParticleInitData(image.Position, Vector3.Zero);
//                for (int i = 0; i < 8; i++)
//                {
//                    ParticleInitData p = particle;
//                    p.Position.X += lib.RandomDifferance(1);
//                    p.Position.Z += lib.RandomDifferance(1);
//                    const float MaxSpeed = 0.1f;
//                    p.StartSpeed.X += lib.RandomDifferance(MaxSpeed);
//                    p.StartSpeed.Y += lib.RandomDifferance(MaxSpeed);
//                    p.StartSpeed.Z += lib.RandomDifferance(MaxSpeed);
//                    Engine.ParticleHandler.AddParticles(ParticleSystemType.GoldenSparkle, p);
//                }
//            }
//#endif
//        }
//        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
//        {
//            base.ObjToNetPacket(writer);
//            colors.WriteStream(writer);
//        }
//        public void Print(string text)
//        {
//            player.Print(text);
//        }
//        virtual protected void BasicInit(Players.Player player)
//        {
//            //rotation = Rotation1D.D180;
//            CollisionBound = bound;
            
//            if (player != null)
//            {
//                Vector3 pos = Vector3.Zero;
//                pos = player.HeroPos + Map.WorldPosition.V2toV3(rotation.Direction(1));
//                pos.Y += 8;
//                UpdateImage(player.Settings, pos);
//            }
//        }

//        static readonly LootFest.ObjSingleBound StandardBound = LootFest.ObjSingleBound.QuickBoundingBox(1f);
//        virtual protected LootFest.ObjSingleBound bound
//        { get { return StandardBound; } }

        
//        protected void HealUp()
//        {
//            if (Health < StartHealth)
//            {
//                Health = lib.SetMaxFloatVal(Health + 0.2f, StartHealth);
//                //new Effects2D.HealUp(image.Position, player.Index, 1);
//                new Effects.HealUp(this, true, false);
//            }
//        }

        

//        public void UpdateImage(Players.PlayerSettings settings, Vector3 pos)
//        {
//#if CMODE
//            colors = settings.GetRCcolorScheme(RcCategory);
//            this.UpdateImage(pos);
//#endif
//        }

//        virtual protected float Yadjust { get { return -4; } }

//        virtual public void UpdateImage(Vector3 pos)
//        {
//            //if (image != null)
//            //{
//            //    image.DeleteMe();
//            //}
//            ////pos diff ska vara här

//            //VoxelObjListData voxels = Editor.VoxelObjDataLoader.VoxelObjListData(VoxelObj);
//            //voxels.ReplaceMaterial(
//            //    new List<ByteVector2>
//            //        {
//            //    new ByteVector2((byte)StartColor(0), colors.Colors.GetArrayIndex(0)),
//            //    new ByteVector2((byte)StartColor(1), colors.Colors.GetArrayIndex(1)),
//            //    new ByteVector2((byte)StartColor(2), colors.Colors.GetArrayIndex(2)),
//            //        });

//            //Vector3 posAdj = Editor.VoxelObjBuilder.CenterAdjust(Editor.VoxelObjDataLoader.StandardLimits);
//            //posAdj.Y +=Yadjust;

//            //image = Editor.VoxelObjBuilder.BuildFromVoxels(Editor.VoxelObjDataLoader.StandardLimits, voxels.Voxels, posAdj);
//            //new Engine.AddDrawObj(image, true);
            
//            //image.Scale = lib.V3(0.1f);
//            //image.Position = pos;
//            //if (physics != null)
//            //    physics.UpdatePosFromParent();
//            //moveImage(new Velocity(rotation,1), 0);
//            //damageColor = new Effects.BouncingBlockColors(
//            //    (Data.MaterialType)colors.Colors.GetArrayIndex(0), 
//            //    (Data.MaterialType)colors.Colors.GetArrayIndex(1), 
//            //    (Data.MaterialType)colors.Colors.GetArrayIndex(2));
//        }

//        abstract protected VoxelModelName VoxelObj { get; }
//        abstract protected Data.MaterialType StartColor(int index);


//        virtual public void Button_Event(ButtonValue e)
//        {
//            if (!LockControls && (e.Button == numBUTTON.RB || e.Button == numBUTTON.RT || e.Button == numBUTTON.A))
//            {
//                fireKeyDown = e.KeyDown;
//                if (fireKeyDown)
//                {//first bullet
//                    fireUpdate(0);
//                }
//            }
//        }

//        bool reloaded = true;
//        protected void fireUpdate(float time)
//        {
//            fireTime -= time;
//            if (!reloaded)
//            {
//                if (fireTime <= 0)
//                {
//                    reloaded = true;
//                    Music.SoundManager.PlaySound(LoadedSound.reload, image.Position);
//                }
//            }
//            if (fireKeyDown)
//            {
//                if (fireTime <= 0)
//                {
//                    fire();
//                    numBullets--;
//                    fireTime = wepFireRate;
//                    if (numBullets <= 0)
//                    {
//                        fireTime = wepReloadTime;
//                        numBullets = wepNumBullets;
//                        reloaded = false;
//                    }
//                }
//            }
//        }
//        virtual protected void fire()
//        {
//            const float Velocity = 0.02f;
//            Vector3 startPos = image.Position;
//            startPos.Y += bulletYadj;
//            new ToyProjectile(ProjectileDamage, startPos, ProjectileStartDir * Velocity, Speed3d, this.ObjOwnerAndId);
//        }
//        virtual protected float bulletYadj
//        { get { return 0.4f; } }

//        abstract protected Vector3 ProjectileStartDir { get; }
//        protected const float LightDamage = 5f;
//        protected const float HeavyDamage = 10f;
//        abstract protected float ProjectileDamage { get; }
//        virtual protected Vector3 Speed3d { get {  return Velocity.Value; } } 

//        virtual public void Pad_Event(JoyStickValue e) { }
//        virtual public void PadUp_Event(Stick padIx, int contolIx) { }

//        public override ObjectType Type
//        {
//            get { return ObjectType.Toy; }
//        }

//        virtual public void ViewControls(List<HUD.ButtonDescriptionData> toGroup)
//        {
//            toGroup.Add(new HUD.ButtonDescriptionData("Fire", TileName.ButtonRB)); //numBUTTON.RB, "Fire");
//            toGroup.Add(new HUD.ButtonDescriptionData("Zoom", TileName.Dpad));//Pad.D, HUD.StickDirType.UpDown, "Zoom");
//            toGroup.Add(new HUD.ButtonDescriptionData("Menu", TileName.ButtonSTART));//numBUTTON.START, "Menu");
//        }
//        //public override void ReceiveDamageCollision(Weapons.DamageData damage, bool local)
//        //{
//        //    if (immortalityTime <= 0)
//        //    {
//        //        immortalityTime = 260;
//        //        base.ReceiveDamageCollision(damage, local);
//        //        if (player != null)
//        //            player.TakeDamageFeedback();
//        //        if (Alive)
//        //            Music.SoundManager.PlaySound(LoadedSound.rc_dmg, image.Position, 0);
//        //        else
//        //            Music.SoundManager.PlaySound(LoadedSound.rc_explode, image.Position, 0);

//        //        BlockSplatter();
//        //    }
//        //}
//        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
//        {
//            immortalityTime = 260;
//            base.handleDamage(damage, local);
//            if (player != null)
//                player.HandleDamage();
//            if (Alive)
//                Music.SoundManager.PlaySound(LoadedSound.rc_dmg, image.Position);
//            else
//                Music.SoundManager.PlaySound(LoadedSound.rc_explode, image.Position);

//            BlockSplatter();
//        }

//        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
//        {
//            return immortalityTime <= 0;
//        }

//        override public Effects.BouncingBlockColors DamageColors
//        {
//            get { return damageColor; }
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//#if CMODE
//            if (race == null || !race.Joinable)
//            {
//                fireUpdate(time);
//                base.Time_Update(args);

//                collectAngleHistory();
//                updateBoost(time);
//            }
//#endif
//        }
//#if CMODE
//        //public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
//        //{
//        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
//        //    if (immortalityTime > 0)
//        //    { immortalityTime -= time; }
//        //}
//#endif
//        protected void boostUp()
//        {
//            const float BoostTime = 600;
//            boostTime = BoostTime;
//        }
//        protected void updateBoost(float time)
//        {
//#if CMODE
//            if (boostTime > 0)
//            {
//                ParticleInitData particle = new ParticleInitData(image.Position, Vector3.Zero);
//                for (int i = 0; i < 4; i++)
//                {
//                    Engine.ParticleHandler.AddParticles(ParticleSystemType.GoldenSparkle, particle);
//                }
//                boostTime -= time;
//            }
//#endif
//        }

//        protected void collectAngleHistory()
//        {
//            const int MaxSamples = 15;
//            anglesHistory.Add(Rotation.Radians);
//            if (anglesHistory.Count > MaxSamples)
//            {
//                anglesHistory.RemoveAt(0);
//            }

//            mediumAngle = anglesHistory[0];
//            for (int i = 1; i < anglesHistory.Count; i++)
//            {
//                mediumAngle = Rotation1D.MidAngle(mediumAngle, anglesHistory[i]);
//            }
//        }
        
//        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
//        {
//#if CMODE
//            if (race == null)
//            {
//                base.DeathEvent(local, damage);
//            }
//            else
//            {
//                DeathReset();
//            }
//#endif
//        }
//        virtual protected void DeathReset()
//        {
//            if (player != null)
//                player.Print("Try again!");
//            Health = StartHealth;
//            rotation.Radians = anglesHistory[0];
//            setImageDirFromRotation();
//            image.Position = positionHistory[0];
//            LockControls = true;
//            new RaceDeathRestarter(this);
//        }

       

//        public override void Time_LasyUpdate(ref float time)
//        {
//            #if CMODE
//            base.Time_LasyUpdate(ref time);
//            const float MinYpos = 0;
//            if (image.Position.Y < MinYpos)
//            {
//                TakeDamage(new Weapons.DamageData(1, Weapons.WeaponTargetType.NON, ushort.MaxValue), true);
//            }

//            if (WorldPosition.ScreenIndex != currentScreen)
//            {
//                //player.EnteredNewScreen = true;
//                currentScreen = WorldPosition.ScreenIndex;
//            }
//            //Search races
//            if (race == null)
//            {
//                race = RaceStarter.SearchRaces(this);
//                if (race != null)
//                    race.AddToy(this);
//            }
//            else
//            {
//                const int NumPositionSamples = 7;
//                positionHistory.Add(image.Position);
//                if (positionHistory.Count > NumPositionSamples)
//                {
//                    positionHistory.RemoveAt(0);
//                }
//            }
//            updatePositionToNewbie--;

//            if (immortalityTime > 0)
//            { immortalityTime -= time; }


//            if (localMember && CheckOutSideWorldsBounds())
//            {
//                TakeDamage(new Weapons.DamageData(1, Weapons.WeaponTargetType.NON, ushort.MaxValue), true);
//                player.Print("Invisible wall!");
//            }
//#endif
//        }
//        //public override bool IsLightSource
//        //{
//        //    get
//        //    {
//        //        return true;
//        //    }
//        //}
//        public string GamerTag
//        {
//            get
//            {
//                if (localMember)
//                    return player.Name;
//                else
//                    return sender.Gamertag;
//            }
//        }
//        abstract protected float wepReloadTime { get; }
//        abstract protected float wepFireRate { get; }
//        abstract protected int wepNumBullets { get; }
//        abstract public RcCategory RcCategory { get; }

//        override public WeaponAttack.WeaponUserType WeaponTargetType { get { return WeaponAttack.WeaponUserType.Toy; } }
//        virtual public bool FlyingToy { get { return false; } }

//        override public bool IsWeaponTarget { get { return true; } }
            
//    }
    
//    struct Velocity
//    {
//        public float Value;
        
//        float maxValue, acceleration, decline;

//        public Velocity(float maxValue, float acceleration, float decline)
//        {
//            this.maxValue = maxValue; this.acceleration = acceleration; this.decline = decline;
//            Value = 0;
//        }
//        public void Accelerate(float acc)
//        {
//            Value = lib.SetBounds(Value + acc * acceleration, -maxValue, maxValue);
//        }
//        public void TimeUpdate()
//        {
//            //the natural break
//            Value *= decline;
//        }
        
//    }
//    enum RcCategory
//    {
//        Ship,
//        Car,
//        Tank,
//        Helicopter,
//        AirPlane,
//    }
//    enum RCCameraType
//    {
//        Behind,
//        RC,
//        FirstPerson,
//        Top,
//        Classic,
//        Pilot,
//        NUM,
//    }
//}
