using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Resource;
using VikingEngine.PJ.Bagatelle;
using VikingEngine.ToGG.ToggEngine.Map;

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
                    new Livestock(tilepos, wp, DssVar.boarModel, SoundLib.pig, 0.3f);
                    break;

                case ItemResourceType.Pig:
                    new Pig(tilepos, wp);
                    break;

                case ItemResourceType.Fowl:
                    new Livestock(tilepos, wp, DssVar.fowlModel, SoundLib.hen, 0.25f);
                    break;

                case ItemResourceType.Hen:
                    new Hen(tilepos, wp);
                    break;

                
                case ItemResourceType.Dog:
                    new Livestock(tilepos, wp,  DssVar.dogModel, SoundLib.dog, 0);
                    break;

                case ItemResourceType.Hound:
                    new Livestock(tilepos, wp, DssVar.dogModel, SoundLib.dog, -0.25f);
                    break;


                case ItemResourceType.Oxen:
                    new Livestock(tilepos, wp, DssVar.oxenModel, SoundLib.oxen, 0);
                    break;
                case ItemResourceType.KineOxen:
                    new Livestock(tilepos, wp, DssVar.kineOxenModel, SoundLib.oxen, -0.25f);
                    break;

                case ItemResourceType.Pony:
                    new Livestock(tilepos, wp, DssVar.ponyModel, SoundLib.horse, 0.25f);
                    break;
                case ItemResourceType.Horse:
                    new Livestock(tilepos, wp, DssVar.horseModel, SoundLib.horse, 0f);
                    break;
                case ItemResourceType.WarHorse:
                    new Livestock(tilepos, wp, DssVar.warHorseModel, SoundLib.horse, -0.25f);
                    break;
                case ItemResourceType.DraftHorse:
                    new Livestock(tilepos, wp, DssVar.draftHorseModel, SoundLib.horse, -0.25f);
                    break;

                case ItemResourceType.WildPig:
                    new Livestock(tilepos, wp, DssVar.hogModel, SoundLib.hog, 0.25f);
                    break;

                case ItemResourceType.WildHog:
                    new Livestock(tilepos, wp, DssVar.hogModel, SoundLib.hog, 0f);
                    break;

                case ItemResourceType.WarHog:
                    new Livestock(tilepos, wp, DssVar.hogModel, SoundLib.hog, -0.25f);
                    break;

                case ItemResourceType.StagHog:
                    new Livestock(tilepos, wp, DssVar.hogModel, SoundLib.hog, -0.25f);
                    break;

                case ItemResourceType.Wolf:
                    new Livestock(tilepos, wp, DssVar.wolfModel, SoundLib.wolf, 0.25f);
                    break;
                case ItemResourceType.Warg:
                    new Livestock(tilepos, wp, DssVar.wargModel, SoundLib.wolf, 0f);
                    break;
                case ItemResourceType.AlphaWarg:
                    new Livestock(tilepos, wp, DssVar.alphaWargModel, SoundLib.wolf, -0.25f);
                    break;

                case ItemResourceType.WildCat:
                    new Livestock(tilepos, wp, DssVar.lionModel, SoundLib.lion, 0.25f);
                    break;
                case ItemResourceType.Lion:
                    new Livestock(tilepos, wp, DssVar.lionModel, SoundLib.lion, 0f);
                    break;
                case ItemResourceType.WarLion:
                    new Livestock(tilepos, wp, DssVar.lionModel, SoundLib.lion, -0.25f);
                    break;

                case ItemResourceType.Elephant:
                    new Livestock(tilepos, wp, DssVar.elephantModel, SoundLib.elephant, 0.25f);
                    break;
                case ItemResourceType.WarElephant:
                    new Livestock(tilepos, wp, DssVar.warElephantModel, SoundLib.elephant, 0f);
                    break;
                case ItemResourceType.Oliphant:
                    new Livestock(tilepos, wp, DssVar.oliphantModel, SoundLib.elephant, -0.25f);
                    break;

                case ItemResourceType.Pheasant:
                    new Pheasant(tilepos, wp);
                    break;
            }
        }
    }
}
