using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna

namespace VikingEngine.LootFest.GO.Characters.CastleEnemy
{
    
    class RotatingTrap : AbsTrap2
    {
        float swingAngularSpeed = 0.002f;
        const float RotationSpeedLvl1 = 0.004f;
        const float RotationSpeedLvlAdd = 0.0005f;
        float rotationSpeed;
        Rotation1D swingPosision;
        Vector2 startPos;
        float swingRadius = 6;

        public RotatingTrap(GoArgs args)
            : base(args)
        {

            rotationSpeed = RotationSpeedLvl1 + RotationSpeedLvlAdd * characterLevel;
            startPos = PlanePos;
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
            else
            {
                moveUpdate(args.packetSender.SendTime);
            }
        }

        //public RotatingTrap(System.IO.BinaryReader r, Network.AbsNetworkPeer sender)
        //    : base(r)
        //{
        //    basicRotatingTrapInit();
        //    //Time_Update(new UpdateArgs(sender.SendTime, 0, null, null, false));
        //    moveUpdate(sender.SendTime);
        //}

        protected void basicRotatingTrapInit()
        {
            rotationSpeed = RotationSpeedLvl1 + RotationSpeedLvlAdd * characterLevel;
            startPos = PlanePos;
        }

        public override void Time_Update(UpdateArgs args)
        {
            moveUpdate(args.time);
            
            setImageDirFromRotation();

            base.Time_Update(args);
        }

        void moveUpdate(float time)
        {
            swingPosision.Radians += swingAngularSpeed * time;
            Vector2 pos = swingPosision.Direction(swingRadius);
            pos.X += startPos.X;
            pos.Y += startPos.Y;

            PlanePos = pos;
            rotation += rotationSpeed * time;
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.TrapRotating; }
        }
    }

    
    
}
