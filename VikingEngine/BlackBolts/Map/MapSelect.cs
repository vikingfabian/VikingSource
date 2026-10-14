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
        Mesh directionArrow;

        public MapSelect()
        {
            selectFrame = new Mesh(LoadedMesh.plane, Vector3.Zero, Vector3.One, TextureEffectType.Flat, SpriteName.LFMenuRectangleSelection, Color.White);
            directionArrow = new Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(0.6f), TextureEffectType.Flat, SpriteName.cmdConvertArrow, Color.White);
        }

        public void Rotation(Dir4 dir)
        {
            directionArrow.Rotation.QuadRotation = Quaternion.Identity;
            directionArrow.Rotation.RotateWorldX(-MathExt.TauOver4 * ((int)dir - 1));
        }

        public void Select(IntVector2 pos)
        {
            selectFrame.position = WP.TileToWp(pos);
            selectFrame.position.Y = 0.02f;
            directionArrow.position = selectFrame.position;
        }
    }
}
