using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.ToGG;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Delivery
{
    class DeliveryMenu
    {
        static readonly int[] BoundControls = { 10, 100, 1000 };
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
                content.Add(new RichBoxBeginTitle(1));

                string typeName = currentStatus.Recruitment() ? DssRef.lang.BuildingType_Recruitment : DssRef.lang.BuildingType_Postal;
                var title = new RichBoxText(typeName + " " + currentStatus.idAndPosition.ToString());
                title.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(title);
                content.space();
                HudLib.CloseButton(content, new RbAction(() => { city.selectedDelivery = -1; }, SoundLib.menuBack));
                


                content.newParagraph();

                if (!currentStatus.Recruitment())
                {
                    HudLib.Label(content, DssRef.lang.Resource);
                    content.space();
                    HudLib.InfoButton(content, new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();
                        HudLib.Description(content, DssRef.lang.BuildingType_Postal_Description);
                        HudLib.Description(content, string.Format(DssRef.lang.Deliver_WillSendXInfo, DssConst.CityDeliveryCount));
                        player.hud.tooltip.create(player, content, true);
                    }));
                    content.newParagraph();
                    content.Add(new RichBoxScale(1.6f));
                    foreach (var item in City.MovableCityResourceTypes)
                    {
                        var button = new RichboxButton(new List<AbsRichBoxMember>{
                                new RichBoxImage(ResourceLib.Icon(item))   
                            //new RichBoxText(LangLib.Item(item))
                            },

                            new RbAction1Arg<ItemResourceType>(itemClick, item, SoundLib.menu),

                            new RbAction(() =>
                               {
                                   RichBoxContent content = new RichBoxContent();

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

                                   //content.text(LangLib.Item(item)).overrideColor = HudLib.TitleColor_TypeName;

                                   player.hud.tooltip.create(player, content, true);
                               }));
                        button.setGroupSelectionColor(HudLib.RbSettings, item == currentStatus.profile.type);
                        content.Add(button);
                        content.space();
                    }

                }
                
                content.Add(new RichBoxScale());
                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Hud_RecieveingCity);
                content.newLine();
                var cities_c = city.faction.cities.counter();
                while (cities_c.Next())
                {
                    if (cities_c.sel != city && city.tilePos.SideLength(cities_c.sel.tilePos) <= DssConst.DeliveryMaxDistance)
                    {
                        var buttonContent = new RichBoxContent();
                        cities_c.sel.tagToHud(buttonContent);
                        if (buttonContent.Count > 0)
                        {
                            buttonContent.space();
                        }
                        buttonContent.Add(new RichBoxText(cities_c.sel.TypeName()));

                        var button = new RichboxButton(buttonContent, 
                            new RbAction1Arg<int>(cityClick, cities_c.sel.parentArrayIndex, SoundLib.menu), new RbAction1Arg<City>((toCity) =>
                            {
                                RichBoxContent content = new RichBoxContent();
                                content.h2(toCity.Name()).overrideColor = HudLib.TitleColor_Label;
                                var time = DeliveryProfile.DeliveryTime(city, toCity, out float distance);
                                content.text(string.Format(DssRef.lang.Delivery_DistanceX, TextLib.OneDecimal(distance)));
                                content.text(string.Format(DssRef.lang.Delivery_DeliveryTimeX, time.LongString()));

                                if (currentStatus.profile.type != ItemResourceType.NONE)
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
                                player.hud.tooltip.create(player, content, true);
                            }, cities_c.sel));
                        button.setGroupSelectionColor(HudLib.RbSettings, cities_c.sel.parentArrayIndex == currentStatus.profile.toCity);
                        content.Add(button);
                        content.space();
                    }
                }

                //AUTO
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                            new RichBoxImage(SpriteName.MenuPixelIconSettings)
                            }, new RbAction1Arg<int>(cityClick, DeliveryProfile.ToCityAuto, SoundLib.menu), new RbAction(() =>
                            {
                                RichBoxContent content = new RichBoxContent();
                                content.h2(DssRef.lang.Automation_Title).overrideColor = HudLib.TitleColor_Name;
                                content.text(DssRef.lang.Delivery_AutoReciever_Description).overrideColor = HudLib.InfoYellow_Light;
                                player.hud.tooltip.create(player, content, true);
                            }));
                    button.setGroupSelectionColor(HudLib.RbSettings, DeliveryProfile.ToCityAuto == currentStatus.profile.toCity);
                    content.Add(button);
                }

                content.newParagraph();

                var minLabel = new RichBoxText(DssRef.lang.Delivery_SenderMinimumCap + ":");
                minLabel.overrideColor = HudLib.TitleColor_Label_Dark;
                content.Add(new RichboxCheckbox(new List<AbsRichBoxMember> { minLabel },
                    UseSenderMinProperty));
                boundsToHud(content, currentStatus, true);

                content.newParagraph();

                var maxLabel = new RichBoxText(DssRef.lang.Delivery_RecieverMaximumCap + ":");
                maxLabel.overrideColor = HudLib.TitleColor_Label_Dark;
                content.Add(new RichboxCheckbox(new List<AbsRichBoxMember> { maxLabel },
                    UseRecieverMaxProperty));
                boundsToHud(content, currentStatus, false);

                if (currentStatus.profile.toCity >= 0)
                {
                    content.newParagraph();
                    que.toHud(player, content, queClick, currentStatus.que);
                }

                content.newParagraph();
                content.Add(new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxImage(player.input.Copy.Icon),
                    new RichBoxSpace(0.5f),
                    new RichBoxText(DssRef.lang.Hud_CopySetup) },
                new RbAction1Arg<LocalPlayer>(city.copyDelivery, player, SoundLib.menuCopy)));

                content.space();
                content.Add(new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxImage(player.input.Paste.Icon),
                    new RichBoxSpace(0.5f),
                    new RichBoxText(DssRef.lang.Hud_Paste)
                },
                new RbAction1Arg<LocalPlayer>(city.pasteDelivery, player, SoundLib.menuPaste)));

                bool isSending = currentStatus.active == DeliveryActiveStatus.Delivering;

                if (isSending || currentStatus.que > 0)
                {

                    //content.newParagraph();
                    content.Add(new RichBoxSeperationLine());
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        var text = new RichBoxText(DssRef.lang.Delivery_ItemsReady);
                        text.overrideColor = isSending || currentStatus.CanSend(city) ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        var text = new RichBoxText(DssRef.lang.Delivery_RecieverReady);
                        text.overrideColor = isSending || currentStatus.CanRecieve() ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);

                        if (isSending && currentStatus.inProgress.toCity == DeliveryProfile.ToCityAuto)
                        {
                            content.Add(new RichBoxText(" - " + DssRef.world.cities[currentStatus.inProgress.autoCity].TypeName()));
                        }
                    }

                    if (isSending)
                    {

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxText(currentStatus.longTimeProgress(city)));
                    }
                }
            }
            else
            {

                content.h2(DssRef.lang.Delivery_ListTitle).overrideColor = HudLib.TitleColor_Action;
                if (city.deliveryServices.Count == 0)
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.Add(new RichBoxImage(SpriteName.WarsBuild_Postal));
                    content.space();
                    content.Add(new RichBoxText(DssRef.lang.BuildingType_Postal));
                    content.newLine();
                    content.text(DssRef.lang.Hud_RequirementOr);
                    content.newLine();
                    content.Add(new RichBoxImage(SpriteName.WarsBuild_Recruitment));
                    content.space();
                    content.Add(new RichBoxText(DssRef.lang.BuildingType_Recruitment));
                }
                else
                {
                    for (int i = 0; i < city.deliveryServices.Count; ++i)
                    {
                        content.newLine();

                        DeliveryStatus currentProfile = city.deliveryServices[i];

                        string title;
                        if (currentProfile.Recruitment())
                        {
                            title = DssRef.lang.BuildingType_Recruitment;
                        }
                        else
                        {
                            title = DssRef.lang.BuildingType_Postal + ": " + currentProfile.profile.type.ToString();
                        }

                        var caption = new RichBoxText(
                                title
                            );
                        caption.overrideColor = HudLib.TitleColor_Name;

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>(){
                        new RichBoxBeginTitle(2),
                        caption,
                        new RichBoxNewLine(),
                        new RichBoxText(currentProfile.shortActiveString())
                    }, new RbAction1Arg<int>(selectClick, i, SoundLib.menu)));

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


            for (int i = BoundControls.Length - 1; i >= 0; i--)
            {
                int change = -BoundControls[i];
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                    new RbAction2Arg<int, bool>(changeResourcePrice, change, minCap)));

                content.space();
            }

            content.Add(new RichBoxText(current.ToString()));
            content.space();

            for (int i = 0; i < BoundControls.Length; i++)
            {
                int change = BoundControls[i];
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                    new RbAction2Arg<int, bool>(changeResourcePrice, change, minCap)));

                content.space();
            }
        }

        bool UseSenderMinProperty(int index, bool _set, bool value)
        {
            DeliveryStatus currentStatus = get();
            if (_set)
            {
                currentStatus.useSenderMin = value;
                set(currentStatus);
            }
            return currentStatus.useSenderMin;
        }

        bool UseRecieverMaxProperty(int index, bool _set, bool value)
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
