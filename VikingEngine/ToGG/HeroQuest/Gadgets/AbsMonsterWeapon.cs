using System;
using System.Collections.Generic;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Data;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsMonsterWeapon : AbsProperty
    {
        protected List<AbsSurgeOption> surgeOptions = null;
        protected List<AbsWeaponProperty> properties = null;
        protected int attackStrength;

        public int attackRange = 1;
        public ToggEngine.Data.AbsBoardArea attackArea = null;

        public override string Desc => (attackRange <= 1? "Melee" : "Ranged") + 
            " weapon";

        public bool IsRanged => attackRange >= 2;

        public bool IsMelee => attackRange <= 1;
    }

    class NetspitWeapon : AbsMonsterWeapon
    {
        public NetspitWeapon(int attackStrength, int range)
        {
            surgeOptions = new List<AbsSurgeOption> { new WebbSurgeOption(2) };
            properties = new List<AbsWeaponProperty> { new NetLineWeaponProperty() };

            this.attackStrength = attackStrength;
            attackRange = 1;
        }
        public override string Name => "Net spit";

    }

    class FireBreath : AbsMonsterWeapon
    {
        public FireBreath(int attackStrength, int range)
        {
            surgeOptions = new List<AbsSurgeOption> { new PierceSurgeOption(1, 1) };

            this.attackStrength = attackStrength;
            attackRange = range;

            attackArea = new ToggEngine.Data.AttackBlast();
        }
                
        public override string Name => "Fire Breath";
        
    }
}
