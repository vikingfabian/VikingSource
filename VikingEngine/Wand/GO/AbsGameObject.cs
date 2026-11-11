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

        virtual public void TakeDamage(int damage)
        {
            health.add(-damage);
            new DamageDisplay(this, damage);

            if (health.IsZero)
            {
                DeleteMe();
            }
        }

        virtual public void DeleteMe() { model.DeleteMe(); }
    }
}

