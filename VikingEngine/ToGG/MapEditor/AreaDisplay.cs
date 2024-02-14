using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.MapEditor
{
    class AreaDisplay
    {
        List<Mesh> models;

        public AreaDisplay(List<IntVector2> tiles)
        {
            models = new List<Mesh>(tiles.Count);

            foreach (var m in tiles)
            {
                Vector3 pos = toggRef.board.toWorldPos_Center(m, -0.04f);
                Mesh model = new Mesh(LoadedMesh.cube_repeating, pos, new Vector3(0.14f),
                     TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.DarkBlue, true);
                models.Add(model);
            }
        }

        public void DeleteMe()
        {
            arraylib.DeleteAndClearArray(models);
        }
    }
}
