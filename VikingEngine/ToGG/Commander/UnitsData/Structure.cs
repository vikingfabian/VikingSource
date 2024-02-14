using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class TacticalBase : CmdUnitData
    {
        public TacticalBase()
            : base("Tactical base", UnitMainType.Special, UnitUnderType.Special_TacticalBase, true, SpriteName.cmdUnitTent,
                    0, 1, 0, 0, 6, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Base(), new Leader());
        }

        public override UnitType Type => UnitType.TacticalBase;
    }

    class SupplyWagon : CmdUnitData
    {
        public SupplyWagon()
            : base("Supplies", UnitMainType.Special, UnitUnderType.Special_Supplies, true,
                SpriteName.cmdUnitSupplyWagon,
               2, 0, 0, 0, 2, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Valuable(), new Cant_retreat());
        }

        public override UnitType Type => UnitType.SupplyWagon;
    }

    class SupplyPile : CmdUnitData
    {
        public SupplyPile()
            : base("Supplies", UnitMainType.Special, UnitUnderType.Special_Supplies, true,
                SpriteName.cmdUnitSupplyWagon,
               0, 0, 0, 0, 2, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Valuable(), new Cant_retreat());
        }

        public override UnitType Type => UnitType.SupplyPile;
    }

    class BatteringRam : CmdUnitData
    {
        public BatteringRam()
            : base("Battering Ram", UnitMainType.Warmashine, UnitUnderType.Warmashine_BatteringRam, true,
                 SpriteName.cmdUnitBatteringRam,
                1, 0, 0, 0, 8, true)
        {
            modelSettings.facingRight = true;
            properties.set(new Valuable(), new Arrow_Block(), new Cant_retreat());
        }

        public override UnitType Type => UnitType.BatteringRam;
    }

}

