using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Conscript
{
    static class AutoConscriptLib
    {
        public static void autoWarQualityToolTip(RichBoxContent content, object tag)
        {
            WarAutoQuality quality = (WarAutoQuality)tag;
            switch (quality)
            {
                case WarAutoQuality.Low:
                    content.Add(new RbText(DssRef.todoLang.FastProduction, HudLib.InfoYellow_Light));
                    break;
                case WarAutoQuality.Medium:
                    HudLib.Label(content, DssRef.todoLang.BlocksProduction);
                    resource(true, ItemResourceType.SlingShot);
                    resource(true, ItemResourceType.SharpStick);
                    break;
                case WarAutoQuality.High:
                    HudLib.Label(content, DssRef.todoLang.BlocksProduction);
                    resource(true, ItemResourceType.SlingShot);
                    resource(true, ItemResourceType.ThrowingSpear);
                    resource(true, ItemResourceType.SharpStick);
                    resource(false, ItemResourceType.NONE);
                    content.newParagraph();
                    content.Add(new RbText(DssRef.todoLang.SlowProduction, HudLib.InfoYellow_Light));
                    break;

            }

            void resource(bool weapon, ItemResourceType resourceType)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.WarsHudCheckNo));
                content.space(0.5f);
                content.Add(new RbText((weapon ? DssRef.lang.Conscript_WeaponTitle : DssRef.lang.Conscript_ArmorTitle) + ":", HudLib.TitleColor_TypeName));
                content.space();
                content.Add(new RbImage(ResourceLib.Icon(resourceType)));
                content.space();
                content.Add(new RbText(LangLib.Item(resourceType), HudLib.NotAvailableColor));
            }
        }

        public static bool MayUseItemInConscript(City city, ItemResourceType item)
        {
            switch (item)
            {
                
                case ItemResourceType.SharpStick:
                case ItemResourceType.SlingShot:
                    if (city.warAutoQuality >= WarAutoQuality.Medium)
                    {
                        return false;
                    }
                    break;

                case ItemResourceType.NONE://Armor
                case ItemResourceType.ThrowingSpear:
                    if (city.warAutoQuality == WarAutoQuality.High)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        public static bool HasEnoughMen(City city)
        {
            if (city.workForce.amount >= city.workersMax())
            {
                return true;
            }

            if (city.warAutoQuality == WarAutoQuality.Medium &&
                city.workForce.amount < city.workersMax() / 2)
            { //Will preserve half the population
                return false;
            }

            return city.workForce.amount < city.HousingCount_Workers - DssConst.SoldierGroup_DefaultCount;
        }
    }
}
