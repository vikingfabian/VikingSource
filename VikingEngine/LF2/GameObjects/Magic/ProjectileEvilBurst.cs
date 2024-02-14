using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Magic
{
    class ProjectileEvilBurst : AbsNoImageObj
    {
       // static readonly 
        bool done = false;
        public ProjectileEvilBurst(Vector3 position)
            :base()
        {
            this.position = position;
            
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (!done)
            {
                WeaponAttack.DamageData damage = new WeaponAttack.DamageData(LootfestLib.ProjectileEvilBurstDamage, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, MagicElement.Evil);

                args.allMembersCounter.Reset();
                while (args.allMembersCounter.Next())
                {
                    if (args.allMembersCounter.GetMember.Type == ObjectType.Character && distanceToObject(args.allMembersCounter.GetMember) <= 6)
                    {
                        args.allMembersCounter.GetMember.TakeDamage(damage, true);
                    }
                }
                done = true;
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (done)
            {
                DeleteMe();
            }
            
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
            get
            {
                return (int)WeaponAttack.WeaponUtype.ProjectileEvilBurst;
            }
        }
    }
}
