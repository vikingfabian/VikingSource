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
        public int inRecyclePool = 0;
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
                //if (inRecyclePool != 0)
                //{
                //    //lib.DoNothing();
                //    return;
                //}
                DssRef.state.voxelModelInstancesPooled.Push(this);
                //inRecyclePool++;
               
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

            //inRecyclePool--;
            //if (inRecyclePool != 0)
            //{
            //    lib.DoNothing();
            //}
        }

        public override void preRemoveFromDrawBatch()
        {
            base.preRemoveFromDrawBatch();
            //if (inRecyclePool != 0)
            //{
            //    lib.DoNothing();
            //}
        }

        //public override void AddToRender()
        //{
        //    throw new Exception();
        //    base.AddToRender();
        //}
        //public override void AddToRender(int layer)
        //{
        //    throw new Exception();
        //    base.AddToRender(layer);
        //}

        public override void DeleteMe()
        {
            //base.DeleteMe();
            preRemoveFromDrawBatch();
        }

        
    }
}
