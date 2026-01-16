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
using Microsoft.Xna.Framework.Content;

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
            var res_food = city.GetRefGroupedResource(EntityComponent.CityResoureIndex.food);

            switch (city.warAutoQuality)
            {
                default:
                    return res_food.amount > 20;
                case WarAutoQuality.Medium:
                    return res_food.amount > 50;
                case WarAutoQuality.High:
                    return res_food.amount > res_food.stockPileLimit / 2;
            }
        }

        public static void WorkPriority(City city, ref WorkTemplate workTemplate)
        {
            int weaponPrio = city.automationFocus == AutomationFocus.Military ? 2 : 1;

            int lowQuality = weaponPrio;
            int mediumQuality = weaponPrio;

            int gunPrio = weaponPrio;
            int meleePrio = weaponPrio;
            int rangedPrio = weaponPrio;
            int warmashinePrio = weaponPrio;

            if (city.automationFocus == AutomationFocus.Military)
            {
                if (city.warAutoWeaponType != WarAutoWeaponType.Mix)
                {

                    switch (city.warAutoWeaponType)
                    {
                        case WarAutoWeaponType.Melee:

                            //workTemplate.craft_blackpowder.set(0);
                            //workTemplate.craft_gunpowder.set(0);
                            //workTemplate.craft_bullet.set(0);
                            gunPrio = 0;
                            rangedPrio = 0;
                            warmashinePrio = 0;
                            //setRanged(ref workTemplate);
                            //setWarmachines(ref workTemplate);
                            break;

                        case WarAutoWeaponType.Ranged:
                            meleePrio = 0;
                            warmashinePrio = 0;
                            //setMelee(ref workTemplate);
                            //setWarmachines(ref workTemplate);
                            break;
                        case WarAutoWeaponType.Warmachine:
                            meleePrio = 0;
                            rangedPrio = 0;
                            //setMelee(ref workTemplate);
                            //setRanged(ref workTemplate);
                            break;
                    }
                }
            }

            setGunPowder(ref workTemplate, gunPrio);
            setMelee(ref workTemplate, meleePrio);
            setRanged(ref workTemplate, rangedPrio);
            setWarmachines(ref workTemplate, warmashinePrio);


            if (city.automationFocus == AutomationFocus.Military &&
                city.warAutoQuality >= WarAutoQuality.Medium)
            {
                workTemplate.craft_slingshot.set(0);
                workTemplate.craft_sharpstick.set(0);

                if (city.warAutoQuality >= WarAutoQuality.High)
                {
                    workTemplate.craft_throwingspear.set(0);
                }
            }
            

            void setGunPowder(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.craft_blackpowder.set(prio);
                workTemplate.craft_gunpowder.set(prio);
                workTemplate.craft_bullet.set(prio);
            }

            void setMelee(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.craft_sharpstick.set(prio);
                workTemplate.craft_bronzesword.set(prio);
                workTemplate.craft_shortsword.set(prio);
                workTemplate.craft_sword.set(prio);
                workTemplate.craft_longsword.set(prio);
                workTemplate.craft_handspear.set(prio);
                workTemplate.craft_mithrilsword.set(prio);
                workTemplate.craft_warhammer.set(prio);
                workTemplate.craft_twohandsword.set(prio);
                workTemplate.craft_knightslance.set(prio);
            }

            void setRanged(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.craft_slingshot.set(prio);
                workTemplate.craft_throwingspear.set(prio);
                workTemplate.craft_bow.set(prio);
                workTemplate.craft_longbow.set(prio);
                workTemplate.craft_crossbow.set(prio);
                workTemplate.craft_mithrilbow.set(prio);

                workTemplate.craft_handcannon.set(prio);
                workTemplate.craft_handculverin.set(prio);
                workTemplate.craft_rifle.set(prio);
                workTemplate.craft_blunderbus.set(prio);
            }

            void setWarmachines(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.craft_ballista.set(prio);
                workTemplate.craft_manuballista.set(prio);
                workTemplate.craft_catapult.set(prio);
                workTemplate.craft_batteringram.set(prio);

                workTemplate.craft_siegecannonbronze.set(prio);
                workTemplate.craft_mancannonbronze.set(prio);
                workTemplate.craft_siegecannoniron.set(prio);
                workTemplate.craft_mancannoniron.set(prio);
            }

            

        }
    }
}
