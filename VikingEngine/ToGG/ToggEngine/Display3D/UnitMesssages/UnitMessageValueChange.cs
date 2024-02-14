using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display3D
{
    class UnitMessageValueChange : AbsUnitMessage
    {
        const float Height = 0.14f;

        public UnitMessageValueChange(AbsUnit unit, ValueType type, int valueChange)
            : base(unit)
        {
            start();

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
                    if (valueChange > 0)
                    {
                        icon = SpriteName.cmdStamina;
                    }
                    else
                    {
                        icon = SpriteName.cmdStaminaDrain;
                    }
                    break;

                case ValueType.BloodRage:
                    textCol = toggLib.BloodRageTextCol;
                    icon = SpriteName.cmdBloodRage;
                    break;
            }

            if (valueChange < 0)
            {//RED
                textCol = toggLib.NegativeTextCol;
            }

            float textureHeight = SpriteSheet.CmdLetterSz * 4f;
            float w = textureHeight * 4;

            model = new Graphics.RenderTargetBillboard(basePos,
                Height, true);
            model.FaceCamera = false;
            model.Rotation = toggLib.PlaneTowardsCam;

            model.images = new List<Graphics.AbsDraw>(4);
            SpriteText text = new SpriteText(TextLib.ValuePlusMinus(valueChange), Vector2.Zero, textureHeight, ImageLayers.Lay0, Vector2.Zero, textCol, false);
            model.images.AddRange(text.letters);

            Graphics.Image iconImage = new Graphics.Image(icon, new Vector2(text.Right, 0), new Vector2(textureHeight), ImageLayers.Lay1, false, false);
            iconImage.Color = iconCol;
            model.images.Add(iconImage);

            model.createTexture(new Vector2(w, textureHeight), model.images, null);
            model.setModelSizeFromTexHeight();

            completeInit(unit);
        }

        public override float MessageHeight => Height;
    }
}
