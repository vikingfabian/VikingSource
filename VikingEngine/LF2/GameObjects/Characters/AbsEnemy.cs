using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.Characters
{
    abstract class AbsEnemy : AbsCharacter
    {
        protected static readonly Data.TempVoxelReplacementSett TempMonsterImage = new Data.TempVoxelReplacementSett(VoxelModelName.temp_block_animated, true);
        
        virtual protected int MaxLevel {  get {  return 1; } }
        protected int bosslevel = 0;
        public int EnemyValueLevel //how much loot it should drop, points it should give, extra for being leader
        {
            get { return areaLevel + bosslevel * 2; }
        }
        const float HitImmortalityTime = 600;
        protected float armor = 0;

        public AbsEnemy(System.IO.BinaryReader r)
            : base(r)
        {
           // Health = float.MaxValue;
            
        }
        public AbsEnemy(int areaLevel)
            : base()
        {
            this.areaLevel = lib.SetMaxVal(areaLevel, MaxLevel);
            LfRef.gamestate.NumEnemies++;
        }
        
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                }
                
            }
            else
                immortalityTime.CountDown();
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            checkCharacterCollision(args.localMembersCounter); 
        }


        virtual protected WeaponAttack.DamageData contactDamage { get { return WeaponAttack.DamageData.BasicCollDamage; } }
        
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            //must be unthreaded
            WeaponAttack.DamageData damage = contactDamage;
            damage.PushDir.Radians = AngleDirToObject(character);
            new Process.UnthreadedDamage(contactDamage, character);
            this.HitCharacter(character);
            return true;
        }


        protected Hero GetRandomHero()
        {
            const int MaxLoops = 4;
            const float MaxDistance = 120;

            int numLoops = 0;
            Hero result;
            do{
                result = LfRef.LocalHeroes.GetRandom();// arraylib.RandomListMemeber(LfRef.LocalHeroes);
            } while (distanceToObject(result) > MaxDistance && numLoops++ < MaxLoops);

            return result;
        }

        virtual protected bool DoObsticleColl { get { return true; } }
        public override void Time_LasyUpdate(ref float time)
        {   
            base.Time_LasyUpdate(ref time);
        }
        

        virtual protected bool pushable { get { return true; } }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
#if WINDOWS
            Debug.DebugLib.CrashIfThreaded();
#endif

            if (armor > 0 && damage.Magic == Magic.MagicElement.NoMagic && Ref.rnd.RandomChance(90)) //10% of the times are critiqal
                damage.Damage = Bound.Min(damage.Damage * 0.8f - armor, 0) + damage.Damage * 0.2f;

            if (pushable && localMember && damage.Push != WeaponAttack.WeaponPush.NON)
                new PushForce(damage, this.physics);

            Music.SoundManager.PlaySound(HurtSound, image.position);
            //HurtSound
            TakeDamageAction();
            base.handleDamage(damage, local);
            immortalityTime.MilliSeconds = HitImmortalityTime;
        }
        
        #region PRE_ATTACK_EFFECT

        virtual protected void preAttackEffect()
        {
            if (localMember)
                NetworkWriteObjectState(AiState.PreAttack);
            new Effects.EnemyAttention(new Time(preAttackTime), image, preAttackEffectPos, preAttackScale);
        }
        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            if (state == AiState.PreAttack)
                preAttackEffect();
            //base.networkReadObjectState(state, r);
        }
        protected void preAttackEffectUnthreading()
        {
            new GameObjectEventTrigger(preAttackEffect, this);
        }
        const int StandardPreAttackTime = 500;
        virtual protected int preAttackTime { get { return StandardPreAttackTime; } }
        virtual protected Vector3 preAttackEffectPos { get { throw new NotImplementedException(); } }
        virtual protected float preAttackScale { get { return 0.25f; } }
        #endregion

        virtual protected LoadedSound HurtSound
        {
            get
            {
                return LoadedSound.MonsterHit1;
        } }

        virtual protected void TakeDamageAction()
        { }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            const float SmallBoostChance = 0.1f;
            const float HealthBoostChance = 0.6f;
            if (local && !(this is Critter) && !(this is FlyingPet))
            {
                if (Ref.rnd.RandomChance(SmallBoostChance))
                {
                    if (Ref.rnd.RandomChance(HealthBoostChance))
                        new PickUp.SmallHealthBoost(LootDropPos());
                    else
                    {
                        if (PlatformSettings.ViewUnderConstructionStuff)
                            new PickUp.SmallMagicBoost(LootDropPos());
                    }
                }
            }
            base.DeathEvent(local, damage);
        }



        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);

            LfRef.gamestate.NumEnemies--;
        }
        public override ObjectType Type
        {
            get
            {
                return base.Type;
            }
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Enemy;
            }
        }

        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.Plane1D;
            }
        }
        protected override bool ViewHealthBar
        {
            get
            {
                return true;
            }
        }
    }
}
