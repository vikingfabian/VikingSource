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
         
        public SelectTileResult buildMode = SelectTileResult.None;
        //public BuildOption placeBuildingType = BuildLib.BuildOptions[0];
        public BuildAndExpandType placeBuildingType = BuildAndExpandType.WorkerHuts;

        LocalPlayer player;

        public BuildControls(LocalPlayer player) 
        { 
            this.player = player;
        }

        public BuildOption placeBuildingOption()
        {
            return BuildLib.BuildOptions[(int)placeBuildingType];
        }

        public void onTileSelect(SelectedSubTile selectedSubTile)
        {   
            if (buildMode == SelectTileResult.Build)
            {
                var mayBuild = selectedSubTile.MayBuild(player);
                if (mayBuild == MayBuildResult.Yes || mayBuild == MayBuildResult.Yes_ChangeCity)
                {
                    //create build order
                    player.addOrder(new BuildOrder(10, true, selectedSubTile.city, selectedSubTile.subTilePos, placeBuildingType));
                }
            }            
        }

        public void toHud(RichBoxContent content)
        {     
            content.newParagraph();
            foreach (var opt in BuildLib.AvailableBuildTypes)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(BuildLib.BuildOptions[(int)opt].Label()) },
                new RbAction1Arg<BuildAndExpandType>(buildingTypeClick, opt),
                new RbAction1Arg<BuildAndExpandType>(buildingTooltip, opt));
                button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.Build && placeBuildingType == opt);
                content.Add(button);
                content.space();
            }
           
            content.newLine();
            //{
            //    var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(DssRef.todoLang.Build_DestroyBuilding) },
            //        new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.Destroy));
            //    button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.Destroy);
            //    content.Add(button);
            //    content.space();
            //}
            //{
            //    var button = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(DssRef.todoLang.Build_ClearTerrain) },
            //        new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.ClearTerrain));
            //    button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.ClearTerrain);
            //    content.Add(button);
            //    content.space();
            //}

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

        void buildingTypeClick(BuildAndExpandType type)
        {
            buildMode = SelectTileResult.Build;
            placeBuildingType = type;
        }

        void buildingTooltip(BuildAndExpandType type)
        {
            RichBoxContent content = new RichBoxContent();

            //content.h2(index.ToString());
            var build = BuildLib.BuildOptions[(int)type];

            HudLib.Description(content, build.Description());
            //CraftBlueprint blueprint = ResourceLib.Blueprint(index);
            build.blueprint.toMenu(content);

            player.hud.tooltip.create(player, content, true);
        }
        
    }

    
}
