using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Work
{
    class WorkerLimitMenu
    {
        static readonly List<float> DragButtonOptions = new List<float> { 10, 100/*, 1000*/ };

        const int MinLimit = 1;
        const int MaxLimit = 10_000;

        RichBoxContent content;
        City city;
        Faction faction;

        public WorkerLimitMenu(RichBoxContent content, City city, Faction faction)
        {
            this.content = content;
            this.city = city;
            this.faction = faction;
        }

        public void toHud(LocalPlayer player, ResourceGroupType tab)
        {
            var resources = ResourceLib.ResourceGroupList(tab);

            foreach (var item in resources)
            {

                IconName.Item(item, out SpriteName itemIcon, out string itemName);
                WorkPriority workPriority;
                content.newLine();

                //GroupedResource groupedResource;
                var workPriorityType = ItemPropertyColl.Get(item).work;
                BoolGetSet_Tag useLimitProperty;
                
                if (city != null)
                {
                    //groupedResource = city.GetGroupedResource(item);
                    workPriority = city.workTemplate.Get(workPriorityType);

                    useLimitProperty = (object tag, bool set, bool value) =>
                    {
                        var res = city.workTemplate.Get(workPriorityType);

                        if (set)
                        {
                            res.useWorkerCountLimit = value;

                            city.workTemplate.SetWorkPriority(item, res);
                        }
                        return res.useWorkerCountLimit;
                    };
                }
                else
                {
                    //groupedResource = faction.GetRefResourceOverview(item);
                    workPriority = faction.workTemplate.Get(workPriorityType);

                    useLimitProperty = (object tag, bool set, bool value) =>
                    {
                        var res = faction.workTemplate.Get(workPriorityType);

                        if (set)
                        {
                            res.useWorkerCountLimit = value;

                            faction.workTemplate.SetWorkPriority(item, res);
                        }
                        return res.useWorkerCountLimit;
                    };
                }

                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                new RbImage(itemIcon),
                new RbSpace(0.5f),
                new RbImage(SpriteName.WarsIcon_WorkQueueTotal) },
                    useLimitProperty, new RbTooltip((RichBoxContent content, object tag) => {
                        content.h1(".Max worker count", HudLib.TitleColor_Head);
                        content.text("Limit how many workers who will do the same task.");

                        content.newParagraph();
                        content.Add(new RbSeperationLine());
                        ResourceLib.FullResourceInfo(faction, city, item, content);
                    })));

                if (workPriority.useWorkerCountLimit)
                {
                    workCountEdit(content, workPriorityType, workPriority);
                }
                else
                {
                    content.Add(new RbText(DssRef.lang.Hud_NoLimit));
                    //List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(2);
                    //IconName.Storage(ItemPropertyColl.Get(item).storageType, out SpriteName storageIcon, out string storageName);
                    //buttonContent.Add(new RbImage(storageIcon));
                    //buttonContent.Add(new RbSpace());
                    //if (city == null)
                    //{
                    //    buttonContent.Add(new RbText(DssRef.lang.Hud_Maximum));
                    //}
                    //else
                    //{
                    //    buttonContent.Add(new RbText(groupedResource.capacity.ToString()));
                    //}
                    //content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent, null, new RbTooltip_Text(storageName)));
                }

            }
        }

        void workCountEdit(RichBoxContent content, WorkPriorityType type, WorkPriority prio)
        {
            IntGetSetTag property;
            
            if (city != null)
            {
                
                property = (object tag, bool set, int value) =>
                {
                    var res = city.workTemplate.Get(type);
                    if (set)
                    {
                        res.workerCountLimit = (ushort)value;
                        city.workTemplate.SetWorkPriority(type, res);
                    }
                    return res.workerCountLimit;
                };
            }
            else
            {
                
                property = (object tag, bool set, int value) =>
                {
                    var res = city.workTemplate.Get(type);
                    if (set)
                    {
                        res.workerCountLimit = (ushort)value;
                        faction.workTemplate.SetWorkPriority(type, res);
                    }
                    return res.workerCountLimit;
                };
            }

            RbDragButton.RbDragButtonGroup(content, DragButtonOptions, new DragButtonSettings(MinLimit, MaxLimit, 1),
                property, true);
        }
    }
}
