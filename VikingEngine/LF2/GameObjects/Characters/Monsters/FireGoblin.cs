using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.GameObjects.WeaponAttack;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    //knallar omkring helt random
    //lvl2 delar på sig
    //droppar bollar med eld
    class FireGoblin: AbsMonster2
    {
        static readonly IntervalF ScaleRange = new IntervalF(2f, 2.2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        float fireCoolDown = 0;

        public FireGoblin(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            setHealth(LootfestLib.FireGoblinHealth);
            NetworkShareObject();
        }
         public FireGoblin(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * 0.25f, imageScale * 0.5f);
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
         }
        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0?  VoxelModelName.fire_goblin : VoxelModelName.fire_goblin2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(7, 0.5f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            Vector3 firePos = image.position;
            firePos.Y += 2;
            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, new Graphics.ParticleInitData(lib.RandomV3(firePos, 0.5f)));
            if (fire)
            {
                fire = false;
                new WeaponAttack.FireGoblinBall(this, areaLevel);
            }
        }

        bool fire = false;
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (localMember)
            {
                fireCoolDown -= args.time;
                base.AIupdate(args);
                if (getClosestCharacter(10, args.allMembersCounter, this.WeaponTargetType) != null && Ref.rnd.RandomChance(0.6f * args.time) && fireCoolDown <= 0)
                {
                    fire = true;
                    fireCoolDown = 2000;
                }
            }
        }
        
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                float dam = LootfestLib.MonsterHighCollisionDamage;
                if (areaLevel > 0)
                {
                    dam *= LootfestLib.Level2DamageMultiply;
                }
                return new WeaponAttack.DamageData(dam, WeaponTargetType, ObjOwnerAndId, Gadgets.GoodsType.NONE, Magic.MagicElement.Fire);
            }
        }
        
        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            new WeaponAttack.FireGoblinBall(this, areaLevel);
            base.handleDamage(damage, local);
        }

        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            return immortalityTime.TimeOut && damage.Magic != Magic.MagicElement.Fire;
        }

        const float WalkingSpeed = 0.008f;
        const float RunningSpeed = 0.014f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return RunningSpeed; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.red, Data.MaterialType.orange, Data.MaterialType.yellow);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.red, Data.MaterialType.orange, Data.MaterialType.bone);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.FireGoblin; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.FireGoblin; }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
              Gadgets.GoodsType.Coal, 60, Gadgets.GoodsType.Horn, 85, Gadgets.GoodsType.Black_tooth, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.Fire;
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return 8;
            }
        }
    }
}
