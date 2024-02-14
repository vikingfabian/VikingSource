using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    abstract class AbsPet : HqUnitData
    {
        public AbsUnitProperty petTargetProperty;

        public AbsPet()
            :base()
        {
            HasHealth = false;
            startHealth = 1;
        }
    }

    class PetCat : AbsPet
    {
        public PetCat()
            :base()
        {
            move = 5;
            modelSettings.image = SpriteName.hqPetCat;
            modelSettings.modelScale = 0.8f;
            modelSettings.facingRight = true;

            petTargetProperty = new UnitAction.Spotter();
            properties.set(new Pet(), petTargetProperty);
        }
        
        public override HqUnitType Type => HqUnitType.PetCat;
        public override string Name => "Pet Cat";
    }

    class PetDog : AbsPet
    {
        public PetDog()
            : base()
        {
            move = 5;
            modelSettings.image = SpriteName.hqPetDog;
            modelSettings.modelScale = 0.85f;
            modelSettings.facingRight = true;

            petTargetProperty = new Scratchy();
            properties.set(new Pet(), petTargetProperty);
        }

        public override HqUnitType Type => HqUnitType.PetDog;
        public override string Name => "Pet Dog";
    }

    
}
