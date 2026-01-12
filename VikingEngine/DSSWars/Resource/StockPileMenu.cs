using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ.Joust;

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

        public void toHud(LocalPlayer player, ResourcesSubTab tab)
        {
            ResourceGroupType groupType = ResourceGroupType.NUM;

            switch (tab)
            {
                case ResourcesSubTab.Stockpile_Resources:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    groupType = ResourceGroupType.Resources;
                    stockpile(ItemResourceType.Wood_Group);
                    stockpile(ItemResourceType.Stone_G);
                    stockpile(ItemResourceType.RawFood_Group);
                    stockpile(ItemResourceType.SkinLinen_Group);
                    content.newParagraph();

                    stockpile(ItemResourceType.Food_G);
                    stockpile(ItemResourceType.Fuel_G);
                    stockpile(ItemResourceType.Beer);
                    stockpile(ItemResourceType.CoolingFluid);
                    content.newParagraph();

                    stockpile(ItemResourceType.Palisade);
                    stockpile(ItemResourceType.Toolkit);
                    stockpile(ItemResourceType.Wagon2Wheel);
                    stockpile(ItemResourceType.Wagon4Wheel);
                    stockpile(ItemResourceType.BlackPowder);
                    stockpile(ItemResourceType.GunPowder);
                    stockpile(ItemResourceType.LedBullet);

                    //content.newParagraph();
                    //HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
                    //GroupedResource.BufferIconInfo(content, false);
                    break;

                case ResourcesSubTab.Stockpile_Metals:
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
                case ResourcesSubTab.Stockpile_Weapons:
                    //content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
                    groupType = ResourceGroupType.Weapons;
                    stockpile(ItemResourceType.SharpStick);
                    stockpile(ItemResourceType.BronzeSword);
                    stockpile(ItemResourceType.ShortSword);
                    stockpile(ItemResourceType.Sword);
                    stockpile(ItemResourceType.LongSword);
                    stockpile(ItemResourceType.HandSpear);
                    content.newParagraph();

                    stockpile(ItemResourceType.Warhammer);
                    stockpile(ItemResourceType.TwoHandSword);
                    stockpile(ItemResourceType.KnightsLance);
                    stockpile(ItemResourceType.MithrilSword);

                    break;

                case ResourcesSubTab.Stockpile_Projectile:
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

                case ResourcesSubTab.Stockpile_Armor:

                    groupType = ResourceGroupType.Armor;
                    stockpile(ItemResourceType.HeavyPaddedArmor);
                    stockpile(ItemResourceType.PaddedArmor);
                    stockpile(ItemResourceType.BronzeArmor);

                    content.newParagraph();

                    stockpile(ItemResourceType.IronArmor);
                    stockpile(ItemResourceType.HeavyIronArmor);
                    stockpile(ItemResourceType.LightPlateArmor);
                    stockpile(ItemResourceType.FullPlateArmor);
                    stockpile(ItemResourceType.MithrilArmor);
                    break;
            }

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.Hud_CurrentPage); content.space();
            copyPasteOptions(groupType);

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.Hud_AllPages); content.space();
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

        void stockpile(ItemResourceType item)
        {
            GroupedResource res;

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
                        new RbImage(ResourceLib.Icon(item))}, null,
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        if (city != null)
                        {
                            ResourceLib.FullResourceInfo(city, item, content);
                            //bool buffer = false;
                            //city.GetGroupedResource(item).toMenu(content, item, false, ref buffer);
                        }
                        else
                        {
                            content.Add(new RbImage(ResourceLib.Icon(item)));
                            content.space();
                            content.Add(new RbText(LangLib.Item(item)));
                        }
                    }
                    )));

            content.space();

            stockPileEdit(content, item, res);
        }

        void stockPileEdit(RichBoxContent content, ItemResourceType item, GroupedResource res)
        {
            IntGetSet property;

            if (city != null)
            {
                property = (bool set, int value) =>
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
                property = (bool set, int value) =>
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

            RbDragButton.RbDragButtonGroup(content, StockPileControls, new DragButtonSettings(DssConst.StockPileMinBound, DssConst.StockPileMaxBound, 100),
                property, true);
        }
    }
}
