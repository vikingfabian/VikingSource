using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.BlockMap.Level
{
    class CreativeArea : AbsLevel
    {
        public CreativeArea()
            : base(LevelEnum.Creative)
        {
            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
            mySeed = 0; //Lock seed
        }

        protected override void generateMapAsynch()
        {
            Rectangle2 area = Rectangle2.FromCenterSize(squares.Size / 2, new IntVector2(108));

            placeOpenArea(area, 0);

            addDesignArea(AreaDesignType.PublicBuild, 0, area);

            levelEntrance = toWorldXZ(area.pos + new IntVector2(16));

            setTeleportLocation(TeleportLocationId.Creative, levelEntrance);
        }

        protected void addDesignArea(AreaDesignType type, int ix, Rectangle2 area)
        {
            area.pos = toChunkPos(area.pos);
            area.size = area.size / BlockMapLib.SquaresPerChunkW;

            designAreas.areas.Add(new DesignAreaStorage(type, ix, area));
        }

        void createGameobjects(GoArgs args)
        {
        }

        protected override List<VoxelModelName> loadTerrainModelsList()
        {
            return new List<VoxelModelName>();
        }
    }
}
