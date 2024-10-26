using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.Sound;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class SoundLib
    {
        public static SoundContainerBase click, back, buy, wrong, 
            select_army, select_city, select_faction,
            ordermove, orderstop, message, trophy,
            woodcut, tree_falling, scythe, drop_item, pickaxe, hen, pig, pickup,
            anvil, dig, genericWork, hammer;

        public static RbSoundProfile menu, menuBack, menuBuy, menuArmyHalt;

        public static void LoadContent()
        {
            string soundDir = DssLib.ContentDir + "Sound" + DataStream.FilePath.Dir;
            click = new SoundContainerSingle(soundDir + "click", 0.7f);
            back = new SoundContainerSingle(soundDir + "back");
            buy = new SoundContainerSingle(soundDir + "buy");
            wrong = new SoundContainerSingle(soundDir + "wrong", 0.6f);

            select_army = new SoundContainerSingle(soundDir + "select_army");
            select_city = new SoundContainerSingle(soundDir + "select_city");
            select_faction = new SoundContainerSingle(soundDir + "select_faction");

            ordermove = new SoundContainerSingle(soundDir + "ordermove");
            orderstop = new SoundContainerSingle(soundDir + "orderstop");
            message = new SoundContainerSingle(soundDir + "chat_message", 0.75f);
            trophy = new SoundContainerSingle(soundDir + "trophy", 0.2f);


            woodcut = new SoundContainerSingle(soundDir + "woodcut", 0.4f, 0.2f);
            tree_falling = new SoundContainerSingle(soundDir + "tree_falling", 0.4f, 0.2f);
            scythe = new SoundContainerSingle(soundDir + "scythe", 0.7f, 0.4f);
            drop_item = new SoundContainerSingle(soundDir + "drop_item", 1f, 0.4f);
            pickaxe = new SoundContainerSingle(soundDir + "pickaxe", 0.6f, 0.2f);
            hen = new SoundContainerMultiple(new string[] { soundDir + "hen1", soundDir + "hen2" }, 0.4f, 0.4f);
            pig = new SoundContainerSingle(soundDir + "pig", 0.4f, 0.8f);
            pickup = new SoundContainerSingle(soundDir + "pickup", 0.6f, 0.4f);
            anvil = new SoundContainerSingle(soundDir + "anvil", 0.45f, 0.4f);
            dig = new SoundContainerSingle(soundDir + "dig", 0.3f, 0.4f);
            genericWork = new SoundContainerMultiple(new string[] { soundDir + "generic_work1", soundDir + "generic_work2", }, 0.3f, 0.4f);
            hammer = new SoundContainerSingle(soundDir + "hammer", 0.6f, 0.4f);

            menu = new RbSoundProfile(click, wrong);
            menuBack = new RbSoundProfile(back);
            menuBuy = new RbSoundProfile(buy, wrong);
            menuArmyHalt = new RbSoundProfile(orderstop);


            Engine.LoadContent.LoadSound(LoadedSound.out_of_ammo, soundDir + "out_of_ammo");
        }
    }
}
