using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Steamworks;
using System;
using System.Collections.Generic;

namespace VikingEngine.PJ
{
    class FinalScoreState : Engine.GameState
    {
        Time readyTimer = new Time(2, TimeUnit.Seconds);
        List<GamerData> winnersInOrder;

        Display.MenuButton returnButton;
        bool fadeOutState = false;
        Time fadeTime = new Time(PjLib.FadeToBlackTime);
        static int SheepWinsInARow = 0;

        public FinalScoreState()
            : base()
        {
            if (Ref.steam.isInitialized)
            {
                SteamTimeline.SetTimelineGameMode(ETimelineGameMode.k_ETimelineGameMode_Staging);

                SteamTimeline.AddInstantaneousTimelineEvent(
                        "🏆",// Title in UI
                       null, // Description in UI
                       "steam_trophy", // Built-in Steam icon 
                       6, // Priority (0 = default, max= 1000)
                       0f,// Offset in seconds
                       ETimelineEventClipPriority.k_ETimelineEventClipPriority_Standard // Clip suggestion priority
               );
            }
            

            Input.Mouse.ViewAll();//Input.Mouse.Hide();//Input.Mouse.Visible = true;
            draw.ClrColor = new Color(137, 191, 245);//Color.CornflowerBlue;

            //Calc order
            List<GamerData> allPlayers = arraylib.MergeArrays(PjRef.storage.startingRemoteGamers, PjRef.storage.joinedGamersSetup);

            winnersInOrder = GamerData.OrderPlayers(allPlayers);
            PjRef.storage.previousVictor = winnersInOrder[0];

            //Display players
            int viewPlayersCount = Bound.Max(winnersInOrder.Count, 4);

            for (int i = 0; i < viewPlayersCount; ++i)
            {
                FinalScorePlayer player = new FinalScorePlayer(winnersInOrder[i], i);
            }
            
            returnButton = Display.MenuButton.ExitToLobbyButton();

            if (Ref.netSession.InMultiplayerSession)
            {
                if (PjRef.host)
                {
                    Ref.netSession.BeginWritingPacket(Network.PacketType.birdFinalScore, Network.PacketReliability.Reliable);
                }
            }
        }
        
        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (fadeOutState)
            {
                if (fadeTime.CountDown())
                {
                    bool scare = false;

                    bool sheepWinner = false;

                    if (PjRef.storage.modeSettings.avatarType == PjEngine.ModeAvatarType.Joust)
                    {
                        if (AnimalSetup.Get(winnersInOrder[0].joustAnimal).mainType == AnimalMainType.Sheep)
                        {
                            sheepWinner = true;
                        }
                    }

                    if (sheepWinner)
                    {
                        SheepWinsInARow++;
                        if (SheepWinsInARow == 2)
                        {
                            scare = true;
                        }
                    }
                    else
                    {
                        SheepWinsInARow = 0;
                    }

                    if (scare)
                    {
                        new GameState.WolfScare();
                    }
                    else
                    {
                        new LobbyState();
                    }
                    //
                    
                }
                return;
            }
            
            if (HudLib.ExitEndScoreInput())
            {
                exitToLobby();
            }

            if (returnButton.update())
            {
                exitToLobby();
            }
        }

        void exitToLobby()
        {
            if (Ref.netSession.InMultiplayerSession)
            {
                Ref.netSession.Disconnect(null);
            }

            fadeOutState = true;
            PjLib.BlackFade(false);
        }
    }
}
