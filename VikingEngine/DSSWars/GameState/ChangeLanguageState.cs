using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;

namespace VikingEngine.DSSWars.GameState
{
    class ChangeLanguageState : Engine.GameState
    {
        public ChangeLanguageState() 
            :base()
        { }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            new Translation().setupLanguage(false);
            Ref.gamesett.Save();
            new LobbyState();
        }
    }
}
