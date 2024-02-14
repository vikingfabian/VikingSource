using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class OrcSoldier : AbsOrc
    {
        public OrcSoldier(GoArgs args)
            : base(args, VoxelModelName.orc_soldier, OrcScaleRange)
        {
            aggresivity = HumanoidEnemyAgressivity.Agressive_3;
            hasRangedWeapon = false;
            hasHandWeapon = true;

            handWeapon = new Gadgets.HumanoidEnemyHandWeapon(
              VoxelModelName.orc_sword1,
              new HandWeaponAttackSettings(
                  GameObjectType.OrcSoldierSwordAttack, 0.8f, 0.18f, 
                  new Vector3(
                      HandWeaponAttackSettings.SwordBoundScaleW,
                      HandWeaponAttackSettings.SwordBoundScaleW,
                      8f),//bound size
                  new Vector3(4.5f, 8.4f, 4f),
                  500,
                  HandWeaponAttackSettings.SwordSwingStartAngle,
                  HandWeaponAttackSettings.SwordSwingEndAngle,
                  HandWeaponAttackSettings.SwordSwingPercTime,
                  new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty)
                  ),
              new Vector3(0, -0.7f, -1f),
              new Effects.BouncingBlockColors(Data.MaterialType.dark_red, Data.MaterialType.darker_red)
              );

            shield = new Gadgets.HumanoidEnemyShield(VoxelModelName.goblin_shield, 1.8f,
                new Vector3(-0.176f, 0.3f, 0.18f) * modelScale,  //posOffset
                this, new Effects.BouncingBlockColors(
                    Data.MaterialType.darker_yellow_orange,
                    Data.MaterialType.gray_85,
                    Data.MaterialType.gray_70));

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void onLevelDestroyed(BlockMap.AbsLevel level)
        {
            base.onLevelDestroyed(level);
        }
        
        public override GameObjectType Type
        { get { return GameObjectType.OrcSoldier; } }

        override public CardType CardCaptureType
        {
            get { return CardType.OrcSoldier; }
        }

        public override float GivesBravery
        { get { return 2; } }
    }

   
}
