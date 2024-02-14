using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Joust
{
    //static class JoustRef
    //{
    //    public static Sounds sounds;

    //    public static void ClearMem()
    //    {
    //        sounds = null;
    //    }
    //}

    static class JoustLib
    {
        public const bool DebugViewInputHand = false;

        public static IntervalF EffectsLayers;

        public static void Init()
        {
            EffectsLayers = new IntervalF(
                Graphics.GraphicsLib.ToPaintLayer(LayerEffectsStart), 
                Graphics.GraphicsLib.ToPaintLayer(LayerEffectsEnd));
        }
        
        public const ImageLayers LayerHealthBar = ImageLayers.Foreground7;
        public const ImageLayers LayerBird = ImageLayers.Foreground8;
        public const ImageLayers LayerGroundTimer = ImageLayers.Foreground9;

        public const ImageLayers LayerShellChick = ImageLayers.Lay3;

        const ImageLayers LayerEffectsStart = ImageLayers.Background1;
        const ImageLayers LayerEffectsEnd = ImageLayers.Background4;

        public const int MaxGamers = 16;
        public const int OnGroundSeconds = 3;
        public const float OnGroundBeforeAutojumpSeconds = 5;
        public const int OnGroundAutojumpSeconds = 5;
        public const int PlayerStartHealth = 3;
        public const int CoinsToHealthUp = 3;
        public static readonly int MatchCount = 
            PlatformSettings.DevBuild? 5
            : 5;//NO TOUCHY
    }
}
