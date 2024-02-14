//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace Game1.LootFest.GameObjects.Magic
//{
//    class EvilMissil : AbsNoImageObj
//    {
//        const float MoveSpeed = 0.1f;
//        const float LifeTime = 1400;
//        const float DamageRadius = 5;
//        static readonly Weapons.DamageData Damage = new Weapons.DamageData(LootfestLib.EvilMissileDamage, Weapons.WeaponUserType.NON, 0, Gadgets.GoodsType.NONE, MagicType.Evil);

//        public EvilMissil(Vector3 startPos, Rotation1D dir)
//            : base()
//        {
//            speed = dir.Direction(MoveSpeed);
//            health = LifeTime;
            
//            addPhysics();
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            movePosition(time);
//            health -= time;

//            Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Smoke, position, 1f, 4);
//            this.physics.Update(time);
//            if (health <= 0)
//            {
//                DeleteMe();
//            }
//        }
//        public override void AIupdate(GameObjects.UpdateArgs args)
//        {
//            //characterCollCheck(active);
//            foreach (AbsUpdateObj obj in active)
//            {
//                if (obj.Type == ObjectType.Character && distanceToObject(obj) <= DamageRadius)
//                {
//                    obj.TakeDamage(Damage, true);
//                }
//            }
//        }
//        //protected override bool handleCharacterColl(AbsUpdateObj character, LootFest.ObjBoundCollData collisionData)
//        //{
//        //    character.TakeDamage(Damage, true);
//        //    return false;
//        //}
//        public override void HandleColl3D(Game1.Physics.Bound3dIntersect collData, GameObjects.AbsUpdateObj ObjCollision)
//        {
//            DeleteMe();
//        }
//        public override ObjPhysicsType PhysicsType
//        {
//            get
//            {
//                return ObjPhysicsType.Projectile;
//            }
//        }

//        public override ObjectType Type
//        {
//            get
//            {
//                return ObjectType.Weapon;
//            }
//        }
//        public override int UnderType
//        {
//            get
//            {
//                return (int)Weapons.WeaponUtype.EvilMissile;
//            }
//        }
//    }
//}
