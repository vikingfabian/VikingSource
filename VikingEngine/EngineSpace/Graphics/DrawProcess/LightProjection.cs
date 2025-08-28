using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace.Graphics.DrawProcess
{
    class LightProjection
    {
        Matrix LightViewMatrix;
        Matrix LightProjectionMatrix;
        public LightProjection(int shadowMapSize)
        {
            // 1) Normalize and validate light direction
            Vector3 lightDirection = new Vector3(-0.1f, -1f, -0.1f);
            if (lightDirection.LengthSquared() < 1e-8f)
                lightDirection = Vector3.Down; // fallback
            lightDirection.Normalize();

            // 2) Choose a stable UP that isn't parallel to lightDirection
            Vector3 up = Vector3.Up;
            if (Math.Abs(Vector3.Dot(lightDirection, up)) > 0.99f)
                up = Vector3.Right; // fallback if nearly parallel

            // 3) Build a view: place the "camera" back along the light ray
            Vector3 target = Vector3.Zero;   // center of the scene (adjust as needed)
            float distance = 100f;           // how far "above" the scene the sun camera sits
            Vector3 lightPos = target - lightDirection * distance;

            LightViewMatrix = Matrix.CreateLookAt(lightPos, target, up);

            // 4) Orthographic projection for directional (parallel) light
            float sceneWidth = shadowMapSize;   // world units to cover; tune per your scene
            float sceneHeight = shadowMapSize;
            float nearPlane = 1f;
            float farPlane = 500f;

            LightProjectionMatrix = Matrix.CreateOrthographic(sceneWidth, sceneHeight, nearPlane, farPlane);
        }

        public Matrix modelToLight(Matrix world)
        { 
            return world * LightViewMatrix * LightProjectionMatrix;
        }
    }
}
