using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Map
{
    class LowResChunkCollection
    {
        int changesMade = 0;
        int lastMeshUpdate = 0;

        SpottedArray<LowResChunk> chunks = new SpottedArray<LowResChunk>(512);
        SpottedArrayCounter<LowResChunk> meshGenCounter1, meshGenCounter2, updateCounter;

        ushort[,][, ,] preparedGrids = new ushort[3, 3][, ,];

        public LowResChunkCollection()
        {
            meshGenCounter1 = new SpottedArrayCounter<LowResChunk>(chunks);
            meshGenCounter2 = new SpottedArrayCounter<LowResChunk>(chunks);
            updateCounter = new SpottedArrayCounter<LowResChunk>(chunks);
        }

        public void update(World world)
        {
            bool bChangesMade = false;

            updateCounter.Reset();
            while (updateCounter.Next())
            {
                int dist = world.smallestHeroDistanceToChunk(updateCounter.sel.index);
                if (dist > World.LowResChunksRadius)
                {
                    updateCounter.RemoveAtCurrent();
                    bChangesMade = true;
                }
            }

            if (bChangesMade)
            {
                changesMade++;
            }
        }

        public void add(Chunk chunk)
        {
            var counter = meshGenCounter1.IClone();
            while (counter.Next())
            {
                if (counter.GetSelection.index == chunk.Index)
                {
                    counter.RemoveAtCurrent();
                    break;
                }
            }

            chunk.hasLowResChunk = true;
            chunks.Add(new LowResChunk(chunk));
            changesMade++;
        }

        public void generateMesh(VikingEngine.LootFest.Map.HDvoxel.MeshBuilder meshbuilder)
        {
            if (changesMade > lastMeshUpdate && chunks.Count > 1)
            {
                lastMeshUpdate = changesMade;
                meshbuilder.resetCounting();
                meshGenCounter1.Reset();

                while (meshGenCounter1.Next())
                {
                    if (renderChunk(meshGenCounter1.sel))
                    {
                        IntVector2 center = meshGenCounter1.sel.index;
                        int collected = 0;
                        preparedGrids[1, 1] = meshGenCounter1.sel.grid;

                        meshGenCounter2.Reset();
                        while (meshGenCounter2.Next() && collected < 8)
                        {
                            if (center.SideLength(meshGenCounter2.sel.index) == 1)
                            {
                                IntVector2 preparePos = (meshGenCounter2.sel.index - center) + IntVector2.One;
                                preparedGrids[preparePos.X, preparePos.Y] = meshGenCounter2.sel.grid;
                                collected++;
                            }
                        }

                        meshbuilder.addLowResChunk(preparedGrids, center);
                    }
                }

                //End the loop and gen the mesh
                meshbuilder.endLowResMesh();
            }
        }

        bool renderChunk(LowResChunk chunk)
        {
            if (lowResChunkIsVisual(chunk))
            {
                chunk.hideDelay = 0;
                return true;
            }
            else
            {
                return ++chunk.hideDelay < 4;
            }
        }

        bool lowResChunkIsVisual(LowResChunk chunk)
        {
            ForXYLoop loop = new ForXYLoop(chunk.index - 1, chunk.index + 1);
            while (loop.Next())
            {
               var c = LfRef.chunks.GetScreenUnsafe(loop.Position);
               if (c == null || c.removeDelay > 0 || c.Mesh == null)
               {
                   return true;
               }
            }

            return false;
        }

        //LowResChunk get(IntVector2 index)
        //{
        //    var counter2 = counter.Clone();
        //     while (cou
        //}
    }

    class LowResChunk
    {
        public const int BlockSteps = 8;
        public const int Width = Map.WorldPosition.ChunkWidth / BlockSteps;
        public const int Height = Map.WorldPosition.ChunkHeight / BlockSteps;

        public IntVector2 index;
        public ushort[, ,] grid;
        public int hideDelay = 0;

        public LowResChunk(Chunk chunk)
        {
            const int SurfaceCheckSteps = BlockSteps - 1;

            IntVector3 wp = IntVector3.Zero;
            grid = new ushort[Width, Height, Width];
            index = chunk.Index;

            for (int z = 0; z < Width; ++z)
            {
                wp.Z = z * BlockSteps;
                for (int x = 0; x < Width; ++x)
                {
                    wp.X = x * BlockSteps;
                
                    for (int y = 0; y < Width; ++y)
                    {
                        wp.Y = y * BlockSteps;

                        ushort value = chunk.DataGrid[wp.X, wp.Y, wp.Z];

                        if (value != 0)
                        {
                            for (int i = 0; i < SurfaceCheckSteps; ++i)
                            {
                                ++wp.Y;
                                ushort aboveBlock = chunk.DataGrid[wp.X, wp.Y, wp.Z];
                                
                                if (aboveBlock == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    value = aboveBlock;
                                }
                            }
                        }

                        grid[x, y, z] = value;
                    }
                }
            }
        }
    }
}
