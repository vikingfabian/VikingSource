using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Interface.CutScene
{
    class ExitScene : AbsSaveScene
    {
        Engine.ExitGameStateThreads exitThreads;
        public ExitScene(Engine.ExitGameStateThreads exitThreads)
            : base()
        {
            this.exitThreads = exitThreads;

        }

        protected override string SaveString => DssRef.todoLang.Progress_ClosingCores;
        public override void Time_Update(float time)
        {
            progress.TextString = string.Format( SaveString, $"{exitThreads.startCount - exitThreads.currentCount}/{exitThreads.startCount}");
        }
    }
}
