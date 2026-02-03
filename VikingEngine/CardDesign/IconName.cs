using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;

namespace VikingEngine.CardDesign
{
    static class IconName
    {
        public static string Select(TargetSelectType type)
        {
            return type switch
            {
                TargetSelectType.First => "First available",
                TargetSelectType.Self => "Self",
                TargetSelectType.All => "All",
                TargetSelectType.ManualSelect => "Manual selection",
                TargetSelectType.Area => "Area",
                TargetSelectType.Row => "Entire row",
                TargetSelectType.Lane => "Entire lane",
                TargetSelectType.Random => "Random",
                TargetSelectType.Closest => "Closest",
                TargetSelectType.Opposite => "Directly opposite",
                TargetSelectType.LeftMost => "Left-most",
                TargetSelectType.CenterMost => "Center-most",
                TargetSelectType.RightMost => "Right-most",
                TargetSelectType.Flank => "Flanks",
                TargetSelectType.Adjacent => "Adjacents",
                TargetSelectType.LeftOfMe => "Target to the left",
                TargetSelectType.RightOfMe => "Target to the right",
                TargetSelectType.FrontOfMe => "Target in front",
                TargetSelectType.BehindMe => "Target behind",
                _ => "Unknown Selection"
            };
        }

        public static string Filter(TargetFilterType type)
        {
            return type switch
            {
                TargetFilterType.HasTag => "Has tag",
                TargetFilterType.HasResource => "Has resource",
                TargetFilterType.HasHealth => "Has health",
                TargetFilterType.HasAttack => "Has attack",
                TargetFilterType.Friendly => "Friendly",
                TargetFilterType.Enemy => "Enemy",
                _ => "Unknown Filter"
            };
        }




        //public static void Resource(DefaultResourceType type, out SpriteName icon, out string name)
        //{
        //    switch (type)
        //    {
        //        case DefaultResourceType.WildMana:
        //            icon = SpriteName.MissingImage;
        //            name = "Wild Mana";
        //            break;

        //        case DefaultResourceType.ActionPoint:
        //            icon = SpriteName.LfMenuMoreMenusArrow;
        //            name = "Action Point";
        //            break;

        //        case DefaultResourceType.Mana:
        //            icon = SpriteName.CardIconMana;
        //            name = "Mana";
        //            break;

        //        case DefaultResourceType.RedMana:
        //            icon = SpriteName.CardIconManaRed;
        //            name = "Red Mana";
        //            break;

        //        case DefaultResourceType.GreenMana:
        //            icon = SpriteName.CardIconManaGreen;
        //            name = "Green Mana";
        //            break;

        //        case DefaultResourceType.BlueMana:
        //            icon = SpriteName.CardIconManaBlue;
        //            name = "Blue Mana";
        //            break;

        //        case DefaultResourceType.YellowMana:
        //            icon = SpriteName.CardIconManaYellow;
        //            name = "Yellow Mana";
        //            break;

        //        case DefaultResourceType.WhiteMana:
        //            icon = SpriteName.CardIconManaWhite;
        //            name = "White Mana";
        //            break;

        //        case DefaultResourceType.BlackMana:
        //            icon = SpriteName.CardIconManaBlack;
        //            name = "Black Mana";
        //            break;

        //        case DefaultResourceType.Coin:
        //            icon = SpriteName.CardIconCoin;
        //            name = "Coin";
        //            break;

        //        case DefaultResourceType.VictoryPoint:
        //            icon = SpriteName.CardIconVictoryPoint;
        //            name = "Victory Point";
        //            break;

        //        case DefaultResourceType.NUM_NONE:
        //        default:
        //            icon = SpriteName.MissingImage;
        //            name = TextLib.Error;
        //            break;
        //    }
        //}
    }
}
