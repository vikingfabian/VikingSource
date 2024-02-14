using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
{
    abstract class AbsBomb: AbsVoxelObj
    {
        ////static readonly Data.TempBlockReplacementSett BombTempImage = new Data.TempBlockReplacementSett(new Color(186,164,106), new Vector3(0.9f));
        //Gadgets.ItemType type;
        protected AbsUpdateObj callBackObj;
        int numBounces = 2;
        protected Rotation1D startDir;
        const float Scale = 1.2f;

        public AbsBomb(GoArgs args, GO.Characters.AbsCharacter h, float sideSpeed)
            : base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.handgranade, 
                Scale, 1, false);

            if (args.LocalMember)
            {
                Velocity = new Velocity(h.Rotation, sideSpeed);
                physics.SpeedY = 0.012f;
                //   bombInit();
                image.position = h.Position;
                image.position.Y += 2;
                WorldPos = new Map.WorldPosition(image.position);
                startDir = h.Rotation;
                const float BoundW = 0.3f;
                CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(new Vector3(BoundW * Scale, 0.5f * Scale, BoundW * Scale), 0.42f * Scale);

                callBackObj = h;
            }
            else
            {
                const float ClientMaxLifeTime = 10000;
                Health = ClientMaxLifeTime;
                //bombInit();

                image.position = ReadPosition(args.reader);
            }
        }

        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            WritePosition(image.position, w);
        }

        //public AbsBomb(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    const float ClientMaxLifeTime = 10000;
        //    Health = ClientMaxLifeTime;
        //    bombInit();

        //    image.Position = ReadPosition(r);
        //}

        //void bombInit()
        //{

        //    image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.handgranade, BombTempImage, Scale, 1, false, new Graphics.AnimationsSettings(2,0));
        //}


        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            numBounces--;
        }

        public override void Time_Update(UpdateArgs args)
        {
            #if !CMODE
            
            if (localMember)
            {
                physics.Update(args.time);
            }
            else
            {
                Health -= args.time;
                if (Health <= 0)
                {
                    this.DeleteMe();
                }
            }

            if (numBounces <= 0)
            {
                basicExplosionEffect(args);
                if (localMember)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.BombExplosion, Network.PacketReliability.Reliable);
                    NetworkShareExplosion(w);
                }
            }
            base.Time_Update(args);
#endif
        }

        void basicExplosionEffect(UpdateArgs args)
        {
            Music.SoundManager.PlaySound(LoadedSound.shieldcrash, image.position);
            Explode(args);

            this.DeleteMe();

            for (int i = 0; i < 3; i++)
            {
                new Effects.BouncingBlock2(image.position, Data.MaterialType.white, 0.2f);
            }
        }

        abstract protected void Explode(UpdateArgs args);

        virtual protected void NetworkShareExplosion(System.IO.BinaryWriter w)
        {
            ObjOwnerAndId.write(w);
            AbsUpdateObj.WritePosition(image.position, w);
        }

        virtual public void NetworkReadExplosion(System.IO.BinaryReader r)
        {
            image.position = AbsUpdateObj.ReadPosition(r);
            numBounces = 0;
        }


        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            character.TakeDamage(WeaponAttack.DamageData.BasicCollDamage, true);
            return false;
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.BouncingObj;
            }
        }

        //public override GameObjectType Type
        //{
        //    get { return GameObjectType.WeaponAttack; }
        //}

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.NON;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.FullExceptRemoval;
            }
        }
    }
}
