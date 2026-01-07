using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichMenu
{
    class RichMenuControllerPointer
    {
        public InputMap inputMap;
        public Image pointer;
        public RichMenu menu;
        public float maxInteractDistance;

        float LowAccelerate;
        float moveAcc;
        Rotation1D accelerateDir = Rotation1D.D0;
        public RichMenuControllerPointer(InputMap inputMap)
        {
            LowAccelerate = Engine.Screen.IconSize * 0.004f;
            moveAcc = LowAccelerate;
            this.inputMap = inputMap;
        }

        public void setMenu(RichMenu menu, Vector2 storedPosition)
        {
            this.menu = menu;

            if (pointer == null)
            {
                maxInteractDistance = Engine.Screen.IconSize;
                pointer = new Image(SpriteName.ColorPickerCircle, Vector2.Zero, Engine.Screen.IconSizeV2, ImageLayers.Lay0, true);
                pointer.Opacity = 0.8f;
            }

            pointer.Layer = menu.layer - 2;

            if (storedPosition == Vector2.Zero)
            {
                pointer.position = menu.renderArea.Position + Engine.Screen.IconSizeV2;
            }
            else
            {
                pointer.position = storedPosition;
            }
        }

        
        public Vector2 accelerateInput(Vector2 input)
        {
            float speed = 1.0f;
            var l = input.Length();
            Vector2 result = Ref.DeltaTimeMs * moveAcc * speed * input;
            if (l < 0.9f)
            {
                setLow();
            }
            else if (Rotation1D.FromDirection(input).AngleDifference(accelerateDir) > MathExt.TauOver8)
            {
                setLow();
            }
            else
            {
                moveAcc = Bound.Max(moveAcc + Ref.DeltaTimeSec * 2f, 1.2f);
            }

            return result;

            void setLow()
            {
                moveAcc = LowAccelerate;
                if (l != 0)
                {
                    accelerateDir = Rotation1D.FromDirection(input);
                }
            }
        }

        public void DeleteMe(out Vector2 storedPos)
        {
            storedPos = pointer.position;
            menu.deleteTooltip();
            pointer?.DeleteMe();
        }
    }

}
