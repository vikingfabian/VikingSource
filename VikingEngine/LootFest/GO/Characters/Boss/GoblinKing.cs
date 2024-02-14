using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class GoblinKing : GoblinLineman
    {
        new const float Scale = 4;

        public GoblinKing(GoArgs args, BlockMap.AbsLevel level)
            : base(args, VoxelModelName.goblin_king, Scale * 0.07f, VoxelModelName.goblin_king_shield, Scale * 0.5f, new Effects.BouncingBlockColors(
                    Data.MaterialType.dark_red,
                    Data.MaterialType.light_yellow_orange,
                    Data.MaterialType.pure_yellow_orange))
        {
            Health = 2;

            if (args.LocalMember)
            {
                aggresivity = HumanoidEnemyAgressivity.Agressive_3;

                //this.subLevel = LfRef.levels.GetSubLevelUnsafe(args.startWp);

                if (level != null)
                {
                    var manager = new Director.BossManager(this, level, Players.BabyLocation.Goblin);
                }
            }
        }

       

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        public override float GivesBravery
        {
            get
            {
                return 3;
            }
        }

        protected override float getGoblinScale()
        {
            return Scale;
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.GoblinKing; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.GoblinKing; }
        }
    }
}
