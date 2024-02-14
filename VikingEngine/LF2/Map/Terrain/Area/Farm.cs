using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class Farm : AbsOutpost
    {
        public Farm(IntVector2 pos, int index)
            : base(pos, index)
        { }


        public override void BuildOnChunk(Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = this.AreaChunkCenter;

                if (dataGenerated)
                {
                    GameObjects.EnvironmentObj.ChestData chestData = new GameObjects.EnvironmentObj.ChestData(wp, GameObjects.EnvironmentObj.MapChunkObjectType.DiscardPile);
                    chestData.GadgetColl.AddItem(new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Apple, GameObjects.Gadgets.Quality.Medium, 8));
                    chestData.GadgetColl.AddItem(new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Seed, GameObjects.Gadgets.Quality.Medium, 4));
                }
                wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC, wp, true);
                wp.WorldGrindex.Z += WorldPosition.ChunkHalfWidth;
                LfRef.chunks.GetScreen(wp).AddChunkObject(new GameObjects.EnvironmentObj.CritterSpawn(wp), true);
            }
        }

        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
           
        //}
        override public string MapLocationName { get { return "Farm"; } }
    }
}
