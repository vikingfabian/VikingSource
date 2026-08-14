using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;

namespace VikingEngine.EngineSpace.Graphics.In3D
{
    class ScreenToSpaceRectangleBound
    {
        public Plane[] Planes = new Plane[6]; // Left, Right, Top, Bottom, Near, Far

        public Vector2 pointerDownPos;
        public Vector3 pointerDownPosWp;
        public Vector2 currentPointerPos;
        PlayerView pview;
        Vector3[] corners;
        Vector2[] screenCorners = new Vector2[4];

        public VectorRect vectorRect;

        float minY, maxY;

        public ScreenToSpaceRectangleBound(PlayerView view, float minY, float maxY)
        {
            this.pview = view;
            corners = new Vector3[8];
            this.minY = minY;
            this.maxY = maxY;
        }

        public Vector3 BottomPlane_TopLeft()
        {
            return corners[0];
        }

        public Vector3 BottomPlane_BottomRight()
        {
            return corners[3];
        }

        public void outerBound(out Vector3 topLeft, out Vector3 bottomRight)
        {
            // Initialize with the first corner (on the bottom plane)
            float minX = corners[0].X;
            float minZ = corners[0].Z;
            float maxX = corners[0].X;
            float maxZ = corners[0].Z;

            // Go through the other 3 bottom-plane corners
            for (int i = 1; i < 4; i++) // corners[0] to corners[3] = bottom corners
            {
                Vector3 c = corners[i];
                if (c.X < minX) minX = c.X;
                if (c.Z < minZ) minZ = c.Z;
                if (c.X > maxX) maxX = c.X;
                if (c.Z > maxZ) maxZ = c.Z;
            }

            // Return the 2 corners of the axis-aligned bounding rectangle on the XZ plane (Y = minY)
            topLeft = new Vector3(minX, minY, minZ);
            bottomRight = new Vector3(maxX, minY, maxZ);
        }




        public void begin(Vector2 screenPos, Vector3 mouseDownWp)
        {
            this.pointerDownPosWp = mouseDownWp;
            pointerDownPos = screenPos;
            currentPointerPos = screenPos;
            vectorRect = new VectorRect(screenPos, Vector2.Zero);
        }

        public void update(Vector2 mousePosition)//, GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            // Step 1: Normalize rectangle
            pointerDownPos = pview.Camera.From3DToScreenPos(pointerDownPosWp, pview.Viewport);

            Rectangle rect = NormalizeRectangle(pointerDownPos, mousePosition);
            currentPointerPos = mousePosition;
            vectorRect.Rectangle = rect;


           Vector3 topLeft = pview.Camera.CastRayInto3DPlane(new Vector2(rect.Left, rect.Top), pview.Viewport, new Plane(Vector3.UnitY, minY), out bool hasValue);

            // Step 2: Unproject corners at near and far planes            
            screenCorners[0] = new Vector2(rect.Left, rect.Top);     // TL
            screenCorners[1] = new Vector2(rect.Right, rect.Top);    // TR
            screenCorners[2] = new Vector2(rect.Left, rect.Bottom);  // BL
            screenCorners[3] = new Vector2(rect.Right, rect.Bottom);  // BR

            for (int i = 0; i < 4; i++)
            {
                corners[i] = pview.Camera.CastRayInto3DPlane(screenCorners[i], pview.Viewport, new Plane(Vector3.UnitY, minY), out bool hasValue1);
                corners[i+4] = pview.Camera.CastRayInto3DPlane(screenCorners[i], pview.Viewport, new Plane(Vector3.UnitY, maxY), out bool hasValue2);

                //corners[i] = Draw.graphicsDeviceManager.GraphicsDevice.Viewport.Unproject(new Vector3(screenCorners[i], minY), pview.Camera.Projection, pview.Camera.ViewMatrix, Matrix.Identity); // Near
                //corners[i + 4] = Draw.graphicsDeviceManager.GraphicsDevice.Viewport.Unproject(new Vector3(screenCorners[i], maxY), pview.Camera.Projection, pview.Camera.ViewMatrix, Matrix.Identity); // Far
            }

            // Step 3: Create planes from corners
            // We define planes using 3 points (Plane.CreateFromPoints)

            Planes[0] = new Plane(corners[0], corners[2], corners[6]); // Left
            Planes[1] = new Plane(corners[3], corners[1], corners[7]); // Right
            Planes[2] = new Plane(corners[1], corners[0], corners[5]); // Top
            Planes[3] = new Plane(corners[2], corners[3], corners[6]); // Bottom
            Planes[4] = new Plane(corners[0], corners[1], corners[2]); // Near
            Planes[5] = new Plane(corners[5], corners[6], corners[7]); // Far


           
            //if (Input.Keyboard.Ctrl)
            //{
            //    test();
            //}
        }

        void test()
        {
            // Utility method to average 3 vectors
            Vector3 Average(Vector3 a, Vector3 b, Vector3 c)
            {
                return (a + b + c) / 3f;
            }

            // Define test points near the center of each plane
            Vector3 leftSide = Average(corners[0], corners[2], corners[6]);
            Vector3 rightSide = Average(corners[3], corners[1], corners[7]);
            Vector3 topSide = Average(corners[5], corners[4], corners[7]);
            Vector3 bottomSide = Average(corners[0], corners[1], corners[2]);
            Vector3 nearSide = Average(corners[0], corners[1], corners[5]);
            Vector3 farSide = Average(corners[6], corners[7], corners[2]);

            // Tiny bounding sphere for testing
            float radius = 0.1f;

            // Test all planes
            var testLeft = Planes[0].Intersects(new BoundingSphere(leftSide, radius));
            var testRight = Planes[1].Intersects(new BoundingSphere(rightSide, radius));
            var testTop = Planes[2].Intersects(new BoundingSphere(topSide, radius));
            var testBottom = Planes[3].Intersects(new BoundingSphere(bottomSide, radius));
            var testNear = Planes[4].Intersects(new BoundingSphere(nearSide, radius));
            var testFar = Planes[5].Intersects(new BoundingSphere(farSide, radius));

            // Print results (or use breakpoints/logging as needed)
            Console.WriteLine($"Left   plane test: {testLeft}");
            Console.WriteLine($"Right  plane test: {testRight}");
            Console.WriteLine($"Top    plane test: {testTop}");
            Console.WriteLine($"Bottom plane test: {testBottom}");
            Console.WriteLine($"Near   plane test: {testNear}");
            Console.WriteLine($"Far    plane test: {testFar}");
        }

        private Rectangle NormalizeRectangle(Vector2 start, Vector2 end)
        {
            int x = (int)Math.Min(start.X, end.X);
            int y = (int)Math.Min(start.Y, end.Y);
            int width = (int)Math.Abs(end.X - start.X);
            int height = (int)Math.Abs(end.Y - start.Y);
            return new Rectangle(x, y, width, height);
        }

        public bool Intersects(BoundingBox box)
        {
            foreach (var plane in Planes)
            {
                if (box.Intersects(plane) == PlaneIntersectionType.Back)
                    return false;
            }
            return true;
        }

        public bool Intersects(Vector3 position, float radius)
        {
            var bound =new BoundingSphere(position, radius);

            foreach (var plane in Planes)
            {
                if (bound.Intersects(plane) == PlaneIntersectionType.Back)
                    return false;
            }
            return true;
        }

        public bool Intersects(BoundingSphere bound)
        {
            foreach (var plane in Planes)
            {
                if (bound.Intersects(plane) == PlaneIntersectionType.Back)
                    return false;
            }
            return true;
        }
    }
}
