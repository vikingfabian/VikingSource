using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class SquareDic
    {
        public static readonly SquareType[] Available = new SquareType[]
        {
            SquareType.Grass,
            SquareType.GrassMud,
             SquareType.GrassObsticle,
            SquareType.GreenForest,
            SquareType.MountainGround,
            SquareType.MountainWall,
            SquareType.MountBrickGround,
            SquareType.MountBrickWall,
            SquareType.RedBrickGround,
             
            SquareType.HouseGround,
            SquareType.HouseWall,
            SquareType.Fallpit,
        };

        AbsSquare[] squares = new AbsSquare[(int)SquareType.NUM_NON];

        public SquareDic()
        {
            toggRef.sq = this;
        }

        public AbsSquare Get(SquareType type)
        {
            var result = squares[(int)type];

            if (result == null)
            {
                switch (type)
                {
                    case SquareType.Fallpit: result = new FallpitSquare(); break;
                    case SquareType.Grass: result = new GrassSquare(); break;
                    case SquareType.GreenForest: result = new GrassForestSquare(); break;
                    case SquareType.GreenHill: result = new GrassHillSquare(); break;
                    case SquareType.GreenMountain: result = new MountainSquare(); break;
                    case SquareType.GreenPalisad: result = new PalisadSquare(); break;
                    case SquareType.GreenRoad: result = new PavedRoadSquare(); break;
                    case SquareType.GreenRubble: result = new RubbleSquare(); break;
                    //case SquareType.GreenStoneGate: result = new GrassSquare(); break;
                    //case SquareType.GreenStoneWall: result = new GrassSquare(); break;
                    case SquareType.GreenSwamp: result = new SwampSquare(); break;
                    case SquareType.GreenTown: result = new TownSquare(); break;
                    case SquareType.GreenWaterPuddle: result = new WaterSquare(); break;
                    case SquareType.MountainGround: result = new MountainGroundSquare(); break;
                    case SquareType.MountainWall: result = new MountainWallSquare(); break;
                    case SquareType.MountBrickGround: result = new MountBrickGroundSquare(); break;
                    case SquareType.MountBrickWall: result = new MountBrickWallSquare(); break;
                    case SquareType.OpenWater: result = new WaterSquare(); break;
                    
                    case SquareType.RedBrickGround: result = new RedBrickGroundSquare(); break;
                    case SquareType.StoneTower: result = new TowerSquare(); break;

                    case SquareType.GrassMud: result = new GrassMudSquare(); break;
                    case SquareType.GrassObsticle: result = new GrassObsticle(); break;
                    case SquareType.HouseGround: result = new HouseGroundSquare(); break;
                    case SquareType.HouseWall: result = new HouseWallSquare(); break;

                    default: throw new NotImplementedExceptionExt("Get Square " + type.ToString(), (int)type); 
                }
                //result = new GrassSquare();
                squares[(int)type] = result;
            }
            return result;
        }
    }
}
