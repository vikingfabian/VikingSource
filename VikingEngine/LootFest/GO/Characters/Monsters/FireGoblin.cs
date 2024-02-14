//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.LootFest.GO.WeaponAttack;

//namespace VikingEngine.LootFest.GO.Characters.Monsters
//{
//    //knallar omkring helt random
//    //lvl2 delar på sig
//    //droppar bollar med eld
//    class FireGoblin: AbsMonster
//    {
//        static readonly IntervalF ScaleRange = new IntervalF(2f, 2.2f);
//        protected override IntervalF scaleRange
//        {
//            get { return ScaleRange; }
//        }
//        float fireCoolDown = 0;

//        public FireGoblin(Map.WorldPosition startPos, int level)
//            : base(startPos, level)
//        {
//            setHealth(LfLib.StandardEnemyHealth);
//            NetworkShareObject();
//        }
//         public FireGoblin(System.IO.BinaryReader r)
//            : base(r)
//        {
//        }
//         override protected void createBound(float imageScale)
//         {
//             CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * 0.25f, imageScale * 0.5f);
//             TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
//         }
//        protected override VoxelModelName imageName
//        {
//            get { return characterLevel == 0?  VoxelModelName.fire_goblin : VoxelModelName.fire_goblin2; }
//        }

//        static readonly Graphics.AnimationsSettings AnimSet = 
//            new Graphics.AnimationsSettings(7, 0.5f);
//        protected override Graphics.AnimationsSettings animSettings
//        {
//            get { return AnimSet; }
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);
//            Vector3 firePos = image.Position;
//            firePos.Y += 2;
//            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(firePos, 0.5f)));
//            if (fire)
//            {
//                fire = false;
//                new WeaponAttack.FireGoblinBall(this, characterLevel);
//            }
//        }

//        bool fire = false;
//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            if (localMember)
//            {
//                fireCoolDown -= args.time;
//                base.AsynchGOUpdate(args);
//                if (getClosestCharacter(10, args.allMembersCounter, this.WeaponTargetType) != null && Ref.rnd.RandomChance(0.6f * args.time) && fireCoolDown <= 0)
//                {
//                    fire = true;
//                    fireCoolDown = 2000;
//                }
//            }
//        }
        
//        protected override WeaponAttack.DamageData contactDamage
//        {
//            get
//            {
//                return new WeaponAttack.DamageData(LfLib.EnemyAttackDamage, WeaponTargetType, ObjOwnerAndId, Magic.MagicElement.Fire);
//            }
//        }
        
//        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
//        {
//            new WeaponAttack.FireGoblinBall(this, characterLevel);
//            base.handleDamage(damage, local);
//        }

//        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
//        {
//            return immortalityTime.TimeOut && damage.Magic != Magic.MagicElement.Fire;
//        }

//        const float WalkingSpeed = 0.008f;
//        const float RunningSpeed = 0.014f;
//        protected override float walkingSpeed
//        {
//            get { return WalkingSpeed; }
//        }
//        protected override float runningSpeed
//        {
//            get { return RunningSpeed; }
//        }
//        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
//            }
//        }
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.FireGoblin; }
//        }
//        protected override Monster2Type monsterType
//        {
//            get { return Monster2Type.FireGoblin; }
//        }

       
//        public override Graphics.LightParticleType LightSourceType
//        {
//            get
//            {
//                return Graphics.LightParticleType.Fire;
//            }
//        }
//        public override float LightSourceRadius
//        {
//            get
//            {
//                return 8;
//            }
//        }
//    }
//}
