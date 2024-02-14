using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.ToggEngine.GO
{
    class BoardEscapePoint : AbsTileObject
    {
        Graphics.Mesh arrowModel;

        public BoardEscapePoint(IntVector2 pos)
            :base(pos)
        {
            arrowModel = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(0.8f),//cmdLib.ToV3(iconPosV2),
                Graphics.TextureEffectType.Flat, SpriteName.EditorForwardArrow, Color.White);

            arrowModel.Rotation = toggLib.Rotation1DToQuaterion(lib.V2ToAngle(IntVector2.Right.Vec) + MathHelper.Pi);

            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);

            arrowModel.Position = toggRef.board.toWorldPos_Center(position, 0.14f);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();

            arrowModel.DeleteMe();
        }

        override public TileObjectType Type { get { return TileObjectType.EscapePoint; } }
    }
}
