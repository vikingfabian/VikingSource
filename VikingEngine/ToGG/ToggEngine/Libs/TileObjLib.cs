using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine
{
    static class TileObjLib
    {
        public static AbsTileObject CreateObject(TileObjectType type,
            IntVector2 pos, object args, bool fromLoading)
        {
            AbsTileObject result;

            switch (type)
            {
                case TileObjectType.TacticalBanner:
                    result = new Commander.GO.Banner(pos);
                    break;

                case TileObjectType.EscapePoint:
                    result = new BoardEscapePoint(pos);
                    break;

                case TileObjectType.SquareTag:
                    result = new ToGG.ToggEngine.Map.SquareTag(pos, args);
                    break;

                case TileObjectType.EventFlag:
                    result = new ToGG.ToggEngine.Map.EventFlag(pos, args);
                    break;

                case TileObjectType.SpawnPoint:
                    result = new ToGG.ToggEngine.Map.SpawnPoint(pos, args);
                    break;

                case TileObjectType.RoomFlag:
                    result = new HeroQuest.MapGen.RoomFlag(pos, args);
                    break;

                case TileObjectType.Furnishing:
                    result = new ToGG.ToggEngine.Map.TileFurnishCollection(pos, args);
                    break;

                case TileObjectType.DamageTrap:
                    result = new HeroQuest.SpikeTrap(pos);
                    break;
                case TileObjectType.DeathTrap:
                    result = new HeroQuest.DeathTrap(pos);
                    break;
                case TileObjectType.SpringTrap:
                    result = new HeroQuest.SpringTrap(pos);
                    break;
                case TileObjectType.RougeTrap:
                    result = new HeroQuest.RougeTrap(pos);
                    break;

                case TileObjectType.NetTrap:
                    result = new HeroQuest.NetTrap(pos);
                    break;

                case TileObjectType.Door:
                    result = new Door(pos, args);
                    break;

                case TileObjectType.Lootdrop:
                    result = new Lootdrop(pos, args);
                    break;

                case TileObjectType.ItemCollection:
                    result = new TileItemCollection(pos, args);
                    break;

                case TileObjectType.Piggybank:
                    result = new HeroQuest.GO.Piggybank(pos);
                    break;
                case TileObjectType.Campfire:
                    result = new HeroQuest.GO.Campfire(pos);
                    break;

                case TileObjectType.FoodStorage:
                    result = new HeroQuest.GO.FoodStorage(pos);
                    break;
                case TileObjectType.AlarmBell:
                    result = new HeroQuest.GO.AlarmBell(pos);
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (!fromLoading)
            {
                result.onLoadComplete();
            }
            return result;
        }

        public static System.IO.BinaryWriter NetWriteEvent(AbsTileObject tileObject, TileObjEventType eventType)
        {
            var w = Ref.netSession.BeginWritingPacket(
                Network.PacketType.hqTileObjEvent, Network.PacketReliability.Reliable);
            //unit.writeIndex(w);
            writeTileObj(w, tileObject);
            w.Write((byte)eventType);

            return w;
        }

        public static void NetReadEvent(System.IO.BinaryReader r)
        {
            AbsTileObject tileObject = readTileObj(r);
            if (tileObject != null)
            {
                TileObjEventType eventType = (TileObjEventType)r.ReadByte();
                tileObject.netReadEvent(r, eventType);
            }
        }

        public static void WriteTileObjRemove(AbsTileObject obj)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqTileObjRemove, Network.PacketReliability.Reliable);
            writeTileObj(w, obj);
        }

        public static void writeTileObj(System.IO.BinaryWriter w, AbsTileObject obj)
        {
            toggRef.board.WritePosition(w, obj.position);
            w.Write((byte)obj.Type);
        }

        public static AbsTileObject readTileObj(System.IO.BinaryReader r)
        {
            var pos = toggRef.board.ReadPosition(r);
            ToggEngine.TileObjectType type = (ToggEngine.TileObjectType)r.ReadByte();

            return toggRef.board.tileGrid.Get(pos).tileObjects.GetObject(type);
        }
    }

    enum TileObjectType
    {
        NONE = 0,
        TacticalBanner = 1,
        EscapePoint = 2,
        SquareTag = 3,
        SpawnPoint = 4,
        Killmarks = 5,
        Furnishing = 6,
        DamageTrap = 7,
        Door = 8,
        ItemCollection = 9,
        NetTrap = 10,
        Lever = 11,
        Campfire = 12,
        DeathTrap = 13,
        Lootdrop = 14,
        Piggybank= 15,
        RoomFlag= 16,
        SpringTrap= 17,
        EventFlag= 18,
        RougeTrap= 19,
        FoodStorage= 20,
        AlarmBell= 21,
        MissionBanner = 22,
        NUM
    }

    enum InteractType
    {
        NONE,
        ActivateItself,
        SendActivation,
    }

    enum TileObjCategory
    {
        Trap,
    }
}
