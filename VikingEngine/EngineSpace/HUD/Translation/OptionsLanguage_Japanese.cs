using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;

namespace VikingEngine.HUD
{
    class OptionsLanguage_Japanese : AbsOptionsLanguage
    {
        public override string Hud_Cancel => "キャンセル";
        public override string Hud_Back => "戻る";

        /// <summary>
        /// プレイヤーが破壊的な選択をする時のサブメニュー
        /// </summary>
        public override string Hud_AreYouSure => "本当に実行しますか？";

        public override string Hud_OK => "OK";
        public override string Hud_Yes => "はい";
        public override string Hud_No => "いいえ";

        /// <summary>
        /// オプションメニューのタイトル
        /// </summary>
        public override string Options_title => "オプション";

        /// <summary>
        /// ゲームコントロール入力オプション、0: 現在の入力
        /// </summary>
        public override string InputSelect => "入力: {0}";

        /// <summary>
        /// ゲーム入力のタイプ
        /// </summary>
        public override string InputKeyboardMouse => "キーボードとマウス";

        /// <summary>
        /// ゲーム入力のタイプ
        /// </summary>
        public override string InputController => "コントローラー";

        /// <summary>
        /// ゲーム入力が選択されていません
        /// </summary>
        public override string InputNotSet => "未設定";

        /// <summary>
        /// チェックボックスのラベル。ローカル分割画面プレイのオプション。
        /// </summary>
        public override string VerticalSplitScreen => "垂直画面分割";

        /// <summary>
        /// サウンドスライダーのラベル
        /// </summary>
        public override string SoundOption_MusicVolume => "音楽の音量";

        /// <summary>
        /// サウンドスライダーのラベル
        /// </summary>
        public override string SoundOption_SoundVolume => "サウンドの音量";

        /// <summary>
        /// 画面解像度
        /// </summary>
        public override string GraphicsOption_Resolution => "解像度";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// ゲームを全画面表示またはウィンドウモードで表示
        /// </summary>
        public override string GraphicsOption_Fullscreen => "全画面表示";

        /// <summary>
        /// オーバーサイズは、マルチモニターサポートのためにゲームウィンドウをモニターより大きくします
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "幅のオーバーサイズ";
        public override string GraphicsOption_PercentageOversizeWidth => "幅の{0}%";
        public override string GraphicsOption_OversizeHeight => "高さのオーバーサイズ";
        public override string GraphicsOption_PercentageOversizeHeight => "高さの{0}%";
        public override string GraphicsOption_Oversize_None => "なし";

        /// <summary>
        /// Youtubeに録画するための特定の解像度
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "録画プリセット";

        /// <summary>
        /// 0: 高さ解像度
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// テキストとアイコンのサイズを変更
        /// </summary>
        public override string GraphicsOption_UiScale => "UIスケール";
    }
}
