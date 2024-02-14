using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Condition
{
    abstract class AbsCharCondition : AbsNoImageObj
    {
        protected Graphics.EmitterGPU emitter;
        protected const float DamageRate = 1000;
        protected static readonly RangeV3 ParticleSpeed = new RangeV3(new Vector3(0, 0.1f, 0), new Vector3(0.1f, 0.2f, 0.1f));
        protected static readonly IntervalF ParticleRate = new IntervalF(0.002f, 0.004f);

        protected virtual IntervalF particleRate
        { get { return ParticleRate; } }

        protected float lifeTime;
        protected AbsCharacter parent;

        public AbsCharCondition(AbsCharacter parent, float lifeTime)
            : base()
        {
            this.parent = parent;
            basicInit(lifeTime);

            if (setCharacterCondition)
            {
                parent.SetCondition(conditionType, true);
            }
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            parent.ObjOwnerAndId.WriteStream(w);
        }
        public AbsCharCondition(System.IO.BinaryReader r, Director.GameObjCollection gameObjColllection, float lifeTime)
            : base(r)
        {
            parent = gameObjColllection.GetActiveOrClientObjFromIndex(r) as AbsCharacter;
            if (parent == null)
            {
                DeleteMe();
                return;
            }
            basicInit(lifeTime);
        }

        void basicInit(float lifeTime)
        {
            this.lifeTime = lifeTime;

            emitter = new Graphics.EmitterGPU(RangeV3.FromRadius(parent.Position, 1),
                ParticleSpeed, particles, particleRate);
        }

        public override void Time_Update(UpdateArgs args)
        {
            emitter.Area.Min = parent.Position;
            emitter.Area.Min.Y += 1.5f;

            lifeTime -= args.time;
            if (!parent.Alive || lifeTime <= 0)
            {
                DeleteMe();
            }
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (emitter != null)
                emitter.DeleteMe();
            
            if (localMember)
            {
                if (setCharacterCondition)
                {
                    parent.SetCondition(conditionType, false);
                }
            }
        }
        protected abstract ParticleSystemType particles { get; }

        public override ObjectType Type
        {
            get
            {
                return ObjectType.CharacterCondition;
            }
        }

        abstract protected bool setCharacterCondition { get; }

        abstract protected ConditionType conditionType
        { get; }
        public override int UnderType
        {
            get {return (int)conditionType; }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.OnlyCreation;
            }
        }
    }

    

    class Stunned : AbsCharCondition
    {
        const float StunnTime = 2000;

        public Stunned(AbsCharacter parent)
            : base(parent, StunnTime)
        {
            parent.StunnedSpeedModifier = 0.4f;
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            parent.StunnedSpeedModifier = 1;
        }
        protected override ParticleSystemType particles
        {
            get { return ParticleSystemType.Sparkle; }
        }
        override protected ConditionType conditionType
        { get { return ConditionType.Stunned; } }

        protected override bool setCharacterCondition
        {
            get { return false; }
        }
    }

    

    enum ConditionType
    {
        Stunned,
        Poisioned,
        Burning,
    }
}
