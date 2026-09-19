using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map.MapLayer
{
    

    class MapLayer_Overview : AbsFarLayer
    {
        public Borders borders;
        public UnitMiniModels unitMiniModels;

        int state_Processing_Sych_Complete = 2;
        MapLayer_Factions factionsMap;
        public bool bRefreshTimer = false;
        public bool bRefreshDataRecieved = false;

        public MapLayer_Overview(MapLayer_Factions factionsMap, CrossHeightMap crossHeightMap)
            :base(DrawGame.MidLayer, crossHeightMap)
        {
            this.factionsMap = factionsMap;
            
            Ref.draw.CurrentRenderLayer = DrawGame.MidLayer;

            //createModel(generateTerrain());
            

           // WaterModel(DrawGame.MidLayer);
            
            //waterBottom.Y = waterSurface.Y - 0.1f;

            //if (DssRef.state.PlayType() != GameState.PlayStateType.BattleLab)
            //{
            //    borders = new Borders();
            //}
            Ref.draw.CurrentRenderLayer = 0;

            unitMiniModels = new UnitMiniModels();
        }
        public void update()
        {
            updateWaterTexture();
            //crossHeightMap.update(DssRef.state.LocalHost());
        }

        

        public void refresh_async()
        {
            if (bRefreshTimer && bRefreshDataRecieved)
            {
                bRefreshTimer = false;
                bRefreshDataRecieved = false;

                var polygons = crossHeightMap.generateTerrain();
                Ref.update.AddSyncAction(new SyncAction1Arg<List<Graphics.CrossPolygonColor>>(crossHeightMap.createModel, polygons));
            }
        }

        //const float TileSideHeight = 1.5f;
       
        //Graphics.PolygonColor side(Vector3 v1, Vector3 v2, Sprite sideTex, Color col, float height)
        //{
            
        //    Vector3 sw = v1;
        //    sw.Y -= height;

        //    Vector3 se = v2;
        //    se.Y -= height;


        //    return new Graphics.PolygonColor(
        //        new Vector3[]
        //        {
        //            v1, v2,  sw,se,
        //        },
        //        sideTex, col);
        //}

        public void runAsyncTask()
        {
            if (state_Processing_Sych_Complete == 2 && DssRef.world.BordersUpdated /*|| StartupSettings.AlwaysRefreshMap*/)
            {
                DssRef.world.BordersUpdated = false;
                state_Processing_Sych_Complete = 0;

                borders?.quedEvent();
                if (DssRef.state.PlayType() != GameState.PlayStateType.BattleLab)
                {
                    //factionsMap.asyncTask();
                    foreach (var p in DssRef.state.localPlayers)
                    {
                        p.factionPixelTexture.refreshWorld();
                        p.minimapPixelTexture.refreshWorld();
                    }
                }
                state_Processing_Sych_Complete = 1;
            }
        }
        public void HalfSecondUpdate()
        {
            if (DssLib.UpdateBorders && 
                state_Processing_Sych_Complete == 1)
            {
                borders?.SetNewModel();
                state_Processing_Sych_Complete = 2;
            }

            unitMiniModels.update();
        }
        

    }
}
