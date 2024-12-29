using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Build
{
    struct BuildingPosition
    {
        public IntVector2 WorkerHuts_pos;
        public IntVector2 Postal_pos;
        public IntVector2 Recruitment_pos;
        public IntVector2 SoldierBarracks_pos;
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
        public IntVector2 WarmashineBarracks_pos;
        public IntVector2 GunBarracks_pos;
        public IntVector2 CannonBarracks_pos;
        public IntVector2 KnightsBarracks_pos;
        public IntVector2 Smelter_pos;
        public IntVector2 Foundry_pos;
        public IntVector2 Armory_pos;
        public IntVector2 Chemist_pos;
        public IntVector2 Gunmaker_pos;
        public IntVector2 School_pos;

        public IntVector2 getPos(BuildAndExpandType type)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerHuts: return WorkerHuts_pos;
                case BuildAndExpandType.Postal: return Postal_pos;
                case BuildAndExpandType.Recruitment: return Recruitment_pos;
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
                case BuildAndExpandType.WheatFarm: return WheatFarm_pos;
                case BuildAndExpandType.LinenFarm: return LinenFarm_pos;
                case BuildAndExpandType.HempFarm: return HempFarm_pos;
                case BuildAndExpandType.RapeSeedFarm: return RapeSeedFarm_pos;
                case BuildAndExpandType.PigPen: return PigPen_pos;
                case BuildAndExpandType.HenPen: return HenPen_pos;
                case BuildAndExpandType.Statue_ThePlayer: return Statue_ThePlayer_pos;
                case BuildAndExpandType.Pavement: return Pavement_pos;
                case BuildAndExpandType.PavementFlower: return PavementFlower_pos;
                case BuildAndExpandType.Bank: return Bank_pos;
                case BuildAndExpandType.CoinMinter: return CoinMinter_pos;
                case BuildAndExpandType.GoldDeliveryLvl1: return GoldDelivery_pos;
                case BuildAndExpandType.WoodCutter: return WoodCutter_pos;
                case BuildAndExpandType.StoneCutter: return StoneCutter_pos;
                case BuildAndExpandType.Embassy: return Embassy_pos;
                case BuildAndExpandType.WaterResovoir: return WaterResovoir_pos;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_pos;
                case BuildAndExpandType.WarmashineBarracks: return WarmashineBarracks_pos;
                case BuildAndExpandType.GunBarracks: return GunBarracks_pos;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_pos;
                case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_pos;
                case BuildAndExpandType.Smelter: return Smelter_pos;
                case BuildAndExpandType.Foundry: return Foundry_pos;
                case BuildAndExpandType.Armory: return Armory_pos;
                case BuildAndExpandType.Chemist: return Chemist_pos;
                case BuildAndExpandType.Gunmaker: return Gunmaker_pos;
                case BuildAndExpandType.School: return School_pos;

                default: throw new NotImplementedException(); // Return 0 for NUM_NONE or any other undefined type
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

        public void miningOverviewHud(RichBoxContent content, LocalPlayer player)
        {
            content.newLine();

            content.Add(new RichBoxImage(SpriteName.WarsWorkMine));
            content.space();

            int totalCount = 0;

            mine(mineCount_coal, ItemResourceType.Coal);
            mine(mineCount_bogIron, ItemResourceType.BogIron);
            mine(mineCount_iron, ItemResourceType.Iron_G);
            mine(mineCount_tin, ItemResourceType.Tin);
            mine(mineCount_copper, ItemResourceType.Copper);
            mine(mineCount_lead, ItemResourceType.Lead);
            mine(mineCount_silver, ItemResourceType.Silver);
            mine(mineCount_gold, ItemResourceType.Gold);
            mine(mineCount_mithril, ItemResourceType.Mithril);
            mine(mineCount_sulfur, ItemResourceType.Sulfur);
            

            if (totalCount == 0)
            {
                content.Add(new RichBoxText(DssRef.lang.Hud_EmptyList));
            }


            void mine(int count, ItemResourceType resource)
            {
                totalCount += count;
                if (count > 0)
                {
                    SpriteName icon = ResourceLib.Icon(resource);
                    string resourceName = LangLib.Item(resource);
                    var infoContent = new RichBoxContent();

                    infoContent.Add(new RichBoxImage(icon));
                    infoContent.space();
                    var countText = new RichBoxText(count.ToString());
                    countText.overrideColor = Color.White;
                    infoContent.Add(countText); 

                    var infoButton = new RichboxButton(infoContent, null, new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();

                        content.Add(new RichBoxImage(icon));
                        content.space();
                        var mineString = string.Format(DssRef.lang.BuildingType_ResourceMine, resourceName);
                        content.Add(new RichBoxText(TextLib.LargeFirstLetter( string.Format(DssRef.lang.Language_XCountIsY, mineString, count))));


                        player.hud.tooltip.create(player, content, true);
                    }));

                    infoButton.overrideBgColor = HudLib.InfoYellow_BG;
                    content.Add(infoButton);
                    content.space();
                }
            }
        }
    }

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

        public int getBarracksCount(BuildAndExpandType type)
        {
            switch (type)
            {
                
                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_count;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
                case BuildAndExpandType.WarmashineBarracks: return WarmashineBarracks_count;
                case BuildAndExpandType.GunBarracks: return GunBarracks_count;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
                case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_count;
               

                default: return 0; // Return 0 for NUM_NONE or any other undefined type
            }
        }
    }
}
