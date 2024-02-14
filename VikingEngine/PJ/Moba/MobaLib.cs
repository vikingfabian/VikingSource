using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Moba
{
    static class MobaLib
    {
        public const ImageLayers LayerPowerWheel = ImageLayers.Top8;

        const ImageLayers LayerUnitsTop = ImageLayers.Foreground0;
        const ImageLayers LayerUnitsBottom = ImageLayers.Foreground9;

        public static float UnitScale;
        public static IntervalF UnitsLayers;

        public static void Init()
        {
            UnitScale = Engine.Screen.Height * 0.05f;

            UnitsLayers = new IntervalF(
                Graphics.GraphicsLib.ToPaintLayer(LayerUnitsTop), 
                Graphics.GraphicsLib.ToPaintLayer(LayerUnitsBottom));
        }
    }
}
