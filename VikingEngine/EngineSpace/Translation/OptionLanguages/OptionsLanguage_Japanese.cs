using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Japanese : AbsOptionsLanguage
    {
        public override string Settings_ControllerVibration => "コントローラーの振動";
        public override string GraphicsOption_IngameMenuWidth => "ゲーム内メニューの幅";
        public override string DisplayMode => "表示モード";
        public override string DisplayMode_Windowed => "ウィンドウモード";
        public override string DisplayMode_BorderlessFullscreen => "ボーダーレスフルスクリーン";
        public override string GameSettings_RenderedMouseCursor => "レンダーされたカーソル";

        //--
        public override string GraphicsOption_FarViewDistance => "遠距離ビュー";

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

        public override string ReversedStereo => "ステレオ反転";
        public override string Option_Low => "低";
        public override string Option_Medium => "中";
        public override string Option_High => "高";

        public override string MouseSettings_Title => "マウス入力";
        public override string KeyboardSettings_Title => "キー割り当て";

        public override string MouseButtonAction_None => "なし";
        public override string MouseButtonAction_Select => "選択";
        public override string MouseButtonAction_Pan => "画面移動";
        public override string MouseButtonAction_PanAndOrder => "画面移動と指示";
        public override string MouseButtonAction_Order => "指示";
        public override string MouseButtonAction_Cancel => "キャンセル";

        public override string MouseButton_Left => "左クリック";
        public override string MouseButton_Right => "右クリック";
        public override string MouseButton_Middle => "中央クリック";
        public override string MouseButton_X1 => "X1ボタン";
        public override string MouseButton_X2 => "X2ボタン";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "パンとキャンセル";
        public override string MouseButtonAction_PanAndOrderAndCancel => "パン、命令、キャンセル";

        public override string GraphicsOption_Shadows => "影";
        public override string GraphicsOption_ShadowType_ModelsToGround => "モデルの影（地面）";
        public override string GraphicsOption_ShadowType_ModelsToModels => "モデルの影（他モデル）";

        public override string GraphicsOption_Shadow_MapResolution => "シャドウマップ解像度";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "{0} ピクセル追加";
        public override string Settings_KeyMapPanSpeed => "パン速度";
        public override string Settings_StoreCameraPosition => "カメラ位置を保存";
        public override string Settings_LoadCameraPosition => "位置を読み込む";


        //Shadow update
        public override string Settings_ModelWaterFoam => "水の泡";
        public override string Settings_ModelShadow => "影";
        public override string Settings_ModelShadowMapSize => "シャドウマップのサイズ";
        public override string Settings_Brightness => "明るさ";
        public override string Settings_Mode_No_Achivements => "実績は利用できません。";
        public override string Settings_FrameRate => "フレームレート";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "インポートしたセーブデータでは実績をブロックする";


    }
}
