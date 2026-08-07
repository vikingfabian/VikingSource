using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.MapEditor;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    class MapEditor2Display
    {
        RichMenu menu;
        MapEditor2_Scene state;
        public Vector2 topRight;
        public ImageGroup2D loadingDisplay;
        
        public MapEditor2Display(MapEditor2_Scene state)
        { 
            this.state = state;
            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;

            topRight = area.RightTop;
            topRight.X += Engine.Screen.BorderWidth;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top2, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top2_Back);


            TextG loadingText = new TextG(LoadedFont.Regular, Engine.Screen.Area.PercentToPosition(0.5f, 0.2f), Screen.TextSizeV2 * 2f, Align.CenterAll, DssRef.lang.Hud_Loading, Color.White, ImageLayers.Top0_Front, true);
            var loadArea = loadingText.GetArea();
            loadArea.Size.X += Screen.IconSize * 0.5f;
            Graphics.Image loadingSpinner = new Image(SpriteName.WhiteArea, loadArea.RightCenter, Screen.IconSizeV2 * 0.6f, ImageLayers.Top1_Front, true);
            loadArea.Size.X += Screen.IconSize * 0.5f;
            loadArea.AddRadius(Screen.IconSize * 0.5f);

            Graphics.Image loadingBg = new Image(SpriteName.WhiteArea, loadArea.Position, loadArea.Size, ImageLayers.Top1_Back, false);
            loadingBg.ColorAndAlpha(Color.Black, 0.2f);

            new Motion2d(MotionType.ROTATE, loadingSpinner, new Vector2(MathHelper.Tau * 0.5f), MotionRepeate.Loop, 1000, true);

            loadingDisplay = new ImageGroup2D(new List<AbsDraw2D> { loadingText, loadingSpinner, loadingBg });
            loadingDisplay.Hide();

            refreshMenu();
        }

        public void update(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            if (menu.needRefresh)
            {
                refreshMenu();
            }
        }

        public void refreshMenu()
        {
            if (state.iconState)
            {
                iconMen();
            }
        }

        void iconMen()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Map 2.0 - Icon editor", HudLib.TitleColor_Head);

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) }, 
                       new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.AllTerrain)));
            menu.Refresh(content);
        }
    }

}
