using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameState
{
    //class SaveChunks : Update
    //{
        
    //    Data.WorldSummary worldSummary;
    //   // List<Map.Chunk> openScreensList;
    //    int currentScreen = 0;
    //    string text;

    //    public override string ToString()
    //    {
    //        return text;
    //    }
    //    public SaveChunks(Data.WorldSummary worldSummary)
    //        : base(true)
    //    {
    //        this.worldSummary = worldSummary;
    //        //openScreensList = LfRef.chunks.OpenChunksList;
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        if (!DataLib.SaveLoad.GetStreamIsOpen)
    //        {



    //            if (currentScreen >= openScreensList.Count)
    //            {
    //                //done
    //               // worldSummary.Save(true, true);
    //               // new MainMenuState();
    //                DeleteMe();

    //            }
    //            else
    //            {
    //                while (currentScreen < openScreensList.Count)
    //                {
    //                    if (openScreensList[currentScreen].UnSavedChanges)
    //                    {
    //                        openScreensList[currentScreen].CloseAndSave();
    //                        text = "Saving Map: Chunk" + (currentScreen + 1).ToString() + "/" + openScreensList.Count.ToString();
    //                        currentScreen++;
    //                        break;
    //                    }
    //                    currentScreen++;
    //                }

    //            }
    //        }

    //    }
    //}
    //class SaveAndExit :Engine.GameState
    //{
    //    TextG text;
    //    Timer.Basic dotTimer = new Timer.Basic(1000);

    //    public SaveAndExit()//Data.WorldSummary worldSummary, PlayState oldState, List<Players.Player> activePlayers)
    //        //:base(true)
    //       // : base(Map.World.RunningAsHost)
    //    {
    //        text = new TextG(LoadedFont.Lootfest, Engine.Screen.CenterScreen, new Vector2(1), Align.CenterAll,
    //            "Closing world", Color.White, ImageLayers.Foreground1);
    //    }
        
    //    public override void Time_Update(float time)
    //    {
    //        if (dotTimer.Update(time))
    //        {
    //            dotTimer.Reset();
    //            text.TextString += ".";
    //        }
           
    //    }
    //    public override Engine.GameStateType Type
    //    {
    //        get { return Engine.GameStateType.LoadingGame; }
    //    }
    //}
    //enum SavePart
    //{
    //    Chunks,
    //    Players,
    //    Done,
    //}
}
