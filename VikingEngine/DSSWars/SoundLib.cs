using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.Sound;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class SoundLib
    {
        public static SoundContainerBase click, hover, hover_disabled, clicktab, hovertab, back, buy, wrong,
            copy, paste, start, stop,
            select_army, select_city, select_faction,
            ordermove, orderstop, message, trophy,
            woodcut, tree_falling, scythe, drop_item, pickaxe, hen, pig, pickup,
            anvil, dig, genericWork, hammer;

        public static RbSoundProfile menu, menuHover, menutab, menutabHover, menuBack, menuBuy, menuArmyHalt, menuCopy, menuPaste, menuStart, menuStop;

        public static RbSoundAction buttonHoverAction, tabHoverAction;

        public static void LoadContent()
        {
            string soundDir = DssLib.ContentDir + "Sound" + DataStream.FilePath.Dir;
            click = new SoundContainerSingle(soundDir + "click", 0.7f);
            hover = new SoundContainerMultiple(new string[] { soundDir + "button_hover1" , soundDir + "button_hover2" }, 0.7f);
            hover_disabled= new SoundContainerSingle(soundDir + "hover_disabled", 0.7f);
            clicktab = new SoundContainerSingle(soundDir + "tab_click", 0.5f);
            hovertab = new SoundContainerSingle(soundDir + "tab_hover", 0.7f);
            back = new SoundContainerSingle(soundDir + "back", 0.05f);
            buy = new SoundContainerSingle(soundDir + "buy");
            wrong = new SoundContainerSingle(soundDir + "wrong", 0.6f);

            copy = new SoundContainerSingle(soundDir + "copy", 1f);
            paste = new SoundContainerSingle(soundDir + "paste", 1f);
            start = new SoundContainerSingle(soundDir + "start", 0.6f);
            stop = new SoundContainerSingle(soundDir + "stop", 0.8f);

            select_army = new SoundContainerSingle(soundDir + "select_army", 0.25f, 0.1f);
            select_city = new SoundContainerSingle(soundDir + "select_city", 0.06f, 0.1f);
            select_faction = new SoundContainerSingle(soundDir + "select_faction", 0.7f, 0.1f);

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

            Engine.LoadContent.LoadSound(LoadedSound.out_of_ammo, soundDir + "out_of_ammo");



            string MusicFolder = DssLib.ContentDir + "Music\\";
            Ref.music.SetPlaylist(new List<Sound.SongData>
            {
                new Sound.SongData(MusicFolder + "BBaaB_loop", true, 0.22f),
                new Sound.SongData(MusicFolder + "Gargoyle_loop", true, 0.3f),

                new Sound.SongData(MusicFolder + "RM 10 - Incubation", false, 0.3f),
                new Sound.SongData(MusicFolder + "RM 2 - Arcane Benevolence", false, 0.22f),
                new Sound.SongData(MusicFolder + "RM 3 - Left in Autumn", false, 0.11f),
                new Sound.SongData(MusicFolder + "RM 4 - Warhogs", false, 0.2f),
                new Sound.SongData(MusicFolder + "RM 5 - Suddenly Empty", false, 0.15f),
                new Sound.SongData(MusicFolder + "RM 6 - Auderesne", false, 0.2f),
                new Sound.SongData(MusicFolder + "RM 7 - For Eternity", false, 0.18f),
                new Sound.SongData(MusicFolder + "RM 8 - Asynchronous Flanking", false, 0.13f),
                new Sound.SongData(MusicFolder + "RM 9 - Weeping Bedlam", false, 0.18f),



                new Sound.SongData(MusicFolder + "digital battleground", false, 0.2f),
                new Sound.SongData(MusicFolder + "echoes of valor", false, 0.18f),
                new Sound.SongData(MusicFolder + "Pixelated Battlefields", false, 0.15f),

                new Sound.SongData(MusicFolder + "ancient space", false, 0.22f),
                new Sound.SongData(MusicFolder + "Dreamscape Adventures", false, 0.2f),
                new Sound.SongData(MusicFolder + "Shadows of Conflict", false, 0.2f),
                new Sound.SongData(MusicFolder + "Veil of Time", false, 0.3f),

            },
            PlatformSettings.PlayMusic);
        }
    }
}
