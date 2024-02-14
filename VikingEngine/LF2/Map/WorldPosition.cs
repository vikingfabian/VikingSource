using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Map
{

    struct WorldPosition
    {
        #region CONSTANTS

        public static readonly WorldPosition EmptyPos = new WorldPosition();

        public static readonly RangeIntV3 ChunkLimits = new RangeIntV3(IntVector3.Zero, new IntVector3(MaxChunkXZ, MaxChunkY, MaxChunkXZ));
        public static readonly Rectangle2 ChunkArea = new Rectangle2(new IntVector2(MaxChunkXZ));
        public static readonly Vector2 ChunkPlaneSize = Vector2.One * ChunkWidth;
        public static readonly IntVector3 ChunkSize = new IntVector3(ChunkWidth, ChunkHeight, ChunkWidth);
        public static readonly Vector3 ChunkHalfV3Sz = new Vector3(ChunkWidth, ChunkHeight, ChunkWidth) * PublicConstants.Half;
        public const int PassableGroundY = 2;

        public const byte EmptyBlock = 0;
        public const int ChunkWidth = 32;
        public const int ChunkBitsWidth = 5;
        public const int ChunkWidthSquare = ChunkWidth * ChunkWidth;
        public const int ChunkBitsWidthSquare = ChunkBitsWidth * 2;
        public const int ChunkHalfWidth = ChunkWidth / PublicConstants.Twice;
        public const int ChunkQuarterWidth = ChunkWidth / 4;
        public const int ChunkHeight = 92;//64;
        public const int ChunkHalfHeight = ChunkHeight / PublicConstants.Twice;
        public const int MaxChunkY = ChunkHeight - 1;
        public const int MaxChunkXZ = ChunkWidth - 1;
#if CMODE
        public const int WorldChunkSize = 512;
#else
        const int WorldChunkSize = 256;
#endif
        public const int WorldChunksX = WorldChunkSize;
        public const int WorldChunksY = WorldChunkSize;

        public const int ScreenSpawnGridPositions = 3;
        //totalt 3072 skärmar i hela världen
        public const int ChunkStandardHeight = 12;

        public const int WorldSizeX = WorldChunksX * ChunkWidth;
        public const int WorldSizeZ = WorldChunksY * ChunkWidth;

        public static readonly IntVector2 CenterChunk = new IntVector2((short)Map.WorldPosition.WorldChunksX / PublicConstants.Twice, (short)Map.WorldPosition.WorldChunksY / PublicConstants.Twice);
        public static readonly WorldPosition WorldCenter = new WorldPosition(CenterChunk, new IntVector3(0, 1, 0));

        #endregion

        public IntVector3 WorldGrindex;

        public int X { get { return WorldGrindex.X; } set { WorldGrindex.X = value; } }
        public int Y { get { return WorldGrindex.Y; } set { WorldGrindex.Y = value; } }
        public int Z { get { return WorldGrindex.Z; } set { WorldGrindex.Z = value; } }

        #region INIT

        public WorldPosition(IntVector3 worldGrindex)
        {
            this.WorldGrindex = worldGrindex;
        }
        public WorldPosition(Vector3 unitPos)
            : this(IntVector3.FromV3(unitPos))
        { }

        public WorldPosition(IntVector2 chunk)
            : this()
        {
            ChunkGrindex = chunk;
        }
        public WorldPosition(IntVector2 chunk, IntVector3 localBlockPos)
            : this()
        {
            SetChunkAndLocalBlock(chunk, localBlockPos);
        }
        public WorldPosition(IntVector2 chunk, int localX, int localY, int localZ)
            : this()
        {
            SetChunkAndLocalBlock(chunk, localX, localY, localZ);
        }




        #endregion

        #region GRINDEX_CONVERT
        public IntVector2 ChunkGrindex
        {
            get { return new IntVector2(WorldGrindex.X >> ChunkBitsWidth, WorldGrindex.Z >> ChunkBitsWidth); }//WorldGrindex.X / ChunkWidth, WorldGrindex.Z / ChunkWidth); }
            set
            {
                ClearChunkPos();
                WorldGrindex.X += value.X << ChunkBitsWidth;//value.X * ChunkWidth;
                WorldGrindex.Z += value.Y << ChunkBitsWidth;//value.Y * ChunkWidth;

            }
        }
        public IntVector3 LocalBlockGrindex
        {
            get
            {
                IntVector3 result = WorldGrindex;
                result.X &= ChunkBitsZero;
                result.Z &= ChunkBitsZero;
                return result;
            }
            set
            {
                ClearLocalPos();
                WorldGrindex.X += value.X;
                WorldGrindex.Y = value.Y;
                WorldGrindex.Z += value.Z;
            }
        }

        public int LocalBlockX
        {
            get { return WorldGrindex.X & ChunkBitsZero; }
        }
        public int LocalBlockY
        {
            get { return WorldGrindex.Y; }
        }
        public int LocalBlockZ
        {
            get { return WorldGrindex.Z & ChunkBitsZero; }
        }



        public int ChunkX
        {
            get { return WorldGrindex.X >> ChunkBitsWidth; }
            set
            {
                ClearChunkPosX();
                WorldGrindex.X += value << ChunkBitsWidth;
            }
        }
        public int ChunkY
        {
            get { return WorldGrindex.Z >> ChunkBitsWidth; }
            set
            {
                ClearChunkPosY();
                WorldGrindex.Z += value << ChunkBitsWidth;
            }
        }

        public void SetChunkAndLocalBlock(IntVector2 chunk, IntVector3 blockPos)
        {
            WorldGrindex.X = (chunk.X << ChunkBitsWidth) + blockPos.X;
            WorldGrindex.Y = blockPos.Y;
            WorldGrindex.Z = (chunk.Y << ChunkBitsWidth) + blockPos.Z;
        }
        public void SetChunkAndLocalBlock(IntVector2 chunk, int localX, int localY, int localZ)
        {
            WorldGrindex.X = (chunk.X << ChunkBitsWidth) + localX;
            WorldGrindex.Y = localY;
            WorldGrindex.Z = (chunk.Y << ChunkBitsWidth) + localZ;
        }
        public Vector2 ChunkPosFloating
        {
            get { return ChunkGrindex.Vec; }//new Vector2(ChunkGrindex.X + (float)LocalBlockGrindex.X / ChunkWidth, ChunkGrindex.Y + (float)LocalBlockGrindex.Z / ChunkWidth); }
        }

        public const int LocalGridBitsZero = int.MaxValue - MaxChunkXZ;
        public const int ChunkBitsZero = MaxChunkXZ;

        /// <summary>
        /// Subtracts world grindex so the local grid pos is 0,0,0
        /// </summary>
        public void ClearLocalPos()
        {
            WorldGrindex.X = WorldGrindex.X & LocalGridBitsZero;
            WorldGrindex.Y = 0;
            WorldGrindex.Z = WorldGrindex.Z & LocalGridBitsZero;
        }

        /// <summary>
        /// Subtracts the width of all chunks, keeping only the local block pos
        /// </summary>
        public void ClearChunkPos()
        {
            WorldGrindex.X = WorldGrindex.X & ChunkBitsZero;
            WorldGrindex.Z = WorldGrindex.Z & ChunkBitsZero;
        }

        public void ClearChunkPosX()
        {
            WorldGrindex.X = WorldGrindex.X & ChunkBitsZero;
        }
        public void ClearChunkPosY()
        {
            WorldGrindex.Z = WorldGrindex.Z & ChunkBitsZero;
        }

        #endregion

        /// <summary>
        /// Returns the distance between two individual blocks anywhere in the world
        /// </summary>
        /// <param name="b1">Block 1</param>
        /// <param name="b2">Block 2</param>
        /// <returns>The distance</returns>
        public static float CalculateBlockDistance(WorldPosition b1, WorldPosition b2)
        {
            IntVector3 diffVec = b2.WorldGrindex - b1.WorldGrindex;
            double distanceSquared = (diffVec.X * diffVec.X + diffVec.Y * diffVec.Y + diffVec.Z * diffVec.Z);
            return (float)Math.Sqrt(distanceSquared);
        }

        /// <summary>
        /// Returns the distance between this and another block anywhere in the world
        /// </summary>
        /// <param name="b2">Block 2</param>
        /// <returns>The distance</returns>
        public float CalculateBlockDistance(WorldPosition b2)
        {
            IntVector3 diffVec = b2.WorldGrindex - WorldGrindex;
            float distanceSquared = (diffVec.X * diffVec.X + diffVec.Y * diffVec.Y + diffVec.Z * diffVec.Z);
            return (float)Math.Sqrt((double)distanceSquared);
        }

        public Chunk Screen
        {
            get
            {
                return LfRef.chunks.GetScreen(ChunkGrindex);
            }
        }
        public Chunk ScreenUnsafe
        {
            get
            {
                return LfRef.chunks.GetScreenUnsafe(ChunkGrindex);
            }
        }
        public Map.ChunkData ChunkData
        {
            get
            {
                return LfRef.worldOverView.GetChunkData(ChunkGrindex);
            }
        }
        public void SetFromGroundY(int add)
        {
            WorldGrindex.Y = (int)LfRef.chunks.GetScreen(ChunkGrindex).GetGroundY(this) + add;
        }

        public float GetGroundY()
        {
            return LfRef.chunks.GetScreen(ChunkGrindex).GetGroundY(this);
        }

        public WorldPosition GetNeighborPos(IntVector3 posAdd)
        {
            WorldPosition result = this;
            result.WorldGrindex += posAdd;
            return result;
        }

        public static WorldPosition GetRandomPosOnChunk(IntVector2 chunk)
        {
            WorldPosition wp = new WorldPosition(chunk);
            wp.WorldGrindex.X += Ref.rnd.Int(ChunkWidth);
            wp.WorldGrindex.Z += Ref.rnd.Int(ChunkWidth);
            return wp;
        }




        public static WorldPosition ScreenSpawnPos(IntVector2 chunk, int gridX, int gridY)
        {
            const int SpawnBorder = 4;
            const int ScreenCellSize = (ChunkWidth - SpawnBorder * PublicConstants.Twice) / ScreenSpawnGridPositions;

            return new WorldPosition(new IntVector3(
                chunk.X * ChunkWidth + gridX * ScreenCellSize + SpawnBorder,
                0,
                chunk.Y * ChunkWidth + gridY * ScreenCellSize + SpawnBorder));

        }

        public static IntVector2 V3ToChunkPos(Vector3 position)
        {
            return new IntVector2((int)position.X >> ChunkBitsWidth, (int)position.Z >> ChunkBitsWidth);
        }

        public static Vector3 V2toV3(Vector2 pos)
        {
            Vector3 ret = Vector3.Zero;
            ret.X = pos.X;
            ret.Z = pos.Y;
            return ret;
        }
        public static Vector3 V2toV3(Vector2 pos, float heightPos)
        {
            Vector3 ret = Vector3.Zero;
            ret.X = pos.X;
            ret.Y = heightPos;
            ret.Z = pos.Y;
            return ret;
        }
        public static Vector2 V3toV2(Vector3 pos)
        {
            Vector2 ret = Vector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Z;
            return ret;
        }
        public static IntVector2 V3toV2(IntVector3 pos)
        {
            IntVector2 ret = IntVector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Z;
            return ret;
        }
        public static IntVector3 V2toV3(IntVector2 pos)
        {
            IntVector3 ret = IntVector3.Zero;
            ret.X = pos.X;
            ret.Z = pos.Y;
            return ret;
        }
        public static IntVector3 V2toV3(IntVector2 pos, int y)
        {
            IntVector3 ret = IntVector3.Zero;
            ret.X = pos.X;
            ret.Z = pos.Y;
            ret.Y = y;
            return ret;
        }
        public static float QuaterionToRotation1D(Quaternion rotation)
        {
            return MathHelper.TwoPi - lib.QuaternionToEuler(rotation).X;
        }
        public static void Rotation1DToQuaterion(Graphics.AbsVoxelObj mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.Identity;
            //Vector3 rot = Vector3.Zero;
            //rot.X = MathHelper.TwoPi - rotation;
            mesh.Rotation.RotateWorldX(MathHelper.TwoPi -rotation);//RotateWorld(rot);
        }
        public static void Rotation1DToQuaterion(Graphics.Mesh mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.Identity;
            Vector3 rot = Vector3.Zero;
            rot.X = MathHelper.TwoPi - rotation;
            mesh.Rotation.RotateWorld(rot);
        }


        public void NextBlock(int rep, ref IntVector3 localBlock)
        {
            IntVector3 storePos = localBlock;

            //calculate linear index
            rep += localBlock.X + (localBlock.Z << ChunkBitsWidth) + (localBlock.Y << ChunkBitsWidthSquare);

            //break up the linear index to a 3dim grindex
            localBlock.Y = rep >> ChunkBitsWidthSquare;  // /ChunkWidthSquare;
            rep -= localBlock.Y << ChunkBitsWidthSquare;
            localBlock.Z = rep >> ChunkBitsWidth;// / ChunkWidth;
            localBlock.X = rep - localBlock.Z << ChunkBitsWidth;//localBlock.Z * ChunkWidth;

            //update the world grindex
            WorldGrindex.X += localBlock.X - storePos.X;
            WorldGrindex.Z += localBlock.Z - storePos.Z;
            WorldGrindex.Y = localBlock.Y;

        }
        public void NextBlock(byte rep)
        {
            IntVector3 local = LocalBlockGrindex;
            NextBlock(rep, ref local);
        }
        public void NextBlock()
        {
            IntVector3 local = LocalBlockGrindex;
            NextBlock(1, ref local);
        }

        /// <summary>
        /// Sets the XZ block pos to be centered in the chunk, with Y at the highest block.
        /// This updates the WorldGridPos
        /// </summary>
        public void CenterAtopOfChunk()
        {
            ClearLocalPos();
            WorldGrindex.X += ChunkHalfWidth;
            WorldGrindex.Z += ChunkHalfWidth;

            SetFromGroundY(0);
        }

        public static Vector3 ChunkCenterToUnit(IntVector2 chunk, float Y)
        {
            Vector3 result = Vector3.Zero;
            result.X = (chunk.X << ChunkBitsWidth) + ChunkHalfWidth;
            result.Z = (chunk.Y << ChunkBitsWidth) + ChunkHalfWidth;
            result.Y = Y;
            return result;
        }

        public bool CorrectGridPos
        {
            get
            {
                if (WorldGrindex.X < 0 || WorldGrindex.X >= WorldSizeX)
                { return false; }
                if (WorldGrindex.Z < 0 || WorldGrindex.Z >= WorldSizeZ)
                { return false; }

                return true;
            }
        }
        public bool CorrectPos
        {
            get
            {
                if (WorldGrindex.X < 0 && WorldGrindex.X >= WorldSizeX &&
                    WorldGrindex.Z < 0 && WorldGrindex.Z >= WorldSizeZ &&
                    WorldGrindex.Y < 0 && WorldGrindex.Y >= ChunkHeight)

                { return false; }

                return true;
            }
        }

        public bool CorrectYPos
        {
            get
            {
                return WorldGrindex.Y >= 0 && WorldGrindex.Y < ChunkHeight;
            }
        }

        /// <summary>
        /// Makes sure the ypos inside byte grid range
        /// </summary>
        public void SetYLimit()
        {
            if (WorldGrindex.Y < 0) WorldGrindex.Y = 0;
            else if (WorldGrindex.Y > MaxChunkY) WorldGrindex.Y = MaxChunkXZ;
        }

        public Vector3 ToV3()
        {
            return WorldGrindex.Vec;
        }
        public Vector3 ToGroundV3()
        {
            Vector3 result = WorldGrindex.Vec;
            result.Y = LfRef.chunks.GetScreen(this).GetGroundY(this);
            return result;
        }

        public Vector3 BlockTopFaceV3()
        {
            Vector3 result = WorldGrindex.Vec;
            result.Y += 0.5f;
            return result;
        }
        public bool IsEmpty()
        {
            return WorldGrindex == IntVector3.Zero;
        }
        public override string ToString()
        {
            return "Chunk" + ChunkGrindex.ToString() + ", World" + WorldGrindex.ToString();
        }

        #region IO
        public WorldPosition(IntVector2 chunk, System.IO.BinaryReader r)
            : this(chunk)
        {
            ReadChunkObjXZ(r);
        }

        public void WritePlanePos(System.IO.BinaryWriter w)
        {
            w.Write((ushort)WorldGrindex.X);
            w.Write((ushort)WorldGrindex.Z);
        }
        public void ReadPlanePos(System.IO.BinaryReader r)
        {
            WorldGrindex.X = r.ReadUInt16();
            WorldGrindex.Z = r.ReadUInt16();
            WorldGrindex.Y = ChunkStandardHeight;
        }

        public void WriteChunkObjXZ(System.IO.BinaryWriter w)
        {
            w.Write((byte)LocalBlockX);
            w.Write((byte)LocalBlockZ);
        }
        public void ReadChunkObjXZ(System.IO.BinaryReader r)
        {
            WorldGrindex.X += r.ReadByte();
            WorldGrindex.Z += r.ReadByte();
        }

        public void WriteChunkGrindex(System.IO.BinaryWriter w)
        {
            WriteChunkGrindex_Static(ChunkGrindex, w);
        }
        public static void WriteChunkGrindex_Static(IntVector2 chunk, System.IO.BinaryWriter w)
        {
            w.Write((ushort)chunk.X);
            w.Write((ushort)chunk.Y);
        }
        public void ReadChunkGrindex(System.IO.BinaryReader r)
        {
            ChunkGrindex = ReadChunkGrindex_Static(r);
        }
        public static IntVector2 ReadChunkGrindex_Static(System.IO.BinaryReader r)
        {
            return new IntVector2(r.ReadUInt16(), r.ReadUInt16());
        }


        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((ushort)WorldGrindex.X);
            w.Write((byte)WorldGrindex.Y);
            w.Write((ushort)WorldGrindex.Z);
        }
        public void Read(System.IO.BinaryReader r)
        {
            WorldGrindex.X = r.ReadUInt16();
            WorldGrindex.Y = r.ReadByte();
            WorldGrindex.Z = r.ReadUInt16();
        }

        public static WorldPosition NetworkRead(System.IO.BinaryReader r)
        {
            WorldPosition result = WorldPosition.EmptyPos;
            result.Read(r);
            return result;
        }


        #endregion
    }
    struct WPRange
    {
        public WorldPosition Min;
        public WorldPosition Max;

        public WPRange(WorldPosition min, WorldPosition max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}
