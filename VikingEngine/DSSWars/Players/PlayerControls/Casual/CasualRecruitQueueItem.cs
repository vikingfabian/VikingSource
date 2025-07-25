using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.LootFest.GO.Characters;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    struct CasualRecruitQueueItem
    {
        public CasualSoldierType soldierType;
        public SoldierPurchaseOption purchaseOption;
        public int count;
        

        public CasualRecruitQueueItem(CasualSoldierType soldierType, SoldierPurchaseOption option, int count)
        {
            this.soldierType = soldierType;
            purchaseOption = option;
            this.count = count;
        }

        public bool Equals(CasualRecruitQueueItem other)
        {
            return soldierType == other.soldierType;
        }

        public ConscriptProfile ConscriptProfile(City city)
        {
            ItemResourceType armor;
            SpecializationType specialization;
            TrainingLevel training;

            switch (soldierType)
            {
                default:
                    armor = ItemResourceType.PaddedArmor;
                    specialization = SpecializationType.None;
                    training = TrainingLevel.Basic;
                    break;

                case CasualSoldierType.Guard:
                    armor = ItemResourceType.PaddedArmor;
                    specialization = SpecializationType.CityGuard;
                    training = TrainingLevel.Basic;
                    break;
                
                case CasualSoldierType.FolkMen:
                    armor = ItemResourceType.NONE;
                    specialization = SpecializationType.None;
                    training = TrainingLevel.Minimal;
                    break;

                case CasualSoldierType.Seamen:
                    armor = ItemResourceType.PaddedArmor;
                    specialization = SpecializationType.Sea;
                    training = TrainingLevel.Basic;
                    break;

                case CasualSoldierType.Siege:
                    armor = ItemResourceType.NONE;
                    specialization = SpecializationType.Siege;
                    training = TrainingLevel.Basic;
                    break;

                case CasualSoldierType.Rider:
                    armor = ItemResourceType.IronArmor;
                    specialization = SpecializationType.Field;
                    training = TrainingLevel.Skillful;
                    break;
            }

            //switch (city.Culture)
            //{ 
            //    case CityCulture.Warriors:

            //        break;
            //}

            return new ConscriptProfile()
            {
                weapon = purchaseOption.weapon,
                armorLevel = armor,
                specialization= specialization,
                training = training,
            };
        }
    }

    

    class CasualControls
    {
        
    }

    
}
