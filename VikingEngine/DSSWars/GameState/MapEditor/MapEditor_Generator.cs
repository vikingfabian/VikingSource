using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapEditor_Generator : AbsDssState
    {
        public Map.Generate.MapGenerateSettings GenerateSettings;
        MapGeneratorDisplay display;
        public MapEditorUserStorage storage;

        public MapEditor_Generator()
            :base() 
        {
            storage = new MapEditorUserStorage();
            GenerateSettings = new Map.Generate.MapGenerateSettings();
            display = new MapGeneratorDisplay(this);
            new Display.EditorBackground();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            bool mouseOverHud = false;
            display.update(ref mouseOverHud);
        }

        public void generate()
        { 
            
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
