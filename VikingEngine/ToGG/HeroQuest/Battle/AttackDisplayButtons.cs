using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest
{
    class AttackDiplayButtons
    {
        public Display.Button rollButton, cancelButton;

        public AttackDiplayButtons(VectorRect area)
        {
            Vector2 largeButtonSize = new Vector2(area.Width, 1.6f * Engine.Screen.IconSize);
            
            rollButton = new Display.Button(new VectorRect(area.Position, largeButtonSize), HudLib.AttackWheelLayer, 
                Display.ButtonTextureStyle.Standard);
            rollButton.addCenterText("ROLL!", Color.White, 0.8f);
            
            rollButton.addInputIcon(Dir4.W, toggRef.inputmap.nextPhase);

            Vector2 smallButtonSize = new Vector2(largeButtonSize.X, largeButtonSize.Y * 0.6f);

            cancelButton = new Display.Button(new VectorRect(
                VectorExt.AddY(rollButton.area.LeftBottom, AttackDisplay.WindowsSpacing()), 
                smallButtonSize), HudLib.AttackWheelLayer, Display.ButtonTextureStyle.Standard);
            cancelButton.addCenterText("Cancel", Color.White, 0.8f);

            cancelButton.addInputIcon(Dir4.W, toggRef.inputmap.back);
        }

        public void DeleteMe()
        {
            rollButton.DeleteMe();
            cancelButton.DeleteMe();
        }
    }
}
