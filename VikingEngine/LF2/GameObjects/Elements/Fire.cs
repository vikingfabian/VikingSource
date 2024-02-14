using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Elements
{
    class Fire : AbsNoImageObj
    {
        const float LifeTime = 2000;
        const float BurnMarkTime = LifeTime * 0.8f;
        bool leftBurntMark = false;

        public Fire(Vector3 pos)
            :base()
        {
            
            position = pos;
            Health = LifeTime;
            CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(position, 1, 1);//.QuickBoundingBox(2);
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (CollisionBound != null)
            {
                



                if (Health > 0)
                {
                    characterCollCheck(args.allMembersCounter);
                    Health -= args.time;
                    ////make a small puff of smoke
                    //DeleteMe();

                    //for (int i = 0; i < 8; i++)
                    //{
                    //    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke,
                    //           new Graphics.ParticleInitData(lib.RandomV3(position, 1), Vector3.Zero));
                    //}
                }
            }
        }

        static readonly Vector3 FireUpSpeed = new Vector3(0, 4f, 0);
        public override void Time_Update(UpdateArgs args)
        {
            if (Health <= 0)
            {
                //make a small puff of smoke
                DeleteMe();

                for (int i = 0; i < 8; i++)
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke,
                           new Graphics.ParticleInitData(lib.RandomV3(position, 1), Vector3.Zero));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire,
                       new Graphics.ParticleInitData(lib.RandomV3(position, 1), FireUpSpeed));
            }
        }
        public override void Time_LasyUpdate(ref float time)
        {
            if (!leftBurntMark && Health <= BurnMarkTime)
            {
                leftBurntMark = true;
                LfRef.chunks.MaterialDamage(position, 2, (byte)Data.MaterialType.dark_gray);
            }
        }

        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(LootfestLib.ElementFireDamage, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Fire);
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            if (character.WeaponTargetType != WeaponAttack.WeaponUserType.NON)
            {
                new Process.UnthreadedDamage(Damage, character);
            }
            return false;
        }
        protected override Vector3 worldPosOffset
        {
            get
            {
                return CollisionBound.MainBound.Offset;
            }
        }
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.NON;
            }
        }
        public override ObjectType Type
        {
            get { return ObjectType.Element; }
        }
        public override int UnderType
        {
            get { return (int)ElementType.Fire; }
        }

        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.Fire;
            }
        }
        public override Graphics.LightSourcePrio LightSourcePrio
        {
            get
            {
                return Graphics.LightSourcePrio.Low;
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return 12;
            }
        }
    }
}
