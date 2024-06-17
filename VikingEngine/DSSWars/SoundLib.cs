using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.Sound;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class SoundLib
    {
        public static SoundContainerBase click, back, buy, wrong, ordermove, orderstop, message, trophy;

        public static RbSoundProfile menu, menuBack, menuBuy, menuArmyHalt;

        public static void LoadContent()
        {
            string soundDir = DssLib.ContentDir + "Sound" + DataStream.FilePath.Dir;
            click = new SoundContainerSingle(soundDir + "click", 0.7f);
            back = new SoundContainerSingle(soundDir + "back");
            buy = new SoundContainerSingle(soundDir + "buy");
            wrong = new SoundContainerSingle(soundDir + "wrong", 0.6f);
            ordermove = new SoundContainerSingle(soundDir + "ordermove");
            orderstop = new SoundContainerSingle(soundDir + "orderstop");
            message = new SoundContainerSingle(soundDir + "chat_message");
            trophy = new SoundContainerSingle(soundDir + "trophy");

            menu = new RbSoundProfile(click, wrong);
            menuBack = new RbSoundProfile(back);
            menuBuy = new RbSoundProfile(buy, wrong);
            menuArmyHalt = new RbSoundProfile(orderstop);

        }
    }
}
