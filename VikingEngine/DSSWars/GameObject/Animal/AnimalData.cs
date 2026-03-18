using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.PJ.Bagatelle;

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

                case ItemResourceType.Boar:
                    new Livestock(tilepos, wp, DssVar.boarModel);
                    break;

                case ItemResourceType.Pig:
                    new Pig(tilepos, wp);
                    break;

                case ItemResourceType.Fowl:
                    new Livestock(tilepos, wp, DssVar.fowlModel);
                    break;

                case ItemResourceType.Hen:
                    new Hen(tilepos, wp);
                    break;

                case ItemResourceType.Hound:
                case ItemResourceType.Dog:
                    new Dog(tilepos, wp);
                    break;

                case ItemResourceType.Oxen:
                    new Livestock(tilepos, wp, DssVar.oxenModel);
                    break;
                case ItemResourceType.KineOxen:
                    new Livestock(tilepos, wp, DssVar.kineOxenModel);
                    break;

                case ItemResourceType.Pony:
                    new Livestock(tilepos, wp, DssVar.ponyModel);
                    break;
                case ItemResourceType.Horse:
                    new Livestock(tilepos, wp, DssVar.horseModel);
                    break;
                case ItemResourceType.WarHorse:
                    new Livestock(tilepos, wp, DssVar.warHorseModel);
                    break;
                case ItemResourceType.DraftHorse:
                    new Livestock(tilepos, wp, DssVar.draftHorseModel);
                    break;

                case ItemResourceType.WildPig:
                case ItemResourceType.WildHog:
                case ItemResourceType.WarHog:
                case ItemResourceType.StagHog:
                    new Livestock(tilepos, wp, DssVar.hogModel);
                    break;

                case ItemResourceType.Wolf:
                    new Livestock(tilepos, wp, DssVar.wolfModel);
                    break;
                case ItemResourceType.Warg:
                    new Livestock(tilepos, wp, DssVar.wargModel);
                    break;
                case ItemResourceType.AlphaWarg:
                    new Livestock(tilepos, wp, DssVar.alphaWargModel);
                    break;

                case ItemResourceType.WildCat:
                case ItemResourceType.Lion:
                case ItemResourceType.WarLion:
                    new Livestock(tilepos, wp, DssVar.boarModel);
                    break;

                case ItemResourceType.Elephant:
                    new Livestock(tilepos, wp, DssVar.elephantModel);
                    break;
                case ItemResourceType.WarElephant:
                    new Livestock(tilepos, wp, DssVar.warElephantModel);
                    break;
                case ItemResourceType.Oliphant:
                    new Livestock(tilepos, wp, DssVar.oliphantModel);
                    break;

                case ItemResourceType.Pheasant:
                    new Pheasant(tilepos, wp);
                    break;
            }
        }
    }
}
