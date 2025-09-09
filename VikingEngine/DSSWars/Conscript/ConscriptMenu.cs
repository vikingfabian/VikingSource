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
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Interface.Component;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.DSSWars.Conscript
{
    class ConscriptMenu
    {
        static readonly ItemResourceType[] SoldierWeapons = {
            ItemResourceType.SharpStick,
            ItemResourceType.BronzeSword,
            ItemResourceType.ShortSword,
            ItemResourceType.Sword,
            ItemResourceType.LongSword,
            ItemResourceType.HandSpear,
        };

        static readonly ItemResourceType[] ArcherWeapons = {
            ItemResourceType.SlingShot,
            ItemResourceType.ThrowingSpear,
            ItemResourceType.Bow,
            ItemResourceType.LongBow,
            ItemResourceType.Crossbow,
        };

        static readonly ItemResourceType[] ArcherGuardWeapons = {
            ItemResourceType.Stone_G,
            ItemResourceType.ThrowingSpear,
            ItemResourceType.Bow,
            ItemResourceType.LongBow,
            ItemResourceType.Crossbow,
        };

        static readonly ItemResourceType[] WarmachineWeapons = {
           
            ItemResourceType.Ballista,
            ItemResourceType.Manuballista,
            ItemResourceType.Catapult,
        };

        static readonly ItemResourceType[] NobelWeapons = {
            ItemResourceType.Warhammer,
            ItemResourceType.TwoHandSword,
            ItemResourceType.KnightsLance,
            ItemResourceType.MithrilSword,
            ItemResourceType.MithrilBow,
        };

        static readonly ItemResourceType[] GunWeapons = {
            ItemResourceType.HandCannon,
            ItemResourceType.HandCulverin,
            ItemResourceType.Rifle,
            ItemResourceType.Blunderbuss,
        };

        static readonly ItemResourceType[] CannonWeapons = {
           ItemResourceType.SiegeCannonBronze,
            ItemResourceType.ManCannonBronze,
            ItemResourceType.SiegeCannonIron,
            ItemResourceType.ManCannonIron,
        };

        public static List<ItemResourceType[]> AllConstriptWeapons()
        {
            return new List<ItemResourceType[]>
            {
                SoldierWeapons,
                ArcherWeapons,
                WarmachineWeapons,
                NobelWeapons,
                GunWeapons,
                CannonWeapons,
            };
        }
        public static List<ItemResourceType[]> AllHandWeapons()
        {
            return new List<ItemResourceType[]>
            {
                SoldierWeapons,
                ArcherWeapons,
                NobelWeapons,
                GunWeapons,
            };
        }

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
                BarracksStatus currentStatus = get();
                int menCostNext = currentStatus.profile.menCost();
                SpriteName icon =  new SoldierConscriptProfile() { conscript = currentStatus.profile }.Icon();
                //content.Add(new RbImage(
                            
                //            ));
                //content.space();
                //content.Add(new RbBeginTitle(1));

                string typeName = null; 
                ItemResourceType[] weapons = null;
                bool hasGuardOption = true;
                switch (currentStatus.type)
                {
                    case Build.BuildAndExpandType.SoldierBarracks:
                        typeName = DssRef.lang.BuildingType_SoldierBarracks;
                        weapons = SoldierWeapons;
                        break;
                    case Build.BuildAndExpandType.ArcherBarracks:
                        typeName = DssRef.lang.BuildingType_SoldierBarracks;
                        weapons = ArcherWeapons;
                        break;
                    case Build.BuildAndExpandType.WarmachineBarracks:
                        typeName = DssRef.lang.BuildingType_SoldierBarracks;
                        weapons = WarmachineWeapons;
                        break;
                    case Build.BuildAndExpandType.KnightsBarracks:
                        hasGuardOption = false;
                        typeName = DssRef.lang.BuildingType_SoldierBarracks;
                        weapons = NobelWeapons;
                        break;
                    case Build.BuildAndExpandType.GunBarracks:
                        typeName = DssRef.lang.BuildingType_SoldierBarracks;
                        weapons = GunWeapons;
                        break;
                    case Build.BuildAndExpandType.CannonBarracks:
                        typeName = DssRef.lang.BuildingType_CannonBarracks;
                        weapons = CannonWeapons;
                        break;
                }


                //var title = new RbText(typeName + " " + currentStatus.idAndPosition.ToString());
                //title.overrideColor = HudLib.TitleColor_TypeName;
                //content.Add(title);

                //content.space();
                //HudLib.CloseButton(content, new RbAction(() => { city.selectedConscript = -1; }, RbSoundType.Back));
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

                content.newParagraph();
                HudLib.Label(content, DssRef.lang.Conscript_WeaponTitle);
                content.newLine();
                
                //for (MainWeapon weapon = 0; weapon < MainWeapon.NUM; weapon++)
                foreach (var weapon in weapons)
                {
                    //ItemResourceType item = ConscriptProfile.WeaponItem(weapon);
                    var buttonContent = new List<AbsRichBoxMember>(3) {
                        new RbImage(ResourceLib.Icon(weapon)),
                       //new RbText( LangLib.Item(weapon))
                    };

                    if (city.GetGroupedResource(weapon).amount >= menCostNext)
                    {
                        buttonContent.Insert(0, new RbImage(SpriteName.warsResourceChunkAvailable));
                    }

                    var button = new ArtOption(weapon == currentStatus.profile.weapon,buttonContent,
                    new RbAction1Arg<ItemResourceType>(weaponClick, weapon, RbSoundType.Option),
                    new RbTooltip(weaponTooltip, weapon)
                    );
                    //button.setGroupSelectionColor(HudLib.RbSettings, weapon == currentStatus.profile.weapon);
                    content.Add(button);
                    //content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_ArmorTitle);
                content.newLine();

                List<ItemResourceType> armorOptions = new List<ItemResourceType>
                {
                    ItemResourceType.NONE,
                    ItemResourceType.PaddedArmor,
                    ItemResourceType.HeavyPaddedArmor,
                    ItemResourceType.BronzeArmor,
                    ItemResourceType.IronArmor,
                    ItemResourceType.HeavyIronArmor,
                    ItemResourceType.LightPlateArmor,
                    ItemResourceType.FullPlateArmor,
                    ItemResourceType.MithrilArmor,
                };



                //for (ArmorLevel armorLvl = 0; armorLvl < ArmorLevel.NUM; armorLvl++)
                foreach ( var armorLvl in armorOptions )
                {
                    var buttonContent = new List<AbsRichBoxMember>(3);
                    //ItemResourceType item = ConscriptProfile.ArmorItem(armorLvl);

                    if (city.GetGroupedResource(armorLvl).amount >= menCostNext)
                    {
                        buttonContent.Add(new RbImage(SpriteName.warsResourceChunkAvailable));
                    }
                    if (armorLvl != ItemResourceType.NONE)
                    {
                        buttonContent.Add(new RbImage(ResourceLib.Icon(armorLvl)));
                    }
                    //buttonContent.Add(new RbText(LangLib.Item(armorLvl)));

                    var button = new ArtOption(armorLvl == currentStatus.profile.armorLevel,buttonContent,
                        new RbAction1Arg<ItemResourceType>(armorClick, armorLvl, RbSoundType.Option),
                    new RbTooltip(armorTooltip, armorLvl));
                    //button.setGroupSelectionColor(HudLib.RbSettings, armorLvl == currentStatus.profile.armorLevel);
                    content.Add(button);
                    //content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_TrainingTitle);
                content.newLine();
                TrainingLevel minLevel = currentStatus.type == Build.BuildAndExpandType.KnightsBarracks ? TrainingLevel.Basic : TrainingLevel.Minimal;

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

                if (!guardTab)
                {
                    content.newParagraph();

                    HudLib.Label(content, DssRef.lang.Conscript_SpecializationTitle);
                    content.space();
                    HudLib.InfoButton(content, new RbTooltip_Text(string.Format(DssRef.lang.Conscript_SpecializationDescription, TextLib.PercentText(DssConst.Conscript_SpecializePercentage))));
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
                        //button.setGroupSelectionColor(HudLib.RbSettings, specialization == currentStatus.profile.specialization);
                        content.Add(button);
                    }
                }
                content.newParagraph();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Label);

                resourcesToMenu(content, city, currentStatus);
                //content.newLine();
                //HudLib.BulletPoint(content);
                //HudLib.ResourceCost(content, ResourceType.Worker, menCostNext, city.workForce.amount);

                //content.newLine();
                //HudLib.BulletPoint(content);
                ////var weaponItem = ConscriptProfile.WeaponItem(currentStatus.profile.weapon);
                //var weaponRes = city.GetGroupedResource(currentStatus.profile.weapon);
                //HudLib.ResourceCost(content, currentStatus.profile.weapon, menCostNext, weaponRes.amount);

                //if (currentStatus.profile.armorLevel != ItemResourceType.NONE)
                //{
                //    content.newLine();
                //    HudLib.BulletPoint(content);
                //    //var armorItem = ConscriptProfile.ArmorItem(currentStatus.profile.armorLevel);
                //    var armorRes = city.GetGroupedResource(currentStatus.profile.armorLevel);
                //    HudLib.ResourceCost(content, currentStatus.profile.armorLevel, menCostNext, armorRes.amount);
                //}

                //if (guardTab)
                //{
                //    //content.newParagraph();
                //    //content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement, HudLib.TitleColor_Label);

                //    content.newLine();
                //    HudLib.BulletPoint(content);
                //    HudLib.ResourceCost(content, SpriteName.WarsBuild_GuardOffice, DssRef.lang.GuardHousingCount, menCostNext, city.AvailableGuardHousing());
                //}

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
                content.Add(new RbImage(player.gameControls.input.Copy.Icon));
                content.hspace();
                content.Add(new ArtButton( RbButtonStyle.Primary,new List<AbsRichBoxMember> {                    
                    new RbText(DssRef.lang.Hud_CopySetup) },
                    new RbAction1Arg<LocalPlayer>(city.copyConscript, player, RbSoundType.Copy)));

                content.space();
                content.Add(new RbImage(player.gameControls.input.Paste.Icon));
                content.hspace();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {                   
                    new RbText(DssRef.lang.Hud_Paste) },
                    new RbAction1Arg<LocalPlayer>(city.pasteConscript, player, RbSoundType.Paste)));

                //if (currentStatus.active != ConscriptActiveStatus.Idle)
                //{
                //    int menCostProgress = currentStatus.menNeeded;

                //    content.Add(new RbSeperationLine());
                //    {
                //        content.newLine();
                //        HudLib.BulletPoint(content);
                //        var text = new RbText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingEquipment, menCostProgress));
                //        text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingEquipment ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                //        content.Add(text);
                //    }
                //    {
                //        content.newLine();
                //        HudLib.BulletPoint(content);
                //        var text = new RbText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingMen, menCostProgress));
                //        text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingMen ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                //        content.Add(text);
                //    }

                //    if (currentStatus.active == ConscriptActiveStatus.Training)
                //    {
                //        content.newLine();
                //        HudLib.BulletPoint(content);
                //        content.Add(new RbText(currentStatus.longTimeProgress()));
                //    }
                //}
            }
            else
            {

                content.h2(DssRef.lang.Conscript_SelectBuilding).overrideColor = HudLib.TitleColor_Action;
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
                    content.newLine();
                    content.text(DssRef.lang.Hud_RequirementOr);
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsBuild_Nobelhouse));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Building_NobleHouse));
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
                            var subTab = new ArtButton(player.conscriptSubTab == filter ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, new List<AbsRichBoxMember> 
                            { 
                                new RbText(filter == BuildAndExpandType.ALL? DssRef.todoLang.Hud_All : LangLib.BuildingName(filter))
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

                            //caption.overrideColor = HudLib.TitleColor_Label_Dark;

                            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>(){
                                new RbImage(icon),
                                new RbSpace(),
                                new RbText(caption, HudLib.TitleColor_Label_Dark),
                                new RbSpace(),
                                new RbImage(LangLib.Training_Icon(currentProfile.profile.training)),
                                new RbImage(ResourceLib.Icon(currentProfile.profile.weapon)),
                                new RbImage(ResourceLib.Icon(currentProfile.profile.armorLevel)),

                                new RbNewLine(),
                                 new RbText(currentProfile.shortActiveString(), HudLib.InfoYellow_Dark),
                            }, new RbAction1Arg<int>(selectClick, i, RbSoundType.Default)));

                        }
                    }

                    //Apply to all options
                    content.h2(DssRef.todoLang.GeneralSetting_SetAll, HudLib.TitleColor_Head2);
                    HudLib.Label(content, DssRef.lang.Hud_ProductionQueue); content.space();
                    que.listToHud(player, content, queueToAll);
                    //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("=0") }, new RbAction1Arg<int>(queueToAll, 0, RbSoundType.Stop)));
                    //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+1") }, new RbAction1Arg<int>(queueToAll, 1, RbSoundType.Start)));
                    //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_NoLimit) }, new RbAction1Arg<int>(queueToAll, ProgressQue.NoLimit, RbSoundType.Start)));

                    if (player.conscriptSubTab != BuildAndExpandType.ALL ||
                        typeCount == 1)
                    {
                        content.newLine();
                        content.Add(new RbImage(player.gameControls.input.Paste.Icon));
                        content.hspace();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbText(DssRef.lang.Hud_Paste) },
                            new RbAction1Arg<LocalPlayer>(city.pasteConscriptToAll, player, RbSoundType.Paste)));
                    }
                }
            }

            void queueToAll(int count)
            {
                for (int i = 0; i < city.conscriptBuildings.Count; ++i)
                {
                    if (player.conscriptSubTab == BuildAndExpandType.ALL ||
                        player.conscriptSubTab == city.conscriptBuildings[i].type)
                    {
                        var status = city.conscriptBuildings[i];
                        if (count == 1)
                        {
                            status.que++;
                        }
                        else
                        {
                            status.que = count;
                        }
                        city.conscriptBuildings[i] = status;
                    }
                }
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


                    //{
                    //    content.newLine();
                    //    HudLib.BulletPoint(content);
                    //    var text = new RbText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingEquipment, menCostProgress));
                    //    text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingEquipment ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    //    content.Add(text);
                    //}
                    //{
                    //    content.newLine();
                    //    HudLib.BulletPoint(content);
                    //    var text = new RbText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingMen, menCostProgress));
                    //    text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingMen ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    //    content.Add(text);
                    //}

                    //if (currentStatus.active == ConscriptActiveStatus.Training)
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
                        //text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingEquipment ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                }
            }
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
                SpriteName icon = ResourceLib.Icon(resource);

                if (icon != SpriteName.NO_IMAGE)
                {
                    content.Add(new RbImage(icon));
                    content.space(0.5f);
                }

                string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                    LangLib.Item(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

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
            ConscriptProfile defaultProfile = new ConscriptProfile();
            defaultProfile.defaultSetup(currentProfile.type);
            currentProfile.profile.specialization = guard? SpecializationType.CityGuard : defaultProfile.specialization;
            set(currentProfile);
        }

        void specializationClick(SpecializationType specialization)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.specialization = specialization;
            set(currentProfile);

        }

        void weaponClick(ItemResourceType weapon)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.weapon = weapon;
            set(currentProfile);
        }

        void weaponTooltip(RichBoxContent content, object tag)
        {
            ItemResourceType weapon = (ItemResourceType)tag;

            
            var data = new SoldierConscriptProfile() { conscript = new ConscriptProfile() { weapon = weapon } }.init();

            content.h1(LangLib.Item(weapon), HudLib.TitleColor_Head);
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
            var res = city.GetGroupedResource(weapon);

            content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;
            bool reachedBuffer = false;
            res.toMenu(content, weapon, false, ref reachedBuffer);
            
           
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

            content.h1(LangLib.Item(armor), HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new RbImage(SpriteName.warsArmyTag_Shield));
            content.Add(new RbSpace());
            content.Add(new RbText(string.Format(DssRef.lang.Conscript_ArmorHealth, ConscriptProfile.ArmorHealth(armor))));

            if (armor != ItemResourceType.NONE)
            {
                content.newParagraph();
                content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;

                bool reachedBuffer = false;
                city.GetGroupedResource(armor).toMenu(content, armor, false, ref reachedBuffer);
               
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
