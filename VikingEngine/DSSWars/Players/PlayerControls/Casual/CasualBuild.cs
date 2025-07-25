using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    enum CasualBuildType
    {
        WorkerHut,
        Barracks,
        ResearchCenter,
        NUM
    }

    class CasualBuildOption
    { 
        public CasualBuildType Type;
        public string Name;
        public SpriteName icon;
        public int price;
        public int buildtime_sec;

        public bool allowMultiBuild;
    }


    static class CasualBuild
    {
        public static CasualBuildOption[] CasualBuildOptionList;

        public static void Init()
        {
            CasualBuildOptionList = new CasualBuildOption[(int)CasualBuildType.NUM];

            add(new CasualBuildOption 
            {
                Type = CasualBuildType.WorkerHut,
                Name = DssRef.lang.BuildingType_WorkerHut,
                icon = SpriteName.WarsBuild_WorkerHuts,
                price = 200,
                buildtime_sec = (int)DssConst.WorkTime_Building_Default,
                allowMultiBuild = true
            });
            add(new CasualBuildOption 
            {
                Type = CasualBuildType.Barracks,
                Name = DssRef.lang.BuildingType_Barracks,
                icon = SpriteName.WarsBuild_Barracks,
                price = 300,
                buildtime_sec = (int)DssConst.WorkTime_Building_Default * 2,
                allowMultiBuild = true
            });
            add(new CasualBuildOption
            {
                Type = CasualBuildType.ResearchCenter,
                Name = DssRef.lang.BuildingType_ReseachCenter,
                icon = SpriteName.WarsBuild_ResearchCenter,
                price = 500,
                buildtime_sec = (int)DssConst.WorkTime_Building_Large,
                allowMultiBuild = false
            });
            
            void add(CasualBuildOption buildOption)
            {
                CasualBuildOptionList[(int)buildOption.Type] = buildOption;
            }
        }

        public static CasualBuildOption Get(CasualBuildType type)
        { 
            return CasualBuildOptionList[(int)type];
        }

        public static void ToHud(LocalPlayer player, RichBoxContent content, City city)
        {
            // Define which buildings to show in the HUD
            List<CasualBuildType> available = new List<CasualBuildType>
            {
                CasualBuildType.WorkerHut,
                CasualBuildType.Barracks,
                //CasualBuildType.ResearchCenter
            };

            foreach (var buildType in available)
            {
                CasualBuildOption option = CasualBuildOptionList[(int)buildType];
                if (option != null)
                {
                    AddBuildButton(option);
                }
            }

            city.GetCasualProgress().BuildToHud(player, city, content);

            void AddBuildButton(CasualBuildOption option)
            {
                content.newLine();
                bool canAfford = player.faction.hasGold(option.price, city);

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
                {
                    new RbImage(option.icon),
                    new RbSpace(),
                    new RbText(option.Name),
                    new RbSpace(2),
                    new RbImage(SpriteName.rtsMoney),
                    new RbText(option.price.ToString(), canAfford ? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark)
                }, new RbAction2Arg<CasualBuildType, int>(city.CasualBuild, option.Type, 1)));
            }
        }



    }
}
