using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Magic
{
    class ProjectilePoisionBurst : AbsNoImageObj
    {
        const float LifeTime = 1600;
        const float MoveSpeed = 0.1f;
        const int CheckRate = 4;
        int checkRate = 0;

        public ProjectilePoisionBurst(Vector3 position, Rotation1D dir)
            :base()
        {
            Health = LifeTime;
            Velocity = new Velocity(dir, MoveSpeed);//dir.Direction(MoveSpeed);
        }

        public override void Time_Update(UpdateArgs args)
        {
            position = Velocity.Update(args.time, position);
            //position += Map.WorldPosition.V2toV3(Speed * args.time);
            checkRate++;

            if (checkRate >= CheckRate)
            {
                if (UpdateWorldPos())
                {
                    WorldPosition.SetFromGroundY(2);
                    position.Y = WorldPosition.WorldGrindex.Y;
                }


                checkRate = 0;

                Health -= args.time;
                if (Health <= 0)
                {
                    DeleteMe();
                }
#if !CMODE
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Poision,
                    new Graphics.ParticleInitData(position));
#endif
            }
            //base.Time_Update(args);
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            characterCollCheck(args.allMembersCounter);
            //base.AIupdate(args);
        }

        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(LootfestLib.ProjectileEvilBurstDamage,
            WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, MagicElement.Poision);
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            character.TakeDamage(Damage, true);
            return false;
        }

        public override ObjectType Type
        {
            get
            {
                return ObjectType.WeaponAttack;
            }
        }
        public override int UnderType
        {
            get { return (int)WeaponAttack.WeaponUtype.ProjectilePoisionBurst; }
        }
    }
}
