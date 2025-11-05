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
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Map
{
    class MiniMap
    {
        RenderTargetDrawContainer renderTargetDrawContainer;
        Graphics.ImageAdvanced mapTexture = null, unitTexture = null;
        Graphics.Image hoverHighLight;
        NineSplitAreaTexture bg;
        RectangleLines cameraOutline;

        //Vector2 mapSize;
        public VectorRect area;
        Vector2 areaHalfSize;
        Vector2 textureSize;
        float scale = 2f;
        bool bMouseInput;
        bool selectDown = false, panDown = false;
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
           
            unitTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, DssRef.world.Size.Vec, ImageLayers.Foreground3, false, false);
            unitTexture.ImageSource = new Rectangle(0, 0, DssRef.world.Size.X, DssRef.world.Size.Y);

            textureSize = mapTexture.size;

            cameraOutline = new RectangleLines(VectorRect.ZeroOne, 1, 0, ImageLayers.Foreground0, false);

            var images = new List<AbsDraw>(8) { mapTexture, unitTexture };
            images.AddRange(cameraOutline.lines);

            renderTargetDrawContainer = new RenderTargetDrawContainer(area.Position, area.Size, HudLib.GUILayer, images);

            refreshScale();

        }

        public void update(LocalPlayer player, bool allowInput, out bool mouseOver)
        {
            if (player.minimapPixelTexture == null)
            {
                mouseOver = false;
                return;
            }
            mapTexture.Texture = player.minimapPixelTexture.texture;
            unitTexture.Texture = player.unitsPixelTexture.texture;

            mouseOver = allowInput && bMouseInput && area.IntersectPoint(Input.Mouse.Position);
            if (mouseOver || selectDown || panDown)
            {
                hoverHighLight.Visible = true;

                updateZoom(0.005f);

                if (player.gameControls.input.mouseSelect.IsDown)
                {
                    if (player.gameControls.input.mouseSelect.DownEvent)
                    {
                        selectDown = true;
                    }                    

                    if (selectDown)
                    {
                        player.gameControls.map.setCameraPosition(screenPosToWorldXZ(Input.Mouse.Position));
                        updateCamera(player);
                    }
                }
                else
                {
                    selectDown = false;
                }

                if (player.gameControls.input.mousePan.IsDown)
                {
                    if (player.gameControls.input.mousePan.DownEvent)
                    {
                        panDown = true;
                    }

                    if (panDown)
                    {
                        mapTexture.position += Input.Mouse.MoveDistance;
                        unitTexture.position = mapTexture.position;

                        foreach (var l in cameraOutline.lines)
                        { 
                            l.position += Input.Mouse.MoveDistance;
                        }
                    }
                }
                else
                {
                    panDown = false;
                }
            }
            else
            {
                hoverHighLight.Visible = false;

                updateZoom(0.002f);

                refreshPosition(player);

                selectDown = false;

                updateCamera(player);
            }

            void updateZoom(float speed)
            {
                float zoom = player.gameControls.input.ZoomValue();
                if (zoom != 0)
                {
                    scale = Bound.Set(scale - zoom * speed * Ref.gamesett.scrollWheelSensitivity_game * scale, 0.5f, 5f);
                    refreshScale();
                    refreshPosition(player);
                }
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

        void updateCamera(LocalPlayer player)
        {   
            var p = DssRef.state.culling.players[player.playerData.localPlayerIndex];
            var state = DssRef.state.culling.cullingStateA ? p.stateA : p.stateB;
            cameraOutline.rectangle.Size = state.enterArea.size.Vec;

            cameraOutline.rectangle.Center = (player.gameControls.map.camera.LookTargetXZ * scale) + mapTexture.position;
            cameraOutline.Refresh();
        }
    }
}
