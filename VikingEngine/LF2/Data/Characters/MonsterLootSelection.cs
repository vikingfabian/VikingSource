using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.Data.Characters
{

    struct MonsterLootSelection
    {
        public static readonly Data.Characters.MonsterLootSelection NON = new Data.Characters.MonsterLootSelection(
              GoodsType.NONE, 0, GoodsType.NONE, 0, GoodsType.NONE, 0);

        public GameObjects.Gadgets.GoodsType Loot1;
        public int LootChance1;
        public GameObjects.Gadgets.GoodsType Loot2;
        public int LootChance2;
        public GameObjects.Gadgets.GoodsType Loot3;
        public int LootChance3;

        public MonsterLootSelection(
            GameObjects.Gadgets.GoodsType loot1, int chance1,
            GameObjects.Gadgets.GoodsType loot2, int chance2,
            GameObjects.Gadgets.GoodsType loot3, int chance3)
        {
            this.Loot1 = loot1; LootChance1 = chance1;
            this.Loot2 = loot2; LootChance2 = chance2 + chance1;
            this.Loot3 = loot3; LootChance3 = chance3 + chance2 + chance1;
        }

        public GameObjects.Gadgets.GoodsType GetRandom()
        {
            int rnd = Ref.rnd.Int(100);
            if (rnd < LootChance1)
            {
                return Loot1;
            }
            if (rnd < LootChance2)
            {
                return Loot2;
            }
            if (rnd < LootChance3)
            {
                return Loot3;
            }
            throw new Exception("Missing loot setup");
        }

        public bool HasLoot { get { return LootChance1 > 0; } }
    }
}
