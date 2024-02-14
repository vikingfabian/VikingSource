using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Strategy
{
    class Gamer
    {
        public int Coins, VictoryPoints = 0;
        public List<MapArea> areas = new List<MapArea>();
        public GamerData data;
        public MapArea startArea;
        public AnimalSetup animalSetup;

        public Gamer(GamerData data)
        {
            this.data = data;
            animalSetup = AnimalSetup.Get(data.joustAnimal);
        }

        public void setStartArea(MapArea start, int playerCount)
        {
            this.startArea = start;

            if (playerCount <= 6)
            {
                start.placeArmy(this, 6);

                int adjPlaceCount = 3;
                switch (start.adjacentAreas.Count)
                {
                    case 2: adjPlaceCount = 5; break;
                    case 3: adjPlaceCount = 4; break;
                    case 4: adjPlaceCount = 3; break;
                    case 5: adjPlaceCount = 2; break;
                }

                foreach (var adj in start.adjacentAreas)
                {
                    adj.toArea.placeArmy(this, adjPlaceCount);
                }
            }
            else
            {
                start.placeArmy(this, 8);
            }

            start.type = AreaType.Castle;

            //One adjacent is always a VP point
            arraylib.RandomListMember(start.adjacentAreas).toArea.type = AreaType.VictoryPoint;
        }

        public bool buy(int cost, GamerDisplay display)
        {
            if (cost <= Coins)
            {
                Coins -= cost;
                display.refreshGamer(this);
                return true;
            }
            return false;
        }
    }
}
