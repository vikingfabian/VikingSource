using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Russian : AbsOptionsLanguage
    {
        public override string GameSettings_UltraWide => "Ультраширокий";
        public override string GameSettings_UltraWide_LeftEdge => "Левый край UI";
        public override string GameSettings_UltraWide_RightEdge => "Правый край UI";
        //-------
        public override string GameSettings_WideScrollbar => "Широкая полоса прокрутки";
        public override string GameSettings_DisplayInputHelp => "Помощь по управлению";
        public override string GameSettings_InputSmoothing => "Сглаживание ввода";

        //Mounts
        public override string InputSteam => "Steam Input";
        public override string Input_SimulateMouse => "Симуляция мыши";
        public override string Input_LockMouseToWindow => "Привязать мышь к окну";
        public override string Input_MouseEdgePush_Title => "Скроллинг у края экрана";
        public override string Input_NoControl => "Нет";
        public override string Input_ActiveControl => "Активно";
        public override string Input_PassiveControl => "Пассивно";
        public override string Setting_MinimapScale => "Масштаб миникарты";
        //##Settings
        public override string Settings_Particles_FadeMapLayers => "Затухание слоев";
        public override string SplitScreen_HorizontalFirst => "Сначала горизонтально";
        public override string SplitScreen_VerticalFirst => "Сначала вертикально";
        public override string SplitScreen_HorizontalOnly => "Только горизонтально";
        public override string SplitScreen_VerticalOnly => "Только вертикально";
        public override string SplitScreen_Title => "Разделение экрана";
        public override string SplitScreen_AdjustSplit => "Настройка разделения {0}";

        public override string Settings_ControllerVibration => "Вибрация геймпада";
        public override string GraphicsOption_IngameMenuWidth => "Ширина игрового меню";
        public override string DisplayMode => "Режим отображения";
        public override string DisplayMode_Windowed => "Оконный режим";
        public override string DisplayMode_BorderlessFullscreen => "Полный экран без рамок";
        public override string GameSettings_RenderedMouseCursor => "Отрисованный курсор";
        public override string GameSettings_MuteControllerDisconnect => "Скрыть ошибки геймпада";
        //--
        public override string GraphicsOption_FarViewDistance => "Дальний обзор";

        public override string Hud_Cancel => "Отмена";
        public override string Hud_Back => "Назад";

        /// <summary>
        /// Подменю для ситуаций, когда игрок будет делать разрушительные выборы
        /// </summary>
        public override string Hud_AreYouSure => "Вы уверены?";

        public override string Hud_OK => "ОК";
        public override string Hud_Yes => "Да";
        public override string Hud_No => "Нет";

        /// <summary>
        /// Заголовок меню настроек
        /// </summary>
        public override string Options_title => "Настройки";

        /// <summary>
        /// Опции ввода игрового управления, 0: текущий ввод
        /// </summary>
        public override string InputSelect => "Ввод: {0}";

        /// <summary>
        /// Тип ввода игры
        /// </summary>
        public override string InputKeyboardMouse => "Клавиатура и мышь";

        /// <summary>
        /// Тип ввода игры
        /// </summary>
        public override string InputController => "Контроллер";

        /// <summary>
        /// Ввод для игры не выбран
        /// </summary>
        public override string InputNotSet => "Не установлено";

        /// <summary>
        /// Метка для флажка. Опция для локального режима разделенного экрана.
        /// </summary>
        public override string VerticalSplitScreen => "Вертикальное разделение экрана";

        /// <summary>
        /// Метка для ползунка громкости звука
        /// </summary>
        public override string SoundOption_MusicVolume => "Громкость музыки";

        /// <summary>
        /// Метка для ползунка громкости звука
        /// </summary>
        public override string SoundOption_SoundVolume => "Громкость звуков";

        /// <summary>
        /// Разрешение экрана
        /// </summary>
        public override string GraphicsOption_Resolution => "Разрешение";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// Отображение игры в полноэкранном или оконном режиме
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Полноэкранный режим";

        /// <summary>
        /// Избыточный размер сделает окно игры больше, чем монитор, для поддержки нескольких мониторов
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Избыточная ширина";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% ширина";
        public override string GraphicsOption_OversizeHeight => "Избыточная высота";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% высота";
        public override string GraphicsOption_Oversize_None => "Нет";

        /// <summary>
        /// Специальные разрешения для записи на Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Пресеты для записи";

        /// <summary>
        /// 0: высота разрешения
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// Изменить размер текста и иконок
        /// </summary>
        public override string GraphicsOption_UiScale => "Масштаб интерфейса";

        public override string ReversedStereo => "Обратное стерео";
        public override string Option_Low => "Низкое";
        public override string Option_Medium => "Среднее";
        public override string Option_High => "Высокое";

        public override string MouseSettings_Title => "Настройки мыши";
        public override string KeyboardSettings_Title => "Назначение клавиш";

        public override string MouseButtonAction_None => "Нет действия";
        public override string MouseButtonAction_Select => "Выбор";
        public override string MouseButtonAction_Pan => "Прокрутка";
        public override string MouseButtonAction_PanAndOrder => "Прокрутка и приказ";
        public override string MouseButtonAction_Order => "Приказ";
        public override string MouseButtonAction_Cancel => "Отмена";

        public override string MouseButton_Left => "Левая кнопка мыши";
        public override string MouseButton_Right => "Правая кнопка мыши";
        public override string MouseButton_Middle => "Средняя кнопка мыши";
        public override string MouseButton_X1 => "Кнопка мыши X1";
        public override string MouseButton_X2 => "Кнопка мыши X2";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Панорамирование и отмена";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Панорамирование, приказ и отмена";

        public override string GraphicsOption_Shadows => "Тени";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Тени от моделей на землю";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Тени от моделей на модели";

        public override string GraphicsOption_Shadow_MapResolution => "Разрешение карты теней";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "Добавить {0} пикселей";
        public override string Settings_KeyMapPanSpeed => "Скорость панорамирования";
        public override string Settings_StoreCameraPosition => "Сохранить позицию камеры";
        public override string Settings_LoadCameraPosition => "Загрузить позицию";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Пена на воде";
        public override string Settings_ModelShadow => "Тени";
        public override string Settings_ModelShadowMapSize => "Размер карты теней";
        public override string Settings_Brightness => "Яркость";
        public override string Settings_Mode_No_Achivements => "Достижения недоступны.";
        public override string Settings_FrameRate => "Частота кадров";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Блокировать достижения для импортированных сохранений";

    }
}
