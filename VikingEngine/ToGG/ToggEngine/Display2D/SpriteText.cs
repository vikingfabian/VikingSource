using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class SpriteText : VikingEngine.Graphics.AbsSpriteText
    {
        const float LetterWidthScale = 0.68f;

        public SpriteText(string textString, Vector2 position, float height, ImageLayers layer, Vector2 relCenter, Color color, bool addToRender)
            : base(LetterWidthScale, textString, position, height, layer, relCenter, color, addToRender)
        { }

        protected override SpriteName charTile(char c)
        {
            return CharTile(c);
        }

        public static SpriteName CharTile(char c)
        {
            switch (c)
            {
                case '0':
                    return SpriteName.cmd0;

                case '1':
                    return SpriteName.cmd1;

                case '2':
                    return SpriteName.cmd2;

                case '3':
                    return SpriteName.cmd3;

                case '4':
                    return SpriteName.cmd4;

                case '5':
                    return SpriteName.cmd5;

                case '6':
                    return SpriteName.cmd6;

                case '7':
                    return SpriteName.cmd7;

                case '8':
                    return SpriteName.cmd8;

                case '9':
                    return SpriteName.cmd9;

                case '/':
                    return SpriteName.cmdDiv;

                case '+':
                    return SpriteName.cmdPlus;

                case '-':
                    return SpriteName.cmdMinus;

                case '!':
                    return SpriteName.cmdExclamation;

                default:
                    return SpriteName.NO_IMAGE;

            }
        }
    }
}

