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
        /// 2つのランダムな単語を組み合わせる方法
        /// </summary>
        public override string NameGenerator_AOfTheB => "{1}の{0}";

        static readonly List<string> adjectives = new List<string> {
    "勇敢な", "神秘的な", "暗黒の", "黄金の", "古代の", "凍てつく", "永遠の",
    "影のある", "明るい", "紅の", "猛烈な", "栄光の", "高貴な", "野蛮な",
    "復讐の", "勇敢な", "嵐の", "荘厳な", "無慈悲な", "狡猾な", "輝かしい",
    "黄昏の", "夜明けの", "夕暮れの", "鉄の", "銀の", "幽霊の", "天の", "地獄の",
    "魔法の", "秘術の", "隠れた", "失われた", "忘れ去られた", "伝説の", "神話の",
    "静かな", "雷鳴の", "燃える", "砕けた", "さまよう", "空想的な", "幻影の",
    "エメラルドの", "ルビーの", "サファイアの", "ダイヤモンドの", "翡翠の", "強力な"
};

        static readonly List<string> colors = new List<string> {
    "赤", "黒", "白", "エメラルド", "蒼", "緋色", "紫", "藍色",
    "金", "銀", "青銅", "銅", "サファイア", "ルビー", "アメジスト",
    "翡翠", "セルリアン", "紅", "マゼンタ", "黒檀", "象牙", "青緑", "ターコイズ",
    "栗色", "オリーブ", "桃色", "灰色", "木炭", "ラベンダー", "ライム", "ネイビー",
    "黄土色", "プラム", "石英", "サーモン", "褐色", "ウルトラマリン", "朱色", "藤色",
    "シャナドゥ", "黄", "ザフレ", "アズール", "青", "緑", "ハニーデュー",
    "アイリス", "ジャスミン", "カーキ"
};

        static readonly List<string> creatures = new List<string> {
    "ドラゴン", "狼", "鷲", "ライオン", "騎士", "グリフィン", "ケンタウロス",
    "エルフ", "ドワーフ", "巨人", "天使", "人魚", "ユニコーン",
    "フェニックス", "牡鹿", "馬", "隼", "虎", "熊", "豹",
    "鷲", "鷹", "イルカ", "鯨", "象", "ヒョウ", "チーター",
    "カラス", "フクロウ", "孔雀", "白鳥", "狐", "鹿",
    "パラディン", "魔術師", "魔法使い", "盗賊", "侍", "忍者",
    "弓兵", "レンジャー", "聖職者", "僧侶", "シャーマン", "ドルイド",
    "スフィンクス", "ペガサス", "クーガー", "ジャガー", "雄牛", "蛇"
};

        static readonly List<string> places = new List<string> {
    "森", "荒地", "廃墟", "樫の木", "山", "湖", "川", "海",
    "城", "塔", "地下牢", "洞窟", "宮殿", "神殿", "祠",
    "庭園", "村", "都市", "王国", "帝国", "砂漠", "氷河",
    "火山", "谷", "崖", "要塞", "港", "島", "半島",
    "平原", "泥沼", "礁", "サバンナ", "ツンドラ", "冥界", "渦",
    "泉", "木立", "牧草地", "フィヨルド", "峡谷", "高原", "湿地",
    "沼地", "森の林間地", "月", "星", "銀河", "星雲", "小惑星",
    "彗星", "流星", "ブラックホール", "虚無", "ネクサス", "次元", "聖域",
    "アリーナ", "コロシアム", "アカデミー", "図書館", "アーカイブ"
};

        static readonly List<string> titles = new List<string> {
    "軍団", "旅団", "大隊", "連隊", "師団", "中隊", "小隊",
    "分遣隊", "派遣隊", "ファランクス", "隊", "チーム", "ユニット", "部隊",
    "ホスト", "大群", "軍隊", "海軍", "艦隊", "船団", "航空団",
    "グループ", "群れ", "サークル", "評議会", "集会", "ギルド", "騎士団",
    "同盟", "氏族", "部族", "親族", "王朝", "帝国", "王家",
    "公国", "男爵領", "支部", "契約", "連合", "同盟",
    "連邦", "同盟", "協会", "アカデミー", "研究所", "人々", "民"
};

        static readonly List<string> symbols = new List<string> {
    "百合", "塔", "槍", "盾", "王冠", "剣", "城", "星",
    "月", "太陽", "彗星", "炎", "波", "山", "木", "森",
    "川", "石", "金床", "ハンマー", "斧", "弓", "矢", "矢筒",
    "兜", "籠手", "鎧", "鎖", "鍵", "指輪", "錠", "本", "巻物",
    "ポーション", "宝玉", "玉座", "旗", "指輪", "宝石", "ピラミッド", "オベリスク",
    "塔", "橋", "門", "壁", "聖杯", "ランタン", "ろうそく", "鐘",
    "羽", "ガラス", "コンパス"
};

        /// <summary>
        /// パフォーマンスのために静的リストを返すことが重要です
        /// </summary>
        public override List<string> NameGenerator_Army_Adjectives => adjectives;
        public override List<string> NameGenerator_Army_Colors => colors;
        public override List<string> NameGenerator_Army_Creatures => creatures;
        public override List<string> NameGenerator_Army_Places => places;
        public override List<string> NameGenerator_Army_Titles => titles;
        public override List<string> NameGenerator_Army_Symbols => symbols;

        static readonly List<string> generalSyllables = new List<string>
{
    "アー", "ベル", "カー", "ダン", "エル", "フェン", "グレン", "ハル", "イバー", "ジュン",
    "ケル", "リム", "モン", "ノー", "オーク", "ペル", "クエン", "リル", "セン", "タル",
    "ウルン", "ベル", "ウェル", "ゼン", "イエル", "ゼル", "アッシュ", "ブロ", "クレ", "デル",
    "エック", "フェイ", "ギル", "ハー", "イスク", "ジョー", "ケイ", "ロン", "ミア", "ノック",
    "オルプ", "ペン", "クイル", "ロスト", "サーン", "ティル", "ウド", "ヴェルン", "ウィスト", "ヤーン", "ゾーン"
};
        static readonly List<string> generalTownSuffixes = new List<string>
{ "タウン", "フォード", "バーグ", "ビル", "ステッド", "ウィック", "モント", "フィールド", "ポート", "デール" };

        static readonly List<string> northSyllables = new List<string>
{
    "フィヨル", "スコル", "ヴァルグ", "ウルフ", "フロスト", "ビョルン", "ストル", "ホワイト", "ジャーン", "スネー",
    "クヴィスト", "ルンド", "ノルド", "オルフ", "ピル", "ルーン", "シグ", "トール", "ウルフ", "ヴァルド",
    "イング", "エーグ", "ブリム", "ドラク", "エイル", "フレイ", "ギム", "ハルヴ", "イーヴァル", "ジョー",
    "ケルド", "リング", "マグン", "ナット", "オーディン", "プライド", "クォー", "ロスト", "シフ", "チョルン",
    "ウルフル", "ヴィド", "ウィンド", "シル", "イール", "ゾーン", "エスク", "ブロク", "ダール", "エング"
};
        static readonly List<string> northTownSuffixes = new List<string>
{ "ヴィク", "スタッド", "フィヨルド", "ベルク", "ネス", "ダル", "ハイム", "ガルド", "ハヴン", "ランド", "ウル" };

        static readonly List<string> westSyllables = new List<string>
{
    "ウィン", "ラン", "ハム", "フォード", "スター", "バーグ", "シャイア", "ウェル", "トン", "ウィック",
    "バード", "クリフ", "デル", "エス", "グラフ", "ホルト", "アイア", "ジェスト", "ケント", "リー",
    "ムーア", "ノー", "オックス", "ペリー", "クエン", "リフト", "スワード", "トレ", "ウルム", "ヴァー",
    "ウォー", "ヤート", "ジール", "アード", "ビーム", "コーヴ", "デール", "エフト", "ゲイル", "ヘース",
    "イングル", "キール", "レイス", "マーシュ", "ニース", "オープ", "ペール", "クイル", "ローブ", "スケイル", "サッチ"
};
        static readonly List<string> westTownSuffixes = new List<string>
{ "トン", "バーグ", "フォード", "ハム", "シャイア", "キャスター", "ウィック", "ベリー", "ステッド", "ビル" };

        static readonly List<string> eastSyllables = new List<string>
{
    "ジン", "シ", "ユアン", "チン", "ル", "チュン", "ミン", "ナン", "ピン", "ジョウ",
    "バイ", "ドン", "フ", "グオ", "フイ", "カイ", "ラン", "メイ", "ニ", "オウ",
    "ペイ", "チン", "ラン", "スー", "タイ", "ウェイ", "シ", "ヤン", "ジュ", "アン",
    "ボ", "シ", "ダ", "エン", "フェイ", "ガン", "ハオ", "ジ", "ケン", "レイ",
    "モ", "ニン", "ポ", "チ", "ロウ", "セン", "ティン", "ワン", "シン", "ユ", "ゼン"
};
        static readonly List<string> eastTownSuffixes = new List<string>
{ "ヤン", "シャン", "ジ", "アン", "ハイ", "チョン", "リン", "タイ", "コウ", "フ" };

        static readonly List<string> southSyllables = new List<string>
{
    "セ", "ネ", "リ", "カ", "レ", "シ", "マル", "ポル", "アス", "コル",
    "デル", "エフ", "ガ", "ヘル", "イオ", "コス", "ラ", "メ", "ニ", "オル",
    "パ", "ロ", "セ", "ティ", "ウル", "ヴェ", "クサン", "イラ", "ゼ", "アル",
    "ブラ", "シ", "ドラ", "エロ", "フ", "グレ", "ハイ", "イル", "ジョ", "クレ",
    "リュー", "マイ", "ノス", "オペ", "フィ", "クエ", "ラ", "シル", "サ", "ヴィル", "ウィン"
};
        static readonly List<string> southTownSuffixes = new List<string>
{ "ポリス", "イウム", "オス", "ウス", "ア", "オン", "オラ", "アカ", "エス", "アイ" };

        public override List<string> NameGenerator_City_GeneralSyllables => generalSyllables;
        public override List<string> NameGenerator_City_GeneralTownSuffixes => generalTownSuffixes;
        public override List<string> NameGenerator_City_NorthSyllables => northSyllables;
        public override List<string> NameGenerator_City_NorthTownSuffixes => northTownSuffixes;
        public override List<string> NameGenerator_City_WestSyllables => westSyllables;
        public override List<string> NameGenerator_City_WestTownSuffixes => westTownSuffixes;
        public override List<string> NameGenerator_City_EastSyllables => eastSyllables;
        public override List<string> NameGenerator_City_EastTownSuffixes => eastTownSuffixes;
        public override List<string> NameGenerator_City_SouthSyllables => southSyllables;
        public override List<string> NameGenerator_City_SouthTownSuffixes => southTownSuffixes;

    }
}
