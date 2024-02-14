using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Maths
{
    class KdNode2<T>
    {
        /* Fields */
        public T data;
        public Vector2 pos;
        public KdNode2<T> l;
        public KdNode2<T> r;
        public int depth;

        /* Constructors */
        public KdNode2(T data, Vector2 pos)
        {
            this.data = data;
            this.pos = pos;
            l = null;
            r = null;
            depth = 0;
        }

        /* Family Methods */
        public override string ToString()
        {
            return "[(" + pos.ToString() + "):" + data.ToString() + "]";
        }
    }

    class KdNode2Comparer<T> : IComparer<KdNode2<T>>
    {
        /* Fields */
        public int axis;

        /* Constructors */
        public KdNode2Comparer()
        {
            axis = 0;
        }

        /* Interface Methods */
        public int Compare(KdNode2<T> n0, KdNode2<T> n1)
        {
            return n0.pos.GetDim(axis).CompareTo(n1.pos.GetDim(axis));
        }
    }

    class KdTree2<T>
    {
        /* Constants */
        const int DIMENSION_COUNT = 2;
        const int PRINT_SPACES_PER_TAB = 2;

        /* Fields */
        KdNode2<T> root;
        KdNode2Comparer<T> comparer;

        /* Constructors */
        public KdTree2()
        {
            comparer = new KdNode2Comparer<T>();
        }
        public KdTree2(KdNode2<T>[] points)
            : this()
        {
            root = ConstructRecursively(points, 0, 0, points.Length);
        }

        /* Novelty Methods */
        public void Construct(KdNode2<T>[] points)
        {
            root = ConstructRecursively(points, 0, 0, points.Length);
        }

        KdNode2<T> ConstructRecursively(KdNode2<T>[] points, int depth, int first, int count)
        {
            // This algorithm can most definitely be optimized quite a bit :)

            if (count == 0)
            {
                return null;
            }

            int axis = depth % DIMENSION_COUNT;
            comparer.axis = axis;

            Array.Sort(points, first, count, comparer);
            int median = count / 2;

            KdNode2<T> node = points[first + median];
            node.depth = depth;
            node.l = ConstructRecursively(points, depth + 1, first, median);
            int move = median + 1;
            node.r = ConstructRecursively(points, depth + 1, first + move, count - move);

            return node;
        }

        public KdNode2<T> NearestNeighborSearch(Vector2 position, INorm norm, out float minDistance)
        {
            Stack<KdNode2<T>> visited = new Stack<KdNode2<T>>();
            KdNode2<T> node = root;
            KdNode2<T> closest = null;
            minDistance = float.MaxValue;
            bool moveDownTree = true;

            // standard nearest neighbor algorithm, but with recursion unfolded
            do
            {
                float distance = norm.Norm(position - node.pos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = node;
                }

                int axis = node.depth % DIMENSION_COUNT;
                float diff = node.pos.GetDim(axis) - position.GetDim(axis);
                bool checkLeft = 0 < diff;
                bool left = node.l != null;
                bool right = node.r != null;

                if (moveDownTree)
                {
                    if (checkLeft)
                    {
                        if (left)
                        {
                            visited.Push(node);
                            node = node.l;
                            continue;
                        }
                    }
                    else if (right)
                    {
                        visited.Push(node);
                        node = node.r;
                        continue;
                    }
                }
                else if (norm.Norm(diff) < minDistance)
                {
                    if (checkLeft)
                    {
                        if (right)
                        {
                            node = node.r;
                            moveDownTree = true;
                            continue;
                        }
                    }
                    else if (left)
                    {
                        node = node.l;
                        moveDownTree = true;
                        continue;
                    }
                }

                // failed to find child candidates

                if (visited.Count > 0)
                {
                    node = visited.Pop();
                    moveDownTree = false;
                }
                else
                {
                    node = null;
                }
            }
            while (node != null);

            visited = null; // help gc

            return closest;
        }

        /* Family Methods */
        public override string ToString()
        {
            string result = string.Empty;

            // depth first print all nodes
            KdNode2<T> node = root;
            Stack<KdNode2<T>> nodesRemaining = new Stack<KdNode2<T>>();
            do
            {
                result += new string(' ', node.depth * PRINT_SPACES_PER_TAB) + node.ToString() + "\n";
                if (node.l != null)
                {
                    if (node.r != null)
                    {
                        nodesRemaining.Push(node.r);
                    }
                    node = node.l;
                }
                else
                {
                    if (nodesRemaining.Count == 0)
                        node = null;
                    else
                        node = nodesRemaining.Pop();
                }
            }
            while (node != null);

            return result;
        }
    }

    class KdTree2Tester
    {
        /* Static */
        public static void Test()
        {
            KdTree2<Rectangle2> tree = new KdTree2<Rectangle2>();

            int N = 100;
            Rectangle2[] rects = new Rectangle2[N];
            for (int i = 0; i != N; ++i)
            {
                rects[i] = new Rectangle2(new IntVector2(Ref.rnd.Int(), Ref.rnd.Int()), Ref.rnd.Int());
            }

            List<KdNode2<Rectangle2>> nodes = new List<KdNode2<Rectangle2>>();
            for (int i = 0; i != N; ++i)
            {
                Rectangle2 r = rects[i];
                nodes.Add(new KdNode2<Rectangle2>(r, r.pos.Vec));
                nodes.Add(new KdNode2<Rectangle2>(r, r.TopRightTile.Vec));
                nodes.Add(new KdNode2<Rectangle2>(r, r.BottomLeftTile.Vec));
                nodes.Add(new KdNode2<Rectangle2>(r, r.BottomRightTile.Vec));
            }
            //for (int i = 0; i != N; ++i)
            //{
            //    nodes.Add(new KdNode2<int>(ints[i], new Vector2((float)Ref.rnd.Double(), (float)Ref.rnd.Double())));
            //}
            //tree.Construct(nodes.ToArray());

            //List<KdNode2<int>> nodes = new List<KdNode2<int>>();
            //nodes.Add(new KdNode2<int>(0, new Vector2(0.52f, 0.85f)));
            //nodes.Add(new KdNode2<int>(1, new Vector2(0.375f, 0.53f)));
            //nodes.Add(new KdNode2<int>(2, new Vector2(0.625f, 0.53f)));
            //nodes.Add(new KdNode2<int>(3, new Vector2(0.425f, 0.2f)));
            //nodes.Add(new KdNode2<int>(4, new Vector2(0.13f, 0.8f)));
            //nodes.Add(new KdNode2<int>(5, new Vector2(0.75f, 0.5f)));
            //nodes.Add(new KdNode2<int>(6, new Vector2(0.8f, 0.67f)));
            //nodes.Add(new KdNode2<int>(7, new Vector2(0.125f, 0.3f)));
            //nodes.Add(new KdNode2<int>(11, new Vector2(0.69f, 0.15f)));
            tree.Construct(nodes.ToArray());

            Vector2 search = new Vector2(0.74f, 0.55f);
            //float l2 = (nodes[2].pos - search).Length();
            //float l5 = (nodes[5].pos - search).Length();

            //KdNode2<int> node = tree.NearestNeighborSearch(search);

            Debug.Log(tree.ToString());
        }
    }
}
