using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall.GO
{
    class SoldierGroup
    {
        Players.House house;
        public List<AbsSoldier> soldiers = new List<AbsSoldier>();
        public List<AbsBuilding> buildings = new List<AbsBuilding>();
        MapArea area;

        public SoldierGroup(MapArea area, Players.House house)
        {
            this.area = area;
            this.house = house;
        }

        public void spawnUnit(UnitType type)
        {
            Ref.draw.CurrentRenderLayer = Draw.ShadowObjLayer;
            {
                AbsUnit unit;
                switch (type)
                {
                    case UnitType.Soldier:
                        unit = new Soldier(house);
                        break;
                    case UnitType.Leader:
                        unit = new Leader(house);
                        break;
                    case UnitType.Castle:
                        unit = new Castle(house);
                        break;

                    default:
                        throw new NotImplementedException();

                }
                addUnit(unit);

            } Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        void addUnit(AbsUnit unit)
        {
            if (unit.IsBuilding)
            {
                buildings.Add((AbsBuilding)unit);
            }
            else
            {
                soldiers.Add((AbsSoldier)unit);
            }
            refreshPositions();
        }

        void refreshPositions()
        {
            Vector2 center = area.houseNode(house).center;
            float spacing = MathExt.Round(moonRef.map.soldierHeight * 0.2f);

            {
                float totW = -spacing;
                for (int i = 0; i < soldiers.Count; ++i)
                {
                    totW += soldiers[i].Size.X + spacing;
                }

                Vector2 pos = VectorExt.AddX(center, -0.5f * totW);
                for (int i = 0; i < soldiers.Count; ++i)
                {
                    soldiers[i].Position = pos;
                    pos.X += soldiers[i].Size.X + spacing;
                }
            }

            {
                float totW = -spacing;
                for (int i = 0; i < buildings.Count; ++i)
                {
                    totW += buildings[i].Size.X + spacing;
                }

                Vector2 pos = center; //, -0.5f * totW);
                pos.X -= 0.5f * totW;
                pos.Y += moonRef.map.soldierHeight;

                for (int i = 0; i < buildings.Count; ++i)
                {
                    buildings[i].Position = pos;
                    pos.X += buildings[i].Size.X + spacing;
                }
            }
        }
    }
}
