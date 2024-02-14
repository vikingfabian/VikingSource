using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.CarBall
{
    static class cballLib
    {
        public const bool DebugStartPoistions = false;
        public const bool WarpOnOuterBound = false;
        public static readonly SpriteSize BallSprite = new SpriteSize(16, 32);
        

        public const int GoalCount = 5;

        public const int CollisionUpdatesPerFrame = 4;
        public const int LastCollisionUpdate = CollisionUpdatesPerFrame - 1;

        public const float PartialUpdateLength = 1f / CollisionUpdatesPerFrame;


        public const ImageLayers LayerGoal = ImageLayers.Foreground4;

        public const ImageLayers LayerCar = ImageLayers.Lay4;

        public const ImageLayers LayerGoalieSpot = ImageLayers.Background2;
        public const ImageLayers LayerGoalBG = ImageLayers.Background3;
        public const ImageLayers LayerFieldEdges = ImageLayers.Background4;

        public const ImageLayers LayerTrackMin = ImageLayers.Background5;
        public const ImageLayers LayerTrackMax = ImageLayers.Background7;
        public const ImageLayers LayerFieldEffects = ImageLayers.Background8_Front;
        public const ImageLayers LayerField = ImageLayers.Background8;
        
    }
}
