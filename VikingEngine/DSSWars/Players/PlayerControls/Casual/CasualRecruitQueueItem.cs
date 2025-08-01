using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.GO.NPC;

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

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)soldierType);
            w.Write((ushort)count);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ref CasualCityProfile cityProfile)
        {
            soldierType = (CasualSoldierType)r.ReadByte();
            count = r.ReadUInt16();

            switch (soldierType)
            {
                case CasualSoldierType.Guard:
                    purchaseOption = cityProfile.guard;
                    break;
                case CasualSoldierType.FolkMen:
                    purchaseOption = cityProfile.folkmen;
                    break;
                case CasualSoldierType.Seamen:
                    purchaseOption = cityProfile.shipmen;
                    break;
                case CasualSoldierType.Melee:
                    purchaseOption = cityProfile.meleeMen;
                    break;
                case CasualSoldierType.Ranged:
                    purchaseOption = cityProfile.rangedMen;
                    break;
                case CasualSoldierType.Rider:
                    purchaseOption = cityProfile.riderMen;
                    break;
                case CasualSoldierType.Siege:
                    purchaseOption = cityProfile.siegeMen;
                    break;

            }

        }

        public bool Equals(CasualRecruitQueueItem other)
        {
            return soldierType == other.soldierType;
        }

        public ConscriptProfile ConscriptProfile(City city)
        {
            //ItemResourceType armor;
            SpecializationType specialization;
            TrainingLevel training;

            switch (soldierType)
            {
                default:
                    //armor = ItemResourceType.PaddedArmor;
                    specialization = SpecializationType.None;
                    training = TrainingLevel.Basic;
                    break;

                case CasualSoldierType.Guard:
                    //armor = ItemResourceType.PaddedArmor;
                    specialization = SpecializationType.CityGuard;
                    training = TrainingLevel.Basic;
                    break;
                
                case CasualSoldierType.FolkMen:
                    //armor = ItemResourceType.NONE;
                    specialization = SpecializationType.None;
                    training = TrainingLevel.Minimal;
                    break;

                case CasualSoldierType.Seamen:
                    //armor = ItemResourceType.PaddedArmor;
                    specialization = SpecializationType.Sea;
                    training = TrainingLevel.Basic;
                    break;

                case CasualSoldierType.Siege:
                    //armor = ItemResourceType.NONE;
                    specialization = SpecializationType.Siege;
                    training = TrainingLevel.Basic;
                    break;

                case CasualSoldierType.Rider:
                    //armor = ItemResourceType.IronArmor;
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
                armorLevel = purchaseOption.armor,
                specialization= specialization,
                training = training,
            };
        }
    }
    
    
}
