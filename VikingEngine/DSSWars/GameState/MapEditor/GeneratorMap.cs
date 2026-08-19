using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class GeneratorMap
    {
        FactionPixelTexture texture;
        Graphics.ImageAdvanced image;

        Vector2 textureSize;
        float scale = 1;

        public GeneratorMap(Vector2 pos)
        {
            texture = new FactionPixelTexture(-1, false, FactionMapFilter.Terrain);
            image = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, pos, Vector2.One, ImageLayers.Lay8, false);
        }

        public void userInput(bool mouseOverHud)
        {
            if ( !mouseOverHud)
            {
                if (Input.Mouse.IsButtonDown(MouseButton.Left))
                {
                    image.position += Input.Mouse.MoveDistance;
                }

                scale = Bound.Set(scale + lib.ToLeftRight(Input.Mouse.ScrollValue) * 0.1f, 0.25f, 4f);

               
            }
            image.size = textureSize * scale;
        }

        public void generate()
        {
            texture.initTexture();
            texture.filter = arraylib.HasMembers(DssRef.world.cities)? FactionMapFilter.FactionCols : FactionMapFilter.Terrain;
            texture.refreshWorld();
           
            image.Texture = texture.texture;
            image.SetFullTextureSource();
            textureSize = new Vector2(texture.texture.Width, texture.texture.Height);
        }


        public void generateNodes(Map.Map2.NodeMap map)
        {
            
            texture.texture = map.texture;
            image.Texture = map.texture;
            image.SetFullTextureSource();
            textureSize = new Vector2(texture.texture.Width, texture.texture.Height) * Map.Map2.NodeMap.NodePixWidth;
            image.Visible = true;
        }

        public void generateIcon(Map.Map2.IconWorldData world)
        {
            texture.texture = new Graphics.PixelTexture(world.iconGrid.Size);

            var loop = world.iconGrid.LoopInstance();
            while (loop.Next())
            {
                var t = world.iconGrid.Get(loop.Position);
                texture.texture.SetPixel(loop.Position, t.color);
            }

            texture.texture.ApplyPixelsToTexture();
            
            image.Texture = texture.texture;
            image.SetFullTextureSource();
            textureSize = new Vector2(texture.texture.Width, texture.texture.Height);
            image.Visible = true;
        }

        public void hide()
        {
            image.Visible = false;
        }
    }
}
