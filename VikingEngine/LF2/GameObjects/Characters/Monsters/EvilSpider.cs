using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters
{
    class EvilSpider : AbsMonster2
    {
        public EvilSpider(Vector3 startPos)
            : base(new Map.WorldPosition(startPos), 0)
        {
            aiState = AiState.Walking;
            aiStateTimer.MilliSeconds = 400 + Ref.rnd.Int(1000);
            Velocity = new VikingEngine.Velocity(Rotation1D.Random, WalkingSpeed);
            setImageDirFromSpeed();
            image.position.Y = startPos.Y + 1;
            setHealth( LootfestLib.SpiderBombHealth);
            NetworkShareObject();
        }
         public EvilSpider(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.3f * imageScale, 0.5f * imageScale, imageScale * 0.5f));
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
         }
        protected override VoxelModelName imageName
        {
            get { return VoxelModelName.evil_spider; }
        }
        
        static readonly Graphics.AnimationsSettings AnimSettings = new Graphics.AnimationsSettings(6, 0.5f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSettings; }
        }

        protected override Monster2Type monsterType
        {
            get { return Monster2Type.EvilSpider; }
        }

        bool lifeTimeOut = false;
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            basicAIupdate(args);
            if (localMember)
            {
                if (aiStateTimer.MilliSeconds <= 0 && !lifeTimeOut)
                {
                        aiStateTimer.MilliSeconds = 3000 + Ref.rnd.Int(600);
                        
                        target = getClosestCharacter(float.MaxValue, args.allMembersCounter, WeaponAttack.WeaponUserType.NON);
                        //aiState = AiState.Attacking;
                        lifeTimeOut = true;
                }
            }
        }
        static readonly IntervalF ScaleRange = new IntervalF(3f, 4f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }


        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, new Graphics.ParticleInitData(image.position));

            if (target != null)
            {
                const float RotationsSpeed = 0.005f;
                rotateTowardsObject(target, RotationsSpeed, args.time, WalkingSpeed);
                setImageDirFromRotation();

            }
            if (lifeTimeOut && aiStateTimer.MilliSeconds <= 0)
            {
                lifeTimeOut = false;
                DeleteMe();
                const float BlockScale = 0.3f;
                new Effects.BouncingBlock2(image.position, Data.MaterialType.black, BlockScale);
                new Effects.BouncingBlock2(image.position, Data.MaterialType.black, BlockScale);
                new Effects.BouncingBlock2Dummie(image.position, Data.MaterialType.black, BlockScale);
                new Effects.BouncingBlock2Dummie(image.position, Data.MaterialType.black, BlockScale);
            }
            
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(LootfestLib.SpiderBombDamage, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Evil);
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return Damage;
            }
        }
        protected override void HitCharacter(AbsUpdateObj character)
        {
            Health -= LootfestLib.SpiderBombHealth * 0.5f;
        }



        const float WalkingSpeed = 0.016f;

        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return WalkingSpeed; }
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.NON;
            }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.black, Data.MaterialType.flat_black, Data.MaterialType.red);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.EvilSpider; }
        }

        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { throw new NotImplementedException(); }
        }

        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.NUM_NON;
            }
        }
    }
}
