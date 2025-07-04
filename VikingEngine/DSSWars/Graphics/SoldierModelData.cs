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
        public ItemResourceType weapon; //value range 0 - 150
        public VisualExperience experience;
        public SpecializationType specialization;
        public int randomVariant; //max 3
        public int profileVariant; //max 3
        public ModelType modelType;

        public SoldierModelData(
           ArmorLevel armor,
           ItemResourceType weapon,
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
        }

        public override int GetHashCode()
        {
            int result = HashCode.Combine(
                armor,
                weapon,
                specialization,
                experience,
                randomVariant,
                profileVariant
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
        Warmashine,
        Riding,
    }
}
