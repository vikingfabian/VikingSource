using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Interface;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.Sound;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class SoundLib
    {
        public static readonly string SoundDir = DssLib.ContentDir + "Sound" + DataStream.FilePath.Dir;

        public static SoundContainerBase click, ui_expand, option_select, option_deselect, /*hover,*/ hover_disabled, clicktab,/* hovertab,*/ back, 
            scroll_back, scroll_forward,
            buy, wrong, soft_buzz_error, start_build_contruct, start_destroy_contruct,
            copy, paste, start, stop,
            select_army, select_city, select_faction,
            ordermove, orderstop, message, trophy,
            woodcut, tree_falling, breaking, scythe, drop_item, pickaxe, hen, pig, pickup,
            anvil, dig, genericWork, hammer, footstep, ship_knirr,

           bow, sword, spear, throwblade, throwitem, clothHit, crossbow, heavyballista, reloadballista,
           blade_light, blade_medium, blade_heavy, spear_whoosh,
           musket, cannon, block_attack, wood_bonk,
            
           painvoice, fleshgore;

        public static RbSoundProfile menu, menuHover, menuOption, menuOptionDeselect, menutab, menuExpand, menutabHover, menuBack, menuBuy, menuArmyHalt, menuCopy, menuPaste, menuStart, menuStop;

        public static RbSoundAction buttonHoverAction, tabHoverAction;

        public static void LoadContent()
        {
            click = new SoundContainerSingle(SoundDir + "click", 0.7f);
            ui_expand = new SoundContainerSingle(SoundDir + "menu_expand", 0.7f);
            option_select = new SoundContainerSingle(SoundDir + "option_select", 0.2f);
            option_deselect = new SoundContainerSingle(SoundDir + "option_deselect", 0.7f);
            //hover = new SoundContainerMultiple([SoundDir + "button_hover1", SoundDir + "button_hover2"], 0.7f);
            hover_disabled = new SoundContainerSingle(SoundDir + "hover_disabled", 0.7f);
            clicktab = new SoundContainerSingle(SoundDir + "tab_click", 0.5f);
            scroll_back = new SoundContainerSingle(SoundDir + "scroll_back", 0.8f);
            scroll_forward = new SoundContainerSingle(SoundDir + "scroll_forward", 0.8f);
            //hovertab = new SoundContainerSingle(SoundDir + "tab_hover", 0.7f);
            back = new SoundContainerSingle(SoundDir + "back", 0.05f);
            buy = new SoundContainerSingle(SoundDir + "buy");
            wrong = new SoundContainerSingle(SoundDir + "wrong", 0.6f);
            soft_buzz_error = new SoundContainerSingle(SoundDir + "soft_buzz_error", 0.1f);
            start_build_contruct = new SoundContainerSingle(SoundDir + "start_build_contruct", 0.6f);
            start_destroy_contruct = new SoundContainerSingle(SoundDir + "start_destroy_contruct", 0.8f);

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
            scythe = new SoundContainerSingle(SoundDir + "scythe", 0.2f, 0.4f);
            drop_item = new SoundContainerSingle(SoundDir + "drop_item", 1f, 0.4f);
            pickaxe = new SoundContainerSingle(SoundDir + "pickaxe", 0.6f, 0.2f);
            hen = new SoundContainerMultiple([SoundDir + "hen1", SoundDir + "hen2"], 0.6f, 0.4f);
            pig = new SoundContainerSingle(SoundDir + "pig", 0.6f, 0.8f);
            pickup = new SoundContainerSingle(SoundDir + "pickup", 0.6f, 0.4f);
            anvil = new SoundContainerSingle(SoundDir + "anvil", 0.45f, 0.4f);
            dig = new SoundContainerSingle(SoundDir + "dig", 0.3f, 0.4f);
            genericWork = new SoundContainerMultiple([SoundDir + "generic_work1", SoundDir + "generic_work2",], 0.3f, 0.4f);
            hammer = new SoundContainerSingle(SoundDir + "hammer", 0.6f, 0.4f);
            footstep = new SoundContainerMultiple([
                SoundDir + "footstep (1)",
                SoundDir + "footstep (2)",
                SoundDir + "footstep (3)",
                SoundDir + "footstep (4)",
                SoundDir + "footstep (5)",
                SoundDir + "footstep (6)",
                SoundDir + "footstep (7)",
                SoundDir + "footstep (8)",
                SoundDir + "footstep (9)",
                SoundDir + "footstep (10)"
            ], 0.20f, 0.4f);
            ship_knirr = new SoundContainerMultiple([SoundDir + "ship_knirr1", SoundDir + "ship_knirr2"], 1.3f, 0.4f);
            //Attacks
            bow = new SoundContainerMultiple([SoundDir + "bow1", SoundDir + "bow2"], 0.5f, 0.4f);
            crossbow = new SoundContainerMultiple([SoundDir + "CrossBow1", SoundDir + "CrossBow2"], 0.5f, 0.4f);
            sword = new SoundContainerMultiple([SoundDir + "sword1", SoundDir + "sword2"], 0.5f, 0.4f);
            spear = new SoundContainerSingle(SoundDir + "spear", 0.4f, 0.2f);
            throwblade = new SoundContainerSingle(SoundDir + "bladethrow", 0.4f, 0.2f);
            throwitem = new SoundContainerSingle(SoundDir + "throw", 0.6f, 0.2f);
            heavyballista = new SoundContainerMultiple([SoundDir + "heavy_ballista1", SoundDir + "heavy_ballista2"], 0.5f, 0.4f);
            reloadballista = new SoundContainerMultiple([SoundDir + "reload_ballista1", SoundDir + "reload_ballista2"], 0.5f, 0.4f);
            clothHit = new SoundContainerMultiple([SoundDir + "cloth_hit1", SoundDir + "cloth_hit2"], 0.3f, 0.4f);
            blade_light = new SoundContainerMultiple([SoundDir + "blade_whoosh_light_01", SoundDir + "blade_whoosh_light_02", SoundDir + "blade_whoosh_light_03", SoundDir + "blade_whoosh_light_08"], 0.06f, 0.4f);
            blade_medium = new SoundContainerMultiple([SoundDir + "blade_whoosh_med_01", SoundDir + "blade_whoosh_med_02", SoundDir + "blade_whoosh_med_03", SoundDir + "blade_whoosh_med_05"], 0.06f, 0.4f);
            blade_heavy = new SoundContainerMultiple([SoundDir + "blade_whoosh_heavy_03", SoundDir + "blade_whoosh_heavy_04", SoundDir + "blade_whoosh_heavy_08", SoundDir + "blade_whoosh_heavy_14"], 0.08f, 0.4f);
            spear_whoosh = new SoundContainerMultiple([SoundDir + "spearwhoosh (1)", SoundDir + "spearwhoosh (2)", SoundDir + "spearwhoosh (3)", SoundDir + "spearwhoosh (4)"], 0.06f, 0.4f);
            musket = new SoundContainerMultiple([SoundDir + "musket1",SoundDir + "musket2",SoundDir + "musket3"], 2f, 0.4f);
            cannon = new SoundContainerMultiple([SoundDir + "cannon1",SoundDir + "cannon2",SoundDir + "cannon3"], 3f, 0.4f);
            block_attack = new SoundContainerMultiple([SoundDir + "block_attack (1)", SoundDir + "block_attack (3)"], 0.8f, 0.4f);
            wood_bonk = new SoundContainerMultiple([SoundDir + "wood_bonk1", SoundDir + "wood_bonk2"], 0.8f, 0.4f);

            //Damage
            painvoice = new SoundContainerMultiple([SoundDir + "Dwarf Pain 1", SoundDir + "Dwarf Pain 2", SoundDir + "Dwarf Pain 3", SoundDir + "Dwarf Pain 4", SoundDir + "Dwarf Pain 5", SoundDir + "Dwarf Pain 6" ], 0.4f, 0.6f);
            fleshgore =new SoundContainerMultiple([SoundDir + "flesh_gore (1)",SoundDir + "flesh_gore (2)",SoundDir + "flesh_gore (3)",SoundDir + "flesh_gore (4)",SoundDir + "flesh_gore (5)",SoundDir + "flesh_gore (6)",SoundDir + "flesh_gore (7)",SoundDir + "flesh_gore (8)",SoundDir + "flesh_gore (9)",SoundDir + "flesh_gore (10)",SoundDir + "flesh_gore (11)",SoundDir + "flesh_gore (12)",SoundDir + "flesh_gore (13)"], 0.1f, 0.5f);
            menu = new RbSoundProfile(click, wrong);
            menuOption = new RbSoundProfile(option_select, wrong);
            menuOptionDeselect = new RbSoundProfile(option_deselect, wrong);
            //menuHover = new RbSoundProfile(hover, hover_disabled);
            menutab = new RbSoundProfile(clicktab, wrong);
            menuExpand = new RbSoundProfile(ui_expand, wrong);
            //menutabHover = new RbSoundProfile(hovertab, hover_disabled);
            menuBack = new RbSoundProfile(back);
            menuBuy = new RbSoundProfile(buy, wrong);
            menuArmyHalt = new RbSoundProfile(orderstop);
            menuCopy = new RbSoundProfile(copy);
            menuPaste = new RbSoundProfile(paste);
            menuStart = new RbSoundProfile(start);
            menuStop = new RbSoundProfile(stop);

            //buttonHoverAction = new RbSoundAction(menuHover);
            //tabHoverAction = new RbSoundAction(menutabHover);

            Engine.LoadContent.LoadSound(LoadedSound.out_of_ammo, SoundDir + "out_of_ammo");
            
            //Ref.music.SetPlaylist(Music.PlayList(), PlatformSettings.PlayMusic);
        }
    }
}
