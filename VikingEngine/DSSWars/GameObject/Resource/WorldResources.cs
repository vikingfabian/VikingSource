using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    class WorldResources
    {
        TerrainContent terrainContent = new TerrainContent();

        public void asyncUpdate()
        { 
            ForXYLoop loop = new ForXYLoop(DssRef.world.subTileGrid.Size);

            while (loop.Next())
            {
               var subtile = DssRef.world.subTileGrid.Get(loop.Position);

                if (subtile.maintype == Map.SubTileMainType.Foil)
                {
                    terrainContent.asyncFoilGroth(loop.Position, subtile);
                }
            }

            DssRef.state.detailMap.needReload = true;
        }

    }
}
