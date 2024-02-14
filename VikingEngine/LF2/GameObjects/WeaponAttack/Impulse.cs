using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;



namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    abstract class Impulse : AbsWeapon
    {
        
        public Impulse(float lifeTime, Graphics.AbsVoxelObj image,
            Vector3 scale, DamageData givesDamage)
            : base( givesDamage, image, scale)
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

                Health -= args.halfUpdateTime;
                if (Health <= 0)
                { DeleteMe(); }
            //}
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
           // base.AIupdate(args);
        }
        static readonly NetworkShare ImpulseShare = new NetworkShare(true, false, false, false);
        override public NetworkShare NetworkShareSettings { get { return ImpulseShare; } }
        

        public override int UnderType
        {
            get { return (int)WeaponUtype.Impulse; }
        }
        //override protected bool NetworkShareDeleteMe
        //{
        //    get { return false; }
        //}
    }
}
