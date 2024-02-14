using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Director
{
    class FrustumCullingDirector : Graphics.FrustumCullingDirector
    {
        Graphics.LFHeightMap heigtmap;

        public FrustumCullingDirector(Graphics.LFHeightMap heigtmap)
            :base(false)
        {
            this.heigtmap = heigtmap;
            Ref.asynchUpdate.AddUpdateThread(threadedUpdate, "Frustum Culling Director for LF terrain");
        }

        protected override void updateCulling()
        {
            base.updateCulling();
            heigtmap.UpdateCulling();
        }
    }
}
