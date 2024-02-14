using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class Terminator : AbsTimer
    {
        IDeleteable obj;
        Graphics.IDrawContainer addToContainer;
        int renderLayer;

        public Terminator(float lifeTime, IDeleteable terminateObj)
            : this(lifeTime, terminateObj, UpdateType.Lazy)
        { }

        public Terminator(float lifeTime, IDeleteable terminateObj, UpdateType lasyUpdate)
            :base(lifeTime, lasyUpdate)
        {
            obj = terminateObj;
            renderLayer = Ref.draw.CurrentRenderLayer;
            addToContainer = Ref.draw.AddToContainer;
            //this.baseInit(lifeTime);
          //  System.Diagnostics.Debug.WriteLine("Terminator Init, "+ terminateObj.ToString());
        }
        protected override void timeTrigger()
        {
            Graphics.IDrawContainer currentContainer = Ref.draw.AddToContainer;
            Ref.draw.AddToContainer = addToContainer;
            int lay = Ref.draw.CurrentRenderLayer;
            Ref.draw.CurrentRenderLayer = renderLayer;
            obj.DeleteMe();
            Ref.draw.CurrentRenderLayer = lay;
            Ref.draw.AddToContainer = currentContainer;
            //DeleteMe();
        }
        public override string ToString()
        {
            return "Terminator, time" + this.timeLeft.ToString();
        }
    }
}
