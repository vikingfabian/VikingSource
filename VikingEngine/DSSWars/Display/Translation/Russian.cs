using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
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
        public override string Lobby_LocalMultiplayerEdit => "Локальный мультиплеер ({0})";

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
        public override string ProfileEditor_SaveAndExit => "Сохранить и выйти";

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

        public override string Hud_IncreaseMaxGuardCount => "Максимальный размер стражи +{0}";

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
        public override string Hud_Immigrants => "Иммигранты: {0}";

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
        /// Описывает ввод кнопки. Перейти к следующему городу.
        /// </summary>
        public override string Input_NextCity => "Следующий город";

        /// <summary>
        /// Описывает ввод кнопки. Перейти к следующей армии.
        /// </summary>
        public override string Input_NextArmy => "Следующая армия";

        /// <summary>
        /// Описывает ввод кнопки. Перейти к следующему сражению.
        /// </summary>
        public override string Input_NextBattle => "Следующее сражение";

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
        public override string EventMessage_DesertersText => "Неоплаченные солдаты дезертируют из ваших армий";

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

        public override string GameMenu_SaveState => "Сохранить";
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
        /// Описывает ввод кнопки. Расширяет или уменьшает количество информации на HUD
        /// </summary>
        public override string Input_ToggleHudDetail => "Переключить детализацию HUD";

        /// <summary>
        /// Описывает ввод кнопки. Переключает выбор между картой и HUD
        /// </summary>
        public override string Input_ToggleHudFocus => "Фокус меню";

        /// <summary>
        /// Описывает ввод кнопки. Ярлык для нажатия на последнее всплывающее окно
        /// </summary>
        public override string Input_ClickMessage => "Нажать сообщение";

        /// <summary>
        /// Описывает ввод кнопки. Общее направление движения
        /// </summary>
        public override string Input_Up => "Вверх";

        /// <summary>
        /// Описывает ввод кнопки. Общее направление движения
        /// </summary>
        public override string Input_Down => "Вниз";

        /// <summary>
        /// Описывает ввод кнопки. Общее направление движения
        /// </summary>
        public override string Input_Left => "Влево";

        /// <summary>
        /// Описывает ввод кнопки. Общее направление движения
        /// </summary>
        public override string Input_Right => "Вправо";

        /// <summary>
        /// Тип ввода, стандартный ПК ввод
        /// </summary>
        public override string Input_Source_Keyboard => "Клавиатура и мышь";

        /// <summary>
        /// Тип ввода, портативный контроллер, как используется на Xbox
        /// </summary>
        public override string Input_Source_Controller => "Контроллер";
    }
}
