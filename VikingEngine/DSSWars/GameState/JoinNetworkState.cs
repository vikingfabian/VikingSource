using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.SteamWrapping;
//

namespace VikingEngine.DSSWars
{
    class JoinNetworkState : Engine.GameState
    {
        Graphics.TextG statusText;
        ushort seed;
        int state_0joining_1waitforseed_2loadingmap_3waiting = 0;

        Timer.Basic pingTimer = new Timer.Basic(500, true);

        public JoinNetworkState(Network.AbsAvailableSession availableSession)
            : base(true)
        {
            availableSession.join();

            statusText = new Graphics.TextG(LoadedFont.Regular, new Vector2(Engine.Screen.SafeArea.X, Engine.Screen.CenterScreen.Y),
                new Vector2(Engine.Screen.TextSize), Graphics.Align.CenterHeight, "Joining " + availableSession.hostName, Color.White, ImageLayers.Foreground1);

            DssRef.world = null;
            //Ref.netSession.JoinAvailableSession(availableSession);
            //new Timer.AsynchActionTrigger(loadThread);

           
        }

        //void loadThread()
        //{
        //    new WorldData(availableSession.SessionProperties[RTSlib.NetworkSeedProperty].Value);
        //}

        public override void NetworkStatusMessage(Network.NetworkStatusMessage message)
        {
            statusText.TextString = message.ToString();
            if (message == Network.NetworkStatusMessage.Joining_failed || message == Network.NetworkStatusMessage.Joining_timed_out)
            {
                new GameState.ExitGamePlay();
            }

            //if (message == Network.NetworkStatusMessage.Joining_session)
            //{
            //    state_0joining_1waitforseed_2loadingmap = 1;
            //}
        }

       

        public override void Time_Update(float time)
        {
            //if (Input.Controller.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.Back))
            //{
            //    new GameState.ExitGamePlay();
            //}

            

            switch (state_0joining_1waitforseed_2loadingmap_3waiting)
            {
                case 1:
                    statusText.TextString = "Joined, waiting for seed";
                    if (pingTimer.Update())
                    {
                        var w = Ref.netSession.BeginWritingPacket(Network.PacketType.rtsWantSeed, Network.PacketReliability.Reliable);
                    }
                    break;
                case 2:
                    //statusText.TextString  = "Loading map: " + WorldData.LoadStatus.ToString() + "%";

                    if (DssRef.world != null && Ref.netSession.InMultiplayerSession)
                    {
                        state_0joining_1waitforseed_2loadingmap_3waiting = 3;

                        Ref.netSession.BeginWritingPacket(Network.PacketType.rtsMapLoadedAndReady, Network.PacketReliability.Reliable);
                        
                        statusText.TextString = "Waiting for all players to get ready";
                    }
                    break;
            }
        }
        public override void NetEvent_GotNetworkId()
        {
            base.NetEvent_GotNetworkId();
        //}

        //public override void NetworkReceivedHail(Network.ReceivedPacket packet)
        //{
        //    base.NetworkReceivedHail(packet);
            state_0joining_1waitforseed_2loadingmap_3waiting = 1;
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);

            switch (packet.type)
            {
                case Network.PacketType.rtsSeed:
                    if (state_0joining_1waitforseed_2loadingmap_3waiting < 2)
                    {
                        state_0joining_1waitforseed_2loadingmap_3waiting = 2;

                        seed = packet.r.ReadUInt16();
                        new Timer.AsynchActionTrigger(loadThread);
                    }
                    break;
                case Network.PacketType.rtsStartGame:
                    if (DssRef.world != null)
                    {
                        new PlayState(false, null);
                    }
                    break;
            }
            
        }

        void loadThread()
        {
            //new WorldData(seed, StartupSettings.MapSz);
        }

        //public override Engine.GameStateType Type
        //{
        //    get { return Engine.GameStateType.LoadingGame; }
        //}
    }
}
