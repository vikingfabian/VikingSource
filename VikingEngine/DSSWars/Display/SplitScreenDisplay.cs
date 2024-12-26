using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Input;
using VikingEngine.ToGG.HeroQuest.Display;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class SplitScreenDisplay
    {
        ImageGroup2D images;
        List<ControllerBump> controllerBumps = new List<ControllerBump>();

        public SplitScreenDisplay()
        { 
            images = new ImageGroup2D();
        }

        public void Refresh(float menuRight)
        {
            images.DeleteAll();
            controllerBumps.Clear();

            if (DssRef.storage.playerCount > 1)
            {
                float borderW = Screen.Width * 0.1f;
                float w = Screen.Width - menuRight;
                w -= borderW * 2;
                float x = menuRight + borderW;
                float h = w / Engine.Screen.Width * Engine.Screen.Height;
                float y = Screen.CenterScreen.Y - h * 0.5f;

                Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, new Vector2(x, y), new Vector2(w, h), ImageLayers.Lay8);
                images.Add(bg);

                var blackAr= bg.Area;
                blackAr.AddRadius(10);
                Graphics.Image blackBg = new Graphics.Image(SpriteName.WhiteArea, blackAr.Position, blackAr.Size, ImageLayers.Lay8_Back);
                blackBg.Color = Color.Black;
                images.Add(blackBg);


                for (int i = 0; i < DssRef.storage.playerCount; ++i)
                {
                    var player = DssRef.storage.PlayerFromScreenIndex(i);
                    PlayerView view = new PlayerView();
                    
                    var area = view.GetDrawArea(DssRef.storage.playerCount, i, !DssRef.storage.verticalScreenSplit, out _);
                    VectorRect area2 = new VectorRect(area);
                    //convert to percent
                    area2.Position /= Engine.Screen.Area.Size;
                    area2.Size /= Engine.Screen.Area.Size;

                    area2.Position = area2.Position * bg.Size + bg.position;
                    area2.Size *= bg.Size;

                    area2.AddRadius(-5);

                    Graphics.Image screenbg = new Graphics.Image(SpriteName.WhiteArea, area2.Position, area2.Size, ImageLayers.Lay7);
                    screenbg.Color = Color.DarkOliveGreen;
                    images.Add(screenbg);

                    Graphics.ImageAdvanced profileIcon = new ImageAdvanced(SpriteName.NO_IMAGE, area2.Position + Engine.Screen.SmallIconSizeV2, Engine.Screen.IconSizeV2, ImageLayers.Lay6, false);
                    var profileData = DssRef.storage.flagStorage.flagDesigns[player.profile];
                    profileIcon.Texture = profileData.flagDesign.CreateTexture(profileData);
                    profileIcon.SetFullTextureSource();
                    images.Add(profileIcon);

                    Graphics.TextG playerName = new Graphics.TextG(LoadedFont.Bold, profileIcon.RightCenter, Engine.Screen.TextSizeV2, Align.CenterHeight,
                        string.Format(DssRef.lang.Player_DefaultName, player.index+1), Color.White, ImageLayers.Lay6);
                    playerName.Xpos += 4;
                    images.Add(playerName);

                    Graphics.Image controllerIcon = new Graphics.Image(SpriteName.birdControllerIcon, 
                        VectorExt.AddY( profileIcon.position, profileIcon.Height*1.2f), profileIcon.size, ImageLayers.Lay6, false);
                    controllerIcon.Width *= 1.5f;
                    controllerIcon.Xpos -= controllerIcon.Width * 0.4f;
                    images.Add(controllerIcon);
                    Graphics.TextG inputType = new TextG(LoadedFont.Regular, controllerIcon.RightCenter, Engine.Screen.TextSizeV2, Align.CenterHeight,
                        player.inputSource.ToString(), Color.White, ImageLayers.Lay6);
                    images.Add(inputType);
                    //button.icon.Texture = flagDesign.CreateTexture(this);
                    //button.icon.SetFullTextureSource();
                    controllerBumps.Add(new ControllerBump(player.inputSource, controllerIcon));
                }

            }
        }

        public void update()
        {
            foreach (var bump in controllerBumps)
            { 
                bump.update();
            }
        }

        class ControllerBump
        {
            Graphics.Image controllerIcon;
            XController controller= null;
            Vector2 iconScale;
            public ControllerBump(InputSource inputSource, Graphics.Image controllerIcon)
            {
                if (inputSource.sourceType == InputSourceType.XController)
                {
                    this.controllerIcon = controllerIcon;
                    iconScale = controllerIcon.size;
                    controller = inputSource.Controller;
                }
            }

            public void update()
            { 
                if (controller != null)
                {
                    bool downInput = controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) ||
                        controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B) ||
                        controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.X) ||
                        controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Y) ||
                        controller.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Start);

                    controllerIcon.size = downInput ? iconScale * 1.5f : iconScale;
                }
            }
        }
    }
    
}
