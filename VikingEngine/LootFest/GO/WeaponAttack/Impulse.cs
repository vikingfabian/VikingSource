using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;



namespace VikingEngine.LootFest.GO.WeaponAttack
{
    abstract class Impulse : AbsWeapon
    {
        
        public Impulse(GoArgs args ,float lifeTime, Graphics.AbsVoxelObj image,
            Vector3 scale, DamageData givesDamage)
            : base(args, givesDamage, image, scale)
        {
            Health = lifeTime;
        }
        public override void Time_Update(UpdateArgs args)
        {
            //if (args.halfUpdate == halfUpdateRandomBool)
            //{
                if (givesDamage.User == WeaponAttack.WeaponUserType.Player || givesDamage.User == WeaponUserType.Friendly)
                {
                    if (localMember)
                        characterCollCheck(args.allMembersCounter);
                }
                else
                {
                    characterCollCheck(args.localMembersCounter);
                }

                Health -= Ref.DeltaTimeMs;
                if (Health <= 0)
                { DeleteMe(); }
            //}
        }
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
           // base.AIupdate(args);
        }
        static readonly NetworkShare ImpulseShare = new NetworkShare(true, false, false, false);
        override public NetworkShare NetworkShareSettings { get { return ImpulseShare; } }
        

        public override GameObjectType Type
        {
            get { return GameObjectType.Impulse; }
        }
        //override protected bool NetworkShareDeleteMe
        //{
        //    get { return false; }
        //}
    }
}
