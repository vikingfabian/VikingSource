using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class MiniMap
    {
        RenderTargetDrawContainer renderTargetDrawContainer;
        Graphics.ImageAdvanced mapTexture = null;
        Vector2 mapSize;
        public MiniMap(LocalPlayer player)
        {
            Vector2 sz = Engine.Screen.IconSizeV2 * 4f;
            Vector2 pos = player.playerData.view.safeScreenArea.RightBottom - sz;

            var bgTex = DssRef.state.factionsMap.factionPixelTex.texture;
            mapSize = new Vector2(bgTex.Width, bgTex.Height);
            mapTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, mapSize, ImageLayers.Background0, false);
            mapTexture.Texture = bgTex;
            mapTexture.SetFullTextureSource();
            mapTexture.Color = Color.Gray;

            renderTargetDrawContainer = new RenderTargetDrawContainer(pos, sz, HudLib.GUILayer, new List<AbsDraw> { mapTexture });
            
            //Vector2 percCenterPos = 
            //mapTexture.Center 
        }
    }
}
