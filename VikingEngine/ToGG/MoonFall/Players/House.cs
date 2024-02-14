using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall.Players
{
    /// <summary>
    /// Base data for all players
    /// </summary>
    class House
    {
        public AbsPlayer player = null;
        public Faction faction;
        public int index;

        public House(int index, Faction faction)
        {
            this.index = index;
            this.faction = faction;

            foreach (var ar in moonRef.map.areas)
            {
                ar.houseNode(this).soldierGroup = new GO.SoldierGroup(ar, this);
            }
        }

        public void startArea(MapArea area)
        {
            area.houseNode(this).soldierGroup.spawnUnit(GO.UnitType.Castle);

            area.houseNode(this).soldierGroup.spawnUnit(GO.UnitType.Leader);
            area.houseNode(this).soldierGroup.spawnUnit(GO.UnitType.Soldier);
        }
    }
}
