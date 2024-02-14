//using VikingEngine.LootFest.Map.Terrain.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.EngineSpace.Maths
//{
//    /// <summary>
//    /// Layouts rectangles...
//    /// </summary>
//    class BSPRectangleLayouter
//    {
//        BSPTree<bool> tree;

//        public BSPRectangleLayouter()
//        {
//            // Make a huge tree to always be able to encompass rects.
//            tree = new BSPTree<bool>(new Rectangle2(0, 0, int.MaxValue, int.MaxValue));
//        }

//        /// <summary>
//        /// Layouts a new rectangle of the given size, and returns it.
//        /// </summary>
//        public Rectangle2 LayoutAdditionalRect(IntVector2 rectSize)
//        {
//            // Create a rectangle2 for fitting
//            Rectangle2 rect = new Rectangle2(rectSize);

//            // We want the leaf that is euclidianly closest to the top left
//            // corner that is big enough to encompass our rect.
//            float minimumDistance = float.MaxValue;
//            BSPNode<bool> closest = null;

//            // Find which leaf to put our new rect in.
//            List<BSPNode<bool>> leaves = tree.GetAllLeaves();
//            foreach (BSPNode<bool> leaf in leaves)
//            {
//                // If we can find a leaf that is big enough to fit the rect,
//                if (leaf.data == false &&
//                    leaf.rect.size.X >= rectSize.X &&
//                    leaf.rect.size.Y >= rectSize.Y)
//                {
//                    // and it's 2-norm closer to the top left corner
//                    float distSq = Mgth.Norm(leaf.rect.pos, 2);
//                    if (distSq < minimumDistance)
//                    {
//                        minimumDistance = distSq;
//                        closest = leaf;
//                    }
//                }
//            }

//            // Now we can position our rectangle.
//            rect.pos = closest.rect.pos;

//            // Finally we divide the BSP tree to perfectly fit the rect, 
//            // beginning with the closest of edges.
//            if (rect.Bottom < rect.Right)
//            {
//                // Y first
//                tree.DivideNode(closest, rect.size.Y, Dimensions.X);
//                tree.DivideNode(closest.l, rect.size.X, Dimensions.Y);
//            }
//            else
//            {
//                // X first
//                tree.DivideNode(closest, rect.size.X, Dimensions.Y);
//                tree.DivideNode(closest.l, rect.size.Y, Dimensions.X);
//            }

//            // Now, make sure we remember that we put something here.
//            closest.l.l.data = true;

//            // And return the layouted rect.
//            return rect;
//        }

//        /// <summary>
//        /// Gives you the smallest size encompassing all added rectangles.
//        /// </summary>
//        /// <returns></returns>
//        public IntVector2 CalculateSize()
//        {
//            IntVector2 size = IntVector2.Zero;
//            foreach (BSPNode<bool> leaf in tree.GetAllLeaves())
//            {
//                if (size.X < leaf.rect.Right && leaf.rect.Right < int.MaxValue)
//                    size.X = leaf.rect.Right;
//                if (size.Y < leaf.rect.Bottom && leaf.rect.Bottom < int.MaxValue)
//                    size.Y = leaf.rect.Bottom;
//            }

//            return size;
//        }
//    }
//}
