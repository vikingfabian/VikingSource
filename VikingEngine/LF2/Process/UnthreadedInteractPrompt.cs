using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Process
{
    class UnthreadedInteractPrompt : OneTimeTrigger
    {
        GameObjects.EnvironmentObj.IInteractionObj talkingTo;
        GameObjects.Characters.Hero hero;

        public UnthreadedInteractPrompt(GameObjects.EnvironmentObj.IInteractionObj talkingTo, GameObjects.Characters.Hero hero)
            :base(false)
        {
            this.talkingTo = talkingTo;
            this.hero = hero;
            AddToUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            hero.InteractPrompt(talkingTo);
        }
    }
}
