using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars
{
    class VoxelModelInstance_Pooled : VoxelModelInstance
    {
        bool allowRecycle;
        public VoxelModelInstance_Pooled(bool allowRecycle)
            : base(null, false)
        {
            this.allowRecycle = allowRecycle;
        }

        public override void OnDrawBatchRemove()
        {
            if (allowRecycle)
            {
                visible = false;
                DssRef.state.voxelModelInstancesPooled.Push(this);
            }
        }

        public void Pool_Reset()
        {
            visible = true;
            Frame = 0;
            SpottedArrayMemberIndex = -1;
            inPlayerCamera = EightBit.AllTrue;
            master = null;
            Rotation = RotationQuarterion.Identity;
        }

        public override void DeleteMe()
        {
            //base.DeleteMe();
            preRemoveFromDrawBatch();
        }
    }
}
