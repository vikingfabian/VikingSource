using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.Sound;

namespace VikingEngine.HUD.RichBox
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

        public void play(bool enabled)
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
        public RbSoundAction(RbSoundType sound)
        {
            this.sound = sound;
        }

        public override void actionTrigger()
        {
            RbSoundSetup.Get(sound)?.play(enabled);
        }
    }

    static class RbSoundSetup
    {
        public static RbSoundProfile Get(RbSoundType type)
        {
            switch (type)
            {
                case RbSoundType.Default:
                    return DSSWars.SoundLib.menu;
                default:
                    return null;
                case RbSoundType.Tab:
                    return DSSWars.SoundLib.menutab;
                case RbSoundType.Back:
                    return DSSWars.SoundLib.menuBack;
                case RbSoundType.Buy:
                    return DSSWars.SoundLib.menuBuy;
                case RbSoundType.Copy:
                    return DSSWars.SoundLib.menuCopy;
                case RbSoundType.Paste:
                    return DSSWars.SoundLib.menuPaste;
                case RbSoundType.Start:
                    return DSSWars.SoundLib.menuStart;
                case RbSoundType.Stop:
                    return DSSWars.SoundLib.menuStop;
            }
        }
    }

    enum RbSoundType
    { 
        Default,
        Tab,
        Back,
        Buy,
        Copy,
        Paste,
        Start,
        Stop,
        NUM_NONE
    }
}
