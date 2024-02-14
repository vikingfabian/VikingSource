using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.Characters.HumanionEnemy;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class ElfKing : ElfKnight
    {
        public ElfKing(GoArgs args, BlockMap.AbsLevel level)
            : base(args, VoxelModelName.elf_king, new IntervalF(6f, 6f), 0.4f, VoxelModelName.elf_king_shield, 3f,
                new Effects.BouncingBlockColors(
                    Data.MaterialType.gray_75,
                    Data.MaterialType.gray_85))
        {
            Health = 2;
            aggresivity = HumanoidEnemyAgressivity.Agressive_3;

            //this.subLevel =  LfRef.levels.GetSubLevelUnsafe(args.startWp);

            if (level != null)
            {
                var manager = new Director.BossManager(this, level, Players.BabyLocation.Elf);
            }

            if (localMember)
            {
                const int GuardsCount = 6;

                Rotation1D dir = Rotation1D.D0;
                
                for (int i = 0; i < GuardsCount; ++i)
                {
                    GoArgs guardArgs = new GoArgs(args.startPos + VectorExt.V2toV3XZ(dir.Direction(8)), 0);
                    new ElfKnight(guardArgs);
                    dir.Add(MathHelper.TwoPi / GuardsCount);
                }
            }
        }

        public override float GivesBravery
        {
            get
            {
                return 3;
            }
        }

        
        public override GameObjectType Type
        {
            get { return GameObjectType.ElfKing; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }
        
    }
}
