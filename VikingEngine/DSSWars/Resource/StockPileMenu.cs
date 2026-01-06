using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Animal;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

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

        public void toHud(LocalPlayer player, ResourceGroupType tab)
        {
            ResourceGroupType groupType = ResourceGroupType.NUM;

            switch (tab)
            {
                case ResourceGroupType.Resources:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    groupType = ResourceGroupType.Resources;
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

                    groupType = ResourceGroupType.Metals;
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
                    groupType = ResourceGroupType.Weapons;
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

                    groupType = ResourceGroupType.Projectile;
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

                    groupType = ResourceGroupType.Armor;
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

            content.newParagraph();
            HudLib.Label(content, ".Current page"); content.space();
            copyPasteOptions(groupType);

            content.newParagraph();
            HudLib.Label(content, ".All pages"); content.space();
            copyPasteOptions(ResourceGroupType.NUM);


            void copyPasteOptions(ResourceGroupType group)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsHudIconCopy) ,
                new RbSpace(),
                new RbText(DssRef.lang.Hud_Copy)},
                new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                    player, faction, city, CopyPasteOption.ToMemory, group, RbSoundType.Copy)));

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsHudIconPaste) ,
                new RbSpace(),
                new RbText(DssRef.lang.Hud_Paste)},
                   new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                        player, faction, city, CopyPasteOption.FromMemory, group, RbSoundType.Paste), null, player.stockPileCopy != null));

                content.newLine();

                if (city == null)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsHudIconExport) ,
                        new RbSpace(),
                        new RbText(".To all cities")},
                       new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                            player, faction, null, CopyPasteOption.ToAllCities, group, RbSoundType.Copy), new RbTooltip_Text(".Use the faction wide setting")));
                }
                else
                {

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsHudIconExport) ,
                        new RbSpace(),
                        new RbText(".To faction")},
                       new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                            player, faction, city, CopyPasteOption.CityToFaction, group, RbSoundType.Copy), new RbTooltip_Text(".Use the faction wide setting")));

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsHudIconImport) ,
                        new RbSpace(),
                        new RbText(".From faction")},
                        new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                            player, faction, city, CopyPasteOption.FactionToCity, group, RbSoundType.Copy), new RbTooltip_Text(".Use the faction wide setting")));
                }
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
                    stockPileLimit = faction.GetResourceOverview(item).stockPileLimit
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

                Color? numberCol = null;
                if (ResourceLib.Limit(limit) >= res.stockPileLimit)
                {
                    numberCol = HudLib.SecondaryTextColor;
                }

                switch (limit)
                {                    
                    case StockpileLimitOption.NoLimit:
                        switch (ItemPropertyColl.Get(item).storageType)
                        { 
                            default:
                            case StorageType.MaterialStorage:
                                storage = SpriteName.WarsBuild_MaterialStorage;
                                break;
                            case StorageType.FoodStorage:
                                storage = SpriteName.WarsBuild_FoodStorage;
                                break;
                            case StorageType.WeaponStorage:
                                storage = SpriteName.WarsBuild_WeaponStorage;
                                break;
                            case StorageType.ArmorStorage:
                                storage = SpriteName.WarsBuild_ArmorStorage;
                                break;
                            case StorageType.AnimalStorage:
                                storage = SpriteName.WarsBuild_AnimalStorage;
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
                    case StockpileLimitOption.Zero:
                        buttonContent.Add(new RbText("0", numberCol));
                        break;
                    case StockpileLimitOption.Value100:
                        buttonContent.Add(new RbText(DssRef.lang.EngineHud_SymbolFor100, numberCol));
                        break;
                    case StockpileLimitOption.Value500:
                        buttonContent.Add(new RbText("5" + DssRef.lang.EngineHud_SymbolFor100, numberCol));
                        break;
                    case StockpileLimitOption.Value2000:
                        buttonContent.Add(new RbText("2" + DssRef.lang.EngineHud_SymbolFor1000, numberCol));
                        break;

                }

                content.Add(new ArtOption(limit == res.limitOption, buttonContent, 
                    new RbAction1Arg<StockpileLimitOption>((StockpileLimitOption limit) => {

                        if (city != null)
                        {
                            ref GroupedResource res = ref city.GetRefGroupedResource(item);
                            res.limitOption = limit;

                        }
                        else
                        {
                            ref GroupedResource res = ref faction.GetRefResourceOverview(item);
                            res.limitOption = limit;
                        }

                        
                    }, limit), 
                    new RbTooltip(limitTooltip, new LimitTooltipArgs() { limit = limit, res = res })));
            }

            //stockPileEdit(content, item, res);
        }

        struct LimitTooltipArgs
        {
            public StockpileLimitOption limit;
            public GroupedResource res;
        }

        void limitTooltip(RichBoxContent content, object tag)
        {
            LimitTooltipArgs args = (LimitTooltipArgs)tag;
            if (args.limit == StockpileLimitOption.NoLimit)
            {
                content.h1(".No limit", HudLib.TitleColor_Head);
                content.text(".Will stockpile up to the storage size");
            }
            else
            {
                content.h1(string.Format( ".Limit stockpile to {0}", ResourceLib.Limit(args.limit)), HudLib.TitleColor_Head);
                content.text(DssRef.lang.Resource_StockPile_Info);
            }
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
