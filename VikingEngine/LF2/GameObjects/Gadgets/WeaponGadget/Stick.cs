using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets.WeaponGadget
{
    /// <summary>
    /// The starting weapon of a hero, will never break
    /// </summary>
    class Stick : HandWeapon
    {
        public const float StickScale =  0.14f;

        public Stick()
        {
            weaponType = Data.Gadgets.BluePrint.Stick;
            damage = new WeaponAttack.DamageData(LootfestLib.WoodenStickDamage, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, GoodsType.Stick); 
            attacktime = 500;
            reloadtime = 400;
            
            updateScale();
        }

        protected override void CreateMesh()
        {
            LoadMasterImage(VoxelModelName.stick);
        }

        public override void WriteStream(System.IO.BinaryWriter w)
        {
            throw new Exception();
            //base.WriteStream(w);
        }

        public override string ToString()
        {
            return "Stick";
        }

        public override SpriteName Icon
        {
            get { return SpriteName.WeaponStick; }
        }

        public override string GadgetInfo
        {
            get
            {
                return damage.DamageText() + TextLib.NewLine + "The stick is your last resource if you have no other weapon to defend yourself with";
            }
        }
        
        public override bool Scrappable
        {
            get
            {
                return false;
            }
        }
        public override GadgetSaveCategory SaveCategory
        {
            get
            {
                return GadgetSaveCategory.NUM_NONE;
            }
        }
        public override bool Empty
        {
            get
            {
                return true;
            }
        }
    }
}
