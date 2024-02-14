using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Editor.Scene
{
    interface SceneModelsParent
    {
        Vector3 SceneCenterPos { get; set; }
        void OpenMenuFile(HUD.File file);
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
            zoom = VikingEngine.Ref.draw.Camera.Zoom;
            position = selectionPos;// Game1.Ref.draw.Camera.Target;
            tilt = VikingEngine.Ref.draw.Camera.Tilt;
        }
        public Vector3 Load()
        {
            VikingEngine.Ref.draw.Camera.Zoom = zoom;
            // Game1.Ref.draw.Camera.Target = position;
            VikingEngine.Ref.draw.Camera.Tilt = tilt;
            return position;
        }

        public void IOStream(System.IO.BinaryWriter w, System.IO.BinaryReader r, byte version)
        {
            SaveLib.ValueIO(ref zoom, w, r);
            SaveLib.ValueIO(ref position, w, r);
            SaveLib.ValueIO(ref tilt, w, r);
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
