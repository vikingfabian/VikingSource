using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Data
{
    static class Culture
    {
        public const float WheelWhrightBonus = 0.2f;

        public static void CultureToolTip(RichBoxContent content, CityCulture culture)
        {
            IconName.CityCulture(culture, out string title, out string description);
            

            var items = CultureAffectedItems(culture);
            if (items != null)
            {
                content.newParagraph();
                HudLib.Label(content, DssRef.lang.Culture_AffectedItems);
                foreach (var iconText in items)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(iconText.Icon));
                    content.hspace();
                    content.Add(new RbText(iconText.Text));
                }
            }

            content.h2(title, HudLib.TitleColor_Head);
            content.text(description);
            int factor = CulturePercChangeFactor(culture);
            if (factor >= 0)
            {
                content.text(string.Format(DssRef.lang.Hud_ChangeFactor, factor + "%"));
            }

            content.newParagraph();
            content.text(DssRef.lang.CityCultureDescription, HudLib.InfoYellow_Light);
        }

        public static int CulturePercChangeFactor(CityCulture culture)
        {
            switch (culture)
            {
                case CityCulture.LargeFamilies:
                    return 200;
                case CityCulture.FertileGround:
                    return 200;
                case CityCulture.Archers:
                    return 120;
                case CityCulture.Warriors:
                    return 120;
                //case CityCulture.AnimalBreeder:
                //    return 200;
                case CityCulture.Miners:
                    return 200;
                case CityCulture.Woodcutters:
                    return 200;
                case CityCulture.Builders:
                    return 50;
                case CityCulture.CrabMentality:
                    return 50;
                case CityCulture.DeepWell:
                    return 200;
                case CityCulture.Networker:
                    return 50;
                case CityCulture.PitMasters:
                    return 200;

                case CityCulture.Stonemason:
                    return 200;
                case CityCulture.Brewmaster:
                    return 150;
                case CityCulture.Weavers:
                    return 200;
                case CityCulture.SiegeEngineer:
                    return 120;
                case CityCulture.Armorsmith:
                    return 200;
                case CityCulture.Noblemen:
                    return 120;
                case CityCulture.Seafaring:
                    return 120;
                case CityCulture.Backtrader:
                    return 50;
                case CityCulture.Lawbiding:
                    return 200;

                case CityCulture.Smelters:
                    return 200;
                case CityCulture.BronzeCasters:
                    return 200;
                case CityCulture.Apprentices:
                    return -1;

                case CityCulture.Nomads:
                    return 50;

                case CityCulture.Butchers:
                    return 125;
                case CityCulture.Skinner:
                    return 125;
                case CityCulture.AnimalBreeder2:
                    return 200;

                case CityCulture.Wainwright:
                    return 125;
                case CityCulture.Wheelwright:
                    return conv.ToPercentage(WheelWhrightBonus);
                case CityCulture.ShieldMaker:
                    return 125;
                case CityCulture.Potters:
                    return 150;
                case CityCulture.Coopers:
                    return 150;
                case CityCulture.Salters:
                    return 125;


                default:
                    return -1;
            }
        }

        public static List<IconAndText> CultureAffectedItems(CityCulture culture)
        {
            List<IconAndText> result = null;


            switch (culture)
            {
                case CityCulture.Armorsmith:
                    result = new List<IconAndText>(8);
                    addItem(ItemResourceType.BronzeArmor);
                    addItem(ItemResourceType.IronArmor);
                    addItem(ItemResourceType.HeavyIronArmor);
                    addItem(ItemResourceType.LightPlateArmor);
                    addItem(ItemResourceType.FullPlateArmor);
                    addItem(ItemResourceType.MithrilArmor);
                    break;

                case CityCulture.FertileGround:
                    result = new List<IconAndText>(8);
                    addBuilding(Build.BuildAndExpandType.OrchardApple);
                    addBuilding(Build.BuildAndExpandType.WheatFarm);
                    addBuilding(Build.BuildAndExpandType.LinenFarm);
                    addBuilding(Build.BuildAndExpandType.RapeSeedFarm);
                    addBuilding(Build.BuildAndExpandType.HempFarm);
                    break;
            }

            return result;

            void addItem(ItemResourceType item)
            {
                if (result == null)
                {
                    result = new List<IconAndText>(8);
                }
                IconName.Item(item, out var ic, out var nm);
                result.Add(new IconAndText(ic, nm));
            }
            void addBuilding(Build.BuildAndExpandType build)
            {
                if (result == null)
                {
                    result = new List<IconAndText>(8);
                }
                IconName.Building(build, out var ic, out var nm);
                result.Add(new IconAndText(ic, nm));
            }
        }
    }


    enum CityCulture
    {
        LargeFamilies,//
        FertileGround,//
        Archers,//
        Warriors,//

        Miners,//
        Woodcutters,//
        Builders,//
        CrabMentality,// 
        DeepWell,//
        Networker,//
        PitMasters,//

        Stonemason,//.
        Brewmaster,//.
        Weavers,//.
        SiegeEngineer,//.
        Armorsmith,//.

        Seafaring,//.
        Backtrader,//.
        Lawbiding,//.

        Smelters,//
        BronzeCasters,//
        Apprentices,//

        Noblemen,//-implemented
        Nomads, //Low settler cost-implemented

        Butchers, //Larger meat production -implemented
        Skinner,//Larger skin production -implemented
        AnimalBreeder2, //Higher chance of successful breeding -implemented

        Wainwright, //High wagon production -implemented
        Wheelwright, //Speed bonus to conscripted carts
        ShieldMaker, //High shield production -implemented

        Potters, //Higher pottery production -implemented
        Coopers, //High wood storage box production -implemented
        Salters, //High conserved food production  -implemented


        NUM_NONE
    }
}
