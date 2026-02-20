using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    class Mesh_MultiLayer : Mesh
    {
        public int layer1Index = -1;
        public int layer1 = -1;
        public int layer2Index = -1;
        public int layer2 = -1;

        public Mesh_MultiLayer(LoadedMesh mesh, Vector3 pos, Vector3 scale,
            TextureEffectType effectType, SpriteName sprite, Color col)
            : base(mesh, pos, scale, effectType, sprite, col, false)
        { }

        public void AddToLayer1(int layer)
        {
#if DEBUG
            if (layer1Index >= 0)
            {
                throw new Exception();
            }
#endif
            layer1 = layer;
            Ref.draw.AddToRenderList(this, ref layer1Index, layer1, true);
            inRenderList = true;
        }
        public void AddToLayer2(int layer)
        {
#if DEBUG
            if (layer2Index >= 0)
            {
                throw new Exception();
            }
#endif
            layer2 = layer;
            Ref.draw.AddToRenderList(this, ref layer2Index, layer2, true);
            inRenderList = true;
        }

        public override void DeleteMe()
        {
            //base.DeleteMe();
            if (inRenderList)
            {
                if (layer1Index >= 0)
                {
                    Ref.draw.AddToRenderList(this, ref layer1Index, layer1, false);
                    layer1Index = -1;
                }

                if (layer2Index >= 0)
                {
                    Ref.draw.AddToRenderList(this, ref layer2Index, layer2, false);
                    layer2Index = -1;
                }
            }

            inRenderList = false;
        }
    }
}
