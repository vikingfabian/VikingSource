using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Network;

namespace VikingEngine.ToGG.HeroQuest.Net
{
    class ConnectState : Engine.GameState
    {
        Time failTimer = new Time(8, TimeUnit.Seconds);

        public ConnectState(Network.AbsAvailableSession available)
            : base(true)
        {

            Ref.lobby.searchLobbies = false;
            available.join();
            init(available.hostName);
            //warsRef.sound.gamejoin.PlayFlat(1f);
        }
        
        void init(string name)
        {
            Ref.music.stop(true);

            Graphics.Text2 text = new Graphics.Text2("Connecting to " + Engine.LoadContent.CheckCharsSafety(name, LoadedFont.Bold),
                LoadedFont.Bold, Engine.Screen.CenterScreen, Engine.Screen.TextTitleHeight, 
                 Color.Yellow, ImageLayers.Lay1);
            text.OrigoAtCenter();

            VectorRect area = Engine.Screen.Area;
            area.AddRadius(2);
            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Background1);
            bg.ColorAndAlpha(Color.Black, 0.2f);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            Ref.music.Update();

            if (failTimer.CountDown())
            {
                //Ref.lobby.startSearchLobbies(true);

                new GameState.ExitState();
                return;
            }
        }

        public override void NetEvent_PingReturned(AbsNetworkPeer gamer)
        {
            base.NetEvent_PingReturned(gamer);

            new Lobby.LobbyState(false, null);
        }

    }
}
