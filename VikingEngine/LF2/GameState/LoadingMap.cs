using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using System.Threading;

namespace VikingEngine.LF2.GameState
{
    class LoadingMap : Engine.GameState
    {
        bool newWorld;
        Graphics.TextG text;
        Graphics.TextBoxSimple infoText;
        const float DotFreq = 1000;
        float addDot = DotFreq;

        public LoadingMap() //joining from Live
            : base()
        {
            newWorld = false; //get towns from host instead
            Init(); 
        }

        public LoadingMap(Data.WorldSummary loadWorld)
            : base()
        {
            Ref.netSession.AbortFindSessions();
            newWorld = loadWorld.NewWorld;
            Init();
            Data.WorldsSummaryColl.CurrentWorld = loadWorld;
        }

        public void Init()
        {
            Ref.draw.ClrColor = Color.Black;

            infoText = new TextBoxSimple(LoadedFont.PhoneText, Engine.Screen.SafeArea.Position, new Vector2(0.8f), Align.Zero,
               newWorld? "Generating a new world, will take about 10sec" : "Loading map", Color.White, ImageLayers.Lay3, Engine.Screen.SafeArea.Width);

            text = new TextG(LoadedFont.PhoneText, infoText.Position, new Vector2(0.5f), Align.Zero,
               "", Color.White, ImageLayers.Lay5);
            text.Ypos += infoText.MesureText().Y + 20;
            
            Thread t = new Thread(loadThreaded);
            t.Name = "Lootfest loading map";
            t.Start();
        }

        bool doneLoading = false;
        float startDelay = 1;//lib.SecondsToMS(10);
        public override void Time_Update(float time)
        {
            if (PlatformSettings.DebugOptions)
            {
                PlayState.DebugWarpLocations = new List<Map.Terrain.Area.AbsArea>();
            }
            base.Time_Update(time);
            if (startDelay <= 0)
            {
                PlayState play = new PlayState();
                play.LoadGame(WorldMap);
                return;
            }
            else if (doneLoading)
            {
                startDelay -= time;
            }
            //else
            //{
                addDot -= time;
                if (addDot <= 0)
                {
                    text.TextString += ".";
                    addDot = DotFreq;
                }
            //}
        }
        Map.World WorldMap;
        void loadThreaded()
        {
            //Network.NetLib.NewWorld();

            GameObjects.Magic.MagicLib.NewWorld();
            Map.Terrain.TerrainLib.Init();

            WorldMap = new Map.World(newWorld, true);//mt
            if (Map.World.RunningAsHost)
            {
                LfRef.worldOverView.Load(Data.WorldsSummaryColl.CurrentWorld.FolderPath); 
            }
#if !CMODE
            GameObjects.Characters.EggNest.NewWorld();
            GameObjects.NPC.BasicNPC.LoadContent();
#endif
            
            doneLoading = true;
            //;
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
           // Network.PacketType type = (Network.PacketType)packet.r.ReadByte();
        }
        public override Engine.GameStateType Type
        {
            get { return Engine.GameStateType.LoadingGame; }
        }
    }
}
