using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Resource;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars
{
    struct SoldierModelData
    {
        static readonly int MaxModelName = (int)VoxelModelName.NUM_NON;

        public ArmorLevel armor;
        public ItemResourceType manType;
        public ItemResourceType weapon;
        public ItemResourceType shield;
        public bool riding;
        //public ItemResourceType animal;
        //public ArmorLevel animalArmor;
        public VisualExperience experience;
        public SpecializationType specialization;
        public int randomVariant; //max 3
        public int profileVariant; //max 3
        public ModelType modelType;

        public SoldierModelData(
           ItemResourceType manType,
           ItemResourceType weapon,
           ItemResourceType shield,
           ArmorLevel armor,
           bool riding,
           SpecializationType specialization,
           VisualExperience experience,
           int randomVariant,
           int profileVariant)
        {
            this.armor = armor;
            this.weapon = weapon;
            this.experience = experience;
            this.randomVariant = randomVariant;
            this.profileVariant = profileVariant;
            this.manType = manType;
            this.shield = shield;
            this.riding = riding;

        }

        public override int GetHashCode()
        {
            int result = HashCode.Combine(
                manType,
                armor,
                weapon,
                shield,
                riding,
                specialization,
                experience,
                randomVariant + profileVariant * 10
                
            );

            if (result > 0)
            {
                result += MaxModelName;
            }

            return result;
        }

        public override string ToString()
        {
            return $"SoldierModelData("
                + $"Armor={armor}, "
                + $"Weapon={weapon}, "
                + $"Experience={experience}, "
                + $"RandomVariants={randomVariant}, "
                + $"ProfileVariant={profileVariant}"
                + $")";
        }
    }

    enum ArmorLevel
    { 
        None,
        Leather,
        Iron,
        Steel,
        Masterful,
        NUM
    }

    enum VisualExperience
    { 
        Fresh,
        Experienced,
        Scarred,
        Broken,
    }

    enum ModelType
    { 
        Soldier,
        Custom,
        Warmachine,
        Riding,
        Ship,
    }
}
