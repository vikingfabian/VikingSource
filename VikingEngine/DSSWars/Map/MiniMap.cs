using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.DSSWars.Map
{
    class MiniMap
    {
        RenderTargetDrawContainer renderTargetDrawContainer;
        Graphics.ImageAdvanced mapTexture = null, unitTexture = null;
        Graphics.Image hoverHighLight;
        NineSplitAreaTexture bg;

        //Vector2 mapSize;
        public VectorRect area;
        Vector2 areaHalfSize;
        Vector2 textureSize;
        float scale = 2f;
        bool bMouseInput;
        bool mouseDown = false;
        public MiniMap(LocalPlayer player)
        {
            bMouseInput = player.gameControls.input.inputSource.HasMouse;
            Vector2 sz = Engine.Screen.IconSizeV2 * 5f;
            Vector2 pos = player.playerData.view.safeScreenArea.RightBottom - sz;
            area = new VectorRect(pos, sz);
            area.Round();
            areaHalfSize = area.Size * 0.5f;
            var bgArea = area;
            bgArea.AddRadius(4);

            //bg = new Image(SpriteName.WhiteArea, bgArea.Position, bgArea.Size, HudLib.GUILayer + 2);
            //bg.Color = Color.Black;
            bg = new NineSplitAreaTexture(HudLib.MinimapBorder, bgArea, HudLib.GUILayer + 2);
            bgArea.AddRadius(2);

            hoverHighLight = new Image(SpriteName.WhiteArea, bgArea.Position, bgArea.Size, HudLib.GUILayer + 2);
            hoverHighLight.Opacity = 0.5f;
            hoverHighLight.Visible = false;

            mapTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, DssRef.world.Size.Vec, ImageLayers.Background0, false, false);
            mapTexture.ImageSource = new Rectangle(0, 0, DssRef.world.Size.X, DssRef.world.Size.Y);
           
            unitTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, DssRef.world.Size.Vec, ImageLayers.Foreground0, false, false);
            unitTexture.ImageSource = new Rectangle(0, 0, DssRef.world.Size.X, DssRef.world.Size.Y);

            textureSize = mapTexture.size;

            renderTargetDrawContainer = new RenderTargetDrawContainer(area.Position, area.Size, HudLib.GUILayer, new List<AbsDraw> { mapTexture, unitTexture });

            refreshScale();

        }

        public void update(LocalPlayer player, bool allowInput, out bool mouseOver)
        {            
            mapTexture.Texture = player.minimapPixelTexture.texture;
            unitTexture.Texture = player.unitsPixelTexture.texture;

            mouseOver = allowInput && bMouseInput && area.IntersectPoint(Input.Mouse.Position);
            if (mouseOver)
            {
                hoverHighLight.Visible = true;

                float zoom = player.gameControls.input.ZoomValue();
                if (zoom != 0)
                {
                    scale = Bound.Set(scale + zoom * 0.005f * scale, 0.5f, 5f);
                    refreshScale();
                    refreshPosition(player);
                }

                if (player.gameControls.input.mouseSelect.IsDown)
                {
                    if (player.gameControls.input.mouseSelect.DownEvent)
                    {
                        mouseDown = true;
                    }

                    if (mouseDown)
                    {
                        player.gameControls.map.setCameraPosition(screenPosToWorldXZ(Input.Mouse.Position));
                    }
                }
                else
                {
                    mouseDown = false;
                }
            }
            else
            {
                hoverHighLight.Visible = false;

                refreshPosition(player);

                mouseDown = false;
            }
        }

        Vector2 screenPosToWorldXZ(Vector2 screenPos)
        {
            Vector2 localPos = screenPos - area.Position;
            Vector2 mapPos = localPos - mapTexture.position;

            return mapPos / scale;
        }

        void refreshScale()
        {
            mapTexture.size = textureSize * scale;
            unitTexture.size = mapTexture.size;
        }

        void refreshPosition(LocalPlayer player) 
        { 
            Vector2 center = player.gameControls.map.camera.LookTargetXZ;
            mapTexture.position = -(center * scale) + areaHalfSize;
            unitTexture.position = mapTexture.position;
        }
    }
}
