using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class EndTomb : AbsArea
    {
        static readonly IntVector2 AreaSize = new IntVector2(1, 1);
        //IntVector2 position;
        public static VoxelObjGridData tombImg;
        public static VoxelObjGridData pillarImg;

        public static void LoadImages()
        {
            tombImg =  Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.end_tomb)[0];
            pillarImg =  Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.end_pillar)[0];
        }

        public EndTomb(IntVector2 pos)
            :base()
        {
            this.position = pos;
            MiniMapData.Locations.Add(this);
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
                return false;
            }
        }
        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                IntVector2 center = this.AreaChunkCenter;
                LfRef.chunks.GetScreen(center).WriteProtected = true;
                new GameObjects.EnvironmentObj.EndTombData(center);
            }
            else
            {

                Map.WorldPosition wp = new WorldPosition(chunk.Index);
                wp.X += Map.WorldPosition.ChunkHalfWidth;
                wp.Z += Map.WorldPosition.ChunkHalfWidth;

                wp.Y = Map.WorldPosition.ChunkStandardHeight - 2;
                tombImg.BuildOnTerrain(wp);

                float pillarAngle = MathHelper.TwoPi / LootfestLib.BossCount;
                float pillarRadius = Map.WorldPosition.ChunkHalfWidth * 0.8f;

                wp.Z += 3;

                Rotation1D rot = Rotation1D.D0;
                for (int i = 0; i < LootfestLib.BossCount; ++i)
                {
                    rot.Add(pillarAngle);
                    Map.WorldPosition pillarPos = wp;

                    Vector2 addXZ = rot.Direction(pillarRadius);
                    pillarPos.X += Convert.ToInt32(addXZ.X);
                    pillarPos.Z += Convert.ToInt32(addXZ.Y);

                    pillarImg.BuildOnTerrain(pillarPos);
                }
                // BeginGenerateEnvironmentObj(chunkIx, IntVector2.Zero);
            }
        }

        //public  void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{
        //   // base.GenerateChunkGameObjects(chunk, dataGenerated);
        //    IntVector2 center = this.AreaChunkCenter;
        //    LfRef.chunks.GetScreen(center).WriteProtected = true;
        //    new GameObjects.EnvironmentObj.EndTombData(center);
        //}
       
        //public IntVector2 MapLocationChunk
        //{ get { return AreaChunkCenter; } }
        override public SpriteName MiniMapIcon { get { return SpriteName.IconEndBossTomb; } }

        override public string MapLocationName { get { return "Evil tomb"; } }
        override public bool TravelTo { get { return false; } }
    }
    
}
