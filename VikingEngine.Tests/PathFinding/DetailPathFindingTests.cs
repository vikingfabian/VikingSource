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
    public class DetailPathFindingTests
    {
        [Fact]
        public void DetailPathFinding_StraightLine_ReachesGoal()
        {
            TestWorldHelper.SetupFlatWorld(64, 64);
            var pf = new DetailPathFinding();

            var start = new IntVector2(20, 20);
            var goal = new IntVector2(20, 60);

            var path = pf.FindPath(-1, start, Rotation1D.D0, goal, false, false, false);

            Assert.NotNull(path);
            Assert.NotEmpty(path.nodes);
            Assert.Equal(goal, path.nodes[0].position);
        }

        [Fact]
        public void DetailPathFinding_MultipleRecycles_Succeeds()
        {
            TestWorldHelper.SetupFlatWorld(64, 64);
            var pf = new DetailPathFinding();

            for (int i = 0; i < 5; i++)
            {
                var start = new IntVector2(20 + i * 2, 20);
                var goal = new IntVector2(20 + i * 2, 50);

                var path = pf.FindPath(-1, start, Rotation1D.D0, goal, false, false, false);

                Assert.NotNull(path);
                Assert.NotEmpty(path.nodes);
                Assert.Equal(goal, path.nodes[0].position);

                pf.recycle();
            }
        }
    }
}
