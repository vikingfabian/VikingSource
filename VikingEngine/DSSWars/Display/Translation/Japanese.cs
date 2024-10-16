using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
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
        public override string Lobby_LocalMultiplayerEdit => "ローカルマルチプレイヤー ({0})";

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
        public override string ProfileEditor_SaveAndExit => "保存して終了";

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

        public override string Hud_IncreaseMaxGuardCount => "最大守衛数 +{0}";

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
        public override string Hud_Immigrants => "移民: {0}";

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
        /// ボタン入力の説明。次の都市に移動。
        /// </summary>
        public override string Input_NextCity => "次の都市";

        /// <summary>
        /// ボタン入力の説明。次の軍隊に移動。
        /// </summary>
        public override string Input_NextArmy => "次の軍隊";

        /// <summary>
        /// ボタン入力の説明。次の戦闘に移動。
        /// </summary>
        public override string Input_NextBattle => "次の戦闘";

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
        public override string EventMessage_DesertersText => "未払いの兵士たちがあなたの軍隊から脱走しています";

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

        public override string GameMenu_SaveState => "保存";
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
        public override string Input_ToggleHudDetail => "HUD詳細の切り替え";

        /// <summary>
        /// ボタン入力を説明します。マップとHUDの選択を切り替えます
        /// </summary>
        public override string Input_ToggleHudFocus => "メニューフォーカス";

        /// <summary>
        /// ボタン入力を説明します。最新のポップアップをクリックするショートカット
        /// </summary>
        public override string Input_ClickMessage => "メッセージをクリック";

        /// <summary>
        /// ボタン入力を説明します。一般的な移動方向
        /// </summary>
        public override string Input_Up => "上";

        /// <summary>
        /// ボタン入力を説明します。一般的な移動方向
        /// </summary>
        public override string Input_Down => "下";

        /// <summary>
        /// ボタン入力を説明します。一般的な移動方向
        /// </summary>
        public override string Input_Left => "左";

        /// <summary>
        /// ボタン入力を説明します。一般的な移動方向
        /// </summary>
        public override string Input_Right => "右";

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
        public override string Resource_TypeName_SkinAndLinen => "皮とリネン";
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
        public override string Hud_Queue => "キュー";

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

        public override string WorkForce_ChildBirthRequirements => "出生要件:";
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
    }
}
