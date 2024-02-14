using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Display
{
    class SpriteText : VikingEngine.Graphics.AbsSpriteText
    {
        const float WidthScale = 0.64f;
       

        public SpriteText(string textString, Vector2 position, float height, ImageLayers layer, Vector2 relCenter, Color color, bool addToRender)
            :base(WidthScale, 
            textString, position, height, layer, relCenter, color, addToRender)
        { }

        protected override SpriteName charTile(char c)
        {
            switch (c)
            {
                case '0':
                    return SpriteName.LFNum0;
                    
                case '1':
                    return SpriteName.LFNum1;
                    
                case '2':
                    return SpriteName.LFNum2;
                    
                case '3':
                    return SpriteName.LFNum3;
                    
                case '4':
                    return SpriteName.LFNum4;
                    
                case '5':
                    return SpriteName.LFNum5;
                    
                case '6':
                    return SpriteName.LFNum6;
                    
                case '7':
                    return SpriteName.LFNum7;
                    
                case '8':
                    return SpriteName.LFNum8;
                    
                case '9':
                    return SpriteName.LFNum9;
                    
                case '+':
                    return SpriteName.LFNumPlus;
                    
                case '-':
                    return SpriteName.LFNumMinus;
                    
                case '*':
                    return SpriteName.LFNumStar;
                    
                default:
                    return SpriteName.NO_IMAGE;
                    
            }
        }
    }

    
}
