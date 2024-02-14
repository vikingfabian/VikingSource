using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class PetRat : AbsPet
    {
        public PetRat()
            : base()
        {
            move = 5;
            modelSettings.image = SpriteName.hqPetRat;
            modelSettings.image2 = SpriteName.hqPetRatBigSack;
            modelSettings.modelScale = 0.75f;
            modelSettings.facingRight = false;
            modelSettings.shadowOffset = -0.1f;


            properties.set(
                new Pet(), 
                new DeliveryService(), 
                new LootCollector(), 
                new LootFinder());
        }

        public override HqUnitType Type => HqUnitType.PetRat;
        public override string Name => "Pet Rat";
    }

    class DeliveryService : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdDeliveryService;

        public override UnitPropertyType Type => UnitPropertyType.DeliveryService;

        public override string Name => "Delivery service";

        public override string Desc => "When adjacent to a hero, all other players may send items to him.";
    }

    class LootCollector : AbsUnitProperty
    {
        Unit unit;
        ItemCollection items = new ItemCollection();
        int pickupCount = 0;

        //public LootCollector(Unit unit)
        //{
        //    this.unit = unit;
        //}

        public void add(ItemCollection tileItems)
        {
            foreach (var m in tileItems)
            {
                items.add_stackAlways(m);
                pickupCount++;
            }
            onCountChange(true);
        }

        public void add(AbsItem item)
        {
            items.add_stackAlways(item);
            pickupCount++;
            onCountChange(true);
        }

        void onCountChange(bool local)
        {
            if (local)
            {
                netWritePropertyStatus(unit);
            }

            unit.setFrame(pickupCount == 0 ? 0 : 1);
        }

        protected override void writeStatus(BinaryWriter w)
        {
            w.Write((byte)pickupCount);
        }

        protected override void readStatus(BinaryReader r)
        {
            pickupCount = r.ReadByte();
            onCountChange(false);
        }

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (unit == null)
            {
                unit = parentUnit.hq();
            }

            if (items.Count > 0)
            {
                bool moveEvent =
                    eventType == EventType.ParentUnitMoved ||
                    (eventType == EventType.OtherUnitMoved && ((Unit)tag).IsHero);

                if (moveEvent)
                {
                    var adj = parentUnit.hq().collectAdjacentHeroes(parentUnit.squarePos, false);
                    if (adj.Count > 0)
                    {
                        var toHero = arraylib.RandomListMember(adj);
                        hqRef.items.interaction_takeAllItems(toHero, items);
                        items.Clear();
                        pickupCount = 0;
                        onCountChange(true);
                    }
                }
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override SpriteName Icon => SpriteName.cmdLootCollector;

        public override UnitPropertyType Type => UnitPropertyType.LootCollector;

        public override string Name => "Loot Collector " + TextLib.Parentheses(pickupCount.ToString());

        public override string Desc => "Picks up loot. Unloads it when adjacent to a hero.";
    }

    class LootFinder : AbsUnitProperty
    {
        public static readonly Percent ChestChance = new Percent(10);
        public const LootLevel ChestLvl = LootLevel.Level1;

        public override SpriteName Icon => SpriteName.cmdLootFinder;

        public override UnitPropertyType Type => UnitPropertyType.LootFinder;

        public override string Name => "Body search";

        public override string Desc => "Adjacent enemies drops extra loot." + 
            ChestChance.ToString() + " chance to drop a " + Gadgets.TileItemCollData.Name(ChestLvl);
    }
}
