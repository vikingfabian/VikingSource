using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class FoodCarry : AbsItem
    {
        public FoodCarry()
            : base(ItemMainType.Carry, 0)
        {
 
        }

        public override SpriteName Icon => SpriteName.cmdCarryFood;

        public override string Name => "Food";

        public override string Description => TextLib.Error;

        public override EquipSlots Equip => EquipSlots.None;

        public override bool CarryOnly => true;
    }

    class CarryProperty : AbsUnitProperty
    {
        public AbsItem item;
        CarryModel model = null;

        public CarryProperty(AbsItem item)
        {
            this.item = item;

        }

        public override void OnApplied(AbsUnit unit)
        {
            base.OnApplied(unit);
            model = new CarryModel(unit, item);
        }

        public override void OnRemoved(AbsUnit unit)
        {
            base.OnRemoved(unit);
            model?.DeleteMe();
        }

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.UnitDeath)
            {
                model?.DeleteMe();
                //if (local && !item.CarryOnly)
                //{

                //}
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override SpriteName Icon => item.Icon;

        public override string Name => "Carry" + TextLib.Parentheses(item.Name, true);

        public override string Desc => "Is holding a mission objective item";

        public override UnitPropertyType Type => UnitPropertyType.CarryItem;
    }

    class CarryModel : Graphics.AbsUpdateableModel
    {
        AbsUnit unit;
        Vector3 offset;
        public CarryModel(AbsUnit unit, AbsItem item)
            :base(LoadedMesh.plane, Vector3.Zero, 
                 new Vector3(Display3D.UnitHoverGui.IconHeight * 1.6f),
                  Graphics.TextureEffectType.Flat, item.Icon, Color.White, true, true)
        {
            this.unit = unit;
            offset = unit.Data.modelSettings.carryIconOffset();
            Rotation = toggLib.PlaneTowardsCam;
        }

        public override void Time_Update(float time_ms)
        {
            this.Position = unit.soldierModel.model.position + offset;
        }
    }
}
