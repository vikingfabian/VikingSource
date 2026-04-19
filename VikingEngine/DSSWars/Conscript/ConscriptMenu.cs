using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Interface.Component;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.PJ;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                    content.Add(new RbSeperationLine());
                }
                ConscriptUnitCount unitCount = new ConscriptUnitCount(currentStatus.profile);
                if (advanced)
                {
                    content.newParagraph();
                    HudLib.Label(content, TextLib.LargeFirstLetter( DssRef.lang.Resource_TypeName_ManType));
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
                        new RbTooltip(manTooltip, new ManTooltipArgs() { item = item, count = unitCount.TotalMen,})
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

                ConscriptOptions conscriptOptions = new ConscriptOptions(currentStatus.profile);
                conscriptOptions.CheckLegal(ref currentStatus.profile);

                if (advanced)
                {
                    content.newParagraph();
                    HudLib.Label(content, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Shield));
                    content.newLine();

                    var shields = conscriptOptions.AvailableShields;
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

                if (advanced && !guardTab)
                {
                    content.newParagraph();
                    HudLib.Label(content, TextLib.LargeFirstLetter( DssRef.lang.Resource_TypeName_Animal));
                    content.newLine();

                    foreach (var item in ConscriptDataLib.AnimalTypes)
                    {
                        IconName.Item(item, out SpriteName itemIcon, out _);

                        var buttonContent = new List<AbsRichBoxMember>(3) { new RbImage(itemIcon)};

                        if (city.GetGroupedResource(item).amount >= menCostNext)
                        {
                            buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                        }
                        var animalTip = new ManTooltipArgs() { item = item, count = unitCount.groupUnitCount, };
                        var button = new ArtOption(item == currentStatus.profile.animal, buttonContent,
                        new RbAction1Arg<ItemResourceType>(animalClick, item, RbSoundType.Option),
                        new RbTooltip(animalTooltip, animalTip)
                        );

                        content.Add(button);
                    }

                    if (currentStatus.profile.animal != ItemResourceType.NONE)
                    {
                        if (conscriptOptions.AvailableAnimalArmor != null)
                        {
                            content.newParagraph();
                            HudLib.Label(content, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_MountArmorTitle));
                            content.newLine();

                            foreach (var item in conscriptOptions.AvailableAnimalArmor)
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
                                new RbTooltip(armorTooltip, item)
                                );

                                content.Add(button);
                            }
                        }

                        if (conscriptOptions.AvailableWagons != null)
                        {
                            content.newParagraph();
                            HudLib.Label(content, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Vehicle));
                            content.newLine();

                            foreach (var item in conscriptOptions.AvailableWagons)
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
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_TrainingTitle);
                content.newLine();
                TrainingLevel minLevel;
                TrainingLevel maxLevel = currentStatus.maxTrainingLevel;
                if (city.cityCulture == CityCulture.CrabMentality)
                {
                    maxLevel = TrainingLevel.Basic;
                }

                if (currentStatus.profile.man == ItemResourceType.NobleMen)
                {
                    minLevel = TrainingLevel.Basic;
                    maxLevel = TrainingLevel.Professional;
                }
                else
                {
                    minLevel = TrainingLevel.Minimal;
                }

                if (currentStatus.profile.training < minLevel || currentStatus.profile.training > maxLevel)
                {
                    currentStatus.profile.training = minLevel;
                    //trainingClick(minLevel);
                    set(currentStatus);
                }

                
                for (TrainingLevel training = minLevel; training <= maxLevel; training++)
                {
                    var button = new ArtOption(training == currentStatus.profile.training,new List<AbsRichBoxMember>{
                        new RbImage(LangLib.Training_Icon(training)),
                        new RbText( LangLib.Training(training))
                    }, new RbAction1Arg<TrainingLevel>(trainingClick, training, RbSoundType.Option),
                    new RbTooltip(trainingTooltip, new TrainingTooltipArgs() { training = training, buildtype = currentStatus.type, soldierCount = unitCount.TotalMen}));
                    
                    content.Add(button);
                }

                if (advanced && !guardTab)
                {
                    content.newParagraph();

                    HudLib.Label(content, DssRef.lang.Conscript_SpecializationTitle);
                    //content.space();
                    //HudLib.InfoButton(content, new RbTooltip_Text(string.Format(DssRef.lang.Conscript_SpecializationDescription, TextLib.PercentTextWithSymbol(DssConst.Conscript_SpecializePercentage))));
                    content.newLine();

                    SpecializationType[] specializationTypes = currentStatus.profile.avaialableSpecializations( BuildAndExpandType.NUM_NONE, out _);


                    foreach (var specialization in specializationTypes)
                    {
                        IconName.SpecializationTypeName(specialization, out var specIcon, out string specName);
                        var button = new ArtOption(specialization == currentStatus.profile.specialization, new List<AbsRichBoxMember>{
                            new RbImage(specIcon, 0.8f),
                            //new RbSpace(0.5f),
                            //new RbText(specText)
                        }, 
                        new RbAction1Arg<SpecializationType>(specializationClick, specialization, RbSoundType.Option),
                        new RbTooltip(specializationToolTip, specialization));
                        
                        content.Add(button);
                    }
                }

                content.newParagraph();
                HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Cost);
                content.newLine();
                RichBoxContent costButtonContent = new RichBoxContent();
                resourcesToMenu(costButtonContent, city, currentStatus, true);
                content.Add(new ArtButton(RbButtonStyle.Outline, costButtonContent, null, new RbTooltip((RichBoxContent tooltipContent, object tag) =>
                {
                    resourcesToMenu(tooltipContent, city, currentStatus, false);
                })));
               
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
                HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Gain);
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.HoverArea, resultContent(currentStatus, false), null, new RbTooltip(resultTooltip)));

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
                            //string caption;
                            //SpriteName icon;
                            //if (currentProfile.profile.specialization == SpecializationType.CityGuard)
                            //{
                            //    icon = SpriteName.WarsGuard;
                            //    caption = DssRef.lang.Conscript_Soldiers_GuardType;
                            //}
                            //else
                            //{
                            //    icon = new SoldierConscriptProfile() { conscript = currentProfile.profile }.Icon();
                            //    caption = DssRef.lang.Conscript_Soldiers_ArmyType;
                            //}

                            //IconName.Item(currentProfile.profile.weapon, out SpriteName weaponicon, out _);
                            //IconName.Item(currentProfile.profile.armorLevel, out SpriteName armoricon, out _);

                            //RichBoxContent buttonContent = new RichBoxContent();
                            RichBoxContent buttonContent = resultContent(currentProfile, true);
                            buttonContent.newLine();
                            buttonContent.Add(new RbText(currentProfile.shortActiveString(), HudLib.InfoYellow_Dark));
                            //currentProfile.profile.toHud(content, true);

                            content.Add(new ArtButton(RbButtonStyle.Primary,
                            //new List<AbsRichBoxMember>(){
                            //new RbImage(icon),
                            //new RbSpace(),
                            //new RbText(caption, HudLib.TitleColor_Label_Dark),
                            //new RbSpace(),
                            //new RbImage(LangLib.Training_Icon(currentProfile.profile.training)),
                            //new RbImage(weaponicon),
                            //new RbImage(armoricon),

                            //    new RbNewLine(),
                            //     new RbText(currentProfile.shortActiveString(), HudLib.InfoYellow_Dark),
                            //}, 
                            buttonContent,
                            new RbAction1Arg<int>(selectClick, i, RbSoundType.Default)));

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
                   // int menCostProgress = currentStatus.menNeeded;
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

                    progressPoint(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingEquipment, currentStatus.unitsCollected, out bool gotEquipment), currentStatus.active > ConscriptActiveStatus.CollectingEquipment, gotEquipment);
                    //progressPoint(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingMen, menCostProgress, out bool gotMen), currentStatus.active > ConscriptActiveStatus.CollectingMen, gotMen);

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

            content.Add(new RbSeperationLine());
            content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn, HudLib.TitleColor_Head2);
            city.SettlerBp().listResources(content, city);
        }

        void specializationToolTip(RichBoxContent content, object tag)
        {
            SpecializationType specialization = (SpecializationType)tag;
            IconName.SpecializationTypeName(specialization, out var specIcon, out string specName);

            content.h1(specIcon, specName, HudLib.TitleColor_Head);

            content.newParagraph();
            content.text(string.Format( DssRef.lang.Conscript_SpecializationDescription, TextLib.PercentTextWithSymbol(DssConst.Conscript_SpecializePercentage)), HudLib.InfoYellow_Light);
        }

        public static void resourcesToMenu(RichBoxContent content, City city, BarracksStatus currentStatus, bool compact)
        {
            ConscriptUnitCount unitCount = new ConscriptUnitCount(currentStatus.profile);
            currentStatus.unitsNeeded = unitCount.groupUnitCount;
            //currentStatus.payItems(city, CommitOption.Preview, out int totalMen);
            if (!compact)
            {
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Head2);
            }
            resource(content, currentStatus.profile.man, unitCount.menPerUnit);
            resource(content, currentStatus.profile.weapon, unitCount.weaponsPerUnit);
            resource(content, currentStatus.profile.shield, unitCount.menPerUnit);
            resource(content, currentStatus.profile.armorLevel, unitCount.menPerUnit);
            resource(content, currentStatus.profile.animal, unitCount.animalsPerUnit);
            resource(content, currentStatus.profile.mountArmor, unitCount.animalsPerUnit);
            resource(content, currentStatus.profile.vehicle, unitCount.vehiclesPerUnit);
            
            if (currentStatus.profile.specialization == SpecializationType.CityGuard)
            {
                //content.newParagraph();
                //content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement, HudLib.TitleColor_Label);
                int totalMen = currentStatus.unitsNeeded * unitCount.menPerUnit;
                content.newLine();
                //HudLib.BulletPoint(content);
                bool available = totalMen <= city.AvailableGuardHousing();
                content.Add(new RbImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
                content.space();
                HudLib.ResourceCost(content, SpriteName.WarsBuild_GuardOffice, DssRef.lang.GuardHousingCount, totalMen, city.AvailableGuardHousing());
            }

            if (!compact)
            {
                content.newParagraph();
                content.h2(DssRef.lang.Hud_Upkeep, HudLib.TitleColor_Head2);
                float goldUpkeep = Money.ToGoldF( unitCount.TotalMen * currentStatus.profile.copperUpkeepPerSoldier());
                HudLib.LabelAndText(content, SpriteName.rtsUpkeep, TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XUpkeep, DssRef.lang.ResourceType_Gold)), TextLib.TwoDecimal(goldUpkeep));
                float foodUpkeep = unitCount.TotalMen * DssRef.difficulty.manFoodUpkeep;
                if (currentStatus.profile.animal != ItemResourceType.NONE)
                {
                    foodUpkeep += ItemPropertyColl.Get(currentStatus.profile.animal).soldierData.animalFoodUpkeep(unitCount.groupUnitCount);
                }
                HudLib.LabelAndText(content, SpriteName.WarsResource_FoodSub, TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XUpkeep, DssRef.lang.Resource_TypeName_Food)), TextLib.TwoDecimal(foodUpkeep));
                content.text(DssRef.lang.Hud_Time_ValuePerSecond, HudLib.InfoYellow_Light);
            }

            void resource(RichBoxContent content, ItemResourceType resource, int perUnitCount)
            {
                if (resource != ItemResourceType.NONE && perUnitCount > 0)
                {
                    if (compact)
                    {
                        HudLib.BulletSeperationPoint(content);
                    }
                    else
                    {
                        content.newLine();
                    }
                    var resourceGroup = city.GetGroupedResource(resource);
                    int needCount = currentStatus.unitsNeeded * perUnitCount;
                    bool available = resourceGroup.amount >= needCount;
                    IconName.Item(resource, out SpriteName icon, out string name);

                    content.Add(new RbImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
                    content.hspace();

                    if (compact)
                    {                        
                        content.Add(new RbText(needCount.ToString(), HudLib.ResourceCostColor(available)));
                        content.hspace();
                        content.Add(new RbImage(icon));
                        
                    }
                    else
                    {   
                        if (icon != SpriteName.NO_IMAGE)
                        {
                            content.Add(new RbImage(icon));
                            content.space(0.5f);
                        }

                        string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                            TextLib.LargeFirstLetter(name), needCount.ToString(), TextLib.LargeNumber(resourceGroup.amount));

                        content.Add(new RbText(text, HudLib.ResourceCostColor(available)));
                    }
                }
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
            if (guard)
            {
                currentProfile.profile.animal = ItemResourceType.NONE;
            }
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

        RichBoxContent resultContent(BarracksStatus profile, bool darkText)
        {
            RichBoxContent content = new RichBoxContent();
            //BarracksStatus profile = get();
            var conscriptPreview = new SoldierConscriptProfile() { conscript = profile.profile };
            var soldierPreview = conscriptPreview.createSoldierData();
            int count = soldierPreview.UnitCount();
            content.Add(new RbText(count.ToString(), null, LoadedFont.Bold));
            content.space();
            content.Add(new RbImage(AllUnits.UnitFilterIcon(conscriptPreview.filterType())));
            content.hspace();
            content.Add(new RbText(profile.profile.TypeName(), darkText? HudLib.TitleColor_TypeName_Dark : HudLib.TitleColor_TypeName));

            content.space();

            profile.profile.toHud(content, true);
            return content;
        }

        void resultTooltip(RichBoxContent content, object tag)
        {
            BarracksStatus currentProfile = get();

            var conscriptPreview = new SoldierConscriptProfile() { conscript = currentProfile.profile };
            var soldierPreview = conscriptPreview.createSoldierData();

            int count = soldierPreview.UnitCount();
            HudLib.LabelAndText(content, AllUnits.UnitFilterIcon(conscriptPreview.filterType()),
                DssRef.lang.SoldierStats_UnitCount, count.ToString());

            float strengthValue = AllUnits.GroupStrengh(count, ref soldierPreview, true);
            HudLib.LabelAndText(content, SpriteName.WarsStrengthIcon, DssRef.lang.Hud_StrengthRating, TextLib.OneDecimal(strengthValue));
            //content.newLine();
            //content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            //content.hspace();
            //content.Add(new RbText(TextLib.OneDecimal(strengthValue)));

            content.newParagraph();
            content.h2(DssRef.lang.SoldierStats_Title, HudLib.TitleColor_Head2);
            soldierPreview.StatsToHud(content);
            //currentProfile.profile.toHud(content, false);
        }

        void weaponTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType weapon = (ItemResourceType)tag;
                        
            var data = new SoldierConscriptProfile() { conscript = new ConscriptProfile() { weapon = weapon } }.createSoldierData();

            IconName.Item(weapon, out SpriteName weaponicon, out string weaponname);
            content.h1(weaponname, HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new RbImage(SpriteName.warsArmyTag_Hit));
            content.space();
            content.Add(new RbText(TextLib.LabelColon(DssRef.lang.Conscript_WeaponDamage), HudLib.TitleColor_Label));
            content.hspace();
            content.Add(new RbText(ConscriptProfile.WeaponDamage(weapon, out int splashCount).ToString()));

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
                    content.Add(new RbText(TextLib.LabelColon(DssRef.lang.Conscript_ArmorHealth), HudLib.TitleColor_Label));
                    content.hspace();
                    content.Add(new RbText(TextLib.PlusMinus(DssConst.WeaponHealthAdd_Handspear)));
                    break;
            }

            if (data.blockReducingAttack_Inv < 1f)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.Conscript_BlockReducingAttack));
            }
            content.newLine();
            content.Add(new RbImage(SpriteName.cmdParry));
            content.space();
            content.Add(new RbText(string.Format( DssRef.lang.Conscript_BlockPerSecond, TextLib.OneDecimal(1f/ data.blocksRefillTimeSec))));
            content.newLine();
            content.Add(new RbText(DssRef.lang.Conscript_BlockDescription, HudLib.InfoYellow_Light));

            content.newParagraph();
            content.Add(new RbSeperationLine() { thick = true });
            ResourceLib.FullResourceInfo(player.faction, city, weapon, content); 
            //var res = city.GetGroupedResource(weapon);

            //content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;
            //bool reachedBuffer = false;
            //res.toMenu(content, weapon, false, ref reachedBuffer);
        }

        struct ManTooltipArgs
        {
            public ItemResourceType item;
            public int count;
        }

        void manTooltip(RichBoxContent content, object tag)
        {
            ManTooltipArgs args = (ManTooltipArgs)tag;

            float skillBonus = args.item == ItemResourceType.NobleMen ? DssConst.NobelMenSkillBonusAdd : 0;

            //HudLib.LabelAndText(content, SpriteName.cmdStatsHealth, DssRef.lang.SoldierStats_Health, TextLib.TwoDecimal(DssConst.Soldier_DefaultHealth));
            HudLib.LabelAndText(content, SpriteName.WarsMobilityIcon, DssRef.lang.Conscript_Mobility, TextLib.TwoDecimal(SoldierData.Mobility(DssConst.Men_StandardWalkingSpeed)));

            SkillbonusUi(content, skillBonus, true);

            content.newParagraph();
            IconName.Item(args.item, out var itemIcon, out var itemName);
            content.h2(string.Format(DssRef.lang.Language_ItemCount, TextLib.LargeFirstLetter( itemName), args.count), HudLib.TitleColor_Head2);
            if (args.item == ItemResourceType.NobleMen)
            {
                HudLib.LabelAndText(content, SpriteName.rtsUpkeepTime, string.Format( DssRef.lang.Language_XUpkeep, DssRef.lang.ResourceType_Gold), TextLib.PlusMinus(Money.ToGoldF(DssConst.NobleHouseMenCount * args.count)));
            }
            HudLib.LabelAndText(content, SpriteName.WarsResource_FoodSub, TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XUpkeep, DssRef.lang.Resource_TypeName_Food)), TextLib.TwoDecimal(args.count * DssRef.difficulty.manFoodUpkeep));
            content.text(DssRef.lang.Hud_Time_ValuePerSecond, HudLib.InfoYellow_Light);

            content.newParagraph();
            content.Add(new RbSeperationLine() { thick = true });

            ResourceLib.FullResourceInfo(player.faction, city, args.item, content);
        }

        void shieldTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType item = (ItemResourceType)tag;
            if (item != ItemResourceType.NONE)
            {
                IconName.Item(item, out SpriteName icon, out string name);
                content.h1(TextLib.LargeFirstLetter( name), HudLib.TitleColor_Head);

                DssVar.Shields[item].ToHud(content);

                content.newParagraph();
                content.Add(new RbSeperationLine() { thick = true });
            }
            ResourceLib.FullResourceInfo(player.faction, city, item, content);
        }
        void animalTooltip(RichBoxContent content, object tag)
        {
            ManTooltipArgs args = (ManTooltipArgs)tag;

            IconName.Item(args.item, out SpriteName icon, out string  name);
            content.h1(TextLib.LargeFirstLetter(name), HudLib.TitleColor_Head);
            content.newLine();
            var properties = ItemPropertyColl.Get(args.item);

            HudLib.LabelAndText(content, SpriteName.WarsResource_Sword, DssRef.lang.Conscript_WeaponDamage, TextLib.PlusMinus(properties.soldierData.attackDamage));
            HudLib.LabelAndText(content, SpriteName.warsArmyTag_Shield, DssRef.lang.Conscript_ArmorHealth, TextLib.PlusMinus(properties.soldierData.basehealth));
            HudLib.LabelAndText(content, SpriteName.WarsMobilityIcon, DssRef.lang.Conscript_RiderMobility, TextLib.TwoDecimal( properties.soldierData.mobilityValue()));
            HudLib.LabelAndText(content, SpriteName.WarsResource_Wagon2Wheel, DssRef.lang.Conscript_LightWagonMobility, TextLib.TwoDecimal(SoldierData.Mobility( properties.soldierData.lightWagonSpeed)));
            HudLib.LabelAndText(content, SpriteName.WarsResource_WagonSteel, DssRef.lang.Conscript_HeavyWagonMobility, TextLib.TwoDecimal(SoldierData.Mobility(properties.soldierData.heavyWagonSpeed)));
            HudLib.LabelAndText(content, SpriteName.NO_IMAGE, DssRef.lang.Conscript_TrainingTime, "+" + new TimeLength(DssConst.TrainingTimeSec_Mount).LongString());

            content.newParagraph();
            content.h2(DssRef.lang.Hud_Upkeep, HudLib.TitleColor_Head2);
            HudLib.LabelAndText(content, SpriteName.WarsResource_FoodSub, TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XUpkeep, DssRef.lang.Resource_TypeName_Food)), TextLib.TwoDecimal(properties.soldierData.animalFoodUpkeep(args.count)));
            content.text(DssRef.lang.Hud_Time_ValuePerSecond, HudLib.InfoYellow_Dark);
            
            content.newParagraph();
            content.Add(new RbSeperationLine() { thick = true });

            ResourceLib.FullResourceInfo(player.faction, city, args.item, content);
        }
        //void mountArmorTooltip(RichBoxContent content, object tag)
        //{
        //    ItemResourceType item = (ItemResourceType)tag;

        //    ResourceLib.FullResourceInfo(player.faction, city, item, content);
        //}
        void vehicleTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType item = (ItemResourceType)tag;

            IconName.Item(item, out SpriteName icon, out string name);
            content.h1(TextLib.LargeFirstLetter(name), HudLib.TitleColor_Head);
            content.newLine();
            var properties = ItemPropertyColl.Get(item);

            HudLib.LabelAndText(content, SpriteName.WarsResource_Sword, DssRef.lang.Conscript_WeaponDamage, TextLib.PlusMinus(properties.soldierData.attackDamage));
            HudLib.LabelAndText(content, SpriteName.warsArmyTag_Shield, DssRef.lang.Conscript_ArmorHealth, TextLib.PlusMinus(properties.soldierData.basehealth));

            content.newParagraph();
            content.Add(new RbSeperationLine() { thick = true });

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

            HudLib.LabelAndText(content, SpriteName.warsArmyTag_Shield, DssRef.lang.Conscript_ArmorHealth, ConscriptProfile.ArmorHealth(armor).ToString());
            //content.Add(new RbImage(SpriteName.warsArmyTag_Shield));
            //content.Add(new RbSpace());
            //content.Add(new RbText(string.Format(DssRef.lang.Conscript_ArmorHealth, ConscriptProfile.ArmorHealth(armor))));

            if (armor != ItemResourceType.NONE)
            {
                content.newParagraph();
                content.Add(new RbSeperationLine() { thick = true });
                ResourceLib.FullResourceInfo(player.faction, city, armor, content);
                //content.newParagraph();
                //content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;

                //bool reachedBuffer = false;
                //city.GetGroupedResource(armor).toMenu(content, armor, ref reachedBuffer);

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
            public int soldierCount;
        }
        void trainingTooltip(RichBoxContent content, object tag)
        {
            TrainingTooltipArgs args = (TrainingTooltipArgs)tag;

            HudLib.LabelAndText(content, SpriteName.NO_IMAGE, DssRef.lang.Conscript_TrainingTime, new TimeLength(ConscriptProfile.TrainingTime(args.training, ItemResourceType.NONE, args.buildtype)).LongString());
            HudLib.LabelAndText(content, SpriteName.NO_IMAGE, DssRef.lang.Conscript_AttackSpeed, TextLib.PercentTextWithSymbol(ConscriptProfile.TrainingAttackSpeed(args.training)));

            HudLib.LabelAndText(content, SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Language_XUpkeep, DssRef.lang.ResourceType_Gold), TextLib.TwoDecimal(Money.ToGoldF( DssConst.TrainingCopperUpkeep[(int)args.training] * args.soldierCount)));
            content.text(DssRef.lang.Hud_Time_ValuePerSecond, HudLib.InfoYellow_Light);
        }
        void queClick(int length)
        {
            BarracksStatus currentStatus = get();
            currentStatus.que = length;
            set(currentStatus, false);
        }

        void selectClick(int index)
        {
            city.selectedConscript = index;
        }

        BarracksStatus get()
        {
            return city.conscriptBuildings[city.selectedConscript];
        }

        void set(BarracksStatus profile, bool triggerChange = true)
        {
            //var spec = profile.profile.avaialableSpecializations(profile.type, out bool mayGuard);

            //if ((profile.profile.specialization == SpecializationType.CityGuard && !mayGuard)
            //    ||
            //    !spec.Contains(profile.profile.specialization))
            //{
            //    profile.profile.specialization = spec[0];
            //}
            profile.checkSpecialization();

            city.conscriptBuildings[city.selectedConscript] = profile;

            if (triggerChange)
            {
                city.onConscriptChange();
            }
        }

        public static void SkillbonusUi(RichBoxContent content, float skillBonus, bool add)
        {
            //HudLib.LabelAndText(content, SpriteName.WarsStrengthIcon, DssRef.lang.Conscript_SkillBonus, TextLib.PercentAddText(skillBonus));

            content.newLine();
            content.Add(new RbBeginTitle());
            content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            content.space();
            content.Add(new RbText(DssRef.lang.Conscript_SkillBonus + ":", HudLib.TitleColor_Label));
            content.hspace();

            bool positive;
            if (add)
            {
                positive = skillBonus >= 0;
                content.Add(new RbText(TextLib.PercentAddText(skillBonus)));
            }
            else
            {
                positive = skillBonus >= 1;
                content.Add(new RbText(TextLib.PercentText(skillBonus)));
            }

            content.newLine();            
            content.Add(new RbImage(SpriteName.WarsAttackSpeedIcon));
            content.Add(new RbText(DssRef.lang.Conscript_AttackSpeed + ":", HudLib.InfoYellow_Dark));
            content.hspace();
            if (add)
            {
                content.Add(new RbText(TextLib.PercentAddText(skillBonus), HudLib.InfoYellow_Light));
            }
            else
            {
                content.Add(new RbText(TextLib.PercentText(skillBonus), HudLib.InfoYellow_Light));
            }

            content.newLine();
            content.Add(new RbImage(SpriteName.warsArmyTag_Shield));
            content.Add(new RbText(DssRef.lang.Conscript_ArmorHealth + ":", HudLib.InfoYellow_Dark));
            content.hspace();
            if (add)
            {
                content.Add(new RbText(TextLib.PercentAddText(skillBonus), HudLib.InfoYellow_Light));
            }
            else
            {
                content.Add(new RbText(TextLib.PercentText(skillBonus), HudLib.InfoYellow_Light));
            }
        }
    }
}
