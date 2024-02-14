using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
    
namespace VikingEngine.EngineSpace.Maths
{ 
    static class TestGJK
    {
        static public void RunTest()
        {
            List<Vector3> cube1verts = new List<Vector3>();
            cube1verts.Add(new Vector3(0, 0, 0));
            cube1verts.Add(new Vector3(1, 0, 0));
            cube1verts.Add(new Vector3(1, 1, 0));
            cube1verts.Add(new Vector3(0, 1, 0));
            cube1verts.Add(new Vector3(0, 0, -1));
            cube1verts.Add(new Vector3(1, 0, -1));
            cube1verts.Add(new Vector3(1, 1, -1));
            cube1verts.Add(new Vector3(0, 1, -1));
            List<Vector3> cube2verts = new List<Vector3>();
            cube2verts.Add(new Vector3(0.5f, 0, 0));
            cube2verts.Add(new Vector3(1.5f, 0, 0));
            cube2verts.Add(new Vector3(1.5f, 1, 0));
            cube2verts.Add(new Vector3(0.5f, 1, 0));
            cube2verts.Add(new Vector3(0.5f, 0, -1));
            cube2verts.Add(new Vector3(1.5f, 0, -1));
            cube2verts.Add(new Vector3(1.5f, 1, -1));
            cube2verts.Add(new Vector3(0.5f, 1, -1));

            ConvexPolyhedra3d shape1 = new ConvexPolyhedra3d(cube1verts);
            //ConvexPolyhedra3d sphere2 = new ConvexPolyhedra3d(cube2verts);
            Sphere3d shape2 = new Sphere3d(new Vector3(0.5f, 0.5f, 1), 0.99f);
            bool intersect = GilbertJohnsonKeerthi.BodiesIntersect(shape1, shape2); //returns true in this case
            Debug.Log(intersect ? "INTERSECTION" : ":(");
        }
    }

    interface GJKSupport3d
    {
        Vector3 AnyPoint();
        Vector3 FurthestPoint(Vector3 dir);
    }

    interface GJKSupport2d
    {
        Vector2 AnyPoint();
        Vector2 FurthestPoint(Vector2 dir);
    }

    class ConvexPolyhedra3d : GJKSupport3d
    {
        /* Fields */
        List<Vector3> vertices;

        /* Constructors */
        public ConvexPolyhedra3d(List<Vector3> vertices)
        {
            this.vertices = vertices;
        }

        /* Interface Methods */
        public Vector3 AnyPoint()
        {
            return vertices[0];
        }

        public Vector3 FurthestPoint(Vector3 dir)
        {
            Vector3 max = vertices[0];
            foreach (Vector3 point in vertices)
            {
                if (max.Dot(dir) < point.Dot(dir))
                {
                    max = point;
                }
            }
            return max;
        }
    }

    class Sphere3d : GJKSupport3d
    {
        /* Fields */
        Vector3 center;
        float radius;

        /* Constructors */
        public Sphere3d(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /* Interface Methods */
        public Vector3 AnyPoint()
        {
            return center + new Vector3(radius, 0, 0);
        }

        public Vector3 FurthestPoint(Vector3 dir)
        {
            dir.Normalize();
            return center + radius * dir;
        }
    }
        
    /// <summary>
    /// Implements the Gilbert-Johnson-Keerthi algorithm for collision detection in 3D, as described in the video lecture at http://mollyrocket.com/849 
    /// See also http://www.cse.ttu.edu.tw/~jmchen/compg/fall04/students-presentation/GJK.pdf
    /// </summary>
    static class GilbertJohnsonKeerthi
    {
        //to prevent infinite loops — if an intersection is not found in 20 rounds, consider there is no intersection
        private const int MaxIterations = 20;
            
        /// <summary>
        /// Given the vertices (in any order) of two convex 3D bodies, calculates whether they intersect
        /// </summary>
        public static bool BodiesIntersect(GJKSupport3d shape1, GJKSupport3d shape2)
        {
            //for initial point, just take the difference between any two vertices (in this case — the first ones)
            Vector3 initialPoint = shape1.AnyPoint() - shape2.AnyPoint();
            Vector3 S = MaxPointInMinkDiffAlongDir(shape1, shape2, initialPoint);
            List<Vector3> simplex = new List<Vector3>() { S };
            Vector3 D = -S;
            for (int i = 0; i < MaxIterations; i++)
            {
                Vector3 A = MaxPointInMinkDiffAlongDir(shape1, shape2, D);
                if (Vector3.Dot(A, D) < 0) return false; // No intersection
                simplex.Add(A);
                if (UpdateSimplexAndDirection(simplex, ref D)) return true; // Intersection
            }
            return false;
        }

        /// <summary>
        /// Given the vertices (in any order) of two convex 2D bodies, calculates whether they intersect
        /// </summary>
        public static bool BodiesIntersect(GJKSupport2d shape1, GJKSupport2d shape2)
        {
            //for initial point, just take the difference between any two vertices (in this case — the first ones)
            Vector2 initialPoint = shape1.AnyPoint() - shape2.AnyPoint();
            Vector2 S = MaxPointInMinkDiffAlongDir(shape1, shape2, initialPoint);
            List<Vector2> simplex = new List<Vector2>() { S };
            Vector2 D = -S;
            for (int i = 0; i < MaxIterations; i++)
            {
                Vector2 A = MaxPointInMinkDiffAlongDir(shape1, shape2, D);
                if (Vector2.Dot(A, D) < 0) return false; // No intersection
                simplex.Add(A);
                if (UpdateSimplexAndDirection(simplex, ref D)) return true; // Intersection
            }
            return false;
        }

        /// <summary>
        /// Updates the current simplex and the direction in which to look for the origin. Called DoSimplex in the video lecture.
        /// </summary>
        /// <param name=“simplex”>A list of points in the current simplex. The last point in the list must be the last point added to the simplex</param>
        /// <param name=“direction”></param>
        /// <returns></returns>
        private static bool UpdateSimplexAndDirection(List<Vector3> simplex, ref Vector3 direction)
        {
            if (simplex.Count == 2) // if the simplex is a line
            {
                // A is the point added last to the simplex
                // Since we came from B approaching the origin, the origin can't be beyond B along AB
                Vector3 A = simplex[1];
                Vector3 B = simplex[0];
                Vector3 AB = B - A;
                Vector3 AO = -A;
                if (AB.Dot(AO) > 0)
                {
                    // The origin is somewhere between A and B
                    direction = AB.Cross(AO).Cross(AB);
                    // 12 mul 6 sub
                    // (AxB)xC = B(A|C)-A(B|C) => AO(AB|AB) - AB(AO|AB)
                    // direction = AO * Vector3.Dot(AB, AB) - AB * Vector3.Dot(AO, AB);
                    // 12 mul, 4 add, 3 sub
                    // Not an optimization.
                }
                else
                {
                    // The origin is beyond A.
                    // If in doubt of this as a possibility, note that A is furthest along BO, not BA.
                    direction = AO;
                }
            }
            else if (simplex.Count == 3) // if the simplex is a triangle
            {
                // A is the point added last to the simplex. C was added first.
                // From the line creation we know that the origin can not lie beyond C along BC.
                // Nor can it lie beyond CB from the triangle, since we went to A from B.
                // It also can not lie beyond B along AB for the same reason.
                Vector3 A = simplex[2];
                Vector3 B = simplex[1];
                Vector3 C = simplex[0];
                Vector3 AO = -A;
                Vector3 AB = B - A;
                Vector3 AC = C - A;
                Vector3 ABC = AB.Cross(AC); // ABC is up in ccw winding order
                if (ABC.Cross(AC).Dot(AO) > 0) // (ABxAC)xAC is outward normal from tri along AC
                {
                    // The origin is outside, beyond AC
                    if (AC.Dot(AO) > 0) // Is it between A and C too?
                    {
                        simplex.Clear();
                        simplex.Add(C);
                        simplex.Add(A);
                        direction = AC.Cross(AO).Cross(AC);
                        // Search inwards in tri OAC from AC.
                        // Note that using direction ABC cross AC does not work, since we are in 3d
                    }
                    else if (AB.Dot(AO) > 0) // if not, in the case that (AB|AC) < 0, O might be between A and B
                    {
                        simplex.Clear();
                        simplex.Add(B);
                        simplex.Add(A);
                        direction = AB.Cross(AO).Cross(AB);
                    }
                    else // Once again, the origin can also be beyond A; A is furthest along BO, not BA.
                    {
                        simplex.Clear();
                        simplex.Add(A);
                        direction = AO;
                    }
                }
                else if (AB.Cross(ABC).Dot(AO) > 0) // ABx(ABxAC) is outward normal from tri along AB.
                {
                    // The origin is outside, beyond AB.
                    if (AB.Dot(AO) > 0) // Is it between A and B?
                    {
                        simplex.Clear();
                        simplex.Add(B);
                        simplex.Add(A);
                        direction = AB.Cross(AO).Cross(AB);
                        // Search inwards in tri OAB from AB.
                        // Again, note that using direction ABC cross AC does not work, since we are in 3d
                    }
                    else // Still, the origin can also be beyond A; A is furthest along BO, not BA.
                    {
                        simplex.Clear();
                        simplex.Add(A);
                        direction = AO;
                    }
                }
                else
                {
                    // The origin is inside the tri.
                    if (ABC.Dot(AO) > 0) // is it above us? (ccw winding order)
                    {
                        //the simplex stays A, B, C
                        direction = ABC; // search upwards.
                    }
                    else
                    {
                        // it should be above us in ccw winding order.
                        simplex.Clear();
                        simplex.Add(B);
                        simplex.Add(C);
                        simplex.Add(A);
                        direction = -ABC; // now search upwards.
                    }
                }
            }
            else // the simplex is a tetrahedron
            {
                // A is the point added last to the simplex
                Vector3 A = simplex[3];
                // BCD is a ccw winding order triangle with A above it
                Vector3 B = simplex[2];
                Vector3 C = simplex[1];
                Vector3 D = simplex[0];
                Vector3 AO = -A;
                Vector3 AB = B - A;
                Vector3 AC = C - A;
                Vector3 AD = D - A;
                Vector3 ABC = AB.Cross(AC);
                Vector3 ACD = AC.Cross(AD);
                Vector3 ADB = AD.Cross(AB);
                //the side (positive or negative) of B, C and D relative to the planes of ACD, ADB and ABC respectively
                int BsideOnACD = Math.Sign(ACD.Dot(AB));
                int CsideOnADB = Math.Sign(ADB.Dot(AC));
                int DsideOnABC = Math.Sign(ABC.Dot(AD));
                //whether the origin is on the same side of ACD/ADB/ABC as B, C and D respectively
                bool ABsameAsOrigin = Math.Sign(ACD.Dot(AO)) == BsideOnACD;
                bool ACsameAsOrigin = Math.Sign(ADB.Dot(AO)) == CsideOnADB;
                bool ADsameAsOrigin = Math.Sign(ABC.Dot(AO)) == DsideOnABC;
                //if the origin is on the same side as all B, C and D, the origin is inside the tetrahedron and thus there is a collision
                if (ABsameAsOrigin && ACsameAsOrigin && ADsameAsOrigin)
                {
                    return true;
                } //if the origin is not on the side of B relative to ACD
                else if (!ABsameAsOrigin)
                {
                    //B is farthest from the origin among all of the tetrahedron’s points, so remove it from the list and go on with the triangle case
                    simplex.Remove(B);
                    //the new direction is on the other side of ACD, relative to B
                    direction = ACD * -BsideOnACD;
                } //if the origin is not on the side of C relative to ADB
                else if (!ACsameAsOrigin)
                {
                    //C is farthest from the origin among all of the tetrahedron’s points, so remove it from the list and go on with the triangle case 
                    simplex.Remove(C);
                    //the new direction is on the other side of ADB, relative to C
                    direction = ADB * -CsideOnADB;
                } //if the origin is not on the side of D relative to ABC
                else //if (!ADsameAsOrigin)
                {
                    //D is farthest from the origin among all of the tetrahedron’s points, so remove it from the list and go on with the triangle case 
                    simplex.Remove(D);
                    //the new direction is on the other side of ABC, relative to D
                    direction = ABC * -DsideOnABC;
                } 
                //go on with the triangle case
                //TODO: maybe we should restrict the depth of the recursion, just like we restricted the number of iterations in BodiesIntersect?
                return UpdateSimplexAndDirection(simplex, ref direction);
            }
            // no intersection found on this iteration.
            return false;
        }

        /// <summary>
        /// Updates the current simplex and the direction in which to look for the origin. Called DoSimplex in the video lecture.
        /// </summary>
        /// <param name=“simplex”>A list of points in the current simplex. The last point in the list must be the last point added to the simplex</param>
        /// <param name=“direction”></param>
        /// <returns></returns>
        private static bool UpdateSimplexAndDirection(List<Vector2> simplex, ref Vector2 direction)
        {
            if (simplex.Count == 2) // if the simplex is a line
            {
                // A is the point added last to the simplex
                // Since we came from B approaching the origin, the origin can't be beyond B along AB
                Vector2 A = simplex[1];
                Vector2 B = simplex[0];
                Vector2 AB = B - A;
                Vector2 AO = -A;
                if (Vector2.Dot(AB,AO) > 0)
                {
                    // The origin is somewhere between A and B
                    direction = MathExt.Tri_In(AB, AO);
                    // search inwards in triangle AOB from side AB
                }
                else
                {
                    // The origin is beyond A.
                    // If in doubt of this as a possibility, note that A is furthest along BO, not BA.
                    direction = AO;
                }
            }
            else if (simplex.Count == 3) // if the simplex is a triangle
            {
                // A is the point added last to the simplex. C was added first.
                // From the line creation we know that the origin can not lie beyond C along BC.
                // Nor can it lie beyond CB from the triangle, since we went to A from B.
                // It also can not lie beyond B along AB for the same reason.
                Vector2 A = simplex[2];
                Vector2 B = simplex[1];
                Vector2 C = simplex[0];
                Vector2 AO = -A;
                Vector2 AB = B - A;
                Vector2 AC = C - A;
                Vector2 ACout = MathExt.Tri_Out(AC, AB);
                Vector2 ABout = MathExt.Tri_Out(AB, AC);
                if (Vector2.Dot(ACout, AO) > 0) // ACout is outward normal from tri's AC side.
                {
                    // The origin is outside, beyond AC
                    if (Vector2.Dot(AC, AO) > 0) // Is it between A and C too?
                    {
                        simplex.Clear();
                        simplex.Add(C);
                        simplex.Add(A);
                        direction = ACout;
                        // Search inwards in tri OAC from AC.
                        // Note that the inward normal of AC in OAC is in this case the same as the outward normal of AC in ABC.
                    }
                    else if (Vector2.Dot(AB, AO) > 0) // if not, in the case that (AB|AC) < 0, O might be between A and B
                    {
                        simplex.Clear();
                        simplex.Add(B);
                        simplex.Add(A);
                        // Search inwards in tri OAB from AB.
                        // Note that the inward normal of AB in OAB is in this case the same as the outward normal of AB in ABC.
                        direction = ABout;
                    }
                    else // Once again, the origin can also be beyond A; A is furthest along BO, not BA.
                    {
                        simplex.Clear();
                        simplex.Add(A);
                        direction = AO;
                    }
                }
                else if (Vector2.Dot(ABout, AO) > 0) // ABx(ABxAC) is outward normal from tri along AB.
                {
                    // The origin is outside, beyond AB.
                    if (Vector2.Dot(AB, AO) > 0) // Is it between A and B?
                    {
                        simplex.Clear();
                        simplex.Add(B);
                        simplex.Add(A);
                        // Search inwards in tri OAB from AB.
                        // Note that the inward normal of AB in OAB is in this case the same as the outward normal of AB in ABC.
                        direction = ABout;
                    }
                    else // Still, the origin can also be beyond A; A is furthest along BO, not BA.
                    {
                        simplex.Clear();
                        simplex.Add(A);
                        direction = AO;
                    }
                }
                else
                {
                    // The origin is inside the tri.
                    return true;
                }
            }
            // No intersection found on this iteration.
            return false;
        }

        /// <summary>
        /// Finds the farthest point along a given direction of the Minkowski difference of two convex polyhedra.
        /// Called Support in the video lecture: max(D.Ai) — max(-D.Bj)
        /// </summary>
        private static Vector3 MaxPointInMinkDiffAlongDir(GJKSupport3d shape1, GJKSupport3d shape2, Vector3 direction)
        {
            return shape1.FurthestPoint(direction) - shape2.FurthestPoint(-direction);
        }

        /// <summary>
        /// Finds the farthest point along a given direction of the Minkowski difference of two convex polyhedra.
        /// Called Support in the video lecture: max(D.Ai) — max(-D.Bj)
        /// </summary>
        private static Vector2 MaxPointInMinkDiffAlongDir(GJKSupport2d shape1, GJKSupport2d shape2, Vector2 direction)
        {
            return shape1.FurthestPoint(direction) - shape2.FurthestPoint(-direction);
        }
    }
}