using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class BigOrcBoss : AbsHumanoidEnemy
    {
        new const float Scale = 8f;



        public BigOrcBoss(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
          //  this.subLevel = subLevel;

            shieldWalkDist = 10;
            createImage(VoxelModelName.bigorc, Scale, new Graphics.AnimationsSettings(6, 1.2f, 2));
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(Scale * 0.3f, Scale * 0.45f, 0f);

            

            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
               VoxelModelName.bigorc_club,
               new HandWeaponAttackSettings(
                   GameObjectType.BigOrcBossAttack, 0.6f, 0.4f,
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

            if (args.LocalMember)
            {
                createAiPhys();

                alwaysFullAttension();


                var manager = new Director.BossManager(this, level, Players.BabyLocation.FinalBoss);

                const int OrcCount = 4;

                Rotation1D dir = Rotation1D.Random();
                for (int i = 0; i < OrcCount; ++i)
                {
                    Vector3 pos = image.position + VectorExt.V2toV3XZ(dir.Direction(6));
                    var orc = new OrcSoldier(new GoArgs(pos, 0));
                    orc.alwaysFullAttension();
                    dir.Add(MathHelper.TwoPi / OrcCount);

                    manager.addBossObject(orc, true);
                }

                NetworkShareObject();
            }
            
        }

        //public BigOrcBoss(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}

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
            get { return GameObjectType.BigOrcBoss; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.BigOrc; }
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

        const float CasualWalkSpeed = StandardCasualWalkSpeed * 1.5f;
        const float ShieldWalkSpeed = StandardShieldWalkSpeed * 1.5f;
        const float WalkingSpeed = StandardWalkingSpeed * 1.5f;

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
            Data.MaterialType.light_blue,
            Data.MaterialType.pure_red_orange,
            Data.MaterialType.light_yellow_orange);
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }

        //override protected bool IsBoss { get { return true; } }

    }
}
