using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class FreeBuild : AbsArea
    {
        static readonly IntVector2 AreaSize = new IntVector2(LootfestLib.PublicBuildAreaSize);
        public FreeBuild(IntVector2 pos)
            : base()
        {
            this.position = pos;
            MiniMapData.Locations.Add(this);
        }
        public override void  BuildOnChunk(Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            chunk.FillFlatFloor(Data.MaterialType.sand);

            if (chunk.Index == AreaChunkCenter)
            {
                if (gameObjects)
                {
                    WorldPosition wp = new WorldPosition(chunk.Index);
                    new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Builder, wp, true);
                }
            }
            else
            {
                if (!gameObjects)
                {
                    Data.RandomSeed.Instance.SetSeedPosition(chunk.Index);
                    byte rnd = Data.RandomSeed.Instance.NextByte();
                    Voxels.VoxelObjGridData model = null;
                    if (rnd < 20)
                    {
                        model = AbsUrban.BuildAreaModel2;
                    }
                    else if (rnd < 50)
                    {
                        model = AbsUrban.BuildAreaModel3;
                    }
                    else if (rnd < 100)
                    {
                        model = AbsUrban.BuildAreaModel1;
                    }

                    if (model != null)
                    {
                        WorldPosition wp = new WorldPosition(chunk.Index);
                        wp.WorldGrindex.Y = WorldPosition.ChunkStandardHeight;
                        wp.WorldGrindex.X += Data.RandomSeed.Instance.Next(Map.WorldPosition.ChunkHalfWidth);
                        wp.WorldGrindex.Z += Data.RandomSeed.Instance.Next(Map.WorldPosition.ChunkHalfWidth);
                        model.BuildOnTerrain(wp);
                    }
                }
            }
        }

        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
        //    if (chunk.Index == AreaChunkCenter)
        //    {
        //        WorldPosition wp = new WorldPosition(chunk.Index);
        //        new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.Builder, wp);
        //    }
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


        override public IntVector2 TravelEntrance { get { return AreaChunkCenter; } }
        override public SpriteName MiniMapIcon { get { return SpriteName.IconFreeBuildArea; } }
        public override string MapLocationName
        {
            get { return "Build area"; }
        }
        override public bool TravelTo { get { return true; } }
    }
}
