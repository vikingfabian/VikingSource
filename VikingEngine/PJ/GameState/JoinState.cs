using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.GameState
{
    class JoinState : Engine.GameState
    {
        const int SearchingBarDotCount = 8;
        Time timeout = new Time(5, TimeUnit.Seconds);
        Graphics.Image[] searchingBar;
        Timer.Basic searchBarTimer = new Timer.Basic(100, true);
        CirkleCounterUp searchBarCount = new CirkleCounterUp(0, SearchingBarDotCount - 1);

        public JoinState(Network.AbsAvailableSession session)
            :base()
        {
            string hostName = "--";

            if (session == null)
            {
#if PCGAME
                if (Ref.steam.P2PManager.Host != null)
                {
                    hostName = Ref.steam.P2PManager.Host.Gamertag;
                }
#endif
            }
            else
            {
                session.join();
                //hostName = session.hostName;
            }

            draw.ClrColor = Color.Black;
            

            Graphics.Image joinIcon = new Graphics.Image(SpriteName.birdJoinNetwork, Engine.Screen.CenterScreen,
                new Vector2(3, 2) * Engine.Screen.IconSize * 0.7f, ImageLayers.Lay2, true);


            searchingBar = new Graphics.Image[SearchingBarDotCount];
            Rotation1D dir = Rotation1D.D0;

            Vector2 center = joinIcon.Position;
            float r = joinIcon.Width * 0.7f;

            for (int i = 0; i < SearchingBarDotCount; ++i)
            {
                Vector2 pos = center + dir.Direction(r);
                Graphics.Image loadDot = new Graphics.Image(SpriteName.birdBallTrace, pos,
                    new Vector2(Engine.Screen.IconSize * 0.5f), ImageLayers.Lay3, true);
                searchingBar[i] = loadDot;

                dir.Add(MathHelper.TwoPi / SearchingBarDotCount);
            }

            Graphics.TextG text = new Graphics.TextG(LoadedFont.Regular, 
                 VectorExt.AddY(center, Engine.Screen.IconSize * 3f), 
                new Vector2(Engine.Screen.TextSize * 2), Graphics.Align.CenterAll,
                hostName, Color.White, ImageLayers.Lay1);
        }

        public override void NetEvent_PingReturned(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PingReturned(gamer);
            new LobbyState(false);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (searchBarTimer.Update())
            {
                nextSearchBarMotion();
            }

            if (timeout.CountDown())
            {
                new LobbyState();
            }
        }

        void nextSearchBarMotion()
        {
            float white = 1f;
            float darken = 0.2f;
            const int DarkenCount = 3;

            CirkleCounterDown count = new CirkleCounterDown(searchBarCount.Max);
            count.value = searchBarCount.Next();

            //searchBarCount.Next();
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
    }
}
