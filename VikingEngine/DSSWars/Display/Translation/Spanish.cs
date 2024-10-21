using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    partial class Spanish : AbsLanguage
    {
        /// <summary>
        /// Nombre de este idioma
        /// </summary>
        public override string MyLanguage => "Español";

        /// <summary>
        /// Cómo mostrar una cantidad de elementos. 0: elemento, 1:Número
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}: {1}";

        /// <summary>
        /// Opción de seleccionar idioma
        /// </summary>
        public override string Lobby_Language => "Idioma";

        /// <summary>
        /// Comenzar a jugar el juego
        /// </summary>
        public override string Lobby_Start => "INICIAR";

        /// <summary>
        /// Botón para seleccionar el número de jugadores en multijugador local, 0: número actual de jugadores
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Multijugador local ({0})";

        /// <summary>
        /// Título del menú donde se selecciona el número de jugadores en pantalla dividida
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Seleccionar número de jugadores";

        /// <summary>
        /// Descripción para multijugador local
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "El multijugador requiere controladores de Xbox";

        /// <summary>
        /// Mover a la siguiente posición de pantalla dividida
        /// </summary>
        public override string Lobby_NextScreen => "Siguiente posición de pantalla";

        /// <summary>
        /// Los jugadores pueden seleccionar la apariencia visual y guardarla en un perfil
        /// </summary>
        public override string Lobby_FlagSelectTitle => "Seleccionar bandera";

        /// <summary>
        /// 0: Numeradas del 1 al 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Bandera {0}";

        /// <summary>
        /// Nombre del juego y número de versión
        /// </summary>
        public override string Lobby_GameVersion => "DSS war party - ver {0}";

        public override string FlagEditor_Description => "Pinta tu bandera y selecciona colores para tus soldados.";

        /// <summary>
        /// Herramienta de pintura que llena un área con un color
        /// </summary>
        public override string FlagEditor_Bucket => "Cubo";

        /// <summary>
        /// Abre el editor de perfil de la bandera
        /// </summary>
        public override string Lobby_FlagEdit => "Editar bandera";

        public override string Lobby_WarningTitle => "Advertencia";
        public override string Lobby_IgnoreWarning => "Ignorar advertencia";

        /// <summary>
        /// Advertencia cuando un jugador no tiene entrada seleccionada.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "Un jugador no tiene entrada";

        /// <summary>
        /// Menú con contenido que está fuera de lo que la mayoría de los jugadores usarán.
        /// </summary>
        public override string Lobby_Extra => "Extra";

        /// <summary>
        /// El contenido extra no está traducido ni tiene soporte completo para el controlador.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "¡Advertencia! Este contenido no está cubierto por la localización ni se espera soporte de entrada/accesibilidad";

        public override string Lobby_MapSizeTitle => "Tamaño del mapa";

        /// <summary>
        /// Nombre del tamaño de mapa 1
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "Minúsculo";

        /// <summary>
        /// Nombre del tamaño de mapa 2
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "Pequeño";

        /// <summary>
        /// Nombre del tamaño de mapa 3
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "Mediano";

        /// <summary>
        /// Nombre del tamaño de mapa 4
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "Grande";

        /// <summary>
        /// Nombre del tamaño de mapa 5
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "Enorme";

        /// <summary>
        /// Nombre del tamaño de mapa 6
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "Épico";

        /// <summary>
        /// Descripción del tamaño del mapa X por Y kilómetros. 0: Ancho, 1: Alto
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";

        /// <summary>
        /// Cerrar la aplicación del juego
        /// </summary>
        public override string Lobby_ExitGame => "Salir";

        /// <summary>
        /// Mostrar el nombre del jugador en multijugador local, 0: número del jugador
        /// </summary>
        public override string Player_DefaultName => "Jugador {0}";

        /// <summary>
        /// En el editor de perfil del jugador. Abre el menú con opciones del editor
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Opciones";

        /// <summary>
        /// En el editor de perfil del jugador. Título para seleccionar los colores de la bandera
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Colores de la bandera";

        /// <summary>
        /// En el editor de perfil del jugador. Opción de color de la bandera
        /// </summary>
        public override string ProfileEditor_MainColor => "Color principal";

        /// <summary>
        /// En el editor de perfil del jugador. Opción de color de la bandera
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Color de detalle 1";

        /// <summary>
        /// En el editor de perfil del jugador. Opción de color de la bandera
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Color de detalle 2";

        /// <summary>
        /// En el editor de perfil del jugador. Título para seleccionar los colores de los soldados
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "Personas";

        /// <summary>
        /// En el editor de perfil del jugador. Opción de color de los soldados
        /// </summary>
        public override string ProfileEditor_SkinColor => "Color de piel";

        /// <summary>
        /// En el editor de perfil del jugador. Opción de color de los soldados
        /// </summary>
        public override string ProfileEditor_HairColor => "Color de cabello";

        /// <summary>
        /// En el editor de perfil del jugador. Abrir paleta de colores y seleccionar color
        /// </summary>
        public override string ProfileEditor_PickColor => "Seleccionar color";

        /// <summary>
        /// En el editor de perfil del jugador. Ajustar la posición de la imagen
        /// </summary>
        public override string ProfileEditor_MoveImage => "Mover imagen";

        /// <summary>
        /// En el editor de perfil del jugador. Dirección de movimiento
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Izquierda";

        /// <summary>
        /// En el editor de perfil del jugador. Dirección de movimiento
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Derecha";

        /// <summary>
        /// En el editor de perfil del jugador. Dirección de movimiento
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Arriba";

        /// <summary>
        /// En el editor de perfil del jugador. Dirección de movimiento
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Abajo";

        /// <summary>
        /// En el editor de perfil del jugador. Cerrar el editor sin guardar
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Descartar y salir";

        /// <summary>
        /// En el editor de perfil del jugador. Tooltip para descartar
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Deshacer todos los cambios";

        /// <summary>
        /// En el editor de perfil del jugador. Guardar cambios y cerrar el editor
        /// </summary>
        public override string ProfileEditor_SaveAndExit => "Guardar y salir";

        /// <summary>
        /// En el editor de perfil del jugador. Parte de las opciones de color de Tono, Saturación y Luminosidad.
        /// </summary>
        public override string ProfileEditor_Hue => "Tono";

        /// <summary>
        /// En el editor de perfil del jugador. Parte de las opciones de color de Tono, Saturación y Luminosidad.
        /// </summary>
        public override string ProfileEditor_Lightness => "Luminosidad";

        /// <summary>
        /// En el editor de perfil del jugador. Moverse entre las opciones de color de la bandera y del soldado.
        /// </summary>
        public override string ProfileEditor_NextColorType => "Siguiente tipo de color";

        /// <summary>
        /// Velocidad actual del juego, comparada con el tiempo real
        /// </summary>
        public override string Hud_GameSpeedLabel => "Velocidad del juego: {0}x";

        public override string Input_GameSpeed => "Velocidad del juego";

        /// <summary>
        /// Pantalla del juego. Producción de oro por unidad
        /// </summary>
        public override string Hud_TotalIncome => "Ingresos totales/segundo: {0}";

        /// <summary>
        /// Costo de mantenimiento de la unidad.
        /// </summary>
        public override string Hud_Upkeep => "Mantenimiento: {0}";
        public override string Hud_ArmyUpkeep => "Mantenimiento del ejército: {0}";

        /// <summary>
        /// Pantalla del juego. Soldados protegiendo un edificio.
        /// </summary>
        public override string Hud_GuardCount => "Guardias";

        public override string Hud_IncreaseMaxGuardCount => "Tamaño máximo de guardias +{0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "Necesitas expandir la ciudad.";

        public override string Hud_SoldierCount => "Cantidad de soldados: {0}";

        public override string Hud_SoldierGroupsCount => "Cantidad de grupos: {0}";

        /// <summary>
        /// Pantalla del juego. Fuerza de batalla calculada de la unidad.
        /// </summary>
        public override string Hud_StrengthRating => "Valor de fuerza: {0}";

        /// <summary>
        /// Pantalla del juego. Fuerza de batalla calculada para toda la nación.
        /// </summary>
        public override string Hud_TotalStrengthRating => "Fuerza militar: {0}";

        /// <summary>
        /// Pantalla del juego. Hombres adicionales provenientes de fuera de la ciudad estado.
        /// </summary>
        public override string Hud_Immigrants => "Inmigrantes: {0}";

        public override string Hud_CityCount => "Cantidad de ciudades: {0}";
        public override string Hud_ArmyCount => "Cantidad de ejércitos: {0}";

        /// <summary>
        /// Botón pequeño para repetir una compra varias veces. Ej. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Requisito";
        public override string Hud_PurchaseTitle_Cost => "Costo";
        public override string Hud_PurchaseTitle_Gain => "Ganancia";

       

        /// <summary>
        /// Cuánto de un recurso se utilizará, "5 oro. (Disponible: 10)". Habrá un título de "costo" encima del texto. 0: Recurso, 1: costo, 2: disponible
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Disponible: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "El costo aumentará en {0}";

        public override string Hud_Purchase_MaxCapacity => "Ha alcanzado la capacidad máxima";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Fuerza: Tu {0} - Su {1}";

        /// <summary>
        /// Mostrar una cadena corta de la fecha como Año, Mes, Día
        /// </summary>
        public override string Hud_Date => "A{0} M{1} D{2}";

        /// <summary>
        /// Mostrar una cadena corta de tiempo como Hora, Minutos, Segundos
        /// </summary>
        public override string Hud_TimeSpan => "H{0} M{1} S{2}";
        /// <summary>
        /// Batalla entre dos ejércitos o entre un ejército y una ciudad
        /// </summary>
        public override string Hud_Battle => "Batalla";

        /// <summary>
        /// Describe la entrada del botón. Mover a la siguiente ciudad.
        /// </summary>
        public override string Input_NextCity => "Siguiente ciudad";

        /// <summary>
        /// Describe la entrada del botón. Mover al siguiente ejército.
        /// </summary>
        public override string Input_NextArmy => "Siguiente ejército";

        /// <summary>
        /// Describe la entrada del botón. Mover a la siguiente batalla.
        /// </summary>
        public override string Input_NextBattle => "Siguiente batalla";

        /// <summary>
        /// Describe la entrada del botón. Pausar.
        /// </summary>
        public override string Input_Pause => "Pausar";

        /// <summary>
        /// Describe la entrada del botón. Reanudar desde la pausa.
        /// </summary>
        public override string Input_ResumePaused => "Reanudar";

        /// <summary>
        /// Recurso genérico de dinero
        /// </summary>
        public override string ResourceType_Gold => "Oro";

        /// <summary>
        /// Recurso de hombres trabajadores
        /// </summary>
        public override string ResourceType_Workers => "Trabajadores";

        public override string ResourceType_Workers_Description => "Los trabajadores proporcionan ingresos y son reclutados como soldados para tus ejércitos";

        /// <summary>
        /// El recurso utilizado en diplomacia
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Puntos de diplomacia";

        /// <summary>
        /// 0: Cuántos puntos tienes, 1: Valor máximo suave (aumentará mucho más lento después de esto), 2: Límite máximo
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Puntos diplomáticos: {0} / {1} ({2})";

        /// <summary>
        /// Tipo de edificio de la ciudad. Edificio para caballeros y diplomáticos.
        /// </summary>
        public override string Building_NobleHouse => "Casa noble";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "1 punto de diplomacia por cada {0} segundos";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} al límite máximo de puntos de diplomacia";
        public override string Building_NobleHouse_UnlocksKnight => "Desbloquea la unidad de Caballero";

        public override string Building_BuildAction => "Construir";
        public override string Building_IsBuilt => "Construido";

        /// <summary>
        /// Tipo de edificio de la ciudad. Producción masiva maligna.
        /// </summary>
        public override string Building_DarkFactory => "Fábrica oscura";

        /// <summary>
        /// Menú de configuración del juego. Suma todas las opciones de dificultad en porcentaje.
        /// </summary>
        public override string Settings_TotalDifficulty => "Dificultad total {0}%";

        /// <summary>
        /// Menú de configuración del juego. Opción de dificultad base.
        /// </summary>
        public override string Settings_DifficultyLevel => "Nivel de dificultad {0}%";

        /// <summary>
        /// Menú de configuración del juego. Opción para crear nuevos mapas en lugar de cargar uno.
        /// </summary>
        public override string Settings_GenerateMaps => "Generar nuevos mapas";

        /// <summary>
        /// Menú de configuración del juego. Crear nuevos mapas tiene un tiempo de carga más largo.
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Generar es más lento que cargar los mapas preconstruidos";

        /// <summary>
        /// Menú de configuración del juego. Opción de dificultad. Bloquear la capacidad de jugar mientras está en pausa.
        /// </summary>
        public override string Settings_AllowPause => "Permitir pausa y comandos";

        /// <summary>
        /// Menú de configuración del juego. Opción de dificultad. Tener jefes que entren en el juego.
        /// </summary>
        public override string Settings_BossEvents => "Eventos de jefes";

        /// <summary>
        /// Menú de configuración del juego. Opción de dificultad. Sin descripción de jefes.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Desactivar los eventos de jefes pondrá el juego en modo sandbox sin final.";


        /// <summary>
        /// Opciones para automatizar las mecánicas del juego. Título del menú.
        /// </summary>
        public override string Automation_Title => "Automatización";
        /// <summary>
        /// Opciones para automatizar las mecánicas del juego. Información sobre cómo funciona la automatización.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Esperará a que la fuerza laboral se maximice";
        /// <summary>
        /// Opciones para automatizar las mecánicas del juego. Información sobre cómo funciona la automatización.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Se pausará si los ingresos son negativos";
        /// <summary>
        /// Opciones para automatizar las mecánicas del juego. Información sobre cómo funciona la automatización.
        /// </summary>
        public override string Automation_InfoLine_Priority => "Las ciudades grandes tienen prioridad";
        /// <summary>
        /// Opciones para automatizar las mecánicas del juego. Información sobre cómo funciona la automatización.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Realiza un máximo de una compra por segundo";


        /// <summary>
        /// Leyenda del botón para la acción. Un edificio especializado para caballeros y diplomáticos.
        /// </summary>
        public override string HudAction_BuyItem => "Comprar {0}";

        /// <summary>
        /// El estado de paz o guerra entre dos naciones
        /// </summary>
        public override string Diplomacy_RelationType => "Relación";

        /// <summary>
        /// Título para la lista de relaciones que otras facciones tienen entre sí
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Sus relaciones con otros";

        /// <summary>
        /// Relación diplomática. Estás en control directo de los recursos de la nación.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Siervo";

        /// <summary>
        /// Relación diplomática. Cooperación total.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Aliado";

        /// <summary>
        /// Relación diplomática. Menor probabilidad de guerra.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "Buena";

        /// <summary>
        /// Relación diplomática. Acuerdo de paz.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Paz";

        /// <summary>
        /// Relación diplomática. Aún no han hecho contacto.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Neutral";

        /// <summary>
        /// Relación diplomática. Acuerdo de paz temporal.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Tregua";

        /// <summary>
        /// Relación diplomática. Guerra.
        /// </summary>
        public override string Diplomacy_RelationType_War => "Guerra";

        /// <summary>
        /// Relación diplomática. Guerra sin posibilidad de paz.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Guerra total";

        /// <summary>
        /// Comunicación diplomática. Qué tan bien puedes discutir términos. 0: Términos
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "Términos: {0}";

        /// <summary>
        /// Comunicación diplomática. Mejor de lo normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "Bueno";

        /// <summary>
        /// Comunicación diplomática. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Normal";

        /// <summary>
        /// Comunicación diplomática. Peor de lo normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Malo";

        /// <summary>
        /// Comunicación diplomática. No se comunicarán.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "Ninguno";

        /// <summary>
        /// Acción diplomática. Forjar una nueva relación diplomática.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Forjar relaciones con: {0}";

        /// <summary>
        /// Acción diplomática. Sugerir una nueva relación diplomática.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Ofrecer paz";

        /// <summary>
        /// Acción diplomática. Sugerir una nueva relación diplomática.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Ofrecer alianza";

        /// <summary>
        /// Título diplomático. Otro jugador sugiere una nueva relación diplomática. 0: nombre del jugador
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} ofrece nuevas relaciones";

        /// <summary>
        /// Acción diplomática. Aceptar una nueva relación diplomática.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Aceptar nueva relación";

        /// <summary>
        /// Descripción diplomática. Otro jugador sugiere una nueva relación diplomática. 0: tipo de relación
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "Nueva relación ofrecida: {0}";

        /// <summary>
        /// Acción diplomática. Hacer que otra nación te sirva.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Absorber como siervo";

        /// <summary>
        /// Descripción diplomática. Está en contra del mal.
        /// </summary>
        public override string Diplomacy_LightSide => "Es aliado del lado luminoso";

        /// <summary>
        /// Descripción diplomática. Cuánto durará la tregua.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "Termina en {0} segundos";

        /// <summary>
        /// Acción diplomática. Hacer que la tregua dure más tiempo.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Extender tregua";

        /// <summary>
        /// Descripción diplomática. Cuánto se extenderá la tregua.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Extiende la tregua por {0} segundos";

        /// <summary>
        /// Descripción diplomática. Ir en contra de una relación acordada costará puntos de diplomacia.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "Romper la relación costará {0} puntos de diplomacia";

        /// <summary>
        /// Descripción diplomática para aliados.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Los aliados comparten las declaraciones de guerra.";

        /// <summary>
        /// Descripción diplomática para una buena relación.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Limita la capacidad de declarar guerra.";

        /// <summary>
        /// Descripción diplomática. Debes tener una fuerza militar mayor que tu siervo (otra nación que controlarás).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "Poder militar {0} veces más fuerte";

        /// <summary>
        /// Descripción diplomática. El siervo debe estar en una guerra desesperada (otra nación que controlarás).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "El siervo debe estar en guerra contra un enemigo más fuerte";

        /// <summary>
        /// Descripción diplomática. Un siervo no puede tener demasiadas ciudades (otra nación que controlarás).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "El siervo puede tener un máximo de {0} ciudades";

        /// <summary>
        /// Descripción diplomática. El costo en puntos de diplomacia aumentará (otra nación que controlarás).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "El precio aumentará por cada siervo";

        /// <summary>
        /// Descripción diplomática. El resultado de la relación con el siervo, toma de control pacífica de otra nación.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Absorber la otra facción";

        /// <summary>
        /// Mensaje cuando recibes una declaración de guerra
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "¡Guerra declarada!";

        /// <summary>
        /// El temporizador de la tregua ha terminado y vuelves a la guerra
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "La tregua ha terminado";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Título de la visualización.
        /// </summary>
        public override string EndGameStatistics_Title => "Estadísticas";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Tiempo total transcurrido en el juego.
        /// </summary>
        public override string EndGameStatistics_Time => "Tiempo en el juego: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Cuántos soldados compraste.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "Soldados reclutados: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Conteo de tus soldados que murieron en batalla.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Soldados perdidos en batalla: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Conteo de soldados enemigos que mataste en batalla.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Soldados enemigos muertos en batalla: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Conteo de tus soldados que te han abandonado.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Soldados desertados: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Conteo de ciudades ganadas en batalla.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "Ciudades capturadas: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Conteo de ciudades perdidas en batalla.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "Ciudades perdidas: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Conteo de batallas ganadas.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Batallas ganadas: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Conteo de batallas perdidas.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Batallas perdidas: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Diplomacia. Declaraciones de guerra hechas por ti.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "Declaraciones de guerra hechas: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Diplomacia. Declaraciones de guerra hechas hacia ti.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "Declaraciones de guerra recibidas: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Alianzas hechas a través de la diplomacia.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Alianzas diplomáticas: {0}";

        /// <summary>
        /// Estadísticas que se muestran en la pantalla de fin de juego. Siervos hechos a través de la diplomacia. Las ciudades y ejércitos de los siervos se convierten en tuyos.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Siervos diplomáticos: {0}";

        /// <summary>
        /// Tipo de unidad colectiva en el mapa. Ejército de soldados.
        /// </summary>
        public override string UnitType_Army => "Ejército";

        /// <summary>
        /// Tipo de unidad colectiva en el mapa. Grupo de soldados.
        /// </summary>
        public override string UnitType_SoldierGroup => "Grupo";

        /// <summary>
        /// Tipo de unidad colectiva en el mapa. Nombre común para pueblo o ciudad.
        /// </summary>
        public override string UnitType_City => "Ciudad";

        /// <summary>
        /// Selección de un grupo de ejércitos
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Grupo de ejércitos, cantidad: {0}";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Soldado de línea estándar.
        /// </summary>
        public override string UnitType_Soldier => "Soldado";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Soldado de batalla naval.
        /// </summary>
        public override string UnitType_Sailor => "Marinero";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Campesinos reclutados.
        /// </summary>
        public override string UnitType_Folkman => "Campesino";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Unidad de escudo y lanza.
        /// </summary>
        public override string UnitType_Spearman => "Lancero";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Fuerza de élite, parte de la guardia del Rey.
        /// </summary>
        public override string UnitType_HonorGuard => "Guardia de Honor";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Anticaballería, lleva lanzas largas de dos manos.
        /// </summary>
        public override string UnitType_Pikeman => "Piquero";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Unidad de caballería blindada.
        /// </summary>
        public override string UnitType_Knight => "Caballero";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Arco y flecha.
        /// </summary>
        public override string UnitType_Archer => "Arquero";

        /// <summary>
        /// Nombre para un tipo especializado de soldado.
        /// </summary>
        public override string UnitType_Crossbow => "Ballestero";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Máquina de guerra que lanza grandes lanzas.
        /// </summary>
        public override string UnitType_Ballista => "Balista";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Un troll de fantasía con un cañón.
        /// </summary>
        public override string UnitType_Trollcannon => "Trollcañón";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Soldado del bosque.
        /// </summary>
        public override string UnitType_GreenSoldier => "Soldado Verde";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Unidad naval del norte.
        /// </summary>
        public override string UnitType_Viking => "Vikingo";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. El maestro malvado.
        /// </summary>
        public override string UnitType_DarkLord => "Señor Oscuro";

        /// <summary>
        /// Nombre para un tipo especializado de soldado. Soldado que lleva una gran bandera.
        /// </summary>
        public override string UnitType_Bannerman => "Abanderado";

        /// <summary>
        /// Nombre para una unidad militar. Barco de guerra que transporta soldados. 0: tipo de unidad que transporta
        /// </summary>
        public override string UnitType_WarshipWithUnit => "Barco de guerra con {0}";

        public override string UnitType_Description_Soldier => "Una unidad de propósito general.";
        public override string UnitType_Description_Sailor => "Fuerte en la guerra naval";
        public override string UnitType_Description_Folkman => "Soldados baratos y sin entrenamiento";
        public override string UnitType_Description_HonorGuard => "Soldados de élite sin costo de mantenimiento";
        public override string UnitType_Description_Knight => "Fuerte en batallas a campo abierto";
        public override string UnitType_Description_Archer => "Solo fuerte cuando está protegido.";
        public override string UnitType_Description_Crossbow => "Soldado de rango poderoso";
        public override string UnitType_Description_Ballista => "Fuerte contra ciudades";
        public override string UnitType_Description_GreenSoldier => "Temido guerrero elfo";
        public override string UnitType_Description_DarkLord => "El jefe final";
        /// <summary>
        /// Información sobre un tipo de soldado
        /// </summary>
        public override string SoldierStats_Title => "Estadísticas por unidad";

        /// <summary>
        /// Cuántos grupos de soldados
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} grupos, un total de {1} unidades";

        /// <summary>
        /// Los soldados tendrán diferentes fortalezas dependiendo si atacan en campo abierto, desde barcos o atacando un asentamiento
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Fuerza de ataque: Tierra {0} | Mar {1} | Ciudad {2}";

        /// <summary>
        /// Cuántas heridas puede soportar un soldado
        /// </summary>
        public override string SoldierStats_Health => "Salud: {0}";

        /// <summary>
        /// Algunos soldados aumentarán la velocidad de movimiento del ejército
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Bono de velocidad del ejército en tierra: {0}";

        /// <summary>
        /// Algunos soldados aumentarán la velocidad de movimiento del barco
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Bono de velocidad del ejército en el mar: {0}";

        /// <summary>
        /// Los soldados comprados comenzarán como reclutas y completarán su entrenamiento después de unos minutos.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Tiempo de entrenamiento: {0} minutos. Será el doble de rápido si los reclutas están adyacentes a una ciudad.";

        /// <summary>
        /// Opción de menú para controlar un ejército. Hacer que dejen de moverse.
        /// </summary>
        public override string ArmyOption_Halt => "Detener";

        /// <summary>
        /// Opción de menú para controlar un ejército. Eliminar soldados.
        /// </summary>
        public override string ArmyOption_Disband => "Disolver unidades";

        /// <summary>
        /// Opción de menú para controlar un ejército. Opciones para enviar soldados entre ejércitos.
        /// </summary>
        public override string ArmyOption_Divide => "Dividir ejército";

        /// <summary>
        /// Opción de menú para controlar un ejército. Eliminar soldados.
        /// </summary>
        public override string ArmyOption_RemoveX => "Eliminar {0}";

        /// <summary>
        /// Opción de menú para controlar un ejército. Eliminar soldados.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Disolver todos";

        /// <summary>
        /// Opción de menú para controlar un ejército. 0: Cantidad, 1: Tipo de unidad
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} grupos: {0}";

        /// <summary>
        /// Opción de menú para controlar un ejército. Opciones para enviar soldados entre ejércitos.
        /// </summary>
        public override string ArmyOption_SendToX => "Enviar unidades a {0}";

        public override string ArmyOption_MergeAllArmies => "Fusionar todos los ejércitos";

        /// <summary>
        /// Opción de menú para controlar un ejército. Opciones para enviar soldados entre ejércitos.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Dividir unidades a un nuevo ejército";

        /// <summary>
        /// Opción de menú para controlar un ejército. Opciones para enviar soldados entre ejércitos.
        /// </summary>
        public override string ArmyOption_SendX => "Enviar {0}";

        /// <summary>
        /// Opción de menú para controlar un ejército. Opciones para enviar soldados entre ejércitos.
        /// </summary>
        public override string ArmyOption_SendAll => "Enviar todos";

        /// <summary>
        /// Opción de menú para controlar un ejército. Opciones para enviar soldados entre ejércitos.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Dividir ejército a la mitad";

        /// <summary>
        /// Opción de menú para controlar un ejército. Opciones para enviar soldados entre ejércitos.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Fusionar ejércitos";

        /// <summary>
        /// Reclutar soldados.
        /// </summary>
        public override string UnitType_Recruit => "Reclutar";

        /// <summary>
        /// Reclutar soldados de tipo. 0:tipo
        /// </summary>
        public override string CityOption_RecruitType => "Reclutar {0}";

        /// <summary>
        /// Número de mercenarios pagados
        /// </summary>
        public override string CityOption_XMercenaries => "Mercenarios: {0}";

        /// <summary>
        /// Indica el número de mercenarios disponibles para contratar en el mercado
        /// </summary>
        public override string Hud_MercenaryMarket => "Mercenarios disponibles para contratar";

        /// <summary>
        /// Comprar un número de mercenarios pagados
        /// </summary>
        public override string CityOption_BuyXMercenaries => "Importar {0} mercenarios";

        public override string CityOption_Mercenaries_Description => "Los soldados serán reclutados de mercenarios en lugar de tu fuerza laboral";

        /// <summary>
        /// Leyenda del botón para la acción. Crear viviendas para más trabajadores.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "Expandir fuerza laboral";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "Máxima fuerza laboral +{0}";
        public override string CityOption_ExpandGuardSize => "Expandir guardia";

        public override string CityOption_Damages => "Daños: {0}";
        public override string CityOption_Repair => "Reparar daños";
        public override string CityOption_RepairGain => "Reparar {0} daños";

        public override string CityOption_Repair_Description => "Los daños reducen el número de trabajadores que puedes tener.";

        public override string CityOption_BurnItDown => "Quemarlo todo";
        public override string CityOption_BurnItDown_Description => "Eliminar la fuerza laboral y aplicar daños máximos";

        /// <summary>
        /// El jefe principal. Nombrado por una piedra de metal brillante incrustada en su frente.
        /// </summary>
        public override string FactionName_DarkLord => "Ojo de la Perdición";

        /// <summary>
        /// Facción inspirada en los orcos. Trabaja para el señor oscuro.
        /// </summary>
        public override string FactionName_DarkFollower => "Siervos del Terror";

        /// <summary>
        /// La facción más grande, el antiguo pero corrupto reino.
        /// </summary>
        public override string FactionName_UnitedKingdom => "Reinos Unidos";

        /// <summary>
        /// Facción inspirada en los elfos. Vive en armonía con el bosque.
        /// </summary>
        public override string FactionName_Greenwood => "Bosqueverde";

        /// <summary>
        /// Facción con sabor asiático al este
        /// </summary>
        public override string FactionName_EasternEmpire => "Imperio Oriental";

        /// <summary>
        /// Reino vikingo en el norte. El más grande.
        /// </summary>
        public override string FactionName_NordicRealm => "Reinos Nórdicos";

        /// <summary>
        /// Reino vikingo en el norte. Usa un símbolo de garra de oso.
        /// </summary>
        public override string FactionName_BearClaw => "Garra de Oso";

        /// <summary>
        /// Reino vikingo en el norte. Usa un símbolo de gallo.
        /// </summary>
        public override string FactionName_NordicSpur => "Espuela Nórdica";

        /// <summary>
        /// Reino vikingo en el norte. Usa un símbolo de cuervo negro.
        /// </summary>
        public override string FactionName_IceRaven => "Cuervo de Hielo";

        /// <summary>
        /// Facción famosa por matar dragones con poderosas balistas.
        /// </summary>
        public override string FactionName_Dragonslayer => "Matadragones";

        /// <summary>
        /// Una unidad mercenaria del sur. Con sabor árabe.
        /// </summary>
        public override string FactionName_SouthHara => "Sur Hara";

        /// <summary>
        /// Nombre para naciones neutrales controladas por la CPU
        /// </summary>
        public override string FactionName_GenericAi => "IA {0}";

        /// <summary>
        /// Nombre para los jugadores y sus números
        /// </summary>
        public override string FactionName_Player => "Jugador {0}";

        /// <summary>
        /// Mensaje cuando se aproxima un miniboss en barcos desde el sur.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "¡Enemigo acercándose!";
        public override string EventMessage_HaraMercenaryText => "Se han avistado mercenarios de Hara en el sur";

        /// <summary>
        /// Primera advertencia de que el jefe principal aparecerá.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "Una oscura profecía";
        public override string EventMessage_ProphesyText => "¡El Ojo de la Perdición aparecerá pronto, y tus enemigos se unirán a él!";

        /// <summary>
        /// Segunda advertencia de que el jefe principal aparecerá.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Tiempos oscuros";
        public override string EventMessage_FinalBossEnterText => "¡El Ojo de la Perdición ha entrado en el mapa!";

        /// <summary>
        /// Mensaje cuando el jefe principal se encontrará contigo en el campo de batalla.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "Un ataque desesperado";
        public override string EventMessage_FinalBattleText => "El señor oscuro se ha unido al campo de batalla. ¡Ahora es tu oportunidad de destruirlo!";

        /// <summary>
        /// Mensaje cuando los soldados abandonan el ejército porque no puedes pagar su mantenimiento
        /// </summary>
        public override string EventMessage_DesertersTitle => "¡Desertores!";
        public override string EventMessage_DesertersText => "Los soldados no pagados están desertando de tus ejércitos";


        public override string DifficultyDescription_AiAggression => "Agresividad de la IA: {0}.";
        public override string DifficultyDescription_BossSize => "Tamaño del jefe: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Tiempo de aparición del jefe: {0}.";
        public override string DifficultyDescription_AiEconomy => "Economía de la IA: {0}%.";
        public override string DifficultyDescription_AiDelay => "Retraso de la IA: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Dificultad diplomática: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Costo de mercenarios: {0}.";
        public override string DifficultyDescription_HonorGuards => "Guardias de honor: {0}.";

        /// <summary>
        /// El juego ha terminado con éxito.
        /// </summary>
        public override string EndScreen_VictoryTitle => "¡Victoria!";

        /// <summary>
        /// Citas del personaje líder que juegas en el juego
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
{
    "En tiempos de paz, lloramos a los muertos.",
    "Cada triunfo lleva una sombra de sacrificio.",
    "Recuerda el viaje que nos trajo aquí, salpicado con las almas de los valientes.",
    "Nuestras mentes son luz por la victoria, nuestros corazones son pesados por el peso de los caídos."
};

        public override string EndScreen_DominationVictoryQuote => "¡Fui elegido por los dioses para dominar el mundo!";

        /// <summary>
        /// El juego ha terminado en fracaso.
        /// </summary>
        public override string EndScreen_FailTitle => "¡Fracaso!";

        /// <summary>
        /// Citas del personaje líder que juegas en el juego
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
{
    "Con nuestros cuerpos desgarrados por las marchas y noches de preocupación, damos la bienvenida al final.",
    "La derrota puede oscurecer nuestras tierras, pero no puede extinguir la luz de nuestra determinación.",
    "Apagad las llamas en nuestros corazones, de sus cenizas, nuestros hijos forjarán un nuevo amanecer.",
    "Que nuestras historias sean la brasa que encienda la victoria de mañana.",
};

        /// <summary>
        /// Una pequeña escena al final del juego
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Ver epílogo";

        /// <summary>
        /// Título de la escena
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Epílogo";

        /// <summary>
        /// Introducción de la escena
        /// </summary>
        public override string EndScreen_Epilogue_Text => "Hace 160 años";

        /// <summary>
        /// El prólogo es un poema corto sobre la historia del juego
        /// </summary>
        public override string GameMenu_WatchPrologue => "Ver prólogo";

        public override string Prologue_Title => "Prólogo";

        /// <summary>
        /// El poema debe tener tres líneas, la cuarta línea se extraerá de las traducciones de nombres para presentar el nombre del jefe
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
{
    "Los sueños te atormentan por la noche,",
    "Una profecía de un futuro oscuro",
    "Prepárate para su llegada,",
};

        /// <summary>
        /// Menú del juego cuando se pausa
        /// </summary>
        public override string GameMenu_Title => "Menú del juego";

        /// <summary>
        /// Continuar jugando después de la pantalla final
        /// </summary>
        public override string GameMenu_ContinueGame => "Continuar";

        /// <summary>
        /// Continuar jugando
        /// </summary>
        public override string GameMenu_Resume => "Reanudar";

        /// <summary>
        /// Salir al lobby del juego
        /// </summary>
        public override string GameMenu_ExitGame => "Salir del juego";

        public override string GameMenu_SaveState => "Guardar";
        public override string GameMenu_SaveStateWarnings => "¡Advertencia! Los archivos de guardado se perderán cuando se actualice el juego.";
        public override string GameMenu_LoadState => "Cargar";
        public override string GameMenu_ContinueFromSave => "Continuar desde guardado";

        public override string GameMenu_AutoSave => "Guardado automático";

        public override string GameMenu_Load_PlayerCountError => "Debes configurar una cantidad de jugadores que coincida con el archivo de guardado: {0}";

        public override string Progressbar_MapLoadingState => "Cargando mapa: {0}";

        public override string Progressbar_ProgressComplete => "completo";

        /// <summary>
        /// 0: progreso en porcentaje, 1: cantidad de fallos
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Generando: {0}%. (Fallos {1})";

        /// <summary>
        /// 0: parte actual, 1: número de partes
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "parte {0}/{1}";

        /// <summary>
        /// 0: Porcentaje o Completo
        /// </summary>
        public override string Progressbar_SaveProgress => "Guardando: {0}";

        /// <summary>
        /// 0: Porcentaje o Completo
        /// </summary>
        public override string Progressbar_LoadProgress => "Cargando: {0}";

        /// <summary>
        /// Progreso terminado, esperando entrada del jugador
        /// </summary>
        public override string Progressbar_PressAnyKey => "Presiona cualquier tecla para continuar";

        /// <summary>
        /// Un corto tutorial donde se supone que debes comprar y mover un soldado. Todos los controles avanzados están bloqueados hasta que se complete el tutorial.
        /// </summary>
        public override string Tutorial_MenuOption => "Ejecutar tutorial";
        public override string Tutorial_MissionsTitle => "Misiones del tutorial";
        public override string Tutorial_Mission_BuySoldier => "Selecciona una ciudad y recluta un soldado";
        public override string Tutorial_Mission_MoveArmy => "Selecciona un ejército y muévelo";

        public override string Tutorial_CompleteTitle => "¡Tutorial completado!";
        public override string Tutorial_CompleteMessage => "Zoom completo y opciones avanzadas de juego desbloqueadas.";

        /// <summary>
        /// Muestra la entrada del botón
        /// </summary>
        public override string Tutorial_SelectInput => "Seleccionar";
        public override string Tutorial_MoveInput => "Comando de movimiento";

        /// <summary>
        /// Versus. Texto que describe los dos ejércitos que entrarán en batalla
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "Declaración de guerra";

        public override string ArmyOption_Attack => "Atacar";

        /// <summary>
        /// Menú de configuración del juego. Cambia lo que hacen las teclas y botones cuando se presionan
        /// </summary>
        public override string Settings_ButtonMapping => "Asignación de botones";

        /// <summary>
        /// Describe la entrada del botón. Expande o reduce la cantidad de información en el HUD
        /// </summary>
        public override string Input_ToggleHudDetail => "Alternar detalle del HUD";

        /// <summary>
        /// Describe la entrada del botón. Alterna la selección entre el mapa y el HUD
        /// </summary>
        public override string Input_ToggleHudFocus => "Enfoque del menú";

        /// <summary>
        /// Describe la entrada del botón. Atajo para hacer clic en la última ventana emergente
        /// </summary>
        public override string Input_ClickMessage => "Mensaje de clic";

        /// <summary>
        /// Describe la entrada del botón. Dirección general de movimiento
        /// </summary>
        public override string Input_Up => "Arriba";

        /// <summary>
        /// Describe la entrada del botón. Dirección general de movimiento
        /// </summary>
        public override string Input_Down => "Abajo";

        /// <summary>
        /// Describe la entrada del botón. Dirección general de movimiento
        /// </summary>
        public override string Input_Left => "Izquierda";

        /// <summary>
        /// Describe la entrada del botón. Dirección general de movimiento
        /// </summary>
        public override string Input_Right => "Derecha";

        /// <summary>
        /// Tipo de entrada, entrada estándar de PC
        /// </summary>
        public override string Input_Source_Keyboard => "Teclado y ratón";

        /// <summary>
        /// Tipo de entrada, controlador portátil como el que usa Xbox
        /// </summary>
        public override string Input_Source_Controller => "Controlador";


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */

        public override string CityMenu_SalePricesTitle => "Precios de venta";
        public override string Blueprint_Title => "Planos";
        public override string Resource_Tab_Overview => "Visión general";
        public override string Resource_Tab_Stockpile => "Almacén";

        public override string Resource => "Recurso";
        public override string Resource_StockPile_Info => "Establece una cantidad objetivo para el almacenamiento de recursos; esto informará a los trabajadores cuándo deben cambiar de recurso.";
        public override string Resource_TypeName_Water => "agua";
        public override string Resource_TypeName_Wood => "madera";
        public override string Resource_TypeName_Fuel => "combustible";
        public override string Resource_TypeName_Stone => "piedra";
        public override string Resource_TypeName_RawFood => "comida cruda";
        public override string Resource_TypeName_Food => "comida";
        public override string Resource_TypeName_Beer => "cerveza";
        public override string Resource_TypeName_Wheat => "trigo";
        public override string Resource_TypeName_Linen => "lino";
        //public override string Resource_TypeName_SkinAndLinen => "piel y lino";
        public override string Resource_TypeName_IronOre => "mineral de hierro";
        public override string Resource_TypeName_GoldOre => "mineral de oro";
        public override string Resource_TypeName_Iron => "hierro";

        public override string Resource_TypeName_SharpStick => "Palo afilado";
        public override string Resource_TypeName_Sword => "Espada";
        public override string Resource_TypeName_KnightsLance => "Lanza de caballero";
        public override string Resource_TypeName_TwoHandSword => "Espada de dos manos";
        public override string Resource_TypeName_Bow => "Arco";

        public override string Resource_TypeName_LightArmor => "Armadura ligera";
        public override string Resource_TypeName_MediumArmor => "Armadura media";
        public override string Resource_TypeName_HeavyArmor => "Armadura pesada";

        public override string ResourceType_Children => "Niños";

        public override string BuildingType_DefaultName => "Edificio";
        public override string BuildingType_WorkerHut => "Cabaña de trabajadores";
        public override string BuildingType_Tavern => "Taberna";
        public override string BuildingType_Brewery => "Cervecería";
        public override string BuildingType_Postal => "Servicio postal";
        public override string BuildingType_Recruitment => "Centro de reclutamiento";
        public override string BuildingType_Barracks => "Cuartel";
        public override string BuildingType_PigPen => "Corral de cerdos";
        public override string BuildingType_HenPen => "Gallinero";
        public override string BuildingType_WorkBench => "Banco de trabajo";
        public override string BuildingType_Carpenter => "Carpintero";
        public override string BuildingType_CoalPit => "Pozo de carbón";
        public override string DecorType_Statue => "Estatua";
        public override string DecorType_Pavement => "Pavimento";
        public override string BuildingType_Smith => "Herrería";
        public override string BuildingType_Cook => "Cocina";
        public override string BuildingType_Storage => "Almacén";

        public override string BuildingType_ResourceFarm => "Granja de {0}";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Amplía el límite de trabajadores en {0}";
        public override string BuildingType_Tavern_Description => "Los trabajadores pueden comer aquí";
        public override string BuildingType_Tavern_Brewery => "Producción de cerveza";
        public override string BuildingType_Postal_Description => "Envía recursos a otras ciudades";
        public override string BuildingType_Recruitment_Description => "Envía hombres a otras ciudades";
        public override string BuildingType_Barracks_Description => "Usa hombres y equipo para reclutar soldados";
        public override string BuildingType_PigPen_Description => "Produce cerdos, que proporcionan comida y piel";
        public override string BuildingType_HenPen_Description => "Produce gallinas y huevos, que proporcionan comida";
        public override string BuildingType_Decor_Description => "Decoración";
        public override string BuildingType_Farm_Description => "Cultiva un recurso";

        public override string BuildingType_Cook_Description => "Estación de elaboración de comida";
        public override string BuildingType_Bench_Description => "Estación de elaboración de objetos";

        public override string BuildingType_Smith_Description => "Estación de trabajo con metal";
        public override string BuildingType_Carpenter_Description => "Estación de trabajo con madera";

        public override string BuildingType_Nobelhouse_Description => "Hogar de caballeros y diplomáticos";
        public override string BuildingType_CoalPit_Description => "Producción eficiente de combustible";
        public override string BuildingType_Storage_Description => "Punto de entrega de recursos";

        public override string MenuTab_Info => "Información";
        public override string MenuTab_Work => "Trabajo";
        public override string MenuTab_Recruit => "Reclutar";
        public override string MenuTab_Resources => "Recursos";
        public override string MenuTab_Trade => "Comercio";
        public override string MenuTab_Build => "Construir";
        public override string MenuTab_Economy => "Economía";
        public override string MenuTab_Delivery => "Entrega";

        public override string MenuTab_Build_Description => "Coloca edificios en tu ciudad";
        public override string MenuTab_BlackMarket_Description => "Coloca edificios en tu ciudad";
        public override string MenuTab_Resources_Description => "Coloca edificios en tu ciudad";
        public override string MenuTab_Work_Description => "Coloca edificios en tu ciudad";
        public override string MenuTab_Automation_Description => "Coloca edificios en tu ciudad";

        public override string BuildHud_OutsideCity => "Fuera de la región de la ciudad";
        public override string BuildHud_OutsideFaction => "¡Fuera de tus fronteras!";

        public override string BuildHud_OccupiedTile => "Terreno ocupado";

        public override string Build_PlaceBuilding => "Construir";
        public override string Build_DestroyBuilding => "Destruir";
        public override string Build_ClearTerrain => "Limpiar terreno";

        public override string Build_ClearOrders => "Limpiar órdenes de construcción";
        public override string Build_Order => "Orden de construcción";
        public override string Build_OrderQue => "Cola de órdenes de construcción: {0}";
        public override string Build_AutoPlace => "Colocación automática";

        public override string Work_OrderPrioTitle => "Prioridad de trabajo";
        public override string Work_OrderPrioDescription => "La prioridad va desde 1 (baja) hasta {0} (alta)";

        public override string Work_OrderPrio_No => "Sin prioridad. No se trabajará en esto.";
        public override string Work_OrderPrio_Min => "Prioridad mínima.";
        public override string Work_OrderPrio_Max => "Prioridad máxima.";

        public override string Work_Move => "Mover artículos";

        public override string Work_GatherXResource => "Recolectar {0}";
        public override string Work_CraftX => "Elaborar {0}";
        public override string Work_Farming => "Agricultura";
        public override string Work_Mining => "Minería";
        public override string Work_Trading => "Comercio";

        public override string Work_AutoBuild => "Construcción y expansión automáticas";

        public override string WorkerHud_WorkType => "Estado del trabajo: {0}";
        public override string WorkerHud_Carry => "Cargar: {0} {1}";
        public override string WorkerHud_Energy => "Energía: {0}";
        public override string WorkerStatus_Exit => "Abandonar fuerza laboral";
        public override string WorkerStatus_Eat => "Comer";
        public override string WorkerStatus_Till => "Labrar";
        public override string WorkerStatus_Plant => "Plantar";
        public override string WorkerStatus_Gather => "Recolectar";
        public override string WorkerStatus_PickUpResource => "Recoger recurso";
        public override string WorkerStatus_DropOff => "Entregar";
        public override string WorkerStatus_BuildX => "Construir {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Regresar al ejército";

        public override string Hud_ToggleFollowFaction => "Alternar configuración de seguimiento de facción";
        public override string Hud_FollowFaction_Yes => "Está configurado para usar los ajustes globales de la facción";
        public override string Hud_FollowFaction_No => "Está configurado para usar los ajustes locales (El valor global es {0})";

        public override string Hud_Idle => "Inactivo";
        public override string Hud_NoLimit => "Sin límite";

        public override string Hud_None => "Ninguno";
        public override string Hud_Queue => "Cola";

        public override string Hud_EmptyList => "- Lista vacía -";

        public override string Hud_RequirementOr => "- o -";

        public override string Hud_BlackMarket => "Mercado negro";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Seleccionar ciudad";
        public override string Conscription_Title => "Reclutamiento";
        public override string Conscript_WeaponTitle => "Arma";
        public override string Conscript_ArmorTitle => "Armadura";
        public override string Conscript_TrainingTitle => "Entrenamiento";

        public override string Conscript_SpecializationTitle => "Especialización";
        public override string Conscript_SpecializationDescription => "Aumentará el ataque en un área y reducirá todas las demás en {0}";
        public override string Conscript_SelectBuilding => "Seleccionar cuartel";

        public override string Conscript_WeaponDamage => "Daño del arma: {0}";
        public override string Conscript_ArmorHealth => "Salud de la armadura: {0}";
        public override string Conscript_TrainingSpeed => "Velocidad de ataque: {0}";
        public override string Conscript_TrainingTime => "Tiempo de entrenamiento: {0}";

        public override string Conscript_Training_Minimal => "Mínimo";
        public override string Conscript_Training_Basic => "Básico";
        public override string Conscript_Training_Skillful => "Hábil";
        public override string Conscript_Training_Professional => "Profesional";

        public override string Conscript_Specialization_Field => "Campo abierto";
        public override string Conscript_Specialization_Sea => "Mar";
        public override string Conscript_Specialization_Siege => "Asedio";
        public override string Conscript_Specialization_Traditional => "Tradicional";
        public override string Conscript_Specialization_AntiCavalry => "Anti caballería";

        public override string Conscription_Status_CollectingEquipment => "Reuniendo equipo: {0}";
        public override string Conscription_Status_CollectingMen => "Reuniendo hombres: {0}";
        public override string Conscription_Status_Training => "Entrenando: {0}";

        public override string ArmyHud_Food_Reserves_X => "Reservas de comida: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Mantenimiento de comida: {0}";
        public override string ArmyHud_Food_Costs_X => "Costos de comida: {0}";

        public override string Deliver_WillSendXInfo => "Se enviará {0} a la vez";
        public override string Delivery_ListTitle => "Seleccionar servicio de entrega";
        public override string Delivery_DistanceX => "Distancia: {0}";
        public override string Delivery_DeliveryTimeX => "Tiempo de entrega: {0}";
        public override string Delivery_SenderMinimumCap => "Capacidad mínima del remitente";
        public override string Delivery_RecieverMaximumCap => "Capacidad máxima del receptor";
        public override string Delivery_ItemsReady => "Artículos listos";
        public override string Delivery_RecieverReady => "Receptor listo";
        public override string Hud_ThisCity => "Esta ciudad";
        public override string Hud_RecieveingCity => "Ciudad receptora";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "Mostrado en Recursos por Segundo.";

        public override string Info_MinuteAverage => "El valor es un promedio del último minuto";

        public override string Message_OutOfFood_Title => "Sin comida";
        public override string Message_CityOutOfFood_Text => "Se comprará comida cara en el mercado negro. Los trabajadores morirán de hambre cuando se acabe tu dinero.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Tipo de terreno";

        public override string Hud_EnergyUpkeepX => "Mantenimiento energético de comida {0}";

        public override string Hud_EnergyAmount => "{0} energía (segundos de trabajo)";

        public override string Hud_CopySetup => "Copiar configuración";
        public override string Hud_Paste => "Pegar";

        public override string Hud_Available => "Disponible";

        public override string WorkForce_ChildBirthRequirements => "Requisitos para el nacimiento de niños:";
        public override string WorkForce_AvailableHomes => "Casas disponibles: {0}";
        public override string WorkForce_Peace => "Paz";
        public override string WorkForce_ChildToManTime => "Edad adulta: {0} minutos";

        public override string Economy_TaxIncome => "Ingreso por impuestos: {0}";
        public override string Economy_ImportCostsForResource => "Costos de importación de {0}: {1}";
        public override string Economy_BlackMarketCostsForResource => "Costos en el mercado negro de {0}: {1}";
        public override string Economy_GuardUpkeep => "Mantenimiento de la guardia: {0}";

        public override string Economy_LocalCityTrade_Export => "Exportación de comercio local: {0}";
        public override string Economy_LocalCityTrade_Import => "Importación de comercio local: {0}";

        public override string Economy_ResourceProduction => "Producción de {0}: {1}";
        public override string Economy_ResourceSpending => "Gastos de {0}: {1}";

        public override string Economy_TaxDescription => "El impuesto es de {0} oro por trabajador";

        public override string Economy_SoldResources => "Recursos vendidos (mineral de oro): {0}";

        public override string UnitType_Cities => "Ciudades";
        public override string UnitType_Armies => "Ejércitos";
        public override string UnitType_Worker => "Trabajador";

        public override string UnitType_FootKnight => "Caballero a pie";
        public override string UnitType_CavalryKnight => "Caballero de caballería";

        public override string CityCulture_LargeFamilies => "Familias numerosas";
        public override string CityCulture_FertileGround => "Terrenos fértiles";
        public override string CityCulture_Archers => "Arqueros hábiles";
        public override string CityCulture_Warriors => "Guerreros";
        public override string CityCulture_AnimalBreeder => "Criadores de animales";
        public override string CityCulture_Miners => "Mineros";
        public override string CityCulture_Woodcutters => "Leñadores";
        public override string CityCulture_Builders => "Constructores";
        public override string CityCulture_CrabMentality => "Mentalidad de cangrejo";
        public override string CityCulture_DeepWell => "Pozo profundo";
        public override string CityCulture_Networker => "Red de contactos";
        public override string CityCulture_PitMasters => "Maestros del pozo";

        public override string CityCulture_CultureIsX => "Cultura: {0}";
        public override string CityCulture_LargeFamilies_Description => "Aumento en el nacimiento de niños";
        public override string CityCulture_FertileGround_Description => "Los cultivos producen más";
        public override string CityCulture_Archers_Description => "Produce arqueros hábiles";
        public override string CityCulture_Warriors_Description => "Produce combatientes cuerpo a cuerpo hábiles";
        public override string CityCulture_AnimalBreeder_Description => "Los animales producen más recursos";
        public override string CityCulture_Miners_Description => "Extrae más mineral";
        public override string CityCulture_Woodcutters_Description => "Los árboles producen más madera";
        public override string CityCulture_Builders_Description => "Construyen rápido";
        public override string CityCulture_CrabMentality_Description => "El trabajo cuesta menos energía. No puede producir soldados de alta habilidad.";
        public override string CityCulture_DeepWell_Description => "El agua se repone más rápido";
        public override string CityCulture_Networker_Description => "Servicio postal eficiente";
        public override string CityCulture_PitMasters_Description => "Mayor producción de combustible";

        public override string CityOption_AutoBuild_Work => "Expansión automática de la fuerza laboral";
        public override string CityOption_AutoBuild_Farm => "Expansión automática de granjas";

        public override string Hud_PurchaseTitle_Resources => "Comprar recursos";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "Posees";

        public override string Tutorial_EndTutorial => "Terminar tutorial";
        public override string Tutorial_MissionX => "Misión {0}";
        public override string Tutorial_CollectXAmountOfY => "Recolecta {0} {1}";
        public override string Tutorial_SelectTabX => "Selecciona pestaña: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Aumenta la prioridad en: {0}";
        public override string Tutorial_PlaceBuildOrder => "Coloca una orden de construcción: {0}";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Selecciona una ciudad";
        public override string Tutorial_ZoomInWorkers => "Haz zoom para ver a los trabajadores";
        public override string Tutorial_CreateSoldiers => "Crea dos unidades de soldados con este equipo: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Aleja el zoom para la vista del mapa";
        public override string Tutorial_ZoomOutDiplomacy => "Aleja el zoom para la vista diplomática";
        public override string Tutorial_ImproveRelations => "Mejora tus relaciones con una facción vecina";
        public override string Tutorial_MissionComplete_Title => "¡Misión completada!";
        public override string Tutorial_MissionComplete_Unlocks => "Nuevos controles desbloqueados";

        //patch1
        public override string Resource_ReachedStockpile => "Objetivo de reserva alcanzado";

        public override string BuildingType_ResourceMine => "Mina de {0}";

        public override string Resource_TypeName_BogIron => "Hierro de pantano";

        public override string Resource_TypeName_Coal => "Carbón";

        public override string Language_XUpkeepIsY => "Mantenimiento de {0}: {1}";
        public override string Language_XCountIsY => "Conteo de {0}: {1}";

        public override string Message_ArmyOutOfFood_Text => "Se comprará comida cara del mercado negro. Los soldados hambrientos desertarán cuando se acabe tu dinero.";

        public override string Info_ArmyFood => "Los ejércitos reabastecerán comida de la ciudad amiga más cercana. Se puede comprar comida de otras facciones. En regiones hostiles, la comida solo puede comprarse en el mercado negro.";

        public override string FactionName_Monger => "Mercader";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

    }
}
