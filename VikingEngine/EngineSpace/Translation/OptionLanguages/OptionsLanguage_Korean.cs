using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Korean : AbsOptionsLanguage
    {
        public override string GameSettings_WideScrollbar => "넓은 스크롤 바";
        public override string GameSettings_DisplayInputHelp => "조작 도움말";
        public override string GameSettings_InputSmoothing => "입력 스무딩";

        //Mounts
        public override string InputSteam => "Steam 입력";
        public override string Input_SimulateMouse => "마우스 시뮬레이션";
        public override string Input_LockMouseToWindow => "마우스를 창에 가두기";
        public override string Input_MouseEdgePush_Title => "가장자리 스크롤";
        public override string Input_NoControl => "없음";
        public override string Input_ActiveControl => "액티브";
        public override string Input_PassiveControl => "패시브";
        public override string Setting_MinimapScale => "미니맵 배율";
        //##Settings
        public override string Settings_Particles_FadeMapLayers => "레이어 페이드"; // "Layer Fade"
        public override string SplitScreen_HorizontalFirst => "가로 우선";
        public override string SplitScreen_VerticalFirst => "세로 우선";
        public override string SplitScreen_HorizontalOnly => "가로 전용";
        public override string SplitScreen_VerticalOnly => "세로 전용";
        public override string SplitScreen_Title => "화면 분할";
        public override string SplitScreen_AdjustSplit => "분할 조정 {0}";

        public override string Settings_ControllerVibration => "컨트롤러 진동";
        public override string GraphicsOption_IngameMenuWidth => "인게임 메뉴 너비";
        public override string DisplayMode => "디스플레이 모드";
        public override string DisplayMode_Windowed => "창 모드";
        public override string DisplayMode_BorderlessFullscreen => "테두리 없는 전체 화면";
        public override string GameSettings_RenderedMouseCursor => "렌더된 커서";
        public override string GameSettings_MuteControllerDisconnect => "컨트롤러 연결 해제 알림 끄기";
        //--
        public override string GraphicsOption_FarViewDistance => "원거리 시야";

        public override string Hud_Cancel => "취소";
        public override string Hud_Back => "뒤로 가기";

        /// <summary>
        /// 파괴적인 행동을 선택할 때 표시되는 확인 서브메뉴
        /// </summary>
        public override string Hud_AreYouSure => "정말로 진행하시겠습니까?";

        public override string Hud_OK => "확인";
        public override string Hud_Yes => "예";
        public override string Hud_No => "아니오";

        /// <summary>
        /// 옵션 메뉴 제목
        /// </summary>
        public override string Options_title => "옵션";

        /// <summary>
        /// 게임 조작 입력 설정, 0: 현재 입력 장치
        /// </summary>
        public override string InputSelect => "입력 장치: {0}";

        /// <summary>
        /// 키보드와 마우스 입력
        /// </summary>
        public override string InputKeyboardMouse => "키보드 & 마우스";

        /// <summary>
        /// 게임 패드 입력
        /// </summary>
        public override string InputController => "컨트롤러";

        /// <summary>
        /// 입력 장치가 선택되지 않았을 때
        /// </summary>
        public override string InputNotSet => "설정되지 않음";

        /// <summary>
        /// 로컬 분할 화면 옵션 (세로 분할)
        /// </summary>
        public override string VerticalSplitScreen => "세로 화면 분할";

        /// <summary>
        /// 음악 볼륨 슬라이더
        /// </summary>
        public override string SoundOption_MusicVolume => "음악 볼륨";

        /// <summary>
        /// 효과음 볼륨 슬라이더
        /// </summary>
        public override string SoundOption_SoundVolume => "효과음 볼륨";

        /// <summary>
        /// 화면 해상도 설정
        /// </summary>
        public override string GraphicsOption_Resolution => "해상도";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// 전체 화면 또는 창 모드 전환
        /// </summary>
        public override string GraphicsOption_Fullscreen => "전체 화면";

        /// <summary>
        /// 여러 모니터 지원용 초과 크기 설정
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "가로 초과 크기";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% 가로";
        public override string GraphicsOption_OversizeHeight => "세로 초과 크기";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% 세로";
        public override string GraphicsOption_Oversize_None => "없음";

        /// <summary>
        /// 유튜브 녹화용 해상도 프리셋
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "녹화 프리셋";

        /// <summary>
        /// 0: 높이 해상도
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "유튜브 {0}p";

        /// <summary>
        /// UI 텍스트 및 아이콘 크기 변경
        /// </summary>
        public override string GraphicsOption_UiScale => "UI 크기 비율";



        //---
        public override string ReversedStereo => "좌우 음향 반전";
        public override string Option_Low => "낮음";
        public override string Option_Medium => "보통";
        public override string Option_High => "높음";

        public override string MouseSettings_Title => "마우스 입력";
        public override string KeyboardSettings_Title => "키 설정";

        public override string MouseButtonAction_None => "동작 없음";
        public override string MouseButtonAction_Select => "선택";
        public override string MouseButtonAction_Pan => "화면 이동";
        public override string MouseButtonAction_PanAndOrder => "이동 및 명령";
        public override string MouseButtonAction_Order => "명령";
        public override string MouseButtonAction_Cancel => "취소";

        public override string MouseButton_Left => "왼쪽 버튼";
        public override string MouseButton_Right => "오른쪽 버튼";
        public override string MouseButton_Middle => "가운데 버튼";
        public override string MouseButton_X1 => "X1 버튼";
        public override string MouseButton_X2 => "X2 버튼";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "이동 및 취소";
        public override string MouseButtonAction_PanAndOrderAndCancel => "이동, 명령 및 취소";

        public override string GraphicsOption_Shadows => "그림자";
        public override string GraphicsOption_ShadowType_ModelsToGround => "모델 → 지면 그림자";
        public override string GraphicsOption_ShadowType_ModelsToModels => "모델 → 모델 그림자";

        public override string GraphicsOption_Shadow_MapResolution => "그림자 맵 해상도";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "{0}픽셀 추가";
        public override string Settings_KeyMapPanSpeed => "화면 이동 속도";
        public override string Settings_StoreCameraPosition => "카메라 위치 저장";
        public override string Settings_LoadCameraPosition => "저장된 위치 불러오기";

        //Shadow update
        public override string Settings_ModelWaterFoam => "물 거품 효과";
        public override string Settings_ModelShadow => "모델 그림자";
        public override string Settings_ModelShadowMapSize => "그림자 맵 크기";
        public override string Settings_Brightness => "밝기";
        public override string Settings_Mode_No_Achivements => "도전 과제를 사용할 수 없습니다.";
        public override string Settings_FrameRate => "프레임 속도";

        /// <summary>
        /// Steam 도전 과제
        /// </summary>
        public override string Settings_ImportNoAchievement => "가져온 세이브 파일의 도전 과제 차단";


    }
}
