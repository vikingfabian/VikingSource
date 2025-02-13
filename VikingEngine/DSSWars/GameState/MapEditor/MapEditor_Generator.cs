using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapEditor_Generator : AbsDssState
    {
        public Map.Generate.MapGenerateSettings GenerateSettings;
        MapGeneratorDisplay display;
        public MapEditorUserStorage storage;
        GeneratorMap map;
        MapBackgroundLoading mapBackgroundLoading;
        bool loadingState = false;
        public MapEditor_Generator()
            :base() 
        {
            storage = new MapEditorUserStorage();
            GenerateSettings = new Map.Generate.MapGenerateSettings();
            display = new MapGeneratorDisplay(this);
            new Display.EditorBackground();
            map = new GeneratorMap(display.topRight);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (loadingState)
            {
                mapBackgroundLoading.Update();
                if (mapBackgroundLoading.Complete())
                {
                    loadingState = false;
                    map.generate();
                }
            }
            else
            {
                bool mouseOverHud = false;
                display.update(ref mouseOverHud);

                map.userInput(mouseOverHud);                
            }
        }

        public void generate()
        {
            loadingState = true;
            mapBackgroundLoading = new MapBackgroundLoading(null);
        }

        public void generate_clear()
        {

        }

        public void generate_paintBuild()
        {

        }

        public void generate_paintDig()
        {

        }


        public void startNewGame()
        {

        }
    }
}
