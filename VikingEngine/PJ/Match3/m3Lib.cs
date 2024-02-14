using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Match3
{
    static class m3Lib
    {
        public const ImageLayers LayerScoreText = ImageLayers.Lay1;
        public const ImageLayers LayerInput = ImageLayers.Lay2;

        public const ImageLayers LayerParticle = ImageLayers.Lay3;
        public const ImageLayers LayerBrick = ImageLayers.Lay5;
        public const ImageLayers LayerBrickOutline = ImageLayers.Lay5_Back;
        public const ImageLayers LayerMarkEndPos = ImageLayers.Lay6;
        public const ImageLayers LayerBox = ImageLayers.Background0;

        
        public const int SpeedUpLevels = 3;

        

        public static int Scoring(int brickLength, int matchIndex, int combos)
        {
            int score = 0;

            switch (brickLength)
            {
                default: return ScoreError();

                case 3: score = 10; break;
                case 4: score = 30; break;
                case 5: score = 80; break;
                case 6: score = 250; break;
                case 7: score = 1000; break;
                case 8: score = 4000; break;
            }

            if (matchIndex > 0)
            {
                score *= 2 * matchIndex;
            }
            if (combos > 0)
            {
                score *= 4 * combos;
            }

            return score;
        }
        
        static int ScoreError()
        {
            if (PlatformSettings.DevBuild) { throw new Exception(); }

            return -1;
        }
    }


    enum BrickColor
    {
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
        Purple,

        Stone,

        NUM
    }
}
