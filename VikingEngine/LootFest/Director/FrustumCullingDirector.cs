using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Director
{
    class FrustumCullingDirector : Graphics.FrustumCullingDirector
    {
        //Graphics.LFHeightMap heigtmap;

        public FrustumCullingDirector()
            :base(false)
        {
            //this.heigtmap = heigtmap;
            //Ref.asynchUpdate.AddUpdateThread(threadedUpdate, "Frustum Culling Director for LF terrain", 0);

            name = "Frustum Culling Director for LF terrain";
            AddToUpdateList();
        }

        protected override void updateCulling()
        {
            base.updateCulling();
            //Ref.draw.heightmap.UpdateCulling();
        }
    }
}
