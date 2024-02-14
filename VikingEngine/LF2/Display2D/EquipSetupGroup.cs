using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2
{
#if !CMODE
    class EquipSetupGroup : IDeleteable
    {
        const float IconW = 92;
        const float Edge = 4;
        const float ButtonW = IconW + Edge * PublicConstants.Twice;
        const float GroupSpace = 8;
        public const float GroupWidth = ButtonW * 3 + GroupSpace;
        Graphics.Image centerButton;
        List<Graphics.AbsDraw2D> images = new List<Graphics.AbsDraw2D>();
        
        public EquipSetupGroup(int index, Vector2 center, Players.EquipSetup setup, bool indexAdj, bool flashingFrame)
        {
            Vector2 equipButtonSize = new Vector2(ButtonW, (ButtonW / LoadTiles.EquipButtonSize.X) * LoadTiles.EquipButtonSize.Y);

            Players.EquippedButtonSlot[] buttons = setup.EquippedButton;

            Vector2 position = center;
            position.X += -1.5f * ButtonW;
            position.Y -= equipButtonSize.Y * PublicConstants.Half;
            if (indexAdj)
            {
                position.X += index * GroupWidth;
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                Graphics.Image button = new Graphics.Image(
                    (SpriteName)((int)SpriteName.LFMenuEquipButtonX + i), position, equipButtonSize, ImageLayers.Foreground8);
                images.Add(button);
                if (buttons[i].Weapon != null)
                {
                    Graphics.Image icon = new Graphics.Image(
                        buttons[i].Weapon.Icon, position + Vector2.One * Edge, Vector2.One * IconW, ImageLayers.Foreground7);
                    images.Add(icon);
                }

                if (i == 1)
                {
                    centerButton = button;
                }
                position.X += ButtonW;
            }

            if (flashingFrame)
            {
                //add a flashing line around the setup
                Vector2 size = new Vector2(GroupWidth * 1.05f, equipButtonSize.Y * 1.4f);

                Graphics.Image frame = new Graphics.Image(SpriteName.LFMenuRectangleSelection, center, size, ImageLayers.Foreground6, true);
                frame.Transparentsy = 0;
                Graphics.Motion2d flashFrame = new Graphics.Motion2d(Graphics.MotionType.TRANSPARENSY,
                    frame, Vector2.One, Graphics.MotionRepeate.BackNForwardLoop, 500, true); 
                 images.Add(frame);
            }
        }

        public void Move(float x, float centerX)
        {
            float diff = Math.Abs(centerButton.Center.X - centerX);
            float trans = 1 - (diff / (GroupWidth * 2));

            foreach (Graphics.AbsDraw2D img in images)
            {
                img.Transparentsy = trans;
                img.Xpos += x;
            }
        }
        public void DeleteMe()
        {
            if (images != null)
            {
                foreach (Graphics.AbsDraw2D img in images)
                {
                    img.DeleteMe();
                }
                images = null;
            }
        }
        public bool IsDeleted
        {
            get { return images == null; }
        }
        void fade(bool fadeIn)
        {
            const float FadeTime = 400;
            Vector2 dir = fadeIn ? Vector2.One : new Vector2NegativeOne;

            foreach (Graphics.AbsDraw2D img in images)
            {
                new Graphics.Motion2d(Graphics.MotionType.TRANSPARENSY, img, dir,
                    Graphics.MotionRepeate.NO_REPEATE, FadeTime, true);
            }
        }
    }
#endif
}
