using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DeepSimStrategy.GameObject
{
    //class King : AbsSoldier
    //{
    //    public const float KingWalkSpeed = StandardWalkingSpeed * 0.8f;

    //    Effects.KingBuffRadiusEffect buffEffect = null;
    //    VikingEngine.Wars.Battle.BuffEmitter buffEmitter;

    //    public King()
    //        : base()
    //    { }

    //    protected override void initData()
    //    {
    //        //Special, explodera
    //        modelScale = StandardModelScale * 1f;
    //        boundRadius = StandardBoundRadius * 1f;

    //        walkingSpeed = KingWalkSpeed;
    //        rotationSpeed = StandardRotatingSpeed * 0.8f;
    //        targetSpotRange = StandardTargetSpotRange;
    //        attackRange = 0.1f;
    //        basehealth = 40;
    //        mainAttack = AttackType.Melee;
    //        attackDamage = 5;
    //        attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.2f;

    //        isKing = true;
    //    }

    //    public override void InitLocal(Microsoft.Xna.Framework.Vector3 startPos, Players.AbsPlayer player, SoldierGroup group)
    //    {
    //        base.InitLocal(startPos, player, group);

    //        if (player is Players.LocalPlayer)
    //        {
    //            buffEffect = new Effects.KingBuffRadiusEffect(this);
    //        }

    //        if (player.IsLocal)
    //        {
    //            buffEmitter = new Battle.BuffEmitter(this, player);
    //        }
    //    }

    //    public override void onDeath(Damage fromDamage)
    //    {
    //        base.onDeath(fromDamage);

    //        if (!player.IsLocal)
    //        {
    //            warsLib.SetAchievement(AchievementIndex.OnlineKingKill);
    //        }
    //    }

    //    protected override LootFest.VoxelModelName modelName()
    //    {
    //        if (player.faction == Faction.Human)
    //            return LootFest.VoxelModelName.little_kingman;
    //        else
    //            return LootFest.VoxelModelName.little_kingorc;
    //    }

    //    public override void update()
    //    {
    //        base.update();
    //        if (buffEffect != null)
    //        {
    //            buffEffect.Update();
    //        }
    //        if (buffEmitter != null)
    //        {
    //            buffEmitter.Update();
    //        }
    //    }
    //    public override void asynchUpdate()
    //    {
    //        base.asynchUpdate();
    //        if (buffEmitter != null)
    //        {
    //            buffEmitter.AsynchUpdate();
    //        }
    //    }

    //    public override void DeleteMe()
    //    {
    //        base.DeleteMe();
    //        if (buffEffect != null)
    //        {
    //            buffEffect.DeleteMe();
    //        }
    //    }
        
    //    public override UnitType Type
    //    {
    //        get { return UnitType.King; }
    //    }

    //    public override string description
    //    {
    //        get { return ""; }
    //    }
    //}

    //class KingsGuard : SpearMan
    //{
    //    public KingsGuard()
    //        : base()
    //    { }

    //    protected override void initData()
    //    {
    //        base.initData();
    //        walkingSpeed = King.KingWalkSpeed;
    //        basehealth = 30;
    //    }

    //    protected override LootFest.VoxelModelName modelName()
    //    {
    //        if (player.faction == Faction.Human)
    //            return LootFest.VoxelModelName.little_hirdman;
    //        else
    //            return LootFest.VoxelModelName.little_hirdorc;
    //    }

    //    public override UnitType Type
    //    {
    //        get { return UnitType.KingsGuard; }
    //    }

    //    public override float getAttackTimeModifiers()
    //    {
    //        return 1f;
    //    }

    //    public override string description
    //    {
    //        get { return ""; }
    //    }
    //}
}
