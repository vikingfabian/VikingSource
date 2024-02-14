using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class AttackerArrowGUI
    {
        public Graphics.Mesh attackerArrow;
        public List2<Graphics.Mesh> dottedLine = new List2<Graphics.Mesh>();
        public IntVector2 storedTarget;

        public AttackerArrowGUI()
        {
            attackerArrow = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(1f),
                Graphics.TextureEffectType.Flat, SpriteName.cmdAttackDirectionRed, Color.White);
        }
        
        public void updateAttackerArrow(AbsUnit fromAttacker, IntVector2 target)
        {
            const float Y = 0.06f;

            storedTarget = target;

            Vector2 center;
            if (fromAttacker != null)
            {
                center = fromAttacker.squarePos.Vec;
            }
            else
            {
                center = Vector2.Zero;
            }
            Vector2 dir = toggRef.board.toWorldPosXZ_Center(target) - center;
            float l;
            Vector2 dir_n = VectorExt.Normalize(dir, out l);

            arraylib.DeleteAndClearArray(dottedLine);

            //float l = dir.Length();
            if (l > 0.5f && fromAttacker != null)
            {
                const float ArrowOffset = 0.32f;
                attackerArrow.Position = fromAttacker.soldierModel.Position + VectorExt.V2toV3XZ(dir_n * ArrowOffset);
                attackerArrow.Y = Y;

                attackerArrow.Visible = true;
                attackerArrow.Rotation = toggLib.Rotation1DToQuaterion(lib.V2ToAngle(dir));

                if (l >= 1.5f)
                {
                    const float DotSpacing = 0.3f;
                    const float DotScale = DotSpacing * 0.5f;

                    int count = (int)((l - 0.5f - DotSpacing) / DotSpacing);
                       
                    Vector3 dotPos = VectorExt.V2toV3XZ(center, Y);
                    Vector3 move_n = VectorExt.V2toV3XZ(dir_n, 0);
                    Vector3 moveSpace = move_n * DotSpacing;

                    dotPos += move_n * (0.8f + DotSpacing * 0.6f);

                    for (int i = 0; i < count; ++i)
                    {
                        Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.plane, dotPos,
                            new Vector3(DotScale), Graphics.TextureEffectType.Flat, 
                            SpriteName.cmdAttackDirectionRedArrow, Color.White);
                        dot.Rotation = attackerArrow.Rotation;
                        dottedLine.Add(dot);

                        dotPos += moveSpace;
                    }
                }
            }
            else
            {
                attackerArrow.Visible = false;
            }
        }

        public void DeleteMe()
        {
            arraylib.DeleteAndClearArray(dottedLine);
            attackerArrow.DeleteMe();
        }
    }
}
