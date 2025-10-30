using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Korean
    {
        //// <summary>
        /// 무작위 단어 조합으로 군대 이름을 생성합니다.
        /// 직역보다는 자연스러운 어감과 판타지 분위기를 살렸습니다.
        /// </summary>
        public override string NameGenerator_AOfTheB => "{1}의 {0}";

        static readonly List<string> adjectives = new List<string> {
    "용맹한", "신비한", "어둠의", "황금의", "고대의", "얼어붙은", "영원의",
    "그림자", "찬란한", "진홍의", "사나운", "영광의", "고귀한", "야만의",
    "복수의", "용감한", "폭풍의", "장엄한", "무자비한", "교활한", "빛나는",
    "황혼의", "새벽의", "황혼", "강철의", "은빛", "유령의", "천상의", "지옥의",
    "마법의", "비밀의", "잃어버린", "망각의", "전설의", "신화의",
    "고요한", "천둥의", "불타는", "산산조각난", "방랑하는", "에테르의", "환영의",
    "에메랄드의", "루비의", "사파이어의", "다이아몬드의", "비취의", "강한"
};

        static readonly List<string> colors = new List<string> {
    "붉은", "검은", "하얀", "에메랄드", "푸른", "진홍", "보랏빛", "남색",
    "황금", "은빛", "청동", "구리빛", "사파이어", "루비", "자수정",
    "비취", "하늘빛", "진홍", "자홍", "칠흑", "상아빛", "청록", "터키옥",
    "적갈색", "올리브빛", "복숭아빛", "회색", "재빛", "연보라", "라임빛", "군청",
    "황토빛", "자두빛", "수정빛", "연어빛", "황갈빛", "심해빛", "주홍빛", "등나무빛",
    "연초록", "노란", "청금석빛", "하늘", "초록", "꿀빛",
    "창포빛", "자스민빛", "카키빛"
};

        static readonly List<string> creatures = new List<string> {
    "용", "늑대", "독수리", "사자", "기사", "그리핀", "켄타우로스",
    "엘프", "드워프", "거인", "천사", "인어", "유니콘",
    "피닉스", "사슴", "말", "매", "호랑이", "곰", "흑표범",
    "까마귀", "올빼미", "공작", "백조", "여우", "사냥꾼",
    "성기사", "마법사", "도적", "사무라이", "닌자",
    "궁수", "레인저", "사제", "주술사", "드루이드",
    "스핑크스", "페가수스", "퓨마", "재규어", "황소", "뱀"
};

        static readonly List<string> places = new List<string> {
    "숲", "황무지", "폐허", "참나무", "산맥", "호수", "강", "바다",
    "성채", "탑", "던전", "동굴", "궁전", "사원", "신전",
    "정원", "마을", "도시", "왕국", "제국", "사막", "빙하",
    "화산", "계곡", "절벽", "요새", "항구", "섬", "반도",
    "평원", "늪지", "암초", "사바나", "툰드라", "지하세계", "소용돌이",
    "샘터", "숲속", "초원", "피오르드", "협곡", "고원", "늪",
    "달", "별", "은하", "성운", "운석", "혜성",
    "공허", "차원", "성소", "투기장", "대전장", "학당", "도서관", "기록관"
};

        static readonly List<string> titles = new List<string> {
    "군단", "여단", "대대", "연대", "사단", "중대",
    "비행단", "분대", "부대", "부서", "호위대", "병력", "병단",
    "집단", "무리", "연합", "기사단", "형제단", "부족", "가문", "왕국",
    "제국", "왕실", "동맹", "연맹", "연합체", "길드", "협회", "학회",
    "성단", "집회", "결사", "조합", "단체", "연대",
    "사교단", "결사단", "학교", "학당", "학원", "집단", "무인단"
};

        static readonly List<string> symbols = new List<string> {
    "백합", "탑", "창", "방패", "왕관", "검", "성", "별",
    "달", "태양", "혜성", "불꽃", "파도", "산", "나무", "숲",
    "강", "돌", "모루", "망치", "도끼", "활", "화살", "투구",
    "장갑", "갑옷", "사슬", "열쇠", "반지", "자물쇠", "책", "두루마리",
    "물약", "보석구", "왕좌", "깃발", "보석", "피라미드", "오벨리스크",
    "다리", "문", "벽", "성배", "등불", "촛불", "종", "깃털", "유리", "나침반"
};


        /// <summary>
        /// Returning static lists is important for performance
        /// </summary>
        public override List<string> NameGenerator_Army_Adjectives => adjectives;
        public override List<string> NameGenerator_Army_Colors => colors;
        public override List<string> NameGenerator_Army_Creatures => creatures;
        public override List<string> NameGenerator_Army_Places => places;
        public override List<string> NameGenerator_Army_Titles => titles;
        public override List<string> NameGenerator_Army_Symbols => symbols;

        /*
 * 도시 이름 생성기 (한국어 현지화)
 * 지역별 어감(북유럽풍, 서양풍, 동양풍, 남부/지중해풍)을 유지하면서
 * 한국어 사용자가 발음하기 쉽게 조정한 음절 세트입니다.
 */

        static readonly List<string> generalSyllables = new List<string>
{
    "아르", "벨", "카르", "둔", "엘", "펜", "글렌", "할", "이버", "준",
    "켈", "림", "몬", "노르", "오크", "펠", "퀜", "릴", "센", "탈",
    "운", "벨", "웰", "젠", "옐", "젤", "애쉬", "브로", "크레", "델",
    "에크", "페이", "길", "헤르", "이스크", "요르", "케이", "론", "마이어", "녹",
    "오르프", "펜", "퀼", "로스트", "사른", "틸", "우드", "번", "위스트", "얀", "조른"
};

        static readonly List<string> generalTownSuffixes = new List<string>
{ "타운", "포드", "버그", "빌", "스테드", "윅", "몬트", "필드", "포트", "데일" };

        static readonly List<string> northSyllables = new List<string>
{
    "피요르", "스콜", "바르그", "울프", "프로스트", "비욘", "스토르", "휘트", "야른", "스네",
    "크비스트", "룬드", "노르드", "올프", "필", "룬", "시그", "토르", "울프", "발드",
    "잉", "아에그", "브림", "드락", "에이르", "프레이", "김", "할브", "이바르", "요",
    "켈드", "륑", "마그", "나트", "오딘", "프리드", "쿠오르", "로스트", "시프", "티욘",
    "울프르", "비드", "윈드", "실", "이를", "조른", "에스크", "브록", "달", "엥"
};

        static readonly List<string> northTownSuffixes = new List<string>
{ "비크", "스타드", "피요르드", "베르그", "네스", "달", "하임", "가르드", "하브른", "란드", "울" };

        static readonly List<string> westSyllables = new List<string>
{
    "윈", "란", "햄", "포드", "스터", "버그", "셔", "웰", "톤", "윅",
    "바드", "클리프", "델", "에스", "그라프", "홀트", "아이르", "제스트", "켄트", "리",
    "무어", "노르", "옥스", "페리", "퀜", "리프트", "스워드", "트레", "울름", "버",
    "워", "예이트", "질", "아드", "빔", "코브", "데일", "에프트", "게일", "히스",
    "잉글", "킬", "리스", "마시", "니스", "오프", "페일", "퀼", "로브", "스케일", "대치"
};

        static readonly List<string> westTownSuffixes = new List<string>
{ "톤", "버그", "포드", "햄", "셔", "캐스터", "윅", "베리", "스테드", "빌" };

        static readonly List<string> eastSyllables = new List<string>
{
    "진", "시", "위안", "칭", "루", "춘", "밍", "난", "핑", "저우",
    "바이", "둥", "푸", "궈", "후이", "카이", "란", "메이", "니", "오우",
    "페이", "친", "란", "수", "타이", "웨이", "시", "양", "주", "안",
    "보", "츠", "다", "엔", "페이", "강", "하오", "지", "켄", "레이",
    "모", "닝", "포", "치", "로우", "센", "팅", "완", "싱", "유", "젠"
};

        static readonly List<string> eastTownSuffixes = new List<string>
{ "양", "산", "지", "안", "하이", "청", "린", "타이", "코우", "푸" };

        static readonly List<string> southSyllables = new List<string>
{
    "테", "네", "리", "카", "레", "시", "마르", "폴", "아트", "코르",
    "델", "에프", "가", "헬", "이오", "코스", "라", "메", "니", "올",
    "파", "로", "세", "티", "우르", "베", "잔", "이라", "제", "알",
    "브라", "시", "드라", "에로", "피", "그레", "히", "일", "조", "클레",
    "레우", "미", "노스", "오페", "피", "퀘", "라", "시르", "타", "비르", "윈"
};

        static readonly List<string> southTownSuffixes = new List<string>
{ "폴리스", "이움", "오스", "우스", "아", "온", "오라", "아카", "에스", "아이" };


        /// <summary>
        /// Returning static lists is important for performance
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
