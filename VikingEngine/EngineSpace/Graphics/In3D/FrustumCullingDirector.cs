using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VikingEngine.Graphics
{
    /// <summary>
    /// Goes through all the models and check if they are in any camera
    /// </summary>
    class FrustumCullingDirector : AsynchUpdateable
    {
        SpottedArrayCounter<AbsDraw> meshes;
        SpottedArrayCounter<AbsDraw> generated;

        public FrustumCullingDirector(bool start)
            :base(null, "Frustum Culling Director", 0, ThreadPriority.Normal, start)
        {
            
            meshes = new SpottedArrayCounter<AbsDraw>(Ref.draw.renderList[0].GetList(DrawObjType.Mesh));
            generated = new SpottedArrayCounter<AbsDraw>(Ref.draw.renderList[0].GetList(DrawObjType.MeshGenerated));

            //if (start)
            //    Ref.asynchUpdate.AddUpdateThread(threadedUpdate, "Frustum Culling Director", 0);
        }

        protected override void asynchAction()
        {
            if (PlatformSettings.BlueScreen)
            {
                try
                {
                    updateCulling();
                }
                catch (Exception e)
                {
                    Debug.LogError("FrustumCullingDirector crashed, " + e.Message);
                }
            }
            else
            {
                updateCulling();
            }

          //  return false;
        }

        virtual protected void updateCulling()
        {
            meshes.Reset();
            while (meshes.Next())
            {
                meshes.sel.UpdateCulling();
            }
            generated.Reset();
            while (generated.Next())
            {
                generated.sel.UpdateCulling();
            }
        }
    }
}
