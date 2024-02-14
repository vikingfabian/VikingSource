using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    

    class Salesman: AbsOutpost
    {
        GameObjects.EnvironmentObj.MapChunkObjectType type;
        public Salesman(IntVector2 pos, int index, GameObjects.EnvironmentObj.MapChunkObjectType type)
            : base(pos, index)
        {
            this.type = type;
        }
        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = this.AreaChunkCenter;
                if (type == GameObjects.EnvironmentObj.MapChunkObjectType.Salesman)
                    new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, wp, true);
                wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
                new Data.Characters.NPCdata(type, wp, true);
                wp.WorldGrindex.Z += WorldPosition.ChunkHalfWidth;
                if (type == GameObjects.EnvironmentObj.MapChunkObjectType.Salesman)
                    new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, wp, true);
            }
            else
            {
                WorldPosition wp = new WorldPosition(chunk.Index);
                wp.WorldGrindex.X += 18;
                Data.Characters.NPCdata.BuildStationOnChunk(type, wp);
            }
        }
        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
            
        //}
        override public string MapLocationName { get { return type.ToString(); } }
    }

    class Wiselady : AbsOutpost
    {
        public Wiselady(IntVector2 pos, int index)
            : base(pos, index)
        {

        }
        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = this.AreaChunkCenter;
                wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Wise_Lady, wp, true);
            }
            else
            {
                WorldPosition wp = new WorldPosition(chunk.Index);
                wp.WorldGrindex.X += 18;
                Data.Characters.NPCdata.BuildStationOnChunk(GameObjects.EnvironmentObj.MapChunkObjectType.Wise_Lady, wp);
            }
        }
        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
           
        //}
        override public string MapLocationName { get { return "Wise lady"; } }
    }

}
