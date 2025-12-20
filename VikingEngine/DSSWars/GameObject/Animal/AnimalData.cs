using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.GameObject.Animal
{
    struct AnimalData
    {
        public Vector3 wp;
        public ItemResourceType animal;

        public AnimalData(Vector3 wp, ItemResourceType animal)
        {
            this.wp = wp;
            this.animal = animal;
        }

        public void create(IntVector2 tilepos)
        {
            switch (animal)
            {
                default:
                    new TempAnimal(tilepos, wp);
                    break;
                case ItemResourceType.Pig:
                    new Pig(tilepos, wp);
                    break;
                case ItemResourceType.Hen:
                    new Hen(tilepos, wp);
                    break;
                case ItemResourceType.Pheasant:
                    new Pheasant(tilepos, wp);
                    break;
            }
        }
    }
}
