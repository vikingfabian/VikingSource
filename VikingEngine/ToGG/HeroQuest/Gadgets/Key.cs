using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    
    class RuneKey : AbsItem
    {
        public RuneKey(RuneKeyType keyType)
            : base(ItemMainType.RuneKey, (int)keyType)
        { }

        public override void OnPickUp()
        {
            base.OnPickUp();
            hqRef.gamestate.levelProgress.AddRuneKey(type.subType);
        }

        public override SpriteName Icon
        {
            get
            {
                switch (Rune)
                {
                    case RuneKeyType.Hera:
                        return SpriteName.cmdKeyRuneH;
                    case RuneKeyType.Bast:
                        return SpriteName.cmdKeyRuneB;
                    case RuneKeyType.Froe:
                        return SpriteName.cmdKeyRuneF;
                    case RuneKeyType.Ami:
                        return SpriteName.cmdKeyRuneA;

                    default:
                        return SpriteName.MissingImage;
                }
            }
        }

        public override string Name => Rune.ToString() + " rune key";

        public override string Description => "Removes " + Rune.ToString() + " rune locks";

        RuneKeyType Rune => (RuneKeyType)type.subType;

        public override EquipSlots Equip => EquipSlots.None;
    }

    enum RuneKeyType
    {
        Hera,
        Bast,
        Froe,
        Ami,
        NUM_NON,
    }
}
