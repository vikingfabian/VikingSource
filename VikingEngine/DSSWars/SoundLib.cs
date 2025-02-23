using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.Sound;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class SoundLib
    {
        public static readonly string SoundDir = DssLib.ContentDir + "Sound" + DataStream.FilePath.Dir;

        public static SoundContainerBase click, hover, hover_disabled, clicktab, hovertab, back, buy, wrong,
            copy, paste, start, stop,
            select_army, select_city, select_faction,
            ordermove, orderstop, message, trophy,
            woodcut, tree_falling, breaking, scythe, drop_item, pickaxe, hen, pig, pickup,
            anvil, dig, genericWork, hammer;

        public static RbSoundProfile menu, menuHover, menutab, menutabHover, menuBack, menuBuy, menuArmyHalt, menuCopy, menuPaste, menuStart, menuStop;

        public static RbSoundAction buttonHoverAction, tabHoverAction;

        public static void LoadContent()
        {
            
            click = new SoundContainerSingle(SoundDir + "click", 0.7f);
            hover = new SoundContainerMultiple(new string[] { SoundDir + "button_hover1" , SoundDir + "button_hover2" }, 0.7f);
            hover_disabled= new SoundContainerSingle(SoundDir + "hover_disabled", 0.7f);
            clicktab = new SoundContainerSingle(SoundDir + "tab_click", 0.5f);
            hovertab = new SoundContainerSingle(SoundDir + "tab_hover", 0.7f);
            back = new SoundContainerSingle(SoundDir + "back", 0.05f);
            buy = new SoundContainerSingle(SoundDir + "buy");
            wrong = new SoundContainerSingle(SoundDir + "wrong", 0.6f);

            copy = new SoundContainerSingle(SoundDir + "copy", 1f);
            paste = new SoundContainerSingle(SoundDir + "paste", 1f);
            start = new SoundContainerSingle(SoundDir + "start", 0.6f);
            stop = new SoundContainerSingle(SoundDir + "stop", 0.8f);

            select_army = new SoundContainerSingle(SoundDir + "select_army", 0.25f, 0.1f);
            select_city = new SoundContainerSingle(SoundDir + "select_city", 0.06f, 0.1f);
            select_faction = new SoundContainerSingle(SoundDir + "select_faction", 0.7f, 0.1f);

            ordermove = new SoundContainerSingle(SoundDir + "ordermove");
            orderstop = new SoundContainerSingle(SoundDir + "orderstop");
            message = new SoundContainerSingle(SoundDir + "chat_message", 0.75f);
            trophy = new SoundContainerSingle(SoundDir + "trophy", 0.2f);

            woodcut = new SoundContainerSingle(SoundDir + "woodcut", 0.4f, 0.2f);
            tree_falling = new SoundContainerSingle(SoundDir + "tree_falling", 0.4f, 0.2f);
            breaking = new SoundContainerSingle(SoundDir + "break", 0.4f, 0.2f);
            scythe = new SoundContainerSingle(SoundDir + "scythe", 0.7f, 0.4f);
            drop_item = new SoundContainerSingle(SoundDir + "drop_item", 1f, 0.4f);
            pickaxe = new SoundContainerSingle(SoundDir + "pickaxe", 0.6f, 0.2f);
            hen = new SoundContainerMultiple(new string[] { SoundDir + "hen1", SoundDir + "hen2" }, 0.4f, 0.4f);
            pig = new SoundContainerSingle(SoundDir + "pig", 0.4f, 0.8f);
            pickup = new SoundContainerSingle(SoundDir + "pickup", 0.6f, 0.4f);
            anvil = new SoundContainerSingle(SoundDir + "anvil", 0.45f, 0.4f);
            dig = new SoundContainerSingle(SoundDir + "dig", 0.3f, 0.4f);
            genericWork = new SoundContainerMultiple(new string[] { SoundDir + "generic_work1", SoundDir + "generic_work2", }, 0.3f, 0.4f);
            hammer = new SoundContainerSingle(SoundDir + "hammer", 0.6f, 0.4f);

            menu = new RbSoundProfile(click, wrong);
            menuHover = new RbSoundProfile(hover, hover_disabled);
            menutab = new RbSoundProfile(clicktab, wrong);
            menutabHover = new RbSoundProfile(hovertab, hover_disabled);
            menuBack = new RbSoundProfile(back);
            menuBuy = new RbSoundProfile(buy, wrong);
            menuArmyHalt = new RbSoundProfile(orderstop);
            menuCopy = new RbSoundProfile(copy);
            menuPaste = new RbSoundProfile(paste);
            menuStart = new RbSoundProfile(start);
            menuStop = new RbSoundProfile(stop);

            buttonHoverAction = new RbSoundAction(menuHover);
            tabHoverAction = new RbSoundAction(menutabHover);

            Engine.LoadContent.LoadSound(LoadedSound.out_of_ammo, SoundDir + "out_of_ammo");



            
            Ref.music.SetPlaylist(Music.PlayList(), PlatformSettings.PlayMusic);
        }
    }
}
