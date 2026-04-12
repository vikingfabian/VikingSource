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
    partial class French : AbsLanguage
    {
        //Mise à jour des montures
        public override string Leaderboards_title => "Leaderboards";
        public override string Leaderboards_domination => "Meilleur temps domination mondiale, {0}% et plus";
        public override string Leaderboards_victory => "Victoire Histoire, top % difficulté";
        public override string Leaderboards_CitySize => "Taille max de ville, en travailleurs";
        public override string Leaderboards_Survival => "Durée de survie à {0}% de difficulté";

        public override string Message_CannotPayUpkeep => "Impossible de payer l'entretien !";
        public override string Animals_ProductionStop => "La production d'animaux va s'arrêter";

        public override string Tutorial_ToCapture => "Pour capturer";
        public override string Tutorial_ClickButton => "Cliquez sur le bouton";
        public override string Tutorial_MoveXToY => "Déplacez {0} vers {1}";

        public override string Workers_Description1_work => "Va construire, récolter des ressources et crafter des objets.";
        public override string Workers_Description2_income => "Payent des taxes pour vos revenus.";
        public override string Workers_Description3_soldiers => "Peuvent être recrutés comme soldats pour vos armées.";

        public override string Hud_Time_ValuePerMinute => "Valeur par minute";
        public override string Hud_Time_ValuePerSecond => "Valeur par seconde";
        public override string Hud_Lock => "Verrouiller";
        public override string Hud_Maximum => "Max";

        public override string Tutorial_SeeThisInThat => "Voir {0} dans {1}";
        public override string Conscript_SkillBonus => "Bonus de skill";
        public override string SoldierStats_UnitCount => "Nombre d'unités";
        /// <summary>
        /// Les zones sont champ, forêt, mer et siège
        /// </summary>
        public override string Conscript_DamagePerSecondInAreaX => "Dégâts par seconde - {0}";
        public override string Conscript_BaseHealth => "HP de base";

        /// <summary>
        /// Valeur résumée pour la capacité à traverser la carte
        /// </summary>
        public override string Conscript_Mobility => "Mobilité";

        public override string Conscript_RiderMobility => "Mobilité de cavalier";
        public override string Conscript_LightWagonMobility => "Mobilité (Chariot léger)";
        public override string Conscript_HeavyWagonMobility => "Mobilité (Chariot lourd)";

        /// <summary>
        /// Généralisé pour tout objet, comme les skills, ressources et bâtiments
        /// </summary>
        public override string Culture_AffectedItems => "Items affectés";
        //## Mise à jour des montures ##
        public override string Progress_ClosingCores => "Fermeture des cœurs CPU {0}";
        public override string Editor_ExportFrame => "Exporter la frame actuelle";
        public override string Editor_FistFrame => "Première frame";
        public override string Editor_LastFrame => "Dernière frame";

        public override string Economy_AnimalPenUpkeep => "Entretien de l'enclos : {0}";
        public override string Work_SlaughterX => "Abattre {0}";

        public override string BuildCategory_Farming => "Agriculture";
        public override string Resource_TypeName_ManType => "type d'homme";
        public override string Resource_TypeName_NobelMen => "nobles";
        public override string Resource_TypeName_ConservedFood => "nourriture conservée";

        public override string UnitType_UnitOnMount => "monte {0}";
        public override string UnitType_UnitOnWagon => "chariot {0}";
        public override string UnitType_NobelUnit => "noble {0}";

        /// <summary>
        /// 0: type de soldat, 1: animal
        /// </summary>
        public override string UnitType_LeashAnimalHandler => "{0} dresseur de {1}";

        public override string Info_ArmyFood4 => "La nourriture conservée permet de plus grandes réserves";
        public override string Info_ArmyFood5 => "La nourriture fraîche sera consommée en premier";

        public override string Resource_ConservedFood_Reserves => "Réserves de nourriture conservée";
        public override string Resource_TypeName_Clay => "argile";
        public override string Resource_TypeName_Brick => "brique";
        public override string Resource_TypeName_Container => "conteneur";
        public override string Resource_TypeName_Meat => "viande";
        public override string Resource_TypeName_Salt => "sel";
        public override string Resource_TypeName_Vehicle => "véhicule";
        public override string Resource_TypeName_WagonClosed => "chariot fermé";
        public override string Resource_TypeName_WagonIron => "carrosse en fer";
        public override string Resource_TypeName_WagonSteel => "carrosse en acier";
        public override string Resource_TypeName_Shield => "bouclier";
        public override string Resource_TypeName_BucklerShield => "bocle";
        public override string Resource_TypeName_RoundShield => "bouclier rond";
        public override string Resource_TypeName_HeaterShield => "écu";
        public override string Resource_TypeName_TowerShield => "pavois";

        public override string Resource_TypeName_Mount => "monture";

        public override string Resource_TypeName_MountArmorTitle => "armure de monture";

        /// <summary>
        /// 0: type d'armure
        /// </summary>
        public override string Resource_TypeName_MountArmorX => "monture {0}";
        public override string Resource_TypeName_Animal => "animal";

        //public override string Resource_TypeName_WildAnimal => "animal sauvage";

        /// <summary>
        /// Zone avec des animaux sauvages
        /// </summary>
        public override string Terrain_XAnimalHabitat => "Habitat de {0}";

        public override string Resource_TypeName_Oxen => "bœuf";
        public override string Resource_TypeName_KineOxen => "bovin d'élevage";

        /// <summary>
        /// Poule de bas tier (pour l'élevage)
        /// </summary>
        public override string Resource_TypeName_Fowl => "volaille";

        /// <summary>
        /// Cochon de bas tier (pour l'élevage)
        /// </summary>
        public override string Resource_TypeName_Boar => "verrat";
        public override string Resource_TypeName_Pig => "cochon";
        public override string Resource_TypeName_Hen => "poule";
        public override string Resource_TypeName_Dog => "chien";
        public override string Resource_TypeName_Hound => "chien de chasse";

        public override string Resource_TypeName_Pony => "poney";
        public override string Resource_TypeName_Horse => "cheval";
        public override string Resource_TypeName_WarHorse => "cheval de guerre";
        public override string Resource_TypeName_DraftHorse => "cheval de trait";

        public override string Resource_TypeName_WildPig => "cochon sauvage";
        public override string Resource_TypeName_WildHog => "sanglier";
        public override string Resource_TypeName_WarHog => "sanglier de guerre";
        public override string Resource_TypeName_StagHog => "sanglier-cerf";

        public override string Resource_TypeName_Wolf => "loup";
        public override string Resource_TypeName_Warg => "warg";
        public override string Resource_TypeName_AlphaWarg => "warg alpha";

        public override string Resource_TypeName_WildCat => "chat sauvage";
        public override string Resource_TypeName_Lion => "lion";
        public override string Resource_TypeName_WarLion => "lion de guerre";

        public override string Resource_TypeName_Elephant => "éléphant";
        public override string Resource_TypeName_WarElephant => "éléphant de guerre";
        public override string Resource_TypeName_Oliphant => "oliphant";

        public override string BuildHud_Select => "Sélectionner un bâtiment";
        public override string BuildHud_AreaRadius => "Rayon de la zone";

        public override string NobleHouse_HousingCount => "Logera {0} nobles";

        public override string BuildingType_GreatHall => "Grande Salle";
        public override string BuildingType_GreatHall_Description => "Débloque le recrutement avancé";

        public override string BuildingType_ClayPit => "Fosse d'argile";
        public override string BuildingType_Butcher => "Boucher";
        public override string BuildingType_Butcher_Description => "Transforme les animaux en viande et en peaux";
        public override string BuildingType_Pottery => "Poterie";
        public override string BuildingType_CraftX_Description => "Station de crafting de {0}";

        public override string BuildingType_GatherX_Description => "Récolte {0}";

        public override string BuildingType_Smoker => "Fumoir";
        public override string BuildingType_Dryer => "Séchoir";
        public override string BuildingType_Shieldmaker => "Fabricant de boucliers";
        public override string BuildingType_DryingPan => "Poêle de séchage";

        public override string BuildingType_TrapperHut => "Cabane de trappeur";
        public override string BuildingType_TrapperHut_Description => "Permet de capturer des animaux sauvages";

        // --- Stockage ---
        public override string BuildingType_MaterialStorage => "Stockage de matériaux";
        public override string BuildingType_FoodStorage => "Stockage de nourriture";
        public override string BuildingType_WeaponStorage => "Stockage d'armes";
        public override string BuildingType_ArmorStorage => "Stockage d'armures";
        public override string BuildingType_AnimalStorage => "Stockage d'animaux";

        public override string BuildingType_Storage_Description => "Augmente le stock max de {0}";

        public override string BuildingType_Cesspit => "Fosse à déchets";
        public override string BuildingType_Cesspit_Description => "Détruit des ressources";

        public override string BuildingType_Cesspit_Info1_StockPile => "Détruit les items qui dépassent la limite de stockage";
        public override string Info_XAmountIsConvertedToY => "{0} est converti en {1}";
        public override string Info_ProductionRestriction => "Production d'items limitée à";

        public override string BuildingType_FowlPen => "Poulailler";
        public override string BuildingType_BoarPen => "Enclos à verrats";

        // --- Enclos à bœufs ---
        public override string BuildingType_OxenPen => "Enclos à bœufs";
        public override string BuildingType_KineOxenPen => "Enclos à bovins";

        // --- Cages à chiens ---
        public override string BuildingType_DogCage => "Chenil";
        public override string BuildingType_HoundCage => "Chenil de chasse";

        // --- Enclos à chevaux ---
        public override string BuildingType_PonyPen => "Enclos à poneys";
        public override string BuildingType_HorsePen => "Enclos à chevaux";
        public override string BuildingType_WarHorsePen => "Enclos à chevaux de guerre";
        public override string BuildingType_DraftHorsePen => "Enclos à chevaux de trait";

        // --- Enclos à cochons/sangliers ---
        public override string BuildingType_WildPigPen => "Enclos à cochons sauvages";
        public override string BuildingType_WildHogPen => "Enclos à sangliers";
        public override string BuildingType_WarHogPen => "Enclos à sangliers de guerre";
        public override string BuildingType_StagHogPen => "Enclos à sangliers-cerfs";

        // --- Cages à loups ---
        public override string BuildingType_WolfCage => "Cage à loups";
        public override string BuildingType_WargCage => "Cage à wargs";
        public override string BuildingType_AlphaWargCage => "Cage à wargs alpha";

        // --- Cages à chats ---
        public override string BuildingType_WildCatCage => "Cage à chats sauvages";
        public override string BuildingType_LionCage => "Cage à lions";
        public override string BuildingType_WarLionCage => "Cage à lions de guerre";

        // --- Cages à éléphants ---
        public override string BuildingType_ElephantCage => "Enclos à éléphants";
        public override string BuildingType_WarElephantCage => "Enclos à éléphants de guerre";
        public override string BuildingType_OliphantCage => "Enclos à oliphants";

        public override string BuildingDescription_Animals => "Produit des animaux pour le recrutement de soldats";
        public override string Pen_Breeding => "Élevage d'animaux";
        public override string Pen_BreedUpChance => "{0}% de chance de monter de tier";
        public override string Pen_BreedDownChance => "{0}% de chance de baisser de tier";

        public override string CityCulture_AnimalBreeder2_Description => "Plus grande chance de succès d'élevage";

        public override string CityCulture_EnhancedProduction => "Production de {0} améliorée";
        public override string CityCulture_Production => "Production de {0}";

        public override string CityCulture_Butchers => "Bouchers";

        public override string CityCulture_Potters => "Potiers";

        public override string CityCulture_Wainwright => "Charrons";

        public override string CityCulture_Wheelwright => "Fabricants de roues";
        public override string CityCulture_Wheelwright_Description => "Bonus de speed pour les chariots recrutés";

        public override string CityCulture_ShieldMaker => "Fabricants de boucliers";

        //public override string CityCulture_Nomads_Description => "Faible coût de colonisation";

        public override string CityCulture_Coopers => "Tonneliers";

        public override string CityCulture_Salters => "Sauniers";

        public override string CityBiome_Title => "Biome";
        public override string CityBiome_Description => "Les biomes affectent l'accès à certaines ressources et bâtiments";

        public override string CityBiome_Fields => "Champs";
        public override string CityBiome_Frozen => "Gelé";
        public override string CityBiome_Forest => "Forêt";
        public override string CityBiome_Mountain => "Montagne";
        public override string CityBiome_Desolate => "Désolé";
        public override string CityBiome_Desert => "Désert";

        public override string Bonus_IncreaseSkin => "Production de peaux augmentée";
        public override string Bonus_FoodStorage => "Plus grand stockage de nourriture";

        public override string StockPile_LimitTitle => "Limite de stockage";


        public override string Help_Work_Automatic => "Le travail est automatique";
        public override string Tutorial_SecondCity => "Obtenir une deuxième ville";
        //## Spring update

        public override string InputAction_SkipAutomated => "Passer l'automatisé";

        public override string Resource_WaterReason => "L'eau limite le nombre d'unités et la taille de votre production";
        public override string BuildingType_Orchard => "Verger";
        public override string BuildingType_ManorLord => "Seigneur du Manoir";
        public override string BuildingType_ManorLord_Description => "Débloque la transformation de nourriture";
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public override string Diplomacy_EndRelations => "Rompre les relations";

        /// <summary>
        /// Where a resource is produced or found
        /// </summary>
        public override string ItemSource => "Source de l'objet";

        public override string ItemSource_Terrain => "Terrain";
        public override string ItemSource_Farm => "Ferme";
        public override string ItemSource_CraftStation => "Atelier";
        public override string ItemSource_Gathering => "Récolte";

        public override string CityCulture_Nomad => "Nomade";

        /// <summary>
        /// A generalized display of buffs and boons, example "+100%" or "Doubled"
        /// </summary>
        public override string Hud_ChangeFactor => "Facteur de modification : {0}";

        public override string Hud_Purchase_LowXCost => "Faible coût en {0}";

        public override string WorkQueue_Title => "File de travail";
        public override string WorkQueue_Length => "Objectifs restants";
        public override string WorkQueue_ActiveWorkers => "Équipes actives";
        public override string WorkQueue_IdleWorkers => "Équipes inactives";

        public override string WorkTeam_Size => "Les villageois travaillent par équipes de {0}";

        public override string ObjectUi_ViewOnMap => "Voir sur la carte";
        public override string ObjectUi_StuckBuildOrders => "Ordres de construction bloqués";
        public override string Hud_AllArmies => "Toutes les armées";

        public override string Hud_CurrentPage => "Page actuelle";
        public override string Hud_AllPages => "Toutes les pages";
        public override string Hud_ToAllCities => "Pour toutes les villes";
        public override string Hud_ToFaction => "Pour la faction";
        public override string Hud_FromFaction => "De la faction";
        public override string Hud_FactionWide => "Utiliser le paramètre de faction";
        /// <summary>
        /// This start a new city
        /// </summary>
        public override string Action_PlaceSettlement => "Placer une colonie";

        public override string Editor_Animation_RemoveAllFramesButThis => "Supprimer les autres images";
        //Winter patch 3
        public override string Hud_Purchase_AllBuildings => "Tout construire (file d'attente)";
        public override string Hud_Purchase_AllTech => "Tout rechercher (file d'attente)";
        public override string BuildingType_CasualBarracks_Description => "Le temps de recrutement est réparti entre les casernes";

        //Winter update patch + spring

        /// <summary>
        /// How much of a resource that will be used, e.g. "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public override string Language_ItemCount => "{1} {0}";

        //public override string DisplayMode => "Mode d'affichage";
        //public override string DisplayMode_Windowed => "Fenêtré";
        //public override string DisplayMode_BorderlessFullscreen => "Plein écran sans bordure";

        //// "Curseur logiciel" implies the game renders it, as opposed to the OS (Hardware cursor)
        //public override string GameSettings_RenderedMouseCursor => "Curseur logiciel";
        //public override string GameSettings_MuteControllerDisconnect => "Masquer alertes manette";

        public override string Delivery_MaxDistance => "Distance de livraison max : {0}";
        public override string Tutorial_WillTakeAWhile => "Ça va prendre un moment, revenez plus tard.";

        /// <summary>
        /// 0: name of building
        /// </summary>
        public override string Tutorial_WaitFor => "Attendre la fin de : {0}";
        public override string GameOverResults => "Historique de la partie";

        public override string UnitType_UnclaimedLand => "Terres non revendiquées";
        // "Colon" is the standard term for Settler in French Civ games
        public override string UnitType_Settler => "Colon";
        public override string UnitType_Settler_Description => "Fonder une nouvelle ville";
        public override string Resource_ConsumedProduced => "Consommé/Produit";
        public override string InputActionName_PlaceTarget => "Placer la cible";

        public override string FactionStartSize => "Taille de départ";
        public override string FactionStartSize_Full => "Complète";
        public override string FactionStartSize_OneCity => "Une ville";
        public override string FactionStartSize_Settler => "Un colon";


        //Winter update
        public override string Resource_StockpileLimit => "Limite de stock";
        public override string GameMode_QuickMatch => "Quick Match";
        public override string GameMode_QuickMatch_Description => "Un format de partie plus court. Lancez-vous dans une guerre totale contre des nations rivales.";
        public override string Lobby_PlayerCount => "Nombre de joueurs";
        public override string Lobby_TwoTeams => "Deux équipes";
        public override string Hud_Produce => "Produire :";
        public override string Tutorial_WaitForWorkerLevel => "Attendez qu’un ouvrier atteigne :";

        public override string Tutorial_PracticeOrSchool => "Entraînez-vous sur {0}, ou utilisez une {1}";
        public override string Tutorial_AddTag => "Ajouter un tag :";
        public override string Tutorial_AddPin => "Ajouter un pin :";
        public override string Tutorial_SelectMostTrees => "Trouvez votre ville avec le plus d’arbres";
        public override string Tutorial_SelectACityWithX => "Sélectionnez une ville avec {0}";

        public override string Tutorial_Select_NotCapital => ". Pas votre capitale.";

        public override string Tutorial_SetXPriorityToY => "Réglez la priorité de {0} sur {1}";
        public override string Tutorial_AdvisorMission => "Mission de l’Advisor";

        public override string Tutorial_AdvisorDescription =>
            "La partie complète a commencé. L’Advisor prolongera le tutoriel avec des missions utiles.";

        public override string Tutorial_EndAdvisor => "Terminer l’Advisor";

        public override string Tutorial_AdvisorCompleteTitle => "Advisor terminé !";
        public override string Tutorial_AdvisorCompleteMessage => "Que votre prochain jour soit béni !";

        public override string Hud_Search => "Recherche";

        public override string DifficultyDescription_ExtremeAggression => "Agression extrême";

        public override string MapFilter => "Filtre de carte";

        public override string Settings_TechMultiplier => "Vitesse de recherche tech";

        public override string EndScreen_MatchComplete => "Résultat de la partie";

        public override string FactionName_DragonGem => "Dragon Gem";
        public override string FactionName_Tomten => "Tomten";
        public override string FactionName_Hælfolc => "Hælfolc";
        public override string FactionName_AerimAngren => "Aerim Angren";

        public override string HUD_NotAvailbleInX => "Non disponible dans {0}";

        public override string InputActionName_MiniMap => "Mini-map";

        //-
        public override string Error_SoundInitFailure => "Échec de l'initialisation du son";

        public override string GameMenu_ControllerDisconnected => "Manette déconnectée";

        public override string Tutorial_HighPriority => "Vos hommes accompliront d'abord les tâches prioritaires.";

        public override string BuildingType_Wall_Description => "Les murs protègent vos troupes des attaques et offrent un petit boost d'attaque.";

        public override string BuildingType_Wall_Siege => "Les armes de siège réduisent la défense des murs.";

        public override string Conscript_BlockChance => "{0}% de chance de bloquer une attaque.";

        public override string Battle_DeclarWarReminder => "Vous devez déclarer la guerre avant d'attaquer.";

        //--


        /// <summary>
        /// Name of this language
        /// </summary>
        public override string MyLanguage => "Français";

        /// <summary>
        /// How to display a number of items. 0: item, 1:Number
        /// </summary>
        public override string Language_ItemCount_Colon => "{0}: {1}";

        /// <summary>
        /// Select language option
        /// </summary>
        public override string Lobby_Language => "Langue";

        /// <summary>
        /// Start playing the game
        /// </summary>
        public override string Lobby_Start => "DÉPART";

        /// <summary>
        /// Button to select local mutiplayer count, 0:current player count
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Multijoueur Local";

        /// <summary>
        /// Title for menu where you select split screen player count
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Choisir nombre de joueurs";

        /// <summary>
        /// Description for local multiplayer
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "Le multijoueur requiert des manettes Xbox";

        /// <summary>
        /// Move to next split screen position
        /// </summary>
        public override string Lobby_NextScreen => "Prochain emplacement d'écran";

        /// <summary>
        /// Players can select visual appearance and store them in a profile
        /// </summary>
        public override string Lobby_FlagSelectTitle => "Choisir Drapeau";

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Drapeau {0}";

        /// <summary>
        /// Game name and version number
        /// </summary>
        //public override string Lobby_GameVersion => "DSS war party - ver {0}";

        public override string FlagEditor_Description => "Dessinez votre drapeau ainsi que les couleurs des soldats de votre armée.";

        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string FlagEditor_Bucket => "Sceau";

        /// <summary>
        /// Opens flag profile editor
        /// </summary>
        public override string Lobby_FlagEdit => "Modifier drapeau";


        public override string Lobby_WarningTitle => "Avertissement";
        public override string Lobby_IgnoreWarning => "Ignorer l'avertissement";

        /// <summary>
        /// Warning when one player has no input selected.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "Un des joueurs n'a pas sélectionné d'entrée.";

        /// <summary>
        /// Menu with content that are outside what most players will use.
        /// </summary>
        public override string Lobby_Extra => "Extra";

        /// <summary>
        /// The extra content is not translated or have full controller support.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "Avertissement! Ce contenu n'est pas encore pleinement couvert par la localisation, ou par le contrôle manette.";


        public override string Lobby_MapSizeTitle => "Taille de la carte";

        /// <summary>
        /// Map size 1 name
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "Minuscule";

        /// <summary>
        /// Map size 2 name
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "Petit";

        /// <summary>
        /// Map size 3 name
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "Moyen";

        /// <summary>
        /// Map size 4 name
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "Grand";

        /// <summary>
        /// Map size 5 name
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "Gigantesque";

        /// <summary>
        /// Map size 6 name
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "Épique";

        /// <summary>
        /// Map size description X by Y kilometers. 0: Width, 1: Height
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";
        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => "Quitter";

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => "Joueur {0}";

        /// <summary>
        /// In player profile editor. Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Options";

        /// <summary>
        /// In player profile editor. Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Couleurs du drapeau";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "Couleur principale";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Couleur du détail 1";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Couleur du détail 2";

        /// <summary>
        /// In player profile editor. Title for selecting you soldiers colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "Soldats";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "Couleur de peau";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "Couleur des cheveux";

        /// <summary>
        /// In player profile editor. Open color palette and select color
        /// </summary>
        public override string ProfileEditor_PickColor => "Choisir couleur";

        /// <summary>
        /// In player profile editor. Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "Déplacer l'image";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Gauche";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Droite";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Haut";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Bas";

        /// <summary>
        /// In player profile editor. Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Annuler et quitter";

        /// <summary>
        /// In player profile editor. Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Annuler tous les changements";

        /// <summary>
        /// In player profile editor. Save changes and close editor
        /// </summary>
        public override string Hud_SaveAndExit => "Sauvegarder et quitter";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Hue => "Teinte";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Lightness => "Luminosité";

        /// <summary>
        /// In player profile editor. Move between flag and soldier color options.
        /// </summary>
        public override string ProfileEditor_NextColorType => "Prochain type de couleur";

        /// <summary>
        /// Current running speed of the game, compared to real time
        /// </summary>
        public override string Hud_GameSpeedLabel => "Vitesse de jeu: {0}x";

        public override string Input_GameSpeed => "Vitesse de jeu";

        /// <summary>
        /// Ingame display. Unit gold production
        /// </summary>
        public override string Hud_TotalIncome => "Revenu total/seconde: {0}";

        /// <summary>
        /// Unit gold cost.
        /// </summary>
        public override string Hud_Upkeep => "Maintenance";
        public override string Hud_ArmyUpkeep => "Maintenance de l'armée: {0}";

        /// <summary>
        /// Ingame display. Soldiers protecting a building.
        /// </summary>
        public override string Hud_GuardCount => "Gardes";

        public override string Hud_IncreaseMaxGuardCount => "{0} gardes max";

        public override string Hud_GuardCount_MustExpandCityMessage => "Vous devez étendre votre ville.";

        public override string Hud_SoldierCount => "Nombre de soldats";

        public override string Hud_SoldierGroupsCount => "Nombre dans le groupe";

        /// <summary>
        /// Ingame display. Unit caculated battle strength.
        /// </summary>
        public override string Hud_StrengthRating => "Note de force";

        /// <summary>
        /// Ingame display. Caculated battle strength for the whole nation.
        /// </summary>
        public override string Hud_TotalStrengthRating => "Force militaire: {0}";

        /// <summary>
        /// Ingame display. Extra men coming from outside the city state.
        /// </summary>
        public override string Hud_Immigrants => "Immigrants";


        public override string Hud_CityCount => "Nombre de villes: {0}";
        public override string Hud_ArmyCount => "Nombre d'armées: {0}";


        /// <summary>
        /// Mini button to repeat a purchase a number of times. E.G. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Requis";
        public override string Hud_PurchaseTitle_Cost => "Coût";
        public override string Hud_PurchaseTitle_Gain => "Gain";

        /// <summary>
        /// How much of a resource that will be used, "5 gold. (Available: 10)". There will be a "cost" title above the text. 0: Resource, 1: cost, 2: available
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Disponible: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "Le coût augmentera de {0}";

        public override string Hud_Purchase_MaxCapacity => "A atteint sa capacité maximale";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Force: Vous {0} - Eux {1}";

        /// <summary>
        /// Display a short string of date as Year, Month, Day
        /// </summary>
        public override string Hud_Date => "Y{0} M{1} D{2}";

        /// <summary>
        /// Display a short string of timespan as Hour, Minutes, Seconds
        /// </summary>
        public override string Hud_TimeSpan => "H{0} M{1} S{2}";

        /// <summary>
        /// Battle between two armies, or army and city
        /// </summary>
        public override string Hud_Battle => "Bataille";



        /// <summary>
        /// Describes button input. Pause.
        /// </summary>
        public override string Input_Pause => "Pause";

        /// <summary>
        /// Describes button input. Resume from paused.
        /// </summary>
        public override string Input_ResumePaused => "Reprendre";

        /// <summary>
        /// Generic money resource
        /// </summary>
        public override string ResourceType_Gold => "Or";

        /// <summary>
        /// Working men resource
        /// </summary>
        public override string ResourceType_Workers => "Ouvriers";


        public override string ResourceType_Workers_Description => "Les ouvriers génèrent du revenu, et peuvent être enrôlés comme soldats pour votre armée.";

        /// <summary>
        /// The resource used in diplomacy
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Points de diplomatie";

        /// <summary>
        /// 0: How many points you got, 1: Soft max value (will increase much slower after this), 2: Hard limit
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Points de diplomatie: {0} / {1} ({2})";

        /// <summary>
        /// City building type. Building for knights and diplomats.
        /// </summary>
        public override string Building_NobleHouse => "Maison noble";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "1 point diplomatique pour {0} secondes";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} à la limite de points diplomatiques";
        public override string Building_NobleHouse_UnlocksKnight => "Débloque l'unité de Chevalier";

        public override string Building_BuildAction => "Construire";
        public override string Building_IsBuilt => "Construit";

        /// <summary>
        /// City building type. Evil mass production.
        /// </summary>
        public override string Building_DarkFactory => "Usine sombre";

        /// <summary>
        /// In game settings menu. Sums all difficulty options in percentage.
        /// </summary>
        public override string Settings_TotalDifficulty => "Difficulté totale {0}%";

        /// <summary>
        /// In game settings menu. Base difficulty option.
        /// </summary>
        public override string Settings_DifficultyLevel => "Niveau de difficulté {0}%";


        /// <summary>
        ///  In game settings menu.Option for creating new maps instead of loading one
        /// </summary>
        public override string Settings_GenerateMaps => "Génère de nouvelles cartes";

        /// <summary>
        ///  In game settings menu.Creating new maps has a longer loading time
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Il est plus long de générer des cartes que d'en charger";

        /// <summary>
        ///  In game settings menu.Difficulty option. Block the ability to play the game while paused.
        /// </summary>
        public override string Settings_AllowPause => "Permet le contrôle en pause";

        /// <summary>
        ///  In game settings menu.Difficulty option. Have bosses that enter the game.
        /// </summary>
        public override string Settings_BossEvents => "Évenements de boss";

        /// <summary>
        ///  In game settings menu.Difficulty option. No Boss description.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Désactiver les boss passe le jeu en mode bac à sable, sans fin possible..";


        /// <summary>
        /// Options for automating game mechanics. Menu title.
        /// </summary>
        public override string Automation_Title => "Automatisation";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Attendra que les ouvriers soient au maximum";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Pause si les revenus sont négatifs";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_Priority => "Les plus grosses villes en priorité";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Permet un maximum d'un achat par seconde";


        /// <summary>
        /// Button caption for action. A specialized building for knights and diplomats.
        /// </summary>
        public override string HudAction_BuyItem => "Acheter {0}";

        /// <summary>
        /// The state of peace or war between two nations
        /// </summary>
        public override string Diplomacy_RelationType => "Relation";

        /// <summary>
        /// Titel for list of relations other factions have with eachother
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Leurs relations avec les autres";

        /// <summary>
        /// Diplomatic relation. You are in direct control over the nations resources.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Serviteur";

        /// <summary>
        /// Diplomatic relation. Full co-operation.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Allié";

        /// <summary>
        /// Diplomatic relation. Reduced chance of war.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "Bon";

        /// <summary>
        /// Diplomatic relation. Peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Paix";

        /// <summary>
        /// Diplomatic relation. Have not yet made any contact.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Neutre";
        /// <summary>
        /// Diplomatic relation. Temporary peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Trève";
        /// <summary>
        /// Diplomatic relation. War.
        /// </summary>
        public override string Diplomacy_RelationType_War => "Guerre";
        /// <summary>
        /// Diplomatic relation. War with no chance of peace.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Guerre Totale";

        /// <summary>
        /// Diplomatic communication. How well you can discuss terms. 0: SpeakTerms
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "Termes de discussion: {0}";

        /// <summary>
        /// Diplomatic communication. Better than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "Bons";

        /// <summary>
        /// Diplomatic communication. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Normaux";

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Mauvais";

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "Aucun";

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Forger des relations avec: {0}";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Offrir la paix";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Offrir une alliance";

        /// <summary>
        /// Diplomatic title. Another player Suggested a new diplomatic relation. 0: player name
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} propose de nouvelles relations";

        /// <summary>
        /// Diplomatic action. Accept new diplomatic relation.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Accepter la relation";

        /// <summary>
        /// Diplomatic description. Another player Suggested a new diplomatic relation. 0: relation type
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "Nouvelle relation offerte: {0}";

        /// <summary>
        /// Diplomatic action. Make another nation to serve you.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Vassaliser";

        /// <summary>
        /// Diplomatic description. Is against evil.
        /// </summary>
        public override string Diplomacy_LightSide => "Est l'allié du bien";

        /// <summary>
        /// Diplomatic description. How long the truce will last.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "Finit dans {0} secondes";

        /// <summary>
        /// Diplomatic action. Make the truce last longer.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Étendre la trève";

        /// <summary>
        /// Diplomatic description. How long the truce will be extended.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Étend la trève de  {0} secondes";

        /// <summary>
        /// Diplomatic description. Going against an agreed relation will cost diplomatic points.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "Briser cette relation coûtera {0} points diplomatiques";

        /// <summary>
        /// Diplomatic description for allies.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Les alliés partagent la déclaration de guerre.";

        /// <summary>
        /// Diplomatic description for good relation.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Limite votre capacité à déclarer la guerre.";

        /// <summary>
        /// Diplomatic description. You must have a larger military force than your servant (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "puissance militaire {0}x plus haute";

        /// <summary>
        /// Diplomatic description. Servant must be stuck in a hopeless war (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "Les vassaux doivent être en guerre contre un adversaire plus fort.";

        /// <summary>
        /// Diplomatic description. A servant can't own too many cities (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "Les vassaux ont un maximum de {0} villes";

        /// <summary>
        /// Diplomatic description. Const in diplomatic points will increase (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "Le prix augmentera pour chaque vassal";

        /// <summary>
        /// Diplomatic description. The result of servant relation, peaceful take over of another nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Absorbe l'autre faction";

        /// <summary>
        /// Messaage when you recieve a war declaration
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "Guerre déclarée!";

        /// <summary>
        /// The truce timer har run out, and you go back to war
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "La trève s'achève";

        /// <summary>
        /// Stats that are shown on the end game screen. Display title.
        /// </summary>
        public override string Statistics_Title => "Stats";
        /// <summary>
        /// Stats that are shown on the end game screen. Total ingame time passed.
        /// </summary>
        public override string EndGameStatistics_Time => "Temps en jeu: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. How many soldiers you bought.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "Soldats recrutés: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that died in battle.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Soldats perdus: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of opponent soldiers you killed in battle.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Soldats ennemis tués: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that have left you.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Déserteurs: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities won in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "Villes capturées: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities lost in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "Villes perdues: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle win results.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Batailles gagnées: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle lost results.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Batailles perdues: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Diplomacy. War declarations made by you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "Déclarations de guerre effectuées: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen.  Diplomacy. War declarations made toward you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "Déclarations de guerre reçues: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Allies made through diplomacy.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Alliances diplomatiques: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Servants made through diplomacy. Servants cities and armies become yours.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Vassaux: {0}";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_Army => "Armée";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_SoldierGroup => "Groupe";

        /// <summary>
        /// Collective unit type on the map. Common name for village or city.
        /// </summary>
        public override string UnitType_City => "Ville";

        /// <summary>
        /// A group selection of armies
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Groupe d'armées, nombre: {0}";

        /// <summary>
        /// Name for a specialized type of soldier. Standard front line soldier.
        /// </summary>
        public override string UnitType_Soldier => "Soldat";

        /// <summary>
        /// Name for a specialized type of soldier. Naval battle soldier.
        /// </summary>
        public override string UnitType_Sailor => "Marin";

        /// <summary>
        /// Name for a specialized type of soldier. Drafted peasants.
        /// </summary>
        public override string UnitType_Folkman => "Citoyen";

        /// <summary>
        /// Name for a specialized type of soldier. Shield and spear unit.
        /// </summary>
        public override string UnitType_Spearman => "Lancier";

        /// <summary>
        /// Name for a specialized type of soldier. Elite force, part of the Kings guard.
        /// </summary>
        public override string UnitType_HonorGuard => "Garde d'honneur";

        /// <summary>
        /// Name for a specialized type of soldier. Anti cavalry, wears long two-handed spears.
        /// </summary>
        public override string UnitType_Pikeman => "Piquier";

        /// <summary>
        /// Name for a specialized type of soldier. Armored cavalry unit.
        /// </summary>
        public override string UnitType_Knight => "Chevalier";

        /// <summary>
        /// Name for a specialized type of soldier. Bow and arrow.
        /// </summary>
        public override string UnitType_Archer => "Archer";

        /// <summary>
        /// Name for a specialized type of soldier. 
        /// </summary>
        public override string UnitType_Crossbow => "Arbalètrier";

        /// <summary>
        /// Name for a specialized type of soldier. Warmashine that slings large spears.
        /// </summary>
        public override string UnitType_Ballista => "Ballista";

        /// <summary>
        /// Name for a specialized type of soldier. A fantasy troll wearing a cannon.
        /// </summary>
        public override string UnitType_Trollcannon => "Canon troll";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier from the forest.
        /// </summary>
        public override string UnitType_GreenSoldier => "Soldat Vert";

        /// <summary>
        /// Name for a specialized type of soldier. Naval unit from the north.
        /// </summary>
        public override string UnitType_Viking => "Viking";

        /// <summary>
        /// Name for a specialized type of soldier. The evil master boss.
        /// </summary>
        public override string UnitType_DarkLord => "Seigneur Sombre";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier that carries a large flag.
        /// </summary>
        public override string UnitType_Bannerman => "Porte-drapeau";

        /// <summary>
        /// Name for a military unit. Soldier carrying ship. 0: unit type it carries
        /// </summary>
        public override string UnitType_WarshipWithUnit => "Navire {0}";

        public override string UnitType_Description_Soldier => "Une unité de combat polyvalente.";
        public override string UnitType_Description_Sailor => "Unité puissante lors de combats maritimes.";
        public override string UnitType_Description_Folkman => "Unité abordable, peu entraînée.";
        public override string UnitType_Description_HonorGuard => "Soldats d'élite sans coût de maintenance.";
        public override string UnitType_Description_Knight => "Unité puissante dans les batailles à champs ouvert.";
        public override string UnitType_Description_Archer => "Unité puissante si protégée.";
        public override string UnitType_Description_Crossbow => "Unité puissante à distance.";
        public override string UnitType_Description_Ballista => "Unité puissante contre les villes.";
        public override string UnitType_Description_GreenSoldier => "Guerrier elfique redoutable.";

        public override string UnitType_Description_DarkLord => "Le boss final.";

        /// <summary>
        /// Information about a soldier type
        /// </summary>
        public override string SoldierStats_Title => "Stats par unité";

        /// <summary>
        /// How many groups of soldiers
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} groupes, pour un total de {1} unités";

        /// <summary>
        /// Soldiers will have different strengths depending if the attack on open field, from ships or attacking a settlement
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Puissance d'attaque: Land {0} | Sea {1} | City {2}";

        /// <summary>
        /// How many wounds a soldier can endure
        /// </summary>
        public override string SoldierStats_Health => "Vie";

        /// <summary>
        /// Some soldiers will increase the army movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Bonus de vitesse terrestre: {0}";

        /// <summary>
        /// Some soldiers will increase the ship movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Bonus de vitesse maritime: {0}";

        /// <summary>
        /// Purchased soliders will start as recruits and complete their training after a few minutes.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Temps d'entraînement: {0} minutes. Deux fois plus rapide si adjacent à une ville.";

        /// <summary>
        /// Menu option to control an army. Make them stop moving.
        /// </summary>
        public override string ArmyOption_Halt => "Halte";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_Disband => "Abandonner les unités";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_Divide => "Diviser l'armée";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_RemoveX => "Retirer {0}";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Tout abandonner";

        /// <summary>
        /// Menu option to control an army. 0: Count, 1: Unit type
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "Groupe {1}: {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToX => "Envoyer les unités vers {0}";

        public override string ArmyOption_MergeAllArmies => "Fusionner toutes les armées";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Diviser les unités vers une nouvelle armée";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendX => "Envoyer {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendAll => "Tout envoyer";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Diviser de moitié";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Fusionner les armées";



        /// <summary>
        /// Purchase soldiers.
        /// </summary>
        public override string UnitType_Recruit => "Recruter";

        /// <summary>
        /// Purchase soldiers of type. 0:type
        /// </summary>
        public override string CityOption_RecruitType => "Recruter {0}";

        /// <summary>
        /// Number of paid soldiers
        /// </summary>
        public override string CityOption_XMercenaries => "Mercenaires: {0}";


        /// <summary>
        /// Indicates the number of mercenaries currently available for hire from the market
        /// </summary>
        public override string Hud_MercenaryMarket => "Mercenaires sur le marché";

        /// <summary>
        /// Purchase a number of paid soldiers
        /// </summary>
        public override string CityOption_BuyXMercenaries => "Importer {0} mercenaires";

        public override string CityOption_Mercenaries_Description => "Les soldats seront des mercenaires plutôt que des ouvriers";

        /// <summary>
        /// Button caption for action. Create housing for more workers.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "Engager ouvriers";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "Ouvriers max +{0}";
        public override string CityOption_ExpandGuardSize => "Étendre la garde";

        public override string CityOption_Damages => "Dégats: {0}";
        public override string CityOption_Repair => "Réparer les dégats";
        public override string CityOption_RepairGain => "Réparer {0} dégats";

        public override string CityOption_Repair_Description => "Les dégats diminuent la capacité en ouvriers.";


        public override string CityOption_BurnItDown => "Brûler";
        public override string CityOption_BurnItDown_Description => "Supprime les ouvriers et applique les dégats maximum";

        /// <summary>
        /// The main boss. Named after a glowing metal stone stuck in their forehead.
        /// </summary>
        public override string FactionName_DarkLord => "l'Oeil du Chaos";

        /// <summary>
        /// Orc inspired faction. Works for the dark lord.
        /// </summary>
        public override string FactionName_DarkFollower => "Serviteurs de la Terreur";

        /// <summary>
        /// The largest faction, the old but corrupted kingdom.
        /// </summary>
        public override string FactionName_UnitedKingdom => "Royaumes-Unis";

        /// <summary>
        /// Elf inspired faction. Lives in harmony with the forest.
        /// </summary>
        public override string FactionName_Greenwood => "Verdebois";

        /// <summary>
        /// Asian flavored faction to the east 
        /// </summary>
        public override string FactionName_EasternEmpire => "Empire de l'Est";

        /// <summary>
        /// Viking flavored kingdom in the north. The largest one.
        /// </summary>
        public override string FactionName_NordicRealm => "Royaume Nordiques";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a bear claw symbol.
        /// </summary>
        public override string FactionName_BearClaw => "Griffe d'Ours";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a cock symbol.
        /// </summary>
        public override string FactionName_NordicSpur => "Plume Nordique";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a black raven symbol.
        /// </summary>
        public override string FactionName_IceRaven => "Corbeau des Glaces";

        /// <summary>
        /// Faction famous for killing dragons with powerful ballistas.
        /// </summary>
        public override string FactionName_Dragonslayer => "Chasse-Dragon";

        /// <summary>
        /// A mercenary unit from the south. Arabic flavored.
        /// </summary>
        public override string FactionName_SouthHara => "Hara du Sud";

        /// <summary>
        /// Name for neutral CPU controlled nations
        /// </summary>
        public override string FactionName_GenericAi => "IA {0}";

        /// <summary>
        /// Display name for players and their numbers
        /// </summary>
        public override string FactionName_Player => "Joueur {0}";

        /// <summary>
        /// Message for when a miniboss is approaching on ships from the south.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "Un ennemi approche!";
        public override string EventMessage_HaraMercenaryText => "Des mercenaires Hara ont été repérés au Sud!";

        /// <summary>
        /// First warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "Une sombre prophétie";
        public override string EventMessage_ProphesyText => "L'oeil du chaos apparaîtra bientôt, et vos ennemis se joindront à lui!";

        /// <summary>
        /// Second warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Une ère sombre";
        public override string EventMessage_FinalBossEnterText => "L'oeil du chaos est apparu sur la carte!";

        /// <summary>
        /// Message when the main boss will meet you on the battlefield.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "Une attaque désesperée";
        public override string EventMessage_FinalBattleText => "Le Seigneur Sombre est entré sur le champ de bataille. C'est votre chance de l'éliminer!";

        /// <summary>
        /// Message when soldiers leave the army when you can't pay thier upkeep
        /// </summary>
        public override string EventMessage_DesertersTitle => "Déserteurs!";
        public override string EventMessage_DesertersText_Money => "Des soldats non-payés désertent votre armée.";

        public override string DifficultyDescription_AiAggression => "Agressivité de l'IA: {0}.";
        public override string DifficultyDescription_BossSize => "Taille du boss: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Temps d'arrivée du boss: {0}.";
        public override string DifficultyDescription_AiEconomy => "Economie IA: {0}%.";
        public override string DifficultyDescription_AiDelay => "Délai IA: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Difficulté diplomatique: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Coût des mercenaires: {0}.";
        public override string DifficultyDescription_HonorGuards => "Gardes d'Honneur: {0}.";


        /// <summary>
        /// Game has ended in success.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Victoire !";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
        {
            "En temps de paix, nous faisons le deuil de nos morts.",
            "Derrière chaque triomphe se cache l'ombre d'un sacrifice.",
            "Souvenez-vous du chemin qui nous a mené ici, pavé des âmes des plus braves.",
            "Nos esprits ont la légéreté de la victoire, mais nos coeurs portent le poids de ceux qui sont tombés."
        };

        public override string EndScreen_DominationVictoryQuote => "Les dieux m'ont choisi pour dominer ce monde!";

        /// <summary>
        /// Game has ended in failure.
        /// </summary>
        public override string EndScreen_FailTitle => "Défaite!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
        {
            "Nous accueillons notre fin, le corps comme l'esprit usé par la guerre.",
            "La défaite peut bien assombrir nos contrées, mais elle n'éteindra pas le feu de notre détermination.",
            "Ils peuvent éteindre la flamme de nos coeurs, mais de ses cendres naîtra une nouvelle aube.",
            "Que nos histoires attisent le feu de la victoire de demain.",
        };

        /// <summary>
        /// A small cutscene at the end of the game
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Regarder l'épilogue";

        /// <summary>
        /// Cutscene title
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Epilogue";

        /// <summary>
        /// Cutscene introduction
        /// </summary>
        public override string EndScreen_Epilogue_Text => "Il y a 160 ans";

        /// <summary>
        /// The Prologue is a short poem about the game's stroy
        /// </summary>
        public override string GameMenu_WatchPrologue => "Regarder le prologue";

        public override string Prologue_Title => "Prologue";

        /// <summary>
        /// The poem must be three lines, the fourth line will be pulled from the names translations to present the name of the boss
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
        {
            "Le fardeau qui vous encombre,",
            "la prophétie d'un triste sort,",
            "Vient celui craint même de la mort,",
        };

        /// <summary>
        /// Ingame menu when pausing
        /// </summary>
        public override string GameMenu_Title => "Menu";

        /// <summary>
        /// Continue playing the game after end screen
        /// </summary>
        public override string GameMenu_ContinueGame => "Continuer";

        /// <summary>
        /// Continue playing the game
        /// </summary>
        public override string GameMenu_Resume => "Reprendre";

        /// <summary>
        /// Exit to game lobby
        /// </summary>
        public override string GameMenu_ExitGame => "Quitter";

        public override string Hud_Save => "Sauvegarder";
        public override string GameMenu_SaveStateWarnings => "Avertissement! Vous perdrez vos sauvegardes lorsque le jeu sera mis à jour.";
        public override string GameMenu_LoadState => "Charger";
        public override string GameMenu_ContinueFromSave => "Continuer sauvegarde";

        public override string GameMenu_AutoSave => "Sauvegarde Auto";

        public override string GameMenu_Load_PlayerCountError => "Vous devez définir un nombre de joueurs correspondant à la sauvegarde: {0}";

        public override string Progressbar_MapLoadingState => "Chargement de la carte: {0}";

        public override string Progressbar_ProgressComplete => "Terminé";

        /// <summary>
        /// 0: progress in percentage, 1: fail count
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Génération: {0}%. ({1} échecs)";


        /// <summary>
        /// 0: current part, 1: number of parts
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "Partie {0}/{1}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_SaveProgress => "Sauvegarde: {0}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_LoadProgress => "Chargement: {0}";

        /// <summary>
        /// Progress done, waiting for player input
        /// </summary>
        public override string Progressbar_PressAnyKey => "Appuyez sur une touche pour continuer";


        /// <summary>
        /// A short tutorial where you are supposed to buy and move a soldier. All advanced controls are locked away until the tutorial is complete.
        /// </summary>
        public override string Tutorial_MenuOption => "Lancer le tutoriel";
        public override string Tutorial_MissionsTitle => "Missions du tutoriel";
        public override string Tutorial_Mission_BuySoldier => "Choisissez une ville, et recrutez un soldat.";
        public override string Tutorial_Mission_MoveArmy => "Sélectionnez une armée pour la déplacer.";

        public override string Tutorial_CompleteTitle => "Tutoriel terminé!";
        public override string Tutorial_CompleteMessage => "Vous avez débloqué le zoom plein ainsi que les options avancées.";

        /// <summary>
        /// Displays the button input
        /// </summary>
        public override string Tutorial_SelectInput => "Selectionner";
        public override string Tutorial_MoveInput => "Ordre de déplacement";



        /// <summary>
        /// Versus. Text describing the two armies that will go into battle
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "Déclaration de guerre";

        public override string ArmyOption_Attack => "Attaque";



        //----
        /// <summary>
        /// In game settings menu. Change what keys and buttons do when pressed
        /// </summary>
        public override string Settings_ButtonMapping => "Raccourcis";



        /// <summary>
        /// Input type, standard PC input
        /// </summary>
        public override string Input_Source_Keyboard => "Clavier & Souris";

        /// <summary>
        /// Input type, handheld controller like the xbox uses
        /// </summary>
        public override string Input_Source_Controller => "Manette";


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */
        public override string CityMenu_SalePricesTitle => "Prix de vente";
        public override string Blueprint_Title => "Plan";
        public override string Resource_Tab_Overview => "Aperçu";
        public override string Resource_Tab_Stockpile => "Réserve";

        public override string Resource => "Ressource";
        public override string Resource_StockPile_Info => "Définissez un objectif de stockage de ressource. Celà ordonnera aux ouvriers de passer à d'autres ressources.";
        public override string Resource_TypeName_Water => "eau";
        public override string Resource_TypeName_Wood => "bois";
        public override string Resource_TypeName_Fuel => "carburant";
        public override string Resource_TypeName_Stone => "pierre";
        public override string Resource_TypeName_RawFood => "nourriture crûe";
        public override string Resource_TypeName_Food => "nourriture";
        public override string Resource_TypeName_Beer => "bière";
        public override string Resource_TypeName_Wheat => "blé";
        public override string Resource_TypeName_Linen => "lin";
        //public override string Resource_TypeName_SkinAndLinen => "skin and linen";
        public override string Resource_TypeName_IronOre => "minerai de fer";
        public override string Resource_TypeName_GoldOre => "minerai d'or";
        public override string Resource_TypeName_Iron => "fer";

        public override string Resource_TypeName_SharpStick => "Bâton Pointu";
        public override string Resource_TypeName_Sword => "Epée";
        public override string Resource_TypeName_KnightsLance => "Lance de chevalier";
        public override string Resource_TypeName_TwoHandSword => "Zweihänder";
        public override string Resource_TypeName_Bow => "Arc";

        public override string Resource_TypeName_LightArmor => "Armure légère";
        public override string Resource_TypeName_MediumArmor => "Armure moyenne";
        public override string Resource_TypeName_HeavyArmor => "Armure lourde";

        public override string ResourceType_Children => "Enfants";

        public override string BuildingType_DefaultName => "Bâtiment";
        public override string BuildingType_WorkerHut => "Hutte d'ouvrier";
        //public override string BuildingType_Tavern => "Taverne";
        public override string BuildingType_Brewery => "Brasserie";
        public override string BuildingType_Postal => "Service postal";
        public override string BuildingType_Recruitment => "Centre de recrutement";
        public override string BuildingType_Barracks => "Garnison";
        public override string BuildingType_PigPen => "Porcherie";
        public override string BuildingType_HenPen => "Poulailler";
        public override string BuildingType_WorkBench => "Atelier";
        public override string BuildingType_Carpenter => "Charpentier";
        public override string BuildingType_CoalPit => "Mine de charbon";
        public override string DecorType_Statue => "Statue";
        public override string DecorType_Pavement => "Pavés";
        public override string BuildingType_Smith => "Forgeron";
        public override string BuildingType_Cook => "Cuisinier";
        public override string BuildingType_Storehouse => "Entrepôt";

        public override string BuildingType_ResourceFarm => "Ferme de {0}";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Augmente la limite d'ouvriers de {0}";
        public override string BuildingType_Tavern_Description => "Les ouvriers peuvent manger ici";
        public override string BuildingType_Tavern_Brewery => "Production de bière";
        public override string BuildingType_Postal_Description => "Envoie des ressources vers d'autres villes";
        public override string BuildingType_Recruitment_Description => "Envoie des hommes vers d'autres villes";
        public override string BuildingType_Barracks_Description => "Consomme des hommes et de l'équipement pour former des soldats";
        public override string BuildingType_PigPen_Description => "Produit des cochons, qui fournissent de la nourriture et des peaux";
        public override string BuildingType_HenPen_Description => "Produit des poules et des oeufs, qui fournissent de la nourriture";
        public override string BuildingType_Decor_Description => "Décoration";
        public override string BuildingType_Farm_Description => "Produit une ressource";

        public override string BuildingType_Cook_Description => "Station de nourriture";
        public override string BuildingType_Bench_Description => "Station de fabrication d'objets";

        public override string BuildingType_Smith_Description => "Station de fabrication de métal";
        public override string BuildingType_Carpenter_Description => "Station de fabrication de bois";

        public override string BuildingType_Nobelhouse_Description => "Logement pour les chevaliers et les diplomates";
        public override string BuildingType_CoalPit_Description => "Fabrique de carburant raffiné";
        public override string BuildingType_Storehouse_Description => "Point de dépôt de ressources";

        public override string MenuTab_Info => "Info";
        public override string MenuTab_Work => "Travail";
        public override string MenuTab_Recruit => "Recrutement";
        public override string MenuTab_Resources => "Ressources";
        public override string MenuTab_Trade => "Echange";
        public override string MenuTab_Build => "Construction";
        public override string MenuTab_Economy => "Economie";
        public override string MenuTab_Delivery => "Livraisons";

        public override string MenuTab_Build_Description => "Placer des bâtiments dans votre ville";
        public override string MenuTab_BlackMarket_Description => "Placer des bâtiments dans votre ville";
        public override string MenuTab_Resources_Description => "Placer des bâtiments dans votre ville";
        public override string MenuTab_Work_Description => "Placer des bâtiments dans votre ville";
        public override string MenuTab_Automation_Description => "Placer des bâtiments dans votre ville";

        public override string BuildHud_OutsideCity => "Hors des limites de la ville";
        public override string BuildHud_OutsideFaction => "Hors des frontières!";

        public override string BuildHud_OccupiedTile => "Case occupée";

        public override string Build_PlaceBuilding => "Bâtiment";
        public override string Build_DestroyBuilding => "Détruire";
        public override string Build_ClearTerrain => "Dégager le terrain";

        public override string Build_ClearOrders => "Dégager les ordres de construction";
        public override string Build_Order => "Ordres de construction";
        public override string Build_OrderQue => "File de construction: {0}";
        public override string Build_AutoPlace => "Placement auto";

        public override string Work_OrderPrioTitle => "Priorité du travail";
        public override string Work_OrderPrioDescription => "La priorité va de 1 (basse) à {0} (haute)";

        public override string Work_OrderPrio_No => "Sans priorité. Ne sera pas exploité.";
        public override string Work_OrderPrio_Min => "Priorité minimum.";
        public override string Work_OrderPrio_Max => "Priorité maximum.";

        public override string Work_Move => "Déplacer des objets";

        public override string Work_GatherXResource => "Récolter {0}";
        public override string Work_CraftX => "Fabriquer {0}";
        public override string Work_Farming => "Agriculture";
        public override string Work_Mining => "Minage";
        public override string Work_Trading => "Echange";

        public override string Work_AutoBuild => "Construction et expansion auto";

        public override string WorkerHud_WorkType => "Etat du travail: {0}";
        public override string WorkerHud_Carry => "Carrière: {0} {1}";
        public override string WorkerHud_Energy => "Energie: {0}";
        public override string WorkerStatus_Exit => "Quitter le travail";
        public override string WorkerStatus_Eat => "Manger";
        public override string WorkerStatus_Till => "Labourer";
        public override string WorkerStatus_Plant => "Planter";
        public override string WorkerStatus_Gather => "Récolter";
        public override string WorkerStatus_PickUpResource => "Ramasser la ressource";
        public override string WorkerStatus_DropOff => "Déposer";
        public override string WorkerStatus_BuildX => "Construire {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Retourner à l'armée";

        public override string Hud_ToggleFollowFaction => "Activer le suivi de faction";
        public override string Hud_FollowFaction_Yes => "Suit le réglage global des factions";
        public override string Hud_FollowFaction_No => "Utilise le réglage local (Le réglage global est {0})";

        public override string Hud_Idle => "Inactif";
        public override string Hud_NoLimit => "Illimité";

        public override string Hud_None => "Aucun";
        public override string Hud_ProductionQueue => "File de production";

        public override string Hud_EmptyList => "- Liste vide -";

        public override string Hud_RequirementOr => "- ou -";

        public override string Hud_BlackMarket => "Marché noir";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Choisir ville";
        public override string Conscription_Title => "Conscription";
        public override string Conscript_WeaponTitle => "Arme";
        public override string Conscript_ArmorTitle => "Armure";
        public override string Conscript_TrainingTitle => "Entraînement";

        public override string Conscript_SpecializationTitle => "Spécialisation";
        public override string Conscript_SpecializationDescription => "Augmente l'attaque dans une zone, et la réduit dans les autres de {0}";
        public override string Conscript_SelectBuilding => "Sélectionner la garnison";

        public override string Conscript_WeaponDamage => "Dégats de l'arme";
        public override string Conscript_ArmorHealth => "Vie de l'armure";
        public override string Conscript_AttackSpeed => "Vitesse d'attaque";
        public override string Conscript_TrainingTime => "Temps d'entraînement";

        public override string Conscript_Training_Minimal => "Minime";
        public override string Conscript_Training_Basic => "Basique";
        public override string Conscript_Training_Skillful => "Talentueux";
        public override string Conscript_Training_Professional => "Professionnel";

        public override string Conscript_Specialization_Field => "Champs ouvert";
        public override string Conscript_Specialization_Sea => "Navire";
        public override string Conscript_Specialization_Siege => "Siège";
        public override string Conscript_Specialization_Traditional => "Traditionnel";
        public override string Conscript_Specialization_AntiCavalry => "Anti-cavalerie";

        public override string Conscription_Status_CollectingEquipment => "Recherche d'équipement: {0}";
        public override string Conscription_Status_CollectingMen => "Recherche d'hommes: {0}";
        public override string Conscription_Status_Training => "Entraînement: {0}";

        public override string ArmyHud_Food_Reserves_X => "Réserves de nourriture: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Maintenance en nourriture: {0}";
        public override string ArmyHud_Food_Costs_X => "Coût en nourriture: {0}";

        public override string Deliver_WillSendXInfo => "Envoie {0} à la fois";
        public override string Delivery_ListTitle => "Choisir le service de livraison";
        public override string Delivery_DistanceX => "Distance: {0}";
        public override string Delivery_DeliveryTimeX => "Temps de livraison: {0}";
        public override string Delivery_SenderMinimumCap => "Minimum de l'envoyeur";
        public override string Delivery_RecieverMaximumCap => "Maximum du destinataire";
        public override string Delivery_ItemsReady => "Objets prêts";
        public override string Delivery_RecieverReady => "Destinataire prêt";
        public override string Hud_ThisCity => "Cette ville";
        public override string Hud_RecieveingCity => "Ville destinataire";

        public override string Info_ButtonIcon => "i";

        public override string Info_ResourcePerSecond => "Affiché en ressources par seconde.";

        public override string Info_MinuteAverage => "La valeur est une moyenne de la dernière minute";

        public override string Message_OutOfFood_Title => "Pénurie de Nourriture";
        public override string Message_CityOutOfFood_Text => "De la nourriture sera achetée à prix fort sur le marché noir. Les ouvriers mourront quand vous n'aurez plus d'or.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Type de terrain";

        public override string Hud_EnergyUpkeepX => "Maintenance en énergie {0}";

        public override string Hud_EnergyAmount => "{0} énergie (secondes de travail)";

        public override string Hud_CopySetup => "Copier";
        public override string Hud_Paste => "Coller";

        public override string Hud_Available => "Disponible";

        public override string WorkForce_ChildBirthRequirements => "Prérequis pour la naissance";
        public override string WorkForce_AvailableHomes => "Maisons disponibles: {0}";
        public override string WorkForce_Peace => "Paix";
        public override string WorkForce_ChildToManTime => "Âge adulte: {0} minutes";

        public override string Economy_TaxIncome => "Revenus des taxes: {0}";
        public override string Economy_ImportCostsForResource => "Coûts des imports pour {0}: {1}";
        public override string Economy_BlackMarketCostsForResource => "Coûts du marché noir pour {0}: {1}";
        public override string Economy_GuardUpkeep => "Maintenance des gardes: {0}";

        public override string Economy_LocalCityTrade_Export => "Exports de la ville: {0}";
        public override string Economy_LocalCityTrade_Import => "Imports de la ville: {0}";

        public override string Economy_ResourceProduction => "Production de {0}: {1}";
        public override string Economy_ResourceSpending => "Dépense de {0}: {1}";

        public override string Economy_TaxDescription => "La taxe est de {0} or par ouvrier";

        public override string Economy_SoldResources => "Ressource vendue (minerai d'or): {0}";

        public override string UnitType_Cities => "Villes";
        public override string UnitType_Armies => "Armées";
        public override string UnitType_Worker => "Ouvrier";

        public override string UnitType_FootKnight => "Chevalier Epéiste";
        public override string UnitType_CavalryKnight => "Chevalier de Cavalerie";

        public override string CityCulture_LargeFamilies => "Familles nombreuses";
        public override string CityCulture_FertileGround => "Terres fertiles";
        public override string CityCulture_Archers => "Archers entraînés";
        public override string CityCulture_Warriors => "Guerriers";
        public override string CityCulture_AnimalBreeder => "Eleveurs d'animaux";
        public override string CityCulture_Miners => "Mineurs";
        public override string CityCulture_Woodcutters => "Bûcherons";
        public override string CityCulture_Builders => "Bâtisseurs";
        public override string CityCulture_CrabMentality => "Mentalité de crabe";
        public override string CityCulture_DeepWell => "Puits profond";
        public override string CityCulture_Networker => "Réseauteur";
        public override string CityCulture_PitMasters => "Maîtres des tunnels";

        public override string CityCulture_Culture => "Culture";
        public override string CityCulture_LargeFamilies_Description => "Plus d'enfants naissent";
        public override string CityCulture_FertileGround_Description => "Meilleurs rendements agricoles";
        public override string CityCulture_Archers_Description => "Produit des archers entraînés";
        public override string CityCulture_Warriors_Description => "Produit des combattants de mêlée entraînés";
        //public override string CityCulture_AnimalBreeder_Description => "Les animaux donnent plus de ressources";
        public override string CityCulture_Miners_Description => "Plus de rendements des mines";
        public override string CityCulture_Woodcutters_Description => "Les arbres donnent plus de bois";
        public override string CityCulture_Builders_Description => "Construction plus rapide";
        public override string CityCulture_CrabMentality_Description => "Le travail coûte moins d'énergie. Ne peut pas produire de soldats experimentés.";
        public override string CityCulture_DeepWell_Description => "L'eau se régénère plus vite";
        public override string CityCulture_Networker_Description => "Service postal plus efficace";
        public override string CityCulture_PitMasters_Description => "Plus de production de carburant";

        public override string CityOption_AutoBuild_Work => "Expansion auto des ouvriers";
        public override string CityOption_AutoBuild_Farm => "Expansion auto des fermes";

        public override string Hud_PurchaseTitle_Resources => "Acheter des ressources";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "Vous possédez";

        public override string Tutorial_EndTutorial => "Finir le tutoriel";
        public override string Tutorial_MissionX => "Mission {0}";
        public override string Tutorial_CollectXAmountOfY => "Collecter {0} {1}";
        public override string Tutorial_SelectTabX => "Choisir onglet: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Augmente la priorité sur: {0}";
        public override string Tutorial_PlaceBuildOrder => "Placer l'ordre de construction: {0}";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Choisir une ville";
        public override string Tutorial_ZoomInWorkers => "Zoomer pour voir les ouvriers";
        public override string Tutorial_CreateSoldiers => "Crée deux unités de soldats avec cet équipement: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Dézoomer vers la vision globale";
        public override string Tutorial_ZoomOutDiplomacy => "Dézoomer vers la vision diplomatique";
        public override string Tutorial_ImproveRelations => "Augmente vos relations avec une faction voisine";
        public override string Tutorial_MissionComplete_Title => "Mission accomplie!";
        public override string Tutorial_MissionComplete_Unlocks => "De nouvelles commandes ont été débloquées.";

        //patch1
        public override string Resource_ReachedStockpile => "Capacité de l'entrepôt atteinte";

        public override string BuildingType_ResourceMine => "mine de {0}";

        public override string Resource_TypeName_BogIron => "Fer de marais";

        public override string Resource_TypeName_Coal => "Charbon";

        public override string Language_XUpkeep => "Maintenance de {0}";
        public override string Language_XCountIsY => "Nombre de {0}: {1}";

        public override string Message_ArmyOutOfFood_Text => "De la nourriture sera achetée à prix fort sur le marché noir. Des soldats déserteront votre armée lorsque vous serez à court d'or.";

        public override string Info_ArmyFood1 => "Les armées se ravitailleront en nourriture dans la ville alliée la plus proche.";
        public override string Info_ArmyFood2 => "De la nourriture peut être achetée auprès d'autres factions.";
        public override string Info_ArmyFood3 => "Dans les régions hostiles, la nourriture ne peut être achetée qu'au marché noir.";

        public override string FactionName_Monger => "Gneurdeger";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

        //patch2
        public override string Tutorial_BuildSomething => "Construire une structure produisant {0}";
        public override string Tutorial_BuildCraft => "Construire un atelier de: {0}";
        public override string Tutorial_IncreaseBufferLimit => "Augmenter la limite de: {0}";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "Atteindre une réserve de {0} {1}";
        public override string Tutorial_LookAtFoodBlueprint => "Regarder la recette de la nourriture";
        public override string Tutorial_CollectFood_Info1 => "Les ouvriers se rendent à l'hotel de ville pour manger.";
        public override string Tutorial_CollectFood_Info2 => "Les armées envoie des ouvriers pour le ravitaillement";
        public override string Tutorial_CollectFood_Info0 => "Pour un contrôle complet des ouvriers, réglez toutes les priorités sur zéro, puis activez-les une à la fols.";

        public override string EndGameStatistics_DecorsBuilt => "Décorations construites: {0}";
        public override string EndGameStatistics_StatuesBuilt => "Statues construites: {0}";


        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "Par défaut, les ouvriers se rendront à l'hotel de ville pour y déposer leurs objets.";
        public override string GameMenu_UseSpeedX => "Vitesse {0}";
        public override string GameMenu_LongerBuildQueue => "File de construction étendue";

        public override string Diplomacy_RelationWithOthers => "Leurs relations avec les autres";
        public override string Automation_queue_description => "Se répétera jusqu'à ce que la file soit vide";

        //public override string BuildingType_Storehouse_Description => "Les ouvriers déposeront leurs objets ici";

        public override string Resource_TypeName_Longbow => "Arc long";
        public override string Resource_TypeName_Rapeseed => "colza";
        public override string Resource_TypeName_Hemp => "chanvre";

        public override string Resource_BogIronDescription => "Il est plus efficace de miner du fer que d'utiliser du fer des marais.";


        public override string Resource_FoodSafeGuard_Description => "Garde-fou. Maximisera la priorité de la production de nourriture si elle tombe en dessous de {0}.";
        public override string Resource_FoodSafeGuard_Active => "Le garde-fou est actif.";

        public override string GameMenu_NextSong => "Chanson suivante";

        public override string BuildingType_Bank => "Banque";
        public override string BuildingType_GoldDelivery_Description => "Envoyer de l'or aux autres villes";

        public override string BuildingType_Logistics => "Logistique";
        public override string BuildingType_Logistics_Description => "Augmente votre capacité à commander les bâtiments";

        public override string BuildingType_Logistics_NationSizeRequirement => "Force ouvrière totale de la nation: {0}";
        public override string Requirements_XItemStorageOfY => "Réserve de {0} dans la ville: {1}";


        public override string XP_UnlockBuildQueue => "Débloque la taille de file: {0}";
        public override string XP_UnlockBuilding => "Débloque le bâtiment: ";
        public override string XP_Upgrade => "Améliorer";

        public override string XP_UpgradeBuildingX => "Amélliore le bâtiment: {0}";

        /// <summary>
        /// Title for describing the production cycle of farms
        /// </summary>
        public override string BuildHud_PerCycle => "Par cycle";
        public override string BuildHud_MayCraft => "Peut produire";
        public override string BuildHud_WorkTime => "Temps de travail: {0}";
        public override string BuildHud_GrowTime => "Temps de pousse: {0}";
        public override string BuildHud_Produce => "Produit:";

        public override string BuildHud_Queue => "File de construction autorisée: {0}/{1}";

        public override string LandType_Flatland => "Terrain plat";
        public override string LandType_Water => "Eau";
        public override string BuildingType_Wall => "Mur";
        public override string Delivery_AutoReciever_Description => "Enverra à la ville avec le moins de ressources";

        public override string Hud_On => "On";
        public override string Hud_Off => "Off";

        public override string Hud_Time_Seconds => "{0} secondes";
        public override string Hud_Time_Minutes => "{0} minutes";
        public override string Hud_Undo => "Annuler";
        public override string Hud_Redo => "Refaire";

        public override string Tag_ViewOnMap => "Voir les tags sur la map";

        public override string MenuTab_Tag => "Tag";

        public override string Input_Build => "Construire";

        public override string FlagEditor_ClearAll => "Tout vider";


        public override string CityCulture_Stonemason => "Tailleurs de pierre";
        public override string CityCulture_Stonemason_Description => "Améliore la récolte de pierre";

        public override string CityCulture_Brewmaster => "Maîtres brasseurs";
        public override string CityCulture_Brewmaster_Description => "Améliore la production de bière";

        public override string CityCulture_Weavers => "Tisserands";
        public override string CityCulture_Weavers_Description => "Améliore la production d'armures légères";

        public override string CityCulture_SiegeEngineer => "Ingénieurs de siège";
        public override string CityCulture_SiegeEngineer_Description => "Engins de siège plus puissants";

        public override string CityCulture_Armorsmith => "Armuriers";
        public override string CityCulture_Armorsmith_Description => "Améliore la production d'armures en fer";

        public override string CityCulture_Noblemen => "Nobles";
        public override string CityCulture_Noblemen_Description => "Chevaliers plus puissants";

        public override string CityCulture_Seafaring => "Marins";
        public override string CityCulture_Seafaring_Description => "Les soldats avec une spécialisation maritime ont des navires plus puissants";

        public override string CityCulture_Backtrader => "Reselleurs";
        public override string CityCulture_Backtrader_Description => "Marché noir moins cher";

        public override string CityCulture_LawAbiding => "Honnêtes gens";
        public override string CityCulture_LawAbiding_Description => "Gagnez plus de taxes. Pas de marché noir.";

        //##2##

        public override string Hud_Advanced => "Avancé";
        public override string Hud_Loading => "Chargement...";

        public override string CityOption_LowerGuardSize => "Réduire la garde";
        public override string Hud_Purchase_MinCapacity => "Capacité minimum atteinte";
        public override string Settings_ResetToDefault => "Réinitialiser";
        public override string Settings_NewGame => "Nouvelle partie";

        public override string Settings_AdvancedGameSettings => "Paramètres avancés";
        public override string Settings_FoodMultiplier => "Multiplicateur de nourriture";
        public override string Settings_FoodMultiplier_Description => "La durée pendant laquelle les ouvriers et soldats restent rassasiés. Une valeur haute réduira les performances de l'ordinateur.";

        public override string Settings_GameMode => "Mode de jeu";

        public override string Settings_Mode_Story => "Histoire complète";
        public override string Settings_Mode_IncludeBoss => "Active les évènements de boss.";
        public override string Settings_Mode_IncludeAttacks => "Active les attaques aléatoires.";
        public override string Settings_Mode_Sandbox => "Bac à sable";
        public override string Settings_Mode_Peaceful => "Pacifique";
        public override string Settings_Mode_Peaceful_Description => "Les guerres ne peuvent être déclarées que par le joueur";

        public override string Lobby_ImportSave => "Importer la sauvegarde";

        public override string Lobby_ExportSave => "Exporter la sauvegarde";
        public override string Lobby_ExportSave_Description => "Crée une copie du fichier et la place dans le dossier d'export: {0}";

        public override string Resource_CurrentAmount => "Quantité actuelle: {0}";
        public override string Resource_MaxAmount_Soft => "Seuil (Limite max): {0}";
        public override string Resource_MaxAmount => "Limite max: {0}";
        public override string Resource_AddPerSec => "Augmente la fréquence: {0} par seconde";

        public override string Resource_WaterAddLimit => "La fréquence d'obtention de l'eau ne peut être augmentée";

        public override string Tutorial_Select_SubTab => "Sélectionnez la catégorie: {0}";



        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */


        public override string Tutorial_OpenGuardSubTab => "Ouvrez une garnison et sélectionnez la catégorie: {0}";
        public override string Tutorial_GuardToWall => "Déplacez un garde vers un mur";
        public override string Demo_MissionObjective_Title => "Objectif de mission";
        public override string Demo_MissionObjective_Description => "Protégez-vous des attaques du sud";
        public override string Demo_Complete_Title => "Démo terminée";
        public override string Demo_TimesUp_Title => "Temps écoulé!";
        public override string Demo_EndInOneMinuteDescription => "La démo s'achèvera dans une minute";

        public override string ArmyOption_NewArmy => "Nouvelle armée";
        public override string ProfileEditor_AltMain => "Principal alternatif";
        public override string Automation_CheckBoxTitle => "Automatisé";

        public override string ArmyStructure_ColumnWidth => "Largeur des colonnes";
        public override string ArmyStructure_ArmyPlacement => "Placement dans l'armée";
        public override string ArmyStructure_Row_Front => "Avant";
        public override string ArmyStructure_Row_Body => "Corps";
        public override string ArmyStructure_Row_Second => "Second";
        public override string ArmyStructure_Row_Behind => "Arrière";

        public override string Diplomacy_RelationType_Enemies => "Ennemis";

        public override string EventMessage_EnemyAlliance_Title => "Peur de domination";
        public override string EventMessage_EnemyAlliance => "Les nations, effrayées par votre puissance croissante, s'unissent dansune alliance contre vous.";

        public override string Settings_CentralGold => "Or central";
        public override string Settings_CentralGold_Description => "On: tout votre or est gardé dans un stock partagé pour utilisation immédiate. Off: L'or est matériel, et doit être transporté.";





        public override string InputActionName_StopStart => "Pause/Départ";
        public override string InputActionName_ToggleHudDetail => "Activer le détail de l'HUD";
        public override string InputActionName_NextCity => "Ville suivante";
        public override string InputActionName_NextArmy => "Armée suivante";
        public override string InputActionName_NextBattle => "Bataille suivante";
        public override string InputActionName_Build => "Construire";
        public override string InputActionName_Copy => "Copier";
        public override string InputActionName_Paste => "Coller";
        public override string InputActionName_Menu => "Menu";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "Couleur précédente";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "Couleur suivante";
        public override string InputActionName_FlagDesign_PaintBucket => "Sceau de peinture";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "Sélecteur de couleurs";
        public override string InputActionName_ControllerFocus => "Focus";
        public override string InputActionName_ControllerCancel => "Annuler";
        public override string InputActionName_ControllerMessageClick => "Clic message";
        public override string InputActionName_ControllerSelect => "Choisir";
        public override string InputActionName_WASD_UP => "Haut";
        public override string InputActionName_WASD_DOWN => "Bas";
        public override string InputActionName_WASD_LEFT => "Gauche";
        public override string InputActionName_WASD_RIGHT => "Droite";
        public override string InputActionName_CameraTiltLeft => "Inclinaison caméra gauche";
        public override string InputActionName_CameraTiltRight => "Inclinaison caméra droite";
        public override string InputActionName_CameraTiltUp => "Inclinaison caméra haut";
        public override string InputActionName_ZoomInKey => "Zoom +";
        public override string InputActionName_ZoomOutKey => "Zoom -";




        public override string Settings_Title_Monitor => "Options d'écran";
        public override string Settings_Title_Graphics => "Options graphiques";
        public override string Settings_Title_Input => "Entrée";
        public override string Settings_Title_Gameplay => "Options de jeu";
        public override string Settings_PanOnZoom => "Descente zoom";
        public override string Settings_ScrollSensitivity_Game => "Sensibilité molette: jeu";
        public override string Settings_ScrollSensitivity_Menu => "Sensibilité molette: menu";
        public override string Settings_Blood => "Sang";

        public override string Settings_MasterVolume => "Volume général";
        public override string Settings_AmbienceVolume => "Volume d'ambience";
        public override string Settings_BattleMelody => "Mélodie de bataille";

        public override string Settings_ModelLight => "Effets de lumière modèles";
        public override string Settings_Particles => "Effets de particules";
        public override string Settings_MapLoadSpeed => "Vitesse de chargement de la carte";
        public override string Lobby_Category_Options => "Options";
        public override string Lobby_Category_Editor => "Editeur";
        public override string Lobby_Category_ExtraModes => "Modes extra";

        public override string Lobby_Editor_MapEditor => "Editeur de map";
        public override string Lobby_Editor_VoxelEditor => "Editeur voxel";

        public override string Lobby_Mode_BattleLab => "Labo de bataille";
        public override string Lobby_Mode_BattleLab_Description => "Faites combattre n'importe quels soldats";
        public override string Lobby_Mode_Commander => "Jouer au Commander";
        public override string Lobby_Mode_Commander_Description => "Un petit jeu de plateau tactique";
        public override string Lobby_MusicPlayList => "Liste d'écoute";

        public override string Lobby_GameSetup => "Préparation de la partie";
        public override string Lobby_PlayerSetup => "Réglages joueurs";
        public override string LobbyDemoMode_Demo => "Démo";

        public override string Lobby_Tutorial => "Tutoriel";

        public override string LobbyDemoMode_ShortTutorial => "Tutoriel Rapide";
        public override string LobbyDemoMode_LongTutorial => "Tutoriel étendu";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "Ajoutez à la liste de souhaits";


        public override string BattleLab_StartHere => "Commencer la bataille ici";
        public override string BattleLab_Start => "Commencer la bataille";
        public override string BattleLab_Attacker => "Attaquant";



        public override string MapGenerator_Name => "Editeur de carte - générer";

        public override string MapType_CustomMap => "Carte personnalisée";
        public override string MapType_GenerateNewMap => "Générer une nouvelle carte";
        public override string MapGenerator_GenerateAction => "Générer";
        public override string MapGenerator_Terrain_CustomSize => "Taille personnalisée";
        public override string MapGenerator_Terrain_StartAs => "Commencer en tant que";
        public override string MapGenerator_Terrain_ClearPass => "Test Clear";
        public override string MapGenerator_Terrain_BuildPass => "Test Construction";
        public override string MapGenerator_Terrain_DigPass => "Test Creuser";
        public override string MapGenerator_Terrain_BuildDigLoops => "Nombre de boucles construire-creuser";
        public override string MapGenerator_Terrain_BuildStrokes => "Nombre de coups de construction";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Mesuré en coups de peinture par 100 cases";
        public override string MapGenerator_Terrain_DigStrokes => "Nombre de creusages";
        public override string MapGenerator_Terrain_CleanUp_Option => "Nettoyage des cases isolées";
        public override string MapGenerator_Terrain_CleanUpPass => "Test Nettoyage";



        public override string Economy_ServicemenUpkeep => "Maintenances des hommes de service: {0}";
        public override string Economy_ServicemenUpkeep_Description => "La maintenance coûte {0} d'or par homme de service";
        public override string Economy_GuardUpkeep_Description => "La maintenance coûte {0} d'or par garde";

        public override string EndScreen_TimeHasEndedTitle => "Le temps est écoulé";

        public override string Hud_AdvancedSettings => "Paramètres avancés";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "Annuler";
        public override string Hud_Delete => "Supprimer";
        public override string Hud_Next => "Suivant";
        //public override string Hud_None => "None";
        public override string Hud_Apply => "Appliquer";
        public override string Hud_AllCities => "Toutes les villes";
        public override string Hud_Time_Hours => "{0} heures";
        public override string Hud_AddX => "Ajouter {0}";
        public override string Hud_Both => "Les deux";
        public override string Hud_Direction => "Direction";
       

        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, nombre: {1}";

        public override string Hud_EffectDoesNotStack => "Cet effet ne s'accumule pas";

        public override string Work_SmeltX => "Fondre {0}";

        public override string Info_TotalFoodProduction => "Production totale de nourriture";
        public override string Info_TotalFoodSpending => "Dépenses totales de nourriture";

        public override string Info_FooodAndDeliveryLocation => "Par défaut, les ouvriers se rendent à l'hotel de ville pour manger ou déposer des objets";

        public override string Delivery_SendChunk => "Objets par livraison";
        public override string Delivery_SpeedBonus => "Bonus de vitesse: {0}%";

        public override string Delivery_AutoResourceDescription => "Livre les objets dont la réserve a atteint sa limite aux villes qui en ont besoin.";

        public override string Conscript_Soldiers_ArmyType => "Hommes d'armes";
        public override string Conscript_Soldiers_ArmyType_Description => "Recrute des soldats pour une armée adjacente";
        public override string Conscript_Soldiers_GuardType => "Garde de la Ville";
        public override string Conscript_Soldiers_GuardType_Description => "Les gardes servent à fortifier les murs";
        //-
        public override string Defence_Title => "Défense";
        public override string Defence_GuardPost => "Poste de gardes";

        public override string Defence_WallDescription_Movement => "Ralentit les mouvements ennemis.";
        public override string Defence_WallDescription_GuardPost => "Des gardes peuvent y être stationnés.";
        public override string Defence_AutoAssign => "Assignement auto";
        public override string Defence_AutoAssign_Description => "Les nouveaux gardes se rendront à ce poste";
        public override string Conscript_SplashDamage => "Dégats de zone";
        public override string Conscript_HighSplashDamage => "Dégats de zone élevés";

        public override string Conscript_Training_Champion => "Champion";
        public override string Conscript_Training_Legendary => "Légendaire";


        public override string Experience_Title => "Expérience";
        public override string Experience_TopExperience => "Meilleurs niveaux d'expérience";

        public override string Experience_TimeReductionDescription => "Le temps de labeur est réduit de {0}% par niveau";

        public override string ExperienceType_Farm => "Fermier";
        public override string ExperienceType_AnimalCare => "Eleveur";
        public override string ExperienceType_HouseBuilding => "Bâtisseur";
        public override string ExperienceType_WoodWork => "Charpentier";
        public override string ExperienceType_StoneCutter => "Tailleur de pierres";
        public override string ExperienceType_Mining => "Mineur";
        public override string ExperienceType_Transport => "Transporteur";
        public override string ExperienceType_Cook => "Cuisinier";
        public override string ExperienceType_Fletcher => "Tailleur d'arcs";
        public override string ExperienceType_RefineOre => "Fondeur";
        public override string ExperienceType_Casting => "Mouleur";
        public override string ExperienceType_CraftMetal => "Forgeron";
        public override string ExperienceType_CraftArmor => "Armurier";
        public override string ExperienceType_CraftWeapon => "Forgeron d'armes";
        public override string ExperienceType_CraftFuel => "Charbonnier";
        public override string ExperienceType_Chemist => "Chimiste";

        public override string ExperienceLevel_1 => "Débutant";
        public override string ExperienceLevel_2 => "Practicien";
        public override string ExperienceLevel_3 => "Expert";
        public override string ExperienceLevel_4 => "Maître";
        public override string ExperienceLevel_5 => "Légendaire";

        public override string ExperenceOrDistancePrio_Title => "Sélection d'ouvrier";
        public override string ExperenceOrDistancePrio_Description => "Les ouvriers inactifs seront sélectionnés pour travailler, soit par distance soit par expérience";


        public override string Technology_Description => "Chaque ville dispose d'un arbe de technologie. Chaque technologie débloque des bâtiments et des objets.";
        public override string Experience_Description => "Les ouvriers gagnent de l'expérience pour s'améliorer";


        public override string Technology_Title => "Technologie";
        public override string Technology_ShareField => "Partage du champ de recherche";

        public override string Technology_GainByNeigborRelation => "Pour chaque ville voisine avec la technologie. Et votre relation est {0}: {1}";
        public override string Technology_ForEachMaster => "Quand {0} atteint un niveau d'expérience de {1} dans le domaine: {2}";
        public override string Technology_CitySpread => "Vos villes partageront les technologies si adjacentes: {0}";
        public override string Technology_CityCapture => "La plupart des technologies sont détruites quand une ville est capturée par la guerre";

        public override string Technology_AdvancedBuildings => "Bâtiments avancés";
        public override string Technology_AdvancedFarming => "Fermes avancées";
        public override string Technology_AdvancedCasting => "Moulage avancé";

        public override string Help_Title => "Aide";
        public override string Help_Work_Title => "Le travail ne se lance pas";
        public override string Help_Work_Resources => "Les bâtiments ont besoin de ressources disponibles";
        public override string Help_Work_Skill => "L'ouvrier doit être au bon niveau d'expérience (ou supérieur)";
        public override string Help_Work_Stockpile => "La récolte de ressources sera haltée si la réserve est pleine";
        public override string Help_Work_Priority => "La tâche peut avoir une priorité basse ou inexistante";


        public override string Help_Soldiers_Title => "Produit des soldats";
        public override string Help_Soldiers_PlaceBuildingX => "Placer le bâtiment: {0}";
        public override string Help_Soldiers_Workers => "Ouvriers disponibles pour le recrutement";
        public override string Help_Soldiers_Weapon => "Une arme pour chaque soldat";
        public override string Help_Soldiers_StartX => "Départ: {0}";


        public override string Hud_SelectHistory => "Sélectionner l'historique";

        public override string Hud_PointsPerMinute => "{0} points par minute";
        public override string Hud_PercentValueCost => "Le service coûte {0}% de la valeur";

        public override string Hud_Mixed => "Mixte";
        public override string Hud_Distance => "Distance";

        public override string Hud_Unlock => "Débloquer";
        public override string Hud_category => "Catégorie";

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public override string Input_StepOneFrame => "Une image à la fois";

        public override string Resource_TypeName_Wagon2Wheel => "Petit chariot";
        public override string Resource_TypeName_Wagon4Wheel => "Grand chariot";
        public override string Resource_TypeName_Tin => "Étain";
        public override string Resource_TypeName_TinOre => "Minerai d'étain";

        public override string Resource_TypeName_Copper => "Cuivre";
        public override string Resource_TypeName_CopperOre => "Minerai de cuivre";
        public override string Resource_TypeName_SilverOre => "Minerai d'argent";
        public override string Resource_TypeName_Silver => "Argent";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Mithril brut";
        public override string Resource_TypeName_Mithril => "Mithril";

        public override string Resource_TypeName_BronzeSword => "Épée de bronze";
        public override string Resource_TypeName_ShortSword => "Épée courte";
        public override string Resource_TypeName_LongSword => "Épée longue";
        public override string Resource_TypeName_HandSpear => "Lance de main";
        public override string Resource_TypeName_Warhammer => "Marteau de guerre";
        public override string Resource_TypeName_MithrilSword => "Épée de mithril";
        public override string Resource_TypeName_SlingShot => "Lance-pierres";
        public override string Resource_TypeName_ThrowingSpear => "Javelot";
        public override string Resource_TypeName_Crossbow => "Arbalète";
        public override string Resource_TypeName_MithrilBow => "Arc de mithril";

        public override string Resource_TypeName_CoolingFluid => "Liquide de refroidissement";
        public override string Resource_TypeName_Palisade => "Palissade";
        public override string Resource_TypeName_Toolkit => "Kit d'outils";

        public override string Resource_TypeName_Sulfur => "Souffre";
        public override string Resource_TypeName_LeadOre => "Minerai de plomb";
        public override string Resource_TypeName_Lead => "PLomb";
        public override string Resource_TypeName_Bronze => "Bronze";
        public override string Resource_TypeName_BloomIron => "Fer recomposé";
        public override string Resource_TypeName_Steel => "Acier";
        public override string Resource_TypeName_CastIron => "Fonte";

        public override string Resource_TypeName_BlackPowder => "Poudre noire";
        public override string Resource_TypeName_GunPowder => "Poudre à canon";
        public override string Resource_TypeName_LedBullet => "Balle";

        public override string Resource_TypeName_HandCannon => "Canon à main";
        public override string Resource_TypeName_HandCulverin => "Mousquet à main";
        public override string Resource_TypeName_Rifle => "Fusil";
        public override string Resource_TypeName_Blunderbuss => "Tromblon";

        public override string Resource_TypeName_Manuballista => "Ballista à main";
        public override string Resource_TypeName_Catapult => "Catapulte";
        public override string Resource_TypeName_BatteringRam => "Bélier";
        public override string Resource_TypeName_SiegeCannonBronze => "Basilique";
        public override string Resource_TypeName_ManCannonBronze => "Bombarde";
        public override string Resource_TypeName_SiegeCannonIron => "Artillerie";
        public override string Resource_TypeName_ManCannonIron => "Canon";

        public override string Resource_TypeName_PaddedArmor => "Armure matelassée";
        public override string Resource_TypeName_HeavyPaddedArmor => "Armure matelassée lourde";

        public override string Resource_TypeName_IronArmor => "Armure en mailles";
        public override string Resource_TypeName_HeavyIronArmor => "Armure en mailles lourdes";

        public override string Resource_TypeName_BronzeArmor => "Armure en bronze";

        public override string Resource_TypeName_LightPlateArmor => "Armure en plaques";
        public override string Resource_TypeName_FullPlateArmor => "Armure complète en plaques";
        public override string Resource_TypeName_MithrilArmor => "Armure en mithril";
        public override string Resource_TypeName_Coin => "Pièce";

        public override string UnitType_Warhammer => "Chevalier au marteau";
        //public override string UnitType_MithrilKnight => "Chevalier immortel";
        //public override string UnitType_MithrilArcher => "Archer immortel";
        public override string UnitType_SpearAndShield => "Infanterie de ligne";

        public override string UnitType_CollectionOfSoldiers => "Groupe de soldats";
        public override string UnitType_CollectionOfArmies => "Groupe d'armées";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => "(id {0})";

        public override string BuildHud_AreaEffectTitle => "Effet de zone";
        public override string BuildHud_BonusRadius => "Rayon bonus: {0}";

        public override string BuildHud_BuildTime => "Temps de construction";
        public override string SchoolHud_ToLevel => "Jusqu'au niveau";
        public override string SchoolHud_TimeDescription => "Temps pour zéro expérience. Réduit avec l'expérience.";
        public override string SchoolHud_SelectSchool => "Choisir école";
        public override string Upgrade_Order => "Ordre d'amélioration";

        public override string Building_ListDescription => "Une liste de tous les bâtiments dans cette catégorie";

        public override string BuildingType_IsUpgraded => "{0} - amélioré";
        public override string BuildingType_WoodCutter => "Scierie";
        public override string BuildingType_Workshop_Description => "Améliore le labeur dans la zone";

        public override string BuildingType_WoodCutter_AreaAffect => "Gagne {0}% de bois en plus des arbres";

        public override string BuildingType_StoneCutter_AreaAffect => "Gagne {0}% de pierre en plus";

        public override string BuildingType_StoneCutter => "Carrière de pierre";

        public override string BuildingType_Embassy => "Embassade";
        public override string BuildingType_Embassy_Description => "Pour les relations diplomatiques";

        public override string BuildingType_SoldierBarracks => "Garnison de soldats";
        public override string BuildingType_ArcherBarracks => "Garnison d'archers";
        public override string BuildingType_WarmachineBarracks => "Garnison d'engins";
        public override string BuildingType_GunBarracks => "Garnison d'armes à feu";
        public override string BuildingType_CannonBarracks => "Garnison de canons";
        public override string BuildingType_KnightsBarracks => "Garnison de chevaliers";

        public override string BuildingType_WaterResovoir => "Réservoir d'eau";
        public override string BuildingType_WaterResovoir_Description => "Augmente la réserve d'eau";

        public override string BuildingType_SmeltingFurnace => "Four de fonte";
        public override string BuildingType_SmeltingFurnace_Description => "Purifie le minerai en métal";

        public override string BuildingType_Foundry => "Fonderie";
        public override string BuildingType_Foundry_Description => "Station de moulage de métal";

        public override string BuildingType_Armory => "Armurerie";
        public override string BuildingType_Armory_Description => "Fabrique d'armures";
        public override string BuildingType_Chemist => "Chimiste";
        public override string BuildingType_Chemist_Description => "Fabrique d'agents chimiques";
        public override string BuildingType_CoinMaker => "Monnayeur";
        public override string BuildingType_CoinMaker_Description => "Transforme le métal en monnaie";
        public override string BuildingType_Gunmaker => "Artilleur";
        public override string BuildingType_Gunmaker_Description => "Fabrique d'armes à poudre et de canons";

        public override string BuildingType_School_Tab => "École";
        public override string BuildingType_School => "Guilde des maîtres";
        public override string BuildingType_School_Description => "Augmente le niveau d'expérience des ouvriers";

        public override string BuildingType_GoldDelivery => "Transporteur d'or";
        public override string BuildingType_Bank_Description => "Gestion de l'or";

        public override string DecorType_CobbleStones => "Pierres pavées";
        public override string DecorType_Square => "Place de la ville";

        public override string DecorType_Garden => "Jardin";
        public override string DecorType_Flag => "Drapeau";
        public override string DecorType_Banner => "Bannière";

        public override string BuildingType_DirtRoad => "Chemin en terre";
        public override string BuildingType_Palisade => "Palissade";

        public override string ResourceType_ServiceMen => "Hommes d'armes";
        public override string BuildingType_ServiceHouse => "Maison d'armes";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "Ajoute {0} hommes d'arme";

        public override string BuildingType_GuardOffice => "Bureau des gardes";
        public override string BuildingType_GuardOffice_DescriptionAddX => "Augmente la limite de gardes de {0}";

        public override string BuildingType_DirtWall => "Mur de terre";
        public override string BuildingType_DirtTower => "Tour de terre";
        public override string BuildingType_WoodWall => "Mur de bois";
        public override string BuildingType_WoodTower => "Tour de bois";
        public override string BuildingType_StoneWall => "Mur de pierre";
        public override string BuildingType_StoneTower => "Tour de pierre";
        public override string BuildingType_StoneGate => "Porte de pierre";
        public override string BuildingType_StoneHouse => "Maison de pierre";


        /// <summary>
        /// When listing slight variations, like "Lamp A" and "Lamp B"
        /// </summary>
        public override string VariantType_A => "{0} A";
        public override string VariantType_B => "{0} B";
        public override string VariantType_C => "{0} C";
        public override string VariantType_D => "{0} D";
        public override string VariantType_E => "{0} E";
        public override string VariantType_F => "{0} F";
        public override string VariantType_G => "{0} G";
        public override string VariantType_H => "{0} H";

        public override string BuildingToolShape_Free => "Enclos";
        public override string BuildingToolShape_Area => "Rectangle";
        public override string BuildingToolShape_Line => "Ligne";
        public override string BuildingToolShape_LShape => "L-shape";


        public override string CityHall_Upgrade => "Améliore l'hotel de ville";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Ouvriers max: {0}";

        public override string CityHall_Size_Small => "Village";
        public override string CityHall_Size_Medium => "Ville";
        public override string CityHall_Size_Large => "Capitale";

        public override string GuardHousingCount => "Habitations du bureau des gardes";
        public override string ServicemenCount => "Hommes d'armes: {0}";


        public override string Work_MiningResource => "Minage de {0}";

        public override string MenuTab_Progress => "Progrès";

        public override string Automation_AutomateCity => "Automatiser la ville";
        public override string Automation_AutomationFocus => "Focus de l'automation";
        public override string Automation_AutomationFocus_Grow => "Planter";
        public override string Automation_AutomationFocus_Export => "Exporter";
        public override string Automation_AutomationFocus_War => "Guerre";

        public override string CityCulture_Smelters_Description => "Fonte de métaux améliorée";
        public override string CityCulture_Smelters => "Fondeurs";

        public override string CityCulture_Apprentices_Description => "Les nouveaux ouvriers gagneront de l'expérience grace aux ouvriers actifs";
        public override string CityCulture_Apprentices => "Apprentis";

        public override string CityCulture_BronzeCasters_Description => "Production améliorée de bronze et d'objets en bronze";
        public override string CityCulture_BronzeCasters => "Mouleurs de bronze";

        //DEMO PATCH 1

        /// <summary>
        /// Evil orcs that roam on the map
        /// </summary>
        public override string FactionName_Barbarian => "Horde sombre";
        public override string Tutorial_AttackAndDestroyX => "Attaquer et détruire: {0}";
        public override string Resource_TypeName_Pike => "Pic";


        public override string BattleTrials_Title => "Entraînement au combat";
        public override string BattleTrials_Description => "Testez vos troupes dans un affrontement direct armée-contre-armée.";


        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "Ces attaques réduisent les chances de bloquer";

        public override string Conscript_BlockPerSecond => "Peut bloquer {0} fois par seconde";

        public override string Conscript_BlockDescription => "Les soldats bloqueront la majorité des attaques provenant de devant eux";

        public override string Map_CustomSeed => "Seed de la map";

        public override string Settings_Mode_Spectator => "Spectateur";

        //public override string Settings_Mode_Spectator_Description => "Regarder";

        public override string Automation_AutomationFocus_NoFocus_Description => "Construira un peu de tout";

        public override string Automation_AutomationFocus_WillProduce => "Produira principalement:";

        public override string Help_Food_WhoEats => "Tous les soldats et les ouvriers consomment de la nourriture";

        public override string Help_Food_BigArmy => "Une grande armée peut causer une pénurie de nourriture dans une ville adjacente";

        public override string Help_Food_DontBuild => "Construire plus de fermes n'augmente pas automatiquement la production de nourriture. Il faut des ouvriers et des cuisines disponibles pour récolter et transformer";

        public override string Help_Food_UseWater => "La production de nourriture requiert de l'eau";

        public override string Help_Food_Postal => "Assurez-vous que vos villes s'entraident en s'envoyant de la nourriture";

        public override string Message_LostCity => "Ville perdue";

        public override string Demo_Description => "Un scénario court: défendez votre ville pendant {0} minutes";


        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "La démo s'achèvera dans {0} minutes";

        public override string Experience_Required => "Experience requise";

        public override string InputActionName_ToggleMenu => "Activer le menu";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "Les ressources peuvent aller en-dessous de zéro, et dépasser légèrement le seuil maximum. Les limites ne sont appliquées que quand la file de travail est créee.";

        public override string Work_SelectCategory => "Choisir la catégorie d'objet";
        public override string Hud_RemoveFromList => "Retirer de la liste";

        public override string Hud_ReturnToPrevious => "Retour";
        public override string Hud_Close => "Fermer";

        public override string Hud_Low => "Bas";
        public override string Hud_Medium => "MOyen";
        public override string Hud_High => "Haut";

        public override string Hud_Copy => "Copier";
        //public override string Hud_Paste => "Paste";
        public override string Hud_Cut => "Couper";
        public override string Hud_SaveCompleted => "Sauvegarde terminée";

        public override string Settings_WaterMultiplier => "Multiplicateur d'eau";
        public override string Settings_WaterMultiplier_Description => "Détermine combien d'eau les villes peuvent produire et stocker. Une valeur élevée réduira les performances de l'ordinateur.";

        public override string Settings_ChildMultiplier => "Multiplicateur de naissances";
        //public override string Settings_CraftMultiplier => "Multiplicateur de vitesse de fabrication";
        public override string Settings_CraftMultiplier_Description => "Une valeur plus basse résulte en des fabrications plus rapides.";

        public override string FastProduction => "Prodution rapide";
        public override string SlowProduction => "Production lente";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "Ne produira pas";

        //public override string CityAutomation_WaitForMaxPopulation => "Wait for population to max out";
        public override string Automation_AutomationFocus_NoFocus => "Tout";
        public override string CityAutomation_SoldierQuality => "Qualité des soldats";
        public override string CityAutomation_SoldierWeaponType => "Type d'arme";

        public override string WarsResourceGroup_Resources => "Ressources";
        public override string WarsResourceGroup_Weapons => "Armes";

        public override string WarsResourceGroup_AllWeaponTypes => "Mixte";
        public override string WarsResourceGroup_MeleeHandWeapons => "Mélée";
        public override string WarsResourceGroup_RangedHandWeapons => "Distance";
        public override string WarsResourceGroup_Warmachines => "Engins";

        public override string FactionSettings_Titel => "Réglages de la faction";
        public override string FactionSettings_Description => "S'applique à toutes vos villes";

        public override string Conscript_MaxPopulation => "Population max";
        public override string Conscript_MaxPopulation_Description => "Ne recrute que quand la population atteint son maximum";

        public override string Conscript_FoodAbundance => "Réserve de nourriture max";
        public override string Conscript_FoodAbundance_Description => "Ne recrute que quand la réserve de nourriture atteint son maximum";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Régler: On";
        public override string GeneralSetting_Off => "Régler: Off";
        public override string GeneralSetting_AllBuildingsDescription => "S'applique à tous les bâtiments";

        public override string GeneralSetting_ApplyMessage => "Changements appliqués à {0} bâtiments";

        public override string MustTurnOffSteamInput => "Pour utiliser des manettes, vous devez désactiver les entrées Steam.";

        public override string Technology_GainTitle => "Moyens d'obtenir des technologies";
        public override string Technology_LevelUp => "Monter de niveau";
        public override string Technology_ForEachLevelUp => "Quand un ouvrier monte de niveau dans le domaine: {0}";

        public override string VoxelEditor_Description => "Créer des modèles cubiques";

        public override string Editor_Tool => "Outil";
        public override string Editor_SelectOptionsMenu => "Options de sélection";
        public override string Editor_Continous => "Continue"; // corrected spelling
        public override string Editor_Tool_PencilSize => "Taille de pinceau";
        public override string Editor_Tool_SizeTolerance => "Tolérance de taille";
        public override string Editor_Tool_RoundPencil => "Pinceau rond";
        public override string Editor_Tool_EdgeSize => "Taille du bord";
        public override string Editor_Tool_PercentFill => "Pourcentage de replissage";
        public override string Editor_Tool_ClearAbove => "Vider au dessus";
        public override string Editor_Tool_FillBelow => "Remplir en dessous";
        public override string Editor_UserModels => "Modèles utilisateur";
        public override string Editor_UserModels_Description => "Parcourir les modèles enregistrés";

        public override string Editor_RetailModels => "Modèles préfabriqués";
        public override string Editor_RetailModels_Description => "Charge les modèles du jeu";

        public override string Editor_ModTemplates => "Patrons de modding";
        public override string Editor_ExportAsOBJ => "Exporter en .OBJ";
        public override string Editor_SelectAll => "Tout sélectionner";

        public override string Editor_Canvas_Title => "Canvas";
        public override string Editor_Canvas_Size => "Taille";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "Tailles prédéfinies";
        public override string Editor_Canvas_Move => "Déplacer";
        public override string Editor_Canvas_Move_Up => "Haut";
        public override string Editor_Canvas_Move_Down => "Bas";
        public override string Editor_Canvas_RotateClockwise => "Pivoter horaire";
        public override string Editor_Canvas_RotateCounterClockwise => "Pivoter antihoraire"; // combined into one word
        public override string Editor_Canvas_Mirror => "Miroir";

        public override string Editor_Canvas_RotateFlip_Title => "Pivoter/Inverser";
        public override string Editor_Canvas_FlipVertical => "Inverse le haut et le bas";
        public override string Editor_Canvas_FlipOrientation => "Inverse horizontal/vertical";
        public override string Editor_Canvas_ClearAll_Description => "Retire tous les blocs et les cadres";

        public override string Editor_Animation => "Animation";
        public override string Editor_Animation_RemoveCurrentFrame => "Retire le cadre actuel";
        public override string Editor_Animation_AddFrameCopy => "Ajouter le cadre en copie";
        public override string Editor_Animation_AddEmptyFrame => "Ajouter cadre vide";
        public override string Editor_Animation_MoveDescription => "Changer la position du cadre";
        public override string Editor_Animation_AllFrames => "Tous les cadres";
        public override string Editor_Animation_AllFrames_ActionDescription => "Performe la même action sur tous les cadres";

        public override string Editor_SettingsMenu => "Paramètres";
        public override string Hud_Exit => "Quitter";
        public override string Editor_Canvas_Clear => "Vider";

        public override string Editor_Stamp => "Tampon";
        public override string Editor_StampOtherFrames => "Tamponne les autres cadres";
        public override string Editor_StampOtherFrames_Description => "Colle les voxels dans ces cadres"; // "this frames" → "these frames"
        public override string Editor_PasteToFrame => "Colle les voxels dans ce cadre";
        public override string Editor_ClearAllFrames => "Vider dans tous les cadres";
        public override string Editor_ClearOtherFrames => "Vider les autres cadres";

        public override string Editor_Settings_MoveSpeed => "Vitesse de déplacement";
        public override string Editor_Settings_BackgroundColor => "Couleur d'arrière-plan";
        public override string Editor_Settings_HideHUD => "Cacher l'HUD";

        public override string Editor_Color => "Couleur";
        public override string Editor_ColorsInUseLabel => "Couleurs utilisées";
        public override string Editor_Color_BrighterPlus => "Luminosité +";
        public override string Editor_Color_Brighter => "Lumineux";
        public override string Editor_Color_Darker => "Sombre";
        public override string Editor_Color_DarkerPlus => "Sombre +";
        public override string Editor_Color_RedTint => "Teinte rouge";
        public override string Editor_Color_Tint => "Teinte";
        public override string Editor_Color_GreenTint => "Teinte verte";
        public override string Editor_Color_BlueTint => "Teinte bleue";
        public override string Editor_Color_YellowTint => "Teinte jaune";
        public override string Editor_Color_PurpleTint => "Teinte violette";
        public override string Editor_NoColor => "Vide";

        public override string Editor_Material => "Matériel";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "Recolorer";
        public override string Editor_Color_RecolorTo => "Recolorer vers";

        public override string Editor_Material_Set => "Définir matériel";

        public override string Editor_Preview => "Prévisualiser";
        public override string Editor_CombineWithCurrent => "Combiner avec le modèle actuel";

        public override string Editor_PickedColor => "Sélection";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "Tente d'immigration";
        public override string BuildingType_ImmigrationTent_Description => "Contient {0} immigrants";
        public override string BuildingType_ReseachCenter => "Centre de recherche"; // fixed typo "Reseach"
        public override string BuildingType_Bookpress => "Presse à livres";
        public override string BuildingType_Bookpress_Description => "Dans un domaine de recherche, tous les points gagnés seront partagés avec tous les {0} dans vos autres villes.";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "Example: Quand un ouvrier produit {0}, il augmentera son niveau de {1}. En montant de niveau, il gagnera des points pour la technologie {2} puisqu'elle fait partie du domaine {1}."; // fixed "Reseach" and plural

        public override string BuildingType_Research_BaseDescription => "Augmente la recherche de technologies.";

        public override string BuildingType_ResearchCenter_Description => "Ajoute {0} points de technologie bonus quand un ouvrier monte de niveau dans ce domaine.";

        //DEMO PATCH 5

        public override string Editor_CropSelection => "Rogner la sélection";

        public override string Immigrants_DisbandedSoldiers => "Les soldats abandonnés migreront";
        public override string Immigrants_RefillWorkers => "Remplit rapidement la force ouvrière";
        public override string Immigrants_UnhousedAreLost => "Les immigrants sans logement disparaîtront après un temps";
        public override string Editor_VoxelCount => "{0} voxels";

        public override string Editor_Layers_Titel => "Couches";
        public override string Editor_Layers_All => "Toutes les couches";
        public override string Editor_LayerNumber => "Couche {0}";

        public override string Editor_Layer_AddEmpty => "Ajouter couche vide";
        public override string Editor_Layer_AddCopy => "Dupliquer la couche";
        public override string Editor_Layer_Remove => "Retirer la couche";
        public override string Editor_Layer_MergeDown => "Fusionner bas";
        public override string Editor_IsAnimated => "Animé";
        public override string Editor_ToggleVisible => "Active la visibilité";
        public override string Editor_ToggleAnimatedLayer => "Active la couche d'animation";
        public override string Editor_Projects => "Fichiers du projet";
        public override string ProfileEditor_ReplaceMaterial => "Couleur du profil: {0}";

        public override string ProfileEditor_ProfileColors_Label => "Couleurs du profil";
        public override string ProfileEditor_TunicColor => "Couleur de la tunique";
        public override string ProfileEditor_PantsColor => "Couleur du pantalon";
        public override string ProfileEditor_LeaderColor => "Couleur du dirigeant";

        public override string MapStartAs_Water => "Eau";
        public override string MapStartAs_Land => "Terre";
        public override string MapStartAs_Circle => "Cercle";

        public override string Hud_NeedToBeAssigned => "Besoin d'être assigné";
        public override string Hud_CommitAssignment => "Assigner";
        public override string Technology_NoAvailableResearch => "Pas de recherche disponible";

        public override string Research_Tab => "Recherche";

        //5.2
        public override string BuildCategory_General => "Général";
        public override string BuildCategory_Military => "Militaire";
        public override string BuildCategory_Decoration => "Décoration";
        public override string BuildCategory_Upgrade => "Amélioration";
        public override string Work_NoMines => "Pas de mines";

 
        //NEXT FEST DEMO
        public override string HUD_DisplayName => "Nom d'affichage";
        public override string HUD_Filter => "Filtre";
        public override string HUD_Scale => "Echelle";
        public override string HUD_Tags => "Tags";
        public override string HUD_ClickToCancel => "Cliquez pour annuler";

        public override string ObjectTag_Description => "Ajoute un symbole sur la carte";
        public override string HudPins => "Repères sur l'HUD";
        public override string HudPins_Description => "Accrocher des informations sur l'HUD";

        public override string Lobby_PlayerProfileNumbered => "Profil {0}";
        public override string Lobby_CharacterCreationNumbered => "Personnage {0}";
        public override string Lobby_PlayerProfileEdit => "Modifier le profil du joueur";

        public override string Editor_ConvertAnimationToLayers => "Convertir les animations en images";
        public override string Editor_StampAllFrames => "Tamponner sur toutes les images";

        public override string Editor_DisplayOptions => "Options d'affichage";
        public override string Editor_CharacterCreator => "Créateur de personnage";
        public override string Editor_CharacterCreator_Description => "Editeur d'apparence des modèles militaires";
        public override string Editor_HatGenre => "Mode d'affichage des chapeaux";
        public override string Editor_HatGenre_FollowWeapon => "Suivre l'arme";
        public override string Editor_HatGenre_Uniform => "Uniforme";
        public override string Editor_CopyPasteSelectedColor => "Copier la couleur sélectionnée";

        public override string Character_Accessories => "Accessoires";
        public override string Character_Hat => "Chapeau";
        public override string Character_Head => "Tête";
        public override string Character_Body => "Corps";
        public override string Character_Arms => "Bras";
        public override string Character_Back => "Dos";
        public override string Character_Face => "Visage";

        public override string BuildingType_Tavern => "Réfectoire";

        public override string Settings_CraftMultiplier => "Multiplicateur de temps de fabrication";
        public override string Settings_ChildMultiplier_Description => "Augmente la vitesse à laquelle les ouvriers sont produits";

        public override string Settings_CasualControls => "Contrôles pour joueur occasionnel";
        public override string Settings_CasualControls_Description => "Simplifie le jeu en réduisant les choix possibles.";

        public override string Settings_AdvancedControls => "Contrôles avancés";
        public override string Settings_AdvancedControls_Description => "L'expérience complète de gestion de ressources.";

        public override string WarsResourceGroup_Metal => "Métal";
        public override string Work_Craft => "Fabriquer";
        public override string Work_OnlyCraftOnFullStock => "Ne fabriquer que quand la réserve est pleine";

        public override string ExperienceType_Smelting => "Fonte";
        public override string Category_Optimize => "Optimiser";
        public override string BuildCategory_Road => "Route";
        public override string XP_UnlockBuildPrio => "Débloque la priorité de bâtiment: {0}";
        public override string Technology_ModernFarming => "Fermes modernes";

        public override string ExportImportDescription => "Pour partager les fichiers de sauvegardes avec d'autres joueurs, tous les fichiers sont dans ce dossier: {0}";

        public override string CityCultureDescription => "La culture donnera un bonus spécial à cette ville";

        public override string UnitType_CloseRangeRifle => "Arquebusier";
        public override string UnitType_LongRangeRifle => "Mousquetaire";
        public override string UnitType_Skirmisher => "Escarmoucheur";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "Archer lunari";
        public override string UnitType_MithrilSwordsman => "Chevalier lunari";

        public override string Defence_AutoAssign_Towers => "Assigner les tours";

        public override string EventMessage_DesertersText_Food => "Des soldats affamés désertent votre armée";

        public override string Tutorial_CasualRecruitSoldiers => "Acheter un groupe d'armées";


        //Shadow update
        public override string Technology_CannotReassign => "La Tech ne peut pas être réassignée tant que la recherche n’est pas terminée";
        public override string Diplomacy_DeclareWarAgainst => "Vous déclarez la guerre à";
        public override string Diplomacy_AllyCount => "Nombre d’alliés";
        public override string Diplomacy_CostPerAlly => "Le coût augmente de {0} par allié";

        public override string Event_ChanceOfFailure => "{0}% de chance d’échec";
        public override string EventMessage_Event_Title => "Événement";
        public override string EventMessage_TheCohalition => "La Coalition";

        public override string EventMessage_DarkHorde => "Horde Sombre";
        public override string EventMessage_DarkHordeKiller_Title => "Tueur de la Horde Sombre";
        public override string EventMessage_DarkHordeKiller_Message => "Des chevaliers champions ont rejoint votre service";

        public override string Settings_Mode_Spectator_Description => "Regardez seulement – ou intervenez avec les God Powers.";
        public override string GodPower => "God Power";

        public override string Building_TreeSprout_Description => "Planter un arbre";
        public override string Building_TreeSprout_Soft => "Jeune pousse de bois tendre";
        public override string Building_TreeSprout_Hard => "Jeune pousse de bois dur";

        public override string GeneralSetting_SetAll => "Appliquer à tout";

        public override string Hud_All => "Tous";

        public override string Hud_Previous => "Précédent";

        public override string Hud_EffectWillStack => "L’effet se cumule";

        public override string Info_WhenFoodRunsOut => "Quand la nourriture est épuisée, les villes et les armées l’achèteront automatiquement au marché noir.";

        //Launch test
        
        public override string InputActionName_NextWar => "Faction suivante en guerre";

        /// <summary>
        /// These symbols are needed to fit large numbers on the HUD,
        /// there will be a tooltip to explain what number it represents
        /// </summary>
        public override string EngineHud_SymbolFor100 => "c";
        public override string EngineHud_SymbolFor1000 => "k";
        public override string EngineHud_SymbolFor10000 => "10k";

        /// <summary>
        /// When loading files from other players, you won’t get their achievement progress
        /// </summary>
        public override string GameMenu_BlockImportAchievements => "Bloquer les succès sur les fichiers importés";

        public override string EndScreen_PeaceVictoryQuote => "Déposons nos épées et embrassons un avenir meilleur";

        public override string VictoryType_DefeatBoss => "Boss vaincu";
        public override string VictoryType_Domination => "Domination";
        public override string VictoryType_WorldPeace => "Paix mondiale";

    }
}
