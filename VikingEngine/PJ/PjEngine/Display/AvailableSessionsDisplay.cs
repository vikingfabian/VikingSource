using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.PJ.Display
{
    class AvailableSessionsDisplay
    {
        public static readonly Color BgCol = new Color(51, 52, 60);

        HUD.ImageButton bg;
        Graphics.Image joinIcon;
        Graphics.TextG lobbyName;
        Vector2 optionsDotPos, optionsDotSz, optionsDotSpace;

        OptionsGroup dots;

        static readonly Time AutoScrollTime = new Time(1.8f, TimeUnit.Seconds);
        static readonly Time AfterHoverScrollTime = new Time(5, TimeUnit.Seconds);

        int selected = 0;
        Time viewNextTimer = AutoScrollTime;
        List<Network.AbsAvailableSession> availableSessions = null;

        const int SearchingBarDotCount = 8;
        Graphics.Image[] searchingBar;
        Graphics.Image searchingBarCenter, noSessionsIcon;
        CirkleCounterUp searchBarCount = new CirkleCounterUp(0, SearchingBarDotCount - 1);
        Timer.Basic searchBarTimer = new Timer.Basic(100, true);

        public AvailableSessionsDisplay(float topBarH)
        {
            float width = Engine.Screen.Height * 0.32f;
            float availableHeight = topBarH - Engine.Screen.IconSize * 0.8f;

            optionsDotSz = new Vector2(Engine.Screen.IconSize * 0.4f);
            optionsDotSpace = optionsDotSz * 0.4f;

            optionsDotPos = Engine.Screen.SafeArea.RightTop;
            optionsDotPos.X -= width;

            float bgY = Engine.Screen.SafeArea.Y + optionsDotSz.Y + optionsDotSpace.Y;
            bg = new HUD.ImageButton(SpriteName.WhiteArea, new VectorRect(
                new Vector2(optionsDotPos.X, bgY),
                new Vector2(width, availableHeight - bgY)),
                ImageLayers.Background5, HudLib.LargeButtonSettings);
            bg.baseImage.Color = BgCol;

            lobbyName = new Graphics.TextG(LoadedFont.Regular, bg.area.Position + new Vector2(Engine.Screen.BorderWidth), 
                Engine.Screen.TextTitleScale,
                Graphics.Align.Zero, "test test test", Color.White, ImageLayers.AbsoluteBottomLayer);
            lobbyName.LayerAbove(bg.baseImage);

            joinIcon = new Graphics.Image(SpriteName.birdJoinNetwork, bg.area.Center,
                new Vector2(3, 2) * 0.8f * Engine.Screen.IconSize, ImageLayers.AbsoluteBottomLayer, true);
            joinIcon.LayerAbove(bg.baseImage);

            lobbyName.Visible = false;

            viewLobbyInfo(false);
            
            searchingBar = new Graphics.Image[SearchingBarDotCount];
            Rotation1D dir = Rotation1D.D0;

            Vector2 center = bg.area.Center;
            float r = Engine.Screen.IconSize * 0.6f;
            
            for (int i = 0; i < SearchingBarDotCount; ++i)
            {
                Vector2 pos = center + dir.Direction(r);
                Graphics.Image loadDot = new Graphics.Image(SpriteName.birdBallTrace, pos, new Vector2(Engine.Screen.IconSize * 0.3f), ImageLayers.AbsoluteBottomLayer, true);
                loadDot.LayerAbove(bg.baseImage);
                searchingBar[i] = loadDot;

                dir.Add(MathHelper.TwoPi / SearchingBarDotCount);
            }

            searchingBarCenter = new Graphics.Image(SpriteName.birdNetworkIcon, center, new Vector2(Engine.Screen.IconSize * 0.5f), ImageLayers.AbsoluteBottomLayer, true);
            searchingBarCenter.LayerAbove(bg.baseImage);

            noSessionsIcon = new Graphics.Image(SpriteName.birdZeroLobbies, center, new Vector2(4, 2) * Engine.Screen.IconSize * 0.5f, ImageLayers.AbsoluteBottomLayer, true);
            noSessionsIcon.LayerAbove(bg.baseImage);
            noSessionsIcon.Visible = false;

            viewSearchingBar(false);
        }

        void viewLobbyInfo(bool view)
        {
            lobbyName.Visible = view;
            joinIcon.Visible = view;
        }
        void viewSearchingBar(bool view)
        {
            foreach (var m in searchingBar)
            {
                m.Visible = view;
            }
            searchingBarCenter.Visible = view;
        }


        void joinClick()
        {
            Network.AbsAvailableSession sess;
            if (arraylib.TryGet(availableSessions, selected, out sess))
            {
                new GameState.JoinState(sess);
            }
        }

        public void startedSearch()
        {
            viewSearchingBar(true);
            viewLobbyInfo(false);
            noSessionsIcon.Visible = false;
        }

        public void foundNoSessions()
        {
            viewLobbyInfo(false);
            viewSearchingBar(false);
            noSessionsIcon.Visible = true;
        }

        public void removeSearch()
        {
            viewLobbyInfo(false);
            viewSearchingBar(false);
            noSessionsIcon.Visible = false;

            availableSessions = null;
            dots?.Clear();
            dots = null;
        }

        public void listSessions(List<Network.AbsAvailableSession> availableSessions)
        {
            this.availableSessions = availableSessions;

            if (PlatformSettings.DevBuild && availableSessions != null)
            {
                availableSessions.Add(availableSessions[0]);
                availableSessions.Add(availableSessions[0]);

            }

            int maxCount = Table.FitCellCount(optionsDotSz.X, optionsDotSpace.X, bg.area.Width);

            int count = 0;
            if (availableSessions != null)
            {
                count = Bound.Max(availableSessions.Count, maxCount);
            }
            
            dots?.Clear();

            if (count == 0)
            {
                foundNoSessions();
            }
            else
            {
                dots = new OptionsGroup();
                dots.buttons = new MenuOptionButton[count];
                for (int i = 0; i < count; ++i)
                {
                    var area = Table.CellPlacement(optionsDotPos, false, i, maxCount, optionsDotSz, optionsDotSpace);
                    dots.buttons[i] = new SessionOptionButton(area);
                }
                sessionsDotHover(0);

                viewLobbyInfo(true);
                viewSearchingBar(false);
            }
        }

        public void update()
        {
            if (dots != null)
            {
                if (bg.update())
                {
                    joinClick();
                }

                if (bg.mouseOver)
                {
                    viewNextTimer = AfterHoverScrollTime;
                }
                dots.update();
                if (dots.mouseOver > -1)
                {
                    viewNextTimer = AfterHoverScrollTime;
                    sessionsDotHover(dots.mouseOver);
                }

                if (viewNextTimer.CountDown())
                {
                    if (dots != null && dots.buttons != null)
                    {
                        sessionsDotHover(Bound.SetRollover(selected + 1, dots.buttons.Length - 1));
                        viewNextTimer = AutoScrollTime;
                    }
                }
            }

            if (searchingBarCenter.Visible)
            {
                if (searchBarTimer.Update())
                {
                    nextSearchBarMotion();
                }
            }
        }

        void nextSearchBarMotion()
        {
            float white = 1f;
            float darken = 0.2f;
            const int DarkenCount = 3;

            CirkleCounterDown count = new CirkleCounterDown(searchBarCount.Max);
            count.value = searchBarCount.Next();
            
            for (int i = 0; i < SearchingBarDotCount; ++i)
            {
                int dotIx = count.Next();
                searchingBar[dotIx].Color = ColorExt.GrayScale(white);

                if (i < DarkenCount)
                {
                    white -= darken;
                }
            }

        }

        void sessionsDotHover(int index)
        {
            if (availableSessions != null && availableSessions.Count > index)
            {
                selected = index;
                dots.select(index);

                lobbyName.TextString = Engine.LoadContent.CheckCharsSafety(availableSessions[selected].hostName, lobbyName.Font);
            }
            else if (PlatformSettings.DevBuild)
            {
                lobbyName.TextString = "Test lobby" + TextLib.IndexToString(index);
                selected = index;
                dots.select(index);
            }

        }
    }
}
