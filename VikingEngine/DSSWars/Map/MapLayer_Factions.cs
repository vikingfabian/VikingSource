using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map
{
    class MapLayer_Factions
    {
        IntVector2 mapsz;
        //public Map.FactionPixelTexture factionPixelTex;

        MapTexturePlane mapPlane, unitPlane;

        public MapLayer_Factions()
        {
            //mapsz = DssRef.world.Size;

            mapPlane = new MapTexturePlane();
            unitPlane = new MapTexturePlane();
            unitPlane.Y += 0.06f;
            //factionPixelTex = new FactionPixelTexture(true,
            //    (DssRef.settings.playType == GameState.PlayStateType.Play || DssRef.settings.playType == GameState.PlayStateType.MapEditor)? 
            //    FactionMapFilter.FactionCols : FactionMapFilter.Terrain);           
        }

        public void Draw(int cameraIndex, LocalPlayer player)
        {
            mapPlane.texture = player.factionPixelTexture.texture;
            unitPlane.texture = player.unitsPixelTexture.texture;

            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            mapPlane.Draw(cameraIndex);
            unitPlane.Draw(cameraIndex);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        }

        public void asyncTask()
        {
            if (mapsz != DssRef.world.Size)
            { 
                mapsz = DssRef.world.Size;
                mapPlane.refreshScale();
                unitPlane.refreshScale();

                foreach (var p in DssRef.state.localPlayers)
                {
                    p.factionPixelTexture.initTexture();
                }
            }

            foreach (var p in DssRef.state.localPlayers)
            {
                p.factionPixelTexture.refreshWorld();
            }
                //factionPixelTex.refreshWorld();
        }

        //public void syncTask()
        //{
        //    factionPixelTex.SetNewTexture();
        //}
    }
}
