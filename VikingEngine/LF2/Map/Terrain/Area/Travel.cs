
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class Travel : AbsOutpost
    {
        public Travel(IntVector2 pos, int index)
            : base(pos, index)
        {
        }

        public override void BuildOnChunk(Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = this.AreaChunkCenter;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, wp, true);// new GameObjects.NPC.Guard(wp, null);
                wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport, wp, true);//new LootfestLib.TravelCost(wp, null);
            }
        }

        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
           
        //}
        override public SpriteName MiniMapIcon { get { return SpriteName.IconTravel; } }
        override public string MapLocationName { get { return "Travel"; } }
    }
}
