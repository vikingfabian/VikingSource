using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Build
{
    struct BuildingPosition
    {
        public IntVector2 WorkerHuts_pos;
        public IntVector2 ServiceHouse_pos;
        public IntVector2 Postal_pos;
        public IntVector2 Recruitment_pos;
        public IntVector2 SoldierBarracks_pos;
        public IntVector2 ImmigrationTent_pos;
        public IntVector2 Nobelhouse_pos;
        public IntVector2 Tavern_pos;
        public IntVector2 Storehouse_pos;
        public IntVector2 Brewery_pos;
        public IntVector2 Cook_pos;
        public IntVector2 CoalPit_pos;
        public IntVector2 WorkBench_pos;
        public IntVector2 Smith_pos;
        public IntVector2 Carpenter_pos;
        public IntVector2 WheatFarm_pos;
        public IntVector2 LinenFarm_pos;
        public IntVector2 HempFarm_pos;
        public IntVector2 RapeSeedFarm_pos;
        public IntVector2 PigPen_pos;
        public IntVector2 HenPen_pos;
        public IntVector2 Statue_ThePlayer_pos;
        public IntVector2 Pavement_pos;
        public IntVector2 PavementFlower_pos;
        public IntVector2 Bank_pos;
        public IntVector2 CoinMinter_pos;
        public IntVector2 GoldDelivery_pos;
        public IntVector2 WoodCutter_pos;
        public IntVector2 StoneCutter_pos;
        public IntVector2 Embassy_pos;
        public IntVector2 WaterResovoir_pos;
        public IntVector2 ArcherBarracks_pos;
        public IntVector2 WarmachineBarracks_pos;
        public IntVector2 GunBarracks_pos;
        public IntVector2 CannonBarracks_pos;
        public IntVector2 KnightsBarracks_pos;
        public IntVector2 Smelter_pos;
        public IntVector2 Foundry_pos;
        public IntVector2 Armory_pos;
        public IntVector2 Chemist_pos;
        public IntVector2 Gunmaker_pos;
        public IntVector2 School_pos;
        public IntVector2 ResearchCenter_pos;
        public IntVector2 BookPress_pos;

        public IntVector2 getPos(BuildAndExpandType type)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerHut:
                case BuildAndExpandType.WorkerHutLarge:
                    return WorkerHuts_pos;

                case BuildAndExpandType.ServiceHouse_Small:
                case BuildAndExpandType.ServiceHouse_Large:
                    return ServiceHouse_pos;

                case BuildAndExpandType.Postal:
                case BuildAndExpandType.PostalLevel2:
                case BuildAndExpandType.PostalLevel3:
                    return Postal_pos;

                case BuildAndExpandType.Recruitment:
                case BuildAndExpandType.RecruitmentLevel2:
                case BuildAndExpandType.RecruitmentLevel3:
                    return Recruitment_pos;

                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_pos;
                case BuildAndExpandType.Nobelhouse: return Nobelhouse_pos;
                case BuildAndExpandType.Tavern: return Tavern_pos;
                case BuildAndExpandType.Storehouse: return Storehouse_pos;
                case BuildAndExpandType.Brewery: return Brewery_pos;
                case BuildAndExpandType.Cook: return Cook_pos;
                case BuildAndExpandType.CoalPit: return CoalPit_pos;
                case BuildAndExpandType.WorkBench: return WorkBench_pos;
                case BuildAndExpandType.Smith: return Smith_pos;
                case BuildAndExpandType.Carpenter: return Carpenter_pos;

                case BuildAndExpandType.WheatFarm:
                case BuildAndExpandType.WheatFarmUpgraded:
                    return WheatFarm_pos;

                case BuildAndExpandType.LinenFarm:
                case BuildAndExpandType.LinenFarmUpgraded:
                    return LinenFarm_pos;

                case BuildAndExpandType.HempFarm:
                case BuildAndExpandType.HempFarmUpgraded:
                    return HempFarm_pos;

                case BuildAndExpandType.RapeSeedFarm:
                case BuildAndExpandType.RapeSeedFarmUpgraded:
                    return RapeSeedFarm_pos;

                case BuildAndExpandType.PigPen: return PigPen_pos;
                case BuildAndExpandType.HenPen: return HenPen_pos;
                case BuildAndExpandType.Statue_ThePlayer: return Statue_ThePlayer_pos;
                case BuildAndExpandType.Pavement: return Pavement_pos;
                case BuildAndExpandType.PavementFlower: return PavementFlower_pos;
                case BuildAndExpandType.Bank: return Bank_pos;
                case BuildAndExpandType.CoinMinter: return CoinMinter_pos;

                case BuildAndExpandType.GoldDeliveryLvl1:
                case BuildAndExpandType.GoldDeliveryLvl2:
                case BuildAndExpandType.GoldDeliveryLvl3:
                    return GoldDelivery_pos;

                case BuildAndExpandType.WoodCutter: return WoodCutter_pos;
                case BuildAndExpandType.StoneCutter: return StoneCutter_pos;
                case BuildAndExpandType.Embassy: return Embassy_pos;
                case BuildAndExpandType.WaterResovoir: return WaterResovoir_pos;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_pos;
                case BuildAndExpandType.WarmachineBarracks: return WarmachineBarracks_pos;
                case BuildAndExpandType.GunBarracks: return GunBarracks_pos;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_pos;
                case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_pos;
                case BuildAndExpandType.Smelter: return Smelter_pos;
                case BuildAndExpandType.Foundry: return Foundry_pos;
                case BuildAndExpandType.Armory: return Armory_pos;
                case BuildAndExpandType.Chemist: return Chemist_pos;
                case BuildAndExpandType.Gunmaker: return Gunmaker_pos;
                case BuildAndExpandType.School: return School_pos;

                case BuildAndExpandType.ImmigrationTent: return ImmigrationTent_pos;

                default:
                    throw new NotImplementedException($"getPos() not implemented for {type}");
            }
        }
    }

    struct TerrainStructure
    {
        public int mineCount_bogIron;

        public int mineCount_iron;
        public int mineCount_tin;
        public int mineCount_copper;
        public int mineCount_lead;
        public int mineCount_silver;
        public int mineCount_gold;
        public int mineCount_mithril;
        public int mineCount_sulfur;
        public int mineCount_coal;

        public int resourceCount_stone;
        public int resourceCount_wood;


        public void miningOverviewHud(RichBoxContent content)
        {
            content.newLine();

            content.Add(new RbImage(SpriteName.WarsWorkMine));
            content.space();

            int totalCount = 0;

            naturalResource(content, resourceCount_wood, ItemResourceType.Wood_Group, ref totalCount);
            naturalResource(content, resourceCount_stone, ItemResourceType.Stone_G, ref totalCount);
            mine(content, mineCount_coal, ItemResourceType.Coal, ref totalCount);
            mine(content, mineCount_bogIron, ItemResourceType.BogIron, ref totalCount);
            mine(content, mineCount_iron, ItemResourceType.Iron_G, ref totalCount);
            mine(content, mineCount_tin, ItemResourceType.Tin, ref totalCount);
            mine(content, mineCount_copper, ItemResourceType.Copper, ref totalCount);
            mine(content, mineCount_lead, ItemResourceType.Lead, ref totalCount);
            mine(content, mineCount_silver, ItemResourceType.Silver, ref totalCount);
            mine(content, mineCount_gold, ItemResourceType.Gold, ref totalCount);
            mine(content, mineCount_mithril, ItemResourceType.Mithril, ref totalCount);
            mine(content, mineCount_sulfur, ItemResourceType.Sulfur, ref totalCount);


            if (totalCount == 0)
            {
                content.Add(new RbText(DssRef.lang.Hud_EmptyList));
            }
            
        }

        public void naturalResource(RichBoxContent content, int count, ItemResourceType resource, ref int totalCount)
        {
            resourceHoverButton(content, count, resource, DssRef.lang.Work_GatherXResource, SpriteName.WarsWorkCollect, ref totalCount);
        }

        public void mine(RichBoxContent content, int count, ItemResourceType resource, ref int totalCount)
        {
            //totalCount += count;
            //if (count > 0)
            //{
            //    SpriteName icon = ResourceLib.Icon(resource);
            //    string resourceName = LangLib.Item(resource);
            //    var infoContent = new RichBoxContent();

            //    infoContent.Add(new RbOverlapImage(new RbImage(icon), SpriteName.WarsWorkMine, VectorExt.V2FromX(-0.2f), 0.8f));
            //    infoContent.space();
            //    var countText = new RbText(count.ToString());
            //    countText.overrideColor = Color.White;
            //    infoContent.Add(countText);

            //    var infoButton = new ArtButton(RbButtonStyle.HoverArea, infoContent, null,
            //        new RbTooltip((RichBoxContent content, object tag) =>
            //        {
            //            content.Add(new RbOverlapImage(new RbImage(icon), SpriteName.WarsWorkMine, Vector2.Zero, 0.8f));
            //            content.space();
            //            var mineString = string.Format(DssRef.lang.BuildingType_ResourceMine, resourceName);
            //            content.Add(new RbText(TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XCountIsY, mineString, count))));

            //        }));

            //    content.Add(infoButton);
            //}
            resourceHoverButton(content, count, resource, DssRef.lang.BuildingType_ResourceMine, SpriteName.WarsWorkMine, ref totalCount);
        }

        public void resourceHoverButton(RichBoxContent content, int count, ItemResourceType resource, string categoryName, SpriteName workIcon, ref int totalCount)
        {
            totalCount += count;
            if (count > 0)
            {
                SpriteName icon = ResourceLib.Icon(resource);
                string resourceName = LangLib.Item(resource);
                var infoContent = new RichBoxContent();

                infoContent.Add(new RbOverlapImage(new RbImage(icon), workIcon, VectorExt.V2FromX(-0.2f), 0.8f));
                infoContent.space();
                var countText = new RbText(count.ToString());
                countText.overrideColor = Color.White;
                infoContent.Add(countText);

                var infoButton = new ArtButton(RbButtonStyle.HoverArea, infoContent, null,
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.Add(new RbOverlapImage(new RbImage(icon), workIcon, Vector2.Zero, 0.8f));
                        content.space();
                        var mineString = string.Format(categoryName, resourceName);
                        content.Add(new RbText(TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XCountIsY, mineString, count))));

                    }));

                content.Add(infoButton);
            }
        }
    }

    struct BuildingStructure
    {
        public int buildingLevel_logistics;
        public int WorkerHuts_count;
        public int WorkerHuts_Large_count;
        public int ServiceMenHouse_count;
        public int ServiceMenHouse_Large_count;
        public int GuardOffice_count;
        public int GuardOffice_Large_count;
        public int ImmigrationTent_count;


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
        public int WarmachineBarracks_count;
        public int GunBarracks_count;
        public int CannonBarracks_count;
        public int KnightsBarracks_count;
        public int Smelter_count;
        public int Foundry_count;
        public int Armory_count;
        public int Chemist_count;
        public int Gunmaker_count;
        public int School_count;
        public int ResearchCenter_count;
        public int BookPress_count;
        

        public int getCount(BuildAndExpandType type)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerHut: return WorkerHuts_count;
                case BuildAndExpandType.WorkerHutLarge: return WorkerHuts_Large_count;

                case BuildAndExpandType.ServiceHouse_Small: return ServiceMenHouse_count;
                case BuildAndExpandType.ServiceHouse_Large: return ServiceMenHouse_Large_count;

                case BuildAndExpandType.Postal: return Postal_count;
                case BuildAndExpandType.PostalLevel2: return Postal_count;
                case BuildAndExpandType.PostalLevel3: return Postal_count;

                case BuildAndExpandType.Recruitment: return Recruitment_count;
                case BuildAndExpandType.RecruitmentLevel2: return Recruitment_count;
                case BuildAndExpandType.RecruitmentLevel3: return Recruitment_count;

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
                case BuildAndExpandType.WheatFarmUpgraded: return WheatFarm_count;
                case BuildAndExpandType.LinenFarm: return LinenFarm_count;
                case BuildAndExpandType.LinenFarmUpgraded: return LinenFarm_count;
                case BuildAndExpandType.HempFarm: return HempFarm_count;
                case BuildAndExpandType.HempFarmUpgraded: return HempFarm_count;
                case BuildAndExpandType.RapeSeedFarm: return RapeSeedFarm_count;
                case BuildAndExpandType.RapeSeedFarmUpgraded: return RapeSeedFarm_count;

                case BuildAndExpandType.PigPen: return PigPen_count;
                case BuildAndExpandType.HenPen: return HenPen_count;
                case BuildAndExpandType.Statue_ThePlayer: return Statue_ThePlayer_count;
                case BuildAndExpandType.Pavement: return Pavement_count;
                case BuildAndExpandType.PavementFlower: return PavementFlower_count;
                case BuildAndExpandType.Bank: return Bank_count;
                case BuildAndExpandType.CoinMinter: return CoinMinter_count;

                case BuildAndExpandType.GoldDeliveryLvl1: return GoldDelivery_count;
                case BuildAndExpandType.GoldDeliveryLvl2: return GoldDelivery_count;
                case BuildAndExpandType.GoldDeliveryLvl3: return GoldDelivery_count;

                case BuildAndExpandType.WoodCutter: return WoodCutter_count;
                case BuildAndExpandType.StoneCutter: return StoneCutter_count;
                case BuildAndExpandType.Embassy: return Embassy_count;
                case BuildAndExpandType.WaterResovoir: return WaterResovoir_count;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
                case BuildAndExpandType.WarmachineBarracks: return WarmachineBarracks_count;
                case BuildAndExpandType.GunBarracks: return GunBarracks_count;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
                case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_count;
                case BuildAndExpandType.Smelter: return Smelter_count;
                case BuildAndExpandType.Foundry: return Foundry_count;
                case BuildAndExpandType.Armory: return Armory_count;
                case BuildAndExpandType.Chemist: return Chemist_count;
                case BuildAndExpandType.Gunmaker: return Gunmaker_count;
                case BuildAndExpandType.School: return School_count;
                case BuildAndExpandType.ImmigrationTent: return ImmigrationTent_count;

                default: return 0; // NUM_NONE or any untracked type
            }
        }

        public void addCount(BuildAndExpandType type, int add)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerHut: WorkerHuts_count += add; break;
                case BuildAndExpandType.WorkerHutLarge: WorkerHuts_Large_count += add; break;

                case BuildAndExpandType.ServiceHouse_Small: ServiceMenHouse_count += add; break;
                case BuildAndExpandType.ServiceHouse_Large: ServiceMenHouse_Large_count += add; break;

                case BuildAndExpandType.Postal:
                case BuildAndExpandType.PostalLevel2:
                case BuildAndExpandType.PostalLevel3:
                    Postal_count += add; break;

                case BuildAndExpandType.Recruitment:
                case BuildAndExpandType.RecruitmentLevel2:
                case BuildAndExpandType.RecruitmentLevel3:
                    Recruitment_count += add; break;

                case BuildAndExpandType.SoldierBarracks: SoldierBarracks_count += add; break;
                case BuildAndExpandType.Nobelhouse: Nobelhouse_count += add; break;
                case BuildAndExpandType.Tavern: Tavern_count += add; break;
                case BuildAndExpandType.Storehouse: Storehouse_count += add; break;
                case BuildAndExpandType.Brewery: Brewery_count += add; break;
                case BuildAndExpandType.Cook: Cook_count += add; break;
                case BuildAndExpandType.CoalPit: CoalPit_count += add; break;
                case BuildAndExpandType.WorkBench: WorkBench_count += add; break;
                case BuildAndExpandType.Smith: Smith_count += add; break;
                case BuildAndExpandType.Carpenter: Carpenter_count += add; break;

                case BuildAndExpandType.WheatFarm:
                case BuildAndExpandType.WheatFarmUpgraded:
                    WheatFarm_count += add; break;

                case BuildAndExpandType.LinenFarm:
                case BuildAndExpandType.LinenFarmUpgraded:
                    LinenFarm_count += add; break;

                case BuildAndExpandType.HempFarm:
                case BuildAndExpandType.HempFarmUpgraded:
                    HempFarm_count += add; break;

                case BuildAndExpandType.RapeSeedFarm:
                case BuildAndExpandType.RapeSeedFarmUpgraded:
                    RapeSeedFarm_count += add; break;

                case BuildAndExpandType.PigPen: PigPen_count += add; break;
                case BuildAndExpandType.HenPen: HenPen_count += add; break;
                case BuildAndExpandType.Statue_ThePlayer: Statue_ThePlayer_count += add; break;
                case BuildAndExpandType.Pavement: Pavement_count += add; break;
                case BuildAndExpandType.PavementFlower: PavementFlower_count += add; break;
                case BuildAndExpandType.Bank: Bank_count += add; break;
                case BuildAndExpandType.CoinMinter: CoinMinter_count += add; break;

                case BuildAndExpandType.GoldDeliveryLvl1:
                case BuildAndExpandType.GoldDeliveryLvl2:
                case BuildAndExpandType.GoldDeliveryLvl3:
                    GoldDelivery_count += add; break;

                case BuildAndExpandType.WoodCutter: WoodCutter_count += add; break;
                case BuildAndExpandType.StoneCutter: StoneCutter_count += add; break;
                case BuildAndExpandType.Embassy: Embassy_count += add; break;
                case BuildAndExpandType.WaterResovoir: WaterResovoir_count += add; break;
                case BuildAndExpandType.ArcherBarracks: ArcherBarracks_count += add; break;
                case BuildAndExpandType.WarmachineBarracks: WarmachineBarracks_count += add; break;
                case BuildAndExpandType.GunBarracks: GunBarracks_count += add; break;
                case BuildAndExpandType.CannonBarracks: CannonBarracks_count += add; break;
                case BuildAndExpandType.KnightsBarracks: KnightsBarracks_count += add; break;
                case BuildAndExpandType.Smelter: Smelter_count += add; break;
                case BuildAndExpandType.Foundry: Foundry_count += add; break;
                case BuildAndExpandType.Armory: Armory_count += add; break;
                case BuildAndExpandType.Chemist: Chemist_count += add; break;
                case BuildAndExpandType.Gunmaker: Gunmaker_count += add; break;
                case BuildAndExpandType.School: School_count += add; break;
                case BuildAndExpandType.ImmigrationTent: ImmigrationTent_count += add; break;

                default: break; // For NUM_NONE or any untracked type
            }
        }


        //public int getCount(BuildAndExpandType type)
        //{
        //    switch (type)
        //    {
        //        case BuildAndExpandType.WorkerHut: return WorkerHuts_count;
        //        case BuildAndExpandType.WorkerHutLarge: return WorkerHuts_Large_count;

        //        case BuildAndExpandType.ServiceHouse_Small: return ServiceMenHouse_count;
        //        case BuildAndExpandType.ServiceHouse_Large: return ServiceMenHouse_Large_count;

        //        case BuildAndExpandType.Postal: return Postal_count;
        //        case BuildAndExpandType.Recruitment: return Recruitment_count;
        //        case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_count;
        //        case BuildAndExpandType.Nobelhouse: return Nobelhouse_count;
        //        case BuildAndExpandType.Tavern: return Tavern_count;
        //        case BuildAndExpandType.Storehouse: return Storehouse_count;
        //        case BuildAndExpandType.Brewery: return Brewery_count;
        //        case BuildAndExpandType.Cook: return Cook_count;
        //        case BuildAndExpandType.CoalPit: return CoalPit_count;
        //        case BuildAndExpandType.WorkBench: return WorkBench_count;
        //        case BuildAndExpandType.Smith: return Smith_count;
        //        case BuildAndExpandType.Carpenter: return Carpenter_count;
        //        case BuildAndExpandType.WheatFarm: return WheatFarm_count;
        //        case BuildAndExpandType.LinenFarm: return LinenFarm_count;
        //        case BuildAndExpandType.HempFarm: return HempFarm_count;
        //        case BuildAndExpandType.RapeSeedFarm: return RapeSeedFarm_count;
        //        case BuildAndExpandType.PigPen: return PigPen_count;
        //        case BuildAndExpandType.HenPen: return HenPen_count;
        //        case BuildAndExpandType.Statue_ThePlayer: return Statue_ThePlayer_count;
        //        case BuildAndExpandType.Pavement: return Pavement_count;
        //        case BuildAndExpandType.PavementFlower: return PavementFlower_count;
        //        case BuildAndExpandType.Bank: return Bank_count;
        //        case BuildAndExpandType.CoinMinter: return CoinMinter_count;
        //        case BuildAndExpandType.GoldDeliveryLvl1: return GoldDelivery_count;
        //        case BuildAndExpandType.WoodCutter: return WoodCutter_count;
        //        case BuildAndExpandType.StoneCutter: return StoneCutter_count;
        //        case BuildAndExpandType.Embassy: return Embassy_count;
        //        case BuildAndExpandType.WaterResovoir: return WaterResovoir_count;
        //        case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
        //        case BuildAndExpandType.WarmashineBarracks: return WarmashineBarracks_count;
        //        case BuildAndExpandType.GunBarracks: return GunBarracks_count;
        //        case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
        //        case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_count;
        //        case BuildAndExpandType.Smelter: return Smelter_count;
        //        case BuildAndExpandType.Foundry: return Foundry_count;
        //        case BuildAndExpandType.Armory: return Armory_count;
        //        case BuildAndExpandType.Chemist: return Chemist_count;
        //        case BuildAndExpandType.Gunmaker: return Gunmaker_count;
        //        case BuildAndExpandType.School: return School_count;

        //        default: return 0; // Return 0 for NUM_NONE or any other undefined type
        //    }
        //}

        public int getBarracksCount(BuildAndExpandType type)
        {
            switch (type)
            {
                
                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_count;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
                case BuildAndExpandType.WarmachineBarracks: return WarmachineBarracks_count;
                case BuildAndExpandType.GunBarracks: return GunBarracks_count;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
                case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_count;
               

                default: return 0; // Return 0 for NUM_NONE or any other undefined type
            }
        }
    }
}
