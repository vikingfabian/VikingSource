using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Build
{
    class BuildControls
    {
        static readonly TerrainBuildingType[] BuildingTypes = { TerrainBuildingType.Tavern, TerrainBuildingType.WorkerHut, TerrainBuildingType.PigPen, TerrainBuildingType.HenPen };
        public SelectTileResult buildMode = SelectTileResult.None;
        public TerrainBuildingType placeBuildingType = BuildingTypes[0];

        LocalPlayer player;

        public BuildControls(LocalPlayer player) 
        { 
            this.player = player;
        }

        public void onTileSelect(SelectedSubTile selectedSubTile)
        {   
            if (buildMode == SelectTileResult.Build)
            {
                var mayBuild = selectedSubTile.MayBuild(player);
                if (mayBuild == MayBuildResult.Yes || mayBuild == MayBuildResult.Yes_ChangeCity)
                {
                    //create build order
                    player.addOrder(new BuildOrder(10, true, selectedSubTile.city, selectedSubTile.subTilePos, player.BuildControls.placeBuildingType));
                }
            }            
        }

        public void toHud(RichBoxContent content)
        {     
            content.newParagraph();
            foreach (var building in BuildingTypes)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(building.ToString()) },
                new RbAction1Arg<TerrainBuildingType>(buildingTypeClick, building),
                new RbAction1Arg<TerrainBuildingType>(buildingTooltip, building));
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

        void buildingTooltip(TerrainBuildingType buildingType)
        {
            RichBoxContent content = new RichBoxContent();

            content.h2(buildingType.ToString());

            HudLib.Description(content, LangLib.BuildingDescription(buildingType));
            CraftBlueprint blueprint = ResourceLib.Blueprint(buildingType);
            blueprint.toMenu(content);

            player.hud.tooltip.create(player, content, true);
        }
        
    }
}
