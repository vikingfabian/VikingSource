//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Leader : AbsSoldier
//    {
//        //Effects.KingBuffRadiusEffect buffEffect = null;
//        //VikingEngine.Wars.Battle.BuffEmitter buffEmitter;

//        public Leader()
//            : base()
//        { }

//        protected override void initData()
//        {
//            //Special, explodera
//            modelScale = StandardModelScale * 1f;
//            boundRadius = StandardBoundRadius * 1f;

//            walkingSpeed = StandardWalkingSpeed;
//            rotationSpeed = StandardRotatingSpeed * 0.8f;
//            //targetSpotRange = StandardTargetSpotRange;
//            //attackRange = 0.1f;
//            //basehealth = 40;
//            //mainAttack = AttackType.NUM_NON;
//            //attackDamage = 5;
//            //attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.2f;

//            canAttackCharacters = false;
//            canAttackStructure = false;
//            canCaptureArea = false;
//            canBeAttackTarget = false;

//            isKing = true;
//        }

//        protected override bool canTargetUnit(AbsUnit unit)
//        {
//            return base.canTargetUnit(unit);
//        }

//        public override void InitLocal(Microsoft.Xna.Framework.Vector3 startPos, AbsPlayer2 player, SoldierGroup group)
//        {
//            base.InitLocal(startPos, player, group);

//            //if (player is Players.LocalPlayer)
//            //{
//            //    buffEffect = new Effects.KingBuffRadiusEffect(this);
//            //}

//            //if (player.IsLocal)
//            //{
//            //    buffEmitter = new Battle.BuffEmitter(this, player);
//            //}
//        }

//        //public override void onDeath(Damage fromDamage)
//        //{
//        //    base.onDeath(fromDamage);

//        //    if (!player.IsLocal)
//        //    {
//        //        warsLib.SetAchievement(AchievementIndex.OnlineKingKill);
//        //    }
//        //}

//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_kingman;
//            //else
//            //    return LootFest.VoxelModelName.little_kingorc;
//        }

//        public override void update()
//        {
//            base.update();
//            //if (buffEffect != null)
//            //{
//            //    buffEffect.Update();
//            //}
//            //if (buffEmitter != null)
//            //{
//            //    buffEmitter.Update();
//            //}
//        }
//        public override void asynchUpdate()
//        {
//            base.asynchUpdate();
//            //if (buffEmitter != null)
//            //{
//            //    buffEmitter.AsynchUpdate();
//            //}
//        }

//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            //if (buffEffect != null)
//            //{
//            //    buffEffect.DeleteMe();
//            //}
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.Leader; }
//        }

//        public override string description
//        {
//            get { return ""; }
//        }
//    }
//}
