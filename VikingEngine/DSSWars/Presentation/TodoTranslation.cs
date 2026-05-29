using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        public string GameSettings_WideScrollbar => "Wide scrollbar";

        public string Network_PlayOffline = "Play offline";
        public string JoinPermission_Title => "Join Permissions";
        public string JoinPermission_Private=> "Private";
        public string JoinPermission_FriendsOnly => "Friends only";
        public string JoinPermission_Public => "Public";



        public string Network_Join => "Join game";
        public string Lobby_Category_MultiplayerSettings => "Multiplayer settings";


        public string GiftAchieve_WhiteKnight_Name => "White Knight";
        public string GiftAchieve_WhiteKnight_Desc => "Looking good, protecting others.";

        public string GiftAchieve_HeroComplexSaviorComplex_Name => "Savior Complex";
        public string GiftAchieve_HeroComplexSaviorComplex_Desc => "Needs to be the hero of the story.";

        public string GiftAchieve_CryBaby_Name => "Cry Baby";
        public string GiftAchieve_CryBaby_Desc => "Stop complaining!";

        public string GiftAchieve_KingMaker_Name => "King Maker";
        public string GiftAchieve_KingMaker_Desc => "You didn't win, but you decided who did.";

        public string GiftAchieve_Turtle_Name => "Turtle";
        public string GiftAchieve_Turtle_Desc => "Hiding behind walls.";

        public string GiftAchieve_MetaPlayer_Name => "Meta Player";
        public string GiftAchieve_MetaPlayer_Desc => "A proven strategy, is the ONLY strategy!";

        public string GiftAchieve_Tryhard_Name => "Tryhard";
        public string GiftAchieve_Tryhard_Desc => "What if you tried to have fun.";

        public string GiftAchieve_DidPracticeInSecret_Name => "Secret Practicer";
        public string GiftAchieve_DidPracticeInSecret_Desc => "Are you sure that you never player before?";

        public string GiftAchieve_TheEncyclopedia_Name => "The Encyclopedia";
        public string GiftAchieve_TheEncyclopedia_Desc => "How do you know everything?";

        public string GiftAchieve_WarCriminal_Name => "War Criminal";
        public string GiftAchieve_WarCriminal_Desc => "There's no reason we can't be civil, is there?";

        public string GiftAchieve_FarmerRush_Name => "Farmer Rush";
        public string GiftAchieve_FarmerRush_Desc => "Why make steel, when you have numbers?";

        public string GiftAchieve_Politian_Name => "Politician";
        public string GiftAchieve_Politian_Desc => "Words is your weapon.";

        public string GiftAchieve_Socializer_Name => "Socializer";
        public string GiftAchieve_Socializer_Desc => "Someone is here just to chat.";

        public string GiftAchieve_OverAchiever_Name => "Overachiever";
        public string GiftAchieve_OverAchiever_Desc => "You look a little sweaty there...";

        public string GiftAchieve_Noob_Name => "Noob";
        public string GiftAchieve_Noob_Desc => "Did you skip the tutorial?";

        public string GiftAchieve_SwedishNeutrality_Name => "Swedish Neutrality";
        public string GiftAchieve_SwedishNeutrality_Desc => "Why take sides...";

        public string GiftAchieve_TroubleMaker_Name => "Troublemaker";
        public string GiftAchieve_TroubleMaker_Desc => "My mom warned me about you";//"Just want to watch the world burn.";

        public string GiftAchieve_ScorchedEarth_Name => "Scorched Earth";
        public string GiftAchieve_ScorchedEarth_Desc => "Nothing but rubble, is left behind";

        public string GiftAchieve_WarMonger_Name => "Warmonger";
        public string GiftAchieve_WarMonger_Desc => "Violence as the only answer!";

        public string GiftAchieve_LivingInABobble_Name => "Living in a Bubble";
        public string GiftAchieve_LivingInABobble_Desc => "Completely oblivious to the world around them.";

        public string GiftAchieve_Bullie_Name => "Bully";
        public string GiftAchieve_Bullie_Desc => "Pick on someone your own size!";

        public string GiftAchieve_ControlFreak_Name => "Control Freak";
        public string GiftAchieve_ControlFreak_Desc => "Why leave anything to chance?";

        public string GiftAchieve_RandomNothingMakesSense_Name => "Agent of Chaos";
        public string GiftAchieve_RandomNothingMakesSense_Desc => "How do you defy all logic and reason?";

        public string GiftAchieve_Hoarder_Name => "Hoarder";
        public string GiftAchieve_Hoarder_Desc => "Going to keep all those resources to yourself?";

        public string GiftAchieve_Scatterbrained_Name => "Scatterbrained";
        public string GiftAchieve_Scatterbrained_Desc => "Focus please!";

        public string GiftAchieve_NearSighted_Name => "Nearsighted";
        public string GiftAchieve_NearSighted_Desc => "Only sees whats in front of your nose.";

        public string GiftAchieve_AutomationAbuser_Name => "Automation Abuser";
        public string GiftAchieve_AutomationAbuser_Desc => "Your game plays itself...";

        public string GiftAchieve_Troll_Name => "Troll";
        public string GiftAchieve_Troll_Desc => "Anything for the LOLs.";

        public string GiftAchieve_MemeLord_Name => "Meme Lord";
        public string GiftAchieve_MemeLord_Desc => "Do you know how to talk outside of internet?";

        public string GiftAchieve_SupportSlave_Name => "Support Slave";
        public string GiftAchieve_SupportSlave_Desc => "Its hard work to carry others.";

        public string GiftAchieve_DarkSidePlayer_Name => "Lost to the Dark Side";
        public string GiftAchieve_DarkSidePlayer_Desc => "Nice guys finish last.";

        public string GiftAchieve_SlaughterHouse_Name => "Slaughterhouse";
        public string GiftAchieve_SlaughterHouse_Desc => "The rivers will run red!";

        public string GiftAchieve_AnimalCruelty_Name => "Animal Cruelty";
        public string GiftAchieve_AnimalCruelty_Desc => "Can someone call the Department of Animal Services?";

        public string GiftAchieve_LuckyBastard_Name => "Lucky Bastard";
        public string GiftAchieve_LuckyBastard_Desc => "The Gods of RNG are with you!";

        public string GiftAchieve_Cursed_Name => "Cursed";
        public string GiftAchieve_Cursed_Desc => "You have no luck!";

        public string GiftAchieve_Backstabber_Name => "Backstabber";
        public string GiftAchieve_Backstabber_Desc => "Betrayal!";

        public string GiftAchieve_Oathbreaker_Name => "Oathbreaker";
        public string GiftAchieve_Oathbreaker_Desc => "Your word is your bond.";

        public string GiftAchieve_Wormtongue_Name => "Wormtongue";
        public string GiftAchieve_Wormtongue_Desc => "Whispers poison in our ears.";

        public string GiftAchieve_ArmchairGeneral_Name => "Armchair General";
        public string GiftAchieve_ArmchairGeneral_Desc => "The Logistics Mastermind (Of Snacks).";

        public string GiftAchieve_Salty_Name => "Salty";
        public string GiftAchieve_Salty_Desc => "Boiling with rage.";

        public string GiftAchieve_SaltMiner_Name => "Salt Miner";
        public string GiftAchieve_SaltMiner_Desc => "Other peoples rage is your reward";

        public string GiftAchieve_PuppetMaster_Name => "Puppet Master";
        public string GiftAchieve_PuppetMaster_Desc => "Orchestrated the entire game from the shadows.";

        public string GiftAchieve_TheCarry_Name => "The Carry";
        public string GiftAchieve_TheCarry_Desc => "Put the entire team on their back to secure the win.";

        public string GiftAchieve_OneManArmy_Name => "One-Man Army";
        public string GiftAchieve_OneManArmy_Desc => "Don't need any help.";

        public string GiftAchieve__4DChessPlayer_Name => "4D Chess Player";
        public string GiftAchieve__4DChessPlayer_Desc => "We can't comprehend your big IQ moves.";

        public string GiftAchieve_SpreadsheetWarrior_Name => "Spreadsheet Warrior";
        public string GiftAchieve_SpreadsheetWarrior_Desc => "Can optimize the fun out of any game.";

        public string GiftAchieve_MeatShield_Name => "Meat Shield";
        public string GiftAchieve_MeatShield_Desc => "Absorbed all the damage.";

        public string GiftAchieve_InDebt_Name => "In Debt";
        public string GiftAchieve_InDebt_Desc => "One day you need to pay that back!";

        public string GiftAchieve_OnLifeSupport_Name => "On Life Support";
        public string GiftAchieve_OnLifeSupport_Desc => "You wouldn't be alive is it weren't for...";

        public string GiftAchieve_LoneWolf_Name => "Lone Wolf";
        public string GiftAchieve_LoneWolf_Desc => "Refused to cooperate or group up with the team.";

        public string GiftAchieve_ShaggyTooDopeAlwaysChilling_Name => "Always Chilling";
        public string GiftAchieve_ShaggyTooDopeAlwaysChilling_Desc => "Never breaking the sweat.";

        public string GiftAchieve_BadInfluence_Name => "Bad Influence";
        public string GiftAchieve_BadInfluence_Desc => "Maybe I shouldn't listen to you.";
    

        public string GiftAchieve_HindsightTactician_Name => "Hindsight Tactician";
        public string GiftAchieve_HindsightTactician_Desc => "Untouched by the fog of war, viewing past chaos through the absolute clarity of the future.";


        public string GiftAchieve_Houseplant_Name => "A Decorative Houseplant";
        public string GiftAchieve_Houseplant_Desc => "Hello? Are you there?";

        public string GiftAchieve_Sheep_Name => "Herd Mentality";
        public string GiftAchieve_Sheep_Desc => "You are allowed your own opinion.";

    }
}