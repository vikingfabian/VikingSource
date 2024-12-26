using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    partial class SimplifiedChinese
    {
        //名称生成器通过组合随机单词为军队创建唯一的名称
        //名称不需要逐字翻译，列表中的名称数量不重要，如果有更多或更少的选项，游戏会自动适应

        /// <summary>
        /// 合并随机单词的一种方式
        /// </summary>
        public override string NameGenerator_AOfTheB => "{0} 的 {1}";

        static readonly List<string> adjectives = new List<string> {
    "英勇的", "神秘的", "黑暗的", "金色的", "古老的", "冰冻的", "永恒的",
    "阴影的", "光明的", "赤红的", "凶猛的", "光辉的", "高贵的", "野蛮的",
    "复仇的", "勇敢的", "暴风的", "庄严的", "无情的", "狡猾的", "光芒的",
    "黄昏的", "黎明的", "暮光的", "铁的", "银的", "幽灵的", "天上的", "地狱的",
    "魔法的", "神秘的", "隐藏的", "失落的", "被遗忘的", "传奇的", "神话的",
    "沉默的", "雷鸣的", "燃烧的", "破碎的", "流浪的", "虚幻的", "幽灵的",
    "翡翠的", "红宝石的", "蓝宝石的", "钻石的", "翡翠的", "强壮的"
};

        static readonly List<string> colors = new List<string> {
    "红色", "黑色", "白色", "翡翠", "天蓝色", "猩红色", "紫色", "靛蓝",
    "金色", "银色", "青铜", "铜色", "蓝宝石", "红宝石", "紫水晶",
    "玉", "蔚蓝色", "赤红色", "洋红色", "乌木", "象牙", "蓝绿色", "绿松石",
    "栗色", "橄榄色", "桃色", "灰色", "木炭色", "薰衣草", "酸橙色", "海军蓝",
    "赭色", "梅子色", "石英色", "鲑鱼色", "棕褐色", "深蓝色", "朱红色", "紫藤色",
    "仙度", "黄色", "藏蓝色", "天蓝色", "蓝色", "绿色", "蜜瓜色",
    "鸢尾色", "茉莉色", "卡其色"
};

        static readonly List<string> creatures = new List<string> {
    "龙", "狼", "鹰", "狮子", "骑士", "狮鹫", "半人马",
    "精灵", "矮人", "巨人", "天使", "美人鱼", "独角兽",
    "凤凰", "牡鹿", "马", "猎鹰", "老虎", "熊", "黑豹",
    "鹰", "鹰", "海豚", "鲸鱼", "大象", "豹子", "猎豹",
    "渡鸦", "猫头鹰", "孔雀", "天鹅", "狐狸", "鹿",
    "圣骑士", "巫师", "法师", "盗贼", "武士", "忍者",
    "弓箭手", "游侠", "牧师", "祭司", "萨满", "德鲁伊",
    "狮身人面像", "天马", "美洲狮", "美洲豹", "公牛", "蛇"
};

        static readonly List<string> places = new List<string> {
    "森林", "废墟", "遗迹", "橡树", "山脉", "湖泊", "河流", "海洋",
    "城堡", "塔楼", "地牢", "洞穴", "宫殿", "寺庙", "神殿",
    "花园", "村庄", "城市", "王国", "帝国", "沙漠", "冰川",
    "火山", "山谷", "悬崖", "堡垒", "港口", "岛屿", "半岛",
    "平原", "沼泽", "礁石", "草原", "苔原", "冥界", "漩涡",
    "泉源", "树林", "草地", "峡湾", "峡谷", "高原", "沼泽",
    "湿地", "林间空地", "月亮", "星星", "银河", "星云", "小行星",
    "彗星", "流星", "黑洞", "虚空", "枢纽", "维度", "圣域",
    "竞技场", "斗兽场", "学院", "图书馆", "档案馆"
};

        static readonly List<string> titles = new List<string> {
    "军团", "旅", "队", "营", "团", "师", "连",
    "中队", "排", "部队", "分遣队", "分队", "方阵", "小队",
    "队", "单位", "力量", "主机", "部落", "军队", "海军", "舰队", "船队",
    "翼", "组", "包", "圈", "议会", "议会", "集会", "公会", "命令",
    "团体", "氏族", "部落", "亲属", "王朝", "帝国", "国王卫队",
    "公国", "公国", "男爵领", "章节", "盟约", "辛迪加",
    "联盟", "联盟", "邦联", "联邦", "联赛", "协会",
    "学院", "研究所", "人", "人", "力量"
};

        static readonly List<string> symbols = new List<string> {
    "百合", "塔", "矛", "盾", "皇冠", "剑", "城堡", "星",
    "月亮", "太阳", "彗星", "火焰", "波浪", "山", "树", "森林",
    "河", "石", "铁砧", "锤", "斧", "弓", "箭", "箭筒",
    "头盔", "手套", "盔甲", "链", "钥匙", "戒指", "锁", "书", "卷轴",
    "药水", "宝珠", "王座", "横幅", "戒指", "宝石", "金字塔", "方尖碑",
    "塔", "桥", "门", "墙", "圣杯", "灯笼", "蜡烛", "钟",
    "羽毛", "玻璃", "指南针"
};

        /// <summary>
        /// 返回静态列表对性能很重要
        /// </summary>
        public override List<string> NameGenerator_Army_Adjectives => adjectives;
        public override List<string> NameGenerator_Army_Colors => colors;
        public override List<string> NameGenerator_Army_Creatures => creatures;
        public override List<string> NameGenerator_Army_Places => places;
        public override List<string> NameGenerator_Army_Titles => titles;
        public override List<string> NameGenerator_Army_Symbols => symbols;


        /*
        * 名称生成器通过组合随机音节创建城市的唯一名称
        * 音节分为普通、北方（北欧风格）、西方（古英语）、东方（亚洲）和南方（地中海）
        * 当本地化到接近英语的语言时，不需要翻译
        * 
        * 列表中的名称数量不重要，游戏会自动适应，如果有更多或更少的选项
        */

        static readonly List<string> generalSyllables = new List<string>
{
    "阿尔", "贝尔", "卡尔", "顿", "埃尔", "芬", "格伦", "哈尔", "伊弗", "君",
    "凯尔", "林", "蒙", "诺尔", "橡树", "佩尔", "昆", "瑞尔", "森", "塔尔",
    "厄恩", "维尔", "维尔", "赛恩", "耶尔", "泽尔", "阿什", "布罗", "克雷", "德尔",
    "艾克", "菲", "吉尔", "赫尔", "伊斯克", "约尔", "凯", "朗", "米尔", "诺克",
    "奥尔普", "彭", "奎尔", "罗斯特", "萨恩", "蒂尔", "厄德", "维恩", "维斯特", "亚恩", "佐恩"
};
        static readonly List<string> generalTownSuffixes = new List<string>
{ "镇", "福特", "堡", "村", "庄", "维克", "蒙特", "田", "港", "谷" };

        static readonly List<string> northSyllables = new List<string>
{
    "弗约", "斯科尔", "瓦尔格", "乌尔夫", "霜", "比约恩", "斯托尔", "维特", "亚恩", "斯尼",
    "克维斯特", "伦德", "诺德", "奥尔夫", "皮尔", "符文", "西格", "托尔", "乌尔夫", "瓦尔德",
    "英格", "埃格", "布里姆", "德拉克", "埃尔", "弗雷", "吉姆", "哈尔夫", "伊瓦尔", "乔",
    "凯尔德", "林", "马格恩", "纳特", "奥丁", "普赖德", "奎尔", "罗斯特", "西夫", "乔恩",
    "乌尔夫尔", "维德", "风", "克西尔", "尤尔", "佐恩", "埃斯克", "布罗克", "达尔", "恩格"
};
        static readonly List<string> northTownSuffixes = new List<string>
{ "维克", "斯塔德", "峡湾", "山", "尼斯", "达尔", "海姆", "加德", "港", "地", "乌尔" };

        static readonly List<string> westSyllables = new List<string>
{
    "温", "兰", "汉", "福特", "斯特", "堡", "郡", "井", "顿", "维克",
    "巴德", "克利夫", "德尔", "埃斯", "格拉夫", "霍尔特", "艾尔", "杰斯特", "肯特", "莱",
    "沼泽", "诺", "牛", "佩里", "昆", "裂谷", "草地", "特雷", "乌尔姆", "维尔",
    "沃尔", "亚特", "热情", "阿德", "光束", "海湾", "山谷", "艾夫特", "风", "荒地",
    "英格尔", "基尔", "利斯", "沼泽", "尼斯", "奥普", "佩尔", "奎尔", "罗夫", "规模", "茅草"
};
        static readonly List<string> westTownSuffixes = new List<string>
{ "顿", "堡", "福特", "村", "郡", "卡斯特", "维克", "伯里", "庄", "村" };

        static readonly List<string> eastSyllables = new List<string>
{
    "金", "石", "元", "青", "路", "春", "明", "南", "平", "周",
    "白", "东", "富", "国", "辉", "开", "兰", "梅", "尼", "欧",
    "佩", "琴", "然", "苏", "泰", "伟", "西", "杨", "朱", "安",
    "波", "次", "达", "恩", "飞", "港", "浩", "吉", "肯", "雷",
    "莫", "宁", "坡", "齐", "柔", "森", "亭", "万", "星", "玉", "禅"
};
        static readonly List<string> eastTownSuffixes = new List<string>
{ "阳", "山", "吉", "安", "海", "城", "林", "台", "口", "府" };

        static readonly List<string> southSyllables = new List<string>
{
    "瑟", "尼", "莱", "卡", "瑞", "西", "马尔", "波尔", "阿斯", "科尔",
    "德尔", "埃夫", "加", "赫尔", "约", "科斯", "拉", "梅", "尼", "奥尔",
    "帕", "若", "西", "泰", "乌尔", "维", "赞", "伊拉", "泽", "阿尔",
    "布拉", "赛", "德拉", "埃罗", "费", "格瑞", "海", "伊莱", "约", "克莱",
    "留", "米", "诺斯", "奥普", "菲", "奎", "瑞", "赛尔", "塔", "维尔", "温"
};
        static readonly List<string> southTownSuffixes = new List<string>
{ "波利斯", "伊姆", "奥斯", "乌斯", "阿", "翁", "奥拉", "阿卡", "埃斯", "艾" };

        /// <summary>
        /// 返回静态列表对性能很重要
        /// </summary>
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
