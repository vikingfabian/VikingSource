using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class VisibleTrigger: AbsTimer
    {
        Graphics.AbsDraw image;
        bool turnVisible;
        
        public VisibleTrigger(float lifeTime, Graphics.AbsDraw _image, bool toVisible)
            : base(lifeTime, UpdateType.Lazy)
        {
            turnVisible = toVisible;
            image = _image;
            //this.baseInit(lifeTime);

        }
        protected override void timeTrigger()
        {
            image.Visible = turnVisible;
        }
    }
}

