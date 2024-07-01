using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.Voxels;
using VikingEngine.Input;
using VikingEngine.DataStream;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Editor
{
    class DesignMenuSystem : Voxels.AbsDesignMenuSystem
    {
        static readonly IntervalF RGBrange = new IntervalF(byte.MinValue, byte.MaxValue);

        VoxelDesigner designer;
        bool menuModelIcons = true;
        

        public DesignMenuSystem(VoxelDesigner designer)
            : base(designer)
        {
            this.designer = designer;
        }

        override public void openMenu()
        {
            if (menu == null)
            {

                if (designer.inGame)
                {
                    menu = designer.parent.createMenu();
                }
                else
                {
                    menu = new Gui(LootFest.MenuSystem2.GuiStyle(), Engine.Screen.SafeArea, 0, ImageLayers.Foreground5,
                        Input.InputSource.DefaultPC);
                    menu.inputmap = designer.menuInput;
                    Input.Mouse.Visible = true;
                }


                mainMenu();
                designer.ShowHUD(true);
            }
        }

        public override void closeMenu()
        {
            if (designer.inGame)
            {
                designer.parent.CloseMenu();
                menu = null;
            }
            else
            {
                base.closeMenu();
            }
        }

        void selectTool()
        {
            GuiLayout layout = new GuiLayout("Select Tool", menu, GuiLayoutMode.MultipleColumns, null);
            {
                for (PaintToolType tool = 0; tool < PaintToolType.NUM; ++tool)
                {
                    new GuiBigIcon(VoxelDesignerInterface.ToolIcon(tool), tool.ToString(), new GuiAction1Arg<PaintToolType>(pickTool, tool), false, layout);
                }
            }
            layout.End();
        }

        void pickTool(PaintToolType tool)
        {
            designer.Settings.DrawTool = tool;
            menu.PopAllLayouts();
            mainMenu();
        }
        
        public void mainMenu()
        {
            openMenu();
            VoxelDesignerSettings sett = designer.Settings;

            GuiLayout layout = new GuiLayout("Main Menu", menu);
            {
                if (PlatformSettings.DevBuild)
                {
                   // new GuiTextButton("*Quick convert materials", null, quickConvertAll, false, layout);
                }

                var colorButton = new GuiIconTextButton(SpriteName.WhiteArea, "Color", null, selectColorMenu, true, layout);
                colorButton.icon.Color = designer.SelectedMaterial.color;

                //List<GuiOption<PaintToolType>> toolsOptList = new List<GuiOption<PaintToolType>>();

                //for (PaintToolType tool = 0; tool < PaintToolType.NUM; ++tool)
                //{
                //    toolsOptList.Add(new GuiOption<PaintToolType>(tool.ToString(), tool));
                //}
                //new GuiOptionsList<PaintToolType>("Tool", toolsOptList, toolProperty, layout);
                new GuiIconTextButton(VoxelDesignerInterface.ToolIcon(designer.Settings.DrawTool), "Tool", null, selectTool, true, layout);

                if (sett.DrawTool == PaintToolType.Pencil || sett.DrawTool == PaintToolType.Road || sett.DrawTool == PaintToolType.ReColor)
                {
                    new GuiIntSlider(SpriteName.NO_IMAGE, "Pencil size", pencilSizeProperty, new IntervalF(1, 17), false, layout);
                    new GuiFloatSlider(SpriteName.NO_IMAGE, "Size tolerance", radiusToleranceProperty, new IntervalF(-0.5f, 0.5f), false, layout);
                    new GuiCheckbox("Round pencil", null, bRoundPencilProperty, layout);
                    if (sett.DrawTool == PaintToolType.Road)
                    {
                        new GuiIntSlider(SpriteName.NO_IMAGE, "Edge size", RoadEdgeSizeProperty, new IntervalF(0, 5), false, layout);
                        //new GuiIconTextButton(Data.BlockTextures.MaterialTile(secondaryMaterial), "Edge Material", 
                        //    null, listSecondaryMaterialsLink, true, layout);
                        new GuiIntSlider(SpriteName.NO_IMAGE, "Percent Fill", RoadPercentFillProperty, new IntervalF(1, 100), false, layout);

                        new GuiIntSlider(SpriteName.NO_IMAGE, "Clear above", RoadUpwardClearProperty, new IntervalF(0, 32), false, layout);
                        new GuiIntSlider(SpriteName.NO_IMAGE, "Fill below", RoadBelowFillProperty, new IntervalF(0, 32), false, layout);
                    }
                }

                if (designer.copiedVoxels != null)
                {
                    new GuiTextButton("Paste", null, designer.Paste, false, layout);
                }
                //if (PlatformSettings.DevBuild)
                //{ new GuiTextButton("New char adj", null, newCharacterSizeAdjust, false, layout); }

                if (designer.drawCoordMaterial.HasMaterial())
                {
                    new GuiIconTextButton(SpriteName.IconColorPick, designer.drawCoordMaterial.ToString(),
                        null, designer.linkPickMaterial, false, layout);
                }

                if (designer.inGame)
                {
                    new GuiTextButton("Flatten area",
                        "Will fill everything to the level of the cursor, with the selected material, and remove all above it",
                        designer.linkFLattenArea, false, layout);

                }

                new GuiTextButton("Load", null, loadOptionsMenu, true, layout);
                if (designer.inGame)
                {
                    new GuiCheckbox("Selection Cut", "Removes blocks you select with RT", bSelectionCutProperty, layout);//(int)ValueLink.SelectionCut);

                    //if (PlatformSettings.ViewUnderConstructionStuff)

                    //    file.AddIconTextLink(SpriteName.IconInfo, "Create door", (int)Link.CreateDoorInfo);
                    //
                    //new GuiTextButton("Letter blocks", "Add a row of blocks with letters on them", designer.linkTypeTextBlocks, false, layout);
                    new GuiTextButton("Exit Creation", null, designer.LinkEXIT, false, layout);
                }
                else
                {


                    new GuiTextButton("Save", null, SaveMenu, true, layout);

                    new GuiTextButton("Select All", null, designer.selectAll, false, layout);
                    new GuiTextButton("Canvas size", "Change the draw limits", LinkCanvasSize, true, layout);

                    //if (PlatformSettings.RunningWindows)
                    //new GuiTextButton("Expand Draw Limits", null, LinkExpandLimits, true, layout);

                    new GuiTextButton("Rotate/Flip", "Rotate or flip the whole model", LinkRotateFlip, true, layout);
                    new GuiTextButton("Move everything", null, MoveAllMenu, true, layout);

                    new GuiTextButton("Animation", null, animationMenu, true, layout);


                    new GuiTextButton("Clear all", "Removes all blocks and all frames", designer.LinkClearAll, false, layout);
                    new GuiTextButton("Settings", null, pageSettings, true, layout);

                    //file.AddDescription(LfLib.ViewBackText + " for help");

                    //if (editTerrain == null)
                    //{
                    new GuiTextButton("Exit", "Exit to main menu", designer.LinkEXIT, false, layout);
                    //}
                    //else
                    //{
                    //    new GuiTextButton("Save & Exit", "Store model and return to game", exitTerrainEdit, false, layout);
                    //}
                }

            } layout.End();
        }

        VoxelModelName currentConvert = VoxelModelName.CATEGORY_WARS_6 + 1;
        void quickConvertAll()
        {
            if (currentConvert < VoxelModelName.NUM_NON)
            {
                designer.storage.loadRetailModel(currentConvert);
                currentConvert++;
                new Timer.TimedAction0ArgTrigger(quickConvert_p2, 200);
            }
        }

        void quickConvert_p2()
        {
            quickConvert();

            new Timer.TimedAction0ArgTrigger(quickConvertAll, 200);
        }

        void quickConvert()
        {
            //MSG[1]: Picked: Unknown-R248 G8 B8, blockVal:61441
            //MSG[1]: Picked: Unknown-R8 G248 B8, blockVal:3841
            //designer.swapMaterials(61441, AppearanceMaterial.Material1.baseColor);
            //designer.swapMaterials(3841, AppearanceMaterial.Material2.baseColor);

            designer.storage.save();
        }

        public void LinkBGcolor()
        {
            var layout = new GuiLayout("Background color", menu);
            {
                new GuiTextButton("White", null, new GuiAction1Arg<Color>(designer.setBgCol, Color.White), false, layout);
                new GuiTextButton("Sky blue", null, new GuiAction1Arg<Color>(designer.setBgCol, Color.CornflowerBlue), false, layout);
                new GuiTextButton("Black", null, new GuiAction1Arg<Color>(designer.setBgCol, Color.Black), false, layout);
            } layout.End();
        }
        public void LinkCanvasSize()
        {
            //mFile = new HUD.File();
            var layout = new GuiLayout("Canvas Size", menu);
            {
                IntVector3 sz = designer.drawLimits.Size;
                new GuiLabel(designer.drawLimits.Size.ToString("*"), layout);//mFile.AddDescription(sz.X.ToString() + "*" + sz.Y.ToString() + "*" + sz.Z.ToString());

                new GuiTextButton("+X", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.PlusX), false, layout);
                new GuiTextButton("+Y", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.PlusY), false, layout);
                new GuiTextButton("+Z", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.PlusZ), false, layout);
                new GuiTextButton("-X", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.NegativeX), false, layout);
                new GuiTextButton("-Y", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.NegativeY), false, layout);
                new GuiTextButton("-Z", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.NegativeZ), false, layout);

                new GuiTextButton("+2X", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.PlusX * 2), false, layout);
                new GuiTextButton("+2Y", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.PlusY * 2), false, layout);
                new GuiTextButton("+2Z", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.PlusZ * 2), false, layout);
                new GuiTextButton("-2X", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.NegativeX * 2), false, layout);
                new GuiTextButton("-2Y", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.NegativeY * 2), false, layout);
                new GuiTextButton("-2Z", null, new GuiAction1Arg<IntVector3>(designer.changeCanvasSize, IntVector3.NegativeZ * 2), false, layout);

                new GuiLabel("Draw limits Width and Height", layout);
                IntVector3[] suggestedDrawLimits = new IntVector3[]
                {
                    new IntVector3(16, 24, 16),
                    new IntVector3(24),
                    new IntVector3(24, 32, 24),
                    new IntVector3(32),
                    new IntVector3(32, 48, 32),
                    new IntVector3(64, 32, 64),
                };

                foreach (IntVector3 lim in suggestedDrawLimits)
                {
                    new GuiTextButton("Set " + lim.ToString("*"), null,
                        new GuiAction1Arg<IntVector3>(designer.setCanvasSize, lim), false, layout);
                }


            } layout.End();
        }

        void MoveAllMenu()
        {
            var layout = new GuiLayout("Move All", menu);
            {
                new GuiTextButton("Move +X", null, new GuiAction1Arg<IntVector3>(designer.moveAll, IntVector3.PlusX), false, layout);
                new GuiTextButton("Move Up", null, new GuiAction1Arg<IntVector3>(designer.moveAll, IntVector3.PlusY), false, layout);
                new GuiTextButton("Move +Z", null, new GuiAction1Arg<IntVector3>(designer.moveAll, IntVector3.PlusZ), false, layout);
                new GuiTextButton("Move -X", null, new GuiAction1Arg<IntVector3>(designer.moveAll, IntVector3.NegativeX), false, layout);
                new GuiTextButton("Move Down", null, new GuiAction1Arg<IntVector3>(designer.moveAll, IntVector3.NegativeY), false, layout);
                new GuiTextButton("Move -Z", null, new GuiAction1Arg<IntVector3>(designer.moveAll, IntVector3.NegativeZ), false, layout);

                allFramesChkBox(layout);

            } layout.End();
        }

        public void loadOptionsMenu()
        {
            //mFile = new HUD.File();
            GuiLayout layout = new GuiLayout("Load options", menu);
            {
                loadUserCreationButton(layout);//new GuiTextButton("User creation", "Models you saved", beginListUserModelsPage, true, layout);
                loadRetailModelButton(layout);//new GuiTextButton("Retail model", "Lists all models used in the game", retailModelCategories, true, layout);
                new GuiTextButton("Template", "You can store away selections from both the editor and in game",
                    listTemplates, true, layout);

                new GuiCheckbox("Combine", "Will merge with the current model", bCombineLoadedModelProperty, layout);//(int)ValueLink.CombineLoadedModel);

            } layout.End();
            //menu.File = mFile;
        }

        protected void loadUserCreationButton(GuiLayout layout)
        {
            new GuiTextButton("User creation", "Models you saved", beginListUserModelsPage, true, layout);
        }
        protected void loadRetailModelButton(GuiLayout layout)
        {
            new GuiTextButton("Retail model", "Lists all models used in the game", retailModelCategories, true, layout);
        }

        void beginListUserModelsPage()
        {
            new Process.AsynchMenuPage<List<string>, int>(asynchListUserModelsPage, int.MinValue, endListUserModelsPage, menu);
        }

        List<string> asynchListUserModelsPage(int none)
        {
            return DataLib.SaveLoad.FilesInStorageDir(DesignerStorage.UserVoxelObjFolder, VoxelDesigner.searchPattern(false));
        }

        void endListUserModelsPage(List<string> files2, int none)
        {
            var layout = new GuiLayout(SpriteName.NO_IMAGE, "User models", menu, HUD.GuiLayoutMode.MultipleColumns);
            {
                if (files2.Count == 0)
                {
                    new GuiIconTextButton(menu.style.headReturnIcon, "No models", null, menu.PopLayout, false, layout);
                }

                for (int i = 0; i < files2.Count; i++)
                {
                    if (menuModelIcons)
                    {
                        new GuiVoxelModelIcon(SpriteName.IconSandGlass, DesignerStorage.CustomVoxelObjPath(files2[i]),
                            "Load \"" + files2[i] + "\"", new GuiAction1Arg<string>(loadUserModelLink, files2[i]), layout);
                    }
                    else
                    {
                        new GuiTextButton(files2[i], null, new GuiAction1Arg<string>(loadUserModelLink, files2[i]), false, layout);
                    }
                }


            } layout.End();
        }

        virtual protected void loadUserModelLink(string name)
        {
            designer.storage.loadUserModel(name);
        }


        void retailModelCategories()
        {
            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                VoxelModelName[] available = new VoxelModelName[]
                    {
                        VoxelModelName.war_worker,
                        VoxelModelName.war_recruit,
                        VoxelModelName.wars_soldier,
                        VoxelModelName.war_archer,
                        VoxelModelName.war_bannerman,
                        VoxelModelName.banner,
                        VoxelModelName.horsebanner,
                        VoxelModelName.war_farmerneutral,
                        VoxelModelName.war_folkman,
                        VoxelModelName.war_ballista,
                        VoxelModelName.war_sailor,
                        VoxelModelName.little_hirdman,
                        VoxelModelName.war_knight,
                        VoxelModelName.horse_brown,
                        VoxelModelName.horse_white,

                        //VoxelModelName.warmap_grass1,
                        //VoxelModelName.warmap_grassdark1,
                        //VoxelModelName.warmap_mountain1,
                        //VoxelModelName.warmap_mountaindark1,
                        //VoxelModelName.warmap_sand1,
                        //VoxelModelName.warmap_sanddark1,
                        VoxelModelName.war_town1,
                        VoxelModelName.war_town2,
                        VoxelModelName.war_town3,
                        VoxelModelName.war_workerhut,
                        VoxelModelName.armystand,

                    };

                var layout = new GuiLayout("Load Model", menu);
                {
                    foreach (var name in available)
                    {
                        new GuiTextButton(name.ToString(), null,
                          new GuiAction1Arg<VoxelModelName>(loadRetailModel, name),
                          false, layout);
                    }
                }
                layout.End();
               

            }
            else
            {
                var layout = new GuiLayout("Select Category", menu);
                {
                    for (ModelCategory cat = 0; cat < ModelCategory.NUM_NON; ++cat)
                    {
                        new GuiTextButton(cat.ToString(), null, new GuiAction1Arg<ModelCategory>(listModelsFromCategory, cat), true, layout);
                    }
                }
                layout.End();
            }
        }

        

        void listModelsFromCategory(ModelCategory cat)
        {
            VoxelModelName start, end;

            switch (cat)
            {
                case ModelCategory.Wars:
                    start = VoxelModelName.CATEGORY_WARS_6 + 1;
                    end = VoxelModelName.NUM_NON;
                    break;
                case ModelCategory.Other:
                    start = VoxelModelName.CATEGORY_OTHER_5 + 1;
                    end = VoxelModelName.CATEGORY_WARS_6;
                    break;
                case ModelCategory.BlockPattern:
                    start = VoxelModelName.CATEGORY_BLOCKPATTERN_4 + 1;
                    end = VoxelModelName.CATEGORY_OTHER_5;
                    break;
                case ModelCategory.Terrain:
                    start = VoxelModelName.CATEGORY_TERRAIN_3 + 1;
                    end = VoxelModelName.CATEGORY_BLOCKPATTERN_4;
                    break;
                case ModelCategory.Appearance:
                    start = VoxelModelName.CATEGORY_APPEARANCE_2 + 1;
                    end = VoxelModelName.CATEGORY_TERRAIN_3;
                    break;
                case ModelCategory.Weapon:
                    start = VoxelModelName.CATEGORY_WEAPON_1 + 1;
                    end = VoxelModelName.CATEGORY_APPEARANCE_2;
                    break;
                case ModelCategory.Character:
                    start = VoxelModelName.CATEGORY_CHARACTER_0 + 1;
                    end = VoxelModelName.CATEGORY_WEAPON_1;
                    break;
                default:
                    start = 0;
                    end = VoxelModelName.NUM_NON;
                    break;
            }

            listRetailModels(start, end);
        }

        public void listTemplates()
        {
            BeginListTemplatesInCathegory(0);
            //var layout = new GuiLayout(SpriteName.NO_IMAGE, "Templates", menu, GuiLayoutMode.MultipleColumns);
            //{
            //    listTemplatesBase(layout);
            //} layout.End();

        }
        void listTemplatesBase(GuiLayout layout)
        {
            for (int i = 1; i < (int)SaveCategory.NUM; i++)
            {
                if (DesignerStorage.HasChatergory[i])
                {
                    SaveCategory cat = (SaveCategory)i;
                    SpriteName id = CategoryIcon(cat);
                    new GuiIcon(id, cat.ToString(), new GuiActionIndex(BeginListTemplatesInCathegory, i), false, layout);
                }
            }
        }

        static SpriteName CategoryIcon(SaveCategory c)
        {
            SpriteName result = SpriteName.RightStick;
            //switch (c)
            //{
            //    case SaveCategory.castle:
            //        result = SpriteName.FolderCastle;
            //        break;
            //    case SaveCategory.squares:
            //        result = SpriteName.FolderSquares;
            //        break;
            //    case SaveCategory.house:
            //        result = SpriteName.FolderHouse;
            //        break;
            //    case SaveCategory.veihcle:
            //        result = SpriteName.FolderVeihcle;
            //        break;
            //    case SaveCategory.art:
            //        result = SpriteName.FolderArt;
            //        break;
            //    case SaveCategory.animals:
            //        result = SpriteName.FolderAnimals;
            //        break;
            //    case SaveCategory.smiley:
            //        result = SpriteName.FolderSmileys;
            //        break;
            //    case SaveCategory.space:
            //        result = SpriteName.FolderSpace;
            //        break;
            //    case SaveCategory.roadSign:
            //        result = SpriteName.FolderRoadSign;
            //        break;
            //    case SaveCategory.temporary:
            //        result = SpriteName.FolderTimer;
            //        break;
            //    case SaveCategory.terrain:
            //        result = SpriteName.FolderTerrain;
            //        break;
            //    case SaveCategory.character:
            //        result = SpriteName.FolderCharacer;
            //        break;
            //    case SaveCategory.dontKnow:
            //        result = SpriteName.FolderQuestion;
            //        break;
            //    case SaveCategory.furniture:
            //        result = SpriteName.FolderFurniture;
            //        break;
            //    case SaveCategory.tools:
            //        result = SpriteName.FolderTools;
            //        break;
            //}
            return result;
        }

        void listRetailModels(VoxelModelName start, VoxelModelName end)
        {
            List<ComparableKeys<string, VoxelModelName>> models = new List<ComparableKeys<string, VoxelModelName>>((int)(end - start));
            for (VoxelModelName v = start; v < end; ++v)
            {
                if (v != VoxelModelName.CATEGORY_CHARACTER_0 &&
                    v != VoxelModelName.CATEGORY_WEAPON_1 &&
                    v != VoxelModelName.CATEGORY_APPEARANCE_2 &&
                    v != VoxelModelName.CATEGORY_TERRAIN_3 &&
                    v != VoxelModelName.CATEGORY_OTHER_5 &&
                    v != VoxelModelName.CATEGORY_WARS_6)
                {
                    models.Add(new ComparableKeys<string, VoxelModelName>(v.ToString(), v));
                }
            }

            ComparableKeys<string, VoxelModelName>[] modelsArray = models.ToArray();

            Array.Sort(modelsArray);

            var layout = new GuiLayout(SpriteName.NO_IMAGE, "Retail models", menu, HUD.GuiLayoutMode.MultipleColumns);
            {
                for (int i = modelsArray.Length - 1; i >= 0; --i)
                {
                    if (menuModelIcons)
                    {
                        new GuiVoxelModelIcon(SpriteName.IconSandGlass, VoxelObjDataLoader.ContentPath(modelsArray[i].value),
                            "Load \"" + modelsArray[i].compareKey + "\"", new GuiAction1Arg<VoxelModelName>(loadRetailModel, modelsArray[i].value), layout);
                    }
                    else
                    {
                        new GuiTextButton(modelsArray[i].compareKey, null,
                            new GuiAction1Arg<VoxelModelName>(loadRetailModel, modelsArray[i].value),
                            false, layout);
                    }
                }
            } layout.End();
        }

        virtual protected void loadRetailModel(VoxelModelName modelName)
        {
            designer.storage.loadRetailModel(modelName);
        }

        void BeginListTemplatesInCathegory(int cathegory)
        {
            new Process.AsynchMenuPage<List<string>, int>(ListTemplatesInCathegory_Asynch, cathegory, EndListTemplatesInCathegory, menu);
        }

        List<string> ListTemplatesInCathegory_Asynch(int cathegory)//int cathegory, int menuId)
        {
            var path = DesignerStorage.TemplatePath(cathegory, null);
            List<string> files = DataLib.SaveLoad.FilesInStorageDir(path.LocalDirectoryPath);
            return files;
        }
        void EndListTemplatesInCathegory(List<string> files, int cathegory)
        {
            var layout = new GuiLayout(SpriteName.NO_IMAGE, "Templates", menu, GuiLayoutMode.MultipleColumns);
            {
                var path = DesignerStorage.TemplatePath(cathegory, null);
                for (int i = files.Count - 1; i >= 0; i--)
                {
                    path.FileName = files[i];
                    new GuiVoxelModelIcon(SpriteName.IconSandGlass, path, "Load template",
                        new GuiAction1Arg<VikingEngine.DataStream.FilePath>(LinkSelectTemplateFile_dialogue, path), layout);
                }

            } layout.End();
        }
        void LinkSelectTemplateFile_dialogue(FilePath path)//string fileName)
        {
            //open a bigger image of the rotating object
            //be able to delete or change category, and see file date/size
            var layout = new GuiLayout(path.FileName, menu);
            {
                //long ticks = Convert.ToInt64(path.FileName);
                //DateTime saveDate = new DateTime().AddTicks(ticks);
                new GuiLabel(path.FileName, layout);

                new GuiTextButton("Use", null, new HUD.GuiAction1Arg<FilePath>(designer.LinkTemplateUse, path), false, layout);
                new GuiDialogButton("Delete Template", null, new HUD.GuiAction1Arg<FilePath>(designer.LinkTemplateDeleteOK, path), false, layout);
            } layout.End();
        }

        void listTemplates_Asynch(int menuId)
        {
            if (menu != null && menu.Visible && menuId == menu.PageId)
            {
                listTemplates();
            }
        }

        public override void selectionMenu()
        {
            openMenu();

            var layout = new GuiLayout("Selection Menu", menu);
            {
                selectionMenuBase(layout);
                new GuiTextButton("Clear", "Removes all blocks in the selected area", designer.clearSelectedArea, false, layout);
                if (designer.animationFrames != null && designer.animationFrames.Frames.Count > 1)
                {
                    new GuiTextButton("Stamp All frames", "Paste the voxels on all frames", designer.LinkStampAllFrames, false, layout);
                    new GuiTextButton("Clear in frames", "Remove blocks in the selected area from all frames", designer.clearSelectedArea_AllFrames, false, layout);
                    new GuiTextButton("Clear other Frames", "Remove blocks from all frames except this", designer.clearSelectedArea_AllFramesButThis, false, layout);
                }

                if (!designer.inGame)
                    new GuiTextButton("Set limits from selection", "Will shrink the draw limit to the selected area", designer.LinkSetLimitsAfterSel, false, layout);


            } layout.End();
        }

        void pageSettings()
        {
            GuiLayout layout = new GuiLayout("Settings", menu);
            {
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Move speed", designer.Settings.moveSpeedProperty, new IntervalF(0.1f, 4f), false, layout);
                new GuiTextButton("Background Color", null, LinkBGcolor, true, layout);
                new GuiTextButton("Hide HUD", "View only the model, great for screen capture", designer.LinkHideHUD, false, layout);
            } layout.End();
        }

        

        void selectColorMenu()
        {
            GuiLayout layout = new GuiLayout("Color Options", menu, GuiLayoutMode.MultipleColumns, null);
            {
                Color current = VikingEngine.LootFest.Map.HDvoxel.BlockHD.FilterColor(designer.SelectedMaterial.color);
                Color brighter = ColorExt.ChangeColor(current, BlockHD.ColorStep, BlockHD.ColorStep, BlockHD.ColorStep);
                Color darker = ColorExt.ChangeColor(current, -BlockHD.ColorStep, -BlockHD.ColorStep, -BlockHD.ColorStep);
                Color brighter2 = ColorExt.ChangeColor(current, BlockHD.ColorStep * 2, BlockHD.ColorStep * 2, BlockHD.ColorStep * 2);
                Color darker2 = ColorExt.ChangeColor(current, -BlockHD.ColorStep * 2, -BlockHD.ColorStep * 2, -BlockHD.ColorStep * 2);
                Color redTint = ColorExt.ChangeColor(current, BlockHD.ColorStep, -BlockHD.ColorStep, -BlockHD.ColorStep);
                Color greenTint = ColorExt.ChangeColor(current, -BlockHD.ColorStep, BlockHD.ColorStep, -BlockHD.ColorStep);
                Color blueTint = ColorExt.ChangeColor(current, -BlockHD.ColorStep, -BlockHD.ColorStep, BlockHD.ColorStep);
                Color yellowTint = ColorExt.ChangeColor(current, BlockHD.ColorStep, BlockHD.ColorStep, -BlockHD.ColorStep);
                Color purpleTint = ColorExt.ChangeColor(current, BlockHD.ColorStep, -BlockHD.ColorStep, BlockHD.ColorStep);


                colorTintButton(brighter2, false, "Brighter+", layout, designer.pickColorLink);
                colorTintButton(brighter, false, "Brighter", layout, designer.pickColorLink);

                colorTintButton(current, true, "Current", layout, designer.pickColorLink);

                colorTintButton(darker, false, "Darker", layout, designer.pickColorLink);
                colorTintButton(darker2, false, "Darker+", layout, designer.pickColorLink);

                colorTintButton(redTint, false, "Red tint", layout, designer.pickColorLink);
                colorTintButton(greenTint, false, "Green tint", layout, designer.pickColorLink);
                colorTintButton(blueTint, false, "Blue tint", layout, designer.pickColorLink);
                colorTintButton(yellowTint, false, "Yellow tint", layout, designer.pickColorLink);
                colorTintButton(purpleTint, false, "Purple tint", layout, designer.pickColorLink);


                new GuiTextButton("Pick Hue", null, designer.openColorPicker, false, layout);

                new GuiIntSlider(SpriteName.NO_IMAGE, "R", redProperty, RGBrange, false, layout);
                new GuiIntSlider(SpriteName.NO_IMAGE, "G", greenProperty, RGBrange, false, layout);
                new GuiIntSlider(SpriteName.NO_IMAGE, "B", blueProperty, RGBrange, false, layout);
                new GuiSectionSeparator(layout);

                colorPalette(layout, designer.pickColorLink);

            } layout.End();
        }

        public void colorPalette(GuiLayout layout, Action<BlockHD> link)
        {
            var inUse = designer.materialsInUse(true);
            foreach (var m in inUse)
            {
                ColorButton(BlockHD.ToColor(m), layout, link);
            }

            new GuiSectionSeparator(layout);

            Color[] MainColors = new Color[]
                {
                    Color.White,
                    Color.LightGray,
                    Color.DarkGray,
                    new Color(100, 100, 100),
                    ColorExt.VeryDarkGray,
                    Color.Black,
                    
                    Color.Red,
                    Color.Orange,
                    Color.Yellow,
                    Color.Green,
                    Color.Purple,
                    Color.Blue,
                };

            foreach (var m in MainColors)
            {
                ColorButton(VikingEngine.LootFest.Map.HDvoxel.BlockHD.FilterColor(m), layout, link);
            }

            new GuiSectionSeparator(layout);

            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                DSSSoldierPalette(layout, link);
//#if RTS
//                appearanceMaterials(DSSWars.ProfileData.SkinCol, "Skin", layout, link);
//                appearanceMaterials(DSSWars.ProfileData.HairCol, "Hair", layout, link);
//                appearanceMaterials(DSSWars.ProfileData.MainCol, "Main", layout, link);
//                appearanceMaterials(DSSWars.ProfileData.AltMainCol, "Alt Main", layout, link);
//                appearanceMaterials(DSSWars.ProfileData.DetailCol1, "Detail1", layout, link);
//                appearanceMaterials(DSSWars.ProfileData.DetailCol2, "Detail2", layout, link);
//#endif
            }
            else
            {
                appearanceMaterials(AppearanceMaterial.Material1, "1", layout, link);
                appearanceMaterials(AppearanceMaterial.Material2, "2", layout, link);
                appearanceMaterials(AppearanceMaterial.Material3, "3", layout, link);
                appearanceMaterials(AppearanceMaterial.Material4, "4", layout, link);
                appearanceMaterials(AppearanceMaterial.Material5, "5", layout, link);
            }
            new GuiSectionSeparator(layout);


            const int HueCount = 16;
            const int LightnessCount = 16;

            double[] Saturations = new double[] { 0.5, 0.25 };

            foreach (var saturate in Saturations)
            {
                for (int hue = 0; hue < HueCount; ++hue)
                {
                    for (int light = LightnessCount - 1; light >= 1; --light)
                    {
                        Color col = lib.HSL2RGB((double)hue / HueCount, saturate, (double)light / LightnessCount);
                        col = VikingEngine.LootFest.Map.HDvoxel.BlockHD.FilterColor(col);
                        ColorButton(col, layout, link);
                    }
                }
            }
        }

        public void DSSSoldierPalette(GuiLayout layout, Action<BlockHD> link)
        {
            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                new GuiTitle("DSS soldier color mapping", layout);
                //SkinCol, HairCol, MainCol, AltMainCol, DetailCol1, DetailCol2;
                appearanceMaterials(VikingEngine.DSSWars.FlagAndColor.SkinCol, "skin", layout, link);
                appearanceMaterials(VikingEngine.DSSWars.FlagAndColor.HairCol, "hair", layout, link);
                appearanceMaterials(VikingEngine.DSSWars.FlagAndColor.MainCol, "main 1", layout, link);
                appearanceMaterials(VikingEngine.DSSWars.FlagAndColor.AltMainCol, "main 2", layout, link);
                appearanceMaterials(VikingEngine.DSSWars.FlagAndColor.DetailCol1, "detail 1", layout, link);
                appearanceMaterials(VikingEngine.DSSWars.FlagAndColor.DetailCol2, "detail 2", layout, link);

            }
        }

        public static void BigPalette(GuiLayout layout, Action<BlockHD> link)
        {
            const int HueCount = 32;
            const int LightnessCount = 16;

            ColorButton(Color.Black, layout, link);
            ColorButton(Color.White, layout, link);

            double[] Saturations = new double[] { 0.5, 0.4, 0.3, 0.1 };

            foreach (var saturate in Saturations)
            {
                for (int hue = 0; hue < HueCount; ++hue)
                {
                    for (int light = LightnessCount - 1; light >= 1; --light)
                    {
                        Color col = lib.HSL2RGB((double)hue / HueCount, saturate, (double)light / LightnessCount);
                        col = VikingEngine.LootFest.Map.HDvoxel.BlockHD.FilterColor(col);
                        ColorButton(col, layout, link);
                    }
                }
            }
        }

        void appearanceMaterials(AppearanceMaterial mat, string type, GuiLayout layout, Action<BlockHD> link)
        {
            string materialName = "Material " + type;
            appearanceMaterialsButton(true, mat.baseColor, materialName + " base", layout, link);
            appearanceMaterialsButton(false, mat.brighter, materialName + " bright", layout, link);
            appearanceMaterialsButton(false, mat.darker, materialName + " dark", layout, link);
            appearanceMaterialsButton(false, mat.redTint, materialName + " red", layout, link);
        }

        void appearanceMaterialsButton(bool bigIcon, ushort col, string name, GuiLayout layout, Action<BlockHD> link)
        {
            //Color color = BlockHD.ToColor(col);
            BlockHD color = new BlockHD(col);
            GuiIcon icon;
            //if (bigIcon)
            //{ 
            icon = new GuiIcon(SpriteName.WhiteArea, name, new GuiAction1Arg<BlockHD>(link, color), false, layout);
            //}
            //else
            //{
            //    icon = new GuiSmallIcon(SpriteName.WhiteArea, name, new GuiAction1Arg<Color>(link, color), false, layout); 
            //}
            icon.iconImage.Color = color.color;
            if (!bigIcon)
            {
                icon.iconImage.Size *= 0.7f;
            }
        }

        void colorTintButton(Color col, bool currentCol, string text, GuiLayout layout, Action<BlockHD> link)
        {
            var icon = new GuiIcon(currentCol? SpriteName.LittleUnitSelectionFilled : SpriteName.WhiteArea, text, new GuiAction1Arg<BlockHD>(link, new BlockHD(col)), false, layout);
            icon.iconImage.Color = col;

        }

        static void ColorButton(Color col, GuiLayout layout, Action<BlockHD> link)
        {
            var icon = new GuiSmallIcon(SpriteName.WhiteArea, col.ToString(), new GuiAction1Arg<BlockHD>(link, new BlockHD(col)), false, layout);
            icon.iconImage.Color = col;
        }

        void listPremadeTemplates(string folder)
        {
            var layout = new GuiLayout("Select model", menu);
            {
               var files = DataLib.SaveLoad.FilesInContentDir(folder);
                for (int i = files.Length - 1; i >= 0; i--)
                {
                    //file.Add(new TemplateIconData(
                    //    new FilePath(
                    //    folder, files[i], TemplateFileEnd, false),
                    //    new HUD.Link((int)dialogue, i)));
                }
            } layout.End();
        }

        void SaveMenu()
        {
            //mFile = new HUD.File();
            var layout = new GuiLayout(designer.storage.saveFileName, menu);
            {
                if (PlatformSettings.RunningWindows)
                {
                    new GuiLabel("All files end up in \"" + DesignerStorage.UserVoxelObjFolder + "\"", layout);
                }
                new GuiTextInputbox(designer.storage.saveFileName, designer.onFileNameChange, layout);
                //new GuiTextButton("Replace Old Save", "Will use the current name", new GuiAction(save, closeMenu), false, layout);
                new GuiTextButton("Save", "Save a .vox file", new GuiAction(designer.storage.save, closeMenu), false, layout);
                //new GuiTextButton("Export as .OBJ", null, designer.exportObjModel, false, layout);
                //new GuiTextButton("Export as textfile", null, designer.exportTextFile, false, layout);
            } layout.End();
            //menu.File = mFile;
        }

        void animationMenu()
        {
            GuiLayout layout = new GuiLayout("Animation", menu);
            {
                new GuiTextButton("Add frame", "Add another frame to the animation", designer.LinkAnimAddFrame, false, layout);

                if (designer.haveAnimation)
                {
                    new GuiTextButton("Prev frame", null, new GuiAction1Arg<bool>(designer.nextFrame, false), false, layout);
                    new GuiTextButton("Next frame", null, new GuiAction1Arg<bool>(designer.nextFrame, true), false, layout);
                    new GuiTextButton("Remove current frame", null, designer.RemoveCurrentFrame, false, layout);
                    new GuiTextButton("Clear Animation", "Remove all frames but this one",
                        new GuiAction(designer.RemoveAllFramesButThis, closeMenu), false, layout);
                    //new GuiTextButton("Move frame", "Change the order of frames in the animation", (int)Link.AnimMoveFrame);
                    new GuiLabel("Move current frame:", layout);
                    new GuiTextButton("Forward", null, new GuiAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.Forward), false, layout);
                    new GuiTextButton("Back", null, new GuiAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.Back), false, layout);
                    new GuiTextButton("To start", null, new GuiAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.ToStart), false, layout);
                    new GuiTextButton("To end", null, new GuiAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.ToEnd), false, layout);
                    if (designer.lockFirstFrames == 0)
                    {
                        if (designer.currentFrame.Value > 0)
                        {
                            new GuiTextButton("Lock as first frame", "Frames before this wont be seen when testing the animation",
                               designer.LinkAnimLockFrame, false, layout);
                        }
                    }
                    else
                    {
                        new GuiTextButton("Remove frame lock", "Will view the first frame again", designer.LinkAnimUnlockFrame, false, layout);
                    }
                }
            } layout.End();
        }

        public void mergeOptions(VoxelObjGridDataAnimHD loadedModel)
        {
            const string FramesTxt = "Frames: ";
            const string SizeTxt = "Size: ";

            var layout = new GuiLayout("Merge options", menu);
            {
                //view merge options

                new GuiLabel("Current", layout);
                new GuiLabel(FramesTxt + designer.animationFrames.Frames.Count.ToString(), layout);
                new GuiLabel(SizeTxt + designer.animationFrames.Size.ToString(), layout);
                new GuiLabel("Loaded model", layout);
                new GuiLabel(FramesTxt + loadedModel.Frames.Count.ToString(), layout);
                new GuiLabel(SizeTxt + loadedModel.Size.ToString(), layout);
                new GuiCheckbox("Override", "The new blocks will replace the old", bMergeNewOverrideProperty, layout); //(int)ValueLink.bMergeNewOverride);
                new GuiCheckbox("Keep size", "keep the current grid size", bMergeKeepSizeProperty, layout);//(int)ValueLink.bMergeKeepSize);

                new GuiTextButton(MergeFramesOptions.NewFirstOnOldFrames.ToString(),
                    "Take the first frame from the loaded model and stamp on all",
                    new HUD.GuiAction2Arg<MergeFramesOptions, VoxelObjGridDataAnimHD>
                        (designer.selectMergeOption, MergeFramesOptions.NewFirstOnOldFrames, loadedModel),
                        false, layout);
                new GuiTextButton(MergeFramesOptions.FrameByFrame.ToString(),
                    "Merged frame by frame",
                    new HUD.GuiAction2Arg<MergeFramesOptions, VoxelObjGridDataAnimHD>
                        (designer.selectMergeOption, MergeFramesOptions.FrameByFrame, loadedModel),
                        false, layout);
                new GuiTextButton(MergeFramesOptions.OldFirstOnNewFrames.ToString(),
                    "Only keep the first frame of current model and use on the loaded one",
                    new HUD.GuiAction2Arg<MergeFramesOptions, VoxelObjGridDataAnimHD>
                        (designer.selectMergeOption, MergeFramesOptions.OldFirstOnNewFrames, loadedModel),
                        false, layout);

            } layout.End();
        }

        

        int redProperty(bool set, int value)
        {
            return colorProperty(set, Dimensions.X, value);
        }
        int greenProperty(bool set, int value)
        {
            return colorProperty(set, Dimensions.Y, value);
        }
        int blueProperty(bool set, int value)
        {
            return colorProperty(set, Dimensions.Z, value);
        }

        int colorProperty(bool set, Dimensions dim, int value)
        {
            if (set)
            {
                designer.SelectedMaterial.SetColor(dim, (byte)value);
            }
            return designer.SelectedMaterial.GetColor(dim);
        }

        int pencilSizeProperty(bool set, int value)
        {
            if (set) { designer.Settings.PencilSize = value; }
            return designer.Settings.PencilSize;
        }

        float radiusToleranceProperty(bool set, float value)
        {
            if (set) { designer.Settings.RadiusTolerance = value; }
            return designer.Settings.RadiusTolerance;
        }

        int RoadUpwardClearProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadUpwardClear = value; }
            return designer.Settings.RoadUpwardClear;
        }
        int RoadBelowFillProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadBelowFill = value; }
            return designer.Settings.RoadBelowFill;
        }
        int RoadEdgeSizeProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadEdgeSize = value; }
            return designer.Settings.RoadEdgeSize;
        }
        int RoadPercentFillProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadPercentFill = value; }
            return designer.Settings.RoadPercentFill;
        }

        bool bSelectionCutProperty(int index, bool set, bool value)
        {
            if (set) { designer.Settings.SelectionCut = value; }
            return designer.Settings.SelectionCut;
        }
        bool bRoundPencilProperty(int index, bool set, bool value)
        {
            if (set) { designer.Settings.RoundPencil = value; }
            return designer.Settings.RoundPencil;
        }
        bool bCombineLoadedModelProperty(int index, bool set, bool value)
        {
            if (set) { designer.combineLoading = value; }
            return designer.combineLoading;
        }
        bool bMergeKeepSizeProperty(int index, bool set, bool value)
        {
            if (set) { designer.mergeModelsOption.KeepOldGridSize = value; }
            return designer.mergeModelsOption.KeepOldGridSize;
        }
        bool bMergeNewOverrideProperty(int index, bool set, bool value)
        {
            if (set) { designer.mergeModelsOption.NewBlocksReplaceOld = value; }
            return designer.mergeModelsOption.NewBlocksReplaceOld;
        }

        //PaintToolType toolProperty(bool set, PaintToolType value)
        //{
        //    if (set) { designer.Settings.DrawTool = value; }
        //    return designer.Settings.DrawTool;
        //}

        

        public void DeleteMe()
        {
            if (menu != null)
            {
                menu.DeleteMe();
            }
        }
    }
}
