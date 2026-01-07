using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Voxels
{
    interface SceneModelsParent
    {
        Vector3 SceneCenterPos { get; set; }
        void OpenMenuFile(HUD.GuiLayout file);
        void CloseMenu();
        void OpenMainMenu();
    }
    //class SceneLib
    //{
    //}
    struct CameraView
    {
        float zoom;
        Vector3 position;
        Vector2 tilt;

        public void Store(Vector3 selectionPos)
        {
            zoom = Ref.draw.Camera.targetZoom;
            position = selectionPos;// VikingEngine.Ref.draw.Camera.Target;
            tilt = Ref.draw.Camera.Tilt;
        }
        public Vector3 Load()
        {
            Ref.draw.Camera.targetZoom = zoom;
            // VikingEngine.Ref.draw.Camera.Target = position;
            Ref.draw.Camera.Tilt = tilt;
            return position;
        }

        public void IOStream(System.IO.BinaryWriter w, System.IO.BinaryReader r, byte version)
        {
            StreamLib.ValueIO(ref zoom, w, r);
            StreamLib.ValueIO(ref position, w, r);
            StreamLib.ValueIO(ref tilt, w, r);
        }

    }

    enum EditType
    {
        Move,
        Scale,
        PlaneRotation,
        FreeRotation,
        NUM
    }

}
