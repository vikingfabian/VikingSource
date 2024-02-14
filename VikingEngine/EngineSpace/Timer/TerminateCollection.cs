using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Timer
{
    class TerminateCollection: AbsTimer
    {
        List<IDeleteable> objects;

        public TerminateCollection(float lifeTime, List<IDeleteable> terminateObj)
            : base(lifeTime, UpdateType.Lazy)
        {
            objects = terminateObj;
            //this.baseInit(lifeTime);

        }
        protected override void timeTrigger()
        {
            foreach (IDeleteable obj in objects)
            { 
                if (obj != null)
                    obj.DeleteMe(); 
            }
        }
    
    
    }
}
