using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Physics
{
    static class PhysicsLib2D
    {
        public static bool ExtremeRadiusColl(AbsBound2D obj1, AbsBound2D obj2)
        {
            float radius = obj1.ExtremeRadius + obj2.ExtremeRadius;
            return CollideWith(obj1.Center, obj2.Center, radius);
        }

        public static bool CollideWith(Vector2 myPos, Vector2 otherObjectPos, float collisionRadius)
        {
            float collitionDiffX = myPos.X - otherObjectPos.X;
            float collitionDiffY = myPos.Y - otherObjectPos.Y;
            return (Math.Abs(collitionDiffX) <= collisionRadius && Math.Abs(collitionDiffY) <= collisionRadius);
        }

        public static bool CirkleCollision(
            Vector3 myPosXZ, float myRadius, 
            Vector3 otherPosXZ, float otherRadius)
        {
            return VectorExt.Length(myPosXZ.X - otherPosXZ.X, myPosXZ.Z - otherPosXZ.Z) <
                myRadius + otherRadius;
        }

        public static float CirkleDistance(
           Vector3 myPosXZ, float myRadius,
           Vector3 otherPosXZ, float otherRadius)
        {
            return VectorExt.Length(myPosXZ.X - otherPosXZ.X, myPosXZ.Z - otherPosXZ.Z) -
                (myRadius + otherRadius);
        }

        public static bool MinRadiusColl(AbsBound2D obj1, AbsBound2D obj2)
        {
            float radius = obj1.InnerCirkleRadius + obj2.InnerCirkleRadius;
            Vector2 collitionDiff = obj1.Center - obj2.Center;
            if (collitionDiff.Length() < radius) return true;

            return false;
        }
        public static bool CollRectangles(AbsBound2D obj1, AbsBound2D obj2)
        {
            if (obj1.VerticeIntersect(obj2.Vertices()))
                return true;
            if (obj2.VerticeIntersect(obj1.Vertices()))
                return true;
            return false;
        }

        public static Collision2D CollRectangles_V2(AbsBound2D myBound, AbsBound2D otherBound)
        {
            //Den kanten som vertisen är närmast, avgör normalen
            Vector2 normal;
            float intersectionSz;

            if (VerticesAndRectIntersect(myBound.Vertices(), otherBound, out intersectionSz, out normal))
            {
                return new Collision2D(normal, intersectionSz);
            }
            if (VerticesAndRectIntersect(otherBound.Vertices(), myBound, out intersectionSz, out normal))
            {
                return new Collision2D(-normal, intersectionSz);
            }

            return Collision2D.NoCollision;
        }

        public static bool VerticesAndRectIntersect(Vector2[] vertices, AbsBound2D rect, out float intersectionSz, out Vector2 edgeNormal)
        {
            float negativRot = -rect.Rotation;

            if (negativRot < 0)
            {
                negativRot += MathExt.Tau;
            }

            var area = rect.AreaCentered;

            foreach (Vector2 v in vertices)
            { 
                Vector2 adjustedVertice = lib.RotatePointAroundCenter(area.Center, v, negativRot);
                if (area.IntersectPoint(adjustedVertice))
                {
                    edgeNormal = IntVector2.FromDir4(area.ClosestEdge(adjustedVertice, out intersectionSz)).Vec;
                    VectorExt.RotateVector(edgeNormal, rect.Rotation);
                    return true;
                }
            }

            edgeNormal = Vector2.Zero;
            intersectionSz = 0;
            return false;
        }


        public static bool CollCirkleAndRectangle(CircleBound cirkle, AbsBound2D rectangle)
        {
            // clamp(value, min, max) - limits value to the range min..max

            // Find the closest point to the circle within the rectangle
            float closestX = Bound.Set(cirkle.Center.X, rectangle.Center.X - rectangle.HalfSize.X, rectangle.Center.X + rectangle.HalfSize.X); //clamp(circle.X, rectangle.Left, rectangle.Right);
            float closestY = Bound.Set(cirkle.Center.Y, rectangle.Center.Y - rectangle.HalfSize.Y, rectangle.Center.Y + rectangle.HalfSize.Y); //clamp(circle.Y, rectangle.Top, rectangle.Bottom);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = cirkle.Center.X - closestX;
            float distanceY = cirkle.Center.Y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (cirkle.radius * cirkle.radius);
        }

        public static Collision2D CollCirkleAndRectangleRotated_v2(AbsBound2D cirkle, AbsBound2D rectangle, bool iAmCirkle)
        {
            RectangleRotatedBound rectConv = (RectangleRotatedBound)rectangle;
            Vector2 cirkleCenter = lib.RotatePointAroundCenter(rectConv.area.Center, cirkle.Center, -rectConv.rotation.Radians);

            Collision2D result = CollCirkleAndRectangle_v2(cirkleCenter, cirkle.InnerCirkleRadius, rectConv, iAmCirkle);

            if (result.IsCollision)
            {
                result.surfaceNormal = VectorExt.RotateVector(result.surfaceNormal, rectConv.rotation.Radians);
            }

            return result;
        }

        public static Collision2D CollCirkleAndRectangle_v2(AbsBound2D cirkle, AbsBound2D rectangle, bool iAmCirkle)
        {
            return CollCirkleAndRectangle_v2(cirkle.Center, cirkle.InnerCirkleRadius, rectangle, iAmCirkle);
        }

        public static Collision2D CollCirkleAndRectangle_v2(Vector2 cirkleCenter, float cirkleRadius, 
            AbsBound2D rectangle, bool iAmCirkle)
        {
            RectangleCentered rectArea = rectangle.AreaCentered;//new RectangleCentered(rectangle.Center, rectangle.HalfSize);
            Vector2 centerDiff = cirkleCenter - rectArea.Center;            

            bool cirkleOnLeftSide = centerDiff.X < 0;
            bool cirkleOnTopSide = centerDiff.Y < 0;

            Vector2 cirkleEdgeX = cirkleCenter;
            cirkleEdgeX.X += cirkleOnLeftSide ? cirkleRadius : -cirkleRadius;

            Vector2 cirkleEdgeY = cirkleCenter;
            cirkleEdgeY.Y += cirkleOnTopSide ? cirkleRadius : -cirkleRadius;

            Collision2D result = Collision2D.NoCollision;
            Vector2 rectCollisionPoint = Vector2.Zero;

            if (rectArea.IntersectPoint(cirkleEdgeX))
            {
                if (cirkleOnLeftSide)
                {
                    result.intersectionSize.X = cirkleEdgeX.X - rectArea.Left;
                    result.surfaceNormal.X = -1;
                }
                else
                {
                    result.intersectionSize.X = rectArea.Right - cirkleEdgeX.X;
                    result.surfaceNormal.X = 1;
                }
            }
            else if (rectArea.IntersectPoint(cirkleEdgeY))
            {
                if (cirkleOnTopSide)
                {
                    result.intersectionSize.Y = cirkleEdgeX.Y - rectArea.Top;
                    result.surfaceNormal.Y = -1;
                }
                else
                {
                    result.intersectionSize.Y = rectArea.Bottom - cirkleEdgeX.Y;
                    result.surfaceNormal.Y = 1;
                }
            }
            else
            {
                Vector2 corner = new Vector2(cirkleOnLeftSide ? rectArea.Left : rectArea.Right, cirkleOnTopSide ? rectArea.Top : rectArea.Bottom);

                if (PointInsideCirkle(corner, cirkleCenter, cirkleRadius))
                {
                    Vector2 cirkleToRectDir = VectorExt.SafeNormalizeV2(centerDiff);
                    Vector2 cirkleClosestPoint = cirkleCenter - cirkleToRectDir * cirkleRadius;
                    result.surfaceNormal = VectorExt.SafeNormalizeV2(cirkleCenter - corner);
                    result.intersectionSize = corner - cirkleClosestPoint;
                }
                else
                {
                    return Collision2D.NoCollision;
                }
            }

            result.depth = result.intersectionSize.Length();
            result.IsCollision = true;
            return result;
        }       


        public static Vector2[] StaticBoxVertices(VectorVolumeC area)
        {
            float left = area.Center.X - area.HalfSize.X;
            float right = area.Center.X + area.HalfSize.X;
            float top = area.Center.Z - area.HalfSize.Z;
            float bottom = area.Center.Z + area.HalfSize.Z;

            return new Vector2[]
                { 
                    new Vector2(left, top), 
                    new Vector2(right, top), 
                    new Vector2(left, bottom), 
                    new Vector2(right, bottom)
                };
        }

        public static void DebugViewCollisionVertices(Vector2[] vertices, Vector3 boundCenter)
        {
            //foreach (Vector2 v in vertices)
            //{
            //    boundCenter.X = v.X;
            //    boundCenter.Z = v.Y;
            //    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.GoldenSparkle, boundCenter);
            //}
        }

        public static bool Vertice2DInsideVolume(Vector2 vertice, VectorVolumeC rectangle)
        {
            return Math.Abs(vertice.X - rectangle.Center.X) < rectangle.HalfSize.X &&
                Math.Abs(vertice.Y - rectangle.Center.Z) < rectangle.HalfSize.Z;
        }

        public static float Vertice2DInsideVolumeDepth(Vector2 vertice, VectorVolumeC rectangle)
        {
            return new Vector2(rectangle.HalfSize.X - Math.Abs(vertice.X - rectangle.Center.X),
                rectangle.HalfSize.Z - Math.Abs(vertice.Y - rectangle.Center.Z)).Length();
        }

        public static bool PointInsideCirkle(Vector2 point, Vector2 cikleCenter, float radius)
        {
            return VectorExt.Length(point.X - cikleCenter.X, point.Y - cikleCenter.Y) < radius;
        }

        public static Vector2 VectorAlongNormal(Vector2 vector, Vector2 normal)
        {
            Vector2 normVec = vector;
            normVec.Normalize();
            Vector2 percent = new Vector2(Math.Abs(Math.Abs(normal.X) / Math.Abs(normVec.X)), Math.Abs(Math.Abs(normal.Y) / Math.Abs(normVec.Y)));

            if (!sameDir(normVec.X, normal.X))
            {
                percent.X = 0;
            }
            if (!sameDir(normVec.Y, normal.Y))
            {
                percent.Y = 0;
            }

            if (percent.X < 1)
            {
                vector.X *= percent.X;
            }
            if (percent.Y < 1)
            {
                vector.Y *= percent.Y;
            }

            return vector;
        }

        static bool sameDir(float val1, float val2)
        {
            return (val1 > 0 && val2 > 0) || (val1 < 0 && val2 < 0);
        }
    }    
}
