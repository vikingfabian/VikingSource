using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;
using Microsoft.Xna.Framework.Input;
using VikingEngine.Engine;
using VikingEngine.LootFest.Editor.Scene;
using VikingEngine.DataStream;
using VikingEngine.Input;


namespace VikingEngine.LootFest.Editor
{

    class SceneMaker : Engine.GameState, SceneModelsParent
    {
        SceneCollection collection;

        static readonly Color[] BackgColors = new Color[] { Color.White, Color.Black, Color.CornflowerBlue, Color.DarkBlue, Color.DarkGray, };
        const float MoveSpeed = 0.01f;
        const float RotateCamSpeed = 0.003f;
        const float ZoomSpeed = 0.05f;
        const float FreeRotationSpeed = 0.0008f;
        Graphics.TopViewCamera camera;
        
        HUD.ButtonLayout buttonLayout = null;
        HUD.MessageHandler messageHandler;
        Gui menu;
        //HUD.GuiLayout mFile;
        Graphics.Mesh freePencil;
        public Vector3 PencilPos { get { return freePencil.Position; } }
        Graphics.Mesh selectionBox;
        Graphics.Mesh centerMark;

        EditType editType = EditType.Move;
        int selectedMemberIndex = -1;
        SceneModel selectedMember = null;
        Graphics.TextG infoText;
        //Input.AbsControllerInstance controller;
        LootFest.Players.InputMap inputMap;
        

        public SceneMaker(int player)
            : base()
        {
            //controller = Input.Controller.Instance(player);
            inputMap = XGuide.GetPlayer(player).inputMap as LootFest.Players.InputMap;
            camera = new Graphics.TopViewCamera(120, new Vector2(MathHelper.PiOver2 - 0.14f, MathHelper.PiOver4));
            Ref.draw.Camera = camera;
            messageHandler = new HUD.MessageHandler(3, SpriteName.WhiteArea, 5000);
            VectorRect menuArea = Engine.Screen.SafeArea; menuArea.Width = 400;
           // menu = new Menu(menuArea, VectorRect.ZeroOne, ImageLayers.Lay2, int.Player1);
            

            Ref.draw.ClrColor = Color.CornflowerBlue;

            freePencil = new Graphics.Mesh(LoadedMesh.sphere, Vector3.Zero, new Vector3(0.4f),
                Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, Color.Blue);
                //new Graphics.TextureEffect(
                //Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea), 0.4f);
            //freePencil.Color = Color.Blue;

            selectionBox = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.Zero,
                Graphics.TextureEffectType.Flat, SpriteName.EditorMultiSelection, Color.White);
            //new Graphics.TextureEffect(
            //Graphics.TextureEffectType.Flat, SpriteName.EditorMultiSelection), 0f);

            centerMark = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(0.2f),
                Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.DarkGray);
            //    new Graphics.TextureEffect(
            //    Graphics.TextureEffectType.Flat, SpriteName.WhiteArea), 0.2f);
            //centerMark.Color = Color.DarkGray;

            infoText = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.SafeArea.LeftBottom, Vector2.One * 0.6f, new Graphics.Align(Dir8.SW),
                "", Color.Black, ImageLayers.Background4);


            collection = new SceneCollection(null, this);

            collection.randomFileName();

            openPage(MenuPage.Main);
            DataStream.FilePath.CreateStorageFolder(SceneCollection.Folder);
        }


        public Vector3 SceneCenterPos { get { return freePencil.Position; } set { freePencil.Position = value; } }

        public void OpenMainMenu() 
        {
            openPage(MenuPage.Main);
        }

        void openPage(int page)
        {
            this.openPage((MenuPage)page);
        }
        void openPage(MenuPage page)
        {
            //mFile = new HUD.File();
            //switch (page)
            //{
            //    case MenuPage.Main:
            //        mFile.AddTextLink("Import model", (int)SceneMakerLink.ImportModel);
            //        if (collection.members.Count > 0)
            //        {
            //            mFile.AddTextLink("List models", new HUD.ActionLink(listModels));
            //        }
            //        mFile.AddTextLink("View all models", "Make all hidden models visible", new HUD.ActionLink(collection.viewAllModels));

            //        mFile.AddTextLink("Save scene", new HUD.ActionLink(saveOptions));
            //        mFile.AddTextLink("Load scene", new HUD.ActionLink(collection.listFiles));
            //        mFile.AddTextLink("Settings", (int)SceneMakerLink.Settings);
            //        collection.CamerasToMenu(mFile);
            //        mFile.AddTextLink("Clear all", new HUD.ActionLink(collection.clearScene));
            //        mFile.AddTextLink("Exit to Menu", (int)SceneMakerLink.ExitToMenu);
            //        break;
            //    case MenuPage.SelectionMenu:
            //        mFile.AddDescription("Selected model");
            //        List<string> edittypes = new List<string>();
            //        for (EditType type = (EditType)0; type < EditType.NUM; type++)
            //        {
            //            edittypes.Add(type.ToString());
            //        }
            //        //mFile.AddTextOptionList("Edit type", (int)SceneMakerLink.SelectEditType, (int)editType, edittypes);
            //        mFile.AddTextLink("Reset rotation", new HUD.ActionLink(resetRotation));
            //        mFile.AddTextLink("Delete", (int)SceneMakerLink.DeleteMember);
            //        break;
            //    case MenuPage.Settings:
            //        List<string> bgcolors = new List<string>();
            //        int ix = 0;
            //        for (int i = 0; i < BackgColors.Length; i++)
            //        {
            //            if (BackgColors[i] == Ref.draw.ClrColor)
            //                ix = i;
            //            bgcolors.Add(BackgColors[i].ToString());
            //        }
            //       // mFile.AddTextOptionList("BG Color", (int)SceneMakerLink.BackgColor, 0, bgcolors);
            //        mFile.AddTextLink("BG Color", new HUD.ActionLink(listBackgroundColors));
            //        mFile.AddTextLink("Hide HUD", (int)SceneMakerLink.HideHUD);
            //        mFile.AddIconTextLink(SpriteName.LFIconGoBack, "Back", new HUD.ActionIndexLink(openPage, (int)MenuPage.Main));
            //        break;
            //}
            //openMenuFile();
        }

        void listModels()
        {
            //mFile = new HUD.File();
            //mFile.AddDescription("Click to select");
            //for (int i = 0; i < collection.members.Count; ++i)
            //{
            //    mFile.AddTextLink(collection.members[i].Name, new HUD.ActionIndexLink(selectModel, i));
            //}
            //openMenuFile();
        }
        void selectModel(int index)
        {
            CloseMenu();
            selectedMemberIndex = index;
            selectMemeber();
        }

        void listBackgroundColors()
        {
            //mFile = new HUD.File();
            //MyColor[] colors = { MyColor.Black, MyColor.White, MyColor.Sky_blue, MyColor.Dark_blue, MyColor.Dark_Gray };
            //foreach (MyColor col in colors)
            //{
            //    mFile.AddTextLink(TextLib.EnumName(col.ToString()), new HUD.ActionIndexLink(selectBGcol, (int)col));
            //}
            //openMenuFile();
        }
        void selectBGcol(int color)
        {
            Ref.draw.ClrColor = lib.GetColor((MyColor)color);
            openPage(MenuPage.Settings);
        }

        //HUD.InputDialogue inputDialogue;
        public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        {
            //inputDialogue = new HUD.InputDialogue(menu, keyInputValues);
        }
        public override void TextInputCancelEvent(int playerIndex)
        {
            //if (inputDialogue != null)
            //{
            //    inputDialogue.DeleteMe();
            //    inputDialogue = null;
            //}
        }
        public override void TextInputEvent(string input, object tag)
        {
            collection.currentFileName = input;
            save();
            CloseMenu();
            TextInputCancelEvent(0);
        }
        //public override void BlockMenuLinkEvent(HUD.IMenuLink link, int playerIx, numBUTTON button)
        //{
        //    switch ((SceneMakerLink)link.Value1)
        //    {
        //        case SceneMakerLink.ImportModel: //börja med bara my voxelobjs
        //            //mFile = VoxelDesigner.listUseMadeModels(false, (int)SceneMakerLink.SelectImportModel);
        //            //mFile.Properties.ParentPage = (int)MenuPage.Main;
        //            //openMenuFile();
        //            break;
        //        case SceneMakerLink.SelectImportModel:
        //            string name = VoxelDesigner.UserMadeModelNameFromIndex(link.Value2);
        //            new SceneModel(name, collection, true);
        //            openPage(MenuPage.Main);
        //            break;
        //        case SceneMakerLink.ExitToMenu:
        //            //new MainMenuState();
        //            break;
        //        case SceneMakerLink.HideHUD:
        //            CloseMenu();
        //            viewHUD(false);
        //            break;
        //        case SceneMakerLink.Settings:
        //            openPage(MenuPage.Settings);
        //            break;
        //        case SceneMakerLink.DeleteMember:
        //            selectedMember.DeleteMe();
        //            selectedMember = null;
        //            collection.RemoveMember(selectedMemberIndex);
        //            CloseMenu();
        //            break;

        //    }
        //}

        //void saveOptions()
        //{
        //     mFile = new HUD.File();
        //     mFile.AddTitle("Save");
        //     mFile.AddDescription("Name: " + collection.currentFileName);
        //     mFile.AddTextLink("Replace Old Save", new HUD.ActionLink(save));
        //     mFile.AddTextLink("Save as new", new HUD.ActionLink(typeFileName));
        //     openMenuFile();
        //}

        void typeFileName()
        {
            collection.randomFileName();
            //Engine.XGuide.BeginKeyBoardInput(new KeyboardInputValues("Name your creation", collection.currentFileName, inputMap.playerIndex));//(int)controller.Index));
        }
       
        void save()
        {
            collection.save(true);
            CloseMenu();
            messageHandler.Print("Saving...");
        }
        

        

        void resetRotation()
        {
            selectedMember.Model.Rotation.QuadRotation = Quaternion.Identity;
        }
        

        void viewHUD(bool view)
        {
            freePencil.Visible = view;
            centerMark.Visible = view;
            selectionBox.Visible = view;
            infoText.Visible = view;
        }

        //public override void BlockMenuListOptionEvent(int link, int option, int playerIx)
        //{
        //    switch ((SceneMakerLink)link)
        //    {
        //        case SceneMakerLink.SelectEditType:
        //            editType = (EditType)option;
        //            break;
        //    }
        //}

        bool InMenu { get { return menu.Visible; } }
       
        protected IntervalF ZoomBounds = new IntervalF(4, 600);

        //bool cameraCtlrKeyDown
        //{
        //    get
        //    {
        //        const Buttons CameraControl = Buttons.LeftTrigger;
        //        return controller.IsButtonDown(CameraControl);
        //    }
        //}

        public void CloseMenu()
        {
            //menu.Visible = false;
            //Input.Mouse.CenterMouse = true;
            Input.Mouse.Visible = false;
            Input.Mouse.Update();
            //Input.Mouse.Update();
        }

        void openMenuFile()
        {
           // OpenMenuFile(mFile);
        }
        public void OpenMenuFile(HUD.GuiLayout file)
        {
            //menu.Visible = true;
            //menu.File = file;
            //if (!Input.Mouse.Visible)
            //{
            //    //Input.Mouse.CenterMouse = false;
            //    Input.Mouse.Visible = true;
            //    Input.Mouse.SetPosition(new IntVector2((int)menu.Position.X, (int)menu.Position.Y));
            //}
        }

       

        void moveFreePencil(Vector3 dir)
        {
            if (selectedMember == null || editType == EditType.Move)
            {

                freePencil.Position += dir * MoveSpeed;

                const string FloatFormat = "#.0";
                string info = "pos X" + freePencil.Position.X.ToString(FloatFormat) + " Y" + freePencil.Position.Y.ToString(FloatFormat)+ " Z" + freePencil.Position.Z.ToString(FloatFormat); //freePencil.Position.ToString();
                //selectClosetObj
                if (collection.members.Count > 0)
                {
                    FindMinValue shortestDist = new FindMinValue(true);
                    for (int i = 0; i < collection.members.Count; i++)
                    {
                        shortestDist.Next((collection.members[i].Model.position - freePencil.Position).Length(), i);
                    }
                    selectedMemberIndex = shortestDist.minMemberIndex;
                    //place box around it
                    VectorVolumeC vol = collection.members[selectedMemberIndex].Volume;
                    selectionBox.Position = vol.Center; selectionBox.Scale = vol.HalfSize;
                    info += " Model:" + collection.members[selectedMemberIndex].Name;
                }

                camera.LookTarget = freePencil.Position;
                infoText.TextString = info;

                if (selectedMember != null)
                {
                    selectedMember.Model.position = freePencil.Position;
                }
            }
        }

        SceneModel closestMember()
        {
            if (collection.members.Count == 0)
                return null;
            else if (selectedMember == null)
                return collection.members[selectedMemberIndex];
            else
                return selectedMember;
        }
        void updateInput()
        {
            if (inputMap.editorInput.OpenClose.DownEvent)//.DownEvent(ButtonActionType.EditorHelp))//controller.KeyDownEvent(Buttons.Back)
                //|| Input.Keyboard.KeyDownEvent(Keys.Back))
            {
                if (buttonLayout == null)
                {
                    string modeTitle;
                    List<HUD.ButtonDescriptionData> data = buttonDescription(out modeTitle);
                    if (data != null)
                    {
                        buttonLayout = new ButtonLayout(data, Engine.Screen.Area,
                          Engine.Screen.SafeArea, modeTitle, null);
                    }
                }
                else
                {
                    buttonLayout.DeleteMe();
                    buttonLayout = null;
                }
            }
            else if (InMenu)
            {
                //if (menuClick(Buttons.A) || menuClick(Buttons.X) || menuClick(Buttons.Y))
                //{

                //}
                if (inputMap.menuInput.click.DownEvent)//.DownEvent(ButtonActionType.MenuClick))
                {
                   // menu.Click(inputMap.Index, numBUTTON.A);
                }
                else if (inputMap.menuInput.back.DownEvent)//.DownEvent(ButtonActionType.MenuBack))//controller.KeyDownEvent(Buttons.B)
                  
                    //|| inputMap.Escape.DownEvent)//Input.Keyboard.KeyDownEvent(Keys.Escape)


                {
                    //if (menu.InSubMenu)
                    //{
                    //    menu.Back(inputMap.Index);
                    //}
                    //else
                    //    CloseMenu();
                }
                else if (inputMap.menuInput.openCloseInputEvent())//.DownEvent(ButtonActionType.EditorMenu))//controller.KeyDownEvent(Buttons.Start))
                {
                    CloseMenu();
                }
            }
            else
            {
                if (inputMap.menuInput.openCloseInputEvent())//.DownEvent(ButtonActionType.EditorMenu))//controller.KeyDownEvent(Buttons.Start)
                //|| Input.Keyboard.KeyDownEvent(Keys.Escape)

                    
                {
                    if (!freePencil.Visible)
                        viewHUD(true);

                    if (selectedMember == null)
                        openPage(MenuPage.Main);
                    else
                        openPage(MenuPage.SelectionMenu);
                }
                else if (inputMap.menuInput.click.DownEvent)//.DownEvent(ButtonActionType.MenuClick))
                //    controller.KeyDownEvent(Buttons.A)
                //|| Input.Keyboard.KeyDownEvent(Keys.Space)
                //    )
                {
                    if (selectedMember == null)
                    {
                        if (selectedMemberIndex >= 0)
                        {
                            selectMemeber();
                            //selectedMember = collection.members[selectedMemberIndex];
                            //freePencil.Position = selectedMember.Model.Position;
                            //moveFreePencil(Vector3.Zero);
                        }
                    }
                    else
                    {//drop model
                        selectedMember = null;
                    }
                }
                else if (inputMap.editorInput.cancel.DownEvent)//.DownEvent(ButtonActionType.EditorDeselect))//controller.KeyDownEvent(Buttons.B))
                {
                    selectedMember = null;
                }
                else if (inputMap.editorInput.select.DownEvent)//.DownEvent(ButtonActionType.EditorSelect))//controller.KeyDownEvent(Buttons.X)
                    //|| Input.Keyboard.KeyDownEvent(Keys.Tab))
                {
                    SceneModel mem = closestMember();
                    if (mem != null)
                    {
                        mem.Model.Visible = !mem.Model.Visible;
                    }
                }
                else if (inputMap.editorInput.draw.DownEvent)//.DownEvent(ButtonActionType.EditorDraw))//controller.KeyDownEvent(Buttons.Y)
                    //|| Input.Keyboard.KeyDownEvent(Keys.V))
                {//stamp clone
                    if (selectedMember != null && editType == EditType.Move)
                    {
                        new SceneModel(selectedMember, collection);
                    }
                }
                else if (inputMap.editorInput.previous.DownEvent)//.DownEvent(ButtonActionType.MenuTabLeftUp))//controller.KeyDownEvent(Buttons.LeftShoulder))
                {
                    nextEditMode(-1);
                }
                else if (inputMap.editorInput.next.DownEvent)//.DownEvent(ButtonActionType.MenuTabRightDown))//controller.KeyDownEvent(Buttons.RightShoulder))
                {
                    nextEditMode(1);
                }
            }

            if (InMenu)
            {
                //sticks
              //  menu.MoveSelection(inputMap.MenuMovement);//controller.JoyStickValue(Stick.Left));
            }
            else
            {
                if (inputMap.editorInput.toggleCameraMode.IsDown)//.IsDown(ButtonActionType.GameAltButton))//cameraCtlrKeyDown)
                {
                    rotateCamera(inputMap.editorInput.cameraXMoveY.directionAndTime);//controller.JoyStickValue(Stick.Right).DirAndTime);
                    zoom(inputMap.editorInput.moveXZ.directionAndTime.Y);//.DirTime(DirActionType.EditorMoveXZ).Y);//controller.JoyStickValue(Stick.Left).DirAndTime.Y);
                }
                else
                {
                    const float MouseSpeed = 2.4f;

                    moveAction(inputMap.editorInput.moveXZ.directionAndTime * MouseSpeed, -inputMap.editorInput.cameraXMoveY.directionAndTime.Y * mouseSpeed);//controller.JoyStickValue(Stick.Left).DirAndTime * MouseSpeed, -rightStick.Y * MouseSpeed);
                    Vector2 rightStick = inputMap.editorInput.cameraXMoveY.directionAndTime;//.DirTime(DirActionType.EditorCamXMoveY);
                    rightStick.Y = 0;
                    rotateCamera(rightStick);
                }
            }


            if (PlatformSettings.PC_platform)
            {
                //mouse input
                if (!InMenu)
                {


                    if (Input.Mouse.Scroll)
                    {
                        nextEditMode(lib.ToLeftRight(Input.Mouse.ScrollValue));
                    }

                    if (Input.Mouse.IsButtonDown(MouseButton.Right))
                    {
                        if (Input.Keyboard.IsKeyDown(Keys.LeftShift))
                        {//zoom
                            zoom(Input.Mouse.MoveDistance.Y * mouseSpeed);
                        }
                        else
                        {
                            Vector2 mouseMove = Input.Mouse.MoveDistance;
                            mouseMove.Y = -mouseMove.Y;
                            rotateCamera(mouseMove * mouseSpeed);
                        }
                    }
                    else if (Input.Mouse.IsButtonDown(MouseButton.Left) || selectedMember == null)
                    {
                        if (Input.Keyboard.IsKeyDown(Keys.LeftShift))
                        {
                            moveAction(Vector2.Zero, -Input.Mouse.MoveDistance.Y * mouseSpeed);
                        }
                        else
                        {
                            moveAction(Input.Mouse.MoveDistance * mouseSpeed, 0);
                        }
                    }
                }
            }
        }

        void selectMemeber()
        {
            selectedMember = collection.members[selectedMemberIndex];
            freePencil.Position = selectedMember.Model.position;
            moveFreePencil(Vector3.Zero);
        }

        const float MouseSpeedNormal = 4;
        const float MouseSpeedLow = 1;
        float mouseSpeed

        {
            get
            {
                if (Input.Keyboard.IsKeyDown(Keys.LeftControl))
                {
                    return MouseSpeedLow;
                }
                return MouseSpeedNormal;
            }
        }

        void rotateCamera(Vector2 planeDir)
        {
            camera.TiltX += planeDir.X * RotateCamSpeed;
            camera.TiltY += planeDir.Y * RotateCamSpeed;
        }
        void zoom(float value)
        {
            camera.targetZoom = Bound.Set(camera.targetZoom + value * ZoomSpeed, ZoomBounds.Min, ZoomBounds.Max); 
        }

        void moveAction(Vector2 XYdir, float Ydir)
        {
           
            if (selectedMember != null)
            {
                switch (editType)
                {
                    case EditType.Scale:
                        selectedMember.Model.scale += XYdir.Y * 0.0004f * Vector3.One;
                        break;
                    case EditType.FreeRotation:
                        selectedMember.Model.Rotation.RotateWorld(new Vector3(XYdir.X, Ydir, XYdir.Y) * FreeRotationSpeed);
                        break;
                    case EditType.PlaneRotation:
                        selectedMember.Model.Rotation.RotateWorld(new Vector3(XYdir.X, 0, 0) * FreeRotationSpeed);
                        break;
                }
            }

            Rotation1D camrot = Rotation1D.FromDirection(XYdir);
            camrot.Radians += camera.TiltX - MathHelper.PiOver2;
            XYdir = camrot.Direction(XYdir.Length());
            moveFreePencil(new Vector3(XYdir.X, Ydir, XYdir.Y));
            //}
        }

        //bool menuClick(Buttons button)
        //{
        //    if (controller.KeyDownEvent(button))
        //    {
        //        menu.Click((int)controller.Index, Input.InputLib.ButtonConvert(button));
        //        return true;
        //    }
        //    return false;
        //}


        List<HUD.ButtonDescriptionData> buttonDescription(out string title)
        {
            List<HUD.ButtonDescriptionData> buttons = null;

            if (menu.Visible)
            {
                title = "Menu";
                buttons = new List<HUD.ButtonDescriptionData>
                    {
                        new HUD.ButtonDescriptionData("Move", SpriteName.LeftStick),
                        new HUD.ButtonDescriptionData("Select", SpriteName.ButtonA),
                        new HUD.ButtonDescriptionData("Back", SpriteName.ButtonB),
                    };
            }
            else
            {
                if (selectedMember == null)
                {
                    title = "Move tool";
                    buttons = new List<HUD.ButtonDescriptionData>
                    {
                        new HUD.ButtonDescriptionData("Move XZ", SpriteName.LeftStick),
                        new HUD.ButtonDescriptionData("Move Y", SpriteName.RightStick_UD),
                        new HUD.ButtonDescriptionData("Next tool", SpriteName.ButtonRB),
                        new HUD.ButtonDescriptionData("Options", SpriteName.ButtonSTART),
                    };
                }
                else
                {
                    title = "Edit model: " + editType.ToString();

                    switch (editType)
                    {
                        case EditType.FreeRotation:
                            buttons = new List<HUD.ButtonDescriptionData>
                        {
                            new HUD.ButtonDescriptionData("Rotate XZ", SpriteName.LeftStick),
                            new HUD.ButtonDescriptionData("Rotate Y", SpriteName.RightStick),

                        };
                            break;
                        default:
                            buttons = new List<HUD.ButtonDescriptionData>
                        {
                            new HUD.ButtonDescriptionData("Move XZ", SpriteName.LeftStick),
                            new HUD.ButtonDescriptionData("Move Y", SpriteName.RightStick),
                            new HUD.ButtonDescriptionData("Make stamp", SpriteName.ButtonY),

                        };

                            break;
                        case EditType.Scale:
                            buttons = new List<HUD.ButtonDescriptionData>
                        {
                            new HUD.ButtonDescriptionData("Scale", SpriteName.RightStick_UD),

                        };
                            break;


                    }
                    buttons.Add(new HUD.ButtonDescriptionData("Hide/Show model", SpriteName.ButtonX));
                }
                buttons.Add(new HUD.ButtonDescriptionData("Pickup/Drop model", SpriteName.ButtonA));
                buttons.Add(new HUD.ButtonDescriptionData("Previous tool", SpriteName.ButtonLB));


                buttons.Add(new HUD.ButtonDescriptionData("Zoom", SpriteName.ButtonLT, SpriteName.LeftStick_UD));
                buttons.Add(new HUD.ButtonDescriptionData("Rotate Camera", SpriteName.ButtonLT, SpriteName.RightStick));

            }

            return buttons;
        }

        void nextEditMode(int dir)
        {
            editType = (EditType)Bound.SetRollover((int)editType + dir, 0, (int)EditType.NUM - 1);
            messageHandler.Print("Edit type:" + editType.ToString());
        }


        //public override Engine.GameStateType Type
        //{
        //    get { return Engine.GameStateType.Editor; }
        //}
        public override void Time_Update(float time)
        {
            selectionBox.Color = selectedMember == null ? Color.Gray : Color.White;
            //if (inputDialogue == null)
            //    updateInput();
            //else
            //    inputDialogue.UpdateKeyboardInput();
            
            base.Time_Update(time);
            draw.Camera.Time_Update(time);
        }
    }
    enum MenuPage
    {
        Main,
        Settings,
        SelectionMenu,
    }
    enum SceneMakerLink
    {
        Save,
        Load,
        ImportModel,
        SelectImportModel,
        ListModels,
        ExitToMenu,
        SelectEditType,
        Settings,
        BackgColor,
        HideHUD,
        DeleteMember,
    }
    
}
