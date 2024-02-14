using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    struct SpawnPointData
    {
        public int playerIx;
        public int spawnType;

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((byte)playerIx);
            w.Write((byte)spawnType);
        }

        public void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            playerIx = r.ReadByte();
            spawnType = r.ReadByte();
        }
    }

    class SpawnPoint : ToggEngine.GO.AbsTileObject
    {
        public SpawnPointData data;
        Graphics.Text3DBillboard model;
        
        public SpawnPoint(IntVector2 pos, object args)
            :base(pos)
        {
            if (args != null)
            {
                data = (SpawnPointData)args;
            }

            if (toggRef.InEditor)
            {
                model = new Graphics.Text3DBillboard(LoadedFont.Console, "00000000", Color.Black, Color.LightBlue, Vector3.Zero,
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
                model.TextString = "P" + data.playerIx.ToString() + ":SPW" + data.spawnType.ToString();
            }
        }

        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);
            data.Write(w);
                //w.Write((byte)playerIx);
                //w.Write((byte)spawnType);
        }

        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);
            data.Read(r, version);
            //playerIx = r.ReadByte();
            //spawnType  = r.ReadByte();

            refreshModel();
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            if (model != null)
            {
                model.Position = toggRef.board.toWorldPos_Center(position, 0.05f);
                model.Z += 0.2f;
            }
        }

        public override string ToString()
        {
            return "Player" + data.playerIx.ToString() + ": Spawnpoint " + data.spawnType.ToString();
            //model.TextString;
        }

        public override void DeleteMe()
        {
            if (model != null) model.DeleteMe();
            base.DeleteMe();
        }

        override public ToggEngine.TileObjectType Type { get { return ToggEngine.TileObjectType.SpawnPoint; } } 
    }
}