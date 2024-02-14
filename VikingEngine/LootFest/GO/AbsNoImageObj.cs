using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO
{
    abstract class AbsNoImageObj : AbsUpdateObj
    {
        protected Vector3 position;

        public AbsNoImageObj(GoArgs args)
            : base(args)
        { }
        //public AbsNoImageObj(System.IO.BinaryReader r)
        //    : base(r)
        //{ }

        public override void HandleTerrainColl3D(LootFest.TerrainColl collSata, Vector3 oldPos)
        {
            //base.HandleTerrainColl3D(collSata, oldPos);
        }
        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //base.HandleColl3D(collData, ObjCollision);
        }
        public override Vector2 PlanePos
        {
            get
            {
                return VectorExt.V3XZtoV2(position);
            }
            set
            {
                position = VectorExt.V2toV3XZ(value, position.Y);
            }
        }
        public override Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected void movePosition(float time)
        {
            //position.X += Speed.X * time;
            //position.Z += Speed.Y * time;
            position = Velocity.Update(time, position);
        }

        public override float X
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public override float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public override float Z
        {
            get { return position.Z; }
            set { position.Z = value; }
        }
        public override bool VisibleInCam(int camIx)
        {
            return true;
        }

        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.NoRecieving; }
        }
    }
}
