using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.ToggEngine.GO
{
    class TileObjectColl
    {
        public List<AbsTileObject> members = null;

        public void stompOnTile(AbsUnit unit, IntVector2 from, bool soundOnly, bool local)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    m.stompOnTile(unit, from, soundOnly, local);
                }
            }
        }

        public void Add(AbsTileObject obj)
        {
            if (members == null)
            {
                members = new List<AbsTileObject> { obj };
            }
            else
            {
                foreach (var m in members)
                {
                    if (m == obj)
                    {
                        throw new Exception("obj double add");
                    }

                    if (m.Type == obj.Type ||
                        m.IsTileFillingUnit && obj.IsTileFillingUnit)
                    {
                        m.DeleteMe();
                        break;
                    }
                }

                members.Add(obj);
            }
        }


        public void AddLootDrop(IntVector2 pos)
        {
            int count = 1;

            foreach (var dir in IntVector2.Dir8Array)
            {//PET RAT CHECK
                var adjUnit = toggRef.board.getUnit(pos + dir);
                if (adjUnit != null && adjUnit.HasProperty(UnitPropertyType.LootFinder))
                {
                    if (adjUnit.hq().PlayerHQ.Dice.next(HeroQuest.Data.LootFinder.ChestChance))
                    {
                        HeroQuest.hqRef.items.spawnChest(pos, HeroQuest.Data.LootFinder.ChestLvl);
                        count = 0;
                        break;
                    }
                    else
                    {
                        count += 1;
                    }
                }
            }

            if (!HeroQuest.hqRef.setup.conditions.EnemyLootDrop)
            {
                count = 0;
            }

            for (int i = 0; i < count; ++i)
            {
                addOne();
            }

            void addOne()
            {
                var loot = GetObject(TileObjectType.Lootdrop);
                if (loot == null)
                {
                    new HeroQuest.Gadgets.Lootdrop(pos, null);
                }
                else
                {
                    ((HeroQuest.Gadgets.Lootdrop)loot).StackOne();
                }
            }
        }

        public bool HasObjectWithOverridingMoveRestriction(AbsUnit unit, out MovementRestrictionType restrictionType)
        {
            restrictionType = MovementRestrictionType.NoRestrictions;

            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.HasOverridingMoveRestriction(unit, out restrictionType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void collectSquareAction(IntVector2 pos, bool enter, bool local, AbsUnit unit, List<QueAction.AbsSquareAction> actions)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    QueAction.AbsSquareAction action;

                    if (enter)
                    {
                        action = m.collectSquareEnterAction(pos, unit, local);
                    }
                    else
                    {
                        action = m.collectSquareLeaveAction(pos, unit, local);
                    }

                    if (action != null)
                    {
                        actions.Add(action);
                    }
                }
            }
        }

        public AbsTileObject GetObject(TileObjectType obj)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.Type == obj)
                    {
                        return m;
                    }
                }
            }

            return null;
        }

        public AbsTileObject GetObject(TileObjCategory category)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.IsCategory(category))
                    {
                        return m;
                    }
                }
            }

            return null;
        }

        public AbsTileObject GetInteractObject(HeroQuest.Unit unit)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.canInteractWithObj(unit))
                    {
                        return m;
                    }
                }
            }

            return null;
        }

        public AbsTileObject GetAttackTargetObject()
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m is HeroQuest.AbsTrap)
                    {
                        return m;
                    }
                }
            }

            return null;
        }


        public bool HasObject(ToggEngine.TileObjectType obj)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.Type == obj)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void newPosition(IntVector2 position)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    m.newPosition(position);
                }
            }
        }

        public void onLoadComplete()
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (!m.hasLoadComplete)
                    {
                        m.onLoadComplete();
                        m.hasLoadComplete = true;
                    }
                }
            }
        }

        public void OnEvent(ToGG.Data.EventType eventType)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    m.OnEvent(eventType);
                }
            }
        }

        public void clear()
        {
            if (members != null)
            {
                while (members.Count > 0)
                {
                    arraylib.Last(members).DeleteMe();
                }

                members = null;
            }
        }

        public void Write(System.IO.BinaryWriter w)
        {
            if (members == null)
            {
                w.Write((byte)0);
            }
            else
            {
                int storeCount = 0;
                foreach (var m in members)
                {
                    if (m.SaveToStorage) { ++storeCount; }
                }

                w.Write((byte)storeCount);
                foreach (var m in members)
                {
                    if (m.SaveToStorage)
                    {
                        w.Write((byte)m.Type);
                        m.Write(w);
                    }
                }
            }
        }

        public void Read(System.IO.BinaryReader r, IntVector2 square, DataStream.FileVersion version, bool isPaste)
        {
            int tileObjectsCount = r.ReadByte();
            if (tileObjectsCount > 0)
            {
                members = new List<AbsTileObject>(tileObjectsCount);
                for (int i = 0; i < tileObjectsCount; ++i)
                {
                    TileObjectType type = (TileObjectType)r.ReadByte();
                    var obj = TileObjLib.CreateObject(type, square, null, true);
                    obj.Read(r, version);
                    if (isPaste)
                    {
                        obj.onLoadComplete();
                    }
                }
            }
        }

        public void AddToUnitCard(ToGG.ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    m.AddToUnitCard(card, ref position);
                }
            }
        }

        public override string ToString()
        {
            if (arraylib.HasMembers(members))
            {
                string result = "Objects(" + members.Count.ToString() + "):";

                foreach (var m in members)
                {
                    result += ", (" + m.ToString() + ")";
                }
                return result;
            }
            else
            {
                return "No Tile Objects";
            }
        }

        public bool HasMembers => members != null && members.Count > 0;

        public bool HasTileFillingUnit()
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.IsTileFillingUnit)
                    {
                        return true;
                    }
                }
                //if (HasObject(TileObjectType.RoomFlag) == false)
                //{
                //    return true;
                //}
            }

            return false;
        }
    }
}
