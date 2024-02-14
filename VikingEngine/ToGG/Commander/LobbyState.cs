using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.ToGG.Commander
{
    class LobbyState : Engine.GameState
    {
        Timer.Basic checkNetworkTimer = new Timer.Basic(TimeExt.SecondsToMS(20), true);
        //int searchSessionCount = 0;
        
        Graphics.TextG networkInfo;
        Graphics.TextG pressStart;
        //Graphics.TextG systemLink;
        GameSetup setup;
        bool host;


        public LobbyState(bool host, bool singlePlayer)
            : base()
        {
            this.host = host;
            setup = new GameSetup();
            setup.lobbyMembers = new List<AbsLobbyMember>
            {
                new LocalLobbyMember(Engine.XGuide.LocalHostIndex),
            };

            networkInfo = new Graphics.TextG(LoadedFont.Regular,
                new Vector2(Engine.Screen.SafeArea.X, Engine.Screen.Height * 0.8f),
                new Vector2(Engine.Screen.TextSize), Graphics.Align.Zero,
                "no connection", Color.Yellow, ImageLayers.Lay1);

            pressStart = new Graphics.TextG(LoadedFont.Regular, networkInfo.Position, new Vector2(Engine.Screen.TextSize), Graphics.Align.Zero,
                "press START to begin (BACK to exit)", Color.White, ImageLayers.Foreground2);

            pressStart.Ypos -= Engine.Screen.IconSize;


            if (host)
            {
                //Ref.netSession.Disconnect();
                //Ref.netSession.SetProperty(cmdLib.NetworkGamestatePropertyIndex, cmdLib.NetworkProtperty_Lobby);
                //Ref.netSession.AbortFindSessions();
                //Ref.netSession.settings.status = Network.SearchAndCreateSessionsStatus.Create;

                //Ref.netSession.CreateSession();
            }
            else
            {
                //if (Ref.netSession.IsConnected)
                //{
                //    foreach (var gamer in Ref.netSession.networkSession.AllGamers)
                //    {
                //        Network.AbsNetworkPeerJoined(gamer);
                //    }
                //    networkInfo.TextString = "Joined lobby";
                //}
                //else
                //{
                //    //joining failed
                //    new MainMenu();
                //}
            }
            updateLobbyMembers();

            //if (singlePlayer)
            //{
            //    setup.lobbyMembers.Add(new AiLobbyMember());
            //    new PlayState(setup);
            //}
        }



        public override void Time_Update(float time)
        {
            base.Time_Update(time);

           // Input.AbsControllerInstance controller = Engine.XGuide.LocalHost.Controller;


            if (Input.Keyboard.KeyDownEvent(Keys.Enter))//controller.KeyDownEvent(Buttons.Start))
            {
                if (Ref.netSession.IsHostOrOffline)
                {
                    new Commander.CmdPlayState(setup);
                    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.cmdGameStarted, Network.PacketReliability.Reliable);
                    w.Write(toggRef.Seed);
                    return;
                }
            }
            else if (Input.Keyboard.KeyDownEvent(Keys.Escape))
            {
                new GameState.MainMenuState();
            }

            if (checkNetworkTimer.Update())
            {
                //tryStartNetwork();
            }
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            //Network.NetLib.PacketType = (Network.PacketType)r.ReadByte();

            switch (packet.type)//Network.NetLib.PacketType)
            {
                case Network.PacketType.cmdGameStarted:
                    AbsLobbyMember first = setup.lobbyMembers[0];
                    setup.lobbyMembers[0] = setup.lobbyMembers[1];
                    setup.lobbyMembers[1] = first;

                    setup.seed = packet.r.ReadInt32();


                    var state = new Commander.CmdPlayState(setup);
                    break;
                //default:
                //    var state = joinSession();
                //    state.read(r, sender, Network.NetLib.PacketType);
                //    break;
            }
        }


        //public override void NetworkAvailableSessionsUpdated(bool isAvailable, AvailableNetworkSessionCollection availableSessions)
        //{
        //    if (availableSessions.Count == 0)
        //    {
        //        if (++searchSessionCount >= 2)
        //        { //Cant find session, create one
        //            Ref.netSession.settings.status = Network.SearchAndCreateSessionsStatus.Create;
        //            Ref.netSession.CreateSession();
                    
        //        }
        //    }
        //    else
        //        Ref.netSession.JoinAvailableSession(availableSessions[0]);
        //}



        public override void  NetworkStatusMessage(Network.NetworkStatusMessage message)
        {
            if (message == Network.NetworkStatusMessage.Session_ended)
            {
                if (host)
                {
                    //searchSessionCount = 0;
                    //Ref.netSession.BeginUpdateAvailableSessions();

                    //clear out remote gamers
                    for (int i = setup.lobbyMembers.Count - 1; i >= 0; --i)
                    {
                        if (setup.lobbyMembers[i] is RemoteLobbyMember)
                        {
                            removeMember(i);
                        }
                    }
                    updateLobbyMembers();
                }
                else
                {
                    new GameState.MainMenuState();
                }
            }

 	         networkInfo.TextString = TextLib.EnumName(message.ToString());
        }

        //public override Engine.GameStateType Type
        //{
        //    get { return Engine.GameStateType.MainMenu; }
        //}

        //public override void Network.AbsNetworkPeerJoined(Network.AbsNetworkPeer gamer)
        //{
        //    if (!gamer.IsLocal)
        //    {
        //        setup.lobbyMembers.Add(new RemoteLobbyMember(gamer));
        //        updateLobbyMembers();
        //    }
        //}
        //public override void Network.AbsNetworkPeerLost(Network.AbsNetworkPeer gamer)
        //{
        //    for (int i = 0; i < setup.lobbyMembers.Count; ++i)
        //    {
        //        if (setup.lobbyMembers[i].IsMember(gamer))
        //        {
        //            removeMember(i);
        //            break;
        //        }
        //    }
        //    updateLobbyMembers();
        //}

        void removeMember(int index)
        {
            setup.lobbyMembers[index].DeleteMe();
            setup.lobbyMembers.RemoveAt(index);
        }

        //public override void GamerSignedInEvent(Engine.PlayerData gamer, Microsoft.Xna.Framework.GamerServices.SignedInGamer signedInGamer)
        //{
        //    //tryStartNetwork();
        //}

        void updateLobbyMembers()
        {
            for (int i = 0; i < setup.lobbyMembers.Count; ++i)
            {
                setup.lobbyMembers[i].updatePos(i);
            }

            pressStart.TextString = "press START to begin ";
            if (setup.lobbyMembers.Count == 1)
            {
                pressStart.TextString += "local MP";
            }
            else
            {
                pressStart.TextString += "online MP";
            }

            pressStart.TextString += " (BACK to exit)";
        }
    }

    
}
