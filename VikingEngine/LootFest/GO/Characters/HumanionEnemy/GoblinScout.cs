using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.WeaponAttack.Monster;

namespace VikingEngine.LootFest.GO.Characters
{
    class GoblinScout : AbsGoblin
    {
        public GoblinScout(GoArgs args)
            : base(args, VoxelModelName.goblin1)
        {
            goblinBoneSword();
            aggresivity = HumanoidEnemyAgressivity.ChickenShit_0;
            hasRangedWeapon = true;
            LfRef.modelLoad.PreLoadImage(VoxelModelName.goblin_spear, false, 0, false);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
        
        public override GameObjectType Type
        {
            get { return GameObjectType.GoblinScout; }
        }

        override public CardType CardCaptureType
        {
            get { return CardType.GoblinScout; }
        }
    }
}
