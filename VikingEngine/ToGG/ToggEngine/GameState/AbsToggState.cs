using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.ToGG
{
    abstract class AbsToggState : Engine.GameState
    {
        protected Graphics.Text2 netStatusText;

        public AbsToggState()
            : base()
        {
            Input.Mouse.LockToScreenArea = DefaultMouseLock;
        }

        protected void createNetStatusText()
        {
            netStatusText = new Graphics.Text2("Offline", LoadedFont.Regular, Engine.Screen.SafeArea.RightTop,
                Engine.Screen.TextBreadHeight, Color.Yellow, ImageLayers.Background0);
            netStatusText.OrigoAtRight();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            Ref.lobby.update();
        }

        protected void listSaveFiles(bool storage)
        {
            var layout = new GuiLayout("Loading...", toggRef.menu.menu);
            {
                new GuiTextButton("Cancel", null, toggRef.menu.menu.PopLayout, false, layout);
            }
            layout.End();

            new Timer.Asynch2ArgTrigger<int, bool>(loadFiles_Asynch, toggRef.menu.menu.PageId, storage);
        }

        void loadFiles_Asynch(int pageId, bool storage)
        {
            var f = Data.FileManager.MapfolderPath(storage);
            f.FileName = "";
            string[] files = f.listFilesInDir();
            new Timer.Action3ArgTrigger<int, string[], bool>(loadFilesComplete, pageId, files, storage);
        }

        void loadFilesComplete(int pageId, string[] files, bool storage)
        {
            if (toggRef.menu.Open &&
                pageId == toggRef.menu.menu.PageId)
            {
                toggRef.menu.menu.PopLayout();


                var layout = new GuiLayout("Load Map", toggRef.menu.menu);
                {
                    if (files.Length == 0)
                    {
                        new GuiLabel("No files", layout);
                    }
                    else
                    {
                        int pathLength = Data.FileManager.MapfolderPath(true).CompleteDirectory.Length + 1;
                        foreach (var m in files)
                        {

                            string name = System.IO.Path.GetFileName(m);
                            new GuiTextButton(name, null, new GuiAction2Arg<string, bool>(loadCustomMap, name, storage), false, layout);
                        }
                    }
                }
                layout.End();
            }
        }

        virtual protected void loadCustomMap(string fileName, bool fromStorage)
        {
            //filemanager.loadFile(fileName, fromStorage);
        }

        abstract protected bool DefaultMouseLock { get; } 
    }
}
