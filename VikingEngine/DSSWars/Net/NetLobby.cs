using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Net;

namespace VikingEngine.DSSWars.Net
{
    class NetLobby : Network.NetLobby
    {
        Timer.Basic checkTimeout = new Timer.Basic(3000);
        //List2<LobbyButton> lobbies = new List2<LobbyButton>();
        List2<TrackedLobby> lobbiesSorted = new List2<TrackedLobby>();
        public NetLobby()
            : base()
        {
            searchLobbies = true;
            autoCreateSession = false;
        }

        public override void update()
        {
            base.update();

            //foreach (var m in lobbies)
            //{
            //    if (m.Update())
            //    {
            //        //connect
            //lockSession();
            //new ConnectState(m.session);
            //        return;
            //    }
            //}

            //if (checkTimeout.Update())
            //{
            //    if (lobbies.Count > 0)
            //    {
            //        SteamAvailableSession.RefreshServerTime();
            //        lobbies.loopBegin(false);
            //        while (lobbies.loopNext())
            //        {
            //            if (lobbies.sel.session.refreshAvailable() == false)
            //            {
            //                lobbies.sel.DeleteMe();
            //                lobbies.loopRemove();
            //            }
            //        }
            //    }
            //}
        }

        protected override void onEndedSession()
        {
            base.onEndedSession();
            applyNewSettings();
        }


        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);

            switch (message)
            {
                case Network.NetworkStatusMessage.Created_Lobby:
                    applyNewSettings();
                    break;
                //case Network.NetworkStatusMessage.Created_session:

                //    break;
            }

            Ref.gamestate.NetworkStatusMessage(message);
        }
        public override void NetEvent_GotNetworkId()
        {
            base.NetEvent_GotNetworkId();
        }

        public override void onNewGameState(Engine.GameState newState)
        {
            //clearLobbies();
            applyNewSettings();
        }

        //void clearLobbies()
        //{
        //    foreach (var m in lobbies)
        //    {
        //        m.DeleteMe();
        //    }
        //    lobbies.Clear();
        //}
        
        public override void NetEvent_SessionsFound(List<AbsAvailableSession> availableSessions/*, List<AbsAvailableSession> prevAvailableSessionsList*/)
        {
            Ref.gamestate.NetEvent_SessionsFound(availableSessions);
            //base.NetEvent_SessionsFound(availableSessions/*, prevAvailableSessionsList*/);

            //if (Ref.gamestate is LobbyState)
            //{
            //    Ref.gamestate.
            //    //foreach (var l in lobbiesSorted)
            //    //{
            //    //    l.available = false;
            //    //}


            //    //List<AbsAvailableSession> newLobbies, lostLobbies;

            //    //prevAvailableSessionsList = new List<AbsAvailableSession>(lobbies.Count);
            //    //foreach (var m in lobbies)
            //    //{
            //    //    prevAvailableSessionsList.Add(m.session);
            //    //}

            //    //filterNewAndOldLobbies(availableSessions, prevAvailableSessionsList, out newLobbies, out lostLobbies);

            //    //if (lostLobbies.Count > 0)
            //    //{
            //    //    for (int i = lobbies.Count - 1; i >= 0; --i)
            //    //    {
            //    //        if (lostLobbies.Contains(lobbies[i].session))
            //    //        {
            //    //            lobbies[i].DeleteMe();
            //    //            lobbies.RemoveAt(i);
            //    //        }
            //    //    }

            //    //    for (int i = 0; i < lobbies.Count; ++i)
            //    //    {
            //    //        lobbies[i].refreshPosition(i);
            //    //    }
            //    //}

            //    //if (newLobbies.Count > 0)
            //    //{
            //    //    //warsRef.sound.lobbyAppears.PlayFlat();
            //    //    foreach (var m in newLobbies)
            //    //    {
            //    //        var lb = new LobbyButton(m, lobbies.Count);
            //    //        lobbies.Add(lb);
            //    //    }
            //    //}
            //}
        }
        public override void NetEvent_PingReturned(AbsNetworkPeer gamer)
        {
            Ref.gamestate.NetEvent_PingReturned(gamer);
        }

        public override void NetEvent_PeerLost(AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerLost(gamer);
            DssRef.state?.NetEvent_PeerLost(gamer);
        }

        public override void NetEvent_ErrorMessage(string message, AbsNetworkPeer peer, bool peerIsSender)
        {
            DssRef.state?.NetEvent_ErrorMessage(message, peer, peerIsSender);
        }

        public override AbsLobbyMetaData NetEvent_StartLobbyMetaData()
        {
            return new LobbyMetaData();
        }

        //public override void NetEvent_PeerJoined(AbsNetworkPeer gamer)
        //{
        //    if (state == NetLobbyState.Lobby)
        //    {
        //        lockSession();
        //        new ConnectState(gamer);
        //    }

        //    //warsRef.storage.lastPlayedAgainst.Add(gamer.SteamUser(), 16);
        //}

        class TrackedLobby
        {
            public bool available = true;
            public AbsAvailableSession session;
        }

        //class LobbyButton
        //{
        //    VectorRect area;
        //    public AbsAvailableSession session;
        //    Graphics.ImageGroupParent2D images;
        //    Vector2 sz;
        //    Graphics.Image outline;

        //    public LobbyButton(AbsAvailableSession session, int index)
        //    {
        //        this.session = session;
        //        sz = new Vector2(6f, 1.6f) * Engine.Screen.IconSizeV2;
        //        Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, sz, ImageLayers.Background1);
        //        bg.Color = Color.DarkBlue;

        //        Graphics.TextG joinText = new Graphics.TextG(LoadedFont.Bold, Vector2.Zero,
        //            Vector2.One, Graphics.Align.Zero, "Join game:", Color.White, ImageLayers.AbsoluteBottomLayer);
        //        joinText.LayerAbove(bg);
        //        joinText.SetHeight(Engine.Screen.IconSize * 0.7f);

        //        Graphics.TextG nameText = new Graphics.TextG(LoadedFont.Console, VectorExt.V2FromY(sz.Y * 0.5f),
        //            Vector2.One, Graphics.Align.Zero, session.name, Color.Yellow, ImageLayers.AbsoluteBottomLayer);
        //        nameText.LayerAbove(bg);
        //        nameText.SetHeight(Engine.Screen.IconSize * 0.5f);

        //        images = new Graphics.ImageGroupParent2D(bg, joinText, nameText);

        //        VectorRect outlineArea = new VectorRect(Vector2.Zero, sz);
        //        outlineArea.AddRadius(4f);

        //        outline = new Graphics.Image(SpriteName.WhiteArea, outlineArea.Position, outlineArea.Size, ImageLayers.AbsoluteBottomLayer);
        //        outline.LayerBelow(bg);
        //        outline.Visible = false;

        //        images.Add(outline);

        //        refreshPosition(index);
        //    }

        //    public void refreshPosition(int index)
        //    {
        //        Vector2 pos = Engine.Screen.SafeArea.RightTop;
        //        pos.X -= sz.X;
        //        pos.Y += Engine.Screen.IconSize * 1.2f + index * (sz.Y + Engine.Screen.IconSize * 0.5f);

        //        images.ParentPosition = pos;

        //        area = new VectorRect(pos, sz);
        //    }

        //    public bool Update()
        //    {
        //        if (area.IntersectPoint(Input.Mouse.Position))
        //        {
        //            if (!outline.Visible)
        //            {
        //                //warsRef.sound.inGameCursormove.PlayFlat();
        //            }
        //            outline.Visible = true;
        //            if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
        //            {
        //                //warsRef.sound.inGameClick.PlayFlat(1.4f);
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            outline.Visible = false;
        //        }

        //        return false;
        //    }

        //    public void DeleteMe()
        //    {
        //        images.DeleteMe();
        //    }
        //}
    }
}
