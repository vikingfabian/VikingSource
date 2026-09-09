using System;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Tests;
using Xunit;

namespace VikingEngine.Tests.Pathfinding
{
    public class RoadPathFindingTests
    {
        [Fact]
        public void RoadPathFinding_BasicRoad_ReachesGoal()
        {
            TestWorldHelper.SetupFlatWorld(64, 64);
            var pf = new RoadPathFinding();

            var start = new IntVector2(20, 20);
            var goal = new IntVector2(30, 30);

            var path = pf.FindPath(start, goal);

            Assert.NotNull(path);
            Assert.NotEmpty(path.nodes);
            Assert.Equal(goal, path.nodes[0].position);
        }
    }
}
