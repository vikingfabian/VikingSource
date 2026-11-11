using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.Map.MapLayer
{
    abstract class AbsFarLayer : AbsMapLayer
    {
        protected CrossHeightMap crossHeightMap;

        public AbsFarLayer(int layer, CrossHeightMap crossHeightMap)
            :base(layer)    
        {
            this.crossHeightMap = crossHeightMap;
        }

        public void Draw(int cameraIndex, LocalPlayer player)
        {            
            crossHeightMap.Draw(cameraIndex, player.factionPixelTexture.texture, 0);
            //if (DssRef.storage.ruleset_instance.module_fogOfWar)
            //{
            //    crossHeightMap.Draw(cameraIndex, DssRef.world.fogTexture.texture, 0.2f);
            //}
        }
        
    }
}
