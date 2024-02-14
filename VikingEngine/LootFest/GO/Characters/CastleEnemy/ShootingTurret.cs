using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna

namespace VikingEngine.LootFest.GO.Characters.CastleEnemy
{
    class ShootingTurret : AbsTrap2
    {
        new AbsCharacter target;
        const float ReloadTime = 1200;
        float reloadTime = 0;

        public ShootingTurret(GoArgs args)
            : base(args)
        {
            if (args.LocalMember)
            {
                rotation = Rotation1D.Random();
                setImageDirFromRotation();
                NetworkShareObject();
            }
        }

        //public ShootingTurret(System.IO.BinaryReader r)
        //    : base(r)
        //{ }

        int searchTargetTime = 0;
        const int SearchTargetRate = 20;
        public override void Time_Update(UpdateArgs args)
        {
            clinkSoundTime.CountDown();
            if (localMember)
            {
                
                reloadTime -= args.time;
                searchTargetTime++;
                if (searchTargetTime >= SearchTargetRate)
                {
                    searchTargetTime = 0;
                    target = getClosestCharacter(32, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
                }

                if (target != null)
                {
                    rotateTowardsObject(target, 0.0008f);
                    setImageDirFromRotation();
                    if (reloadTime <= 0)
                    {
                        if (LookingTowardObject(target, 0.3f))
                        {
                            reloadTime = ReloadTime;
                            //fire
                            Vector3 pos = image.position;
                            pos.Y += 0.24f;
                            new WeaponAttack.TurretBullet(pos, rotation);
                        }

                    }
                }
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                }
            }
            else
            {
                rotateTowardClientGoalRot();
                setImageDirFromRotation();
            }
            UpdateBound();
        }
        override protected void trapBound()
        {
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated(Vector3.One, new Vector3(0.3f * Scale, 0.2f * Scale, 0.42f * Scale), 
                new Vector3(0, 0.2f * Scale, 0));
        }
        protected override VoxelModelName voxelImage
        {
            get
            {
                return VoxelModelName.shootingturret;
            }
        }
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Gray, new Vector3(1.5f, 1.5f, 3));
        //override protected Data.IReplacementImage tempImage
        //{
        //    get { return TempImage; }
        //}

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Enemy;
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.ShootingTurret; }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return LootFest.GO.NetworkShare.FullExceptClientDel;
            }
        }
        public override void NetWriteUpdate(System.IO.BinaryWriter writer)
        {
            writer.Write(rotation.ByteDir);
        }
        public override void NetReadUpdate(System.IO.BinaryReader reader)
        {
            clientGoalRotation.ByteDir = reader.ReadByte();
        }
    }
}
