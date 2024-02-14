using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class Collisions
    {
        public float maxMoveLengthBeforeCollsion;
        SafeCollectAsynchList<FieldEdge> edges;
        SafeCollectAsynchList<AbsItem> items;
        SafeCollectAsynchList<Ball> balls;

        public Collisions()
        {
            maxMoveLengthBeforeCollsion = GolfRef.field.squareSize.X * 0.05f;

            edges = new SafeCollectAsynchList<FieldEdge>(GolfRef.objects.edges.Count);
            items = new SafeCollectAsynchList<AbsItem>();
            balls = new SafeCollectAsynchList<Ball>();
        }

        public void updateBallCollisions(Ball ball)
        {
            itemCollisions(ball, ball.bound, true, true);
            fieldCollisions(ball);
            balls.checkForUpdatedList();

            Physics.Collision2D coll;
            foreach (var m in balls.list)
            {
                if (m != ball)
                {
                    coll = m.bound.Intersect2(ball.bound);

                    if (coll.IsCollision)
                    {
                        ball.onBallToBallColl();
                        Vector2 forceToBall2, forceToBall1;
                        
                        bool bForce1 = ballCollsionForce(ball, coll.surfaceNormal, out forceToBall2);
                        bool bForce2 = ballCollsionForce(m, -coll.surfaceNormal, out forceToBall1);

                        Vector2 prevVel1 = ball.velocity;
                        Vector2 prevVel2 = m.velocity;

                        if (bForce1)
                        {
                            ball.addForce(-forceToBall2, m);
                            m.addForce(forceToBall2, ball);
                        }

                        if (bForce2)
                        {
                            m.addForce(-forceToBall1, ball);
                            ball.addForce(forceToBall1, m);
                        }

                        ball.addRotationForce(prevVel1);
                        m.addRotationForce(prevVel2);

                    }
                }
            }
        }

        public void itemCollisions(AbsGameObject obj, Physics.AbsBound2D bound, bool mainBound, bool breakOnCollsion)
        {
            Physics.Collision2D coll;
            items.checkForUpdatedList();

            for (int i = items.list.Count - 1; i >= 0; --i)
            {
                var m = items.list[i];
                coll = bound.Intersect2(m.bound);
                if (coll.IsCollision && m.IsAlive)
                {
                    if (obj.ableToCollideWith(m) && m.ableToCollideWith(obj))
                    {
                        obj.onItemCollision(m, coll, mainBound);
                        if (breakOnCollsion)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public void updateBumpCollisions(BumpWave bump)
        {
            itemCollisions(bump.ball, bump.bound, true, false);
        }

        bool ballCollsionForce(Ball ball, Vector2 surfaceNormal, out Vector2 sendForce)
        {
            sendForce = Vector2.Zero;

            if (VectorExt.HasValue(ball.velocity))
            {
                float dot = Vector2.Dot(VectorExt.SafeNormalizeV2(ball.velocity), surfaceNormal);

                if (dot > 0)
                {
                    float velocityL = ball.velocity.Length();
                    sendForce = (velocityL * dot * ball.weight) * surfaceNormal;

                    return true;
                }
            }
            return false;
        }
        
        public void fieldCollisions(AbsGameObject item)
        {
            //var edges = this.edges;
            edges.checkForUpdatedList();

            Physics.Collision2D coll;
            foreach (var m in edges.list)
            {
                coll = item.bound.Intersect2(m.bound);
                if (coll.IsCollision)
                {
                    item.onFieldCollision(coll, m.elasticity);

                    //test
                    item.bound.Intersect2(m.bound);
                }
            }
        }

        public void otherBallsCollisions(AbsGameObject go)
        {
            Physics.Collision2D coll;

            foreach (var m in balls.list)
            {
                coll = m.bound.Intersect2(go.bound);

                if (coll.IsCollision)
                {
                    go.onBallCollision(m, coll);
                }
            }
        }

        public void collect_asynch(AbsGameObject parentGO, Physics.AbsBound2D bound,
            bool bEdges, bool bItems, bool bBalls)
        {
            if (bEdges)
            {//EDGES
                if (edges.ReadyForAsynchProcessing())
                {
                    edges.processList.Clear();
                    for (int i = 0; i < GolfRef.objects.edges.Count; ++i)
                    {
                        if (bound.Intersect2(GolfRef.objects.edges[i].extremeBound).IsCollision)
                        {
                            edges.processList.Add(GolfRef.objects.edges[i]);
                        }
                    }
                    edges.onAsynchProcessComplete();
                }
            }

            if (bItems)
            {//ITEMS
                if (items.ReadyForAsynchProcessing())
                {
                    items.processList.Clear();
                    for (int i = 0; i < GolfRef.objects.items.Count; ++i)
                    {
                        var item = GolfRef.objects.items[i];
                        if (item != null)
                        {
                            if ((item.image.Position - bound.Center).Length() <= GolfRef.field.itemsExtremeRadius)
                            {
                                if (item != parentGO)
                                {
                                    items.processList.Add(item);
                                }
                            }
                        }
                    }
                    items.onAsynchProcessComplete();
                }
            }

            if (bBalls)
            {//BALLS
                if (balls.ReadyForAsynchProcessing())
                {
                    balls.processList.Clear();
                    for (int i = 0; i < GolfRef.objects.balls.Count; ++i)
                    {
                        var ball = GolfRef.objects.balls[i];
                        if ((ball.image.Position - bound.Center).Length() <= GolfRef.field.itemsExtremeRadius)
                        {
                            if (ball != parentGO)
                            {
                                balls.processList.Add(ball);
                            }
                        }
                    }

                    balls.onAsynchProcessComplete();
                }
            }
        }
    }
}
