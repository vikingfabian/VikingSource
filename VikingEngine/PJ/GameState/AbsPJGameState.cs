using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using VikingEngine.SteamWrapping;
using VikingEngine.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using VikingEngine.PJ.Lobby;

namespace VikingEngine.PJ
{
    class AbsPJGameState : Engine.GameState
    {
        bool isPlayState;
        protected VikingEngine.PJ.Display.PauseMenu pauseMenu = null;
        public VectorRect activeScreenArea;
        public VectorRect activeScreenSafeArea;

        public List2<GamerData> joinedLocalGamers;
        public int matchCount;
        public float timeSinceInput = 0;

       

        public AbsPJGameState(bool isPlayState)
            : base()
        {
            this.isPlayState = isPlayState;
            Ref.isPaused = false;
            activeScreenArea = Engine.Screen.Area;
            activeScreenSafeArea = Engine.Screen.SafeArea;
#if PCGAME
            if (isPlayState && Ref.steam.statsInitialized)
            {
                Ref.steam.stats.upload();
            }
#endif
            if (Ref.lobby == null)
            {
                new NetLobby();
            }
            Ref.lobby.EnterLobby(!isPlayState);
        }

        protected void set1080pScreenArea()
        {
            Vector2 res = Engine.Screen.RecordingPresetsResolution(Engine.RecordingPresets.YouTube1080p).Vec;

            float wToH = (float)Engine.Screen.Width / res.X * res.Y;
            float hToW = (float)Engine.Screen.Height / res.Y * res.X;

            if (wToH <= Engine.Screen.Height + 1)
            {
                activeScreenArea.Y = (Engine.Screen.Height - wToH) * 0.5f;
                activeScreenArea.Height = wToH;
            }
            else
            {
                activeScreenArea.X = (Engine.Screen.Width - hToW) * 0.5f;
                activeScreenArea.Width = hToW;
            }

            activeScreenSafeArea = activeScreenArea;
            activeScreenSafeArea.AddXRadius(-Engine.Screen.SafeArea.X);
            activeScreenSafeArea.AddYRadius(-Engine.Screen.SafeArea.Y);
        }

        public void exitToLobby()
        {
            if (Ref.netSession.InMultiplayerSession)
            {
                Ref.netSession.Disconnect(null);
            }
            new ExitToLobbyState();
        }

        protected bool pauseInput(out InputSource inputType)
        {
            if (Input.Keyboard.KeyDownEvent(Keys.Escape))
            {
                inputType = new InputSource( InputSourceType.Keyboard);
                return true;
            }

            foreach (XController controller in Input.XInput.controllers)
            {
                if (controller.Connected && 
                    (controller.KeyDownEvent(Buttons.Start) || controller.BackButtonDownEvent()))
                {
                    Engine.XGuide.LocalHostIndex = controller.Index;
                    inputType = new InputSource(InputSourceType.XController, controller.Index);
                    return true;
                }
            }

            checkIdlePlay();


            inputType = InputSource.Empty;
            return false;
        }

        protected bool tutorialSkipInput()
        {
            bool startInput = false;

            if (this.UpdateCount > 10)
            {   
                bool menuInput;
                Input.InputSource menuUser;

                PjLib.UpdateManagerInput(out startInput, out menuInput, out menuUser);
            }
            return startInput;
        }

        void checkIdlePlay()
        {
            foreach (var m in joinedLocalGamers)
            {
                if (m.button.IsDown)
                {
                    timeSinceInput = 0;
                    return;
                }
            }

            timeSinceInput += Ref.DeltaTimeSec; 
            if (timeSinceInput > 120)
            {
                new ExitToLobbyState();
            }
        }

        /// <returns>Overrides input</returns>
        protected bool baseClassUpdate()
        {
            updateLostControllers();

            InputSource inputType = InputSource.Empty;
            
            if (pauseMenu != null)
            {
                if (pauseMenu.Update())
                {
                    pause(inputType);
                }

                if (Ref.netSession.InMultiplayerSession == false)
                {
                    return true;
                }
            }
            else
            {
                if (pauseInput(out inputType))
                {
                    pause(inputType);
                }
            }

            return false;
        }

        public void pause(InputSource inputType)
        {
            if (pauseMenu == null)
            {
                createPauseMenu(inputType);
            }
            else
            {
                Engine.Sound.PauseAllLoopedSounds(false);
                if (hasMusic)
                {
                    MediaPlayer.Resume();
                }
                closeMenu();

                Input.Mouse.Visible = false;
            }
        }

        void createPauseMenu(InputSource inputType)
        {
            if (pauseMenu == null && isPlayState)
            {
                timeSinceInput = 0;
                MediaPlayer.Pause();
                Engine.Sound.PauseAllLoopedSounds(true);

                setMenuLayer();
                pauseMenu = new Display.PauseMenu(Engine.XGuide.LocalHostIndex, this, inputType);

                if (Ref.netSession.InMultiplayerSession == false)
                {
                    Ref.isPaused = true;
                }

                Input.Mouse.Visible = true;
            }
        }

        public void refreshControllersInUse()
        {
            foreach (var m in Input.XInput.controllers)
            {
                m.hasUser = false;
            }

            foreach (var m in joinedLocalGamers)
            {
                if (m.button.inputSource == InputSourceType.XController)
                {
                    Input.XInput.Instance(m.button.ControllerIndex).hasUser = true;
                }
            }
        }

        IntervalTimer lostControllerTimer = new IntervalTimer(500);

        protected void updateLostControllers()
        {
            if (lostControllerTimer.CountDown())
            {
                foreach (var m in Input.XInput.controllers)
                {
                    if (m.hasUser && !m.Connected)
                    {
                        onLostController();
                        return;
                    }
                }
            }
        }

        virtual protected void onLostController()
        {
            if (isPlayState)
            {
                if (pauseMenu == null)
                {
                    pause(PjRef.HostingPlayerSource);
                }

                if (pauseMenu.lostControllerDisplay == null)
                {
                    pauseMenu.createLostCtrlDispay();
                }
            }
        }

        public override void OnAppResume()
        {
            base.OnAppResume();
            createPauseMenu(PjRef.HostingPlayerSource);
        }

        public override void OnEnteredBackground(bool inBackground)
        {
            if (inBackground)
            {
                createPauseMenu(PjRef.HostingPlayerSource);
            }
        }

        protected void onInput()
        {
            timeSinceInput = 0;
        }

        protected void closeMenu()
        {
            if (pauseMenu != null)
            {
                setMenuLayer();
                pauseMenu.DeleteMe();
                pauseMenu = null;
                Ref.isPaused = false;
            }
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);

            if (packet.type == Network.PacketType.birdJoinedGamers)
            {
                PjLib.NetReadJoindedGamers(packet);
            }
        }

        public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerJoined(gamer);
            PjLib.checkHostStatus();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            Ref.lobby?.update();
        }

        virtual protected void setMenuLayer()
        { }

        virtual protected bool hasMusic
        {
            get { return true; }
        }
    }
}
