using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    abstract class TerrainProperty : AbsProperty
    {
        public override PropertyMainType MainType => PropertyMainType.TerrainProperty;

        abstract public TerrainPropertyType Type { get; }
    }

    class BlocksLOS : TerrainProperty
    {
        public override string Name => "Blocks line of sight";

        public override string Desc => "Ranged units can't fire through this tile.";

        public override TerrainPropertyType Type => TerrainPropertyType.BlocksLOS;
    }

    class MustStop : TerrainProperty
    {
        public override string Name => "Must Stop";

        public override string Desc => "Units must end their movement on this tile.";

        public override TerrainPropertyType Type => TerrainPropertyType.MustStop;
    }
    class Block1 : TerrainProperty
    {
        public override string Name => "Defence +1";

        public override string Desc => Commander.UnitsData.Block.Block1Desc;

        public override TerrainPropertyType Type => TerrainPropertyType.Block1;
    }
    class ReducedTo1 : TerrainProperty
    {
        public override string Name => "Attacks reduced 1";

        public override string Desc => "Close and Ranged combat will be reduced to 1 attack";

        public override TerrainPropertyType Type => TerrainPropertyType.ReducedTo1;
    }
    class ReducedTo2 : TerrainProperty
    {
        public override string Name => "Attacks reduced 2";

        public override string Desc => "Close and Ranged combat will be reduced to 2 attacks";

        public override TerrainPropertyType Type => TerrainPropertyType.ReducedTo2;
    }
    class HorseMustStop : TerrainProperty
    {
        public override string Name => "Bad horse terrain";

        public override string Desc => "Cavalry units must end their movement on this tile.";

        public override TerrainPropertyType Type => TerrainPropertyType.HorseMustStop;
    }
    class Damageing : TerrainProperty
    {
        public override string Name => "Damaging";

        public override string Desc => TextLib.PercentText(toggLib.DamagingTerrainHitChance) + 
            " chance to take one hit each time a unit move to this square";

        public override TerrainPropertyType Type => TerrainPropertyType.Damageing;
    }
    class ArrowBlock : TerrainProperty
    {
        public override string Name => "Blocks arrows";

        public override string Desc => "Can't be attacked with ranged combat.";

        public override TerrainPropertyType Type => TerrainPropertyType.ArrowBlock;
    }
    class Impassable : TerrainProperty
    {
        public override string Name => "Impassable";

        public override string Desc => "Units can't move to this square.";

        public override TerrainPropertyType Type => TerrainPropertyType.Impassable;
    }
    class SittingDuck : TerrainProperty
    {
        public override string Name => "Sitting duck";

        public override string Desc => "Each attack towards this unit will always hit.";

        public override TerrainPropertyType Type => TerrainPropertyType.SittingDuck;
    }
    class MoveBonus : TerrainProperty
    {
        public override string Name => "Move bonus";

        public override string Desc => "Units starting on this tile may move +1 square.";

        public override TerrainPropertyType Type => TerrainPropertyType.MoveBonus;
    }
    class Trample : TerrainProperty
    {
        public override string Name => "Trample";

        public override string Desc => "A unit starting their turn on this tile, will destroy the terrain.";

        public override TerrainPropertyType Type => TerrainPropertyType.Trample;
    }
    class Oversight : TerrainProperty
    {
        public override string Name => "Oversight";

        public override string Desc => "Ranged units will see across view blocking terrain, and gain +1 range.";

        public override TerrainPropertyType Type => TerrainPropertyType.Oversight;
    }
    class FlyOverObsticle : TerrainProperty
    {
        public override string Name => "Fly Over Obsticle";

        public override string Desc => "Can jump or fly over this terrain.";

        public override TerrainPropertyType Type => TerrainPropertyType.FlyOverObsticle;
    }
    class Pit : TerrainProperty
    {
        public override string Name => "Pit";

        public override string Desc => "Units can't move to this square.";

        public override TerrainPropertyType Type => TerrainPropertyType.Pit;
    }

}
