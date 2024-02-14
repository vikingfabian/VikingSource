using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    struct Damage
    {
        public static readonly Damage Default = new Damage(1);
        public static readonly Damage Zero = new Damage(0);

        public ValueRange value;
        //int value;
        public int effectValue;

        public DamageApplyType applyType;
        public DamageElementType element;
        public DamageEffectType damageEffect;
        //public ValueRangeType range;

        public Damage(int value,
            DamageApplyType type = DamageApplyType.Direct,
            ValueRangeType range = ValueRangeType.None,
            DamageElementType element = DamageElementType.None,
            DamageEffectType damageEffect = DamageEffectType.None,
            int effectValue = 0)
        {
            this.value = new ValueRange(value, range);
            //this.value = 0;
            this.applyType = type;
            this.element = element;
            this.damageEffect = damageEffect;
            this.effectValue = effectValue;
            //this.range = range;

            //Value = value;
        }

        public static Damage BellDamage(int value)
        {
            return new Damage(value, DamageApplyType.Direct, ValueRangeType.BellValue);
        }

        public Damage NextDamage()
        {   
            int value = this.value.Next();
            var result = asDirectDamage(value);
            return result;
        }

        public Damage NextDamage(AbsUnit user)
        {
            return NextDamage(user.Player.Dice);
        }

        public Damage NextDamage(DiceRoll dice)
        {
            int value = this.value.Next(dice);
            var result = asDirectDamage(value);
            return result;
        }

        Damage asDirectDamage(int value)
        {
            var result = this;
            result.value.set(value, ValueRangeType.None);
            result.applyType = DamageApplyType.Direct;

            return result;
        }

        public void write(System.IO.BinaryWriter w)
        {
            value.write(w);//w.Write((byte)value);
        }

        public void read(System.IO.BinaryReader r)
        {
            value.read(r);//value = r.ReadByte();
        }

        public override string ToString()
        {
            return "Damage(" + value.ToString() + ")";
        }

        public string description()
        {
            string elementText;
            switch (element)
            {
                default: elementText = ""; break;
                case DamageElementType.Fire: elementText = "fire "; break;
            }

            string damage = null;
            switch (applyType)
            {
                case DamageApplyType.Attack: damage = "attack " + elementText + "damage"; break;
                case DamageApplyType.Direct: damage = "direct " + elementText + "damage"; break;
                case DamageApplyType.Pure: damage = elementText + "damage"; break;
            }

            string valueText;
            switch (value.range)
            {
                default: valueText = value.ToString(); break;
                case ValueRangeType.BellValue: valueText = value.IntervalToString(); break;
            }

            string result = valueText + " " + damage;

            if (damageEffect != DamageEffectType.None)
            {
                string effectText = ", with " + damageEffect.ToString();
                if (EffectHasValue(damageEffect))
                {
                    effectText += effectValue.ToString();
                }

                result += effectText;
            }

            return result;
        }

        public string ValueToString()
        {
            return value.ToString();
        }

        public int Value
        {
            get { return value.value; }
            set { this.value.set(value); }
        }

        public void Add(int add)
        {
            value.Add(add);
        }
        public void Add(Damage add)
        {
            value.Add(add.value);
        }

        

        static bool EffectHasValue(DamageEffectType type)
        {
            return type != DamageEffectType.Stunn;
        }

        public override bool Equals(object obj)
        {
            Damage other = (Damage)obj;

            return other.value == value &&
                other.applyType == applyType &&
                other.damageEffect == damageEffect &&
                other.effectValue == effectValue &&
                other.element == element;
        }

        //public BellValue BellValue => new BellValue(value);

        public bool IsEmpty => value.value == 0 && effectValue == 0;
    }

    enum DamageApplyType
    {
        Attack,
        Direct,
        Pure,//Unblockable, from bleed and such
    }

    enum DamageWeaponType
    {
        Blunt, //Sticks, hammer
        Cutting, //Swords
        Chopping, //Axes
        Projectile, //Arrow
        Eplosive, //Bombs
        Natural, //Claws and teeth
    }

    enum DamageElementType
    {
        None,
        Fire,
        Ice,
        Earth,
        Astral,
        Poision,
    }

    enum DamageEffectType
    {
        None,
        Stunn,
        Bleed,        
        Push,
        Pull,

    }

    
}
