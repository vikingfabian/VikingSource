using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Animal;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.GO.Characters.Monsters;

namespace VikingEngine.DSSWars.Resource
{
    class StockPileMenu
    {
        static readonly List<float> StockPileControls = new List<float> { 100, 1000 };

        RichBoxContent content;
        City city; 
        Faction faction;
        public StockPileMenu(RichBoxContent content, City city, Faction faction)
        { 
            this.content = content;
            this.city = city;
            this.faction = faction;
        }

        public void toHud(ResourceGroupType tab)
        {
            switch (tab)
            {
                case ResourceGroupType.Resources:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);

                    stockpile(ItemResourceType.Wood_Group);
                    stockpile(ItemResourceType.Stone_G);
                    stockpile(ItemResourceType.Brick);
                    stockpile(ItemResourceType.RawFood_Group);
                    stockpile(ItemResourceType.SkinLinen_Group);
                    content.newParagraph();

                    stockpile(ItemResourceType.Food_G);
                    stockpile(ItemResourceType.Fuel_G);
                    stockpile(ItemResourceType.Beer);
                    stockpile(ItemResourceType.CoolingFluid);
                    content.newParagraph();

                    stockpile(ItemResourceType.Container);
                    stockpile(ItemResourceType.Palisade);
                    stockpile(ItemResourceType.Toolkit);
                    stockpile(ItemResourceType.Wagon2Wheel);
                    stockpile(ItemResourceType.Wagon4Wheel);
                    stockpile(ItemResourceType.WagonClosed);
                    stockpile(ItemResourceType.WagonIron);
                    stockpile(ItemResourceType.WagonSteel);
                    stockpile(ItemResourceType.BlackPowder);
                    stockpile(ItemResourceType.GunPowder);
                    stockpile(ItemResourceType.LedBullet);

                    //content.newParagraph();
                    //HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
                    //GroupedResource.BufferIconInfo(content, false);
                    break;

                case ResourceGroupType.Metals:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);

                    stockpile(ItemResourceType.IronOre_G);
                    stockpile(ItemResourceType.TinOre);
                    stockpile(ItemResourceType.CopperOre);
                    stockpile(ItemResourceType.LeadOre);
                    stockpile(ItemResourceType.SilverOre);
                    stockpile(ItemResourceType.GoldOre);
                    content.newParagraph();

                    stockpile(ItemResourceType.Iron_G);
                    stockpile(ItemResourceType.Tin);
                    stockpile(ItemResourceType.Copper);
                    stockpile(ItemResourceType.Lead);
                    stockpile(ItemResourceType.Silver);
                    stockpile(ItemResourceType.RawMithril);
                    stockpile(ItemResourceType.Sulfur);
                    content.newParagraph();

                    stockpile(ItemResourceType.Bronze);
                    stockpile(ItemResourceType.CastIron);
                    stockpile(ItemResourceType.BloomeryIron);
                    stockpile(ItemResourceType.Steel);
                    stockpile(ItemResourceType.Mithril);

                    break;
                case ResourceGroupType.Weapons:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    stockpile(ItemResourceType.SharpStick);
                    stockpile(ItemResourceType.BronzeSword);
                    stockpile(ItemResourceType.ShortSword);
                    stockpile(ItemResourceType.Sword);
                    stockpile(ItemResourceType.LongSword);
                    stockpile(ItemResourceType.HandSpear);
                    stockpile(ItemResourceType.Warhammer);
                    stockpile(ItemResourceType.TwoHandSword);
                    //stockpile(ItemResourceType.KnightsLance);
                    stockpile(ItemResourceType.MithrilSword);
                    content.newParagraph();

                    stockpile(ItemResourceType.BucklerShield);
                    stockpile(ItemResourceType.RoundShield);
                    stockpile(ItemResourceType.HeaterShield);
                    stockpile(ItemResourceType.TowerShield);
                    break;

                case ResourceGroupType.Projectile:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);

                    stockpile(ItemResourceType.SlingShot);
                    stockpile(ItemResourceType.ThrowingSpear);
                    stockpile(ItemResourceType.Bow);
                    stockpile(ItemResourceType.LongBow);
                    stockpile(ItemResourceType.Crossbow);
                    stockpile(ItemResourceType.MithrilBow);
                    content.newParagraph();

                    stockpile(ItemResourceType.HandCannon);
                    stockpile(ItemResourceType.HandCulverin);
                    stockpile(ItemResourceType.Rifle);
                    stockpile(ItemResourceType.Blunderbuss);

                    content.newParagraph();

                    stockpile(ItemResourceType.Ballista);
                    stockpile(ItemResourceType.Manuballista);
                    stockpile(ItemResourceType.Catapult);

                    stockpile(ItemResourceType.SiegeCannonBronze);
                    stockpile(ItemResourceType.ManCannonBronze);
                    stockpile(ItemResourceType.SiegeCannonIron);
                    stockpile(ItemResourceType.ManCannonIron);

                    break;

                case ResourceGroupType.Armor:
                    
                    stockpile(ItemResourceType.HeavyPaddedArmor);
                    stockpile(ItemResourceType.PaddedArmor);
                    stockpile(ItemResourceType.BronzeArmor);
                    stockpile(ItemResourceType.IronArmor);
                    stockpile(ItemResourceType.HeavyIronArmor);
                    stockpile(ItemResourceType.LightPlateArmor);
                    stockpile(ItemResourceType.FullPlateArmor);
                    stockpile(ItemResourceType.MithrilArmor);

                    content.newParagraph();

                    stockpile(ItemResourceType.MountHeavyPaddedArmor);
                    stockpile(ItemResourceType.MountPaddedArmor);
                    stockpile(ItemResourceType.MountBronzeArmor);
                    stockpile(ItemResourceType.MountIronArmor);
                    stockpile(ItemResourceType.MountHeavyIronArmor);
                    stockpile(ItemResourceType.MountLightPlateArmor);
                    stockpile(ItemResourceType.MountFullPlateArmor);
                    stockpile(ItemResourceType.MountMithrilArmor);
                    break;

                case ResourceGroupType.Animals:
                    stockpile(ItemResourceType.Hen);
                    stockpile(ItemResourceType.Pig);
                    stockpile(ItemResourceType.Oxen);
                    stockpile(ItemResourceType.KineOxen);
                    content.newParagraph();
                    stockpile(ItemResourceType.Dog);
                    stockpile(ItemResourceType.Hound);
                    content.newParagraph();
                    stockpile(ItemResourceType.Pony);
                    stockpile(ItemResourceType.Horse);
                    stockpile(ItemResourceType.WarHorse);
                    stockpile(ItemResourceType.DraftHorse);
                    content.newParagraph();
                    stockpile(ItemResourceType.WildPig);
                    stockpile(ItemResourceType.WildHog);
                    stockpile(ItemResourceType.WarHog);
                    stockpile(ItemResourceType.StagHog);
                    content.newParagraph();
                    stockpile(ItemResourceType.Wolf);
                    stockpile(ItemResourceType.Warg);
                    stockpile(ItemResourceType.AlphaWarg);
                    content.newParagraph();
                    stockpile(ItemResourceType.WildCat);
                    stockpile(ItemResourceType.Lion);
                    stockpile(ItemResourceType.WarLion);
                    content.newParagraph();
                    stockpile(ItemResourceType.Elephant);
                    stockpile(ItemResourceType.WarElephant);
                    stockpile(ItemResourceType.Oliphant);
                    break;
            }
        }

        void stockpile(ItemResourceType item)
        {
            GroupedResource res;
            IconName.Item(item, out SpriteName itemIcon, out string itemName);

            if (city != null)
            {
                res = city.GetGroupedResource(item);
            }
            else
            {
                res = new GroupedResource() {
                    stockPileLimit = faction.GetResourceOverview(item).goalBuffer
                };
            }

            content.newLine();

            content.Add(new ArtButton(RbButtonStyle.HoverArea,
                new List<AbsRichBoxMember>{
                        new RbImage(res.amount >= res.stockPileLimit ? SpriteName.WarsStockpileStop : SpriteName.WarsStockpileAdd),
                        new RbImage(itemIcon)}, null,
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        ResourceLib.FullResourceInfo(city, item, content);
                        //if (city != null)
                        //{
                            
                        //    //bool buffer = false;
                        //    //city.GetGroupedResource(item).toMenu(content, item, false, ref buffer);
                        //}
                        //else
                        //{
                        //    content.Add(new RbImage(itemIcon));
                        //    content.space();
                        //    content.Add(new RbText(itemName));
                        //}
                    }
                    )));

            content.space();

            for (StockpileLimitOption limit = 0; limit < StockpileLimitOption.NUM; limit++)
            {
                int max = ResourceLib.Limit(limit);

                List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(2);
                SpriteName storage;
                switch (limit)
                {                    
                    case StockpileLimitOption.NoLimit:
                        switch (ItemPropertyColl.Get(item).storageType)
                        { 
                            default:
                            case StorageType.MaterialStorage:
                                storage = SpriteName.WarsBuild_MaterialStorage;
                                break;
                        }
                        buttonContent.Add(new RbImage(storage));
                        buttonContent.Add(new RbSpace());
                        if (city == null)
                        {
                            buttonContent.Add(new RbText(".Max"));
                        }
                        else
                        {
                            buttonContent.Add(new RbText(res.stockPileLimit.ToString()));
                        }
                        break;
                    case StockpileLimitOption.Value100:
                        buttonContent.Add(new RbText(DssRef.lang.EngineHud_SymbolFor100));
                        break;
                    case StockpileLimitOption.Value500:
                        buttonContent.Add(new RbText("5" + DssRef.lang.EngineHud_SymbolFor100));
                        break;
                    case StockpileLimitOption.Value2000:
                        buttonContent.Add(new RbText("2" + DssRef.lang.EngineHud_SymbolFor1000));
                        break;

                }

                content.Add(new ArtOption(limit == res.stockPileLimit, buttonContent, 
                    new RbAction1Arg<StockpileLimitOption>((StockpileLimitOption limit) => {

                        
                        if (city != null)
                        {
                            ref var res = ref city.GetRefGroupedResource(item);
                            res.stockPileLimit = limit;
                        }
                        else
                        {
                            ref var res = ref faction.GetRefResourceOverview(item);
                            res.stockPileLimit = limit;
                        }
                    }
            }

            //stockPileEdit(content, item, res);
        }

        //void stockPileEdit(RichBoxContent content, ItemResourceType item, GroupedResource res)
        //{
        //    IntGetSet property;

        //    if (city != null)
        //    {
        //        property = (bool set, int value) =>
        //        {

        //            var res = city.GetGroupedResource(item);
        //            if (set)
        //            {
        //                res.stockPileLimit = value;
        //                city.SetGroupedResource(item, res);
        //            }
        //            return res.stockPileLimit;
        //        };
        //    }
        //    else
        //    {
        //        property = (bool set, int value) =>
        //        {
        //            ref var res = ref faction.GetRefResourceOverview(item);
        //            if (set)
        //            {
        //                res.goalBuffer = value;
        //                //todo set all cities
        //            }

        //            return res.goalBuffer;
        //        };
        //    }

        //    RbDragButton.RbDragButtonGroup(content, StockPileControls, new DragButtonSettings(DssConst.StockPileMinBound, DssConst.StockPileMaxBound, 100),
        //        property, true);
        //}
    }
}
