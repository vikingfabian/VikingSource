using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.GO.Creature;
using VikingEngine.Core.Wand.Laws;

namespace VikingEngine.Core.Wand.Map
{
    class World
    {
        
        public PlayerCharacter hero;
        public MapData map;
        public TurnOrderManager turnOrderManager = new TurnOrderManager();

        public SpottedArray<AbsCreature> creatures = new SpottedArray<AbsCreature>();

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

        public void newCreature(AbsCreature creature)
        {
            creatures.Add(creature);
        }
    }
}
