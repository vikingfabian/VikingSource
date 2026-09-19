using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PCity
    {
        public static readonly PCity Empty = new PCity(EmptyPointer);
        const ushort EmptyPointer = ushort.MaxValue;
        public ushort cityIndex;

        public PCity(ushort cityIndex)
        {
            this.cityIndex = cityIndex;
        }

        public PCity(int cityIndex)
        {
            this.cityIndex = (ushort)cityIndex;
        }

        public bool HasValue()
        {
            return cityIndex != EmptyPointer;
        }
        public bool IsEmpty()
        {
            return cityIndex == EmptyPointer;
        }

        public City City()
        {
            if (cityIndex == EmptyPointer)
            {
                return null;
            }
            return DssRef.world.cities[cityIndex];
        }

        public Faction Faction()
        {
            if (cityIndex == EmptyPointer)
            {
                return null;
            }
            return DssRef.world.cities[cityIndex].pfaction.GetFaction();
        }
    }
}
