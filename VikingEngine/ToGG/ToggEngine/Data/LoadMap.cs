using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.Data
{
    class LoadMap
    {
        Commander.LevelSetup.GameSetup setup;
        Data.FileManager filemanager;
        Graphics.TextG text;

        public LoadMap(Commander.LevelSetup.GameSetup setup)
        {
            Ref.draw.ClrColor = Color.Black;
            text = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.CenterScreen,
                new Vector2(Engine.Screen.TextSize * 1.6f), Graphics.Align.CenterAll,
                "Loading " + setup.loadMap, Color.White, ImageLayers.Lay1);

            filemanager = new Data.FileManager(null);
            filemanager.loadFile(setup.loadMap, false);

            this.setup = setup;
        }
        
        public void DeleteMe()
        {
            text.DeleteMe();
        }

    }
}
