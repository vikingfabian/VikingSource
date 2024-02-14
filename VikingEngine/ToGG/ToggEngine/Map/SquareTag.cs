using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class SquareTag : ToggEngine.GO.AbsTileObject
    {
        public byte TagId = 0;
        Graphics.Text3DBillboard model;

        public SquareTag(IntVector2 pos, object args)
            :base(pos)
        {
            if (args != null)
            {
                TagId = (byte)args;
            }

            if (toggRef.InEditor)
            {
                model = new Graphics.Text3DBillboard(LoadedFont.Console, "Tag00", Color.Black, Color.White, Vector3.Zero,
                    0.2f, 1f, true);
                model.FaceCamera = false;
                refreshModel();
            }
            newPosition(pos);
        }

        public void refreshModel()
        {
            if (model != null)
            {
                model.TextString = "Tag" + TagId.ToString();
            }
        }

        public override string ToString()
        {
            return model.TextString;
        }

        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);
            w.Write(TagId);
        }
        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);
            TagId = r.ReadByte();

            refreshModel();
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            if (model != null)
            {
                model.Position = toggRef.board.toWorldPos_Center(position, 0.05f);
                model.Z -= 0.2f;
            }
        }

        public override void DeleteMe()
        {
            model?.DeleteMe();
            base.DeleteMe();
        }

        override public ToggEngine.TileObjectType Type { get { return ToggEngine.TileObjectType.SquareTag; } } 
    }
}
