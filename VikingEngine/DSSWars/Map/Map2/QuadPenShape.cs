using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map.Map2
{
    class QuadPenShape
    {
        Vector2[] corners = new Vector2[4];
        float[] angles;
        public Vector2 center;
        public float radius;
        public QuadPenShape(PcgRandom rnd, Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
            // 1. GENERATE CORNERS
            // Get 4 random angles in radians
            angles = new float[4];
            for (int i = 0; i < 4; i++)
            {
                angles[i] = rnd.Rotation();
            }

            // Sorting the angles is CRITICAL so the vertices connect in a circle 
            // without crossing each other (preventing a self-intersecting polygon)
            Array.Sort(angles);
        }

        //public void refresh()
        //{ 
            
        //}

        public Intvector2MinMax BeginDraw(WorldData2 world /*PcgRandom rnd, Vector2 center, float radius*/)
        {
            //Random rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                corners[i] = new Vector2(
                    center.X + radius * (float)Math.Cos(angles[i]),
                    center.Y + radius * (float)Math.Sin(angles[i])
                );
            }

            // 2. FIND BOUNDING BOX
            // Only iterate over the pixels near the quad, not the whole texture

            Intvector2MinMax minMax = new Intvector2MinMax(
                new IntVector2(
                (int)Math.Max(0, Math.Floor(corners.Min(c => c.X))),
                (int)Math.Max(0, Math.Floor(corners.Min(c => c.Y)))),
                 new IntVector2(
                     (int)Math.Min(world.iconGrid.Width - 1, Math.Ceiling(corners.Max(c => c.X))),
                     (int)Math.Min(world.iconGrid.Height - 1, Math.Ceiling(corners.Max(c => c.Y))))
                 );
            return minMax;
            //int minX = (int)Math.Max(0, Math.Floor(corners.Min(c => c.X)));
            //int maxX = (int)Math.Min(world.tileGrid.Width - 1, Math.Ceiling(corners.Max(c => c.X)));
            //int minY = (int)Math.Max(0, Math.Floor(corners.Min(c => c.Y)));
            //int maxY = (int)Math.Min(world.tileGrid.Height - 1, Math.Ceiling(corners.Max(c => c.Y)));

            //// 3. RASTERIZE AND SHADE
            //for (int y = minY; y <= maxY; y++)
            //{
            //    for (int x = minX; x <= maxX; x++)
            //    {

            //        // Test if pixel is inside the 4 corners
            //        if (IsPointInQuad(new Vector2(x, y), corners))
            //        {
            //            // Calculate distance from center: d = sqrt((px - cx)^2 + (py - cy)^2)
            //            float dx = x - center.X;
            //            float dy = y - center.Y;
            //            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            //            // Map distance to a 0.0 to 1.0 gradient
            //            // 1.0 is the exact center (White), 0.0 is the radius edge (Black)
            //            float intensity = 1.0f - (distance / radius);

            //            // Clamp just in case a corner stretches slightly past floating point radius
            //            intensity = Math.Max(0.0f, Math.Min(1.0f, intensity));

            //            // Write to buffer (1.0 = White, 0.0 = Black)
            //            // TODO: Replace this with texture.SetPixel(x, y, new Color(intensity, intensity, intensity)) 
            //            //textureBuffer[x, y] = intensity;
            //        }
            //    }
            //}           
        }

        public bool DrawPixel(IntVector2 point, out float intensity)
        {
            if (IsPointInQuad(point.Vec, corners))
            {
                // Calculate distance from center: d = sqrt((px - cx)^2 + (py - cy)^2)
                float dx = point.X - center.X;
                float dy = point.Y - center.Y;
                float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                // Map distance to a 0.0 to 1.0 gradient
                // 1.0 is the exact center (White), 0.0 is the radius edge (Black)
                intensity = 1.0f - (distance / radius);

                // Clamp just in case a corner stretches slightly past floating point radius
                intensity = Math.Max(0.0f, Math.Min(1.0f, intensity));

                // Write to buffer (1.0 = White, 0.0 = Black)
                // TODO: Replace this with texture.SetPixel(x, y, new Color(intensity, intensity, intensity)) 
                //textureBuffer[x, y] = intensity;
                return true;
            }
            intensity = 0;
            return false;
        }

        bool IsPointInQuad(Vector2 p, Vector2[] corners)
        {
            bool isNegative = MathExt.V2Cross(corners[0], corners[1], p) < 0;

            if (MathExt.V2Cross(corners[1], corners[2], p) < 0 != isNegative) return false;
            if (MathExt.V2Cross(corners[2], corners[3], p) < 0 != isNegative) return false;
            if (MathExt.V2Cross(corners[3], corners[0], p) < 0 != isNegative) return false;

            return true;
        }
    }
}
