using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars
{
    class MapFileGeneratorState : Engine.GameState
    {
        public const int MapCountPerSize = 16;

        MapSize loadingSz = 0;
        int loadingNumber = 1;

        Graphics.TextBoxSimple loadingStatusText;
        bool lockedInSaving = false;
        WorldData currentlyBuilding = null;
        bool complete = false;
        int failCount = 0;

        public MapFileGeneratorState()
           : base()
        {
            loadingStatusText = new Graphics.TextBoxSimple(LoadedFont.Regular,
                Engine.Screen.SafeArea.Position, 
                Engine.Screen.TextTitleScale,
                Graphics.Align.Zero, "Build Map", Color.White, ImageLayers.Lay1, Engine.Screen.Width * 0.6f);
            
            new Timer.AsynchActionTrigger(loadThread);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
                        
            if (complete)
            {
                loadingStatusText.TextString = 
                    "Complete!";
            }
            else
            {
                loadingStatusText.TextString = "Building " + loadingSz.ToString() + " map " +
                    loadingNumber.ToString() + ". Process: " + GenerateMap.LoadStatus.ToString() + "%" + Environment.NewLine +
                    "Failed maps: " + failCount.ToString();
                
            }
        }

        void loadThread()
        {
            if (StartupSettings.SaveLoadSpecificMap.HasValue)
            {
                loadingSz = StartupSettings.SaveLoadSpecificMap.Value;
                generateLoopUntilSucess(loadingSz, 1);
            }
            else
            {
                MapSize startSize = 0;
                for (loadingSz = startSize; loadingSz < MapSize.NUM; ++loadingSz)
                {
                    for (loadingNumber = 1; loadingNumber <= MapCountPerSize; ++loadingNumber)
                    {
                        generateLoopUntilSucess(loadingSz, loadingNumber);
                    }
                }
            }
            new Timer.TimedAction0ArgTrigger(()=> 
            {
                new ExitGamePlay();
            }, 5000);
            complete = true;


            void generateLoopUntilSucess(MapSize size, int number)
            {
                while (true)
                {
                    bool success = new GenerateMap().Generate(loadingSz, loadingNumber, true);

                    if (success)
                    {
                        return;
                    }
                    else
                    {
                        failCount++;
                    }
                }              
                
            }
        }

        
    }
}
