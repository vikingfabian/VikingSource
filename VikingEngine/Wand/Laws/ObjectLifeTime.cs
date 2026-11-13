using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Display.GameDisplay;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Core.Wand.GO.Creature;
using VikingEngine.Core.Wand.GO.Item;
using VikingEngine.Core.Wand.GO.ObjectPointer;
using VikingEngine.Core.Wand.Map;

namespace VikingEngine.Core.Wand.Laws
{
    static class ObjectLifeTime
    {
         
        public static AbsCreature SpawnCreature(IntVector2 pos, CreatureType creatureType, bool roomSpawn)
        {
            AbsCreature result = null;
            Dir4 lookAtHero = (WandRef.world.hero.currentPos.tilePos - pos).ToDir4;
            switch (creatureType)
            {
                case CreatureType.Goblin:
                    result = new Goblin(new MapPlacement(pos, lookAtHero));
                    break;
                case CreatureType.Orc:
                    result = new Orc(new MapPlacement(pos, lookAtHero));
                    break;
                case CreatureType.BlackKnight:
                    result = new BlackKnight(new MapPlacement(pos, lookAtHero));
                    break;

            }
            result.summoningSickness = roomSpawn;
            return result;
        }

        public static AbsItem SpawnItem(IntVector2 pos, TileItemType itemType, bool isDrop)
        {
            //Find free pos
            if (isDrop)
            {
                pos = findItemDropPos(pos);
            }

            AbsItem result = null;
            switch (itemType)
            {
                case TileItemType.Book:
                    result = new Book(new MapPlacement(pos, Dir4.S));
                    break;
                case TileItemType.HealthPotion:
                    result = new HealthPotion(new MapPlacement(pos, Dir4.S));
                    break;
                case TileItemType.MagicWand:
                    result = new MagicWandItem(new MapPlacement(pos, Dir4.S));
                    break;
                case TileItemType.Altar:
                    result = new Altar(new MapPlacement(pos, Dir4.S));
                    break;

            }

            return result;
        }

        public static AbsItem DestroyItem(IntVector2 pos)
        {
            var tile = WandRef.world.map.tileGrid.Get(pos);
            var go = tile.item.Get();
            tile.item = ItemPointer.Empty;
            go.DeleteMe();
            return go;
        }

        static IntVector2 findItemDropPos(IntVector2 center)
        {
            if (!legalDropPos(center))
            {
                for (int radius = 1; radius < 24; ++radius)
                {
                    WandRef.world.map.randomPicker.start(Rectangle2.FromCenterTileAndRadius(center, radius));

                    while (WandRef.world.map.randomPicker.Next())
                    {
                        if (legalDropPos(WandRef.world.map.randomPicker.Position))
                        {
                            return WandRef.world.map.randomPicker.Position;
                        }
                    }
                }
            }

            return center;

            bool legalDropPos(IntVector2 pos)
            {
                return WandRef.world.map.tileGrid.TryGet(pos, out var tile) && tile.tileType == TileType.Floor && !tile.item.HasGameObject();
            }
        }

        public static bool CloneObject(AbsGameObject gameObject)
        {
            switch (gameObject.ObjectCategory)
            {
                case GameObjectCategory.Creature:
                    AbsCreature creature = gameObject as AbsCreature;
                    List<IntVector2> dirs = new List<IntVector2>(IntVector2.Dir8Array);
                    while (dirs.Count > 0)
                    {
                        IntVector2 toDir = arraylib.RandomListMemberPop(dirs);
                        var toPos = gameObject.currentPos.tilePos + toDir;
                        bool freePath;
                        if (toDir.IsDiagonal())
                        {
                            freePath = WandRef.world.map.CanMove_WallCheck_Diagonal(gameObject.currentPos.tilePos, toPos);
                        }
                        else
                        {
                            freePath = WandRef.world.map.CanMove_WallCheck(gameObject.currentPos.tilePos, toPos, false, out _);
                        }

                        if (freePath && !WandRef.world.map.tileGrid.Get(toPos).creature.HasGameObject())
                        {
                            var clone = SpawnCreature(toPos, creature.pointer.creatureType, false);
                            clone.CloneStatusFrom(creature);
                            return true;
                        }
                    }
                    break;
                case GameObjectCategory.Item:
                    AbsItem item = gameObject as AbsItem;
                    SpawnItem(item.currentPos.tilePos, item.pointer.itemType, true);
                    return true;
            }
            return false;
        }

        public static bool LevelUpObject(AbsGameObject gameObject, int tier)
        {
            switch (gameObject.ObjectCategory)
            {
                case GameObjectCategory.Creature:
                    AbsCreature creature = gameObject as AbsCreature;
                    CreatureType nextLevel = CreatureType.NONE;
                    switch (creature.pointer.creatureType)
                    {
                        case CreatureType.Goblin:
                            nextLevel = CreatureType.Orc;
                            break;
                        case CreatureType.Orc:
                            nextLevel = CreatureType.BlackKnight;
                            break;
                    }

                    if (nextLevel != CreatureType.NONE)
                    {
                        creature.DeleteMe();
                        var creatureUp = SpawnCreature(creature.currentPos.tilePos, nextLevel, false);
                        creatureUp.CloneStatusFrom(creature);
                        new LevelUpDisplay(creatureUp, 1);
                        return true;
                    }

                    break;

                case GameObjectCategory.Item:
                    AbsItem item = gameObject as AbsItem;
                    if (item.LevelUp(tier))
                    {
                        new LevelUpDisplay(gameObject, tier);
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
