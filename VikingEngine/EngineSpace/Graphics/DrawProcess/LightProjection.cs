using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.EngineSpace.Graphics.DrawProcess
{
    class LightProjection
    {
        public Matrix LightViewMatrix;
        public Matrix LightProjectionMatrix;
        public Matrix ViewProjection;
        public Vector3 lightPos;
        public Vector3 lightDirection = new Vector3(-0.2f, -1f, -0.2f);
        public Vector3 TargetAdj = new Vector3(0, 0, 1);
        public Vector3 SunColor = new Vector3(0.5f, 0.45f, 0.45f);
        public float distance = 500f;
        Vector3 target = Vector3.Zero;   // center of the scene (adjust as needed)
        float sceneWidth = 100;   // world units to cover; tune per your scene
        float sceneHeight = 100;

        public LightProjection()
        {
            //refresh();
        }

        public void updateScene(AbsCamera camera, float sceneWidth, float sceneHeight)
        {
            TargetAdj.Z = -0.5f;
            this.target = camera.LookTarget + TargetAdj;
            this.sceneWidth = sceneWidth;
            this.sceneHeight = sceneHeight;
            refresh(camera);
        }

        //public void refresh()
        //{
        //    // 1) Normalize and validate light direction

        //    if (lightDirection.LengthSquared() < 1e-8f)
        //        lightDirection = Vector3.Down; // fallback
        //    lightDirection.Normalize();

        //    // 2) Choose a stable UP that isn't parallel to lightDirection
        //    Vector3 up = Vector3.Up;
        //    if (Math.Abs(Vector3.Dot(lightDirection, up)) > 0.99f)
        //        up = Vector3.Right; // fallback if nearly parallel

        //    // 3) Build a view: place the "camera" back along the light ray

        //    // how far "above" the scene the sun camera sits
        //    lightPos = target - lightDirection * distance;

        //    LightViewMatrix = Matrix.CreateLookAt(lightPos, target, up);

        //    // 4) Orthographic projection for directional (parallel) light

        //    float nearPlane = distance - 10;
        //    float farPlane = distance + 4;

        //    LightProjectionMatrix = Matrix.CreateOrthographic(sceneWidth, sceneHeight, nearPlane, farPlane);

        //    ViewProjection = LightViewMatrix * LightProjectionMatrix;
        //}
        public void refresh(AbsCamera camera)
        {
            if (lightDirection.LengthSquared() < 1e-8f) lightDirection = Vector3.Down;
            lightDirection.Normalize();

            // Project cameraRight onto plane perpendicular to lightDirection
            Vector3 L = lightDirection;
            Vector3 r = Vector3.Cross(L, Vector3.Cross(camera.Right(), L));
            if (r.LengthSquared() < 1e-6f) r = Vector3.Right; // fallback
            r.Normalize();

            // Build an up that matches this right (r × L gives a consistent up)
            Vector3 up = Vector3.Cross(r, L);
            up.Normalize();

            lightPos = target - L * distance;
            LightViewMatrix = Matrix.CreateLookAt(lightPos, target, up);

            float nearPlane = distance - 6f;
            float farPlane = distance + 10;
            LightProjectionMatrix = Matrix.CreateOrthographic(sceneWidth, sceneHeight, nearPlane, farPlane);

            ViewProjection = LightViewMatrix * LightProjectionMatrix;
        }


        public Matrix modelToLight(Matrix world)
        { 
            return world * LightViewMatrix * LightProjectionMatrix;
        }
    }
}
