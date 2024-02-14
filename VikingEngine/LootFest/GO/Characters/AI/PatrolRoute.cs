using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.AI
{
    class PatrolRoute : AbsSpawnArgument
    {
        int nextDir = 1;
        bool cirkular;
        List<Vector3> nodes;
        int movingTowardsIndex;

        public bool activePatrol = true;
        
        public PatrolRoute(List<Vector3> nodes, bool cirkular, int startMoveTowardsNodeIx)
        {
            this.nodes = nodes;
            this.cirkular = cirkular;
            movingTowardsIndex = Bound.ExMax(startMoveTowardsNodeIx, nodes.Count);

            if (PlatformSettings.ViewCollisionBounds)
            {
                new Timer.Action0ArgTrigger(addDebugBillboards);
            }
        }

        void addDebugBillboards()
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                Map.WorldPosition wp = new Map.WorldPosition(nodes[i]);
                //wp.SetAtHeightMap();
                wp.Y += 3;
                new Graphics.Text3DBillboard(LoadedFont.Console, i.ToString(), Color.Purple, Color.White, wp.PositionV3, 1f, 1f, true);
            }
        }

        public void SetFromClosestPos(Vector3 position)
        {
            FindMinValue lowest = new FindMinValue();
            for (int i = 0; i < nodes.Count; ++i)
            {
                lowest.Next(VectorExt.SideLength(position, nodes[i]), i);
            }

            movingTowardsIndex = lowest.minMemberIndex;
        }

        public void Next()
        {
            movingTowardsIndex += nextDir;
            if (movingTowardsIndex < 0)
            {
                movingTowardsIndex = 1;
                nextDir = 1;
            }
            else if (movingTowardsIndex >= nodes.Count)
            {
                if (cirkular)
                {
                    movingTowardsIndex = 0;
                }
                else
                {
                    movingTowardsIndex = nodes.Count - 2;
                    nextDir = -1;
                }
            }
        }

        public Vector3 WalkingTowards()
        {
            return nodes[movingTowardsIndex];
        }

        public Map.WorldPosition startWp()
        {
            return new Map.WorldPosition(nodes[0]);
        }
    }
}
