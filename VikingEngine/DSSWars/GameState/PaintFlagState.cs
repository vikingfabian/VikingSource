using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using VikingEngine.DSSWars.Profile;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.PJ.Tanks;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;


namespace VikingEngine.DSSWars
{
    class PaintFlagState : Engine.GameState, DataStream.IStreamIOCallback
    {        
        InputMap input;
        int profileIx;
        public FlagAndColor profile;
        public ProfileColorType selectedColorType;
        public FlagDesign file;
        Grid2D<Image> imageGrid;
        Vector2 start;
        public VectorRect paintArea;
        float squareWidth;
        Vector2 squareSz;
        //ColorButtonGroup colorButtons;
        Display.MenuSystem menuSystem;
        PaintFlagHud hud;
        bool isExiting = false;

        public PaintFlagState(int profileIx)
            : base(false)
        {
            draw.ClrColor = Color.SaddleBrown;

            Engine.XGuide.UnjoinAll();
            int player = 0;
            Engine.XGuide.LocalHostIndex = player;

            input = new InputMap(player);
            this.profileIx = profileIx;
            profile = DssRef.storage.flagStorage.flagDesigns[profileIx].Clone();

            

            file = profile.flagDesign;

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

            //float paintAreaEdge = Engine.Screen.Width * 0.05f;

            //colorButtons = new ColorButtonGroup(paintArea, profile);
            
            updateImageGrid();

            //HUD
            HudLib.Init();
            menuSystem = new Display.MenuSystem(input,  Display.MenuType.Editor);

            //HUD.RichBox.RichBoxContent rbContent = new HUD.RichBox.RichBoxContent();
            //rbContent.add(SpriteName.KeyEsc, DssRef.lang.ProfileEditor_OptionsMenu);
            //rbContent.add(SpriteName.KeyAlt, "Bucket");

            //new HUD.RichBox.RichBoxGroup(Engine.Screen.SafeArea.Position, Engine.Screen.SafeArea.Width * 0.25f,
            //    ImageLayers.Background1, HudLib.RbSettings, rbContent);
            hud = new PaintFlagHud(input, this);
            setColorType(ProfileColorType.Main);

        }
        
        public override void Time_Update(float time)
        { 
            base.Time_Update(time);
            hud.update();
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
            if (selectedColorType <= ProfileColorType.Detail2)
            {
                Vector2 pointer = Input.Mouse.Position;

                bool inPaintArea = paintArea.IntersectPoint(pointer);
                if (inPaintArea)
                {
                    Vector2 paintPos = pointer - paintArea.Position;
                    gridPosition = new IntVector2(
                        (int)(paintPos.X / squareWidth),
                        (int)(paintPos.Y / squareWidth));
                    gridPositionBounds();

                    if (input.Select.IsDown || Input.Mouse.IsButtonDown(MouseButton.Left))
                    {
                        setColor(selectedColorType);
                    }
                }

            }
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

        //void mainMenu()
        //{
        //    menuSystem.openMenu();

        //    GuiLayout layout = new GuiLayout(DssRef.lang.ProfileEditor_OptionsMenu, menuSystem.menu, GuiLayoutMode.MultipleColumns, null);
        //    {
        //        new GuiLabel(DssRef.lang.ProfileEditor_FlagColorsTitle, layout);
        //        profileColorButton(ProfileColorType.Main, layout);
        //        profileColorButton(ProfileColorType.Detail1, layout);
        //        profileColorButton(ProfileColorType.Detail2, layout);

        //        new GuiSectionSeparator(layout);
        //        new GuiLabel(DssRef.lang.ProfileEditor_PeopleColorsTitle, layout);
        //        profileColorButton(ProfileColorType.Skin, layout);
        //        profileColorButton(ProfileColorType.Hair, layout);

        //        new GuiSectionSeparator(layout);

        //        new GuiTextButton(DssRef.lang.ProfileEditor_MoveImage, null, moveImage, true, layout);
        //        if (PlatformSettings.DevBuild)
        //        {
        //            new GuiTextButton("*Print array", null, file.dataGrid.Print, false, layout);
        //        }
        //        new GuiTextButton(DssRef.lang.ProfileEditor_DiscardAndExit, DssRef.lang.ProfileEditor_DiscardAndExitDescription, discardAndExit, false, layout);
        //        new GuiTextButton(DssRef.lang.ProfileEditor_SaveAndExit, null, saveAndExit, false, layout);
        //    }
        //    layout.End();
        //}

        void moveImage()
        {
            GuiLayout layout = new GuiLayout(DssRef.lang.ProfileEditor_MoveImage, menuSystem.menu, GuiLayoutMode.MultipleColumns, null);
            {
                new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageLeft, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.Left), true, layout);
                new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageRight, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.Right), true, layout);
                new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageUp, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.NegativeY), true, layout);
                new GuiTextButton(DssRef.lang.ProfileEditor_MoveImageDown, null, new GuiAction1Arg<IntVector2>(moveOption, IntVector2.PositiveY), true, layout);
            }
            layout.End();
        }

        public void moveOption(IntVector2 dir)
        {
            file.dataGrid.MoveEveryThing(dir);
            updateImageGrid();
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
                imageGrid.LoopValueGet().Color = profile.colors[file.dataGrid.Get(imageGrid.LoopPosition)];
                setTexturePos(imageGrid.LoopPosition);
            }
        }

        void setTexturePos(IntVector2 pos)
        {
        }

        private void gridPositionBounds()
        {
            gridPosition = Bound.Set(gridPosition, IntVector2.Zero, GridMaxPos);
        }

        void setColor(ProfileColorType color)
        {
            if (Input.Keyboard.Alt)
            {
                byte prev = file.dataGrid.Get(gridPosition);
                if (prev != (byte)color)
                {
                    bucket(prev, (byte)color, gridPosition);
                    updateImageGrid();
                }
            }
            else
            {
                file.dataGrid.Set(gridPosition, (byte)color);
                imageGrid.Get(gridPosition).Color = profile.getColor(color);
                setTexturePos(gridPosition);
            }
        }

        void bucket(byte fromColor, byte toColor, IntVector2 pos)
        {
            byte prev = file.dataGrid.Get(pos);
            if (prev == fromColor)
            {
                file.dataGrid.Set(pos, toColor);
                foreach (IntVector2 dir in IntVector2.Dir4Array)
                {
                    IntVector2 neighbor = dir + pos;
                    if (file.dataGrid.InBounds(neighbor))
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
