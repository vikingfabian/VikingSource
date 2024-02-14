//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    class RCPlane : AbsFlyingRCtoy
//    {
//        HUD.ProcessBar gasMeter;
//        const float LiftRatio = 0.6f;
//        const float EngineMaxForce = 1.4f;
//        float engineForce = EngineMaxForce;
//        RangeF engineForceRange = new RangeF(0.1f, EngineMaxForce);
//        new Vector3 speed = Vector3.Zero;
//        override protected Vector3 Speed3d 
//        { 
//            get 
//            { 
//                return speed * 0.05f; 
//            } 
//        }

//        bool gasBreakManuver = false;
//        float timeSinceManuvering = 0;
//        Velocity pitch, yaw, roll;

//        public RCPlane(Characters.Hero parent, VectorRect drawArea)
//            : base(parent.Player)
//        {
//            const float RotationNaturalDecline = 0.9f;
//            const float PitchAcc = 0.007f;
//            const float MaxPitch = 0.04f;
//            pitch = new Velocity(MaxPitch, PitchAcc, RotationNaturalDecline);
//            const float YawAcc = 0.007f;
//            const float MaxYaw = 0.03f;
//            yaw = new Velocity(MaxYaw,YawAcc, RotationNaturalDecline);
//            const float RollAcc = 0.014f;
//            const float MaxRoll = 0.1f;
//            roll = new Velocity(MaxRoll, RollAcc, RotationNaturalDecline);

//            Vector2 meterSize = new Vector2(12, 120);
//            gasMeter = new HUD.ProcessBar(
//                new Vector2(drawArea.RightMostX - meterSize.X, drawArea.RightMostY - meterSize.Y), meterSize, ImageLayers.Lay4, Percent.Full, Color.Blue);
//            gasMeter.Value = Percent.Full;
//        }
//        public RCPlane(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }

//        protected override void BasicInit(Players.Player player)
//        {
//            base.BasicInit(player);
//            //Vector3 pos = Vector3.Zero;
//            //if (player != null)
//            //{
//            //    pos = player.HeroPos;
//            //}
//           // ImageSetup(VoxelModelName.RCplane1, pos, lib.V3(0.1f));
//            image.Position.Y += 8;
//        }


//        static readonly Vector3 EngineForceDir = Vector3.Backward;
//        static readonly Vector3 LiftForceDir = Vector3.Up;
//        public override void Time_Update(UpdateArgs args)
//        {
//#if CMODE
            
//            fireUpdate(time);
//            updateBoost(time);
//            if (!(LockControls || InMenu))
//            {
//                updateFlyingToy();
//                collectAngleHistory();
//                float forwardForce = engineForce;
//                if (boostTime > 0)
//                {
//                    forwardForce *= 1.4f;
//                }
//                Vector3 forwardDir = image.Rotation.TranslateAlongAxis(EngineForceDir * forwardForce, Vector3.Zero);
//                Vector3 force = forwardDir;
//                //If Vector3.Dot( vector1.Normalize(), vector2.Normalize() ) == 1, 
//                //the angle between the two vectors is 0 degrees; that is, the vectors point in the same direction and are parallel.
//                //Used to calc the speed in the planes forward direction
//                Vector3 speedDir = speed;
//                rotation = Rotation1D.FromDirection(Map.WorldPosition.V3toV2(speed));
//                forwardDir.Normalize();
//                speedDir = lib.SafeNormalizeV3(speedDir);
//                dir = speedDir;
//                float forwardSpeed = Vector3.Dot(forwardDir, speedDir) * speed.Length();

//                const float GravityForce = -1f * LiftRatio;
//                const float LiftForce = 2.4f * LiftRatio;
//                Vector3 downwardDir = image.Rotation.TranslateAlongAxis(LiftForceDir * LiftForce * forwardSpeed, Vector3.Zero);
//                force += downwardDir;
//                force.Y += GravityForce;
//                //Wing resistance force, any speed towards the wing creates a opposite force
//                downwardDir = lib.SafeNormalizeV3(downwardDir);
//                //downwardDir.Normalize();
//                float downwardSpeed = Vector3.Dot(downwardDir, speedDir) * speed.Length();
//                const float WingResistance = -1f;
//                force += downwardDir * downwardSpeed * WingResistance;
//                //ving motkraften borde ge en kraft framåt
//                const float DownwardSpeedToForwardForce = 2f;
//                force += forwardDir * Math.Abs(downwardSpeed) * DownwardSpeedToForwardForce;

//                speed += force * 0.1f;
//                image.Position += speed;

//                image.Rotation.RotateAxis(new Vector3(yaw.Value, pitch.Value, roll.Value));

//                //Speed decline
//                yaw.TimeUpdate(); pitch.TimeUpdate(); roll.TimeUpdate();
//                const float SpeedNaturalDecline = 0.8f;
//                speed *= SpeedNaturalDecline;

//                if (player.settings.FlyingRCStabilize)
//                {
//                    timeSinceManuvering += time;
//                    if (timeSinceManuvering > 400)
//                    {
//                        if (Math.Abs(image.Rotation.Zradians) > 0.02f)
//                        {
//                            roll.Accelerate(0.6f * -image.Rotation.Zradians);
//                        }
                        
//                        //image.Rotation.Zradians *= 0.97f;
//                    }
//                }
//                if (player.settings.FlyingRCAutoGas)
//                {
//                    if (!gasBreakManuver)
//                    {
//                        engineForce = lib.SetMaxFloatVal(engineForce + 1, engineForceRange.Max);
//                        updateGasMeter();
//                    }
//                    else
//                    {
//                        gasBreakManuver = false;
//                    }
//                }
//                physics.ObsticleCollision();
//            }
//#endif
//        }
//        public override void HandleTerrainColl3D(LootFest.TerrainColl collSata, Vector3 oldPos)
//        {
//            // const float CollDirSpeed = 0.002f; // Unused constant
//            Vector3 posDiff = oldPos - image.Position;
//            collisionDamage(posDiff);
//            posDiff = lib.SafeNormalizeV3(posDiff);
//            image.Position += posDiff * 0.5f;

//        }
//        public override void DeleteMe(bool local)
//        {
//            if (gasMeter != null)
//                gasMeter.DeleteMe();
//            base.DeleteMe(local);
//        }
//        //protected override ByteVector3 LowResRotation3d
//        //{
//        //    get
//        //    {
//        //        return new Rotation3D(image.Rotation.RotationV3).LowRes;
//        //    }
//        //    set
//        //    {
//        //       image.Rotation.RotationV3 = new Vector3(
//        //           Rotation1D.ByteToRadians( value.X), Rotation1D.ByteToRadians(value.Y), Rotation1D.ByteToRadians(value.Z));
//        //    }
//        //}

//        static readonly Vector3 PitchDir = new Vector3(0, 1, 0);
//        public override void Pad_Event(JoyStickValue e)
//        {
//#if CMODE
//            bool bPitch = player.settings.AirPlanePitchLS == (e.Stick == Stick.Left);
//            if (bPitch && player.settings.AirPlaneInvertPitch)
//            {
//                e.Direction.Y = -e.Direction.Y;
//            }
//            bool bRoll = player.settings.AirPlaneRollLS == (e.Stick == Stick.Left);

//            const float MaxValue = 0.7f;
//            if (Math.Abs(e.Direction.X) > MaxValue)
//                e.Direction.X = MaxValue * lib.FloatToDir(e.Direction.X);
//            if (bRoll)
//            {
//                //Roll, Zrot
//                roll.Accelerate(e.Direction.X);
//                timeSinceManuvering = 0;
//            }
//            else
//            {
//                //Yaw, turn left right, Xrot
//                yaw.Accelerate(-e.Direction.X);
//            }
//            if (bPitch)
//            {
//                if (Math.Abs(e.Direction.Y) > MaxValue)
//                    e.Direction.X = MaxValue * lib.FloatToDir(e.Direction.Y);
//                //Pitch, turn up/down, Yrot
//                pitch.Accelerate(e.Direction.Y);
//                timeSinceManuvering = 0;
//            }
//            else
//            {
//                if (Math.Abs(e.Direction.Y) >= 0.8f)
//                {
//                    //change enginge thrust
//                    engineForce = lib.SetBounds(engineForce + 0.08f * -e.Direction.Y, engineForceRange);
//                    updateGasMeter();
//                    gasBreakManuver = true;
//                }
//            }
//#endif
//        }
//        override protected float bulletYadj
//        { get { return 0.3f; } }
//        void updateGasMeter()
//        {
//            gasMeter.Value = new Percent(engineForce, engineForceRange.Max); ;
//        }

//        protected override float ProjectileDamage
//        {
//            get { return LightDamage; }
//        }

//        //override public void ViewControls(HUD.ExplainControlsGroup toGroup)
//        //{
//        //    toGroup.Add(Pad.Left, HUD.StickDirType.LeftRight, "Roll");
//        //    toGroup.Add(Pad.Left, HUD.StickDirType.UpDown, "Pitch");
//        //    toGroup.Add(Pad.Right, HUD.StickDirType.LeftRight, "Yaw");
//        //    toGroup.Add(Pad.Right, HUD.StickDirType.UpDown, "Gas");
//        //    base.ViewControls(toGroup);
//        //}
//        override public void ViewControls(List<HUD.ButtonDescriptionData> toGroup)
//        {
//#if CMODE
//            bool pitch = player.settings.HelicopterInvertPitch;
//            toGroup.Add(new HUD.ButtonDescriptionData("Pitch", player.settings.AirPlanePitchLS ? TileName.BoardCtrlLS_UD : TileName.BoardCtrlRS_UD));//Pad.Left, HUD.StickDirType.LeftRight, "Wheel");
//            toGroup.Add(new HUD.ButtonDescriptionData("Roll", player.settings.AirPlaneRollLS ? TileName.BoardCtrlLS_LR : TileName.BoardCtrlRS_LR));//(numBUTTON.X, "Gas");
//            toGroup.Add(new HUD.ButtonDescriptionData("Gas", !player.settings.AirPlaneRollLS ? TileName.BoardCtrlLS_UD : TileName.BoardCtrlRS_UD));
//            toGroup.Add(new HUD.ButtonDescriptionData("Yaw", !player.settings.AirPlanePitchLS ? TileName.BoardCtrlLS_LR : TileName.BoardCtrlRS_LR));//numBUTTON.B, "Brake");
//            base.ViewControls(toGroup);
//#endif
//        }
//        override protected float wepReloadTime { get { return 2000; } }
//        override protected float wepFireRate { get { return 160; } }
//        override protected int wepNumBullets { get { return 16; } }

//        public override int UnderType
//        {
//            get { return (int)ToyType.LightPlane; }
//        }
//        public override RcCategory RcCategory
//        {
//            get { return Toys.RcCategory.AirPlane; }
//        }
//        override protected VoxelModelName VoxelObj
//        { get { return VoxelModelName.RCplane1; } }

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
//        public override string ToString()
//        {
//            return "RC Plane";
//        }
//    }
//}
