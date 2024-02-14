using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{

    class HomeBase : AbsArea
    {
        static readonly IntVector2 AreaSize = new IntVector2(2, 1);
        //IntVector2 position;


        public HomeBase(IntVector2 pos, int index)
            :base()
        {
            this.position = pos;
            housePos = new WorldPosition(pos, 0, Map.WorldPosition.ChunkStandardHeight - 2, 8);
            this.areaLevel = index;
            if (index == 0)
            {
                MiniMapData.Locations.Add(this);
            }

            //the players personal chest
            //WorldPosition chestPos = WorldPosition.EmptyPos;
            //chestPos.ScreenIndex = new IntVector2(pos.X +1, pos.Y +2);
            //chestPos.BlockPos = new IntVector3(2, 4, 4);
        }
        //public override IntVector2 AreaChunkCenter
        //{
        //    get { return position; } 
        //}
        override public IntVector2 TravelEntrance
        {
            get { return position; }
        }
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

        Map.WorldPosition housePos;
        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                GenerateChunkGameObjects(chunk, dataGenerated);
            }
            else
            {
                if (chunk.Index.X == position.X)
                {
                    AbsUrban.homeHouse.BuildOnTerrain(housePos);
                }
            }
            //else
            //    BeginGenerateEnvironmentObj(chunkIx, IntVector2.Zero);
        }

        public void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        {
            if (chunk.Index.X != position.X)
            {
                //base.GenerateChunkGameObjects(chunk, dataGenerated);
                WorldPosition wp = new WorldPosition(chunk.Index);
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Father, wp, true);

                System.Diagnostics.Debug.WriteLine("Father - generate home base");

                WorldPosition guardPos = wp;
                guardPos.WorldGrindex.X += 4;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, guardPos, true);
                //
                guardPos.WorldGrindex.X += 4;
                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Guard, guardPos, true);

                wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
                //if (dataGenerated)
                //{
                //    GameObjects.EnvironmentObj.ChestData chestData = new GameObjects.EnvironmentObj.ChestData(wp, GameObjects.EnvironmentObj.MapChunkObjectType.Chest);
                //    chestData.GadgetColl.AddItem(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Arrow, 10));
                //    chestData.GadgetColl.AddItem(new GameObjects.Gadgets.Goods(GameObjects.Gadgets.GoodsType.Grilled_meat, GameObjects.Gadgets.Quality.Medium));
                //}
                wp.SetChunkAndLocalBlock(chunk.Index, 22, 0, 22);

                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport, wp, true);

                new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Mother, housePos.GetNeighborPos(new IntVector3(5, 2, 5)), true);
            }
        }

        
        override public SpriteName MiniMapIcon { get { return SpriteName.IconHomeBase; } }

        override public string MapLocationName { get { return "Home"; } }
        override public bool TravelTo { get { return true; } }
    }

}
