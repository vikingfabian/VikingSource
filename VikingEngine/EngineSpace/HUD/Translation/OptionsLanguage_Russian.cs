using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;

namespace VikingEngine.EngineSpace.HUD.Translation
{
    class OptionsLanguage_Russian : AbsOptionsLanguage
    {
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
    }
}
