using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
                    stockpile(player, ItemResourceType.Wood_Group);
                    stockpile(player, ItemResourceType.Stone_G);
                    stockpile(player, ItemResourceType.Clay);
                    stockpile(player, ItemResourceType.Brick);
                    stockpile(player, ItemResourceType.RawFood_Group);
                    stockpile(player, ItemResourceType.Salt);
                    stockpile(player, ItemResourceType.SkinLinen_Group);
                    content.newParagraph();

                    stockpile(player, ItemResourceType.Food_G);
                    stockpile(player, ItemResourceType.ConservedFood);
                    stockpile(player, ItemResourceType.Fuel_G);
                    stockpile(player, ItemResourceType.Beer);
                    stockpile(player, ItemResourceType.CoolingFluid);
                    content.newParagraph();

                    stockpile(player, ItemResourceType.Container);
                    stockpile(player, ItemResourceType.Palisade);
                    stockpile(player, ItemResourceType.Toolkit);
                    stockpile(player, ItemResourceType.Wagon2Wheel);
                    stockpile(player, ItemResourceType.Wagon4Wheel);
                    stockpile(player, ItemResourceType.WagonClosed);
                    stockpile(player, ItemResourceType.WagonIron);
                    stockpile(player, ItemResourceType.WagonSteel);
                    stockpile(player, ItemResourceType.BlackPowder);
                    stockpile(player, ItemResourceType.GunPowder);
                    stockpile(player, ItemResourceType.LedBullet);

                    break;

                case ResourceGroupType.Metals:
                   
                    groupType = ResourceGroupType.Metals;
                    stockpile(player, ItemResourceType.IronOre_G);
                    stockpile(player, ItemResourceType.TinOre);
                    stockpile(player, ItemResourceType.CopperOre);
                    stockpile(player, ItemResourceType.LeadOre);
                    stockpile(player, ItemResourceType.SilverOre);
                    stockpile(player, ItemResourceType.GoldOre);
                    content.newParagraph();

                    stockpile(player, ItemResourceType.Iron_G);
                    stockpile(player, ItemResourceType.Tin);
                    stockpile(player, ItemResourceType.Copper);
                    stockpile(player, ItemResourceType.Lead);
                    stockpile(player, ItemResourceType.Silver);
                    stockpile(player, ItemResourceType.RawMithril);
                    stockpile(player, ItemResourceType.Sulfur);
                    content.newParagraph();

                    stockpile(player, ItemResourceType.Bronze);
                    stockpile(player, ItemResourceType.CastIron);
                    stockpile(player, ItemResourceType.BloomeryIron);
                    stockpile(player, ItemResourceType.Steel);
                    stockpile(player, ItemResourceType.Mithril);

                    break;
                case ResourceGroupType.Weapons:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    groupType = ResourceGroupType.Weapons;
                    stockpile(player, ItemResourceType.SharpStick);
                    stockpile(player, ItemResourceType.BronzeSword);
                    stockpile(player, ItemResourceType.HandSpear);
                    stockpile(player, ItemResourceType.ShortSword);
                    stockpile(player, ItemResourceType.Sword);
                    stockpile(player, ItemResourceType.LongSword);
                    
                    stockpile(player, ItemResourceType.Warhammer);
                    stockpile(player, ItemResourceType.TwoHandSword);
                    //stockpile(player, ItemResourceType.KnightsLance);
                    stockpile(player, ItemResourceType.MithrilSword);
                    content.newParagraph();

                    stockpile(player, ItemResourceType.BucklerShield);
                    stockpile(player, ItemResourceType.RoundShield);
                    stockpile(player, ItemResourceType.HeaterShield);
                    stockpile(player, ItemResourceType.TowerShield);
                    break;

                case ResourceGroupType.Projectile:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);

                    groupType = ResourceGroupType.Projectile;
                    stockpile(player, ItemResourceType.SlingShot);
                    stockpile(player, ItemResourceType.ThrowingSpear);
                    stockpile(player, ItemResourceType.Bow);
                    stockpile(player, ItemResourceType.LongBow);
                    stockpile(player, ItemResourceType.Crossbow);
                    stockpile(player, ItemResourceType.MithrilBow);
                    content.newParagraph();

                    stockpile(player, ItemResourceType.HandCannon);
                    stockpile(player, ItemResourceType.HandCulverin);
                    stockpile(player, ItemResourceType.Rifle);
                    stockpile(player, ItemResourceType.Blunderbuss);

                    content.newParagraph();

                    stockpile(player, ItemResourceType.Ballista);
                    stockpile(player, ItemResourceType.Manuballista);
                    stockpile(player, ItemResourceType.Catapult);

                    stockpile(player, ItemResourceType.SiegeCannonBronze);
                    stockpile(player, ItemResourceType.ManCannonBronze);
                    stockpile(player, ItemResourceType.SiegeCannonIron);
                    stockpile(player, ItemResourceType.ManCannonIron);

                    break;

                case ResourceGroupType.Armor:

                    groupType = ResourceGroupType.Armor;
                    stockpile(player, ItemResourceType.HeavyPaddedArmor);                 
                    stockpile(player, ItemResourceType.BronzeArmor);
                    stockpile(player, ItemResourceType.IronArmor);
                    stockpile(player, ItemResourceType.HeavyIronArmor);
                    stockpile(player, ItemResourceType.LightPlateArmor);
                    stockpile(player, ItemResourceType.FullPlateArmor);
                    stockpile(player, ItemResourceType.MithrilArmor);

                    content.newParagraph();
                    stockpile(player, ItemResourceType.MountPaddedArmor);
                    stockpile(player, ItemResourceType.MountHeavyPaddedArmor);                    
                    stockpile(player, ItemResourceType.MountBronzeArmor);
                    stockpile(player, ItemResourceType.MountIronArmor);
                    stockpile(player, ItemResourceType.MountHeavyIronArmor);
                    stockpile(player, ItemResourceType.MountLightPlateArmor);
                    stockpile(player, ItemResourceType.MountFullPlateArmor);
                    stockpile(player, ItemResourceType.MountMithrilArmor);
                    break;

                case ResourceGroupType.Animals:
                    groupType = ResourceGroupType.Animals;

                    stockpile(player, ItemResourceType.Fowl);
                    stockpile(player, ItemResourceType.Hen);
                    stockpile(player, ItemResourceType.Boar);
                    stockpile(player, ItemResourceType.Pig);
                    
                    stockpile(player, ItemResourceType.Oxen);
                    stockpile(player, ItemResourceType.KineOxen);
                    
                    stockpile(player, ItemResourceType.Dog);
                    stockpile(player, ItemResourceType.Hound);
                   
                    stockpile(player, ItemResourceType.Pony);
                    stockpile(player, ItemResourceType.Horse);
                    stockpile(player, ItemResourceType.WarHorse);
                    stockpile(player, ItemResourceType.DraftHorse);
                    
                    stockpile(player, ItemResourceType.WildPig);
                    stockpile(player, ItemResourceType.WildHog);
                    stockpile(player, ItemResourceType.WarHog);
                    stockpile(player, ItemResourceType.StagHog);
                   
                    stockpile(player, ItemResourceType.Wolf);
                    stockpile(player, ItemResourceType.Warg);
                    stockpile(player, ItemResourceType.AlphaWarg);
                   
                    stockpile(player, ItemResourceType.WildCat);
                    stockpile(player, ItemResourceType.Lion);
                    stockpile(player, ItemResourceType.WarLion);
                   
                    stockpile(player, ItemResourceType.Elephant);
                    stockpile(player, ItemResourceType.WarElephant);
                    stockpile(player, ItemResourceType.Oliphant);
                    break;
            }

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.Hud_CurrentPage); content.space();
            copyPasteOptions(groupType);
            setAll(groupType);

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.Hud_AllPages); content.space();
            copyPasteOptions(ResourceGroupType.NUM);

            void setAll(ResourceGroupType group)
            {
                content.newLine();

                setAllButton(0);
                setAllButton(100);
                setAllButton(200);
                setAllButton(500);
                setAllButton(int.MaxValue);


                void setAllButton(int value)
                {
                    RbImage icon;
                    RbText text;

                    if (value < ushort.MaxValue)
                    {
                        icon = new RbImage(SpriteName.WarsStockpileLimit);
                        text = new RbText(value.ToString());
                    }
                    else
                    {
                        icon = new RbImage(SpriteName.WarsBuild_MaterialStorage);
                        text = new RbText(DssRef.lang.Hud_Maximum);
                    }

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { icon, new RbSpace(0.5f), text },
                        new RbAction(() =>
                        {
                           var list = Resource.ResourceLib.ResourceGroupList(group);
                            foreach (var item in list)
                            {
                                if (city != null)
                                {
                                    ref GroupedResource resources = ref city.GetRefGroupedResource(item);
                                    resources.setLimit(value);
                                }
                                else
                                {
                                    ref GroupedResource resources = ref faction.GetRefResourceOverview(item);
                                    resources.capacity = value;
                                    resources.setLimit(value);
                                }
                            }
                            player.hud.needRefresh = true;
                        }), 
                        new RbTooltip_Text(DssRef.lang.StockPile_LimitTitle)));
                }
            }

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
                        new RbText(DssRef.lang.Hud_ToAllCities)},
                       new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                            player, faction, null, CopyPasteOption.ToAllCities, group, RbSoundType.Copy), new RbTooltip_Text(DssRef.lang.Hud_FactionWide)));
                }
                else
                {

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsHudIconExport) ,
                        new RbSpace(),
                        new RbText(DssRef.lang.Hud_ToFaction)},
                       new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                            player, faction, city, CopyPasteOption.CityToFaction, group, RbSoundType.Copy), new RbTooltip_Text(DssRef.lang.Hud_FactionWide)));

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsHudIconImport) ,
                        new RbSpace(),
                        new RbText(DssRef.lang.Hud_FromFaction)},
                        new RbAction5Arg<Players.LocalPlayer, Faction, City, CopyPasteOption, ResourceGroupType>(DssRef.world.copyStockPile,
                            player, faction, city, CopyPasteOption.FactionToCity, group, RbSoundType.Copy), new RbTooltip_Text(DssRef.lang.Hud_FactionWide)));
                }
            }

        }

        void stockpile(LocalPlayer player, ItemResourceType item)
        {
            //GroupedResource res;
            IconName.Item(item, out SpriteName itemIcon, out string itemName);

            content.newLine();

            GroupedResource groupedResource;
            BoolGetSet_Tag useLimitProperty;
            if (city != null)
            {
                groupedResource = city.GetGroupedResource(item);

                useLimitProperty = (object tag, bool set, bool value) =>
                {
                    var res = city.GetGroupedResource(item);
                    if (set)
                    {
                        res.useStockLimit = !res.useStockLimit;
                        
                        city.SetGroupedResource(item, res);
                    }
                    return res.useStockLimit;
                };
            }
            else
            {
                groupedResource = faction.GetRefResourceOverview(item);

                useLimitProperty = (object tag, bool set, bool value) =>
                {
                    ref var res = ref faction.GetRefResourceOverview(item);
                    if (set)
                    {
                        res.useStockLimit = !res.useStockLimit;
                        //todo set all cities
                    }

                    return res.useStockLimit;
                };
            }

            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { 
                new RbImage(itemIcon),
                new RbSpace(0.5f),
                new RbImage(SpriteName.WarsStockpileLimit) },
                useLimitProperty, new RbTooltip((RichBoxContent content, object tag)=> {
                    content.h1(DssRef.lang.StockPile_LimitTitle, HudLib.TitleColor_Head);
                    content.text(DssRef.lang.Resource_StockPile_Info);
                    content.text(DssRef.lang.StockPile_ItemsAreNotLost, HudLib.InfoYellow_Light);

                    content.newParagraph();
                    content.Add(new RbSeperationLine());
                    ResourceLib.FullResourceInfo(faction, city, item, content);
                })));

            if (groupedResource.useStockLimit)
            {
                stockPileEdit(content, item, groupedResource);
            }
            else
            {
                List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(2);
                IconName.Storage(ItemPropertyColl.Get(item).storageType, out SpriteName storageIcon, out string storageName);
                buttonContent.Add(new RbImage(storageIcon));
                buttonContent.Add(new RbSpace());
                if (city == null)
                {
                    buttonContent.Add(new RbText(DssRef.lang.Hud_Maximum));
                }
                else
                {
                    buttonContent.Add(new RbText(groupedResource.capacity.ToString()));
                }
                content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent, null, new RbTooltip_Text(storageName)));
            }
                


            if (groupedResource.hasCesspit)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.KeyDelete)},
                    new RbAction2Arg<LocalPlayer, ItemResourceType>(city.itemCesspitClick, player, item),
                    new RbTooltip_Text(DssRef.lang.BuildingType_Cesspit))
                { AddXRadius = -2 });
                
            }
            
        }

        struct LimitTooltipArgs
        {
            IntGetSetTag property;
            public StockpileLimitOption limit;
            public GroupedResource res;
        }

        //void limitTooltip(RichBoxContent content, object tag)
        //{
        //    LimitTooltipArgs args = (LimitTooltipArgs)tag;
        //    if (args.limit == StockpileLimitOption.NoLimit)
        //    {
        //        content.h1(".No limit", HudLib.TitleColor_Head);
        //        content.text(".Will stockpile up to the storage size");
        //    }
        //    else
        //    {
        //        content.h1(string.Format( ".Limit stockpile to {0}", ResourceLib.Limit(args.limit)), HudLib.TitleColor_Head);
        //        content.text(DssRef.lang.Resource_StockPile_Info);
        //    }
        //}

        void stockPileEdit(RichBoxContent content, ItemResourceType item, GroupedResource res)
        {
            IntGetSetTag property;
            int max;
            if (city != null)
            {
                max = city.GetGroupedResource(item).capacity;
                property = (object tag, bool set, int value) =>
                {

                    var res = city.GetGroupedResource(item);
                    if (set)
                    {
                        res.stockPileLimit = value;
                        city.SetGroupedResource(item, res);
                    }
                    return res.stockPileLimit;
                };
            }
            else
            {
                max = DssConst.StockPileMaxBound;
                property = (object tag, bool set, int value) =>
                {
                    ref var res = ref faction.GetRefResourceOverview(item);
                    if (set)
                    {
                        res.stockPileLimit = value;
                        //todo set all cities
                    }

                    return res.stockPileLimit;
                };
            }

            RbDragButton.RbDragButtonGroup(content, StockPileControls, new DragButtonSettings(DssConst.StockPileMinBound, max, 100),
                property, true);
        }
    }
}
