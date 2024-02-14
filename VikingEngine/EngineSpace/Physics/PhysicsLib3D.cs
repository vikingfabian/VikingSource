using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Physics
{
    
    static class PhysicsLib3D
    {
        static int nextBoundID = 0;
        public static int NextID() { return nextBoundID++; }

        public static bool CollBoxes(IBound3D obj1, IBound3D obj2)
        {
            if (obj1.VerticeIntersect(obj2.Vertices()))
                return true;
            if (obj2.VerticeIntersect(obj1.Vertices()))
                return true;
            return false;
        }
        public static Collision3D CollBoxes2(IBound3D myBound, IBound3D otherBound)
        {
            IntersectDetails2D depth = myBound.VerticeIntersect2(otherBound.Vertices());
            if (depth != null)
                return new Collision3D(myBound, otherBound, depth.Depth, depth.IntersectionCenter);
            depth = otherBound.VerticeIntersect2(myBound.Vertices());
            if (depth != null)
                return new Collision3D(myBound, otherBound, depth);
            return null;
        }
        public static Collision3D CollRectAndCyliner2(IBound3D rect, IBound3D cylinder, IBound3D myBound, IBound3D otherBound)
        {
            if (rect.InnerCirkle.Intersect(cylinder.InnerCirkle))
                return new Collision3D(myBound, otherBound, rect.InnerCirkle.IntersectLength2D(cylinder.InnerCirkle));

            //Treat the cirkle as a point and check is it is inside the box
            Circle flatCirkle = cylinder.InnerCirkle.PlaneCirkle();
            RectangleCentered flatRect = rect.PlaneCenterScale;
            if (rect.Rotation != 0)
            {
                flatCirkle.Center = lib.RotatePointAroundCenter(flatRect.Center, flatCirkle.Center, -rect.Rotation);
            }
            if (flatRect.IntersectCirkle(flatCirkle))
            {
                return new Collision3D(myBound, otherBound, flatRect.IntersectCirkleDepth(flatCirkle, rect.Rotation, cylinder.InnerCirkle.PlaneCirkle()));
            }
            return null;
        }
        public static bool CollRectAndCyliner(IBound3D rect, IBound3D cylinder)
        {
            if (rect.InnerCirkle.Intersect(cylinder.InnerCirkle))
                return true;
            return cylinder.VerticeIntersect(rect.Vertices());
        }
        

        //public static bool PointInTriangle(Vector3 point, Vector3 triangleVertice1, Vector3 triangleVertice2, Vector3 triangleVertice3)
        //{
        //    if (SameSide(px, py, pz, tp1x, tp1y, tp1z,
        //        tp2x, tp2y, tp2z, tp3x, tp3y, tp3z) &&
        //    SameSide(px, py, pz, tp2x, tp2y, tp2z,
        //        tp1x, tp1y, tp1z, tp3x, tp3y, tp3z) &&
        //    SameSide(px, py, pz, tp3x, tp3y, tp3z,
        //        tp1x, tp1y, tp1z, tp2x, tp2y, tp2z))
        //        return true;
        //    else
        //        return false;
        //}
        //public static bool SameSide(double p1x, double p1y, double p1z,
        //   double p2x, double p2y, double p2z,
        //   double ax, double ay, double az,
        //   double bx, double by, double bz)
        //{
        //    double cp1x = 0, cp1y = 0, cp1z = 0, cp2x = 0, cp2y = 0, cp2z = 0;

        //    Cross3(bx - ax, by - ay, bz - az, p1x - ax,
        //        p1y - ay, p1z - az, ref cp1x, ref cp1y, ref cp1z);
        //    Cross3(bx - ax, by - ay, bz - az, p2x - ax,
        //        p2y - ay, p2z - az, ref cp2x, ref cp2y, ref cp2z);
        //    if (Dot3(cp1x, cp1y, cp1z, cp2x, cp2y, cp2z) >= 0)
        //        return true;
        //    else
        //        return false;
        //}

        //public static void Cross3(double ux, double uy, double uz,
        //    double wx,
        //    double wy, double wz,
        //    ref double vx, ref double vy,
        //    ref double vz)
        //{
        //    // u x w
        //    vx = wz * uy - wy * uz;
        //    vy = wx * uz - wz * ux;
        //    vz = wy * ux - wx * uy;
        //}

        //public static void Cross3(float ux, float uy, float uz,
        //    float wx,
        //    float wy, float wz,
        //    ref float vx, ref float vy,
        //    ref float vz)
        //{
        //    // u x w
        //    vx = wz * uy - wy * uz;
        //    vy = wx * uz - wz * ux;
        //    vz = wy * ux - wx * uy;
        //}

        //public static double Dot3(double x1, double y1, double z1,
        //    double x2, double y2, double z2)
        //{
        //    return (x1 * x2) + (y1 * y2) + (z1 * z2);
        //}

        //public static bool SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b)
        //{
        //    Vector3 cp1 = Vector3.Zero, cp2 = Vector3.Zero;

        //    Cross3(b.X - a.X, b.Y - a.Y, b.Z - a.Z, p1.X - a.X,
        //        p1.Y - a.Y, p1.Z - a.Z, ref cp1.X, ref cp1.Y, ref cp1.Z);
        //    Cross3(b.X - a.X, b.Y - a.Y, b.Z - a.Z, p2.X - a.X,
        //        p2.Y - a.Y, p2.Z - a.Z, ref cp2.X, ref cp2.Y, ref cp2.Z);
        //    if (Dot3(cp1, cp2) >= 0)
        //        return true;
        //    else
        //        return false;
        //}

        ////public static Vector3 Cross3(
        ////   Vector3 u, Vector3 w)
        ////{
        ////    return new Vector3(
        ////        w.Z * u.Y - w.Y * u.Z,
        ////        w.X * u.Z - w.Z * u.X,
        ////        w.Y * u.X - w.X * u.Y);
        ////}

        //public static double Dot3(Vector3 v1, Vector3 v2)
        //{
        //    return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        //}

        //public static bool TwoModels(Model m1, Model m2)
        //{
        //    //m1.Meshes[0].MeshParts[0]
        //   var ray =  new Microsoft.Xna.Framework.Ray();
        //    ray.Intersects(
        //}
    }
}
