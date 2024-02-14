using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class TrollBoss : AbsHumanoidEnemy
    {
        public const float TrollBossScale = 10f;

        TrollBossBaby baby;
        BodyShieldSymbol shieldModel;
        bool bodyShield = true;

        public TrollBoss(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
            shieldWalkDist = 10;
            createImage(VoxelModelName.troll_boss, TrollBossScale, new Graphics.AnimationsSettings(6, 1.8f, 2));
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(TrollBossScale * 0.3f, TrollBossScale * 0.45f, 0f);


            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
               VoxelModelName.bigorc_club,
               new HandWeaponAttackSettings(
                   GameObjectType.BigOrcBossAttack, 0.6f, 0.5f,
                    new Vector3(
                        HandWeaponAttackSettings.SwordBoundScaleW * 1.3f,
                        HandWeaponAttackSettings.SwordBoundScaleW * 1.6f,
                        HandWeaponAttackSettings.SwordBoundScaleL * 0.87f),
                   new Vector3(3.5f, 6, 4f),
                   500,
                   HandWeaponAttackSettings.SwordSwingStartAngle,
                   HandWeaponAttackSettings.SwordSwingEndAngle,
                   HandWeaponAttackSettings.SwordSwingPercTime,
                   new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty)
                   ),
                new Vector3(-0.3f, -0.65f, -3f),
               new Effects.BouncingBlockColors(Data.MaterialType.dark_warm_brown, Data.MaterialType.pastel_cyan_blue)
               );
            handWeapon.settings.weaponPosDiff.X += 2.4f;
            handWeapon.settings.weaponPosDiff.Z += 1.6f;

            weaponAttackFrame = 1;
            Health = 3;
            preAttackTime = 800;
            maxDistanceFromStart = 100;

            if (args.LocalMember)
            {
                createAiPhys();
                alwaysFullAttension();

                if (level != null)
                {
                    var manager = new Director.BossManager(this, level, Players.BabyLocation.EmoVsTroll);
                }
                NetworkShareObject();
            }

            baby = new TrollBossBaby(image);
            AddChildObject(baby);

            shieldModel = new BodyShieldSymbol(image);
            AddChildObject(shieldModel);
        }

        protected override void updateAnimation()
        {
            base.updateAnimation();
        }

        protected override void handleDamage(DamageData damage, bool local)
        {
            if (bodyShield)
            {
                if (damage.Special == SpecialDamage.ShieldBreakAttack)
                {
                    bodyShield = false;
                    
                    shieldModel.explode();
                }
                else
                {
                    shieldModel.view();
                    Music.SoundManager.WeaponClink(image.position);
                    return;
                }
            }
            base.handleDamage(damage, local);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            //if (subLevel != null)
            //{ 
            //    subLevel.KeepGOInsidePlayerUnlockedLevelBounds(this);
            //}
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.TrollBoss; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }
        
        public override MountType MountType
        {
            get
            {
                return GO.MountType.NumNone;
            }
        }
        protected override void lootDrop()
        {
            //base.lootDrop();
        }
        public override float GivesBravery
        {
            get
            {
                return 4;
            }
        }

        const float CasualWalkSpeed = StandardCasualWalkSpeed * 1.5f;
        const float ShieldWalkSpeed = StandardShieldWalkSpeed * 1.7f;
        const float WalkingSpeed = StandardWalkingSpeed * 1.7f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float shieldWalkSpeed
        {
            get { return ShieldWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pastel_blue, 
            Data.MaterialType.darker_red_orange, 
            Data.MaterialType.darker_warm_brown);
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }

        
        public override bool canBeStunned
        {
            get
            {
                return false;
            }
        }
    }

    class TrollBossBaby : VikingEngine.LootFest.GO.AbsChildModel
    {
        static readonly Vector3 Rotation = new Vector3(MathHelper.Pi, MathHelper.PiOver2, 0);

        public TrollBossBaby(Graphics.AbsVoxelObj parentModel)
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.baby,
                2.2f, 0, false);
            posOffset = new Vector3(0, TrollBoss.TrollBossScale * 0.7f, TrollBoss.TrollBossScale * -0.32f);
        }

        public override bool ChildObject_Update(AbsCharacter parent)
        {
            bool result = base.ChildObject_Update(parent);
            model.Rotation.RotateAxis(Rotation);
            return result;
        }
    }

    class BodyShieldSymbol : VikingEngine.LootFest.GO.AbsChildModel
    {
        static readonly Effects.BouncingBlockColors DamageColors = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_cyan_blue,
             Data.MaterialType.pure_blue_violet,
             Data.MaterialType.white);

        Time viewTime = 0;
        bool bExplodeMode = false;
        float goalSize;

        public BodyShieldSymbol(Graphics.AbsVoxelObj parentModel)
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.shield_symbol,
               4f, 0, true);
            model.Visible = false;
            posOffset = new Vector3(0, TrollBoss.TrollBossScale * 0.4f, TrollBoss.TrollBossScale * 0.3f);
        }

        public void view()
        {
            viewTime.Seconds = 1f;
        }

        public void explode()
        {
            viewTime.MilliSeconds = float.MaxValue;
            goalSize = model.Size1D * 1.8f;
            bExplodeMode = true;
            model.Visible = true;
        }

        public override bool ChildObject_Update(AbsCharacter parent)
        {
            if (bExplodeMode)
            {
                model.Size1D += 20f * Ref.DeltaTimeSec;
                if (model.Size1D >= goalSize)
                {
                    Effects.EffectLib.DamageBlocks(8, model, DamageColors);
                    Music.SoundManager.PlaySound(LoadedSound.shieldcrash, model.position);
                    DeleteMe();
                    return true;
                }
            }

            if (viewTime.CountDown())
            {
                model.Visible = false;
                return false;
            }
            else
            {
                model.Visible = true;
                return base.ChildObject_Update(parent);
            }
            
        }

        public override void ChildObject_OnParentRemoval(AbsCharacter parent)
        {
            base.ChildObject_OnParentRemoval(parent);
        }
    }
}