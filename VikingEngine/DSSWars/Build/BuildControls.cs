using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Build
{
    class BuildControls
    {
        static readonly TerrainBuildingType[] BuildingTypes = { TerrainBuildingType.Tavern, TerrainBuildingType.WorkerHut, TerrainBuildingType.PigPen, TerrainBuildingType.HenPen };
        public SelectTileResult buildMode = SelectTileResult.None;
        public TerrainBuildingType placeBuildingType = BuildingTypes[0];
        public BuildControls() 
        { }

        public void toHud(RichBoxContent content)
        {
            

            content.newParagraph();
            foreach (var building in BuildingTypes)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(building.ToString()) },
                new RbAction1Arg<TerrainBuildingType>(buildingTypeClick, building));
                button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.Build && placeBuildingType == building);
                content.Add(button);
                content.space();
            }
           
            content.newLine();
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(DssRef.todoLang.Build_DestroyBuilding) },
                    new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.Destroy));
                button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.Destroy);
                content.Add(button);
                content.space();
            }
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(DssRef.todoLang.Build_ClearTerrain) },
                    new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.ClearTerrain));
                button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.ClearTerrain);
                content.Add(button);
                content.space();
            }

            if (buildMode != SelectTileResult.None)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { 
                    new RichBoxSpace(),
                    new RichBoxText(DssRef.todoLang.Hud_EndSessionIcon),
                    new RichBoxSpace(),
                    },
                    new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.None));
                button.setGroupSelectionColor(HudLib.RbSettings, false);
                content.Add(button);
                content.space();
            }
        }

        void modeClick(SelectTileResult set)
        { 
            buildMode = set;
        }
        void buildingTypeClick(TerrainBuildingType buildingType)
        {
            buildMode = SelectTileResult.Build;
            placeBuildingType = buildingType;
        }

        
    }

    //enum BuildMode
    //{
    //    None,
    //    Build,
    //    ClearTerrain,
    //    Destroy,
    //}
}
