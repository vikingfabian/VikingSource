using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.CharacterCreator;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.GameState.ShaderLab
{
    class ShaderLabScene : Engine.GameState
    {
        RichMenu menu;
        public ShaderLabScene()
            : base()
        {
            openMenu();

            new Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.One, TextureEffectType.Flat, SpriteName.cmdTileGrass1, Color.White);

            new Mesh(LoadedMesh.cube_repeating, new Vector3(0, -2, 0), new Vector3(10, 1, 10), TextureEffectType.Flat, SpriteName.cmdTileEmpty, Color.White);
        }
        protected override void createDrawManager()
        {
            draw = new LabDraw();
        }
        void openMenu()
        {
            if (menu == null)
            {

                var objectMenuArea = Screen.SafeArea;
                objectMenuArea.Width = (int)(Engine.Screen.IconSize * 9f);

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
                mainMenu();


            }
        }



        void mainMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Shader lab", HudLib.TitleColor_Head);


            Refresh(content);
        }

        public void Refresh(RichBoxContent content)
        {
            //openMenu();
            menu.Refresh(content);
        }
    }
}
