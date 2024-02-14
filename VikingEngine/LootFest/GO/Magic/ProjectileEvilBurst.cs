//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.Magic
//{
//    class ProjectileEvilBurst : AbsNoImageObj
//    {
//       // static readonly 
//        bool done = false;
//        public ProjectileEvilBurst(Vector3 position)
//            :base()
//        {
//            this.position = position;
            
//        }
//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            if (!done)
//            {
//                WeaponAttack.DamageData damage = new WeaponAttack.DamageData(LfLib.ProjectileEvilBurstDamage, WeaponAttack.WeaponUserType.NON, NetworkId.Empty, MagicElement.Evil);

//                args.allMembersCounter.Reset();
//                while (args.allMembersCounter.Next())
//                {
//                    if (args.allMembersCounter.GetMember is Characters.AbsCharacter && distanceToObject(args.allMembersCounter.GetMember) <= 6)
//                    {
//                        args.allMembersCounter.GetMember.TakeDamage(damage, true);
//                    }
//                }
//                done = true;
//            }
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            if (done)
//            {
//                DeleteMe();
//            }
            
//        }

//        public override GameObjectType Type
//        {
//            get
//            {
//                return GameObjectType.ProjectileEvilBurst;
//            }
//        }
//    }
//}
