using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class MainTerrainProperties
    {
        public static MainTerrainProperties Get(MainTerrainType type) { return mainTerrainTypes[(int)type]; }
        static MainTerrainProperties[] mainTerrainTypes;
        public static void Init()
        {
            mainTerrainTypes = new MainTerrainProperties[(int)MainTerrainType.NUM_NON];

            mainTerrainTypes[(int)MainTerrainType.Ground] = new MainTerrainProperties("Ground", 
                new List<TerrainProperty>());

            mainTerrainTypes[(int)MainTerrainType.Hill] = new MainTerrainProperties("Hill",
                new List<TerrainProperty> { new BlocksLOS(), new Block1() });

            mainTerrainTypes[(int)MainTerrainType.Forest] = new MainTerrainProperties("Forest",
               new List<TerrainProperty> { new BlocksLOS(), new MustStop(), new ReducedTo2() });

            mainTerrainTypes[(int)MainTerrainType.Mountain] = new MainTerrainProperties("Mountain",
                           new List<TerrainProperty> { new BlocksLOS(), new Impassable(), });

            mainTerrainTypes[(int)MainTerrainType.cmdTown] = new MainTerrainProperties("Building",
               new List<TerrainProperty> { new BlocksLOS(), new MustStop(),
                    new Block1(), new ReducedTo2() });

            mainTerrainTypes[(int)MainTerrainType.Swamp] = new MainTerrainProperties("Swamp",
                           new List<TerrainProperty> { new MustStop(), new Damageing() });

            mainTerrainTypes[(int)MainTerrainType.Water] = new MainTerrainProperties("Water",
               new List<TerrainProperty> { new MustStop(), new SittingDuck() });

            mainTerrainTypes[(int)MainTerrainType.cmdRubble] = new MainTerrainProperties("Rubble",
              new List<TerrainProperty> { new HorseMustStop(), });

            mainTerrainTypes[(int)MainTerrainType.cmdPalisad] = new MainTerrainProperties("Palisad",
               new List<TerrainProperty> { new BlocksLOS(), new MustStop(), new ReducedTo1(), new Trample() });

            mainTerrainTypes[(int)MainTerrainType.PavedRoad] = new MainTerrainProperties("Road",
               new List<TerrainProperty> { new MoveBonus() });

            mainTerrainTypes[(int)MainTerrainType.Tower] = new MainTerrainProperties("Tower",
               new List<TerrainProperty> { new BlocksLOS(), new MustStop(), new Block1(), new Oversight() });

            mainTerrainTypes[(int)MainTerrainType.Wall] = new MainTerrainProperties("Wall",
               new List<TerrainProperty> { new BlocksLOS(), new Impassable() });
            
            mainTerrainTypes[(int)MainTerrainType.Gate] = new MainTerrainProperties("Gate",
              new List<TerrainProperty> { new BlocksLOS(), new Impassable() });

            mainTerrainTypes[(int)MainTerrainType.Pit] = new MainTerrainProperties("Pit",
              new List<TerrainProperty> { new Pit(), new FlyOverObsticle() });

        }

        public string Name;
        public List<TerrainProperty> properties;//TerrainPropertyType[] properties;

        public MainTerrainProperties(string Name, List<TerrainProperty> properties)
        {
            this.Name = Name;
            this.properties = properties;
        }

        public bool HasProperty(TerrainPropertyType type)
        {
            foreach (var m in properties)
            {
                if (m.Type == type)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasProperty(TerrainPropertyType type1, TerrainPropertyType type2)
        {
            foreach (var m in properties)
            {
                if (m.Type == type1 || m.Type == type2)
                {
                    return true;
                }
            }

            return false;
        }

        public TerrainProperty GetProperty(TerrainPropertyType type)
        {
            foreach (var m in properties)
            {
                if (m.Type == type)
                {
                    return m;
                }
            }

            return null;
        }
        //public static string PropertyName(TerrainPropertyType type)
        //{
        //    switch (type)
        //    {
        //        case TerrainPropertyType.Block1:
        //            return "Defence +1";
        //        case TerrainPropertyType.BlocksLOS:
        //            return "Blocks line of sight";
        //        case TerrainPropertyType.MustStop:
        //            return "Must Stop";
        //        case TerrainPropertyType.ReducedTo1:
        //            return "Attacks reduced 1";
        //        case TerrainPropertyType.ReducedTo2:
        //            return "Attacks reduced 2";
        //        case TerrainPropertyType.ArrowBlock:
        //            return "Blocks arrows";
        //        case TerrainPropertyType.Damageing:
        //            return "Damaging";
        //        case TerrainPropertyType.HorseMustStop:
        //            return "Bad horse terrain";
        //        case TerrainPropertyType.Impassable:
        //            return "Impassable";
        //        case TerrainPropertyType.SittingDuck:
        //            return "Sitting duck";
        //        case TerrainPropertyType.MoveBonus:
        //            return "Move bonus";
        //        case TerrainPropertyType.Trample:
        //            return "Trample";
        //        case TerrainPropertyType.Oversight:
        //            return "Oversight";
        //        case TerrainPropertyType.Pit:
        //            return "Pit";
        //        case TerrainPropertyType.FlyOverObsticle:
        //            return "Fly Over Obsticle";

        //        default:
        //            throw new NotImplementedException("TerrainPropertyType name: " + type.ToString());
        //    }
        //}

        //public static string PropertyDesc(TerrainPropertyType type)
        //{
        //    switch (type)
        //    {
        //        case TerrainPropertyType.Block1:
        //            return Commander.UnitsData.Block.Block1Desc;
        //        case TerrainPropertyType.BlocksLOS:
        //            return "Ranged units can't fire through this tile.";
        //        case TerrainPropertyType.MustStop:
        //            return "Units must end their movement on this tile.";

        //        case TerrainPropertyType.ReducedTo1:
        //            return "Close and Ranged combat will be reduced to 1 attack";
        //        case TerrainPropertyType.ReducedTo2:
        //            return "Close and Ranged combat will be reduced to 2 attacks";

        //        case TerrainPropertyType.ArrowBlock:
        //            return "Can't be attacked with ranged combat.";
        //        case TerrainPropertyType.Damageing:
        //            return TextLib.PercentText(toggLib.DamagingTerrainHitChance) + " chance to take one hit each time a unit move to this square";
        //        case TerrainPropertyType.HorseMustStop:
        //            return "Cavalry units must end their movement on this tile.";
        //        case TerrainPropertyType.Impassable:
        //            return "Units can't move to this square.";
        //        case TerrainPropertyType.SittingDuck:
        //            return "Each attack towards this unit will always hit.";
        //        case TerrainPropertyType.MoveBonus:
        //            return "Units starting on this tile may move +1 square.";
        //        case TerrainPropertyType.Trample:
        //            return "A unit starting their turn on this tile, will destroy the terrain.";
        //        case TerrainPropertyType.Oversight:
        //            return "Ranged units will see across view blocking terrain, and gain +1 range.";
        //        case TerrainPropertyType.Pit:
        //            return "Units can't move to this square.";
        //        case TerrainPropertyType.FlyOverObsticle:
        //            return "Can jump or fly over this terrain.";

        //        default:
        //            throw new NotImplementedException("TerrainPropertyType name: " + type.ToString());
        //    }
        //}
    }

    enum MainTerrainType
    {
        Ground,
        Forest,
        Hill,

        Mountain,
        Water,
        Swamp,
        cmdRubble,
        cmdTown,

        cmdPalisad,
        PavedRoad,
        Tower,
        Wall,
        Gate,

        Pit,


        NUM_NON,
    }
}
