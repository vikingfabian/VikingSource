using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    class WorldResources
    {
        public void asyncUpdate()
        { 
            ForXYLoop loop = new ForXYLoop(DssRef.world.subTileGrid.Size);

            while (loop.Next())
            {
               var subtile = DssRef.world.subTileGrid.Get(loop.Position);

                if (subtile.maintype == Map.SubTileMainType.Foil)
                {
                    if (subtile.undertype == (int)Map.SubTileFoilType.TreeHard)
                    {
                        IntVector2 rndDir = arraylib.RandomListMember(IntVector2.Dir8Array);
                        Map.SubTile ntile;
                        var npos = loop.Position + rndDir;
                        if (DssRef.world.subTileGrid.TryGet(npos, out ntile))
                        {
                            if (ntile.maintype == Map.SubTileMainType.DefaultLand)
                            {
                                ntile.maintype = Map.SubTileMainType.Foil;
                                ntile.undertype = (int)Map.SubTileFoilType.TreeHardSprout;

                                DssRef.world.subTileGrid.Set(npos, ntile);
                            }
                        }
                    }
                }
            }

            DssRef.state.detailMap.needReload = true;
        }
    }
}
