using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Core.Wand.GO.Creature;
using VikingEngine.Core.Wand.GO.ObjectPointer;
using VikingEngine.Core.Wand.Laws;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.Core.Wand.Map
{

    struct SpawnPos
    {
        public IntVector2 pos;
        public GameObjectCategory objectCategory;
    }

    class RoomSpawner
    {
        public Dictionary<int, RoomSpawnerInstance> rooms_index_insance = new Dictionary<int, RoomSpawnerInstance>(2);

        public void Add(ItemPointer item, IntVector2 pos)
        {
            add(item.index, pos, GameObjectCategory.Item);
        }

        public void Add(CreaturePointer item, IntVector2 pos)
        {
            add(item.index, pos, GameObjectCategory.Creature);
        }

        void add(int itemIndex, IntVector2 pos, GameObjectCategory category)
        {
            if (!rooms_index_insance.TryGetValue(itemIndex, out RoomSpawnerInstance instance))
            {
                instance = new RoomSpawnerInstance() { roomIndex = itemIndex };
                rooms_index_insance.Add(itemIndex, instance);
            }

            instance.spawnPos.Add(new SpawnPos() { pos = pos, objectCategory = category });
        }

        public void Spawn()
        {
            foreach (var kv in rooms_index_insance)
            {
                kv.Value.Spawn();
            }
        }
    }

    class RoomSpawnerInstance
    {
        public List<SpawnPos> spawnPos = new List<SpawnPos>();

        public int roomIndex;

        public void Spawn()
        {
            BSPNode node = WandRef.world.map.leaves[roomIndex];

            List<IntVector2> creatureSpawns = new List<IntVector2>(8);
            List<IntVector2> itemSpawns = new List<IntVector2>(8);
            foreach (var sp in spawnPos)
            {
                switch (sp.objectCategory)
                {
                    case GameObjectCategory.Creature:
                        creatureSpawns.Add(sp.pos);
                        break;
                    case GameObjectCategory.Item:
                        itemSpawns.Add(sp.pos);
                        break;
                }
            }

            if (creatureSpawns.Count >= 3 && Ref.rnd.Chance(0.5))
            {
                ObjectLifeTime.SpawnCreature(arraylib.RandomListMemberPop(creatureSpawns), CreatureType.BlackKnight, true);
            }
            
            if (creatureSpawns.Count >= 2 && Ref.rnd.Chance(0.7))
            {
                ObjectLifeTime.SpawnCreature(arraylib.RandomListMemberPop(creatureSpawns), CreatureType.Orc, true);
            }

            foreach (var pos in creatureSpawns)
            {
                ObjectLifeTime.SpawnCreature(pos, CreatureType.Goblin, true);
            }

            if (itemSpawns.Count > 0 && Ref.rnd.Chance(0.8))
            {
                ObjectLifeTime.SpawnItem(arraylib.RandomListMemberPop(itemSpawns), TileItemType.Altar, false);
            }
            if (itemSpawns.Count > 0 && Ref.rnd.Chance(0.8))
            {
                ObjectLifeTime.SpawnItem(arraylib.RandomListMemberPop(itemSpawns), TileItemType.Magic_wand, false);
            }
            if (itemSpawns.Count > 0 && Ref.rnd.Chance(0.7))
            {
                ObjectLifeTime.SpawnItem(arraylib.RandomListMemberPop(itemSpawns), TileItemType.Health_potion, false);
            }
            if (itemSpawns.Count > 0 && Ref.rnd.Chance(0.7))
            {
                ObjectLifeTime.SpawnItem(arraylib.RandomListMemberPop(itemSpawns), TileItemType.Book, false);
            }
            if (itemSpawns.Count > 0 && Ref.rnd.Chance(0.7))
            {
                ObjectLifeTime.SpawnItem(arraylib.RandomListMemberPop(itemSpawns), TileItemType.Magic_wand, false);
            }
        }


    }
}
