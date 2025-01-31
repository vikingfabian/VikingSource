using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapEditor_Generator : AbsDssState
    {
        MapGeneratorDisplay display;

        public MapEditor_Generator()
            :base() 
        { 
            display = new MapGeneratorDisplay(this);
            new Display.EditorBackground();
        }

        public void generate()
        { 
            
        }

        public void startNewGame()
        {

        }
    }
}
