using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest
{
    class DoomData
    {
        public const int TurnsToSkull = 4;

        public int turn = 0;
        public ValueBar skulls;

        public int goldChest, silverChest, bronzeChest;
        
        public DoomData(int skullCount)
        {
            skulls = new ValueBar(0, skullCount, false);

            goldChest = skullCount - 1;
            silverChest = skullCount / 2;
            bronzeChest = 1;
        }

        public void OnHeroDeath()
        {
            skulls.plusOne();
        }

        public void OnSkullObjective()
        {
            skulls.plusOne();
        }

        public bool OnDungeonMasterTurn()
        {
            turn++;
            if (turn >= TurnsToSkull)
            {
                turn = 0;
                skulls.add(1);
                return true;
            }
            return false;
        }

        public void checkGameOver()
        {
            if (skulls.IsMax)
            {
                new QueAction.GameOver(false);
            }
        }

        public int GetChest(DoomChestLevel chestLevel)
        {
            switch (chestLevel)
            {
                default:
                    return bronzeChest;

                case DoomChestLevel.Silver:
                    return silverChest;

                case DoomChestLevel.Gold:
                    return goldChest;
            }
        }

        public bool chestIsOpen(DoomChestLevel chestLevel)
        {
            return GetChest(chestLevel) <= skulls.ValueRemoved;
        }

        public int TotalSkullCount => skulls.maxValue;
    }
}
