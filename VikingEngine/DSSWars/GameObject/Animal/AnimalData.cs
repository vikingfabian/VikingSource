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

        public void create(IntVector2 tilepos)
        {
            switch (animal)
            {
                case AnimalType.Pig:
                    new Pig(tilepos, wp);
                    break;
                case AnimalType.Hen:
                    new Hen(tilepos, wp);
                    break;
                case AnimalType.Pheasant:
                    new Pheasant(tilepos, wp);
                    break;
            }
        }
    }
}
