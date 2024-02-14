using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    abstract class AbsSpeechNpc : AbsNPC
    {
        public AbsSpeechNpc(GoArgs args)
            : base(args)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Defending;
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact2_SearchPlayer(false);
            }

            updateSpeechDialogue();
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LfChatBobbleIcon;
            }
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            if (start)
            {
                startSpeechDialogue(hero);         
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
    }
}
