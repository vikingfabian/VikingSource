using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class LocalPlayersSetup
    {
        public List2<PlayerSetup> setups;

        public LocalPlayersSetup()
        {
            hqRef.localPlayers = this;

            setups = new List2<PlayerSetup>();
            setups.Add(new PlayerSetup());
        }
    }

    class PlayerSetup
    {
        public List2<Data.PlayerVisualSetup> visualSetups;

        public PlayerSetup()
        {
            HqUnitType[] defaultHeroes = new HqUnitType[]
                {
                    HqUnitType.RecruitHeroSword,
                    HqUnitType.KnightHero,
                    HqUnitType.ElfHero,
                };

            const int HeroCount = 3;
            visualSetups = new List2<PlayerVisualSetup>(HeroCount);

            for (int i = 0; i < HeroCount; ++i)
            {
                visualSetups.Add(new PlayerVisualSetup(defaultHeroes[i]));
            }
        }
    }
}
