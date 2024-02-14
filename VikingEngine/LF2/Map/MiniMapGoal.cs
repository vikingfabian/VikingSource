using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Map
{
    struct MiniMapGoal
    {
        public bool Visible;
        IntVector2 chunk;
        Vector2 planePos;
        public Vector2 PlanePos { get { return planePos; } }

        public IntVector2 Chunk
        {
            set
            {
                chunk = value;
                WorldPosition wp = WorldPosition.EmptyPos;
                wp.ChunkGrindex = chunk;
                planePos = WorldPosition.V3toV2(wp.ToV3());
            }
            get
            {
                return chunk;
            }
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            Map.WorldPosition.WriteChunkGrindex_Static(chunk, w);//chunk.WriteStream(w);
            w.Write(Visible);
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            //chunk.ReadStream(r);
            this.Chunk = Map.WorldPosition.ReadChunkGrindex_Static(r);
            Visible = r.ReadBoolean();
        }

        public Rotation1D DirToTarget(Vector2 heroPlanePos)
        {
            return Rotation1D.FromDirection(planePos - heroPlanePos);
        }
    }
}
