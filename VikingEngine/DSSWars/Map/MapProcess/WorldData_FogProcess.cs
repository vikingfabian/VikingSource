using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars
{

    partial class WorldData
    {
        public void async_fog_update()
        {
            if (DssRef.storage.ruleset_instance.module_fogOfWar)
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    var armiesC = p.pfaction.GetFaction().armies.counter();
                    while (armiesC.Next())
                    {
                        int view = 4 + MathExt.MultiplyInt(armiesC.sel.groups.Count, 0.75);
                        async_fog_addViewPoint(WP.MaptileToSumTile(armiesC.sel.maptilePos), view, 0.75f, 0.9f);
                    }
                }

                fogTexture.Update_async();
            }
        }

        public void async_fog_addCityView(City city)
        {
            if (DssRef.storage.ruleset_instance.module_fogOfWar)
            {
                async_fog_addViewPoint(WP.MaptileToSumTile(city.maptilePos), city.HallSetup().claimRadius_sum * 3, 0.6f, 0.8f);
            }
        }

        void async_fog_addViewPoint(IntVector2 sumTilePos, int viewRadius, float unhinderedViewPercent, float fullViewPercent)
        {
            int centerHeight = DssRef.world.tileGrid.Get(sumTilePos).biomColorHeight +1;
            float unhinderedViewR = viewRadius * unhinderedViewPercent;
            float fullViewR = viewRadius * fullViewPercent;
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(sumTilePos, viewRadius);
            area.SetBounds(DssRef.world.tileGrid.Area);
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                var tile = DssRef.world.tileGrid.Get(loop.Position);
                float dist = (loop.Position - sumTilePos).Length();

                if (dist <= viewRadius /*&& (dist <= unhinderedViewPercent || tile.biomColorHeight <= centerHeight)*/)
                {
                    Map.MapData.FogOfWar_ExlporeLevel explore =  dist <= fullViewR ? Map.MapData.FogOfWar_ExlporeLevel.Full : Map.MapData.FogOfWar_ExlporeLevel.TerrainOnly;
                    if (explore > tile.fogExploreLevel)
                    {
                        tile.fogExploreLevel = explore;
                        DssRef.world.tileGrid.Set(loop.Position, tile);
                    }
                }
            }
            
            fogTexture.refreshArea(area);
        }
    }
}
