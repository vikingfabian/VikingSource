//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    class RCTank : AbsRCVeihcle
//    {
//        float leftWheel;
//        float rightWheel;
//        //float velocity = 0;

//        public RCTank(Characters.Hero parent)
//            : base(parent)
//        {

//        }
//        public RCTank(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }
//        //public override ObjPhysicsType PhysicsType
//        //{
//        //    get
//        //    {
//        //        return GameObjects.ObjPhysicsType.Character;
//        //    }
//        //}
//        public override void Pad_Event(JoyStickValue e)
//        {
//            if (e.Stick == Stick.Right)
//                rightWheel = padToWheel(e);
//            else if (e.Stick == Stick.Left)
//                leftWheel = padToWheel(e);
//        }
//        public override void PadUp_Event(Stick padIx, int contolIx)
//        {
//            if (padIx == Stick.Right)
//                rightWheel = 0;
//            else if (padIx == Stick.Left)
//                leftWheel = 0;
//        }
//        float padToWheel(JoyStickValue e)
//        {
//            if (Math.Abs(e.Direction.Y) >= 0.9f)
//            {
//                e.Direction.Y = lib.FloatToDir(e.Direction.Y);
//            }
//            return -e.Direction.Y;
//        }

        
//        public override void Time_Update(UpdateArgs args)
//        {
//            if (!LockControls)
//            {
//                const float TurnSpeed = 0.10f;
//                float wheelTurn = leftWheel - rightWheel;
//                rotation.Add(TurnSpeed * wheelTurn);

//                const float MaxVelocity = 0.016f;
//                //turning the tank will reduce the top speed
//                const float RotationToSpeedReduce = 0.3f;
//                float max = MaxVelocity * (1 - Math.Abs(wheelTurn) * RotationToSpeedReduce);
//                const float Acc = 0.0016f;
//                float acceleration = Acc;
//                addTerrainEffect(ref acceleration, ref max);

//                velocity = lib.SetBounds(velocity + (leftWheel + rightWheel) * acceleration, -max, max);

//                Velocity.Set( rotation, velocity);

//                base.Time_Update(args);

//                const float VelocityNaturalDecline = 0.8f;
//                velocity *= VelocityNaturalDecline;
//            }
//        }
//        public override void HandleColl3D(Game1.Physics.Bound3dIntersect collData, GameObjects.AbsUpdateObj ObjCollision)
//        {
//            base.HandleColl3D(collData, ObjCollision);
//            velocity = 0;
//        }
//        protected override float ProjectileDamage
//        {
//            get { return LightDamage; }
//        }
//        //override public void ViewControls(HUD.ExplainControlsGroup toGroup)
//        //{
//        //    toGroup.Add(Pad.Left, HUD.StickDirType.UpDown, "Left thread");
//        //    toGroup.Add(Pad.Right, HUD.StickDirType.UpDown, "Right thread");

//        //    base.ViewControls(toGroup);
//        //}
//        override public void ViewControls(List<HUD.ButtonDescriptionData> toGroup)
//        {
//#if CMODE
//            bool pitch = player.settings.HelicopterInvertPitch;
//            toGroup.Add(new HUD.ButtonDescriptionData("Left thread", TileName.BoardCtrlLS_UD));
//            toGroup.Add(new HUD.ButtonDescriptionData("Right thread", TileName.BoardCtrlRS_UD));
//            base.ViewControls(toGroup);
//#endif
//        }
//        override protected float wepReloadTime { get { return 1000; } }
//        override protected float wepFireRate { get { return 200; } }
//        override protected int wepNumBullets { get { return 2; } }
//        //protected override VoxelModelName imageName
//        //{
//        //    get { return VoxelModelName.RCTank1; }
//        //}
//        public override int UnderType
//        {
//            get { return (int)ToyType.LightTank; }
//        }
//        public override RcCategory RcCategory
//        {
//            get { return Toys.RcCategory.Tank; }
//        }
//        override protected VoxelModelName VoxelObj
//        { get { return VoxelModelName.RCTank1; } }

//        override protected Data.MaterialType StartColor(int index)
//        {
//            switch (index)
//            {
//                case 0:
//                    return Data.MaterialType.dark_gray;
//                case 1:
//                    return Data.MaterialType.red_orange;
//                case 2:
//                    return Data.MaterialType.blue_gray;

//            }
//            throw new IndexOutOfRangeException();
//        }
//        protected override NetworkClientRotationUpdateType NetRotationType
//        {
//            get
//            {
//                return NetworkClientRotationUpdateType.Plane1D;
//            }
//        }
//        public override string ToString()
//        {
//            return "RC Tank";
//        }

//        //public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
//        //{
//        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
//        //}
//    }
//}
