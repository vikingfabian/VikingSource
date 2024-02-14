using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.Players
{
    partial class Player
    {
        Display.DebugTerrainGenDisplay debugTerrainGenDisplay = null; 


        void pageDebug() { }

        public Map.WorldPosition spawnNextToHeroPos
        {
            get {
                var wp = hero.WorldPos;
                wp.WorldGrindex.X -= Ref.rnd.Int(10, 14);
                wp.WorldGrindex.Z += Ref.rnd.Int(-5, 5);
                wp.SetAtClosestFreeY(2);
                return wp;
            }
        }
        public Map.WorldPosition spawnItemOnHeroPos
        {
            get
            {
                var wp = hero.WorldPos;
                wp.WorldGrindex.X -= 2;
                wp.SetAtClosestFreeY(2);
                return wp;
            }
        }

        

        public void debugJumpTo(TeleportLocationId location)
        {
            CloseMenu();
            hero.teleportToLocation(location);
            //LfRef.levels.GetLevel(Map.WorldLevelEnum.Lobby, hero, hero.startInLevel, false);
            
        }
        

        //string debugChunkText()
        //{
        //    Map.WorldPosition wp = new Map.WorldPosition( hero.Position);
        //    IntVector2 chunkGix = wp.ChunkGrindex;
        //    string result = "chunk info: ";  // x" + chunkGix.X.ToString() + "y" + chunkGix.Y.ToString();

        //    Map.Chunk c = LfRef.chunks.GetScreen(chunkGix);
        //    if (c == null)
        //    {
        //        result += "null";
        //    }
        //    else
        //    {
        //        result += " p" + dataOriginShort(c.PreparedDataOrigin) + "d" + dataOriginShort(c.DataOrigin);
        //    }

        //    return result;
        //}

        static string dataOriginShort(Map.ChunkDataOrigin cdo)
        {
            switch (cdo)
            {
                default:
                    return "0";

                case Map.ChunkDataOrigin.Generated:
                    return "g";
                case Map.ChunkDataOrigin.Loaded:
                    return "l";
                case Map.ChunkDataOrigin.RecievedByNet:
                    return "n";

            }
        }

        //void debugMusic()
        //{
        //    Engine.LoadContent.LoadAndPlaySongThreaded("The Lazy Frog");
        //}
        
      
        void debugChangeChunk()
        {
            Map.WorldPosition wp = hero.WorldPos;
            wp.Y = LfRef.chunks.GetScreen(wp).GetHighestYpos(wp);
            IntVector3 sz = new IntVector3(2);
            Map.Terrain.AlgoObjects.AlgoLib.FillRectangle(wp, sz, (byte)Data.MaterialType.bricks_red);
            Map.World.ReloadChunkMesh(wp, sz, false);
        }

        

        //Map.WorldPosition debugSpawnPosition()
        //{
        //    Map.WorldPosition wp = hero.WorldPos;
        //    wp.WorldGrindex.X += 8;
        //    return wp;
        //}
    }
}
