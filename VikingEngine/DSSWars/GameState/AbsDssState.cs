using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameState
{
    class AbsDssState : Engine.GameState
    {
        public AbsDssState() 
            : base() 
        { }
        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            Ref.lobby?.update();
        }
    }
}
