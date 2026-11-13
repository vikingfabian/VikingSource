using System;
using System.Reflection;
using VikingEngine.Core.Wand.Display.GameDisplay;
using VikingEngine.Core.Wand.GO.ObjectPointer;
using VikingEngine.Core.Wand.Laws;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;


namespace VikingEngine.Core.Wand.GO.Item
{
    abstract class AbsItem : AbsGameObject
    {
        public int itemTier = 1;
        public ItemPointer pointer;
        public AbsItem(TileItemType itemType, MapPlacement placementData)
        {
            this.currentPos = placementData;

            pointer.itemType = itemType;
            pointer.index = WandRef.world.newItem(this);
            WandRef.world.map.tileGrid.Get(currentPos.tilePos).item = pointer;
        }

        protected void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            WandRef.world.map.tileGrid.Get(currentPos.tilePos).item = ItemPointer.Empty;
            WandRef.world.items.RemoveAt(pointer.index);
        }

        virtual public bool LevelUp(int tier)
        {
            itemTier++;
            return true;
        }

        public override GameObjectCategory ObjectCategory => GameObjectCategory.Item;
    }
}
