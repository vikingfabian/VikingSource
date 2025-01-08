using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class SpecialArrow : AbsItem
    {
        public const int MailPierceValue = 2;

        public ArrowType arrowType;
        public ArrowSpecialType specialType;

        public SpecialArrow(ArrowType arrowType, ArrowSpecialType specialType, int count)
            :base(arrowType == ArrowType.Arrow ? ItemMainType.SpecialArrow : ItemMainType.SpecialBolt, 
                 (int)specialType)
        {
            this.count = count;
            this.arrowType = arrowType;
            this.specialType = specialType;
        }

        public string ArrowName()
        {
            switch (arrowType)
            {
                case ArrowType.Arrow: return "arrow";
                case ArrowType.Bolt: return "bolt";
            }
            return "ERR";
        }

        public override string Name
        {
            get
            {
                switch (specialType)
                {
                    case ArrowSpecialType.Piercing2:
                        return "Mail piercing " + ArrowName();

                    case ArrowSpecialType.Normal:
                        return "Standard " + ArrowName();
                }

                throw new NotImplementedException();
            }
        }

        public override string Description
        {
            get
            {
                switch (specialType)
                {
                    case ArrowSpecialType.Piercing2:
                        return "Gain " + LanguageLib.AccValue(MailPierceValue) + " pierce";

                    default:
                        return null;
                }
            }
        }

        public override List<AbsRichBoxMember> DescriptionIcons()
        {
            switch (specialType)
            {
                case ArrowSpecialType.Piercing2:
                    return arraylib.Repeate_List<AbsRichBoxMember>(
                        new HUD.RichBox.RbImage(SpriteName.cmdPierce), MailPierceValue);
                default:
                    return null;
            }
        }

        public override SpriteName Icon
        {
            get
            {
                switch (specialType)
                {
                    case ArrowSpecialType.Piercing2:
                        return SpriteName.cmdArrowPierce;

                    case ArrowSpecialType.Normal:
                        return SpriteName.cmdArrowRegular;

                    default:
                        return SpriteName.MissingImage;
                }
            }
        }

        public override void quickUse(LocalPlayer player, IntVector2 pos)
        {
            base.quickUse(player, pos);
        }
        //public override ItemMainType MainType => arrowType == ArrowType.Arrow? 
        //    ItemMainType.SpecialArrow : ItemMainType.SpecialBolt;

        //public override int SubType => (int)specialType;
        
        public override EquipSlots Equip => EquipSlots.None;

        public override int StackLimit => MaxStackCount;
    }
    

    enum ArrowType
    {
        Arrow,
        Bolt,
    }

    enum ArrowSpecialType
    {
        Normal,
        Piercing2,
    }
}
