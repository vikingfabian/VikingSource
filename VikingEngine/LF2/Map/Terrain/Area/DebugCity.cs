using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class DebugCity : AbsUrban
    {
        const int Size = 8;
        Data.Characters.NPCdata[,][] dataGrid; 
        public DebugCity(IntVector2 position)
            : base(new IntVector2(Size), position, 0)
        {
            //NPCdata = new List<Data.Characters.NPCdata>
            //{
            //    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.Healer),
            //    new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.Granpa),
            //    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.Guard),
            //    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport),
            //    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.ImBug),
            //    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.ImError),
            //    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.Lumberjack),
            //    new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.Salesman),

            //};
//            for (int i = 0; i < Size * Size; i++)
//            {
//NPCdata.Add(
//            }
            dataGrid = new Data.Characters.NPCdata[Size, Size][];
            ForXYLoop loop = new ForXYLoop(new IntVector2(Size));
            while (loop.Next())
            {
                dataGrid[loop.Position.X, loop.Position.Y] = new Data.Characters.NPCdata[]
                {
                    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.DebugNPC),
                    //new Data.Characters.NPCdata(Data.Characters.NPCdataArgs.GenerateLater, GameObjects.EnvironmentObj.MapChunkObjectType.DebugNPC),
                };
            }
            //IntVector2 markPOs = new IntVector2(169, 120) - position;
            //dataGrid[markPOs.X, markPOs.Y][1].EarMark = true;
        }

        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            ////int xpos = chunkIx.X - position.X;
            //IntVector2 localPos = chunkIx - position;
            //Map.WorldPosition wp = new WorldPosition(chunkIx);
            //Data.Characters.NPCdata[] dataList = dataGrid[localPos.X, localPos.Y];

            ////bool earMark = false;
            ////if (chunkIx == new IntVector2(169, 120))
            ////{
            ////    earMark = true;
            ////}

            //for (int i = 0; i < dataList.Length; i++)
            //{
            //    if (dataList[i] != null)
            //    {
            //        //Build npc object
            //        VoxelObjGridData stationImg = dataList[i].WorkingStation;
            //        if (stationImg != null)
            //        {
            //            stationImg.BuildOnTerrain(wp);
            //        }
            //        dataList[i].wp = wp;
            //        dataList[i].BeginGeneratorRequest(wp.ChunkGrindex);
            //    }
            //    wp.WorldGrindex.X += 16;
            //}
        }
        public override SpriteName MiniMapIcon
        {
            get { return SpriteName.ImageMissingIcon; }
        }
        override public UrbanType Type { get { return UrbanType.DebugCity; } }
        override public bool MonsterFree { get { return true; } }
    }
}
