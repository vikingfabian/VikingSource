using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    abstract class AbsArea : IMiniMapLocation
    {
        public AbsArea()
        { }
        public int areaLevel;

        protected IntVector2 position;
        public IntVector2 AreaChunkCenter { get { return position + ChunkSize / PublicConstants.Twice; } }
        abstract public IntVector2 ChunkSize { get; }

        /// <param name="dataGenerated">If the chunk was generated or loaded from save file</param>
        /// <param name="gameObjects">Will be called twice, once for terrain and another for gameobjects
        abstract public void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects);


        virtual public bool MonsterFree { get { return false; } }

        virtual public void GenerateToStorage() { }

        ///// <param name="dataGenerated">If the chunk was generated or loaded from save file</param>
        //virtual public void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        //{}

        public IntVector2 MapLocationChunk { get { return AreaChunkCenter; } }
        abstract public IntVector2 TravelEntrance { get; }
        abstract public SpriteName MiniMapIcon { get; }
        abstract public string MapLocationName { get; }
        abstract public bool TravelTo { get; }
        virtual public bool VisibleOnMiniMap { get { return true; } }

        public IntVector2 ToLocalPos(IntVector2 chunk)
        {
            return chunk - position;
        }
        protected IntVector2 toWorldChunk(IntVector2 localPos)
        {
            return localPos + position;
        }
        
    }
}
