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
        public override string Hud_Queue => "Очередь";

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

        public override string WorkForce_ChildBirthRequirements => "Требования к рождению детей:";
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

    }
}
