using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map;


namespace VikingEngine.LootFest.GO.Characters
{
    abstract class AbsEnemy : AbsCharacter
    {
       // public static readonly Data.TempVoxelReplacementSett TempMonsterImage = new Data.TempVoxelReplacementSett(VoxelModelName.temp_block_animated, true);
        protected AbsCharacter target, storedTarget;
        virtual protected int MaxLevel {  get {  return 1; } }
        protected int bosslevel = 0;
        public int EnemyValueLevel //how much loot it should drop, points it should give, extra for being leader
        {
            get { return characterLevel + bosslevel * 2; }
        }
        const float HitImmortalityTime = 600;

        

        //public AbsEnemy(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //   // Health = float.MaxValue;
            
        //}
        public AbsEnemy(GoArgs args)
            : base(args)
        {
            if (args.LocalMember)
            {
                WorldPos = args.startWp;
            }
            this.characterLevel = Bound.Max(args.characterLevel, MaxLevel);
            LfRef.gamestate.NumEnemies++;
        }
        
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                activeCheckUpdate();
            }
            immortalityTime.CountDown();

            base.Time_Update(args);
        }

        

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            if (givesContactDamage)
            {
                checkCharacterCollision(args.localMembersCounter, true);
            }
            UpdateWorldPos();
        }

        virtual protected bool givesContactDamage
        { get { return aiState != AiState.IsStunned; } }
        virtual protected WeaponAttack.DamageData contactDamage { get { return WeaponAttack.DamageData.BasicCollDamage; } }
        
        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            //must be unthreaded
            if (givesContactDamage)
            {
                WeaponAttack.DamageData damage = contactDamage;
                damage.PushDir.Radians = AngleDirToObject(character);
                new Process.UnthreadedDamage(contactDamage, character);
                this.onHitCharacter(character);
            }
            return true;
        }


        protected GO.PlayerCharacter.AbsHero GetRandomHero()
        {
            const int MaxLoops = 4;
            const float MaxDistance = 120;

            int numLoops = 0;
            GO.PlayerCharacter.AbsHero result;
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
#if PCGAME
            Debug.CrashIfThreaded();
#endif
            base.handleDamage(damage, local);

            if (damage.Damage > LfLib.MinVisualDamage)
            {
                if (Alive)
                {
                    if (pushable && localMember && damage.Push != WeaponAttack.WeaponPush.NON && this.physics != null)
                    {
                        if (this.physics is AiPhysicsLf3)
                        {
                            ((AiPhysicsLf3)this.physics).AddPushForce(damage);
                        }
                        else
                        {
                            new PushForce(damage, this.physics);
                        }
                    }
                }
                Music.SoundManager.PlaySound(HurtSound, image.position);
            }
            //HurtSound
            TakeDamageAction();
           
            immortalityTime.MilliSeconds = HitImmortalityTime;
        }
        
        #region PRE_ATTACK_EFFECT

        virtual protected void preAttackEffect()
        {
            if (localMember)
                NetworkWriteObjectState(AiState.PreAttack);
            new Effects.EnemyAttention(new Time(PreAttackTime), image, expressionEffectPosOffset, preAttackScale, Effects.EnemyAttentionType.PreAttack);
        }
        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            if (state == AiState.PreAttack)
                preAttackEffect();
        }
        protected void preAttackEffectUnthreading()
        {
            new GameObjectEventTrigger(preAttackEffect, this);
        }
        const int StandardPreAttackTime = 500;
        virtual protected int PreAttackTime { get { return StandardPreAttackTime; } }
        
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

        static readonly RandomObjects<GameObjectType> LootDropChance = new RandomObjects<GameObjectType>(
            new ObjectCommonessPair<GameObjectType>( GameObjectType.HealUp, 7),
            new ObjectCommonessPair<GameObjectType>( GameObjectType.SpecialAmmo1, 8),
            new ObjectCommonessPair<GameObjectType>( GameObjectType.SpecialAmmoFull, 1)
        );

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            if (local)
            {
                lootDrop();
            }
            base.DeathEvent(local, damage);

            //if (IsBoss && worldLevel != null)
            //{ worldLevel.OnKilledBoss(); }
        }

        //virtual protected bool IsBoss { get { return false; } }

        virtual protected void lootDrop()
        {
            if (!(this is Critter))
            {
                switch (LootDropChance.GetRandom())
                {
                    case GameObjectType.Coin:
                        new PickUp.Coin(LootDropPos());
                        break;
                    case GameObjectType.SpecialAmmo1:
                        new PickUp.SpecialAmmoAdd1(LootDropPos());
                        break;
                    case GameObjectType.SpecialAmmoFull:
                        new PickUp.SpecialAmmoFullAdd(LootDropPos());
                        break;
                    case GameObjectType.HealUp:
                        new PickUp.HealUp(LootDropPos());
                        break;
                }
            }
        }

        public override void onObjectSwapOut(AbsUpdateObj original, AbsUpdateObj replacedWith)
        {
            if (target == original)
            {
                target = replacedWith as AbsCharacter;
            }
        }


        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);

            LfRef.gamestate.NumEnemies--;
        }
        //public override GameObjectType Type
        //{
        //    get
        //    {
        //        return base.Type;
        //    }
        //}

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

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }

        public override void onResetBoss()
        {
            this.DeleteMe();
        }
        public override bool canBeCardCaptured
        {
            get
            {
                return true;
            }
        }

        protected bool hasTarget()
        {
            if (target == null || target.Dead)
            {
                //aiState = AiState.Waiting;
                target = null;
                return false;
            }
            storedTarget = target;
            return true;
        }
    }
}
