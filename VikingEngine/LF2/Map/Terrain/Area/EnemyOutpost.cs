using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class EnemyOutpost : AbsOutpost
    {
        public EnemyOutpost(IntVector2 pos, int index)
            : base(pos, index)
        {
        }
        public override bool MonsterFree
        {
            get
            {
                return false;
            }
        }
        public override void BuildOnChunk(Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = this.AreaChunkCenter;


                GameObjects.EnvironmentObj.ChestData chestData = new GameObjects.EnvironmentObj.ChestData(wp, GameObjects.EnvironmentObj.MapChunkObjectType.Chest);

                chestData.GadgetColl.AddItem(new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Apple, GameObjects.Gadgets.Quality.Medium, 8));
                chestData.GadgetColl.AddItem(new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Seed, GameObjects.Gadgets.Quality.Medium, 4));
            }
        }
        //public override void Time_Update(float time)
        //public override void  GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
 	
            
            
        //}
        override public SpriteName MiniMapIcon { get { return SpriteName.IconEnemyOutpost; } }
        override public string MapLocationName { get { return "Enemy outpost"; } }
    }
}
