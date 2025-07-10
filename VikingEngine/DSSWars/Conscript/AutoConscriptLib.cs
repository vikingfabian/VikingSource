using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Work;
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
                    content.Add(new RbText(DssRef.lang.FastProduction, HudLib.InfoYellow_Light));
                    break;
                case WarAutoQuality.Medium:
                    HudLib.Label(content, DssRef.lang.BlocksProduction);
                    resource(true, ItemResourceType.SlingShot);
                    resource(true, ItemResourceType.SharpStick);
                    break;
                case WarAutoQuality.High:
                    HudLib.Label(content, DssRef.lang.BlocksProduction);
                    resource(true, ItemResourceType.SlingShot);
                    resource(true, ItemResourceType.ThrowingSpear);
                    resource(true, ItemResourceType.SharpStick);
                    resource(false, ItemResourceType.NONE);
                    content.newParagraph();
                    content.Add(new RbText(DssRef.lang.SlowProduction, HudLib.InfoYellow_Light));
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

        public static bool MayUseItemInConscript(City city, ItemResourceType item, bool isWeapon)
        {
            
            if (isWeapon && city.warAutoWeaponType != WarAutoWeaponType.Mix)
            {
                ConscriptProfile profile = new ConscriptProfile() { weapon = item };
                profile.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool knight, out bool warmachine);

                switch (city.warAutoWeaponType)
                {
                    case WarAutoWeaponType.Melee:
                        if (!meleeMan)
                        { 
                            return false;
                        }
                        break;
                    case WarAutoWeaponType.Ranged:
                        if (!rangedMan)
                        {
                            return false;
                        }
                        break;
                    case WarAutoWeaponType.Warmachine:
                        if (!warmachine)
                        {
                            return false;
                        }
                        break;
                }
            }

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

        public static bool HasEnoughFood(City city)
        {
            switch (city.warAutoQuality)
            {
                default:
                    return city.res_food.amount > 20;
                case WarAutoQuality.Medium:
                    return city.res_food.amount > 50;
                case WarAutoQuality.High:
                    return city.res_food.amount > city.res_food.goalBuffer / 2;
            }
        }

        public static void WorkPriority(City city, ref WorkTemplate workTemplate)
        {
            if (city.warAutoQuality >= WarAutoQuality.Medium)
            {
                workTemplate.craft_slingshot.set(0);
                workTemplate.craft_sharpstick.set(0);

                if (city.warAutoQuality >= WarAutoQuality.High)
                {
                    workTemplate.craft_throwingspear.set(0);
                }
            }

            if (city.warAutoWeaponType != WarAutoWeaponType.Mix)
            {

                switch (city.warAutoWeaponType)
                {
                    case WarAutoWeaponType.Melee:

                        workTemplate.craft_blackpowder.set(0);
                        workTemplate.craft_gunpowder.set(0);
                        workTemplate.craft_bullet.set(0);

                        removeRanged(ref workTemplate);
                        removeWarmachines(ref workTemplate);
                        break;

                    case WarAutoWeaponType.Ranged:
                        removeMelee(ref workTemplate);
                        removeWarmachines(ref workTemplate);
                        break;
                    case WarAutoWeaponType.Warmachine:
                        removeMelee(ref workTemplate);
                        removeRanged(ref workTemplate);
                        break;
                }


                void removeMelee(ref WorkTemplate workTemplate)
                {
                    workTemplate.craft_sharpstick.set(0);
                    workTemplate.craft_bronzesword.set(0);
                    workTemplate.craft_shortsword.set(0);
                    workTemplate.craft_sword.set(0);
                    workTemplate.craft_longsword.set(0);
                    workTemplate.craft_handspear.set(0);
                    workTemplate.craft_mithrilsword.set(0);
                    workTemplate.craft_warhammer.set(0);
                    workTemplate.craft_twohandsword.set(0);
                    workTemplate.craft_knightslance.set(0);
                }

                void removeRanged(ref WorkTemplate workTemplate)
                {
                    workTemplate.craft_slingshot.set(0);
                    workTemplate.craft_throwingspear.set(0);
                    workTemplate.craft_bow.set(0);
                    workTemplate.craft_longbow.set(0);
                    workTemplate.craft_crossbow.set(0);
                    workTemplate.craft_mithrilbow.set(0);

                    workTemplate.craft_handcannon.set(0);
                    workTemplate.craft_handculverin.set(0);
                    workTemplate.craft_rifle.set(0);
                    workTemplate.craft_blunderbus.set(0);
                }

                void removeWarmachines(ref WorkTemplate workTemplate)
                {
                    workTemplate.craft_ballista.set(0);
                    workTemplate.craft_manuballista.set(0);
                    workTemplate.craft_catapult.set(0);
                    workTemplate.craft_batteringram.set(0);

                    workTemplate.craft_siegecannonbronze.set(0);
                    workTemplate.craft_mancannonbronze.set(0);
                    workTemplate.craft_siegecannoniron.set(0);
                    workTemplate.craft_mancannoniron.set(0);
                }

            }

        }
    }
}
