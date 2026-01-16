using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameState
{
    class ChangeLanguageRefresh : ExitToLobby
    {
        public ChangeLanguageRefresh() 
            :base(true)
        {
        }

        //public override void Time_Update(float time)
        //{
        //    base.Time_Update(time);

        //    new Presentation.Translation().setupLanguage(false);
        //    Ref.gamesett.Save();
        //    new LobbyState();
        //}
        protected override void launch()
        {
            new Presentation.Translation().setupLanguage(false);
            Ref.gamesett.Save();
            base.launch();
        }
    }
}
