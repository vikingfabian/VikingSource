using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.MapEditor;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    class MapEditor2_Scene : AbsDssState
    {
        public MapEditor2Display display;
        public MapEditor3_Tool tool;
        bool loadingState = false;
        public Map2Generator generator = new Map2Generator();
        public Map2GenerateSettings generateSettings = new Map2GenerateSettings();
        public GeneratorMap map;
        public bool iconState = true;

        List<InputMap> controller;

        public MapEditor2_Scene()
            : base()
        {
            display = new MapEditor2Display(this);
            tool = new MapEditor3_Tool(this);
            map = new GeneratorMap(display.topRight);
            new Interface.EditorBackground();

            controller = new List<InputMap>{
                Ref.gamesett.keyboardMap,
            };
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (loadingState)
            {
                if (generator.complete())
                {
                    loadingState = false;
                    display.loadingDisplay.Hide();
                    display.refreshMenu();

                    if (generator.currentPass <= Map2Pass.NewWorld)
                    {
                        map.hide();
                    }
                    else if (generator.currentPass < Map2Pass.Icon)
                    {
                        map.generateNodes(generator.nodeMap);
                    }
                    else
                    {
                        map.generateIcon(generator.ActiveIconWorld);
                        if (generator.currentPass >= Map2Pass.ScaleUp)
                        {
                            map.scale = 0.25f;
                        }
                    }
                }
            }
            else
            {
                bool mouseOverHud = false;
                display.update(ref mouseOverHud);

                if (!mouseOverHud)
                {
                    foreach (var input in controller)
                    {
                        map.userInput(input, mouseOverHud);

                        tool.paintInput(input);
                    }
                }
            }
        }

        bool redrawLock = false;
        public void redrawPixels()
        {
            if (display.tab == Map2GeneratorTab.Bioms && generator.currentPass < Map2Pass.Bioms)
            {
                generator.currentPass = Map2Pass.Bioms;
            }

            if (!redrawLock)
            {
                redrawLock = true;

                Task.Run(() =>
                {
                    generator.processTexturePixels();
                    map.refreshTexture(generator.ActiveIconWorld);
                    redrawLock = false;
                });
            }
        }

        public void generatePass(Map2Pass start, Map2Pass end)
        {
            if (start <= Map2Pass.Empty)
            {
                map.resetPos();
            }

            loadingState = true;
            display.loadingDisplay.Show();
            generator.generatePass(generateSettings, start, end);
        }

        public void revertToIconPass()
        {
            loadingState = true;
            display.loadingDisplay.Show();
            generator.revertToIconPass();
        }

        public void generateHeightMap()
        {
            loadingState = true;
            display.loadingDisplay.Show();
            generator.generateHeightMap();
        }
    }
}
