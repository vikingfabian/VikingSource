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
        public string GameSettings_DisplayInputHelp => "Input help";
        public string GameSettings_InputSmoothing => "Input smoothing";

        //--
        public string FactionName_Ellium => "Ellium";
        public string FactionName_GrakPushdug => "Grak pushdug";
        public string FactionName_Draugost => "Draugost";

        //--
        public string Unlock_PublicGames => "public games";
        public string UnlockPublic_Warning1 => "Do not play with strangers";
        public string UnlockPublic_Warning2 => "The game has zero protection against cheating or trolling";
        public string Unlock_WarningBadExperience => "You will have a bad experience";
        public string Hud_Accept => "Accept";
        public string Hud_Deny => "Deny";
        public string Hud_Reason => "Reason";
        public string Hud_Full => "Full";
        public string Hud_Version => "Version";
        public string Unlock_PlayerVersusPlayer => "player versus player";
        public string UnlockPvp_Warning1 => "DSS is not designed for competetive games";
        public string UnlockPvp_Warning2 => "There is no balance, matches will be unfair";
        public string BlockedPlayersTitle => "Blocked players";
        public string ClickToRemoveBan => "Click: remove ban";
        public string HostSettingsTitle => "Host settings";
        public string MaxPlayerCount => "Max player count";
        public string DistanceBetweenPlayers => "Distance between players";
        public string AllowHandicap => "Allow handicap";
        public string AllowCasualControls => "Allow casual controls";
        public string AutoRecolorPlayerFlags => "Auto recolor player flags";
        public string DefaultDiplomacy => "Default diplomacy";
        public string ClientSettingsTitle => "Client settings";
        public string UseHandicap => "Use handicap";
        public string DifficultyDescription_BotAggression => "Bot aggression";
        public string Hud_GetExtraX => "Extra {0}";
        public string Hud_Hide => "Hide";
        public string Hud_ModelType => "Model";

        /// <summary>
        /// Label: Text text
        /// </summary>
        public string Language_LabelAndText_Colon => "{0}: {1}";
        public string Language_CathergoryDashUndercathegory => "{0} - {1}";

        public string ResourceBoost => "Resource boost";
        //public string TaxIncome => "Tax income";
        public string PlayerInteractionTitle => "Player interaction";
        public string DefaultPeaceful => "Default: Peaceful";
        public string DefaultCoOptional => "Default: Co-optional";
        public string DefaultHardcore => "Default: Hardcore";
        public string GeneralTitle => "General";
        
        public string InputButton_Hold => "Button hold";
        public string InputButton_Toggle => "Button toggle";
        public string VoiceOptAlwaysOn => "Always on";
        public string VoiceTitle => "Voice";
        public string GiftOptAllow => "Allow";
        public string GiftOptFriendsOnly => "Friends only";
        public string GiftOptBlocked => "Blocked";
        public string ReceiveAchievementsTitle => "Recieve achievements";
        public string GiftWarning => "Warning! Gifted achievements can feel demeaning";
        public string FullReset => "Full reset";
        public string AllowAllianceTitle => "Allow alliance";
        public string CanBreakAlliance => "Can break alliance";
        public string AllowWarTitle => "Allow war";
        public string FairProtection => "Fair protection";
        public string FairProtectionTooltip => "Protected players must use their rules on you";
        public string MustAsk => "Must ask";
        public string MustAskTooltip => "Both players must agree to fight";
        public string AllianceLimit => "Alliance limt";
        public string AllianceLimitTooltip => "Can't be attacked by a larger player alliance";
        public string GameStartProtection => "Game start protection";
        public string Hud_Time_Minutes => "minutes";
        public string WarPreparationTime => "War preparation time";
        public string WarPreparationTimeTooltip => "A delay from war declaration until attacks are available";
        public string Hud_Allow => "Allow";
        public string Hud_Blocked => "Blocked";

        public string Hud_DeleteAll => "Delete all";
        public string DiplomacyPlayersChoice => "Players choice";
        public string UnlockSureTitle => "Are you really, really sure?";
        public string UnlockSureDescription => "Will you be a big boy and not cry on the forum later?";



        public string Network_PlayOffline = "Play offline";
        public string JoinPermission_Title => "Join Permissions";
        public string JoinPermission_Private => "Private";
        public string JoinPermission_FriendsOnly => "Friends only";
        public string JoinPermission_Public => "Public";

        public string Network_Join => "Join game";
        public string Network_ConnectingToGame => "Connecting...";
        public string Lobby_Category_MultiplayerSettings => "Multiplayer settings";

        public string Hud_Default = "Default";
        public string Group_Team = "team";
        public string Group_Everyone = "everyone";

        public string EngineHud_SymbolForMillion => "M";

        public string DecorType_DiplomaticStatue => "Diplomatic statue";

        public string Message => "Message";
        public string ObjectType_LocationPin => "Location pin";
        public string ObjectType_LocationPin_Share => "Share and ping";
        public string InputActionName_TextChat => "Text chat";
        public string InputActionName_TextChatLog => "Chat log";
        public string InputActionName_VoiceChat => "Voice chat";
        public string InputActionName_NextPin => "Next pin";
        public string InputActionName_PinAndPing => "Add pin";

        public string Leaderboards_ArmySize => "Army size, in strength";
        public string Leaderboards_MultiplayerPlayerCount => "Hosting player count";

        public string Multiplayer_BanWarning => "ban warning";
        public string Multiplayer_SentToHost => "Will be sent to the host";
        public string Multiplayer_AddToOwnBlocks => "Add to your own block list";

        public string Multiplayer_Message_RequestSent => "Request sent";

        public string Multiplayer_BlockPlayer => "Block player";
        public string Multiplayer_NetSession => "Net session";
        public string GiftedAchievements => "Gifted achievements";
        public string GiftedAchievements_Description => "Reward your friends bad behaiviour";

        public string Multiplayer_Title => "Multiplayer";
        public string Multiplayer_Lobby => "Multiplayer lobby";

        public string Multiplayer_Tutorial_HostStart => "1. The host starts a game";
        public string Multiplayer_Tutorial_JoinButton => "2. A join button will appear here";
        public string Multiplayer_Tutorial_Visible => "The host must have a visible Steam profile";

        public string Multiplayer_KickPlayer => "Kick player";
        public string Multiplayer_RequestBlockPlayer => "Request: Block player";
        public string Multiplayer_HandoverComplete => "Handover complete";
        public string Multiplayer_LoadingClientSave => "Loading client save";
        public string Multiplayer_ClientSaveComplete => "Client save complete";
        public string Multiplayer_BadActor => "Bad actor";
        public string Multiplayer_NetworkError => "Network error";
        public string Multiplayer_Sender => "Sender";
        public string Multiplayer_Reciever => "Reciever";
        public string Multiplayer_PlayerJoined => "Player joined";
        public string Multiplayer_PlayerLeft => "Player left";
        public string Multiplayer_RequestingClientGamestates => "Requesting client gamestates...";
        public string Multiplayer_TextChat => "Text chat";
        public string Multiplayer_VoiceChat => "Voice chat";
        //public string Multiplayer_ => ;
        //public string Multiplayer_ => ;
        //public string Multiplayer_ => ;
        public string Steam_UserProfile => "User profile";
        public string Steam_OpenSteamOverlay => "Open Steam overlay";
        
        /// <summary>
        /// In this relation there is a countdown towards war
        /// </summary>
        public string Diplomacy_RelationType_Mobilizing => "Mobilizing";
        public string Diplomacy_OfferRelation => "Offer relation";
        public string Diplomacy_OfferRelation_Declined => "Declined offer relation";
        public string Diplomacy_SendGold => "Send gold";
        public string Diplomacy_GiftToPlayer => "Gift to player";
        public string Diplomacy_OnAccept => "If the other player accepts:";
        public string Diplomacy_WarPreparationTime => "War preparation time";

        public string Diplomacy_AboveSoftCap => "Above soft cap";
        public string Diplomacy_BelowSoftCap => "Below soft cap";
        public string Diplomacy_OpenPlayerToPlayer => "Open player diplomacy";
        //--

        public string GiftAchieve_WhiteKnight_Name => "White Knight";
        public string GiftAchieve_WhiteKnight_Desc => "Looking good, protecting others.";

        public string GiftAchieve_HeroComplexSaviorComplex_Name => "Savior Complex";
        public string GiftAchieve_HeroComplexSaviorComplex_Desc => "Needs to be the hero of the story.";

        public string GiftAchieve_CryBaby_Name => "Cry Baby";
        public string GiftAchieve_CryBaby_Desc => "Stop complaining!";

        public string GiftAchieve_KingMaker_Name => "King Maker";
        public string GiftAchieve_KingMaker_Desc => "You helped them win...";

        public string GiftAchieve_Turtle_Name => "Turtle";
        public string GiftAchieve_Turtle_Desc => "Hiding behind walls.";

        public string GiftAchieve_MetaPlayer_Name => "Meta Player";
        public string GiftAchieve_MetaPlayer_Desc => "A proven strategy is the ONLY strategy!";

        public string GiftAchieve_Tryhard_Name => "Tryhard";
        public string GiftAchieve_Tryhard_Desc => "What if you tried to have fun?";

        public string GiftAchieve_DidPracticeInSecret_Name => "Secret Practicer";
        public string GiftAchieve_DidPracticeInSecret_Desc => "Are you sure that you never played before?";

        public string GiftAchieve_TheEncyclopedia_Name => "The Encyclopedia";
        public string GiftAchieve_TheEncyclopedia_Desc => "How do you know everything?";

        public string GiftAchieve_WarCriminal_Name => "War Criminal";
        public string GiftAchieve_WarCriminal_Desc => "There's no reason we can't be civil, is there?";

        public string GiftAchieve_FarmerRush_Name => "Farmer Rush";
        public string GiftAchieve_FarmerRush_Desc => "Why make steel when you have numbers?";

        public string GiftAchieve_Politician_Name => "Politician";
        public string GiftAchieve_Politician_Desc => "Words are your weapon.";

        public string GiftAchieve_Socializer_Name => "Socializer";
        public string GiftAchieve_Socializer_Desc => "Someone is here just to chat.";

        public string GiftAchieve_OverAchiever_Name => "Overachiever";
        public string GiftAchieve_OverAchiever_Desc => "You look a little sweaty there...";

        public string GiftAchieve_Noob_Name => "Noob";
        public string GiftAchieve_Noob_Desc => "Did you skip the tutorial?";

        public string GiftAchieve_SwedishNeutrality_Name => "Swedish Neutrality";
        public string GiftAchieve_SwedishNeutrality_Desc => "Why take sides?";

        public string GiftAchieve_TroubleMaker_Name => "Troublemaker";
        public string GiftAchieve_TroubleMaker_Desc => "My mom warned me about you."; // "Just want to watch the world burn."

        public string GiftAchieve_ScorchedEarth_Name => "Scorched Earth";
        public string GiftAchieve_ScorchedEarth_Desc => "Nothing but rubble is left behind.";

        public string GiftAchieve_WarMonger_Name => "Warmonger";
        public string GiftAchieve_WarMonger_Desc => "Violence is the only answer!";

        public string GiftAchieve_LivingInABubble_Name => "Living in a Bubble";
        public string GiftAchieve_LivingInABubble_Desc => "Completely oblivious to the world around them.";

        public string GiftAchieve_Bully_Name => "Bully";
        public string GiftAchieve_Bully_Desc => "Pick on someone your own size!";

        public string GiftAchieve_ControlFreak_Name => "Control Freak";
        public string GiftAchieve_ControlFreak_Desc => "Why leave anything to chance?";

        public string GiftAchieve_RandomNothingMakesSense_Name => "Agent of Chaos";
        public string GiftAchieve_RandomNothingMakesSense_Desc => "How do you defy all logic and reason?";

        public string GiftAchieve_Hoarder_Name => "Hoarder";
        public string GiftAchieve_Hoarder_Desc => "Going to keep all those resources to yourself?";

        public string GiftAchieve_Scatterbrained_Name => "Scatterbrained";
        public string GiftAchieve_Scatterbrained_Desc => "Focus, please!";

        public string GiftAchieve_NearSighted_Name => "Nearsighted";
        public string GiftAchieve_NearSighted_Desc => "Only sees what's in front of their nose.";

        public string GiftAchieve_AutomationAbuser_Name => "Automation Abuser";
        public string GiftAchieve_AutomationAbuser_Desc => "Your game plays itself...";

        public string GiftAchieve_Troll_Name => "Troll";
        public string GiftAchieve_Troll_Desc => "Anything for the LOLs.";

        public string GiftAchieve_MemeLord_Name => "Meme Lord";
        public string GiftAchieve_MemeLord_Desc => "Do you know how to talk outside of the internet?";

        public string GiftAchieve_SupportSlave_Name => "Support Slave";
        public string GiftAchieve_SupportSlave_Desc => "It's hard work to carry others.";

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
        public string GiftAchieve_ArmchairGeneral_Desc => "The Logistics Mastermind (of Snacks).";

        public string GiftAchieve_Salty_Name => "Salty";
        public string GiftAchieve_Salty_Desc => "Boiling with rage.";

        public string GiftAchieve_SaltMiner_Name => "Salt Miner";
        public string GiftAchieve_SaltMiner_Desc => "Other people's rage is your reward.";

        public string GiftAchieve_PuppetMaster_Name => "Puppet Master";
        public string GiftAchieve_PuppetMaster_Desc => "Did you pull the strings?";

        public string GiftAchieve_TheCarry_Name => "The Carry";
        public string GiftAchieve_TheCarry_Desc => "Someone dragged his team over the finish line.";

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
        public string GiftAchieve_OnLifeSupport_Desc => "You wouldn't be alive if it weren't for...";

        public string GiftAchieve_LoneWolf_Name => "Lone Wolf";
        public string GiftAchieve_LoneWolf_Desc => "Strongest when the pack is gone, right?";

        public string GiftAchieve_ShaggyTooDopeAlwaysChilling_Name => "Always Chilling";
        public string GiftAchieve_ShaggyTooDopeAlwaysChilling_Desc => "Never breaking a sweat.";

        public string GiftAchieve_BadInfluence_Name => "Bad Influence";
        public string GiftAchieve_BadInfluence_Desc => "Maybe I shouldn't listen to you.";

        public string GiftAchieve_HindsightTactician_Name => "Hindsight Tactician";
        public string GiftAchieve_HindsightTactician_Desc => "Untouched by the fog of war, viewing past chaos through the absolute clarity of the future.";

        public string GiftAchieve_Houseplant_Name => "A Decorative Houseplant";
        public string GiftAchieve_Houseplant_Desc => "Hello? Are you there?";

        public string GiftAchieve_Sheep_Name => "Herd Mentality";
        public string GiftAchieve_Sheep_Desc => "You are allowed your own opinion.";

        public string GiftAchieve_GlitchRider_Name => "Glitch Rider";
        public string GiftAchieve_GlichRider_Desc => "Stop abusing the game!";

        public string GiftAchieve_ChickenShit_Name => "Chicken Shit";
        public string GiftAchieve_ChickenShit_Desc => "Are you afraid?";

        public string GiftAchieve_TheLeakyCanteen_Name => "The Leaky Canteen";
        public string GiftAchieve_TheLeakyCanteen_Desc => "So dry, not a drop of water!";

        

    }
}