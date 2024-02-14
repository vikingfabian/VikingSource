using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;
using VikingEngine.SteamWrapping;
using VikingEngine.Engine;

namespace VikingEngine.LootFest.Display
{
    class SelectControllerMenu
    {
        List<ControllerChoice> controllers;
        int selectedIndex = 0;
        SelectControllerMenuMember[] members;
        Graphics.Image selectionFrame;
        //PlayerData player;
        Players.Player player;
        //Vector2 selectionFrameSz;

        public SelectControllerMenu(Players.Player player)
        {
            this.player = player;
            float minDim = Math.Min(Screen.SafeArea.Width, Screen.SafeArea.Height);
            Vector2 bigButtonSz = new Vector2((int)(0.6f * minDim), (int)(0.45f * minDim));
            if (Screen.SafeArea.Width < Screen.SafeArea.Height)
            {
                // portrait layout, use different size for the big button size
                bigButtonSz = new Vector2((int)(0.5f * minDim), (int)(0.5f * minDim));
            }
            Vector2 spacing = new Vector2((int)(0.03f * minDim));
            float borderSz = (int)(bigButtonSz.Y * 0.05f);
            //selectionFrameSz = new Vector2(borderSz);

            Vector2 smallButtonSz = new Vector2((bigButtonSz.X - spacing.X) * 0.5f);

            Vector2 pos = Engine.Screen.SafeArea.Center - (spacing + bigButtonSz) * 0.5f;
            pos.Y += smallButtonSz.Y * 0.3f;
            Vector2 smallButtonsPos = pos;
            smallButtonsPos.X += (-bigButtonSz.X + smallButtonSz.X) * 0.5f;
            smallButtonsPos.Y += (-bigButtonSz.Y + smallButtonSz.Y) * 0.5f + spacing.Y + bigButtonSz.Y;



            members = new SelectControllerMenuMember[(int)PlayerControllerSelection.Num_Non];

            for (PlayerControllerSelection c = PlayerControllerSelection.KeyboardMouse; c <= PlayerControllerSelection.Keyboard; ++c)
            {
                members[(int)c] = new SelectControllerMenuMember(pos, bigButtonSz, borderSz, c, null);
                pos.X += spacing.X + bigButtonSz.X;
            }

            int controllerIx = 0;
            controllers = listControllers();
            //SteamWrapping.SteamGamepad.SetActionSet_All(ControllerActionSetType.MenuControls);

            for (PlayerControllerSelection c = PlayerControllerSelection.Controller1; c <= PlayerControllerSelection.Controller4; ++c)
            {
                var controller = controllers[controllerIx];
                members[(int)c] = new SelectControllerMenuMember(smallButtonsPos, smallButtonSz, borderSz, c, controller);
               // members[(int)c].text.TextString = TextLib.KeepFirstLettersDotDotDot(controller.name(), 16);

                smallButtonsPos.X += spacing.X + smallButtonSz.X;
                controllerIx++;
            }
            ////GENERIC CONTROLLER JOIN
            //for (int controllerIx = 0; controllerIx < SharpDXInput.controllers.Count; ++controllerIx)
            //{
            //    GenericController controller = SharpDXInput.controllers[controllerIx];
            //    for (GenericControllerButton button = 0; button < GenericControllerButton.NUM; ++button)
            //    {
            //        if (controller.DownEvent(button))
            //        {
            //            tryAddGamer(new GenericControllerButtonMap(button, (int)controllerIx));
            //        }
            //    }
            //}
            selectionFrame = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, LfLib.Layer_SaveFileMenu + 1, true);

            //SteamGamepad.SetActionSet_All(ControllerActionSetType.MenuControls);

            Input.Mouse.Visible = true;
        }

        List<ControllerChoice> listControllers()
        {
            List<ControllerChoice> controllers = new List<ControllerChoice>(4);

            //if (PlatformSettings.DevBuild)
            //{
            //    for (int controllerIx = 0; controllerIx < SharpDXInput.controllers.Count; ++controllerIx)
            //    {
            //        controllers.Add(new ControllerChoice(PlayerInputSource.GenericController, controllerIx));
            //    }
            //}

            //for (int controllerIx = 0; controllerIx < SteamGamepad.controllerCount; ++controllerIx)
            //{
            //    controllers.Add(new ControllerChoice(PlayerInputSource.SteamController, controllerIx));
            //}

            for (int controllerIx = 0; controllerIx < 4; ++controllerIx)
            {
                controllers.Add(new ControllerChoice(InputSourceType.XController, controllerIx));
            }

            return controllers;
        }

        public bool Update()
        {
            if (PlatformSettings.DevBuild && 
                DebugSett.AutoSelectInputController != InputSourceType.Num_Non &&
                LfRef.gamestate.localPlayers.Count == 1)
            {
                onSelect(DebugSett.AutoSelectInputController, 0);
                return true;
            }
            
            for (int i = 0; i < controllers.Count; i++)
            {
                bool connected = controllers[i].connected();

                if (connected && controllers[i].joinClick())
                {
                    onSelect(controllers[i].inputSource, controllers[i].controllerIx);
                    return true;
                }
                members[i + 2].setConnected(connected);
            }

            if (Input.Mouse.MoveDistance != Vector2.Zero)
            {
                for (int i = 0; i < members.Length; ++i)
                {
                    if (members[i].mouseClickArea.IntersectPoint(Input.Mouse.Position))
                    {
                        //setGrindexFromIndex(i);
                        selectedIndex = i;

                        break;
                    }
                }
            }

            if (Input.Mouse.ButtonDownEvent(MouseButton.Left) ||
                Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Enter) ||
                Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                PlayerControllerSelection selType = (PlayerControllerSelection)selectedIndex;
                if (selType == PlayerControllerSelection.Keyboard)
                {
                    onSelect(InputSourceType.KeyboardMouse, 1);
                }
                else if (selType == PlayerControllerSelection.KeyboardMouse)
                {
                    onSelect(InputSourceType.KeyboardMouse, 0);
                }
                else
                {
                    int index = selectedIndex - (int)PlayerControllerSelection.Controller1;
                    onSelect(controllers[index].inputSource, controllers[index].controllerIx);
                    //if (index < SteamGamepad.controllerCount)
                    //{
                    //    onSelect(PlayerInputSource.SteamController, index);
                    //}
                    //else
                    //{
                    //    index -= SteamGamepad.controllerCount;
                    //    onSelect(PlayerInputSource.XboxController, index);
                    //}
                }
                return true;
            }

            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Left) ||
                Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                if (selectedIndex == 0)
                    selectedIndex = 1;
                else
                    selectedIndex = 0;
            }
            


            //for (int i = 0; i < SteamGamepad.controllerCount; ++i)
            //{
            //    var pad = SteamGamepad.GetController(i);
            //    if (pad != null)
            //    {
            //        if (pad.DownEvent(ButtonActionType.MenuClick) ||
            //            pad.DownEvent(ButtonActionType.MenuReturnToGame))
            //        {
            //            onSelect(PlayerInputSource.SteamController, i);
            //            return true;
            //        }
            //    }
            //}
            //for (int i = 0; i < 4; ++i)
            //{
            //    if (Input.Controller.Instance(i).KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.A, 
            //        Microsoft.Xna.Framework.Input.Buttons.X, 
            //        Microsoft.Xna.Framework.Input.Buttons.Start))
            //    {
            //        onSelect(PlayerInputSource.XboxController, i);
            //        return true;
            //    }
            //}

            var selArea = members[selectedIndex].selectionArea;
            //selArea.Size += selectionFrameSz;
            selectionFrame.Position = selArea.Position;
            selectionFrame.Size = selArea.Size;
            return false;
        }

        void onSelect(InputSourceType inputSource, int controllerIndex)
        {
            foreach (SelectControllerMenuMember mem in members)
            {
                mem.DeleteMe();
            }
            selectionFrame.DeleteMe();

            player.inputMap.setInputSource(inputSource, controllerIndex);
            player.selectSaveFile();
        }
    }

    

    class SelectControllerMenuMember
    {
        static readonly Color FillColor = Color.LightGray;
        Graphics.Image border, fill, quickButton;
        Graphics.ImageGroupParent2D images;
        Graphics.TextG text;

        public VectorRect mouseClickArea;
        public VectorRect selectionArea;
            //, icon;

        public SelectControllerMenuMember(Vector2 center, Vector2 sz, float borderSz, PlayerControllerSelection controller,
            ControllerChoice controllerChoice)//, bool isAlreadyInUse)
        {
            mouseClickArea = VectorRect.FromCenterSize(center, sz);
            selectionArea = new VectorRect(center, sz + new Vector2(borderSz));

            border = new Graphics.Image(SpriteName.WhiteArea, center, sz, LfLib.Layer_SaveFileMenu, true);
            border.Color = Color.Gray;
            fill = new Graphics.Image(SpriteName.WhiteArea, center, sz - new Vector2(borderSz), ImageLayers.NUM, true);
            fill.Color = FillColor;
            fill.LayerAbove(border);
            text = new Graphics.TextG(LoadedFont.Regular, center - new Vector2(0, sz.Y * 0.4f), new Vector2(Engine.Screen.TextSize),
                Graphics.Align.CenterWidth, "", Color.Black, ImageLayers.NUM);
            text.LayerAbove(fill);

            images = new Graphics.ImageGroupParent2D(text);

            if (controller == PlayerControllerSelection.Controller1 ||
                controller == PlayerControllerSelection.Controller2 ||
                controller == PlayerControllerSelection.Controller3 ||
                controller == PlayerControllerSelection.Controller4)
            {
                var icon = new Graphics.Image((SpriteName)((int)SpriteName.ControllerIconP1 + ((int)controller - (int)PlayerControllerSelection.Controller1)),
                    center, sz * 0.4f, ImageLayers.NUM, true);
                icon.LayerAbove(fill);
                images.Add(icon);

                text.TextString = TextLib.FirstLettersDotDotDot(controllerChoice.name(), 16);

                Vector2 qButtonSz = new Vector2(Engine.Screen.IconSize * 1.2f);
                quickButton = new Graphics.Image(controllerChoice.joinButtonIcon(), center + (sz - qButtonSz) * 0.5f - new Vector2(borderSz * 0.5f), qButtonSz, ImageLayers.NUM, true);
                quickButton.LayerAbove(fill);
            }
            else if (controller == PlayerControllerSelection.Keyboard)
            {
                var icon = new Graphics.Image(SpriteName.Keyboard, center, sz * 0.5f, ImageLayers.NUM, true);
                icon.LayerAbove(fill);
                images.Add(icon);
            }
            else if (controller == PlayerControllerSelection.KeyboardMouse)
            {
                var keyboardIc = new Graphics.Image(SpriteName.Keyboard, center, sz * 0.5f, ImageLayers.NUM, true);
                keyboardIc.Xpos -= 0.1f * sz.X;
                var mouseIc = new Graphics.Image(SpriteName.Mouse, center + new Vector2(0.3f, 0.05f) * sz, new Vector2( sz.Y * 0.3f), ImageLayers.NUM, true);

                keyboardIc.LayerAbove(fill);
                mouseIc.LayerAbove(fill);
                images.Add(keyboardIc);
                images.Add(mouseIc);
            }

        }

        public void setConnected(bool connected)
        {
            images.SetOpacity(connected ? 1f : 0.1f);
            fill.Color = connected ? FillColor : Color.DarkGray;

            if (quickButton != null)
            {
                quickButton.Visible = connected;
            }
        }

        public void DeleteMe()
        {
            border.DeleteMe(); fill.DeleteMe();
            images.DeleteMe();
            if (quickButton != null)
            {
                quickButton.DeleteMe();
            }
        }
    }
}
