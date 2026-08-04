using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Conscript
{
    struct UnitFilter
    {
        ThirtyTwoBit value;

        public void Add(UnitFilterType type)
        {
            value.Set((int)type, true);
        }
        public void Remove(UnitFilterType type)
        {
            value.Set((int)type, false);
        }
        public bool Contains(UnitFilterType type)
        {
           return value.Get((int)type);
        }

        public bool RangedNotWarMachine()
        {
            return value.Get((int)UnitFilterType.Ranged) && !value.Get((int)UnitFilterType.WarMachine);
        }
        public bool MeleeNotWarMachine()
        {
            return value.Get((int)UnitFilterType.Melee) && !value.Get((int)UnitFilterType.WarMachine);
        }
    }

    enum UnitFilterType
    {
        Melee,
        Ranged,

        FootSoldier,
        Animal,
        AnimalCompanion,
        AnimalRider,
        WagonRider,
        WarMachine,

        Primitive,
        GunPowder,
    }
}
