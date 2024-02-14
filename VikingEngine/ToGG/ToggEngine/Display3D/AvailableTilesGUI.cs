using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG
{
    /// <summary>
    /// View all tiles that can be selected in some way
    /// </summary>
    class AvailableTilesGUI : Graphics.ImageGroup
    {
        public const float PlaneY = 0.01f;

        public AvailableTilesGUI(List<IntVector2> available, List<IntVector2> targets)
            :base(arraylib.CountSafe(available) + arraylib.CountSafe(targets))
        {
            Vector3 Scale = new Vector3(0.98f);

            if (available != null)
            {
                foreach (IntVector2 pos in available)
                {
                    Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.plane, toggRef.board.toWorldPos_Center(pos, PlaneY), Scale,
                      Graphics.TextureEffectType.Flat, SpriteName.cmdInteractiveSquareFrame, Color.White);

                    dot.Opacity = toggRef.board.tileGrid.Get(pos).Square.brightColors ? 0.8f : 0.3f;
                    Add(dot);
                }
            }

            if (targets != null)
            {
                //Places a white target square 
                foreach (IntVector2 pos in targets)
                {
                    Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.plane, new Vector3(pos.X, PlaneY, pos.Y), Scale,
                       Graphics.TextureEffectType.Flat, SpriteName.cmdInteractiveSquareFrame_Attack, Color.White);

                    dot.Opacity = toggRef.board.tileGrid.Get(pos).Square.brightColors ? 0.8f : 0.3f;
                    Add(dot);
                }
            }
        }
    }
}
