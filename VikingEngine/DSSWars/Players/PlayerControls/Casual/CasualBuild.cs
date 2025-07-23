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
    }

    static class CasualBuild
    {

        //public static List<BuildAndExpandType> AvailableBuildTypes_Casual(City city)
        //{
        //    List<BuildAndExpandType> list = new List<BuildAndExpandType>(8);

        //    list.Add(BuildAndExpandType.WorkerHut);
        //    list.Add(BuildAndExpandType.SoldierBarracks);
        //    list.Add(BuildAndExpandType.ResearchCenter);

        //    return list;
        //}

        public static void ToHud(LocalPlayer player, RichBoxContent content, City city)
        {
            purchaseButton(CasualBuildType.WorkerHut, SpriteName.WarsBuild_WorkerHuts, DssRef.lang.BuildingType_WorkerHut, 200, true);
            purchaseButton(CasualBuildType.Barracks, SpriteName.WarsBuild_Barracks, DssRef.lang.BuildingType_Barracks, 300, true);
            purchaseButton(CasualBuildType.WorkerHut, SpriteName.WarsBuild_ResearchCenter, DssRef.lang.BuildingType_ReseachCenter, 500, false);

            void purchaseButton(CasualBuildType buildType, SpriteName icon, string caption, int price, bool multiBuild)
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
                {
                    new RbImage(icon),
                    new RbSpace(),
                    new RbText(caption),
                    new RbSpace(2),
                    new RbImage(SpriteName.rtsMoney),
                    new RbText(price.ToString(), player.faction.hasGold(price, city)? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark)
                }, new RbAction3Arg<CasualBuildType, int, int>(city.CasualBuild, buildType, price, 1)));
            }

            
        }

        
    }
}
