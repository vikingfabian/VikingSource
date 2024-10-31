using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.Sound;

namespace VikingEngine.EngineSpace.HUD.RichBox
{

    class RbSoundProfile
    {
        SoundContainerBase yes;
        SoundContainerBase no;

        public RbSoundProfile(SoundContainerBase yes, SoundContainerBase no = null)
        { 
            this.yes = yes;
            this.no = no;
        }

        public void onActionTrigger(bool enabled)
        {
            if (enabled)
            {
                yes.Play();
            }
            else
            {
                no?.Play();
            }
        }
    }


    class RbSoundAction : AbsRbAction
    {
        public RbSoundAction(RbSoundProfile sound)
        {
            this.sound = sound;
        }

        public override void actionTrigger()
        {
            sound?.onActionTrigger(enabled);
        }
    }
}
