using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    abstract class AbsOutpost: AbsArea
    {
        static readonly IntVector2 AreaSize = new IntVector2(1, 1);
        //IntVector2 position;

        public AbsOutpost(IntVector2 pos, int index)
            : base()
        {
            this.position = pos;
            this.areaLevel = index;
            //if (index == 0)
            //{
                MiniMapData.Locations.Add(this);
            //}
        }

        //public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        //{
        //}

        //public override IntVector2 AreaChunkCenter
        //{
        //    get { return position; }
        //}
        public override IntVector2 ChunkSize
        {
            get { return AreaSize; }
        }
        public override bool MonsterFree
        {
            get
            {
                return true;
            }
        }


        override public IntVector2 TravelEntrance { get { return position; } }
        override public SpriteName MiniMapIcon { get { return SpriteName.IconOutPost; } }

       // abstract public string MapLocationName { get; }
        override public bool TravelTo { get { return false || PlatformSettings.TravelEverywhere; } }
    }
}
