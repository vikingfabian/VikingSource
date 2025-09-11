using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class German : AbsLanguage
    {
        /// <summary>
        /// Name dieser Sprache
        /// </summary>
        public override string MyLanguage => "Deutsch";

        /// <summary>
        /// Darstellung der Anzahl von Gegenständen. 0: Gegenstand, 1: Anzahl
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}: {1}";

        /// <summary>
        /// Sprache auswählen
        /// </summary>
        public override string Lobby_Language => "Sprache";

        /// <summary>
        /// Spiel starten
        /// </summary>
        public override string Lobby_Start => "START";

        /// <summary>
        /// Lokalen Mehrspielermodus auswählen, 0: aktuelle Spieleranzahl
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Lokaler Mehrspielermodus";

        /// <summary>
        /// Titel für Menü zur Auswahl der Split-Screen-Spieleranzahl
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Spieleranzahl auswählen";

        /// <summary>
        /// Beschreibung für lokalen Mehrspielermodus
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "Mehrspieler erfordert Xbox-Controller";

        /// <summary>
        /// Zur nächsten Split-Screen-Position wechseln
        /// </summary>
        public override string Lobby_NextScreen => "Nächste Bildschirmposition";

        /// <summary>
        /// Spieler können ihr visuelles Erscheinungsbild auswählen und in einem Profil speichern
        /// </summary>
        public override string Lobby_FlagSelectTitle => "Flagge auswählen";

        /// <summary>
        /// 0: Nummerierung von 1 bis 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Flagge {0}";

        /// <summary>
        /// Spielname und Versionsnummer
        /// </summary>
        public override string Lobby_GameVersion => "DSS Kriegsparty - Ver {0}";

        public override string FlagEditor_Description => "Gestalte deine Flagge und wähle Farben für deine Soldaten.";

        /// <summary>
        /// Malwerkzeug, das eine Fläche mit einer Farbe füllt
        /// </summary>
        public override string FlagEditor_Bucket => "Eimer";

        /// <summary>
        /// Öffnet den Flaggenprofil-Editor
        /// </summary>
        public override string Lobby_FlagEdit => "Flagge bearbeiten";

        public override string Lobby_WarningTitle => "Warnung";
        public override string Lobby_IgnoreWarning => "Warnung ignorieren";

        /// <summary>
        /// Warnung, wenn ein Spieler keine Eingabe ausgewählt hat.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "Ein Spieler hat keine Eingabe";

        /// <summary>
        /// Menü mit Inhalten, die über das hinausgehen, was die meisten Spieler nutzen werden.
        /// </summary>
        public override string Lobby_Extra => "Extra";

        /// <summary>
        /// Die zusätzlichen Inhalte sind nicht übersetzt oder vollständig unterstützt.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "Warnung! Dieser Inhalt ist nicht lokalisiert oder vollständig mit Eingabe-/Barrierefreiheitsunterstützung kompatibel";

        public override string Lobby_MapSizeTitle => "Kartengröße";

        public override string Lobby_MapSizeOptTiny => "Winzig";
        public override string Lobby_MapSizeOptSmall => "Klein";
        public override string Lobby_MapSizeOptMedium => "Mittel";
        public override string Lobby_MapSizeOptLarge => "Groß";
        public override string Lobby_MapSizeOptHuge => "Riesig";
        public override string Lobby_MapSizeOptEpic => "Episch";

        public override string Lobby_MapSizeDesc => "{0}x{1} km";

        public override string Lobby_ExitGame => "Beenden";

        public override string Player_DefaultName => "Spieler {0}";

        public override string ProfileEditor_OptionsMenu => "Optionen";
        public override string ProfileEditor_FlagColorsTitle => "Flaggenfarben";
        public override string ProfileEditor_MainColor => "Hauptfarbe";
        public override string ProfileEditor_Detail1Color => "Detailfarbe 1";
        public override string ProfileEditor_Detail2Color => "Detailfarbe 2";
        public override string ProfileEditor_PeopleColorsTitle => "Soldaten";
        public override string ProfileEditor_SkinColor => "Hautfarbe";
        public override string ProfileEditor_HairColor => "Haarfarbe";
        public override string ProfileEditor_PickColor => "Farbe wählen";
        public override string ProfileEditor_MoveImage => "Bild verschieben";
        public override string ProfileEditor_MoveImageLeft => "Links";
        public override string ProfileEditor_MoveImageRight => "Rechts";
        public override string ProfileEditor_MoveImageUp => "Hoch";
        public override string ProfileEditor_MoveImageDown => "Runter";
        public override string ProfileEditor_DiscardAndExit => "Verwerfen und beenden";
        public override string ProfileEditor_DiscardAndExitDescription => "Alle Änderungen verwerfen";

        /// <summary>
        /// Im Spielerprofil-Editor. Teil der Farboptionen für Farbton, Sättigung und Helligkeit.
        /// </summary>
        public override string ProfileEditor_Hue => "Farbton";

        /// <summary>
        /// Im Spielerprofil-Editor. Teil der Farboptionen für Farbton, Sättigung und Helligkeit.
        /// </summary>
        public override string ProfileEditor_Lightness => "Helligkeit";

        /// <summary>
        /// Im Spielerprofil-Editor. Wechselt zwischen Flaggen- und Soldatenfarboptionen.
        /// </summary>
        public override string ProfileEditor_NextColorType => "Nächster Farbtyp";


        public override string Hud_SaveAndExit => "Speichern und beenden";

        public override string Hud_GameSpeedLabel => "Spielgeschwindigkeit: {0}x";
        public override string Input_GameSpeed => "Spielgeschwindigkeit";
        public override string Hud_TotalIncome => "Gesamteinnahmen/Sekunde: {0}";
        public override string Hud_Upkeep => "Unterhalt: {0}";
        public override string Hud_ArmyUpkeep => "Armeeunterhalt: {0}";

        public override string Hud_GuardCount => "Wachen";
        public override string Hud_IncreaseMaxGuardCount => "Maximale Wachengröße {0}";
        public override string Hud_GuardCount_MustExpandCityMessage => "Du musst die Stadt erweitern.";
        public override string Hud_SoldierCount => "Soldatenanzahl: {0}";
        public override string Hud_SoldierGroupsCount => "Gruppenanzahl: {0}";
        public override string Hud_StrengthRating => "Stärkewertung: {0}";
        public override string Hud_TotalStrengthRating => "Militärische Stärke: {0}";
        public override string Hud_Immigrants => "Einwanderer";
        public override string Hud_CityCount => "Stadtanzahl: {0}";
        public override string Hud_ArmyCount => "Armeeanzahl: {0}";

        /// <summary>
        /// Mini-Button, um einen Kauf mehrmals zu wiederholen, z. B. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Anforderung";
        public override string Hud_PurchaseTitle_Cost => "Kosten";
        public override string Hud_PurchaseTitle_Gain => "Gewinn";

        /// <summary>
        /// Wie viel von einer Ressource verwendet wird, z. B. "5 Gold. (Verfügbar: 10)". 
        /// Über dem Text wird ein "Kosten"-Titel stehen. 0: Ressource, 1: Kosten, 2: Verfügbar
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Verfügbar: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "Kosten steigen um {0}";

        public override string Hud_Purchase_MaxCapacity => "Maximale Kapazität erreicht";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Stärke: Deine {0} - Ihre {1}";

        /// <summary>
        /// Kurze Datumsanzeige im Format Jahr, Monat, Tag
        /// </summary>
        public override string Hud_Date => "J{0} M{1} T{2}";

        /// <summary>
        /// Kurze Zeitspanne anzeigen als Stunden, Minuten, Sekunden
        /// </summary>
        public override string Hud_TimeSpan => "H{0} M{1} S{2}";

        /// <summary>
        /// Kampf zwischen zwei Armeen oder zwischen Armee und Stadt
        /// </summary>
        public override string Hud_Battle => "Schlacht";



        /// <summary>
        /// Tastenbeschreibung: Pause
        /// </summary>
        public override string Input_Pause => "Pause";

        /// <summary>
        /// Tastenbeschreibung: Fortsetzen nach Pause
        /// </summary>
        public override string Input_ResumePaused => "Fortsetzen";

        /// <summary>
        /// Allgemeine Währungsressource
        /// </summary>
        public override string ResourceType_Gold => "Gold";

        /// <summary>
        /// Arbeitskraft-Ressource
        /// </summary>
        public override string ResourceType_Workers => "Arbeiter";

        public override string ResourceType_Workers_Description => "Arbeiter erzeugen Einkommen und werden als Soldaten für deine Armeen rekrutiert.";

        /// <summary>
        /// Ressource für Diplomatie
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Diplomatiepunkte";

        /// <summary>
        /// 0: Aktuelle Punkte, 1: Weiche Obergrenze (danach langsamerer Anstieg), 2: Harte Grenze
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Diplomatiepunkte: {0} / {1} ({2})";

        /// <summary>
        /// Stadtgebäude: Gebäude für Ritter und Diplomaten
        /// </summary>
        public override string Building_NobleHouse => "Adelshaus";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "1 Diplomatiepunkt alle {0} Sekunden";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} zur maximalen Diplomatiepunktgrenze";
        public override string Building_NobleHouse_UnlocksKnight => "Schaltet die Rittereinheit frei";

        public override string Building_BuildAction => "Bauen";
        public override string Building_IsBuilt => "Gebaut";

        /// <summary>
        /// Stadtgebäude: Dunkle Fabrik für Massenproduktion
        /// </summary>
        public override string Building_DarkFactory => "Dunkle Fabrik";

        /// <summary>
        /// Einstellungsmenü: Gesamt-Schwierigkeitsgrad in Prozent
        /// </summary>
        public override string Settings_TotalDifficulty => "Gesamtschwierigkeit {0}%";

        /// <summary>
        /// Einstellungsmenü: Grund-Schwierigkeitsstufe
        /// </summary>
        public override string Settings_DifficultyLevel => "Schwierigkeitsgrad {0}%";

        /// <summary>
        /// Einstellungsmenü: Option zum Erstellen neuer Karten anstelle des Ladens bestehender Karten
        /// </summary>
        public override string Settings_GenerateMaps => "Neue Karten erstellen";

        /// <summary>
        /// Einstellungsmenü: Das Erstellen neuer Karten dauert länger als das Laden vorhandener Karten
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Das Generieren dauert länger als das Laden der vorgefertigten Karten";

        /// <summary>
        /// Einstellungsmenü: Schwierigkeitsoption - Sperrt die Möglichkeit, während der Pause zu spielen
        /// </summary>
        public override string Settings_AllowPause => "Pause und Befehle erlauben";

        /// <summary>
        /// Einstellungsmenü: Schwierigkeitsoption - Bosse erscheinen im Spiel
        /// </summary>
        public override string Settings_BossEvents => "Boss-Ereignisse";

        /// <summary>
        /// Einstellungsmenü: Schwierigkeitsoption - Kein Bossmodus
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Das Deaktivieren von Boss-Ereignissen versetzt das Spiel in einen Sandbox-Modus ohne Ende.";

        /// <summary>
        /// Menü für Automatisierungsoptionen
        /// </summary>
        public override string Automation_Title => "Automatisierung";

        /// <summary>
        /// Automatisierungsinfo: Wartet darauf, dass die Arbeitskraft maximal wird
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Wartet, bis die Arbeitskraft maximiert ist";

        /// <summary>
        /// Automatisierungsinfo: Wird pausiert, wenn das Einkommen negativ ist
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Wird pausiert, wenn das Einkommen negativ ist";

        /// <summary>
        /// Automatisierungsinfo: Bevorzugt große Städte
        /// </summary>
        public override string Automation_InfoLine_Priority => "Große Städte haben Priorität";

        /// <summary>
        /// Automatisierungsinfo: Führt maximal einen Kauf pro Sekunde durch
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Führt maximal einen Kauf pro Sekunde durch";

        /// <summary>
        /// Button-Beschriftung für eine Kaufaktion
        /// </summary>
        public override string HudAction_BuyItem => "Kaufe {0}";

        /// <summary>
        /// Der Zustand von Frieden oder Krieg zwischen zwei Nationen
        /// </summary>
        public override string Diplomacy_RelationType => "Beziehung";

        /// <summary>
        /// Titel für eine Liste von Beziehungen zwischen anderen Fraktionen
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Ihre Beziehungen zu anderen";

        /// <summary>
        /// Diplomatische Beziehung: Du hast direkte Kontrolle über die Ressourcen einer Nation
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Diener";

        /// <summary>
        /// Diplomatische Beziehung: Volle Zusammenarbeit
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Verbündeter";

        /// <summary>
        /// Diplomatische Beziehung: Reduzierte Kriegsgefahr
        /// </summary>
        public override string Diplomacy_RelationType_Good => "Gut";

        /// <summary>
        /// Diplomatische Beziehung: Friedensabkommen
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Frieden";

        /// <summary>
        /// Diplomatische Beziehung: Noch kein Kontakt aufgenommen
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Neutral";

        /// <summary>
        /// Diplomatische Beziehung: Temporäres Friedensabkommen
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Waffenstillstand";

        /// <summary>
        /// Diplomatische Beziehung: Krieg
        /// </summary>
        public override string Diplomacy_RelationType_War => "Krieg";

        /// <summary>
        /// Diplomatische Beziehung: Totaler Krieg ohne Chance auf Frieden
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Totaler Krieg";

        /// <summary>
        /// Meldung, wenn eine Kriegserklärung eingeht
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "Krieg erklärt!";

        /// <summary>
        /// Der Waffenstillstand ist abgelaufen, und der Krieg geht weiter
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "Der Waffenstillstand ist beendet";


        /// <summary>
        /// Diplomatische Kommunikation. Wie gut die Verhandlungen laufen. 0: Gesprächsbedingungen
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "Gesprächsbedingungen: {0}";

        /// <summary>
        /// Diplomatische Kommunikation. Besser als normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "Gut";

        /// <summary>
        /// Diplomatische Kommunikation. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Normal";

        /// <summary>
        /// Diplomatische Kommunikation. Schlechter als normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Schlecht";

        /// <summary>
        /// Diplomatische Kommunikation. Wird nicht kommunizieren.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "Keine";

        /// <summary>
        /// Diplomatische Aktion. Eine neue diplomatische Beziehung aufbauen.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Beziehungen aufbauen mit: {0}";

        /// <summary>
        /// Diplomatische Aktion. Einen Friedensvertrag vorschlagen.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Frieden anbieten";

        /// <summary>
        /// Diplomatische Aktion. Ein Bündnis vorschlagen.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Bündnis anbieten";

        /// <summary>
        /// Diplomatischer Titel. Ein anderer Spieler hat eine neue diplomatische Beziehung vorgeschlagen. 0: Spielername
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} bietet neue Beziehungen an";

        /// <summary>
        /// Diplomatische Aktion. Eine neue diplomatische Beziehung akzeptieren.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Neue Beziehung akzeptieren";

        /// <summary>
        /// Diplomatische Beschreibung. Ein anderer Spieler hat eine neue diplomatische Beziehung vorgeschlagen. 0: Beziehungstyp
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "Neue Beziehung angeboten: {0}";

        /// <summary>
        /// Diplomatische Aktion. Eine andere Nation zum Vasallen machen.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Als Vasall aufnehmen";

        /// <summary>
        /// Diplomatische Beschreibung. Ist gegen das Böse.
        /// </summary>
        public override string Diplomacy_LightSide => "Ist ein Verbündeter der hellen Seite";

        /// <summary>
        /// Diplomatische Beschreibung. Wie lange der Waffenstillstand andauert.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "Endet in {0} Sekunden";

        /// <summary>
        /// Diplomatische Aktion. Den Waffenstillstand verlängern.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Waffenstillstand verlängern";

        /// <summary>
        /// Diplomatische Beschreibung. Wie lange der Waffenstillstand verlängert wird.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Waffenstillstand verlängert um {0} Sekunden";

        /// <summary>
        /// Diplomatische Beschreibung. Ein Bruch der Vereinbarung kostet Diplomatiepunkte.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "Das Brechen der Beziehung kostet {0} Diplomatiepunkte";

        /// <summary>
        /// Diplomatische Beschreibung für Verbündete.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Verbündete teilen Kriegserklärungen.";

        /// <summary>
        /// Diplomatische Beschreibung für gute Beziehungen.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Begrenzt die Möglichkeit, Krieg zu erklären.";

        /// <summary>
        /// Diplomatische Beschreibung. Der Vasall muss eine kleinere militärische Streitkraft haben.
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "{0}x stärkere militärische Macht erforderlich";

        /// <summary>
        /// Diplomatische Beschreibung. Der Vasall muss sich in einem aussichtslosen Krieg befinden.
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "Der Vasall muss sich im Krieg gegen einen stärkeren Feind befinden";

        /// <summary>
        /// Diplomatische Beschreibung. Ein Vasall darf nicht zu viele Städte besitzen.
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "Ein Vasall darf maximal {0} Städte haben";

        /// <summary>
        /// Diplomatische Beschreibung. Die Kosten in Diplomatiepunkten werden steigen.
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "Der Preis steigt mit jedem weiteren Vasallen";

        /// <summary>
        /// Diplomatische Beschreibung. Das Ergebnis der Vasallenbeziehung: friedliche Übernahme einer anderen Nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Übernimmt die andere Fraktion";



        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Titel.
        /// </summary>
        public override string EndGameStatistics_Title => "Statistiken";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Gesamtspielzeit.
        /// </summary>
        public override string EndGameStatistics_Time => "Spielzeit: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der rekrutierten Soldaten.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "Rekrutierte Soldaten: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der in der Schlacht gefallenen Soldaten.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Soldaten im Kampf verloren: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der im Kampf getöteten feindlichen Soldaten.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Getötete feindliche Soldaten: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der Soldaten, die desertiert sind.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Desertierte Soldaten: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der eroberten Städte.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "Eroberte Städte: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der verlorenen Städte.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "Verlorene Städte: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der gewonnenen Schlachten.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Gewonnene Schlachten: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der verlorenen Schlachten.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Verlorene Schlachten: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der von dir erklärten Kriege.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "Erklärte Kriege: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Anzahl der gegen dich erklärten Kriege.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "Erhaltene Kriegserklärungen: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Durch Diplomatie geschlossene Allianzen.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Diplomatische Allianzen: {0}";

        /// <summary>
        /// Statistiken, die auf dem Endspiel-Bildschirm angezeigt werden. Durch Diplomatie erzwungene Vasallen.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Diplomatische Vasallen: {0}";

        /// <summary>
        /// Kollektive Einheit auf der Karte. Armee von Soldaten.
        /// </summary>
        public override string UnitType_Army => "Armee";

        /// <summary>
        /// Kollektive Einheit auf der Karte. Gruppe von Soldaten.
        /// </summary>
        public override string UnitType_SoldierGroup => "Gruppe";

        /// <summary>
        /// Kollektive Einheit auf der Karte. Allgemeine Bezeichnung für Dörfer oder Städte.
        /// </summary>
        public override string UnitType_City => "Stadt";

        /// <summary>
        /// Eine Auswahl mehrerer Armeen.
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Armeegruppe, Anzahl: {0}";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Standard-Frontliniensoldat.
        /// </summary>
        public override string UnitType_Soldier => "Soldat";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Seeschlacht-Soldat.
        /// </summary>
        public override string UnitType_Sailor => "Matrose";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Eingezogene Bauern.
        /// </summary>
        public override string UnitType_Folkman => "Bauerkrieger";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Einheit mit Speer und Schild.
        /// </summary>
        public override string UnitType_Spearman => "Speerkämpfer";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Eliteeinheit, Teil der königlichen Garde.
        /// </summary>
        public override string UnitType_HonorGuard => "Ehrengarde";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Antikavallerie, trägt lange Zweihandspeere.
        /// </summary>
        public override string UnitType_Pikeman => "Pikenier";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Gepanzerte Kavallerie.
        /// </summary>
        public override string UnitType_Knight => "Ritter";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Bogenschütze.
        /// </summary>
        public override string UnitType_Archer => "Bogenschütze";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Armbrustschütze.
        /// </summary>
        public override string UnitType_Crossbow => "Armbrustschütze";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Kriegsmaschine, die große Speere schleudert.
        /// </summary>
        public override string UnitType_Ballista => "Ballista";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Fantasietroll mit einer Kanone.
        /// </summary>
        public override string UnitType_Trollcannon => "Trollkanone";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Soldat aus dem Wald.
        /// </summary>
        public override string UnitType_GreenSoldier => "Grüner Soldat";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Seestreitmacht aus dem Norden.
        /// </summary>
        public override string UnitType_Viking => "Wikinger";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Der böse Meisterboss.
        /// </summary>
        public override string UnitType_DarkLord => "Dunkler Herrscher";

        /// <summary>
        /// Name für eine spezialisierte Soldateneinheit. Soldat, der eine große Flagge trägt.
        /// </summary>
        public override string UnitType_Bannerman => "Fahnenträger";

        /// <summary>
        /// Name für eine Militäreinheit. Kriegsschiff, das eine Einheit transportiert. 0: Einheitstyp
        /// </summary>
        public override string UnitType_WarshipWithUnit => "{0} Kriegsschiff";

        public override string UnitType_Description_Soldier => "Eine vielseitige Einheit.";
        public override string UnitType_Description_Sailor => "Stark in Seeschlachten.";
        public override string UnitType_Description_Folkman => "Billige, untrainierte Soldaten.";
        public override string UnitType_Description_HonorGuard => "Elite-Soldaten ohne Unterhaltskosten.";
        public override string UnitType_Description_Knight => "Stark in offenen Feldschlachten.";
        public override string UnitType_Description_Archer => "Nur stark, wenn geschützt.";
        public override string UnitType_Description_Crossbow => "Mächtiger Fernkämpfer.";
        public override string UnitType_Description_Ballista => "Effektiv gegen Städte.";
        public override string UnitType_Description_GreenSoldier => "Gefürchteter Elfenkrieger.";
        public override string UnitType_Description_DarkLord => "Der Endgegner.";


        /// <summary>
        /// Informationen über eine Soldateneinheit
        /// </summary>
        public override string SoldierStats_Title => "Statistiken pro Einheit";

        /// <summary>
        /// Anzahl der Soldatengruppen
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} Gruppen, insgesamt {1} Einheiten";

        /// <summary>
        /// Soldaten haben unterschiedliche Stärken auf offenem Feld, auf See oder bei Angriffen auf Siedlungen
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Angriffsstärke: Land {0} | See {1} | Stadt {2}";

        /// <summary>
        /// Wie viele Treffer ein Soldat aushalten kann
        /// </summary>
        public override string SoldierStats_Health => "Lebenspunkte: {0}";

        /// <summary>
        /// Einige Soldaten erhöhen die Bewegungsgeschwindigkeit der Armee an Land
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Armee-Geschwindigkeitsbonus an Land: {0}";

        /// <summary>
        /// Einige Soldaten erhöhen die Bewegungsgeschwindigkeit der Schiffe
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Armee-Geschwindigkeitsbonus auf See: {0}";

        /// <summary>
        /// Gekaufte Soldaten beginnen als Rekruten und beenden ihr Training nach einigen Minuten.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Trainingszeit: {0} Minuten. Wird doppelt so schnell abgeschlossen, wenn die Rekruten sich neben einer Stadt befinden.";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Bewegung stoppen.
        /// </summary>
        public override string ArmyOption_Halt => "Anhalten";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Soldaten entfernen.
        /// </summary>
        public override string ArmyOption_Disband => "Einheiten auflösen";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Soldaten zwischen Armeen versenden.
        /// </summary>
        public override string ArmyOption_Divide => "Armee teilen";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Soldaten entfernen.
        /// </summary>
        public override string ArmyOption_RemoveX => "{0} entfernen";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Alle Soldaten auflösen.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Alle auflösen";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. 0: Anzahl, 1: Einheitstyp
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1}-Gruppen: {0}";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Soldaten zwischen Armeen senden.
        /// </summary>
        public override string ArmyOption_SendToX => "Einheiten senden an {0}";

        public override string ArmyOption_MergeAllArmies => "Alle Armeen zusammenführen";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Soldaten in eine neue Armee versetzen.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Einheiten in eine neue Armee aufteilen";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Soldaten zwischen Armeen versenden.
        /// </summary>
        public override string ArmyOption_SendX => "{0} senden";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Alle Soldaten versenden.
        /// </summary>
        public override string ArmyOption_SendAll => "Alle senden";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Armee in zwei gleich große Teile teilen.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Armee halbieren";

        /// <summary>
        /// Menüoption zur Steuerung einer Armee. Armeen zusammenführen.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Armeen zusammenführen";

        /// <summary>
        /// Soldaten rekrutieren.
        /// </summary>
        public override string UnitType_Recruit => "Rekrut";

        /// <summary>
        /// Soldaten eines bestimmten Typs rekrutieren. 0: Typ
        /// </summary>
        public override string CityOption_RecruitType => "{0} rekrutieren";

        /// <summary>
        /// Anzahl der bezahlten Soldaten
        /// </summary>
        public override string CityOption_XMercenaries => "Söldner: {0}";

        /// <summary>
        /// Zeigt die Anzahl der Söldner an, die derzeit auf dem Markt zur Anwerbung verfügbar sind.
        /// </summary>
        public override string Hud_MercenaryMarket => "Söldnermarkt für Anwerbungen";

        /// <summary>
        /// Eine bestimmte Anzahl von Söldnern kaufen
        /// </summary>
        public override string CityOption_BuyXMercenaries => "{0} Söldner importieren";

        public override string CityOption_Mercenaries_Description => "Soldaten werden von Söldnern rekrutiert, anstatt aus deiner Arbeitskraft.";

        /// <summary>
        /// Schaltflächentext für Aktion: Wohnraum für mehr Arbeiter schaffen.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "Arbeitskraft erweitern";

        public override string CityOption_ExpandWorkForce_IncreaseMax => "Maximale Arbeitskraft +{0}";
        public override string CityOption_ExpandGuardSize => "Wache vergrößern";

        public override string CityOption_Damages => "Schäden: {0}";
        public override string CityOption_Repair => "Schäden reparieren";
        public override string CityOption_RepairGain => "{0} Schäden reparieren";

        public override string CityOption_Repair_Description => "Schäden reduzieren die Anzahl der verfügbaren Arbeiter.";

        public override string CityOption_BurnItDown => "Niederbrennen";
        public override string CityOption_BurnItDown_Description => "Die Arbeitskraft entfernen und maximalen Schaden verursachen.";

        /// <summary>
        /// Der Hauptboss. Benannt nach einem leuchtenden Metallstein, der in seiner Stirn steckt.
        /// </summary>
        public override string FactionName_DarkLord => "Auge des Verderbens";

        /// <summary>
        /// Von Orks inspirierte Fraktion. Dient dem dunklen Herrscher.
        /// </summary>
        public override string FactionName_DarkFollower => "Diener des Schreckens";

        /// <summary>
        /// Die größte Fraktion, das alte, aber korrumpierte Königreich.
        /// </summary>
        public override string FactionName_UnitedKingdom => "Vereinigte Königreiche";

        /// <summary>
        /// Von Elfen inspirierte Fraktion. Lebt in Harmonie mit dem Wald.
        /// </summary>
        public override string FactionName_Greenwood => "Grünwald";

        /// <summary>
        /// Asiatisch inspirierte Fraktion im Osten.
        /// </summary>
        public override string FactionName_EasternEmpire => "Östliches Imperium";

        /// <summary>
        /// Wikinger-Königreich im Norden. Das größte von allen.
        /// </summary>
        public override string FactionName_NordicRealm => "Nordische Reiche";

        /// <summary>
        /// Wikinger-Königreich im Norden. Nutzt ein Bärenkrallen-Symbol.
        /// </summary>
        public override string FactionName_BearClaw => "Bärenklaue";

        /// <summary>
        /// Wikinger-Königreich im Norden. Nutzt ein Hahnen-Symbol.
        /// </summary>
        public override string FactionName_NordicSpur => "Nordischer Sporn";

        /// <summary>
        /// Wikinger-Königreich im Norden. Nutzt ein schwarzes Raben-Symbol.
        /// </summary>
        public override string FactionName_IceRaven => "Eisrabe";

        /// <summary>
        /// Fraktion, die für das Töten von Drachen mit mächtigen Ballisten bekannt ist.
        /// </summary>
        public override string FactionName_Dragonslayer => "Drachentöter";

        /// <summary>
        /// Eine Söldnereinheit aus dem Süden. Arabisch inspiriert.
        /// </summary>
        public override string FactionName_SouthHara => "Süd-Hara";

        /// <summary>
        /// Name für neutrale, KI-gesteuerte Nationen.
        /// </summary>
        public override string FactionName_GenericAi => "KI {0}";

        /// <summary>
        /// Anzeigename für Spieler und ihre Nummern.
        /// </summary>
        public override string FactionName_Player => "Spieler {0}";

        /// <summary>
        /// Nachricht, wenn sich ein Miniboss auf Schiffen aus dem Süden nähert.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "Feind naht!";
        public override string EventMessage_HaraMercenaryText => "Hara-Söldner wurden im Süden gesichtet.";

        /// <summary>
        /// Erste Warnung, dass der Hauptboss erscheinen wird.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "Eine dunkle Prophezeiung";
        public override string EventMessage_ProphesyText => "Das Auge des Verderbens wird bald erscheinen, und deine Feinde werden sich ihm anschließen!";

        /// <summary>
        /// Zweite Warnung, dass der Hauptboss erscheinen wird.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Dunkle Zeiten";
        public override string EventMessage_FinalBossEnterText => "Das Auge des Verderbens ist auf der Karte erschienen!";

        /// <summary>
        /// Nachricht, wenn der Hauptboss dich auf dem Schlachtfeld erwartet.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "Ein verzweifelter Angriff";
        public override string EventMessage_FinalBattleText => "Der dunkle Herrscher hat sich der Schlacht angeschlossen. Jetzt ist deine Chance, ihn zu vernichten!";

        /// <summary>
        /// Nachricht, wenn Soldaten die Armee verlassen, weil du ihren Unterhalt nicht bezahlen kannst.
        /// </summary>
        public override string EventMessage_DesertersTitle => "Deserteure!";
        public override string EventMessage_DesertersText_Money => "Unbezahlte Soldaten desertieren aus deinen Armeen.";

        public override string DifficultyDescription_AiAggression => "KI-Aggressivität: {0}.";
        public override string DifficultyDescription_BossSize => "Boss-Größe: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Boss-Eintrittszeit: {0}.";
        public override string DifficultyDescription_AiEconomy => "KI-Wirtschaft: {0}%.";
        public override string DifficultyDescription_AiDelay => "KI-Verzögerung: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Diplomatie-Schwierigkeit: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Söldnerkosten: {0}.";
        public override string DifficultyDescription_HonorGuards => "Ehrengarde: {0}.";

        /// <summary>
        /// Das Spiel wurde erfolgreich beendet.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Sieg!";

        /// <summary>
        /// Zitate des Anführer-Charakters, den du im Spiel spielst.
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
{
    "In Zeiten des Friedens trauern wir um die Toten.",
    "Jeder Triumph trägt einen Schatten des Opfers.",
    "Erinnere dich an die Reise, die uns hierhergebracht hat, gesäumt von den Seelen der Tapferen.",
    "Unsere Gedanken sind leicht vom Sieg, unsere Herzen schwer vom Gewicht der Gefallenen."
};

        public override string EndScreen_DominationVictoryQuote => "Ich wurde von den Göttern auserwählt, die Welt zu beherrschen!";

        /// <summary>
        /// Das Spiel wurde mit einer Niederlage beendet.
        /// </summary>
        public override string EndScreen_FailTitle => "Niederlage!";

        /// <summary>
        /// Zitate des Anführer-Charakters, den du im Spiel spielst.
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
{
    "Mit zerschundenen Körpern vom Marschieren und Nächten voller Sorgen begrüßen wir das Ende.",
    "Die Niederlage mag unser Land verdunkeln, aber sie kann das Licht unserer Entschlossenheit nicht auslöschen.",
    "Löscht die Flammen in unseren Herzen aus, aus ihrer Asche werden unsere Kinder eine neue Morgendämmerung schmieden.",
    "Lasst unsere Geschichten die Glut sein, die den Sieg von morgen entfacht."
};

        /// <summary>
        /// Eine kurze Zwischensequenz am Ende des Spiels.
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Epilog ansehen";

        /// <summary>
        /// Titel der Zwischensequenz.
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Epilog";

        /// <summary>
        /// Einleitung der Zwischensequenz.
        /// </summary>
        public override string EndScreen_Epilogue_Text => "Vor 160 Jahren";

        /// <summary>
        /// Das Prolog ist ein kurzes Gedicht über die Geschichte des Spiels.
        /// </summary>
        public override string GameMenu_WatchPrologue => "Prolog ansehen";

        public override string Prologue_Title => "Prolog";

        /// <summary>
        /// Das Gedicht muss drei Zeilen haben, die vierte Zeile wird aus den Namensübersetzungen gezogen, um den Namen des Bosses anzuzeigen.
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
{
    "Träume verfolgen dich in der Nacht,",
    "Eine Prophezeiung einer dunklen Zukunft",
    "Bereite dich auf seine Ankunft vor,"
};

        /// <summary>
        /// Ingame-Menü beim Pausieren.
        /// </summary>
        public override string GameMenu_Title => "Spielmenü";

        /// <summary>
        /// Weiter das Spiel nach dem Endbildschirm spielen.
        /// </summary>
        public override string GameMenu_ContinueGame => "Weiter";

        /// <summary>
        /// Das Spiel fortsetzen.
        /// </summary>
        public override string GameMenu_Resume => "Fortsetzen";

        /// <summary>
        /// Zum Spiel-Lobby zurückkehren.
        /// </summary>
        public override string GameMenu_ExitGame => "Spiel verlassen";

        public override string Hud_Save => "Speichern";
        public override string GameMenu_SaveStateWarnings => "Warnung! Spielstände gehen verloren, wenn das Spiel aktualisiert wird.";
        public override string GameMenu_LoadState => "Laden";
        public override string GameMenu_ContinueFromSave => "Vom Speicherstand fortsetzen";

        public override string GameMenu_AutoSave => "Automatisches Speichern";

        public override string GameMenu_Load_PlayerCountError => "Du musst die Spieleranzahl entsprechend dem Speicherstand einstellen: {0}";

        public override string Progressbar_MapLoadingState => "Karte wird geladen: {0}";

        public override string Progressbar_ProgressComplete => "abgeschlossen";

        /// <summary>
        /// 0: Fortschritt in Prozent, 1: Fehlversuche
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Erstelle: {0}%. (Fehlversuche {1})";

        /// <summary>
        /// 0: Aktueller Teil, 1: Anzahl der Teile
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "Teil {0}/{1}";

        /// <summary>
        /// 0: Prozentsatz oder „Abgeschlossen“
        /// </summary>
        public override string Progressbar_SaveProgress => "Speichern: {0}";

        /// <summary>
        /// 0: Prozentsatz oder „Abgeschlossen“
        /// </summary>
        public override string Progressbar_LoadProgress => "Laden: {0}";

        /// <summary>
        /// Fortschritt abgeschlossen, wartet auf eine Eingabe des Spielers.
        /// </summary>
        public override string Progressbar_PressAnyKey => "Drücke eine beliebige Taste zum Fortfahren";

        /// <summary>
        /// Ein kurzes Tutorial, in dem du einen Soldaten kaufen und bewegen sollst.
        /// Alle erweiterten Steuerungen sind gesperrt, bis das Tutorial abgeschlossen ist.
        /// </summary>
        public override string Tutorial_MenuOption => "Tutorial starten";
        public override string Tutorial_MissionsTitle => "Tutorial-Missionen";
        public override string Tutorial_Mission_BuySoldier => "Wähle eine Stadt und rekrutiere einen Soldaten";
        public override string Tutorial_Mission_MoveArmy => "Wähle eine Armee und bewege sie";

        public override string Tutorial_CompleteTitle => "Tutorial abgeschlossen!";
        public override string Tutorial_CompleteMessage => "Voller Zoom und erweiterte Spieloptionen freigeschaltet.";

        /// <summary>
        /// Zeigt die Tastenbelegung an.
        /// </summary>
        public override string Tutorial_SelectInput => "Auswählen";
        public override string Tutorial_MoveInput => "Bewegungsbefehl";

        /// <summary>
        /// Anzeige des Kampfes zwischen zwei Armeen.
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "Kriegserklärung";

        public override string ArmyOption_Attack => "Angreifen";

        /// <summary>
        /// Ingame-Einstellungsmenü. Ändere, welche Tasten und Buttons welche Aktionen auslösen.
        /// </summary>
        public override string Settings_ButtonMapping => "Tastenbelegung";



        /// <summary>
        /// Eingabetyp, Standard-PC-Eingabe.
        /// </summary>
        public override string Input_Source_Keyboard => "Tastatur & Maus";

        /// <summary>
        /// Eingabetyp, Handheld-Controller wie z. B. Xbox.
        /// </summary>
        public override string Input_Source_Controller => "Controller";

        /// <summary>
        /// Ressourcen-Menüüberschrift für Verkaufspreise.
        /// </summary>
        public override string CityMenu_SalePricesTitle => "Verkaufspreise";
        public override string Blueprint_Title => "Bauplan";
        public override string Resource_Tab_Overview => "Übersicht";
        public override string Resource_Tab_Stockpile => "Lagerbestand";

        public override string Resource => "Ressource";
        public override string Resource_StockPile_Info => "Setze eine Zielmenge für die Lagerung von Ressourcen, um die Arbeiter zu informieren, wann sie sich anderen Aufgaben widmen sollen.";
        public override string Resource_TypeName_Water => "Wasser";
        public override string Resource_TypeName_Wood => "Holz";
        public override string Resource_TypeName_Fuel => "Brennstoff";
        public override string Resource_TypeName_Stone => "Stein";
        public override string Resource_TypeName_RawFood => "Rohkost";
        public override string Resource_TypeName_Food => "Essen";
        public override string Resource_TypeName_Beer => "Bier";
        public override string Resource_TypeName_Wheat => "Weizen";
        public override string Resource_TypeName_Linen => "Leinen";
        public override string Resource_TypeName_IronOre => "Eisenerz";
        public override string Resource_TypeName_GoldOre => "Golderz";
        public override string Resource_TypeName_Iron => "Eisen";
        public override string Resource_TypeName_SharpStick => "Spitzer Stock";
        public override string Resource_TypeName_Sword => "Schwert";
        public override string Resource_TypeName_KnightsLance => "Ritterlanze";
        public override string Resource_TypeName_TwoHandSword => "Zweihänder";
        public override string Resource_TypeName_Bow => "Bogen";

        public override string Resource_TypeName_LightArmor => "Leichte Rüstung";
        public override string Resource_TypeName_MediumArmor => "Mittlere Rüstung";
        public override string Resource_TypeName_HeavyArmor => "Schwere Rüstung";
        public override string ResourceType_Children => "Kinder";


        public override string BuildingType_DefaultName => "Gebäude";
        public override string BuildingType_WorkerHut => "Arbeiterhütte";
       
        public override string BuildingType_Brewery => "Brauerei";
        public override string BuildingType_Postal => "Postdienst";
        public override string BuildingType_Recruitment => "Rekrutierungszentrum";
        public override string BuildingType_Barracks => "Kaserne";
        public override string BuildingType_PigPen => "Schweinestall";
        public override string BuildingType_HenPen => "Hühnerstall";
        public override string BuildingType_WorkBench => "Werkbank";
        public override string BuildingType_Carpenter => "Zimmermann";
        public override string BuildingType_CoalPit => "Kohlemeiler";
        public override string DecorType_Statue => "Statue";
        public override string DecorType_Pavement => "Pflaster";
        public override string BuildingType_Smith => "Schmied";
        public override string BuildingType_Cook => "Koch";
        public override string BuildingType_Storage => "Lagerhaus";

        public override string BuildingType_ResourceFarm => "{0}-Farm";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Erhöht das Arbeiterlimit um {0}";
        public override string BuildingType_Tavern_Description => "Arbeiter können hier essen";
        public override string BuildingType_Tavern_Brewery => "Bierproduktion";
        public override string BuildingType_Postal_Description => "Versendet Ressourcen in andere Städte";
        public override string BuildingType_Recruitment_Description => "Sendet Männer in andere Städte";
        public override string BuildingType_Barracks_Description => "Nutzt Männer und Ausrüstung zur Rekrutierung von Soldaten";
        public override string BuildingType_PigPen_Description => "Produziert Schweine, die Nahrung und Leder liefern";
        public override string BuildingType_HenPen_Description => "Produziert Hühner und Eier, die Nahrung liefern";
        public override string BuildingType_Decor_Description => "Dekoration";
        public override string BuildingType_Farm_Description => "Erzeugt eine Ressource";

        public override string BuildingType_Cook_Description => "Station zur Herstellung von Nahrungsmitteln";
        public override string BuildingType_Bench_Description => "Station zur Herstellung von Gegenständen";

        public override string BuildingType_Smith_Description => "Metallverarbeitungsstation";
        public override string BuildingType_Carpenter_Description => "Holzbearbeitungsstation";

        public override string BuildingType_Nobelhouse_Description => "Wohnsitz für Ritter und Diplomaten";
        public override string BuildingType_CoalPit_Description => "Effiziente Brennstoffproduktion";
        public override string BuildingType_Storage_Description => "Ablagepunkt für Ressourcen";

        public override string MenuTab_Info => "Info";
        public override string MenuTab_Work => "Arbeit";
        public override string MenuTab_Recruit => "Rekrutieren";
        public override string MenuTab_Resources => "Ressourcen";
        public override string MenuTab_Trade => "Handel";
        public override string MenuTab_Build => "Bauen";
        public override string MenuTab_Economy => "Wirtschaft";
        public override string MenuTab_Delivery => "Lieferung";

        public override string MenuTab_Build_Description => "Platziere Gebäude in deiner Stadt";
        public override string MenuTab_BlackMarket_Description => "Platziere Gebäude in deiner Stadt";
        public override string MenuTab_Resources_Description => "Platziere Gebäude in deiner Stadt";
        public override string MenuTab_Work_Description => "Platziere Gebäude in deiner Stadt";
        public override string MenuTab_Automation_Description => "Platziere Gebäude in deiner Stadt";

        public override string BuildHud_OutsideCity => "Außerhalb der Stadtregion";
        public override string BuildHud_OutsideFaction => "Außerhalb deiner Grenzen!";

        public override string BuildHud_OccupiedTile => "Besetztes Feld";

        public override string Build_PlaceBuilding => "Gebäude";
        public override string Build_DestroyBuilding => "Zerstören";
        public override string Build_ClearTerrain => "Gelände räumen";

        public override string Build_ClearOrders => "Bauaufträge löschen";
        public override string Build_Order => "Bauauftrag";
        public override string Build_OrderQue => "Bauauftragswarteschlange: {0}";
        public override string Build_AutoPlace => "Automatisch platzieren";

        public override string Work_OrderPrioTitle => "Arbeitspriorität";
        public override string Work_OrderPrioDescription => "Priorität reicht von 1 (niedrig) bis {0} (hoch)";

        public override string Work_OrderPrio_No => "Keine Priorität. Wird nicht bearbeitet.";
        public override string Work_OrderPrio_Min => "Minimale Priorität.";
        public override string Work_OrderPrio_Max => "Maximale Priorität.";

        public override string Work_Move => "Gegenstände bewegen";

        public override string Work_GatherXResource => "Sammle {0}";
        public override string Work_CraftX => "Stelle {0} her";
        public override string Work_Farming => "Landwirtschaft";
        public override string Work_Mining => "Bergbau";
        public override string Work_Trading => "Handel";

        public override string Work_AutoBuild => "Automatisch bauen und erweitern";

        public override string WorkerHud_WorkType => "Arbeitsstatus: {0}";
        public override string WorkerHud_Carry => "Trägt: {0} {1}";
        public override string WorkerHud_Energy => "Energie: {0}";
        public override string WorkerStatus_Exit => "Arbeitskraft verlassen";
        public override string WorkerStatus_Eat => "Essen";
        public override string WorkerStatus_Till => "Pflügen";
        public override string WorkerStatus_Plant => "Pflanzen";
        public override string WorkerStatus_Gather => "Sammeln";
        public override string WorkerStatus_PickUpResource => "Ressource aufnehmen";
        public override string WorkerStatus_DropOff => "Abliefern";
        public override string WorkerStatus_BuildX => "Baue {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Zur Armee zurückkehren";


        public override string Hud_ToggleFollowFaction => "Fraktionseinstellungen umschalten";
        public override string Hud_FollowFaction_Yes => "Verwendet globale Fraktionseinstellungen";
        public override string Hud_FollowFaction_No => "Verwendet lokale Einstellungen (Globaler Wert ist {0})";

        public override string Hud_Idle => "Untätig";
        public override string Hud_NoLimit => "Kein Limit";

        public override string Hud_None => "Keine";
        public override string Hud_ProductionQueue => "Produktions-warteschlange";

        public override string Hud_EmptyList => "- Leere Liste -";

        public override string Hud_RequirementOr => "- oder -";

        public override string Hud_BlackMarket => "Schwarzmarkt";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Stadt auswählen";
        public override string Conscription_Title => "Wehrpflicht";
        public override string Conscript_WeaponTitle => "Waffe";
        public override string Conscript_ArmorTitle => "Rüstung";
        public override string Conscript_TrainingTitle => "Training";

        public override string Conscript_SpecializationTitle => "Spezialisierung";
        public override string Conscript_SpecializationDescription => "Erhöht den Angriff in einem Bereich und verringert ihn in allen anderen um {0}";
        public override string Conscript_SelectBuilding => "Kaserne auswählen";

        public override string Conscript_WeaponDamage => "Waffenschaden: {0}";
        public override string Conscript_ArmorHealth => "Rüstungsschutz: {0}";
        public override string Conscript_TrainingSpeed => "Angriffsgeschwindigkeit: {0}";
        public override string Conscript_TrainingTime => "Trainingszeit: {0}";

        public override string Conscript_Training_Minimal => "Minimal";
        public override string Conscript_Training_Basic => "Grundlegend";
        public override string Conscript_Training_Skillful => "Erfahren";
        public override string Conscript_Training_Professional => "Professionell";

        public override string Conscript_Specialization_Field => "Offenes Feld";
        public override string Conscript_Specialization_Sea => "Schiff";
        public override string Conscript_Specialization_Siege => "Belagerung";
        public override string Conscript_Specialization_Traditional => "Traditionell";
        public override string Conscript_Specialization_AntiCavalry => "Antikavallerie";

        public override string Conscription_Status_CollectingEquipment => "Sammle Ausrüstung: {0}";
        public override string Conscription_Status_CollectingMen => "Sammle Männer: {0}";
        public override string Conscription_Status_Training => "Training: {0}";

        public override string ArmyHud_Food_Reserves_X => "Nahrungsmittelvorräte: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Nahrungsmittelunterhalt: {0}";
        public override string ArmyHud_Food_Costs_X => "Nahrungskosten: {0}";

        public override string Deliver_WillSendXInfo => "Wird jeweils {0} senden";
        public override string Delivery_ListTitle => "Lieferservice auswählen";
        public override string Delivery_DistanceX => "Entfernung: {0}";
        public override string Delivery_DeliveryTimeX => "Lieferzeit: {0}";
        public override string Delivery_SenderMinimumCap => "Mindestbestand des Senders";
        public override string Delivery_RecieverMaximumCap => "Maximalbestand des Empfängers";
        public override string Delivery_ItemsReady => "Gegenstände bereit";
        public override string Delivery_RecieverReady => "Empfänger bereit";
        public override string Hud_ThisCity => "Diese Stadt";
        public override string Hud_RecieveingCity => "Empfangende Stadt";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "Angezeigt als Ressourcen pro Sekunde.";

        public override string Info_MinuteAverage => "Der Wert ist ein Durchschnitt der letzten Minute.";

        public override string Message_OutOfFood_Title => "Nahrung aufgebraucht";
        public override string Message_CityOutOfFood_Text => "Teure Nahrung wird vom Schwarzmarkt gekauft. Arbeiter werden verhungern, wenn dein Geld aufgebraucht ist.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Geländetyp";

        public override string Hud_EnergyUpkeepX => "Energieverbrauch durch Nahrung {0}";

        public override string Hud_EnergyAmount => "{0} Energie (Sekunden Arbeit)";

        public override string Hud_CopySetup => "Einstellungen kopieren";
        public override string Hud_Paste => "Einfügen";

        public override string Hud_Available => "Verfügbar";

        public override string WorkForce_ChildBirthRequirements => "Geburtsanforderungen";
        public override string WorkForce_AvailableHomes => "Verfügbare Häuser: {0}";
        public override string WorkForce_Peace => "Frieden";
        public override string WorkForce_ChildToManTime => "Erwachsenenalter: {0} Minuten";

        public override string Economy_TaxIncome => "Steuereinnahmen: {0}";
        public override string Economy_ImportCostsForResource => "Importkosten für {0}: {1}";
        public override string Economy_BlackMarketCostsForResource => "Schwarzmarktpreise für {0}: {1}";
        public override string Economy_GuardUpkeep => "Wachunterhalt: {0}";

        public override string Economy_LocalCityTrade_Export => "Stadthandel Export: {0}";
        public override string Economy_LocalCityTrade_Import => "Stadthandel Import: {0}";

        public override string Economy_ResourceProduction => "{0}-Produktion: {1}";
        public override string Economy_ResourceSpending => "{0}-Verbrauch: {1}";

        public override string Economy_TaxDescription => "Steuern betragen {0} Gold pro Arbeiter";

        public override string Economy_SoldResources => "Verkaufte Ressourcen (Golderz): {0}";


        public override string UnitType_Cities => "Städte";
        public override string UnitType_Armies => "Armeen";
        public override string UnitType_Worker => "Arbeiter";

        public override string UnitType_FootKnight => "Langschwert-Ritter";
        public override string UnitType_CavalryKnight => "Kavallerie-Ritter";

        public override string CityCulture_LargeFamilies => "Große Familien";
        public override string CityCulture_FertileGround => "Fruchtbare Böden";
        public override string CityCulture_Archers => "Erfahrene Bogenschützen";
        public override string CityCulture_Warriors => "Krieger";
        public override string CityCulture_AnimalBreeder => "Tierzüchter";
        public override string CityCulture_Miners => "Bergarbeiter";
        public override string CityCulture_Woodcutters => "Holzfäller";
        public override string CityCulture_Builders => "Baumeister";
        public override string CityCulture_CrabMentality => "Krebmentalität";
        public override string CityCulture_DeepWell => "Tiefer Brunnen";
        public override string CityCulture_Networker => "Netzwerker";
        public override string CityCulture_PitMasters => "Grubenmeister";

        public override string CityCulture_CultureIsX => "Kultur: {0}";
        public override string CityCulture_LargeFamilies_Description => "Erhöhte Geburtenrate";
        public override string CityCulture_FertileGround_Description => "Erhöhte Ernteerträge";
        public override string CityCulture_Archers_Description => "Produziert erfahrene Bogenschützen";
        public override string CityCulture_Warriors_Description => "Produziert erfahrene Nahkämpfer";
        public override string CityCulture_AnimalBreeder_Description => "Tiere liefern mehr Ressourcen";
        public override string CityCulture_Miners_Description => "Schürft mehr Erz";
        public override string CityCulture_Woodcutters_Description => "Bäume liefern mehr Holz";
        public override string CityCulture_Builders_Description => "Schnelleres Bauen";
        public override string CityCulture_CrabMentality_Description => "Arbeit kostet weniger Energie. Kann keine hochqualifizierten Soldaten ausbilden.";
        public override string CityCulture_DeepWell_Description => "Wasser füllt sich schneller auf";
        public override string CityCulture_Networker_Description => "Effizienter Postdienst";
        public override string CityCulture_PitMasters_Description => "Höhere Brennstoffproduktion";

        public override string CityOption_AutoBuild_Work => "Arbeitskraft automatisch erweitern";
        public override string CityOption_AutoBuild_Farm => "Farmen automatisch erweitern";

        public override string Hud_PurchaseTitle_Resources => "Ressourcen kaufen";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "Du besitzt";

        public override string Tutorial_EndTutorial => "Tutorial beenden";
        public override string Tutorial_MissionX => "Mission {0}";
        public override string Tutorial_CollectXAmountOfY => "Sammle {0} {1}";
        public override string Tutorial_SelectTabX => "Wähle Tab: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Priorität erhöhen für: {0}";
        public override string Tutorial_PlaceBuildOrder => "Baubefehl erteilen: {0}";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Wähle eine Stadt";
        public override string Tutorial_ZoomInWorkers => "Hineinzoomen, um die Arbeiter zu sehen";
        public override string Tutorial_CreateSoldiers => "Erstelle zwei Soldateneinheiten mit dieser Ausrüstung: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Herauszoomen zur Kartenübersicht";
        public override string Tutorial_ZoomOutDiplomacy => "Herauszoomen zur Diplomatieansicht";
        public override string Tutorial_ImproveRelations => "Verbessere deine Beziehungen zu einer Nachbarfraktion";
        public override string Tutorial_MissionComplete_Title => "Mission abgeschlossen!";
        public override string Tutorial_MissionComplete_Unlocks => "Neue Steuerungen wurden freigeschaltet";

        // Patch 1
        public override string Resource_ReachedStockpile => "Lagerziel erreicht";

        public override string BuildingType_ResourceMine => "{0}-Mine";

        public override string Resource_TypeName_BogIron => "Moor-Eisen";

        public override string Resource_TypeName_Coal => "Kohle";

        public override string Language_XUpkeepIsY => "{0}-Unterhalt: {1}";
        public override string Language_XCountIsY => "{0}-Anzahl: {1}";

        public override string Message_ArmyOutOfFood_Text => "Teure Nahrung wird vom Schwarzmarkt gekauft. Hungrige Soldaten werden desertieren, wenn dein Geld aufgebraucht ist.";

        public override string Info_ArmyFood => "Armeen versorgen sich mit Nahrung aus der nächstgelegenen freundlichen Stadt. Nahrung kann von anderen Fraktionen gekauft werden. In feindlichen Gebieten kann Nahrung nur auf dem Schwarzmarkt erworben werden.";

        public override string FactionName_Monger => "Monger";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

        // Patch 2
        public override string Tutorial_BuildSomething => "Baue etwas, das {0} produziert";
        public override string Tutorial_BuildCraft => "Baue eine Werkstatt für: {0}";
        public override string Tutorial_IncreaseBufferLimit => "Pufferlimit erhöhen für: {0}";

        /// <summary>
        /// 0: Anzahl, 1: Gegenstandstyp
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "Erreiche einen Lagerbestand von {0} {1}";
        public override string Tutorial_LookAtFoodBlueprint => "Sieh dir die Blaupause für Nahrung an";
        public override string Tutorial_CollectFood_Info1 => "Die Arbeiter gehen zum Rathaus, um zu essen";
        public override string Tutorial_CollectFood_Info2 => "Die Armee schickt Tross-Arbeiter, um Nahrung zu sammeln";
        public override string Tutorial_CollectFood_Info0 => "Willst du die volle Kontrolle über die Arbeiter? Setze alle Arbeitsprioritäten auf Null und aktiviere sie dann einzeln.";

        public override string EndGameStatistics_DecorsBuilt => "Gebauter Schmuck: {0}";
        public override string EndGameStatistics_StatuesBuilt => "Gebauter Statuen: {0}";


        //############
        // WEIHNACHTS-UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "Standardmäßig gehen Arbeiter zum Rathaus, um zu essen oder Gegenstände abzugeben";
        public override string GameMenu_UseSpeedX => "{0}-Geschwindigkeitsoption";
        public override string GameMenu_LongerBuildQueue => "Erweiterte Bauwarteschlange";

        public override string Diplomacy_RelationWithOthers => "Ihre Beziehungen zu anderen";
        public override string Automation_queue_description => "Wird sich wiederholen, bis die Warteschlange leer ist";

        public override string BuildingType_Storehouse_Description => "Arbeiter können hier Gegenstände ablegen";

        public override string Resource_TypeName_Longbow => "Langbogen";
        public override string Resource_TypeName_Rapeseed => "Raps";
        public override string Resource_TypeName_Hemp => "Hanf";

        public override string Resource_BogIronDescription => "Der Bergbau nach Eisen ist effizienter als die Verwendung von Moor-Eisen.";

        public override string Resource_FoodSafeGuard_Description => "Schutzmechanismus. Maximiert die Priorität der Nahrungsproduktion, wenn sie unter {0} fällt.";
        public override string Resource_FoodSafeGuard_Active => "Schutzmechanismus ist aktiv.";

        public override string GameMenu_NextSong => "Nächstes Lied";

        public override string BuildingType_Bank => "Bank";
        public override string BuildingType_GoldDelivery_Description => "Gold an andere Städte senden";

        public override string BuildingType_Logistics => "Logistik";
        public override string BuildingType_Logistics_Description => "Verbessert die Fähigkeit, Gebäude zu beauftragen";

        public override string BuildingType_Logistics_NationSizeRequirement => "Gesamte Arbeitskraft der Nation: {0}";
        public override string Requirements_XItemStorageOfY => "Lagerbestand der Stadt {0} von: {1}";

        public override string XP_UnlockBuildQueue => "Bauwarteschlange freischalten bis: {0}";
        public override string XP_UnlockBuilding => "Gebäude freischalten: ";
        public override string XP_Upgrade => "Verbessern";

        public override string XP_UpgradeBuildingX => "Gebäude verbessern: {0}";

        /// <summary>
        /// Titel zur Beschreibung des Produktionszyklus von Farmen
        /// </summary>
        public override string BuildHud_PerCycle => "Pro Zyklus";
        public override string BuildHud_MayCraft => "Kann hergestellt werden";
        public override string BuildHud_WorkTime => "Arbeitszeit: {0}";
        public override string BuildHud_GrowTime => "Wachstumszeit: {0}";
        public override string BuildHud_Produce => "Produziert:";

        public override string BuildHud_Queue => "Erlaubte Bauwarteschlange: {0}/{1}";

        public override string LandType_Flatland => "Flachland";
        public override string LandType_Water => "Wasser";
        public override string BuildingType_Wall => "Mauer";
        public override string Delivery_AutoReciever_Description => "Wird an die Stadt mit den geringsten Ressourcen gesendet";

        public override string Hud_On => "An";
        public override string Hud_Off => "Aus";

        public override string Hud_Time_Seconds => "{0} Sekunden";
        public override string Hud_Time_Minutes => "{0} Minuten";
        public override string Hud_Undo => "Rückgängig";
        public override string Hud_Redo => "Wiederholen";

        public override string Tag_ViewOnMap => "Markierungen auf der Karte anzeigen";

        public override string MenuTab_Tag => "Markierung";

        public override string Input_Build => "Bauen";

        public override string FlagEditor_ClearAll => "Alles löschen";

        public override string CityCulture_Stonemason => "Steinmetz";
        public override string CityCulture_Stonemason_Description => "Verbesserte Steinsammlung";

        public override string CityCulture_Brewmaster => "Braumeister";
        public override string CityCulture_Brewmaster_Description => "Verbesserte Bierproduktion";

        public override string CityCulture_Weavers => "Weber";
        public override string CityCulture_Weavers_Description => "Erhöhte Produktion von leichter Rüstung";

        public override string CityCulture_SiegeEngineer => "Belagerungsingenieur";
        public override string CityCulture_SiegeEngineer_Description => "Mächtigere Kriegsmaschinen";

        public override string CityCulture_Armorsmith => "Rüstungsschmied";
        public override string CityCulture_Armorsmith_Description => "Verbesserte Produktion von Eisenrüstungen";

        public override string CityCulture_Noblemen => "Adelige";
        public override string CityCulture_Noblemen_Description => "Stärkere Ritter";

        public override string CityCulture_Seafaring => "Seefahrer";
        public override string CityCulture_Seafaring_Description => "Soldaten mit Seespezialisierung haben stärkere Schiffe";

        public override string CityCulture_Backtrader => "Hinterhändler";
        public override string CityCulture_Backtrader_Description => "Günstigerer Schwarzmarkt";

        public override string CityCulture_LawAbiding => "Gesetzestreu";
        public override string CityCulture_LawAbiding_Description => "Mehr Steuereinnahmen. Kein Schwarzmarkt.";

        public override string Hud_Advanced => "Erweitert";
        public override string Hud_Loading => "Lädt...";

        public override string CityOption_LowerGuardSize => "Wache entlassen";
        public override string Hud_Purchase_MinCapacity => "Minimale Kapazität erreicht";
        public override string Settings_ResetToDefault => "Auf Standard zurücksetzen";
        public override string Settings_NewGame => "Neues Spiel";

        public override string Settings_AdvancedGameSettings => "Erweiterte Spieleinstellungen";
        public override string Settings_FoodMultiplier => "Nahrungsmittel-Multiplikator";
        public override string Settings_FoodMultiplier_Description => "Wie lange ein Arbeiter oder Soldat mit vollem Magen auskommt. Ein hoher Wert kann die Computerleistung verringern.";

        public override string Settings_GameMode => "Spielmodus";

        public override string Settings_Mode_Story => "Komplette Geschichte";
        public override string Settings_Mode_IncludeBoss => "Boss-Ereignisse einbeziehen.";
        public override string Settings_Mode_IncludeAttacks => "Zufällige Angriffe einbeziehen.";
        public override string Settings_Mode_Sandbox => "Sandkastenmodus";
        public override string Settings_Mode_Peaceful => "Friedlich";
        public override string Settings_Mode_Peaceful_Description => "Alle Kriege werden nur vom Spieler initiiert";

        public override string Lobby_ImportSave => "Spielstand importieren";

        public override string Lobby_ExportSave => "Spielstand exportieren";
        public override string Lobby_ExportSave_Description => "Erstellt eine Kopie der Datei und speichert sie im Import-Ordner: {0}";

        public override string Resource_CurrentAmount => "Aktuelle Menge: {0}";
        public override string Resource_MaxAmount_Soft => "Weiches Limit (Maximale Grenze): {0}";
        public override string Resource_MaxAmount => "Maximales Limit: {0}";
        public override string Resource_AddPerSec => "Erhöhungsrate: {0} pro Sekunde";

        public override string Resource_WaterAddLimit => "Die Erhöhungsrate für Wasser kann nicht geändert werden";

        public override string Tutorial_Select_SubTab => "Und wähle die Kategorie: {0}";


        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */

        public override string Tutorial_OpenGuardSubTab => "Öffne eine Kaserne und wähle die Kategorie: {0}";
        public override string Tutorial_GuardToWall => "Bewege einen Wächter an die Mauer";
        public override string Demo_MissionObjective_Title => "Missionsziel";
        public override string Demo_MissionObjective_Description => "Verteidige dich gegen den Angriff aus dem Süden";
        public override string Demo_Complete_Title => "Demo abgeschlossen";
        public override string Demo_TimesUp_Title => "Zeit abgelaufen!";
        public override string Demo_EndInOneMinuteDescription => "Die Demo endet in einer Minute";

        public override string ArmyOption_NewArmy => "Neue Armee";
        public override string ProfileEditor_AltMain => "Alternative Hauptauswahl";
        public override string Automation_CheckBoxTitle => "Automatisiert";

        public override string ArmyStructure_ColumnWidth => "Armeespaltenbreite";
        public override string ArmyStructure_ArmyPlacement => "Platzierung in der Armee";
        public override string ArmyStructure_Row_Front => "Vorne";
        public override string ArmyStructure_Row_Body => "Mitte";
        public override string ArmyStructure_Row_Second => "Zweite Reihe";
        public override string ArmyStructure_Row_Behind => "Hinten";

        public override string Diplomacy_RelationType_Enemies => "Feinde";

        public override string EventMessage_EnemyAlliance_Title => "Angst vor Vorherrschaft";
        public override string EventMessage_EnemyAlliance => "Die Nationen, die deine wachsende Macht fürchten, schließen sich zu einem Bündnis gegen dich zusammen.";

        public override string Settings_CentralGold => "Zentrales Gold";
        public override string Settings_CentralGold_Description => "Ein: Dein gesamtes Gold befindet sich in einem gemeinsamen Pool zur sofortigen Nutzung. Aus: Gold ist physisch und muss transportiert werden.";

        public override string InputActionName_StopStart => "Start/Stopp";
        public override string InputActionName_ToggleHudDetail => "HUD-Details umschalten";
        public override string InputActionName_NextCity => "Nächste Stadt";
        public override string InputActionName_NextArmy => "Nächste Armee";
        public override string InputActionName_NextBattle => "Nächste Schlacht";
        public override string InputActionName_Build => "Bauen";
        public override string InputActionName_Copy => "Kopieren";
        public override string InputActionName_Paste => "Einfügen";
        public override string InputActionName_Menu => "Menü";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "Vorherige Farbe";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "Nächste Farbe";
        public override string InputActionName_FlagDesign_PaintBucket => "Farbeimer";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "Farbwähler";
        public override string InputActionName_ControllerFocus => "Fokus";
        public override string InputActionName_ControllerCancel => "Abbrechen";
        public override string InputActionName_ControllerMessageClick => "Nachricht auswählen";
        public override string InputActionName_ControllerSelect => "Auswählen";
        public override string InputActionName_WASD_UP => "Hoch";
        public override string InputActionName_WASD_DOWN => "Runter";
        public override string InputActionName_WASD_LEFT => "Links";
        public override string InputActionName_WASD_RIGHT => "Rechts";
        public override string InputActionName_CameraTiltLeft => "Kamera nach links kippen";
        public override string InputActionName_CameraTiltRight => "Kamera nach rechts kippen";
        public override string InputActionName_CameraTiltUp => "Kamera nach oben kippen";
        public override string InputActionName_ZoomInKey => "Hineinzoomen";
        public override string InputActionName_ZoomOutKey => "Herauszoomen";

        public override string Settings_Title_Monitor => "Monitoroptionen";
        public override string Settings_Title_Graphics => "Grafikoptionen";
        public override string Settings_Title_Input => "Eingabe";
        public override string Settings_Title_Gameplay => "Spieloptionen";
        public override string Settings_PanOnZoom => "Schwenken beim Zoomen";
        public override string Settings_ScrollSensitivity_Game => "Scroll-Empfindlichkeit: Spiel";
        public override string Settings_ScrollSensitivity_Menu => "Scroll-Empfindlichkeit: Menü";
        public override string Settings_Blood => "Blut";

        public override string Settings_MasterVolume => "Gesamtlautstärke";
        public override string Settings_AmbienceVolume => "Umgebungslautstärke";
        public override string Settings_BattleMelody => "Kampfmusik";

        public override string Settings_ModelLight => "Modelleffektlicht";
        public override string Settings_Particles => "Partikeleffekte";
        public override string Settings_MapLoadSpeed => "Kartenladegeschwindigkeit";
        public override string Lobby_Category_Options => "Optionen";
        public override string Lobby_Category_Editor => "Editor";
        public override string Lobby_Category_ExtraModes => "Zusätzliche Modi";

        public override string Lobby_Editor_MapEditor => "Karteneditor";
        public override string Lobby_Editor_VoxelEditor => "Voxel-Editor";

        public override string Lobby_Mode_BattleLab => "Kampflabor";
        public override string Lobby_Mode_BattleLab_Description => "Lasse beliebige Soldaten gegeneinander antreten";
        public override string Lobby_Mode_Commander => "Commander spielen";
        public override string Lobby_Mode_Commander_Description => "Ein kleines taktisches Brettspiel";
        public override string Lobby_MusicPlayList => "Musikwiedergabeliste";

        public override string Lobby_GameSetup => "Spielsetup";
        public override string Lobby_PlayerSetup => "Spielereinstellungen";
        public override string LobbyDemoMode_Demo => "Demo";

        public override string Lobby_Tutorial => "Tutorial";
        public override string LobbyDemoMode_ShortTutorial => "Kurzes Tutorial";
        public override string LobbyDemoMode_LongTutorial => "Erweitertes Tutorial";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "Zur Wunschliste hinzufügen";
        public override string BattleLab_StartHere => "Schlacht hier starten";
        public override string BattleLab_Start => "Schlacht starten";
        public override string BattleLab_Attacker => "Angreifer";

        public override string MapGenerator_Name => "Karteneditor – generieren";

        public override string MapType_CustomMap => "Benutzerdefinierte Karte";
        public override string MapType_GenerateNewMap => "Neue Karte generieren";
        public override string MapGenerator_GenerateAction => "Generieren";
        public override string MapGenerator_Terrain_CustomSize => "Benutzerdefinierte Größe";
        public override string MapGenerator_Terrain_StartAs => "Starten als";
        public override string MapGenerator_Terrain_ClearPass => "Clear-Pass ausführen";
        public override string MapGenerator_Terrain_BuildPass => "Build-Pass ausführen";
        public override string MapGenerator_Terrain_DigPass => "Dig-Pass ausführen";
        public override string MapGenerator_Terrain_BuildDigLoops => "Anzahl Build-Dig-Schleifen";
        public override string MapGenerator_Terrain_BuildStrokes => "Anzahl der Baupinselstriche";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Gemessen in Pinselstrichen pro 100 Kacheln";
        public override string MapGenerator_Terrain_DigStrokes => "Anzahl der Grabpinselstriche";
        public override string MapGenerator_Terrain_CleanUp_Option => "Einzelfeldbereinigung";
        public override string MapGenerator_Terrain_CleanUpPass => "Cleanup-Pass ausführen";

        public override string Economy_ServicemenUpkeep => "Unterhalt für Dienstleute: {0}";
        public override string Economy_ServicemenUpkeep_Description => "Unterhalt beträgt {0} Gold pro Dienstmann";
        public override string Economy_GuardUpkeep_Description => "Unterhalt beträgt {0} Gold pro Wächter";

        public override string EndScreen_TimeHasEndedTitle => "Zeit abgelaufen";
        public override string Hud_AdvancedSettings => "Erweiterte Einstellungen";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "Abbrechen";
        public override string Hud_Delete => "Löschen";
        public override string Hud_Next => "Weiter";
        //public override string Hud_None => "Keine";
        public override string Hud_Apply => "Anwenden";
        public override string Hud_AllCities => "Alle Städte";
        public override string Hud_Time_Hours => "{0} Stunden";
        public override string Hud_AddX => "{0} hinzufügen";
        public override string Hud_Both => "Beide";
        public override string Hud_Direction => "Richtung";
        public override string MusicIsBroken => "Musik ist derzeit defekt";

        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, Anzahl: {1}";

        public override string Hud_EffectDoesNotStack => "Dieser Effekt ist nicht stapelbar";

        public override string Work_SmeltX => "{0} schmelzen";

        public override string Info_TotalFoodProduction => "Gesamte Nahrungsproduktion";
        public override string Info_TotalFoodSpending => "Gesamter Nahrungsverbrauch";

        public override string Info_FooodAndDeliveryLocation => "Standardmäßig gehen Arbeiter zum Rathaus, um zu essen oder Gegenstände abzugeben";

        public override string Delivery_SendChunk => "Gegenstände pro Lieferung";
        public override string Delivery_SpeedBonus => "Geschwindigkeitsbonus: {0}%";

        public override string Delivery_AutoResourceDescription => "Liefert Gegenstände, die das Lagerlimit erreicht haben, an bedürftige Städte";

        public override string Conscript_Soldiers_ArmyType => "Soldaten";
        public override string Conscript_Soldiers_ArmyType_Description => "Rekrutiere Soldaten in eine benachbarte Armee";
        public override string Conscript_Soldiers_GuardType => "Stadtwache";
        public override string Conscript_Soldiers_GuardType_Description => "Wachen werden zur Verstärkung von Mauern eingesetzt";

        public override string Defence_Title => "Verteidigung";
        public override string Defence_GuardPost => "Wachposten";

        public override string Defence_WallDescription_Movement => "Erschwert feindliche Bewegungen.";
        public override string Defence_WallDescription_GuardPost => "Wache kann hier stationiert werden.";
        public override string Defence_AutoAssign => "Automatisch zuweisen";
        public override string Defence_AutoAssign_Description => "Neue Wachen begeben sich zu diesem Posten";

        public override string Conscript_SplashDamage => "Flächenschaden";
        public override string Conscript_HighSplashDamage => "Starker Flächenschaden";

        public override string Conscript_Training_Champion => "Champion";
        public override string Conscript_Training_Legendary => "Legendär";

        public override string Experience_Title => "Erfahrung";
        public override string Experience_TopExperience => "Höchste Erfahrungsstufen";

        public override string Experience_TimeReductionDescription => "Arbeitszeit wird pro Stufe um {0}% reduziert";

        public override string ExperienceType_Farm => "Landwirt";
        public override string ExperienceType_AnimalCare => "Tierpflege";
        public override string ExperienceType_HouseBuilding => "Hausbauer";
        public override string ExperienceType_WoodWork => "Holzarbeiter";
        public override string ExperienceType_StoneCutter => "Steinmetz";
        public override string ExperienceType_Mining => "Bergarbeiter";
        public override string ExperienceType_Transport => "Transport";
        public override string ExperienceType_Cook => "Koch";
        public override string ExperienceType_Fletcher => "Bogenmacher";
        public override string ExperienceType_RefineOre => "Schmelzer";
        public override string ExperienceType_Casting => "Gießer";
        public override string ExperienceType_CraftMetal => "Schmied";
        public override string ExperienceType_CraftArmor => "Rüstungsschmied";
        public override string ExperienceType_CraftWeapon => "Waffenschmied";
        public override string ExperienceType_CraftFuel => "Köhler";
        public override string ExperienceType_Chemist => "Chemiker";

        public override string ExperienceLevel_1 => "Anfänger";
        public override string ExperienceLevel_2 => "Fortgeschrittener";
        public override string ExperienceLevel_3 => "Experte";
        public override string ExperienceLevel_4 => "Meister";
        public override string ExperienceLevel_5 => "Legendär";

        public override string ExperenceOrDistancePrio_Title => "Arbeiterwahl";
        public override string ExperenceOrDistancePrio_Description => "Arbeitslose Arbeiter werden je nach Entfernung oder Erfahrung ausgewählt";

        public override string Technology_Description => "Jede Stadt hat einen Technologiebaum. Jede Technologie schaltet Gebäude und Gegenstände frei.";
        public override string Experience_Description => "Arbeiter sammeln Erfahrung und verbessern sich";

        public override string Technology_Title => "Technologie";
        public override string Technology_ShareField => "Technologiefeld teilen";

        public override string Technology_GainByNeigborRelation => "Für jede Nachbarstadt mit der Technologie. Und deine Beziehung ist {0}: {1}";
        public override string Technology_ForEachMaster => "Wenn ein(e) {0} die Erfahrungsstufe {1} erreicht, im Technologiefeld: {2}";
        public override string Technology_CitySpread => "Deine Städte teilen Technologie, wenn sie benachbart sind: {0}";
        public override string Technology_CityCapture => "Die meisten Technologien gehen verloren, wenn eine Stadt im Kampf eingenommen wird";

        public override string Technology_AdvancedBuildings => "Fortschrittliche Gebäude";
        public override string Technology_AdvancedFarming => "Fortschrittliche Landwirtschaft";
        public override string Technology_AdvancedCasting => "Fortschrittlicher Guss";

        public override string Help_Title => "Hilfe";
        public override string Help_Work_Title => "Arbeit beginnt nicht";
        public override string Help_Work_Resources => "Gebäude benötigen verfügbare Ressourcen";
        public override string Help_Work_Skill => "Der Arbeiter benötigt das passende Fähigkeitsniveau (oder höher)";
        public override string Help_Work_Stockpile => "Das Sammeln von Ressourcen wird durch ein volles Lager blockiert";
        public override string Help_Work_Priority => "Die Arbeit hat möglicherweise eine niedrige oder keine Priorität";

        public override string Help_Soldiers_Title => "Soldaten ausbilden";
        public override string Help_Soldiers_PlaceBuildingX => "Gebäude platzieren: {0}";
        public override string Help_Soldiers_Workers => "Verfügbare Arbeiter zur Rekrutierung";
        public override string Help_Soldiers_Weapon => "Jeder Soldat benötigt eine Waffe";
        public override string Help_Soldiers_StartX => "Start: {0}";

        public override string Hud_SelectHistory => "Verlauf auswählen";

        public override string Hud_PointsPerMinute => "{0} Punkte pro Minute";
        public override string Hud_PercentValueCost => "Der Dienst kostet {0}% des Wertes";

        public override string Hud_Mixed => "Gemischt";
        public override string Hud_Distance => "Entfernung";

        public override string Hud_Unlock => "Freischalten";
        public override string Hud_category => "Kategorie";

        /// <summary>
        /// Setzt die Spielgeschwindigkeit auf Einzelbildmodus
        /// </summary>
        public override string Input_StepOneFrame => "1 Bild weiter";

        public override string Resource_TypeName_Wagon2Wheel => "Kleiner Wagen";
        public override string Resource_TypeName_Wagon4Wheel => "Großer Wagen";
        public override string Resource_TypeName_Tin => "Zinn";
        public override string Resource_TypeName_TinOre => "Zinnerz";

        public override string Resource_TypeName_Copper => "Kupfer";
        public override string Resource_TypeName_CopperOre => "Kupfererz";
        public override string Resource_TypeName_SilverOre => "Silbererz";
        public override string Resource_TypeName_Silver => "Silber";

        /// <summary>
        /// Mithril ist ein Fantasiemetall
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Rohes Mithril";
        public override string Resource_TypeName_Mithril => "Mithril";

        public override string Resource_TypeName_BronzeSword => "Bronzeschwert";
        public override string Resource_TypeName_ShortSword => "Kurzschwert";
        public override string Resource_TypeName_LongSword => "Langschwert";
        public override string Resource_TypeName_HandSpear => "Handspeer";
        public override string Resource_TypeName_Warhammer => "Kriegshammer";
        public override string Resource_TypeName_MithrilSword => "Mithrilschwert";
        public override string Resource_TypeName_SlingShot => "Schleuder";
        public override string Resource_TypeName_ThrowingSpear => "Wurfspeer";
        public override string Resource_TypeName_Crossbow => "Armbrust";
        public override string Resource_TypeName_MithrilBow => "Mithrilbogen";

        public override string Resource_TypeName_CoolingFluid => "Kühlflüssigkeit";
        public override string Resource_TypeName_Palisade => "Palisade";
        public override string Resource_TypeName_Toolkit => "Werkzeugkasten";

        public override string Resource_TypeName_Sulfur => "Schwefel";
        public override string Resource_TypeName_LeadOre => "Bleierz";
        public override string Resource_TypeName_Lead => "Blei";
        public override string Resource_TypeName_Bronze => "Bronze";
        public override string Resource_TypeName_BloomIron => "Rohstahl";
        public override string Resource_TypeName_Steel => "Stahl";
        public override string Resource_TypeName_CastIron => "Gusseisen";

        public override string Resource_TypeName_BlackPowder => "Schwarzpulver";
        public override string Resource_TypeName_GunPowder => "Schießpulver";
        public override string Resource_TypeName_LedBullet => "Kugel";

        public override string Resource_TypeName_HandCannon => "Handkanone";
        public override string Resource_TypeName_HandCulverin => "Handbüchse";
        public override string Resource_TypeName_Rifle => "Gewehr";
        public override string Resource_TypeName_Blunderbuss => "Donnerbüchse";

        public override string Resource_TypeName_Manuballista => "Manubaliste";
        public override string Resource_TypeName_Catapult => "Katapult";
        public override string Resource_TypeName_BatteringRam => "Rammbock";
        public override string Resource_TypeName_SiegeCannonBronze => "Basilisk";
        public override string Resource_TypeName_ManCannonBronze => "Bombarde";
        public override string Resource_TypeName_SiegeCannonIron => "Haubitze";
        public override string Resource_TypeName_ManCannonIron => "Kanone";

        public override string Resource_TypeName_PaddedArmor => "Gepolsterte Rüstung";
        public override string Resource_TypeName_HeavyPaddedArmor => "Schwere gepolsterte Rüstung";

        public override string Resource_TypeName_IronArmor => "Kettenrüstung";
        public override string Resource_TypeName_HeavyIronArmor => "Schwere Kettenrüstung";

        public override string Resource_TypeName_BronzeArmor => "Bronzerüstung";

        public override string Resource_TypeName_LightPlateArmor => "Plattenrüstung";
        public override string Resource_TypeName_FullPlateArmor => "Vollplattenrüstung";
        public override string Resource_TypeName_MithrilArmor => "Mithrilrüstung";
        public override string Resource_TypeName_Coin => "Münze";

        public override string UnitType_Warhammer => "Hammerritter";
        
        public override string UnitType_SpearAndShield => "Linienkämpfer";

        public override string UnitType_CollectionOfSoldiers => "Soldatenbündel";
        public override string UnitType_CollectionOfArmies => "Armeenbündel";

        /// <summary>
        /// Die ID ist eine eindeutige Zahl
        /// </summary>
        public override string UnitId => "(ID {0})";

        public override string BuildHud_AreaEffectTitle => "Gebietseffekt";
        public override string BuildHud_BonusRadius => "Bonusradius: {0}";

        public override string BuildHud_BuildTime => "Bauzeit";
        public override string SchoolHud_ToLevel => "Bis Stufe";
        public override string SchoolHud_TimeDescription => "Zeit basiert auf null Erfahrung; reduziert sich mit Erfahrung.";
        public override string SchoolHud_SelectSchool => "Schule auswählen";
        public override string Upgrade_Order => "Aufrüstungsreihenfolge";

        public override string Building_ListDescription => "Eine Liste aller Gebäude in dieser Kategorie";

        public override string BuildingType_IsUpgraded => "{0} – verbessert";
        public override string BuildingType_WoodCutter => "Sägewerk";
        public override string BuildingType_Workshop_Description => "Verbessert die Arbeit im Umkreis";

        public override string BuildingType_WoodCutter_AreaAffect => "{0}% mehr Holz von Bäumen";

        public override string BuildingType_StoneCutter_AreaAffect => "{0}% mehr Stein";

        public override string BuildingType_StoneCutter => "Steinbruch";

        public override string BuildingType_Embassy => "Botschaft";
        public override string BuildingType_Embassy_Description => "Für diplomatische Beziehungen";

        public override string BuildingType_SoldierBarracks => "Kaserne (Soldaten)";
        public override string BuildingType_ArcherBarracks => "Kaserne (Bogenschützen)";
        public override string BuildingType_WarmachineBarracks => "Kaserne (Kriegsmaschinen)";
        public override string BuildingType_GunBarracks => "Kaserne (Schützen)";
        public override string BuildingType_CannonBarracks => "Kaserne (Kanonen)";
        public override string BuildingType_KnightsBarracks => "Kaserne (Ritter)";

        public override string BuildingType_WaterResovoir => "Wasserspeicher";
        public override string BuildingType_WaterResovoir_Description => "Erhöht die Wasserspeicherung";

        public override string BuildingType_SmeltingFurnace => "Schmelzofen";
        public override string BuildingType_SmeltingFurnace_Description => "Erz zu Metall verarbeiten";

        public override string BuildingType_Foundry => "Gießerei";
        public override string BuildingType_Foundry_Description => "Station für Metallguss";

        public override string BuildingType_Armory => "Rüstkammer";
        public override string BuildingType_Armory_Description => "Station zum Herstellen von Rüstungen";

        public override string BuildingType_Chemist => "Alchemist";
        public override string BuildingType_Chemist_Description => "Station zur Herstellung von Chemikalien";

        public override string BuildingType_CoinMaker => "Münzprägestelle";
        public override string BuildingType_CoinMaker_Description => "Verwandelt Metalle in Geld";

        public override string BuildingType_Gunmaker => "Waffenmanufaktur";
        public override string BuildingType_Gunmaker_Description => "Herstellung von Schusswaffen und Kanonen";

        public override string BuildingType_School_Tab => "Schule";
        public override string BuildingType_School => "Gilde der Meister";
        public override string BuildingType_School_Description => "Erhöht das Fähigkeitsniveau der Arbeiter";

        public override string BuildingType_GoldDelivery => "Goldkurier";
        public override string BuildingType_Bank_Description => "Goldverwaltung";

        public override string DecorType_CobbleStones => "Kopfsteinpflaster";
        public override string DecorType_Square => "Stadtplatz";

        public override string DecorType_Garden => "Garten";
        public override string DecorType_Flag => "Fahne";
        public override string DecorType_Banner => "Banner";

        public override string BuildingType_DirtRoad => "Lehmpfad";
        public override string BuildingType_Palisade => "Palisadenfestung";

        public override string ResourceType_ServiceMen => "Dienende";
        public override string BuildingType_ServiceHouse => "Diensthaus";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "Fügt {0} Dienende hinzu";

        public override string BuildingType_GuardOffice => "Wachbüro";
        public override string BuildingType_GuardOffice_DescriptionAddX => "Erhöht das Wachlimit um {0}";

        public override string BuildingType_DirtWall => "Lehmwand";
        public override string BuildingType_DirtTower => "Lehmturm";
        public override string BuildingType_WoodWall => "Holzwand";
        public override string BuildingType_WoodTower => "Holzturm";
        public override string BuildingType_StoneWall => "Steinmauer";
        public override string BuildingType_StoneTower => "Steinturm";
        public override string BuildingType_StoneGate => "Steintor";
        public override string BuildingType_StoneHouse => "Steinhaus";

        /// <summary>
        /// Beim Auflisten kleiner Varianten, z. B. „Lampe A“ und „Lampe B“
        /// </summary>
        public override string VariantType_A => "{0} A";
        public override string VariantType_B => "{0} B";
        public override string VariantType_C => "{0} C";
        public override string VariantType_D => "{0} D";
        public override string VariantType_E => "{0} E";
        public override string VariantType_F => "{0} F";
        public override string VariantType_G => "{0} G";
        public override string VariantType_H => "{0} H";

        public override string BuildingToolShape_Free => "Stift";
        public override string BuildingToolShape_Area => "Rechteck";
        public override string BuildingToolShape_Line => "Linie";
        public override string BuildingToolShape_LShape => "L-Form";

        public override string CityHall_Upgrade => "Rathaus aufrüsten";

        /// <summary>
        /// Begrenzung der maximal unterstützten Arbeiteranzahl
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Maximale unterstützte Arbeiter: {0}";

        public override string CityHall_Size_Small => "Dorf";
        public override string CityHall_Size_Medium => "Stadt";
        public override string CityHall_Size_Large => "Hauptstadt";

        public override string GuardHousingCount => "Wachunterkunft";
        public override string ServicemenCount => "Dienstpersonal: {0}";

        public override string Work_MiningResource => "Abbau von {0}";

        public override string MenuTab_Progress => "Fortschritt";

        public override string Automation_AutomateCity => "Stadt automatisieren";
        public override string Automation_AutomationFocus => "Automatisierungsfokus";
        public override string Automation_AutomationFocus_Grow => "Wachstum";
        public override string Automation_AutomationFocus_Export => "Export";
        public override string Automation_AutomationFocus_War => "Krieg";

        public override string CityCulture_Smelters_Description => "Verbesserte Erzverhüttung";
        public override string CityCulture_Smelters => "Schmelzer";

        public override string CityCulture_Apprentices_Description => "Neue Arbeiter erhalten Erfahrung von erfahrenen Kollegen";
        public override string CityCulture_Apprentices => "Lehrlinge";

        public override string CityCulture_BronzeCasters_Description => "Verbesserte Produktion von Bronze und Bronzeartikeln";
        public override string CityCulture_BronzeCasters => "Bronzegießer";


        //DEMO PATCH 1
        /// <summary>
        /// Böse Orks, die über die Karte streifen
        /// </summary>
        public override string FactionName_Barbarian => "Dunkle Horde";
        public override string Tutorial_AttackAndDestroyX => "Angreifen und zerstören: {0}";
        public override string Resource_TypeName_Pike => "Pike";

        public override string BattleTrials_Title => "Kampfübungen";
        public override string BattleTrials_Description => "Teste deine Taktik in einem direkten Armee-gegen-Armee-Gefecht.";

        //DEMO PATCH 2

        public override string Conscript_BlockReducingAttack => "Diese Angriffe verringern die Blockchance";

        public override string Conscript_BlockPerSecond => "Kann bis zu {0} Mal pro Sekunde blocken";

        public override string Conscript_BlockDescription => "Soldaten blocken die meisten Angriffe, die aus ihrem vorderen Bereich kommen";

        public override string Map_CustomSeed => "Karten-Seed";

        public override string Settings_Mode_Spectator => "Zuschauer";

        //public override string Settings_Mode_Spectator_Description => "Nur zuschauen";

        public override string Automation_AutomationFocus_NoFocus_Description => "Baut von allem ein wenig";

        public override string Automation_AutomationFocus_WillProduce => "Wird hauptsächlich produzieren:";

        public override string Help_Food_WhoEats => "Alle Soldaten und Arbeiter verbrauchen Nahrung";

        public override string Help_Food_BigArmy => "Eine große Armee kann die Stadt in ihrem Gebiet aushungern";

        public override string Help_Food_DontBuild => "Mehr Bauernhöfe erhöhen die Nahrungsproduktion nicht automatisch – du brauchst verfügbare Arbeiter und Kochstationen, um die Nahrung zu sammeln und zu verarbeiten";

        public override string Help_Food_UseWater => "Für die Nahrungsproduktion wird Wasser benötigt";

        public override string Help_Food_Postal => "Stelle sicher, dass sich deine Städte gegenseitig mit Nahrung versorgen";

        public override string Message_LostCity => "Stadt verloren";

        public override string Demo_Description => "Ein kurzes Szenario: Verteidige deine Stadt für {0} Minuten";


        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "Die Demo endet in {0} Minuten";

        public override string Experience_Required => "Erforderliche Erfahrung";

        public override string InputActionName_ToggleMenu => "Menü umschalten";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "Ressourcen können unter null fallen und leicht das Lagerlimit überschreiten. Die Grenzen werden nur beim Erstellen der Arbeitswarteschlange überprüft.";

        public override string Work_SelectCategory => "Kategorie auswählen";
        public override string Hud_RemoveFromList => "Aus der Liste entfernen";

        public override string Hud_ReturnToPrevious => "Zurück";
        public override string Hud_Close => "Schließen";

        public override string Hud_Low => "Niedrig";
        public override string Hud_Medium => "Mittel";
        public override string Hud_High => "Hoch";

        public override string Hud_Copy => "Kopieren";
        //public override string Hud_Paste => "Einfügen";
        public override string Hud_Cut => "Ausschneiden";
        public override string Hud_SaveCompleted => "Speichern abgeschlossen";

        public override string Settings_WaterMultiplier => "Wasser-Multiplikator";
        public override string Settings_WaterMultiplier_Description => "Bestimmt, wie viel Wasser Städte produzieren und speichern. Höhere Werte verringern die Leistung des Computers.";

        public override string Settings_ChildMultiplier => "Geburtenrate-Multiplikator";
        public override string Settings_CraftMultiplier_Description => "Niedrigere Werte führen zu schnellerer Produktion.";

        public override string FastProduction => "Schnelle Produktion";
        public override string SlowProduction => "Langsame Produktion";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "Wird nicht produziert";

        //public override string CityAutomation_WaitForMaxPopulation => "Warten, bis maximale Bevölkerung erreicht ist";
        public override string Automation_AutomationFocus_NoFocus => "Alle";
        public override string CityAutomation_SoldierQuality => "Soldatenqualität";
        public override string CityAutomation_SoldierWeaponType => "Waffentyp";

        public override string WarsResourceGroup_Resources => "Ressourcen";
        public override string WarsResourceGroup_Weapons => "Waffen";

        public override string WarsResourceGroup_AllWeaponTypes => "Gemischt";
        public override string WarsResourceGroup_MeleeHandWeapons => "Nahkampf";
        public override string WarsResourceGroup_RangedHandWeapons => "Fernkampf";
        public override string WarsResourceGroup_Warmachines => "Kriegsmaschinen";

        public override string FactionSettings_Titel => "Fraktionsweite Einstellungen";
        public override string FactionSettings_Description => "Gilt für alle deine Städte";

        public override string Conscript_MaxPopulation => "Maximale Bevölkerung";
        public override string Conscript_MaxPopulation_Description => "Rekrutiert nur, wenn die Bevölkerung das Maximum erreicht hat";

        public override string Conscript_FoodAbundance => "Maximale Lebensmittelvorräte";
        public override string Conscript_FoodAbundance_Description => "Rekrutiert nur, wenn die Nahrungsmittel das maximale Lager erreicht haben";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Setzen auf: Ein";
        public override string GeneralSetting_Off => "Setzen auf: Aus";
        public override string GeneralSetting_AllBuildingsDescription => "Wird auf alle Gebäude angewendet";

        public override string GeneralSetting_ApplyMessage => "Änderung auf {0} Gebäude angewendet";

        public override string MustTurnOffSteamInput => "Um Controller zu verwenden, musst du Steam Input deaktivieren.";

        public override string Technology_GainTitle => "Möglichkeiten, Technologie zu erlangen";
        public override string Technology_LevelUp => "Levelaufstieg";
        public override string Technology_ForEachLevelUp => "Wenn ein Arbeiter im Technologiebereich aufsteigt: {0}";

        public override string VoxelEditor_Description => "Erstelle klotzige Modelle";


        public override string Editor_Tool => "Werkzeug";
        public override string Editor_SelectOptionsMenu => "Auswahloptionen";
        public override string Editor_Continous => "Kontinuierlich";

        public override string Editor_Tool_PencilSize => "Stiftgröße";
        public override string Editor_Tool_SizeTolerance => "Größentoleranz";
        public override string Editor_Tool_RoundPencil => "Runder Stift";
        public override string Editor_Tool_EdgeSize => "Kantengröße";
        public override string Editor_Tool_PercentFill => "Prozentfüllung";
        public override string Editor_Tool_ClearAbove => "Darüber löschen";
        public override string Editor_Tool_FillBelow => "Darunter füllen";

        public override string Editor_UserModels => "Benutzermodelle";
        public override string Editor_UserModels_Description => "Durchsuche gespeicherte Modelle";

        public override string Editor_RetailModels => "Spielmodelle";
        public override string Editor_RetailModels_Description => "Lade Modelle aus dem Spiel";

        public override string Editor_ModTemplates => "Modding-Vorlagen";
        public override string Editor_ExportAsOBJ => "Als .OBJ exportieren";
        public override string Editor_SelectAll => "Alle auswählen";

        public override string Editor_Canvas_Title => "Leinwand";
        public override string Editor_Canvas_Size => "Größe";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "Größenvoreinstellungen";
        public override string Editor_Canvas_Move => "Bewegen";
        public override string Editor_Canvas_Move_Up => "Nach oben";
        public override string Editor_Canvas_Move_Down => "Nach unten";
        public override string Editor_Canvas_RotateClockwise => "Im Uhrzeigersinn drehen";
        public override string Editor_Canvas_RotateCounterClockwise => "Gegen den Uhrzeigersinn drehen";
        public override string Editor_Canvas_Mirror => "Spiegeln";

        public override string Editor_Canvas_RotateFlip_Title => "Drehen/Spiegeln";
        public override string Editor_Canvas_FlipVertical => "Vertikal spiegeln";
        public override string Editor_Canvas_FlipOrientation => "Liegend/Stehend umkehren";
        public override string Editor_Canvas_ClearAll_Description => "Entfernt alle Blöcke und Frames";

        public override string Editor_Animation => "Animation";
        public override string Editor_Animation_RemoveCurrentFrame => "Aktuellen Frame entfernen";
        public override string Editor_Animation_AddFrameCopy => "Frame als Kopie hinzufügen";
        public override string Editor_Animation_AddEmptyFrame => "Leeren Frame hinzufügen";
        public override string Editor_Animation_MoveDescription => "Frame-Position ändern";
        public override string Editor_Animation_AllFrames => "Alle Frames";
        public override string Editor_Animation_AllFrames_ActionDescription => "Aktion auf alle Frames anwenden";

        public override string Editor_SettingsMenu => "Einstellungen";
        public override string Hud_Exit => "Beenden";
        public override string Editor_Canvas_Clear => "Leeren";

        public override string Editor_Stamp => "Stempel";
        public override string Editor_StampOtherFrames => "In andere Frames stempeln";
        public override string Editor_StampOtherFrames_Description => "Fügt die Voxel in diesen Frames ein";
        public override string Editor_PasteToFrame => "Fügt die Voxel in diesen Frame ein";
        public override string Editor_ClearAllFrames => "In allen Frames leeren";
        public override string Editor_ClearOtherFrames => "Andere Frames leeren";

        public override string Editor_Settings_MoveSpeed => "Bewegungsgeschwindigkeit";
        public override string Editor_Settings_BackgroundColor => "Hintergrundfarbe";
        public override string Editor_Settings_HideHUD => "HUD ausblenden";

        public override string Editor_Color => "Farbe";
        public override string Editor_ColorsInUseLabel => "Verwendete Farben";
        public override string Editor_Color_BrighterPlus => "Heller +";
        public override string Editor_Color_Brighter => "Heller";
        public override string Editor_Color_Darker => "Dunkler";
        public override string Editor_Color_DarkerPlus => "Dunkler +";
        public override string Editor_Color_RedTint => "Roter Farbton";
        public override string Editor_Color_Tint => "Farbton";
        public override string Editor_Color_GreenTint => "Grüner Farbton";
        public override string Editor_Color_BlueTint => "Blauer Farbton";
        public override string Editor_Color_YellowTint => "Gelber Farbton";
        public override string Editor_Color_PurpleTint => "Lila Farbton";
        public override string Editor_NoColor => "Leer";

        public override string Editor_Material => "Material";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "Umfärben";
        public override string Editor_Color_RecolorTo => "Umfärben in";

        public override string Editor_Material_Set => "Material festlegen";

        public override string Editor_Preview => "Vorschau";
        public override string Editor_CombineWithCurrent => "Mit aktuellem Modell kombinieren";

        public override string Editor_PickedColor => "Ausgewählt";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "Einwanderungszelt";
        public override string BuildingType_ImmigrationTent_Description => "Speichert {0} Einwanderer";
        public override string BuildingType_ReseachCenter => "Forschungszentrum";
        public override string BuildingType_Bookpress => "Buchpresse";
        public override string BuildingType_Bookpress_Description => "In einem Forschungsgebiet werden alle gesammelten Punkte mit allen {0} in deinen anderen Städten geteilt.";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "Beispiel: Wenn ein Arbeiter {0} produziert, verbessert er seine {1}-Fähigkeit. Beim Stufenaufstieg werden Punkte der {2}-Technologie gutgeschrieben, da sie dasselbe Gebiet teilen.";

        public override string BuildingType_Research_BaseDescription => "Erhöht die Technologieforschung.";

        public override string BuildingType_ResearchCenter_Description => "Fügt {0} zusätzliche Forschungspunkte hinzu, wenn ein Arbeiter im selben Bereich aufsteigt.";


        //DEMO PATCH 5
       

        public override string Editor_CropSelection => "Auf Auswahl zuschneiden";

        public override string Immigrants_DisbandedSoldiers => "Entlassene Soldaten werden einwandern";
        public override string Immigrants_RefillWorkers => "Füllt die Arbeitskraft schnell auf";
        public override string Immigrants_UnhousedAreLost => "Einwanderer ohne Unterkunft verschwinden nach einiger Zeit";
        public override string Editor_VoxelCount => "{0} Voxel";

        public override string Editor_Layers_Titel => "Ebenen";
        public override string Editor_Layers_All => "Alle Ebenen";
        public override string Editor_LayerNumber => "Ebene {0}";

        public override string Editor_Layer_AddEmpty => "Leere Ebene hinzufügen";
        public override string Editor_Layer_AddCopy => "Ebene duplizieren";
        public override string Editor_Layer_Remove => "Ebene entfernen";
        public override string Editor_Layer_MergeDown => "Nach unten zusammenführen";
        public override string Editor_IsAnimated => "Animiert";
        public override string Editor_ToggleVisible => "Sichtbarkeit umschalten";
        public override string Editor_ToggleAnimatedLayer => "Animierte Ebene umschalten";
        public override string Editor_Projects => "Projektdateien";
        public override string ProfileEditor_ReplaceMaterial => "Profilfarbe: {0}";

        public override string ProfileEditor_ProfileColors_Label => "Profilfarben";
        public override string ProfileEditor_TunicColor => "Tunika-Farbe";
        public override string ProfileEditor_PantsColor => "Hosenfarbe";
        public override string ProfileEditor_LeaderColor => "Führerfarbe";

        public override string MapStartAs_Water => "Wasser";
        public override string MapStartAs_Land => "Land";
        public override string MapStartAs_Circle => "Kreis";

        public override string Hud_NeedToBeAssigned => "Zuweisung erforderlich";
        public override string Hud_CommitAssignment => "Zuweisen";
        public override string Technology_NoAvailableResearch => "Keine verfügbare Forschung";

        public override string Research_Tab => "Forschung";

        //5.2
        public override string BuildCategory_General => "Allgemein";
        public override string BuildCategory_Military => "Militär";
        public override string BuildCategory_Decoration => "Dekoration";
        public override string BuildCategory_Upgrade => "Aufrüstung";
        public override string Work_NoMines => "Keine Minen";

        //NEXT FEST DEMO
        public override string HUD_DisplayName => "Anzeigename";
        public override string HUD_Filter => "Filter";
        public override string HUD_Scale => "Skalierung";
        public override string HUD_Tags => "Tags";
        public override string HUD_ClickToCancel => "Klicken zum Abbrechen";

        public override string ObjectTag_Description => "Ein Symbol auf der Karte hinzufügen";
        public override string HudPins => "HUD-Markierungen";
        public override string HudPins_Description => "Informationen am Bildschirm anheften";

        public override string Lobby_PlayerProfileNumbered => "Profil {0}";
        public override string Lobby_CharacterCreationNumbered => "Charakter {0}";
        public override string Lobby_PlayerProfileEdit => "Spielerprofil bearbeiten";

        public override string Editor_ConvertAnimationToLayers => "Animation in Ebenen umwandeln";
        public override string Editor_StampAllFrames => "Auf alle Frames stempeln";

        public override string Editor_DisplayOptions => "Anzeigeoptionen";
        public override string Editor_CharacterCreator => "Charakter-Editor";
        public override string Editor_CharacterCreator_Description => "Editor für das Aussehen militärischer Modelle";
        public override string Editor_HatGenre => "Hut-Anzeigemodus";
        public override string Editor_HatGenre_FollowWeapon => "Waffe folgen";
        public override string Editor_HatGenre_Uniform => "Uniform";
        public override string Editor_CopyPasteSelectedColor => "Farbe vom Ausgewählten kopieren";

        public override string Character_Accessories => "Zubehör";
        public override string Character_Hat => "Hut";
        public override string Character_Head => "Kopf";
        public override string Character_Body => "Körper";
        public override string Character_Arms => "Arme";
        public override string Character_Back => "Rücken";
        public override string Character_Face => "Gesicht";

        public override string BuildingType_Tavern => "Gemeinschaftshalle";

        public override string Settings_CraftMultiplier => "Produktionszeit-Multiplikator";
        public override string Settings_ChildMultiplier_Description => "Erhöht die Geschwindigkeit, mit der neue Arbeiter hinzukommen";

        public override string Settings_CasualControls => "Steuerung für Gelegenheitsspieler";
        public override string Settings_CasualControls_Description => "Vereinfacht das Gameplay, indem Entscheidungen auf das Wesentliche reduziert werden. Nur Geld wird als Ressource verwendet.";

        public override string Settings_AdvancedControls => "Erweiterte Steuerung";
        public override string Settings_AdvancedControls_Description => "Das vollständige Ressourcenmanagement-Erlebnis.";

        public override string WarsResourceGroup_Metal => "Metall";
        public override string Work_Craft => "Produzieren";
        public override string Work_OnlyCraftOnFullStock => "Nur produzieren bei vollem Lagerbestand";

        public override string ExperienceType_Smelting => "Schmelzen";
        public override string Category_Optimize => "Optimieren";
        public override string BuildCategory_Road => "Straße";
        public override string XP_UnlockBuildPrio => "Baupriorität freischalten: {0}";
        public override string Technology_ModernFarming => "Moderne Landwirtschaft";

        public override string ExportImportDescription => "Zum Teilen von Spielständen mit anderen Spielern befinden sich alle Dateien in diesem Ordner: {0}";

        public override string CityCultureDescription => "Kultur verleiht der Stadt einen besonderen Bonus";

        public override string UnitType_CloseRangeRifle => "Arkebusier";
        public override string UnitType_LongRangeRifle => "Musketier";
        public override string UnitType_Skirmisher => "Plänkler";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "Lunari-Bogenschütze";
        public override string UnitType_MithrilSwordsman => "Lunari-Ritter";

        public override string Defence_AutoAssign_Towers => "Türme zuweisen";

        public override string EventMessage_DesertersText_Food => "Hungernde Soldaten desertieren aus deiner Armee";

        public override string Tutorial_CasualRecruitSoldiers => "Kaufe eine Soldatengruppe";

        //Shadow update
        public override string Technology_CannotReassign => "Tech kann nicht neu zugewiesen werden, bis die Forschung abgeschlossen ist";
        public override string Diplomacy_DeclareWarAgainst => "Du erklärst den Krieg gegen";
        public override string Diplomacy_AllyCount => "Anzahl der Verbündeten";
        public override string Diplomacy_CostPerAlly => "Kosten steigen um {0} pro Verbündetem";

        public override string Event_ChanceOfFailure => "{0}% Chance auf Fehlschlag";
        public override string EventMessage_Event_Title => "Ereignis";
        public override string EventMessage_TheCohalition => "Die Koalition";

        public override string EventMessage_DarkHorde => "Dunkle Horde";
        public override string EventMessage_DarkHordeKiller_Title => "Killer der Dunklen Horde";
        public override string EventMessage_DarkHordeKiller_Message => "Champion-Ritter treten in deinen Dienst";

        public override string Settings_Mode_Spectator_Description => "Nur zuschauen – oder mit God Powers eingreifen.";
        public override string GodPower => "God Power";

        public override string Building_TreeSprout_Description => "Einen Baum pflanzen";
        public override string Building_TreeSprout_Soft => "Weichholz-Setzling";
        public override string Building_TreeSprout_Hard => "Hartholz-Setzling";

        public override string GeneralSetting_SetAll => "Auf alle anwenden";

        public override string Hud_All => "Alle";

        public override string Hud_Previous => "Zurück";

        public override string Hud_EffectWillStack => "Der Effekt stackt";

        public override string Info_WhenFoodRunsOut => "Wenn die Nahrung ausgeht, kaufen Städte und Armeen sie automatisch auf dem Schwarzmarkt.";

    }
}
