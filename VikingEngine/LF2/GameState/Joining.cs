using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameState
{
    abstract class AbsJoiningState : Engine.GameState
    {
        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
           // Network.PacketType type = (Network.PacketType)System.IO.BinaryReader.ReadByte();
            switch (packet.type)
            {
                case Network.PacketType.LF2_WorldIndex:
                    Data.RandomSeed.RecieveWorldIx(packet.r);
                    Map.World.RunningAsHost = false;
                    new GameState.LoadingMap();
                    break;
            }
        }

        protected void createMenuStateBackg(bool dark)
        {
            const float SizeAdd = 10;
            Graphics.Image bakgTexture = new Graphics.Image(SpriteName.BoardTxtGrayBakg, new Vector2(-SizeAdd), Engine.Screen.Resolution + new Vector2(SizeAdd * PublicConstants.Twice),
                 ImageLayers.Background9);
            bakgTexture.Color = new Color(131, 165, 225);
            if (dark)
            {
               bakgTexture.Color = new Color(bakgTexture.Color.ToVector3() * 0.3f);
                
            }
         }
    }

   

    class Joining : AbsJoiningState
    {
        CirkleCounterUp numDots = new CirkleCounterUp(3);
        Timer.Basic dotTime = new Timer.Basic(400, true);
        Timer.Basic connectingTimeOut = new Timer.Basic(4000);
        Timer.Basic receivingDataTimeOut = new Timer.Basic(lib.SecondsToMS(6));
        Graphics.TextG text;
        JoiningState state = JoiningState.Joining;
        bool local;
        //int hostIndex;
        string host;
        const int MaxTrials = 10;
        int numTrials = 0;
        bool endReasonTimeOutNotConnectionLost;
        const float StateBeginningSec = 1;
        Time beginningOfAState = new Time(StateBeginningSec, TimeUnit.Seconds);
        

        public Joining(int ix, bool local)
            : base()
        {
            AvailableNetworkSession available = Ref.netSession.GetAvailableSession(ix);
            Init();
            this.local = local;
            if (available != null)
            {
                host = available.HostGamertag;
                tryJoin();
                setState(JoiningState.Joining);
            }
            else
            {
                setState(JoiningState.QuitJoining);
            }
        }

        public Joining(int player) //From invite
            : base()
        {
            setState(JoiningState.RecievingData);
            Init();
            //Invite join
            Engine.XGuide.UnjoinAll();
            Engine.XGuide.GetPlayer(player).IsActive = true;
            if (Network.Session.Connected)
            {
                host = Ref.netSession.Host.Gamertag;
            }
        }
        public override void NetworkJoined(Network.AbsNetworkPeer me)
        {
            if (Network.Session.Connected)
            {
                host = Ref.netSession.Host.Gamertag;
            }
            setState(JoiningState.RecievingData);
            //base.NetworkJoined(me);
        }
        public override void NetworkConnectionLost(Network.AbsNetworkPeer host, NetworkSessionEndReason endReason, bool onPurpose)
        {
            if (endReason == NetworkSessionEndReason.Disconnected)
            {//network poblems, try again
                endReasonTimeOutNotConnectionLost = false;
                cancelConnection();
            }
            else
            { //impossible to join, quit to menu
                setState(JoiningState.QuitJoining);
            }
        }
        public override void NetworkAvailableSessionsUpdated(bool isAvailable, AvailableNetworkSessionCollection availableSessions)
        {
            if (isAvailable)
            {
                List<AvailableNetworkSession> sessions = Ref.netSession.SortAvailableSessions();
                foreach (AvailableNetworkSession session in sessions)
                {
                    if (session.HostGamertag == host)
                    {
                        tryJoin();
                        setState(JoiningState.Joining);
                    }
                }
            }
        }

        void updateJoiningText()
        {
            beginningOfAState.CountDown();
            bool dots = false;
            switch (state)
            {
                case JoiningState.Joining:
                    if (beginningOfAState.TimeOut)
                    {
                        text.TextString = "Connecting to " + host;
                        dots = true;
                    }
                    else
                    {
                        if (numTrials == 0)
                        {
                            text.TextString = "Joining game session";
                        }
                        else
                        {
                            if (endReasonTimeOutNotConnectionLost)
                            {
                                text.TextString = "Session found, trying again";
                            }
                            else
                            {
                                text.TextString = "Trying to connect again";
                            }
                        }
                    }
                    break;
                case JoiningState.RecievingData:
                    if (beginningOfAState.TimeOut)
                    {
                        text.TextString = "Recieving world data";
                        dots = true;
                    }
                    else
                    {
                        if (Ref.netSession.InviteWaiting)
                            text.TextString = "Accepted invite";
                        else
                            text.TextString = "Connected";
                    }
                    break;
                case JoiningState.SearchSessions:
                    if (endReasonTimeOutNotConnectionLost)
                    {
                        text.TextString = "Session not found, searching again";
                    }
                    else
                    {
                        text.TextString = "Connection lost, searching again";
                    }
                    break;
                case JoiningState.QuitJoining:
                    text.TextString = "Joining failed";
                    break;
            }
            if (dots)
            {
                if (dotTime.Update())
                {
                    numDots.Next();
                }
                for (int i = 0; i < numDots.Value; ++i)
                {
                    text.TextString += '.';
                }
            }

            text.Centertext(Align.CenterAll);
            //get
            //{
            //    return //"Connecting to " + host;
            //}
        }
        void setState(JoiningState newState)
        {

            if (this.state != newState)
            {
                
                receivingDataTimeOut.Reset();
                connectingTimeOut.Reset();
                this.state = newState;
                beginningOfAState = new Time(StateBeginningSec, TimeUnit.Seconds);
            }
        }


        public void Init()
        {
            createMenuStateBackg(true);

            text = new Graphics.TextG(LoadedFont.Lootfest, Engine.Screen.CenterScreen, new Vector2(0.8f),
              Align.CenterAll, null, Color.White, ImageLayers.Background5);
            updateJoiningText();
            Ref.draw.ClrColor = new Color(0, 31, 3);
        }
        //public override void NetworkConnectionLost(Network.AbsNetworkPeer host, NetworkSessionEndReason endReason, bool onPurpose)
        //{
        //    exit();
        //}

        Time exitTimer = new Time(1.6f, TimeUnit.Seconds);
        public override void Time_Update(float time)
        {
            updateJoiningText();

            if (state == JoiningState.Joining || state == JoiningState.SearchSessions)
            {
                if (connectingTimeOut.Update())
                {
                    endReasonTimeOutNotConnectionLost = true;
                    if (state == JoiningState.Joining)
                    {
                        cancelConnection();
                    }
                    else
                    { //cant find any sessions
                        setState(JoiningState.QuitJoining);
                    }
                }
            }
            else if (state == JoiningState.RecievingData)
            {
                if (receivingDataTimeOut.Update())
                {
                    endReasonTimeOutNotConnectionLost = true;
                    cancelConnection();
                }
            }
            else if (state == JoiningState.QuitJoining)
            {
                if (exitTimer.CountDown())
                {
                    exit();
                }
            }

            base.Time_Update(time);
        }

        void cancelConnection()
        {
            numTrials++;
            Ref.netSession.InviteWaiting = false;
            Ref.netSession.Disconnect();
            Ref.netSession.BeginUpdateAvailableSessions();
            setState(JoiningState.SearchSessions);
        }

        public override void Button_Event(ButtonValue e)
        {
            if (e.KeyDown && (e.Button == numBUTTON.B || e.Button == numBUTTON.Back))
            {
                Ref.netSession.CancelJoin();
                exit();
            }
        }
        public override Engine.GameStateType Type
        {
            get { return Engine.GameStateType.LoadingGame; }
        }
        void tryJoin()
        {
            if (Ref.netSession.InviteWaiting)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_InviteReady, Network.PacketReliability.Reliable, Ref.netSession.invitedPlayer);
                
            }
            else
            {
                Ref.netSession.JoinSelectedHost(host);
            }
        }
        void exit()
        {
            Ref.netSession.InviteWaiting = false;
            new MainMenuState(true);
        }
    }

    enum JoiningState
    {
        Joining,
        RecievingData,
        SearchSessions,
        QuitJoining,
        NUM
    }
    
}
