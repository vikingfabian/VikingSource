using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.HeroQuest.GO;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class BoardMetaData
    {
        public int NoRecieverId = 0;

        public TagGroup tags = new TagGroup();
        public TagGroup mapEntrace = new TagGroup();
        public List2<IntVector2> heroStartPositions = new List2<IntVector2>(8);

        public Dictionary<int, ActionReciever> actionRecievers = new Dictionary<int, ActionReciever>();
        public List<Campfire> campfires = new List<Campfire>(4);

        public List<HeroQuest.MapGen.RoomFlag> flags = new List<HeroQuest.MapGen.RoomFlag>(16);

        public Objective foodstorage = null;
        public Objective alarmbell = null;

        public void collect_asynch()
        {
            TagGroup entrance = new TagGroup();

            ForXYLoop loop = new ForXYLoop(toggRef.board.Size);

            while (loop.Next())
            {
                BoardSquareContent square = toggRef.board.tileGrid.Get(loop.Position);

                if (square.tag.HasValue)
                {
                    switch (square.tag.tagType)
                    {
                        case SquareTagType.Tag:
                            tags.add(square.tag.tagId, loop.Position);
                            break;
                        case SquareTagType.MapEnter:
                            mapEntrace.add(square.tag.tagId, loop.Position);
                            break;
                        case SquareTagType.HeroStart:
                            entrance.add(square.tag.tagId, loop.Position);
                            break;
                    }
                }

                var objloop = square.objLoop();
                while (objloop.next())
                {
                    switch (objloop.sel.Type)
                    {
                        case TileObjectType.Door:
                            {
                                var door = objloop.sel as Door;
                                if (door.sett.lockId != NoRecieverId)
                                {
                                    ActionReciever reciever;
                                    if (actionRecievers.TryGetValue(door.sett.lockId, out reciever))
                                    {
                                        reciever.recievers.Add(objloop.sel);
                                    }
                                    else
                                    {
                                        reciever = new ActionReciever(objloop.sel);
                                        actionRecievers.Add(door.sett.lockId, reciever);
                                    }
                                }
                            }
                            break;
                        case TileObjectType.RoomFlag:
                            var flag = objloop.sel as HeroQuest.MapGen.RoomFlag;
                            flag.collectTiles();
                            flags.Add(flag);
                            break;

                        case TileObjectType.FoodStorage:
                            if (foodstorage == null)
                            { foodstorage = new Objective(); }

                            foodstorage.add(loop.Position);
                            break;

                        case TileObjectType.AlarmBell:
                            if (alarmbell == null)
                            { alarmbell = new Objective(); }

                            alarmbell.add(loop.Position);
                            break;
                    }
                }
            }//End loop


            for (int i = 0; i < 3; ++i)
            { //entrances in order of prio
                var idListed = entrance.list(i);
                if (idListed != null)
                {
                    heroStartPositions.AddRange(idListed);
                }
            }
        }

        public IntVector2 getEntrance(int index)
        {
            while (index >= heroStartPositions.Count)
            {
                IntVector2 start;

                if (heroStartPositions.Count == 0)
                {
                    start = new IntVector2(0, toggRef.board.Size.Y - 1);
                }
                else
                {
                    start = heroStartPositions[0];
                }

                int count = 0;

                for (int radius = 1; radius < 32 && count < 8; ++radius)
                {
                    ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(start, radius));
                    while (loop.Next())
                    {

                        BoardSquareContent sq;
                        if (toggRef.board.tileGrid.TryGet(loop.Position, out sq))
                        {
                            if (toggRef.board.IsSpawnAvailableSquare(loop.Position))
                            {
                                sq.tag.tagType = SquareTagType.HeroStart;
                                heroStartPositions.Add(loop.Position);
                                ++count;
                            }
                        }
                    }
                }
            }

            return heroStartPositions[index];
        }

        public int AllyUnitsStartIndex()
        {
            int index = HeroQuest.hqRef.players.HeroPlayersCount;
            if (HeroQuest.hqRef.players.PetsFillout)
            {
                index *= 2;
            }

            Bound.Min(ref index, 4);

            return index;
        }
    }

    class Objective
    {
        public List<IntVector2> interactPositions = new List<IntVector2>();

        public void add(IntVector2 pos)
        {
            foreach (var dir in IntVector2.Dir8Array)
            {
                var adj = pos + dir;
                if (toggRef.board.IsEmptyFloorSquare(pos + dir))
                {
                    arraylib.AddIfMissing(interactPositions, adj);
                }
            }
        }
    }

    class TagGroup
    {
        Dictionary<int, List<IntVector2>> tags = new Dictionary<int, List<IntVector2>>();

        public void add(int id, IntVector2 pos)
        {
            List<IntVector2> positions;

            if (tags.TryGetValue(id, out positions))
            {
                positions.Add(pos);
            }
            else
            {
                tags.Add(id, new List<IntVector2> { pos });
            }
        }

        public List<IntVector2> list(int id)
        {
            List<IntVector2> positions;

            if (tags.TryGetValue(id, out positions))
            {
                return positions;
            }
            else
            {
                return null;
            }
        }

        public List<IntVector2> listAll()
        {
            List<IntVector2> positions = new List<IntVector2>();

            foreach (var kv in tags)
            {
                positions.AddRange(kv.Value);
            }

            return positions;
        }

        public IntVector2 first(int id)
        {
            List<IntVector2> positions;

            if (tags.TryGetValue(id, out positions))
            {
                return positions[0];
            }
            else
            {
                return IntVector2.NegativeOne;
            }
        }
    }

    class TeamSpawnCollection
    {
        public Dictionary<int, List<IntVector2>> typeSpawns = new Dictionary<int, List<IntVector2>>();

        public void add(int type, IntVector2 pos)
        {
            List<IntVector2> positions;
            if (typeSpawns.TryGetValue(type, out positions))
            {
                positions.Add(pos);
            }
            else
            {
                typeSpawns.Add(type, new List<IntVector2> { pos });
            }
        }

        public IntVector2 nextSpawn(int type)
        {
            List<IntVector2> positions;
            if (typeSpawns.TryGetValue(type, out positions))
            {
                if (positions.Count > 0)
                {
                    return arraylib.RandomListMemberPop(positions);
                }
                else
                {
                    throw new Exception("Type" + type.ToString() + " spawns are too few");
                }
            }
            else
            {
                throw new Exception("No available type" + type.ToString() + " spawn");
            }
        }
    }

    class ActionReciever
    {
        public List<AbsTileObject> recievers;

        public ActionReciever(AbsTileObject obj)
        {
            recievers = new List<AbsTileObject> { obj };
        }

        public void interactEvent()
        {
            foreach (var m in recievers)
            {
                m.interactEvent(null);
            }
        }
    }
}
