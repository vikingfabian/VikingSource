using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState;
using VikingEngine.LootFest.BlockMap.Level;
using VikingEngine.Network;
using VikingEngine.ToGG.GameState;

namespace VikingEngine.DSSWars.Net
{
    class ConnectState : AbsDssState
    {
        Time failTimer = new Time(8, TimeUnit.Seconds);

        public ConnectState(Network.AbsAvailableSession available)
            : base()
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

                new ExitGamePlay();
                
                return;
            }
        }
       

        public override void NetEvent_PingReturned(AbsNetworkPeer gamer)
        {
            base.NetEvent_PingReturned(gamer);

            new StartGame(false, null, null, null);
            //new Lobby.LobbyState(false, null);
        }
        public override void NetworkReadPacket(ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);
        }

    }
}
