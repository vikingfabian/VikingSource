

using VikingEngine.Core.Wand.GO.Creature;

namespace VikingEngine.Core.Wand.GO.ObjectPointer
{

    struct CreaturePointer
    {
        public static readonly CreaturePointer Empty = new CreaturePointer();
        public CreatureType creatureType;
        public int index;

        public bool HasGameObject()
        {
            return creatureType > CreatureType.SpawnPos;
        }

        public AbsCreature Get()
        {
            return WandRef.world.creatures.GetIndex_Safe(index);
        }
    }



    enum CreatureType
    {
        NONE,
        SpawnPos,

        Hero,
        Hen,
        Goblin,
        Orc,
        Spider,
    }
}
