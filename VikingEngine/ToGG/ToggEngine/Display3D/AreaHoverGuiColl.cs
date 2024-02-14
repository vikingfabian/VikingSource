using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Display3D
{
    class AreaHoverGuiColl
    {
        List<IDeleteable> members = new List<IDeleteable>();

        public void refresh(AbsUnit unit, UnitStatusGuiSettings sett)
        {
            clear();
            addUnit(unit, sett);
        }

        public void refresh(List<AbsUnit> units, UnitStatusGuiSettings sett)
        {
            clear();
            foreach (var m in units)
            {
                addUnit(m, sett);
            }
        }

        public void refresh(IntVector2 pos, AbsUnit holdingUnit = null)
        {
            clear();
            ForXYLoop loop = new ForXYLoop(new Rectangle2(pos, 1));

            ToggEngine.Map.BoardSquareContent sq;
            while (loop.Next())
            {
                if (toggRef.board.tileGrid.TryGet(loop.Position, out sq))
                {
                    if (sq.unit != null && sq.unit != holdingUnit)
                    {
                        addUnit(sq.unit, UnitStatusGuiSettings.MouseHover);
                    }

                    var objloop = sq.objLoop();
                    while (objloop.next())
                    {
                        var gui = objloop.sel.areaHoverGui();
                        if (gui != null)
                        {
                            members.Add(gui);
                        }
                    }
                }
            }
        }

        void addUnit(AbsUnit unit, UnitStatusGuiSettings sett)
        {
            if (unit != null && unit.Alive)
            {
                bool view = false;

                if (sett.health)
                {
                    view |= unit.hasHealth();
                }

                if (sett.stamina)
                {
                    view |= unit.hasStamina();
                }

                if (view)
                {
                    members.Add(new UnitHoverGui(unit, sett));
                }
            }
        }

        public void clear()
        {
            foreach (var m in members)
            {
                m.DeleteMe();
            }

            members.Clear();
        }
    }

    
}
