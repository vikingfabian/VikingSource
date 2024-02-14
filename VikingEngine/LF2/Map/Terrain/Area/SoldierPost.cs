using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class SoldierPost : AbsOutpost
    {
        public SoldierPost(IntVector2 pos, int index)
            : base(pos, index)
        {
        }

        public override void BuildOnChunk(Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = this.AreaChunkCenter;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, wp, true);
                wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Healer, wp, true);
                wp.WorldGrindex.Z += WorldPosition.ChunkHalfWidth;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, wp, true);
            }
        }

        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
            
        //}
        override public string MapLocationName { get { return "Soldier post"; } }
    }
}
