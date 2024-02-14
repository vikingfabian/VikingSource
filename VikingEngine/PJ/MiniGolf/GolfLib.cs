using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    static class GolfLib
    {
        public const ImageLayers PointEffectsLayer = ImageLayers.Lay1;

        public const ImageLayers BorderLayer = ImageLayers.Lay3;

        public const ImageLayers FieldEditorSelectionLayer = ImageLayers.Background4;
        public const ImageLayers FieldEditorLayer = ImageLayers.Background5;

        public const ImageLayers ClubLayer = ImageLayers.Background7;

        public const ImageLayers BallEffectLayer = ImageLayers.Background8;

        public const ImageLayers ObsticleLayer = ImageLayers.Background9;
        public const ImageLayers BallLayer = ImageLayers.Bottom0;
        public const ImageLayers ItemsLayer = ImageLayers.Bottom1;
        public const ImageLayers BallTraceLayer = ImageLayers.Bottom2;
        
        public const ImageLayers FieldEdgesLayer = ImageLayers.Bottom3;

        public const ImageLayers ObjectsLayer = ImageLayers.Bottom4_Front;
        public const ImageLayers ShadowLayer = ImageLayers.Bottom4;
        public const ImageLayers HoleLayer = ImageLayers.Bottom5;
        public const ImageLayers FieldTerrainLayer = ImageLayers.Bottom8;
        public const ImageLayers FieldLayer = ImageLayers.Bottom9;

        public const int ClubStrikeCost = 2;
        public const int DamageCost = 4;
        public const float ClubAngleSpeed = 2.6f;

        public static readonly int MatchCount =
            PlatformSettings.DevBuild ? 5
            : 5;//NO TOUCHY
    }

    struct DamageOrigin
    {
        public bool fromTerrain;
        public Vector2 center;
        
    }
}
