using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.PJ;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Italian : AbsLanguage
    {
       
        //Winter update
        public override string Resource_StockpileLimit => "Limite scorte";
        public override string GameMode_QuickMatch => "Quick Match";
        public override string GameMode_QuickMatch_Description =>
            "Un formato di partita più breve. Entra in una guerra su larga scala contro nazioni rivali.";
        public override string Lobby_PlayerCount => "Numero giocatori";
        public override string Lobby_TwoTeams => "Due squadre";
        public override string Hud_Produce => "Produci:";
        public override string Tutorial_WaitForWorkerLevel => "Aspetta che un lavoratore raggiunga:";

        public override string Tutorial_PracticeOrSchool => "Allenati su {0}, oppure usa una {1}";
        public override string Tutorial_AddTag => "Aggiungi tag:";
        public override string Tutorial_AddPin => "Aggiungi pin:";
        public override string Tutorial_SelectMostTrees => "Trova la tua città con più alberi";
        public override string Tutorial_SelectACityWithX => "Seleziona una città con {0}";

        public override string Tutorial_Select_NotCapital => ". Non la tua capitale.";

        public override string Tutorial_SetXPriorityToY => "Imposta la priorità di {0} su {1}";
        public override string Tutorial_AdvisorMission => "Missione Advisor";

        public override string Tutorial_AdvisorDescription =>
            "Il gioco completo è iniziato. L’Advisor estenderà il tutorial con missioni utili.";

        public override string Tutorial_EndAdvisor => "Termina Advisor";

        public override string Tutorial_AdvisorCompleteTitle => "Advisor completato!";
        public override string Tutorial_AdvisorCompleteMessage => "Che il tuo prossimo giorno sia benedetto!";

        public override string Hud_Search => "Cerca";

        public override string DifficultyDescription_ExtremeAggression => "Aggressività estrema";

        public override string MapFilter => "Filtro mappa";

        public override string Settings_TechMultiplier => "Velocità ricerca tech";

        public override string EndScreen_MatchComplete => "Risultato partita";

        public override string FactionName_DragonGem => "Dragon Gem";
        public override string FactionName_Tomten => "Tomten";
        public override string FactionName_Hælfolc => "Hælfolc";
        public override string FactionName_AerimAngren => "Aerim Angren";

        public override string HUD_NotAvailbleInX => "Non disponibile in {0}";

        public override string InputActionName_MiniMap => "Mini-map";

        //--
        public override string Error_SoundInitFailure => "Inizializzazione del suono non riuscita";

        public override string GameMenu_ControllerDisconnected => "Controller disconnesso";

        public override string Tutorial_HighPriority => "I tuoi uomini completeranno prima i compiti con priorità alta.";

        public override string BuildingType_Wall_Description => "Le mura proteggono le tue truppe dagli attacchi e danno un piccolo boost all’attacco.";

        public override string BuildingType_Wall_Siege => "Le armi d’assedio riducono la difesa delle mura.";

        public override string Conscript_BlockChance => "{0}% di probabilità di bloccare un attacco.";

        public override string Battle_DeclarWarReminder => "Devi dichiarare guerra prima di attaccare.";

        //--


        /// <summary>
        /// Name of this language
        /// </summary>
        public override string MyLanguage => "Italiano";

        /// <summary>
        /// How to display a number of items. 0: item, 1:Number
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}:{1}";

        /// <summary>
        /// Select language option
        /// </summary>
        public override string Lobby_Language => "Lingua";

        /// <summary>
        /// Start playing the game
        /// </summary>
        public override string Lobby_Start => "AVVIA";

        /// <summary>
        /// Button to select local mutiplayer count, 0:current player count
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Multigiocatore locale";

        /// <summary>
        /// Title for menu where you select split screen player count
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Seleziona numero giocatori";

        /// <summary>
        /// Description for local multiplayer
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "Il multigiocatore richiede controller Xbox";

        /// <summary>
        /// Move to next split screen position
        /// </summary>
        public override string Lobby_NextScreen => "Posizione schermo successiva";

        /// <summary>
        /// Players can select visual appearance and store them in a profile
        /// </summary>
        public override string Lobby_FlagSelectTitle => "Seleziona bandiera";

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Bandiera {0}";

        /// <summary>
        /// Game name and version number
        /// </summary>
        public override string Lobby_GameVersion => "DSS war party - ver {0}";

        public override string FlagEditor_Description => "Dipingi la bandiera e scegli i colori per i tuoi soldati.";

        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string FlagEditor_Bucket => "Secchiello";

        /// <summary>
        /// Opens flag profile editor
        /// </summary>
        public override string Lobby_FlagEdit => "Modifica bandiera";


        public override string Lobby_WarningTitle => "Avviso";
        public override string Lobby_IgnoreWarning => "Ignora avviso";

        /// <summary>
        /// Warning when one player has no input selected.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "Un giocatore non ha input";

        /// <summary>
        /// Menu with content that are outside what most players will use.
        /// </summary>
        public override string Lobby_Extra => "Extra";

        /// <summary>
        /// The extra content is not translated or have full controller support.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "Attenzione! Questo contenuto non è coperto dalla localizzazione o dal supporto input/accessibilità previsto";


        public override string Lobby_MapSizeTitle => "Dimensione mappa";

        /// <summary>
        /// Map size 1 name
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "Minuscola";

        /// <summary>
        /// Map size 2 name
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "Piccola";

        /// <summary>
        /// Map size 3 name
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "Media";

        /// <summary>
        /// Map size 4 name
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "Grande";

        /// <summary>
        /// Map size 5 name
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "Enorme";

        /// <summary>
        /// Map size 6 name
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "Epica";

        /// <summary>
        /// Map size description X by Y kilometers. 0: Width, 1: Height
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";
        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => "Esci";

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => "Giocatore {0}";

        /// <summary>
        /// In player profile editor. Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Opzioni";

        /// <summary>
        /// In player profile editor. Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Colori bandiera";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "Colore principale";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Colore dettaglio 1";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Colore dettaglio 2";

        /// <summary>
        /// In player profile editor. Title for selecting you soldiers colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "Popolo";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "Colore pelle";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "Colore capelli";

        /// <summary>
        /// In player profile editor. Open color palette and select color
        /// </summary>
        public override string ProfileEditor_PickColor => "Scegli colore";

        /// <summary>
        /// In player profile editor. Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "Sposta immagine";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Sinistra";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Destra";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Su";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Giù";

        /// <summary>
        /// In player profile editor. Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Annulla e esci";

        /// <summary>
        /// In player profile editor. Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Annulla tutte le modifiche";

        /// <summary>
        /// In player profile editor. Save changes and close editor
        /// </summary>
        public override string Hud_SaveAndExit => "Salva e esci";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Hue => "Tonalità";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Lightness => "Luminosità";

        /// <summary>
        /// In player profile editor. Move between flag and soldier color options.
        /// </summary>
        public override string ProfileEditor_NextColorType => "Prossimo tipo di colore";

        /// <summary>
        /// Current running speed of the game, compared to real time
        /// </summary>
        public override string Hud_GameSpeedLabel => "Velocità di gioco: {0}x";

        public override string Input_GameSpeed => "Velocità di gioco";

        /// <summary>
        /// Ingame display. Unit gold production
        /// </summary>
        public override string Hud_TotalIncome => "Entrate totali/secondo: {0}";

        /// <summary>
        /// Unit gold cost.
        /// </summary>
        public override string Hud_Upkeep => "Mantenimento: {0}";
        public override string Hud_ArmyUpkeep => "Mantenimento esercito: {0}";

        /// <summary>
        /// Ingame display. Soldiers protecting a building.
        /// </summary>
        public override string Hud_GuardCount => "Guardie";

        public override string Hud_IncreaseMaxGuardCount => "Dimensione massima guardie {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "Devi espandere la città.";

        public override string Hud_SoldierCount => "Numero soldati: {0}";

        public override string Hud_SoldierGroupsCount => "Numero gruppi: {0}";

        /// <summary>
        /// Ingame display. Unit caculated battle strength.
        /// </summary>
        public override string Hud_StrengthRating => "Valutazione forza: {0}";

        /// <summary>
        /// Ingame display. Caculated battle strength for the whole nation.
        /// </summary>
        public override string Hud_TotalStrengthRating => "Forza militare: {0}";

        /// <summary>
        /// Ingame display. Extra men coming from outside the city state.
        /// </summary>
        public override string Hud_Immigrants => "Immigrati";


        public override string Hud_CityCount => "Numero città: {0}";
        public override string Hud_ArmyCount => "Numero eserciti: {0}";


        /// <summary>
        /// Mini button to repeat a purchase a number of times. E.G. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Requisito";
        public override string Hud_PurchaseTitle_Cost => "Costo";
        public override string Hud_PurchaseTitle_Gain => "Guadagno";

        /// <summary>
        /// How much of a resource that will be used, "5oro.(Available:10)". There will be a "costo" title above the text. 0: Resource, 1: cost, 2: available
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Disponibile: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "Il costo aumenterà di {0}";

        public override string Hud_Purchase_MaxCapacity => "Ha raggiunto la capacità massima";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Forza: Tua {0} - Loro {1}";

        /// <summary>
        /// Display a short string of date as Year, Month, Day
        /// </summary>
        public override string Hud_Date => "A{0} M{1} G{2}";
        
        /// <summary>
        /// Display a short string of timespan as Hour, Minutes, Seconds
        /// </summary>
        public override string Hud_TimeSpan => "O{0} M{1} S{2}";

        /// <summary>
        /// Battle between two armies, or army and city
        /// </summary>
        public override string Hud_Battle => "Battaglia";



        /// <summary>
        /// Describes button input. Pause.
        /// </summary>
        public override string Input_Pause => "Pausa";

        /// <summary>
        /// Describes button input. Resume from paused.
        /// </summary>
        public override string Input_ResumePaused => "Riprendi";

        /// <summary>
        /// Generic money resource
        /// </summary>
        public override string ResourceType_Gold => "Oro";

        /// <summary>
        /// Working men resource
        /// </summary>
        public override string ResourceType_Workers => "Lavoratori";


        public override string ResourceType_Workers_Description => "I lavoratori forniscono entrate e possono essere arruolati come soldati per i tuoi eserciti";

        /// <summary>
        /// The resource used in diplomacy
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Punti diplomazia";

        /// <summary>
        /// 0: How many points you got, 1: Soft max value (will increase much slower after this), 2: Hard limit
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Punti diplomatici: {0} / {1} ({2})";

        /// <summary>
        /// City building type. Building for knights and diplomats.
        /// </summary>
        public override string Building_NobleHouse => "Casa nobiliare";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "1 punto diplomazia ogni {0} secondi";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} al limite massimo dei punti diplomazia";
        public override string Building_NobleHouse_UnlocksKnight => "Sblocca unità Cavaliere";

        public override string Building_BuildAction => "Costruisci";
        public override string Building_IsBuilt => "Costruito";

        /// <summary>
        /// City building type. Evil mass production.
        /// </summary>
        public override string Building_DarkFactory => "Fabbrica oscura";

        /// <summary>
        /// In game settings menu. Sums all difficulty options in percentage.
        /// </summary>
        public override string Settings_TotalDifficulty => "Difficoltà totale {0}%";

        /// <summary>
        /// In game settings menu. Base difficulty option.
        /// </summary>
        public override string Settings_DifficultyLevel => "Livello difficoltà {0}%";


        /// <summary>
        ///  In game settings menu. Option for creating new maps instead of loading one. You can load pre-generated maps or create new ones.
        /// </summary>
        public override string Settings_GenerateMaps => "Genera nuove mappe";

        /// <summary>
        ///  In game settings menu.Creating new maps has a longer loading time
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Generare è più lento che caricare le mappe predefinite";

        /// <summary>
        ///  In game settings menu.Difficulty option. Block the ability to play the game while paused.
        /// </summary>
        public override string Settings_AllowPause => "Consenti pausa e comandi";

        /// <summary>
        ///  In game settings menu.Difficulty option. Have bosses that enter the game.
        /// </summary>
        public override string Settings_BossEvents => "Eventi boss";

        /// <summary>
        ///  In game settings menu.Difficulty option. No Boss description.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Disattivare gli eventi boss mette il gioco in modalità sandbox senza finale.";


        /// <summary>
        /// Options for automating game mechanics. Menu title.
        /// </summary>
        public override string Automation_Title => "Automazione";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Attenderà che la forza lavoro raggiunga il massimo";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Metterà in pausa se le entrate sono negative";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_Priority => "Le città grandi hanno priorità";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Esegue al massimo un acquisto al secondo";


        /// <summary>
        /// Button caption for action. A specialized building for knights and diplomats.
        /// </summary>
        public override string HudAction_BuyItem => "Compra {0}";

        /// <summary>
        /// The state of peace or war between two nations
        /// </summary>
        public override string Diplomacy_RelationType => "Relazione";

        /// <summary>
        /// Titel for list of relations other factions have with eachother
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Le loro relazioni con gli altri";

        /// <summary>
        /// Diplomatic relation. You are in direct control over the nations resources.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Servitore";

        /// <summary>
        /// Diplomatic relation. Full co-operation.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Alleato";

        /// <summary>
        /// Diplomatic relation. Reduced chance of war.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "Buona";

        /// <summary>
        /// Diplomatic relation. Peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Pace";

        /// <summary>
        /// Diplomatic relation. Have not yet made any contact.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Neutrale";
        /// <summary>
        /// Diplomatic relation. Temporary peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Tregua";
        /// <summary>
        /// Diplomatic relation. War.
        /// </summary>
        public override string Diplomacy_RelationType_War => "Guerra";
        /// <summary>
        /// Diplomatic relation. War with no chance of peace.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Guerra totale";

        /// <summary>
        /// Diplomatic communication. How well you can discuss terms. 0: SpeakTerms
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "Dialogo: {0}";

        /// <summary>
        /// Diplomatic communication. Better than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "Buona";

        /// <summary>
        /// Diplomatic communication. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Normale";

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Scarsa";

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "Nessuno";

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Forgerelazionia:{0}";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Offri pace";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Offri alleanza";

        /// <summary>
        /// Diplomatic title. Another player Suggested a new diplomatic relation. 0: player name
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} propone nuove relazioni";

        /// <summary>
        /// Diplomatic action. Accept new diplomatic relation.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Accetta nuova relazione";

        /// <summary>
        /// Diplomatic description. Another player Suggested a new diplomatic relation. 0: relation type
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "Nuova relazione proposta: {0}";

        /// <summary>
        /// Diplomatic action. Make another nation to serve you.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Assorbi come servitore";

        /// <summary>
        /// Diplomatic description. Is against evil.
        /// </summary>
        public override string Diplomacy_LightSide => "È alleato della Luce";

        /// <summary>
        /// Diplomatic description. How long the truce will last.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "Termina tra {0} secondi";

        /// <summary>
        /// Diplomatic action. Make the truce last longer.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Estendi tregua";

        /// <summary>
        /// Diplomatic description. How long the truce will be extended.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Estende la tregua di {0} secondi";

        /// <summary>
        /// Diplomatic description. Going against an agreed relation will cost diplomatic points.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "Rompere la relazione costerà {0} punti diplomazia";

        /// <summary>
        /// Diplomatic description for allies.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Gli alleati condividono le dichiarazioni di guerra.";

        /// <summary>
        /// Diplomatic description for good relation.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Limita la possibilità di dichiarare guerra.";

        /// <summary>
        /// Diplomatic description. You must have a larger military force than your servant (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "Potenza militare {0}x più forte";

        /// <summary>
        /// Diplomatic description. Servant must be stuck in a hopeless war (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "Il servitore deve essere in guerra contro un nemico più forte";

        /// <summary>
        /// Diplomatic description. A servant can't own too many cities (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "Il servitore può avere al massimo {0} città";

        /// <summary>
        /// Diplomatic description. Const in diplomatic points will increase (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "Il prezzo aumenterà per ogni servitore";

        /// <summary>
        /// Diplomatic description. The result of servant relation, peaceful take over of another nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Assorbi l'altra fazione";

        /// <summary>
        /// Messaage when you recieve a war declaration
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "Guerra dichiarata!";

        /// <summary>
        /// The truce timer har run out, and you go back to war
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "La tregua è terminata";

        /// <summary>
        /// Stats that are shown on the end game screen. Display title.
        /// </summary>
        public override string EndGameStatistics_Title => "Statistiche";
        /// <summary>
        /// Stats that are shown on the end game screen. Total ingame time passed.
        /// </summary>
        public override string EndGameStatistics_Time => "Tempo di gioco: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. How many soldiers you bought.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "Soldati reclutati: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that died in battle.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Soldati persi in battaglia: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of opponent soldiers you killed in battle.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Soldati nemici uccisi in battaglia: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that have left you.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Soldati disertati: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities won in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "Città conquistate: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities lost in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "Città perse: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle win results.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Battaglie vinte: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle lost results.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Battaglie perse: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Diplomacy. War declarations made by you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "Dichiarazioni di guerra fatte: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen.  Diplomacy. War declarations made toward you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "Dichiarazioni di guerra ricevute: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Allies made through diplomacy.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Alleanze diplomatiche: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Servants made through diplomacy. Servants cities and armies become yours.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Servitori diplomatici: {0}";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_Army => "Esercito";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_SoldierGroup => "Gruppo";

        /// <summary>
        /// Collective unit type on the map. Common name for village or city.
        /// </summary>
        public override string UnitType_City => "Città";

        /// <summary>
        /// A group selection of armies
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Gruppo d’eserciti, numero: {0}";

        /// <summary>
        /// Name for a specialized type of soldier. Standard front line soldier.
        /// </summary>
        public override string UnitType_Soldier => "Soldato";

        /// <summary>
        /// Name for a specialized type of soldier. Naval battle soldier.
        /// </summary>
        public override string UnitType_Sailor => "Marinaio";

        /// <summary>
        /// Name for a specialized type of soldier. Drafted peasants.
        /// </summary>
        public override string UnitType_Folkman => "Contadino armato";

        /// <summary>
        /// Name for a specialized type of soldier. Shield and spear unit.
        /// </summary>
        public override string UnitType_Spearman => "Lanciere";

        /// <summary>
        /// Name for a specialized type of soldier. Elite force, part of the Kings guard.
        /// </summary>
        public override string UnitType_HonorGuard => "Guardia d’onore";

        /// <summary>
        /// Name for a specialized type of soldier. Anti cavalry, wears long two-handed spears.
        /// </summary>
        public override string UnitType_Pikeman => "Picchiere";

        /// <summary>
        /// Name for a specialized type of soldier. Armored cavalry unit.
        /// </summary>
        public override string UnitType_Knight => "Cavaliere";

        /// <summary>
        /// Name for a specialized type of soldier. Bow and arrow.
        /// </summary>
        public override string UnitType_Archer => "Arciere";

        /// <summary>
        /// Name for a specialized type of soldier. 
        /// </summary>
        public override string UnitType_Crossbow => "Balestriere";

        /// <summary>
        /// Name for a specialized type of soldier. Warmashine that slings large spears.
        /// </summary>
        public override string UnitType_Ballista => "Balista";

        /// <summary>
        /// Name for a specialized type of soldier. A fantasy troll wearing a cannon.
        /// </summary>
        public override string UnitType_Trollcannon => "Cannone troll";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier from the forest.
        /// </summary>
        public override string UnitType_GreenSoldier => "Soldato verde";

        /// <summary>
        /// Name for a specialized type of soldier. Naval unit from the north.
        /// </summary>
        public override string UnitType_Viking => "Vichingo";

        /// <summary>
        /// Name for a specialized type of soldier. The evil master boss.
        /// </summary>
        public override string UnitType_DarkLord => "Oscuro Signore";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier that carries a large flag.
        /// </summary>
        public override string UnitType_Bannerman => "Alfiere";

        /// <summary>
        /// Name for a military unit. Soldier carrying ship. 0: unit type it carries
        /// </summary>
        public override string UnitType_WarshipWithUnit => "Nave da guerra {0}";

        public override string UnitType_Description_Soldier => "Unità versatile.";
        public override string UnitType_Description_Sailor => "Forte nelle battaglie navali";
        public override string UnitType_Description_Folkman => "Soldati economici e non addestrati";
        public override string UnitType_Description_HonorGuard => "Soldati élite senza mantenimento";
        public override string UnitType_Description_Knight => "Forte in campo aperto";
        public override string UnitType_Description_Archer => "Forte solo se protetto.";
        public override string UnitType_Description_Crossbow => "Unità a distanza potente";
        public override string UnitType_Description_Ballista => "Forte contro le città";
        public override string UnitType_Description_GreenSoldier => "Temuto guerriero elfico";

        public override string UnitType_Description_DarkLord => "Il boss finale";

        /// <summary>
        /// Information about a soldier type
        /// </summary>
        public override string SoldierStats_Title => "Statistiche per unità";

        /// <summary>
        /// How many groups of soldiers
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} gruppi, per un totale di {1} unità";

        /// <summary>
        /// Soldiers will have different strengths depending if the attack on open field, from ships or attacking a settlement
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Forza d’attacco: Terra {0} | Mare {1} | Città {2}";

        /// <summary>
        /// How many wounds a soldier can endure
        /// </summary>
        public override string SoldierStats_Health => "Salute: {0}";

        /// <summary>
        /// Some soldiers will increase the army movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Bonus velocità esercito su terra: {0}";

        /// <summary>
        /// Some soldiers will increase the ship movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Bonus velocità esercito su mare: {0}";

        /// <summary>
        /// Purchased soliders will start as recruits and complete their training after a few minutes.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Tempo di addestramento: {0} minuti. Sarà dimezzato se le reclute sono adiacenti a una città.";

        /// <summary>
        /// Menu option to control an army. Make them stop moving.
        /// </summary>
        public override string ArmyOption_Halt => "Alt";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_Disband => "Sciogli unità";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_Divide => "Dividi esercito";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_RemoveX => "Rimuovi {0}";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Sciogli tutto";

        /// <summary>
        /// Menu option to control an army. 0: Count, 1: Unit type
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "Gruppi {1}: {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToX => "Invia unità a {0}";

        public override string ArmyOption_MergeAllArmies => "Unisci tutti gli eserciti";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Dividi unità in un nuovo esercito";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendX => "Invia {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendAll => "Invia tutto";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Dividi l’esercito a metà";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Unisci eserciti";



        /// <summary>
        /// Purchase soldiers.
        /// </summary>
        public override string UnitType_Recruit => "Recluta";

        /// <summary>
        /// Purchase soldiers of type. 0:type
        /// </summary>
        public override string CityOption_RecruitType => "Recluta {0}";

        /// <summary>
        /// Number of paid soldiers
        /// </summary>
        public override string CityOption_XMercenaries => "Mercenari: {0}";


        /// <summary>
        /// Indicates the number of mercenaries currently available for hire from the market
        /// </summary>
        public override string Hud_MercenaryMarket => "Mercenari sul mercato";

        /// <summary>
        /// Purchase a number of paid soldiers
        /// </summary>
        public override string CityOption_BuyXMercenaries => "Assolda {0} mercenari";

        public override string CityOption_Mercenaries_Description => "I soldati verranno arruolati dai mercenari anziché dalla forza lavoro";

        /// <summary>
        /// Button caption for action. Create housing for more workers.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "Espandi forza lavoro";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "Forza lavoro max +{0}";
        public override string CityOption_ExpandGuardSize => "Espandi guardia";

        public override string CityOption_Damages => "Danni: {0}";
        public override string CityOption_Repair => "Ripara danni";
        public override string CityOption_RepairGain => "Ripara {0} danni";

        public override string CityOption_Repair_Description => "I danni riducono il numero di lavoratori ospitabili.";


        public override string CityOption_BurnItDown => "Dai fuoco";
        public override string CityOption_BurnItDown_Description => "Rimuovi la forza lavoro e applica danni massimi";

        /// <summary>
        /// The main boss. Named after a glowing metal stone stuck in their forehead.
        /// </summary>
        public override string FactionName_DarkLord => "Occhio della Rovina";

        /// <summary>
        /// Orc inspired faction. Works for the dark lord.
        /// </summary>
        public override string FactionName_DarkFollower => "Servi del Terrore";

        /// <summary>
        /// The largest faction, the old but corrupted kingdom.
        /// </summary>
        public override string FactionName_UnitedKingdom => "Regni Uniti";

        /// <summary>
        /// Elf inspired faction. Lives in harmony with the forest.
        /// </summary>
        public override string FactionName_Greenwood => "Boscoverde";

        /// <summary>
        /// Asian flavored faction to the east 
        /// </summary>
        public override string FactionName_EasternEmpire => "Impero dell’Est";

        /// <summary>
        /// Viking flavored kingdom in the north. The largest one.
        /// </summary>
        public override string FactionName_NordicRealm => "Reami Nordici";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a bear claw symbol.
        /// </summary>
        public override string FactionName_BearClaw => "Artiglio d’orso";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a cock symbol.
        /// </summary>
        public override string FactionName_NordicSpur => "Sperone nordico";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a black raven symbol.
        /// </summary>
        public override string FactionName_IceRaven => "Corvo del Ghiaccio";

        /// <summary>
        /// Faction famous for killing dragons with powerful ballistas.
        /// </summary>
        public override string FactionName_Dragonslayer => "Ammazzadraghi";

        /// <summary>
        /// A mercenary unit from the south. Arabic flavored.
        /// </summary>
        public override string FactionName_SouthHara => "Hara del Sud";

        /// <summary>
        /// Name for neutral CPU controlled nations
        /// </summary>
        public override string FactionName_GenericAi => "IA {0}";

        /// <summary>
        /// Display name for players and their numbers
        /// </summary>
        public override string FactionName_Player => "Giocatore {0}";

        /// <summary>
        /// Message for when a miniboss is approaching on ships from the south.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "Nemico in avvicinamento!";
        public override string EventMessage_HaraMercenaryText => "Mercenari Hara avvistati a sud";

        /// <summary>
        /// First warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "Una profezia oscura";
        public override string EventMessage_ProphesyText => "L’Occhio della Rovina apparirà presto e i tuoi nemici si uniranno a lui!";

        /// <summary>
        /// Second warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Tempi oscuri";
        public override string EventMessage_FinalBossEnterText => "L’Occhio della Rovina è entrato sulla mappa!";

        /// <summary>
        /// Message when the main boss will meet you on the battlefield.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "Un attacco disperato";
        public override string EventMessage_FinalBattleText => "L’oscuro signore è sceso in battaglia. Ora è la tua occasione per distruggerlo!";

        /// <summary>
        /// Message when soldiers leave the army when you can't pay thier upkeep
        /// </summary>
        public override string EventMessage_DesertersTitle => "Disertori!";
        public override string EventMessage_DesertersText_Money => "Soldati non pagati stanno disertando dai tuoi eserciti";

        public override string DifficultyDescription_AiAggression => "Aggressività IA: {0}.";
        public override string DifficultyDescription_BossSize => "Dimensione boss: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Tempo di ingresso boss: {0}.";
        public override string DifficultyDescription_AiEconomy => "Economia IA: {0}%.";
        public override string DifficultyDescription_AiDelay => "Ritardo IA: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Difficoltà diplomatica: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Costo mercenari: {0}.";
        public override string DifficultyDescription_HonorGuards => "Guardie d’onore: {0}.";


        /// <summary>
        /// Game has ended in success.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Vittoria!";

        /// <summary>
        /// Citazioni del leader che interpreti nel gioco
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
{
    "In tempi di pace, piangiamo i morti.",
    "Ogni trionfo porta con sé l’ombra del sacrificio.",
    "Ricordiamo il cammino che ci ha condotti fin qui, costellato dalle anime dei coraggiosi.",
    "Le nostre menti brillano per la vittoria, ma i nostri cuori sono gravati dal peso dei caduti."
};

        public override string EndScreen_DominationVictoryQuote => "Sono stato scelto dagli Dei per dominare il mondo!";

        /// <summary>
        /// Il gioco è terminato con una sconfitta.
        /// </summary>
        public override string EndScreen_FailTitle => "Sconfitta!";

        /// <summary>
        /// Citazioni del leader che interpreti nel gioco
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
{
    "Con i nostri corpi logorati da notti di marcia e di preoccupazione, accogliamo la fine.",
    "La sconfitta può oscurare le nostre terre, ma non potrà spegnere la luce della nostra determinazione.",
    "Spente le fiamme nei nostri cuori, dalle loro ceneri i nostri figli forgeranno una nuova alba.",
    "Che i nostri racconti siano la brace che accende la vittoria di domani."
};


        /// <summary>
        /// A small cutscene at the end of the game
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Guarda epilogo";

        /// <summary>
        /// Cutscene title
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Epilogo";

        /// <summary>
        /// Cutscene introduction
        /// </summary>
        public override string EndScreen_Epilogue_Text => "160 anni fa";

        /// <summary>
        /// The Prologue is a short poem about the game's stroy
        /// </summary>
        public override string GameMenu_WatchPrologue => "Guarda prologo";

        public override string Prologue_Title => "Prologo";

        /// <summary>
        /// La poesia deve avere tre versi; il quarto verrà aggiunto con il nome del boss.
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
{
    "I sogni ti tormentano nella notte,",
    "Una profezia annuncia un oscuro futuro,",
    "Preparati al suo arrivo,"
};


        /// <summary>
        /// Ingame menu when pausing
        /// </summary>
        public override string GameMenu_Title => "Menu di gioco";

        /// <summary>
        /// Continue playing the game after end screen
        /// </summary>
        public override string GameMenu_ContinueGame => "Continua";

        /// <summary>
        /// Continue playing the game
        /// </summary>
        public override string GameMenu_Resume => "Riprendi";

        /// <summary>
        /// Exit to game lobby
        /// </summary>
        public override string GameMenu_ExitGame => "Esci dal gioco";

        public override string Hud_Save => "Salva";
        public override string GameMenu_SaveStateWarnings => "Attenzione! I salvataggi andranno persi quando il gioco verrà aggiornato.";
        public override string GameMenu_LoadState => "Carica";
        public override string GameMenu_ContinueFromSave => "Continua dal salvataggio";

        public override string GameMenu_AutoSave => "Salvataggio automatico";

        public override string GameMenu_Load_PlayerCountError => "Devi impostare un numero di giocatori corrispondente al salvataggio: {0}";

        public override string Progressbar_MapLoadingState => "Caricamento mappa: {0}";

        public override string Progressbar_ProgressComplete => "completo";

        /// <summary>
        /// 0: progress in percentage, 1: fail count
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Generazione: {0}%. (Errori {1})";


        /// <summary>
        /// 0: current part, 1: number of parts
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "parte {0}/{1}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_SaveProgress => "Salvataggio: {0}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_LoadProgress => "Caricamento: {0}";

        /// <summary>
        /// Progress done, waiting for player input
        /// </summary>
        public override string Progressbar_PressAnyKey => "Premi un tasto per continuare";


        /// <summary>
        /// A short tutorial where you are supposed to buy and move a soldier. All advanced controls are locked away until the tutorial is complete.
        /// </summary>
        public override string Tutorial_MenuOption => "Avvia tutorial";
        public override string Tutorial_MissionsTitle => "Missioni tutorial";
        public override string Tutorial_Mission_BuySoldier => "Seleziona una città e recluta un soldato";
        public override string Tutorial_Mission_MoveArmy => "Seleziona un esercito e spostalo";

        public override string Tutorial_CompleteTitle => "Tutorial completato!";
        public override string Tutorial_CompleteMessage => "Sbloccati zoom completo e opzioni avanzate.";

        /// <summary>
        /// Displays the button input
        /// </summary>
        public override string Tutorial_SelectInput => "Seleziona";
        public override string Tutorial_MoveInput => "Comando di spostamento";


        
        /// <summary>
        /// Versus. Text describing the two armies that will go into battle
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "Dichiarazione di guerra";

        public override string ArmyOption_Attack => "Attacca";



        //----
        /// <summary>
        /// In game settings menu. Change what keys and buttons do when pressed
        /// </summary>
        public override string Settings_ButtonMapping => "Associazione tasti";

       

        /// <summary>
        /// Input type, standard PC input
        /// </summary>
        public override string Input_Source_Keyboard => "Tastiera e mouse";

        /// <summary>
        /// Input type, handheld controller like the xbox uses
        /// </summary>
        public override string Input_Source_Controller => "Controller";


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */
        public override string CityMenu_SalePricesTitle => "Prezzi di vendita";
        public override string Blueprint_Title => "Progetto";

        public override string Resource_Tab_Overview => "Panoramica";
        public override string Resource_Tab_Stockpile => "Scorta";

        public override string Resource => "Risorsa";

        public override string Resource_StockPile_Info =>
            "Imposta una quantità obiettivo per lo stoccaggio delle risorse; informerà i lavoratori su quando passare ad altre risorse.";

        public override string Resource_TypeName_Water => "Acqua";
        public override string Resource_TypeName_Wood => "Legno";
        public override string Resource_TypeName_Fuel => "Combustibile";
        public override string Resource_TypeName_Stone => "Pietra";
        public override string Resource_TypeName_RawFood => "Cibo grezzo";
        public override string Resource_TypeName_Food => "Cibo";
        public override string Resource_TypeName_Beer => "Birra";
        public override string Resource_TypeName_Wheat => "Grano";
        public override string Resource_TypeName_Linen => "Lino";
        //public override string Resource_TypeName_SkinAndLinen => "Pelle e lino";
        public override string Resource_TypeName_IronOre => "Minerale di ferro";
        public override string Resource_TypeName_GoldOre => "Minerale d’oro";
        public override string Resource_TypeName_Iron => "Ferro";

        public override string Resource_TypeName_SharpStick => "Bastone appuntito";
        public override string Resource_TypeName_Sword => "Spada";
        public override string Resource_TypeName_KnightsLance => "Lancia da cavaliere";
        public override string Resource_TypeName_TwoHandSword => "Zweihänder";
        public override string Resource_TypeName_Bow => "Arco";


        public override string Resource_TypeName_LightArmor => "Armatura leggera";
        public override string Resource_TypeName_MediumArmor => "Armatura media";
        public override string Resource_TypeName_HeavyArmor => "Armatura pesante";

        public override string ResourceType_Children => "Bambini";

        public override string BuildingType_DefaultName => "Edificio";
        public override string BuildingType_WorkerHut => "Capanna dei lavoratori";
        public override string BuildingType_Brewery => "Birrificio";
        public override string BuildingType_Postal => "Servizio postale";
        public override string BuildingType_Recruitment => "Centro reclutamento";
        public override string BuildingType_Barracks => "Caserma";
        public override string BuildingType_PigPen => "Porcile";
        public override string BuildingType_HenPen => "Pollaio";
        public override string BuildingType_WorkBench => "Banco da lavoro";
        public override string BuildingType_Carpenter => "Falegname";
        public override string BuildingType_CoalPit => "Fossa per carbone";
        public override string DecorType_Statue => "Statua";
        public override string DecorType_Pavement => "Pavimentazione";
        public override string BuildingType_Smith => "Fabbro";
        public override string BuildingType_Cook => "Cucina";
        public override string BuildingType_Storage => "Magazzino";

        public override string BuildingType_ResourceFarm => "Fattoria di {0}";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Aumenta il limite di lavoratori di {0}";
        public override string BuildingType_Tavern_Description => "I lavoratori possono mangiare qui";
        public override string BuildingType_Tavern_Brewery => "Produzione di birra";
        public override string BuildingType_Postal_Description => "Invia risorse ad altre città";
        public override string BuildingType_Recruitment_Description => "Invia uomini ad altre città";
        public override string BuildingType_Barracks_Description => "Usa uomini ed equipaggiamento per reclutare soldati";
        public override string BuildingType_PigPen_Description => "Produce maiali, che danno cibo e pelle";
        public override string BuildingType_HenPen_Description => "Produce galline e uova, che danno cibo";
        public override string BuildingType_Decor_Description => "Decorazione";
        public override string BuildingType_Farm_Description => "Coltiva una risorsa";

        public override string BuildingType_Cook_Description => "Stazione di lavorazione del cibo";
        public override string BuildingType_Bench_Description => "Stazione di creazione oggetti";

        public override string BuildingType_Smith_Description => "Stazione di lavorazione metalli";
        public override string BuildingType_Carpenter_Description => "Stazione di lavorazione legno";

        public override string BuildingType_Nobelhouse_Description => "Casa per cavalieri e diplomatici";
        public override string BuildingType_CoalPit_Description => "Produzione di combustibile efficiente";
        public override string BuildingType_Storage_Description => "Punto di consegna delle risorse";

        public override string MenuTab_Info => "Info";
        public override string MenuTab_Work => "Lavoro";
        public override string MenuTab_Recruit => "Recluta";
        public override string MenuTab_Resources => "Risorse";
        public override string MenuTab_Trade => "Commercio";
        public override string MenuTab_Build => "Costruisci";
        public override string MenuTab_Economy => "Economia";
        public override string MenuTab_Delivery => "Consegna";

        public override string MenuTab_Build_Description => "Posiziona edifici nella tua città";
        public override string MenuTab_BlackMarket_Description => "Posiziona edifici nella tua città";
        public override string MenuTab_Resources_Description => "Posiziona edifici nella tua città";
        public override string MenuTab_Work_Description => "Posiziona edifici nella tua città";
        public override string MenuTab_Automation_Description => "Posiziona edifici nella tua città";

        public override string BuildHud_OutsideCity => "Fuori dall’area cittadina";
        public override string BuildHud_OutsideFaction => "Fuori dai tuoi confini!";

        public override string BuildHud_OccupiedTile => "Cella occupata";

        public override string Build_PlaceBuilding => "Edificio";
        public override string Build_DestroyBuilding => "Demolisci";
        public override string Build_ClearTerrain => "Ripulisci terreno";

        public override string Build_ClearOrders => "Cancella ordini di costruzione";
        public override string Build_Order => "Ordine di costruzione";
        public override string Build_OrderQue => "Coda ordini di costruzione: {0}";
        public override string Build_AutoPlace => "Posizionamento automatico";

        public override string Work_OrderPrioTitle => "Priorità lavoro";
        public override string Work_OrderPrioDescription => "La priorità va da 1 (bassa) a {0} (alta)";

        public override string Work_OrderPrio_No => "Nessuna priorità. Non verrà lavorato.";
        public override string Work_OrderPrio_Min => "Priorità minima.";
        public override string Work_OrderPrio_Max => "Priorità massima.";

        public override string Work_Move => "Sposta oggetti";

        public override string Work_GatherXResource => "Raccogli {0}";
        public override string Work_CraftX => "Crea {0}";
        public override string Work_Farming => "Agricoltura";
        public override string Work_Mining => "Estrazione";
        public override string Work_Trading => "Commercio";

        public override string Work_AutoBuild => "Costruzione ed espansione automatiche";

        public override string WorkerHud_WorkType => "Stato lavoro: {0}";
        public override string WorkerHud_Carry => "Trasporta: {0} {1}";
        public override string WorkerHud_Energy => "Energia: {0}";
        public override string WorkerStatus_Exit => "Lascia la forza lavoro";
        public override string WorkerStatus_Eat => "Mangia";
        public override string WorkerStatus_Till => "Aratura";
        public override string WorkerStatus_Plant => "Pianta";
        public override string WorkerStatus_Gather => "Raccogli";
        public override string WorkerStatus_PickUpResource => "Raccogli risorsa";
        public override string WorkerStatus_DropOff => "Deposita";
        public override string WorkerStatus_BuildX => "Costruisci {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Ritorna all’esercito";

        public override string Hud_ToggleFollowFaction => "Segui impostazioni di fazione";
        public override string Hud_FollowFaction_Yes => "Impostato per usare le impostazioni globali di fazione";
        public override string Hud_FollowFaction_No => "Impostato per usare impostazioni locali (valore globale {0})";

        public override string Hud_Idle => "Inattivo";
        public override string Hud_NoLimit => "Nessun limite";

        public override string Hud_None => "Nessuno";
        public override string Hud_ProductionQueue => "Coda produzione";

        public override string Hud_EmptyList => "- Lista vuota -";

        public override string Hud_RequirementOr => "- oppure -";

        public override string Hud_BlackMarket => "Mercato nero";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Seleziona città";
        public override string Conscription_Title => "Coscrizione";
        public override string Conscript_WeaponTitle => "Arma";
        public override string Conscript_ArmorTitle => "Armatura";
        public override string Conscript_TrainingTitle => "Addestramento";
        public override string Conscript_SpecializationTitle => "Specializzazione";
        public override string Conscript_SpecializationDescription => "Aumenta l’attacco in un’area e lo riduce in tutte le altre di {0}.";
        public override string Conscript_SelectBuilding => "Seleziona caserma";

        public override string Conscript_WeaponDamage => "Danno arma: {0}";
        public override string Conscript_ArmorHealth => "Integrità armatura: {0}";
        public override string Conscript_TrainingSpeed => "Velocità d’attacco: {0}";
        public override string Conscript_TrainingTime => "Tempo di addestramento: {0}";

        public override string Conscript_Training_Minimal => "Minimo";
        public override string Conscript_Training_Basic => "Base";
        public override string Conscript_Training_Skillful => "Esperto";
        public override string Conscript_Training_Professional => "Professionale";

        public override string Conscript_Specialization_Field => "Campo aperto";
        public override string Conscript_Specialization_Sea => "Navale";
        public override string Conscript_Specialization_Siege => "Assedio";
        public override string Conscript_Specialization_Traditional => "Tradizionale";
        public override string Conscript_Specialization_AntiCavalry => "Anticavalleria";

        public override string Conscription_Status_CollectingEquipment => "Raccolta equipaggiamento: {0}";
        public override string Conscription_Status_CollectingMen => "Reclutamento uomini: {0}";
        public override string Conscription_Status_Training => "Addestramento: {0}";

        public override string ArmyHud_Food_Reserves_X => "Scorte di cibo: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Mantenimento cibo: {0}";
        public override string ArmyHud_Food_Costs_X => "Costi cibo: {0}";

        public override string Deliver_WillSendXInfo => "Invierà {0} alla volta.";
        public override string Delivery_ListTitle => "Seleziona servizio di consegna";
        public override string Delivery_DistanceX => "Distanza: {0}";
        public override string Delivery_DeliveryTimeX => "Tempo di consegna: {0}";
        public override string Delivery_SenderMinimumCap => "Capacità minima del mittente";
        public override string Delivery_RecieverMaximumCap => "Capacità massima del destinatario";
        public override string Delivery_ItemsReady => "Oggetti pronti";
        public override string Delivery_RecieverReady => "Destinatario pronto";
        public override string Hud_ThisCity => "Questa città";
        public override string Hud_RecieveingCity => "Città destinataria";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "Mostrato in risorse al secondo.";
        public override string Info_MinuteAverage => "Il valore è una media dell’ultimo minuto.";

        public override string Message_OutOfFood_Title => "Cibo esaurito";
        public override string Message_CityOutOfFood_Text =>
            "Il cibo costoso verrà acquistato dal mercato nero. I lavoratori moriranno di fame quando il denaro sarà finito.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Tipo di terreno";

        public override string Hud_EnergyUpkeepX => "Mantenimento energetico: {0}";
        public override string Hud_EnergyAmount => "{0} energia (secondi di lavoro)";

        public override string Hud_CopySetup => "Copia configurazione";
        public override string Hud_Paste => "Incolla";

        public override string Hud_Available => "Disponibile";

        public override string WorkForce_ChildBirthRequirements => "Requisiti per la nascita";
        public override string WorkForce_AvailableHomes => "Case disponibili: {0}";

        public override string WorkForce_Peace => "Pace";
        public override string WorkForce_ChildToManTime => "Età adulta: {0} minuti";

        public override string Economy_TaxIncome => "Entrate fiscali: {0}";
        public override string Economy_ImportCostsForResource => "Costi d’importazione per {0}: {1}";
        public override string Economy_BlackMarketCostsForResource => "Costi del mercato nero per {0}: {1}";
        public override string Economy_GuardUpkeep => "Mantenimento guardie: {0}";

        public override string Economy_LocalCityTrade_Export => "Esportazioni cittadine: {0}";
        public override string Economy_LocalCityTrade_Import => "Importazioni cittadine: {0}";

        public override string Economy_ResourceProduction => "Produzione di {0}: {1}";
        public override string Economy_ResourceSpending => "Spesa di {0}: {1}";

        public override string Economy_TaxDescription => "La tassa è di {0} oro per lavoratore.";
        public override string Economy_SoldResources => "Risorse vendute (oro guadagnato): {0}";

        public override string UnitType_Cities => "Città";
        public override string UnitType_Armies => "Eserciti";
        public override string UnitType_Worker => "Lavoratore";

        public override string UnitType_FootKnight => "Cavaliere a piedi";
        public override string UnitType_CavalryKnight => "Cavaliere a cavallo";

        public override string CityCulture_LargeFamilies => "Grandi famiglie";
        public override string CityCulture_FertileGround => "Terre fertili";
        public override string CityCulture_Archers => "Abili arcieri";
        public override string CityCulture_Warriors => "Guerrieri";
        public override string CityCulture_AnimalBreeder => "Allevatori";
        public override string CityCulture_Miners => "Minatori";
        public override string CityCulture_Woodcutters => "Taglialegna";
        public override string CityCulture_Builders => "Costruttori";

        /// <summary>
        /// Mentalità del granchio: cultura in cui si tende a ostacolare chi è più capace
        /// </summary>
        public override string CityCulture_CrabMentality => "Mentalità del granchio";
        public override string CityCulture_DeepWell => "Pozzo profondo";
        public override string CityCulture_Networker => "Comunicatore";

        /// <summary>
        /// Maestro della fossa: esperto nella produzione di carbone
        /// </summary>
        public override string CityCulture_PitMasters => "Maestri della fossa";

        public override string CityCulture_CultureIsX => "Cultura: {0}";

        public override string CityCulture_LargeFamilies_Description => "Aumenta la natalità.";
        public override string CityCulture_FertileGround_Description => "I raccolti producono di più.";
        public override string CityCulture_Archers_Description => "Forma arcieri esperti.";
        public override string CityCulture_Warriors_Description => "Forma abili combattenti corpo a corpo.";
        //public override string CityCulture_AnimalBreeder_Description => "Gli animali forniscono più risorse.";
        public override string CityCulture_Miners_Description => "Le miniere estraggono più minerali.";
        public override string CityCulture_Woodcutters_Description => "Gli alberi forniscono più legno.";
        public override string CityCulture_Builders_Description => "Costruzioni più rapide.";
        public override string CityCulture_CrabMentality_Description => "Il lavoro consuma meno energia, ma non può produrre soldati altamente qualificati.";
        public override string CityCulture_DeepWell_Description => "L’acqua si rigenera più velocemente.";
        public override string CityCulture_Networker_Description => "Servizio postale efficiente.";
        public override string CityCulture_PitMasters_Description => "Aumenta la produzione di combustibile.";

        public override string CityOption_AutoBuild_Work => "Espandi automaticamente la forza lavoro";
        public override string CityOption_AutoBuild_Farm => "Espandi automaticamente le fattorie";

        public override string Hud_PurchaseTitle_Resources => "Acquista risorse";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "Possiedi";

        public override string Tutorial_EndTutorial => "Termina il tutorial";
        public override string Tutorial_MissionX => "Missione {0}";
        public override string Tutorial_CollectXAmountOfY => "Raccogli {0} {1}";
        public override string Tutorial_SelectTabX => "Seleziona scheda: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Aumenta la priorità su: {0}";
        public override string Tutorial_PlaceBuildOrder => "Posiziona ordine di costruzione: {0}";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Seleziona una città";
        public override string Tutorial_ZoomInWorkers => "Esegui uno zoom per vedere i lavoratori";
        public override string Tutorial_CreateSoldiers => "Crea due unità di soldati con questo equipaggiamento: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Riduci lo zoom per una panoramica della mappa";
        public override string Tutorial_ZoomOutDiplomacy => "Riduci lo zoom per aprire la vista diplomatica";
        public override string Tutorial_ImproveRelations => "Migliora le relazioni con una fazione vicina";
        public override string Tutorial_MissionComplete_Title => "Missione completata!";
        public override string Tutorial_MissionComplete_Unlocks => "Nuovi comandi sono stati sbloccati";

        public override string Resource_ReachedStockpile => "Raggiunto l’obiettivo di scorte";

        public override string BuildingType_ResourceMine => "Miniera di {0}";

        public override string Resource_TypeName_BogIron => "Ferro palustre";
        public override string Resource_TypeName_Coal => "Carbone";

        public override string Language_XUpkeepIsY => "{0} mantenimento: {1}";
        public override string Language_XCountIsY => "{0} quantità: {1}";

        public override string Message_ArmyOutOfFood_Text =>
    "Il cibo costoso verrà acquistato dal mercato nero. I soldati affamati diserteranno quando il denaro sarà esaurito.";

        public override string Info_ArmyFood =>
            "Gli eserciti si riforniranno di cibo dalla città amica più vicina. Il cibo può essere acquistato da altre fazioni. Nelle regioni ostili, può essere comprato solo dal mercato nero.";

        public override string FactionName_Monger => "Monger";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

        // patch2
        public override string Tutorial_BuildSomething => "Costruisci qualcosa che produca {0}";
        public override string Tutorial_BuildCraft => "Costruisci una stazione di creazione per: {0}";
        public override string Tutorial_IncreaseBufferLimit => "Aumenta il limite di riserva per: {0}";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "Raggiungi una scorta di {0} {1}";
        public override string Tutorial_LookAtFoodBlueprint => "Guarda il progetto del cibo";
        public override string Tutorial_CollectFood_Info1 => "I lavoratori andranno nella città più vicina per mangiare.";
        public override string Tutorial_CollectFood_Info2 => "L’esercito invia i propri lavoratori a raccogliere il cibo.";
        public override string Tutorial_CollectFood_Info0 => "Vuoi il pieno controllo dei lavoratori? Imposta tutte le priorità di lavoro a zero, poi attiva solo una alla volta.";

        public override string EndGameStatistics_DecorsBuilt => "Decorazioni costruite: {0}";
        public override string EndGameStatistics_StatuesBuilt => "Statue costruite: {0}";



        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation =>
    "Per impostazione predefinita, i lavoratori vanno alla città più vicina per mangiare o consegnare gli oggetti.";

        public override string GameMenu_UseSpeedX => "Opzione velocità {0}";
        public override string GameMenu_LongerBuildQueue => "Coda di costruzione estesa";

        public override string Diplomacy_RelationWithOthers => "Le loro relazioni con gli altri";

        public override string Automation_queue_description => "Continuerà a ripetersi finché la coda non sarà vuota.";

        public override string BuildingType_Storehouse_Description => "I lavoratori possono depositare gli oggetti qui.";

        public override string Resource_TypeName_Longbow => "Arco lungo";
        public override string Resource_TypeName_Rapeseed => "Colza";
        public override string Resource_TypeName_Hemp => "Canapa";

        public override string Resource_BogIronDescription =>
            "L’estrazione del ferro è più efficiente rispetto all’utilizzo del ferro palustre.";

        public override string Resource_FoodSafeGuard_Description =>
            "Salvaguardia attiva. Aumenta al massimo la priorità della catena di produzione del cibo se scende sotto {0}.";
        public override string Resource_FoodSafeGuard_Active => "La salvaguardia è attiva.";

        public override string GameMenu_NextSong => "Brano successivo";

        public override string BuildingType_Bank => "Banca";
        public override string BuildingType_GoldDelivery_Description => "Invia oro ad altre città.";

        public override string BuildingType_Logistics => "Logistica";
        public override string BuildingType_Logistics_Description => "Potenzia la capacità di ordinare nuove costruzioni.";

        public override string BuildingType_Logistics_NationSizeRequirement => "Forza lavoro totale della nazione: {0}";
        public override string Requirements_XItemStorageOfY => "La città deve avere {0} unità immagazzinate di: {1}";

        public override string XP_UnlockBuildQueue => "Sblocca la coda di costruzione a: {0}";
        public override string XP_UnlockBuilding => "Sblocca edificio:";
        public override string XP_Upgrade => "Potenzia";

        public override string XP_UpgradeBuildingX => "Potenzia edificio: {0}";

        /// <summary>
        /// Titolo per descrivere il ciclo di produzione delle fattorie
        /// </summary>
        public override string BuildHud_PerCycle => "Per ciclo";
        public override string BuildHud_MayCraft => "Può creare";
        public override string BuildHud_WorkTime => "Tempo di lavoro: {0}";
        public override string BuildHud_GrowTime => "Tempo di crescita: {0}";
        public override string BuildHud_Produce => "Produce:";

        public override string BuildHud_Queue => "Coda di costruzione consentita: {0}/{1}";

        public override string LandType_Flatland => "Pianura";
        public override string LandType_Water => "Acqua";
        public override string BuildingType_Wall => "Muro";

        public override string Delivery_AutoReciever_Description =>
            "Invierà risorse alle città con la quantità più bassa.";

        public override string Hud_On => "Attivo";
        public override string Hud_Off => "Disattivo";

        public override string Hud_Time_Seconds => "{0} secondi";
        public override string Hud_Time_Minutes => "{0} minuti";
        public override string Hud_Undo => "Annulla";
        public override string Hud_Redo => "Ripristina";

        public override string Tag_ViewOnMap => "Mostra tag sulla mappa";
        public override string MenuTab_Tag => "Tag";

        public override string Input_Build => "Costruisci";
        public override string FlagEditor_ClearAll => "Cancella tutto";

        public override string CityCulture_Stonemason => "Scalpellini";
        public override string CityCulture_Stonemason_Description => "Migliora la raccolta della pietra.";

        public override string CityCulture_Brewmaster => "Mastro birraio";
        public override string CityCulture_Brewmaster_Description => "Aumenta la produzione di birra.";

        public override string CityCulture_Weavers => "Tessitori";
        public override string CityCulture_Weavers_Description => "Aumenta la produzione di armature leggere.";

        public override string CityCulture_SiegeEngineer => "Ingegneri d’assedio";
        public override string CityCulture_SiegeEngineer_Description => "Macchine da guerra più potenti.";

        public override string CityCulture_Armorsmith => "Fabbri d’armature";
        public override string CityCulture_Armorsmith_Description => "Aumenta la produzione di armature di ferro.";

        public override string CityCulture_Noblemen => "Nobili";
        public override string CityCulture_Noblemen_Description => "Cavalieri più potenti.";

        public override string CityCulture_Seafaring => "Naviganti";
        public override string CityCulture_Seafaring_Description => "I soldati con specializzazione navale dispongono di navi più robuste.";

        public override string CityCulture_Backtrader => "Contrabbandieri";
        public override string CityCulture_Backtrader_Description => "Riduce i costi del mercato nero.";

        public override string CityCulture_LawAbiding => "Osservanti della legge";
        public override string CityCulture_LawAbiding_Description => "Aumenta le entrate fiscali. Nessun mercato nero.";


        //##2##
        public override string Hud_Advanced => "Avanzate";
        public override string Hud_Loading => "Caricamento...";

        public override string CityOption_LowerGuardSize => "Rilascia guardie";
        public override string Hud_Purchase_MinCapacity => "Capacità minima raggiunta";
        public override string Settings_ResetToDefault => "Ripristina impostazioni predefinite";
        public override string Settings_NewGame => "Nuova partita";

        public override string Settings_AdvancedGameSettings => "Impostazioni di gioco avanzate";
        public override string Settings_FoodMultiplier => "Moltiplicatore cibo";
        public override string Settings_FoodMultiplier_Description =>
            "Determina per quanto tempo un lavoratore o un soldato resiste a stomaco pieno. Un valore alto può ridurre le prestazioni del computer.";

        public override string Settings_GameMode => "Modalità di gioco";

        public override string Settings_Mode_Story => "Storia completa";
        public override string Settings_Mode_IncludeBoss => "Includi eventi boss";
        public override string Settings_Mode_IncludeAttacks => "Includi attacchi casuali";
        public override string Settings_Mode_Sandbox => "Sandbox";
        public override string Settings_Mode_Peaceful => "Pacifica";
        public override string Settings_Mode_Peaceful_Description => "Tutte le guerre vengono iniziate solo dal giocatore.";

        public override string Lobby_ImportSave => "Importa salvataggio";
        public override string Lobby_ExportSave => "Esporta salvataggio";
        public override string Lobby_ExportSave_Description =>
            "Crea una copia del file e la inserisce nella cartella di importazione: {0}";

        public override string Resource_CurrentAmount => "Quantità attuale: {0}";
        public override string Resource_MaxAmount_Soft => "Limite morbido (massimo): {0}";
        public override string Resource_MaxAmount => "Limite massimo: {0}";
        public override string Resource_AddPerSec => "Tasso di incremento: {0} al secondo";

        public override string Resource_WaterAddLimit => "La velocità di incremento dell’acqua non può essere modificata";

        public override string Tutorial_Select_SubTab => "Seleziona categoria: {0}";


        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */

        public override string Tutorial_OpenGuardSubTab => "Apri una caserma e seleziona la categoria: {0}";
        public override string Tutorial_GuardToWall => "Sposta una guardia sul muro";

        public override string Demo_MissionObjective_Title => "Obiettivo della missione";
        public override string Demo_MissionObjective_Description => "Difenditi da un attacco proveniente da sud";
        public override string Demo_Complete_Title => "Demo completata";
        public override string Demo_TimesUp_Title => "Tempo scaduto!";
        public override string Demo_EndInOneMinuteDescription => "La demo terminerà tra un minuto";

        public override string ArmyOption_NewArmy => "Nuovo esercito";
        public override string ProfileEditor_AltMain => "Alternativo principale";
        public override string Automation_CheckBoxTitle => "Automatico";

        public override string ArmyStructure_ColumnWidth => "Larghezza colonna esercito";
        public override string ArmyStructure_ArmyPlacement => "Posizionamento nell’esercito";
        public override string ArmyStructure_Row_Front => "Fronte";
        public override string ArmyStructure_Row_Body => "Centro";
        public override string ArmyStructure_Row_Second => "Seconda linea";
        public override string ArmyStructure_Row_Behind => "Retro";

        public override string Diplomacy_RelationType_Enemies => "Nemici";

        public override string EventMessage_EnemyAlliance_Title => "Timore di dominazione";
        public override string EventMessage_EnemyAlliance =>
            "Le nazioni, temendo la tua crescente potenza, si uniscono in un’alleanza contro di te.";

        public override string Settings_CentralGold => "Oro centralizzato";
        public override string Settings_CentralGold_Description =>
            "Attivo: tutto il tuo oro è in un fondo condiviso e utilizzabile istantaneamente. Disattivo: l’oro è fisico e deve essere trasportato.";


        public override string InputActionName_StopStart => "Ferma/Avvia";
        public override string InputActionName_ToggleHudDetail => "Mostra/Nascondi dettagli HUD";
        public override string InputActionName_NextCity => "Città successiva";
        public override string InputActionName_NextArmy => "Esercito successivo";
        public override string InputActionName_NextBattle => "Battaglia successiva";
        public override string InputActionName_Build => "Costruisci";
        public override string InputActionName_Copy => "Copia";
        public override string InputActionName_Paste => "Incolla";
        public override string InputActionName_Menu => "Menu";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "Colore precedente";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "Colore successivo";
        public override string InputActionName_FlagDesign_PaintBucket => "Secchiello";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "Selettore colore";
        public override string InputActionName_ControllerFocus => "Metti a fuoco";
        public override string InputActionName_ControllerCancel => "Annulla";
        public override string InputActionName_ControllerMessageClick => "Seleziona messaggio";
        public override string InputActionName_ControllerSelect => "Seleziona";
        public override string InputActionName_WASD_UP => "Su";
        public override string InputActionName_WASD_DOWN => "Giù";
        public override string InputActionName_WASD_LEFT => "Sinistra";
        public override string InputActionName_WASD_RIGHT => "Destra";
        public override string InputActionName_CameraTiltLeft => "Inclina camera a sinistra";
        public override string InputActionName_CameraTiltRight => "Inclina camera a destra";
        public override string InputActionName_CameraTiltUp => "Inclina camera in alto";
        public override string InputActionName_ZoomInKey => "Zoom avanti";
        public override string InputActionName_ZoomOutKey => "Zoom indietro";




        public override string Settings_Title_Monitor => "Opzioni monitor";
        public override string Settings_Title_Graphics => "Opzioni grafiche";
        public override string Settings_Title_Input => "Comandi";
        public override string Settings_Title_Gameplay => "Opzioni di gioco";
        public override string Settings_PanOnZoom => "Panoramica durante lo zoom";
        public override string Settings_ScrollSensitivity_Game => "Sensibilità scorrimento: gioco";
        public override string Settings_ScrollSensitivity_Menu => "Sensibilità scorrimento: menu";
        public override string Settings_Blood => "Sangue";

        public override string Settings_MasterVolume => "Volume principale";
        public override string Settings_AmbienceVolume => "Volume ambiente";
        public override string Settings_BattleMelody => "Melodia di battaglia";

        public override string Settings_ModelLight => "Effetto luce modelli";
        public override string Settings_Particles => "Effetti particellari";
        public override string Settings_MapLoadSpeed => "Velocità di caricamento mappa";

        public override string Lobby_Category_Options => "Opzioni";
        public override string Lobby_Category_Editor => "Editor";
        public override string Lobby_Category_ExtraModes => "Modalità extra";

        public override string Lobby_Editor_MapEditor => "Editor mappa";
        public override string Lobby_Editor_VoxelEditor => "Editor voxel";

        public override string Lobby_Mode_BattleLab => "Laboratorio di battaglia";
        public override string Lobby_Mode_BattleLab_Description => "Metti alla prova qualsiasi tipo di soldato l’uno contro l’altro.";
        public override string Lobby_Mode_Commander => "Comandante";
        public override string Lobby_Mode_Commander_Description => "Un piccolo gioco tattico da tavolo.";
        public override string Lobby_MusicPlayList => "Playlist musicale";

        public override string Lobby_GameSetup => "Impostazioni partita";
        public override string Lobby_PlayerSetup => "Impostazioni giocatori";
        public override string LobbyDemoMode_Demo => "Demo";

        public override string Lobby_Tutorial => "Tutorial";

        public override string LobbyDemoMode_ShortTutorial => "Tutorial rapido";
        public override string LobbyDemoMode_LongTutorial => "Tutorial esteso";

        /// <summary>
        /// Mostra “wishlist on”, seguito dal logo di Steam
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "Aggiungi alla wishlist";



        public override string BattleLab_StartHere => "Avvia battaglia qui";
        public override string BattleLab_Start => "Avvia battaglia";
        public override string BattleLab_Attacker => "Attaccante";

        public override string MapGenerator_Name => "Editor mappa - generatore";

        public override string MapType_CustomMap => "Mappa personalizzata";
        public override string MapType_GenerateNewMap => "Genera nuova mappa";
        public override string MapGenerator_GenerateAction => "Genera";
        public override string MapGenerator_Terrain_CustomSize => "Dimensioni personalizzate";
        public override string MapGenerator_Terrain_StartAs => "Inizia come";
        public override string MapGenerator_Terrain_ClearPass => "Esegui passaggio di pulizia";
        public override string MapGenerator_Terrain_BuildPass => "Esegui passaggio di costruzione";
        public override string MapGenerator_Terrain_DigPass => "Esegui passaggio di scavo";
        public override string MapGenerator_Terrain_BuildDigLoops => "Cicli costruzione-scavo";
        public override string MapGenerator_Terrain_BuildStrokes => "Numero tratti di costruzione";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Misurato in pennellate per ogni 100 caselle";
        public override string MapGenerator_Terrain_DigStrokes => "Numero tratti di scavo";
        public override string MapGenerator_Terrain_CleanUp_Option => "Ripulisci le singole caselle";
        public override string MapGenerator_Terrain_CleanUpPass => "Esegui passaggio di pulizia";

        public override string Economy_ServicemenUpkeep => "Mantenimento del personale di servizio: {0}";
        public override string Economy_ServicemenUpkeep_Description => "Il mantenimento è di {0} oro per uomo di servizio";
        public override string Economy_GuardUpkeep_Description => "Il mantenimento è di {0} oro per guardia";

        public override string EndScreen_TimeHasEndedTitle => "Tempo scaduto";

        public override string Hud_AdvancedSettings => "Impostazioni avanzate";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "Annulla";
        public override string Hud_Delete => "Elimina";
        public override string Hud_Next => "Avanti";
        // public override string Hud_None => "Nessuno";
        public override string Hud_Apply => "Applica";
        public override string Hud_AllCities => "Tutte le città";
        public override string Hud_Time_Hours => "{0} ore";
        public override string Hud_AddX => "Aggiungi {0}";
        public override string Hud_Both => "Entrambi";
        public override string Hud_Direction => "Direzione";
        //public override string MusicIsBroken => "La musica non funziona al momento";

        /// <summary>
        /// 0: nome del tipo di oggetto raccolto, 1: numero di oggetti
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, quantità: {1}";

        public override string Hud_EffectDoesNotStack => "Questo effetto non è cumulabile";

        public override string Work_SmeltX => "Fondi {0}";

        public override string Info_TotalFoodProduction => "Produzione totale di cibo";
        public override string Info_TotalFoodSpending => "Consumo totale di cibo";

        public override string Info_FooodAndDeliveryLocation =>
            "Per impostazione predefinita, i lavoratori vanno nella città più vicina per mangiare o consegnare gli oggetti.";

        public override string Delivery_SendChunk => "Oggetti in consegna";
        public override string Delivery_SpeedBonus => "Bonus velocità: {0}%";

        public override string Delivery_AutoResourceDescription =>
            "Consegna automaticamente gli oggetti che hanno raggiunto il limite di scorte alle città che ne hanno bisogno.";

        public override string Conscript_Soldiers_ArmyType => "Soldati dell’esercito";
        public override string Conscript_Soldiers_ArmyType_Description => "Recluta soldati in un esercito adiacente";
        public override string Conscript_Soldiers_GuardType => "Guardia cittadina";
        public override string Conscript_Soldiers_GuardType_Description => "Le guardie vengono utilizzate per fortificare le mura";

        //-
        public override string Defence_Title => "Difesa";
        public override string Defence_GuardPost => "Postazione di guardia";

        public override string Defence_WallDescription_Movement => "Ostacola il movimento dei nemici.";
        public override string Defence_WallDescription_GuardPost => "Le guardie possono essere assegnate qui.";
        public override string Defence_AutoAssign => "Assegnazione automatica";
        public override string Defence_AutoAssign_Description => "Le nuove guardie verranno assegnate automaticamente a questo posto.";

        public override string Conscript_SplashDamage => "Danno ad area";
        public override string Conscript_HighSplashDamage => "Alto danno ad area";

        public override string Conscript_Training_Champion => "Campione";
        public override string Conscript_Training_Legendary => "Leggendario";


        public override string Experience_Title => "Esperienza";
        public override string Experience_TopExperience => "Livelli di esperienza più alti";

        public override string Experience_TimeReductionDescription => "Il tempo di lavoro è ridotto del {0}% per livello";

        public override string ExperienceType_Farm => "Agricoltore";
        public override string ExperienceType_AnimalCare => "Allevatore";
        public override string ExperienceType_HouseBuilding => "Costruttore di case";
        public override string ExperienceType_WoodWork => "Falegname";
        public override string ExperienceType_StoneCutter => "Scalpellino";
        public override string ExperienceType_Mining => "Minatore";
        public override string ExperienceType_Transport => "Trasportatore";
        public override string ExperienceType_Cook => "Cuoco";
        public override string ExperienceType_Fletcher => "Armaiolo di archi";
        public override string ExperienceType_RefineOre => "Fonditore";
        public override string ExperienceType_Casting => "Colatore";
        public override string ExperienceType_CraftMetal => "Fabbro";
        public override string ExperienceType_CraftArmor => "Armaturaio";
        public override string ExperienceType_CraftWeapon => "Armaiolo";
        public override string ExperienceType_CraftFuel => "Carbonaro";
        public override string ExperienceType_Chemist => "Chimico";

        public override string ExperienceLevel_1 => "Principiante";
        public override string ExperienceLevel_2 => "Praticante";
        public override string ExperienceLevel_3 => "Esperto";
        public override string ExperienceLevel_4 => "Maestro";
        public override string ExperienceLevel_5 => "Leggendario";

        public override string ExperenceOrDistancePrio_Title => "Selezione lavoratori";
        public override string ExperenceOrDistancePrio_Description =>
            "I lavoratori inattivi verranno assegnati ai compiti in base alla distanza o all’esperienza.";

        public override string Technology_Description =>
    "Ogni città ha un albero tecnologico. Ogni tecnologia sblocca edifici e oggetti.";

        public override string Experience_Description =>
            "I lavoratori guadagnano esperienza e migliorano nel tempo.";

        public override string Technology_Title => "Tecnologia";
        public override string Technology_ShareField => "Condivisione del campo tecnologico";

        public override string Technology_GainByNeigborRelation =>
            "Per ogni città vicina con tecnologia. Se la tua relazione è {0}: {1}";

        public override string Technology_ForEachMaster =>
            "Quando un {0} raggiunge il livello di esperienza {1}, nel campo tecnologico: PH2";

        public override string Technology_CitySpread =>
            "Le tue città condivideranno la tecnologia se adiacenti: {0}";

        public override string Technology_CityCapture =>
            "La maggior parte delle tecnologie viene distrutta quando una città viene conquistata in battaglia.";

        public override string Technology_AdvancedBuildings => "Edifici avanzati";
        public override string Technology_AdvancedFarming => "Agricoltura avanzata";
        public override string Technology_AdvancedCasting => "Fusione avanzata";

        public override string Help_Title => "Aiuto";
        public override string Help_Work_Title => "Il lavoro non parte";
        public override string Help_Work_Resources => "Gli edifici richiedono risorse disponibili.";
        public override string Help_Work_Skill =>
            "I lavoratori devono avere il livello di abilità corretto (o superiore).";
        public override string Help_Work_Stockpile =>
            "La raccolta delle risorse verrà bloccata se il magazzino è pieno.";
        public override string Help_Work_Priority =>
            "Il lavoro potrebbe avere priorità bassa o nulla.";

        public override string Help_Soldiers_Title => "Produzione soldati";
        public override string Help_Soldiers_PlaceBuildingX => "Costruisci edificio: {0}";
        public override string Help_Soldiers_Workers => "Lavoratori disponibili per il reclutamento";
        public override string Help_Soldiers_Weapon => "Un’arma per ogni soldato";
        public override string Help_Soldiers_StartX => "Avvia: {0}";
        public override string Hud_SelectHistory => "Seleziona cronologia";

        public override string Hud_PointsPerMinute => "{0} punti al minuto";
        public override string Hud_PercentValueCost => "Il servizio costa il {0}% del valore";

        public override string Hud_Mixed => "Misto";
        public override string Hud_Distance => "Distanza";

        public override string Hud_Unlock => "Sblocca";
        public override string Hud_category => "Categoria";

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public override string Input_StepOneFrame => "Avanza di un fotogramma";

        public override string Resource_TypeName_Wagon2Wheel => "Carro a due ruote";
        public override string Resource_TypeName_Wagon4Wheel => "Carro a quattro ruote";
        public override string Resource_TypeName_Tin => "Stagno";
        public override string Resource_TypeName_TinOre => "Minerale di stagno";

        public override string Resource_TypeName_Copper => "Rame";
        public override string Resource_TypeName_CopperOre => "Minerale di rame";
        public override string Resource_TypeName_SilverOre => "Minerale d’argento";
        public override string Resource_TypeName_Silver => "Argento";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Mithril grezzo";
        public override string Resource_TypeName_Mithril => "Mithril";

        public override string Resource_TypeName_BronzeSword => "Spada di bronzo";
        public override string Resource_TypeName_ShortSword => "Spada corta";
        public override string Resource_TypeName_LongSword => "Spada lunga";
        public override string Resource_TypeName_HandSpear => "Lancia corta";
        public override string Resource_TypeName_Warhammer => "Martello da guerra";
        public override string Resource_TypeName_MithrilSword => "Spada di mithril";
        public override string Resource_TypeName_SlingShot => "Fionda";
        public override string Resource_TypeName_ThrowingSpear => "Giavellotto";
        public override string Resource_TypeName_Crossbow => "Balestra";
        public override string Resource_TypeName_MithrilBow => "Arco di mithril";

        public override string Resource_TypeName_CoolingFluid => "Liquido di raffreddamento";
        public override string Resource_TypeName_Palisade => "Palizzata";
        public override string Resource_TypeName_Toolkit => "Kit degli attrezzi";

        public override string Resource_TypeName_Sulfur => "Zolfo";
        public override string Resource_TypeName_LeadOre => "Minerale di piombo";
        public override string Resource_TypeName_Lead => "Piombo";
        public override string Resource_TypeName_Bronze => "Bronzo";
        public override string Resource_TypeName_BloomIron => "Ferro da fucina";
        public override string Resource_TypeName_Steel => "Acciaio";
        public override string Resource_TypeName_CastIron => "Ghisa";

        public override string Resource_TypeName_BlackPowder => "Polvere nera";
        public override string Resource_TypeName_GunPowder => "Polvere da sparo";
        public override string Resource_TypeName_LedBullet => "Proiettile di piombo";

        public override string Resource_TypeName_HandCannon => "Schioppo";
        public override string Resource_TypeName_HandCulverin => "Colubrina portatile";
        public override string Resource_TypeName_Rifle => "Fucile";
        public override string Resource_TypeName_Blunderbuss => "Archibugio";

        public override string Resource_TypeName_Manuballista => "Manuballista";
        public override string Resource_TypeName_Catapult => "Catapulta";
        public override string Resource_TypeName_BatteringRam => "Ariete";
        public override string Resource_TypeName_SiegeCannonBronze => "Cannone d’assedio di bronzo";
        public override string Resource_TypeName_ManCannonBronze => "Bombarda";
        public override string Resource_TypeName_SiegeCannonIron => "Cannone d’assedio di ferro";
        public override string Resource_TypeName_ManCannonIron => "Cannone";

        public override string Resource_TypeName_PaddedArmor => "Armatura imbottita";
        public override string Resource_TypeName_HeavyPaddedArmor => "Armatura imbottita pesante";

        public override string Resource_TypeName_IronArmor => "Armatura di maglia";
        public override string Resource_TypeName_HeavyIronArmor => "Armatura di maglia pesante";

        public override string Resource_TypeName_BronzeArmor => "Armatura di bronzo";

        public override string Resource_TypeName_LightPlateArmor => "Corazza leggera";
        public override string Resource_TypeName_FullPlateArmor => "Corazza completa";
        public override string Resource_TypeName_MithrilArmor => "Armatura di mithril";
        public override string Resource_TypeName_Coin => "Moneta";

        public override string UnitType_Warhammer => "Cavaliere con martello";

        public override string UnitType_SpearAndShield => "Lanciere con scudo";

        public override string UnitType_CollectionOfSoldiers => "Gruppo di soldati";
        public override string UnitType_CollectionOfArmies => "Gruppo di eserciti";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => "(id {0})";

        public override string BuildHud_AreaEffectTitle => "Effetto area";
        public override string BuildHud_BonusRadius => "Raggio bonus: {0}";

        public override string BuildHud_BuildTime => "Tempo di costruzione";
        public override string SchoolHud_ToLevel => "Verso livello";
        public override string SchoolHud_TimeDescription => "Il tempo assume esperienza zero; diminuisce con l’esperienza.";
        public override string SchoolHud_SelectSchool => "Seleziona scuola";
        public override string Upgrade_Order => "Ordine di potenziamento";

        public override string Building_ListDescription => "Elenco di tutti gli edifici in questa categoria";

        public override string BuildingType_IsUpgraded => "{0} - migliorato";
        public override string BuildingType_WoodCutter => "Segheria";
        public override string BuildingType_Workshop_Description => "Migliora il lavoro nell’area circostante";

        public override string BuildingType_WoodCutter_AreaAffect => "Guadagna il {0}% di legno in più dagli alberi";

        public override string BuildingType_StoneCutter_AreaAffect => "Guadagna il {0}% di pietra in più";

        public override string BuildingType_StoneCutter => "Cava di pietra";

        public override string BuildingType_Embassy => "Ambasciata";
        public override string BuildingType_Embassy_Description => "Per le relazioni diplomatiche";

        public override string BuildingType_SoldierBarracks => "Caserma soldati";
        public override string BuildingType_ArcherBarracks => "Caserma arcieri";
        public override string BuildingType_WarmachineBarracks => "Caserma macchine da guerra";
        public override string BuildingType_GunBarracks => "Caserma armi da fuoco";
        public override string BuildingType_CannonBarracks => "Caserma cannoni";
        public override string BuildingType_KnightsBarracks => "Caserma cavalieri";

        public override string BuildingType_WaterResovoir => "Serbatoio d’acqua";
        public override string BuildingType_WaterResovoir_Description => "Aumenta la capacità di stoccaggio dell’acqua";

        public override string BuildingType_SmeltingFurnace => "Fornace di fusione";
        public override string BuildingType_SmeltingFurnace_Description => "Purifica il minerale in metallo";

        public override string BuildingType_Foundry => "Fonderia";
        public override string BuildingType_Foundry_Description => "Stazione per la fusione dei metalli";

        public override string BuildingType_Armory => "Armeria";
        public override string BuildingType_Armory_Description => "Stazione di produzione di armature";
        public override string BuildingType_Chemist => "Laboratorio chimico";
        public override string BuildingType_Chemist_Description => "Stazione di produzione di sostanze chimiche";
        public override string BuildingType_CoinMaker => "Zecca";
        public override string BuildingType_CoinMaker_Description => "Trasforma i metalli in monete";
        public override string BuildingType_Gunmaker => "Armaiolo";
        public override string BuildingType_Gunmaker_Description => "Stazione di produzione per armi da fuoco e cannoni";

        public override string BuildingType_School_Tab => "Scuola";
        public override string BuildingType_School => "Gilda dei maestri";
        public override string BuildingType_School_Description => "Aumenta il livello di abilità dei lavoratori";

        public override string BuildingType_GoldDelivery => "Corriere d’oro";
        public override string BuildingType_Bank_Description => "Gestione delle riserve d’oro";

        public override string DecorType_CobbleStones => "Ciottolato";
        public override string DecorType_Square => "Piazza cittadina";

        public override string DecorType_Garden => "Giardino";
        public override string DecorType_Flag => "Bandiera";
        public override string DecorType_Banner => "Stendardo";

        public override string BuildingType_DirtRoad => "Strada sterrata";
        public override string BuildingType_Palisade => "Forte di palizzata";

        public override string ResourceType_ServiceMen => "Addetti ai servizi";
        public override string BuildingType_ServiceHouse => "Casa dei servizi";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "Aggiunge {0} addetti ai servizi";

        public override string BuildingType_GuardOffice => "Ufficio della guardia";
        public override string BuildingType_GuardOffice_DescriptionAddX => "Aumenta il limite di guardie di {0}";

        public override string BuildingType_DirtWall => "Muro di terra";
        public override string BuildingType_DirtTower => "Torre di terra";
        public override string BuildingType_WoodWall => "Muro di legno";
        public override string BuildingType_WoodTower => "Torre di legno";
        public override string BuildingType_StoneWall => "Muro di pietra";
        public override string BuildingType_StoneTower => "Torre di pietra";
        public override string BuildingType_StoneGate => "Porta di pietra";
        public override string BuildingType_StoneHouse => "Casa di pietra";



        /// <summary>
        /// When listing slight variations, like "LampA" and "LampB"
        /// </summary>
        public override string VariantType_A => "{0}A";
        public override string VariantType_B => "{0}B";
        public override string VariantType_C => "{0}C";
        public override string VariantType_D => "{0}D";
        public override string VariantType_E => "{0}E";
        public override string VariantType_F => "{0}F";
        public override string VariantType_G => "{0}G";
        public override string VariantType_H => "{0}H";
        public override string BuildingToolShape_Free => "Penna";
        public override string BuildingToolShape_Area => "Rettangolo";
        public override string BuildingToolShape_Line => "Linea";
        public override string BuildingToolShape_LShape => "Forma a L";

        public override string CityHall_Upgrade => "Potenziamento municipio";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Numero massimo di lavoratori supportati: {0}";

        public override string CityHall_Size_Small => "Villaggio";
        public override string CityHall_Size_Medium => "Cittadina";
        public override string CityHall_Size_Large => "Capitale";

        public override string GuardHousingCount => "Alloggi ufficio della guardia";
        public override string ServicemenCount => "Addetti ai servizi: {0}";

        public override string Work_MiningResource => "Estrazione di {0}";

        public override string MenuTab_Progress => "Progresso";

        public override string Automation_AutomateCity => "Automatizza città";
        public override string Automation_AutomationFocus => "Focus automazione";
        public override string Automation_AutomationFocus_Grow => "Crescita";
        public override string Automation_AutomationFocus_Export => "Esportazione";
        public override string Automation_AutomationFocus_War => "Guerra";

        public override string CityCulture_Smelters_Description => "Fusione del minerale migliorata";
        public override string CityCulture_Smelters => "Fusori";

        public override string CityCulture_Apprentices_Description => "I nuovi lavoratori guadagnano esperienza dai lavoratori attivi";
        public override string CityCulture_Apprentices => "Apprendisti";

        public override string CityCulture_BronzeCasters_Description => "Produzione migliorata di bronzo e oggetti in bronzo";
        public override string CityCulture_BronzeCasters => "Fusori di bronzo";


        //DEMO PATCH 1

        /// <summary>
        /// Evil orcs that roam on the map
        /// </summary>
        public override string FactionName_Barbarian => "Orde oscure";

        public override string Tutorial_AttackAndDestroyX => "Attacca e distruggi: {0}";
        public override string Resource_TypeName_Pike => "Picca";

        public override string BattleTrials_Title => "Prove di battaglia";
        public override string BattleTrials_Description => "Metti alla prova le tue tattiche in uno scontro diretto esercito contro esercito.";

        // DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "Questi attacchi riducono la probabilità di blocco";

        public override string Conscript_BlockPerSecond => "Può bloccare {0} volte al secondo";

        public override string Conscript_BlockDescription => "I soldati bloccano la maggior parte degli attacchi provenienti dal loro arco frontale";

        public override string Map_CustomSeed => "Seed mappa";

        public override string Settings_Mode_Spectator => "Spettatore";

        // public override string Settings_Mode_Spectator_Description => "Solo osserva";

        public override string Automation_AutomationFocus_NoFocus_Description => "Costruirà un po’ di tutto";

        public override string Automation_AutomationFocus_WillProduce => "Produrrà principalmente:";

        public override string Help_Food_WhoEats => "Tutti i soldati e i lavoratori consumano cibo";

        public override string Help_Food_BigArmy => "Un grande esercito può affamare le città nella propria area";

        public override string Help_Food_DontBuild => "Costruire più fattorie non aumenta automaticamente il cibo: servono lavoratori disponibili e stazioni di cucina per raccogliere e processare le risorse";

        public override string Help_Food_UseWater => "La produzione di cibo richiede acqua";

        public override string Help_Food_Postal => "Assicurati che le tue città si supportino inviandosi cibo";

        public override string Message_LostCity => "Città perduta";

        public override string Demo_Description => "Uno scenario breve: difendi la tua città per {0} minuti";

        // DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "La demo terminerà tra {0} minuti";

        public override string Experience_Required => "Esperienza richiesta";

        public override string InputActionName_ToggleMenu => "Apri/Chiudi menu";

        // DEMO PATCH 4
        public override string Work_BadValueDescription =>
            "Le risorse possono scendere sotto zero o superare leggermente il limite di stoccaggio. I limiti vengono applicati solo quando la coda di lavoro viene creata.";

        public override string Work_SelectCategory => "Seleziona categoria oggetti";
        public override string Hud_RemoveFromList => "Rimuovi dalla lista";

        public override string Hud_ReturnToPrevious => "Torna indietro";
        public override string Hud_Close => "Chiudi";

        public override string Hud_Low => "Basso";
        public override string Hud_Medium => "Medio";
        public override string Hud_High => "Alto";

        public override string Hud_Copy => "Copia";
        // public override string Hud_Paste => "Incolla";
        public override string Hud_Cut => "Taglia";
        public override string Hud_SaveCompleted => "Salvataggio completato";

        public override string Settings_WaterMultiplier => "Moltiplicatore acqua";
        public override string Settings_WaterMultiplier_Description => "Determina quanta acqua le città producono e immagazzinano. Valori più alti riducono le prestazioni del computer.";

        public override string Settings_ChildMultiplier => "Moltiplicatore nascite";

        public override string Settings_CraftMultiplier_Description => "Valori più bassi comportano una produzione più veloce.";

        public override string FastProduction => "Produzione veloce";
        public override string SlowProduction => "Produzione lenta";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "Non produrrà";

        public override string Automation_AutomationFocus_NoFocus => "Tutto";
        public override string CityAutomation_SoldierQuality => "Qualità soldati";
        public override string CityAutomation_SoldierWeaponType => "Tipo di arma";

        public override string WarsResourceGroup_Resources => "Risorse";
        public override string WarsResourceGroup_Weapons => "Armi";

        public override string WarsResourceGroup_AllWeaponTypes => "Miste";
        public override string WarsResourceGroup_MeleeHandWeapons => "Corpo a corpo";
        public override string WarsResourceGroup_RangedHandWeapons => "A distanza";
        public override string WarsResourceGroup_Warmachines => "Macchine da guerra";

        public override string FactionSettings_Titel => "Impostazioni fazione";
        public override string FactionSettings_Description => "Si applica a tutte le tue città";

        public override string Conscript_MaxPopulation => "Popolazione massima";
        public override string Conscript_MaxPopulation_Description => "Arruola solo quando la popolazione ha raggiunto il massimo";

        public override string Conscript_FoodAbundance => "Scorte massime di cibo";
        public override string Conscript_FoodAbundance_Description => "Arruola solo quando il cibo ha raggiunto il livello massimo di stoccaggio";
        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Imposta: Attivo";
        public override string GeneralSetting_Off => "Imposta: Disattivo";
        public override string GeneralSetting_AllBuildingsDescription => "Si applica a tutti gli edifici";

        public override string GeneralSetting_ApplyMessage => "Modifica applicata a {0} edifici";

        public override string MustTurnOffSteamInput => "Per usare i controller, devi disattivare Steam Input.";

        public override string Technology_GainTitle => "Modi per ottenere tecnologia";
        public override string Technology_LevelUp => "Aumenta livello";
        public override string Technology_ForEachLevelUp => "Quando un lavoratore sale di livello nel campo tecnologico: {0}";

        public override string VoxelEditor_Description => "Crea modelli a blocchi";

        public override string Editor_Tool => "Strumento";
        public override string Editor_SelectOptionsMenu => "Menu di selezione opzioni";
        public override string Editor_Continous => "Continuo";
        public override string Editor_Tool_PencilSize => "Dimensione pennello";
        public override string Editor_Tool_SizeTolerance => "Tolleranza dimensione";
        public override string Editor_Tool_RoundPencil => "Pennello rotondo";
        public override string Editor_Tool_EdgeSize => "Dimensione bordo";
        public override string Editor_Tool_PercentFill => "Percentuale riempimento";
        public override string Editor_Tool_ClearAbove => "Cancella sopra";
        public override string Editor_Tool_FillBelow => "Riempi sotto";
        public override string Editor_UserModels => "Modelli utente";
        public override string Editor_UserModels_Description => "Sfoglia i modelli che hai salvato";

        public override string Editor_RetailModels => "Modelli di gioco";
        public override string Editor_RetailModels_Description => "Carica modelli dal gioco";

        public override string Editor_ModTemplates => "Template modding";
        public override string Editor_ExportAsOBJ => "Esporta come .OBJ";
        public override string Editor_SelectAll => "Seleziona tutto";

        public override string Editor_Canvas_Title => "Tela";
        public override string Editor_Canvas_Size => "Dimensione";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "Predefiniti dimensione";
        public override string Editor_Canvas_Move => "Sposta";
        public override string Editor_Canvas_Move_Up => "Su";
        public override string Editor_Canvas_Move_Down => "Giù";
        public override string Editor_Canvas_RotateClockwise => "Ruota in senso orario";
        public override string Editor_Canvas_RotateCounterClockwise => "Ruota in senso antiorario";
        public override string Editor_Canvas_Mirror => "Specchia";

        public override string Editor_Canvas_RotateFlip_Title => "Ruota / Rifletti";
        public override string Editor_Canvas_FlipVertical => "Rifletti su/giù";
        public override string Editor_Canvas_FlipOrientation => "Rifletti orizzontale/verticale";
        public override string Editor_Canvas_ClearAll_Description => "Rimuove tutti i blocchi e i frame";

        public override string Editor_Animation => "Animazione";
        public override string Editor_Animation_RemoveCurrentFrame => "Rimuovi frame corrente";
        public override string Editor_Animation_AddFrameCopy => "Aggiungi copia frame";
        public override string Editor_Animation_AddEmptyFrame => "Aggiungi frame vuoto";
        public override string Editor_Animation_MoveDescription => "Cambia posizione frame";
        public override string Editor_Animation_AllFrames => "Tutti i frame";
        public override string Editor_Animation_AllFrames_ActionDescription => "Esegui la stessa azione su tutti i frame";

        public override string Editor_SettingsMenu => "Impostazioni";
        public override string Hud_Exit => "Esci";
        public override string Editor_Canvas_Clear => "Pulisci";

        public override string Editor_Stamp => "Timbro";
        public override string Editor_StampOtherFrames => "Timbra in altri frame";
        public override string Editor_StampOtherFrames_Description => "Incolla i voxel in questi frame";
        public override string Editor_PasteToFrame => "Incolla voxel in questo frame";
        public override string Editor_ClearAllFrames => "Pulisci tutti i frame";
        public override string Editor_ClearOtherFrames => "Pulisci altri frame";

        public override string Editor_Settings_MoveSpeed => "Velocità movimento";
        public override string Editor_Settings_BackgroundColor => "Colore sfondo";
        public override string Editor_Settings_HideHUD => "Nascondi HUD";

        public override string Editor_Color => "Colore";
        public override string Editor_ColorsInUseLabel => "Colori in uso";
        public override string Editor_Color_BrighterPlus => "Più chiaro++";
        public override string Editor_Color_Brighter => "Più chiaro";
        public override string Editor_Color_Darker => "Più scuro";
        public override string Editor_Color_DarkerPlus => "Più scuro++";
        public override string Editor_Color_RedTint => "Tinta rossa";
        public override string Editor_Color_Tint => "Tinta";
        public override string Editor_Color_GreenTint => "Tinta verde";
        public override string Editor_Color_BlueTint => "Tinta blu";
        public override string Editor_Color_YellowTint => "Tinta gialla";
        public override string Editor_Color_PurpleTint => "Tinta viola";
        public override string Editor_NoColor => "Vuoto";

        public override string Editor_Material => "Materiale";
        /// <summary>
        /// L’utente può sostituire un colore con un altro in tutto il modello
        /// </summary>
        public override string Editor_Color_Recolor => "Ricolora";
        public override string Editor_Color_RecolorTo => "Ricolora in";

        public override string Editor_Material_Set => "Imposta materiale";

        public override string Editor_Preview => "Anteprima";
        public override string Editor_CombineWithCurrent => "Combina con modello corrente";

        public override string Editor_PickedColor => "Colore selezionato";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "Tenda per immigrati";
        public override string BuildingType_ImmigrationTent_Description => "Ospita {0} immigrati";

        public override string BuildingType_ReseachCenter => "Centro di ricerca";
        public override string BuildingType_Bookpress => "Pressa per libri";
        public override string BuildingType_Bookpress_Description => "In un campo di ricerca, tutti i punti ottenuti verranno condivisi con tutti i {0} nelle altre città.";

        ///
        /// 0: birra, 1: chimica, 2: polvere da sparo
        ///
        public override string Technology_ReseachExample =>
            "Esempio: quando un lavoratore produce {0}, aumenta la sua abilità in {1}. " +
            "Salendo di livello, aggiungerà punti alla tecnologia {2}, poiché condividono lo stesso campo di ricerca.";

        public override string BuildingType_Research_BaseDescription => "Aumenta la ricerca tecnologica.";

        public override string BuildingType_ResearchCenter_Description => "Aggiunge {0} punti ricerca extra quando un lavoratore sale di livello nello stesso campo.";
        // DEMO PATCH 5

        public override string Editor_CropSelection => "Ritaglia selezione";

        public override string Immigrants_DisbandedSoldiers => "I soldati congedati diventeranno immigrati";
        public override string Immigrants_RefillWorkers => "Ripristina rapidamente la forza lavoro";
        public override string Immigrants_UnhousedAreLost => "Gli immigrati senza casa scompariranno dopo un po' di tempo";

        public override string Editor_VoxelCount => "{0} voxel";

        public override string Editor_Layers_Titel => "Livelli";
        public override string Editor_Layers_All => "Tutti i livelli";
        public override string Editor_LayerNumber => "Livello {0}";

        public override string Editor_Layer_AddEmpty => "Aggiungi livello vuoto";
        public override string Editor_Layer_AddCopy => "Duplica livello";
        public override string Editor_Layer_Remove => "Rimuovi livello";
        public override string Editor_Layer_MergeDown => "Unisci in basso";
        public override string Editor_IsAnimated => "Animato";
        public override string Editor_ToggleVisible => "Attiva/disattiva visibilità";
        public override string Editor_ToggleAnimatedLayer => "Attiva/disattiva livello animato";
        public override string Editor_Projects => "File progetto";

        public override string ProfileEditor_ReplaceMaterial => "Colore profilo: {0}";
        public override string ProfileEditor_ProfileColors_Label => "Colori profilo";
        public override string ProfileEditor_TunicColor => "Colore tunica";
        public override string ProfileEditor_PantsColor => "Colore pantaloni";
        public override string ProfileEditor_LeaderColor => "Colore capo";

        public override string MapStartAs_Water => "Acqua";
        public override string MapStartAs_Land => "Terra";
        public override string MapStartAs_Circle => "Cerchio";

        public override string Hud_NeedToBeAssigned => "Richiede assegnazione";
        public override string Hud_CommitAssignment => "Assegna";

        public override string Technology_NoAvailableResearch => "Nessuna ricerca disponibile";
        public override string Research_Tab => "Ricerca";

        // 5.2
        public override string BuildCategory_General => "Generale";
        public override string BuildCategory_Military => "Militare";
        public override string BuildCategory_Decoration => "Decorazione";
        public override string BuildCategory_Upgrade => "Potenziamento";

        public override string Work_NoMines => "Nessuna miniera";

        // NEXT FEST DEMO

        public override string HUD_DisplayName => "Nome visualizzato";
        public override string HUD_Filter => "Filtro";
        public override string HUD_Scale => "Scala";
        public override string HUD_Tags => "Tag";
        public override string HUD_ClickToCancel => "Clicca per annullare";

        public override string ObjectTag_Description => "Aggiungi un simbolo sulla mappa";
        public override string HudPins => "Puntine HUD";
        public override string HudPins_Description => "Fissa le informazioni sullo schermo";

        public override string Lobby_PlayerProfileNumbered => "Profilo {0}";
        public override string Lobby_CharacterCreationNumbered => "Personaggio {0}";
        public override string Lobby_PlayerProfileEdit => "Modifica profilo giocatore";

        public override string Editor_ConvertAnimationToLayers => "Converti animazione in livelli";
        public override string Editor_StampAllFrames => "Timbra tutti i frame";

        public override string Editor_DisplayOptions => "Opzioni di visualizzazione";
        public override string Editor_CharacterCreator => "Creatore di personaggi";
        public override string Editor_CharacterCreator_Description => "Editor dell’aspetto dei modelli militari";
        public override string Editor_HatGenre => "Modalità visualizzazione cappello";
        public override string Editor_HatGenre_FollowWeapon => "Segui arma";
        public override string Editor_HatGenre_Uniform => "Uniforme";
        public override string Editor_CopyPasteSelectedColor => "Copia dal colore selezionato";

        public override string Character_Accessories => "Accessori";
        public override string Character_Hat => "Cappello";
        public override string Character_Head => "Testa";
        public override string Character_Body => "Corpo";
        public override string Character_Arms => "Braccia";
        public override string Character_Back => "Schiena";
        public override string Character_Face => "Viso";
        public override string BuildingType_Tavern => "Sala comune";

        public override string Settings_CraftMultiplier => "Moltiplicatore tempo di produzione";
        public override string Settings_ChildMultiplier_Description => "Aumenta la velocità con cui vengono aggiunti nuovi lavoratori.";

        public override string Settings_CasualControls => "Controlli semplificati";
        public override string Settings_CasualControls_Description => "Semplifica il gameplay riducendo le scelte e le decisioni: viene usato solo l’oro come risorsa.";

        public override string Settings_AdvancedControls => "Controlli avanzati";
        public override string Settings_AdvancedControls_Description => "Esperienza completa di gestione delle risorse.";

        public override string WarsResourceGroup_Metal => "Metallo";
        public override string Work_Craft => "Produzione";
        public override string Work_OnlyCraftOnFullStock => "Produci solo con magazzino pieno";

        public override string ExperienceType_Smelting => "Fusione";
        public override string Category_Optimize => "Ottimizza";
        public override string BuildCategory_Road => "Strade";
        public override string XP_UnlockBuildPrio => "Sblocca priorità costruzione: {0}";
        public override string Technology_ModernFarming => "Agricoltura moderna";

        public override string ExportImportDescription => "Per condividere i file di salvataggio con altri giocatori, tutti i file si trovano in questa cartella: {0}";

        public override string CityCultureDescription => "La cultura fornisce un bonus speciale alla città.";

        public override string UnitType_CloseRangeRifle => "Archibugiere";
        public override string UnitType_LongRangeRifle => "Moschettiere";
        public override string UnitType_Skirmisher => "Guastatore";

        // From lumen (light)
        public override string UnitType_MithrilArcher => "Arciere Lunare";
        public override string UnitType_MithrilSwordsman => "Cavaliere Lunare";

        public override string Defence_AutoAssign_Towers => "Assegna torri automaticamente";

        public override string EventMessage_DesertersText_Food => "I soldati affamati stanno disertando dal tuo esercito.";

        public override string Tutorial_CasualRecruitSoldiers => "Acquista un gruppo di soldati.";

        // Shadow Update

        public override string Technology_CannotReassign => "La tecnologia non può essere riassegnata finché la ricerca non è completata.";

        public override string Diplomacy_DeclareWarAgainst => "Dichiara guerra a";
        public override string Diplomacy_AllyCount => "Numero di alleati";
        public override string Diplomacy_CostPerAlly => "Il costo aumenta di {0} per ogni alleato.";

        public override string Event_ChanceOfFailure => "{0}% di probabilità di fallimento";
        public override string EventMessage_Event_Title => "Evento";
        public override string EventMessage_TheCohalition => "La Coalizione";

        public override string EventMessage_DarkHorde => "Orda Oscura";
        public override string EventMessage_DarkHordeKiller_Title => "Uccisore dell’Orda Oscura";
        public override string EventMessage_DarkHordeKiller_Message => "Cavalieri campioni si sono uniti al tuo servizio.";

        public override string Settings_Mode_Spectator_Description => "Modalità spettatore – oppure intervieni con i Poteri Divini.";
        public override string GodPower => "Potere Divino";

        public override string Building_TreeSprout_Description => "Pianta un albero.";
        public override string Building_TreeSprout_Soft => "Germoglio di legno tenero";
        public override string Building_TreeSprout_Hard => "Germoglio di legno duro";

        public override string GeneralSetting_SetAll => "Applica a tutti";

        public override string Hud_All => "Tutti";
        public override string Hud_Previous => "Precedente";
        public override string Hud_EffectWillStack => "L’effetto si accumula.";

        public override string Info_WhenFoodRunsOut => "Quando il cibo finisce, città ed eserciti lo acquisteranno automaticamente dal mercato nero.";

        public override string InputActionName_NextWar => "Prossima fazione in guerra";

        /// <summary>
        /// Questi simboli abbreviati vengono usati per visualizzare numeri grandi nell’HUD.
        /// Un suggerimento spiegherà il valore completo.
        /// </summary>
        public override string EngineHud_SymbolFor100 => "c";
        public override string EngineHud_SymbolFor1000 => "k";
        public override string EngineHud_SymbolFor10000 => "10k";

        public override string GameMenu_BlockImportAchievements => "Blocca gli obiettivi nei file di salvataggio importati.";

        public override string EndScreen_PeaceVictoryQuote => "Depogliamo le spade e abbracciamo un futuro migliore.";

        public override string VictoryType_DefeatBoss => "Boss sconfitto";
        public override string VictoryType_Domination => "Dominazione";
        public override string VictoryType_WorldPeace => "Pace mondiale";

    }
}
