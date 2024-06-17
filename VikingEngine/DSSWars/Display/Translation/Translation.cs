using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.Display.Translation
{
    class Translation
    {
        public Translation()
        {
            //if (PlatformSettings.DevBuild)
            //{
            //    DssRef.lang = new SimplifiedChinese();
            //    Ref.langOpt= new HUD.OptionsLanguage_SimplifiedChinese();

            //    LoadContent.setFontLanguage(FontLanguage.Chinese);
            //}
            //else
            {
                DssRef.lang = new English();
                Ref.langOpt = new HUD.OptionsLanguage_English();
            }
            
        }
    }
}
