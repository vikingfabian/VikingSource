using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Sound;

namespace VikingEngine.EngineSpace.HUD.RichBox
{
    //abstract class RbSoundProfileBase
    //{
    //    public static readonly RbEmptySound Empty = new RbEmptySound();
    //    abstract public void onActionTrigger(bool enabled);
    //}

    class RbSoundProfile//: RbSoundProfileBase
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

    //class RbEmptySound: RbSoundProfileBase
    //{
    //    public override void onActionTrigger(bool enabled)
    //    {//Do nothing
    //    }
    //}
}
