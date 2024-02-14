using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace VikingEngine.LootFest.Creation.Weapon
//{
//    class Sword : GameObjects.Gadgets.WeaponGadget.HandWeapon
//    {
//        public Sword(int level, GameObjects.AbsUpdateObj owner, VoxelModelName img)
//            : base(Data.Gadgets.BluePrint.Sword, GameObjects.Gadgets.GoodsType.Iron)//new Data.Gadgets.CraftingTemplate(), img)
//        {

//            //damage.Magic = Magic.MagicType.Lightning;
//            float dam;
//            switch (level)
//            {
//                default:
//                    attacktime = 300;
//                    reloadtime = 300;
//                    scale = 0.15f;
//                    dam = 1;
//                    break;
//                case 1:
//                    attacktime = 350;
//                    reloadtime = 270;
//                    scale = 0.17f;
//                    dam = 1.5f;
//                    break;
//                case 2:
//                    attacktime = 400;
//                    reloadtime = 200;
//                    scale = 0.23f;
//                    dam = 2;
//                    break;
//            }
//            //originalSwordMesh = LootfestLib.Images.StandardVoxelObjects[VoxelModelName.Sword1];
//            new Process.LoadImage(this, img);
//            damage = new GameObjects.WeaponAttack.DamageData(dam, GameObjects.WeaponAttack.WeaponUserType.Player, (owner == null ? ByteVector2.Zero : owner.ObjOwnerAndId), GameObjects.Gadgets.GoodsType.Iron); 
//        }
        
//        protected override void createMesh()
//        {
//            //base.createMesh();
//        }
//        public override string ToString()
//        {
//            return "Sword";
//        }
//    }
//}
