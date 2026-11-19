using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Display.GameDisplay;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO
{
    abstract class AbsGameObject
    {
        public ValueBar health;
        public MapPlacement currentPos;
        public VoxelModelInstance model;

        virtual public bool TakeDamage(int damage)
        {
            if (health.HasValue)
            {
                health.add(-damage);
                new DamageDisplay(this, damage);

                if (health.IsZero)
                {
                    DeleteMe();
                }
                return true;
            }
            return false;
        }

        virtual public void Heal(int addHealth)
        {
            health.add(addHealth);
            new HealDisplay(this, addHealth);
        }

        virtual public void DeleteMe() { model.DeleteMe(); }

        abstract public GameObjectCategory ObjectCategory { get; }
    }
}

