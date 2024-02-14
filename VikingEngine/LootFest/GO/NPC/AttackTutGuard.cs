using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.NPC
{
    class AttackTutGuard : AbsSpeechNpc
    {
        public const float VeteranScale = 0.16f;

        public AttackTutGuard(GoArgs args)
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

        public override GameObjectType Type
        {
            get { return GameObjectType.AttackTutGuard; }
        }

        protected override void startSpeechDialogue(PlayerCharacter.AbsHero hero)
        {
            base.startSpeechDialogue(hero);
            speechbobble.attackTutorial();
        }

        override protected float scale
        {
            get
            {
                return VeteranScale;
            }
        }
    }

    
}
