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
        float zoom = 1;
        public float scale = 1;

        Vector2 topLeft;

        public GeneratorMap(Vector2 pos)
        {
            topLeft = pos;
            texture = new FactionPixelTexture(-1, false, FactionMapFilter.Terrain);
            image = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, pos, Vector2.One, ImageLayers.Lay8, false);
        }

        public void resetPos()
        {
            image.position = topLeft;
        }

        public void userInput(InputMap input, bool mouseOverHud)
        {
            if ( !mouseOverHud)
            {
                if (input.mousePan.IsDown)
                {
                    image.position += Input.Mouse.MoveDistance + input.moveCursor.direction;
                }
                image.position -= input.move.directionAndTime * 4f;

                zoom = Bound.Set(zoom + input.ZoomValue() * -0.014f * zoom, 0.25f, 4f);



            }
            image.size = textureSize * zoom * scale;
        }

        public bool pointerToTilePos(Vector2 pointer, IntVector2 tileSize, out IntVector2 tilePos)
        {
            tilePos = IntVector2.Zero;
            var ar = image.Area;
            if (ar.IntersectPoint(pointer))
            {
                tilePos.Vec = ar.PositionToPercent(pointer) * tileSize.Vec;
                return true;
            }
            return false;
        }

        public Vector2 TileToScreenPos(IntVector2 tilePos, IntVector2 tileSize)
        {
           return image.Area.PercentToPosition(tilePos.Vec / tileSize.Vec);
        }

        public void generate()
        {
            texture.initTexture();
            texture.filter = arraylib.HasMembers(DssRef.world.cities)? FactionMapFilter.FactionCols : FactionMapFilter.Terrain;
            texture.refreshWorld();
           
            image.Texture = texture.texture;
            image.SetFullTextureSource();
            textureSize = new Vector2(texture.texture.Width, texture.texture.Height);
            scale = 1;
        }


        public void generateNodes(Map.Map2.NodeMap map)
        {
            
            texture.texture = map.texture;
            image.Texture = map.texture;
            image.SetFullTextureSource();
            textureSize = new Vector2(texture.texture.Width, texture.texture.Height);
            scale = 4;
            image.Visible = true;
        }

        public void generateIcon(Map.Map2.IconWorldData world)
        {
            texture.texture = new Graphics.PixelTexture(world.iconGrid.Size);

            refreshTexture(world);

            image.Texture = texture.texture;
            image.SetFullTextureSource();
            textureSize = new Vector2(texture.texture.Width, texture.texture.Height);
            scale = 2;
            image.Visible = true;
        }

        public void refreshTexture(Map.Map2.IconWorldData world)
        {
            var loop = world.iconGrid.LoopInstance();
            while (loop.Next())
            {
                var t = world.iconGrid.Get(loop.Position);
                texture.texture.SetPixel(loop.Position, t.color);
            }

            texture.texture.ApplyPixelsToTexture();

        }

        public void hide()
        {
            image.Visible = false;
        }
    }
}
