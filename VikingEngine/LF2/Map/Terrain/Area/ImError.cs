using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class ImError : AbsOutpost
    {
        public ImError(IntVector2 pos, int index)
            : base(pos, index)
        {
        }

        public override void BuildOnChunk(Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = this.AreaChunkCenter;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.ImError, wp, true);
                wp.WorldGrindex.X += 6;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.ImGlitch, wp, true);
                wp.WorldGrindex.X += 6;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.ImBug, wp, true);
            }
        }

        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
            
        //}
        override public string MapLocationName { get { return "Error"; } }
        public override SpriteName MiniMapIcon
        {
            get
            {
                return SpriteName.NO_IMAGE;
            }
        }
    }
}
