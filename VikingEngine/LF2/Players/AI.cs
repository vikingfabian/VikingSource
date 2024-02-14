using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Players
{
    class AI : AbsUpdateable
    {
        GameObjects.Characters.Hero hero;
        Rotation1D moveDir = Rotation1D.Random();

        public AI(GameObjects.Characters.Hero hero)
            : base(true)
        {
            this.hero = hero;
            hero.AI = this;
        }

        public override void Time_Update(float time)
        {
            if (Ref.rnd.RandomChance(5))
            {
                moveDir.Radians += Ref.rnd.Plus_MinusF(1f);
            }
            //if (Ref.rnd.RandomChance(1))
            //{
            //    hero.Attack(true, true);
            //}
            //else
            //{
            //    hero.Attack(true, false);
            //}

            //hero.Move(moveDir.Direction(1));
        }
        public void HandleCollision()
        {
            moveDir.Radians += MathHelper.Pi + Ref.rnd.Plus_MinusF(1);
        }
        public void TakeDamage(float health)
        {
            //hero.Attack(true, true);
            if (health <= 2)
            {
                hero.addHealth(8);
            }
        }
    }
}
