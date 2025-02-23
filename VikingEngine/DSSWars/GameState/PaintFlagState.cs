﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using VikingEngine.DSSWars.Profile;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Input;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;


namespace VikingEngine.DSSWars
{
    class PaintFlagState : Engine.GameState, DataStream.IStreamIOCallback
    {
        InputMap keyboardInput, controllerInput;
        int profileIx;
        public FlagAndColor profile;
        public ProfileColorType selectedColorType;
        //public FlagDesign file;
        Grid2D<Image> imageGrid;
        Vector2 start;
        public VectorRect paintArea;
        float squareWidth;
        Vector2 squareSz;
        Display.MenuSystem menuSystem;
        PaintFlagHud hud;
        bool isExiting = false;
        public bool controllerMode;
        public bool controllerPickColorState = false;
        bool paitingHasChanged = false;
        Graphics.Image pointer;
        Graphics.Image pixelOutline;
        public List<FlagAndColor> undoHistory = new List<FlagAndColor>();
        public List<FlagAndColor> redoHistory = new List<FlagAndColor>();

        public PaintFlagState(int profileIx, bool bController)
            : base(false)
        {
            draw.ClrColor = new Color(40, 45, 47);

            Engine.XGuide.UnjoinAll();
            int player = 0;
            Engine.XGuide.LocalHostIndex = player;


            keyboardInput = new InputMap(player);
            controllerInput = new InputMap(player);
            controllerInput.xboxSetup();

            this.profileIx = profileIx;
            profile = DssRef.storage.flagStorage.flagDesigns[profileIx].Clone();
            undoHistory.Add(profile.Clone());
            //file = profile.flagDesign;

            //GRID
            float gridW = Engine.Screen.SafeArea.Height * 0.6f;

            squareWidth = MathExt.RoundAndEven(gridW / DssLib.UserHeraldicWidth);
            squareSz = new Vector2(squareWidth);
            float totalWidth = squareWidth * DssLib.UserHeraldicWidth;
            start = new Vector2(
                Engine.Screen.CenterScreen.X - totalWidth * 0.5f,
                Engine.Screen.SafeArea.Y);

            paintArea = new VectorRect(start, new Vector2(totalWidth));

            imageGrid = new Grid2D<Image>(DssLib.UserHeraldicWidth);

            imageGrid.LoopBegin();
            while (imageGrid.LoopNext())
            {
                VectorRect area = new VectorRect(start + imageGrid.LoopPosition.Vec * squareSz,
                    squareSz);
                area.AddRadius(-1);
                imageGrid.LoopValueSet(new Image(SpriteName.WhiteArea, area.Position,
                    area.Size, ImageLayers.Bottom5));
            }

            updateImageGrid();

            pointer = new Graphics.Image(SpriteName.ColorPickerCircle,
                    paintArea.Center, Engine.Screen.SmallIconSizeV2, ImageLayers.Lay1_Front, true);

            pixelOutline = new Image(SpriteName.InterfaceBorder, Vector2.Zero, squareSz, ImageLayers.Bottom4);

            setControllerMode(bController);

            //HUD
            HudLib.Init();
            menuSystem = new Display.MenuSystem(keyboardInput, Display.MenuType.Editor);

            hud = new PaintFlagHud(keyboardInput, this);
            setColorType(ProfileColorType.Main);
            new Timer.AsynchActionTrigger(load_asynch, true);
        }
        Texture2D bgTex;
        void load_asynch()
        {
            bgTex = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "flag painter bg");
            new Timer.Action0ArgTrigger(loadingComplete);
        }

        void loadingComplete()
        {
            float w = Engine.Screen.Width + 4;
            float h = w / bgTex.Width * bgTex.Height;
            float x = -2;
            float y = Screen.CenterScreen.Y - h * 0.5f;

            ImageAdvanced bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                new Vector2(x, y), new Vector2(w, h), ImageLayers.AbsoluteBottomLayer, false);
            bgImage.Texture = bgTex;
            bgImage.SetFullTextureSource();
            bgImage.Opacity = 0.3f;

            //Vector2 promoworkerSz = new Vector2(h * 0.05f);

            //var worker1 = new Graphics.Image(SpriteName.warsWorkerPromoHammer, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.84f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            //worker1.LayerAbove(bgImage);

            //var worker2 = new Graphics.Image(SpriteName.warsWorkerPromoBox, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.6f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            //worker2.LayerAbove(bgImage);

            //var worker3 = new Graphics.Image(SpriteName.warsWorkerPromoBox, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.5f, 1f), -promoworkerSz.Y * 0.8f), promoworkerSz, ImageLayers.Background5);
            //worker3.LayerAbove(bgImage);

            //var worker4 = new Graphics.Image(SpriteName.warsWorkerPromoBox, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.2f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            //worker4.LayerAbove(bgImage);

        }
        void setControllerMode(bool value)
        {
            controllerMode = value;
            pointer.Visible = value;
        }

        public InputMap VisualInput => controllerMode ? controllerInput : keyboardInput;

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            hud.update();
            pixelOutline.Visible = false;

            if (!isExiting)
            {
                if (menuSystem.menuUpdate())
                {
                }
                else
                {
                    if (hud.colorArea.updateInput())
                    {
                        profile.setColor(selectedColorType, hud.colorArea.getColor());

                        onColorChange();
                    }
                    else
                    {
                        updateInput();
                    }
                }
            }
        }

        public void setColorType(ProfileColorType colorType)
        {
            selectedColorType = colorType;
            var color = profile.getColor(colorType);

            hud.colorArea.setColor(color);
        }

        IntVector2 gridPosition;
        static readonly IntVector2 GridMaxPos = new IntVector2(DssLib.UserHeraldicWidth - 1);
        void updateInput()
        {
            if (controllerMode)
            {
                if (Input.Keyboard.AnyActivationKey_DownEvent())
                {
                    setControllerMode(false);
                    hud.part.refresh();
                    return;
                }
            }
            else
            {
                int p = -1;
                if (Input.XInput.AnyActivationKey_DownEvent(ref p))
                {
                    setControllerMode(true);
                    hud.part.refresh();
                    return;
                }
            }

            if (keyboardInput.FlagDesign_ToggleColor_Prev.DownEvent || controllerInput.FlagDesign_ToggleColor_Prev.DownEvent_AnyInstance)
            {
                nextColorType(false);
            }
            else if (keyboardInput.FlagDesign_ToggleColor_Next.DownEvent || controllerInput.FlagDesign_ToggleColor_Next.DownEvent_AnyInstance)
            {
                nextColorType(true);
            }

            if (controllerPickColorState)
            {
                if (hud.colorArea.updateControllerInput())
                {
                    profile.setColor(selectedColorType, hud.colorArea.getColor());
                    onColorChange();
                }

                if (XInput.KeyDownEvent(Buttons.Back) ||
                    controllerInput.ControllerCancel.DownEvent_AnyInstance ||
                    controllerInput.Select.DownEvent_AnyInstance ||
                    controllerInput.Controller_FlagDesign_Colorpicker.DownEvent_AnyInstance)
                {
                    controllerPickColorState = false;
                    hud.part.refresh();
                }
            }
            else
            {
                if (controllerInput.Controller_FlagDesign_Colorpicker.DownEvent_AnyInstance)
                {
                    controllerPickColorState = true;
                    hud.part.refresh();
                }

                foreach (var ins in XInput.controllers)
                {
                    if (ins.Connected)
                    {
                        if (ins.bLeftStick)
                        {
                            pointer.position += 0.4f * paintArea.Width * Ref.DeltaGameTimeSec * ins.JoyStickValue(ThumbStickType.Left).Direction;
                            pointer.position = paintArea.KeepPointInsideBound_Position(pointer.position);
                        }

                        if (ins.IsButtonDown(Buttons.A))
                        {
                            paintInput(pointer.position, true, false);
                        }
                        else if (ins.IsButtonDown(Buttons.X))
                        {
                            paintInput(pointer.position, true, true);
                        }
                    }
                }

                //if (controllerInput.Select.DownEvent_AnyInstance)
                //{

                //}

                if (XInput.KeyDownEvent(Buttons.DPadUp))
                {
                    moveOption(IntVector2.FromDir4(Dir4.N));
                }
                if (XInput.KeyDownEvent(Buttons.DPadDown))
                {
                    moveOption(IntVector2.FromDir4(Dir4.S));
                }
                if (XInput.KeyDownEvent(Buttons.DPadLeft))
                {
                    moveOption(IntVector2.FromDir4(Dir4.W));
                }
                if (XInput.KeyDownEvent(Buttons.DPadRight))
                {
                    moveOption(IntVector2.FromDir4(Dir4.E));
                }


                paintInput(Input.Mouse.Position,
                    keyboardInput.Select.IsDown || Input.Mouse.IsButtonDown(MouseButton.Left),
                    keyboardInput.FlagDesign_PaintBucket.IsDown);

                if (XInput.KeyDownEvent(Buttons.Back))
                {
                    discardAndExit();
                }
                if (XInput.KeyDownEvent(Buttons.Start))
                {
                    saveAndExit();
                }
            }

            if (VikingEngine.Input.Keyboard.KeyDownEvent(Keys.B) && VikingEngine.Input.Keyboard.Ctrl)
            {
                debugMenu();

            }
        }

        void debugMenu()
        {
            List<FactionType> factionTypes = WorldData.NamedAiTypes();
            factionTypes.AddRange(WorldData.AvailableGenericAiTypes());

            menuSystem.openMenu();
            GuiLayout layout = new GuiLayout("*DEBUG*", menuSystem.menu);
            {
                foreach (var ftype in factionTypes)
                {
                    new GuiTextButton(ftype.ToString(), null, new GuiAction1Arg<FactionType>(loadFactionFlag, ftype), false, layout);
                }
            }
            layout.End();


        }

        void loadFactionFlag(FactionType factionType)
        {
            menuSystem.closeMenu();
            FlagAndColor flag = new FlagAndColor(factionType, 0, null);
            profile = flag;
            updateImageGrid();
            hud.part.refresh();
            setUndoPoint();
        }

        void paintInput(Vector2 pointer, bool keyIsDown, bool bBucket)
        {
            if (selectedColorType <= ProfileColorType.Detail2)
            {
                bool inPaintArea = paintArea.IntersectPoint(pointer);
                if (inPaintArea)
                {
                    Vector2 paintPos = pointer - paintArea.Position;
                    gridPosition = new IntVector2(
                        (int)(paintPos.X / squareWidth),
                        (int)(paintPos.Y / squareWidth));
                    gridPositionBounds();

                    if (keyIsDown)
                    {
                        setColor(selectedColorType, bBucket);
                    }

                    pixelOutline.Visible = true;
                    var ar = imageGrid.Get(gridPosition).Area;
                    ar.AddRadius(2);
                    pixelOutline.Area = ar;
                }

                if (!keyIsDown && paitingHasChanged)
                {
                    setUndoPoint();
                }
            }
        }

        void nextColorType(bool forward)
        {
            if (forward)
            {
                selectedColorType++;
                if (selectedColorType >= ProfileColorType.NUM)
                {
                    selectedColorType = 0;
                }
            }
            else
            {
                selectedColorType--;
                if (selectedColorType < 0)
                {
                    selectedColorType = ProfileColorType.NUM - 1;
                }
            }
            hud.part.selectColorType(selectedColorType);
        }

        public void discardAndExit()
        {
            new LobbyState();
        }
        public void saveAndExit()
        {
            //EXIT
            DssRef.storage.flagStorage.flagDesigns[profileIx] = profile;
            new LobbyState();
            DssRef.storage.flagStorage.Save(profileIx);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
        }


        //void moveImage()
        //{
        //    GuiLayout layout = new GuiLayout(DssRef.lang.ProfileEditor_MoveImage, menuSystem.menu, GuiLayoutMode.MultipleColumns, null);
        //    {
        //        new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageLeft, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.Left), true, layout);
        //        new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageRight, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.Right), true, layout);
        //        new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageUp, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.NegativeY), true, layout);
        //        new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageDown, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.PositiveY), true, layout);
        //    }
        //    layout.End();
        //}

        public void moveOption(IntVector2 dir)
        {
            profile.flagDesign.MoveEveryThing(dir);
            updateImageGrid();
            setUndoPoint();
        }

        public void clearAll()
        {
            profile.flagDesign.SetAll(0);
            updateImageGrid();
            setUndoPoint();
        }

        void profileColorButton(ProfileColorType colType, GuiLayout layout)
        {
            var button = new GuiIconTextButton(SpriteName.WhiteArea,
                colType.ToString(), null, new GuiAction1Arg<ProfileColorType>(changeColor, colType), 
                false, layout);
            button.icon.Color = profile.getColor(colType);

        }

        public void changeColor(ProfileColorType type)
        {
            menuSystem.openMenu();

            const int HueCount = 32;
            const int LightnessCount = 24;

            double[] Saturations = new double[] { 1, 0.9, 0.8, 0.7, 0.6, 0.5, 0.4, 0.3, 0.2, 0.1, 0 };

            GuiLayout layout = new GuiLayout(DssRef.lang.ProfileEditor_PickColor, menuSystem.menu, GuiLayoutMode.MultipleColumns, null);
            {

                foreach (var saturate in Saturations)
                {
                    for (int hue = 0; hue < HueCount; ++hue)
                    {
                        for (int light = LightnessCount - 1; light >= 1; --light)
                        {
                            Color col = lib.HSL2RGB((double)hue / HueCount, saturate, (double)light / LightnessCount);
                            col = BlockHD.FilterColor(col);

                            var icon = new GuiSmallIcon(SpriteName.WhiteArea, col.ToString(), 
                                new GuiAction2Arg<ProfileColorType, Color>(changeColor, type, col), false, layout);
                            icon.iconImage.Color = col;
                        }
                    }
                }
            }
            layout.End();
        }


        void changeColor(ProfileColorType type, Color toCol)
        {
            profile.setColor(type, toCol);

            menuSystem.menu.PopAllLayouts();
            //mainMenu();

            onColorChange();
            setUndoPoint();
        }

        void onColorChange()
        {
            updateImageGrid();
            hud.part.refresh();
            //colorButtons.refreshColors(profile);
        }

        void updateImageGrid()
        {
            imageGrid.LoopBegin();
            while (imageGrid.LoopNext())
            {
                imageGrid.LoopValueGet().Color = profile.getColor((ProfileColorType)profile.flagDesign.Get(imageGrid.LoopPosition));
                setTexturePos(imageGrid.LoopPosition);
            }
        }


        public void setUndoPoint()
        { 
            undoHistory.Add(profile.Clone());
            redoHistory.Clear();

            paitingHasChanged = false;
            hud.part.refresh();
        }

        public void undo()
        {
            if (undoHistory.Count > 1)
            {
                redoHistory.Add(arraylib.PullLastMember(undoHistory));
                profile = undoHistory.Last().Clone();
                updateImageGrid();
                hud.part.refresh();
            }
        }

        public void redo()
        {
            if (redoHistory.Count > 0)
            {
                var r = arraylib.PullLastMember(redoHistory);
                profile = r.Clone();
                undoHistory.Add(r);
                updateImageGrid();
                hud.part.refresh();
            }
        }

        void setTexturePos(IntVector2 pos)
        {
        }

        private void gridPositionBounds()
        {
            gridPosition = Bound.Set(gridPosition, IntVector2.Zero, GridMaxPos);
        }

       

        void setColor(ProfileColorType color, bool bBucket)
        {
            if (bBucket)
            {
                byte prev = profile.flagDesign.Get(gridPosition);
                if (prev != (byte)color)
                {
                    bucket(prev, (byte)color, gridPosition);
                    updateImageGrid();
                }
            }
            else
            {
                profile.flagDesign.Set(gridPosition, (byte)color);
                imageGrid.Get(gridPosition).Color = profile.getColor(color);
                setTexturePos(gridPosition);
            }

            paitingHasChanged = true;
        }

        void bucket(byte fromColor, byte toColor, IntVector2 pos)
        {
            byte prev = profile.flagDesign.Get(pos);
            if (prev == fromColor)
            {
                profile.flagDesign.Set(pos, toColor);
                foreach (IntVector2 dir in IntVector2.Dir4Array)
                {
                    IntVector2 neighbor = dir + pos;
                    if (profile.flagDesign.InBounds(neighbor))
                    {
                        bucket(fromColor, toColor, neighbor);
                    }
                }
            }
        }

        public static string ProfileColorName(ProfileColorType color)
        {
            switch (color)
            {
                case ProfileColorType.Main: return DssRef.lang.ProfileEditor_MainColor;
                case ProfileColorType.Detail1: return DssRef.lang.ProfileEditor_Detail1Color;
                case ProfileColorType.Detail2: return DssRef.lang.ProfileEditor_Detail2Color;
                case ProfileColorType.Skin: return DssRef.lang.ProfileEditor_SkinColor;
                case ProfileColorType.Hair: return DssRef.lang.ProfileEditor_HairColor;
            }
            throw new NotImplementedException();
        }
    }
}
