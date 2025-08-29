using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.EngineSpace.Graphics.DrawProcess
{
    class LightProjection
    {
        public Matrix LightViewMatrix;
        public Matrix LightProjectionMatrix;
        public Matrix ViewProjection;
        public Vector3 lightPos;
        Vector3 lightDirection = new Vector3(-0.2f, -1f, -0.2f);
        public float distance = 500f;
        Vector3 target = Vector3.Zero;   // center of the scene (adjust as needed)
        float sceneWidth = 100;   // world units to cover; tune per your scene
        float sceneHeight = 100;

        public LightProjection(int shadowMapSize)
        {
            refresh();
        }

        public void updateScene(Vector3 target, float sceneWidth, float sceneHeight)
        { 
            this.target = target;
            this.sceneWidth = sceneWidth;
            this.sceneHeight = sceneHeight;
            refresh();
        }

        public void refresh()
        {
            // 1) Normalize and validate light direction

            if (lightDirection.LengthSquared() < 1e-8f)
                lightDirection = Vector3.Down; // fallback
            lightDirection.Normalize();

            // 2) Choose a stable UP that isn't parallel to lightDirection
            Vector3 up = Vector3.Up;
            if (Math.Abs(Vector3.Dot(lightDirection, up)) > 0.99f)
                up = Vector3.Right; // fallback if nearly parallel

            // 3) Build a view: place the "camera" back along the light ray

            // how far "above" the scene the sun camera sits
            lightPos = target - lightDirection * distance;

            LightViewMatrix = Matrix.CreateLookAt(lightPos, target, up);

            // 4) Orthographic projection for directional (parallel) light
            
            float nearPlane = distance - 10;
            float farPlane = distance + 4;

            LightProjectionMatrix = Matrix.CreateOrthographic(sceneWidth, sceneHeight, nearPlane, farPlane);

            ViewProjection = LightViewMatrix * LightProjectionMatrix;
        }

        public Matrix modelToLight(Matrix world)
        { 
            return world * LightViewMatrix * LightProjectionMatrix;
        }
    }
}
