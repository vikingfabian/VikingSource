
using VikingEngine.Input;
using VikingEngine.LootFest.GO.PlayerCharacter;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{
    class BtnBindUpdateObj : AbsUpdateable
    {
        /* Fields */
        protected Gui menu;
        protected GuiLayout layout;
        ButtonActionType map;
        protected AbsHero hero;
        List<Keys> keysToCheck;
        protected bool addAlternative;

        bool ignoreFirstFrame;

        /* Constructors */
        public BtnBindUpdateObj(Gui menu, GuiLayout layout, ButtonActionType map, AbsHero hero, bool addAlternative)
            : base(true)
        {
            this.menu = menu;
            this.layout = layout;
            this.map = map;
            this.hero = hero;
            this.addAlternative = addAlternative;
            keysToCheck = new List<Keys>(Input.Keyboard.AllKeys);
            menu.BlockInput();

            ignoreFirstFrame = true;
        }
        /* Methods */
        public override void Time_Update(float time)
        {
            if (ignoreFirstFrame)
            {
                ignoreFirstFrame = false;
                return;
            }

            if (layout == menu.PeekLayout())
            {
                if (AttemptComplete())
                {
                    menu.PopLayout();
                    menu.ReenableInput();
                    DeleteMe();
                }
            }
        }

        protected virtual bool AttemptComplete()
        {
            return SetFromKeyboard() || SetFromMouse() || SetFromXboxController();
        }

        protected bool SetFromKeyboard()
        {
            Keys? key = Input.Keyboard.CheckKeyDowns(keysToCheck);

            if (key.HasValue)
            {
                SetMap(new KeyboardButtonMap(key.Value));
                return true;
            }

            return false;
        }

        protected bool SetFromMouse()
        {
            for (int i = 0; i < (int)MouseButton.NUM; ++i)
            {
                MouseButton btn = (MouseButton)i;
                if (Input.Mouse.ButtonDownEvent(btn))
                {
                    SetMap(new MouseButtonMap(btn));
                    return true;
                }
            }

            return false;
        }

        protected bool SetFromXboxController()
        {
            int playerIndex = hero.player.PlayerIndex;
            foreach (Buttons btn in Input.XInput.AllButtons)
            {
                if (Input.XInput.Instance(playerIndex).KeyDownEvent(btn))
                {
                    SetMap(new XboxButtonMap(btn, (int)playerIndex));
                    return true;
                }
            }
            return false;
        }

        //protected bool SetFromGenericController()
        //{
        //    int playerIndex = hero.player.PlayerIndex;
        //    for (int i = 0; i != (int)GenericControllerButton.NUM; ++i)
        //    {
        //        GenericControllerButton btn = (GenericControllerButton)i;
        //        if (SharpDXInput.DownEvent(btn, playerIndex))
        //        {
        //            SetMap(new GenericControllerButtonMap(btn, playerIndex));
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        protected virtual void SetMap(IButtonMap newMap)
        {
            //if (addAlternative)
            //    hero.inputMap.buttonMappings[(int)map] = new AlternativeButtonsMap(hero.inputMap.buttonMappings[(int)map], newMap);
            //else
            //    hero.inputMap.buttonMappings[(int)map] = newMap;
        }
    }

    class FourButtonDirectionMapCreator : BtnBindUpdateObj
    {
        enum SetIndex
        {
            Up,
            Down,
            Left,
            Right,
            NUM
        }

        IButtonMap[] btns;
        int settingIndex;
        DirActionType dirMap;

        public FourButtonDirectionMapCreator(Gui menu, DirActionType dirMap, AbsHero hero, bool addAlternative)
            : base(menu, null, ButtonActionType.NUM_NON, hero, addAlternative)
        {
            btns = new IButtonMap[(int)SetIndex.NUM];
            settingIndex = 0;
            this.dirMap = dirMap;

            layout = new GuiLayout("Bind " + ((SetIndex)settingIndex).ToString(), menu);
            {
                new GuiLabel("Waiting for input...", layout);
            }
            layout.End();
        }

        protected override bool AttemptComplete()
        {
            //base.AttemptComplete();
            //if (settingIndex == (int)SetIndex.NUM)
            //{
            //    int i = (int)dirMap;
            //    IDirectionalMap newMap = new DirectionalButtonsMap(btns[(int)SetIndex.Up], btns[(int)SetIndex.Down], btns[(int)SetIndex.Left], btns[(int)SetIndex.Right]);
            //    if (addAlternative)
            //        hero.inputMap.directionalMappings[i] = new AlternativeDirectionalMap(hero.inputMap.directionalMappings[i], newMap);
            //    else
            //        hero.inputMap.directionalMappings[i] = newMap;

            //    return true;
            //}
            return false;
        }

        protected override void SetMap(IButtonMap newMap)
        {
            btns[settingIndex++] = newMap;
            if (settingIndex < (int)SetIndex.NUM)
            {
                menu.PopLayout();
                GuiLayout newLayout = new GuiLayout("Bind " + ((SetIndex)settingIndex).ToString(), menu);
                {
                    new GuiLabel("Waiting for input...", newLayout);
                }
                newLayout.End();
                layout = newLayout;
            }
        }
    }
}
