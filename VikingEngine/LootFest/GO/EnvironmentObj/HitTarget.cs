using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class HitTarget : AbsDestuctableEnvironment
    {
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Red, new Vector3(3, 3, 3));
        const float Scale = 4.2f;
        bool firstAreaKey;

        public HitTarget(GoArgs args, Rotation1D dir, bool firstAreaKey, VikingEngine.LootFest.BlockMap.AbsLevel level)
            :base(args)
        {
            //managedGameObject = true;
            levelCollider = new VikingEngine.LootFest.BlockMap.LevelPointer(level);
            this.firstAreaKey = firstAreaKey;
            WorldPos = args.startWp;
            SetAsManaged();

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.hit_target, Scale, 0, true);
            image.position = WorldPos.PositionV3;

            rotation = dir;
            setImageDirFromRotation();
            CollisionAndDefaultBound = new Bounds.ObjectBound(Bounds.BoundShape.Box1axisRotation, Vector3.Zero, new Vector3(0.7f, 0.7f, 0.1f) * Scale, Vector3.Zero);
                //LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.7f, 0.7f, 0.1f) * Scale, -0.2f * Scale);
            UpdateBound();
            Health = LfLib.HeroStunDamage;
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);

            WorldPos.SetAtClosestFreeY(1);
            new PickUp.Key(new GoArgs(WorldPos), firstAreaKey, this.Level, VoxelModelName.key_lvl1);
            //LfRef.levels.GetLevel(Map.WorldLevelEnum.Tutorial, null, createKey, 0);
            BlockSplatter();
        }

        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            return base.willReceiveDamage(damage);
        }

        public override void stunForce(float power, float takeDamage, bool headStomp, bool local)
        {
            //base.stunForce(power, takeDamage, headStomp, local);
            this.handleDamage(new WeaponAttack.DamageData(1f), local);
        }
        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
        }

        //void createKey(AbsWorldLevel level, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero, object args)
        //{
        //    //new PickUp.Key(new GoArgs(WorldPos), firstAreaKey, level, VoxelModelName.key_lvl1);
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.HitTarget; }
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return base.WeaponTargetType;
            }
        }
        //public override ObjLevelCollType LevelCollType
        //{
        //    get
        //    {
        //        return ;
        //    }
        //}

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_red,
            Data.MaterialType.gray_15,
            Data.MaterialType.gray_85);
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }

    //class HitLock : AbsDestuctableEnvironment
    //{
    //    //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Red, new Vector3(3, 3, 3));
    //    const float Scale = 4.2f;
    //    SubLevel subLevel;
    //    int lockId;

    //    public HitLock(Map.WorldPosition wp, Rotation1D dir, int lockId, SubLevel subLevel)
    //        : base(0)
    //    {
    //        this.lockId = lockId;
    //        this.subLevel = subLevel;
    //        WorldPos = wp;

    //        image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.hit_target, TempImage, Scale, 0, false);
    //        image.Position = WorldPos.PositionV3;

    //        rotation = dir;
    //        setImageDirFromRotation();
    //        CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.7f, 0.7f, 0.1f) * Scale, -0.2f * Scale);
    //        UpdateBound();
    //        Health = 0.1f;
    //    }

    //    protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
    //    {
    //        base.DeathEvent(local, damage);
    //        subLevel.OnOpenLock(lockId);
    //        BlockSplatter();
    //    }

    //    public override GameObjectType Type
    //    {
    //        get { return GameObjectType.HitTarget; }
    //    }

    //    static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
    //        Data.MaterialType.pure_red,
    //        Data.MaterialType.gray_15,
    //        Data.MaterialType.gray_85);
    //    public override Effects.BouncingBlockColors DamageColors
    //    { get { return DamageColorsLvl1; } }
    //}
}
