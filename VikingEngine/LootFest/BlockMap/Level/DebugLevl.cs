using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.BlockMap.Level
{
    class DebugLevl : AbsLevel
    {
        public DebugLevl()
            : base(LevelEnum.Debug)
        {
            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
        }

        protected override void generateMapAsynch()
        {
            //BlockMapSegment segment = new BlockMapSegment(  //LfRef.blockmaps.loadSegment(66);

            //IntVector2 start = standardStartPos();

            //var pointer = placeSegment(segment, start, Dir4.N, 0, OpenAreaLockId);
            Rectangle2 area = Rectangle2.FromCenterSize(squares.Size / 2, new IntVector2(80));//new Rectangle2(new IntVector2(16), new IntVector2(48));

            placeOpenArea(area, 0);

            addDesignArea(AreaDesignType.PublicBuild, 0, area);

            levelEntrance = toWorldXZ(area.Center);

            setTeleportLocation(TeleportLocationId.Debug, levelEntrance);

            //var center = pointer.specialPoints[0];

            //addSpawn(new SpawnPointDelegate(toWorldXZ(center.position), createGameobjects, SpawnPointData.Empty,
            //                SpawnImportance.Must_0, true, 1, true));
        }

        protected void addDesignArea(AreaDesignType type, int ix, Rectangle2 area)
        {
            area.pos = toChunkPos(area.pos);
            area.size = area.size / BlockMapLib.SquaresPerChunkW;

            designAreas.areas.Add(new DesignAreaStorage(type, ix, area));
        }

        void createGameobjects(GoArgs args)
        {
            //new VikingEngine.LootFest.GO.Characters.GoblinBerserk(args);

            //args.startWp.X += 4;
            //args.updatePosV3();

            //new VikingEngine.LootFest.GO.Characters.Boss.SpiderBot(args, this);
            //var wolf = new VikingEngine.LootFest.GO.Characters.GreatWolf(args);
            //wolf.GoToSleep();
        }

        protected override List<VoxelModelName> loadTerrainModelsList()
        {
            return new List<VoxelModelName>();
        }
    }
}
