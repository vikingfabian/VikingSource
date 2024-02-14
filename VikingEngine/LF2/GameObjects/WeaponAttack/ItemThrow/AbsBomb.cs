using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.WeaponAttack.ItemThrow
{
    abstract class AbsBomb: AbsVoxelObj
    {
        static readonly Data.TempBlockReplacementSett BombTempImage = new Data.TempBlockReplacementSett(new Color(186,164,106), new Vector3(0.9f));
        //Gadgets.ItemType type;
        protected AbsUpdateObj callBackObj;
        int numBounces = 2;
        protected Rotation1D startDir;
        const float Scale = 1.6f;

        public AbsBomb(GameObjects.Characters.AbsCharacter h, float sideSpeed)
            :base()
        {
            Velocity = new Velocity( h.Rotation, sideSpeed);
            physics.SpeedY = 0.012f;
            bombInit();
            image.position = h.Position;
            image.position.Y += 2;
            WorldPosition = new Map.WorldPosition(image.position);
            startDir = h.Rotation;
            const float BoundW = 0.3f;
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(new Vector3(BoundW * Scale, 0.5f * Scale, BoundW * Scale), 0.42f * Scale);

            callBackObj = h;
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WritePosition(image.position, w);
        }

        public AbsBomb(System.IO.BinaryReader r)
            : base(r)
        {
            const float ClientMaxLifeTime = 10000;
            Health = ClientMaxLifeTime;
            bombInit();

            image.position = ReadPosition(r);
        }

        void bombInit()
        {

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.bottle, BombTempImage, Scale, 1);
        }


        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
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
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_BombExplosion, Network.PacketReliability.Reliable);
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
                new Effects.BouncingBlock2(image.position, Data.MaterialType.blue_gray, 0.2f);
            }
        }

        abstract protected void Explode(UpdateArgs args);

        virtual protected void NetworkShareExplosion(System.IO.BinaryWriter w)
        {
            ObjOwnerAndId.WriteStream(w);
            AbsUpdateObj.WritePosition(image.position, w);
        }

        virtual public void NetworkReadExplosion(System.IO.BinaryReader r)
        {
            image.position = AbsUpdateObj.ReadPosition(r);
            numBounces = 0;
        }


        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
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

        public override ObjectType Type
        {
            get { return ObjectType.WeaponAttack; }
        }

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
                return GameObjects.NetworkShare.FullExceptRemoval;
            }
        }
    }
}
