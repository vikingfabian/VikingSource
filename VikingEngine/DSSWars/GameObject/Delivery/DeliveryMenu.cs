using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Delivery
{
    class DeliveryMenu
    {
        static readonly int[] BoundControls = { 10, 100 };
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

                string typeName = currentStatus.Recruitment() ? DssRef.todoLang.BuildingType_Recruitment : DssRef.todoLang.BuildingType_Postal;
                content.Add(new RichBoxText(typeName + " " + currentStatus.idAndPosition.ToString()));
                content.space();
                content.Add(new RichboxButton(new List<AbsRichBoxMember>
                    { new RichBoxSpace(), new RichBoxText(DssRef.todoLang.Hud_EndSessionIcon),new RichBoxSpace(), },
                    new RbAction(() => { city.selectedDelivery = -1; })));

                content.newLine();
                HudLib.Description(content, string.Format("Will send {0} at a time", DssConst.CityDeliveryCount));

                content.newParagraph();

                if (!currentStatus.Recruitment())
                {
                    HudLib.Label(content, "Item");
                    content.newLine();
                    foreach (var item in City.MovableCityResourceTypes)
                    {
                        var button = new RichboxButton(new List<AbsRichBoxMember>{
                               new RichBoxText(LangLib.Item(item))
                            }, new RbAction1Arg<ItemResourceType>(itemClick, item));
                        button.setGroupSelectionColor(HudLib.RbSettings, item == currentStatus.profile.type);
                        content.Add(button);
                        content.space();
                    }
                }
                content.newParagraph();

                HudLib.Label(content, "To city");
                content.newLine();
                var cities_c = city.faction.cities.counter();
                while (cities_c.Next())
                {
                    if (cities_c.sel != city)
                    {
                        var button = new RichboxButton(new List<AbsRichBoxMember>{
                            new RichBoxText(cities_c.sel.TypeName())
                            }, new RbAction1Arg<int>(cityClick, cities_c.sel.parentArrayIndex));
                        button.setGroupSelectionColor(HudLib.RbSettings, cities_c.sel.parentArrayIndex == currentStatus.profile.toCity);
                        content.Add(button);
                        content.space();
                    }
                }

                content.newParagraph();

                HudLib.Label(content, "Sender minimum cap");
                boundsToHud(content, currentStatus, true);

                content.newParagraph();

                HudLib.Label(content, "Reciever maximum cap");
                boundsToHud(content, currentStatus, false);

                if (currentStatus.profile.toCity >= 0)
                {
                    content.newParagraph();
                    que.toHud(player, content, queClick, currentStatus.que);
                }

                if (currentStatus.active != DeliveryActiveStatus.Idle)
                {
                    bool isSending = currentStatus.active == DeliveryActiveStatus.Delivering;
                    content.newParagraph();
                    content.Add(new RichBoxSeperationLine());
                    {
                        content.newLine();
                        content.BulletPoint();
                        var text = new RichBoxText("Items ready");
                        text.overrideColor = isSending || currentStatus.CanSend(city) ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        content.BulletPoint();
                        var text = new RichBoxText("Reciever ready");
                        text.overrideColor = isSending || currentStatus.CanRecieve() ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        content.BulletPoint();
                        content.Add(new RichBoxText(currentStatus.longTimeProgress(city)));
                    }
                }
            }
            else
            {

                content.h2("Select delivery service");
                if (city.deliveryServices.Count == 0)
                {
                    content.text("- Empty list -").overrideColor = HudLib.InfoYellow_Light;
                }

                for (int i = 0; i < city.deliveryServices.Count; ++i)
                {
                    content.newLine();

                    DeliveryStatus currentProfile = city.deliveryServices[i];

                    string title;
                    if (currentProfile.Recruitment())
                    {
                        title = DssRef.todoLang.BuildingType_Recruitment;
                    }
                    else
                    {
                        title = DssRef.todoLang.BuildingType_Postal + ": " + currentProfile.profile.type.ToString();
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
                    }, new RbAction1Arg<int>(selectClick, i)));

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
            for (int i = BoundControls.Length - 1; i >= 0; i--)
            {
                int change = -BoundControls[i];
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                    new RbAction2Arg<int, bool>(changeResourcePrice, change, minCap)));

                content.space();
            }

            int current = minCap ? currentStatus.senderMin : currentStatus.recieverMax;
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
