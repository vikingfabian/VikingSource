using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest
{
    class FlyingObjPhysics2 : AbsPhysics
    {
        new GO.AbsVoxelObj parent;
        Vector3 oldPos;
        Vector3 oldPosAdjusted;

        public FlyingObjPhysics2(GO.AbsUpdateObj parent)
            : base(parent)
        {
            this.parent = (GO.AbsVoxelObj)parent;
        }

        public override GO.Bounds.BoundCollisionResult ObsticleCollision()
        {
            Vector3 pos = parent.Position;
            pos.Z += 0.5f;
            pos.X += 0.5f;
            pos.Y += 0.6f;
            TerrainColl coll = parent.GetGroundInteractBound.CollectTerrainObsticles(pos, oldPos, false);
            if (coll.Collition)
            {
                parent.HandleTerrainColl3D(coll, oldPos);
                return null;
            }
            oldPos = parent.Position;
            oldPosAdjusted = pos;
            return null;
        }
       
    }
}

