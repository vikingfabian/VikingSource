using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.MiniGolf.Level
{
    class LevelVisualsSetup
    {
        public SpriteName brickTex;
        public SpriteName groundTex;
        public List<TwoSprites> sandTex;

        public Color fieldEdgeColor, fieldEdgeOutlineColor;
        //public bool useDamageTraps;

        public LevelVisualsSetup(int level)
        {
            sandTex = new List<TwoSprites>
            {
                new TwoSprites(SpriteName.golfSand1Bg, SpriteName.golfSand1Top),
                new TwoSprites(SpriteName.golfSand2Bg, SpriteName.golfSand2Top),
            };

           // useDamageTraps = level > 1;

            switch (level)
            {
                default:
                    groundTex = SpriteName.golfGrassGreen;
                    brickTex = SpriteName.golfBrickBrown;

                    fieldEdgeColor = new Color(238, 64, 159);
                    fieldEdgeOutlineColor = new Color(255, 173, 217);
                    break;
                case 1:
                    groundTex = SpriteName.golfGrassBlue;
                    brickTex = SpriteName.golfBrickBrown;

                    fieldEdgeColor = new Color(0, 49, 90);
                    fieldEdgeOutlineColor = new Color(0, 86, 155);
                    break;
                case 2:
                    groundTex = SpriteName.golfGrassRed;
                    brickTex = SpriteName.golfBrickBlue;

                    fieldEdgeColor = new Color(103,33,0);
                    fieldEdgeOutlineColor = new Color(164,54,0);
                    break;

                //case 3:
                //    groundTex = SpriteName.golfGrassYellow;
                //    brickTex = SpriteName.golfBrickBlue;

                //    fieldEdgeColor = new Color(103, 33, 0);
                //    fieldEdgeOutlineColor = new Color(164, 54, 0);
                //    break;
                case 4:
                    groundTex = SpriteName.golfGrassGray;
                    brickTex = SpriteName.golfBrickBrown;

                    fieldEdgeColor = new Color(36,44,49);
                    fieldEdgeOutlineColor = new Color(70,84,92);
                    break;
            }
            
        }
    }
}
