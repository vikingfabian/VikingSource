using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.GameObjects.Characters.CastleEnemy
{
    abstract class AbsTrap2 : AbsEnemy
    {
        Map.WorldPosition spawnPos;
        protected VectorRect roomSize;
        protected const float Scale = 4.5f;
        protected Time clinkSoundTime = new Time();
         
        public AbsTrap2(Map.WorldPosition wp, int level)
            : base(level)
        {
            //if (ObjOwnerAndId.X == 0)
            //{
            //    throw new Exception();
            //}

            spawnPos = wp;
            basicTrapInit();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);

            writer.Write((byte)areaLevel);
            spawnPos.WritePlanePos(writer);
        }

        public AbsTrap2(System.IO.BinaryReader r)
            : base(r)
        {
            
            areaLevel = r.ReadByte();
            spawnPos.ReadPlanePos(r);

            basicTrapInit();
        }

        virtual protected VoxelModelName voxelImage
        {
            get { return VoxelModelName.BlockTrap; }
        }

        static readonly Data.TempBlockReplacementSett BlockTrapTempImage = new Data.TempBlockReplacementSett(new Color(92,84,64), new Vector3(3, 2f, 3));
        virtual protected Data.IReplacementImage tempImage
        {
            get { return BlockTrapTempImage; }
        }

        void basicTrapInit()
        {
            WorldPosition = spawnPos;
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(voxelImage, tempImage, Scale, 0);
            trapBound();
            image.position = WorldPosition.ToV3();
            image.position.Y = CastleEnemy.AbsCastleMonster.CastleFloorY;
            Health = float.MaxValue;
            rotation = Rotation1D.Random();
            roomSize = CastleEnemy.AbsCastleMonster.CreateRoomSize(WorldPosition.ChunkGrindex);
        }

        virtual protected void trapBound()
        {
            const float BoundW = 0.36f;
            CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated(WorldPosition.ToV3(), new Vector3(BoundW * Scale, 0.2f * Scale, BoundW * Scale), new Vector3(0, 0.2f * Scale, 0));
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (CollisionBound != null)
                checkCharacterCollision(args.localMembersCounter);
        }
        public override void Time_Update(UpdateArgs args)
        {
            clinkSoundTime.CountDown();
            Velocity.PlaneUpdate(args.time, image);
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                }
            }

            CollisionBound.UpdatePosition2(rotation + Rotation1D.D45, image.position);

        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (clinkSoundTime.TimeOut)
            {
                Music.SoundManager.PlaySound(LoadedSound.weaponclink, image.position);
                clinkSoundTime.MilliSeconds = 600;
            }
            return false;//cant die
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return  ObjPhysicsType.NO_PHYSICS;
            }
        }
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return  WeaponAttack.WeaponUserType.PassiveEnemy;
            }
        }
        public override bool IsWeaponTarget
        { get { return true; } }
        override protected bool DoObsticleColl { get { return false; } }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return LF2.GameObjects.NetworkShare.NoUpdate;
            }
        }
        protected override bool ViewHealthBar
        {
            get
            {
                return false;
            }
        }
        protected override int MaxLevel
        {
            get
            {
                return LootfestLib.BossCount;
            }
        }
        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.WeaponBounce;
            }
        }
    }
}
