using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DSSWars.GameState.CharacterCreator;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.GameState.ShaderLab
{
    class ShaderLabScene : Engine.GameState
    {
        RichMenu menu;
        Mesh upNDown, movable;
        public ShaderLabScene()
            : base()
        {
            openMenu();

            upNDown = new Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.One, TextureEffectType.Flat, SpriteName.TestTexture, Color.White);

            movable = new Mesh(LoadedMesh.cube_repeating, new Vector3(-2, 2, -4), Vector3.One, TextureEffectType.Flat, SpriteName.TestTexture, Color.White);

            Graphics.VoxelModel master = DssRef.models.voxelModels[ LootFest.VoxelModelName.city_mine];
            Graphics.VoxelModelInstance instance = new VoxelModelInstance(master);
            instance.scale = VectorExt.V3(master.OneBlockScale / master.gridSideLength) * 2f;
            instance.position = new Vector3(-2, 1, -1);

            new Mesh(LoadedMesh.cube_repeating, new Vector3(0, -2, 0), new Vector3(10, 1, 10), TextureEffectType.Flat, SpriteName.TestTexture, Color.White);
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
        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            movable.position += VectorExt.V2toV3XZ(Ref.gamesett.keyboardMap.move.directionAndTime * 0.01f);

            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageDown))
            {
                upNDown.Y -= time * 0.01f;
            }
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.PageUp))
            {
                upNDown.Y += time * 0.01f;
            }

            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Home))
            {
                Ref.draw.Camera.TiltX -= 0.01f * time;
            }
            if (Input.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.End))
            {
                Ref.draw.Camera.TiltX += 0.01f * time;
            }

            Ref.draw.Camera.CurrentZoom += Input.Mouse.ScrollValue *0.1f;
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
