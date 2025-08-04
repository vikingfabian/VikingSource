using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Japanese : AbsLanguage
    {
        /// <summary>
        /// この言語の名前
        /// </summary>
        public override string MyLanguage => "英語";

        /// <summary>
        /// アイテム数の表示方法。0: アイテム, 1: 数量
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}: {1}";

        /// <summary>
        /// 言語オプションの選択
        /// </summary>
        public override string Lobby_Language => "言語";

        /// <summary>
        /// ゲームを開始する
        /// </summary>
        public override string Lobby_Start => "スタート";

        /// <summary>
        /// ローカルマルチプレイヤー数を選択するボタン, 0: 現在のプレイヤー数
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "ローカルマルチプレイヤー";

        /// <summary>
        /// 分割画面のプレイヤー数を選択するメニューのタイトル
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "プレイヤー数を選択";

        /// <summary>
        /// ローカルマルチプレイヤーの説明
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "マルチプレイヤーにはXboxコントローラーが必要です";

        /// <summary>
        /// 次の分割画面の位置に移動
        /// </summary>
        public override string Lobby_NextScreen => "次の画面位置";

        /// <summary>
        /// プレイヤーは外見を選択し、プロフィールに保存できます
        /// </summary>
        public override string Lobby_FlagSelectTitle => "フラッグを選択";

        /// <summary>
        /// 0: 1から16までの番号
        /// </summary>
        public override string Lobby_FlagNumbered => "フラッグ {0}";

        /// <summary>
        /// ゲーム名とバージョン番号
        /// </summary>
        public override string Lobby_GameVersion => "DSS ウォーパーティー - ver {0}";

        /// <summary>
        /// フラッグを塗り、軍隊の色を選択します。
        /// </summary>
        public override string FlagEditor_Description => "フラッグを塗り、軍隊の色を選択します。";

        /// <summary>
        /// 色を塗りつぶすツール
        /// </summary>
        public override string FlagEditor_Bucket => "バケツ";

        /// <summary>
        /// フラッグプロフィールエディタを開きます。
        /// </summary>
        public override string Lobby_FlagEdit => "フラッグを編集";

        public override string Lobby_WarningTitle => "警告";
        public override string Lobby_IgnoreWarning => "警告を無視";

        /// <summary>
        /// 一人のプレイヤーが入力を選択していないときの警告。
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "一人のプレイヤーが入力を選択していません";

        /// <summary>
        /// 多くのプレイヤーが使用しないコンテンツが含まれているメニュー。
        /// </summary>
        public override string Lobby_Extra => "追加コンテンツ";

        /// <summary>
        /// 追加コンテンツは翻訳されていないか、完全なコントローラーサポートがありません。
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "警告！このコンテンツはローカリゼーションや入力/アクセシビリティサポートが含まれていません";

        public override string Lobby_MapSizeTitle => "マップサイズ";

        /// <summary>
        /// マップサイズ1の名前
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "極小";

        /// <summary>
        /// マップサイズ2の名前
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "小";

        /// <summary>
        /// マップサイズ3の名前
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "中";

        /// <summary>
        /// マップサイズ4の名前
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "大";

        /// <summary>
        /// マップサイズ5の名前
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "巨大";

        /// <summary>
        /// マップサイズ6の名前
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "超巨大";

        /// <summary>
        /// マップサイズの説明 X by Y キロメートル。0: 幅, 1: 高さ
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";

        /// <summary>
        /// ゲームアプリケーションを終了
        /// </summary>
        public override string Lobby_ExitGame => "終了";

        /// <summary>
        /// ローカルマルチプレイヤー名を表示、0: プレイヤー番号
        /// </summary>
        public override string Player_DefaultName => "プレイヤー {0}";

        /// <summary>
        /// プレイヤープロファイルエディタにて。エディタオプションのメニューを開きます
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "オプション";

        /// <summary>
        /// プレイヤープロファイルエディタにて。フラッグの色を選択するタイトル
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "フラッグの色";

        /// <summary>
        /// プレイヤープロファイルエディタにて。フラッグの色オプション
        /// </summary>
        public override string ProfileEditor_MainColor => "メインカラー";

        /// <summary>
        /// プレイヤープロファイルエディタにて。フラッグの色オプション
        /// </summary>
        public override string ProfileEditor_Detail1Color => "ディテールカラー1";

        /// <summary>
        /// プレイヤープロファイルエディタにて。フラッグの色オプション
        /// </summary>
        public override string ProfileEditor_Detail2Color => "ディテールカラー2";

        /// <summary>
        /// プレイヤープロファイルエディタにて。兵士の色を選択するタイトル
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "兵士の色";

        /// <summary>
        /// プレイヤープロファイルエディタにて。兵士の色オプション
        /// </summary>
        public override string ProfileEditor_SkinColor => "肌の色";

        /// <summary>
        /// プレイヤープロファイルエディタにて。兵士の色オプション
        /// </summary>
        public override string ProfileEditor_HairColor => "髪の色";

        /// <summary>
        /// プレイヤープロファイルエディタにて。カラーパレットを開き、色を選択します
        /// </summary>
        public override string ProfileEditor_PickColor => "色を選択";

        /// <summary>
        /// プレイヤープロファイルエディタにて。画像の位置を調整します
        /// </summary>
        public override string ProfileEditor_MoveImage => "画像を移動";

        /// <summary>
        /// プレイヤープロファイルエディタにて。移動方向
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "左";

        /// <summary>
        /// プレイヤープロファイルエディタにて。移動方向
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "右";

        /// <summary>
        /// プレイヤープロファイルエディタにて。移動方向
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "上";

        /// <summary>
        /// プレイヤープロファイルエディタにて。移動方向
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "下";

        /// <summary>
        /// プレイヤープロファイルエディタにて。保存せずにエディタを閉じます
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "破棄して終了";

        /// <summary>
        /// プレイヤープロファイルエディタにて。破棄するためのツールチップ
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "すべての変更を元に戻します";

        /// <summary>
        /// プレイヤープロファイルエディタにて。変更を保存してエディタを閉じます
        /// </summary>
        public override string Hud_SaveAndExit => "保存して終了";

        /// <summary>
        /// プレイヤープロファイルエディタにて。色相、彩度、明度のカラーオプションの一部。
        /// </summary>
        public override string ProfileEditor_Hue => "色相";

        /// <summary>
        /// プレイヤープロファイルエディタにて。色相、彩度、明度のカラーオプションの一部。
        /// </summary>
        public override string ProfileEditor_Lightness => "明度";

        /// <summary>
        /// プレイヤープロファイルエディタにて。フラッグと兵士の色オプションを切り替えます。
        /// </summary>
        public override string ProfileEditor_NextColorType => "次の色タイプ";

        /// <summary>
        /// ゲームの現在の速度、実時間と比較して
        /// </summary>
        public override string Hud_GameSpeedLabel => "ゲーム速度: {0}x";

        public override string Input_GameSpeed => "ゲーム速度";

        /// <summary>
        /// ゲーム内表示。ユニットの金生産
        /// </summary>
        public override string Hud_TotalIncome => "総収入/秒: {0}";

        /// <summary>
        /// ユニットの金コスト。
        /// </summary>
        public override string Hud_Upkeep => "維持費: {0}";
        public override string Hud_ArmyUpkeep => "軍隊維持費: {0}";

        /// <summary>
        /// ゲーム内表示。建物を守る兵士。
        /// </summary>
        public override string Hud_GuardCount => "守衛";

        public override string Hud_IncreaseMaxGuardCount => "最大守衛数 {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "都市を拡張する必要があります。";

        public override string Hud_SoldierCount => "兵士数: {0}";

        public override string Hud_SoldierGroupsCount => "グループ数: {0}";

        /// <summary>
        /// ゲーム内表示。ユニットの計算された戦闘力。
        /// </summary>
        public override string Hud_StrengthRating => "戦力評価: {0}";

        /// <summary>
        /// ゲーム内表示。国全体の計算された戦闘力。
        /// </summary>
        public override string Hud_TotalStrengthRating => "軍事力: {0}";

        /// <summary>
        /// ゲーム内表示。都市国家外から来る追加の人々。
        /// </summary>
        public override string Hud_Immigrants => "移民";

        public override string Hud_CityCount => "都市数: {0}";
        public override string Hud_ArmyCount => "軍隊数: {0}";

        /// <summary>
        /// 購入を複数回繰り返すためのミニボタン。例："x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "必要条件";
        public override string Hud_PurchaseTitle_Cost => "コスト";
        public override string Hud_PurchaseTitle_Gain => "利益";

        /// <summary>
        /// 使用される資源の量、"5ゴールド。(利用可能: 10)"。テキストの上に「コスト」というタイトルが表示されます。0: 資源, 1: コスト, 2: 利用可能
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}。(利用可能: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "コストは{0}増加します";

        public override string Hud_Purchase_MaxCapacity => "最大容量に達しました";

        public override string Hud_CompareMilitaryStrength_YourToOther => "戦力: あなた {0} - 彼ら {1}";

        /// <summary>
        /// 日付を「年、月、日」として短い文字列で表示する
        /// </summary>
        public override string Hud_Date => "Y{0} M{1} D{2}";

        /// <summary>
        /// 時間を「時、分、秒」として短い文字列で表示する
        /// </summary>
        public override string Hud_TimeSpan => "H{0} M{1} S{2}";

        /// <summary>
        /// 二つの軍隊、または軍隊と都市の間の戦闘
        /// </summary>
        public override string Hud_Battle => "戦闘";



        /// <summary>
        /// ボタン入力の説明。一時停止。
        /// </summary>
        public override string Input_Pause => "一時停止";

        /// <summary>
        /// ボタン入力の説明。一時停止から再開。
        /// </summary>
        public override string Input_ResumePaused => "再開";

        /// <summary>
        /// 一般的なお金の資源
        /// </summary>
        public override string ResourceType_Gold => "ゴールド";

        /// <summary>
        /// 労働者の資源
        /// </summary>
        public override string ResourceType_Workers => "労働者";

        public override string ResourceType_Workers_Description => "労働者は収入を提供します。また、軍隊の兵士として徴用されます";

        /// <summary>
        /// 外交に使用される資源
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "外交ポイント";

        /// <summary>
        /// 0: 獲得したポイント数, 1: ソフト上限値（この後は増加が遅くなる）, 2: ハードリミット
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "外交ポイント: {0} / {1} ({2})";

        /// <summary>
        /// 都市の建物の種類。騎士と外交官のための建物。
        /// </summary>
        public override string Building_NobleHouse => "貴族の館";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "{0}秒ごとに1外交ポイント";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "外交ポイントの最大限度に+{0}";
        public override string Building_NobleHouse_UnlocksKnight => "騎士ユニットを解放";

        public override string Building_BuildAction => "建築";
        public override string Building_IsBuilt => "建築済み";

        /// <summary>
        /// 都市の建物の種類。邪悪な大量生産。
        /// </summary>
        public override string Building_DarkFactory => "ダークファクトリー";

        /// <summary>
        /// ゲーム設定メニューにて。すべての難易度オプションをパーセンテージで合計します。
        /// </summary>
        public override string Settings_TotalDifficulty => "総難易度 {0}%";

        /// <summary>
        /// ゲーム設定メニューにて。基本難易度オプション。
        /// </summary>
        public override string Settings_DifficultyLevel => "難易度レベル {0}%";

        /// <summary>
        /// ゲーム設定メニューにて。マップを読み込む代わりに新しいマップを作成するオプション
        /// </summary>
        public override string Settings_GenerateMaps => "新しいマップを生成";

        /// <summary>
        /// ゲーム設定メニューにて。新しいマップを作成するとロード時間が長くなります
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "生成は事前に作成されたマップを読み込むよりも遅いです";

        /// <summary>
        /// ゲーム設定メニューにて。難易度オプション。ポーズ中にゲームをプレイする機能をブロックします。
        /// </summary>
        public override string Settings_AllowPause => "ポーズとコマンドを許可";

        /// <summary>
        /// ゲーム設定メニューにて。難易度オプション。ゲームにボスが登場します。
        /// </summary>
        public override string Settings_BossEvents => "ボスイベント";

        /// <summary>
        /// ゲーム設定メニューにて。難易度オプション。ボスの説明なし。
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "ボスイベントを無効にすると、エンディングのないサンドボックスモードになります。";

        /// <summary>
        /// ゲームメカニズムを自動化するオプション。メニュータイトル。
        /// </summary>
        public override string Automation_Title => "自動化";

        /// <summary>
        /// ゲームメカニズムを自動化するオプション。自動化の仕組みについての情報。
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "労働力が最大になるまで待機します";

        /// <summary>
        /// ゲームメカニズムを自動化するオプション。自動化の仕組みについての情報。
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "収入がマイナスの場合、一時停止します";

        /// <summary>
        /// ゲームメカニズムを自動化するオプション。自動化の仕組みについての情報。
        /// </summary>
        public override string Automation_InfoLine_Priority => "大都市が優先されます";

        /// <summary>
        /// ゲームメカニズムを自動化するオプション。自動化の仕組みについての情報。
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "1秒あたり最大1回の購入を行います";

        /// <summary>
        /// 行動のためのボタンキャプション。騎士と外交官のための専門的な建物。
        /// </summary>
        public override string HudAction_BuyItem => "{0}を購入";

        /// <summary>
        /// 二国間の平和または戦争の状態
        /// </summary>
        public override string Diplomacy_RelationType => "関係";

        /// <summary>
        /// 他の派閥同士の関係のリストのタイトル
        /// </summary>
        public override string Diplomacy_RelationToOthers => "他との関係";

        /// <summary>
        /// 外交関係。あなたが国の資源を直接管理しています。
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "従者";

        /// <summary>
        /// 外交関係。完全な協力。
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "同盟";

        /// <summary>
        /// 外交関係。戦争の可能性が減少します。
        /// </summary>
        public override string Diplomacy_RelationType_Good => "良好";

        /// <summary>
        /// 外交関係。平和協定。
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "平和";

        /// <summary>
        /// 外交関係。まだ接触していません。
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "中立";

        /// <summary>
        /// 外交関係。一時的な平和協定。
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "休戦";

        /// <summary>
        /// 外交関係。戦争。
        /// </summary>
        public override string Diplomacy_RelationType_War => "戦争";

        /// <summary>
        /// 外交関係。平和の可能性がない戦争。
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "全面戦争";

        /// <summary>
        /// 外交的コミュニケーション。交渉の状況。0: 用語
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "交渉の状況: {0}";

        /// <summary>
        /// 外交的コミュニケーション。通常よりも良好。
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "良好";

        /// <summary>
        /// 外交的コミュニケーション。通常。
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "普通";

        /// <summary>
        /// 外交的コミュニケーション。通常よりも悪い。
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "悪い";

        /// <summary>
        /// 外交的コミュニケーション。コミュニケーションしません。
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "なし";

        /// <summary>
        /// 外交アクション。新しい外交関係を築く。
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "新しい関係を築く: {0}";

        /// <summary>
        /// 外交アクション。新しい外交関係を提案する。
        /// </summary>
        public override string Diplomacy_OfferPeace => "平和を提案";

        /// <summary>
        /// 外交アクション。新しい外交関係を提案する。
        /// </summary>
        public override string Diplomacy_OfferAlliance => "同盟を提案";

        /// <summary>
        /// 外交タイトル。他のプレイヤーが新しい外交関係を提案しました。0: プレイヤー名
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0}が新しい関係を提案";

        /// <summary>
        /// 外交アクション。新しい外交関係を受け入れる。
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "新しい関係を受け入れる";

        /// <summary>
        /// 外交説明。他のプレイヤーが新しい外交関係を提案しました。0: 関係タイプ
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "新しい関係が提案されました: {0}";

        /// <summary>
        /// 外交アクション。他国を従者にする。
        /// </summary>
        public override string Diplomacy_AbsorbServant => "従者として吸収";

        /// <summary>
        /// 外交説明。悪に対抗します。
        /// </summary>
        public override string Diplomacy_LightSide => "光の側の同盟";

        /// <summary>
        /// 外交説明。休戦がどれだけ続くか。
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "{0}秒後に終了";

        /// <summary>
        /// 外交アクション。休戦を延長する。
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "休戦を延長";

        /// <summary>
        /// 外交説明。休戦がどれだけ延長されるか。
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "休戦を{0}秒延長";

        /// <summary>
        /// 外交説明。合意された関係に反することは外交ポイントがかかります。
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "関係を破るには{0}外交ポイントが必要";

        /// <summary>
        /// 同盟のための外交説明。
        /// </summary>
        public override string Diplomacy_AllyDescription => "同盟は戦争宣言を共有します。";

        /// <summary>
        /// 良好な関係のための外交説明。
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "戦争宣言の可能性を制限します。";

        /// <summary>
        /// 従者に対する外交説明。従者（他国を支配する）よりも大きな軍事力を持っていなければなりません。
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "{0}倍の軍事力が必要";

        /// <summary>
        /// 従者に対する外交説明。従者は絶望的な戦争に巻き込まれていなければなりません（他国を支配する）。
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "従者はより強い敵との戦争に巻き込まれている必要があります";

        /// <summary>
        /// 従者に対する外交説明。従者は多くの都市を所有できません（他国を支配する）。
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "従者は最大{0}都市を持つことができます";

        /// <summary>
        /// 従者に対する外交説明。外交ポイントのコストが増加します（他国を支配する）。
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "従者ごとにコストが増加します";

        /// <summary>
        /// 従者関係の結果、他国の平和的な吸収。
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "他の派閥を吸収";

        /// <summary>
        /// 戦争宣言を受け取ったときのメッセージ
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "宣戦布告！";

        /// <summary>
        /// 休戦タイマーが切れて、再び戦争に戻ります。
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "休戦が終了しました";

        /// <summary>
        /// ゲーム終了画面に表示される統計。表示タイトル。
        /// </summary>
        public override string EndGameStatistics_Title => "統計";

        /// <summary>
        /// ゲーム終了画面に表示される統計。経過した総ゲーム内時間。
        /// </summary>
        public override string EndGameStatistics_Time => "ゲーム内時間: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。購入した兵士の数。
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "募集された兵士: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。戦闘で死亡した兵士の数。
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "戦闘で失った兵士: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。戦闘で殺した敵兵の数。
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "戦闘で殺した敵兵: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。離反した兵士の数。
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "離反した兵士: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。戦闘で勝ち取った都市の数。
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "占領した都市: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。戦闘で失った都市の数。
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "失った都市: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。勝利した戦闘の数。
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "勝利した戦闘: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。敗北した戦闘の数。
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "敗北した戦闘: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。外交。あなたが宣戦布告した数。
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "宣戦布告した数: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。外交。あなたに対して宣戦布告された数。
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "受け取った宣戦布告: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。外交によって結ばれた同盟。
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "外交同盟: {0}";

        /// <summary>
        /// ゲーム終了画面に表示される統計。外交によって結ばれた従者。従者の都市と軍隊はあなたのものになります。
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "外交従者: {0}";

        /// <summary>
        /// マップ上の集合ユニットタイプ。兵士の軍隊。
        /// </summary>
        public override string UnitType_Army => "軍隊";

        /// <summary>
        /// マップ上の集合ユニットタイプ。兵士のグループ。
        /// </summary>
        public override string UnitType_SoldierGroup => "グループ";

        /// <summary>
        /// マップ上の集合ユニットタイプ。村や都市の一般名称。
        /// </summary>
        public override string UnitType_City => "都市";

        /// <summary>
        /// 軍隊のグループ選択
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "軍隊グループ、数: {0}";


        /// <summary>
        /// 特殊な兵士のタイプの名前。標準的な前線兵士。
        /// </summary>
        public override string UnitType_Soldier => "兵士";

        /// <summary>
        /// 特殊な兵士のタイプの名前。海戦の兵士。
        /// </summary>
        public override string UnitType_Sailor => "船員";

        /// <summary>
        /// 特殊な兵士のタイプの名前。徴兵された農民。
        /// </summary>
        public override string UnitType_Folkman => "農民兵";

        /// <summary>
        /// 特殊な兵士のタイプの名前。盾と槍のユニット。
        /// </summary>
        public override string UnitType_Spearman => "槍兵";

        /// <summary>
        /// 特殊な兵士のタイプの名前。王の護衛部隊の一部であるエリート部隊。
        /// </summary>
        public override string UnitType_HonorGuard => "名誉護衛";

        /// <summary>
        /// 特殊な兵士のタイプの名前。対騎兵、長い両手槍を装備。
        /// </summary>
        public override string UnitType_Pikeman => "槍兵";

        /// <summary>
        /// 特殊な兵士のタイプの名前。装甲騎兵ユニット。
        /// </summary>
        public override string UnitType_Knight => "騎士";

        /// <summary>
        /// 特殊な兵士のタイプの名前。弓と矢。
        /// </summary>
        public override string UnitType_Archer => "弓兵";

        /// <summary>
        /// 特殊な兵士のタイプの名前。
        /// </summary>
        public override string UnitType_Crossbow => "クロスボウ兵";

        /// <summary>
        /// 特殊な兵士のタイプの名前。大型の槍を投げる戦闘機。
        /// </summary>
        public override string UnitType_Ballista => "バリスタ";

        /// <summary>
        /// 特殊な兵士のタイプの名前。大砲を持ったファンタジートロール。
        /// </summary>
        public override string UnitType_Trollcannon => "トロールキャノン";

        /// <summary>
        /// 特殊な兵士のタイプの名前。森の兵士。
        /// </summary>
        public override string UnitType_GreenSoldier => "グリーンソルジャー";

        /// <summary>
        /// 特殊な兵士のタイプの名前。北からの海軍ユニット。
        /// </summary>
        public override string UnitType_Viking => "ヴァイキング";

        /// <summary>
        /// 特殊な兵士のタイプの名前。邪悪なマスターボス。
        /// </summary>
        public override string UnitType_DarkLord => "ダークロード";

        /// <summary>
        /// 特殊な兵士のタイプの名前。大きな旗を持つ兵士。
        /// </summary>
        public override string UnitType_Bannerman => "旗持ち兵";

        /// <summary>
        /// 軍事ユニットの名前。兵士を運ぶ船。0: 運ぶユニットタイプ
        /// </summary>
        public override string UnitType_WarshipWithUnit => "{0}の戦艦";

        public override string UnitType_Description_Soldier => "汎用ユニット。";
        public override string UnitType_Description_Sailor => "海戦に強い";
        public override string UnitType_Description_Folkman => "安価な訓練されていない兵士";
        public override string UnitType_Description_HonorGuard => "維持費のかからないエリート兵士";
        public override string UnitType_Description_Knight => "野戦で強い";
        public override string UnitType_Description_Archer => "保護されている時のみ強い";
        public override string UnitType_Description_Crossbow => "強力な遠隔兵士";
        public override string UnitType_Description_Ballista => "都市に対して強い";
        public override string UnitType_Description_GreenSoldier => "恐れられるエルフの戦士";
        public override string UnitType_Description_DarkLord => "最終ボス";
        /// <summary>
        /// 兵士タイプの情報
        /// </summary>
        public override string SoldierStats_Title => "ユニットごとの統計";

        /// <summary>
        /// 兵士グループの数
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} グループ、合計 {1} ユニット";

        /// <summary>
        /// 兵士は平地での攻撃、船からの攻撃、または居留地への攻撃に応じて異なる強さを持ちます
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "攻撃力: 陸地 {0} | 海 {1} | 都市 {2}";

        /// <summary>
        /// 兵士が耐えられる傷の数
        /// </summary>
        public override string SoldierStats_Health => "健康: {0}";

        /// <summary>
        /// 一部の兵士は軍隊の移動速度を上げます
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "陸上での軍隊速度ボーナス: {0}";

        /// <summary>
        /// 一部の兵士は船の移動速度を上げます
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "海上での軍隊速度ボーナス: {0}";

        /// <summary>
        /// 購入された兵士は新兵として開始し、数分後に訓練を完了します。
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "訓練時間: {0} 分。新兵が都市に隣接している場合、訓練時間は2倍速くなります。";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。移動を停止させる。
        /// </summary>
        public override string ArmyOption_Halt => "停止";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を除去する。
        /// </summary>
        public override string ArmyOption_Disband => "ユニットを解散";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を軍隊間で送るオプション。
        /// </summary>
        public override string ArmyOption_Divide => "軍隊を分割";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を除去する。
        /// </summary>
        public override string ArmyOption_RemoveX => "{0} を除去";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を除去する。
        /// </summary>
        public override string ArmyOption_DisbandAll => "すべてを解散";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。0: 数, 1: ユニットタイプ
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} グループ: {0}";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を軍隊間で送るオプション。
        /// </summary>
        public override string ArmyOption_SendToX => "{0} にユニットを送る";

        public override string ArmyOption_MergeAllArmies => "すべての軍隊を統合";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を軍隊間で送るオプション。
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "新しい軍隊にユニットを分割";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を軍隊間で送るオプション。
        /// </summary>
        public override string ArmyOption_SendX => "{0} を送る";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を軍隊間で送るオプション。
        /// </summary>
        public override string ArmyOption_SendAll => "すべて送る";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を軍隊間で送るオプション。
        /// </summary>
        public override string ArmyOption_DivideHalf => "軍隊を半分に分割";

        /// <summary>
        /// 軍隊を制御するためのメニューオプション。兵士を軍隊間で送るオプション。
        /// </summary>
        public override string ArmyOption_MergeArmies => "軍隊を統合";

        /// <summary>
        /// 兵士を購入します。
        /// </summary>
        public override string UnitType_Recruit => "徴兵";

        /// <summary>
        /// 特定のタイプの兵士を購入します。0: タイプ
        /// </summary>
        public override string CityOption_RecruitType => "{0}を徴兵";

        /// <summary>
        /// 雇われた兵士の数
        /// </summary>
        public override string CityOption_XMercenaries => "傭兵: {0}";

        /// <summary>
        /// 現在市場で雇用可能な傭兵の数を示します
        /// </summary>
        public override string Hud_MercenaryMarket => "市場で雇用可能な傭兵";

        /// <summary>
        /// 一定数の傭兵を購入します
        /// </summary>
        public override string CityOption_BuyXMercenaries => "{0}人の傭兵を輸入";

        public override string CityOption_Mercenaries_Description => "兵士は労働力の代わりに傭兵から徴用されます";

        /// <summary>
        /// 行動のためのボタンキャプション。より多くの労働者のための住居を作成します。
        /// </summary>
        public override string CityOption_ExpandWorkForce => "労働力を拡大";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "最大労働力 +{0}";
        public override string CityOption_ExpandGuardSize => "守衛を拡大";

        public override string CityOption_Damages => "損害: {0}";
        public override string CityOption_Repair => "損害を修理";
        public override string CityOption_RepairGain => "{0}の損害を修理";

        public override string CityOption_Repair_Description => "損害は収容できる労働者の数を減らします。";

        public override string CityOption_BurnItDown => "焼き払う";
        public override string CityOption_BurnItDown_Description => "労働力を除去し、最大の損害を与えます";

        /// <summary>
        /// メインボス。額に輝く金属の石が刺さっていることから名付けられました。
        /// </summary>
        public override string FactionName_DarkLord => "破滅の目";

        /// <summary>
        /// オークにインスパイアされた派閥。ダークロードのために働きます。
        /// </summary>
        public override string FactionName_DarkFollower => "恐怖の従者";

        /// <summary>
        /// 最大の派閥、古くからあるが腐敗した王国。
        /// </summary>
        public override string FactionName_UnitedKingdom => "統一王国";

        /// <summary>
        /// エルフにインスパイアされた派閥。森と調和して暮らしています。
        /// </summary>
        public override string FactionName_Greenwood => "グリーンウッド";

        /// <summary>
        /// 東のアジア風派閥
        /// </summary>
        public override string FactionName_EasternEmpire => "東帝国";

        /// <summary>
        /// 北のヴァイキング風の王国。最大のもの。
        /// </summary>
        public override string FactionName_NordicRealm => "ノルディックレルム";

        /// <summary>
        /// 北のヴァイキング風の王国。熊の爪のシンボルを使用します。
        /// </summary>
        public override string FactionName_BearClaw => "ベアクロー";

        /// <summary>
        /// 北のヴァイキング風の王国。鶏のシンボルを使用します。
        /// </summary>
        public override string FactionName_NordicSpur => "ノルディックスパー";

        /// <summary>
        /// 北のヴァイキング風の王国。黒いカラスのシンボルを使用します。
        /// </summary>
        public override string FactionName_IceRaven => "アイスレイヴン";

        /// <summary>
        /// 強力なバリスタでドラゴンを倒すことで有名な派閥。
        /// </summary>
        public override string FactionName_Dragonslayer => "ドラゴンスレイヤー";

        /// <summary>
        /// 南からの傭兵部隊。アラビア風。
        /// </summary>
        public override string FactionName_SouthHara => "サウスハラ";

        /// <summary>
        /// 中立のCPU制御の国の名前
        /// </summary>
        public override string FactionName_GenericAi => "AI {0}";

        /// <summary>
        /// プレイヤーとその番号の表示名
        /// </summary>
        public override string FactionName_Player => "プレイヤー {0}";

        /// <summary>
        /// 南から船でミニボスが近づいてくるときのメッセージ。
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "敵が接近中!";
        public override string EventMessage_HaraMercenaryText => "南でハラ傭兵が発見されました";

        /// <summary>
        /// メインボスが出現する最初の警告。
        /// </summary>
        public override string EventMessage_ProphesyTitle => "暗い予言";
        public override string EventMessage_ProphesyText => "破滅の目がまもなく現れ、あなたの敵が彼に加わるでしょう!";

        /// <summary>
        /// メインボスが出現する二度目の警告。
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "暗黒の時代";
        public override string EventMessage_FinalBossEnterText => "破滅の目がマップに登場しました!";

        /// <summary>
        /// メインボスが戦場に現れるときのメッセージ。
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "絶望的な攻撃";
        public override string EventMessage_FinalBattleText => "ダークロードが戦場に加わりました。今が彼を倒すチャンスです!";

        /// <summary>
        /// 兵士の維持費を払えないときに軍隊を離れるメッセージ
        /// </summary>
        public override string EventMessage_DesertersTitle => "脱走者！";
        public override string EventMessage_DesertersText_Money => "未払いの兵士たちがあなたの軍隊から脱走しています";

        public override string DifficultyDescription_AiAggression => "AIの攻撃性: {0}。";
        public override string DifficultyDescription_BossSize => "ボスのサイズ: {0}。";
        public override string DifficultyDescription_BossEnterTime => "ボス登場時間: {0}。";
        public override string DifficultyDescription_AiEconomy => "AIの経済: {0}%。";
        public override string DifficultyDescription_AiDelay => "AIの遅延: {0}。";
        public override string DifficultyDescription_DiplomacyDifficulty => "外交の難易度: {0}。";
        public override string DifficultyDescription_MercenaryCost => "傭兵のコスト: {0}。";
        public override string DifficultyDescription_HonorGuards => "名誉護衛: {0}。";

        /// <summary>
        /// ゲームが成功で終了しました。
        /// </summary>
        public override string EndScreen_VictoryTitle => "勝利!";

        /// <summary>
        /// ゲーム内のリーダーキャラクターからの名言
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
{
    "平和の時代には、私たちは死者を悼む。",
    "すべての勝利には犠牲の影が伴う。",
    "勇敢な魂で点在する、この旅の軌跡を忘れないでください。",
    "勝利の光で心が軽くなり、犠牲者の重みで心が重い。"
};

        public override string EndScreen_DominationVictoryQuote => "私は神々に世界を支配するために選ばれた！";

        /// <summary>
        /// ゲームが失敗で終了しました。
        /// </summary>
        public override string EndScreen_FailTitle => "失敗!";

        /// <summary>
        /// ゲーム内のリーダーキャラクターからの名言
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
{
    "行進と心配の夜で体が引き裂かれ、終わりを歓迎します。",
    "敗北は我々の土地を暗くするかもしれませんが、それは我々の決意の光を消すことはできません。",
    "私たちの心の炎を消し、その灰から、私たちの子供たちが新しい夜明けを築くでしょう。",
    "私たちの物語が明日の勝利を呼び起こす火種となりますように。"
};

        /// <summary>
        /// ゲーム終了時の小さなカットシーン
        /// </summary>
        public override string EndScreen_WatchEpilogue => "エピローグを見る";

        /// <summary>
        /// カットシーンのタイトル
        /// </summary>
        public override string EndScreen_Epilogue_Title => "エピローグ";

        /// <summary>
        /// カットシーンの導入
        /// </summary>
        public override string EndScreen_Epilogue_Text => "160年前";

        /// <summary>
        /// ゲームの物語を短い詩で紹介するプロローグ
        /// </summary>
        public override string GameMenu_WatchPrologue => "プロローグを見る";

        public override string Prologue_Title => "プロローグ";

        /// <summary>
        /// 詩は3行でなければならず、4行目はボスの名前の翻訳から取られて提示されます
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
{
    "夜には夢があなたを悩ませ、",
    "暗い未来の予言",
    "彼の到来に備えなさい、",
};

        /// <summary>
        /// ゲームを一時停止すると表示されるメニュー
        /// </summary>
        public override string GameMenu_Title => "ゲームメニュー";

        /// <summary>
        /// エンド画面後にゲームを続行
        /// </summary>
        public override string GameMenu_ContinueGame => "続行";

        /// <summary>
        /// ゲームを続行
        /// </summary>
        public override string GameMenu_Resume => "再開";

        /// <summary>
        /// ゲームロビーに戻る
        /// </summary>
        public override string GameMenu_ExitGame => "ゲームを終了";

        public override string Hud_Save => "保存";
        public override string GameMenu_SaveStateWarnings => "警告！ゲームが更新されるとセーブファイルは失われます。";
        public override string GameMenu_LoadState => "読み込み";
        public override string GameMenu_ContinueFromSave => "セーブから続行";

        public override string GameMenu_AutoSave => "自動保存";

        public override string GameMenu_Load_PlayerCountError => "セーブファイルに一致するプレイヤー数を設定する必要があります: {0}";

        public override string Progressbar_MapLoadingState => "マップ読み込み: {0}";

        public override string Progressbar_ProgressComplete => "完了";

        /// <summary>
        /// 0: 進行状況のパーセンテージ、1: 失敗回数
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "生成中: {0}%。 (失敗 {1})";

        /// <summary>
        /// 0: 現在の部分、1: 部分の数
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "部分 {0}/{1}";

        /// <summary>
        /// 0: パーセンテージまたは完了
        /// </summary>
        public override string Progressbar_SaveProgress => "保存中: {0}";

        /// <summary>
        /// 0: パーセンテージまたは完了
        /// </summary>
        public override string Progressbar_LoadProgress => "読み込み中: {0}";

        /// <summary>
        /// プレイヤーの入力を待っています
        /// </summary>
        public override string Progressbar_PressAnyKey => "続行するには任意のキーを押してください";

        /// <summary>
        /// 兵士を購入して移動させるチュートリアル。チュートリアルが完了するまで高度な操作はロックされています。
        /// </summary>
        public override string Tutorial_MenuOption => "チュートリアルを実行";
        public override string Tutorial_MissionsTitle => "チュートリアルミッション";
        public override string Tutorial_Mission_BuySoldier => "都市を選択して兵士を徴兵";
        public override string Tutorial_Mission_MoveArmy => "軍隊を選択して移動";

        public override string Tutorial_CompleteTitle => "チュートリアル完了！";
        public override string Tutorial_CompleteMessage => "フルズームと高度なゲームオプションがアンロックされました。";

        /// <summary>
        /// ボタン入力を表示
        /// </summary>
        public override string Tutorial_SelectInput => "選択";
        public override string Tutorial_MoveInput => "移動コマンド";

        /// <summary>
        /// 対戦。戦闘に入る二つの軍隊を説明するテキスト
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "宣戦布告";

        public override string ArmyOption_Attack => "攻撃";

        /// <summary>
        /// ゲーム設定メニュー。押したときのキーとボタンの動作を変更します
        /// </summary>
        public override string Settings_ButtonMapping => "ボタンマッピング";

        /// <summary>
        /// ボタン入力を説明します。HUDの情報量を拡大または縮小します
        /// </summary>


        /// <summary>
        /// 入力タイプ、標準のPC入力
        /// </summary>
        public override string Input_Source_Keyboard => "キーボードとマウス";

        /// <summary>
        /// 入力タイプ、Xboxのような携帯型コントローラー
        /// </summary>
        public override string Input_Source_Controller => "コントローラー";


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */

        public override string CityMenu_SalePricesTitle => "販売価格";
        public override string Blueprint_Title => "設計図";
        public override string Resource_Tab_Overview => "概要";
        public override string Resource_Tab_Stockpile => "備蓄";

        public override string Resource => "資源";
        public override string Resource_StockPile_Info => "資源の備蓄量を設定し、次にどの資源に取り掛かるかを労働者に指示します。";
        public override string Resource_TypeName_Water => "水";
        public override string Resource_TypeName_Wood => "木材";
        public override string Resource_TypeName_Fuel => "燃料";
        public override string Resource_TypeName_Stone => "石材";
        public override string Resource_TypeName_RawFood => "生の食料";
        public override string Resource_TypeName_Food => "食料";
        public override string Resource_TypeName_Beer => "ビール";
        public override string Resource_TypeName_Wheat => "小麦";
        public override string Resource_TypeName_Linen => "リネン";
        //public override string Resource_TypeName_SkinAndLinen => "皮とリネン";
        public override string Resource_TypeName_IronOre => "鉄鉱石";
        public override string Resource_TypeName_GoldOre => "金鉱石";
        public override string Resource_TypeName_Iron => "鉄";

        public override string Resource_TypeName_SharpStick => "尖った棒";
        public override string Resource_TypeName_Sword => "剣";
        public override string Resource_TypeName_KnightsLance => "騎士の槍";
        public override string Resource_TypeName_TwoHandSword => "両手剣";
        public override string Resource_TypeName_Bow => "弓";

        public override string Resource_TypeName_LightArmor => "軽装甲";
        public override string Resource_TypeName_MediumArmor => "中装甲";
        public override string Resource_TypeName_HeavyArmor => "重装甲";

        public override string ResourceType_Children => "子供";

        public override string BuildingType_DefaultName => "建物";
        public override string BuildingType_WorkerHut => "労働者の小屋";
        public override string BuildingType_Tavern => "居酒屋";
        public override string BuildingType_Brewery => "醸造所";
        public override string BuildingType_Postal => "郵便サービス";
        public override string BuildingType_Recruitment => "募集センター";
        public override string BuildingType_Barracks => "兵舎";
        public override string BuildingType_PigPen => "豚小屋";
        public override string BuildingType_HenPen => "鶏小屋";
        public override string BuildingType_WorkBench => "作業台";
        public override string BuildingType_Carpenter => "大工";
        public override string BuildingType_CoalPit => "石炭採掘場";
        public override string DecorType_Statue => "彫像";
        public override string DecorType_Pavement => "舗装";
        public override string BuildingType_Smith => "鍛冶屋";
        public override string BuildingType_Cook => "料理人";
        public override string BuildingType_Storage => "倉庫";

        public override string BuildingType_ResourceFarm => "{0}農場";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "労働者の上限を{0}増やします";
        public override string BuildingType_Tavern_Description => "労働者はここで食事ができます";
        public override string BuildingType_Tavern_Brewery => "ビールの生産";
        public override string BuildingType_Postal_Description => "他の都市に資源を送ります";
        public override string BuildingType_Recruitment_Description => "他の都市に兵士を送ります";
        public override string BuildingType_Barracks_Description => "兵士の募集に必要な人員と装備を使用します";
        public override string BuildingType_PigPen_Description => "豚を飼育し、食料と皮を提供します";
        public override string BuildingType_HenPen_Description => "鶏と卵を生産し、食料を提供します";
        public override string BuildingType_Decor_Description => "装飾";
        public override string BuildingType_Farm_Description => "資源を育てます";

        public override string BuildingType_Cook_Description => "食料を加工する場所";
        public override string BuildingType_Bench_Description => "アイテムを作成する場所";

        public override string BuildingType_Smith_Description => "金属加工所";
        public override string BuildingType_Carpenter_Description => "木工所";

        public override string BuildingType_Nobelhouse_Description => "騎士と外交官の家";
        public override string BuildingType_CoalPit_Description => "効率的な燃料生産";
        public override string BuildingType_Storage_Description => "資源の集積所";

        public override string MenuTab_Info => "情報";
        public override string MenuTab_Work => "仕事";
        public override string MenuTab_Recruit => "徴兵";
        public override string MenuTab_Resources => "資源";
        public override string MenuTab_Trade => "貿易";
        public override string MenuTab_Build => "建設";
        public override string MenuTab_Economy => "経済";
        public override string MenuTab_Delivery => "配送";

        public override string MenuTab_Build_Description => "都市に建物を配置します";
        public override string MenuTab_BlackMarket_Description => "都市に建物を配置します";
        public override string MenuTab_Resources_Description => "都市に建物を配置します";
        public override string MenuTab_Work_Description => "都市に建物を配置します";
        public override string MenuTab_Automation_Description => "都市に建物を配置します";

        public override string BuildHud_OutsideCity => "都市の外の領域です";
        public override string BuildHud_OutsideFaction => "領土の外です！";

        public override string BuildHud_OccupiedTile => "占有されたタイル";

        public override string Build_PlaceBuilding => "建設";
        public override string Build_DestroyBuilding => "破壊";
        public override string Build_ClearTerrain => "地形を整地";

        public override string Build_ClearOrders => "建設命令をクリア";
        public override string Build_Order => "建設命令";
        public override string Build_OrderQue => "建設命令キュー: {0}";
        public override string Build_AutoPlace => "自動配置";

        public override string Work_OrderPrioTitle => "作業優先度";
        public override string Work_OrderPrioDescription => "優先度は1（低）から{0}（高）までです";

        public override string Work_OrderPrio_No => "優先度なし。作業されません。";
        public override string Work_OrderPrio_Min => "最小優先度。";
        public override string Work_OrderPrio_Max => "最大優先度。";

        public override string Work_Move => "アイテムを移動";

        public override string Work_GatherXResource => "{0}を集める";
        public override string Work_CraftX => "{0}を作成";
        public override string Work_Farming => "農業";
        public override string Work_Mining => "採掘";
        public override string Work_Trading => "取引";

        public override string Work_AutoBuild => "自動建設と拡張";

        public override string WorkerHud_WorkType => "作業状況: {0}";
        public override string WorkerHud_Carry => "持ち運び: {0} {1}";
        public override string WorkerHud_Energy => "エネルギー: {0}";
        public override string WorkerStatus_Exit => "労働力を離れる";
        public override string WorkerStatus_Eat => "食べる";
        public override string WorkerStatus_Till => "耕す";
        public override string WorkerStatus_Plant => "植える";
        public override string WorkerStatus_Gather => "集める";
        public override string WorkerStatus_PickUpResource => "資源を拾う";
        public override string WorkerStatus_DropOff => "納品";
        public override string WorkerStatus_BuildX => "{0}を建設";
        public override string WorkerStatus_TrossReturnToArmy => "軍隊に戻る";

        public override string Hud_ToggleFollowFaction => "派閥設定を追従する";
        public override string Hud_FollowFaction_Yes => "派閥のグローバル設定に従っています";
        public override string Hud_FollowFaction_No => "ローカル設定を使用しています（グローバル値は{0}です）";

        public override string Hud_Idle => "待機中";
        public override string Hud_NoLimit => "制限なし";

        public override string Hud_None => "なし";
        public override string Hud_ProductionQueue => "生産キュー";

        public override string Hud_EmptyList => "- 空のリスト -";

        public override string Hud_RequirementOr => "- または -";

        public override string Hud_BlackMarket => "ブラックマーケット";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "都市を選択";
        public override string Conscription_Title => "徴兵";
        public override string Conscript_WeaponTitle => "武器";
        public override string Conscript_ArmorTitle => "鎧";
        public override string Conscript_TrainingTitle => "訓練";

        public override string Conscript_SpecializationTitle => "専門化";
        public override string Conscript_SpecializationDescription => "特定の分野で攻撃力が{0}増加し、他のすべての分野が減少します";
        public override string Conscript_SelectBuilding => "兵舎を選択";

        public override string Conscript_WeaponDamage => "武器のダメージ: {0}";
        public override string Conscript_ArmorHealth => "鎧の耐久力: {0}";
        public override string Conscript_TrainingSpeed => "攻撃速度: {0}";
        public override string Conscript_TrainingTime => "訓練時間: {0}";

        public override string Conscript_Training_Minimal => "最低限";
        public override string Conscript_Training_Basic => "基本";
        public override string Conscript_Training_Skillful => "熟練";
        public override string Conscript_Training_Professional => "専門的";

        public override string Conscript_Specialization_Field => "平原";
        public override string Conscript_Specialization_Sea => "海";
        public override string Conscript_Specialization_Siege => "攻城";
        public override string Conscript_Specialization_Traditional => "伝統的";
        public override string Conscript_Specialization_AntiCavalry => "対騎兵";

        public override string Conscription_Status_CollectingEquipment => "装備を集めています: {0}";
        public override string Conscription_Status_CollectingMen => "兵士を集めています: {0}";
        public override string Conscription_Status_Training => "訓練中: {0}";

        public override string ArmyHud_Food_Reserves_X => "食料備蓄: {0}";
        public override string ArmyHud_Food_Upkeep_X => "食料維持: {0}";
        public override string ArmyHud_Food_Costs_X => "食料コスト: {0}";

        public override string Deliver_WillSendXInfo => "{0}ずつ送信されます";
        public override string Delivery_ListTitle => "配送サービスを選択";
        public override string Delivery_DistanceX => "距離: {0}";
        public override string Delivery_DeliveryTimeX => "配送時間: {0}";
        public override string Delivery_SenderMinimumCap => "送信者の最低限の上限";
        public override string Delivery_RecieverMaximumCap => "受信者の最大上限";
        public override string Delivery_ItemsReady => "準備ができているアイテム";
        public override string Delivery_RecieverReady => "受信者が準備完了";
        public override string Hud_ThisCity => "この都市";
        public override string Hud_RecieveingCity => "受信都市";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "毎秒あたりの資源で表示されます。";

        public override string Info_MinuteAverage => "この値は、過去1分の平均値です。";

        public override string Message_OutOfFood_Title => "食料が不足しています";
        public override string Message_CityOutOfFood_Text => "ブラックマーケットから高価な食料が購入されます。資金が尽きると労働者は餓死します。";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "地形の種類";

        public override string Hud_EnergyUpkeepX => "食料エネルギー維持 {0}";

        public override string Hud_EnergyAmount => "{0} エネルギー（作業時間）";

        public override string Hud_CopySetup => "設定をコピー";
        public override string Hud_Paste => "貼り付け";

        public override string Hud_Available => "利用可能";

        public override string WorkForce_ChildBirthRequirements => "出生要件";
        public override string WorkForce_AvailableHomes => "利用可能な住居: {0}";
        public override string WorkForce_Peace => "平和";
        public override string WorkForce_ChildToManTime => "成人年齢: {0} 分";

        public override string Economy_TaxIncome => "税収: {0}";
        public override string Economy_ImportCostsForResource => "{0}の輸入コスト: {1}";
        public override string Economy_BlackMarketCostsForResource => "{0}のブラックマーケットコスト: {1}";
        public override string Economy_GuardUpkeep => "警備の維持: {0}";

        public override string Economy_LocalCityTrade_Export => "都市間貿易の輸出: {0}";
        public override string Economy_LocalCityTrade_Import => "都市間貿易の輸入: {0}";

        public override string Economy_ResourceProduction => "{0}の生産: {1}";
        public override string Economy_ResourceSpending => "{0}の消費: {1}";

        public override string Economy_TaxDescription => "労働者1人あたり{0}ゴールドの税金";

        public override string Economy_SoldResources => "販売された資源（金鉱石）: {0}";

        public override string UnitType_Cities => "都市";
        public override string UnitType_Armies => "軍隊";
        public override string UnitType_Worker => "労働者";

        public override string UnitType_FootKnight => "剣士騎士";
        public override string UnitType_CavalryKnight => "騎馬騎士";

        public override string CityCulture_LargeFamilies => "大家族";
        public override string CityCulture_FertileGround => "肥沃な大地";
        public override string CityCulture_Archers => "熟練した弓兵";
        public override string CityCulture_Warriors => "戦士";
        public override string CityCulture_AnimalBreeder => "動物の飼育者";
        public override string CityCulture_Miners => "鉱夫";
        public override string CityCulture_Woodcutters => "木こり";
        public override string CityCulture_Builders => "建設者";
        public override string CityCulture_CrabMentality => "蟹マインド";
        public override string CityCulture_DeepWell => "深い井戸";
        public override string CityCulture_Networker => "ネットワーカー";
        public override string CityCulture_PitMasters => "燃料生産者";

        public override string CityCulture_CultureIsX => "文化: {0}";
        public override string CityCulture_LargeFamilies_Description => "出生率が上昇します";
        public override string CityCulture_FertileGround_Description => "作物の収穫量が増えます";
        public override string CityCulture_Archers_Description => "熟練した弓兵を生産します";
        public override string CityCulture_Warriors_Description => "熟練した近接戦闘兵を生産します";
        public override string CityCulture_AnimalBreeder_Description => "動物がより多くの資源を提供します";
        public override string CityCulture_Miners_Description => "鉱石の採掘量が増えます";
        public override string CityCulture_Woodcutters_Description => "木材の生産量が増えます";
        public override string CityCulture_Builders_Description => "建設が速くなります";
        public override string CityCulture_CrabMentality_Description => "労働に必要なエネルギーが減少します。熟練した兵士を生産できません。";
        public override string CityCulture_DeepWell_Description => "水の再生速度が速くなります";
        public override string CityCulture_Networker_Description => "効率的な郵便サービス";
        public override string CityCulture_PitMasters_Description => "燃料生産量が増加します";

        public override string CityOption_AutoBuild_Work => "労働力の自動拡大";
        public override string CityOption_AutoBuild_Farm => "農場の自動拡大";

        public override string Hud_PurchaseTitle_Resources => "資源を購入";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "現在所有";

        public override string Tutorial_EndTutorial => "チュートリアルを終了";
        public override string Tutorial_MissionX => "ミッション{0}";
        public override string Tutorial_CollectXAmountOfY => "{0}を{1}集める";
        public override string Tutorial_SelectTabX => "タブを選択: {0}";
        public override string Tutorial_IncreasePriorityOnX => "{0}の優先度を上げる";
        public override string Tutorial_PlaceBuildOrder => "{0}の建設指示を出す";
        public override string Tutorial_ZoomInput => "ズーム";

        public override string Tutorial_SelectACity => "都市を選択";
        public override string Tutorial_ZoomInWorkers => "労働者にズームイン";
        public override string Tutorial_CreateSoldiers => "次の装備で兵士ユニットを2つ作成: {0}。{1}。";
        public override string Tutorial_ZoomOutOverview => "ズームアウトしてマップの概要を表示";
        public override string Tutorial_ZoomOutDiplomacy => "ズームアウトして外交ビューを表示";
        public override string Tutorial_ImproveRelations => "隣接する派閥との関係を改善";
        public override string Tutorial_MissionComplete_Title => "ミッション完了！";
        public override string Tutorial_MissionComplete_Unlocks => "新しい操作がアンロックされました";

        //patch1
        public override string Resource_ReachedStockpile => "ストックパイル目標に到達";

        public override string BuildingType_ResourceMine => "{0}鉱山";

        public override string Resource_TypeName_BogIron => "湿地鉄";

        public override string Resource_TypeName_Coal => "石炭";

        public override string Language_XUpkeepIsY => "{0}の維持費：{1}";
        public override string Language_XCountIsY => "{0}の数：{1}";

        public override string Message_ArmyOutOfFood_Text => "高価な食料はブラックマーケットから購入されます。お金がなくなると、飢えた兵士は脱走します。";

        public override string Info_ArmyFood => "軍隊は最も近い友好都市から食料を補給します。他の派閥から食料を購入することもできます。敵対地域では、食料をブラックマーケットからしか購入できません。";

        public override string FactionName_Monger => "商人";
        public override string FactionName_Hatu => "ハツ";
        public override string FactionName_Destru => "デストル";

        //patch2
        public override string Tutorial_BuildSomething => "{0} を生産するものを建設する";
        public override string Tutorial_BuildCraft => "{0} のためのクラフトステーションを建設する";
        public override string Tutorial_IncreaseBufferLimit => "{0} のバッファリミットを増加する";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "{0} {1} のストックパイルに達する";
        public override string Tutorial_LookAtFoodBlueprint => "食品の設計図を見る";
        public override string Tutorial_CollectFood_Info1 => "労働者は市役所に食事に行く";
        public override string Tutorial_CollectFood_Info2 => "軍は支援労働者を派遣して食料を集める";
        public override string Tutorial_CollectFood_Info0 => "労働者を完全にコントロールしたいですか？全ての作業優先度をゼロに設定し、一つずつアクティブにする。";

        public override string EndGameStatistics_DecorsBuilt => "建てられた装飾：{0}";
        public override string EndGameStatistics_StatuesBuilt => "建てられた像：{0}";


        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "標準では、労働者は市役所に食事をしに行くか、アイテムを預けに行きます";
        public override string GameMenu_UseSpeedX => "{0} スピードオプション";
        public override string GameMenu_LongerBuildQueue => "建設キューを延長";

        public override string Diplomacy_RelationWithOthers => "他者との関係";
        public override string Automation_queue_description => "キューが空になるまで繰り返し処理を行います";

        public override string BuildingType_Storehouse_Description => "労働者はここにアイテムを置くことができます";

        public override string Resource_TypeName_Longbow => "ロングボウ";
        public override string Resource_TypeName_Rapeseed => "菜種";
        public override string Resource_TypeName_Hemp => "麻";

        public override string Resource_BogIronDescription => "沼鉄を使うよりも鉄の採掘の方が効率的です。";

        public override string Resource_FoodSafeGuard_Description => "セーフガード。食料生産チェーンの優先度を最大化します。値が{0}以下になった場合。";
        public override string Resource_FoodSafeGuard_Active => "セーフガードが有効です。";

        public override string GameMenu_NextSong => "次の曲";

        public override string BuildingType_Bank => "銀行";
        public override string BuildingType_GoldDelivery_Description => "他の都市に金を送る";

        public override string BuildingType_Logistics => "物流";
        public override string BuildingType_Logistics_Description => "建築の注文能力を向上させる";

        public override string BuildingType_Logistics_NationSizeRequirement => "国の総労働力：{0}";
        public override string Requirements_XItemStorageOfY => "{0}市の{1}の保管";

        public override string XP_UnlockBuildQueue => "ビルドキューの解除：{0}";
        public override string XP_UnlockBuilding => "建物の解除：";
        public override string XP_Upgrade => "アップグレード";

        public override string XP_UpgradeBuildingX => "建物のアップグレード：{0}";

        public override string BuildHud_PerCycle => "サイクルごと";
        public override string BuildHud_MayCraft => "作成可能";
        public override string BuildHud_WorkTime => "作業時間：{0}";
        public override string BuildHud_GrowTime => "成長時間：{0}";
        public override string BuildHud_Produce => "生産：";

        public override string BuildHud_Queue => "許可された建設キュー：{0}/{1}";

        public override string LandType_Flatland => "平地";
        public override string LandType_Water => "水域";
        public override string BuildingType_Wall => "壁";
        public override string Delivery_AutoReciever_Description => "資源が最も少ない都市に送信されます";

        public override string Hud_On => "オン";
        public override string Hud_Off => "オフ";

        public override string Hud_Time_Seconds => "{0}秒";
        public override string Hud_Time_Minutes => "{0}分";
        public override string Hud_Undo => "元に戻す";
        public override string Hud_Redo => "やり直し";

        public override string Tag_ViewOnMap => "地図上でタグを表示";

        public override string MenuTab_Tag => "タグ";

        public override string Input_Build => "建設";

        public override string FlagEditor_ClearAll => "すべてクリア";

        public override string CityCulture_Stonemason => "石工";
        public override string CityCulture_Stonemason_Description => "石の収集を改善";

        public override string CityCulture_Brewmaster => "醸造師";
        public override string CityCulture_Brewmaster_Description => "ビールの生産を強化";

        public override string CityCulture_Weavers => "織工";
        public override string CityCulture_Weavers_Description => "軽装甲の生産を強化";

        public override string CityCulture_SiegeEngineer => "攻城技師";
        public override string CityCulture_SiegeEngineer_Description => "より強力な攻城兵器";

        public override string CityCulture_Armorsmith => "鎧職人";
        public override string CityCulture_Armorsmith_Description => "鉄の鎧の生産を改善";

        public override string CityCulture_Noblemen => "貴族";
        public override string CityCulture_Noblemen_Description => "より強力な騎士";

        public override string CityCulture_Seafaring => "海洋技術";
        public override string CityCulture_Seafaring_Description => "海の特化兵士がより強力な船を持つ";

        public override string CityCulture_Backtrader => "裏取引商";
        public override string CityCulture_Backtrader_Description => "ブラックマーケットが安くなる";

        public override string CityCulture_LawAbiding => "法を守る";
        public override string CityCulture_LawAbiding_Description => "税金が増える。ブラックマーケットは利用不可。";



        public override string Hud_Advanced => "詳細設定";
        public override string Hud_Loading => "読み込み中...";

        public override string CityOption_LowerGuardSize => "ガード解除";
        public override string Hud_Purchase_MinCapacity => "最小容量に達しました";
        public override string Settings_ResetToDefault => "デフォルトにリセット";
        public override string Settings_NewGame => "新しいゲーム";

        public override string Settings_AdvancedGameSettings => "高度なゲーム設定";
        public override string Settings_FoodMultiplier => "食料倍率";
        public override string Settings_FoodMultiplier_Description => "満腹時の労働者または兵士の持続時間。高い値はコンピュータのパフォーマンスを低下させます。";

        public override string Settings_GameMode => "ゲームモード";

        public override string Settings_Mode_Story => "フルストーリー";
        public override string Settings_Mode_IncludeBoss => "ボスイベントを含む。";
        public override string Settings_Mode_IncludeAttacks => "ランダムアタックを含む。";
        public override string Settings_Mode_Sandbox => "サンドボックス";
        public override string Settings_Mode_Peaceful => "平和";
        public override string Settings_Mode_Peaceful_Description => "すべての戦争はプレイヤーによって開始されます";

        public override string Lobby_ImportSave => "セーブデータをインポート";

        public override string Lobby_ExportSave => "セーブデータをエクスポート";
        public override string Lobby_ExportSave_Description => "ファイルのコピーを作成し、インポートフォルダーに配置します：{0}";

        public override string Resource_CurrentAmount => "現在の量：{0}";
        public override string Resource_MaxAmount_Soft => "ソフトキャップ（最大限界）：{0}";
        public override string Resource_MaxAmount => "最大限界：{0}";
        public override string Resource_AddPerSec => "増加率：{0}毎秒";

        public override string Resource_WaterAddLimit => "水の増加率は変更できません";

        public override string Tutorial_Select_SubTab => "そしてカテゴリを選択：{0}";



        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */
        public override string Tutorial_OpenGuardSubTab => "兵舎を開いてカテゴリを選択してください: {0}";
        public override string Tutorial_GuardToWall => "衛兵を壁に移動させる";
        public override string Demo_MissionObjective_Title => "ミッション目標";
        public override string Demo_MissionObjective_Description => "南からの攻撃に備えて防衛する";
        public override string Demo_Complete_Title => "デモ完了";
        public override string Demo_TimesUp_Title => "時間切れ！";
        public override string Demo_EndInOneMinuteDescription => "デモは1分後に終了します";

        public override string ArmyOption_NewArmy => "新しい軍隊";
        public override string ProfileEditor_AltMain => "代替メイン";
        public override string Automation_CheckBoxTitle => "自動化";

        public override string ArmyStructure_ColumnWidth => "軍隊の列の幅";
        public override string ArmyStructure_ArmyPlacement => "軍隊内の配置";
        public override string ArmyStructure_Row_Front => "前列";
        public override string ArmyStructure_Row_Body => "中列";
        public override string ArmyStructure_Row_Second => "第2列";
        public override string ArmyStructure_Row_Behind => "後列";

        public override string Diplomacy_RelationType_Enemies => "敵";

        public override string EventMessage_EnemyAlliance_Title => "支配への恐れ";
        public override string EventMessage_EnemyAlliance => "諸国はあなたの勢力拡大を恐れて、あなたに対抗する同盟を結成しました。";

        public override string Settings_CentralGold => "中央ゴールド";
        public override string Settings_CentralGold_Description => "オン：すべてのゴールドは共有プールで即時使用可能になります。オフ：ゴールドは物理的に存在し、輸送が必要です。";

        public override string InputActionName_StopStart => "開始/停止";
        public override string InputActionName_ToggleHudDetail => "HUD詳細の切り替え";
        public override string InputActionName_NextCity => "次の都市";
        public override string InputActionName_NextArmy => "次の軍隊";
        public override string InputActionName_NextBattle => "次の戦闘";
        public override string InputActionName_Build => "建設";
        public override string InputActionName_Copy => "コピー";
        public override string InputActionName_Paste => "貼り付け";
        public override string InputActionName_Menu => "メニュー";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "前の色";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "次の色";
        public override string InputActionName_FlagDesign_PaintBucket => "ペイントバケツ";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "カラーピッカー";
        public override string InputActionName_ControllerFocus => "フォーカス";
        public override string InputActionName_ControllerCancel => "キャンセル";
        public override string InputActionName_ControllerMessageClick => "メッセージクリック";
        public override string InputActionName_ControllerSelect => "選択";
        public override string InputActionName_WASD_UP => "上";
        public override string InputActionName_WASD_DOWN => "下";
        public override string InputActionName_WASD_LEFT => "左";
        public override string InputActionName_WASD_RIGHT => "右";
        public override string InputActionName_CameraTiltLeft => "カメラ左傾き";
        public override string InputActionName_CameraTiltRight => "カメラ右傾き";
        public override string InputActionName_CameraTiltUp => "カメラ上傾き";
        public override string InputActionName_ZoomInKey => "ズームイン";
        public override string InputActionName_ZoomOutKey => "ズームアウト";
        public override string Settings_Title_Monitor => "モニター設定";
        public override string Settings_Title_Graphics => "グラフィック設定";
        public override string Settings_Title_Input => "入力設定";
        public override string Settings_Title_Gameplay => "ゲームプレイ設定";
        public override string Settings_PanOnZoom => "ズーム時にパンする";
        public override string Settings_ScrollSensitivity_Game => "スクロール感度：ゲーム";
        public override string Settings_ScrollSensitivity_Menu => "スクロール感度：メニュー";
        public override string Settings_Blood => "血の表現";

        public override string Settings_MasterVolume => "マスターボリューム";
        public override string Settings_AmbienceVolume => "環境音ボリューム";
        public override string Settings_BattleMelody => "戦闘メロディ";

        public override string Settings_ModelLight => "モデル光の効果";
        public override string Settings_Particles => "パーティクルエフェクト";
        public override string Settings_MapLoadSpeed => "マップ読み込み速度";
        public override string Lobby_Category_Options => "オプション";
        public override string Lobby_Category_Editor => "エディター";
        public override string Lobby_Category_ExtraModes => "追加モード";

        public override string Lobby_Editor_MapEditor => "マップエディター";
        public override string Lobby_Editor_VoxelEditor => "ボクセルエディター";

        public override string Lobby_Mode_BattleLab => "バトルラボ";
        public override string Lobby_Mode_BattleLab_Description => "任意の兵士同士を戦わせる";
        public override string Lobby_Mode_Commander => "コマンダーモード";
        public override string Lobby_Mode_Commander_Description => "小規模な戦術ボードゲーム";
        public override string Lobby_MusicPlayList => "音楽プレイリスト";

        public override string Lobby_GameSetup => "ゲーム設定";
        public override string Lobby_PlayerSetup => "プレイヤー設定";
        public override string LobbyDemoMode_Demo => "デモ";
        public override string Lobby_Tutorial => "チュートリアル";

        public override string LobbyDemoMode_ShortTutorial => "クイックチュートリアル";
        public override string LobbyDemoMode_LongTutorial => "拡張チュートリアル";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "ウィッシュリストに追加";
        public override string BattleLab_StartHere => "ここで戦闘を開始";
        public override string BattleLab_Start => "戦闘を開始";
        public override string BattleLab_Attacker => "攻撃側";

        public override string MapGenerator_Name => "マップエディタ - 生成";

        public override string MapType_CustomMap => "カスタムマップ";
        public override string MapType_GenerateNewMap => "新しいマップを生成";
        public override string MapGenerator_GenerateAction => "生成";
        public override string MapGenerator_Terrain_CustomSize => "カスタムサイズ";
        public override string MapGenerator_Terrain_StartAs => "開始位置";
        public override string MapGenerator_Terrain_ClearPass => "クリアパスを実行";
        public override string MapGenerator_Terrain_BuildPass => "建設パスを実行";
        public override string MapGenerator_Terrain_DigPass => "掘削パスを実行";
        public override string MapGenerator_Terrain_BuildDigLoops => "建設-掘削ループ回数";
        public override string MapGenerator_Terrain_BuildStrokes => "建設ストローク数";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "100タイルあたりのストローク数で測定";
        public override string MapGenerator_Terrain_DigStrokes => "掘削ストローク数";
        public override string MapGenerator_Terrain_CleanUp_Option => "単一タイルのクリーンアップ";
        public override string MapGenerator_Terrain_CleanUpPass => "クリーンアップパスを実行";

        public override string Economy_ServicemenUpkeep => "従者の維持費: {0}";
        public override string Economy_ServicemenUpkeep_Description => "維持費は従者1人あたり{0}ゴールドです";
        public override string Economy_GuardUpkeep_Description => "維持費は衛兵1人あたり{0}ゴールドです";

        public override string EndScreen_TimeHasEndedTitle => "時間切れ";
        public override string Hud_AdvancedSettings => "詳細設定";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "キャンセル";
        public override string Hud_Delete => "削除";
        public override string Hud_Next => "次へ";
        //public override string Hud_None => "なし";
        public override string Hud_Apply => "適用";
        public override string Hud_AllCities => "すべての都市";
        public override string Hud_Time_Hours => "{0} 時間";
        public override string Hud_AddX => "{0} を追加";
        public override string Hud_Both => "両方";
        public override string Hud_Direction => "方向";
        public override string MusicIsBroken => "現在、音楽が再生できません";

        /// <summary>
        /// 0: オブジェクトの種類名, 1: 数量
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, 数: {1}";

        public override string Hud_EffectDoesNotStack => "この効果は重複しません";

        public override string Work_SmeltX => "{0} を精錬";

        public override string Info_TotalFoodProduction => "総食料生産量";
        public override string Info_TotalFoodSpending => "総食料消費量";

        public override string Info_FooodAndDeliveryLocation => "デフォルトでは、労働者は市庁舎に食事またはアイテムを届けに行きます";

        public override string Delivery_SendChunk => "配送ごとのアイテム数";
        public override string Delivery_SpeedBonus => "速度ボーナス: {0}%";

        public override string Delivery_AutoResourceDescription => "在庫上限に達したアイテムを、必要としている都市に配送します";

        public override string Conscript_Soldiers_ArmyType => "兵士";
        public override string Conscript_Soldiers_ArmyType_Description => "隣接する軍に兵士を徴兵します";
        public override string Conscript_Soldiers_GuardType => "都市警備兵";
        public override string Conscript_Soldiers_GuardType_Description => "警備兵は壁の防衛に使用されます";

        public override string Defence_Title => "防衛";
        public override string Defence_GuardPost => "警備所";

        public override string Defence_WallDescription_Movement => "敵の移動を妨げます。";
        public override string Defence_WallDescription_GuardPost => "警備兵を配置できます。";
        public override string Defence_AutoAssign => "自動割り当て";
        public override string Defence_AutoAssign_Description => "新しい警備兵がこの場所に移動します";

        public override string Conscript_SplashDamage => "範囲ダメージ";
        public override string Conscript_HighSplashDamage => "強力な範囲ダメージ";

        public override string Conscript_Training_Champion => "チャンピオン";
        public override string Conscript_Training_Legendary => "伝説";

        public override string Experience_Title => "経験";
        public override string Experience_TopExperience => "最高経験レベル";

        public override string Experience_TimeReductionDescription => "作業時間はレベルごとに{0}%短縮されます";

        public override string ExperienceType_Farm => "農夫";
        public override string ExperienceType_AnimalCare => "動物の世話";
        public override string ExperienceType_HouseBuilding => "家屋建築者";
        public override string ExperienceType_WoodWork => "木工職人";
        public override string ExperienceType_StoneCutter => "石工";
        public override string ExperienceType_Mining => "鉱夫";
        public override string ExperienceType_Transport => "運搬係";
        public override string ExperienceType_Cook => "料理人";
        public override string ExperienceType_Fletcher => "矢職人";
        public override string ExperienceType_RefineOre => "製錬工";
        public override string ExperienceType_Casting => "鋳造師";
        public override string ExperienceType_CraftMetal => "鍛冶師";
        public override string ExperienceType_CraftArmor => "防具職人";
        public override string ExperienceType_CraftWeapon => "武器職人";
        public override string ExperienceType_CraftFuel => "炭焼き職人";
        public override string ExperienceType_Chemist => "化学者";

        public override string ExperienceLevel_1 => "初心者";
        public override string ExperienceLevel_2 => "中級者";
        public override string ExperienceLevel_3 => "上級者";
        public override string ExperienceLevel_4 => "達人";
        public override string ExperienceLevel_5 => "伝説級";

        public override string ExperenceOrDistancePrio_Title => "作業者の選択";
        public override string ExperenceOrDistancePrio_Description => "待機中の作業者は距離または経験に基づいて選ばれます";

        public override string Technology_Description => "各都市には技術ツリーがあります。技術は建物やアイテムを解放します。";
        public override string Experience_Description => "作業者は経験を積み、成長していきます";

        public override string Technology_Title => "技術";
        public override string Technology_ShareField => "技術分野の共有";

        public override string Technology_GainByNeigborRelation => "隣接する都市がこの技術を持ち、関係が{0}であれば: {1}";
        public override string Technology_ForEachMaster => "{0}が経験レベル{1}に到達した時、技術分野: {2}";
        public override string Technology_CitySpread => "隣接する都市同士で技術が共有されます: {0}";
        public override string Technology_CityCapture => "都市が戦闘で占領された場合、ほとんどの技術は失われます";

        public override string Technology_AdvancedBuildings => "高度な建築技術";
        public override string Technology_AdvancedFarming => "高度な農業技術";
        public override string Technology_AdvancedCasting => "高度な鋳造技術";

        public override string Help_Title => "ヘルプ";
        public override string Help_Work_Title => "作業が開始されない";
        public override string Help_Work_Resources => "建物には利用可能な資源が必要です";
        public override string Help_Work_Skill => "作業者には適切なスキルレベル（またはそれ以上）が必要です";
        public override string Help_Work_Stockpile => "倉庫が満杯だと資源の収集が妨げられます";
        public override string Help_Work_Priority => "作業の優先度が低い、またはゼロになっている可能性があります";

        public override string Help_Soldiers_Title => "兵士の生産";
        public override string Help_Soldiers_PlaceBuildingX => "建物を設置: {0}";
        public override string Help_Soldiers_Workers => "募集可能な作業者";
        public override string Help_Soldiers_Weapon => "兵士一人につき武器が必要です";
        public override string Help_Soldiers_StartX => "開始: {0}";

        public override string Hud_SelectHistory => "履歴を選択";

        public override string Hud_PointsPerMinute => "毎分 {0} ポイント";
        public override string Hud_PercentValueCost => "このサービスのコストは価値の {0}% です";

        public override string Hud_Mixed => "混合";
        public override string Hud_Distance => "距離";

        public override string Hud_Unlock => "アンロック";
        public override string Hud_category => "カテゴリ";


        /// <summary>
        /// ゲーム速度を1フレームずつに設定します
        /// </summary>
        public override string Input_StepOneFrame => "1フレーム進める";

        public override string Resource_TypeName_Wagon2Wheel => "小型の荷車";
        public override string Resource_TypeName_Wagon4Wheel => "大型の荷車";
        public override string Resource_TypeName_Tin => "錫";
        public override string Resource_TypeName_TinOre => "錫鉱石";

        public override string Resource_TypeName_Copper => "銅";
        public override string Resource_TypeName_CopperOre => "銅鉱石";
        public override string Resource_TypeName_SilverOre => "銀鉱石";
        public override string Resource_TypeName_Silver => "銀";

        /// <summary>
        /// ミスリルはファンタジー金属です
        /// </summary>
        public override string Resource_TypeName_RawMithril => "未精錬ミスリル";
        public override string Resource_TypeName_Mithril => "ミスリル";

        public override string Resource_TypeName_BronzeSword => "青銅の剣";
        public override string Resource_TypeName_ShortSword => "ショートソード";
        public override string Resource_TypeName_LongSword => "ロングソード";
        public override string Resource_TypeName_HandSpear => "手槍";
        public override string Resource_TypeName_Warhammer => "ウォーハンマー";
        public override string Resource_TypeName_MithrilSword => "ミスリルの剣";
        public override string Resource_TypeName_SlingShot => "投石器";
        public override string Resource_TypeName_ThrowingSpear => "投槍";
        public override string Resource_TypeName_Crossbow => "クロスボウ";
        public override string Resource_TypeName_MithrilBow => "ミスリルの弓";

        public override string Resource_TypeName_CoolingFluid => "冷却液";
        public override string Resource_TypeName_Palisade => "柵";
        public override string Resource_TypeName_Toolkit => "工具セット";

        public override string Resource_TypeName_Sulfur => "硫黄";
        public override string Resource_TypeName_LeadOre => "鉛鉱石";
        public override string Resource_TypeName_Lead => "鉛";
        public override string Resource_TypeName_Bronze => "青銅";
        public override string Resource_TypeName_BloomIron => "鍛鉄";
        public override string Resource_TypeName_Steel => "鋼鉄";
        public override string Resource_TypeName_CastIron => "鋳鉄";

        public override string Resource_TypeName_BlackPowder => "黒色火薬";
        public override string Resource_TypeName_GunPowder => "火薬";
        public override string Resource_TypeName_LedBullet => "弾丸";

        public override string Resource_TypeName_HandCannon => "手砲";
        public override string Resource_TypeName_HandCulverin => "ハンドカルヴァリン";
        public override string Resource_TypeName_Rifle => "ライフル";
        public override string Resource_TypeName_Blunderbuss => "ブランダーバス";

        public override string Resource_TypeName_Manuballista => "マヌバリスタ";
        public override string Resource_TypeName_Catapult => "カタパルト";
        public override string Resource_TypeName_BatteringRam => "破城槌";
        public override string Resource_TypeName_SiegeCannonBronze => "バジリスク砲";
        public override string Resource_TypeName_ManCannonBronze => "ボンバルド";
        public override string Resource_TypeName_SiegeCannonIron => "ハウィッツァー";
        public override string Resource_TypeName_ManCannonIron => "カノン砲";

        public override string Resource_TypeName_PaddedArmor => "布詰め鎧";
        public override string Resource_TypeName_HeavyPaddedArmor => "重布詰め鎧";

        public override string Resource_TypeName_IronArmor => "鎖かたびら";
        public override string Resource_TypeName_HeavyIronArmor => "重鎖かたびら";

        public override string Resource_TypeName_BronzeArmor => "青銅の鎧";

        public override string Resource_TypeName_LightPlateArmor => "プレートアーマー";
        public override string Resource_TypeName_FullPlateArmor => "フルプレートアーマー";
        public override string Resource_TypeName_MithrilArmor => "ミスリルアーマー";
        public override string Resource_TypeName_Coin => "コイン";

        public override string UnitType_Warhammer => "ハンマー騎士";
        public override string UnitType_MithrilKnight => "不死の騎士";
        public override string UnitType_MithrilArcher => "不死の弓兵";
        public override string UnitType_SpearAndShield => "槍盾兵";

        public override string UnitType_CollectionOfSoldiers => "兵士の束";
        public override string UnitType_CollectionOfArmies => "軍隊の束";

        /// <summary>
        /// IDタグは一意の番号になります
        /// </summary>
        public override string UnitId => "（ID {0}）";

        public override string BuildHud_AreaEffectTitle => "範囲効果";
        public override string BuildHud_BonusRadius => "ボーナス範囲：{0}";

        public override string BuildHud_BuildTime => "建設時間";
        public override string SchoolHud_ToLevel => "次のレベルまで";
        public override string SchoolHud_TimeDescription => "時間は経験がゼロの場合を基準とします。経験により短縮されます。";
        public override string SchoolHud_SelectSchool => "学校を選択";
        public override string Upgrade_Order => "アップグレード順";

        public override string Building_ListDescription => "このカテゴリのすべての建物の一覧";

        public override string BuildingType_IsUpgraded => "{0} - 強化済み";
        public override string BuildingType_WoodCutter => "製材所";
        public override string BuildingType_Workshop_Description => "周囲の作業効率を向上させます";

        public override string BuildingType_WoodCutter_AreaAffect => "木からの木材取得量 +{0}%";

        public override string BuildingType_StoneCutter_AreaAffect => "石の取得量 +{0}%";

        public override string BuildingType_StoneCutter => "採石場";

        public override string BuildingType_Embassy => "大使館";
        public override string BuildingType_Embassy_Description => "外交関係のための建物";

        public override string BuildingType_SoldierBarracks => "兵士の兵舎";
        public override string BuildingType_ArcherBarracks => "弓兵の兵舎";
        public override string BuildingType_WarmachineBarracks => "兵器兵の兵舎";
        public override string BuildingType_GunBarracks => "銃兵の兵舎";
        public override string BuildingType_CannonBarracks => "砲兵の兵舎";
        public override string BuildingType_KnightsBarracks => "騎士の兵舎";

        public override string BuildingType_WaterResovoir => "貯水施設";
        public override string BuildingType_WaterResovoir_Description => "水の貯蔵量を増加させます";

        public override string BuildingType_SmeltingFurnace => "製錬炉";
        public override string BuildingType_SmeltingFurnace_Description => "鉱石を金属に精錬します";

        public override string BuildingType_Foundry => "鋳造所";
        public override string BuildingType_Foundry_Description => "金属の鋳造施設";

        public override string BuildingType_Armory => "防具工房";
        public override string BuildingType_Armory_Description => "防具を製作する施設";

        public override string BuildingType_Chemist => "錬金術工房";
        public override string BuildingType_Chemist_Description => "化学薬品の製作所";

        public override string BuildingType_CoinMaker => "造幣所";
        public override string BuildingType_CoinMaker_Description => "金属をお金に変換します";

        public override string BuildingType_Gunmaker => "銃製作所";
        public override string BuildingType_Gunmaker_Description => "銃や大砲を製作する施設";

        public override string BuildingType_School_Tab => "学校";
        public override string BuildingType_School => "職人ギルド";
        public override string BuildingType_School_Description => "労働者のスキルレベルを上げます";

        public override string BuildingType_GoldDelivery => "金貨運搬所";
        public override string BuildingType_Bank_Description => "金貨の管理施設";

        public override string DecorType_CobbleStones => "石畳";
        public override string DecorType_Square => "広場";

        public override string DecorType_Garden => "庭園";
        public override string DecorType_Flag => "旗";
        public override string DecorType_Banner => "バナー";

        public override string BuildingType_DirtRoad => "土の道";
        public override string BuildingType_Palisade => "木柵砦";

        public override string ResourceType_ServiceMen => "サービス係";
        public override string BuildingType_ServiceHouse => "サービス小屋";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "サービス係 +{0}人";

        public override string BuildingType_GuardOffice => "警備詰所";
        public override string BuildingType_GuardOffice_DescriptionAddX => "警備員の上限 +{0}";

        public override string BuildingType_DirtWall => "土壁";
        public override string BuildingType_DirtTower => "土の塔";
        public override string BuildingType_WoodWall => "木の壁";
        public override string BuildingType_WoodTower => "木の塔";
        public override string BuildingType_StoneWall => "石の壁";
        public override string BuildingType_StoneTower => "石の塔";
        public override string BuildingType_StoneGate => "石の門";
        public override string BuildingType_StoneHouse => "石造りの家";

        /// <summary>
        /// 「ランプA」「ランプB」のようなバリエーションを表示する場合
        /// </summary>
        public override string VariantType_A => "{0} A";
        public override string VariantType_B => "{0} B";
        public override string VariantType_C => "{0} C";
        public override string VariantType_D => "{0} D";
        public override string VariantType_E => "{0} E";
        public override string VariantType_F => "{0} F";
        public override string VariantType_G => "{0} G";
        public override string VariantType_H => "{0} H";

        public override string BuildingToolShape_Free => "フリーハンド";
        public override string BuildingToolShape_Area => "四角形";
        public override string BuildingToolShape_Line => "直線";
        public override string BuildingToolShape_LShape => "L字形";

        public override string CityHall_Upgrade => "市庁舎をアップグレード";

        /// <summary>
        /// 街がサポートできる最大の労働者数の上限
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "最大労働者数：{0}";

        public override string CityHall_Size_Small => "村";
        public override string CityHall_Size_Medium => "町";
        public override string CityHall_Size_Large => "首都";

        public override string GuardHousingCount => "警備詰所の収容数";
        public override string ServicemenCount => "サービス要員：{0}";

        public override string Work_MiningResource => "{0}を採掘中";

        public override string MenuTab_Progress => "進行状況";

        public override string Automation_AutomateCity => "街を自動化";
        public override string Automation_AutomationFocus => "自動化の方針";
        public override string Automation_AutomationFocus_Grow => "発展重視";
        public override string Automation_AutomationFocus_Export => "輸出重視";
        public override string Automation_AutomationFocus_War => "戦争重視";

        public override string CityCulture_Smelters_Description => "鉱石の精錬効率を向上";
        public override string CityCulture_Smelters => "精錬師";

        public override string CityCulture_Apprentices_Description => "新しい労働者が熟練者から経験を得る";
        public override string CityCulture_Apprentices => "見習い";

        public override string CityCulture_BronzeCasters_Description => "青銅およびその製品の生産効率が向上";
        public override string CityCulture_BronzeCasters => "青銅鋳造師";


        //DEMO PATCH 1
        /// <summary>
        /// マップ上を徘徊する邪悪なオークたち
        /// </summary>
        public override string FactionName_Barbarian => "ダークホード";
        public override string Tutorial_AttackAndDestroyX => "攻撃して破壊せよ：{0}";
        public override string Resource_TypeName_Pike => "パイク";

        public override string BattleTrials_Title => "戦闘試練";
        public override string BattleTrials_Description => "軍対軍の直接対決で戦術を試そう。";

        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "これらの攻撃はブロック率を下げます";

        public override string Conscript_BlockPerSecond => "1秒間に最大{0}回ブロック可能";

        public override string Conscript_BlockDescription => "兵士は前方の弧内からのほとんどの攻撃をブロックします";

        public override string Map_CustomSeed => "マップシード";

        public override string Settings_Mode_Spectator => "観戦モード";

        public override string Settings_Mode_Spectator_Description => "観戦のみ";

        public override string Automation_AutomationFocus_NoFocus_Description => "すべてを少しずつ建設します";

        public override string Automation_AutomationFocus_WillProduce => "主に生産するもの：";

        public override string Help_Food_WhoEats => "すべての兵士と労働者は食料を消費します";

        public override string Help_Food_BigArmy => "大軍はその地域の都市を飢えさせる可能性があります";

        public override string Help_Food_DontBuild => "農場を増やしても自動的に食料は増えません。収穫と加工のための労働者と調理設備が必要です";

        public override string Help_Food_UseWater => "食料生産には水が必要です";

        public override string Help_Food_Postal => "都市同士で食料を送り合って支援し合いましょう";

        public override string Message_LostCity => "都市を失いました";

        public override string Demo_Description => "短いシナリオ：{0}分間都市を防衛してください";

        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "デモはあと {0} 分で終了します";

        public override string Experience_Required => "必要な経験値";

        public override string InputActionName_ToggleMenu => "メニューの切り替え";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "資源はゼロ未満になったり、備蓄上限をわずかに超えたりすることがあります。制限は作業キューが作成されるときにのみ適用されます。";

        public override string Work_SelectCategory => "アイテムカテゴリを選択";
        public override string Hud_RemoveFromList => "リストから削除";

        public override string Hud_ReturnToPrevious => "戻る";
        public override string Hud_Close => "閉じる";

        public override string Hud_Low => "低";
        public override string Hud_Medium => "中";
        public override string Hud_High => "高";

        public override string Hud_Copy => "コピー";
        //public override string Hud_Paste => "貼り付け";
        public override string Hud_Cut => "切り取り";
        public override string Hud_SaveCompleted => "保存完了";

        public override string Settings_WaterMultiplier => "水の倍率";
        public override string Settings_WaterMultiplier_Description => "都市が生産・貯蔵する水の量を決定します。値が高いとパフォーマンスが低下します。";

        public override string Settings_ChildMultiplier => "出生倍率";
        public override string Settings_CraftMultiplier => "生産速度倍率";
        public override string Settings_CraftMultiplier_Description => "値が低いほど生産が速くなります。";

        public override string FastProduction => "高速生産";
        public override string SlowProduction => "低速生産";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "生産不可";

        //public override string CityAutomation_WaitForMaxPopulation => "人口が最大になるまで待機";
        public override string Automation_AutomationFocus_NoFocus => "すべて";
        public override string CityAutomation_SoldierQuality => "兵士の質";
        public override string CityAutomation_SoldierWeaponType => "武器の種類";

        public override string WarsResourceGroup_Resources => "資源";
        public override string WarsResourceGroup_Weapons => "武器";

        public override string WarsResourceGroup_AllWeaponTypes => "混合";
        public override string WarsResourceGroup_MeleeHandWeapons => "近接";
        public override string WarsResourceGroup_RangedHandWeapons => "遠距離";
        public override string WarsResourceGroup_Warmachines => "攻城兵器";

        public override string FactionSettings_Titel => "派閥全体の設定";
        public override string FactionSettings_Description => "すべての都市に適用されます";

        public override string Conscript_MaxPopulation => "最大人口";
        public override string Conscript_MaxPopulation_Description => "人口が最大のときのみ徴兵されます";

        public override string Conscript_FoodAbundance => "食料が最大のとき";
        public override string Conscript_FoodAbundance_Description => "食料が最大備蓄に達したときのみ徴兵されます";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "設定：オン";
        public override string GeneralSetting_Off => "設定：オフ";
        public override string GeneralSetting_AllBuildingsDescription => "すべての建物に適用されます";

        public override string GeneralSetting_ApplyMessage => "{0} 件の建物に変更を適用しました";

        public override string MustTurnOffSteamInput => "コントローラーを使用するには、Steam Input を無効にしてください。";

        public override string Technology_GainTitle => "技術を習得する方法";
        public override string Technology_LevelUp => "レベルアップ";
        public override string Technology_ForEachLevelUp => "作業者が技術分野でレベルアップしたとき：{0}";

        public override string VoxelEditor_Description => "ブロック状のモデルを作成";

        public override string Editor_Tool => "ツール";
        public override string Editor_SelectOptionsMenu => "選択オプション";
        public override string Editor_Continous => "連続";

        public override string Editor_Tool_PencilSize => "ペンのサイズ";
        public override string Editor_Tool_SizeTolerance => "サイズ許容範囲";
        public override string Editor_Tool_RoundPencil => "丸ペン";
        public override string Editor_Tool_EdgeSize => "境界サイズ";
        public override string Editor_Tool_PercentFill => "塗りつぶし率";
        public override string Editor_Tool_ClearAbove => "上をクリア";
        public override string Editor_Tool_FillBelow => "下を塗りつぶし";

        public override string Editor_UserModels => "ユーザーモデル";
        public override string Editor_UserModels_Description => "保存したモデルを表示";

        public override string Editor_RetailModels => "ゲーム内モデル";
        public override string Editor_RetailModels_Description => "ゲームからモデルを読み込む";

        public override string Editor_ModTemplates => "MOD用テンプレート";
        public override string Editor_ExportAsOBJ => ".OBJ 形式でエクスポート";
        public override string Editor_SelectAll => "すべて選択";

        public override string Editor_Canvas_Title => "キャンバス";
        public override string Editor_Canvas_Size => "サイズ";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "サイズプリセット";
        public override string Editor_Canvas_Move => "移動";
        public override string Editor_Canvas_Move_Up => "上へ";
        public override string Editor_Canvas_Move_Down => "下へ";
        public override string Editor_Canvas_RotateClockwise => "時計回りに回転";
        public override string Editor_Canvas_RotateCounterClockwise => "反時計回りに回転";
        public override string Editor_Canvas_Mirror => "反転";

        public override string Editor_Canvas_RotateFlip_Title => "回転／反転";
        public override string Editor_Canvas_FlipVertical => "上下反転";
        public override string Editor_Canvas_FlipOrientation => "横向き／縦向きを切替";
        public override string Editor_Canvas_ClearAll_Description => "すべてのブロックとフレームを削除";

        public override string Editor_Animation => "アニメーション";
        public override string Editor_Animation_RemoveCurrentFrame => "現在のフレームを削除";
        public override string Editor_Animation_AddFrameCopy => "コピーとしてフレームを追加";
        public override string Editor_Animation_AddEmptyFrame => "空のフレームを追加";
        public override string Editor_Animation_MoveDescription => "フレーム位置の変更";
        public override string Editor_Animation_AllFrames => "すべてのフレーム";
        public override string Editor_Animation_AllFrames_ActionDescription => "すべてのフレームに同じ操作を実行";

        public override string Editor_SettingsMenu => "設定";
        public override string Hud_Exit => "終了";
        public override string Editor_Canvas_Clear => "クリア";

        public override string Editor_Stamp => "スタンプ";
        public override string Editor_StampOtherFrames => "他のフレームにスタンプ";
        public override string Editor_StampOtherFrames_Description => "このボクセルを他のフレームに貼り付け";
        public override string Editor_PasteToFrame => "このフレームに貼り付け";
        public override string Editor_ClearAllFrames => "すべてのフレームをクリア";
        public override string Editor_ClearOtherFrames => "他のフレームをクリア";

        public override string Editor_Settings_MoveSpeed => "移動速度";
        public override string Editor_Settings_BackgroundColor => "背景色";
        public override string Editor_Settings_HideHUD => "HUDを非表示";

        public override string Editor_Color => "色";
        public override string Editor_ColorsInUseLabel => "使用中の色";
        public override string Editor_Color_BrighterPlus => "もっと明るく";
        public override string Editor_Color_Brighter => "明るく";
        public override string Editor_Color_Darker => "暗く";
        public override string Editor_Color_DarkerPlus => "もっと暗く";
        public override string Editor_Color_RedTint => "赤み";
        public override string Editor_Color_Tint => "色合い";
        public override string Editor_Color_GreenTint => "緑み";
        public override string Editor_Color_BlueTint => "青み";
        public override string Editor_Color_YellowTint => "黄み";
        public override string Editor_Color_PurpleTint => "紫み";
        public override string Editor_NoColor => "なし";

        public override string Editor_Material => "素材";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "色を変更";
        public override string Editor_Color_RecolorTo => "次の色に変更";

        public override string Editor_Material_Set => "素材を設定";

        public override string Editor_Preview => "プレビュー";
        public override string Editor_CombineWithCurrent => "現在のモデルと結合";

        public override string Editor_PickedColor => "選択色";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "移民テント";
        public override string BuildingType_ImmigrationTent_Description => "{0} 人の移民を収容";
        public override string BuildingType_ReseachCenter => "研究センター";
        public override string BuildingType_Bookpress => "印刷所";
        public override string BuildingType_Bookpress_Description => "1つの研究分野で得られたポイントは、他の都市にあるすべての {0} に共有されます。";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "例：作業者が {0} を生産すると、{1} スキルが上昇します。レベルアップ時に {2} 技術にポイントが加算されます（同じ分野を共有しているため）。";

        public override string BuildingType_Research_BaseDescription => "技術研究を強化します。";

        public override string BuildingType_ResearchCenter_Description => "同じ分野で作業者がレベルアップすると、技術研究ポイントが {0} 増加します。";


        //DEMO PATCH 5
       

        public override string Editor_CropSelection => "選択範囲でトリミング";

        public override string Immigrants_DisbandedSoldiers => "解散された兵士は移住してきます";
        public override string Immigrants_RefillWorkers => "労働力をすばやく補充します";
        public override string Immigrants_UnhousedAreLost => "住居のない移民はしばらくすると消えます";
        public override string Editor_VoxelCount => "{0} ボクセル";

        public override string Editor_Layers_Titel => "レイヤー";
        public override string Editor_Layers_All => "すべてのレイヤー";
        public override string Editor_LayerNumber => "レイヤー {0}";

        public override string Editor_Layer_AddEmpty => "空のレイヤーを追加";
        public override string Editor_Layer_AddCopy => "レイヤーを複製";
        public override string Editor_Layer_Remove => "レイヤーを削除";
        public override string Editor_Layer_MergeDown => "下のレイヤーと結合";
        public override string Editor_IsAnimated => "アニメーションあり";
        public override string Editor_ToggleVisible => "表示を切り替え";
        public override string Editor_ToggleAnimatedLayer => "アニメーションレイヤーを切り替え";
        public override string Editor_Projects => "プロジェクトファイル";
        public override string ProfileEditor_ReplaceMaterial => "プロフィールカラー: {0}";

        public override string ProfileEditor_ProfileColors_Label => "プロフィールカラー";
        public override string ProfileEditor_TunicColor => "チュニックの色";
        public override string ProfileEditor_PantsColor => "ズボンの色";
        public override string ProfileEditor_LeaderColor => "リーダーの色";

        public override string MapStartAs_Water => "水";
        public override string MapStartAs_Land => "陸";
        public override string MapStartAs_Circle => "円形";

        public override string Hud_NeedToBeAssigned => "割り当てが必要です";
        public override string Hud_CommitAssignment => "割り当てる";
        public override string Technology_NoAvailableResearch => "利用可能な研究がありません";

        public override string Research_Tab => "研究";

        //5.2
        public override string BuildCategory_General => "一般";
        public override string BuildCategory_Military => "軍事";
        public override string BuildCategory_Decoration => "装飾";
        public override string BuildCategory_Upgrade => "アップグレード";
        public override string Work_NoMines => "鉱山がありません";




    }
}
