using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna
using VikingEngine.LootFest.Data;

namespace VikingEngine.LootFest.GO.Characters.CastleEnemy
{
    class BackNForwardTrap : AbsTrap2
    {
        const float MoveSpeedLvl1 =  0.025f;
        const float MoveSpeedLvlAdd = 0.005f;

        public BackNForwardTrap(GoArgs goArgs)// Map.WorldPosition wp, int level)
            : base(goArgs)
        {
            WorldPos = goArgs.startWp;
            initMotion();

            if (goArgs.LocalMember)
            {
                NetworkShareObject();
            }
            else
            {
                Velocity.PlaneUpdate(goArgs.packetSender.SendTime, image);
            }
        }


        //public BackNForwardTrap(System.IO.BinaryReader r, Network.AbsNetworkPeer sender)
        //    : base(r)
        //{
        //    initMotion();
        //    Velocity.PlaneUpdate(sender.SendTime, image);
        //   // Time_Update(new UpdateArgs( sender.SendTime, 0, null, null, false));
        //}

        void initMotion()
        {
            float moveSpeed = MoveSpeedLvl1 + MoveSpeedLvlAdd * characterLevel;
            //RandomSeedInstance rnd = new RandomSeedInstance();
            //rnd.SetSeedPosition(WorldPosition.ChunkGrindex);

            if (Ref.rnd.Bool())
            {
                Velocity.PlaneX = moveSpeed;
            }
            else
            {
                Velocity.PlaneY = moveSpeed;
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            CastleEnemy.AbsCastleMonster.CheckCastleRoomBounds(this, roomSize);
            //UpdateBound();
        }

        public override void HandleCastleRoomCollision()
        {
            Velocity *= -1;
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.TrapBackNforward; }
        }
    }
}
