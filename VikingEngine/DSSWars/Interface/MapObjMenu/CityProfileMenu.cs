using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using VikingEngine.Core.DSSWars.Data;
using VikingEngine.Core.DSSWars.Work;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players.PlayerControls;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.MoonFall;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.Core.DSSWars.Interface.MapObjMenu
{
    class CityProfileMenu
    {
        public const string ProfilesMenu = "city_profiles";
        const string EditProfileMenu = "edit_profile";
        const string EditProfileWorkMenu = "edit_profile_work";
        const string EditProfileResourcesMenu = "edit_profile_resources";
const string EditProfileBuildingsMenu = "edit_profile_buildings";
        RichMenu menu;

        public void ToHud(City city, RichMenu menu, RichBoxContent content)
        {
            this.menu = menu;

            HudLib.returnButton(content, menu, true, menu.menuStack.Clear);
            
            CityProfileStorage profiles = DssRef.storage.cityProfileStorage;

            switch (menu.menuStack.Last())
            {
                default:
                    content.h1("City Profile", HudLib.TitleColor_Head);
                    content.newParagraph();
                    if (profiles.profiles.Count == 0)
                    {
                        content.text("No city profiles available.");
                    }
                    else
                    {
                        listAllProfiles(true, city, menu, content, profiles);
                        listAllProfiles(false, city, menu, content, profiles);
                    }

                    content.newParagraph();
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsHudIconAdd),
                        new RbSpace(0.5f),
                        new RbText("Add City Profile") }, new RbAction(() =>
                    {
                        profiles.profiles.selIndex = profiles.profiles.Count;
                        profiles.profiles.Add(new CityProfile() { needsSave = true });
                        profiles.needsSave = true;
                        menu.menuStack.Add(EditProfileMenu);
                        SoundLib.add_item.Play();
                    })));
                    break;
                case EditProfileMenu:
                    {
                        CityProfile profile = profiles.profiles.Sel();
                        //EDIT ONE PROFILE
                        WorkerLimitMenu workerLimitMenu = new WorkerLimitMenu(content, city, city.pfaction.GetFaction());
                        StockPileMenu stockPileMenu = new StockPileMenu(content, city, city.pfaction.GetFaction());
                        DeliveryRequestMenu requestMenu = new DeliveryRequestMenu(content, city, city.pfaction.GetFaction());

                        content.h1(string.Format(DssRef.todoLang.Language_EditX, "City Profile"), HudLib.TitleColor_Head);
                        content.text(string.Format(DssRef.lang.UnitId, profile.id), HudLib.SecondaryTextColor);

                        content.newLine();

                        if (Ref.update.textInput != null &&
                               !Ref.update.textInput.Exiting)
                        {
                            content.Add(new RbButton(new List<AbsRichBoxMember> {
                                    new RbImage(SpriteName.InterfaceTextInput),
                                    new RbSpace(),
                                    new RbText(Ref.update.textInput.DisplayText(), Color.Black),
                                }, null, null)
                            { overrideBgColor = Color.White });
                        }
                        else
                        {
                            var editButton = new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.InterfaceTextInput) },
                            new RbAction(beginEditName), null);
                            content.Add(editButton);
                            content.Add(new RbText(profile.name, HudLib.ColEditText));
                        }
                        content.newLine();

                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Favorite") },
                            FavoriteProperty));
                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Auto-Apply") },
                            AutoApplyProperty, new RbTooltip_Text("All your new cities will automatically apply this profile")));

                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.EditorToolOptionToggle),
                            new RbSpace(0.5f),
                            new RbText("Add/Remove work") },
                             new RbAction2Arg<string, StackOption>(menu.OpenMenu, EditProfileWorkMenu, StackOption.Stack)));
                        if (profile.workProfiles.HasValue())
                        {
                            for (int i = 0; i < profile.workProfiles.Count; i++)
                            {
                                content.newParagraph();
                                var workProfile = profile.workProfiles[i];
                                var prop = WorkPropertyColl.Get(workProfile.workType);

                                content.Add(new RbImage(prop.icon1));
                                if (prop.icon2 != SpriteName.NO_IMAGE)
                                {
                                    content.Add(new RbImage(prop.icon2));
                                }
                                content.hspace();
                                content.Add(new RbText(prop.name));

                                content.newLine();
                                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.todoLang.Language_IncludeX, DssRef.lang.Work_OrderPrioTitle)) },
                                    WorkPriorityProperty)
                                { propertyTag = i });
                                if (workProfile.bPriority)
                                {
                                    content.newLine();
                                    content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                                    content.space();
                                    workProfile.workPriority.toHud(workProfile.workType, null, content, null, city.pfaction.GetFaction(), i);//workTemplate.cityProfileHud(workProfile.workType, i);
                                }
                                content.newLine();
                                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.todoLang.Language_IncludeX, DssRef.todoLang.Work_WorkerCount)) },
                                    WorkWorkerCountProperty)
                                { propertyTag = i });
                                if (workProfile.bWorkerCount)
                                {
                                    content.newLine();
                                    content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                                    content.space();
                                    workerLimitMenu.cityProfileHud(workProfile.workType, i);
                                }
                            }
                        }
                        content.Add(new RbSeperationLine());

                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.EditorToolOptionToggle),
                            new RbSpace(0.5f),
                            new RbText("Add/Remove Resources") },
                             new RbAction2Arg<string, StackOption>(menu.OpenMenu, EditProfileResourcesMenu, StackOption.Stack)));
                        if (profile.resourceProfiles.HasValue())
                        {
                            for (int i = 0; i < profile.resourceProfiles.Count; i++)
                            {
                                content.newParagraph();

                                var resProfile = profile.resourceProfiles[i];
                                IconName.Item(resProfile.resourceType, out var icon, out string name);
                                content.Add(new RbImage(icon));
                                content.hspace();
                                content.Add(new RbText(name));

                                content.newLine();
                                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.todoLang.Language_IncludeX, DssRef.lang.Resource_Tab_Stockpile)) },
                                    ResourceStockpileProperty)
                                { propertyTag = i });

                                if (resProfile.bStockpile)
                                {
                                    content.newLine();
                                    content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                                    content.space();
                                    stockPileMenu.cityProfileHud(resProfile.resourceType, i);
                                }
                                content.newLine();
                                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.todoLang.Language_IncludeX, DssRef.todoLang.Resource_RequestDelivery)) },
                                    ResourceRequestDeliveryProperty)
                                { propertyTag = i });
                                if (resProfile.bRequestDelivery)
                                {
                                    content.newLine();
                                    content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                                    content.space();
                                    requestMenu.cityProfileHud(resProfile.resourceType, i);
                                }
                            }
                        }

                        content.Add(new RbSeperationLine());

                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.EditorToolOptionToggle),
                            new RbSpace(0.5f),
                            new RbText("Add/Remove Buildings") },
                             new RbAction2Arg<string, StackOption>(menu.OpenMenu, EditProfileBuildingsMenu, StackOption.Stack)));

                        if (profile.buildProfiles.HasValue())
                        {
                            for (int i = 0; i < profile.buildProfiles.Count; i++)
                            {
                                content.newParagraph();

                                var itemProfile = profile.buildProfiles[i];
                                IconName.Building(itemProfile.buildType, out var icon, out string name);
                                content.Add(new RbImage(icon));
                                content.hspace();
                                content.Add(new RbText(name));

                                content.newLine();
                                RbDragButton.RbDragButtonGroup(content, new List<float> { 10 },
                                    new DragButtonSettings(1, 100, 1), buildCountProperty, false, i);

                                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Accumilative") },
                                    buildCountAccProperty)
                                { propertyTag = i });

                                content.newLine();
                                for (int prio = 1; prio <= WorkTemplate.MaxPrio; prio++)
                                {
                                    var button = new ArtToggle(prio == itemProfile.buildPriority, new List<AbsRichBoxMember> {
                                        new RbText(prio.ToString())
                                    },
                                    new RbAction1Arg<int>((int selected) =>
                                    {
                                        profile.buildProfiles.Array[i].buildPriority = (byte)selected;
                                    }, prio));

                                    content.Add(button);
                                    if (prio == 0)
                                    {
                                        content.space();
                                    }
                                }
                            }
                        }
                                // content.newParagraph();

                    }
                    break;

                case EditProfileWorkMenu:
                    {
                        CityProfile profile = profiles.profiles.Sel();

                        HashSet<WorkPriorityType> existingWork = new HashSet<WorkPriorityType>();
                        if (profile.workProfiles.HasValue())
                        {
                            foreach (var p in profile.workProfiles)
                            {
                                existingWork.Add(p.workType);
                            }
                        }

                        for (ResourceGroupType group = 0; group < ResourceGroupType.NUM; group++)
                        {
                            var workList = WorkLib.ViewOrder_Group_Work[(int)group];
                            foreach (WorkPriorityType workType in workList)
                            {
                                var prop = WorkPropertyColl.Get(workType);

                                RichBoxContent buttonCont = new RichBoxContent();
                                buttonCont.Add(new RbImage(prop.icon1));
                                if (prop.icon2 != SpriteName.NO_IMAGE)
                                {
                                    buttonCont.Add(new RbImage(prop.icon2));
                                }
                                buttonCont.Add(new RbSpace(0.5f));
                                buttonCont.Add(new RbText(prop.name));

                                bool contains = existingWork.Contains(workType);
                                content.Add(new ArtToggle(contains,
                                    buttonCont,
                                    new RbAction2Arg<WorkPriorityType, bool>((WorkPriorityType selected, bool add) =>
                                    {
                                        if (add)
                                        {
                                            if (profile.workProfiles.IsEmpty())
                                                profiles.profiles.SelRef().workProfiles = new List2<WorkProfile>(8);
                                            profiles.profiles.SelRef().workProfiles.Add(new WorkProfile(selected));
                                        }
                                        else
                                        {
                                            if (profile.resourceProfiles.HasValue())
                                            {
                                                for (int i = 0; i < profile.workProfiles.Count; i++)
                                                {
                                                    if (profile.workProfiles[i].workType == selected)
                                                    {
                                                        profiles.profiles.SelRef().workProfiles.RemoveAt(i);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }, workType, !contains)));

                            }
                            content.newLine();
                        }

                        content.Add(new ArtButton(RbButtonStyle.SuperPrimary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Hud_Apply, HudLib.Color_SuperPrimary) },
                            new RbAction(menu.menuBack)));
                    }
                    break;

                case EditProfileResourcesMenu:
                    {
                        //Add / remove resources
                        CityProfile profile = profiles.profiles.Sel();

                        HashSet<ItemResourceType> existingResources = new HashSet<ItemResourceType>();
                        if (profile.resourceProfiles.HasValue())
                        {
                            foreach (var resProfile in profile.resourceProfiles)
                            {
                                existingResources.Add(resProfile.resourceType);
                            }
                        }

                        for (ResourceGroupType groupType = 0; groupType < ResourceGroupType.Mint; groupType++)
                        {
                            var resourceList = ResourceLib.ResourceGroupList(groupType);
                            foreach (var res in resourceList)
                            {
                                IconName.Item(res, out var icon, out string name);
                                bool contains = existingResources.Contains(res);
                                content.Add(new ArtToggle(contains,
                                    new List<AbsRichBoxMember>{
                                        new RbImage(icon),
                                        new RbSpace(0.5f),
                                        new RbText(name),
                                    },
                                    new RbAction2Arg<ItemResourceType, bool>((ItemResourceType selected, bool add) =>
                                    {
                                        if (add)
                                        {
                                            if (profile.resourceProfiles.IsEmpty())
                                                profiles.profiles.SelRef().resourceProfiles = new List2<ResourceProfile>(8);
                                            profiles.profiles.SelRef().resourceProfiles.Add(new ResourceProfile(selected));
                                        }
                                        else
                                        {
                                            if (profile.resourceProfiles.HasValue())
                                            {
                                                //profile.resourceProfiles.Array.RemoveAll(rp => rp.resourceType == selected);
                                                for (int i = 0; i < profile.resourceProfiles.Count; i++)
                                                {
                                                    if (profile.resourceProfiles[i].resourceType == selected)
                                                    {
                                                        profiles.profiles.SelRef().resourceProfiles.RemoveAt(i);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }, res, !contains)));

                            }
                            content.newLine();
                        }

                        content.Add(new ArtButton(RbButtonStyle.SuperPrimary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Hud_Apply, HudLib.Color_SuperPrimary) },
                            new RbAction(menu.menuBack)));

                    }
                    break;

                case EditProfileBuildingsMenu:
                    {
                        CityProfile profile = profiles.profiles.Sel();

                        List<BuildAndExpandType> buildingsList = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);
                        BuildLib.AvailableBuildTypes(buildingsList, null, false);

                        HashSet<BuildAndExpandType> existing = new HashSet<BuildAndExpandType>();
                        if (profile.buildProfiles.HasValue())
                        {
                            foreach (var m in profile.buildProfiles)
                            {
                                existing.Add(m.buildType);
                            }
                        }

                        foreach (var build in buildingsList)
                        {
                            IconName.Building(build, out var icon, out string name);
                            bool contains = existing.Contains(build);
                            content.Add(new ArtToggle(contains,
                                new List<AbsRichBoxMember>{
                                        new RbImage(icon),
                                        new RbSpace(0.5f),
                                        new RbText(name),
                                },
                                new RbAction2Arg<BuildAndExpandType, bool>((BuildAndExpandType selected, bool add) =>
                                {
                                    if (add)
                                    {
                                        if (profile.buildProfiles.IsEmpty())
                                            profiles.profiles.Sel().buildProfiles = new List2<BuidingProfile>(8);
                                        profiles.profiles.SelRef().buildProfiles.Add(new BuidingProfile(selected));
                                    }
                                    else
                                    {
                                        if (profile.buildProfiles.HasValue())
                                        {
                                            //profile.resourceProfiles.Array.RemoveAll(rp => rp.resourceType == selected);
                                            for (int i = 0; i < profile.buildProfiles.Count; i++)
                                            {
                                                if (profile.buildProfiles[i].buildType == selected)
                                                {
                                                    profiles.profiles.Sel().buildProfiles.RemoveAt(i);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }, build, !contains)));

                        }
                        content.newParagraph();


                        content.Add(new ArtButton(RbButtonStyle.SuperPrimary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Hud_Apply, HudLib.Color_SuperPrimary) },
                               new RbAction(menu.menuBack)));
                    }
                    break;
            }
        }
        //public void beginEditName()
        //{
        //    var reciever = new TextInputState(DssRef.storage.cityProfileStorage.profiles.Sel().name, NameEditEvent, null);
        //    SteamInputManager.tryOpenSteamKeyboard(reciever);
        //}
        public void beginEditName()
        {
            var reciever = new CityProfileTextInput();
            SteamInputManager.tryOpenSteamKeyboard(reciever);
        }
        //void NameEditEvent(string result, object tag)
        //{
        //    if (string.IsNullOrEmpty(result) == false)
        //    {
        //        DssRef.storage.cityProfileStorage.profiles.Sel().name = result;
        //    }
        //    menu.needRefresh = true;
        //}
        private void listAllProfiles(bool favorite, City city, RichMenu menu, RichBoxContent content, CityProfileStorage profiles)
        {
            for (int i = 0; i < profiles.profiles.Count; i++)
            {
                CityProfile profile = profiles.profiles[i];
                if (favorite == profile.favorite)
                {

                    RichBoxContent buttonCont = new RichBoxContent();

                    buttonCont.Add(new RbImage(SpriteName.WarsHudIconOpen));
                    buttonCont.hspace();
                    if (profile.tag.tagToHud(buttonCont))
                    {
                        buttonCont.hspace();
                    }
                    buttonCont.Add(new RbText(profile.name, HudLib.TitleColor_Name));
                    buttonCont.space();

                    const float AdjustIconRight = -0.4f;

                    if (profile.workProfiles.HasValue())
                    {
                        for (int j = 0; j < Math.Min(profile.workProfiles.Count, 3); j++)
                        {
                            var prop = WorkPropertyColl.Get(profile.workProfiles[j].workType);
                            buttonCont.Add(new RbImage(prop.icon1, 1, null, 0, AdjustIconRight));
                            if (prop.icon2 != SpriteName.NO_IMAGE)
                            { 
                                buttonCont.Add(new RbImage(prop.icon2, 1, null, 0, AdjustIconRight));
                            }
                        }
                    }
                    if (profile.resourceProfiles.HasValue())
                    {
                        for (int j = 0; j < Math.Min(profile.resourceProfiles.Count, 3); j++)
                        {
                            IconName.Item(profile.resourceProfiles[j].resourceType, out var icon, out _);
                            buttonCont.Add(new RbImage(icon, 1, null, 0, AdjustIconRight));
                        }
                    }
                    if (profile.buildProfiles.HasValue())
                    {
                        for (int j = 0; j < Math.Min(profile.buildProfiles.Count, 3); j++)
                        {
                            IconName.Building(profile.buildProfiles[j].buildType, out var icon, out _);
                            buttonCont.Add(new RbImage(icon, 1, null, 0, AdjustIconRight));
                        }
                    }

                    content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonCont, null));

                    content.Add(new ArtButton(RbButtonStyle.SuperPrimary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Apply, HudLib.Color_SuperPrimary) },
                        new RbAction(() =>
                        {
                            profile.applyToCity(city);
                            menu.menuStack.Clear();
                            SoundLib.processComplete.Play();
                        }),
                        new RbTooltip(applyTooltip, city)));

                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconEditor) },
                        new RbAction1Arg<int>((int selected) =>
                        {
                            profiles.profiles.selIndex = selected;
                            profiles.profiles.SelRef().needsSave = true;
                            profiles.needsSave = true;
                            menu.menuStack.Add(EditProfileMenu);
                            SoundLib.option_select.Play();
                        }, i), new RbTooltip_Text(string.Format(DssRef.todoLang.Language_EditX, profile.name))));

                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbImage(SpriteName.EditorRemoveCross) },
                        new RbAction1Arg<int>((int selected) =>
                        {
                            profiles.destroyProfile(selected);
                            SoundLib.start_destroy_contruct.Play();
                        }, i), new RbTooltip_Text(string.Format(DssRef.lang.Edit_RemoveX, profile.name))));

                    content.newLine();
                }
            }
        }

        public bool FavoriteProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.cityProfileStorage.profiles.SelRef().favorite = value;
            }
            return DssRef.storage.cityProfileStorage.profiles.Sel().favorite;
        }
        public bool AutoApplyProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.cityProfileStorage.profiles.SelRef().autoApply = value;
            }
            return DssRef.storage.cityProfileStorage.profiles.Sel().autoApply;
        }

        public bool WorkPriorityProperty(object tag, bool set, bool value)
        {
            int resourceIndex = (int)tag;
            CityProfile profile = DssRef.storage.cityProfileStorage.profiles.Sel();

            if (set)
            {
                profile.workProfiles[resourceIndex].bPriority = value;
            }
            return profile.workProfiles[resourceIndex].bPriority;
        }
        public bool WorkWorkerCountProperty(object tag, bool set, bool value)
        {
            int resourceIndex = (int)tag;
            CityProfile profile = DssRef.storage.cityProfileStorage.profiles.Sel();

            if (set)
            {
                profile.workProfiles[resourceIndex].bWorkerCount = value;
            }
            return profile.workProfiles[resourceIndex].bWorkerCount;
        }

        public bool ResourceStockpileProperty(object tag, bool set, bool value)
        {
            int resourceIndex = (int)tag;
            CityProfile profile = DssRef.storage.cityProfileStorage.profiles.Sel();

            if (set)
            {
                profile.resourceProfiles[resourceIndex].bStockpile = value;
            }
            return profile.resourceProfiles[resourceIndex].bStockpile;
        }
        public bool ResourceRequestDeliveryProperty(object tag, bool set, bool value)
        {
            int resourceIndex = (int)tag;
            CityProfile profile = DssRef.storage.cityProfileStorage.profiles.Sel();

            if (set)
            {
                profile.resourceProfiles[resourceIndex].bRequestDelivery = value;
            }
            return profile.resourceProfiles[resourceIndex].bRequestDelivery;
        }

        void applyTooltip(RichBoxContent content, object tag)
        {
            City city = (City)tag;
            content.text("Apply profile settings to city", HudLib.TitleColor_Action);
            content.newLine();
            city.CityPresentationHud(new VikingEngine.DSSWars.Interface.ObjectHudArgs(null, content, null, true), true);
        }

        int buildCountProperty(object tag, bool set, int value)
        {
            int itemIndex = (int)tag;
            CityProfile profile = DssRef.storage.cityProfileStorage.profiles.Sel();

            if (set)
            {
                profile.buildProfiles[itemIndex].count = (byte)value;
            }
            return profile.buildProfiles[itemIndex].count;
        }
        bool buildCountAccProperty(object tag, bool set, bool value)
        {
            int itemIndex = (int)tag;
            CityProfile profile = DssRef.storage.cityProfileStorage.profiles.Sel();

            if (set)
            {
                profile.buildProfiles[itemIndex].accumulate = value;
            }
            return profile.buildProfiles[itemIndex].accumulate;
        }
    }
}
