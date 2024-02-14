//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    class RCCar : AbsRCCar
//    {
//        public RCCar(Characters.Hero parent)
//            : base(parent)
//        {
            
//        }
//        public RCCar(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }
//        protected override float ProjectileDamage
//        {
//            get { return LightDamage; }
//        }
//        override public void ViewControls(List<HUD.ButtonDescriptionData> toGroup)
//        {
//            toGroup.Add(new HUD.ButtonDescriptionData("Wheel", TileName.LeftStick));//Pad.Left, HUD.StickDirType.LeftRight, "Wheel");
//            toGroup.Add(new HUD.ButtonDescriptionData("Gas", TileName.ButtonX));//(numBUTTON.X, "Gas");
//            toGroup.Add(new HUD.ButtonDescriptionData("Brake", TileName.ButtonB));//numBUTTON.B, "Brake");
//            base.ViewControls(toGroup);
//        }
//        override protected float wepReloadTime { get { return 2000; } }
//        override protected float wepFireRate { get { return 200; } }
//        override protected int wepNumBullets { get { return 4; } }

//        public override int UnderType
//        {
//            get { return (int)ToyType.LightCar; }
//        }
//        //protected override VoxelModelName imageName
//        //{
//        //    get { return VoxelModelName.RCCar1; }
//        //}
//        public override RcCategory RcCategory
//        {
//            get { return Toys.RcCategory.Car; }
//        }
//        override protected VoxelModelName VoxelObj
//        { get { return VoxelModelName.RCCar1; } }
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
//        public override string ToString()
//        {
//            return "RC Car";
//        }
//    }
//}
