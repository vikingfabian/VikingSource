using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    abstract class AbsBallPower
    {
        public int activationCount = 1;

        /// <returns>Completed</returns>
        virtual public bool activate(Ball ball)
        {
            bool complete = --activationCount <= 0;
            return complete;
        }
    }

    class BumpPower : AbsBallPower
    {
        public override bool activate(Ball ball)
        {
            ball.bump();
            return base.activate(ball);
        }
    }
}
