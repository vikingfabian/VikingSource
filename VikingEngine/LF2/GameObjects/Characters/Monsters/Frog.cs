using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2;
using VikingEngine.Physics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    //bara hoppa omkring som idioter
    class Frog: AbsMonster2
    {
        const float ScaleToBound = 0.3f;
        static readonly IntervalF JumpTimeRange = new IntervalF(1000, 1400);
        Timer.Basic jumpTime = new Timer.Basic(JumpTimeRange.GetRandom());
        static readonly IntervalF ScaleRange = new IntervalF(1.4f,1.8f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        public Frog(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {

            setHealth(LootfestLib.FrogHealth);

            BouncingObjPhysics2 phys = (BouncingObjPhysics2)physics;
            phys.Gravity = -0.0014f;
            phys.Bounciness = 0.4f;
            NetworkShareObject();
        }
         public Frog(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             Vector3 sz = new Vector3(0.32f * imageScale, 0.4f * imageScale, imageScale * 0.5f);
             CollisionBound = new ObjSingleBound(new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, sz), Rotation1D.D0), new Vector3(0, sz.Y * 0.6f, 0)));//LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.32f * imageScale, 0.4f * imageScale, imageScale * 0.5f));
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * ScaleToBound, imageScale * 0.2f);
         }
        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0?  VoxelModelName.frog1 : VoxelModelName.frog2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(2, 0.8f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        static readonly IntervalF JumpSpeedLvl1 = new IntervalF(0.01f, 0.012f);
        static readonly IntervalF JumpSpeedLvl2 = JumpSpeedLvl1 * 1.4f;
        
        
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (physics.Sleeping)
                {//sitting still
                    image.Currentframe = 0;
                    if (jumpTime.Update(args.time))
                    {
                        Rotation1D newRotation = Rotation1D.Random();
                        jumpTime.Set(JumpTimeRange.GetRandom());
                        target = getClosestCharacter(48, args.allMembersCounter, this.WeaponTargetType);
                        if (Ref.rnd.RandomChance(50) && target != null)
                        {
                            newRotation.Radians = AngleDirToObject(target) + Ref.rnd.Plus_MinusF(0.2f);
                        }

                        float angleDiff = newRotation.AngleDifference(rotation);
                        const float MaxRot = MathHelper.PiOver2;
                        if (Math.Abs(angleDiff) > MaxRot)
                        {
                            rotation.Radians += angleDiff;
                        }
                        else
                        {
                            rotation = newRotation;
                        }
                        UpdateWorldPos();
                        WorldPosition.SetFromGroundY(1);
                        image.position.Y = WorldPosition.WorldGrindex.Y + 0.5f;
                        setImageDirFromRotation();
                        Velocity.Set(rotation,
                            (areaLevel == 0 ? JumpSpeedLvl1 : JumpSpeedLvl2).GetRandom());
                        physics.SpeedY = areaLevel == 0 ? 0.02f : 0.024f;
                        //numBounces = 0;
                        physics.WakeUp();
                    }
                }
                else
                {
                    image.Currentframe = 1;
                    physics.Update(args.time);
                }
                //immortalityTime.CountDown();
                characterCritiqalUpdate(true);
            }
            else
            {
                base.Time_Update(args);
            }
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            basicAIupdate(args);
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            if (ObjCollision == null)
            {
                Velocity *= 0.5f;
            }
            if (ObjCollision != null)
            {
                if (WeaponAttack.WeaponLib.IsFoeTarget(this, ObjCollision, false))
                {
                    //unthread this
                    handleCharacterColl(ObjCollision, new  LF2.ObjBoundCollData(collData));
                }
            }
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
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Frog; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.mossy_green, Data.MaterialType.brown, Data.MaterialType.white);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.light_red, Data.MaterialType.dark_skin, Data.MaterialType.white);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Frog; }
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.BouncingObj2;
            }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return areaLevel == 0 ? MediumCollDamageLvl1 : MediumCollDamageLvl2;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
              Gadgets.GoodsType.Meat, 40, Gadgets.GoodsType.Bladder_stone, 80, Gadgets.GoodsType.Animal_paw, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
    }
}
