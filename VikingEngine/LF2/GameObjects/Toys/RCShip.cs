//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;
//using Game1.LootFest;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    class RCShip: AbsRCCar
//    {
//        bool fireRight = false;
//        public RCShip(Characters.Hero parent)
//            : base(parent)
//        {
            
//        }
//        public RCShip(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }
//        const float BoundW = 0.5f;
//        static readonly LootFest.ObjSingleBound Bound = new LootFest.ObjSingleBound(
//              new BoundData2(new Physics.StaticBoxBound(
//                    new VectorVolume(Vector3.Zero, new Vector3(BoundW, 0.25f, BoundW))), new Vector3(0, 0.2f, 0)));
//        override protected LootFest.ObjSingleBound bound
//        { get { return Bound; } }
//        protected override float ProjectileDamage
//        {
//            get { return HeavyDamage; }
//        }
//        public override void Button_Event(ButtonValue e)
//        {
//            bool right = e.Button == numBUTTON.RB || e.Button == numBUTTON.RT || e.Button == numBUTTON.B;
//            bool left = e.Button == numBUTTON.LB || e.Button == numBUTTON.LT || e.Button == numBUTTON.Y;

//            if (!LockControls && (left || right))
//            {
//                fireRight = right;
//                fireKeyDown = e.KeyDown;
//                if (fireKeyDown)
//                {//first bullet
//                    fireUpdate(0);
//                }
//            }

//            if (e.Button == numBUTTON.X)
//            {
//                gas = lib.BoolToInt01(e.KeyDown);
//            }
//        }
//        public override void Pad_Event(JoyStickValue e)
//        {
//            if (e.Stick == Stick.Right)
//                gas = lib.SetMinVal(-e.Direction.Y, 0);
//            else
//                wheelTurning = e.Direction.X;
//        }

//        override public void ViewControls(List<HUD.ButtonDescriptionData> toGroup)
//        {
//#if CMODE
//            bool pitch = player.settings.HelicopterInvertPitch;
//            toGroup.Add(new HUD.ButtonDescriptionData("Wheel", TileName.BoardCtrlLS));
//            toGroup.Add(new HUD.ButtonDescriptionData("Full sail", TileName.BoardCtrlX));
//            toGroup.Add(new HUD.ButtonDescriptionData("Fire Left", TileName.BoardCtrlLB));
//            toGroup.Add(new HUD.ButtonDescriptionData("Fire Right", TileName.BoardCtrlRB));
//            base.ViewControls(toGroup);
//#endif
//        }
//        override protected float wepReloadTime { get { return 1600; } }
//        override protected float wepFireRate { get { return 200; } }
//        override protected int wepNumBullets { get { return 1; } }

//        public override int UnderType
//        {
//            get { return (int)ToyType.Ship; }
//        }
//        const float MaxVelocity = 0.0022f;
//        override protected float maxVelocity
//        { get { return MaxVelocity; } }
//        const float Acceleration = 0.0024f;
//        override protected float acceleration
//        { get { return Acceleration; } }
//        override protected float baseSpeed
//        { 
//            get {
//                if (InMenu || LockControls)
//                    return 0;
//                return 0.0018f; 
//        } }
//        const float NaturalBreak = 0.94f;
//        override protected float naturalBreak
//        { get { return NaturalBreak; } }

//        const float AccelerationSlopeEffect = 0.0001f;
//        override protected float accelerationSlopeEffect
//        { get { return AccelerationSlopeEffect; } }
//        const float NaturalBreakSlopeEffect = 0f;
//        override protected float naturalBreakSlopeEffect
//        { get { return NaturalBreakSlopeEffect; } }
//        const float MaxVelocitySlopeEffect = 0.001f;
//        override protected float maxVelocitySlopeEffect
//        { get { return MaxVelocitySlopeEffect; } }

//        override protected void fire()
//        {
//            const float Velocity = 0.007f;
//            Vector3 startPos = image.Position;
//            startPos.Y -= 0.03f;
//            const float SpeedY = 0.15f;
//            Rotation1D dir = Rotation;
//            if (fireRight)
//                dir += Rotation1D.D90;
//            else
//                dir -= Rotation1D.D90;
//            new ShipProjectile(ProjectileDamage, startPos, Map.WorldPosition.V2toV3(dir.Direction(1), SpeedY) * Velocity, this.ObjOwnerAndId);
//        }
//        public override RcCategory RcCategory
//        {
//            get { return Toys.RcCategory.Ship; }
//        }
//        override protected VoxelModelName VoxelObj
//        { get { return VoxelModelName.RCShip1; } }
//        override protected Data.MaterialType StartColor(int index)
//        {
//            switch (index)
//            {
//                case 0:
//                    return Data.MaterialType.dark_gray;
//                case 1:
//                    return Data.MaterialType.dark_skin;
//                case 2:
//                    return Data.MaterialType.marble;

//            }
//            throw new IndexOutOfRangeException();
//        }
//        override protected void terrainEffect(Data.MaterialType material)
//        {
            
//            switch (material)
//            {
//                case Data.MaterialType.lava:
//                    break;
//                case Data.MaterialType.water:
//                    break;
//                default:
//                    badTerrain = TerrainSpeed.Bad;
//                    break;
//                case Data.MaterialType.gold:
//                    //boost
//                    boostUp();
//                    if (fireTime > wepFireRate)
//                    {
//                        fireTime = wepFireRate;
//                    }
//                    break;
//                case Data.MaterialType.iron:
//                    TakeDamage(LavaDamage, true);
//                    break;
//                case Data.MaterialType.bronze:
//                    TakeDamage(WatereDamage, true);
//                    break;
//                case Data.MaterialType.white:
//                    HealUp();

//                    break;
//            }
//        }
//        const float BadTerrainEffect = 0.2f;
//        override protected float badTerrainEffect
//        { get { return BadTerrainEffect; } }
//        const float BoostEffect = 1.8f;
//        override protected float boostEffect
//        { get { return BoostEffect; } }

//        public override string ToString()
//        {
//            return "Pirate ship";
//        }

//        //temp
//        public override void NetReadUpdate(System.IO.BinaryReader reader)
//        {
//            base.NetReadUpdate(reader);
//        }
//        public override void NetWriteUpdate(System.IO.BinaryWriter writer)
//        {
//            base.NetWriteUpdate(writer);
//        }
//        //public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
//        //{
//        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
//        //}
//        //public override bool NetUpdateable
//        //{
//        //    get
//        //    {
//        //        return true; //base.NetUpdateable;
//        //    }
//        //}
//    }
//}
