//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;

//namespace VikingEngine.LootFest.GO.Characters.Condition
//{
//    class Burning : AbsCharCondition
//    {
//        const float AttackInterval = 800;
//        const float BurnTime = AttackInterval * 3.2f;
//        Timer.Basic attackTime = new Timer.Basic(AttackInterval, true);

//        public Burning(AbsCharacter parent, WeaponAttack.DamageData damage)
//            : base(parent, BurnTime)
//        {
//            if (damage.Special == WeaponAttack.SpecialDamage.TinyBoost)
//            {
//                lifeTime *= LfLib.TinyMagicBoost;
//            }
//            else if (damage.Special == WeaponAttack.SpecialDamage.SmallBoost)
//            {
//                lifeTime *= LfLib.SmallMagicBoost;
//            }
//            parent.StunnedSpeedModifier = 0.4f;

//            NetworkShareObject();
//        }

//        public Burning(System.IO.BinaryReader r, Director.GameObjCollection gameObjColllection)
//            : base(r, gameObjColllection, BurnTime)
//        {

//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            if (localMember)
//            {
//                if (attackTime.Update(args.time))
//                {
//                    parent.TakeDamage(new WeaponAttack.DamageData(0.5f,
//                                WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Magic.MagicElement.Fire), true);
//                }
//            }
//            base.Time_Update(args);
//        }

//        public bool AlreadyBurning(GO.Characters.AbsCharacter parent)
//        {
//            if (parent == this.parent)
//            {
//                lifeTime += BurnTime * 0.5f;
//                return true;
//            }
//            return false;
//        }

//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            if (localMember)
//            {
//                //search for nearby characters and burn them
//                //foreach (AbsUpdateObj obj in active)
//                ISpottedArrayCounter<GO.AbsUpdateObj> counter2 = args.localMembersCounter.Clone();

//                args.allMembersCounter.Reset();
//                while (args.allMembersCounter.Next())
//                {
//                    if (args.allMembersCounter.GetMember.Type == GameObjectType.Character && args.allMembersCounter.GetMember != parent)
//                    {
//                        if (parent.DistaceBetweenBounds(args.allMembersCounter.GetMember) <= 4)
//                        {
//                            //make sure he's not already burning
//                            //go through all conditions
//                            //foreach (AbsUpdateObj obj2 in args.localMembersCounter)
//                            while (counter2.Next())
//                            {
//                                if (counter2.GetMember.Type == GameObjectType.CharacterCondition && counter2.GetMember.UnderType == UnderType && 
//                                    counter2.GetMember != args.allMembersCounter.GetMember)
//                                {
//                                    if (((Burning)counter2.GetMember).parent == parent)
//                                    {
//                                        break;
//                                    }
//                                }
//                                new Process.UnthreadedDamage(new WeaponAttack.DamageData(LfLib.BasicDamage,
//                                    WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Magic.MagicElement.Fire), 
//                                    args.allMembersCounter.GetMember);
//                            }
//                        }
//                    }
//                }
//            }
//        }


//        public override void DeleteMe(bool local)
//        {
//            base.DeleteMe(local);
//            //smoke puff
//            if (parent != null)
//                Engine.ParticleHandler.AddParticleArea(ParticleSystemType.Smoke, emitter.Area.Min, 0.6f, 9);
//        }
//        protected override ParticleSystemType particles
//        {
//            get { return ParticleSystemType.Fire; }
//        }
//        protected new static readonly IntervalF ParticleRate = new IntervalF(0.02f, 0.04f);
//        protected override IntervalF particleRate
//        { get { return ParticleRate; } }
//        override protected ConditionType conditionType
//        { get { return ConditionType.Burning; } }
//        protected override bool setCharacterCondition
//        {
//            get { return true; }
//        }
//    }
//}
