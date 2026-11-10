using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Core.Wand.GO.Creature;
using VikingEngine.Core.Wand.GO.ObjectPointer;
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
            foreach (var sp in spawnPos)
            {
                switch (sp.objectCategory)
                {
                    case GameObjectCategory.Creature:
                        WandRef.world.newCreature( new Goblin(new MapPlacement(sp.pos, (WandRef.world.hero.currentPos.tilePos - sp.pos).ToDir4)));
                        break;
                }
            }
        }
    }
}
