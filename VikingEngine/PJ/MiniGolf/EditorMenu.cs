using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.HUD;

namespace VikingEngine.PJ.MiniGolf
{
    class EditorMenu
    {
        public Gui menu;

        public EditorMenu()
        {  }

        public void OpenMenu()
        {
            if (menu == null)
            {
                menu = new Gui(GuiStyle(), Screen.SafeArea, 0f, ImageLayers.Foreground7, Input.InputSource.DefaultPC);

                Input.Mouse.LockToScreenArea = false;
            }
        }

        public void main()
        {
            OpenMenu();

            GuiLayout layout = new GuiLayout("Options", menu);
            {
                new GuiIntSlider(SpriteName.NO_IMAGE, "Tool Index", toolIndexProperty, new IntervalF(0, 19), false, layout);
                new GuiTextButton("Tool (" + GolfRef.editor.tool.ToString() + ")", null, selectToolMenu, true, layout);
                new GuiTextButton("Clear map", null, new GuiAction(clearMapLink), false, layout);
                new GuiSectionSeparator(layout);
                new GuiTextInputbox(GolfRef.field.storage.saveFileName, onFileNameChange, layout);
                new GuiTextButton("Save Map", null, save, false, layout);
                new GuiTextButton("Load Map", null, listSaveFiles, false, layout);
                new GuiTextButton("Copy Retail Map", null, listRetailMaps, false, layout);
            }
            layout.End();
        }

        void selectToolMenu()
        {
            GuiLayout layout = new GuiLayout("Tools", menu);
            {
                new GuiIntSlider(SpriteName.NO_IMAGE, "Tool Index", toolIndexProperty, new IntervalF(0, 19), false, layout);
                new GuiTextButton("Edges", null, new GuiAction1Arg<GolfEditorTool>(toolLink, GolfEditorTool.Edges), false, layout);
                new GuiTextButton("Area", null, new GuiAction1Arg<GolfEditorTool>(toolLink, GolfEditorTool.Area), false, layout);
                new GuiTextButton("Choke", null, new GuiAction1Arg<GolfEditorTool>(toolLink, GolfEditorTool.Choke), false, layout);
                new GuiTextButton("Launch Cannon", null, new GuiAction1Arg<GolfEditorTool>(toolLink, GolfEditorTool.LaunchCannon), false, layout);

                List<GuiOption<Dir8>> launchAngles = new List<GuiOption<Dir8>>((int)Dir8.NUM);
                for (Dir8 d = 0; d < Dir8.NUM; ++d)
                {
                    launchAngles.Add(new GuiOption<Dir8>(d.ToString(), d));
                }

                new GuiOptionsList<Dir8>(SpriteName.NO_IMAGE, "Launch angle", launchAngles, launchAngleProperty, layout);
            }
            layout.End();
        }

        Dir8 launchAngleProperty(bool set, Dir8 value)
        {
            if (set)
            {
                GolfRef.field.launchAngle = value;
            }
            return GolfRef.field.launchAngle;
        }

        int toolIndexProperty(bool set, int value)
        {
            if (set)
            {
                GolfRef.editor.toolIndex = value;
            }
            return GolfRef.editor.toolIndex;
        }

        void toolLink(GolfEditorTool tool)
        {
            GolfRef.editor.tool = tool;
            CloseMenu();
        }

        void save()
        {
            new Timer.AsynchActionTrigger(GolfRef.field.storage.saveAsynch, true);
            CloseMenu();
        }

        void clearMapLink()
        {
            GolfRef.field.clearMap(true);
            CloseMenu();
        }

        void listRetailMaps()
        {
            var layout = new GuiLayout("Load Retail Level", menu);
            {
                foreach (var lvl in FieldStorage.RetailLevels)
                {
                    new GuiTextButton(lvl.ToString(), null, new GuiAction1Arg<LevelName>(GolfRef.field.storage.loadLevel, lvl), false, layout);
                }
            }
            layout.End();
        }

        void listSaveFiles()
        {
            var layout = new GuiLayout("Loading...", menu);
            {
                new GuiTextButton("Cancel", null, menu.PopLayout, false, layout);
            }
            layout.End();

            new Timer.Asynch1ArgTrigger<int>(loadFiles_Asynch, menu.PageId, true);
        }

        void loadFiles_Asynch(int pageId)
        {
            var f = FieldStorage.file(true);
            f.FileName = "";
            string[] files = DataStream.DataStreamHandler.SearchFilesInStorageDir(f, true);
            new Timer.Action2ArgTrigger<int, string[]>(loadFilesComplete, pageId, files);
        }

        void loadFilesComplete(int pageId, string[] files)
        {
            if (Open &&
                pageId == menu.PageId)
            {
                menu.PopLayout();


                var layout = new GuiLayout("Load Map", menu);
                {
                    if (files.Length == 0)
                    {
                        new GuiLabel("No files", layout);
                    }
                    else
                    {
                        foreach (var m in files)
                        {
                            new GuiTextButton(m, null, new GuiAction2Arg<string, bool>(GolfRef.field.storage.loadLevel, m, true), false, layout);
                        }
                    }
                }
                layout.End();
            }
        }

        void onFileNameChange(int user, string result, int index)
        {
            GolfRef.field.storage.saveFileName = TextLib.checkFileName(result);
        }

        void closeMenuLink()
        {
            CloseMenu();
        }

        public bool CloseMenu()
        {
            if (menu != null)
            {   
                menu.DeleteMe();
                menu = null;
                
                return true;
            }
            return false;
        }

        /// <returns>Close me</returns>
        public bool Update()
        {
            if (menu.Update() || CloseMenuInput())
            {
                return true;
            }
            return false;
        }

        public static GuiStyle GuiStyle()
        {
            var style = new GuiStyle(
                (Screen.PortraitOrientation ? Screen.Width : Screen.Height) * 0.61f, 5, SpriteName.LFMenuRectangleSelection);
           
            return style;
        }

        public static bool CloseMenuInput()
        {
            return Input.Keyboard.KeyDownEvent(Keys.Escape) ||
                Input.XInput.KeyDownEvent(Buttons.Start) ||
                Input.XInput.KeyDownEvent(Buttons.Back);
        }

        public bool Open { get { return menu != null; } }
    }

    enum GolfEditorTool
    {
        Edges,
        Area,
        Choke,
        LaunchCannon,
    }
}


