using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Data
{
    class SaveStateManager
    {
        Data.FileManager filemanager;

        public SaveStateManager()
        {
            filemanager = new FileManager(null);
        }

        public void savestate()
        {
            filemanager.saveState();
        }

        public void loadstate()
        {
            filemanager.loadState();
        }
    }
}
