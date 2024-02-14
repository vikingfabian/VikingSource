using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.Display3D
{
    class CatapultTargetGUI
    {
        const float Y = 0.02f;

        Graphics.Mesh[] squares;

        public CatapultTargetGUI(IntVector2 pos)
        {
            squares = new Graphics.Mesh[9];

            for (int i = 0; i < squares.Length; ++i)
            {
                squares[i] = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(0.6f),
                    Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.White);
            }

            setVisible(false);
            //setPosition(pos);
        }

        public void setPosition(IntVector2 pos)
        {
            Rectangle2 area = new Rectangle2(pos, 1);

            int i = 0;
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                squares[i].Visible = true;
                squares[i++].Position = toggRef.board.toWorldPos_Center(loop.Position, Y);
            }
        }

        public void viewHitPos(IntVector2 pos)
        {
            setVisible(false);

            squares[0].Position = toggRef.board.toWorldPos_Center(pos, Y);
            squares[0].Visible = true;
        }

        public void setVisible(bool visible)
        {
            for (int i = 0; i < squares.Length; ++i)
            {
                squares[i].Visible = visible;
            }
        }

        public void DeleteMe()
        {
            for (int i = 0; i < squares.Length; ++i)
            {
                squares[i].DeleteMe();
            }
        }
    }
}
