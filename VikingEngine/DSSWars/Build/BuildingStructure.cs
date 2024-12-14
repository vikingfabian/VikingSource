using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Build
{
    struct BuildingStructure
    {
        public int buildingLevel_logistics;
        public int WorkerHuts_count;
        
        public int Postal_count;
        public int Recruitment_count;
        public int SoldierBarracks_count;
        public int Nobelhouse_count;
        public int Tavern_count;
        public int Storehouse_count;
        public int Brewery_count;
        public int Cook_count;
        public int CoalPit_count;
        public int WorkBench_count;
        public int Smith_count;
        public int Carpenter_count;
        public int WheatFarm_count;
        public int LinenFarm_count;
        public int HempFarm_count;
        public int RapeSeedFarm_count;
        public int PigPen_count;
        public int HenPen_count;
        public int Statue_ThePlayer_count;
        public int Pavement_count;
        public int PavementFlower_count;
        public int Bank_count;
        public int CoinMinter_count;
        public int GoldDelivery_count;
        public int WoodCutter_count;
        public int StoneCutter_count;
        public int Embassy_count;
        public int WaterResovoir_count;
        public int ArcherBarracks_count;
        public int WarmashineBarracks_count;
        public int GunBarracks_count;
        public int CannonBarracks_count;
        public int KnightsBarracks_count;
        public int Smelter_count;
        public int Foundry_count;
        public int Armory_count;
        public int Chemist_count;
        public int Gunmaker_count;
        public int School_count;

        public int getCount(BuildAndExpandType type)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerHuts: return WorkerHuts_count;
                case BuildAndExpandType.Postal: return Postal_count;
                case BuildAndExpandType.Recruitment: return Recruitment_count;
                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_count;
                case BuildAndExpandType.Nobelhouse: return Nobelhouse_count;
                case BuildAndExpandType.Tavern: return Tavern_count;
                case BuildAndExpandType.Storehouse: return Storehouse_count;
                case BuildAndExpandType.Brewery: return Brewery_count;
                case BuildAndExpandType.Cook: return Cook_count;
                case BuildAndExpandType.CoalPit: return CoalPit_count;
                case BuildAndExpandType.WorkBench: return WorkBench_count;
                case BuildAndExpandType.Smith: return Smith_count;
                case BuildAndExpandType.Carpenter: return Carpenter_count;
                case BuildAndExpandType.WheatFarm: return WheatFarm_count;
                case BuildAndExpandType.LinenFarm: return LinenFarm_count;
                case BuildAndExpandType.HempFarm: return HempFarm_count;
                case BuildAndExpandType.RapeSeedFarm: return RapeSeedFarm_count;
                case BuildAndExpandType.PigPen: return PigPen_count;
                case BuildAndExpandType.HenPen: return HenPen_count;
                case BuildAndExpandType.Statue_ThePlayer: return Statue_ThePlayer_count;
                case BuildAndExpandType.Pavement: return Pavement_count;
                case BuildAndExpandType.PavementFlower: return PavementFlower_count;
                case BuildAndExpandType.Bank: return Bank_count;
                case BuildAndExpandType.CoinMinter: return CoinMinter_count;
                case BuildAndExpandType.GoldDeliveryLvl1: return GoldDelivery_count;
                case BuildAndExpandType.WoodCutter: return WoodCutter_count;
                case BuildAndExpandType.StoneCutter: return StoneCutter_count;
                case BuildAndExpandType.Embassy: return Embassy_count;
                case BuildAndExpandType.WaterResovoir: return WaterResovoir_count;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
                case BuildAndExpandType.WarmashineBarracks: return WarmashineBarracks_count;
                case BuildAndExpandType.GunBarracks: return GunBarracks_count;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
                case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_count;
                case BuildAndExpandType.Smelter: return Smelter_count;
                case BuildAndExpandType.Foundry: return Foundry_count;
                case BuildAndExpandType.Armory: return Armory_count;
                case BuildAndExpandType.Chemist: return Chemist_count;
                case BuildAndExpandType.Gunmaker: return Gunmaker_count;
                case BuildAndExpandType.School: return School_count;

                default: return 0; // Return 0 for NUM_NONE or any other undefined type
            }
        }
    }
}
