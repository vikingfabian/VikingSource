using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.Director;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    class MonsterSpawn : AbsArea
    {
        public MonsterSpawn(IntVector2 pos, int lvl)
        {
            this.areaLevel = lvl;
            this.position = pos;
            MiniMapData.Locations.Add(this);
        }

        override public IntVector2 TravelEntrance { get { return position; } }
        override public SpriteName MiniMapIcon
        {
            get { return 
            LfRef.gamestate.Progress.AliveMonstersSpawns.Get(areaLevel)? 
            SpriteName.IconEggNest : SpriteName.IconEggNestDestroyed; } }

        
        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            if (gameObjects)
            {
                if (LfRef.gamestate.Progress.AliveMonstersSpawns.Get(areaLevel))
                {
                    new NestSpawner(AreaChunkCenter, areaLevel);
                }
            }
        }

        //public override void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{


        //}

        public void DeleMe()
        {
            MiniMapData.Locations.Remove(this);
            PlayState.DebugWarpLocations.Remove(this);
        }
        public override IntVector2 ChunkSize
        {
            get { return IntVector2.One; }
        }

        public override string ToString()
        {
            return "MonsterSpawn" + areaLevel.ToString();
        }
        override public bool TravelTo { get { return false; } }
        override public string MapLocationName
        {
            get
            {
                return 
            (LfRef.gamestate.Progress.AliveMonstersSpawns.Get(areaLevel)?
            "Monster nest" : "Destroyed nest")
            ; } }
    }

    class NestSpawner : IEnvObjGenerator
    {
        int areaLevel;
        public NestSpawner(IntVector2 pos, int areaLvl)
        {
            this.areaLevel = areaLvl;
            LfRef.worldOverView.EnvironmentObjectQue.AddGeneratorRequest(new WaitingEnvObjGenerator(this, pos));
        }
        public void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
            
            new GameObjects.Characters.EggNest(chunk.Index, areaLevel);
        }
    }
}
