using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Russian : AbsLanguage
    {
        /// <summary>
        /// Название этого языка
        /// </summary>
        public override string MyLanguage => "Английский";

        /// <summary>
        /// Как отображать количество предметов. 0: предмет, 1: количество
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}: {1}";

        /// <summary>
        /// Выбор языка
        /// </summary>
        public override string Lobby_Language => "Язык";

        /// <summary>
        /// Начать игру
        /// </summary>
        public override string Lobby_Start => "НАЧАТЬ";

        /// <summary>
        /// Кнопка для выбора количества локальных игроков, 0: текущее количество игроков
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Локальный мультиплеер";

        /// <summary>
        /// Заголовок меню, где выбирается количество игроков в режиме разделенного экрана
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Выберите количество игроков";

        /// <summary>
        /// Описание локального мультиплеера
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "Для мультиплеера требуются контроллеры Xbox";

        /// <summary>
        /// Перейти к следующей позиции разделенного экрана
        /// </summary>
        public override string Lobby_NextScreen => "Следующая позиция экрана";

        /// <summary>
        /// Игроки могут выбрать визуальное оформление и сохранить его в профиле
        /// </summary>
        public override string Lobby_FlagSelectTitle => "Выбор флага";

        /// <summary>
        /// 0: Пронумеровано от 1 до 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Флаг {0}";

        /// <summary>
        /// Название игры и номер версии
        /// </summary>
        public override string Lobby_GameVersion => "DSS war party - версия {0}";

        public override string FlagEditor_Description => "Нарисуйте свой флаг и выберите цвета для своих солдат.";

        /// <summary>
        /// Инструмент для заливки области цветом
        /// </summary>
        public override string FlagEditor_Bucket => "Заливка";

        /// <summary>
        /// Открывает редактор профиля флага
        /// </summary>
        public override string Lobby_FlagEdit => "Редактировать флаг";

        public override string Lobby_WarningTitle => "Предупреждение";
        public override string Lobby_IgnoreWarning => "Игнорировать предупреждение";

        /// <summary>
        /// Предупреждение, когда у одного из игроков не выбран ввод.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "У одного игрока не выбран ввод";

        /// <summary>
        /// Меню с содержимым, которое не используется большинством игроков.
        /// </summary>
        public override string Lobby_Extra => "Дополнительно";

        /// <summary>
        /// Дополнительное содержание не переведено и не поддерживает полное управление контроллером.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "Внимание! Это содержание не локализовано и не имеет поддержки ожидаемого ввода/доступности";

        public override string Lobby_MapSizeTitle => "Размер карты";

        /// <summary>
        /// Название размера карты 1
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "Крошечный";

        /// <summary>
        /// Название размера карты 2
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "Маленький";

        /// <summary>
        /// Название размера карты 3
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "Средний";

        /// <summary>
        /// Название размера карты 4
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "Большой";

        /// <summary>
        /// Название размера карты 5
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "Огромный";

        /// <summary>
        /// Название размера карты 6
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "Эпический";

        /// <summary>
        /// Описание размера карты X на Y километров. 0: Ширина, 1: Высота
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} км";

        /// <summary>
        /// Закрыть приложение игры
        /// </summary>
        public override string Lobby_ExitGame => "Выход";

        /// <summary>
        /// Отображение имени локального мультиплеера, 0: номер игрока
        /// </summary>
        public override string Player_DefaultName => "Игрок {0}";

        /// <summary>
        /// В редакторе профиля игрока. Открывает меню с параметрами редактора
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Опции";

        /// <summary>
        /// В редакторе профиля игрока. Заголовок для выбора цветов флага
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Цвета флага";

        /// <summary>
        /// В редакторе профиля игрока. Опция цвета флага
        /// </summary>
        public override string ProfileEditor_MainColor => "Основной цвет";

        /// <summary>
        /// В редакторе профиля игрока. Опция цвета флага
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Цвет детали 1";

        /// <summary>
        /// В редакторе профиля игрока. Опция цвета флага
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Цвет детали 2";

        /// <summary>
        /// В редакторе профиля игрока. Заголовок для выбора цветов солдат
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "Люди";

        /// <summary>
        /// В редакторе профиля игрока. Опция цвета солдата
        /// </summary>
        public override string ProfileEditor_SkinColor => "Цвет кожи";

        /// <summary>
        /// В редакторе профиля игрока. Опция цвета солдата
        /// </summary>
        public override string ProfileEditor_HairColor => "Цвет волос";

        /// <summary>
        /// В редакторе профиля игрока. Открыть палитру цветов и выбрать цвет
        /// </summary>
        public override string ProfileEditor_PickColor => "Выбрать цвет";

        /// <summary>
        /// В редакторе профиля игрока. Настроить положение изображения
        /// </summary>
        public override string ProfileEditor_MoveImage => "Переместить изображение";

        /// <summary>
        /// В редакторе профиля игрока. Направление перемещения
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Влево";

        /// <summary>
        /// В редакторе профиля игрока. Направление перемещения
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Вправо";

        /// <summary>
        /// В редакторе профиля игрока. Направление перемещения
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Вверх";

        /// <summary>
        /// В редакторе профиля игрока. Направление перемещения
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Вниз";

        /// <summary>
        /// В редакторе профиля игрока. Закрыть редактор без сохранения
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Отменить и выйти";

        /// <summary>
        /// В редакторе профиля игрока. Подсказка для отмены
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Отменить все изменения";

        /// <summary>
        /// В редакторе профиля игрока. Сохранить изменения и закрыть редактор
        /// </summary>
        public override string Hud_SaveAndExit => "Сохранить и выйти";

        /// <summary>
        /// В редакторе профиля игрока. Часть опций цвета Тон, Насыщенность и Светлота
        /// </summary>
        public override string ProfileEditor_Hue => "Тон";

        /// <summary>
        /// В редакторе профиля игрока. Часть опций цвета Тон, Насыщенность и Светлота
        /// </summary>
        public override string ProfileEditor_Lightness => "Светлота";

        /// <summary>
        /// В редакторе профиля игрока. Переключение между опциями цвета флага и солдата
        /// </summary>
        public override string ProfileEditor_NextColorType => "Следующий тип цвета";

        /// <summary>
        /// Текущая скорость игры по сравнению с реальным временем
        /// </summary>
        public override string Hud_GameSpeedLabel => "Скорость игры: {0}x";

        public override string Input_GameSpeed => "Скорость игры";

        /// <summary>
        /// Отображение в игре. Производство золота юнитом
        /// </summary>
        public override string Hud_TotalIncome => "Общий доход/секунда: {0}";

        /// <summary>
        /// Стоимость содержания юнита
        /// </summary>
        public override string Hud_Upkeep => "Содержание: {0}";
        public override string Hud_ArmyUpkeep => "Содержание армии: {0}";

        /// <summary>
        /// Отображение в игре. Солдаты, защищающие здание
        /// </summary>
        public override string Hud_GuardCount => "Стражники";

        public override string Hud_IncreaseMaxGuardCount => "Максимальный размер стражи {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "Вам нужно расширить город.";

        public override string Hud_SoldierCount => "Количество солдат: {0}";

        public override string Hud_SoldierGroupsCount => "Количество групп: {0}";

        /// <summary>
        /// Отображение в игре. Расчетная боевая мощь юнита
        /// </summary>
        public override string Hud_StrengthRating => "Рейтинг силы: {0}";

        /// <summary>
        /// Отображение в игре. Расчетная боевая мощь всей нации
        /// </summary>
        public override string Hud_TotalStrengthRating => "Военная мощь: {0}";

        /// <summary>
        /// Отображение в игре. Дополнительные люди, прибывающие извне город-государства
        /// </summary>
        public override string Hud_Immigrants => "Иммигранты";

        public override string Hud_CityCount => "Количество городов: {0}";
        public override string Hud_ArmyCount => "Количество армий: {0}";

        /// <summary>
        /// Мини-кнопка для повторения покупки несколько раз, например "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Требование";
        public override string Hud_PurchaseTitle_Cost => "Стоимость";
        public override string Hud_PurchaseTitle_Gain => "Доход";

        /// <summary>
        /// Сколько ресурса будет использовано, например, "5 золота. (Доступно: 10)". Над текстом будет заголовок "стоимость". 0: Ресурс, 1: стоимость, 2: доступно
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Доступно: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "Стоимость увеличится на {0}";

        public override string Hud_Purchase_MaxCapacity => "Достигнута максимальная вместимость";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Сила: Ваша {0} - Их {1}";

        /// <summary>
        /// Отображение короткой строки даты как Год, Месяц, День
        /// </summary>
        public override string Hud_Date => "Г{0} М{1} Д{2}";

        /// <summary>
        /// Отображение короткой строки временного промежутка как Часы, Минуты, Секунды
        /// </summary>
        public override string Hud_TimeSpan => "Ч{0} М{1} С{2}";

        /// <summary>
        /// Битва между двумя армиями или армией и городом
        /// </summary>
        public override string Hud_Battle => "Битва";



        /// <summary>
        /// Описывает ввод кнопки. Пауза.
        /// </summary>
        public override string Input_Pause => "Пауза";

        /// <summary>
        /// Описывает ввод кнопки. Возобновить из паузы.
        /// </summary>
        public override string Input_ResumePaused => "Возобновить";

        /// <summary>
        /// Генерический денежный ресурс
        /// </summary>
        public override string ResourceType_Gold => "Золото";

        /// <summary>
        /// Ресурс рабочих людей
        /// </summary>
        public override string ResourceType_Workers => "Рабочие";

        public override string ResourceType_Workers_Description => "Рабочие приносят доход. Также их призывают в солдаты для ваших армий";

        /// <summary>
        /// Ресурс, используемый в дипломатии
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Очки дипломатии";

        /// <summary>
        /// 0: Сколько очков вы получили, 1: Мягкий максимум (после этого будет увеличиваться намного медленнее), 2: Жесткий лимит
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Дипломатические очки: {0} / {1} ({2})";

        /// <summary>
        /// Тип городского здания. Здание для рыцарей и дипломатов.
        /// </summary>
        public override string Building_NobleHouse => "Дом знати";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "1 очко дипломатии каждые {0} секунд";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} к максимальному лимиту очков дипломатии";
        public override string Building_NobleHouse_UnlocksKnight => "Разблокирует юнит Рыцарь";

        public override string Building_BuildAction => "Строить";
        public override string Building_IsBuilt => "Построено";

        /// <summary>
        /// Тип городского здания. Злое массовое производство.
        /// </summary>
        public override string Building_DarkFactory => "Темная фабрика";

        /// <summary>
        /// В меню настроек игры. Суммирует все опции сложности в процентах.
        /// </summary>
        public override string Settings_TotalDifficulty => "Общая сложность {0}%";

        /// <summary>
        /// В меню настроек игры. Базовый уровень сложности.
        /// </summary>
        public override string Settings_DifficultyLevel => "Уровень сложности {0}%";

        /// <summary>
        /// В меню настроек игры. Опция создания новых карт вместо загрузки существующих
        /// </summary>
        public override string Settings_GenerateMaps => "Создать новые карты";

        /// <summary>
        /// В меню настроек игры. Создание новых карт занимает больше времени
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Создание занимает больше времени, чем загрузка готовых карт";

        /// <summary>
        /// В меню настроек игры. Опция сложности. Блокирует возможность играть в игре во время паузы.
        /// </summary>
        public override string Settings_AllowPause => "Разрешить паузу и команды";

        /// <summary>
        /// В меню настроек игры. Опция сложности. Добавление боссов, которые появляются в игре.
        /// </summary>
        public override string Settings_BossEvents => "События с боссами";

        /// <summary>
        /// В меню настроек игры. Опция сложности. Описание отсутствия боссов.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Отключение событий с боссами переведет игру в песочницу без завершения.";


        /// <summary>
        /// Опции для автоматизации игровых механик. Заголовок меню.
        /// </summary>
        public override string Automation_Title => "Автоматизация";
        /// <summary>
        /// Опции для автоматизации игровых механик. Информация о том, как работает автоматизация.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Будет ждать максимального числа рабочей силы";
        /// <summary>
        /// Опции для автоматизации игровых механик. Информация о том, как работает автоматизация.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Будет приостановлено, если доход отрицательный";
        /// <summary>
        /// Опции для автоматизации игровых механик. Информация о том, как работает автоматизация.
        /// </summary>
        public override string Automation_InfoLine_Priority => "В приоритете крупные города";
        /// <summary>
        /// Опции для автоматизации игровых механик. Информация о том, как работает автоматизация.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Совершает максимум одну покупку в секунду";


        /// <summary>
        /// Надпись на кнопке для действия. Специализированное здание для рыцарей и дипломатов.
        /// </summary>
        public override string HudAction_BuyItem => "Купить {0}";

        /// <summary>
        /// Состояние мира или войны между двумя нациями
        /// </summary>
        public override string Diplomacy_RelationType => "Отношения";

        /// <summary>
        /// Заголовок списка отношений других фракций друг с другом
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Их отношения с другими";

        /// <summary>
        /// Дипломатические отношения. Вы имеете прямой контроль над ресурсами нации.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Слуга";

        /// <summary>
        /// Дипломатические отношения. Полное сотрудничество.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Союзник";

        /// <summary>
        /// Дипломатические отношения. Уменьшенный шанс на войну.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "Хорошие";

        /// <summary>
        /// Дипломатические отношения. Мирное соглашение.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Мир";

        /// <summary>
        /// Дипломатические отношения. Еще не установлены контакты.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Нейтральные";

        /// <summary>
        /// Дипломатические отношения. Временное мирное соглашение.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Перемирие";

        /// <summary>
        /// Дипломатические отношения. Война.
        /// </summary>
        public override string Diplomacy_RelationType_War => "Война";

        /// <summary>
        /// Дипломатические отношения. Война без шансов на мир.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Тотальная война";

        /// <summary>
        /// Дипломатическое общение. Насколько хорошо вы можете обсуждать условия. 0: Условия обсуждения
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "Условия обсуждения: {0}";

        /// <summary>
        /// Дипломатическое общение. Лучше обычного.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "Хорошие";

        /// <summary>
        /// Дипломатическое общение. Обычные.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Обычные";

        /// <summary>
        /// Дипломатическое общение. Хуже обычного.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Плохие";

        /// <summary>
        /// Дипломатическое общение. Отказ от общения.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "Нет";

        /// <summary>
        /// Дипломатическое действие. Установить новые дипломатические отношения.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Установить отношения с: {0}";

        /// <summary>
        /// Дипломатическое действие. Предложить мир.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Предложить мир";

        /// <summary>
        /// Дипломатическое действие. Предложить союз.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Предложить союз";

        /// <summary>
        /// Дипломатический заголовок. Другой игрок предложил новые дипломатические отношения. 0: имя игрока
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} предлагает новые отношения";

        /// <summary>
        /// Дипломатическое действие. Принять новые дипломатические отношения.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Принять новые отношения";

        /// <summary>
        /// Дипломатическое описание. Другой игрок предложил новые дипломатические отношения. 0: тип отношений
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "Предложены новые отношения: {0}";

        /// <summary>
        /// Дипломатическое действие. Сделать другую нацию своим слугой.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Принять как слугу";

        /// <summary>
        /// Дипломатическое описание. Против зла.
        /// </summary>
        public override string Diplomacy_LightSide => "Союзник светлой стороны";

        /// <summary>
        /// Дипломатическое описание. Сколько времени будет длиться перемирие.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "Заканчивается через {0} секунд";

        /// <summary>
        /// Дипломатическое действие. Продлить перемирие.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Продлить перемирие";

        /// <summary>
        /// Дипломатическое описание. На сколько будет продлено перемирие.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Продлить перемирие на {0} секунд";

        /// <summary>
        /// Дипломатическое описание. Нарушение согласованных отношений будет стоить дипломатических очков.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "Нарушение отношений будет стоить {0} очков дипломатии";

        /// <summary>
        /// Дипломатическое описание для союзников.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Союзники делятся объявлениями о войне.";

        /// <summary>
        /// Дипломатическое описание для хороших отношений.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Ограничивает возможность объявления войны.";

        /// <summary>
        /// Дипломатическое описание. У вас должна быть более крупная военная сила, чем у вашего слуги (другой нации, которую вы контролируете).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "{0}x более мощная военная сила";

        /// <summary>
        /// Дипломатическое описание. Слуга должен быть в безнадежной войне (другая нация, которую вы контролируете).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "Слуга должен быть в войне против более сильного врага";

        /// <summary>
        /// Дипломатическое описание. Слуга не может иметь слишком много городов (другая нация, которую вы контролируете).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "Слуга может иметь максимум {0} городов";

        /// <summary>
        /// Дипломатическое описание. Стоимость в дипломатических очках будет увеличиваться (другая нация, которую вы контролируете).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "Стоимость будет увеличиваться с каждым слугой";

        /// <summary>
        /// Дипломатическое описание. Результат отношений со слугой, мирное поглощение другой нации.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Поглотить другую фракцию";

        /// <summary>
        /// Сообщение о получении объявления войны
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "Война объявлена!";

        /// <summary>
        /// Время перемирия истекло, и вы возвращаетесь к войне
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "Перемирие закончилось";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Заголовок.
        /// </summary>
        public override string EndGameStatistics_Title => "Статистика";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Общее время в игре.
        /// </summary>
        public override string EndGameStatistics_Time => "Время в игре: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Сколько солдат вы наняли.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "Нанято солдат: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Количество ваших солдат, погибших в бою.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Солдат погибло в бою: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Количество убитых вами солдат противника в бою.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Убито солдат противника в бою: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Количество ваших солдат, покинувших вас.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Солдат дезертировало: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Количество городов, захваченных в бою.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "Захвачено городов: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Количество городов, потерянных в бою.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "Потеряно городов: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Количество выигранных боев.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Выиграно боев: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Количество проигранных боев.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Проиграно боев: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Дипломатия. Количество объявлений войны, сделанных вами.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "Объявлено войн: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Дипломатия. Количество объявлений войны, сделанных против вас.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "Получено объявлений войны: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Союзники, приобретенные через дипломатию.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Дипломатические союзы: {0}";

        /// <summary>
        /// Статистика, отображаемая на экране окончания игры. Слуги, приобретенные через дипломатию. Города и армии слуг становятся вашими.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Дипломатические слуги: {0}";

        /// <summary>
        /// Коллективный тип юнита на карте. Армия солдат.
        /// </summary>
        public override string UnitType_Army => "Армия";

        /// <summary>
        /// Коллективный тип юнита на карте. Группа солдат.
        /// </summary>
        public override string UnitType_SoldierGroup => "Группа";

        /// <summary>
        /// Коллективный тип юнита на карте. Общее название для деревни или города.
        /// </summary>
        public override string UnitType_City => "Город";

        /// <summary>
        /// Групповой выбор армий
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Группа армий, количество: {0}";

        /// <summary>
        /// Название специализированного типа солдата. Стандартный солдат передовой линии.
        /// </summary>
        public override string UnitType_Soldier => "Солдат";

        /// <summary>
        /// Название специализированного типа солдата. Морской солдат.
        /// </summary>
        public override string UnitType_Sailor => "Моряк";

        /// <summary>
        /// Название специализированного типа солдата. Мобилизованные крестьяне.
        /// </summary>
        public override string UnitType_Folkman => "Народный солдат";

        /// <summary>
        /// Название специализированного типа солдата. Юнит с щитом и копьем.
        /// </summary>
        public override string UnitType_Spearman => "Копейщик";

        /// <summary>
        /// Название специализированного типа солдата. Элитная сила, часть королевской охраны.
        /// </summary>
        public override string UnitType_HonorGuard => "Гвардия Чести";

        /// <summary>
        /// Название специализированного типа солдата. Противокавалерийский юнит, носит длинные двуручные копья.
        /// </summary>
        public override string UnitType_Pikeman => "Пикейщик";

        /// <summary>
        /// Название специализированного типа солдата. Бронированный кавалерийский юнит.
        /// </summary>
        public override string UnitType_Knight => "Рыцарь";

        /// <summary>
        /// Название специализированного типа солдата. Лук и стрелы.
        /// </summary>
        public override string UnitType_Archer => "Лучник";

        /// <summary>
        /// Название специализированного типа солдата.
        /// </summary>
        public override string UnitType_Crossbow => "Арбалетчик";

        /// <summary>
        /// Название специализированного типа солдата. Военная машина, метающая большие копья.
        /// </summary>
        public override string UnitType_Ballista => "Баллиста";

        /// <summary>
        /// Название специализированного типа солдата. Фантастический тролль с пушкой.
        /// </summary>
        public override string UnitType_Trollcannon => "Тролль с пушкой";

        /// <summary>
        /// Название специализированного типа солдата. Солдат из леса.
        /// </summary>
        public override string UnitType_GreenSoldier => "Зеленый Солдат";

        /// <summary>
        /// Название специализированного типа солдата. Морской юнит севера.
        /// </summary>
        public override string UnitType_Viking => "Викинг";

        /// <summary>
        /// Название специализированного типа солдата. Злой мастер-босс.
        /// </summary>
        public override string UnitType_DarkLord => "Темный Лорд";

        /// <summary>
        /// Название специализированного типа солдата. Солдат, несущий большой флаг.
        /// </summary>
        public override string UnitType_Bannerman => "Флагоносец";

        /// <summary>
        /// Название военного юнита. Корабль, несущий солдат. 0: тип юнита, который он несет
        /// </summary>
        public override string UnitType_WarshipWithUnit => "{0} военный корабль";

        public override string UnitType_Description_Soldier => "Универсальный юнит.";
        public override string UnitType_Description_Sailor => "Сильны в морских сражениях";
        public override string UnitType_Description_Folkman => "Дешевые необученные солдаты";
        public override string UnitType_Description_HonorGuard => "Элитные солдаты без содержания";
        public override string UnitType_Description_Knight => "Сильны в полевых сражениях";
        public override string UnitType_Description_Archer => "Сильны только под защитой.";
        public override string UnitType_Description_Crossbow => "Мощный дальнобойный солдат";
        public override string UnitType_Description_Ballista => "Сильна против городов";
        public override string UnitType_Description_GreenSoldier => "Страшный эльфийский воин";
        public override string UnitType_Description_DarkLord => "Последний босс";
        /// <summary>
        /// Информация о типе солдата
        /// </summary>
        public override string SoldierStats_Title => "Статистика по юниту";

        /// <summary>
        /// Сколько групп солдат
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} групп, всего {1} юнитов";

        /// <summary>
        /// Солдаты будут иметь различную силу в зависимости от атаки на открытой местности, с кораблей или при атаке поселения
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Сила атаки: Суша {0} | Море {1} | Город {2}";

        /// <summary>
        /// Сколько ранений может выдержать солдат
        /// </summary>
        public override string SoldierStats_Health => "Здоровье: {0}";

        /// <summary>
        /// Некоторые солдаты увеличат скорость передвижения армии
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Бонус скорости армии на суше: {0}";

        /// <summary>
        /// Некоторые солдаты увеличат скорость передвижения кораблей
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Бонус скорости армии на море: {0}";

        /// <summary>
        /// Купленные солдаты начнут как новобранцы и завершат свою подготовку через несколько минут.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Время обучения: {0} минут. Будет вдвое быстрее, если новобранцы находятся рядом с городом.";

        /// <summary>
        /// Опция меню для управления армией. Остановить их движение.
        /// </summary>
        public override string ArmyOption_Halt => "Стоп";

        /// <summary>
        /// Опция меню для управления армией. Удалить солдат.
        /// </summary>
        public override string ArmyOption_Disband => "Расформировать юниты";

        /// <summary>
        /// Опция меню для управления армией. Опции отправки солдат между армиями.
        /// </summary>
        public override string ArmyOption_Divide => "Разделить армию";

        /// <summary>
        /// Опция меню для управления армией. Удалить солдат.
        /// </summary>
        public override string ArmyOption_RemoveX => "Удалить {0}";

        /// <summary>
        /// Опция меню для управления армией. Удалить солдат.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Расформировать всех";

        /// <summary>
        /// Опция меню для управления армией. 0: Количество, 1: Тип юнита
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} групп: {0}";

        /// <summary>
        /// Опция меню для управления армией. Опции отправки солдат между армиями.
        /// </summary>
        public override string ArmyOption_SendToX => "Отправить юниты к {0}";

        public override string ArmyOption_MergeAllArmies => "Объединить все армии";

        /// <summary>
        /// Опция меню для управления армией. Опции отправки солдат между армиями.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Отправить юниты в новую армию";

        /// <summary>
        /// Опция меню для управления армией. Опции отправки солдат между армиями.
        /// </summary>
        public override string ArmyOption_SendX => "Отправить {0}";

        /// <summary>
        /// Опция меню для управления армией. Опции отправки солдат между армиями.
        /// </summary>
        public override string ArmyOption_SendAll => "Отправить всех";

        /// <summary>
        /// Опция меню для управления армией. Опции отправки солдат между армиями.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Разделить армию пополам";

        /// <summary>
        /// Опция меню для управления армией. Опции отправки солдат между армиями.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Объединить армии";

        /// <summary>
        /// Нанять солдат.
        /// </summary>
        public override string UnitType_Recruit => "Нанять";

        /// <summary>
        /// Нанять солдат типа. 0:тип
        /// </summary>
        public override string CityOption_RecruitType => "Нанять {0}";

        /// <summary>
        /// Количество наемных солдат
        /// </summary>
        public override string CityOption_XMercenaries => "Наемники: {0}";

        /// <summary>
        /// Указывает количество наемников, доступных для найма на рынке
        /// </summary>
        public override string Hud_MercenaryMarket => "Рынок наемников для найма";

        /// <summary>
        /// Нанять определенное количество наемных солдат
        /// </summary>
        public override string CityOption_BuyXMercenaries => "Импортировать {0} наемников";

        public override string CityOption_Mercenaries_Description => "Солдаты будут набраны из числа наемников, а не из вашей рабочей силы";

        /// <summary>
        /// Надпись на кнопке для действия. Создать жилье для большего количества рабочих.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "Расширить рабочую силу";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "Макс. рабочая сила +{0}";
        public override string CityOption_ExpandGuardSize => "Увеличить размер охраны";

        public override string CityOption_Damages => "Повреждения: {0}";
        public override string CityOption_Repair => "Ремонтировать повреждения";
        public override string CityOption_RepairGain => "Ремонтировать {0} повреждений";

        public override string CityOption_Repair_Description => "Повреждения уменьшают количество рабочих, которых можно разместить.";

        public override string CityOption_BurnItDown => "Сжечь дотла";
        public override string CityOption_BurnItDown_Description => "Удалите рабочую силу и нанесите максимальные повреждения";

        /// <summary>
        /// Главный босс. Назван в честь светящегося металлического камня, застрявшего в их лбу.
        /// </summary>
        public override string FactionName_DarkLord => "Око Рока";

        /// <summary>
        /// Фракция, вдохновленная орками. Служит темному лорду.
        /// </summary>
        public override string FactionName_DarkFollower => "Слуги Ужаса";

        /// <summary>
        /// Крупнейшая фракция, старое, но испорченное королевство.
        /// </summary>
        public override string FactionName_UnitedKingdom => "Объединенные Королевства";

        /// <summary>
        /// Фракция, вдохновленная эльфами. Живут в гармонии с лесом.
        /// </summary>
        public override string FactionName_Greenwood => "Зеленый Лес";

        /// <summary>
        /// Фракция с азиатским уклоном на востоке.
        /// </summary>
        public override string FactionName_EasternEmpire => "Восточная Империя";

        /// <summary>
        /// Королевство викингов на севере. Самое большое.
        /// </summary>
        public override string FactionName_NordicRealm => "Северные Королевства";

        /// <summary>
        /// Королевство викингов на севере. Использует символ медвежьего когтя.
        /// </summary>
        public override string FactionName_BearClaw => "Медвежий Коготь";

        /// <summary>
        /// Королевство викингов на севере. Использует символ петуха.
        /// </summary>
        public override string FactionName_NordicSpur => "Северный Шпор";

        /// <summary>
        /// Королевство викингов на севере. Использует символ черного ворона.
        /// </summary>
        public override string FactionName_IceRaven => "Ледяной Ворон";

        /// <summary>
        /// Фракция, известная убийством драконов с помощью мощных баллист.
        /// </summary>
        public override string FactionName_Dragonslayer => "Драконоборцы";

        /// <summary>
        /// Наемное подразделение с юга. Арабский стиль.
        /// </summary>
        public override string FactionName_SouthHara => "Южная Хара";

        /// <summary>
        /// Название для нейтральных наций под управлением ИИ
        /// </summary>
        public override string FactionName_GenericAi => "ИИ {0}";

        /// <summary>
        /// Отображаемое имя для игроков и их номера
        /// </summary>
        public override string FactionName_Player => "Игрок {0}";

        /// <summary>
        /// Сообщение о приближении мини-босса на кораблях с юга.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "Враг приближается!";
        public override string EventMessage_HaraMercenaryText => "На юге замечены наемники Хара";

        /// <summary>
        /// Первое предупреждение о появлении главного босса.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "Темное пророчество";
        public override string EventMessage_ProphesyText => "Око Рока скоро появится, и ваши враги присоединятся к нему!";

        /// <summary>
        /// Второе предупреждение о появлении главного босса.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Темные времена";
        public override string EventMessage_FinalBossEnterText => "Око Рока вошло на карту!";

        /// <summary>
        /// Сообщение, когда главный босс встретится с вами на поле боя.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "Отчаянная атака";
        public override string EventMessage_FinalBattleText => "Темный лорд присоединился к полю боя. Теперь ваш шанс уничтожить его!";

        /// <summary>
        /// Сообщение, когда солдаты покидают армию, если вы не можете оплатить их содержание
        /// </summary>
        public override string EventMessage_DesertersTitle => "Дезертиры!";
        public override string EventMessage_DesertersText_Money => "Неоплаченные солдаты дезертируют из ваших армий";

        public override string DifficultyDescription_AiAggression => "Агрессивность ИИ: {0}.";
        public override string DifficultyDescription_BossSize => "Размер босса: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Время появления босса: {0}.";
        public override string DifficultyDescription_AiEconomy => "Экономика ИИ: {0}%.";
        public override string DifficultyDescription_AiDelay => "Задержка ИИ: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Сложность дипломатии: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Стоимость наемников: {0}.";
        public override string DifficultyDescription_HonorGuards => "Гвардия чести: {0}.";

        /// <summary>
        /// Игра закончилась успехом.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Победа!";

        /// <summary>
        /// Цитаты лидера, которого вы играете в игре
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
{
    "В мирное время мы скорбим о погибших.",
    "Каждая победа несет тень жертвы.",
    "Помните путь, который привел нас сюда, усыпанный душами храбрых.",
    "Наши умы светлы от победы, наши сердца тяжелы от тяжести павших."
};

        public override string EndScreen_DominationVictoryQuote => "Меня избрали боги, чтобы властвовать над миром!";

        /// <summary>
        /// Игра закончилась неудачей.
        /// </summary>
        public override string EndScreen_FailTitle => "Поражение!";

        /// <summary>
        /// Цитаты лидера, которого вы играете в игре
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
{
    "С нашими телами, изнуренными от маршей и ночей тревог, мы приветствуем конец.",
    "Поражение может омрачить наши земли, но оно не может погасить свет нашей решимости.",
    "Погасите пламя в наших сердцах, из их пепла наши дети создадут новый рассвет.",
    "Пусть наши рассказы станут угольком, разжигающим завтрашнюю победу."
};

        /// <summary>
        /// Короткая заставка в конце игры
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Смотреть эпилог";

        /// <summary>
        /// Название заставки
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Эпилог";

        /// <summary>
        /// Введение к заставке
        /// </summary>
        public override string EndScreen_Epilogue_Text => "160 лет назад";

        /// <summary>
        /// Пролог - это короткое стихотворение о сюжете игры
        /// </summary>
        public override string GameMenu_WatchPrologue => "Смотреть пролог";

        public override string Prologue_Title => "Пролог";

        /// <summary>
        /// Стихотворение должно состоять из трех строк, четвертая строка будет взята из переводов имен, чтобы представить имя босса
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
{
    "Сны преследуют тебя ночью,",
    "Пророчество о темном будущем",
    "Готовьтесь к его приходу,",
};

        /// <summary>
        /// Меню в игре при паузе
        /// </summary>
        public override string GameMenu_Title => "Игровое меню";

        /// <summary>
        /// Продолжить игру после экрана завершения
        /// </summary>
        public override string GameMenu_ContinueGame => "Продолжить";

        /// <summary>
        /// Продолжить игру
        /// </summary>
        public override string GameMenu_Resume => "Возобновить";

        /// <summary>
        /// Выйти в игровое лобби
        /// </summary>
        public override string GameMenu_ExitGame => "Выйти из игры";

        public override string Hud_Save => "Сохранить";
        public override string GameMenu_SaveStateWarnings => "Внимание! Файлы сохранения будут утеряны при обновлении игры.";
        public override string GameMenu_LoadState => "Загрузить";
        public override string GameMenu_ContinueFromSave => "Продолжить с сохранения";

        public override string GameMenu_AutoSave => "Автосохранение";

        public override string GameMenu_Load_PlayerCountError => "Вы должны настроить соответствующее количество игроков для файла сохранения: {0}";

        public override string Progressbar_MapLoadingState => "Загрузка карты: {0}";

        public override string Progressbar_ProgressComplete => "завершено";

        /// <summary>
        /// 0: прогресс в процентах, 1: количество неудач
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Генерация: {0}%. (Неудачи {1})";

        /// <summary>
        /// 0: текущая часть, 1: количество частей
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "часть {0}/{1}";

        /// <summary>
        /// 0: Процент или Завершено
        /// </summary>
        public override string Progressbar_SaveProgress => "Сохранение: {0}";

        /// <summary>
        /// 0: Процент или Завершено
        /// </summary>
        public override string Progressbar_LoadProgress => "Загрузка: {0}";

        /// <summary>
        /// Прогресс завершен, ожидание ввода игрока
        /// </summary>
        public override string Progressbar_PressAnyKey => "Нажмите любую клавишу для продолжения";

        /// <summary>
        /// Краткий учебник, в котором нужно купить и переместить солдата. Все расширенные команды заблокированы до завершения обучения.
        /// </summary>
        public override string Tutorial_MenuOption => "Пройти учебник";
        public override string Tutorial_MissionsTitle => "Учебные задания";
        public override string Tutorial_Mission_BuySoldier => "Выберите город и наймите солдата";
        public override string Tutorial_Mission_MoveArmy => "Выберите армию и переместите ее";

        public override string Tutorial_CompleteTitle => "Учебник завершен!";
        public override string Tutorial_CompleteMessage => "Разблокированы полный зум и расширенные опции игры.";

        /// <summary>
        /// Отображает ввод кнопки
        /// </summary>
        public override string Tutorial_SelectInput => "Выбрать";
        public override string Tutorial_MoveInput => "Команда перемещения";

        /// <summary>
        /// Против. Текст, описывающий две армии, которые вступят в бой
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "Объявление войны";

        public override string ArmyOption_Attack => "Атака";

        /// <summary>
        /// Меню настроек игры. Измените, что делают клавиши и кнопки при нажатии
        /// </summary>
        public override string Settings_ButtonMapping => "Назначение кнопок";



        /// <summary>
        /// Тип ввода, стандартный ПК ввод
        /// </summary>
        public override string Input_Source_Keyboard => "Клавиатура и мышь";

        /// <summary>
        /// Тип ввода, портативный контроллер, как используется на Xbox
        /// </summary>
        public override string Input_Source_Controller => "Контроллер";

        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */

        public override string CityMenu_SalePricesTitle => "Цены на продажу";
        public override string Blueprint_Title => "Чертеж";
        public override string Resource_Tab_Overview => "Обзор";
        public override string Resource_Tab_Stockpile => "Запасы";

        public override string Resource => "Ресурс";
        public override string Resource_StockPile_Info => "Установите целевое количество для хранения ресурсов; это сообщит рабочим, когда переключиться на другой ресурс.";
        public override string Resource_TypeName_Water => "вода";
        public override string Resource_TypeName_Wood => "дерево";
        public override string Resource_TypeName_Fuel => "топливо";
        public override string Resource_TypeName_Stone => "камень";
        public override string Resource_TypeName_RawFood => "сырая еда";
        public override string Resource_TypeName_Food => "еда";
        public override string Resource_TypeName_Beer => "пиво";
        public override string Resource_TypeName_Wheat => "пшеница";
        public override string Resource_TypeName_Linen => "лен";
        //public override string Resource_TypeName_SkinAndLinen => "кожа и лен";
        public override string Resource_TypeName_IronOre => "железная руда";
        public override string Resource_TypeName_GoldOre => "золотая руда";
        public override string Resource_TypeName_Iron => "железо";

        public override string Resource_TypeName_SharpStick => "Острый палка";
        public override string Resource_TypeName_Sword => "Меч";
        public override string Resource_TypeName_KnightsLance => "Копье рыцаря";
        public override string Resource_TypeName_TwoHandSword => "Двуручный меч";
        public override string Resource_TypeName_Bow => "Лук";

        public override string Resource_TypeName_LightArmor => "Легкая броня";
        public override string Resource_TypeName_MediumArmor => "Средняя броня";
        public override string Resource_TypeName_HeavyArmor => "Тяжелая броня";

        public override string ResourceType_Children => "Дети";

        public override string BuildingType_DefaultName => "Здание";
        public override string BuildingType_WorkerHut => "Хижина рабочих";
        public override string BuildingType_Tavern => "Таверна";
        public override string BuildingType_Brewery => "Пивоварня";
        public override string BuildingType_Postal => "Почтовая служба";
        public override string BuildingType_Recruitment => "Центр набора";
        public override string BuildingType_Barracks => "Казармы";
        public override string BuildingType_PigPen => "Свинарник";
        public override string BuildingType_HenPen => "Курятник";
        public override string BuildingType_WorkBench => "Верстак";
        public override string BuildingType_Carpenter => "Плотник";
        public override string BuildingType_CoalPit => "Угольная шахта";
        public override string DecorType_Statue => "Статуя";
        public override string DecorType_Pavement => "Тротуар";
        public override string BuildingType_Smith => "Кузница";
        public override string BuildingType_Cook => "Повар";
        public override string BuildingType_Storage => "Склад";

        public override string BuildingType_ResourceFarm => "Ферма {0}";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Увеличивает лимит рабочих на {0}";
        public override string BuildingType_Tavern_Description => "Здесь рабочие могут есть";
        public override string BuildingType_Tavern_Brewery => "Производство пива";
        public override string BuildingType_Postal_Description => "Отправляйте ресурсы в другие города";
        public override string BuildingType_Recruitment_Description => "Отправляйте людей в другие города";
        public override string BuildingType_Barracks_Description => "Используйте людей и снаряжение для набора солдат";
        public override string BuildingType_PigPen_Description => "Производит свиней, которые дают еду и кожу";
        public override string BuildingType_HenPen_Description => "Производит кур и яйца, которые дают еду";
        public override string BuildingType_Decor_Description => "Украшение";
        public override string BuildingType_Farm_Description => "Выращивайте ресурс";

        public override string BuildingType_Cook_Description => "Станция приготовления пищи";
        public override string BuildingType_Bench_Description => "Станция создания предметов";

        public override string BuildingType_Smith_Description => "Станция обработки металлов";
        public override string BuildingType_Carpenter_Description => "Станция обработки древесины";

        public override string BuildingType_Nobelhouse_Description => "Дом для рыцарей и дипломатов";
        public override string BuildingType_CoalPit_Description => "Эффективное производство топлива";
        public override string BuildingType_Storage_Description => "Пункт сдачи ресурсов";

        public override string MenuTab_Info => "Информация";
        public override string MenuTab_Work => "Работа";
        public override string MenuTab_Recruit => "Набор";
        public override string MenuTab_Resources => "Ресурсы";
        public override string MenuTab_Trade => "Торговля";
        public override string MenuTab_Build => "Строительство";
        public override string MenuTab_Economy => "Экономика";
        public override string MenuTab_Delivery => "Доставка";

        public override string MenuTab_Build_Description => "Размещайте здания в вашем городе";
        public override string MenuTab_BlackMarket_Description => "Размещайте здания в вашем городе";
        public override string MenuTab_Resources_Description => "Размещайте здания в вашем городе";
        public override string MenuTab_Work_Description => "Размещайте здания в вашем городе";
        public override string MenuTab_Automation_Description => "Размещайте здания в вашем городе";

        public override string BuildHud_OutsideCity => "За пределами города";
        public override string BuildHud_OutsideFaction => "За пределами вашей территории!";

        public override string BuildHud_OccupiedTile => "Занятая клетка";

        public override string Build_PlaceBuilding => "Построить";
        public override string Build_DestroyBuilding => "Разрушить";
        public override string Build_ClearTerrain => "Очистить местность";

        public override string Build_ClearOrders => "Очистить заказы на строительство";
        public override string Build_Order => "Заказ на строительство";
        public override string Build_OrderQue => "Очередь заказов на строительство: {0}";
        public override string Build_AutoPlace => "Автоматическая расстановка";

        public override string Work_OrderPrioTitle => "Приоритет работы";
        public override string Work_OrderPrioDescription => "Приоритет варьируется от 1 (низкий) до {0} (высокий)";

        public override string Work_OrderPrio_No => "Нет приоритета. Работы не будет.";
        public override string Work_OrderPrio_Min => "Минимальный приоритет.";
        public override string Work_OrderPrio_Max => "Максимальный приоритет.";

        public override string Work_Move => "Переместить предметы";

        public override string Work_GatherXResource => "Собрать {0}";
        public override string Work_CraftX => "Создать {0}";
        public override string Work_Farming => "Сельское хозяйство";
        public override string Work_Mining => "Горное дело";
        public override string Work_Trading => "Торговля";

        public override string Work_AutoBuild => "Автоматическое строительство и расширение";

        public override string WorkerHud_WorkType => "Статус работы: {0}";
        public override string WorkerHud_Carry => "Перенос: {0} {1}";
        public override string WorkerHud_Energy => "Энергия: {0}";
        public override string WorkerStatus_Exit => "Покинуть рабочую силу";
        public override string WorkerStatus_Eat => "Есть";
        public override string WorkerStatus_Till => "Вспахать";
        public override string WorkerStatus_Plant => "Посадить";
        public override string WorkerStatus_Gather => "Собрать";
        public override string WorkerStatus_PickUpResource => "Поднять ресурс";
        public override string WorkerStatus_DropOff => "Сдать";
        public override string WorkerStatus_BuildX => "Построить {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Вернуться в армию";

        public override string Hud_ToggleFollowFaction => "Переключить настройки следования фракции";
        public override string Hud_FollowFaction_Yes => "Настроено на использование глобальных настроек фракции";
        public override string Hud_FollowFaction_No => "Настроено на использование локальных настроек (Глобальное значение: {0})";

        public override string Hud_Idle => "Без дела";
        public override string Hud_NoLimit => "Без ограничений";

        public override string Hud_None => "Нет";
        public override string Hud_ProductionQueue => "Очередь производства";

        public override string Hud_EmptyList => "- Пустой список -";

        public override string Hud_RequirementOr => "- или -";

        public override string Hud_BlackMarket => "Черный рынок";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Выберите город";
        public override string Conscription_Title => "Призыв";
        public override string Conscript_WeaponTitle => "Оружие";
        public override string Conscript_ArmorTitle => "Броня";
        public override string Conscript_TrainingTitle => "Тренировка";

        public override string Conscript_SpecializationTitle => "Специализация";
        public override string Conscript_SpecializationDescription => "Увеличивает атаку в одной области, снижая эффективность в остальных на {0}";
        public override string Conscript_SelectBuilding => "Выберите казармы";

        public override string Conscript_WeaponDamage => "Урон оружия: {0}";
        public override string Conscript_ArmorHealth => "Прочность брони: {0}";
        public override string Conscript_TrainingSpeed => "Скорость атаки: {0}";
        public override string Conscript_TrainingTime => "Время тренировки: {0}";

        public override string Conscript_Training_Minimal => "Минимум";
        public override string Conscript_Training_Basic => "Базовая";
        public override string Conscript_Training_Skillful => "Умелая";
        public override string Conscript_Training_Professional => "Профессиональная";

        public override string Conscript_Specialization_Field => "Открытое поле";
        public override string Conscript_Specialization_Sea => "Море";
        public override string Conscript_Specialization_Siege => "Осада";
        public override string Conscript_Specialization_Traditional => "Традиционная";
        public override string Conscript_Specialization_AntiCavalry => "Против кавалерии";

        public override string Conscription_Status_CollectingEquipment => "Сбор снаряжения: {0}";
        public override string Conscription_Status_CollectingMen => "Сбор солдат: {0}";
        public override string Conscription_Status_Training => "Тренировка: {0}";

        public override string ArmyHud_Food_Reserves_X => "Запасы продовольствия: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Расход продовольствия: {0}";
        public override string ArmyHud_Food_Costs_X => "Стоимость продовольствия: {0}";

        public override string Deliver_WillSendXInfo => "Будет отправлено {0} за раз";
        public override string Delivery_ListTitle => "Выберите службу доставки";
        public override string Delivery_DistanceX => "Расстояние: {0}";
        public override string Delivery_DeliveryTimeX => "Время доставки: {0}";
        public override string Delivery_SenderMinimumCap => "Минимальный предел отправителя";
        public override string Delivery_RecieverMaximumCap => "Максимальный предел получателя";
        public override string Delivery_ItemsReady => "Предметы готовы";
        public override string Delivery_RecieverReady => "Получатель готов";
        public override string Hud_ThisCity => "Этот город";
        public override string Hud_RecieveingCity => "Город-получатель";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "Отображено в ресурсах в секунду.";

        public override string Info_MinuteAverage => "Значение представляет собой среднее за последнюю минуту.";

        public override string Message_OutOfFood_Title => "Закончилась еда";
        public override string Message_CityOutOfFood_Text => "Дорогостоящая еда будет куплена на черном рынке. Рабочие будут умирать от голода, когда у вас закончатся деньги.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Тип местности";

        public override string Hud_EnergyUpkeepX => "Расход энергии на еду {0}";

        public override string Hud_EnergyAmount => "{0} энергии (секунд работы)";

        public override string Hud_CopySetup => "Скопировать настройки";
        public override string Hud_Paste => "Вставить";

        public override string Hud_Available => "Доступно";

        public override string WorkForce_ChildBirthRequirements => "Требования к рождению детей";
        public override string WorkForce_AvailableHomes => "Доступные дома: {0}";
        public override string WorkForce_Peace => "Мир";
        public override string WorkForce_ChildToManTime => "Возраст совершеннолетия: {0} минут";

        public override string Economy_TaxIncome => "Налоговый доход: {0}";
        public override string Economy_ImportCostsForResource => "Стоимость импорта {0}: {1}";
        public override string Economy_BlackMarketCostsForResource => "Стоимость на черном рынке {0}: {1}";
        public override string Economy_GuardUpkeep => "Содержание охраны: {0}";

        public override string Economy_LocalCityTrade_Export => "Экспорт из города: {0}";
        public override string Economy_LocalCityTrade_Import => "Импорт в город: {0}";

        public override string Economy_ResourceProduction => "Производство {0}: {1}";
        public override string Economy_ResourceSpending => "Расход {0}: {1}";

        public override string Economy_TaxDescription => "Налог составляет {0} золота за работника";

        public override string Economy_SoldResources => "Проданные ресурсы (золотая руда): {0}";

        public override string UnitType_Cities => "Города";
        public override string UnitType_Armies => "Армии";
        public override string UnitType_Worker => "Рабочий";

        public override string UnitType_FootKnight => "Рыцарь с мечом";
        public override string UnitType_CavalryKnight => "Рыцарь на коне";

        public override string CityCulture_LargeFamilies => "Большие семьи";
        public override string CityCulture_FertileGround => "Плодородные земли";
        public override string CityCulture_Archers => "Опытные лучники";
        public override string CityCulture_Warriors => "Воины";
        public override string CityCulture_AnimalBreeder => "Животноводы";
        public override string CityCulture_Miners => "Шахтеры";
        public override string CityCulture_Woodcutters => "Лесорубы";
        public override string CityCulture_Builders => "Строители";
        public override string CityCulture_CrabMentality => "Менталитет краба";
        public override string CityCulture_DeepWell => "Глубокий колодец";
        public override string CityCulture_Networker => "Сетевик";
        public override string CityCulture_PitMasters => "Мастера по топливу";

        public override string CityCulture_CultureIsX => "Культура: {0}";
        public override string CityCulture_LargeFamilies_Description => "Увеличивается рождаемость";
        public override string CityCulture_FertileGround_Description => "Урожайность выше";
        public override string CityCulture_Archers_Description => "Производит опытных лучников";
        public override string CityCulture_Warriors_Description => "Производит опытных бойцов ближнего боя";
        public override string CityCulture_AnimalBreeder_Description => "Животные дают больше ресурсов";
        public override string CityCulture_Miners_Description => "Больше добычи руды";
        public override string CityCulture_Woodcutters_Description => "Больше древесины";
        public override string CityCulture_Builders_Description => "Быстрее строят";
        public override string CityCulture_CrabMentality_Description => "Работа требует меньше энергии. Невозможно производить высококвалифицированных солдат.";
        public override string CityCulture_DeepWell_Description => "Вода восстанавливается быстрее";
        public override string CityCulture_Networker_Description => "Эффективная почтовая служба";
        public override string CityCulture_PitMasters_Description => "Высокая производительность топлива";

        public override string CityOption_AutoBuild_Work => "Автоматическое расширение рабочей силы";
        public override string CityOption_AutoBuild_Farm => "Автоматическое расширение ферм";

        public override string Hud_PurchaseTitle_Resources => "Купить ресурсы";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "В вашем распоряжении";

        public override string Tutorial_EndTutorial => "Завершить обучение";
        public override string Tutorial_MissionX => "Задание {0}";
        public override string Tutorial_CollectXAmountOfY => "Соберите {0} {1}";
        public override string Tutorial_SelectTabX => "Выберите вкладку: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Увеличьте приоритет на: {0}";
        public override string Tutorial_PlaceBuildOrder => "Разместите заказ на строительство: {0}";
        public override string Tutorial_ZoomInput => "Масштабирование";

        public override string Tutorial_SelectACity => "Выберите город";
        public override string Tutorial_ZoomInWorkers => "Увеличьте масштаб, чтобы увидеть рабочих";
        public override string Tutorial_CreateSoldiers => "Создайте два отряда солдат с этим снаряжением: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Уменьшите масштаб для обзора карты";
        public override string Tutorial_ZoomOutDiplomacy => "Уменьшите масштаб для дипломатического обзора";
        public override string Tutorial_ImproveRelations => "Улучшите отношения с соседней фракцией";
        public override string Tutorial_MissionComplete_Title => "Задание выполнено!";
        public override string Tutorial_MissionComplete_Unlocks => "Новые функции разблокированы";

        //patch1
        public override string Resource_ReachedStockpile => "Достигнут резервный запас цели";

        public override string BuildingType_ResourceMine => "{0} шахта";

        public override string Resource_TypeName_BogIron => "Болотная железная руда";

        public override string Resource_TypeName_Coal => "Уголь";

        public override string Language_XUpkeepIsY => "{0} содержание: {1}";
        public override string Language_XCountIsY => "{0} количество: {1}";

        public override string Message_ArmyOutOfFood_Text => "Дорогая еда будет закупаться на черном рынке. Голодные солдаты дезертируют, когда закончатся деньги.";

        public override string Info_ArmyFood => "Армии будут пополнять запасы еды из ближайшего дружественного города. Пищу можно покупать у других фракций. Во враждебных регионах еду можно купить только на черном рынке.";

        public override string FactionName_Monger => "Торговец";
        public override string FactionName_Hatu => "Хату";
        public override string FactionName_Destru => "Дестру";

        //patch2
        public override string Tutorial_BuildSomething => "Постройте что-то, что производит {0}";
        public override string Tutorial_BuildCraft => "Постройте мастерскую для: {0}";
        public override string Tutorial_IncreaseBufferLimit => "Увеличьте буферный лимит для: {0}";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "Достигните запаса {0} {1}";
        public override string Tutorial_LookAtFoodBlueprint => "Посмотрите на чертежи еды";
        public override string Tutorial_CollectFood_Info1 => "Рабочие пойдут в ратушу, чтобы поесть";
        public override string Tutorial_CollectFood_Info2 => "Армия отправляет поддерживающих работников для сбора еды";
        public override string Tutorial_CollectFood_Info0 => "Хотите полный контроль над рабочими? Установите все приоритеты работы на ноль, а затем активируйте по одному.";

        public override string EndGameStatistics_DecorsBuilt => "Построено декораций: {0}";
        public override string EndGameStatistics_StatuesBuilt => "Построено статуй: {0}";


        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "По умолчанию рабочие идут в мэрию, чтобы поесть или сдать предметы";
        public override string GameMenu_UseSpeedX => "Опция скорости {0}";
        public override string GameMenu_LongerBuildQueue => "Расширенная очередь строительства";

        public override string Diplomacy_RelationWithOthers => "Их отношения с другими";
        public override string Automation_queue_description => "Будет повторяться, пока очередь не опустеет";

        public override string BuildingType_Storehouse_Description => "Рабочие могут оставлять предметы здесь";

        public override string Resource_TypeName_Longbow => "длинный лук";
        public override string Resource_TypeName_Rapeseed => "рапс";
        public override string Resource_TypeName_Hemp => "конопля";

        public override string Resource_BogIronDescription => "Добыча железа более эффективна, чем использование болотной руды.";

        public override string Resource_FoodSafeGuard_Description => "Защита. Максимизирует приоритет цепочки производства пищи, если он упадет ниже {0}.";
        public override string Resource_FoodSafeGuard_Active => "Защита активирована.";

        public override string GameMenu_NextSong => "Следующая песня";

        public override string BuildingType_Bank => "Банк";
        public override string BuildingType_GoldDelivery_Description => "Отправлять золото в другие города";

        public override string BuildingType_Logistics => "Логистика";
        public override string BuildingType_Logistics_Description => "Улучшите вашу способность заказывать строительство";

        public override string BuildingType_Logistics_NationSizeRequirement => "Общая рабочая сила страны: {0}";
        public override string Requirements_XItemStorageOfY => "Хранение в городе {0}: {1}";

        public override string XP_UnlockBuildQueue => "Разблокировать очередь строительства до: {0}";
        public override string XP_UnlockBuilding => "Разблокировать здание: ";
        public override string XP_Upgrade => "Улучшение";

        public override string XP_UpgradeBuildingX => "Улучшить здание: {0}";

        public override string BuildHud_PerCycle => "За цикл";
        public override string BuildHud_MayCraft => "Можно изготовить";
        public override string BuildHud_WorkTime => "Время работы: {0}";
        public override string BuildHud_GrowTime => "Время роста: {0}";
        public override string BuildHud_Produce => "Производить:";

        public override string BuildHud_Queue => "Допустимая очередь строительства: {0}/{1}";

        public override string LandType_Flatland => "Равнина";
        public override string LandType_Water => "Вода";
        public override string BuildingType_Wall => "Стена";
        public override string Delivery_AutoReciever_Description => "Отправлять в город с наименьшим количеством ресурсов";

        public override string Hud_On => "Включено";
        public override string Hud_Off => "Выключено";

        public override string Hud_Time_Seconds => "{0} секунд";
        public override string Hud_Time_Minutes => "{0} минут";
        public override string Hud_Undo => "Отменить";
        public override string Hud_Redo => "Повторить";

        public override string Tag_ViewOnMap => "Посмотреть теги на карте";

        public override string MenuTab_Tag => "Тег";

        public override string Input_Build => "Строить";

        public override string FlagEditor_ClearAll => "Очистить всё";

        public override string CityCulture_Stonemason => "Каменщик";
        public override string CityCulture_Stonemason_Description => "Улучшенная добыча камня";

        public override string CityCulture_Brewmaster => "Пивовар";
        public override string CityCulture_Brewmaster_Description => "Улучшенное производство пива";

        public override string CityCulture_Weavers => "Ткачи";
        public override string CityCulture_Weavers_Description => "Улучшенное производство легкой брони";

        public override string CityCulture_SiegeEngineer => "Инженер осадных машин";
        public override string CityCulture_SiegeEngineer_Description => "Более мощные осадные машины";

        public override string CityCulture_Armorsmith => "Бронник";
        public override string CityCulture_Armorsmith_Description => "Улучшенное производство железной брони";

        public override string CityCulture_Noblemen => "Дворяне";
        public override string CityCulture_Noblemen_Description => "Более мощные рыцари";

        public override string CityCulture_Seafaring => "Мореходство";
        public override string CityCulture_Seafaring_Description => "Солдаты с морской специализацией имеют более сильные корабли";

        public override string CityCulture_Backtrader => "Черный торговец";
        public override string CityCulture_Backtrader_Description => "Дешевый черный рынок";

        public override string CityCulture_LawAbiding => "Законопослушный";
        public override string CityCulture_LawAbiding_Description => "Больше налогов. Черный рынок недоступен.";




        public override string Hud_Advanced => "Расширенные";
        public override string Hud_Loading => "Загрузка...";

        public override string CityOption_LowerGuardSize => "Отпустить стражу";
        public override string Hud_Purchase_MinCapacity => "Достигнут минимальный объем";
        public override string Settings_ResetToDefault => "Сбросить настройки";
        public override string Settings_NewGame => "Новая игра";

        public override string Settings_AdvancedGameSettings => "Расширенные настройки игры";
        public override string Settings_FoodMultiplier => "Множитель еды";
        public override string Settings_FoodMultiplier_Description => "Сколько времени работник или солдат может продержаться на полном желудке. Высокое значение снизит производительность компьютера.";

        public override string Settings_GameMode => "Режим игры";

        public override string Settings_Mode_Story => "Полная история";
        public override string Settings_Mode_IncludeBoss => "События с боссами.";
        public override string Settings_Mode_IncludeAttacks => "Случайные атаки.";
        public override string Settings_Mode_Sandbox => "Песочница";
        public override string Settings_Mode_Peaceful => "Мирный";
        public override string Settings_Mode_Peaceful_Description => "Все войны начинаются по инициативе игрока";

        public override string Lobby_ImportSave => "Импортировать сохранение";

        public override string Lobby_ExportSave => "Экспортировать сохранение";
        public override string Lobby_ExportSave_Description => "Создает копию файла и помещает ее в папку импорта: {0}";

        public override string Resource_CurrentAmount => "Текущее количество: {0}";
        public override string Resource_MaxAmount_Soft => "Мягкий предел (Макс. лимит): {0}";
        public override string Resource_MaxAmount => "Максимальный лимит: {0}";
        public override string Resource_AddPerSec => "Скорость увеличения: {0} в секунду";

        public override string Resource_WaterAddLimit => "Скорость увеличения воды изменить нельзя";

        public override string Tutorial_Select_SubTab => "Выберите подкатегорию: {0}";



        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */

        public override string Tutorial_OpenGuardSubTab => "Откройте казарму и выберите категорию: {0}";
        public override string Tutorial_GuardToWall => "Переместите охранника на стену";
        public override string Demo_MissionObjective_Title => "Цель миссии";
        public override string Demo_MissionObjective_Description => "Защитите себя от атаки с юга";
        public override string Demo_Complete_Title => "Демо завершено";
        public override string Demo_TimesUp_Title => "Время вышло!";
        public override string Demo_EndInOneMinuteDescription => "Демо завершится через одну минуту";

        public override string ArmyOption_NewArmy => "Новая армия";
        public override string ProfileEditor_AltMain => "Альтернативный основной";
        public override string Automation_CheckBoxTitle => "Автоматически";

        public override string ArmyStructure_ColumnWidth => "Ширина колонны армии";
        public override string ArmyStructure_ArmyPlacement => "Расположение в армии";
        public override string ArmyStructure_Row_Front => "Передний ряд";
        public override string ArmyStructure_Row_Body => "Центр";
        public override string ArmyStructure_Row_Second => "Второй ряд";
        public override string ArmyStructure_Row_Behind => "Задний ряд";

        public override string Diplomacy_RelationType_Enemies => "Враги";

        public override string EventMessage_EnemyAlliance_Title => "Страх перед доминированием";
        public override string EventMessage_EnemyAlliance => "Государства, опасаясь вашего растущего могущества, объединились в альянс против вас.";

        public override string Settings_CentralGold => "Централизованное золото";
        public override string Settings_CentralGold_Description => "Вкл.: всё золото в общем пуле и доступно сразу. Выкл.: золото является физическим и требует транспортировки.";

        public override string InputActionName_StopStart => "Старт/Стоп";
        public override string InputActionName_ToggleHudDetail => "Переключить подробности HUD";
        public override string InputActionName_NextCity => "Следующий город";
        public override string InputActionName_NextArmy => "Следующая армия";
        public override string InputActionName_NextBattle => "Следующее сражение";
        public override string InputActionName_Build => "Строить";
        public override string InputActionName_Copy => "Копировать";
        public override string InputActionName_Paste => "Вставить";
        public override string InputActionName_Menu => "Меню";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "Предыдущий цвет";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "Следующий цвет";
        public override string InputActionName_FlagDesign_PaintBucket => "Заливка";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "Палитра";
        public override string InputActionName_ControllerFocus => "Фокус";
        public override string InputActionName_ControllerCancel => "Отмена";
        public override string InputActionName_ControllerMessageClick => "Щелчок по сообщению";
        public override string InputActionName_ControllerSelect => "Выбрать";
        public override string InputActionName_WASD_UP => "Вверх";
        public override string InputActionName_WASD_DOWN => "Вниз";
        public override string InputActionName_WASD_LEFT => "Влево";
        public override string InputActionName_WASD_RIGHT => "Вправо";
        public override string InputActionName_CameraTiltLeft => "Наклон камеры влево";
        public override string InputActionName_CameraTiltRight => "Наклон камеры вправо";
        public override string InputActionName_CameraTiltUp => "Наклон камеры вверх";
        public override string InputActionName_ZoomInKey => "Приблизить";
        public override string InputActionName_ZoomOutKey => "Отдалить";

        public override string Settings_Title_Monitor => "Настройки монитора";
        public override string Settings_Title_Graphics => "Графические настройки";
        public override string Settings_Title_Input => "Настройки управления";
        public override string Settings_Title_Gameplay => "Настройки геймплея";
        public override string Settings_PanOnZoom => "Прокрутка при увеличении";
        public override string Settings_ScrollSensitivity_Game => "Чувствительность прокрутки: игра";
        public override string Settings_ScrollSensitivity_Menu => "Чувствительность прокрутки: меню";
        public override string Settings_Blood => "Кровь";

        public override string Settings_MasterVolume => "Общая громкость";
        public override string Settings_AmbienceVolume => "Громкость окружения";
        public override string Settings_BattleMelody => "Мелодия сражения";

        public override string Settings_ModelLight => "Эффект освещения модели";
        public override string Settings_Particles => "Эффекты частиц";
        public override string Settings_MapLoadSpeed => "Скорость загрузки карты";
        public override string Lobby_Category_Options => "Параметры";
        public override string Lobby_Category_Editor => "Редактор";
        public override string Lobby_Category_ExtraModes => "Дополнительные режимы";

        public override string Lobby_Editor_MapEditor => "Редактор карт";
        public override string Lobby_Editor_VoxelEditor => "Редактор вокселей";

        public override string Lobby_Mode_BattleLab => "Боевой полигон";
        public override string Lobby_Mode_BattleLab_Description => "Сразитесь любыми солдатами друг с другом";
        public override string Lobby_Mode_Commander => "Командир";
        public override string Lobby_Mode_Commander_Description => "Небольшая тактическая настольная игра";
        public override string Lobby_MusicPlayList => "Плейлист музыки";

        public override string Lobby_GameSetup => "Настройка игры";
        public override string Lobby_PlayerSetup => "Настройка игрока";
        public override string LobbyDemoMode_Demo => "Демо";

        public override string Lobby_Tutorial => "Обучение";

        public override string LobbyDemoMode_ShortTutorial => "Краткое обучение";
        public override string LobbyDemoMode_LongTutorial => "Расширенное обучение";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "Добавить в список желаемого";
        public override string BattleLab_StartHere => "Начать битву здесь";
        public override string BattleLab_Start => "Начать битву";
        public override string BattleLab_Attacker => "Атакующий";

        public override string MapGenerator_Name => "Редактор карт — генерация";

        public override string MapType_CustomMap => "Пользовательская карта";
        public override string MapType_GenerateNewMap => "Сгенерировать новую карту";
        public override string MapGenerator_GenerateAction => "Генерировать";
        public override string MapGenerator_Terrain_CustomSize => "Пользовательский размер";
        public override string MapGenerator_Terrain_StartAs => "Начать как";
        public override string MapGenerator_Terrain_ClearPass => "Выполнить очистку";
        public override string MapGenerator_Terrain_BuildPass => "Выполнить построение";
        public override string MapGenerator_Terrain_DigPass => "Выполнить выемку";
        public override string MapGenerator_Terrain_BuildDigLoops => "Количество циклов постройки/выемки";
        public override string MapGenerator_Terrain_BuildStrokes => "Количество штрихов постройки";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Измеряется в штрихах на 100 плиток";
        public override string MapGenerator_Terrain_DigStrokes => "Количество штрихов выемки";
        public override string MapGenerator_Terrain_CleanUp_Option => "Очистка одиночных плиток";
        public override string MapGenerator_Terrain_CleanUpPass => "Выполнить проход очистки";

        public override string Economy_ServicemenUpkeep => "Содержание обслуживающего персонала: {0}";
        public override string Economy_ServicemenUpkeep_Description => "Содержание составляет {0} золота за одного работника";
        public override string Economy_GuardUpkeep_Description => "Содержание составляет {0} золота за одного стражника";

        public override string EndScreen_TimeHasEndedTitle => "Время вышло";

        public override string Hud_AdvancedSettings => "Дополнительные настройки";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "Отмена";
        public override string Hud_Delete => "Удалить";
        public override string Hud_Next => "Далее";
        //public override string Hud_None => "Нет";
        public override string Hud_Apply => "Применить";
        public override string Hud_AllCities => "Все города";
        public override string Hud_Time_Hours => "{0} часов";
        public override string Hud_AddX => "Добавить {0}";
        public override string Hud_Both => "Оба";
        public override string Hud_Direction => "Направление";
        public override string MusicIsBroken => "Музыка сейчас не работает";

        /// <summary>
        /// 0: тип объектов, 1: количество
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, количество: {1}";

        public override string Hud_EffectDoesNotStack => "Этот эффект не суммируется";

        public override string Work_SmeltX => "Плавить {0}";

        public override string Info_TotalFoodProduction => "Общее производство пищи";
        public override string Info_TotalFoodSpending => "Общее потребление пищи";

        public override string Info_FooodAndDeliveryLocation => "По умолчанию рабочие идут в ратушу, чтобы поесть или отнести предметы";

        public override string Delivery_SendChunk => "Предметов за доставку";
        public override string Delivery_SpeedBonus => "Бонус к скорости: {0}%";

        public override string Delivery_AutoResourceDescription => "Доставляет предметы, достигшие предела хранения, в нуждающиеся города";

        public override string Conscript_Soldiers_ArmyType => "Армейские солдаты";
        public override string Conscript_Soldiers_ArmyType_Description => "Призвать солдат в соседнюю армию";
        public override string Conscript_Soldiers_GuardType => "Городская стража";
        public override string Conscript_Soldiers_GuardType_Description => "Стража используется для укрепления стен";

        public override string Defence_Title => "Оборона";
        public override string Defence_GuardPost => "Караульный пост";

        public override string Defence_WallDescription_Movement => "Замедляет передвижение врага.";
        public override string Defence_WallDescription_GuardPost => "Здесь можно разместить стражу.";
        public override string Defence_AutoAssign => "Автоназначение";
        public override string Defence_AutoAssign_Description => "Новые стражи будут направлены на этот пост";

        public override string Conscript_SplashDamage => "Урон по области";
        public override string Conscript_HighSplashDamage => "Высокий урон по области";

        public override string Conscript_Training_Champion => "Чемпион";
        public override string Conscript_Training_Legendary => "Легендарный";

        public override string Experience_Title => "Опыт";
        public override string Experience_TopExperience => "Максимальные уровни опыта";

        public override string Experience_TimeReductionDescription => "Время работы сокращается на {0}% за каждый уровень";

        public override string ExperienceType_Farm => "Фермер";
        public override string ExperienceType_AnimalCare => "Уход за животными";
        public override string ExperienceType_HouseBuilding => "Строитель домов";
        public override string ExperienceType_WoodWork => "Плотник";
        public override string ExperienceType_StoneCutter => "Каменотёс";
        public override string ExperienceType_Mining => "Шахтёр";
        public override string ExperienceType_Transport => "Транспортировщик";
        public override string ExperienceType_Cook => "Повар";
        public override string ExperienceType_Fletcher => "Стрелок";
        public override string ExperienceType_RefineOre => "Плавильщик";
        public override string ExperienceType_Casting => "Литейщик";
        public override string ExperienceType_CraftMetal => "Кузнец";
        public override string ExperienceType_CraftArmor => "Бронник";
        public override string ExperienceType_CraftWeapon => "Оружейник";
        public override string ExperienceType_CraftFuel => "Угольщик";
        public override string ExperienceType_Chemist => "Химик";

        public override string ExperienceLevel_1 => "Новичок";
        public override string ExperienceLevel_2 => "Практик";
        public override string ExperienceLevel_3 => "Эксперт";
        public override string ExperienceLevel_4 => "Мастер";
        public override string ExperienceLevel_5 => "Легендарный";

        public override string ExperenceOrDistancePrio_Title => "Выбор работника";
        public override string ExperenceOrDistancePrio_Description => "Свободные работники будут выбраны по расстоянию или опыту";

        public override string Technology_Description => "У каждого города есть технологическое дерево. Технологии открывают здания и предметы.";
        public override string Experience_Description => "Рабочие получают опыт и становятся лучше";

        public override string Technology_Title => "Технологии";
        public override string Technology_ShareField => "Общий технологический сектор";

        public override string Technology_GainByNeigborRelation => "За каждый город-сосед с этой технологией. Если ваши отношения {0}: {1}";
        public override string Technology_ForEachMaster => "Когда {0} достигает уровня опыта {1}, в технологической области: {2}";
        public override string Technology_CitySpread => "Ваши города обмениваются технологиями, если они соседствуют: {0}";
        public override string Technology_CityCapture => "При захвате города в бою большинство технологий утрачиваются";

        public override string Technology_AdvancedBuildings => "Продвинутое строительство";
        public override string Technology_AdvancedFarming => "Продвинутое земледелие";
        public override string Technology_AdvancedCasting => "Продвинутая литейка";


        public override string Help_Title => "Помощь";
        public override string Help_Work_Title => "Работа не начинается";
        public override string Help_Work_Resources => "Для строительства нужны доступные ресурсы";
        public override string Help_Work_Skill => "Работнику необходим соответствующий уровень навыка (или выше)";
        public override string Help_Work_Stockpile => "Сбор ресурсов блокируется из-за переполненного склада";
        public override string Help_Work_Priority => "Работа может иметь низкий или нулевой приоритет";

        public override string Help_Soldiers_Title => "Создание солдат";
        public override string Help_Soldiers_PlaceBuildingX => "Постройте здание: {0}";
        public override string Help_Soldiers_Workers => "Доступные рабочие для набора";
        public override string Help_Soldiers_Weapon => "Каждому солдату требуется оружие";
        public override string Help_Soldiers_StartX => "Начало: {0}";

        public override string Hud_SelectHistory => "Выбрать историю";

        public override string Hud_PointsPerMinute => "{0} очков в минуту";
        public override string Hud_PercentValueCost => "Услуга стоит {0}% от стоимости";

        public override string Hud_Mixed => "Смешанный";
        public override string Hud_Distance => "Расстояние";

        public override string Hud_Unlock => "Разблокировать";
        public override string Hud_category => "Категория";

        /// <summary>
        /// Устанавливает скорость игры по одному кадру
        /// </summary>
        public override string Input_StepOneFrame => "Пошагово: 1 кадр";

        public override string Resource_TypeName_Wagon2Wheel => "Малый фургон";
        public override string Resource_TypeName_Wagon4Wheel => "Большой фургон";
        public override string Resource_TypeName_Tin => "Олово";
        public override string Resource_TypeName_TinOre => "Оловянная руда";

        public override string Resource_TypeName_Copper => "Медь";
        public override string Resource_TypeName_CopperOre => "Медная руда";
        public override string Resource_TypeName_SilverOre => "Серебряная руда";
        public override string Resource_TypeName_Silver => "Серебро";

        /// <summary>
        /// Мифрил — это фантастический металл
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Неочищенный мифрил";
        public override string Resource_TypeName_Mithril => "Мифрил";

        public override string Resource_TypeName_BronzeSword => "Бронзовый меч";
        public override string Resource_TypeName_ShortSword => "Короткий меч";
        public override string Resource_TypeName_LongSword => "Длинный меч";
        public override string Resource_TypeName_HandSpear => "Ручное копьё";
        public override string Resource_TypeName_Warhammer => "Боевой молот";
        public override string Resource_TypeName_MithrilSword => "Мифриловый меч";
        public override string Resource_TypeName_SlingShot => "Праща";
        public override string Resource_TypeName_ThrowingSpear => "Метательное копьё";
        public override string Resource_TypeName_Crossbow => "Арбалет";
        public override string Resource_TypeName_MithrilBow => "Мифриловый лук";

        public override string Resource_TypeName_CoolingFluid => "Охлаждающая жидкость";
        public override string Resource_TypeName_Palisade => "Палисад";
        public override string Resource_TypeName_Toolkit => "Набор инструментов";

        public override string Resource_TypeName_Sulfur => "Сера";
        public override string Resource_TypeName_LeadOre => "Свинцовая руда";
        public override string Resource_TypeName_Lead => "Свинец";
        public override string Resource_TypeName_Bronze => "Бронза";
        public override string Resource_TypeName_BloomIron => "Крица";
        public override string Resource_TypeName_Steel => "Сталь";
        public override string Resource_TypeName_CastIron => "Чугун";

        public override string Resource_TypeName_BlackPowder => "Чёрный порох";
        public override string Resource_TypeName_GunPowder => "Порох";
        public override string Resource_TypeName_LedBullet => "Пуля";

        public override string Resource_TypeName_HandCannon => "Ручная пушка";
        public override string Resource_TypeName_HandCulverin => "Ручной кулеврин";
        public override string Resource_TypeName_Rifle => "Ружьё";
        public override string Resource_TypeName_Blunderbuss => "Аркебуза";

        public override string Resource_TypeName_Manuballista => "Манубаллиста";
        public override string Resource_TypeName_Catapult => "Катапульта";
        public override string Resource_TypeName_BatteringRam => "Таран";
        public override string Resource_TypeName_SiegeCannonBronze => "Базилиск";
        public override string Resource_TypeName_ManCannonBronze => "Бомбарда";
        public override string Resource_TypeName_SiegeCannonIron => "Гаубица";
        public override string Resource_TypeName_ManCannonIron => "Пушка";

        public override string Resource_TypeName_PaddedArmor => "Стёганая броня";
        public override string Resource_TypeName_HeavyPaddedArmor => "Тяжёлая стёганая броня";

        public override string Resource_TypeName_IronArmor => "Кольчуга";
        public override string Resource_TypeName_HeavyIronArmor => "Тяжёлая кольчуга";

        public override string Resource_TypeName_BronzeArmor => "Бронзовая броня";

        public override string Resource_TypeName_LightPlateArmor => "Лёгкая пластинчатая броня";
        public override string Resource_TypeName_FullPlateArmor => "Полная пластинчатая броня";
        public override string Resource_TypeName_MithrilArmor => "Мифриловая броня";
        public override string Resource_TypeName_Coin => "Монета";

        public override string UnitType_Warhammer => "Рыцарь с молотом";
        public override string UnitType_MithrilKnight => "Бессмертный рыцарь";
        public override string UnitType_MithrilArcher => "Бессмертный лучник";
        public override string UnitType_SpearAndShield => "Копейщик с щитом";

        public override string UnitType_CollectionOfSoldiers => "Отряд солдат";
        public override string UnitType_CollectionOfArmies => "Сборная армия";

        /// <summary>
        /// Идентификатор будет уникальным числом
        /// </summary>
        public override string UnitId => "(ID {0})";

        public override string BuildHud_AreaEffectTitle => "Эффект области";
        public override string BuildHud_BonusRadius => "Радиус бонуса: {0}";

        public override string BuildHud_BuildTime => "Время строительства";
        public override string SchoolHud_ToLevel => "До уровня";
        public override string SchoolHud_TimeDescription => "Время указано для нулевого опыта; уменьшается с ростом опыта.";
        public override string SchoolHud_SelectSchool => "Выбрать школу";
        public override string Upgrade_Order => "Порядок улучшения";

        public override string Building_ListDescription => "Список всех зданий в данной категории";

        public override string BuildingType_IsUpgraded => "{0} – улучшено";
        public override string BuildingType_WoodCutter => "Лесопилка";
        public override string BuildingType_Workshop_Description => "Улучшает работу в окрестностях";

        public override string BuildingType_WoodCutter_AreaAffect => "Добыча древесины +{0}% от деревьев";

        public override string BuildingType_StoneCutter_AreaAffect => "Добыча камня +{0}%";

        public override string BuildingType_StoneCutter => "Каменоломня";

        public override string BuildingType_Embassy => "Посольство";
        public override string BuildingType_Embassy_Description => "Для дипломатических отношений";

        public override string BuildingType_SoldierBarracks => "Казарма (пехота)";
        public override string BuildingType_ArcherBarracks => "Казарма (лучники)";
        public override string BuildingType_WarmachineBarracks => "Казарма (осадные орудия)";
        public override string BuildingType_GunBarracks => "Казарма (стрелки)";
        public override string BuildingType_CannonBarracks => "Казарма (артиллерия)";
        public override string BuildingType_KnightsBarracks => "Казарма (рыцари)";

        public override string BuildingType_WaterResovoir => "Водохранилище";
        public override string BuildingType_WaterResovoir_Description => "Увеличивает запас воды";

        public override string BuildingType_SmeltingFurnace => "Плавильная печь";
        public override string BuildingType_SmeltingFurnace_Description => "Очистка руды для получения металла";

        public override string BuildingType_Foundry => "Литейная";
        public override string BuildingType_Foundry_Description => "Мастерская по литью металла";

        public override string BuildingType_Armory => "Оружейная";
        public override string BuildingType_Armory_Description => "Производство доспехов";

        public override string BuildingType_Chemist => "Алхимическая лаборатория";
        public override string BuildingType_Chemist_Description => "Создание химических веществ";

        public override string BuildingType_CoinMaker => "Монетный двор";
        public override string BuildingType_CoinMaker_Description => "Преобразование металлов в деньги";

        public override string BuildingType_Gunmaker => "Оружейная мастерская";
        public override string BuildingType_Gunmaker_Description => "Производство огнестрельного оружия и пушек";

        public override string BuildingType_School_Tab => "Школа";
        public override string BuildingType_School => "Гильдия мастеров";
        public override string BuildingType_School_Description => "Повышает уровень навыков рабочих";

        public override string BuildingType_GoldDelivery => "Золотой курьер";
        public override string BuildingType_Bank_Description => "Управление золотым запасом";

        public override string DecorType_CobbleStones => "Брусчатка";
        public override string DecorType_Square => "Городская площадь";

        public override string DecorType_Garden => "Сад";
        public override string DecorType_Flag => "Флаг";
        public override string DecorType_Banner => "Знамя";

        public override string BuildingType_DirtRoad => "Грунтовая дорога";
        public override string BuildingType_Palisade => "Палисадная крепость";

        public override string ResourceType_ServiceMen => "Служащие";
        public override string BuildingType_ServiceHouse => "Дом обслуживания";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "Добавляет служащих: {0}";

        public override string BuildingType_GuardOffice => "Офис охраны";
        public override string BuildingType_GuardOffice_DescriptionAddX => "Увеличивает лимит охраны на {0}";

        public override string BuildingType_DirtWall => "Глиняная стена";
        public override string BuildingType_DirtTower => "Глиняная башня";
        public override string BuildingType_WoodWall => "Деревянная стена";
        public override string BuildingType_WoodTower => "Деревянная башня";
        public override string BuildingType_StoneWall => "Каменная стена";
        public override string BuildingType_StoneTower => "Каменная башня";
        public override string BuildingType_StoneGate => "Каменные ворота";
        public override string BuildingType_StoneHouse => "Каменный дом";

        /// <summary>
        /// При отображении небольших вариаций, например «Лампа A» и «Лампа B»
        /// </summary>
        public override string VariantType_A => "{0} A";
        public override string VariantType_B => "{0} B";
        public override string VariantType_C => "{0} C";
        public override string VariantType_D => "{0} D";
        public override string VariantType_E => "{0} E";
        public override string VariantType_F => "{0} F";
        public override string VariantType_G => "{0} G";
        public override string VariantType_H => "{0} H";

        public override string BuildingToolShape_Free => "Свободная форма";
        public override string BuildingToolShape_Area => "Прямоугольник";
        public override string BuildingToolShape_Line => "Линия";
        public override string BuildingToolShape_LShape => "L-образная форма";

        public override string CityHall_Upgrade => "Улучшить ратушу";

        /// <summary>
        /// Ограничение на количество рабочих в городе
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Макс. число рабочих: {0}";

        public override string CityHall_Size_Small => "Деревня";
        public override string CityHall_Size_Medium => "Город";
        public override string CityHall_Size_Large => "Столица";

        public override string GuardHousingCount => "Места для охраны";
        public override string ServicemenCount => "Служащие: {0}";

        public override string Work_MiningResource => "Добыча ресурса: {0}";

        public override string MenuTab_Progress => "Прогресс";

        public override string Automation_AutomateCity => "Автоматизировать город";
        public override string Automation_AutomationFocus => "Фокус автоматизации";
        public override string Automation_AutomationFocus_Grow => "Развитие";
        public override string Automation_AutomationFocus_Export => "Экспорт";
        public override string Automation_AutomationFocus_War => "Война";

        public override string CityCulture_Smelters_Description => "Улучшенная выплавка руды";
        public override string CityCulture_Smelters => "Плавильщики";

        public override string CityCulture_Apprentices_Description => "Новые рабочие получают опыт от активных работников";
        public override string CityCulture_Apprentices => "Подмастерья";

        public override string CityCulture_BronzeCasters_Description => "Улучшенное производство бронзы и бронзовых изделий";
        public override string CityCulture_BronzeCasters => "Бронзолитейщики";

        //DEMO PATCH 1
        /// <summary>
        /// Злые орки, блуждающие по карте
        /// </summary>
        public override string FactionName_Barbarian => "Тёмная Орда";
        public override string Tutorial_AttackAndDestroyX => "Атакуйте и уничтожьте: {0}";
        public override string Resource_TypeName_Pike => "Пика";

        public override string BattleTrials_Title => "Испытания в бою";
        public override string BattleTrials_Description => "Проверьте свои тактические навыки в сражении армия против армии.";

        //DEMO PATCH 2

        public override string Conscript_BlockReducingAttack => "Эти атаки снижают шанс блока";

        public override string Conscript_BlockPerSecond => "Может блокировать до {0} раз в секунду";

        public override string Conscript_BlockDescription => "Солдаты блокируют большинство атак, идущих с фронта";

        public override string Map_CustomSeed => "Сид карты";

        public override string Settings_Mode_Spectator => "Наблюдатель";

        public override string Settings_Mode_Spectator_Description => "Только наблюдать";

        public override string Automation_AutomationFocus_NoFocus_Description => "Будет понемногу строить всё";

        public override string Automation_AutomationFocus_WillProduce => "Основное производство:";

        public override string Help_Food_WhoEats => "Все солдаты и рабочие потребляют еду";

        public override string Help_Food_BigArmy => "Большая армия может вызвать голод в городе в своей зоне";

        public override string Help_Food_DontBuild => "Дополнительные фермы не увеличивают количество еды автоматически — нужны доступные рабочие и кухни для сбора и переработки";

        public override string Help_Food_UseWater => "Для производства еды требуется вода";

        public override string Help_Food_Postal => "Убедитесь, что города поддерживают друг друга, отправляя еду";

        public override string Message_LostCity => "Город потерян";

        public override string Demo_Description => "Короткий сценарий: защитите свой город в течение {0} минут";

        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "Демонстрация завершится через {0} минут";

        public override string Experience_Required => "Требуемый опыт";

        public override string InputActionName_ToggleMenu => "Переключить меню";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "Ресурсы могут уходить в минус и немного превышать лимит склада. Ограничения применяются только при создании очереди задач.";

        public override string Work_SelectCategory => "Выберите категорию предметов";
        public override string Hud_RemoveFromList => "Удалить из списка";

        public override string Hud_ReturnToPrevious => "Назад";
        public override string Hud_Close => "Закрыть";

        public override string Hud_Low => "Низкий";
        public override string Hud_Medium => "Средний";
        public override string Hud_High => "Высокий";

        public override string Hud_Copy => "Копировать";
        //public override string Hud_Paste => "Вставить";
        public override string Hud_Cut => "Вырезать";
        public override string Hud_SaveCompleted => "Сохранение завершено";

        public override string Settings_WaterMultiplier => "Коэффициент воды";
        public override string Settings_WaterMultiplier_Description => "Определяет, сколько воды производят и хранят города. Более высокие значения снижают производительность.";

        public override string Settings_ChildMultiplier => "Коэффициент рождаемости";
        public override string Settings_CraftMultiplier => "Коэффициент скорости производства";
        public override string Settings_CraftMultiplier_Description => "Меньшие значения ускоряют производство.";

        public override string FastProduction => "Быстрое производство";
        public override string SlowProduction => "Медленное производство";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "Не будет производиться";

        //public override string CityAutomation_WaitForMaxPopulation => "Ожидать достижения максимального населения";
        public override string Automation_AutomationFocus_NoFocus => "Все";
        public override string CityAutomation_SoldierQuality => "Качество солдат";
        public override string CityAutomation_SoldierWeaponType => "Тип оружия";

        public override string WarsResourceGroup_Resources => "Ресурсы";
        public override string WarsResourceGroup_Weapons => "Оружие";

        public override string WarsResourceGroup_AllWeaponTypes => "Смешанное";
        public override string WarsResourceGroup_MeleeHandWeapons => "Ближний бой";
        public override string WarsResourceGroup_RangedHandWeapons => "Дальний бой";
        public override string WarsResourceGroup_Warmachines => "Военные машины";

        public override string FactionSettings_Titel => "Настройки фракции";
        public override string FactionSettings_Description => "Применяется ко всем вашим городам";

        public override string Conscript_MaxPopulation => "Максимальное население";
        public override string Conscript_MaxPopulation_Description => "Призыв осуществляется только при достижении максимального населения";

        public override string Conscript_FoodAbundance => "Максимальный запас еды";
        public override string Conscript_FoodAbundance_Description => "Призыв осуществляется только при максимальных запасах еды";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Установлено: Вкл.";
        public override string GeneralSetting_Off => "Установлено: Выкл.";
        public override string GeneralSetting_AllBuildingsDescription => "Будет применено ко всем зданиям";

        public override string GeneralSetting_ApplyMessage => "Изменение применено к {0} зданиям";

        public override string MustTurnOffSteamInput => "Чтобы использовать контроллеры, необходимо отключить Steam Input.";

        public override string Technology_GainTitle => "Способы получения технологий";
        public override string Technology_LevelUp => "Повышение уровня";
        public override string Technology_ForEachLevelUp => "Когда работник повышает уровень в области технологий: {0}";

        public override string VoxelEditor_Description => "Создание блочных моделей";

        public override string Editor_Tool => "Инструмент";
        public override string Editor_SelectOptionsMenu => "Параметры выбора";
        public override string Editor_Continous => "Непрерывно";

        public override string Editor_Tool_PencilSize => "Размер кисти";
        public override string Editor_Tool_SizeTolerance => "Допуск размера";
        public override string Editor_Tool_RoundPencil => "Круглая кисть";
        public override string Editor_Tool_EdgeSize => "Размер края";
        public override string Editor_Tool_PercentFill => "Процент заполнения";
        public override string Editor_Tool_ClearAbove => "Очистить сверху";
        public override string Editor_Tool_FillBelow => "Заполнить снизу";

        public override string Editor_UserModels => "Модели пользователя";
        public override string Editor_UserModels_Description => "Просмотр сохранённых моделей";

        public override string Editor_RetailModels => "Игровые модели";
        public override string Editor_RetailModels_Description => "Загрузка моделей из игры";

        public override string Editor_ModTemplates => "Шаблоны для моддинга";
        public override string Editor_ExportAsOBJ => "Экспортировать как .OBJ";
        public override string Editor_SelectAll => "Выделить всё";

        public override string Editor_Canvas_Title => "Холст";
        public override string Editor_Canvas_Size => "Размер";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "Пресеты размера";
        public override string Editor_Canvas_Move => "Переместить";
        public override string Editor_Canvas_Move_Up => "Вверх";
        public override string Editor_Canvas_Move_Down => "Вниз";
        public override string Editor_Canvas_RotateClockwise => "Повернуть по часовой";
        public override string Editor_Canvas_RotateCounterClockwise => "Повернуть против часовой";
        public override string Editor_Canvas_Mirror => "Отразить";

        public override string Editor_Canvas_RotateFlip_Title => "Повернуть/Отразить";
        public override string Editor_Canvas_FlipVertical => "Отразить по вертикали";
        public override string Editor_Canvas_FlipOrientation => "Перевернуть (вертик./гориз.)";
        public override string Editor_Canvas_ClearAll_Description => "Удаляет все блоки и кадры";

        public override string Editor_Animation => "Анимация";
        public override string Editor_Animation_RemoveCurrentFrame => "Удалить текущий кадр";
        public override string Editor_Animation_AddFrameCopy => "Добавить копию кадра";
        public override string Editor_Animation_AddEmptyFrame => "Добавить пустой кадр";
        public override string Editor_Animation_MoveDescription => "Изменить позицию кадра";
        public override string Editor_Animation_AllFrames => "Все кадры";
        public override string Editor_Animation_AllFrames_ActionDescription => "Выполнить действие для всех кадров";

        public override string Editor_SettingsMenu => "Настройки";
        public override string Hud_Exit => "Выход";
        public override string Editor_Canvas_Clear => "Очистить";

        public override string Editor_Stamp => "Штамп";
        public override string Editor_StampOtherFrames => "Штамповать в другие кадры";
        public override string Editor_StampOtherFrames_Description => "Вставить воксели в эти кадры";
        public override string Editor_PasteToFrame => "Вставить в текущий кадр";
        public override string Editor_ClearAllFrames => "Очистить во всех кадрах";
        public override string Editor_ClearOtherFrames => "Очистить другие кадры";

        public override string Editor_Settings_MoveSpeed => "Скорость перемещения";
        public override string Editor_Settings_BackgroundColor => "Цвет фона";
        public override string Editor_Settings_HideHUD => "Скрыть HUD";

        public override string Editor_Color => "Цвет";
        public override string Editor_ColorsInUseLabel => "Используемые цвета";
        public override string Editor_Color_BrighterPlus => "Ярче +";
        public override string Editor_Color_Brighter => "Ярче";
        public override string Editor_Color_Darker => "Темнее";
        public override string Editor_Color_DarkerPlus => "Темнее +";
        public override string Editor_Color_RedTint => "Красный оттенок";
        public override string Editor_Color_Tint => "Оттенок";
        public override string Editor_Color_GreenTint => "Зелёный оттенок";
        public override string Editor_Color_BlueTint => "Синий оттенок";
        public override string Editor_Color_YellowTint => "Жёлтый оттенок";
        public override string Editor_Color_PurpleTint => "Фиолетовый оттенок";
        public override string Editor_NoColor => "Пусто";

        public override string Editor_Material => "Материал";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "Перекрасить";
        public override string Editor_Color_RecolorTo => "Перекрасить в";

        public override string Editor_Material_Set => "Назначить материал";

        public override string Editor_Preview => "Превью";
        public override string Editor_CombineWithCurrent => "Объединить с текущей моделью";

        public override string Editor_PickedColor => "Выбранный";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "Палатка иммиграции";
        public override string BuildingType_ImmigrationTent_Description => "Вмещает {0} иммигрантов";
        public override string BuildingType_ReseachCenter => "Научный центр";
        public override string BuildingType_Bookpress => "Печатный станок";
        public override string BuildingType_Bookpress_Description => "В одной исследовательской области все полученные очки делятся между всеми {0} в других ваших городах.";

        public override string Technology_ReseachExample => "Пример: когда рабочий производит {0}, улучшается его навык {1}. При повышении уровня добавляются очки к технологии {2}, так как они используют одно и то же направление.";

        public override string BuildingType_Research_BaseDescription => "Увеличивает исследование технологий.";

        public override string BuildingType_ResearchCenter_Description => "Добавляет {0} дополнительных очков технологии, когда рабочий повышает уровень в той же области.";


        //DEMO PATCH 5
        public override string Editor_CropSelection => "Обрезать по выделению";

        public override string Immigrants_DisbandedSoldiers => "Распущенные солдаты будут иммигрировать";
        public override string Immigrants_RefillWorkers => "Быстро восполняет рабочую силу";
        public override string Immigrants_UnhousedAreLost => "Иммигранты без жилья со временем исчезнут";
        public override string Editor_VoxelCount => "{0} вокселей";

        public override string Editor_Layers_Titel => "Слои";
        public override string Editor_Layers_All => "Все слои";
        public override string Editor_LayerNumber => "Слой {0}";

        public override string Editor_Layer_AddEmpty => "Добавить пустой слой";
        public override string Editor_Layer_AddCopy => "Дублировать слой";
        public override string Editor_Layer_Remove => "Удалить слой";
        public override string Editor_Layer_MergeDown => "Объединить с нижним";
        public override string Editor_IsAnimated => "Анимированный";
        public override string Editor_ToggleVisible => "Переключить видимость";
        public override string Editor_ToggleAnimatedLayer => "Переключить анимированный слой";
        public override string Editor_Projects => "Файлы проектов";
        public override string ProfileEditor_ReplaceMaterial => "Цвет профиля: {0}";

        public override string ProfileEditor_ProfileColors_Label => "Цвета профиля";
        public override string ProfileEditor_TunicColor => "Цвет туники";
        public override string ProfileEditor_PantsColor => "Цвет штанов";
        public override string ProfileEditor_LeaderColor => "Цвет лидера";

        public override string MapStartAs_Water => "Вода";
        public override string MapStartAs_Land => "Земля";
        public override string MapStartAs_Circle => "Круг";

        public override string Hud_NeedToBeAssigned => "Требуется назначение";
        public override string Hud_CommitAssignment => "Назначить";
        public override string Technology_NoAvailableResearch => "Нет доступных исследований";

        public override string Research_Tab => "Исследования";

        //5.2
        public override string BuildCategory_General => "Общее";
        public override string BuildCategory_Military => "Военное";
        public override string BuildCategory_Decoration => "Украшения";
        public override string BuildCategory_Upgrade => "Улучшения";
        public override string Work_NoMines => "Нет шахт";




    }
}
