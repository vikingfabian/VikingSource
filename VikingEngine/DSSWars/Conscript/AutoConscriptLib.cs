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
                IconName.Item(resourceType, out var icon, out var name);    

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsHudCheckNo));
                content.space(0.5f);
                content.Add(new RbText((weapon ? DssRef.lang.Conscript_WeaponTitle : DssRef.lang.Conscript_ArmorTitle) + ":", HudLib.TitleColor_TypeName));
                content.space();
                content.Add(new RbImage(icon));
                content.space();
                content.Add(new RbText(name, HudLib.NotAvailableColor));
            }
        }

        public static bool MayUseItemInConscript(City city, ItemResourceType item, bool isWeapon)
        {
            
            if (isWeapon && city.warAutoWeaponType != WarAutoWeaponType.Mix)
            {
                ConscriptProfile profile = new ConscriptProfile() { weapon = item };
                profile.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide);

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

        public static bool HasEnoughFoodAndGold(Faction faction, City city, bool guard, bool aggresive)
        {
            if (faction.GetGold(city) > DssConst.Gold_RichStatus)
            {
                //Too rich to care
                return true;
            }
               

            if (guard)
            {
                if (DssRef.storage.gameRuleset.centralGold)
                {
                    return faction.money.copper > 0 && (aggresive || faction.GoldSecDiff() > -(DssConst.UpkeepPerGuard_copp * Money.CopperToGold * 50));
                }
                else
                {
                    return city.money.GetGold() > 0 && (aggresive || city.previousIncome_copp > -(DssConst.UpkeepPerGuard_copp * 10));
                }
            }
            else
            {
                var res_food = city.GetRefGroupedResource(EntityComponent.CityResoureIndex.food);

                if (aggresive || res_food.changeRate.Change > -20)
                {
                    switch (city.warAutoQuality)
                    {
                        default:
                            return res_food.amount > 50;
                        case WarAutoQuality.Medium:
                            return res_food.amount > 200;
                        case WarAutoQuality.High:
                            return res_food.amount > res_food.stockPileLimit / 2;
                    }
                }
                else
                {
                    return false;
                }
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
                workTemplate.Get(WorkPriorityType.craftSlingshot).set(0);
                workTemplate.Get(WorkPriorityType.craftSharpStick).set(0);

                if (city.warAutoQuality >= WarAutoQuality.High)
                {
                    workTemplate.Get(WorkPriorityType.craftThrowingspear).set(0);
                }
            }

            void setGunPowder(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.Get(WorkPriorityType.craftBlackPowder).set(prio);
                workTemplate.Get(WorkPriorityType.craftGunPowder).set(prio);
                workTemplate.Get(WorkPriorityType.craftBullet).set(prio);
            }

            void setMelee(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.Get(WorkPriorityType.craftSharpStick).set(prio);
                workTemplate.Get(WorkPriorityType.craftBronzeSword).set(prio);
                workTemplate.Get(WorkPriorityType.craftShortSword).set(prio);
                workTemplate.Get(WorkPriorityType.craftSword).set(prio);
                workTemplate.Get(WorkPriorityType.craftLongSword).set(prio);
                workTemplate.Get(WorkPriorityType.craftHandSpear).set(prio);
                workTemplate.Get(WorkPriorityType.craftMithrilSword).set(prio);
                workTemplate.Get(WorkPriorityType.craftWarhammer).set(prio);
                workTemplate.Get(WorkPriorityType.craftTwoHandSword).set(prio);
                // workTemplate.Get(WorkPriorityType.craftKnightslance).set(prio); // TODO: Add to Enum
            }

            void setRanged(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.Get(WorkPriorityType.craftSlingshot).set(prio);
                workTemplate.Get(WorkPriorityType.craftThrowingspear).set(prio);
                workTemplate.Get(WorkPriorityType.craftBow).set(prio);
                workTemplate.Get(WorkPriorityType.craftLongbow).set(prio);
                workTemplate.Get(WorkPriorityType.craftCrossbow).set(prio);
                workTemplate.Get(WorkPriorityType.craftMithrilbow).set(prio);

                workTemplate.Get(WorkPriorityType.craftHandCannon).set(prio);
                workTemplate.Get(WorkPriorityType.craftHandCulverin).set(prio);
                workTemplate.Get(WorkPriorityType.craftRifle).set(prio);
                workTemplate.Get(WorkPriorityType.craftBlunderbuss).set(prio);
            }

            void setWarmachines(ref WorkTemplate workTemplate, int prio)
            {
                workTemplate.Get(WorkPriorityType.craftBallista).set(prio);
                workTemplate.Get(WorkPriorityType.craftManuBallista).set(prio);
                workTemplate.Get(WorkPriorityType.craftCatapult).set(prio);
                workTemplate.Get(WorkPriorityType.craftBatteringRam).set(prio);

                workTemplate.Get(WorkPriorityType.craftSiegeCannonBronze).set(prio);
                workTemplate.Get(WorkPriorityType.craftManCannonBronze).set(prio);
                workTemplate.Get(WorkPriorityType.craftSiegeCannonIron).set(prio);
                workTemplate.Get(WorkPriorityType.craftManCannonIron).set(prio);
            }



        }
    }
}
