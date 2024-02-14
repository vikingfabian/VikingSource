using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//
using VikingEngine.LF2.Data;

namespace VikingEngine.LF2.GameObjects.Characters.CastleEnemy
{
    class BackNForwardTrap : AbsTrap2
    {
        const float MoveSpeedLvl1 =  0.025f;
        const float MoveSpeedLvlAdd = 0.005f;
        
        public BackNForwardTrap(Map.WorldPosition wp, int level)
            : base(wp, level)
        {
            WorldPosition = wp;
            initMotion();
            NetworkShareObject();
        }


        public BackNForwardTrap(System.IO.BinaryReader r, Network.AbsNetworkPeer sender)
            : base(r)
        {
            initMotion();
            Velocity.PlaneUpdate(sender.SendTime, image);
           // Time_Update(new UpdateArgs( sender.SendTime, 0, null, null, false));
        }

        void initMotion()
        {
            float moveSpeed = MoveSpeedLvl1 + MoveSpeedLvlAdd * areaLevel;
            RandomSeedInstance rnd = new RandomSeedInstance();
            rnd.SetSeedPosition(WorldPosition.ChunkGrindex);

            if (rnd.Bool())
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

        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.TrapBackNforward; }
        }
    }
}
