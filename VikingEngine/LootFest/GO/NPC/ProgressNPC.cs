using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    class ProgressNPC : AbsNPC
    {
        public ProgressNPC(GoArgs args)
            : base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.war_veteran, 0f, 1f);
            postImageSetup();
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
            animSettings = StandardCharacterAnimations;
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact2_SearchPlayer(false);
            }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LfBabyProgressIcon;
            }
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            if (start)
            {
                targetHero = hero;
                SoundLib.NpcChatSound.PlayFlat();
                new Display.ProgressDisplay(hero.player);
            }
        }

        protected override float maxWalkingLength
        {
            get
            {
                return 4;
            }
        }

        protected override bool Immortal
        {
            get { return true; }
        }

        override protected float scale
        {
            get
            {
                return AttackTutGuard.VeteranScale;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.ProgressNPC; }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return NetworkShare.None;
            }
        }
    }
}
