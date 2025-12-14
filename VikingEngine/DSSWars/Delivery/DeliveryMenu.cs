using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Interface.Component;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.ToGG;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.DSSWars.Delivery
{
    class DeliveryMenu
    {
        static readonly float[] BoundControls = { 10, 100, 1000 };
        static readonly float[] BoundControls_Gold = { 100, 1000, 10000 };

        City city;
        LocalPlayer player;
        ProgressQue que = new ProgressQue();

        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            this.city = city;
            this.player = player;


            if (arraylib.InBound(city.deliveryServices, city.selectedDelivery))
            {
                DeliveryStatus currentStatus = get();
                content.Add(new RbBeginTitle(1));


                SpriteName icon;
                string caption;
                switch (currentStatus.GetFilterType())
                {
                    case ItemResourceType.RESOURCES:
                        icon = SpriteName.WarsBuild_Postal;
                        caption = DssRef.lang.BuildingType_Postal;
                        break;
                    case ItemResourceType.Men:
                        icon = SpriteName.WarsBuild_Recruitment;
                        caption = DssRef.lang.BuildingType_Recruitment;
                        break;
                    default:
                        icon = SpriteName.WarsBuild_GoldDeliver;
                        caption = DssRef.lang.BuildingType_GoldDelivery;
                        break;

                }
                //string typeName = currentStatus.IsRecruitment() ? DssRef.lang.BuildingType_Recruitment : DssRef.lang.BuildingType_Postal;
                //var title = new RbText(typeName + " " + currentStatus.idAndPosition.ToString());
                //title.overrideColor = HudLib.TitleColor_TypeName;
                //content.Add(title);
                //content.space();
                //HudLib.CloseButton(content, new RbAction(() => { city.selectedDelivery = -1; }, RbSoundType.Back));
                HudLib.buildingMenuTitle(content, icon, caption, currentStatus.idAndPosition,
                    city.selectedDelivery, city.deliveryServices.Count,
                    () => { city.selectedDelivery = -1; },
                    (int next) => {
                        city.selectedDelivery = Bound.SetRollover(city.selectedDelivery + next, 0, city.deliveryServices.Count - 1);
                    });

                content.newParagraph();

                if (currentStatus.IsPostal())
                {
                    HudLib.Label(content, DssRef.lang.Resource);
                    content.space();
                    HudLib.InfoButton(content, new RbTooltip_Text(DssRef.lang.BuildingType_Postal_Description));
                    //{
                    //    RichBoxContent content = new RichBoxContent();
                    //    HudLib.Description(content, DssRef.lang.BuildingType_Postal_Description);
                    //    //HudLib.Description(content, string.Format(DssRef.lang.Deliver_WillSendXInfo, DssConst.CityDeliveryChunkSize_Level1));
                    //    player.hud.tooltip.create(player, content, true);
                    //}));
                    content.newLine();

                    if (currentStatus.profile.type == ItemResourceType.AutomatedItem)
                    {
                        content.Add(new RbImage(SpriteName.AutomationGearIcon));
                        content.space();
                        content.Add(new RbText(DssRef.lang.Automation_Title));
                    }
                    else if (currentStatus.profile.type != ItemResourceType.NONE)
                    {
                        bool reachedBuffer = false;
                        city.GetGroupedResource(currentStatus.profile.type).toMenu(content, currentStatus.profile.type, false, ref reachedBuffer);
                    }
                    content.newLine();
                    //for (ResourcesSubTab resourcesSubTab = ResourcesSubTab.Overview_Resources; resourcesSubTab <= ResourcesSubTab.Overview_Armor; ++resourcesSubTab)
                    for (ResourceGroup resourceGroup = 0; resourceGroup < ResourceGroup.NUM; resourceGroup++)
                    {
                        var tabContent = new RichBoxContent();
                        //string text = null;
                        switch (resourceGroup)
                        {
                            case ResourceGroup.Resources:
                                tabContent.Add(new RbText(DssRef.lang.Hud_category));
                                tabContent.space();
                                tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                                break;

                            case ResourceGroup.Metals:
                                tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                                break;
                            case ResourceGroup.Weapons:
                                tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                                break;

                            case ResourceGroup.Projectile:
                                tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                                break;

                            case ResourceGroup.Armor:
                                tabContent.Add(new RbImage(SpriteName.cmdMailArmor));
                                break;
                        }
                        var subTab = new ArtButton(player.resourcesSubTab.resourceGroup == resourceGroup? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, 
                            tabContent,
                            new RbAction1Arg<ResourceGroup>((ResourceGroup resourcesSubTab) =>
                            {
                                player.resourcesSubTab.resourceGroup = resourcesSubTab;
                            }, resourceGroup, RbSoundType.Tab));
                        
                        //subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
                        content.Add(subTab);
                        //content.space();
                    }
                    //AUTO RESOURCE
                    {
                        content.space();
                        var tabContent = new RichBoxContent();
                        tabContent.Add(new RbImage(SpriteName.AutomationGearIcon));

                        var subTab = new ArtToggle(currentStatus.profile.type == ItemResourceType.AutomatedItem, tabContent,
                            new RbAction1Arg<ResourceGroup>((ResourceGroup resourcesSubTab) =>
                            {
                                player.resourcesSubTab.resourceGroup = resourcesSubTab;
                                itemClick(ItemResourceType.AutomatedItem);

                            }, ResourceGroup.Auto, RbSoundType.Tab),
                            new RbTooltip((RichBoxContent content, object tag) =>
                            {
                                //RichBoxContent content = new RichBoxContent();

                                content.h2(DssRef.lang.Automation_Title);
                                content.text(DssRef.lang.Delivery_AutoResourceDescription).overrideColor = HudLib.InfoYellow_Light;

                                //player.hud.tooltip.create(player, content, true);
                            }));
                        //subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == ResourcesSubTab.Auto);
                        content.Add(subTab);
                        content.space();
                    }

                    if (player.resourcesSubTab.resourceGroup !=  ResourceGroup.Auto)
                    {

                        content.Add(new RichBoxScale(1.6f));
                        content.newLine();
                        ItemResourceType[] resourceTypes;

                        switch (player.resourcesSubTab.resourceGroup)
                        {
                            default: resourceTypes = City.MovableCityResource_Misc; break;
                            case ResourceGroup.Metals: resourceTypes = City.MovableCityResource_Metals; break;
                            case ResourceGroup.Animals: resourceTypes = City.MovableCityResource_Animals; break;
                            case ResourceGroup.Weapons: resourceTypes = City.MovableCityResource_WeaponMelee; break;
                            case ResourceGroup.Projectile: resourceTypes = City.MovableCityResource_WeaponRanged; break;
                            case ResourceGroup.Armor: resourceTypes = City.MovableCityResource_Armor; break;
                        }

                        foreach (var item in resourceTypes)
                        {
                            IconName.Item(item, out SpriteName itemIcon, out string itemName);

                            var button = new ArtToggle(item == currentStatus.profile.type, new List<AbsRichBoxMember>{
                                new RbImage(itemIcon)   
                            },
                            new RbAction1Arg<ItemResourceType>(itemClick, item, RbSoundType.Option),
                            new RbTooltip((RichBoxContent content, object tag) =>
                                {
                                    //RichBoxContent content = new RichBoxContent();

                                    content.h2(DssRef.lang.Hud_ThisCity).overrideColor = HudLib.TitleColor_Label;
                                    bool reachedBuffer = false;
                                    bool safeGuard = city.foodSafeGuardIsActive(item);

                                    city.GetGroupedResource(item).toMenu(content, item, safeGuard, ref reachedBuffer);

                                    if (currentStatus.profile.toCity >= 0 && currentStatus.profile.toCity != DeliveryProfile.ToCityAuto)
                                    {
                                        content.newParagraph();
                                        content.h2(DssRef.lang.Hud_RecieveingCity).overrideColor = HudLib.TitleColor_Label;
                                        DssRef.world.cities[currentStatus.profile.toCity].GetGroupedResource(item).toMenu(content, item, safeGuard, ref reachedBuffer);

                                    }

                                    //player.hud.tooltip.create(player, content, true);
                                }));
                            //button.setGroupSelectionColor(HudLib.RbSettings, item == currentStatus.profile.type);
                            content.Add(button);
                            //content.space();
                        }
                        content.Add(new RichBoxScale());
                        content.newParagraph();
                    }
                   
                }
                HudLib.Label(content, DssRef.lang.Hud_RecieveingCity);
                content.newLine();
                //var cities_c = city.GetFaction().cities.counter();
                //while (cities_c.Next())
                //{
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref city.GetFaction().cities, DssRef.world.cities, out City citySel))
                {
                    if (citySel != city && city.tilePos.SideLength(citySel.tilePos) <= DssConst.DeliveryMaxDistance)
                    {
                        var buttonContent = new RichBoxContent();
                        citySel.tagToHud(buttonContent);
                        if (buttonContent.Count > 0)
                        {
                            buttonContent.space();
                        }
                        buttonContent.Add(new RbText(citySel.TypeName()));

                        var button = new ArtToggle(citySel.myIndex == currentStatus.profile.toCity, buttonContent, 
                            new RbAction1Arg<int>(cityClick, citySel.myIndex, RbSoundType.Option), 
                            new RbTooltip((RichBoxContent content, object tag /*City toCity*/) =>
                            {
                                City toCity = (City)tag;
                                //RichBoxContent content = new RichBoxContent();
                                content.h2(toCity.Name(out _)).overrideColor = HudLib.TitleColor_Label;
                                TimeLength time = DeliveryProfile.DeliveryTime(city, toCity, currentStatus.level, out float distance);
                                content.text(string.Format(DssRef.lang.Delivery_DistanceX, TextLib.OneDecimal(distance)));
                                content.text(string.Format(DssRef.lang.Delivery_DeliveryTimeX, time.LongString()));

                                if (currentStatus.profile.type != ItemResourceType.NONE &&
                                    currentStatus.profile.type != ItemResourceType.AutomatedItem)
                                {
                                    content.newParagraph();
                                    content.h2(DssRef.lang.Hud_ThisCity).overrideColor = HudLib.TitleColor_Label;
                                    bool reachedBuffer = false;
                                    bool safeGuard = city.foodSafeGuardIsActive(currentStatus.profile.type);
                                    city.GetGroupedResource(currentStatus.profile.type).toMenu(content, currentStatus.profile.type, safeGuard, ref reachedBuffer);

                                    //if (currentStatus.profile.toCity >= 0)
                                    //{
                                    content.newParagraph();
                                    content.h2(DssRef.lang.Hud_RecieveingCity).overrideColor = HudLib.TitleColor_Label;
                                    //if (currentStatus.profile.toCity == DeliveryProfile.ToCityAuto)
                                    //{

                                    //}
                                    //else
                                    //{
                                    toCity.GetGroupedResource(currentStatus.profile.type).toMenu(content, currentStatus.profile.type, false, ref reachedBuffer);
                                        //}
                                    //}
                                }
                                //player.hud.tooltip.create(player, content, true);
                            }, citySel));
                        //button.setGroupSelectionColor(HudLib.RbSettings, citySel.parentArrayIndex == currentStatus.profile.toCity);
                        content.Add(button);
                        //content.space();
                    }
                }

                //AUTO CITY
                {
                    var button = new ArtToggle(DeliveryProfile.ToCityAuto == currentStatus.profile.toCity, 
                        new List<AbsRichBoxMember>{
                            new RbImage(SpriteName.AutomationGearIcon)
                            }, 
                            new RbAction1Arg<int>(cityClick, DeliveryProfile.ToCityAuto, RbSoundType.Option), 
                            new RbTooltip((RichBoxContent content, object tag) =>
                            {
                                //RichBoxContent content = new RichBoxContent();
                                content.h2(DssRef.lang.Automation_Title).overrideColor = HudLib.TitleColor_Name;
                                content.text(DssRef.lang.Delivery_AutoReciever_Description).overrideColor = HudLib.InfoYellow_Light;
                                //player.hud.tooltip.create(player, content, true);
                            }));
                    //button.setGroupSelectionColor(HudLib.RbSettings, DeliveryProfile.ToCityAuto == currentStatus.profile.toCity);
                    content.Add(button);
                }

                content.newParagraph();

                if (currentStatus.profile.toCity >= 0)
                {
                    //SEND CHUNK SIZE
                    HudLib.Label(content, DssRef.lang.Delivery_SendChunk);
                    content.newLine();

                    List<int> sendChunkOptions = new List<int>(4);
                    if (currentStatus.IsGold())
                    {
                        sendChunkOptions.Add(DssConst.GoldDeliveryChunkSize_Mini);
                        sendChunkOptions.Add(DssConst.GoldDeliveryChunkSize_Level1);

                        if (currentStatus.level >= 2)
                        {
                            sendChunkOptions.Add(DssConst.GoldDeliveryChunkSize_Level2);
                        }
                        if (currentStatus.level >= 3)
                        {
                            sendChunkOptions.Add(DssConst.GoldDeliveryChunkSize_Level3);
                        }
                    }
                    else
                    {
                        sendChunkOptions.Add(DssConst.CityDeliveryChunkSize_Mini);
                        sendChunkOptions.Add(DssConst.CityDeliveryChunkSize_Level1);

                        if (currentStatus.level >= 2)
                        {
                            sendChunkOptions.Add(DssConst.CityDeliveryChunkSize_Level2);
                        }
                        if (currentStatus.level >= 3)
                        {
                            sendChunkOptions.Add(DssConst.CityDeliveryChunkSize_Level3);
                        }
                    }

                    foreach (int amount in sendChunkOptions)
                    {
                        var button = new ArtToggle(amount == currentStatus.profile.SendAmount, new List<AbsRichBoxMember> { new RbText(amount.ToString()) },
                            new RbAction(() =>
                            {
                                DeliveryStatus currentStatus = get();
                                currentStatus.profile.SendAmount = amount;
                                set(currentStatus);
                            }, RbSoundType.Option));

                        //button.setGroupSelectionColor(HudLib.RbSettings, amount == currentStatus.profile.SendAmount);

                        content.Add(button);
                        //content.space();
                    }

                    if (currentStatus.profile.type != ItemResourceType.AutomatedItem)
                    {
                        content.newParagraph();

                        var minLabel = new RbText(DssRef.lang.Delivery_SenderMinimumCap + ":");
                        minLabel.overrideColor = HudLib.TitleColor_Label_Dark;
                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { minLabel },
                            UseSenderMinProperty));
                        boundsToHud(content, currentStatus, true);
                    }
                    content.newParagraph();

                    var maxLabel = new RbText(DssRef.lang.Delivery_RecieverMaximumCap + ":");
                    maxLabel.overrideColor = HudLib.TitleColor_Label_Dark;
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { maxLabel },
                        UseRecieverMaxProperty));
                    boundsToHud(content, currentStatus, false);
                }
                else
                {
                    content.Add(new RbSeperationLine());
                }

                if (currentStatus.profile.toCity >= 0)
                {
                    content.newParagraph();
                    que.labelToHud(content);
                    progress(currentStatus);
                    que.buttonsToHud(player, content, queClick, currentStatus.que, Conscript.BarracksStatus.MaxQue, true);
                }

                content.newParagraph();

                HudLib.copyPaste(content, player,
                    new RbAction1Arg<LocalPlayer>(city.copyDelivery, player, RbSoundType.Copy),
                    new RbAction1Arg<LocalPlayer>(city.pasteDelivery, player, RbSoundType.Paste),
                    currentStatus.fullSetup(), city.getDeliveryCopyRef(player, currentStatus.profile.type).fullSetup());
                
            }
            else
            {
               
                if (city.deliveryServices.Count == 0)
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light);
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement, HudLib.TitleColor_Label);
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_Postal));
                    content.space();
                    content.Add(new RbText(DssRef.lang.BuildingType_Postal));
                    content.newLine();
                    content.text(DssRef.lang.Hud_RequirementOr);
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_Recruitment));
                    content.space();
                    content.Add(new RbText(DssRef.lang.BuildingType_Recruitment));
                }
                else
                {
                    bool hasRecruit = false;
                    bool hasGoldDeliver = false;
                    bool hasPostal = false;

                    for (int i = 0; i < city.deliveryServices.Count; ++i)
                    {
                        DeliveryStatus currentProfile = city.deliveryServices[i];

                        if (currentProfile.IsRecruitment())
                        {
                            hasRecruit = true;
                        }
                        else if (currentProfile.IsGold())
                        {
                            hasGoldDeliver = true;
                        }
                        else
                        {
                            hasPostal = true;
                        }
                    }

                    int typeCount = 0;
                    if (hasRecruit) { typeCount++; }
                    if (hasGoldDeliver) { typeCount++; }
                    if (hasPostal) { typeCount++; }


                    //Apply to all options
                    content.h2(DssRef.lang.GeneralSetting_SetAll, HudLib.TitleColor_Action);
                    HudLib.Label(content, DssRef.lang.Hud_ProductionQueue); content.space();
                    que.listToHud(player, content, queueToAll, true);

                    if (player.deliverySupTab != ItemResourceType.NUM || typeCount == 1)
                    {
                        content.newLine();
                        player.gameControls.input.Paste.ToRichContent(content);
                        content.hspace();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.WarsHudIconPaste),
                            new RbSpace(),
                            new RbText(DssRef.lang.Hud_Paste) 
                        },
                            new RbAction1Arg<LocalPlayer>(city.pasteDeliveryToAll, player, RbSoundType.Paste)));
                    }
                    content.Add(new RbSeperationLine());
                    content.h2(DssRef.lang.Delivery_ListTitle, HudLib.TitleColor_Action);

                    if (typeCount > 1)
                    {
                        content.newLine();
                        SubTab(ItemResourceType.NUM);

                        if (hasRecruit) { SubTab(ItemResourceType.Men); }
                        if (hasGoldDeliver) { SubTab(ItemResourceType.Gold); }
                        if (hasPostal) { SubTab(ItemResourceType.RESOURCES); }

                        void SubTab(ItemResourceType filter)
                        {
                            List<AbsRichBoxMember> tabContent = new List<AbsRichBoxMember>(1);
                            switch (filter)
                            {
                                case ItemResourceType.NUM:
                                    tabContent.Add(new RbText(DssRef.lang.Hud_All));
                                    break;
                                case ItemResourceType.Men:
                                    tabContent.Add(new RbText(DssRef.lang.BuildingType_Recruitment));
                                    break;
                                case ItemResourceType.Gold:
                                    tabContent.Add(new RbText(DssRef.lang.BuildingType_GoldDelivery));
                                    break;
                                default:
                                    tabContent.Add(new RbText(DssRef.lang.BuildingType_Postal));
                                    break;
                            }

                            var subTab = new ArtButton(player.deliverySupTab == filter ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                                tabContent,
                               new RbAction1Arg<ItemResourceType>((ItemResourceType filter) =>
                               {
                                   player.deliverySupTab = filter;
                               }, filter, RbSoundType.Tab));
                            content.Add(subTab);
                        }
                    }
                    else
                    {
                        player.deliverySupTab = ItemResourceType.NUM;
                    }

                    for (int i = 0; i < city.deliveryServices.Count; ++i)
                    {
                        content.newLine();

                        DeliveryStatus currentProfile = city.deliveryServices[i];
                        bool fitFilter = player.deliverySupTab == ItemResourceType.NUM;

                        string title;
                        SpriteName icon;
                        if (currentProfile.IsRecruitment())
                        {
                            fitFilter |= player.deliverySupTab == ItemResourceType.Men;
                            icon = SpriteName.WarsWorker;
                            title = DssRef.lang.BuildingType_Recruitment;
                        }
                        else if (currentProfile.IsGold())
                        {
                            fitFilter |= player.deliverySupTab == ItemResourceType.Gold;
                            icon = SpriteName.WarsResource_Gold;
                            title = DssRef.lang.BuildingType_GoldDelivery;
                        }
                        else
                        {
                            fitFilter |= player.deliverySupTab == ItemResourceType.RESOURCES;
                            IconName.Item(currentProfile.profile.type, out icon, out string itemName);
                            //icon = ResourceLib.Icon(currentProfile.profile.type);
                            title = DssRef.lang.BuildingType_Postal + ": " + currentProfile.profile.type.ToString();
                        }
                        if (fitFilter)
                        {

                            var caption = new RbText(title);
                            caption.overrideColor = HudLib.TitleColor_Label_Dark;

                            var buttonContent = new List<AbsRichBoxMember>(){
                            new RbBeginTitle(2),
                            caption,
                            new RbNewLine(),
                            new RbText(currentProfile.shortActiveString(),  HudLib.InfoYellow_Dark)
                        };

                            if (icon != SpriteName.NO_IMAGE)
                            {
                                buttonContent.Insert(1, new RbImage(icon));
                            }

                            content.Add(new ArtButton(RbButtonStyle.Primary, buttonContent,
                                new RbAction1Arg<int>(selectClick, i, RbSoundType.Default)));
                        }
                    }
                    
                }
            }

            void queueToAll(int count)
            {
                for (int i = 0; i < city.deliveryServices.Count; ++i)
                {
                    if (player.deliverySupTab == ItemResourceType.NUM ||
                        player.deliverySupTab == city.deliveryServices[i].GetFilterType())
                    {
                        var status = city.deliveryServices[i];
                        if (count == 1)
                        {
                            status.que++;
                        }
                        else
                        {
                            status.que = count;
                        }
                        city.deliveryServices[i] = status;
                    }
                }
            }

            void progress(DeliveryStatus currentStatus)
            {
                bool isSending = currentStatus.active == DeliveryActiveStatus.Delivering;

                if (isSending || currentStatus.que > 0)
                {

                    //content.newParagraph();
                    content.Add(new RbSeperationLine());
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        var text = new RbText(DssRef.lang.Delivery_ItemsReady);
                        bool ready = isSending || currentStatus.CanSend(city, out currentStatus.inProgress.type);
                        text.overrideColor = ready ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);

                        if (ready)
                        {
                            IconName.Item(currentStatus.inProgress.type, out SpriteName itemIcon, out string itemName);
                            content.newLine();
                            content.Add(new RbImage(itemIcon));
                            content.space();
                            content.Add(new RbText(itemName + ": " + currentStatus.inProgress.SendAmount.ToString()));
                        }
                    }
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        var text = new RbText(DssRef.lang.Delivery_RecieverReady);
                        text.overrideColor = isSending || currentStatus.CanRecieve(currentStatus.inProgress.type) ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);

                        if (isSending && currentStatus.inProgress.toCity == DeliveryProfile.ToCityAuto)
                        {
                            content.Add(new RbText(" - " + DssRef.world.cities[currentStatus.inProgress.autoCity].TypeName()));
                        }
                    }

                    string timeString = currentStatus.longTimeProgress(city, out bool hasTime);
                    if (hasTime)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(timeString, isSending ? null : HudLib.SecondaryTextColor));
                    }
                }
            }
        }

        void selectClick(int index)
        {
            city.selectedDelivery = index;
        }

        void boundsToHud(RichBoxContent content, DeliveryStatus currentStatus, bool minCap)
        {
            content.newLine();
            int current;

            if (minCap)
            {
                current = currentStatus.senderMin;
            }
            else
            {
                current = currentStatus.recieverMax;
            }

            List<float> bounds = new List<float>( currentStatus.IsGold() ? BoundControls_Gold : BoundControls);

            RbDragButton.RbDragButtonGroup(content, bounds, new DragButtonSettings(0, 10000, bounds[0]),
               minCap ? MinProperty : MaxProperty , true);

            //content.newLine();
            //for (int i = bounds.Length - 1; i >= 0; i--)
            //{
            //    int change = -bounds[i];
            //    content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(change)) },
            //        new RbAction2Arg<int, bool>(changeResourcePrice, change, minCap)));

            //    content.space();
            //}

            //content.Add(new RbText(current.ToString()));
            //content.space();

            //for (int i = 0; i < bounds.Length; i++)
            //{
            //    int change = bounds[i];
            //    content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(change)) },
            //        new RbAction2Arg<int, bool>(changeResourcePrice, change, minCap)));

            //    content.space();
            //}
        }

        bool UseSenderMinProperty(object tag, bool _set, bool value)
        {
            DeliveryStatus currentStatus = get();
            if (_set)
            {
                currentStatus.useSenderMin = value;
                set(currentStatus);
            }
            return currentStatus.useSenderMin;
        }

        bool UseRecieverMaxProperty(object tag, bool _set, bool value)
        {
            DeliveryStatus currentStatus = get();
            if (_set)
            {
                currentStatus.useRecieverMax = value;
                set(currentStatus);
            }
            return currentStatus.useRecieverMax;
        }

        void changeResourcePrice(int change, bool minCap)
        {
            DeliveryStatus currentStatus = get();

            if (minCap) currentStatus.senderMin = Bound.Set(currentStatus.senderMin + change, 0, 10000);
            else currentStatus.recieverMax = Bound.Set(currentStatus.recieverMax + change, 0, 10000);

            set(currentStatus);
        }

        int MaxProperty(bool _set, int value)
        {
            var currentStatus = get();
            if (_set)
            {
                currentStatus.recieverMax = value;
                set(currentStatus);
            }
            return currentStatus.recieverMax;
        }
        int MinProperty(bool _set, int value)
        {
            var currentStatus = get();
            if (_set)
            {
                currentStatus.senderMin = value;
                set(currentStatus);
            }
            return currentStatus.senderMin;
        }
        //void changeResourcePrice(int change, bool minCap)
        //{
        //    DeliveryStatus currentStatus = get();

        //    if (minCap) currentStatus.senderMin = Bound.Set(currentStatus.senderMin + change, 0, 10000);
        //    else currentStatus.recieverMax = Bound.Set(currentStatus.recieverMax + change, 0, 10000);

        //    set(currentStatus);
        //}

        void itemClick(ItemResourceType item)
        {
            DeliveryStatus currentStatus = get();

            currentStatus.profile.type = item;

            set(currentStatus);
        }

        void cityClick(int index)
        {
            DeliveryStatus currentStatus = get();

            currentStatus.profile.toCity = index;

            set(currentStatus);
        }

        DeliveryStatus get()
        {
            return city.deliveryServices[city.selectedDelivery];
        }

        void set(DeliveryStatus profile)
        {
            city.deliveryServices[city.selectedDelivery] = profile;
        }

        void queClick(int length)
        {
            DeliveryStatus currentStatus = get();
            currentStatus.que = length;
            set(currentStatus);
        }
    }
}
