using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameState
{
    class SeedGenerator2 : Engine.GameState
    {
        int page = 32;
        int currentSeedIx = 0;

        List<byte> seed;
        DataLib.SaveLoad save;
        Graphics.TextS text;

        public SeedGenerator2()
            : base()
        {
            save = new DataLib.SaveLoad();
            seed = new List<byte>();
            text = new Graphics.TextS(LoadedFont.Lootfest, new Vector2(200), Vector2.One, Align.Zero,
                "Seedgen", Color.White, ImageLayers.Foreground1);
        }
        public override void Button_Event(ButtonValue e)
        {
            for (int i = 0; i < 32; i++)
            {
                seed.Add((byte)Ref.rnd.Int(byte.MaxValue));
                currentSeedIx++;
                if (currentSeedIx >= Data.RandomSeed.SeedFileLength)
                {
                    currentSeedIx = 0;
                    //print the old list
                    DataLib.SaveLoad.SaveByteArray(Path(page), seed.ToArray());
                    seed.Clear();
                    page++;
                }
            }
            text.TextString = "ix" + currentSeedIx.ToString() + ", page" + page.ToString();
        }
        public static string Path(int seedIx)
        {
            return "seed" + seedIx.ToString() + ".sed";
        }

        public override Engine.GameStateType Type
        {
            get { return Engine.GameStateType.Other; }
        }
    }
    //class SeedGenerator : Engine.GameState
    //{
    //    int page = 8;
    //    int currentSeedIx = 0;

    //    List<string> seed;
    //    DataLib.SaveLoad save;
    //    Graphics.TextS text;

    //    public SeedGenerator()
    //        :base()
    //    {
    //        save = new DataLib.SaveLoad();
    //        seed = new List<string>();
    //        text = new Graphics.TextS(LoadedFont.Lootfest, new Vector2(200), Vector2.One, Align.Zero, 
    //            "Seedgen", Color.White, ImageLayers.Foreground1);
    //    }
    //    public override void Button_Event(ButtonValue e)
    //    {
    //        for (int i = 0; i < 5; i++)
    //        {
    //            seed.Add(Ref.rnd.Int(byte.MaxValue).ToString());
    //            currentSeedIx++;
    //            if (currentSeedIx >= byte.MaxValue)
    //            {
    //                currentSeedIx = 0;
    //                //print the old list
    //                save.CreateTextFile("seed" + page.ToString() + ".txt", seed);
    //                seed.Clear();
    //                page++;
    //            }
    //        }
    //        text.TextString = "ix" + currentSeedIx.ToString() + ", page" + page.ToString() ;
    //    }
    //    public override Engine.GameStateType Type
    //    {
    //        get { return Engine.GameStateType.Other; }
    //    }
    //}
}
