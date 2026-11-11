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
    }
}
