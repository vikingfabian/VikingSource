using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.MoonFall
{
    class MoonFallState : AbsToggState
    {   
        protected bool mapLoaded = false;
        

        public MoonFallState()
            : base()
        {
            moonRef.playState = this;

            draw.ClrColor = Color.Black;
            new Map();
            //editor = new Editor();
            new MenuSystem(Input.InputSource.DefaultPC);

            ((Draw)draw).initShadow();   
        }

        protected override void createDrawManager()
        {
            draw = new Draw();
        }

        //void playerStart()
        //{
        //    for (int i = 0; i < 
        //}

        public void mapLoadingComplete()
        {
            mapLoaded = true;
            new Players.Players();
            //base.mapLoadingComplete();
            //playerSetup();
            //mapSetup();
        }

        public override void Time_Update(float time)
        {
            if (toggRef.menu.Open)
            {
                if (toggRef.menu.Update())
                {
                    toggRef.menu.CloseMenu();
                }
                return;
            }

            if (mapLoaded)
            {
                //editor.update();
                gameUpdate();

                if (toggRef.inputmap.menuInput.openCloseInputEvent())
                {
                    openMenu();
                }
            }
        }

        virtual protected void gameUpdate()
        {

        }

        void openMenu()
        {
            toggRef.menu.OpenMenu(false);

            GuiLayout layout = new GuiLayout("Main Menu", toggRef.menu.menu);
            {
                new GuiTextButton("Resume", null, toggRef.menu.CloseMenu, false, layout);
                //
                mainMenu(layout);

            }
            layout.End();
        }

        virtual protected void mainMenu(GuiLayout layout)
        {
            new GuiTextButton("Editor", null, openEditor, false, layout);
        }

        void openEditor()
        {
            new EditorState();
        }

        protected void save()
        {
            moonRef.map.saveload(true);
            toggRef.menu.CloseMenu();
        }

        protected override bool DefaultMouseLock => true;

        virtual public bool InEditor => false;
    }
}
