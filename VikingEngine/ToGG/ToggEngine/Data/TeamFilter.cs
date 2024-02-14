using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    struct PlayerFilter
    {
        public static readonly PlayerFilter Good = new PlayerFilter(false);
        public static readonly PlayerFilter Bad = new PlayerFilter(true);

        bool scenarioOpponent;

        public PlayerFilter(bool scenarioOpponent)
        {
            this.scenarioOpponent = scenarioOpponent;
        }

        public bool unit(AbsUnit unit)
        {
            return unit.IsScenarioOpponent() == scenarioOpponent;
        }

        public static PlayerFilter OpponentsTo(AbsUnit unit)
        {
            return new PlayerFilter(!unit.IsScenarioOpponent());
        }
    }
}
