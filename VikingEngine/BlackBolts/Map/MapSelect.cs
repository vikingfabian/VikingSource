using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.Map
{
    class MapSelect
    {
        Mesh selectFrame;

        public MapSelect()
        {
            selectFrame = new Mesh(LoadedMesh.plane, Vector3.Zero, Vector3.One, TextureEffectType.Flat, SpriteName.LFMenuRectangleSelection, Color.White);

        }

        public void Select(IntVector2 pos)
        {
            selectFrame.position = VectorExt.V2toV3XZ(pos.Vec);
            selectFrame.position.Y = 0.02f;
        }
    }
}
