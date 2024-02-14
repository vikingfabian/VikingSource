using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.MapEditor;

namespace VikingEngine.ToGG
{
    static class toggRef
    {
        public static GameMode mode;
        public static bool UnitsBlockLOS;
        public static IntVector2 worldsize
        {
            get { return board.tileGrid.Size; }
        }
        public static AbsPlayState gamestate;
        public static  EditorState editor;

        public static AbsPlayerCollection absPlayers;

        public static MenuSystem menu;
        public static ToggEngine.Map.Board board;
        public static ToggEngine.Map.SquareDic sq;
        public static ToggEngine.Data.Achievements achievements;
        
              
        public static Storage.GameStorage storage;
        public static int Seed = Ref.rnd.Int(ushort.MaxValue);

        public static InputMap inputmap;
        public static ToggEngine.Display2D.AbsPlayerHUD hud;
        public static ToggEngine.Camera cam;

        public static bool InEditor
        { get { return Ref.gamestate is EditorState; } }

        public static bool InRunningGame
        {
            get { return gamestate.gameHasStarted; }
        }

        public static void ClearMEM()
        {
            //units = null;
            Commander.cmdRef.ClearMEM();
            HeroQuest.hqRef.ClearMEM();
            absPlayers = null;
            editor = null;
        }

        public static void WriteSeed(System.IO.BinaryWriter w)
        {
            w.Write((ushort)Seed);
        }

        public static void ReadSeed(System.IO.BinaryReader r)
        {
            Seed = r.ReadUInt16();
        }

        public static void NewSeed()
        {
            Seed = Ref.rnd.Int(ushort.MaxValue);
        }

        public static ToggEngine.Map.BoardSquareContent Square(IntVector2 pos)
        {
            ToggEngine.Map.BoardSquareContent sq;
            if (board.tileGrid.TryGet(pos, out sq))
            {
                return sq;
            }

            return null;
        }

        public static bool NetHost
        {
            get
            {
                if (mode == GameMode.HeroQuest)
                {
                    return HeroQuest.hqRef.netManager.host;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    enum GameMode
    {
        Commander,
        HeroQuest,
        //Editor,
    }
}
