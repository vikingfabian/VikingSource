using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//xna

namespace VikingEngine.Engine
{
    abstract class GameState : VikingEngine.AbsInput, VikingEngine.Network.INetworkUpdateReviever
    {
        /// <summary>
        /// Overriding another gamestate
        /// </summary>
        public GameState previousGameState = null;
        public Draw draw;
        protected Update update;
        public int UpdateCount = 0;

        public void GotFocus(GameState previousGameState)
        {
            if (previousGameState != null)
                this.previousGameState = previousGameState;

            if (Ref.draw != null)
            {
                Ref.draw.DeleteMe();
            }
            Ref.draw = draw;
            Ref.update = update;
            Ref.gamestate = this;

            if (Ref.main.criticalContentIsLoaded)
            {
                VikingEngine.Input.InputLib.OnGameStateChange();
            }
            if (Ref.lobby != null)
            {
                Ref.lobby.onNewGameState(this);
            }
        }

        virtual public void LostFocus()
        {

        }

        virtual public void OnDestroy()
        {
            Engine.Sound.StopAllLoopedSounds();            
        }
        
        public GameState()
            :this(true)
        {
            
        }

        public GameState(bool replaceState)
        {
            createDrawManager();
            createUpdateManager();
            Ref.SetGameSpeed(1f);

            if (replaceState)
            {
                Engine.StateHandler.ReplaceGamestate(this);
            }
            else
            {
                Engine.StateHandler.PushGamestate(this);
            }
           
        }

        virtual public void FirstUpdate()
        { }

        

        virtual protected void createDrawManager()
        {
            draw = new Draw();
        }
        virtual protected void createUpdateManager()
        {
            update = new Update(this);
        }
        
        virtual public bool UseInputEvents { get { return false; } }       

        PlayerView[] storedViews = null;
        public void StoreView(bool store)
        {

            if (store)
            {
                storedViews = new PlayerView[Engine.Draw.MaxScreenSplit];
                for (int i = 0; i < Engine.Draw.MaxScreenSplit; ++i)
                {
                    storedViews[i] = Engine.XGuide.GetOrCreatePlayer(i).view.Clone();
                }
            }
            else
            {
                for (int i = 0; i < Engine.Draw.MaxScreenSplit; ++i)
                {
                    Engine.XGuide.GetPlayer(i).view = storedViews[i];
                }
                storedViews = null;
            }
        }

        virtual public void GamerSignedInEvent(Engine.AbsApiGamer gamer) { }
        virtual public void GamerSignedOutEvent(Engine.AbsApiGamer gamer) { }
               
        virtual public void NetworkStatusMessage(Network.NetworkStatusMessage message)
        { }
        virtual public void NetEvent_PeerJoined(Network.AbsNetworkPeer peer)
        { }
        virtual public void NetEvent_PeerLost(Network.AbsNetworkPeer peer)
        { }

        virtual public void NetEvent_JoinedLobby(string name, ulong lobbyHost, bool fromInvite)
        { }
        virtual public void NetEvent_GotNetworkId()
        { }
        
        virtual public void NetworkReadPacket(Network.ReceivedPacket packet)
        { }

        virtual public void NetEvent_LargePacket(Network.ReceivedPacket packet) 
        { }

        virtual public void NetEvent_PingReturned(Network.AbsNetworkPeer peer)
        { }
        virtual public void NetEvent_ConnectionLost(string reason)
        { }
        virtual public void NetEvent_SessionsFound(
            List<Network.AbsAvailableSession> availableSessions, 
            List<Network.AbsAvailableSession> prevAvailableSessionsList)
        { }

        virtual public void OnAppSuspend(bool fullExit)
        { }
        //virtual public void onClosingApplication() { }
        virtual public void OnAppResume()
        { }

        virtual public void OnEnteredBackground(bool inBackground)
        { }
                              
        virtual public void OnResolutionChange()
        { }
        

        virtual public void NetUpdate()
        { }
        virtual public void GameCrashed()
        { }

        public bool IsActiveGameState { get { return Ref.gamestate == this; } }
    }
    enum GameStateType
    {
        LoadingContent,
        PressStart,
        MainMenu,
        LoadingGame,
        InGame,
        Editor,
        Other,
    }

}
