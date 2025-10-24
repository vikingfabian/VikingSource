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
        Graphics.ImageAdvanced mapTexture = null, unitTexture = null;
        //Vector2 mapSize;
        public VectorRect area;
        Vector2 areaHalfSize;
        Vector2 textureSize;
        float scale = 2f;
        public MiniMap(LocalPlayer player)
        {
            Vector2 sz = Engine.Screen.IconSizeV2 * 5f;
            Vector2 pos = player.playerData.view.safeScreenArea.RightBottom - sz;
            area = new VectorRect(pos, sz);
            area.Round();
            areaHalfSize = area.Size * 0.5f;
            var bgArea = area;
            bgArea.AddRadius(4);

            Graphics.Image bg = new Image(SpriteName.WhiteArea, bgArea.Position, bgArea.Size, HudLib.GUILayer + 2);
            bg.Color = Color.Black;
            
            mapTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, DssRef.world.Size.Vec, ImageLayers.Background0, false, false);
            mapTexture.ImageSource = new Rectangle(0, 0, DssRef.world.Size.X, DssRef.world.Size.Y);
           
            unitTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, DssRef.world.Size.Vec, ImageLayers.Foreground0, false, false);
            unitTexture.ImageSource = new Rectangle(0, 0, DssRef.world.Size.X, DssRef.world.Size.Y);

            textureSize = mapTexture.size;

            renderTargetDrawContainer = new RenderTargetDrawContainer(area.Position, area.Size, HudLib.GUILayer, new List<AbsDraw> { mapTexture, unitTexture });

        }

        public void update(LocalPlayer player)
        {            
            mapTexture.Texture = player.minimapPixelTexture.texture;
            unitTexture.Texture = player.unitsPixelTexture.texture;

            Vector2 center = player.gameControls.map.camera.LookTargetXZ;

            mapTexture.size = textureSize * scale;
            mapTexture.position = -(center * scale) + areaHalfSize;

            unitTexture.size = mapTexture.size;
            unitTexture.position = mapTexture.position;
        }
    }
}
