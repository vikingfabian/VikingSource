using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;

namespace VikingEngine.Core.BlackBolts.GO
{
    abstract class AbsMachine : AbsGameObject
    {
        abstract public void AnimateUpdate();
        virtual public void OnCykleEnd() { }

        virtual public bool WalkableTile() { return true; }

        public AbsMachine(PlaceObjectData placementData)
            : base(placementData)
        { }

        public override void DeleteMe()
        {
            base.DeleteMe();
            BlackRef.mapData.machineList.RemoveAt(pointer.objIndex);
        }
    }
}
