using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameState
{
//    class PressStart : Engine.GameState, DataLib.IStorageSelectionComplete, IDialogueCallback
//    {
//        Dialogue dialogue;

//        public PressStart(List<AbsDraw> backGimages)
//            : base()
//        {
//            //IntroScene.IntroBackg();
//            foreach (AbsDraw img in backGimages)
//            {
//                Ref.draw.AddToRenderList(img, true);
//            }

//            DataLib.PromptStorageSelection.ResetDeviceSelection();
//            Graphics.TextG pressStartText = new Graphics.TextG(LoadedFont.Lootfest, 
//                new Vector2(Engine.Screen.CenterScreen.X,Engine.Screen.Height * 0.7f), new Vector2(1),
//                Align.CenterAll, "Press Start", Color.White, ImageLayers.TopLayer);

//            if (backGimages.Count > 0)
//            {
//                pressStartText.Transparentsy = 0;
//                new Graphics.Motion2d(MotionType.TRANSPARENSY, pressStartText, Vector2.One, MotionRepeate.NO_REPEATE, 600, true);
//            }

//            Ref.draw.ClrColor = new Color(0, 31, 3);

//#if WINDOWS
//            StorageSelectionCompleteEvent();
//#endif
            
//        }
//        public override void Button_Event(ButtonValue e)
//        {
//            if (e.KeyDown)
//            {
//                if (dialogue != null)
//                {

//                    dialogue.Click();

//                }
//                else
//                {
//                    if (e.KeyDown && (e.Button == numBUTTON.Start || e.Button == numBUTTON.A))
//                    {
//                        if (PlatformSettings.RunningWindows)
//                        {
//                            StorageSelectionCompleteEvent();
//                            return;
//                        }

//                        Engine.XGuide.LastLocalHost = e.PlayerIx;
//                        if (Engine.XGuide.GetPlayer(e.PlayerIx).SignedIn)
//                        {
//                            //bring up storage selection
//                            new DataLib.PromptStorageSelection(this, e.PlayerIx, true);

//                        }
//                        else
//                        {
//                            dialogue = new Dialogue(new HUD.DialogueData("", "You need to be signed in"), Engine.Screen.SafeArea, this);
//                        }
//                    }
//                }
//            }
//        }
//        public void DialogueClosedEvent()
//        {
//            dialogue = null;
//        }
//        public void StorageSelectionCompleteEvent()
//        {
//#if ARCADE
//            new SnakesTanks.GameState.MainMenu();
//#else
//            new MainMenuState(true);
//#endif       
//        }
//        public void StorageSelectionFailedEvent()
//        {
//            dialogue = new Dialogue(new HUD.DialogueData("", "This game requires a storage device"), Engine.Screen.SafeArea, this);
//            //reset the storage prompt

//        }
//        public override void Time_Update(float time)
//        {
//            base.Time_Update(time);
//        }
//        public override Engine.GameStateType Type
//        {
//            get { return Engine.GameStateType.PressStart; }
//        }
//    }
}
