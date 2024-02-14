using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    class FlyingObjPhysics2 : AbsPhysics
    {
        GameObjects.AbsVoxelObj parent;
        Vector3 oldPos;
        Vector3 oldPosAdjusted;

        public FlyingObjPhysics2(GameObjects.AbsUpdateObj parent)
            : base(parent)
        {
            this.parent = (GameObjects.AbsVoxelObj)parent;
        }
        public override VikingEngine.Physics.Collision3D  ObsticleCollision()
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

