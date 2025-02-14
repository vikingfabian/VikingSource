using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class GeneratorMap
    {
        FactionColorsTexture texture;
        Graphics.ImageAdvanced image;

        Vector2 textureSize;
        float scale = 1;

        public GeneratorMap(Vector2 pos)
        {
            texture = new FactionColorsTexture();
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
            if (DssRef.world.cities == null)
            {
                texture.RefreshWorld_TerrainCol();
            }
            else
            {
                texture.RefreshWorld_FactionCol();
            }
            image.Texture = texture.texture;
            image.SetFullTextureSource();
            textureSize = new Vector2(texture.texture.Width, texture.texture.Height);
        }
    }
}
