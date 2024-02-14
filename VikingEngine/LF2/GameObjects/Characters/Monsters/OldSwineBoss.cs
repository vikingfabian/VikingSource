using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    //fixa loot
    class OldSwineBoss : Hog
    {
        public OldSwineBoss(Map.WorldPosition startPos)
            : base(startPos, 0)
        {
            Health = LootfestLib.OldSwineHealth;
            new NPC.i(this);
        }
         public OldSwineBoss(System.IO.BinaryReader r)
            : base(r)
        {
        }

        static readonly IntervalF ScaleRange = new IntervalF(6f, 7f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }

        protected override VoxelModelName imageName
        {
            get { return VoxelModelName.old_swine; }
        }

        public override CharacterUtype CharacterType
        {
            get
            {
                return CharacterUtype.OldSwine;
            }
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            LfRef.gamestate.Progress.GeneralProgress = Data.GeneralProgress.TalkToFather2_HogKilled;
            base.DeathEvent(local, damage);
            LfRef.gamestate.MusicDirector.SuccessSong();
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.KillOldSwine)
            {
                LfRef.gamestate.Progress.GeneralProgress--;
            }
        }

        const float SwineBossWalkingSpeed = WalkingSpeed * 0.5f;
        const float SwineBossRunningSpeed = RunningSpeed * 0.5f;
        protected override float walkingSpeed
        {
            get { return SwineBossWalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return SwineBossRunningSpeed; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.gray, Data.MaterialType.white, Data.MaterialType.light_brown);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return LowCollDamageLvl1;
            }
        }
    }
}
