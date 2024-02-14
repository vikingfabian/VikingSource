using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    static class SoundLib
    {
        public const string SoundDir = LfLib.ContentFolder + "Sound\\";
        public const string NewSoundDir = SoundDir + "NEW\\";

        public static readonly Sound.SoundSettings UnavailableActionSound = new Sound.SoundSettings(LoadedSound.out_of_ammo);
        public static readonly Sound.SoundSettings SmallSuccessSound = new Sound.SoundSettings(LoadedSound.CraftSuccessful);
        public static readonly Sound.SoundSettings LargeSuccessSound = new Sound.SoundSettings(LoadedSound.Dialogue_QuestAccomplished);
        public static readonly Sound.SoundSettings NpcChatSound = new Sound.SoundSettings(LoadedSound.Dialogue_Neutral);
        public static readonly Sound.SoundSettings CoinSound = new Sound.SoundSettings(LoadedSound.Coin1, 0.6f, 3);

        public static void LoadMusic()
        {
            string MusicFolder = LfLib.MusicFolder;

            Ref.music.SetPlaylist(new List<Sound.SongData>
                {
                    new Sound.SongData(MusicFolder + "AgriculturalHuts", "Agricultural Huts", true, 1f),
                    new Sound.SongData(MusicFolder + "Apple Pie Sensation_loop", "Apple Pie Sensation", true, 1f),
                    new Sound.SongData(MusicFolder + "BabyBadgersAndABird", "Baby Badgers And A Bird", true, 1f),
                    new Sound.SongData(MusicFolder + "BunnyMassacre", "Bunny Massacre", true, 1f),
                    new Sound.SongData(MusicFolder + "Clumsy_Lord_HQ","Clumsy Lord", true, 1f),
                    new Sound.SongData(MusicFolder + "DungeonKeepersKey_HQ", "Dungeon Keepers Key", true, 1f),
                    new Sound.SongData(MusicFolder + "Endless Plains", "Endless Plains", true, 1f),
                    new Sound.SongData(MusicFolder + "Gargoyle_HQ", "Gargoyle", true, 1f),
                    new Sound.SongData(MusicFolder + "Immortals","Immortals", false, 1f),
                    new Sound.SongData(MusicFolder + "King_Of_Sleepers_HQ", "King Of Sleepers", true, 1f),
                    new Sound.SongData(MusicFolder + "MissingCardinals", "Missing Cardinals", true, 1f),
                    new Sound.SongData(MusicFolder + "Mother_loop","Mother", false, 1f),
                    new Sound.SongData(MusicFolder + "Mysterious Grotto","Mysterious Grotto", false, 1.6f),
                    new Sound.SongData(MusicFolder + "Night_HQ", "Night", true, 1f),
                    new Sound.SongData(MusicFolder + "Restart_loop", "Restart", true, 1f), //new
                    new Sound.SongData(MusicFolder + "Sad_Porcupine", "Sad Porcupine", true, 1f),
                    new Sound.SongData(MusicFolder + "Solitary_Comfort_HQ", "Solitary Comfort", false, 1f),
                    new Sound.SongData(MusicFolder + "TheBeetles_HQ", "The Beetles", true, 1f),
                    new Sound.SongData(MusicFolder + "TheLazyFrogHQ","The Lazy Frog", true, 1f),
                    new Sound.SongData(MusicFolder + "TheOldPhilosopherHQ","The Old Philosopher", true, 1f),
                    new Sound.SongData(MusicFolder + "YesIAmYourGodHQ","Yes I Am Your God", true, 1f),
                },
                PlatformSettings.PlayMusic);
        }

        

        public static void LoadSound()
        {
            LoadVoxEditorSound();

            Engine.LoadContent.LoadSound(LoadedSound.MenuSelect, SoundDir + "Button_Clicked");
                    
            Engine.LoadContent.LoadSound(LoadedSound.MenuBack, SoundDir + "Returning");

            Engine.LoadContent.LoadSound(LoadedSound.Coin1, SoundDir + "coin1");
            Engine.LoadContent.LoadSound(LoadedSound.Coin2, SoundDir + "coin2");
            Engine.LoadContent.LoadSound(LoadedSound.Coin3, SoundDir + "coin3");
            Engine.LoadContent.LoadSound(LoadedSound.Sword1, SoundDir + "sword1");
            Engine.LoadContent.LoadSound(LoadedSound.Sword2, SoundDir + "sword2");
            Engine.LoadContent.LoadSound(LoadedSound.Sword3, SoundDir + "sword3");
            Engine.LoadContent.LoadSound(LoadedSound.TakeDamage1, SoundDir + "takedamage1");
            Engine.LoadContent.LoadSound(LoadedSound.TakeDamage2, SoundDir + "takedamage2");
            Engine.LoadContent.LoadSound(LoadedSound.PickUp, SoundDir + "pickup");
            Engine.LoadContent.LoadSound(LoadedSound.MonsterHit1, SoundDir + "animal_hurt1");
            Engine.LoadContent.LoadSound(LoadedSound.MonsterHit2, SoundDir + "animal_hurt2");
            Engine.LoadContent.LoadSound(LoadedSound.Bow, SoundDir + "bow");
            Engine.LoadContent.LoadSound(LoadedSound.EnemyProj1, SoundDir + "enemyproj1");
            Engine.LoadContent.LoadSound(LoadedSound.EnemyProj2, SoundDir + "enemyproj2");

            Engine.LoadContent.LoadSound(LoadedSound.door, SoundDir + "door");
            Engine.LoadContent.LoadSound(LoadedSound.chat_message, SoundDir + "chat_message");

            Engine.LoadContent.LoadSound(LoadedSound.express_anger, SoundDir + "express_anger");
            Engine.LoadContent.LoadSound(LoadedSound.express_hi1, SoundDir + "express_hi1");
            Engine.LoadContent.LoadSound(LoadedSound.express_hi2, SoundDir + "express_hi2");
            Engine.LoadContent.LoadSound(LoadedSound.express_hi3, SoundDir + "express_hi3");
            Engine.LoadContent.LoadSound(LoadedSound.express_laugh, SoundDir + "express_laugh");
            Engine.LoadContent.LoadSound(LoadedSound.express_teasing1, SoundDir + "express_teasing1");
            Engine.LoadContent.LoadSound(LoadedSound.express_teasing2, SoundDir + "express_teasing2");
            Engine.LoadContent.LoadSound(LoadedSound.express_thumbup1, SoundDir + "express_thumbup1");
            Engine.LoadContent.LoadSound(LoadedSound.express_thumbup2, SoundDir + "express_thumbup2");
            Engine.LoadContent.LoadSound(LoadedSound.player_enters, SoundDir + "player_enters");
            Engine.LoadContent.LoadSound(LoadedSound.enter_build, SoundDir + "enter_build");
           


            Engine.LoadContent.LoadSound(LoadedSound.buy, SoundDir + "buy");
            Engine.LoadContent.LoadSound(LoadedSound.Trophy, SoundDir + "Trophy");

            Engine.LoadContent.LoadSound(LoadedSound.open_map, SoundDir + "open_map");
            Engine.LoadContent.LoadSound(LoadedSound.shieldcrash, SoundDir + "shieldcrash");
            Engine.LoadContent.LoadSound(LoadedSound.weaponclink, SoundDir + "weaponclink");

            Engine.LoadContent.LoadSound(LoadedSound.deathpop, SoundDir + "deathpop");
            Engine.LoadContent.LoadSound(LoadedSound.out_of_ammo, SoundDir + "out_of_ammo");


            Engine.LoadContent.LoadSound(LoadedSound.MenuNotAllowed, SoundDir + "Not_Allowed");
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_Neutral, SoundDir + "Talking");
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_DidYouKnow, SoundDir + "Did_You_Know");
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_Question, SoundDir + "Question");
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_QuestAccomplished, SoundDir + "Quest_Accomplished");
            Engine.LoadContent.LoadSound(LoadedSound.CraftSuccessful, SoundDir + "Craft_Successful");

            // LF3 NEW
            
            //Engine.LoadContent.LoadSound(LoadedSound.MenuLo100MS, soundDir + "lo_100ms");
            //Engine.LoadContent.LoadSound(LoadedSound.MenuHi100MS, soundDir + "hi_100ms");
            VikingEngine.HUD.Gui.LoadContent();
            Engine.LoadContent.LoadSound(LoadedSound.SmallExplosion, NewSoundDir + "Small Explosion");
            Engine.LoadContent.LoadSound(LoadedSound.LargeExplosion, NewSoundDir + "Large Explosion");
            Engine.LoadContent.LoadSound(LoadedSound.FastSwing, NewSoundDir + "Fast Swing");
            Engine.LoadContent.LoadSound(LoadedSound.Melee, NewSoundDir + "Melee");
            Engine.LoadContent.LoadSound(LoadedSound.HealthUp, NewSoundDir + "Heal");
        }

        public static void LoadVoxEditorSound()
        {
            Engine.LoadContent.LoadSound(LoadedSound.block_place_1, SoundDir + "block_place_1");
            Engine.LoadContent.LoadSound(LoadedSound.tool_dig, SoundDir + "tool_dig");
            Engine.LoadContent.LoadSound(LoadedSound.tool_select, SoundDir + "tool_select");
        }
    }
}
