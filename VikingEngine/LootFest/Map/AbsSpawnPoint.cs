//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.LootFest.Map.Terrain.Generation;
//using VikingEngine.LootFest.GO;

//namespace VikingEngine.LootFest.Map
//{
//    /// <summary>
//    /// Fabians new superawesome spawn system
//    /// </summary>
//    abstract class AbsSpawnPoint
//    {
//        /// <summary>
//        /// If spawnlock is larger than Zero, it wont spawn
//        /// </summary>
//        public int spawnLock = 0;

//        static int NextIndex = 0;
//        public int uniqueIndex = NextIndex++;

//        abstract public IntVector2 chunkGrindex { get; }

//        abstract public void GenerateGameObjects();
//    }

//    /// <summary>
//    /// Ersätter List<CreateGameObjectArgs> i RoomAttribute
//    /// </summary>
//    class DelegateSpawnPoint : AbsSpawnPoint
//    {
//        IntVector2 localPosition;
//        CreateGameObjectDelegate action;
//        IntVector2 worldXZ;
//        Map.WorldPosition wp;
//        GameObjectType type;
//        int characterLevel;

//        public DelegateSpawnPoint(IntVector2 localPosition, CreateGameObjectDelegate action, GameObjectType type, int characterLevel)
//        {
//            this.localPosition = localPosition;
//            this.action = action;
//            this.type = type;
//            this.characterLevel = characterLevel;
//        }

//        public void OnLayoutComplete(IntVector2 worldXZ)
//        {
//            this.worldXZ = worldXZ;
//            wp = new WorldPosition(localPosition + worldXZ, 0);
//        }

//        public override void GenerateGameObjects()
//        {
//            if (spawnLock <= 0)
//            {
//                wp.SetFromHeightMap(1);
//                //action(new GoArgs(wp, type, characterLevel, this));

//               // Debug.Log("Delegate Spawn" + uniqueIndex.ToString() + ", ongenerate: " + action.ToString());
//            }
//        }

//        public override IntVector2 chunkGrindex
//        {
//            get { return wp.ChunkGrindex; }
//        }
//    }
//}
