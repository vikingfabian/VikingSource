using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars
{

    partial class WorldData
    {
        public const byte FogOfWarView_None = 0;
        public const byte FogOfWarView_TerrainOnly = 1;
        public const byte FogOfWarView_Full = FogOfWarView_TerrainOnly + 6;
        public const byte FogOfWarView_LockedFull = byte.MaxValue;

        const int FogOfWarTimePerLevelSec = 20;
        GameTimeStamp lastFogCountDown = GameTimeStamp.None;


        public void async_fog_update()
        {
            if (DssRef.storage.ruleset_instance.module_fogOfWar)
            {
                if (lastFogCountDown.IsEmpty())
                {
                    lastFogCountDown = GameTimeStamp.Now();
                }
                else if (lastFogCountDown.hasSecPassed(FogOfWarTimePerLevelSec))
                {
                    lastFogCountDown = GameTimeStamp.Now();
                    var loop = tileGrid.LoopInstance();
                    while (loop.Next())
                    {
                        var tile = DssRef.world.tileGrid.Get(loop.Position);
                        if (tile.fogExploreLevel > FogOfWarView_TerrainOnly && tile.fogExploreLevel < FogOfWarView_LockedFull)
                        {
                            tile.fogExploreLevel--;
                            tileGrid.Set(loop.Position, tile);
                        }
                    }
                }

                //bool hasCrossMap = false;
                foreach (var p in DssRef.state.localPlayers)
                {
                    var armiesC = p.pfaction.GetFaction().armies.counter();
                    while (armiesC.Next())
                    {
                        async_fog_addViewPoint(WP.ToSumTilePos(armiesC.sel.position), armiesC.sel.fogViewRadius(), FogOfWarView_Full, 0.75f, 0.9f);
                    }

                    // = p.mapLayer() != Map.MapLayer.MapDetailLayerType.Layer1_Near;
                }
                fogTexture.refreshWorld();
                fogTexture.Update_async();
                DssRef.world.CrossMapTextureNeedsUpdate = true;
            }
        }

        public void async_fog_addCityView(City city, bool fullReveal)
        {
            if (DssRef.storage.ruleset_instance.module_fogOfWar)
            {
                city.fogExplored = true;
                async_fog_addViewPoint(WP.MaptileToSumTile(city.maptilePos), city.HallSetup().claimRadius_sum * 5, FogOfWarView_LockedFull, 0.6f, fullReveal? 0.8f : 0f);
            }
        }

        public void async_fog_addDiscoveredCity(City city)
        {
            if (DssRef.storage.ruleset_instance.module_fogOfWar)
            {
                city.fogExplored = true;
                city.pfaction.GetFaction().fogExplored = true;
                async_fog_addViewPoint(WP.MaptileToSumTile(city.maptilePos), 3, FogOfWarView_TerrainOnly, 1f, 0f);
            }
        }

        void async_fog_addViewPoint(IntVector2 sumTilePos, int viewRadius, byte viewLevel, float unhinderedViewPercent, float fullViewPercent)
        {
            PCity prevCity = PCity.Empty;
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

                if (dist < viewRadius /*&& (dist <= unhinderedViewPercent || tile.biomColorHeight <= centerHeight)*/)
                {
                    if (tile.pcity.HasValue() && tile.pcity != prevCity)
                    {
                        prevCity = tile.pcity;
                        if (!tile.pcity.City().fogExplored)
                        {
                            async_fog_addDiscoveredCity(tile.pcity.City());
                        }
                    }

                    byte explore =  dist < fullViewR ? viewLevel : FogOfWarView_TerrainOnly;
                    if (explore > tile.fogExploreLevel)
                    {
                        tile.fogExploreLevel = explore;
                        DssRef.world.tileGrid.Set(loop.Position, tile);
                    }
                }
            }
            
            //fogTexture.refreshArea(area);
        }
    }
}
