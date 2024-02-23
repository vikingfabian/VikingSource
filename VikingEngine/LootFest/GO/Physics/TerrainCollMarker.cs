using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest
{
    class TerrainCollMarker : LazyUpdate
    {
        Graphics.Mesh img;
        float time = 2000;
        public TerrainCollMarker(VectorVolumeC box)
            :base(true)
        {

            img = new Graphics.Mesh(LoadedMesh.cube_repeating, box.Center, box.HalfSize * 2.4f, TextureEffectType.Flat, SpriteName.InterfaceBorder, Color.White);
                //new Graphics.TextureEffect(
                //  TextureEffectType.Flat, SpriteName.InterfaceBorder), , Vector3.Zero);
        }
        public override void Time_Update(float time)
        {
            this.time -= time;
            if (this.time <= 0)
            {
                DeleteMe();
                img.DeleteMe();
            }
        }
    }
}
