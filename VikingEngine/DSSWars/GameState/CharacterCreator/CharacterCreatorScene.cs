using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.DSSWars.GameState.CharacterCreator
{
    class CharacterCreatorScene : AbsDssState
    {
        public const float MinScale = 0.6f;
        public const float MaxScale = 1.4f;

        const float DefaultIconScale = 0.8f;

        const string Page_Accessory = "accessories";
        public RichMenu menu, optMenu;
        CharacterPreview soldierPreview/*, animalPreview*/;

        bool bAutoAnimate = false;
        ListCirkleCounter<FrameTime> autoAnimateCounter;
        Time autoAnimateTimer = new Time(2000);
        List<FrameTime> autoAnimate;
        EditorBackground bg;
        public CharacterCreatorScene()
            : base()
        {
            bg = new Interface.EditorBackground();
            openMenu();

            

            float backWidth = Engine.Screen.SafeArea.Width - menu.backgroundArea.Width;
            Vector2 previewSz = new Vector2(backWidth * 0.4f);
            Vector2 pos = new Vector2(menu.backgroundArea.Right + Engine.Screen.IconSize, Engine.Screen.SafeArea.Center.Y - previewSz.Y * 0.5f);

            VectorRect previewArea = new VectorRect(pos, previewSz);
            soldierPreview = new CharacterPreview(previewArea, CharacterPreviewType.Soldier);

            previewArea.nextAreaX(1, Engine.Screen.IconSize);
            //animalPreview = new CharacterPreview(previewArea, CharacterPreviewType.RideAnimal);

            openOptionsMenu();

            autoAnimate = new List<FrameTime>(64);
            autoAnimate.Add(new FrameTime(0, 1000));
            //Blink
            autoAnimate.Add(new FrameTime(1, 100));
            autoAnimate.Add(new FrameTime(0, 200));
            autoAnimate.Add(new FrameTime(1, 100));
            autoAnimate.Add(new FrameTime(0, 500));

            //Attack
            for (int i = 0; i < 2; ++i)
            {
                autoAnimate.Add(new FrameTime(2, 400));
                autoAnimate.Add(new FrameTime(0, 600));
            }

            //Walk
            for (int i = 0; i < 6; ++i)
            {
                for (int frame = 3; frame < soldierPreview.FrameCount; ++frame)
                {
                    autoAnimate.Add(new FrameTime(frame, 150));
                }
            }

            autoAnimateCounter = new ListCirkleCounter<FrameTime>(autoAnimate, 0);
        }
        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            bool mouseOver = false;

            menu.updateMouseInput(ref mouseOver);

            if (menu.needRefresh)
            {
                refreshPage();
                menu.needRefresh = false;
            }

            optMenu.updateMouseInput(ref mouseOver);

            if (optMenu.needRefresh)
            {
                displayOptionsPage();
                optMenu.needRefresh = false;
            }


            soldierPreview.update();

            if (bAutoAnimate)
            {
                if (autoAnimateTimer.CountDown())
                {
                    autoAnimateCounter.Next();
                    soldierPreview.setFrame(autoAnimateCounter.CurrentValue.frame);
                    autoAnimateTimer = new Time(autoAnimateCounter.CurrentValue.time);
                }
            }
        }
        void refreshPage()
        {
            switch (menu.menuStack.LastOrDefault())
            {
                default:
                    mainMenu();
                    break;
                //case Page_Accessory:
                //    accessoriesPage();
                //    break;
            }
        }

        void openMenu()
        {            
            if (menu == null)
            {

                var objectMenuArea = Screen.SafeArea;
                objectMenuArea.Width = (int)(Engine.Screen.IconSize * 9f);

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
                mainMenu();
            }
        }

        void openOptionsMenu()
        {
            //OPTIONS MENU
            var optionsMenuArea = Screen.SafeArea;
            optionsMenuArea.AddToLeftSide(-menu.backgroundArea.Width);
            optionsMenuArea.AddWidth(-Engine.Screen.IconSize * 2f);

            optMenu = new RichMenu(HudLib.RbSettings, optionsMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
            displayOptionsPage();
            optMenu.updateHeightFromContent();
            optMenu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);
        }

        public void Refresh(RichBoxContent content)
        {
            //openMenu();
            menu.Refresh(content);
        }

        //int faceOption = 0;
        //int bodyOption = 0;
        //float scale = 1f;
        //CharacterCreatorTab tab = 0;

        void displayOptionsPage()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Diplay options", HudLib.TitleColor_Head);

            content.newLine();
            var weapons = ConscriptMenu.AllHandWeapons();
            foreach (var wepArray in weapons)
            {
                foreach (var weapon in wepArray)
                {
                    IconName.Item(weapon, out var weaponIcon, out var weaponName);
                    var button = new ArtOption(soldierPreview.soldierModelData.weapon == weapon, new List<AbsRichBoxMember>()
                        {
                            new RbImage(weaponIcon)
                        },
                    new RbAction1Arg<ItemResourceType>((ItemResourceType weapon) => { soldierPreview.soldierModelData.weapon = weapon; refreshPreview(); }, weapon, RbSoundType.Option)
                    );
                    content.Add(button);
                }
            }

            content.newLine();
            for (ArmorLevel armorLevel = 0; armorLevel < ArmorLevel.NUM; armorLevel++)
            {
                SpriteName armorIcon;
                switch (armorLevel)
                {
                    default://case ArmorLevel.None:
                        armorIcon = CityTag.NoBackSprite;
                        break;
                    case ArmorLevel.Leather:
                        armorIcon = SpriteName.WarsResource_PaddedArmor;
                        break;
                    case ArmorLevel.Iron:
                        armorIcon = SpriteName.WarsResource_IronArmor;
                        break;
                    case ArmorLevel.Steel:
                        armorIcon = SpriteName.WarsResource_FullPlateArmor;
                        break;
                    case ArmorLevel.Masterful:
                        armorIcon = SpriteName.WarsResource_MithrilArmor;
                        break;
                }

                var button = new ArtOption(soldierPreview.soldierModelData.armor == armorLevel, new List<AbsRichBoxMember>()
                        {
                            new RbImage(armorIcon)
                        },
                    new RbAction1Arg<ArmorLevel>((ArmorLevel armor) => { soldierPreview.soldierModelData.armor = armor; refreshPreview(); }, armorLevel, RbSoundType.Option)
                    );
                content.Add(button);
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFramePrevious) }, new RbAction1Arg<bool>(soldierPreview.nextFrame, false)));
            for (int frame = 0; frame < soldierPreview.FrameCount; frame++)
            {
                SpriteName frameicon;
                RbButtonStyle buttonStyle;
                if (frame == soldierPreview.Frame && !bAutoAnimate)
                {
                    buttonStyle = RbButtonStyle.OptionSelected;
                    frameicon = SpriteName.VoxelEditorFrameSelected;
                }
                else
                {
                    buttonStyle = RbButtonStyle.OptionNotSelected;
                    frameicon = SpriteName.VoxelEditorFrame;
                }

                content.Add(new ArtButton(buttonStyle,
                    new List<AbsRichBoxMember> { new RbText(TextLib.IndexToString(frame)), new RbImage(frameicon) }, new RbAction1Arg<int>((int frame)=> {
                        bAutoAnimate = false;
                        soldierPreview.setFrame(frame);  
                    }, frame)));
            }
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFrameNext) }, new RbAction1Arg<bool>(soldierPreview.nextFrame, true)));
            content.Add(new ArtOption(bAutoAnimate, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarPlayIcon) }, new RbAction(() => { bAutoAnimate = true; }, RbSoundType.Option)));

            optMenu.Refresh(content);
        }

        void mainMenu()
        {
           var profile = GetProfile();

            RichBoxContent content = new RichBoxContent();
            content.h1(DssRef.lang.Editor_CharacterCreator, HudLib.TitleColor_Head);
            content.newLine();
            content.text(DssRef.lang.Editor_CharacterCreator_Description, HudLib.InfoYellow_Light);

            
            listAndEditCharacter(content);
            content.newLine();
            listAndEditFlag(content, 1, DssRef.storage.localPlayers.First(), true);

            content.Add(new RbSeperationLine());
            //List<CharacterCreatorTab> availableTabs = new List<CharacterCreatorTab> {
            //    CharacterCreatorTab.Soldiers,
            //    CharacterCreatorTab.Workers,
            //    CharacterCreatorTab.Animals
            //};

            //var tabs = new List<ArtTabMember>(availableTabs.Count);
                        
            //for (int i = 0; i < availableTabs.Count; ++i)
            //{
            //    var text = new RbText(availableTabs[i].ToString());
            //    text.overrideColor = HudLib.RbSettings.tabSelected.Color;

            //    tabs.Add(new ArtTabMember(new List<AbsRichBoxMember>
            //    { text }));
            //}
            //var tabGroup = new ArtTabgroup(tabs, arraylib.IndexFromValue(availableTabs, tab), 
            //    (int tabIx)=> { tab = availableTabs[tabIx]; }, null, SoundLib.menutab, null);

            //content.Add(tabGroup);

            //content.h2("Default setup", HudLib.TitleColor_TypeName);

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.HUD_Scale);
            content.space();
            RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f }, new DragButtonSettings(MinScale, MaxScale, 0.1f), ScaleProperty);
            content.newParagraph();
            
            DropDownBuilder hatGenreDropdown = new DropDownBuilder("hat genre");
            {
                hatGenreDropdown.AddOption(DssRef.lang.Hud_None, profile.hatGenre == CharacterHatGenre.NoHat,
                    true, new RbAction1Arg<CharacterHatGenre>(setHatGenre, CharacterHatGenre.NoHat), null);
                hatGenreDropdown.AddOption(DssRef.lang.Editor_HatGenre_FollowWeapon, profile.hatGenre == CharacterHatGenre.FollowWeapon,
                    true, new RbAction1Arg<CharacterHatGenre>(setHatGenre, CharacterHatGenre.FollowWeapon), null);
                //hatGenreDropdown.AddOption("Follow armor", profile.hatGenre == CharacterHatGenre.FollowArmor,
                //    false, new RbAction1Arg<CharacterHatGenre>(setHatGenre, CharacterHatGenre.FollowArmor), null);
                hatGenreDropdown.AddOption(DssRef.lang.Editor_HatGenre_Uniform, profile.hatGenre == CharacterHatGenre.Uniform,
                    false, new RbAction1Arg<CharacterHatGenre>(setHatGenre, CharacterHatGenre.Uniform), null);
            }
            hatGenreDropdown.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Editor_HatGenre, menu);

            if (profile.hatGenre == CharacterHatGenre.Uniform)
            {
                content.newLine();
                for (int i = 0; i < DssRef.models.rawModels[VoxelModelName.modsoldier_hat_custom_all].Frames.Count; i++)
                {
                    content.Add(new ArtOption(i == profile.customHat, new List<AbsRichBoxMember> { new RbText(TextLib.AddIndexToString(DssRef.lang.Character_Hat ,i)) },
                        new RbAction1Arg<int>((int hat) => {
                            var profile = GetProfile();
                            {
                                profile.customHat = hat;
                            }
                            SetProfile(profile);

                            refreshPreview();
                        }, i, RbSoundType.Option)));
                }
                //content.newLine();
                //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                //new RbImage(SpriteName.WarsHudIconOpen),
                //new RbSpace(),
                //new RbText("Import model")
                //}, null));
            }

            content.newParagraph();
            for (FaceTheme i = 0; i < FaceTheme.NUM; i++)
            {
                content.Add(new ArtOption(i == profile.face, new List<AbsRichBoxMember> { new RbText(TextLib.AddIndexToString(DssRef.lang.Character_Head,(int)i)) },
                    new RbAction1Arg<FaceTheme>((FaceTheme face)=> {
                        var profile = GetProfile();
                        {
                            profile.face = face;                            
                        }
                        SetProfile(profile);

                        refreshPreview();
                    }, i, RbSoundType.Option)));
            }
            //content.newLine();
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
            //    new RbImage(SpriteName.WarsHudIconOpen),
            //    new RbSpace(),
            //    new RbText("Import model")
            //}, null));

            content.newParagraph();
            for (int i = 0; i < 3; i++)
            {
                content.Add(new ArtOption(i == profile.body, new List<AbsRichBoxMember> { new RbText(TextLib.AddIndexToString(DssRef.lang.Character_Body, i)) },
                    new RbAction1Arg<int>((int body) => {
                        var profile = GetProfile();
                        {
                            profile.body = body;
                        }
                        SetProfile(profile);

                        refreshPreview();
                    }, i, RbSoundType.Option)));
            }
            //content.newLine();
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
            //    new RbImage(SpriteName.WarsHudIconOpen),
            //    new RbSpace(),
            //    new RbText("Import model")
            //}, null));

            content.newParagraph();
            for (ArmsTheme i = 0; i <  ArmsTheme.NUM; i++)
            {
                content.Add(new ArtOption(i == profile.arms, new List<AbsRichBoxMember> { new RbText(TextLib.AddIndexToString(DssRef.lang.Character_Arms, (int)i)) },
                    new RbAction1Arg<ArmsTheme>((ArmsTheme arms) => {
                        var profile = GetProfile();
                        {
                            profile.arms = arms;
                        }
                        SetProfile(profile);

                        refreshPreview();
                    }, i, RbSoundType.Option)));
            }

            content.newParagraph();
            content.h2(DssRef.lang.Character_Accessories, HudLib.TitleColor_Label);

            content.newLine();
            content.Add(new ArtOption(profile.accessoryBack == -1,  new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_None) },
                new RbAction1Arg<int>((int backpack) => {
                    var profile = GetProfile();
                    {
                        profile.accessoryBack = backpack;
                    }
                    SetProfile(profile);

                    refreshPreview();
                }, -1, RbSoundType.Option)));
            for (int i = 0; i < DssRef.models.rawModels[VoxelModelName.modsoldier_addons].Frames.Count; i++)
            {                
                content.Add(new ArtOption(i == profile.accessoryBack, new List<AbsRichBoxMember> { new RbText(TextLib.AddIndexToString(DssRef.lang.Character_Back, i)) },
                    new RbAction1Arg<int>((int back) => {
                        var profile = GetProfile();
                        {
                            profile.accessoryBack = back;
                        }
                        SetProfile(profile);

                        refreshPreview();
                    }, i, RbSoundType.Option)));
            }


            content.newParagraph();


            content.Add(new ArtOption(profile.accessoryFace == -1, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_None) },
                new RbAction1Arg<int>((int faceadd) => {
                    var profile = GetProfile();
                    {
                        profile.accessoryFace = faceadd;
                    }
                    SetProfile(profile);

                    refreshPreview();
                }, -1, RbSoundType.Option)));
            for (int i = 0; i < DssRef.models.rawModels[VoxelModelName.modsoldier_face_access].Frames.Count; i++)
            {
                content.Add(new ArtOption(i == profile.accessoryFace, new List<AbsRichBoxMember> { new RbText(TextLib.AddIndexToString(DssRef.lang.Character_Face, i)) },
                    new RbAction1Arg<int>((int faceadd) => {
                        var profile = GetProfile();
                        {
                            profile.accessoryFace = faceadd;
                        }
                        SetProfile(profile);

                        refreshPreview();
                    }, i, RbSoundType.Option)));
            }
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
            //    new RbImage(SpriteName.pjNumPlus, 1, Color.Green),
            //    new RbSpace(),
            //    new RbText("Add accessory")
            //}, new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Accessory, StackOption.Stack)));
            content.Add(new RbSeperationLine());

            //DssRef.lang.Editor_Settings_BackgroundColor

            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Settings_BackgroundColor) },
                bg.visibleProperty_inv));

            content.newParagraph();
            
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                 new RbImage(SpriteName.WarsHudIconExit, DefaultIconScale), new RbSpace(), new RbText(DssRef.lang.Hud_Exit) },
               new RbAction(()=> { new ExitToLobby(true); })));


            Refresh(content);
        }

        void listAndEditFlag(RichBoxContent content, int playerNum, LocalPlayerStorage playerData, bool editor)
        {
            DropDownBuilder flagOptions = new DropDownBuilder("listflags" + playerNum.ToString());
            {
                for (int i = 0; i < DssRef.storage.flagStorage.flagDesigns.Count; ++i)
                {
                    flagOptions.AddSubOption(DssRef.storage.flagStorage.flagDesigns[i].RbButton(), i == DssRef.storage.flagStorage.selectedIx, false, new RbAction2Arg<int, int>(selectFlagLink, playerNum, i), null);
                }
                flagOptions.menuCaption = DssRef.storage.flagStorage.Selected().RbButton();
                flagOptions.injectAfter = new List<AbsRichBoxMember>() {
                                    new ArtButton(editor? RbButtonStyle.Primary : RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                                        new RbImage(SpriteName.EditorToolPencil) }, new RbAction1Arg<int>(openProfileEditor, DssRef.storage.flagStorage.selectedIx), new RbTooltip_Text(DssRef.lang.Lobby_FlagEdit))
                                };
                flagOptions.Build(content, SpriteName.NO_IMAGE, null, menu);
            }
        }

        void selectFlagLink(int playerNumber, int profile)
        {
            int ix = playerNumber - 1;
            LocalPlayerStorage playerData = DssRef.storage.localPlayers[ix];

            //TODO
            //playerData.flagDesignIndex = profile;
            DssRef.storage.flagStorage.selectedIx = profile;

            DssRef.storage.checkPlayerDoublettes(ix);

            DssRef.storage.Save(null);
            //refreshSplitScreen();

            //underMenu.CloseDropDown();
            refreshPreview();
        }

        void listAndEditCharacter(RichBoxContent content)
        {
            DropDownBuilder flagOptions = new DropDownBuilder("listcharacters");
            {
                for (int i = 0; i < DssRef.storage.characterStorage.profiles.Count; ++i)
                {
                    flagOptions.AddSubOption((DropDownOption)DssRef.storage.characterStorage.profiles[i].RbButton(DssRef.storage.flagStorage.selectedIx, false), i == DssRef.storage.characterStorage.selectedIx, false, new RbAction1Arg<int>(selectCharacterLink, i), null);
                }
                flagOptions.menuCaption = DssRef.storage.characterStorage.Selected().RbButton(DssRef.storage.flagStorage.selectedIx, false);
                
                flagOptions.Build(content, SpriteName.NO_IMAGE, null, menu);
            }
        }

        void selectCharacterLink(int charIx)
        {
            DssRef.storage.characterStorage.SaveSelected();
            DssRef.storage.characterStorage.selectedIx = charIx;
           
            DssRef.storage.Save(null);
            refreshPreview();
        }

        void refreshPreview()
        {
            soldierPreview.characterIndex = DssRef.storage.characterStorage.selectedIx;
            soldierPreview.flagIndex = DssRef.storage.flagStorage.selectedIx;

            soldierPreview.refresh();
        }

        void openProfileEditor(int ProfileIx)
        {
            int p = -1;
            bool bController = Input.XInput.KeyIsDown(Buttons.A, ref p) || Input.XInput.KeyIsDown(Buttons.X, ref p);
            new StartEditor(ProfileIx, bController, 0);
        }

        void setHatGenre(CharacterHatGenre genre)
        {

            var profile = GetProfile();
            {
                profile.hatGenre = genre;
            }
            SetProfile(profile);
            
            refreshPreview();
            menu.CloseDropDown();
        }

        //void accessoriesPage()
        //{
        //    var profile = DssRef.storage.GetHostProfile();

        //    RichBoxContent content = new RichBoxContent();
        //    content.h1("Add accessory", HudLib.TitleColor_Head);

        //    content.newLine();
        //    for (int i = 0; i < 8; i++)
        //    {
        //        content.Add(new ArtOption(i == profile.character.accessoryBack, new List<AbsRichBoxMember> { new RbText("Accessory " + TextLib.IndexToString(i)) },
        //            new RbAction1Arg<int>((int index)=>{
                        
        //                var profile = GetProfile();
        //                {
        //                    profile.accessoryBack = index;
        //                }
        //                SetProfile(profile);
        //                refreshPreview();
        //            }, i)));
        //    }
        //    content.newLine();
        //    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
        //        new RbImage(SpriteName.WarsHudIconOpen),
        //        new RbSpace(),
        //        new RbText("Import model")
        //    }, null));

        //    content.newParagraph();
        //    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
        //        new RbImage(SpriteName.pjNumPlus),
        //        new RbSpace(),
        //        new RbText("Add")
        //    }, new RbAction(menu.clearState)));

        //    Refresh(content);
        //}


        float ScaleProperty(bool set, float value)
        {
            var profile = GetProfile();
            if (set) { 
                profile.soldierScale = value;
                SetProfile(profile);
            }
            return profile.soldierScale;
        }
        //public bool AutoAnimate(int index, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        panOnZoom = value;
        //        settingsHasChanged = true;
        //    }
        //    return panOnZoom;
        //}

        public CharacterProfile GetProfile()
        {
            //var profile = DssRef.storage.profileStorage.profiles[DssRef.storage.localPlayers[0].profileIndex];
            //return profile;
            return DssRef.storage.characterStorage.Selected();
        }
        public void SetProfile(CharacterProfile profile)
        {
            //DssRef.storage.profileStorage.profiles[DssRef.storage.localPlayers[0].profileIndex] = profile;
            DssRef.storage.characterStorage.SetSelected(profile);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DssRef.storage.characterStorage.SaveSelected();
        }
        //bool overrideHatProperty(int index, bool set, bool value)
        //{
        //    if (set) { DssRef.storage.HostProfile().character.overrideHat = value; }
        //    return DssRef.storage.HostProfile().character.overrideHat;
        //}
    }

    enum CharacterCreatorTab
    { 
        Soldiers,
        Workers,
        Animals,
        NUM
    }

    struct FrameTime
    {
        public int frame;
        public float time;

        public FrameTime(int frame, float time)
        {
            this.frame = frame;
            this.time = time;
        }
    }
}
