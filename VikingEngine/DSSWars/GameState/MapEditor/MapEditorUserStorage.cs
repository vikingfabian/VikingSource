using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapEditorUserStorage
    {

        public bool viewAdvancedSettings = false;
        public bool ViewAdvancedProperty(int index, bool set, bool value)
        {
            if (set)
            {
                viewAdvancedSettings = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return viewAdvancedSettings;
        }
    }
}
