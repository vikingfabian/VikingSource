//#define VISUAL_NODES
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Map
{
    
    class WorldChunks : VikingEngine.Voxels.IVoxelGridIOPortal
    {
        //public static WorldChunks Instance;

        public SpottedArray<Chunk> OpenChunksList;

        /// <summary>
        /// Counter for OpenChunksList, main thread use only
        /// </summary>
        public SpottedArrayCounter<Chunk> OpenChunksCounter;

        /// <summary>
        /// Counter for OpenChunksList, terrain generating thread use only
        /// </summary>
        public SpottedArrayCounter<Chunk> OpenChunksWorldGenCounter;



        static readonly VectorRect StandardBound = new VectorRect(Vector2.Zero, new Vector2(Data.Block.TerrainBlockScale));

        Chunk[,] chunksGrid;

        public WorldChunks()
        {
            LfRef.chunks = this;
            chunksGrid = new Chunk[WorldPosition.WorldChunksX, WorldPosition.WorldChunksY];

            OpenChunksList = new SpottedArray<Chunk>(lib.TileRadiusToArea(World.OpenScreenRadius_TopographData));
            OpenChunksCounter = new SpottedArrayCounter<Chunk>(OpenChunksList);
            OpenChunksWorldGenCounter = new SpottedArrayCounter<Chunk>(OpenChunksList);
        }


        #region GETSET
        public Chunk GetScreen(IntVector2 pos)
        {
            Chunk s = chunksGrid[pos.X, pos.Y];
            if (s == null)
            {
                s = new Chunk(pos);
                OpenChunksList.Add(s);
                chunksGrid[pos.X, pos.Y] = s;
            }
            return s;
        }
        public Chunk GetScreen(WorldPosition pos)
        {
            return GetScreen(pos.ChunkGrindex);
        }

        public Chunk GetScreenUnsafe(IntVector2 pos)
        {
            return chunksGrid[pos.X, pos.Y];
        }
        public Chunk GetScreenUnsafe(WorldPosition wp)
        {
            return chunksGrid[wp.WorldGrindex.X >> Map.WorldPosition.ChunkBitsWidth, wp.WorldGrindex.Z >> Map.WorldPosition.ChunkBitsWidth];
        }

        public byte Get(Vector3 pos)
        {
            return Get(new WorldPosition(pos));
        }

        public byte Get(WorldPosition wp)
        {
            try
            {
                return GetScreen(wp.ChunkGrindex).Get(wp);
            }
            catch (Exception e)
            {
                Debug.LogError( "Get block pos:" + wp.ToString() + ", " + e.Message);
                return 0;
            }
        }

        public void Set(WorldPosition wp, byte value)
        {
            GetScreen(wp).Set(wp, value);
            //screens[wp.WorldGrindex.X >> Map.WorldPosition.ChunkBitsWidth, wp.WorldGrindex.Z >> Map.WorldPosition.ChunkBitsWidth].Set(wp, value);
            
        }

        public void Set(IntVector3 position, byte value)
        {
            Chunk chunk = chunksGrid[position.X >> Map.WorldPosition.ChunkBitsWidth, position.Z >> Map.WorldPosition.ChunkBitsWidth];
            if (chunk == null)
            {
                return;
            }
            chunk.Set(position.X & Map.WorldPosition.ChunkBitsZero, position.Y, position.Z & Map.WorldPosition.ChunkBitsZero, value);
        }
        #endregion


        /// <summary>
        /// Check the chunk and the surrounding ones if they have the data grid loaded
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public bool ChunksDataLoaded(IntVector2 center)
        {
            ForXYLoop loop = new ForXYLoop(IntVector2.NegativeOne, IntVector2.One);

            while (!loop.Done)
            {
                IntVector2 pos = center + loop.Next_Old();
                Chunk chunk = chunksGrid[pos.X, pos.Y];
                if (chunk == null || !chunk.DataGridLoadingComplete)
                {
                    return false;
                }
            }
            return true;
        }

        public void RemoveChunk(IntVector2 index)
        {
            Chunk c = chunksGrid[index.X, index.Y];
            if (c != null)
            {
                OpenChunksList.Remove(c);
                chunksGrid[index.X, index.Y] = null;
                c.OnDestroy();
            }
        }
        
        

        static readonly Range screenRangeX = new Range(0, WorldPosition.WorldChunksX - 1);
        static readonly Range screenRangeY = new Range(0, WorldPosition.WorldChunksY - 1);
        public Chunk GetScreenSafe(IntVector2 pos)
        {
            if (screenRangeX.IsWithinRange(pos.X) &&
                screenRangeY.IsWithinRange(pos.Y))
                return chunksGrid[pos.X, pos.Y];
            else
                return null;
        }

        public bool ScreenDataGridLoadingComplete(IntVector2 pos)
        {
            Chunk s  = chunksGrid[pos.X, pos.Y];
            if (s == null) 
                return false;
            return s.DataGridLoadingComplete;
        }

        
       
        public int GetHighestYpos(WorldPosition pos)
        {
            if (!pos.CorrectGridPos)
                return 0;
            return GetScreen(pos).GetHighestYpos(pos);
            // return 0;
        }
        public byte GetSafe(WorldPosition pos)
        {
            if (!pos.CorrectPos)
                return 0;
            return Get(pos);
        }

        
        

        public byte GetVoxelFromPortal(WorldPosition wp)
        {
            //wp.UpdateChunkPos();
            return Get(wp);
        }
        public void SetVoxelToPortal(WorldPosition wp, byte value)
        {
            //wp.UpdateChunkPos();
            SetIfOpen(wp, value);
        }

        public void SetIfOpen(WorldPosition pos, byte value)
        {

            Chunk screen = GetScreenUnsafe(pos);//screens[pos.ChunkGrindex.X, pos.ChunkGrindex.Y];
            if (screen != null)
                screen.Set(pos, value);
        }

        /// <summary>
        /// The material in the radius will transform to another
        /// </summary>
        public void MaterialDamage(Vector3 position, float radius, byte toMaterial)
        {
            float length;
            //WorldPosition WorldPosition = Map.WorldPosition.EmptyPos;
            Map.WorldPosition min = new Map.WorldPosition(position - lib.V3(radius));
            Map.WorldPosition max = new Map.WorldPosition(position + lib.V3(radius));

            min.SetYLimit();
            max.SetYLimit();

            //min.Y = Bound.Min(min.WorldGrindex.Y, 0);
            //max.Y = lib.SetMaxVal(max.WorldGrindex.Y, Map.WorldPosition.ChunkHeight - 1);

            WorldPosition pos = Map.WorldPosition.EmptyPos;

            for (pos.WorldGrindex.Z = min.WorldGrindex.Z; pos.WorldGrindex.Z <= max.WorldGrindex.Z; ++pos.WorldGrindex.Z)
            {
                for (pos.WorldGrindex.X = min.WorldGrindex.X; pos.WorldGrindex.X <= max.WorldGrindex.X; ++pos.WorldGrindex.X)
                {
                    if (!GetScreen(pos).WriteProtected)
                    {
                        for (pos.WorldGrindex.Y = min.WorldGrindex.Y; pos.WorldGrindex.Y <= max.WorldGrindex.Y; ++pos.WorldGrindex.Y)
                        {
                            length = (pos.WorldGrindex.Vec - position).Length();

                            if (length <= radius && Ref.rnd.RandomChance(1.2f - (length / radius)))
                            {
                                //WorldPosition.WorldGrindex = pos;
                                ////WorldPosition.UpdateChunkPos();
                                if (Get(pos) > 0)
                                    Set(pos, toMaterial);
                            }
                        }
                    }
                }
            }

            Map.World.ReloadChunkMesh(min, max);
        }

        public void TerrainDestruction(Vector3 position, float radius)
        {
           // WorldPosition WorldPosition = Map.WorldPosition.EmptyPos;
            Map.WorldPosition min = new Map.WorldPosition(position - lib.V3(radius));
            Map.WorldPosition max = new Map.WorldPosition(position + lib.V3(radius));

            min.SetYLimit();
            max.SetYLimit();

            WorldPosition pos = Map.WorldPosition.EmptyPos;

            for (pos.WorldGrindex.Z = min.WorldGrindex.Z; pos.WorldGrindex.Z <= max.WorldGrindex.Z; ++pos.WorldGrindex.Z)
            {
                for (pos.WorldGrindex.X = min.WorldGrindex.X; pos.WorldGrindex.X <= max.WorldGrindex.X; ++pos.WorldGrindex.X)
                {
                    if (!GetScreen(pos).WriteProtected)
                    {
                        for (pos.WorldGrindex.Y = min.WorldGrindex.Y; pos.WorldGrindex.Y <= max.WorldGrindex.Y; ++pos.WorldGrindex.Y)
                        {
                            if ((pos.WorldGrindex.Vec - position).Length() <= radius)
                            {
                                SetIfOpen(pos, 0);
                            }
                        }
                    }
                }
            }
            
            //min.Y = Bound.Min(min.WorldGrindex.Y, 0);
            //max.Y = lib.SetMaxVal(max.WorldGrindex.Y, Map.WorldPosition.ChunkHeight - 1);

            //IntVector3 pos = IntVector3.Zero;
            //for (pos.Y = min.WorldGrindex.Y; pos.Y <= max.WorldGrindex.Y; pos.Y++)
            //{
            //    for (pos.Z = min.WorldGrindex.Z; pos.Z <= max.WorldGrindex.Z; pos.Z++)
            //    {
            //        for (pos.X = min.WorldGrindex.X; pos.X <= max.WorldGrindex.X; pos.X++)
            //        {
            //            if ((pos.Vec - position).Length() <= radius)
            //            {
            //                WorldPosition.WorldGrindex = pos;
            //                //WorldPosition.UpdateChunkPos();
            //                SetIfOpen(WorldPosition, 0);
            //            }
            //        }
            //    }
            //}

            //min.UpdateChunkPos();
            //max.UpdateChunkPos();
            //IntVector2 chunk = IntVector2.Zero;
            //for (chunk.Y = min.ChunkGrindex.Y; chunk.Y <= max.ChunkGrindex.Y; chunk.Y++)
            //{
            //    for (chunk.X = min.ChunkGrindex.X; chunk.X <= max.ChunkGrindex.X; chunk.X++)
            //    {
            //        Map.World.ReloadChunkMesh(chunk);
            //    }
            //}
            Map.World.ReloadChunkMesh(min, max);
        }


        public List<VectorRect> CollectBounds(WorldPosition wp)
        {

            List<VectorRect> result = new List<VectorRect>();
            const int CheckRadius = 2;
            for (int z = -CheckRadius; z <= CheckRadius; z++)
            {
                for (int x = -CheckRadius; x <= CheckRadius; x++)
                {
                    WorldPosition checkPos = wp;

                    ++checkPos.WorldGrindex.X;
                    ++checkPos.WorldGrindex.Z;
                    if (checkPos.CorrectPos)
                    {
                        Chunk c = chunksGrid[checkPos.ChunkGrindex.X, checkPos.ChunkGrindex.Y];
                        if (c != null  && c.Check2DCollision(checkPos))
                        {
                            //add collision
                            VectorRect bound = StandardBound;
                            //checkPos.UpdateWorldGridPos();
                            bound.X = checkPos.WorldGrindex.X * Data.Block.TerrainBlockScale;
                            bound.Y = checkPos.WorldGrindex.Z * Data.Block.TerrainBlockScale;
                            result.Add(bound);
                        }
                    }
                }
            }

            return result;
        }
       
        public Voxels.VoxelObjListData CollectVoxels(WorldPosition start, IntVector3 size)
        {
            size.Sub(IntVector3.One);

            Voxels.VoxelObjListData result = new Voxels.VoxelObjListData(new List<Voxels.Voxel>());

            WorldPosition end = start;
            end.WorldGrindex.Add(size);
            //end.UpdateChunkPos();

            IntVector2 screen = IntVector2.Zero;
            IntVector3 posAdd = IntVector3.Zero;
            posAdd.Z = -start.LocalBlockZ;
            for (screen.Y = start.ChunkGrindex.Y; screen.Y <= end.ChunkGrindex.Y; screen.Y++)
            {
                posAdd.X = -start.LocalBlockX;
                for (screen.X = start.ChunkGrindex.X; screen.X <= end.ChunkGrindex.X; screen.X++)
                {
                    result.Voxels.AddRange(GetScreen(screen).CollectVoxels(start, end, posAdd));
                    posAdd.X += WorldPosition.ChunkWidth;
                }
                posAdd.Z += WorldPosition.ChunkWidth;
            }
            return result;
        }
        public void ClearArea(WorldPosition start, IntVector3 size)
        {
            size.Sub(IntVector3.One);

            WorldPosition end = start;
            end.WorldGrindex.Add(size);
            //end.UpdateChunkPos();

            IntVector2 screen = IntVector2.Zero;
            //IntVector3 posAdd = IntVector3.Zero;
            //posAdd.Z = -start.BlockPos.Z;
            for (screen.Y = start.ChunkGrindex.Y; screen.Y <= end.ChunkGrindex.Y; screen.Y++)
            {
                //posAdd.X = -start.BlockPos.X;
                for (screen.X = start.ChunkGrindex.X; screen.X <= end.ChunkGrindex.X; screen.X++)
                {
                    GetScreen(screen).RemoveVoxels(start, end);
                    //result.Voxels.AddRange(GetScreen(screen).CollectVoxels(start, end, posAdd));
                    //posAdd.X += WorldPosition.ChunkWith;
                }
                //posAdd.Z += WorldPosition.ChunkWith;
            }
        }
        public void OpenChunkArea(Rectangle2 chunkArea, bool preGen)
        {
            IntVector2 pos = IntVector2.Zero;
            const int OpenRadius = 2;

            for (ScreenOpenStatus status = (ScreenOpenStatus)0; status <= ScreenOpenStatus.HeightMapToDataGrid; status++)
            {
                IntVector2 min = chunkArea.Position;// -OpenRadius;
                IntVector2 max = chunkArea.BottomRight;// +OpenRadius;
                if (status < ScreenOpenStatus.HeightMapToDataGrid)
                {
                    min -= OpenRadius;
                    max += OpenRadius;
                }
                for (pos.X = min.X; pos.X < max.X; pos.X++)
                {
                    for (pos.Y = min.Y; pos.Y < max.Y; pos.Y++)
                    {
                        LfRef.chunks.GetScreen(pos).GeneratePart(status, preGen);
                    }
                }
            }
        }

        public WalkingPath PathFindRandomDir(Vector3 center)
        {
            return PathFinding(center, center + Map.WorldPosition.V2toV3(Rotation1D.Random.Direction(64)));
        }

        public WalkingPath PathFinding(Vector3 center, Vector3 goal)
        {
            /*
            * Path finding algorithm
            * ruta in världen, kanske var fjärde ruta
            * 1. Kolla 8riktingar
            * 2. Ge värde till rutorna
            * G - kostnad att gå dit, 10 rakt, 14 diagonalt
            * H - Avståndet till målet X + Y
            * F - totalt värde G+H
            * Parent - håll reda på parent ruta
            * -värdet ska vara oändligt om det finns hinder
            * -en liten bonus (2poäng) om man behåller riktingen, checka mot parentDir
            * 3.Varje kollad center ruta ska till en sluten lista
            * 4.Varje ny ruta ska till en öppen lista
            */

            //num blocks between each node
            const int MaxCheckLength = Map.WorldPosition.ChunkWidth * 2 / PathNode.CheckStep;
            const int GridRadius = 4;

            const int GridSize = (MaxCheckLength + GridRadius) * PublicConstants.Twice;
            PathNode[,] nodes = new PathNode[GridSize, GridSize];

            WorldPosition centerWP = new WorldPosition(center);
            IntVector2 centerGridPos = new IntVector2(GridSize / 2);
            Vector2 diff = WorldPosition.V3toV2(goal - center);

           // System.Diagnostics.Debug.WriteLine("##PathFinding## diff:" + diff.ToString());
            float length = diff.Length();
            if (length > MaxCheckLength)
            {
                diff.Normalize();
                diff *= MaxCheckLength;
            }
            else if (length < 4)
            {
                return new WalkingPath(new List<Vector2> { Map.WorldPosition.V3toV2(goal) });
            }
            IntVector2 goalGridPos = centerGridPos + new IntVector2((int)(diff.X / PathNode.CheckStep), (int)(diff.Y / PathNode.CheckStep));
            List<PathNode> open = new List<PathNode>();
            PathNode startNode = new PathNode(centerGridPos, Facing8Dir.NO_DIR, (int)center.Y);
            startNode.Closed = true;
            nodes[centerGridPos.X, centerGridPos.Y] = startNode;

            WorldPosition gridWP = centerWP;
            //grid//wp.UpdateWorldGridPos();
            gridWP.WorldGrindex.X -= centerGridPos.X * PathNode.CheckStep;
            gridWP.WorldGrindex.Z -= centerGridPos.Y * PathNode.CheckStep;
            //grid//wp.UpdateChunkPos();


            PathNode currentNode = startNode;
            Rectangle2 area = new Rectangle2(IntVector2.Zero, new IntVector2(GridSize - 1));
            //#if WINDOWS
            int numLoops = 0;
            //#endif
            while (true)
            {
                //System.Diagnostics.Debug.WriteLine("Check node sourrounds:" + currentNode.Position.ToString());
                for (Facing8Dir dir = (Facing8Dir)0; dir < Facing8Dir.NUM; dir++)
                {
                    IntVector2 pos = IntVector2.FromFacing8Dir(dir) + currentNode.Position;
                    if (area.IntersectPoint(pos) && nodes[pos.X, pos.Y] == null)
                    {
                        //add a node to open list
                        PathNode node = new PathNode(pos, dir, currentNode, goalGridPos, gridWP);
                        if (!node.Closed)
                            open.Add(node);
                        nodes[pos.X, pos.Y] = node;
                    }
                }

                LowestValue lowest = new LowestValue(false);
                for (int i = 0; i < open.Count; i++)
                {
                    lowest.Next(open[i].Value, i);
                }


                if (open.Count > 1)
                {
                    currentNode = open[lowest.LowestMemberIndex];
                    open.RemoveAt(lowest.LowestMemberIndex);
                }
                currentNode.Closed = true;

                if (currentNode.Position == goalGridPos)
                {
                    break;
                }

                //#if WINDOWS
                numLoops++;
                if (numLoops > 400)
                {
                    break;
                }
                //if (numLoops > 10000)
                //    throw new Debug.EndlessLoopException("");
                //#endif
            }

            List<Vector2> result = new List<Vector2>();


            while (currentNode != startNode)
            {
                result.Add(new Vector2(currentNode.Position.X * PathNode.CheckStep + gridWP.WorldGrindex.X, currentNode.Position.Y * PathNode.CheckStep + gridWP.WorldGrindex.Z));
                IntVector2 pos = currentNode.Position - IntVector2.FromFacing8Dir(currentNode.parentDir);
                currentNode = nodes[pos.X, pos.Y];

#if WINDOWS
                numLoops++;
                if (numLoops > 10000)
                    throw new Debug.EndlessLoopException("");
#endif
            }

//#if WINDOWS
//            System.Diagnostics.Debug.WriteLine("#Node result");
//            for (int i = result.Count - 1; i >= 0; i--)
//            {
//                System.Diagnostics.Debug.WriteLine(result[i].ToString());
//            }
//#endif

            return new WalkingPath(result);
        }
    }

    class WalkingPath
    {
#if VISUAL_NODES
        List<Graphics.Mesh> nodeImages;
#endif
        const int IgnoreDirChangeTimes = 10;
        int numIngoreDirChange = 0;

        bool firstTimeUse = true;

        int currentNode;
        List<Vector2> nodes;
        public WalkingPath(List<Vector2> nodes)
        {
            this.nodes = nodes;
            currentNode = nodes.Count - 1;

#if VISUAL_NODES
            nodeImages = new List<Graphics.Mesh>();
            foreach (Vector2 n in nodes)
            {
                Vector3 pos = WorldPosition.V2toV3(n);
                WorldPosition wp = new WorldPosition(pos);
                wp.SetFromGroundY(2);
                nodeImages.Add(new Graphics.Mesh(LoadedMesh.cube_repeating, wp.ToWorldPos(), 
                    new Graphics.TextureEffect( Graphics.TextureEffectType.Flat, SpriteName.ControllerB), 0.4f));
            }

#endif
        }

        public Vector2 WalkTowardsNode(Vector3 currentPos, Rotation1D currentDir, float turnSpeedAndTime, float walkingSpeed)
        {
            if (currentNode < 0 || nodes.Count == 0)
                return Vector2.Zero;

            Vector2 planePos = WorldPosition.V3toV2(currentPos);
            if (firstTimeUse)
            {
                firstTimeUse = false;
                float closest = (nodes[0] - planePos).Length();
                for (int i = nodes.Count - 2; i >= 0; i--)
                {
                    float l = (nodes[i] - planePos).Length();
                    if (l < closest)
                    {
                        closest = l;
                        currentNode = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }



            Vector2 diff = nodes[currentNode] - planePos;
            float length = diff.Length();
            //check next node to make sure it isnt closer


            const float MinLenght = 1f;
            if (length < MinLenght)
            {
                currentNode--;
            }

            diff.Normalize();
            Rotation1D dir = Rotation1D.FromDirection(diff);
            float dirDiff = dir.AngleDifference(currentDir);
            if (dirDiff > turnSpeedAndTime)
            {
                numIngoreDirChange++;
                if (numIngoreDirChange >= IgnoreDirChangeTimes)
                {
                    currentDir.Add(dirDiff);

                }
                //else
                //{

                //}
                return currentDir.Direction(walkingSpeed);
            }
            else
            {
                numIngoreDirChange = 0;
            }

            return diff * walkingSpeed;
        }
        //public void NextNode()
        //{
        //    currentNode-= 4;
        //}

        public void DeleteMe()
        {
#if VISUAL_NODES
            foreach (Graphics.Mesh m in nodeImages)
            {
                m.DeleteMe();
            }
#endif
        }
    }

    class PathNode
    {
        public const int CheckStep = 2;
        public Facing8Dir parentDir; //the direction the parent node walked in
        const int MoveCostStraight = 10;
        const int MoveCostDiagonal = 14;
        public int CurrentY;
        public int Value;
        public int moveCost;
        public bool Closed;
        public IntVector2 Position;

        public PathNode(IntVector2 pos, Facing8Dir parentDir, int CurrentY)
        {
            this.Position = pos;
            this.parentDir = parentDir;
            this.CurrentY = CurrentY;
        }
        public PathNode(IntVector2 pos, Facing8Dir parentDir, PathNode parent, IntVector2 goalPos, WorldPosition gridWP)
        {
            this.Position = pos;
            this.parentDir = parentDir;

            //check terrain
            gridWP.WorldGrindex.X += pos.X * CheckStep;
            gridWP.WorldGrindex.Z += pos.Y * CheckStep;
            //grid//wp.UpdateChunkPos();
            CurrentY = parent.CurrentY;
            if (LfRef.chunks.GetScreen(gridWP).PathFindingPass(gridWP, ref CurrentY))
            {

                moveCost = MoveCostStraight;
                if (parent.parentDir == Facing8Dir.NE || parent.parentDir == Facing8Dir.NW || parent.parentDir == Facing8Dir.SE || parent.parentDir == Facing8Dir.SW)
                    moveCost = MoveCostDiagonal;

                moveCost += parent.moveCost;
                if (parentDir == parent.parentDir)
                    moveCost -= 2;

                Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * MoveCostStraight;
            }
            else
            {
                Value = int.MaxValue;
                Closed = true;
            }
        }
    }
}
