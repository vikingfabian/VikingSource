using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.Effects
{
    class ValueChangeAnimation : AbsUpdateable
    {
        Time viewTime = new Time(900);
        Graphics.RenderTargetBillboard model;
        Vector3 speed;

        public ValueChangeAnimation(IntVector2 pos, ValueType type, int valueChange)
            :base(true)
        {           

            Color textCol = Color.White;
            Color iconCol = Color.White;
            SpriteName icon = SpriteName.MissingImage;

            switch (type)
            {
                case ValueType.Health:
                    if (valueChange > 0)
                    {
                        textCol = new Color(16, 196, 7);
                        icon = SpriteName.cmdStatsHealth;
                    }
                    else
                    {
                        icon = SpriteName.cmdBrokenHeart;
                        iconCol = toggLib.NegativeTextCol;
                    }
                    break;
                case ValueType.Stamina:
                    textCol = toggLib.StaminaTextCol;
                    icon = SpriteName.cmdStamina;
                    break;
            }

            if (valueChange < 0)
            {//RED
                textCol = toggLib.NegativeTextCol;
            }

            float textureHeight = SpriteSheet.CmdLetterSz * 4f;
            float w = textureHeight * 4;

            model = new Graphics.RenderTargetBillboard(toggRef.board.toWorldPos_Center(pos, 0.8f),
                0.14f, true);
            model.FaceCamera = false;
            model.Rotation = toggLib.PlaneTowardsCam;

            model.images = new List<Graphics.AbsDraw>(4);
            ToggEngine.Display2D.SpriteText text = new ToggEngine.Display2D.SpriteText(TextLib.ValuePlusMinus(valueChange), Vector2.Zero, textureHeight, ImageLayers.Lay0, Vector2.Zero, textCol, false);
            model.images.AddRange(text.letters);

            Graphics.Image iconImage = new Graphics.Image(icon, new Vector2(text.Right, 0), new Vector2(textureHeight), ImageLayers.Lay1, false, false);
            iconImage.Color = iconCol;
            model.images.Add(iconImage);

            model.createTexture(new Vector2(w, textureHeight), model.images, null);
            model.setModelSizeFromTexHeight();

            speed = toggLib.UpVector * 2f;
            //SpriteName.cmd1
        }

        public override void Time_Update(float time_ms)
        {
            model.Position += speed * Ref.DeltaTimeSec;

            if (Ref.TimePassed16ms)
            {
                speed *= 0.7f;
            }

            if (viewTime.CountDown())
            {
                model.Opacity -= 2f * Ref.DeltaTimeSec;
                if (model.Opacity <= 0)
                {
                    DeleteMe();
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
