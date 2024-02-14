using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Physics
{
    class RTTriangle
    {
        protected Vector3 v0, v1, v2;
        protected Vector3 normal;
        protected Vector3 u, v;
        protected Vector3 uPerp, vPerp;
        protected float denominatorST;

        public RTTriangle(Vector3 v0, Vector3 v1, Vector3 v2, bool clockwise = false)
        {
            Init(v0, v1, v2, clockwise);
        }

        protected void Init(Vector3 v0, Vector3 v1, Vector3 v2, bool clockwise = false)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            u = v1 - v0;
            v = v2 - v0;

            // Unity uses clockwise winding order to determine front-facing triangles
            // Unity uses a left-handed coordinate system
            // the normal will face the front
            // if the direction of the normal is not important to you
            // just remove the clockwise branching
            normal = (clockwise ? 1 : -1) * Vector3.Cross(u, v);
            normal.Normalize();

            uPerp = Vector3.Cross(normal, u);
            vPerp = Vector3.Cross(normal, v);
            denominatorST = Vector3.Dot(u, vPerp);
            if (Math.Abs(denominatorST) < float.Epsilon)
            {
                Debug.LogError("Triangle is broken");
                return;
            }
        }

        public RayHitInfo Intersect(Ray ray)
        {

            RayHitInfo info = RayHitInfo.Empty;

            Vector3 d = ray.Direction;
            float denominator = Vector3.Dot(d, normal);

            if (Math.Abs(denominator) < float.Epsilon) return info;      // direction and plane parallel, no intersection

            float tHit = Vector3.Dot(v0 - ray.Position, normal) / denominator;
            if (tHit < 0) return info;    // plane behind ray's origin

            // we have a hit point with the triangle's plane
            Vector3 rayPoint = ray.Position + ray.Direction * tHit;
            Vector3 w = rayPoint - v0;

            float s = Vector3.Dot(w, vPerp) / denominatorST;
            if (s < 0 || s > 1) return info;    // won't be inside triangle

            float t = Vector3.Dot(w, uPerp) / -denominatorST;
            if (t >= 0 && (s + t) <= 1)
            {
                info.hitPoint = rayPoint;
                info.normal = normal;
            }

            return info;
        }


    }
    struct RayHitInfo
    {
        public static readonly RayHitInfo Empty = new RayHitInfo();

        public bool isHit;
        public Vector3 hitPoint;
        public Vector3 normal;
    }
}
