using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using System.Threading;
//using System.Runtime.InteropServices;

//[return: MarshalAs(UnmanagedType.Bool)]
//[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer); //Used to use ref with comment below
//                                                                            // but ref doesn't work.(Use of [In, Out] instead of ref
//                                                                            //causes access violation exception on windows xp
//                                                                            //comment: most probably caused by MEMORYSTATUSEX being declared as a class
//                                                                            //(at least at pinvoke.net). On Win7, ref and struct work.

//// Alternate Version Using "ref," And Works With Alternate Code Below.
//// Also See Alternate Version Of [MEMORYSTATUSEX] Structure With
//// Fields Documented.
//[return: MarshalAs(UnmanagedType.Bool)]
//[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GlobalMemoryStatusEx", SetLastError = true)]
//static extern bool _GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

namespace VikingEngine.LootFest.GameState
{



    class LoadingMap : Engine.GameState
    {
        Graphics.TextG text;
        Graphics.TextBoxSimple infoText;
        const float DotFreq = 1000;
        float addDot = DotFreq;
        VikingEngine.LootFest.Data.WorldData worldData;

        public LoadingMap(VikingEngine.LootFest.Data.WorldData worldData )
            : base()
        {

           

            this.worldData = worldData;

            if (worldData.hostingWorld)
            {
                LfRef.storage.resetSelectStorageLock();
            }
            Engine.XGuide.UnjoinAll();
            Engine.XGuide.LocalHost.IsActive = true;
            //Init();
        }

        //public void Init()
        //{
        //    Ref.draw.ClrColor = Color.Black;

        //    infoText = new TextBoxSimple(LoadedFont.PhoneText, Engine.Screen.SafeArea.Position, VectorExt.V2(0.8f), Align.Zero,
        //       "Loading map", Color.White, ImageLayers.Lay3, Engine.Screen.SafeArea.Width);

        //    text = new TextG(LoadedFont.PhoneText, infoText.Position, VectorExt.V2(0.5f), Align.Zero,
        //       "", Color.White, ImageLayers.Lay5);
        //    text.Ypos += infoText.MeasureText().Y + 20;
            

        //    //Thread t = new Thread(loadThreaded);
        //    //t.Name = "Lootfest loading map";
        //    //t.Start();
        //}

        //bool doneLoading = false;
        //float startDelay = 1;//lib.SecondsToMS(10);
        public override void Time_Update(float time)
        {
            //if (PlatformSettings.DebugOptions)
            //{
            //    PlayState.DebugWarpLocations = new List<Map.Terrain.Area.AbsArea>();
            //}
            base.Time_Update(time);
            //if (startDelay <= 0)
            //{

            switch (UpdateCount)
            {
                case 2:
                    //Clear RAM
                    LfRef.ClearRAM();
                    break;
                case 4:
                    GC.Collect();
//#if PCGAME
//                    var memStored = GC.GetTotalMemory(false);

//                    var performance = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
//                    var ram = performance.NextValue();

//                    Debug.Log("RAM (MB): " + (ram).ToString());
//                    Debug.Log("GARBAGE (MB): " + (memStored/1000000.0).ToString());
//#endif
                    var WorldMap = new Map.World(worldData);
                    PlayState play = new PlayState();
                    play.LoadGame(WorldMap, worldData);
                    break;
            }
            //}
            //else if (doneLoading)
            //{
            //    startDelay -= time;
            //}
            ////else
            ////{
            //    addDot -= time;
            //    if (addDot <= 0)
            //    {
            //        text.TextString += ".";
            //        addDot = DotFreq;
            //    }
            ////}

                LfRef.net.update();
        }

        //Map.World WorldMap;
        //void loadThreaded()
        //{
            
        //    doneLoading = true;
        //}

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
           // Network.PacketType type = (Network.PacketType)packet.r.ReadByte();
        }
        
    }
}
