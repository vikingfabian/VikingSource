using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Editor
{
    struct UndoAction : IBinaryIOobj
    {
        ////public List<Voxel> Voxels;
        byte[] voxels;
        public RangeIntV3 selectionArea;
        public Map.WorldPosition WorldPos;
        public UndoType Type;
        //DateTime time;

        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write(voxels.Length);
            w.Write(voxels);
            //w.Write(Voxels.Count);
            //for (int i = 0; i < Voxels.Count; i++)
            //{
            //    Voxels[i].WriteStream(w);
            //}
            selectionArea.WriteStream(w);
            //WorldPos.UpdateWorldGridPos();
            WorldPos.WorldGrindex.WriteStream(w);
            w.Write((byte)Type);
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            int num = r.ReadInt32();
            voxels = r.ReadBytes(num);
            //Voxels = new List<Voxel>();
            //for (int i = 0; i < num; i++)
            //{
            //    Voxels.Add(Voxel.FromStream(r));
            //}
            selectionArea.ReadStream(r);
            WorldPos.WorldGrindex.ReadStream(r);
            //WorldPos.UpdateChunkPos();
            Type = (UndoType)r.ReadByte();
        }
        public static UndoAction FromStream(System.IO.BinaryReader r)
        {
            UndoAction result = new UndoAction();
            result.ReadStream(r);
            return result;
        }

        public UndoAction(RangeIntV3 selectionArea, Map.WorldPosition wp, Voxels.UndoType Type)
        {
            //time = DateTime.Now;
            this.Type = Type;
            this.WorldPos = wp;
            selectionArea.Max += 1;
            this.selectionArea = selectionArea;
            //Voxels = new List<Voxel>();
            IntVector3 dimentions = selectionArea.Add;
            voxels = new byte[dimentions.X * dimentions.Y * dimentions.Z];
            IntVector3 pos = IntVector3.Zero;
            int i = 0;
            for (pos.Z = selectionArea.Min.Z; pos.Z < selectionArea.Max.Z; pos.Z++)
            {
                for (pos.Y = selectionArea.Min.Y; pos.Y < selectionArea.Max.Y; pos.Y++)
                {
                    for (pos.X = selectionArea.Min.X; pos.X < selectionArea.Max.X; pos.X++)
                    {
                        //Voxels.Add(new Voxel(pos, LfRef.chunks.GetSafe(wp.ReturnWPVoxelPos_Safe(pos))));
                        voxels[i] = LfRef.chunks.GetSafe(wp.GetNeighborPos(pos));
                        i++;
                    }
                }
            }

        }
        public void Undo()
        {
            //foreach (Voxel v in Voxels)
            //{
            //    LfRef.chunks.SetIfOpen(WorldPos.ReturnWPVoxelPos_Safe(v.Position), v.Material);
            //}
            //WorldPos.UpdateWorldGridPos();
            IntVector3 pos = IntVector3.Zero;
            int i = 0;
            for (pos.Z = selectionArea.Min.Z; pos.Z < selectionArea.Max.Z; pos.Z++)
            {
                for (pos.Y = selectionArea.Min.Y; pos.Y < selectionArea.Max.Y; pos.Y++)
                {
                    for (pos.X = selectionArea.Min.X; pos.X < selectionArea.Max.X; pos.X++)
                    {
                        LfRef.chunks.SetIfOpen(WorldPos.GetNeighborPos(pos), voxels[i]);
                        i++;
                    }
                }
            }
        }
        
    }
    
}
