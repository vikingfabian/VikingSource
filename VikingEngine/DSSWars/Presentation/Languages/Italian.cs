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
        /// <summary>
        /// Name of this language
        /// </summary>
        public override string MyLanguage => "Italiano";

        /// <summary>
        /// How to display a number of items. 0: item, 1:Number
        /// </summary>
        public override string Language_ItemCountPresentation => "PH0:PH1";

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
        public override string Diplomacy_SpeakTerms_Normal => "Normal";

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Bad";

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "Nessuno";

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Forgerelazionia:PH0";

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

        public override string DifficultyDescription_AiAggression => "Aiaggressivity:PH0.";
        public override string DifficultyDescription_BossSize => "Bosssize:PH0.";
        public override string DifficultyDescription_BossEnterTime => "Bossentertempo:PH0.";
        public override string DifficultyDescription_AiEconomy => "AiEconomia:PH0%.";
        public override string DifficultyDescription_AiDelay => "Aidelay:PH0.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Diplomacydifficulty:PH0.";
        public override string DifficultyDescription_MercenaryCost => "Mercenarycosto:PH0.";
        public override string DifficultyDescription_HonorGuards => "Honorguardie:PH0.";


        /// <summary>
        /// Game has ended in success.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Vittoria!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
        {
            "Intimesdipace,wemourndead.",
            "Everytriumphcarriesashadowdisacrifice.",
            "Rememberjourneythatbroughtushere,dottedconsoulsdibrave.",
            "Ourmindsarelightfromvictory,ourheartsareheavyfromweightdifallen"
        };

        public override string EndScreen_DominationVictoryQuote => "Sono stato scelto dagli Dei per dominare il mondo!";

        /// <summary>
        /// Game has ended in failure.
        /// </summary>
        public override string EndScreen_FailTitle => "Sconfitta!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
        {
            "Conourbodiestornfrommarchingenightsdiworry,wewelcomeend.",
            "Defeatmaydarkenourlands,buttheycannotextinguishlightdiourdetermination.",
            "Extinguishflamesinourhearts,fromtheirashes,ourchildrenshallforgeanewdawn.",
            "Letourtalesbeemberthatkindlestomorrow'svictory.",
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
        /// The poem must be three lines, the fourth line will be pulled from the names translations to present the name of the boss
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
        {
            "Dreamshauntyouatnight,",
            "Aprophecydiadarkfuture",
            "Prepareperhisarrival,",
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
        public override string CityMenu_SalePricesTitle => "Saleprices";
        public override string Blueprint_Title => "Blueprint";
        public override string Resource_Tab_Overview => "Panoramica";
        public override string Resource_Tab_Stockpile => "Scorta";

        public override string Resource => "Risorsa";
        public override string Resource_StockPile_Info => "Imposta una quantità obiettivo per lo stoccaggio delle risorse; informerà i lavoratori su quando passare ad altre risorse.";
        public override string Resource_TypeName_Water => "acqua";
        public override string Resource_TypeName_Wood => "legno";
        public override string Resource_TypeName_Fuel => "combustibile";
        public override string Resource_TypeName_Stone => "pietra";
        public override string Resource_TypeName_RawFood => "cibo grezzo";
        public override string Resource_TypeName_Food => "cibo";
        public override string Resource_TypeName_Beer => "birra";
        public override string Resource_TypeName_Wheat => "grano";
        public override string Resource_TypeName_Linen => "lino";
        //public override string Resource_TypeName_SkinAndLinen => "skinelino";
        public override string Resource_TypeName_IronOre => "minerale di ferro";
        public override string Resource_TypeName_GoldOre => "minerale d’oro";
        public override string Resource_TypeName_Iron => "ferro";

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

        public override string Conscript_SpecializationTitle => "Specialization";
        public override string Conscript_SpecializationDescription => "Willincreaseattackinonearea,ereduceallothers,byPH0";
        public override string Conscript_SelectBuilding => "Selezionabarracks";

        public override string Conscript_WeaponDamage => "Armadamage:PH0";
        public override string Conscript_ArmorHealth => "Armaturahealth:PH0";
        public override string Conscript_TrainingSpeed => "Attaccavelocità:PH0";
        public override string Conscript_TrainingTime => "Addestramentotempo:PH0";

        public override string Conscript_Training_Minimal => "Minimal";
        public override string Conscript_Training_Basic => "Basic";
        public override string Conscript_Training_Skillful => "Skillful";
        public override string Conscript_Training_Professional => "Professional";

        public override string Conscript_Specialization_Field => "Openfield";
        public override string Conscript_Specialization_Sea => "Ship";
        public override string Conscript_Specialization_Siege => "Siege";
        public override string Conscript_Specialization_Traditional => "Traditional";
        public override string Conscript_Specialization_AntiCavalry => "Anticavalry";

        public override string Conscription_Status_CollectingEquipment => "Collectingequipment:PH0";
        public override string Conscription_Status_CollectingMen => "Collectingmen:PH0";
        public override string Conscription_Status_Training => "Addestramento:PH0";

        public override string ArmyHud_Food_Reserves_X => "Ciboreserves:PH0";
        public override string ArmyHud_Food_Upkeep_X => "Cibomantenimento:PH0";
        public override string ArmyHud_Food_Costs_X => "Cibocosti:PH0";

        public override string Deliver_WillSendXInfo => "WillinviaPH0atatempo";
        public override string Delivery_ListTitle => "Selezionadeliveryservice";
        public override string Delivery_DistanceX => "Distanza:PH0";
        public override string Delivery_DeliveryTimeX => "Consegnatempo:PH0";
        public override string Delivery_SenderMinimumCap => "Senderminimumcap";
        public override string Delivery_RecieverMaximumCap => "Receivermaximumcap";
        public override string Delivery_ItemsReady => "Itemsready";
        public override string Delivery_RecieverReady => "Receiverready";
        public override string Hud_ThisCity => "Thiscittà";
        public override string Hud_RecieveingCity => "Receivingcittà";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "DisplayedinRisorsaAlSecondo.";

        public override string Info_MinuteAverage => "valueisanaveragefromlastminuto";

        public override string Message_OutOfFood_Title => "Outdicibo";
        public override string Message_CityOutOfFood_Text => "Expensivecibowillbepurchasedfromblackmarket.Lavoratoriwillstarvewhenyourmoneyrunsout.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Terraintype";

        public override string Hud_EnergyUpkeepX => "CiboenergymantenimentoPH0";

        public override string Hud_EnergyAmount => "PH0energy(secondidiwork)";

        public override string Hud_CopySetup => "Copysetup";
        public override string Hud_Paste => "Paste";

        public override string Hud_Available => "Available";

        public override string WorkForce_ChildBirthRequirements => "Childbirthrequirements";
        public override string WorkForce_AvailableHomes => "Availablehomes:PH0";
        
        /// <summary>
        /// workers require peace to grow(make babies)
        /// </summary>
        public override string WorkForce_Peace => "Pace";
        public override string WorkForce_ChildToManTime => "Grownupage:PH0minuti";

        public override string Economy_TaxIncome => "Taxentrate:PH0";
        public override string Economy_ImportCostsForResource => "ImportcostiperPH0:PH1";
        public override string Economy_BlackMarketCostsForResource => "NeromarketcostiperPH0:PH1";
        public override string Economy_GuardUpkeep => "Guardiamantenimento:PH0";

        public override string Economy_LocalCityTrade_Export => "Cittàtradeexport:PH0";
        public override string Economy_LocalCityTrade_Import => "Cittàtradeimport:PH0";

        public override string Economy_ResourceProduction => "PH0produzione:PH1";
        public override string Economy_ResourceSpending => "PH0spesa:PH1";

        public override string Economy_TaxDescription => "TaxisPH0oroallavoratore";

        public override string Economy_SoldResources => "Soldresources(oroore):PH0";

        public override string UnitType_Cities => "Città";
        public override string UnitType_Armies => "Eserciti";
        public override string UnitType_Worker => "Lavoratore";

        public override string UnitType_FootKnight => "Longswordknight";
        public override string UnitType_CavalryKnight => "Cavalryknight";

        public override string CityCulture_LargeFamilies => "Grandefamilies";
        public override string CityCulture_FertileGround => "Fertilegrounds";
        public override string CityCulture_Archers => "Skilledarchers";
        public override string CityCulture_Warriors => "Warriors";
        public override string CityCulture_AnimalBreeder => "Animalbreeders";
        public override string CityCulture_Miners => "Miners";
        public override string CityCulture_Woodcutters => "Lumbermen";
        public override string CityCulture_Builders => "Builders";

        /// <summary>
        /// Crab mentality: culture where you suppress those who are better at something
        /// </summary>
        public override string CityCulture_CrabMentality => "Crabmentality";
        public override string CityCulture_DeepWell => "Deepwell";
        public override string CityCulture_Networker => "Networker";

        /// <summary>
        /// Pit master: someone who is good at burning work (char coal) 
        /// </summary>
        public override string CityCulture_PitMasters => "Pitmasters";

        public override string CityCulture_CultureIsX => "Culture:PH0";
        public override string CityCulture_LargeFamilies_Description => "Increasedchildbirth";
        public override string CityCulture_FertileGround_Description => "Cropsgivemore";
        public override string CityCulture_Archers_Description => "Producesskilledarchers";
        public override string CityCulture_Warriors_Description => "Producesskilledmeleefighters";
        public override string CityCulture_AnimalBreeder_Description => "Animalsgivemoreresources";
        public override string CityCulture_Miners_Description => "Minesmoreore";
        public override string CityCulture_Woodcutters_Description => "Treesgivemorelegno";
        public override string CityCulture_Builders_Description => "Fastatbuilding";
        public override string CityCulture_CrabMentality_Description => "Lavorocostilessenergy.Cannotproducehigh-skillsoldati.";
        public override string CityCulture_DeepWell_Description => "Acquareplenishesfaster";
        public override string CityCulture_Networker_Description => "Efficientpostalservice";
        public override string CityCulture_PitMasters_Description => "Highercombustibileproduzione";

        public override string CityOption_AutoBuild_Work => "Autoespandiworkforce";
        public override string CityOption_AutoBuild_Farm => "Autoespandifarms";

        public override string Hud_PurchaseTitle_Resources => "Buyresources";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "Youown";

        public override string Tutorial_EndTutorial => "Endtutorial";
        public override string Tutorial_MissionX => "MissionPH0";
        public override string Tutorial_CollectXAmountOfY => "CollectPH0PH1";
        public override string Tutorial_SelectTabX => "Selezionatab:PH0";
        public override string Tutorial_IncreasePriorityOnX => "Increasepriorityon:PH0";
        public override string Tutorial_PlaceBuildOrder => "Placecostruisciorder:PH0";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Selezionaacittà";
        public override string Tutorial_ZoomInWorkers => "Zoominaseelavoratori";
        public override string Tutorial_CreateSoldiers => "Createtwosoldatounitsconthisequipment:PH0.PH1.";
        public override string Tutorial_ZoomOutOverview => "Zoomout,amappaoverview";
        public override string Tutorial_ZoomOutDiplomacy => "Zoomout,adiplomacyview";
        public override string Tutorial_ImproveRelations => "Improveyourrelazioniconaneighborfaction";
        public override string Tutorial_MissionComplete_Title => "Missioncompleto!";
        public override string Tutorial_MissionComplete_Unlocks => "Newcontrolshavebeenunlocked";

        //patch1
        public override string Resource_ReachedStockpile => "Reachedstockpilegoalbuffer";

        public override string BuildingType_ResourceMine => "PH0mine";

        public override string Resource_TypeName_BogIron => "Bogferro";

        public override string Resource_TypeName_Coal => "Carbone";

        public override string Language_XUpkeepIsY => "PH0mantenimento:PH1";
        public override string Language_XCountIsY => "PH0count:PH1";

        public override string Message_ArmyOutOfFood_Text => "Expensivecibowillbepurchasedfromblackmarket.Hungrysoldatiwilldesertwhenyourmoneyrunsout.";

        public override string Info_ArmyFood => "Esercitiwillrestockcibofromclosestfriendlycittà.Cibocanbepurchasedfromotherfactions.Inhostileregions,cibocanonlybepurchasedfromblackmarket.";

        public override string FactionName_Monger => "Monger";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

        //patch2
        public override string Tutorial_BuildSomething => "CostruiscisomethingthatproducesPH0";
        public override string Tutorial_BuildCraft => "Costruisciacraftingstationper:PH0";
        public override string Tutorial_IncreaseBufferLimit => "Increasebufferlimitper:PH0";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "ReachastockpilediPH0PH1";
        public override string Tutorial_LookAtFoodBlueprint => "Lookatciboblueprint";
        public override string Tutorial_CollectFood_Info1 => "lavoratoriwillwalkacittàhallaeat";
        public override string Tutorial_CollectFood_Info2 => "esercitosendstrosslavoratoriacollectcibo";
        public override string Tutorial_CollectFood_Info0 => "Wantfullcontroldilavoratori?Setallworkprioritiesazero,ethenjustactivateoneatatempo.";

        public override string EndGameStatistics_DecorsBuilt => "Decorationsbuilt:PH0";
        public override string EndGameStatistics_StatuesBuilt => "Statuesbuilt:PH0";


        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "Bydefault,lavoratorigoacittàhallaeatordropoffitems";
        public override string GameMenu_UseSpeedX => "PH0velocitàoption";
        public override string GameMenu_LongerBuildQueue => "Extendedcostruisciqueue";

        public override string Diplomacy_RelationWithOthers => "Le loro relazioni con gli altri";
        public override string Automation_queue_description => "Willkeeprepeatinguntilqueueisempty";

        public override string BuildingType_Storehouse_Description => "Lavoratorimaydropitemshere";

        public override string Resource_TypeName_Longbow => "longbow";
        public override string Resource_TypeName_Rapeseed => "rapeseed";
        public override string Resource_TypeName_Hemp => "hemp";

        public override string Resource_BogIronDescription => "Estrazioneferroismoreefficientthanusingbogferro.";


        public override string Resource_FoodSafeGuard_Description => "Safeguardia.Willmaximizeprioritydiciboproduzionechain,ifitfallsbelowPH0.";
        public override string Resource_FoodSafeGuard_Active => "Safeguardiaisactive.";

        public override string GameMenu_NextSong => "Nextsong";

        public override string BuildingType_Bank => "Bank";
        public override string BuildingType_GoldDelivery_Description => "Inviaoroaothercittà";

        public override string BuildingType_Logistics => "Logistics";
        public override string BuildingType_Logistics_Description => "Potenziayourabilityaorderbuildings";

        public override string BuildingType_Logistics_NationSizeRequirement => "Nationtotalworkforce:PH0";
        public override string Requirements_XItemStorageOfY => "CittàPH0storagedi:PH1";


        public override string XP_UnlockBuildQueue => "Unlockcostruisciqueuea:PH0";
        public override string XP_UnlockBuilding => "Unlockbuilding:";
        public override string XP_Upgrade => "Potenzia";

        public override string XP_UpgradeBuildingX => "Potenziabuilding:PH0";

        /// <summary>
        /// Title for describing the production cycle of farms
        /// </summary>
        public override string BuildHud_PerCycle => "Alcycle";
        public override string BuildHud_MayCraft => "Maycraft";
        public override string BuildHud_WorkTime => "Lavorotempo:PH0";
        public override string BuildHud_GrowTime => "Growtempo:PH0";
        public override string BuildHud_Produce => "Produce:";

        public override string BuildHud_Queue => "Allowedcostruisciqueue:PH0/PH1";

        public override string LandType_Flatland => "Flatland";
        public override string LandType_Water => "Acqua";
        public override string BuildingType_Wall => "Muro";
        public override string Delivery_AutoReciever_Description => "Willinviaacittàconlowestamountdiresources";

        public override string Hud_On => "On";
        public override string Hud_Off => "Off";

        public override string Hud_Time_Seconds => "PH0secondi";
        public override string Hud_Time_Minutes => "PH0minuti";
        public override string Hud_Undo => "Undo";
        public override string Hud_Redo => "Redo";

        public override string Tag_ViewOnMap => "Viewtagsonmappa";

        public override string MenuTab_Tag => "Tag";

        public override string Input_Build => "Costruisci";

        public override string FlagEditor_ClearAll => "Clearall";


        public override string CityCulture_Stonemason => "Stonemason";
        public override string CityCulture_Stonemason_Description => "Improvedpietracollecting";

        public override string CityCulture_Brewmaster => "Brewmaster";
        public override string CityCulture_Brewmaster_Description => "Enhancedbirraproduzione";

        public override string CityCulture_Weavers => "Weavers";
        public override string CityCulture_Weavers_Description => "Enhancedlightarmaturaproduzione";

        public override string CityCulture_SiegeEngineer => "Siegeengineer";
        public override string CityCulture_SiegeEngineer_Description => "Morepowerfulwarmachines";

        public override string CityCulture_Armorsmith => "Armorsmith";
        public override string CityCulture_Armorsmith_Description => "Improvedferroarmaturaproduzione";

        public override string CityCulture_Noblemen => "Noblemen";
        public override string CityCulture_Noblemen_Description => "Morepowerfulknights";

        public override string CityCulture_Seafaring => "Seafaring";
        public override string CityCulture_Seafaring_Description => "Soldaticonseaspecialzation,havestrongerships";

        public override string CityCulture_Backtrader => "Backtrader";
        public override string CityCulture_Backtrader_Description => "Cheaperblackmarket";

        public override string CityCulture_LawAbiding => "Law-abiding";
        public override string CityCulture_LawAbiding_Description => "Guadagnomoretax.Noblackmarket.";

        //##2##

        public override string Hud_Advanced => "Advanced";
        public override string Hud_Loading => "Caricamento...";

        public override string CityOption_LowerGuardSize => "Releaseguardia";
        public override string Hud_Purchase_MinCapacity => "Minimumcapacityreached";
        public override string Settings_ResetToDefault => "Resetadefault";
        public override string Settings_NewGame => "Newgame";

        public override string Settings_AdvancedGameSettings => "AdvancedGameSettings";
        public override string Settings_FoodMultiplier => "Cibomultiplier";
        public override string Settings_FoodMultiplier_Description => "Howlongalavoratoreorsoldatolastsonafullstomach.Ahighvaluewilllowercomputerperformance.";

        public override string Settings_GameMode => "Gamemode";

        public override string Settings_Mode_Story => "Fullstory";
        public override string Settings_Mode_IncludeBoss => "IncludeBossEvents.";
        public override string Settings_Mode_IncludeAttacks => "IncludeRandomAttacks.";
        public override string Settings_Mode_Sandbox => "Sandbox";
        public override string Settings_Mode_Peaceful => "Peaceful";
        public override string Settings_Mode_Peaceful_Description => "Allwarsareinitiatedbyplayer";

        public override string Lobby_ImportSave => "Importsave";

        public override string Lobby_ExportSave => "Exportsave";
        public override string Lobby_ExportSave_Description => "Createsacopydifileeplacesitinimportfolder:PH0";

        public override string Resource_CurrentAmount => "Currentamount:PH0";
        public override string Resource_MaxAmount_Soft => "SoftCap(MaxLimit):PH0";
        public override string Resource_MaxAmount => "Maxlimit:PH0";
        public override string Resource_AddPerSec => "IncreaseRate:PH0alsecondo";

        public override string Resource_WaterAddLimit => "Acquaincreaseratecan'tbealtered";

        public override string Tutorial_Select_SubTab => "Eselectcategory:PH0";



        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */


        public override string Tutorial_OpenGuardSubTab => "Openabarrackseselectcategory:PH0";
        public override string Tutorial_GuardToWall => "Moveaguardiaaawall";
        public override string Demo_MissionObjective_Title => "MissionObjective";
        public override string Demo_MissionObjective_Description => "Defendagainstattackfromsouth";
        public override string Demo_Complete_Title => "Democompleto";
        public override string Demo_TimesUp_Title => "Tempo'sup!";
        public override string Demo_EndInOneMinuteDescription => "demowillendinoneminuto";

        public override string ArmyOption_NewArmy => "Newesercito";
        public override string ProfileEditor_AltMain => "Alternativemain";
        public override string Automation_CheckBoxTitle => "Automated";

        public override string ArmyStructure_ColumnWidth => "Esercitocolumnwidth";
        public override string ArmyStructure_ArmyPlacement => "Placementinesercito";
        public override string ArmyStructure_Row_Front => "Front";
        public override string ArmyStructure_Row_Body => "Body";
        public override string ArmyStructure_Row_Second => "Secondo";
        public override string ArmyStructure_Row_Behind => "Behind";

        public override string Diplomacy_RelationType_Enemies => "Enemies";

        public override string EventMessage_EnemyAlliance_Title => "FeardiDomination";
        public override string EventMessage_EnemyAlliance => "nations,fearingyourgrowingpower,uniteinanallianceagainstyou.";

        public override string Settings_CentralGold => "Centraloro";
        public override string Settings_CentralGold_Description => "On:allyouroroisinasharedpoolperinstantuse.Off:oroisphysicaleneedsabetransported.";





        public override string InputActionName_StopStart => "Stop/Start";
        public override string InputActionName_ToggleHudDetail => "ToggleHUDDetail";
        public override string InputActionName_NextCity => "NextCittà";
        public override string InputActionName_NextArmy => "NextEsercito";
        public override string InputActionName_NextBattle => "NextBattaglia";
        public override string InputActionName_Build => "Costruisci";
        public override string InputActionName_Copy => "Copy";
        public override string InputActionName_Paste => "Paste";
        public override string InputActionName_Menu => "Menu";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "PreviousColor";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "NextColor";
        public override string InputActionName_FlagDesign_PaintBucket => "PaintSecchiello";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "ColorPicker";
        public override string InputActionName_ControllerFocus => "Focus";
        public override string InputActionName_ControllerCancel => "Annulla";
        public override string InputActionName_ControllerMessageClick => "MessageClick";
        public override string InputActionName_ControllerSelect => "Seleziona";
        public override string InputActionName_WASD_UP => "Su";
        public override string InputActionName_WASD_DOWN => "Giù";
        public override string InputActionName_WASD_LEFT => "Sinistra";
        public override string InputActionName_WASD_RIGHT => "Destra";
        public override string InputActionName_CameraTiltLeft => "CameraTiltSinistra";
        public override string InputActionName_CameraTiltRight => "CameraTiltDestra";
        public override string InputActionName_CameraTiltUp => "CameraTiltSu";
        public override string InputActionName_ZoomInKey => "ZoomIn";
        public override string InputActionName_ZoomOutKey => "ZoomOut";




        public override string Settings_Title_Monitor => "Monitoroptions";
        public override string Settings_Title_Graphics => "Graphicoptions";
        public override string Settings_Title_Input => "Input";
        public override string Settings_Title_Gameplay => "Gameplayoptions";
        public override string Settings_PanOnZoom => "Panoramicaonzoom";
        public override string Settings_ScrollSensitivity_Game => "Pergamenasensitivity:game";
        public override string Settings_ScrollSensitivity_Menu => "Pergamenasensitivity:menu";
        public override string Settings_Blood => "Blood";

        public override string Settings_MasterVolume => "MasterVolume";
        public override string Settings_AmbienceVolume => "AmbienceVolume";
        public override string Settings_BattleMelody => "BattagliaMelody";

        public override string Settings_ModelLight => "Modellighteffect";
        public override string Settings_Particles => "Particleeffects";
        public override string Settings_MapLoadSpeed => "Mappacaricamentovelocità";
        public override string Lobby_Category_Options => "Opzioni";
        public override string Lobby_Category_Editor => "Editor";
        public override string Lobby_Category_ExtraModes => "Extramodes";

        public override string Lobby_Editor_MapEditor => "Mappaeditor";
        public override string Lobby_Editor_VoxelEditor => "Voxeleditor";

        public override string Lobby_Mode_BattleLab => "Battaglialab";
        public override string Lobby_Mode_BattleLab_Description => "Pitqualunquesoldatiagainsteachother";
        public override string Lobby_Mode_Commander => "PlayCommander";
        public override string Lobby_Mode_Commander_Description => "Asmalltacticalboardgame";
        public override string Lobby_MusicPlayList => "Musicplaylist";

        public override string Lobby_GameSetup => "Gamesetup";
        public override string Lobby_PlayerSetup => "Playersetup";
        public override string LobbyDemoMode_Demo => "Demo";

        public override string Lobby_Tutorial => "Tutorial";
        
        public override string LobbyDemoMode_ShortTutorial => "QuickTutorial";
        public override string LobbyDemoMode_LongTutorial => "ExtendedTutorial";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "Wishliston";


        public override string BattleLab_StartHere => "Startbattagliahere";
        public override string BattleLab_Start => "Startbattaglia";
        public override string BattleLab_Attacker => "Attacker";



        public override string MapGenerator_Name => "Mappaeditor-generate";

        public override string MapType_CustomMap => "CustomMappa";
        public override string MapType_GenerateNewMap => "Generateanewmappa";
        public override string MapGenerator_GenerateAction => "Generate";
        public override string MapGenerator_Terrain_CustomSize => "Customsize";
        public override string MapGenerator_Terrain_StartAs => "Startas";
        public override string MapGenerator_Terrain_ClearPass => "RunClearPass";
        public override string MapGenerator_Terrain_BuildPass => "RunCostruisciPass";
        public override string MapGenerator_Terrain_DigPass => "RunDigPass";
        public override string MapGenerator_Terrain_BuildDigLoops => "Costruisci-Digloopcount";
        public override string MapGenerator_Terrain_BuildStrokes => "Costruiscistrokescount";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Measuredinpaintstrokesal100tiles";
        public override string MapGenerator_Terrain_DigStrokes => "Digstrokescount";
        public override string MapGenerator_Terrain_CleanUp_Option => "Cleanupdisingletiles";
        public override string MapGenerator_Terrain_CleanUpPass => "RuncleanupPass";



        public override string Economy_ServicemenUpkeep => "Servicemenmantenimento:PH0";
        public override string Economy_ServicemenUpkeep_Description => "MantenimentoisPH0oroalserviceman";
        public override string Economy_GuardUpkeep_Description => "MantenimentoisPH0oroalguardia";

        public override string EndScreen_TimeHasEndedTitle => "Tempo'sup";

        public override string Hud_AdvancedSettings => "Advancedsettings";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "Annulla";
        public override string Hud_Delete => "Delete";
        public override string Hud_Next => "Next";
        //public override string Hud_None => "Nessuno";
        public override string Hud_Apply => "Apply";
        public override string Hud_AllCities => "Allcittà";
        public override string Hud_Time_Hours => "PH0ore";
        public override string Hud_AddX => "AggiungiPH0";
        public override string Hud_Both => "Both";
        public override string Hud_Direction => "Direction";
        public override string MusicIsBroken => "Musiciscurrentlybroken";


        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => "PH0,count:PH1";

        public override string Hud_EffectDoesNotStack => "Thiseffectdoesnotstack";

        public override string Work_SmeltX => "SmeltPH0";

        public override string Info_TotalFoodProduction => "Totalciboproduzione";
        public override string Info_TotalFoodSpending => "Totalcibospesa";

        public override string Info_FooodAndDeliveryLocation => "Bydefault,lavoratorigoacittàhallaeatordropoffitems";
        
        public override string Delivery_SendChunk => "ItemsalConsegna";
        public override string Delivery_SpeedBonus => "Velocitàbonus:PH0%";

        public override string Delivery_AutoResourceDescription => "Deliversitemsthathasreachedstockpilelimit,acittàinneed.";

        public override string Conscript_Soldiers_ArmyType => "Esercitomen";
        public override string Conscript_Soldiers_ArmyType_Description => "Reclutasoldatiaanadjacentesercito";
        public override string Conscript_Soldiers_GuardType => "Cittàguardia";
        public override string Conscript_Soldiers_GuardType_Description => "Guardieareusedafortifywalls";
        //-
        public override string Defence_Title => "Defence";
        public override string Defence_GuardPost => "Guardiapost";

        public override string Defence_WallDescription_Movement => "Hindersenemymovement.";
        public override string Defence_WallDescription_GuardPost => "Guardiacanbepostedhere.";
        public override string Defence_AutoAssign => "Autoassign";
        public override string Defence_AutoAssign_Description => "Newguardiewillmoveathispost";
        public override string Conscript_SplashDamage => "Splashdamage";
        public override string Conscript_HighSplashDamage => "Altosplashdamage";

        public override string Conscript_Training_Champion => "Champion";
        public override string Conscript_Training_Legendary => "Leggendari";


        public override string Experience_Title => "Experience";
        public override string Experience_TopExperience => "Topexperiencelevels";

        public override string Experience_TimeReductionDescription => "LavorotempoisreducedbyPH0%allevel";

        public override string ExperienceType_Farm => "Farmer";
        public override string ExperienceType_AnimalCare => "Animalcare";
        public override string ExperienceType_HouseBuilding => "Housebuilder";
        public override string ExperienceType_WoodWork => "Legnolavoratore";
        public override string ExperienceType_StoneCutter => "Pietracutter";
        public override string ExperienceType_Mining => "Miner";
        public override string ExperienceType_Transport => "Transport";
        public override string ExperienceType_Cook => "Cucina";
        public override string ExperienceType_Fletcher => "Fletcher";
        public override string ExperienceType_RefineOre => "Smelter";
        public override string ExperienceType_Casting => "Casting";
        public override string ExperienceType_CraftMetal => "Fabbro";
        public override string ExperienceType_CraftArmor => "Armorer";
        public override string ExperienceType_CraftWeapon => "Armasmith";
        public override string ExperienceType_CraftFuel => "Collier";
        public override string ExperienceType_Chemist => "Chemist";

        public override string ExperienceLevel_1 => "Beginner";
        public override string ExperienceLevel_2 => "Practitioner";
        public override string ExperienceLevel_3 => "Expert";
        public override string ExperienceLevel_4 => "Master";
        public override string ExperienceLevel_5 => "Leggendari";

        public override string ExperenceOrDistancePrio_Title => "Lavoratoreselection";
        public override string ExperenceOrDistancePrio_Description => "Inattivolavoratoriwillbeselectedaworkeitherbydistanzaorexperience";


        public override string Technology_Description => "Eachcittàhasatechnologytree.Eachtechnologywillunlockbuildingseitems.";
        public override string Experience_Description => "Lavoratoriwillguadagnoexperienceeimprove";


        public override string Technology_Title => "Technology";
        public override string Technology_ShareField => "Sharingtechnologyfield";

        public override string Technology_GainByNeigborRelation => "Pereachneighborcittàcontechnology.EyourrelazioneisPH0:PH1";
        public override string Technology_ForEachMaster => "WhenaPH0reachesanexperienceleveldiPH1,intechnologyfield:PH2";
        public override string Technology_CitySpread => "Yourcittàwillsharetechnologywhenadjacent:PH0";
        public override string Technology_CityCapture => "Mosttechnolgiesaredestroyedwhenacittàiscapturedinbattaglia";

        public override string Technology_AdvancedBuildings => "Advancedbuildings";
        public override string Technology_AdvancedFarming => "Advancedfarming";
        public override string Technology_AdvancedCasting => "Advancedcasting";

        public override string Help_Title => "Help";
        public override string Help_Work_Title => "Lavorodoesn'tstart";
        public override string Help_Work_Resources => "Buildingsneedavailableresources";
        public override string Help_Work_Skill => "lavoratoreneedscorrectskilllevel(orhigher)";
        public override string Help_Work_Stockpile => "Risorsacollectionwillbeblockedbyafullstockpile";
        public override string Help_Work_Priority => "Lavoromayhaveloworzeropriority";


        public override string Help_Soldiers_Title => "Producesoldati";
        public override string Help_Soldiers_PlaceBuildingX => "Placebuilding:PH0";
        public override string Help_Soldiers_Workers => "Availablelavoratoriarecruit";
        public override string Help_Soldiers_Weapon => "Aarmapereachsoldato";
        public override string Help_Soldiers_StartX => "Start:PH0";


        public override string Hud_SelectHistory => "Selezionahistory";

        public override string Hud_PointsPerMinute => "PH0pointsalminuto";
        public override string Hud_PercentValueCost => "servicecostiPH0%divalue";

        public override string Hud_Mixed => "Mixed";
        public override string Hud_Distance => "Distanza";

        public override string Hud_Unlock => "Unlock";
        public override string Hud_category => "Category";

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public override string Input_StepOneFrame => "Step1frame";

        public override string Resource_TypeName_Wagon2Wheel => "Piccolawagon";
        public override string Resource_TypeName_Wagon4Wheel => "Grandewagon";
        public override string Resource_TypeName_Tin => "Tin";
        public override string Resource_TypeName_TinOre => "Tinore";

        public override string Resource_TypeName_Copper => "Rame";
        public override string Resource_TypeName_CopperOre => "Rameore";
        public override string Resource_TypeName_SilverOre => "Argentoore";
        public override string Resource_TypeName_Silver => "Argento";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Rawmithril";
        public override string Resource_TypeName_Mithril => "Mithril";

        public override string Resource_TypeName_BronzeSword => "Bronzosword";
        public override string Resource_TypeName_ShortSword => "Shortsword";
        public override string Resource_TypeName_LongSword => "Longsword";
        public override string Resource_TypeName_HandSpear => "Handspear";
        public override string Resource_TypeName_Warhammer => "Warhammer";
        public override string Resource_TypeName_MithrilSword => "Mithrilsword";
        public override string Resource_TypeName_SlingShot => "Slingshot";
        public override string Resource_TypeName_ThrowingSpear => "Javelin";
        public override string Resource_TypeName_Crossbow => "Balestriere";
        public override string Resource_TypeName_MithrilBow => "Mithrilbow";

        public override string Resource_TypeName_CoolingFluid => "Coolingfluid";
        public override string Resource_TypeName_Palisade => "Palisade";
        public override string Resource_TypeName_Toolkit => "Toolkit";

        public override string Resource_TypeName_Sulfur => "Sulfur";
        public override string Resource_TypeName_LeadOre => "Leadore";
        public override string Resource_TypeName_Lead => "Lead";
        public override string Resource_TypeName_Bronze => "Bronzo";
        public override string Resource_TypeName_BloomIron => "Bloomeryferro";
        public override string Resource_TypeName_Steel => "Steel";
        public override string Resource_TypeName_CastIron => "Castferro";

        public override string Resource_TypeName_BlackPowder => "Neropowder";
        public override string Resource_TypeName_GunPowder => "Gunpowder";
        public override string Resource_TypeName_LedBullet => "Bullet";

        public override string Resource_TypeName_HandCannon => "Handcannon";
        public override string Resource_TypeName_HandCulverin => "Handculverin";
        public override string Resource_TypeName_Rifle => "Rifle";
        public override string Resource_TypeName_Blunderbuss => "Blunderbuss";

        public override string Resource_TypeName_Manuballista => "Manuballista";
        public override string Resource_TypeName_Catapult => "Catapult";
        public override string Resource_TypeName_BatteringRam => "BatteringRam";
        public override string Resource_TypeName_SiegeCannonBronze => "Basilic";
        public override string Resource_TypeName_ManCannonBronze => "Bombard";
        public override string Resource_TypeName_SiegeCannonIron => "Haubitz";
        public override string Resource_TypeName_ManCannonIron => "Cannon";

        public override string Resource_TypeName_PaddedArmor => "Paddedarmatura";
        public override string Resource_TypeName_HeavyPaddedArmor => "Heavypaddedarmatura";

        public override string Resource_TypeName_IronArmor => "Mailarmatura";
        public override string Resource_TypeName_HeavyIronArmor => "Heavymailarmatura";

        public override string Resource_TypeName_BronzeArmor => "Bronzoarmatura";

        public override string Resource_TypeName_LightPlateArmor => "Platearmatura";
        public override string Resource_TypeName_FullPlateArmor => "Fullplatearmatura";
        public override string Resource_TypeName_MithrilArmor => "Mithrilarmatura";
        public override string Resource_TypeName_Coin => "Coin";

        public override string UnitType_Warhammer => "Martelloknight";
        
        public override string UnitType_SpearAndShield => "Lineman";

        public override string UnitType_CollectionOfSoldiers => "SoldatoBundle";
        public override string UnitType_CollectionOfArmies => "EsercitoBundle";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => "(idPH0)";

        public override string BuildHud_AreaEffectTitle => "Areaeffect";
        public override string BuildHud_BonusRadius => "Bonusradius:PH0";

        public override string BuildHud_BuildTime => "Costruiscitempo";
        public override string SchoolHud_ToLevel => "Alevel";
        public override string SchoolHud_TimeDescription => "Tempoassumeszeroexperience;itdecreasesconexperience.";
        public override string SchoolHud_SelectSchool => "Selezionaschool";
        public override string Upgrade_Order => "Potenziaorder";

        public override string Building_ListDescription => "Alistdiallbuildingsinthiscategory";

        public override string BuildingType_IsUpgraded => "PH0-upgraded";
        public override string BuildingType_WoodCutter => "Lumbermill";
        public override string BuildingType_Workshop_Description => "Improvesworkinarea";

        public override string BuildingType_WoodCutter_AreaAffect => "GuadagnoPH0%morelegnofromtrees";

        public override string BuildingType_StoneCutter_AreaAffect => "GuadagnoPH0%morepietra";

        public override string BuildingType_StoneCutter => "Pietraquarry";

        public override string BuildingType_Embassy => "Embassy";
        public override string BuildingType_Embassy_Description => "Perdiplomaticrelazioni";

        public override string BuildingType_SoldierBarracks => "Soldatobarracks";
        public override string BuildingType_ArcherBarracks => "Arcierebarracks";
        public override string BuildingType_WarmachineBarracks => "Warmachinebarracks";
        public override string BuildingType_GunBarracks => "Gunbarracks";
        public override string BuildingType_CannonBarracks => "Cannonbarracks";
        public override string BuildingType_KnightsBarracks => "Cavalieribarracks";

        public override string BuildingType_WaterResovoir => "Acquareservoir";
        public override string BuildingType_WaterResovoir_Description => "Increasesacquastorage";

        public override string BuildingType_SmeltingFurnace => "Smeltingfurnace";
        public override string BuildingType_SmeltingFurnace_Description => "Purifyoreametal";

        public override string BuildingType_Foundry => "Foundry";
        public override string BuildingType_Foundry_Description => "Metalcastingstation";

        public override string BuildingType_Armory => "Armory";
        public override string BuildingType_Armory_Description => "Armaturacraftingstation";
        public override string BuildingType_Chemist => "Chemist";
        public override string BuildingType_Chemist_Description => "Chemicalscraftingstation";
        public override string BuildingType_CoinMaker => "Coinminter";
        public override string BuildingType_CoinMaker_Description => "Turnmetalsamoney";
        public override string BuildingType_Gunmaker => "Gunmaker";
        public override string BuildingType_Gunmaker_Description => "Craftingstationpergunsecannons";

        public override string BuildingType_School_Tab => "School";
        public override string BuildingType_School => "Mastersguild";
        public override string BuildingType_School_Description => "Increaseskillleveldilavoratori";

        public override string BuildingType_GoldDelivery => "Orocourier";
        public override string BuildingType_Bank_Description => "Oromanagement";

        public override string DecorType_CobbleStones => "Cobblestones";
        public override string DecorType_Square => "Cittàsquare";

        public override string DecorType_Garden => "Giardino";
        public override string DecorType_Flag => "Flag";
        public override string DecorType_Banner => "Stendardo";

        public override string BuildingType_DirtRoad => "Dirtroad";
        public override string BuildingType_Palisade => "PalisadeFort";

        public override string ResourceType_ServiceMen => "Servicemen";
        public override string BuildingType_ServiceHouse => "Servicehouse";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "AggiungiPH0servicemen";

        public override string BuildingType_GuardOffice => "Guardieoffice";
        public override string BuildingType_GuardOffice_DescriptionAddX => "IncreaseguardialimitbyPH0";

        public override string BuildingType_DirtWall => "Dirtwall";
        public override string BuildingType_DirtTower => "Dirttower";
        public override string BuildingType_WoodWall => "Legnowall";
        public override string BuildingType_WoodTower => "Legnotower";
        public override string BuildingType_StoneWall => "Pietrawall";
        public override string BuildingType_StoneTower => "Pietratower";
        public override string BuildingType_StoneGate => "Pietragate";
        public override string BuildingType_StoneHouse => "Pietragate";


        /// <summary>
        /// When listing slight variations, like "LampA" and "LampB"
        /// </summary>
        public override string VariantType_A => "PH0A";
        public override string VariantType_B => "PH0B";
        public override string VariantType_C => "PH0C";
        public override string VariantType_D => "PH0D";
        public override string VariantType_E => "PH0E";
        public override string VariantType_F => "PH0F";
        public override string VariantType_G => "PH0G";
        public override string VariantType_H => "PH0H";

        public override string BuildingToolShape_Free => "Pen";
        public override string BuildingToolShape_Area => "Rectangle";
        public override string BuildingToolShape_Line => "Line";
        public override string BuildingToolShape_LShape => "L-shape";


        public override string CityHall_Upgrade => "Potenziacittàhall";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Maxsupportedlavoratori:PH0";

        public override string CityHall_Size_Small => "Villaggio";
        public override string CityHall_Size_Medium => "Town";
        public override string CityHall_Size_Large => "Capital";

        public override string GuardHousingCount => "Guardiaofficehousing";
        public override string ServicemenCount => "Servicemen:PH0";


        public override string Work_MiningResource => "EstrazionePH0";

        public override string MenuTab_Progress => "Progress";

        public override string Automation_AutomateCity => "Automatecittà";
        public override string Automation_AutomationFocus => "Automazionefocus";
        public override string Automation_AutomationFocus_Grow => "Grow";
        public override string Automation_AutomationFocus_Export => "Export";
        public override string Automation_AutomationFocus_War => "Guerra";

        public override string CityCulture_Smelters_Description => "Improvedoresmelting";
        public override string CityCulture_Smelters => "Smelters";

        public override string CityCulture_Apprentices_Description => "Newlavoratoriwillguadagnoexperiencefromactivelavoratori";
        public override string CityCulture_Apprentices => "Apprentices";

        public override string CityCulture_BronzeCasters_Description => "Improvedproduzionedibronzeebronzeitems";
        public override string CityCulture_BronzeCasters => "Bronzocasters";

        //DEMO PATCH 1

        /// <summary>
        /// Evil orcs that roam on the map
        /// </summary>
        public override string FactionName_Barbarian => "Oscurihorde";
        public override string Tutorial_AttackAndDestroyX => "Attaccaedistruggi:PH0";
        public override string Resource_TypeName_Pike => "Pike";


        public override string BattleTrials_Title => "BattagliaTrials";
        public override string BattleTrials_Description => "Testyourtacticsinadirectesercito-versus-esercitoencounter.";


        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "Theseattacksreduceblockchance";

        public override string Conscript_BlockPerSecond => "MayblockPH0timesalsecondo";

        public override string Conscript_BlockDescription => "Soldatiwillblockmostattackscomingfromtheirforwardarc";

        public override string Map_CustomSeed => "Mappaseed";

        public override string Settings_Mode_Spectator => "Spectator";

        //public override string Settings_Mode_Spectator_Description => "Justwatch";

        public override string Automation_AutomationFocus_NoFocus_Description => "Willcostruiscialittlebitdieverything";

        public override string Automation_AutomationFocus_WillProduce => "Willmainlyproduce:";

        public override string Help_Food_WhoEats => "Allsoldatielavoratoriconsumecibo";

        public override string Help_Food_BigArmy => "Alargeesercitocanstarveoutcittàinitsarea";

        public override string Help_Food_DontBuild => "Edificiomorefarmsdoesn'tautomaticallyincreasecibo;youneedavailablelavoratoriecookstationsagathereprocessit";

        public override string Help_Food_UseWater => "Ciboproduzionerequiresacqua";

        public override string Help_Food_Postal => "Makesureyourcittàsupporteachotherbysendingcibo";

        public override string Message_LostCity => "Cittàlost";

        public override string Demo_Description => "Ashortscenario:defendyourcittàperPH0minuti";


        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "demowillendinPH0minuti";

        public override string Experience_Required => "Requiredexperience";

        public override string InputActionName_ToggleMenu => "Togglemenu";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "Risorsecangobelowzeroeslightlyexceedstockpilelimit.boundsareonlyenforcedwhenworkqueueiscreated.";

        public override string Work_SelectCategory => "SelezionaItemCategory";
        public override string Hud_RemoveFromList => "RimuovifromList";

        public override string Hud_ReturnToPrevious => "Return";
        public override string Hud_Close => "Close";

        public override string Hud_Low => "Basso";
        public override string Hud_Medium => "Media";
        public override string Hud_High => "Alto";

        public override string Hud_Copy => "Copy";
        //public override string Hud_Paste => "Paste";
        public override string Hud_Cut => "Cut";
        public override string Hud_SaveCompleted => "SalvaCompleted";

        public override string Settings_WaterMultiplier => "AcquaMultiplier";
        public override string Settings_WaterMultiplier_Description => "Determineshowmuchacquacittàproduceestore.Highervaluesreducecomputerperformance.";

        public override string Settings_ChildMultiplier => "ChildbirthMultiplier";
        
        public override string Settings_CraftMultiplier_Description => "Lowervaluesresultinfasterproduzione.";

        public override string FastProduction => "FastProduzione";
        public override string SlowProduction => "SlowProduzione";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "WillNotProduce";

        //public override string CityAutomation_WaitForMaxPopulation => "Waitperpopulationamaxout";
        public override string Automation_AutomationFocus_NoFocus => "All";
        public override string CityAutomation_SoldierQuality => "SoldatoQuality";
        public override string CityAutomation_SoldierWeaponType => "ArmaType";

        public override string WarsResourceGroup_Resources => "Risorse";
        public override string WarsResourceGroup_Weapons => "Weapons";

        public override string WarsResourceGroup_AllWeaponTypes => "Mixed";
        public override string WarsResourceGroup_MeleeHandWeapons => "Melee";
        public override string WarsResourceGroup_RangedHandWeapons => "Ranged";
        public override string WarsResourceGroup_Warmachines => "Warmachines";

        public override string FactionSettings_Titel => "Faction-WideSettings";
        public override string FactionSettings_Description => "Appliesaallyourcittà";

        public override string Conscript_MaxPopulation => "MaxPopulation";
        public override string Conscript_MaxPopulation_Description => "Onlyrecruitswhenpopulationismaxedout";

        public override string Conscript_FoodAbundance => "MaxCiboStock";
        public override string Conscript_FoodAbundance_Description => "Onlyrecruitswhenciboreachesmaximumstockpile";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Seta:On";
        public override string GeneralSetting_Off => "Seta:Off";
        public override string GeneralSetting_AllBuildingsDescription => "Appliesaallbuildings";

        public override string GeneralSetting_ApplyMessage => "ChangeappliedaPH0buildings";

        public override string MustTurnOffSteamInput => "Ausecontrollers,youmustturnoffSteamInput.";

        public override string Technology_GainTitle => "WaysaGuadagnoTechnology";
        public override string Technology_LevelUp => "LevelSu";
        public override string Technology_ForEachLevelUp => "Whenalavoratorelevelsupintechnologyfield:PH0";

        public override string VoxelEditor_Description => "Createblockymodels";

        public override string Editor_Tool => "Tool";
        public override string Editor_SelectOptionsMenu => "SelectionOpzioni";
        public override string Editor_Continous => "Continuous"; // corrected spelling
        public override string Editor_Tool_PencilSize => "PencilSize";
        public override string Editor_Tool_SizeTolerance => "SizeTolerance";
        public override string Editor_Tool_RoundPencil => "RoundPencil";
        public override string Editor_Tool_EdgeSize => "EdgeSize";
        public override string Editor_Tool_PercentFill => "PercentFill";
        public override string Editor_Tool_ClearAbove => "ClearAbove";
        public override string Editor_Tool_FillBelow => "FillBelow";
        public override string Editor_UserModels => "UserModels";
        public override string Editor_UserModels_Description => "Browsemodelsyouhavesaved";

        public override string Editor_RetailModels => "RetailModels";
        public override string Editor_RetailModels_Description => "Caricamodelsfromgame";

        public override string Editor_ModTemplates => "ModdingTemplates";
        public override string Editor_ExportAsOBJ => "Exportas.OBJ";
        public override string Editor_SelectAll => "SelezionaAll";

        public override string Editor_Canvas_Title => "Canvas";
        public override string Editor_Canvas_Size => "Size";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "SizePresets";
        public override string Editor_Canvas_Move => "Move";
        public override string Editor_Canvas_Move_Up => "Su";
        public override string Editor_Canvas_Move_Down => "Giù";
        public override string Editor_Canvas_RotateClockwise => "RotateClockwise";
        public override string Editor_Canvas_RotateCounterClockwise => "RotateCounterclockwise"; // combined into one word
        public override string Editor_Canvas_Mirror => "Mirror";

        public override string Editor_Canvas_RotateFlip_Title => "Rotate/Flip";
        public override string Editor_Canvas_FlipVertical => "FlipSueGiù";
        public override string Editor_Canvas_FlipOrientation => "FlipLying/Standing";
        public override string Editor_Canvas_ClearAll_Description => "Removesallblockseframes";

        public override string Editor_Animation => "Animation";
        public override string Editor_Animation_RemoveCurrentFrame => "RimuoviCurrentFrame";
        public override string Editor_Animation_AddFrameCopy => "AggiungiFrameasCopy";
        public override string Editor_Animation_AddEmptyFrame => "AggiungiEmptyFrame";
        public override string Editor_Animation_MoveDescription => "ChangeFramePosition";
        public override string Editor_Animation_AllFrames => "AllFrames";
        public override string Editor_Animation_AllFrames_ActionDescription => "Performsameactiononallframes";

        public override string Editor_SettingsMenu => "Settings";
        public override string Hud_Exit => "Esci";
        public override string Editor_Canvas_Clear => "Clear";

        public override string Editor_Stamp => "Stamp";
        public override string Editor_StampOtherFrames => "StampinOtherFrames";
        public override string Editor_StampOtherFrames_Description => "Pastevoxelsintheseframes"; // "thisframes" → "theseframes"
        public override string Editor_PasteToFrame => "Pastevoxelsinthisframe";
        public override string Editor_ClearAllFrames => "ClearinAllFrames";
        public override string Editor_ClearOtherFrames => "ClearOtherFrames";

        public override string Editor_Settings_MoveSpeed => "MoveVelocità";
        public override string Editor_Settings_BackgroundColor => "BackgroundColor";
        public override string Editor_Settings_HideHUD => "HideHUD";

        public override string Editor_Color => "Color";
        public override string Editor_ColorsInUseLabel => "ColorsinUse";
        public override string Editor_Color_BrighterPlus => "Brighter+";
        public override string Editor_Color_Brighter => "Brighter";
        public override string Editor_Color_Darker => "Darker";
        public override string Editor_Color_DarkerPlus => "Darker+";
        public override string Editor_Color_RedTint => "RossoTint";
        public override string Editor_Color_Tint => "Tint";
        public override string Editor_Color_GreenTint => "VerdeTint";
        public override string Editor_Color_BlueTint => "BluTint";
        public override string Editor_Color_YellowTint => "GialloTint";
        public override string Editor_Color_PurpleTint => "PurpleTint";
        public override string Editor_NoColor => "Empty";

        public override string Editor_Material => "Material";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "Recolor";
        public override string Editor_Color_RecolorTo => "RecolorA";

        public override string Editor_Material_Set => "SetMaterial";

        public override string Editor_Preview => "Preview";
        public override string Editor_CombineWithCurrent => "CombineconCurrentModel";

        public override string Editor_PickedColor => "Picked";
        public override string Editor_ColorRGBvalues => "R:PH0G:PH1B:PH2";

        public override string BuildingType_ImmigrationTent => "ImmigrationTent";
        public override string BuildingType_ImmigrationTent_Description => "StoresPH0immigrants";
        public override string BuildingType_ReseachCenter => "ResearchCenter"; // fixed typo "Reseach"
        public override string BuildingType_Bookpress => "LibroPress";
        public override string BuildingType_Bookpress_Description => "Inoneresearchfield,allpointsgainedwillbesharedconallPH0inyourothercittà.";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "Example:WhenalavoratoreproducesPH0,theywillincreasetheirPH1skill.Whenlevelingup,itwillaggiungipointstowardsPH2technologysincetheysharePH3field."; // fixed "Reseach" and plural

        public override string BuildingType_Research_BaseDescription => "Increasestechnologyresearch.";

        public override string BuildingType_ResearchCenter_Description => "AddsPH0extratechnologyresearchpointswhenalavoratorelevelsupinsamefield.";

        //DEMO PATCH 5
        
         public override string Editor_CropSelection => "Cropaselection";

         public override string Immigrants_DisbandedSoldiers => "Disbandedsoldatiwillimmigrate";
         public override string Immigrants_RefillWorkers => "Quicklyrefillsworkforce";
         public override string Immigrants_UnhousedAreLost => "Immigratiwithouthousingwilldisappearaftersometempo";
         public override string Editor_VoxelCount => "PH0voxels";

         public override string Editor_Layers_Titel => "Layers";
         public override string Editor_Layers_All => "Alllayers";
         public override string Editor_LayerNumber => "LayerPH0";

         public override string Editor_Layer_AddEmpty => "Aggiungiemptylayer";
         public override string Editor_Layer_AddCopy => "Duplicatelayer";
         public override string Editor_Layer_Remove => "Rimuovilayer";
         public override string Editor_Layer_MergeDown => "Mergedown";
         public override string Editor_IsAnimated => "Animated";
         public override string Editor_ToggleVisible => "Togglevisibility";
         public override string Editor_ToggleAnimatedLayer => "Toggleanimatedlayer";
         public override string Editor_Projects => "Projectfiles";
         public override string ProfileEditor_ReplaceMaterial => "Profilecolor:PH0";

         public override string ProfileEditor_ProfileColors_Label => "Profilecolors";
         public override string ProfileEditor_TunicColor => "Tuniccolor";
         public override string ProfileEditor_PantsColor => "Pantscolor";
         public override string ProfileEditor_LeaderColor => "Leadercolor";

         public override string MapStartAs_Water => "Acqua";
         public override string MapStartAs_Land => "Land";
         public override string MapStartAs_Circle => "Cerchia";

         public override string Hud_NeedToBeAssigned => "Needsassignment";
         public override string Hud_CommitAssignment => "Assign";
         public override string Technology_NoAvailableResearch => "Noavailableresearch";

         public override string Research_Tab => "Research";

        //5.2
        public override string BuildCategory_General => "General";
        public override string BuildCategory_Military => "Military";
        public override string BuildCategory_Decoration => "Decorazione";
        public override string BuildCategory_Upgrade => "Potenzia";
        public override string Work_NoMines => "Nomines";

        //NEXT FEST DEMO
        public override string HUD_DisplayName => "Displayname";
        public override string HUD_Filter => "Filter";
        public override string HUD_Scale => "Scale";
        public override string HUD_Tags => "Tags";
        public override string HUD_ClickToCancel => "Clickacancel";

        public override string ObjectTag_Description => "Aggiungiasymbolonmappa";
        public override string HudPins => "HUDpins";
        public override string HudPins_Description => "Stickinformationascreen";

        public override string Lobby_PlayerProfileNumbered => "ProfilePH0";
        public override string Lobby_CharacterCreationNumbered => "CharacterPH0";
        public override string Lobby_PlayerProfileEdit => "Editplayerprofile";

        public override string Editor_ConvertAnimationToLayers => "Convertanimationalayers";
        public override string Editor_StampAllFrames => "Stamponallframes";

        public override string Editor_DisplayOptions => "Displayoptions";
        public override string Editor_CharacterCreator => "Charactercreator";
        public override string Editor_CharacterCreator_Description => "Militarymodelappearanceeditor";
        public override string Editor_HatGenre => "Hatdisplaymode";
        public override string Editor_HatGenre_FollowWeapon => "Followarma";
        public override string Editor_HatGenre_Uniform => "Uniform";
        public override string Editor_CopyPasteSelectedColor => "Copyfromselectedcolor";

        public override string Character_Accessories => "Accessories";
        public override string Character_Hat => "Hat";
        public override string Character_Head => "Head";
        public override string Character_Body => "Body";
        public override string Character_Arms => "Arms";
        public override string Character_Back => "Indietro";
        public override string Character_Face => "Face";

        public override string BuildingType_Tavern => "CommonHall";

        public override string Settings_CraftMultiplier => "Crafttempomultiplier";
        public override string Settings_ChildMultiplier_Description => "Increasesvelocitàatwhichnewlavoratoriareadded";

        public override string Settings_CasualControls => "Casualplayercontrols";
        public override string Settings_CasualControls_Description => "Simplifiesgameplaybyreducingchoicesatastodecisions.Onlymoneyisusedasaresource.";

        public override string Settings_AdvancedControls => "Advancedcontrols";
        public override string Settings_AdvancedControls_Description => "fullresourcemanagementexperience.";

        public override string WarsResourceGroup_Metal => "Metal";
        public override string Work_Craft => "Craft";
        public override string Work_OnlyCraftOnFullStock => "Onlycraftonfullstockpile";

        public override string ExperienceType_Smelting => "Smelting";
        public override string Category_Optimize => "Optimize";
        public override string BuildCategory_Road => "Road";
        public override string XP_UnlockBuildPrio => "Unlockcostruiscipriority:PH0";
        public override string Technology_ModernFarming => "Modernfarming";

        public override string ExportImportDescription => "Persharingsavefilesconotherplayer,allfilesarethisfolder:PH0";

        public override string CityCultureDescription => "Culuturewillgiveitaspecialbonusacittà";

        public override string UnitType_CloseRangeRifle => "Arquebusier";
        public override string UnitType_LongRangeRifle => "Musketeer";
        public override string UnitType_Skirmisher => "Skirmisher";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "Lunariarcher";
        public override string UnitType_MithrilSwordsman => "Lunariknight";

        public override string Defence_AutoAssign_Towers => "Assigntowers";

        public override string EventMessage_DesertersText_Food => "Hungrysoldatiaredesertingfromyouresercito";

        public override string Tutorial_CasualRecruitSoldiers => "Purchaseonesoldatogruppo";


        //Shadow update
        public override string Technology_CannotReassign => "La Tech non può essere riassegnata finché la ricerca non è completata";
        public override string Diplomacy_DeclareWarAgainst => "Dichiari guerra a";
        public override string Diplomacy_AllyCount => "Numero di alleati";
        public override string Diplomacy_CostPerAlly => "Il costo aumenta di {0} per ogni alleato";

        public override string Event_ChanceOfFailure => "{0}% di possibilità di fallimento";
        public override string EventMessage_Event_Title => "Evento";
        public override string EventMessage_TheCohalition => "La Coalizione";

        public override string EventMessage_DarkHorde => "Orda Oscura";
        public override string EventMessage_DarkHordeKiller_Title => "Uccisore dell’Orda Oscura";
        public override string EventMessage_DarkHordeKiller_Message => "Cavalieri campioni si sono uniti al tuo servizio";

        public override string Settings_Mode_Spectator_Description => "Solo spettatore – oppure intervieni con i God Powers.";
        public override string GodPower => "God Power";

        public override string Building_TreeSprout_Description => "Pianta un albero";
        public override string Building_TreeSprout_Soft => "Germoglio di legno tenero";
        public override string Building_TreeSprout_Hard => "Germoglio di legno duro";

        public override string GeneralSetting_SetAll => "Applica a tutti";

        public override string Hud_All => "Tutti";

        public override string Hud_Previous => "Precedente";

        public override string Hud_EffectWillStack => "L’effetto si accumula";

        public override string Info_WhenFoodRunsOut => "Quando il cibo finisce, città ed eserciti lo acquisteranno automaticamente dal mercato nero.";


    }
}
