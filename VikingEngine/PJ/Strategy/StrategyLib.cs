using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Strategy
{
    static class StrategyLib
    {
        public const int SoldierCost = 2;
        public const int ShippingCost = 4;
        public const int UpgradeIncomeCost = 4;
        public const int UpgradeVpCost = 8;

        public const int StartAreaIncome = 4;

        public const int MapLayer = 0, HudLayer = 1;

        public const int WinnerVP = 30;

        public static Time nextUnitTime = new Time(1.4f, TimeUnit.Seconds);
        public static Time nextDirTime = new Time(0.7f, TimeUnit.Seconds);

        public static void SetMapLayer()
        {
            Ref.draw.CurrentRenderLayer = MapLayer;
        }
        public static void SetHudLayer()
        {
            Ref.draw.CurrentRenderLayer = HudLayer;
        }
    }

    enum AreaType
    {
        Standard,
        VictoryPoint,
        Castle,
        NUM_NON
    }

    enum AreaActionType
    {
        NoAction,
        BuySoldier,
        UpgradeIncome,
        UpgradeVp,
    }
}
