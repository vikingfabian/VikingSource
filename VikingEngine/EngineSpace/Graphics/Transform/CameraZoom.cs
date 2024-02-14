using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class CameraZoom : AbsUpdateable
    {
        AbsCamera camera;
        float goalZoom;
        float speed;
        bool accelerate;

        public CameraZoom(AbsCamera camera, float goalZoom, float speed, bool accelerate)
            :base(true)
        {
            this.camera = camera;
            this.goalZoom = goalZoom;
            this.speed = speed;
            this.accelerate = accelerate;
        }
        public override void Time_Update(float time)
        {
            float diff = goalZoom - camera.targetZoom;
            float zoom;
            if (accelerate)
            {
                 zoom = diff * speed * time;
                 if (Math.Abs(diff) <= 0.05f)
                 {
                     DeleteMe();
                     zoom = 0;
                 }
            }
            else
            {
                zoom = lib.ToLeftRight(diff) * speed * time;
                if (Math.Abs(zoom) > Math.Abs(diff))
                {
                    DeleteMe();
                    zoom = 0;
                }
            }

            camera.targetZoom += zoom;
        }
    }
}
