using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna

namespace VikingEngine.LootFest.GO.Characters.CastleEnemy
{
    abstract class AbsTrap2 : AbsEnemy
    {
        Map.WorldPosition spawnPos;
        protected VectorRect roomSize;
        protected new const float Scale = 4.5f;
        protected Time clinkSoundTime = new Time();
         
        public AbsTrap2(GoArgs args)
            : base(args)
        {
            //if (ObjOwnerAndId.X == 0)
            //{
            //    throw new Exception();
            //}
            if (args.LocalMember)
            {
                spawnPos = args.startWp;
            }
            else
            {
                spawnPos.ReadPlanePos(args.reader);
            }

            WorldPos = spawnPos;
            image = LfRef.modelLoad.AutoLoadModelInstance(voxelImage, Scale, 0, false);
            trapBound();
            image.position = WorldPos.PositionV3;
            //image.Position.Y = CastleEnemy.AbsCastleMonster.CastleFloorY;
            Health = float.MaxValue;
            rotation = Rotation1D.Random();
            roomSize = CastleEnemy.AbsCastleMonster.CreateRoomSize(WorldPos.ChunkGrindex);
        }

        public override void netWriteGameObject(System.IO.BinaryWriter writer)
        {
            base.netWriteGameObject(writer);

            //writer.Write((byte)characterLevel);
            spawnPos.WritePlanePos(writer);
        }

        //public AbsTrap2(System.IO.BinaryReader r)
        //    : base(r)
        //{
            
        //    //characterLevel = r.ReadByte();
        //    spawnPos.ReadPlanePos(r);

        //    basicTrapInit();
        //}

        virtual protected VoxelModelName voxelImage
        {
            get { return VoxelModelName.BlockTrap; }
        }

        //static readonly Data.TempBlockReplacementSett BlockTrapTempImage = new Data.TempBlockReplacementSett(new Color(92,84,64), new Vector3(3, 2f, 3));
        //virtual protected Data.IReplacementImage tempImage
        //{
        //    get { return BlockTrapTempImage; }
        //}

        //void basicTrapInit()
        //{
        //    WorldPos = spawnPos;
        //    image = LfRef.modelLoad.AutoLoadModelInstance(voxelImage, tempImage, Scale, 0, false);
        //    trapBound();
        //    image.Position = WorldPos.PositionV3;
        //    //image.Position.Y = CastleEnemy.AbsCastleMonster.CastleFloorY;
        //    Health = float.MaxValue;
        //    rotation = Rotation1D.Random();
        //    roomSize = CastleEnemy.AbsCastleMonster.CreateRoomSize(WorldPos.ChunkGrindex);
        //}

        virtual protected void trapBound()
        {
            const float BoundW = 0.36f;
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated(WorldPos.PositionV3, new Vector3(BoundW * Scale, 0.2f * Scale, BoundW * Scale), new Vector3(0, 0.2f * Scale, 0));
        }

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            if (CollisionAndDefaultBound != null)
                checkCharacterCollision(args.localMembersCounter, false);
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

            CollisionAndDefaultBound.UpdatePosition2(rotation + Rotation1D.D45, image.position);

        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (clinkSoundTime.TimeOut)
            {
                Music.SoundManager.WeaponClink(image.position);
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
                return LootFest.GO.NetworkShare.NoUpdate;
            }
        }
        //protected override bool ViewHealthBar
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
        protected override int MaxLevel
        {
            get
            {
                return 8;
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
