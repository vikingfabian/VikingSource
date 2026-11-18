using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Interface
{
    class MiniMap
    {
        RenderTargetDrawContainer renderTargetDrawContainer;
        Graphics.ImageAdvanced mapTexture = null, unitTexture = null;
        Graphics.Image hoverHighLight;
        NineSplitAreaTexture bg;
        RectangleLines cameraOutline;
        Graphics.Image buttonHighLight;

        public VectorRect area, buttonsArea;
        Vector2 areaHalfSize;
        Vector2 textureSize;
        float scale = 2f;
        bool bMouseInput;
        bool selectDown = false, panDown = false;
        bool viewCameraBound = true;

        List2<MinimapButton> buttons;

        int? hoverButton = null;

        public MiniMap(LocalPlayer player, bool runtime)
        {
            bMouseInput = player.gameControls.input.inputSource.useTouchAsMouseSim;
            Vector2 sz = Engine.Screen.IconSizeV2 * 5f * Ref.gamesett.MinimapScale;
            Vector2 pos = player.playerData.view.wideScreenSafeScreenArea.RightBottom - sz;
            area = new VectorRect(pos, sz);
            area.Round();
            areaHalfSize = area.Size * 0.5f;
            var bgArea = area;
            bgArea.AddRadius(4);

            bg = new NineSplitAreaTexture(HudLib.MinimapBorder, bgArea, HudLib.GUILayer + 2);
            bgArea.AddRadius(2);

            hoverHighLight = new Image(SpriteName.WhiteArea_LFtiles, bgArea.Position, bgArea.Size, HudLib.GUILayer + 2);
            hoverHighLight.Opacity = 0.5f;
            hoverHighLight.Visible = false;
            buttonHighLight  = new Image(SpriteName.WhiteArea_LFtiles, Vector2.Zero, Vector2.One, HudLib.MinimapButtonLayer + 2);
            buttonHighLight.Opacity = 0.5f;
            buttonHighLight.Visible = false;

            var textureSz = AbsMapPixelTexture.PixelTextureScale();
            mapTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, textureSz.Vec, ImageLayers.Background0, false, false);
            mapTexture.ImageSource = new Rectangle(0, 0, textureSz.X, textureSz.Y);
           
            unitTexture = new ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, textureSz.Vec, ImageLayers.Foreground3, false, false);
            unitTexture.ImageSource = new Rectangle(0, 0, textureSz.X, textureSz.Y);

            textureSize = mapTexture.size;

            cameraOutline = new RectangleLines(VectorRect.ZeroOne, 1, 0, ImageLayers.Foreground0, false);

            var images = new List<AbsDraw>(8) { mapTexture, unitTexture };
            images.AddRange(cameraOutline.lines);

            renderTargetDrawContainer = new RenderTargetDrawContainer(area.Position, area.Size, HudLib.GUILayer, images);
            renderTargetDrawContainer.alwaysRedraw = true;

            refreshScale();

            buttons = new List2<MinimapButton>((int)MinimapButtonAction.NUM);
            Vector2 butttonsz = Engine.Screen.SmallIconSizeV2;
            VectorRect buttonAr = new VectorRect(area.LeftBottom - butttonsz + new Vector2(2), butttonsz);
            for (MinimapButtonAction action = MinimapButtonAction.NUM -1; action >= 0; action--)
            {
                buttons.Add(new MinimapButton(action, buttonAr));
                
                if (action == 0)
                {
                    buttonsArea = buttonAr;
                    buttonsArea.SetBottom(area.Bottom, true);
                    buttonsArea.AddRadius(2);
                }
                buttonAr.nextAreaY(-1, 0);
            }

            if (runtime)
            {
                refreshPosition(player);
                updateCamera(player);
            }
        }

        public void DeleteMe()
        {
            hoverHighLight.DeleteMe();
            bg.DeleteMe();
            renderTargetDrawContainer.DeleteMe();


            foreach (var button in buttons)
            {
                button.image.DeleteMe();
            }
        }

        public void update(LocalPlayer player, bool allowInput, out bool mouseOver)
        {
            hoverButton = null;
            var mouse = player.gameControls.input.mouse;

            if (player.minimapPixelTexture == null)
            {
                mouseOver = false;
                return;
            }
            mapTexture.Texture = player.minimapPixelTexture.texture;
            unitTexture.Texture = player.unitsPixelTexture.texture;

            mouseOver = allowInput && bMouseInput && area.IntersectPoint(mouse.Position);
            if (mouseOver || selectDown || panDown)
            {
                hoverHighLight.Visible = true;

                zoomInput(player.gameControls.input.ZoomValue(), 0.005f, player);

                if (player.gameControls.input.mouseSelect.IsDown)
                {
                    if (player.gameControls.input.mouseSelect.DownEvent)
                    {
                        selectDown = true;
                    }                    

                    if (selectDown)
                    {
                        player.gameControls.map.setCameraPosition(screenPosToWorldXZ(mouse.Position));
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
                        mapTexture.position += mouse.MoveDistance;
                        unitTexture.position = mapTexture.position;

                        foreach (var l in cameraOutline.lines)
                        { 
                            l.position += mouse.MoveDistance;
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

                if (DssRef.storage.minimapOptions.autoPan)
                {
                    refreshPosition(player);
                }
                selectDown = false;
                //if (DssRef.storage.minimapOptions.autoZoom)
                {
                    updateCamera(player);
                }

                if (buttonsArea.IntersectPoint(mouse.Position))
                {
                    mouseOver = true;
                    for (int i = 0; i < buttons.Count; ++i)
                    {
                        if (buttons.Array[i].area.IntersectPoint(mouse.Position))
                        {
                            hoverButton = i;
                            break;
                        }
                    }
                }
            }

            if (hoverButton.HasValue)
            {
                var area = buttons.Array[hoverButton.Value].area;
                area.AddRadius(2);
                buttonHighLight.Area = area;
                buttonHighLight.Visible = true;

                if (player.gameControls.input.mouseSelect.DownEvent)
                {
                    buttons.Array[hoverButton.Value].onClick(player);
                }
            }
            else
            { 
                buttonHighLight.Visible = false;
            }
        } 

        public void OnMapZoom(float zoominput, LocalPlayer player)
        {
            if (DssRef.storage.minimapOptions.autoZoom)
            {
                zoomInput(zoominput, 0.001f, player);
            }
        }

        void zoomInput(float zoom, float speed, LocalPlayer player)
        {
            if (zoom != 0)
            {
                scale = Bound.Set(scale - zoom * speed * Ref.gamesett.scrollWheelSensitivity_game * scale, 0.5f, 5f);
                refreshScale();
                refreshPosition(player);
                updateCamera(player);
            }
        }

        Vector2 screenPosToWorldXZ(Vector2 screenPos)
        {
            Vector2 localPos = screenPos - area.Position;
            Vector2 mapPos = localPos - mapTexture.position;

            return mapPos / scale / textureSize * DssRef.world.unitSize;
        }

        void refreshScale()
        {
            mapTexture.size = textureSize * scale;
            unitTexture.size = mapTexture.size;
        }

        void refreshPosition(LocalPlayer player)
        {
            mapTexture.position = -(lookCenter(player) * scale) + areaHalfSize;
            unitTexture.position = mapTexture.position;
        }

        void updateCamera(LocalPlayer player)
        {   
            var p = DssRef.state.culling.players[player.playerData.localPlayerIndex];
            var state = DssRef.state.culling.cullingStateA ? p.stateA : p.stateB;
            cameraOutline.rectangle.Size = scale * MapTile1_1.ModelScale * state.enterArea.size.Vec * DssRef.world.unitSizeInv * textureSize; //area is in unit size

            cameraOutline.rectangle.Center = (lookCenter(player) * scale) + mapTexture.position;
            cameraOutline.Refresh();

            if (viewCameraBound == state.farLayer)
            {
                viewCameraBound = !state.farLayer;
                cameraOutline.setOpacity(viewCameraBound ? 1f: 0.25f);
            }
        }

        Vector2 lookCenter(LocalPlayer player)
        {
            return player.gameControls.map.camera.LookTargetXZ * DssRef.world.unitSizeInv * textureSize;
        }
    }

    struct MinimapButton
    {
        MinimapButtonAction buttonAction;
        public bool toggle;

        public VectorRect area;

        public Graphics.Image image;

        public MinimapButton(MinimapButtonAction buttonAction, VectorRect area)
        {
            this.buttonAction = buttonAction;
            this.area = area;
            image = new Image(SpriteName.MissingImage, area.Position, area.Size, HudLib.MinimapButtonLayer);

            toggle = DssRef.storage.minimapOptions.Get(buttonAction);
            
            refresh();
        }

        public void refresh()
        {
            switch (buttonAction)
            {
                case MinimapButtonAction.MapLayer1:
                    image.SetSpriteName(SpriteName.WarsMinimapButtonMapLay1);
                    break;
                case MinimapButtonAction.MapLayer2:
                    image.SetSpriteName(SpriteName.WarsMinimapButtonMapLay2);
                    break;
                case MinimapButtonAction.MapLayer3:
                    image.SetSpriteName(SpriteName.WarsMinimapButtonMapLay3);
                    break;
                case MinimapButtonAction.ToggleColor:
                    image.SetSpriteName(toggle ? SpriteName.WarsMinimapButtonBlackAndWhite : SpriteName.WarsMinimapButtonFullColor);
                    break;
                case MinimapButtonAction.TogglePan:
                    image.SetSpriteName(toggle ? SpriteName.WarsMinimapButtonAutoPanOn : SpriteName.WarsMinimapButtonAutoPanOff);
                    break;
                case MinimapButtonAction.ToggleZoom:
                    image.SetSpriteName(toggle ? SpriteName.WarsMinimapButtonAutoZoomOn : SpriteName.WarsMinimapButtonAutoZoomOff);
                    break;

            }
        }

        public void onClick(LocalPlayer player)
        {
            switch (buttonAction)
            {
                case MinimapButtonAction.MapLayer1:
                    player.gameControls.map.SetTargetZoom(Map.MapLayer.MapDetailLayerType.Layer3_Far);
                    break;
                case MinimapButtonAction.MapLayer2:
                    player.gameControls.map.SetTargetZoom(Map.MapLayer.MapDetailLayerType.Layer2_Mid);
                    break;
                case MinimapButtonAction.MapLayer3:
                    player.gameControls.map.SetTargetZoom(Map.MapLayer.MapDetailLayerType.Layer1_Near);
                    break;
                default:
                    toggle = !toggle;
                    DssRef.storage.minimapOptions.Set(buttonAction, toggle);
                    refresh();

                    if (buttonAction == MinimapButtonAction.ToggleColor)
                    {
                        player.minimapPixelTexture.refreshMiniMapFilter();
                        DssRef.world.CrossMapTextureNeedsUpdate = true;
                    }
                    break;

            }
        }
    }

    struct MinimapOptions
    {
        public bool autoPan = true;
        public bool autoZoom = true;
        public bool blackAndWhite = true;

        public MinimapOptions()
        { }

        public void write(System.IO.BinaryWriter w)
        {
            new EightBit(autoPan, autoZoom, blackAndWhite).write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            EightBit.FromStream(r).Get(out autoPan, out autoZoom, out blackAndWhite);
        }

        public bool Get(MinimapButtonAction opt)
        {
            switch (opt)
            {
                case MinimapButtonAction.ToggleColor:
                    return blackAndWhite;
                case MinimapButtonAction.TogglePan:
                    return autoPan;
                case MinimapButtonAction.ToggleZoom:
                    return autoZoom;

            }

            return false;
        }
        public void Set(MinimapButtonAction opt, bool value)
        {
            switch (opt)
            {
                case MinimapButtonAction.ToggleColor:
                    blackAndWhite = value;break;
                case MinimapButtonAction.TogglePan:
                    autoPan = value; break;
                case MinimapButtonAction.ToggleZoom:
                    autoZoom = value; break;
            }
        }
    }

    enum MinimapButtonAction
    {
        MapLayer1,
        MapLayer2,
        MapLayer3,
        ToggleZoom,
        TogglePan,
        ToggleColor,
        NUM
    }
}
