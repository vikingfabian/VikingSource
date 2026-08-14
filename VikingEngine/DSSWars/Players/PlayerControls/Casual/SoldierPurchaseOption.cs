using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    enum CasualSoldierType
    {
        Guard,
        FolkMen,
        Seamen,
        Melee,
        Ranged,
        Rider,
        Siege,
        Settler,
    }

    struct SoldierPurchaseOption
    {
        public int price;
        public int upgradePrice;
        public ItemResourceType armor;
        public ItemResourceType weapon;
        public TrainingLevel training;

        public SoldierPurchaseOption(int price,
            ItemResourceType armor, ItemResourceType weapon, TrainingLevel training)
        {
            this.price = price;
            upgradePrice = 0;
            this.armor = armor;
            this.weapon = weapon;
            this.training = training;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)price);
            w.Write((byte)weapon);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            price = r.ReadUInt16();
            weapon = (ItemResourceType)r.ReadByte();
        }

        public int FullPrice => price + upgradePrice;

        public bool Available => price > 0;

        public SoldierConscriptProfile SoldierProfile()
        {
            SoldierConscriptProfile soldierConscript = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile() { weapon = weapon },

            };

            return soldierConscript;
        }

        public void ButtonVisuals(CasualSoldierType soldierType, out SpriteName icon, out string caption)
        {
            if (soldierType == CasualSoldierType.Guard)
            {
                icon = SpriteName.WarsGuard;
                caption = DssRef.lang.Conscript_Soldiers_GuardType;
            }
            else
            {
                var profile = SoldierProfile();
                icon = profile.Icon();
                caption = profile.conscript.TypeName();
            }
        }


    }
}
