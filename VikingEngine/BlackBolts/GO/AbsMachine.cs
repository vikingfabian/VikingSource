using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.BlackBolts.GO
{
    abstract class AbsMachine : AbsGameObject
    {
        abstract public void AnimateUpdate();
        virtual public void OnCykleEnd() { }

        virtual public bool WalkableTile() { return true; }
    }
}
