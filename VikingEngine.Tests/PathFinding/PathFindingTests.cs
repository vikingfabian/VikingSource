using System;
using System.Collections.Generic;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Tests;
using VikingEngine.Tests.Pathfinding.Legacy;
using Xunit;

namespace VikingEngine.Tests.Pathfinding
{
    public class PathFindingTests
    {
        [Fact]
        public void FindPath_StraightLine_ReachesGoal()
        {
            TestWorldHelper.SetupFlatWorld(64, 64);
            var pf = new PathFinding();

            var start = new IntVector2(5, 5);
            var goal = new IntVector2(5, 15);

            var path = pf.FindPath(-1, start, 0, goal, false);

            Assert.NotNull(path);
            Assert.NotEmpty(path.nodes);
            Assert.Equal(goal, path.nodes[0].position);
        }

        [Fact]
        public void FindPath_DiagonalPath_ReachesGoal()
        {
            TestWorldHelper.SetupFlatWorld(64, 64);
            var pf = new PathFinding();

            var start = new IntVector2(0, 0);
            var goal = new IntVector2(20, 20);

            var path = pf.FindPath(-1, start, 0, goal, false);

            Assert.NotNull(path);
            Assert.NotEmpty(path.nodes);
            Assert.Equal(goal, path.nodes[0].position);
        }

        [Fact]
        public void FindPath_AroundWater_AvoidsWater()
        {
            // A 7-tile water barrier from y=12 to y=18 at x=10.
            // Detouring around y=11 costs ~50, which is cheaper than the ~320 water embark/disembark penalty.
            var water = new HashSet<IntVector2>();
            for (int y = 12; y <= 18; y++)
            {
                water.Add(new IntVector2(10, y));
            }

            TestWorldHelper.SetupFlatWorld(64, 64, Height.MinLandHeight, water);
            var pf = new PathFinding();

            var start = new IntVector2(5, 15);
            var goal = new IntVector2(15, 15);

            var path = pf.FindPath(-1, start, 0, goal, false);

            Assert.NotNull(path);
            Assert.NotEmpty(path.nodes);
            Assert.Equal(goal, path.nodes[0].position);

            foreach (var node in path.nodes)
            {
                Assert.False(water.Contains(node.position), $"Path stepped on water at {node.position}");
            }
        }

        [Fact]
        public void FindPath_WaterEmbarkation_ShipFlagCorrect()
        {
            var water = new HashSet<IntVector2>();
            for (int x = 20; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    water.Add(new IntVector2(x, y));
                }
            }

            TestWorldHelper.SetupFlatWorld(64, 64, Height.MinLandHeight, water);
            var pf = new PathFinding();

            var start = new IntVector2(5, 10);
            var goal = new IntVector2(30, 10);

            var path = pf.FindPath(-1, start, 0, goal, false);

            Assert.NotNull(path);
            Assert.NotEmpty(path.nodes);
            Assert.Equal(goal, path.nodes[0].position);
            Assert.True(path.nodes[0].ship, "Goal node on water should have ship = true");
        }

        [Fact]
        public void FindPath_SameStartAndGoal_ReturnsEmptyPath()
        {
            TestWorldHelper.SetupFlatWorld(64, 64);
            var pf = new PathFinding();

            var pos = new IntVector2(10, 10);
            var path = pf.FindPath(-1, pos, 0, pos, false);

            Assert.NotNull(path);
            Assert.Empty(path.nodes);
        }

        [Fact]
        public void FindPath_PathAdjacency_AllStepsAdjacent()
        {
            TestWorldHelper.SetupFlatWorld(128, 128);
            var pf = new PathFinding();

            var start = new IntVector2(10, 10);
            var goal = new IntVector2(60, 80);

            var path = pf.FindPath(-1, start, 0, goal, false);

            Assert.NotNull(path);
            Assert.NotEmpty(path.nodes);

            // path.nodes is in reverse order (goal first, start's neighbor last)
            IntVector2 current = goal;
            for (int i = 0; i < path.nodes.Count; i++)
            {
                var next = path.nodes[i].position;
                var diff = next - current;
                Assert.True(Math.Abs(diff.X) <= 1 && Math.Abs(diff.Y) <= 1,
                    $"Discontinuous step at index {i}: from {current} to {next}");
                current = next;
            }

            var toStart = current - start;
            Assert.True(Math.Abs(toStart.X) <= 1 && Math.Abs(toStart.Y) <= 1,
                $"Last path node {current} not adjacent to start {start}");
        }

        [Fact]
        public void Recycle_MultipleRuns_NoStaleData()
        {
            TestWorldHelper.SetupFlatWorld(128, 128);
            var pf = new PathFinding();

            var path1 = pf.FindPath(-1, new IntVector2(10, 10), 0, new IntVector2(30, 30), false);
            Assert.NotEmpty(path1.nodes);
            Assert.Equal(new IntVector2(30, 30), path1.nodes[0].position);

            pf.recycle();

            var path2 = pf.FindPath(-1, new IntVector2(50, 50), 0, new IntVector2(80, 80), false);
            Assert.NotEmpty(path2.nodes);
            Assert.Equal(new IntVector2(80, 80), path2.nodes[0].position);

            foreach (var node in path2.nodes)
            {
                Assert.True(node.position.X >= 49 && node.position.Y >= 49,
                    $"Path 2 contained unexpected node from Path 1: {node.position}");
            }
        }

        [Fact]
        public void FindPath_NodeCount_RefactoredNotWorseThanLegacy()
        {
            TestWorldHelper.SetupFlatWorld(200, 176);
            var pfRefactored = new PathFinding();
            var pfLegacy = new LegacyPathFinding();

            var start = new IntVector2(10, 10);
            var goal = new IntVector2(150, 150);

            var pathLegacy = pfLegacy.FindPath(-1, start, 0, goal, false);
            var pathRefactored = pfRefactored.FindPath(-1, start, 0, goal, false);

            Assert.NotEmpty(pathLegacy.nodes);
            Assert.NotEmpty(pathRefactored.nodes);

            // Refactored should be within +2 tolerance of legacy path length
            Assert.True(pathRefactored.nodes.Count <= pathLegacy.nodes.Count + 2,
                $"Refactored length ({pathRefactored.nodes.Count}) significantly worse than legacy ({pathLegacy.nodes.Count})");
        }

        [Fact]
        public void FindPath_NodeCount_ShortPath_Reasonable()
        {
            TestWorldHelper.SetupFlatWorld(64, 64);
            var pf = new PathFinding();

            var path = pf.FindPath(-1, new IntVector2(5, 5), 0, new IntVector2(5, 15), false);
            Assert.NotEmpty(path.nodes);
            Assert.InRange(path.nodes.Count, 9, 12);
        }

        [Fact]
        public void FindPath_ReconstructedCost_RefactoredNotWorseThanLegacy()
        {
            TestWorldHelper.SetupFlatWorld(200, 176);

            // Add mountain terrain in a strip
            for (int y = 30; y < 70; y++)
            {
                for (int x = 30; x < 70; x++)
                {
                    var t = new Tile();
                    t.heightLevel = Height.MountainHeightStart;
                    DssRef.world.tileGrid.Set(new IntVector2(x, y), t);
                }
            }

            var pfRefactored = new PathFinding();
            var pfLegacy = new LegacyPathFinding();

            var start = new IntVector2(10, 10);
            var goal = new IntVector2(90, 90);

            var pathLegacy = pfLegacy.FindPath(-1, start, 0, goal, false);
            var pathRefactored = pfRefactored.FindPath(-1, start, 0, goal, false);

            float ReconstructCost(WalkingPath path)
            {
                float cost = 0;
                for (int i = 0; i < path.nodes.Count; i++)
                {
                    var tile = DssRef.world.tileGrid.Get(path.nodes[i].position);
                    cost += tile.TroupWalkingDistance(path.nodes[i].ship);
                }
                return cost;
            }

            float costLegacy = ReconstructCost(pathLegacy);
            float costRefactored = ReconstructCost(pathRefactored);

            Assert.True(costRefactored <= costLegacy * 1.05f,
                $"Refactored cost ({costRefactored}) exceeded legacy ({costLegacy}) by more than 5%");
        }

        [Fact]
        public void FindPath_BatchComparison_HighConcordanceWithLegacy()
        {
            // Run on Epic map size (1184x1024) — the largest standard map size in DSS2
            const int MapWidth = WorldData.EpicMapWidth;
            const int MapHeight = WorldData.EpicMapHeigth;
            const int TotalQueries = 100;

            // Generate a realistic procedural terrain with lakes, hills, and mountains using a fixed seed
            var rng = new Random(42);
            var water = new HashSet<IntVector2>();

            // Create procedural water bodies across the Epic map
            for (int i = 0; i < 20; i++)
            {
                int lakeCenterX = rng.Next(50, MapWidth - 50);
                int lakeCenterY = rng.Next(50, MapHeight - 50);
                int radius = rng.Next(10, 35);

                for (int y = lakeCenterY - radius; y <= lakeCenterY + radius; y++)
                {
                    for (int x = lakeCenterX - radius; x <= lakeCenterX + radius; x++)
                    {
                        if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                        {
                            var pt = new IntVector2(x, y);
                            if ((pt - new IntVector2(lakeCenterX, lakeCenterY)).Length() <= radius)
                            {
                                water.Add(pt);
                            }
                        }
                    }
                }
            }

            TestWorldHelper.SetupFlatWorld(MapWidth, MapHeight, Height.MinLandHeight, water);

            // Add scattered hill and mountain clusters across the Epic map
            for (int i = 0; i < 30; i++)
            {
                int hillCenterX = rng.Next(30, MapWidth - 30);
                int hillCenterY = rng.Next(30, MapHeight - 30);
                int radius = rng.Next(10, 30);

                for (int y = hillCenterY - radius; y <= hillCenterY + radius; y++)
                {
                    for (int x = hillCenterX - radius; x <= hillCenterX + radius; x++)
                    {
                        if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                        {
                            var pt = new IntVector2(x, y);
                            if (!water.Contains(pt))
                            {
                                var tile = new Tile();
                                tile.heightLevel = (byte)((i % 3 == 0) ? Height.MountainHeightStart : Height.MinLandHeight + 1);
                                DssRef.world.tileGrid.Set(pt, tile);
                            }
                        }
                    }
                }
            }

            var pfLegacy = new LegacyPathFinding();
            var pfRefactored = new PathFinding();

            int equalOrBetterCostCount = 0;
            int equalLengthCount = 0;
            int exactNodeMatchCount = 0;
            int totalValid = 0;

            float ReconstructCost(WalkingPath path)
            {
                float cost = 0;
                for (int i = 0; i < path.nodes.Count; i++)
                {
                    var tile = DssRef.world.tileGrid.Get(path.nodes[i].position);
                    cost += tile.TroupWalkingDistance(path.nodes[i].ship);
                }
                return cost;
            }

            for (int q = 0; q < TotalQueries; q++)
            {
                var start = new IntVector2(rng.Next(5, MapWidth - 5), rng.Next(5, MapHeight - 5));
                var goal = new IntVector2(rng.Next(5, MapWidth - 5), rng.Next(5, MapHeight - 5));

                if (start == goal)
                {
                    continue;
                }

                var legacyPath = pfLegacy.FindPath(-1, start, 0, goal, false);
                pfLegacy.recycle();

                var refactoredPath = pfRefactored.FindPath(-1, start, 0, goal, false);
                pfRefactored.recycle();

                totalValid++;

                if (legacyPath.nodes.Count == 0 && refactoredPath.nodes.Count == 0)
                {
                    exactNodeMatchCount++;
                    equalLengthCount++;
                    equalOrBetterCostCount++;
                    continue;
                }

                if (legacyPath.nodes.Count == 0 || refactoredPath.nodes.Count == 0)
                {
                    continue;
                }

                float costLegacy = ReconstructCost(legacyPath);
                float costRefactored = ReconstructCost(refactoredPath);

                // Refactored cost should be equal or better (allowing 1% float margin)
                if (costRefactored <= costLegacy * 1.01f)
                {
                    equalOrBetterCostCount++;
                }

                // Length within +/- 1 step
                if (Math.Abs(legacyPath.nodes.Count - refactoredPath.nodes.Count) <= 1)
                {
                    equalLengthCount++;
                }

                // Exact coordinate match
                bool isExact = (legacyPath.nodes.Count == refactoredPath.nodes.Count);
                if (isExact)
                {
                    for (int i = 0; i < legacyPath.nodes.Count; i++)
                    {
                        if (legacyPath.nodes[i].position != refactoredPath.nodes[i].position)
                        {
                            isExact = false;
                            break;
                        }
                    }
                }

                if (isExact)
                {
                    exactNodeMatchCount++;
                }
            }

            double costEquivalenceRate = (double)equalOrBetterCostCount / totalValid;
            double lengthEquivalenceRate = (double)equalLengthCount / totalValid;
            double exactMatchRate = (double)exactNodeMatchCount / totalValid;

            // In grid A* on Epic maps (1184x1024), multiple symmetrical paths share similar costs,
            // and legacy pathfinding had a 20k loop cutoff and fuzzy 0.5f threshold.
            // Assert that >= 85% of queries achieve equal or superior traversal cost,
            // and >= 80% have equivalent step counts (+/- 1 step).
            Assert.True(costEquivalenceRate >= 0.85,
                $"Cost equivalence rate ({costEquivalenceRate:P1}) was below 85% threshold across {totalValid} queries.");
            Assert.True(lengthEquivalenceRate >= 0.80,
                $"Length equivalence rate ({lengthEquivalenceRate:P1}) was below 80% threshold across {totalValid} queries.");
        }
    }
}
