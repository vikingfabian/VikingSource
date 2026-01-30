using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface.Component;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Conscript
{
    class ConscriptMenu
    {
        City city;
        LocalPlayer player;
        ProgressQue que = new ProgressQue();

        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            this.city = city;
            this.player = player;

            if (arraylib.InBound(city.conscriptBuildings, city.selectedConscript))
            {
                bool advanced = city.buildingStructure.greatHall || StartupSettings.UnlockAllProgress;
                BarracksStatus currentStatus = get();
                int menCostNext = currentStatus.profile.menCost();
                SpriteName icon =  new SoldierConscriptProfile() { conscript = currentStatus.profile }.Icon();
                
                string typeName = null; 
                ItemResourceType[] weapons = null;
                bool hasGuardOption = true;
                switch (currentStatus.type)
                {
                    case Build.BuildAndExpandType.SoldierBarracks:
                        typeName = DssRef.lang.BuildingType_SoldierBarracks;
                        weapons = ConscriptDataLib.SoldierWeapons;
                        break;
                    case Build.BuildAndExpandType.ArcherBarracks:
                        typeName = DssRef.lang.BuildingType_ArcherBarracks;
                        weapons = ConscriptDataLib.ArcherWeapons;
                        break;
                    case Build.BuildAndExpandType.WarmachineBarracks:
                        typeName = DssRef.lang.BuildingType_WarmachineBarracks;
                        weapons = ConscriptDataLib.WarmachineWeapons;
                        break;
                    //case Build.BuildAndExpandType.KnightsBarracks:
                    //    hasGuardOption = false;
                    //    typeName = DssRef.lang.BuildingType_KnightsBarracks;
                    //    weapons = NobelWeapons;
                    //    break;
                    case Build.BuildAndExpandType.GunBarracks:
                        typeName = DssRef.lang.BuildingType_GunBarracks;
                        weapons = ConscriptDataLib.GunWeapons;
                        break;
                    case Build.BuildAndExpandType.CannonBarracks:
                        typeName = DssRef.lang.BuildingType_CannonBarracks;
                        weapons = ConscriptDataLib.CannonWeapons;
                        break;
                }

                HudLib.buildingMenuTitle(content, icon, typeName, currentStatus.idAndPosition, city.selectedConscript,
                    city.conscriptBuildings.Count, () => { city.selectedConscript = -1; },
                    (int next) => {
                        city.selectedConscript = Bound.SetRollover(city.selectedConscript + next, 0, city.conscriptBuildings.Count - 1);
                    });


                content.newParagraph();

                bool guardTab = currentStatus.profile.specialization == SpecializationType.CityGuard;

                if (hasGuardOption)
                {
                    content.Add(new ArtOption(!guardTab,
                        new List<AbsRichBoxMember> { 
                            new RbImage(SpriteName.WarsArmy),
                            new RbSpace(),
                            new RbText(DssRef.lang.Conscript_Soldiers_ArmyType) },
                        new RbAction1Arg<bool>(guardTabClick, false, RbSoundType.Option), new RbTooltip_Text(DssRef.lang.Conscript_Soldiers_ArmyType_Description)));
                    content.Add(new ArtOption(guardTab,

                        new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.WarsGuard),
                            new RbSpace(),
                            new RbText(DssRef.lang.Conscript_Soldiers_GuardType) },
                        new RbAction1Arg<bool>(guardTabClick, true, RbSoundType.Option), new RbTooltip_Text(DssRef.lang.Conscript_Soldiers_GuardType_Description)));
                }

                if (advanced)
                {
                    content.newParagraph();
                    HudLib.Label(content, DssRef.todoLang.Resource_TypeName_ManType);
                    content.space();
                    foreach (var item in ConscriptDataLib.MenTypes)
                    {
                        IconName.Item(item, out SpriteName itemIcon, out _);

                        var buttonContent = new List<AbsRichBoxMember>(3) {
                        new RbImage(itemIcon),
                    };

                        if (city.GetGroupedResource(item).amount >= menCostNext)
                        {
                            buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                        }

                        var button = new ArtOption(item == currentStatus.profile.man, buttonContent,
                        new RbAction1Arg<ItemResourceType>(manClick, item, RbSoundType.Option),
                        new RbTooltip(manTooltip, item)
                        );

                        content.Add(button);
                    }
                }

                content.newParagraph();
                HudLib.Label(content, DssRef.lang.Conscript_WeaponTitle);
                content.newLine();
                
                foreach (var weapon in weapons)
                {
                    IconName.Item(weapon, out SpriteName weaponicon, out _);

                    var buttonContent = new List<AbsRichBoxMember>(3) {
                        new RbImage(weaponicon),
                    };

                    if (city.GetGroupedResource(weapon).amount >= menCostNext)
                    {
                        buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                    }

                    var button = new ArtOption(weapon == currentStatus.profile.weapon,buttonContent,
                    new RbAction1Arg<ItemResourceType>(weaponClick, weapon, RbSoundType.Option),
                    new RbTooltip(weaponTooltip, weapon)
                    );
                    
                    content.Add(button);
                }

                if (advanced)
                {
                    content.newParagraph();
                    HudLib.Label(content, DssRef.todoLang.Resource_TypeName_Shield);
                    content.newLine();

                    var shields = currentStatus.profile.AvailableShields();
                    foreach (var item in shields)
                    {
                        IconName.Item(item, out SpriteName itemIcon, out _);

                        var buttonContent = new List<AbsRichBoxMember>(3) {
                        new RbImage(itemIcon),
                    };

                        if (city.GetGroupedResource(item).amount >= menCostNext)
                        {
                            buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                        }

                        var button = new ArtOption(item == currentStatus.profile.shield, buttonContent,
                        new RbAction1Arg<ItemResourceType>(shieldClick, item, RbSoundType.Option),
                        new RbTooltip(shieldTooltip, item)
                        );

                        content.Add(button);
                    }
                }

                HudLib.Label(content, DssRef.lang.Conscript_ArmorTitle);
                content.newLine();

                


                foreach (var armorLvl in ConscriptDataLib.ArmorOptions)
                {
                    var buttonContent = new List<AbsRichBoxMember>(3);
                    if (city.GetGroupedResource(armorLvl).amount >= menCostNext)
                    {
                        buttonContent.Add(new RbImage(SpriteName.warsResourceChunkAvailable));
                    }
                    if (armorLvl != ItemResourceType.NONE)
                    {
                        IconName.Item(armorLvl, out SpriteName armoricon, out _);
                        buttonContent.Add(new RbImage(armoricon));
                    }

                    var button = new ArtOption(armorLvl == currentStatus.profile.armorLevel, buttonContent,
                        new RbAction1Arg<ItemResourceType>(armorClick, armorLvl, RbSoundType.Option),
                    new RbTooltip(armorTooltip, armorLvl));
                    content.Add(button);
                }

                if (advanced)
                {
                    content.newParagraph();
                    HudLib.Label(content, DssRef.todoLang.Resource_TypeName_Animal);
                    content.newLine();

                    foreach (var item in ConscriptDataLib.AnimalTypes)
                    {
                        IconName.Item(item, out SpriteName itemIcon, out _);

                        var buttonContent = new List<AbsRichBoxMember>(3) {
                        new RbImage(itemIcon),
                    };

                        if (city.GetGroupedResource(item).amount >= menCostNext)
                        {
                            buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                        }

                        var button = new ArtOption(item == currentStatus.profile.animal, buttonContent,
                        new RbAction1Arg<ItemResourceType>(animalClick, item, RbSoundType.Option),
                        new RbTooltip(animalTooltip, item)
                        );

                        content.Add(button);
                    }

                    if (currentStatus.profile.animal != ItemResourceType.NONE)
                    {
                        content.newParagraph();
                        HudLib.Label(content, DssRef.todoLang.Resource_TypeName_MountArmorTitle);
                        content.newLine();

                        foreach (var item in ConscriptDataLib.MountArmorTypes)
                        {
                            IconName.Item(item, out SpriteName itemIcon, out _);

                            var buttonContent = new List<AbsRichBoxMember>(3) {
                        new RbImage(itemIcon),
                    };

                            if (city.GetGroupedResource(item).amount >= menCostNext)
                            {
                                buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                            }

                            var button = new ArtOption(item == currentStatus.profile.mountArmor, buttonContent,
                            new RbAction1Arg<ItemResourceType>(mountArmorClick, item, RbSoundType.Option),
                            new RbTooltip(mountArmorTooltip, item)
                            );

                            content.Add(button);
                        }

                        content.newParagraph();
                        HudLib.Label(content, DssRef.todoLang.Resource_TypeName_Vehicle);
                        content.newLine();

                        foreach (var item in ConscriptDataLib.VehicleTypes)
                        {
                            IconName.Item(item, out SpriteName itemIcon, out _);

                            var buttonContent = new List<AbsRichBoxMember>(3) {
                        new RbImage(itemIcon),
                    };

                            if (city.GetGroupedResource(item).amount >= menCostNext)
                            {
                                buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                            }

                            var button = new ArtOption(item == currentStatus.profile.vehicle, buttonContent,
                            new RbAction1Arg<ItemResourceType>(vehicleClick, item, RbSoundType.Option),
                            new RbTooltip(vehicleTooltip, item)
                            );

                            content.Add(button);
                        }
                    }
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_TrainingTitle);
                content.newLine();
                TrainingLevel minLevel = currentStatus.profile.man == ItemResourceType.NobelMen ? TrainingLevel.Basic : TrainingLevel.Minimal;

                TrainingLevel maxLevel = currentStatus.maxTrainingLevel;
                if (city.Culture == CityCulture.CrabMentality)
                {
                    maxLevel = TrainingLevel.Basic;
                }

                for (TrainingLevel training = minLevel; training <= maxLevel; training++)
                {
                    var button = new ArtOption(training == currentStatus.profile.training,new List<AbsRichBoxMember>{
                        new RbImage(LangLib.Training_Icon(training)),
                        new RbText( LangLib.Training(training))
                    }, new RbAction1Arg<TrainingLevel>(trainingClick, training, RbSoundType.Option),
                    new RbTooltip(trainingTooltip, new TrainingTooltipArgs() { training = training, buildtype = currentStatus.type }));
                    
                    content.Add(button);
                }

                if (advanced && !guardTab)
                {
                    content.newParagraph();

                    HudLib.Label(content, DssRef.lang.Conscript_SpecializationTitle);
                    content.space();
                    HudLib.InfoButton(content, new RbTooltip_Text(string.Format(DssRef.lang.Conscript_SpecializationDescription, TextLib.PercentTextWithSymbol(DssConst.Conscript_SpecializePercentage))));
                    content.newLine();

                    SpecializationType[] specializationTypes = currentStatus.profile.avaialableSpecializations();


                    foreach (var specialization in specializationTypes)
                    {
                        var specText = LangLib.SpecializationTypeName(specialization, out var specIcon);
                        var button = new ArtOption(specialization == currentStatus.profile.specialization, new List<AbsRichBoxMember>{
                            new RbImage(specIcon, 0.8f),
                            new RbSpace(0.5f),
                            new RbText(specText)
                        }, new RbAction1Arg<SpecializationType>(specializationClick, specialization, RbSoundType.Option));
                        
                        content.Add(button);
                    }
                }
                content.newParagraph();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Label);

                resourcesToMenu(content, city, currentStatus);
               
                content.newParagraph();
                que.labelToHud(content);
                progress(currentStatus);
                que.buttonsToHud(player, content, queClick, currentStatus.que, BarracksStatus.MaxQue, true);

                content.newParagraph();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsWorker), new RbSpace(), new RbText(DssRef.lang.Conscript_MaxPopulation) },
                    maxPopulationProperty, new RbTooltip_Text(DssRef.lang.Conscript_MaxPopulation_Description)));
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsResource_Food), new RbSpace(), new RbText(DssRef.lang.Conscript_FoodAbundance) },
                    maxFoodProperty, new RbTooltip_Text(DssRef.lang.Conscript_FoodAbundance_Description)));

                content.newParagraph();
                HudLib.copyPaste(content, player,
                    new RbAction1Arg<LocalPlayer>(city.copyConscript, player, RbSoundType.Copy),
                     new RbAction1Arg<LocalPlayer>(city.pasteConscript, player, RbSoundType.Paste));
                
            }
            else
            {
                if (city.conscriptBuildings.Count == 0)
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_Barracks));
                    content.space();
                    content.Add(new RbText(DssRef.lang.BuildingType_Barracks));
                    //content.newLine();
                    //content.text(DssRef.lang.Hud_RequirementOr);
                    //content.newLine();
                    //content.Add(new RbImage(SpriteName.WarsBuild_Nobelhouse));
                    //content.space();
                    //content.Add(new RbText(DssRef.lang.Building_NobleHouse));
                }
                else
                {
                    int typeCount = 0;
                    Span<bool> containsBarrack = stackalloc bool[ConscriptDataLib.BarrackTypes.Length];
                    bool hasPreSelectedTab = false;
                    for (int i = 0; i < city.conscriptBuildings.Count; ++i)
                    {
                        var type = city.conscriptBuildings[i].type;
                        int containsIx = ConscriptDataLib.TypeToBarrackTypeIx[type];
                        if (!containsBarrack[containsIx])
                        {
                            if (type == player.conscriptSubTab)
                            {
                                hasPreSelectedTab = true;
                            }
                            containsBarrack[containsIx] = true;
                            typeCount++;
                        }
                    }

                    //Apply to all options
                    content.h2(DssRef.lang.GeneralSetting_SetAll, HudLib.TitleColor_Action);
                    HudLib.Label(content, DssRef.lang.Hud_ProductionQueue); content.space();
                    que.listToHud(player, content, queueToAll, true);

                    if (player.conscriptSubTab != BuildAndExpandType.ALL ||
                        typeCount == 1)
                    {
                        content.newLine();

                        player.gameControls.input.Paste.ToRichContent(content);
                        content.hspace();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.WarsHudIconPaste),
                            new RbSpace(),
                            new RbText(DssRef.lang.Hud_Paste) },
                            new RbAction1Arg<LocalPlayer>(city.pasteConscriptToAll, player, RbSoundType.Paste)));
                        
                    }

                    content.Add(new RbSeperationLine());

                    content.h2(DssRef.lang.Conscript_SelectBuilding, HudLib.TitleColor_Action);

                    if (!hasPreSelectedTab)
                    {
                        player.conscriptSubTab = BuildAndExpandType.ALL;
                    }

                    if (typeCount > 1)
                    {
                        content.newLine();
                        SubTab(BuildAndExpandType.ALL);

                        for (int i = 0; i < ConscriptDataLib.BarrackTypes.Length; ++i)
                        {
                            if (containsBarrack[i])
                            {
                                SubTab(ConscriptDataLib.BarrackTypes[i]);
                            }
                        }

                        void SubTab(BuildAndExpandType filter)
                        {
                            string filterName;
                            if (filter == BuildAndExpandType.ALL)
                            {
                                filterName = DssRef.lang.Hud_All;
                            }
                            else
                            {
                                IconName.Building(filter, out _, out filterName);
                            }


                            var subTab = new ArtButton(player.conscriptSubTab == filter ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, new List<AbsRichBoxMember>
                            {
                                new RbText(filterName)
                            },
                               new RbAction1Arg<BuildAndExpandType>((BuildAndExpandType filter) =>
                               {
                                   player.conscriptSubTab = filter;
                               }, filter, RbSoundType.Tab));
                            content.Add(subTab);
                        }
                    }

                    for (int i = 0; i < city.conscriptBuildings.Count; ++i)
                    {
                        content.newLine();

                        BarracksStatus currentProfile = city.conscriptBuildings[i];

                        if (player.conscriptSubTab == BuildAndExpandType.ALL ||
                            player.conscriptSubTab == currentProfile.type)
                        {
                            

                            string caption;
                            SpriteName icon;
                            if (currentProfile.profile.specialization == SpecializationType.CityGuard)
                            {
                                icon = SpriteName.WarsGuard;
                                caption = DssRef.lang.Conscript_Soldiers_GuardType;
                            }
                            else
                            {
                                icon = new SoldierConscriptProfile() { conscript = currentProfile.profile }.Icon();
                                caption = DssRef.lang.Conscript_Soldiers_ArmyType;
                            }

                            IconName.Item(currentProfile.profile.weapon, out SpriteName weaponicon, out _);
                            IconName.Item(currentProfile.profile.armorLevel, out SpriteName armoricon, out _);

                            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>(){
                                new RbImage(icon),
                                new RbSpace(),
                                new RbText(caption, HudLib.TitleColor_Label_Dark),
                                new RbSpace(),
                                new RbImage(LangLib.Training_Icon(currentProfile.profile.training)),
                                new RbImage(weaponicon),
                                new RbImage(armoricon),

                                new RbNewLine(),
                                 new RbText(currentProfile.shortActiveString(), HudLib.InfoYellow_Dark),
                            }, new RbAction1Arg<int>(selectClick, i, RbSoundType.Default)));

                        }
                    }

                    
                }

                //settler
                content.Add(new RbSeperationLine());
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsSettlerAdd), new RbSpace(), new RbText(DssRef.lang.UnitType_Settler) },
                    new RbAction(city.conscriptSettlerLink),
                    new RbTooltip(settlerTooltip),
                     city.SettlerBp().available(city)));

                if (DssRef.difficulty.GodPowers() || StartupSettings.EndlessResources)
                {
                    content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsSettlerAdd) },
                       new RbAction(city.conscriptSettlerLink_Free),
                       null, true));
                }
            }

            void queueToAll(int count)
            {
                city.queueToAllConscripts(count, player);
                
            }

            void progress(BarracksStatus currentStatus)
            {
                if (currentStatus.active != ConscriptActiveStatus.Idle)
                {
                    int menCostProgress = currentStatus.menNeeded;
                    currentStatus.followsRequirements(city, out bool hasPopulation, out bool hasFood);

                    content.Add(new RbSeperationLine());
                    if (currentStatus.requireMaxPopulation)
                    {
                        progressPoint(DssRef.lang.Conscript_MaxPopulation, true, hasPopulation);
                    }
                    if (currentStatus.requireMaxFood)
                    {
                        progressPoint(DssRef.lang.Conscript_FoodAbundance, true, hasFood);
                    }

                    progressPoint(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingEquipment, menCostProgress, out bool gotEquipment), currentStatus.active > ConscriptActiveStatus.CollectingEquipment, gotEquipment);
                    progressPoint(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingMen, menCostProgress, out bool gotMen), currentStatus.active > ConscriptActiveStatus.CollectingMen, gotMen);

                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(currentStatus.longTimeProgress(), currentStatus.active == ConscriptActiveStatus.Training? null : HudLib.SecondaryTextColor));
                    }

                    void progressPoint(string textString, bool active, bool collected)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        var text = new RbText(textString);
                        if (active)
                        {
                            if (collected)
                            {
                                text.overrideColor = HudLib.AvailableColor;
                                content.Add(new RbImage(HudLib.AvailableIcon));
                            }
                            else
                            {
                                text.overrideColor = HudLib.NotAvailableColor;
                                content.Add(new RbImage(HudLib.NotAvailableIcon));
                            }
                            content.space();
                            
                        }
                        else
                        {
                            text.overrideColor = HudLib.SecondaryTextColor;
                        }
                        content.Add(text);
                    }
                }
            }
        }

        void settlerTooltip(RichBoxContent content, object tag)
        {
            content.h2(SpriteName.WarsSettler, DssRef.lang.UnitType_Settler, HudLib.TitleColor_TypeName);
            content.text(DssRef.lang.UnitType_Settler_Description, HudLib.InfoYellow_Light);
            city.SettlerBp().toMenu(content, city);
        }

        public static void resourcesToMenu(RichBoxContent content, City city, BarracksStatus currentStatus)
        {
            int menCostNext = currentStatus.profile.menCost();

            resource(content, ItemResourceType.Men, menCostNext, city.workForce.amount);


            //content.newLine();
            //HudLib.BulletPoint(content);
            //HudLib.ResourceCost(content, ResourceType.Worker, menCostNext, city.workForce.amount);

            //content.newLine();
            //HudLib.BulletPoint(content);
            ////var weaponItem = ConscriptProfile.WeaponItem(currentStatus.profile.weapon);
            var weaponRes = city.GetGroupedResource(currentStatus.profile.weapon);
            //HudLib.ResourceCost(content, currentStatus.profile.weapon, menCostNext, weaponRes.amount);

            resource(content, currentStatus.profile.weapon, menCostNext, weaponRes.amount);

            if (currentStatus.profile.armorLevel != ItemResourceType.NONE)
            {
                //content.newLine();
                //HudLib.BulletPoint(content);
                //var armorItem = ConscriptProfile.ArmorItem(currentStatus.profile.armorLevel);
                var armorRes = city.GetGroupedResource(currentStatus.profile.armorLevel);
                //HudLib.ResourceCost(content, currentStatus.profile.armorLevel, menCostNext, armorRes.amount);
                resource(content, currentStatus.profile.armorLevel, menCostNext, armorRes.amount);
            }

            if (currentStatus.profile.specialization == SpecializationType.CityGuard)
            {
                //content.newParagraph();
                //content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement, HudLib.TitleColor_Label);

                content.newLine();
                //HudLib.BulletPoint(content);
                bool available = menCostNext <= city.AvailableGuardHousing();
                content.Add(new RbImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
                content.space();
                HudLib.ResourceCost(content, SpriteName.WarsBuild_GuardOffice, DssRef.lang.GuardHousingCount, menCostNext, city.AvailableGuardHousing());
            }

            void resource(RichBoxContent content, ItemResourceType resource, int needResource, int hasResource)
            {
                content.newLine();

                bool available = hasResource >= needResource;
                content.Add(new RbImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
                content.space();
                //SpriteName icon = ResourceLib.Icon(resource);
                IconName.Item(resource, out SpriteName icon, out string name);

                if (icon != SpriteName.NO_IMAGE)
                {
                    content.Add(new RbImage(icon));
                    content.space(0.5f);
                }

                string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                    name, TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

                content.Add(new RbText(text, HudLib.ResourceCostColor(available)));
            }
        }

        bool maxPopulationProperty(object tag, bool setValue, bool value)
        {
            BarracksStatus currentProfile = get();
            if (setValue)
            {
                currentProfile.requireMaxPopulation = value;
                set(currentProfile);
            }
            return currentProfile.requireMaxPopulation;
        }
        bool maxFoodProperty(object tag, bool setValue, bool value)
        {
            BarracksStatus currentProfile = get();
            if (setValue)
            {
                currentProfile.requireMaxFood = value;
                set(currentProfile);
            }
            return currentProfile.requireMaxFood;
        }

        void guardTabClick(bool guard)
        {
            BarracksStatus currentProfile = get();
            BarracksStatus defaultProfile = new BarracksStatus(currentProfile.type);
            currentProfile.profile.specialization = guard? SpecializationType.CityGuard : defaultProfile.profile.specialization;
            set(currentProfile);
        }

        void specializationClick(SpecializationType specialization)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.specialization = specialization;
            set(currentProfile);

        }

        void manClick(ItemResourceType item)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.man = item;
            set(currentProfile);
        }
        void weaponClick(ItemResourceType item)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.weapon = item;
            set(currentProfile);
        }
        void shieldClick(ItemResourceType item)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.shield = item;
            set(currentProfile);
        }
        
        void animalClick(ItemResourceType item)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.animal = item;
            set(currentProfile);
        }
        void mountArmorClick(ItemResourceType item)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.mountArmor = item;
            set(currentProfile);
        }
        void vehicleClick(ItemResourceType item)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.vehicle = item;
            set(currentProfile);
        }

        void weaponTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType weapon = (ItemResourceType)tag;
                        
            var data = new SoldierConscriptProfile() { conscript = new ConscriptProfile() { weapon = weapon } }.init();

            IconName.Item(weapon, out SpriteName weaponicon, out string weaponname);
            content.h1(weaponname, HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new RbImage(SpriteName.warsArmyTag_Hit));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Conscript_WeaponDamage, ConscriptProfile.WeaponDamage(weapon, out int splashCount))));

            if (splashCount > 0)
            {
                content.newLine();
                content.Add(new RbText(splashCount < 6 ? DssRef.lang.Conscript_SplashDamage : DssRef.lang.Conscript_HighSplashDamage));
            }
            
            switch (weapon)
            {
                case ItemResourceType.HandSpear:
                    content.newLine();
                    content.Add(new RbImage(SpriteName.warsArmyTag_Shield));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Conscript_ArmorHealth, TextLib.PlusMinus(DssConst.WeaponHealthAdd_Handspear))));
                    break;
            }

            if (data.blockReducingAttack_Inv < 1f)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.Conscript_BlockReducingAttack));
            }
            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbText(string.Format( DssRef.lang.Conscript_BlockPerSecond, TextLib.OneDecimal(1f/ data.blocksRefillTimeSec))));
            content.newLine();
            content.Add(new RbText(DssRef.lang.Conscript_BlockDescription, HudLib.InfoYellow_Light));

            content.newParagraph();

            ResourceLib.FullResourceInfo(player.faction, city, weapon, content); 
            //var res = city.GetGroupedResource(weapon);

            //content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;
            //bool reachedBuffer = false;
            //res.toMenu(content, weapon, false, ref reachedBuffer);
        }

        void manTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType item = (ItemResourceType)tag;

            ResourceLib.FullResourceInfo(player.faction, city, item, content);
        }

        void shieldTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType item = (ItemResourceType)tag;

            ResourceLib.FullResourceInfo(player.faction, city, item, content);
        }
        void animalTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType item = (ItemResourceType)tag;

            ResourceLib.FullResourceInfo(player.faction, city, item, content);
        }
        void mountArmorTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType item = (ItemResourceType)tag;

            ResourceLib.FullResourceInfo(player.faction, city, item, content);
        }
        void vehicleTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType item = (ItemResourceType)tag;

            ResourceLib.FullResourceInfo(player.faction, city, item, content);
        }

        void armorClick(ItemResourceType armor)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.armorLevel = armor;
            set(currentProfile);
        }
        void armorTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType armor = (ItemResourceType)tag;
            IconName.Item(armor, out SpriteName armoricon, out string armorname);
            content.h1(armorname, HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new RbImage(SpriteName.warsArmyTag_Shield));
            content.Add(new RbSpace());
            content.Add(new RbText(string.Format(DssRef.lang.Conscript_ArmorHealth, ConscriptProfile.ArmorHealth(armor))));

            if (armor != ItemResourceType.NONE)
            {
                content.newParagraph();
                content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;

                bool reachedBuffer = false;
                city.GetGroupedResource(armor).toMenu(content, armor, ref reachedBuffer);
               
            }
        }

        void trainingClick(TrainingLevel training)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.training = training;

            set(currentProfile);
        }

        struct TrainingTooltipArgs
        {
            public TrainingLevel training;
            public Build.BuildAndExpandType buildtype;
        }
        void trainingTooltip(RichBoxContent content, object tag)
        {
            TrainingTooltipArgs args = (TrainingTooltipArgs)tag;
            content.text(string.Format(DssRef.lang.Conscript_TrainingTime, new TimeLength(ConscriptProfile.TrainingTime(args.training, args.buildtype)).LongString()));
            content.text(string.Format(DssRef.lang.Conscript_TrainingSpeed, TextLib.OneDecimal(ConscriptProfile.TrainingAttackSpeed(args.training))));

        }
        void queClick(int length)
        {
            BarracksStatus currentStatus = get();
            currentStatus.que = length;
            set(currentStatus);
        }

        void selectClick(int index)
        {
            city.selectedConscript = index;
        }

        BarracksStatus get()
        {
            return city.conscriptBuildings[city.selectedConscript];
        }

        void set(BarracksStatus profile)
        {
            var spec = profile.profile.avaialableSpecializations();
            if (profile.profile.specialization != SpecializationType.CityGuard &&
                !spec.Contains(profile.profile.specialization))
            {
                profile.profile.specialization = spec[0];
            }

            city.conscriptBuildings[city.selectedConscript] = profile;

            city.onConscriptChange();
        }


    }
}
