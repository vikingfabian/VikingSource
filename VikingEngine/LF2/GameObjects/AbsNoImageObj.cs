using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects
{
    abstract class AbsNoImageObj : AbsUpdateObj
    {
        protected Vector3 position;

        public AbsNoImageObj()
            : base()
        { }
        public AbsNoImageObj(System.IO.BinaryReader r)
            : base(r)
        { }

        public override void HandleTerrainColl3D(LF2.TerrainColl collSata, Vector3 oldPos)
        {
            //base.HandleTerrainColl3D(collSata, oldPos);
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, AbsUpdateObj ObjCollision)
        {
            //base.HandleColl3D(collData, ObjCollision);
        }
        public override Vector2 PlanePos
        {
            get
            {
                return Map.WorldPosition.V3toV2(position);
            }
            set
            {
                position = Map.WorldPosition.V2toV3(value, position.Y);
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
