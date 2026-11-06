using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Delivery
{
    class DeliveryRequestMenu
    {
        RichBoxContent content;
        City city;
        Faction faction;

        public DeliveryRequestMenu(RichBoxContent content, City city, Faction faction)
        {
            this.content = content;
            this.city = city;
            this.faction = faction;
        }

        public void cityProfileHud(ItemResourceType resourceType, int resourceIndex)
        {
            request(resourceType, resourceIndex);
        }
        public void toHud(LocalPlayer player, ResourceGroupType tab)
        {
            var resources = ResourceLib.ResourceGroupList(tab);

            foreach (var item in resources)
            {
                request(item);
            }
        }

        void request(ItemResourceType item, int? profileResourceIndex = null)
        {

            IconName.Item(item, out SpriteName itemIcon, out string itemName);
            if (profileResourceIndex == null)
            {
                content.newLine();
            }           

            GroupedResource groupedResource;
            BoolGetSet_Tag useLimitProperty;
            if (profileResourceIndex.HasValue)
            {
                var resProfile = DssRef.storage.cityProfileStorage.profiles.Sel().resourceProfiles.Array[profileResourceIndex.Value];
                groupedResource = resProfile.resource;
                useLimitProperty = (object tag, bool set, bool value) =>
                {
                    if (set)
                    {
                        resProfile.resource.requestDelivery = !resProfile.resource.requestDelivery;
                        DssRef.storage.cityProfileStorage.profiles.SelRef().resourceProfiles.Array[profileResourceIndex.Value] = resProfile;
                    }
                    return resProfile.resource.requestDelivery;
                };
            }
            else if (city != null)
            {
                groupedResource = city.GetGroupedResource(item);

                useLimitProperty = (object tag, bool set, bool value) =>
                {
                    var res = city.GetGroupedResource(item);
                    if (set)
                    {
                        res.requestDelivery = !res.requestDelivery;

                        city.SetGroupedResource(item, res);
                    }
                    return res.requestDelivery;
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
                        res.requestDelivery = !res.requestDelivery;
                        //todo set all cities
                    }

                    return res.requestDelivery;
                };
            }

            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                new RbImage(itemIcon),
                new RbSpace(0.5f),
                new RbText(DssRef.todoLang.Resource_RequestDelivery) },
                useLimitProperty, new RbTooltip((RichBoxContent content, object tag) => {
                    content.text(DssRef.todoLang.Resource_RequestDelivery_Description, HudLib.InfoYellow_Light);
                    content.Add(new RbSeperationLine());
                    content.newParagraph();
                    ResourceLib.FullResourceInfo(faction, city, item, content);
                })));

            
        }
    }
}
