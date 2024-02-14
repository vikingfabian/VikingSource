using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Input;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Display
{
    /// <summary>
    /// Views all the special input options while paused
    /// </summary>
    class InputOverview : AbsUpdateable
    {
        Graphics.ImageGroup images;
        Time waitTime = new Time(5, TimeUnit.Seconds);
        float opacity = 1f;

        public InputOverview(VectorRect screenSafeArea, Players.InputMap inputMap, int mode_0InGame_1Editor_2EditorSelection)
            : base(true)
        {
            Vector2 pos = screenSafeArea.RightTop;

            images = new Graphics.ImageGroup();

            if (mode_0InGame_1Editor_2EditorSelection == 0)
            {
                nextInput(null, inputMap.camera, SpriteName.CamAngleY, ref pos, inputMap);
                nextInput(null, inputMap.camera, SpriteName.CamAngleX, ref pos, inputMap);
                nextInput(null, inputMap.cameraZoom, SpriteName.CamZoom, ref pos, inputMap);
                nextInput(inputMap.firstPersonToggle, null, SpriteName.CamPersonMode, ref pos, inputMap);
                nextInput(inputMap.chat, null, SpriteName.LfChatBobbleIcon, ref pos, inputMap);
            }
            else if (mode_0InGame_1Editor_2EditorSelection == 1)
            {
                nextInput(null, inputMap.editorInput.moveXZ, SpriteName.Xdir, ref pos, inputMap);
                //nextInput(null, inputMap.editorInput.cameraXMoveY, SpriteName.Ydir, ref pos, inputMap);
                //nextInput(null, inputMap.editorInput.moveXZ, SpriteName.Zdir, ref pos, inputMap);
                //nextInput(null, DirActionType.EditorCamXY, SpriteName.CamAngleX, ref pos, inputMap);
                //nextInput(null, DirActionType.EditorCamXY, SpriteName.CamAngleY, ref pos, inputMap);
                //nextInput(null, DirActionType.EditorCamZoom, SpriteName.CamZoom, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorDraw, DirActionType.NUM_NON, SpriteName.IconBuildAdd, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorErase, DirActionType.NUM_NON, SpriteName.IconBuildRemove, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorSelect, DirActionType.NUM_NON, SpriteName.IconBuildSelection, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorPick, DirActionType.NUM_NON, SpriteName.IconColorPick, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorUndo, DirActionType.NUM_NON, SpriteName.MenuIconResume, ref pos, inputMap);

                waitTime.Seconds = 8f;
            }
            else
            {
                nextInput(null, inputMap.editorInput.moveXZ, SpriteName.IconBuildSelection, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorMirrorX, null, SpriteName.FlipHori, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorMirrorY, null, SpriteName.FlipVerti, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorPrevious, null, SpriteName.RotateCCW, ref pos, inputMap);
                //nextInput(ButtonActionType.EditorNext, null, SpriteName.RotateCW, ref pos, inputMap);
            }
        }

        void nextInput(IButtonMap button, IDirectionalMap dir, SpriteName icon, ref Vector2 nextPos, Input.PlayerInputMap inputMap)
        {
            List<SpriteName> buttons = new List<SpriteName>(1);
            SpriteName plusKey = SpriteName.NO_IMAGE;

            if (button != null)
            {
                buttons.Add(button.Icon);
            }
            else
            {
                dir.ListIcons(buttons, out plusKey, false);//inputMap.directionalMappings[(int)dir].ListIcons(buttons, out plusKey, false);
            }

            Vector2 pos = nextPos;
            Vector2 iconSz = new Vector2(Engine.Screen.IconSize);
            pos += iconSz * 0.5f;//center

            pos.X -= iconSz.X * 0.7f;
            Graphics.Image actionIc = new Graphics.Image(icon, pos, iconSz * 1.4f, LfLib.Layer_GuiMenu, true);
            images.Add(actionIc);

            pos.X -= iconSz.X * 1.2f;
            Graphics.Image arrow = new Graphics.Image(SpriteName.LfMenuMoreMenusArrow, pos, iconSz * 0.5f, LfLib.Layer_GuiMenu, true);
            images.Add(arrow);

            pos.X -= iconSz.X * 1f;

            for (int i = buttons.Count - 1; i >= 0; --i)
            {
                
                Graphics.Image buttonIc = new Graphics.Image(buttons[i], pos, iconSz, LfLib.Layer_GuiMenu, true);
                images.Add(buttonIc);
                pos.X -= iconSz.X;
            }

            if (plusKey != SpriteName.NO_IMAGE)
            {
                Graphics.Image plusSymb = new Graphics.Image(SpriteName.LFNumPlus, pos, iconSz * 1f, LfLib.Layer_GuiMenu, true);
                //Graphics.TextG plusSymb = new Graphics.TextG( LoadedFont.PhoneText, pos,
                //    new Vector2(Engine.Screen.TextSize), Graphics.Align.CenterAll, "+", Color.White, LfLib.Layer_GuiMenu);
                images.Add(plusSymb);

                pos.X -= iconSz.X * 1f;

                
                Graphics.Image buttonIc = new Graphics.Image(plusKey, pos, iconSz, LfLib.Layer_GuiMenu, true);
                images.Add(buttonIc);

                //pos.X -= iconSz.X;
            }

            nextPos.Y += iconSz.Y * 1.2f;
        }

        public override void Time_Update(float time_ms)
        {
            if (waitTime.CountDown())
            {
                opacity -= Ref.DeltaTimeSec * 2f;
                images.SetOpacity(opacity);

                if (opacity <= 0f)
                {
                    DeleteMe();
                }
            }
        }

        public void DeleteMe()
        {
            images.DeleteAll();
        }
    }
}
