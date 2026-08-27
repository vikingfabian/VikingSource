using System;
using System.Collections.Generic;
using System.Linq;
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
        MapEditor2Display display;
        bool loadingState = false;
        public Map2Generator generator = new Map2Generator();
        public Map2GenerateSettings generateSettings = new Map2GenerateSettings();
        GeneratorMap map;
        public bool iconState = true;
        public MapEditor2_Scene()
            : base()
        {
            display = new MapEditor2Display(this);
            map = new GeneratorMap(display.topRight);
            new Interface.EditorBackground();
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
                        map.generateIcon(generator.iconWorld);
                    }
                }
            }
            else
            {
                bool mouseOverHud = false;
                display.update(ref mouseOverHud);

                map.userInput(mouseOverHud);
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
    }
}
