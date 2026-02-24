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

                case ItemResourceType.Hound:
                case ItemResourceType.Dog:
                    new Dog(tilepos, wp);
                    break;

                case ItemResourceType.Pony:
                case ItemResourceType.Horse:
                case ItemResourceType.WarHorse:
                case ItemResourceType.DraftHorse:
                    new Horse(tilepos, wp);
                    break;

                case ItemResourceType.WildPig:
                case ItemResourceType.WildHog:
                case ItemResourceType.WarHog:
                case ItemResourceType.StagHog:
                    new Hog(tilepos, wp);
                    break;

                case ItemResourceType.Wolf:
                case ItemResourceType.Warg:
                case ItemResourceType.AlphaWarg:
                    new Wolf(tilepos, wp);
                    break;

                case ItemResourceType.WildCat:
                case ItemResourceType.Lion:
                case ItemResourceType.WarLion:
                    new Lion(tilepos, wp);
                    break;

                case ItemResourceType.Elephant:
                case ItemResourceType.WarElephant:
                case ItemResourceType.Oliphant:
                    new Elephant(tilepos, wp);
                    break;

                case ItemResourceType.Pheasant:
                    new Pheasant(tilepos, wp);
                    break;
            }
        }
    }
}
