using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Animal
{
    struct AnimalData
    {
        public Vector3 wp;
        public AnimalType animal;

        public AnimalData(Vector3 wp, AnimalType animal)
        {
            this.wp = wp;
            this.animal = animal;
        }

        public void create(Map.Tile tile)
        {
            switch (animal)
            {
                case AnimalType.Pig:
                    new Pig(tile, wp);
                    break;
                case AnimalType.Hen:
                    new Hen(tile, wp);
                    break;
            }
        }
    }
}
