using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.PlayerCharacter;

namespace VikingEngine.LootFest.GO.Characters.Condition
{
    class RushAttackLock : AbsHeroCondition
    {
        Vector2 move;
        float time;
        public RushAttackLock(Hero hero, float time)
            : base(hero)
        {
            this.time = time;
            hero.LockControls = true;
            move = hero.Rotation.Direction(1.2f);
        }

        public override bool Update(UpdateArgs args)
        {
            //hero.Move(move);
            this.time -= time;

            if (this.time <= 0)
            {
                hero.LockControls = false;
                return false;
            }
            return true;
        }
    }
}
