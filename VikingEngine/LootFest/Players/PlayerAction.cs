using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Players
{
    /// <summary>
    /// An action that occupies the hero, mostly attacks
    /// </summary>
    abstract class AbsPlayerAction
    {
        protected Time performAndCooldownTime, performTime;

        /// <returns>Complete, remove me</returns>
        virtual public bool Update()
        {
            //performTime.CountDown();
            return performAndCooldownTime.CountDown();
        }
        virtual public void OnPlayerActionComplete() { }
        abstract public bool BlocksSecondaryActions { get; }
        virtual public bool BlocksDamage { get { return false; } }
        virtual public bool OverridesMovement { get { return false; } }
        virtual public bool HeroAttackAnimation { get { return false; } }
    }

    class PlayerActionTimer : AbsPlayerAction
    {
        //Time time;
        bool blockSecondaryAction;
        bool heroAttackAnimation;

        public PlayerActionTimer(Time time, bool blockSecondaryAction, bool heroAttackAnimation)
        {
            this.performAndCooldownTime = time;
            this.blockSecondaryAction = blockSecondaryAction;
            this.heroAttackAnimation = heroAttackAnimation;
        }

        public override bool BlocksSecondaryActions
        {
            get { return blockSecondaryAction; }
        }
        override public bool HeroAttackAnimation { get { return heroAttackAnimation; } }
    }

    class DashAttackAction : AbsPlayerAction
    {
        //ime time;
        VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero;

        public DashAttackAction(Time performTime, Time performAndCooldownTime, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero)
        {
            this.performTime = performTime;
            this.performAndCooldownTime = performAndCooldownTime;
            this.hero = hero;
        }

        public override bool Update()
        {
            if (performTime.CountDown() == false)
            {
                hero.updateDashAttackMove();
            }
            return performAndCooldownTime.CountDown();
        }

        public override bool BlocksSecondaryActions
        {
            get { return true; }
        }
        override public bool BlocksDamage { get { return performTime.HasTime; } }
        override public bool OverridesMovement { get { return performTime.HasTime; } }
        override public bool HeroAttackAnimation { get { return true; } }
    }

    class DualAxeWhirrWind : AbsPlayerAction
    {
        //Time time;
        VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero;

        public DualAxeWhirrWind(Time time, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero)
        {
            this.performAndCooldownTime = time;
            this.hero = hero;
        }

        public override bool Update()
        {
            hero.updateDualAxeWhirrWind();
            return performAndCooldownTime.CountDown();
        }

        public override bool BlocksSecondaryActions
        {
            get { return true; }
        }
        override public bool OverridesMovement { get { return true; } }
    }
}
