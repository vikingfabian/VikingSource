using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Data;
using VikingEngine.Core.Wand.GO.Creature;
using VikingEngine.Core.Wand.GO.Item;
using VikingEngine.Core.Wand.Laws;

namespace VikingEngine.Core.Wand.Map
{
    class World
    {
        public MagicFormulaLib magicFormulas = new MagicFormulaLib();
        public PlayerCharacter hero;
        public MapData map;
        public TurnOrderManager turnOrderManager = new TurnOrderManager();
        public SpottedArray<AbsCreature> creatures = new SpottedArray<AbsCreature>();
        public SpottedArray<AbsItem> items = new SpottedArray<AbsItem>();

        public int SpawnTimer = 0;

        public World()
        {
            WandRef.world = this;
        }

        public void StartLevel()
        {
            map = new MapData();
            new GenerateMap(map).Generate(/*new IntVector2(5, 4)*/);
            RevealRoom.Reveal(map.playerEnterPos);

            hero = new PlayerCharacter(new MapPlacement(map.playerEnterPos, Dir4.S));
            
        }

        public int newCreature(AbsCreature creature)
        {
            int ix = creatures.Add(creature);
            return ix;
        }

        public int newItem(AbsItem item)
        {
            int ix = items.Add(item);
            return ix;
        }
    }
}
