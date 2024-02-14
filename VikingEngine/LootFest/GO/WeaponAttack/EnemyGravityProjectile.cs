using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    

    
    abstract class EnemyGravityProjectile: AbsGravityProjectile
    {
        public EnemyGravityProjectile(GoArgs args,DamageData givesDamage, Vector3 startPos, Vector3 target, GO.Bounds.ObjectBound bound)
            : base(args, givesDamage, startPos, TargetRandomness(target, 4), bound, 0.04f)
        {
            WorldPos = new Map.WorldPosition(startPos);
            Music.SoundManager.PlaySound(LoadedSound.EnemyProj1, startPos);
            
        }

        override protected void hitShield(Vector3 shieldPos, Rotation1D heroDir)
        {
            for (int i = 0; i < 5; i++)
            {
                new Effects.BouncingBlock2(shieldPos, DamageColors.GetRandom(), 0.16f, heroDir);
            }
        }

        //public override WeaponUserType WeaponTargetType
        //{
        //    get
        //    {
        //        return base.WeaponTargetType;
        //    }
        //}
        
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Arrow; }
        }
        protected override float ImageScale
        {
            get { return 3f; }
        }
        protected override bool LocalTargetsCheck
        {
            get
            {
                return true;
            }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
    }
}
