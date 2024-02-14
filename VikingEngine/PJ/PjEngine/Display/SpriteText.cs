using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Display
{
    class SpriteText : VikingEngine.Graphics.AbsSpriteText
    {
        public const float LetterSideEdge = 1f/16;
        public const float LetterWidthScale = 1f - LetterSideEdge * 2;

        public SpriteText(string textString, Vector2 position, float height, ImageLayers layer, Vector2 relCenter, Color color, bool addToRender)
            : base(LetterWidthScale,
            textString, position, height, layer, relCenter, color, addToRender)
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
                    return SpriteName.pjNum0;

                case '1':
                    return SpriteName.pjNum1;

                case '2':
                    return SpriteName.pjNum2;

                case '3':
                    return SpriteName.pjNum3;

                case '4':
                    return SpriteName.pjNum4;

                case '5':
                    return SpriteName.pjNum5;

                case '6':
                    return SpriteName.pjNum6;

                case '7':
                    return SpriteName.pjNum7;

                case '8':
                    return SpriteName.pjNum8;

                case '9':
                    return SpriteName.pjNum9;

                case '+':
                    return SpriteName.pjNumPlus;

                case '-':
                    return SpriteName.pjNumMinus;

                case '*':
                    return SpriteName.pjNumMultiply;

                case '=':
                    return SpriteName.pjNumEquals;

                case '>':
                    return SpriteName.pjNumArrowR;

                case '!':
                    return SpriteName.pjNumExpression;

                case '<':
                    return SpriteName.pjNumQuestion;                    

                default:
                    return SpriteName.MissingImage;

            }
        }
    }
}
