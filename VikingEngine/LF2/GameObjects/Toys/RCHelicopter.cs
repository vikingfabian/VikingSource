//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;
//using Game1.Voxels;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    class RCHelicopter : AbsFlyingRCtoy
//    {
//        /*LS
//         *Latteral, tilt åt sidorna
//         *Elevator, tilt framåt/bakåt
//         *
//         *RS
//         *Yaw, rotera som en bil
//         *Collective, hastighet i höjdled, faller snabbare
//         */
//       static readonly Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(4, 100);
//        float latteralSpeed = 0;
//        float elevatorSpeed = 0;
//        float yawSpeed = 0;
//        float collectiveVelocity = 0;
//        Rotation3D yawPitchRoll = new Rotation3D();
//        public override Rotation1D Rotation
//        {
//            get
//            {
//                return new Rotation1D(MathHelper.Pi - yawPitchRoll.Xradians);
//            }
//            set
//            {
//                base.Rotation = value;
//            }
//        }
//        public RCHelicopter(Characters.Hero parent)
//            : base(parent.Player)
//        {
//            //ImageSetup( VoxelModelName.Pig, parent.Position, lib.V3(0.1f));
//        }
//        public RCHelicopter(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }
//        protected override void BasicInit(Players.Player player)
//        {
//            base.BasicInit(player);
//            Vector3 pos = Vector3.Zero;
//            if (player != null)
//            {
//                pos = player.HeroPos;
//            }
//            image.Scale = lib.V3(0.1f);
//            image.Position.Y += 6;
//        }
//        public override void HandleTerrainColl3D(LootFest.TerrainColl collSata, Vector3 oldPos)
//        {
            
//            //image.Position += collSata.CollDir * 0.2f;
//            const float CollDirSpeed = 0.002f;
//            Vector3 posDiff = oldPos - image.Position;
//            collisionDamage(posDiff);
            
//            posDiff = lib.SafeNormalizeV3(posDiff);
//            image.Position += posDiff * 0.2f;

//            Velocity.Add( Map.WorldPosition.V3toV2(posDiff + collSata.CollDir) * CollDirSpeed);
//            collectiveVelocity += (collSata.CollDir.Y + posDiff.Y) * CollDirSpeed;

//            if (collSata.CollDir.Y > 0 && Velocity.PlaneLength() < -collectiveVelocity)
//            {
//                //check highest block and place it on it
                
//                const float PosAbove = 0.7f;
//                if (image.Position.Y < (firstBlockBelow.Position.Y + PosAbove))
//                {
//                    image.Position.Y = (firstBlockBelow.Position.Y + PosAbove);
//                }
//            }
//        }

        

//        public override void Pad_Event(JoyStickValue e)
//        {
//#if CMODE
//            /*LS
//             *Latteral, tilt åt sidorna
//             *Elevator, tilt framåt/bakåt
//             *
//             *RS
//             *Yaw, rotera som en bil
//             *Collective, hastighet i höjdled, faller snabbare
//             */
//            if (!LockControls)
//            {

//                bool pitch = player.settings.HelicopterPitchLS == (e.Stick == Stick.Left);
//                if (pitch && player.settings.HelicopterInvertPitch)
//                {
//                    e.Direction.Y = -e.Direction.Y;
//                }
//                bool roll = player.settings.HelicopterRollLS == (e.Stick == Stick.Left);

//                if (roll)
//                {
//                    const float LatteralAcc = 0.05f;
//                    const float MaxLatteralSpeed = 0.4f;
//                    latteralSpeed = lib.SetBounds(latteralSpeed + e.Direction.X * LatteralAcc, -MaxLatteralSpeed, MaxLatteralSpeed);
//                }
//                else
//                {
//                    const float YawAcc = 0.012f;
//                    const float MaxYaw = 0.1f;
//                    yawSpeed = lib.SetBounds(yawSpeed + YawAcc * -e.Direction.X, -MaxYaw, MaxYaw);
//                }
//                if (pitch)
//                {
//                    const float ElevatorAcc = 0.05f;
//                    const float MaxElevatorSpeed = 0.4f;
//                    elevatorSpeed = lib.SetBounds(elevatorSpeed - e.Direction.Y * ElevatorAcc, -MaxElevatorSpeed, MaxElevatorSpeed);
//                }
//                else
//                {
//                    const float CollectiveAccUp = 0.024f;
//                    const float CollectiveAccDown = CollectiveAccUp * 2f;
//                    const float MaxCollective = 1.2f;
//                    collectiveVelocity += lib.SetMaxFloatVal(
//                            -e.Direction.Y * (e.Direction.Y < 0 ? CollectiveAccUp : CollectiveAccDown), MaxCollective);
//                }
//            }
//#endif
//        }
//        public override void PadUp_Event(Stick padIx, int contolIx)
//        {
            
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            if (localMember)
//            {
//                if (!InMenu && !LockControls)
//                {
//                    const float MaxTilt = 0.6f;
//                    image.Position.Y += collectiveVelocity;
//                    yawPitchRoll.Xradians += yawSpeed;
//                    const float MaxElevator = MaxTilt;
//                    yawPitchRoll.Yradians = lib.SetBounds(yawPitchRoll.Yradians + elevatorSpeed, -MaxElevator, MaxElevator);
//                    const float MaxLatteral = MaxTilt;
//                    yawPitchRoll.Zradians = lib.SetBounds(yawPitchRoll.Zradians + latteralSpeed, -MaxLatteral, MaxLatteral);

//                    const float TiltToSpeed = -0.0012f;
//                    const float ElevatorToForwardSpeed = TiltToSpeed;
//                    const float CollectiveForwardAdd = TiltToSpeed;
//                    float forwardSpeed = ElevatorToForwardSpeed * (1 + CollectiveForwardAdd * collectiveVelocity) * yawPitchRoll.Yradians;
//                    Rotation1D planeDir = new Rotation1D(MathHelper.TwoPi - yawPitchRoll.Xradians);
//                    if (boostTime > 0)
//                    {
//                        forwardSpeed *= 1.6f;
//                    }
//                    Velocity.Add(planeDir, forwardSpeed); //+= planeDir.Direction(forwardSpeed);

//                    const float LatteralToStaif = TiltToSpeed * 0.6f;
//                    const float CollectiveStraifAdd = LatteralToStaif;
//                    float straifSpeed = LatteralToStaif * (1 + CollectiveStraifAdd * collectiveVelocity) * yawPitchRoll.Zradians;
//                    //Rotation1D dir = new Rotation1D((MathHelper.TwoPi - yawPitchRoll.Xradians) + MathHelper.PiOver2);
//                    planeDir.Add(MathHelper.PiOver2);
//                    Velocity.Add(planeDir, straifSpeed); //+= planeDir.Direction(straifSpeed);

//                    image.Rotation.QuadRotation = yawPitchRoll.QuadRotation;

//                    base.Time_Update(args);

//                    const float ElevatorNaturalDecline = 0.8f;
//                    const float LatteralNaturalDecline = 0.8f;
//                    const float YawNaturalBreak = 0.95f;
//                    const float CollectiveNaturalBreak = 0.95f;
//                    latteralSpeed *= LatteralNaturalDecline;
//                    yawPitchRoll.Zradians *= LatteralNaturalDecline;
//                    elevatorSpeed *= ElevatorNaturalDecline;
//                    yawPitchRoll.Yradians *= ElevatorNaturalDecline;
//                    yawSpeed *= YawNaturalBreak;
//                    collectiveVelocity *= CollectiveNaturalBreak;
//                    const float SpeedNaturalDecline = 0.97f;
//                    Velocity *= SpeedNaturalDecline;

//                    dir = image.Rotation.TranslateAlongAxis(Vector3.Backward, Vector3.Zero);
//                }
//            }
//            else
//            {
//                base.Time_Update(args);
//            }
//            animate();
//        }
//        //public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
//        //{
//        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
//        //    animate();
//        //}

//        void animate()
//        {
//            image.NextAnimationFrame();
//        }

//        override protected Vector3 Speed3d { get { return Map.WorldPosition.V2toV3(Velocity.PlaneValue, collectiveVelocity * 0.01f); } }

//        override protected float bulletYadj
//        { get { return -0.4f; } }
        
//        override protected void setImageDirFromSpeed()
//        {
            
//        }

//        protected override ByteVector3 LowResRotation3d
//        {
//            get
//            {
//                return yawPitchRoll.LowRes;
//            }
//            set
//            {
//                base.LowResRotation3d = value;
//            }
//        }

//        protected override float ProjectileDamage
//        {
//            get { return LightDamage; }
//        }
//        override public void ViewControls(List<HUD.ButtonDescriptionData> toGroup)
//        {
//#if CMODE
//            bool pitch = player.settings.HelicopterInvertPitch;
//            toGroup.Add(new HUD.ButtonDescriptionData("Sideways", player.settings.HelicopterPitchLS ? TileName.BoardCtrlLS_LR : TileName.BoardCtrlRS_LR));//Pad.Left, HUD.StickDirType.LeftRight, "Wheel");
//            toGroup.Add(new HUD.ButtonDescriptionData("Forward", player.settings.HelicopterRollLS ? TileName.BoardCtrlLS_UD : TileName.BoardCtrlRS_UD));//(numBUTTON.X, "Gas");
//            toGroup.Add(new HUD.ButtonDescriptionData("Yaw", !player.settings.HelicopterPitchLS ? TileName.BoardCtrlLS_LR : TileName.BoardCtrlRS_LR));//numBUTTON.B, "Brake");
//            toGroup.Add(new HUD.ButtonDescriptionData("Height", !player.settings.HelicopterRollLS ? TileName.BoardCtrlLS_UD : TileName.BoardCtrlRS_UD));
//            base.ViewControls(toGroup);
//#endif
//        }

//        override protected float wepReloadTime { get { return 2000; } }
//        public const float HeliFireRate = 160;
//        override protected float wepFireRate { get { return HeliFireRate; } }
//        override protected int wepNumBullets { get { return 12; } }

//        public override int UnderType
//        {
//            get { return (int)ToyType.LightHelicoper; }
//        }
//        public override RcCategory RcCategory
//        {
//            get { return Toys.RcCategory.Helicopter; }
//        }
//        override protected VoxelModelName VoxelObj
//        { get { return VoxelModelName.RCheli1; } }
//        override protected Data.MaterialType StartColor(int index)
//        {
//            switch (index)
//            {
//                case 0:
//                    return Data.MaterialType.dark_gray;
//                case 1:
//                    return Data.MaterialType.red_orange;
//                case 2:
//                    return Data.MaterialType.marble;

//            }
//            throw new IndexOutOfRangeException();
//        }
//         override public void UpdateImage(Vector3 pos)
//        {
//           // if (image != null)
//           // {
//           //     image.DeleteMe();
//           // }

//           // Graphics.VoxelObjAnimated org = Editor.VoxelObjDataLoader.GetVoxelObjAnimWithColReplace(VoxelModelName.RCheli1, new Vector3(0, -6, 0),
//           //     new List<byte>
//           //     {
//           //         (byte)StartColor(0),(byte)StartColor(1),(byte)StartColor(2),
//           //     },
//           //     new List<byte>
//           //     {
//           //         colors.Colors.GetArrayIndex(0), colors.Colors.GetArrayIndex(1), colors.Colors.GetArrayIndex(2),
//           //     });
//           // org.DeleteMe();
//           // image = new Graphics.VoxelModelInstance(org, AnimationsSettings);

//           ////VoxelObjListData voxels = Editor.VoxelObjDataLoader.VoxelObjListData(VoxelModelName.RCheli1);
//           //// voxels.ReplaceMaterial(
//           ////     (byte)StartColor(0), colors.Colors.GetArrayIndex(0),
//           ////     (byte)StartColor(1), colors.Colors.GetArrayIndex(1),
//           ////     (byte)StartColor(2), colors.Colors.GetArrayIndex(2));
//           //// image =
//           ////     Editor.VoxelObjBuilder.BuildFromVoxels(Editor.VoxelObjDataLoader.StandardLimits, voxels.Voxels,
//           ////     Editor.VoxelObjBuilder.CenterAdjust(Editor.VoxelObjDataLoader.StandardLimits));

//           // image.Scale = lib.V3(0.1f);
//           // image.Position = pos;
//           // if (physics != null)
//           //     physics.UpdatePosFromParent();
//           // moveImage(new Velocity(rotation, 1), 0);
//        }
//        static readonly LootFest.ObjSingleBound HelicopterBound = LootFest.ObjSingleBound.QuickBoundingBox(3f);
//        override protected LootFest.ObjSingleBound bound
//        { get { return HelicopterBound; } }
//        public override string ToString()
//        {
//            return "RC Helicopter";
//        }
//    }
//}
