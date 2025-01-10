using System;
using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//xna

namespace VikingEngine.Engine
{
    static class XGuide
    {
        public static int LocalHostIndex = 0;
        static MainGame mainGame = null;
        public static List<PlayerData> players = new List<PlayerData>(4);
        static bool waitingKeyInput = false;
        static KeyboardInputValues keyInputValues;
        static int overridePlayerInput = 0;
        public static bool OverridePlayerInput { get { return overridePlayerInput > 0; } }
        public static bool UseQuickInputDialogue = true;

        public static void Init(MainGame inMainGame)
        {
            mainGame = inMainGame;
            players.Add(new PlayerData(0));
        }

        public static PlayerData LocalHost
        {
            get { return players[(int)LocalHostIndex]; }
        }

        public static String GamerName
        {
            get 
            {
#if PCGAME
                if (Ref.steam.isInitialized)
                {
                    return Valve.Steamworks.SteamAPI.SteamFriends().GetPersonaName();
                }
#endif
                return "Player";
            }
        }
        
        public static PlayerData GetPlayer(int ix)
        {
            return players[ix];
        }

        public static PlayerData GetOrCreatePlayer(int ix)
        {
            while (players.Count <= ix)
            {
                players.Add(new PlayerData(players.Count));
            }
            return players[ix];
        }
        
        public static void SetPlayer(PlayerData p)
        {
            players[(int)p.localPlayerIndex] = p;
        }

        public static void BeginKeyBoardInput(KeyboardInputValues values)
        {
            //waitingKeyInput = true;
            keyInputValues = values;
            new SteamWrapping.SteamInput(values.Description, values.DefaultText);
            //TextBox textBox = new TextBox();
            //textBox.Location = new Point(10, 10);
            //textBox.Width = 200;
            //textBox.TextChanged += (sender, args) =>
            //{
            //    Console.WriteLine($"Text changed: {textBox.Text}");
            //};
            //this.Controls.Add(textBox);
        }

        public static void TextInputEvent(string input)
        {
            if (input == null)
            {
                Ref.gamestate.TextInputCancelEvent(keyInputValues.PlayerIndex);
            }
            else
            {
                if (keyInputValues.callBack == null)
                    Ref.gamestate.TextInputEvent(keyInputValues.PlayerIndex, input, keyInputValues.Link);
                else
                    keyInputValues.callBack(keyInputValues.PlayerIndex, input, keyInputValues.Link);
            }
        }

        public static bool InOverlay
        {
            get
            {
#if PCGAME
                return Ref.steam.inOverlay;
#else
                return false;
#endif
            }
        }

        public static void OnSuspend(bool fullExit)
        {
            Input.InputLib.OnGameStateChange();
            Ref.gamestate.OnAppSuspend(fullExit);
        }
        public static void OnResume()
        {
            Ref.gamestate.OnAppResume();
        }

        public static void OnEnteredBackground(bool inBackground)
        {
            Ref.gamestate.OnEnteredBackground(inBackground);
        }



        public static void Update()
        {
            if (VikingEngine.Input.InputLib.inPopupWindow || InOverlay)
            {
                overridePlayerInput = 3;
            }
            else
            {
                overridePlayerInput--;
            }


            if (waitingKeyInput)
            {   
                waitingKeyInput = false;
                VikingEngine.Input.InputLib.inPopupWindow = true;
//#if PCGAME
//                HUD.TextInputForm dialogueForm = new HUD.TextInputForm(keyInputValues.DefaultText);

//                if (dialogueForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//                {
//                    if (keyInputValues.callBack != null)
//                    {
//                        keyInputValues.callBack(keyInputValues.PlayerIndex, dialogueForm.Result, keyInputValues.Link);
//                    }
//                    else
//                    {
//                        Ref.gamestate.TextInputEvent(keyInputValues.PlayerIndex, dialogueForm.Result, keyInputValues.Link);
//                    }
//                }

//                VikingEngine.Input.InputLib.inPopupWindow = false;
//                dialogueForm.Dispose();
//#endif
            }
        }
        
        public static void UnjoinAll()
        {
            foreach (var m in players)
            {
                m.IsActive = false;
            }
        }
       

    }

    public struct KeyboardInputValues
    {
        //public string Title;
        public string Description;
        public string DefaultText;
        public int PlayerIndex;
        public int Link;
        public TextInputEvent callBack;

      
        public KeyboardInputValues( string desciption, string defaultText, int playerIx)
            : this(desciption, defaultText, playerIx, 0, null)
        { }
        public KeyboardInputValues(string desciption, string defaultText, int playerIx, 
            int link, TextInputEvent callBack)
        {
            //Title = title;
            Description = desciption;
            DefaultText = defaultText;

            PlayerIndex = playerIx;

            this.Link = link;
            this.callBack = callBack;
        }
    }

    //public enum ErrorType
    //{
    //    Other,
    //    IllegalInput,
    //    EmptyValue,
    //}
}
