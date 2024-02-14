using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Magic;
//

namespace VikingEngine.LF2.Data.Characters
{
    class MagicianData
    {
        public ElementSensitivity Weakness;
        public ElementSensitivity Immune;
        public int BossLevel;
        public MagicElement AttackMagic;

        public MagicianData(int lvl)
        {
            this.BossLevel = lvl;
            Weakness = ElementSensitivity.GetRandom();
            do{
                Immune = ElementSensitivity.GetRandom();
            }while(Weakness.Equals(Immune));

            List<MagicElement> baseMagicOptions = new List<MagicElement>
            {
                MagicElement.Fire,
                MagicElement.Lightning,
                MagicElement.Poision,
            };
            if (!Weakness.IsMateraial)
            {
                baseMagicOptions.Remove(Weakness.Magic);
            }
            AttackMagic = baseMagicOptions[Ref.rnd.Int(baseMagicOptions.Count)];
        }
        
    }

    struct ElementSensitivity
    {
        public bool IsMateraial;
        public GameObjects.Magic.MagicElement Magic;
        public GameObjects.Gadgets.GoodsType Goods;

       
        public static ElementSensitivity GetRandom()
        {
            ElementSensitivity result = new ElementSensitivity();
            result.IsMateraial = Data.RandomSeed.Instance.BytePercent(180);
            if (result.IsMateraial)
            {
                List<GameObjects.Gadgets.GoodsType> materials = new List<GameObjects.Gadgets.GoodsType>
                {
                    GameObjects.Gadgets.GoodsType.Iron, GameObjects.Gadgets.GoodsType.Bronze, GameObjects.Gadgets.GoodsType.Silver, GameObjects.Gadgets.GoodsType.Gold,
                    GameObjects.Gadgets.GoodsType.Wood,
                };
                result.Goods = materials[Data.RandomSeed.Instance.Next(materials.Count)];
            }
            else
            {
                List<GameObjects.Magic.MagicElement> magics = new List<GameObjects.Magic.MagicElement>
                {
                    GameObjects.Magic.MagicElement.Fire, GameObjects.Magic.MagicElement.Lightning, GameObjects.Magic.MagicElement.Poision,
                };
                result.Magic = magics[Data.RandomSeed.Instance.Next(magics.Count)];
            }

            return result;
        }

        public bool DamageSensitive(GameObjects.WeaponAttack.DamageData damage)
        {
            if (IsMateraial)
            {
                return damage.Material == Goods;
            }
            else
            {
                return damage.Magic == Magic;
            }
        }

        public override bool Equals(object obj)
        {
            ElementSensitivity other = (ElementSensitivity)obj;
            return IsMateraial == other.IsMateraial && Magic == other.Magic && Goods == other.Goods;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return IsMateraial? GameObjects.Gadgets.Goods.Name(Goods) : Magic.ToString();
        }
    }
}
