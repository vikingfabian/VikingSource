using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.WeaponAttack.Monster
{
    class FireBreath : AbsNoImageObj
    {
        List<FireBreathBlock> blocks = new List<FireBreathBlock>();
        DamageData damage;
        float lifeTime;

        public FireBreath(Vector3 startPos, Rotation1D targetDir, float timeLength)
            : base()
        {
            lifeTime = timeLength;
            position = startPos;
            rotation = targetDir;
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(1);
            damage = new DamageData(LootfestLib.FireBreathDamage, WeaponUserType.Enemy, ByteVector2.Zero, 
                Gadgets.GoodsType.NONE, Magic.MagicElement.Fire, SpecialDamage.NONE, 
                WeaponPush.Normal, targetDir, false);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            lifeTime -= args.time;
            if (lifeTime < 0)
            {
                if (blocks.Count == 0)
                {
                    DeleteMe();
                }
            }
            else
            {
                if (Ref.rnd.RandomChance(60))
                {
                    blocks.Add(new FireBreathBlock(position, rotation));
                }
            }

            for (int i  = blocks.Count -1; i >=0; --i)//FireBreathBlock block in blocks)
            {
                if (blocks[i].Update(args.time)) blocks.RemoveAt(i);
            }

            if (blocks.Count > 1)
            {
                FireBreathBlock b1 = arraylib.RandomListMemeber(blocks);
                FireBreathBlock b2 = arraylib.RandomListMemeber(blocks);

                //foreach (AbsUpdateObj obj in args.args.localMembersCounter)
                //for (int i = 0; i < args.localMembersCounter.Count; ++i)
                args.localMembersCounter.Reset();
                while (args.localMembersCounter.Next())
                {
                    collCheck(b1, args.localMembersCounter.GetMember);
                    collCheck(b2, args.localMembersCounter.GetMember);
                }
            }
        }

        void collCheck(FireBreathBlock b, AbsUpdateObj obj)
        {
            if (obj.Type == ObjectType.Character && WeaponLib.IsFoeTarget(this,obj, false) &&  
                CollisionBound.Intersect2(obj.CollisionBound) != null)
            {
                obj.TakeDamage(damage, true);
            }
        }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponUserType.Enemy;
            }
        }
        public override ObjectType Type
        {
            get { return ObjectType.WeaponAttack; }
        }
        public override int UnderType
        {
            get {  return (int)WeaponUtype.FireBreath; }
        }
    }

    class FireBreathBlock
    {
        public Vector3 Position
        {
            get { return Image.Position; }
        }
        Graphics.Mesh Image;
        Vector3 speed;
        Vector3 rotSpeed;
        Time lifeTime;
        static readonly IntervalF LifeTimeRange = new IntervalF(1, 1.2f);

        public FireBreathBlock(Vector3 startPos, Rotation1D targetDir)
        {
            const float Velocity = 0.018f;

            Image = new Graphics.Mesh(LoadedMesh.cube_repeating, startPos, new Graphics.TextureEffect(
                 Graphics.TextureEffectType.LambertFixed,
                Data.MaterialBuilder.MaterialTile(Data.MaterialType.red)), 0.6f);

            targetDir.Add(Ref.rnd.Plus_MinusF(0.3f));
            Vector2 planetDir = targetDir.Direction(Velocity);
            speed = new Vector3(planetDir.X, Ref.rnd.Plus_MinusF(Velocity * 0.2f), planetDir.Y);
            rotSpeed = lib.RandomV3(Vector3.Zero, 0.002f);
            lifeTime = new Time(LifeTimeRange.GetRandom(), TimeUnit.Seconds);
        }

        public bool Update(float time)
        {
            Image.Position += speed * time;
            Image.Rotation.RotateWorld(rotSpeed * time);
            if (Ref.rnd.RandomChance(20))
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, Image.Position);
            }
            if (lifeTime.CountDown())
            {
                Image.DeleteMe();
                return true;
            }
            return false;

        }

        
    }
}
