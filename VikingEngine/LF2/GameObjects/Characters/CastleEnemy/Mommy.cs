using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.CastleEnemy
{

    /// <summary>
    /// Walks randomly inside the room, in straight lines
    /// </summary>
    class Mummy : AbsCastleMonster
    {
        public Mummy(Map.WorldPosition startPos, int areaLevel)
            : base(startPos, areaLevel)
        {
            rotation = new Rotation1D(MathHelper.PiOver2 * Ref.rnd.Int(4));
            turn();
            Health = LootfestLib.MummyHealth * (1 + LootfestLib.CastleMonsterHealthLvlMulti * areaLevel);
            NetworkShareObject();
        }

        public Mummy(System.IO.BinaryReader r)
            : base(r)
        {
        }
        override protected void createBound(float imageScale)
        {
            CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.24f * imageScale, 0.36f * imageScale, imageScale * 0.31f));
            TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (CollisionBound != null)
                characterCollCheck(args.localMembersCounter);
        }

        void turn()
        {
            rotation.Radians += MathHelper.PiOver2 * lib.RandomDirection();
            Velocity.Set(rotation, walkingSpeed); //= rotation.Direction(walkingSpeed);
            setImageDirFromSpeed();
            aiStateTimer.MilliSeconds = StateTimeRange.GetRandom();
        }

        static readonly IntervalF StateTimeRange = new IntervalF(800, 2000);
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                //image.Position += Map.WorldPosition.V2toV3(Speed * args.time);
                Velocity.PlaneUpdate(args.time, image);
                if (aiStateTimer.CountDown(args.time))
                {
                    turn();
                }
                else
                {
                    CheckCastleRoomBounds(this, roomSize);
                }
                image.UpdateAnimation(Velocity.PlaneLength(), args.time);
                
            }
            else
                base.Time_Update(args);
            characterCritiqalUpdate(true);
        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {//Immune to evil magic
            return damage.Magic != Magic.MagicElement.Evil && base.willReceiveDamage(damage);
        }
        public override void HandleCastleRoomCollision()
        {
            turn();
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
        }
        //protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        //{
        //    System.Diagnostics.Debug.WriteLine("speed before " + speed.ToString());
        //    base.handleDamage(damage, local);
        //    System.Diagnostics.Debug.WriteLine("speed after " + speed.ToString());

        //}
        protected override void TakeDamageAction()
        {
            //base.TakeDamageAction();
        }
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return new Graphics.AnimationsSettings(5, 0.5f); }
        }
        protected override VoxelModelName imageName
        {
            get { return VoxelModelName.mommy; }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Mummy; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Mummy; }
        }

        static readonly IntervalF ScaleRange = new IntervalF(5, 5.2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }

        const float WalkingSpeedLvl1 = 0.0052f;
        const float LvlSpeedAdd = 0.0004f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeedLvl1 * 4 + LvlSpeedAdd * areaLevel; }
        }
        protected override float runningSpeed
        {
            get { return WalkingSpeedLvl1; }
        }

        override public NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.FullExceptClientDel;
            }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.light_gray, Data.MaterialType.leather);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
             Gadgets.GoodsType.Thread, 60, Gadgets.GoodsType.Black_tooth, 85, Gadgets.GoodsType.Plastma, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
        protected override int MaxLevel
        {
            get
            {
                return LootfestLib.BossCount;
            }
        }
    }
}
