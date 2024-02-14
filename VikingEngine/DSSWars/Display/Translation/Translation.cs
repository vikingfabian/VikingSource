using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class Translation
    {
        public Translation()
        {
            if (PlatformSettings.DevBuild)
            {
                DssRef.lang = new Swedish();
                Ref.langOpt= new HUD.OptionsLanguage_Swedish();
            }
            else
            {
                DssRef.lang = new English();
                Ref.langOpt = new HUD.OptionsLanguage_English();
            }
            
        }
    }
}
