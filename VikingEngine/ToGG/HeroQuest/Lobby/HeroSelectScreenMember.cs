using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Lobby
{
    class HeroSelectScreenMember
    {
        public Data.PlayerVisualSetup visualSetup = new Data.PlayerVisualSetup();

        VectorRect area;
        public Network.AbsNetworkPeer peer = null;
        Graphics.ImageAdvanced gamericon;
        Graphics.Text2 gamername;
        Graphics.Image bg;

        Graphics.Image hero;
        Graphics.Image joiningEffect;
        ReadyButton readyButton;

        public ReadyStatus readyStatus;
        public RemotePlayerPointer remotePlayerPointer = null;


        public HeroSelectScreenMember(VectorRect area)
        {
            this.area = area;
            Graphics.RectangleLines outline = new Graphics.RectangleLines(
                area, Engine.Screen.BorderWidth, 0, ImageLayers.Background3);
            outline.setColor(Color.DarkGray);
        }

        public void joining(bool begin)
        {
            if (begin)
            {
                if (joiningEffect == null)
                {
                    joiningEffect = new Graphics.Image(SpriteName.WhiteArea_LFtiles,
                    area.Center, new Vector2(area.Width * 0.2f), ImageLayers.Foreground8, true);
                    Graphics.Motion2d rotate = new Graphics.Motion2d(Graphics.MotionType.ROTATE,
                        joiningEffect, new Vector2(5f), Graphics.MotionRepeate.Loop, 1000, true);
                }
            }
            else
            {
                joiningEffect?.DeleteMe();
                joiningEffect = null;
            }

            readyStatus.joined = !begin;
        }

        public void set(Network.AbsNetworkPeer peer)
        {
            DeleteMe();

            this.peer = peer;
            gamericon = new Graphics.ImageAdvanced(SpriteName.MissingImage,
                VectorExt.Add(area.Position, Engine.Screen.BorderWidth),
                Engine.Screen.SmallIconSizeV2, ImageLayers.Lay4, false);
            gamername = new Graphics.Text2("Player", LoadedFont.Bold,
                VectorExt.AddX(gamericon.RightCenter, Engine.Screen.BorderWidth),
                Engine.Screen.TextTitleHeight, Color.White, ImageLayers.Lay3);
            if (peer != null)
            {
                gamername.TextString = peer.Gamertag;

                if (!peer.IsInstance)
                {
                    readyButton = new ReadyButton(area, peer.IsLocal);
                }
            }

            gamername.OrigoAtCenterHeight();

            bg = new Graphics.Image(SpriteName.WhiteArea, area.Position,
                area.Size, ImageLayers.Background5);
            bg.Color = Color.SandyBrown;

            hero = new Graphics.Image(SpriteName.hqHero_Recruit_Sword,
                area.PercentToPosition(0.5f, 0.7f),
                new Vector2(area.Width * 1f), ImageLayers.Foreground9, true);
            hero.Color = Color.Black;

            new SteamWrapping.LoadGamerIcon(gamericon, peer, false);
        }

        public void setLocal()
        {
            readyStatus.joined = true;

            if (peer.IsInstance)
            {
                readyStatus.ready = true;
                readyStatus.mapLoaded = true;
            }
        }

        public void setVisuals(Data.PlayerVisualSetup visualSetup)
        {
            this.visualSetup = visualSetup;
            refreshVisual();
        }

        public void setUnit(HqUnitType unit)
        {
            this.visualSetup.unit = unit;
            refreshVisual();
        }

        void refreshVisual()
        {
            if (visualSetup.unit == HqUnitType.Num_None)
            {
                hero.Color = Color.Black;
            }
            else
            {
                var data = hqRef.unitsdata.Get(visualSetup.unit);
                hero.SetSpriteName(data.modelSettings.image);
                hero.Color = Color.White;
            }
        }

        //public void netReadStatus(System.IO.BinaryReader r)
        //{
        //    //unit = (HqUnitType)r.ReadByte();
        //    visualSetup.netReadStatus(r);

        //    setUnit(visualSetup.unit);
        //}

        public void DeleteMe()
        {
            visualSetup = new Data.PlayerVisualSetup();
            joining(false);

            if (Occupied)
            {
                peer = null;
                gamericon.DeleteMe();
                gamername.DeleteMe();
                bg.DeleteMe();
                hero.DeleteMe();
                readyButton?.DeleteMe();

                remotePlayerPointer?.DeleteMe();
                remotePlayerPointer = null;
            }
        }

        public void update()
        {
            if (readyButton != null)
            {
                readyButton.update();
                readyStatus.ready = readyButton.ready;
            }

            remotePlayerPointer?.Update();
        }

        public void refreshClientStatus()
        {
            setUnit(visualSetup.unit);
            readyButton.refresh(readyStatus.ready);
        }

        //public bool Ready()
        //{
        //    return joiningEffect == null;
        //}

        //public ReadyStatus readyStatus()
        //{
        //    ReadyStatus result = new ReadyStatus();
        //    result.
        //}

        public bool Occupied => peer != null;

        public bool Local => peer != null && peer.IsLocal;
    }

    class ReadyButton
    {
        public bool ready = false;
        Graphics.Text2 text;
        Graphics.Image icon;
        Display.Button readyButton = null;
        Graphics.Image bgTex;

        public ReadyButton(VectorRect heroArea, bool localPlayer)
        {
            VectorRect buttonAr = heroArea;
            buttonAr.nextAreaY(1, Engine.Screen.BorderWidth);
            buttonAr.Height = Engine.Screen.IconSize;

            if (localPlayer)
            {
                readyButton = new Display.Button(buttonAr, ImageLayers.Lay2,
                    Display.ButtonTextureStyle.Standard);
            }
            else
            {
                bgTex = new Graphics.Image(SpriteName.WhiteArea,
                    buttonAr.Position, buttonAr.Size,
                    ImageLayers.Lay2);
                bgTex.ColorAndAlpha(Color.DarkBlue, 0.5f);
                
            }

            icon = new Graphics.Image(SpriteName.cmdHudCheckOn,
                VectorExt.AddX(buttonAr.LeftCenter, Engine.Screen.SmallIconSize * 0.5f),
                Engine.Screen.SmallIconSizeV2, ImageLayers.Lay0);
            icon.OrigoAtCenterHeight();

            text = new Graphics.Text2(TextLib.Error, LoadedFont.Bold,
                VectorExt.AddX(icon.RightTop, Engine.Screen.BorderWidth),
                Engine.Screen.TextTitleHeight, 
                Color.White,//localPlayer? Color.White : Color.LightGray, 
                ImageLayers.Lay0);
            text.OrigoAtCenterHeight();

            refresh(false);
        }

        public void refresh(bool ready)
        {
            this.ready = ready;

            if (ready)
            {
                text.TextString = "Ready";
                text.Color = Color.White;
                icon.SetSpriteName(SpriteName.cmdHudCheckOn);
            }
            else
            {
                text.TextString = "Not ready";
                text.Color = HudLib.UnavailableRedCol;
                icon.SetSpriteName(SpriteName.cmdHudCheckOff);
            }
        }

        public void update()
        {
            if (readyButton != null)
            {
                if (readyButton.update() ||
                    toggRef.inputmap.nextPhase.DownEvent)
                {
                    refresh(!ready);
                    ((LobbyState)Ref.gamestate).netWriteStatus();
                }
            }
        }

        //public void netReadStatus()
        //{

        //}

        public void DeleteMe()
        {
            readyButton?.DeleteMe();
            bgTex?.DeleteMe();

            icon.DeleteMe();
            text.DeleteMe();
        }
    }
}
