using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map
{
    class DebugMap
    {
        public DebugMap()
        {
            Ref.draw.CurrentRenderLayer = DrawGame.TerrainLayer;
            
            ForXYLoop loop = new ForXYLoop(DssRef.world.Size);
            while (loop.Next())
            {

                var tile = DssRef.world.tileGrid.Get(loop.Position);
                if (tile.seaDistanceHeatMap != int.MinValue)
                {
                    Graphics.Text3DBillboard text3D = new Graphics.Text3DBillboard(LoadedFont.Console,
                        tile.seaDistanceHeatMap.ToString(),
                        Color.White, null,
                        VectorExt.V2toV3XZ(loop.Position.Vec, 0), 0.6f, 0.5f, true);
                    text3D.Y = DssRef.world.GetGroundHeight(text3D.position) + 0.03f;
                }
            }

            //Ref.draw.CurrentRenderLayer = DrawGame.TerrainLayer;
        }
    }
}
