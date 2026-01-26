using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Thai : AbsLanguage
    {
        public override string Help_Work_Automatic => TextLib.ThaiConv("การ|ทำ|งาน|เป็น|ไป|โดย|อัตโนมัติ");
        public override string Tutorial_SecondCity => TextLib.ThaiConv("ครอบ|ครอง|เมือง|ที่|สอง");
        public override string InputAction_SkipAutomated => TextLib.ThaiConv("ข้าม|อัตโนมัติ");

        public override string Resource_WaterReason => TextLib.ThaiConv("น้ำ|จะ|จำกัด|จำนวน|ยูนิต|ที่|คุณ|รอง|รับ|ได้|และ|จำกัด|ขนาด|การ|ผลิต|ของ|คุณ");
        public override string BuildingType_Orchard => TextLib.ThaiConv("สวน|ผลไม้");
        public override string BuildingType_ManorLord => TextLib.ThaiConv("คฤหาสน์|เจ้า|เมือง");
        public override string BuildingType_ManorLord_Description => TextLib.ThaiConv("ปลด|ล็อก|การ|แปรรูป|อาหาร");
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public override string Diplomacy_EndRelations => TextLib.ThaiConv("ยุติ|ความ|สัมพันธ์");

        /// <summary>
        /// Where a resource is produced or found
        /// </summary>
        public override string ItemSource => TextLib.ThaiConv("แหล่ง|ที่|มา|ไอเทม");

        public override string ItemSource_Terrain => TextLib.ThaiConv("ภูมิประเทศ");
        public override string ItemSource_Farm => TextLib.ThaiConv("ฟาร์ม");
        public override string ItemSource_CraftStation => TextLib.ThaiConv("โรง|คราฟต์");
        public override string ItemSource_Gathering => TextLib.ThaiConv("การ|เก็บ|เกี่ยว");

        public override string CityCulture_Nomad => TextLib.ThaiConv("ชน|เผ่า|เร่|ร่อน");

        /// <summary>
        /// A generalized display of buffs and boons, example "+100%" or "Doubled"
        /// </summary>
        public override string Hud_ChangeFactor => TextLib.ThaiConv("ตาม|อัตรา|การ|เปลี่ยน|แปลง: {0}");

        public override string Hud_Purchase_LowXCost => TextLib.ThaiConv("ค่า| {0} |ต่ำ");

        public override string WorkQueue_Title => TextLib.ThaiConv("คิว|งาน");
        public override string WorkQueue_Length => TextLib.ThaiConv("งาน|ที่|เหลือ|อยู่");
        public override string WorkQueue_ActiveWorkers => TextLib.ThaiConv("ทีม|งาน|ที่|กำลัง|ทำ|งาน");
        public override string WorkQueue_IdleWorkers => TextLib.ThaiConv("ทีม|งาน|ที่|ว่าง|งาน");

        public override string WorkTeam_Size => TextLib.ThaiConv("ชาว|บ้าน|ทำ|งาน|เป็น|ทีม|ละ| {0} |คน");

        public override string ObjectUi_ViewOnMap => TextLib.ThaiConv("ดู|บน|แผนที่");
        public override string ObjectUi_StuckBuildOrders => TextLib.ThaiConv("การ|ก่อ|สร้าง|ที่|ติด|ขัด");
        public override string Hud_AllArmies => TextLib.ThaiConv("กอง|ทัพ|ทั้งหมด");

        public override string Hud_CurrentPage => TextLib.ThaiConv("หน้า|ปัจจุบัน");
        public override string Hud_AllPages => TextLib.ThaiConv("หน้า|ทั้งหมด");
        public override string Hud_ToAllCities => TextLib.ThaiConv("ถึง|ทุก|เมือง");
        public override string Hud_ToFaction => TextLib.ThaiConv("ให้|กับ|ฝ่าย");
        public override string Hud_FromFaction => TextLib.ThaiConv("จาก|ฝ่าย");
        public override string Hud_FactionWide => TextLib.ThaiConv("ใช้|การ|ตั้ง|ค่า|แบบ|ทั้ง|ฝ่าย");
        /// <summary>
        /// This start a new city
        /// </summary>
        public override string Action_PlaceSettlement => TextLib.ThaiConv("ตั้ง|รก|ราก");

        public override string Editor_Animation_RemoveAllFramesButThis => TextLib.ThaiConv("ลบ|เฟรม|อื่น|ออก|ทั้งหมด");


        //Winter patch 3
        public override string Hud_Purchase_AllBuildings => TextLib.ThaiConv("ใส่|คิว|สิ่ง|ก่อ|สร้าง|ทั้งหมด");
        public override string Hud_Purchase_AllTech => TextLib.ThaiConv("ใส่|คิว|เทคโนโลยี|ทั้งหมด");
        public override string BuildingType_CasualBarracks_Description => TextLib.ThaiConv("เวลา|ใน|การ|ฝึก|ทหาร|จะ|ถูก|แบ่ง|กระจาย|ไป|ตาม|โรง|ทหาร|ต่างๆ");

        //Winter update patch + spring

        /// <summary>
        /// How much of a resource that will be used, e.g. "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public override string Hud_Purchase_ResourceCost => TextLib.ThaiConv("{1} {0}");

        public override string DisplayMode => TextLib.ThaiConv("โหมด|แสดง|ผล");
        public override string DisplayMode_Windowed => TextLib.ThaiConv("แบบ|หน้าต่าง");
        public override string DisplayMode_BorderlessFullscreen => TextLib.ThaiConv("เต็ม|จอ|ไร้|ขอบ");

        public override string GameSettings_RenderedMouseCursor => TextLib.ThaiConv("เรนเดอร์|เคอร์เซอร์");
        public override string GameSettings_MuteControllerDisconnect => TextLib.ThaiConv("ปิด|การ|แจ้ง|เตือน|คอนโทรลเลอร์|หลุด");

        public override string Delivery_MaxDistance => TextLib.ThaiConv("ระยะ|ขน|ส่ง|สูงสุด: {0}");
        public override string Tutorial_WillTakeAWhile => TextLib.ThaiConv("ขั้น|ตอนนี้|ใช้|เวลา|สัก|พัก |ลอง|กลับ|มา|ดู|ใหม่|ภาย|หลัง");

        /// <summary>
        /// 0: name of building
        /// </summary>
        public override string Tutorial_WaitFor => TextLib.ThaiConv("รอ|ให้| {0} |เสร็จ|สมบูรณ์");
        public override string GameOverResults => TextLib.ThaiConv("บันทึก|ประวัติ|การ|เล่น");

        public override string UnitType_UnclaimedLand => TextLib.ThaiConv("ดิน|แดน|ไร้|เจ้า|ของ");
        public override string UnitType_Settler => TextLib.ThaiConv("ผู้|บุก|เบิก");
        public override string UnitType_Settler_Description => TextLib.ThaiConv("สร้าง|เมือง|ใหม่");
        public override string Resource_ConsumedProduced => TextLib.ThaiConv("ใช้|ไป/ผลิต|ได้");
        public override string InputActionName_PlaceTarget => TextLib.ThaiConv("วาง|เป้า|หมาย");

        public override string FactionStartSize => TextLib.ThaiConv("ขนาด|เริ่มต้น|ของ|ฝ่าย");
        public override string FactionStartSize_Full => TextLib.ThaiConv("เต็ม|อัตรา");
        public override string FactionStartSize_OneCity => TextLib.ThaiConv("หนึ่ง|เมือง");
        public override string FactionStartSize_Settler => TextLib.ThaiConv("ผู้|บุก|เบิก|หนึ่ง|คน");

        //Winter update
        public override string Resource_StockpileLimit => TextLib.ThaiConv("ขีด|จำกัด|คลัง|เก็บ|ของ");
        public override string GameMode_QuickMatch => TextLib.ThaiConv("ควิก|แมตช์");
        public override string GameMode_QuickMatch_Description => TextLib.ThaiConv("รูปแบบ|เกม|ที่|สั้น|ลง |เข้า|ร่วม|สงคราม|เต็ม|รูปแบบ|กับ|อาณาจักร|คู่|แข่ง");
        public override string Lobby_PlayerCount => TextLib.ThaiConv("จำนวน|ผู้|เล่น");
        public override string Lobby_TwoTeams => TextLib.ThaiConv("สอง|ทีม");
        public override string Hud_Produce => TextLib.ThaiConv("ผลิต:");
        public override string Tutorial_WaitForWorkerLevel => TextLib.ThaiConv("รอ|ให้|คน|งาน|ถึง|ระดับ:");

        /// <summary>
        /// 0: Production item, 1: School
        /// </summary>
        public override string Tutorial_PracticeOrSchool => TextLib.ThaiConv("ฝึก|ฝน|ที่| {0} |หรือ|ใช้| {1}");
        public override string Tutorial_AddTag => TextLib.ThaiConv("เพิ่ม|แท็ก:");
        public override string Tutorial_AddPin => TextLib.ThaiConv("ปัก|หมุด:");
        public override string Tutorial_SelectMostTrees => TextLib.ThaiConv("ค้นหา|เมือง|ของ|คุณ|ที่|มี|ต้น|ไม้|มาก|ที่สุด");
        public override string Tutorial_SelectACityWithX => TextLib.ThaiConv("เลือก|เมือง|ที่|มี| {0}");

        /// <summary>
        /// Will continue on another sentence "Select a city"
        /// </summary>
        public override string Tutorial_Select_NotCapital => TextLib.ThaiConv(". ไม่|ใช่|เมือง|หลวง|ของ|คุณ");

        public override string Tutorial_SetXPriorityToY => TextLib.ThaiConv("ตั้ง|ค่า|ความ|สำคัญ| {0} |เป็น| {1}");
        public override string Tutorial_AdvisorMission => TextLib.ThaiConv("ภารกิจ|ที่|ปรึกษา");

        public override string Tutorial_AdvisorDescription => TextLib.ThaiConv("เกม|ตัว|เต็ม|เริ่ม|แล้ว |ที่|ปรึกษา|จะ|คอย|ให้|คำ|แนะนำ|เพิ่มเติม|ผ่าน|ภารกิจ|ต่างๆ");

        public override string Tutorial_EndAdvisor => TextLib.ThaiConv("จบ|การ|แนะนำ");


        public override string Tutorial_AdvisorCompleteTitle => TextLib.ThaiConv("ที่|ปรึกษา|ทำ|หน้าที่|เสร็จ|สิ้น!");
        public override string Tutorial_AdvisorCompleteMessage => TextLib.ThaiConv("ขอ|ให้|วัน|รุ่ง|ขึ้น|ของ|ท่าน|จง|รุ่ง|โรจน์!");

        public override string Hud_Search => TextLib.ThaiConv("ค้นหา");



        public override string DifficultyDescription_ExtremeAggression => TextLib.ThaiConv("ดุดัน|ขั้น|สุด");

        public override string MapFilter => TextLib.ThaiConv("ตัว|กรอง|แผนที่");

        public override string Settings_TechMultiplier => TextLib.ThaiConv("ความ|เร็ว|วิจัย|เทคโนโลยี");

        public override string EndScreen_MatchComplete => TextLib.ThaiConv("ผล|การ|แข่ง|ขัน");

        /// <summary>
        /// Theme: Four headed dragon symbol. Known for having an unpenetrable castle.
        /// </summary>
        public override string FactionName_DragonGem => TextLib.ThaiConv("มณี|มังกร");

        /// <summary>
        /// Theme: Easter egg for december. "Tomten" is an old nordic name for father christmas
        /// </summary>
        public override string FactionName_Tomten => TextLib.ThaiConv("ทอมเทน");

        /// <summary>
        /// Theme: The blessed folk. A horde like farmers faction.
        /// </summary>
        public override string FactionName_Hælfolc => TextLib.ThaiConv("เฮล|โฟล์ก");

        /// <summary>
        /// The Iron Saints, people who guard a mountain pass against evil.
        /// </summary>
        public override string FactionName_AerimAngren => TextLib.ThaiConv("เอริม|อังเกรน");

        public override string HUD_NotAvailbleInX => TextLib.ThaiConv("ไม่|พร้อม|ใช้|งาน|ใน| {0}");

        public override string InputActionName_MiniMap => TextLib.ThaiConv("มินิ|แมตป์");

        //--
        public override string Error_SoundInitFailure => TextLib.ThaiConv("การ|ตั้ง|ค่า|เสียง|ล้ม|เหลว");

        public override string GameMenu_ControllerDisconnected => TextLib.ThaiConv("คอนโทรลเลอร์|หลุด");

        public override string Tutorial_HighPriority => TextLib.ThaiConv("คน|ของ|คุณ|จะ|ทำ|งาน|ที่|มี|ความ|สำคัญ|สูง|ก่อน");

        public override string BuildingType_Wall_Description => TextLib.ThaiConv("กำแพง|ช่วย|ป้องกัน|การ|โจมตี|และ|เพิ่ม|โบนัส|การ|โจมตี|เล็ก|น้อย");

        public override string BuildingType_Wall_Siege => TextLib.ThaiConv("เครื่อง|ล้อม|เมือง|จะ|ลด|การ|ป้องกัน|ของ|กำแพง");

        public override string Conscript_BlockChance => TextLib.ThaiConv("มี|โอกาส| {0}% |ที่|จะ|บล็อก|การ|โจมตี");

        public override string Battle_DeclarWarReminder => TextLib.ThaiConv("คุณ|ต้อง|ประกาศ|สงคราม|ก่อน|จะ|โจมตี|ได้");

        //--

        /// <summary>
        /// Name of this language
        /// </summary>
        public override string MyLanguage => TextLib.ThaiConv("ภาษา|ไทย");

        /// <summary>
        /// How to display a number of items. 0: item, 1:Number
        /// </summary>
        public override string Language_ItemCountPresentation => TextLib.ThaiConv("{0}: {1}");

        /// <summary>
        /// Select language option
        /// </summary>
        public override string Lobby_Language => TextLib.ThaiConv("ภาษา");

        /// <summary>
        /// Start playing the game
        /// </summary>
        public override string Lobby_Start => TextLib.ThaiConv("เริ่ม|เกม");

        /// <summary>
        /// Button to select local mutiplayer count, 0:current player count
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => TextLib.ThaiConv("มัลติ|เพลเยอร์|ใน|เครื่อง");

        /// <summary>
        /// Title for menu where you select split screen player count
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => TextLib.ThaiConv("เลือก|จำนวน|ผู้|เล่น");

        /// <summary>
        /// Description for local multiplayer
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => TextLib.ThaiConv("มัลติ|เพลเยอร์|ต้อง|ใช้|คอนโทรลเลอร์| Xbox");

        /// <summary>
        /// Move to next split screen position
        /// </summary>
        public override string Lobby_NextScreen => TextLib.ThaiConv("ตำแหน่ง|หน้าจอ|ถัด|ไป");

        /// <summary>
        /// Players can select visual appearance and store them in a profile
        /// </summary>
        public override string Lobby_FlagSelectTitle => TextLib.ThaiConv("เลือก|ธง");

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_FlagNumbered => TextLib.ThaiConv("ธง| {0}");

        /// <summary>
        /// Game name and version number
        /// </summary>
        //public override string Lobby_GameVersion => TextLib.ThaiConv("DSS war party - ver {0}");

        public override string FlagEditor_Description => TextLib.ThaiConv("ระบาย|สี|ธง|และ|เลือก|สี|สำหรับ|กอง|ทัพ|ของ|คุณ");

        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string FlagEditor_Bucket => TextLib.ThaiConv("ถัง|สี");

        /// <summary>
        /// Opens flag profile editor
        /// </summary>
        public override string Lobby_FlagEdit => TextLib.ThaiConv("แก้ไข|ธง");


        public override string Lobby_WarningTitle => TextLib.ThaiConv("คำ|เตือน");
        public override string Lobby_IgnoreWarning => TextLib.ThaiConv("ละ|เว้น|คำ|เตือน");

        /// <summary>
        /// Warning when one player has no input selected.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => TextLib.ThaiConv("มี|ผู้|เล่น|หนึ่ง|คน|ยัง|ไม่|ได้|เลือก|อินพุต");

        /// <summary>
        /// Menu with content that are outside what most players will use.
        /// </summary>
        public override string Lobby_Extra => TextLib.ThaiConv("พิเศษ");

        /// <summary>
        /// The extra content is not translated or have full controller support.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => TextLib.ThaiConv("คำ|เตือน! |เนื้อหา|นี้|ไม่|รองรับ|การ|แปล|ภาษา|หรือ|การ|ใช้|งาน|คอนโทรลเลอร์|อย่าง|เต็ม|รูปแบบ");


        public override string Lobby_MapSizeTitle => TextLib.ThaiConv("ขนาด|แผนที่");

        /// <summary>
        /// Map size 1 name
        /// </summary>
        public override string Lobby_MapSizeOptTiny => TextLib.ThaiConv("จิ๋ว");

        /// <summary>
        /// Map size 2 name
        /// </summary>
        public override string Lobby_MapSizeOptSmall => TextLib.ThaiConv("เล็ก");

        /// <summary>
        /// Map size 3 name
        /// </summary>
        public override string Lobby_MapSizeOptMedium => TextLib.ThaiConv("กลาง");

        /// <summary>
        /// Map size 4 name
        /// </summary>
        public override string Lobby_MapSizeOptLarge => TextLib.ThaiConv("ใหญ่");

        /// <summary>
        /// Map size 5 name
        /// </summary>
        public override string Lobby_MapSizeOptHuge => TextLib.ThaiConv("มหึมา");

        /// <summary>
        /// Map size 6 name
        /// </summary>
        public override string Lobby_MapSizeOptEpic => TextLib.ThaiConv("มหากาพย์");

        /// <summary>
        /// Map size description X by Y kilometers. 0: Width, 1: Height
        /// </summary>
        public override string Lobby_MapSizeDesc => TextLib.ThaiConv("{0}x{1} | กม.");
        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => TextLib.ThaiConv("ออก|จาก|เกม");

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => TextLib.ThaiConv("ผู้|เล่น| {0}");

        /// <summary>
        /// In player profile editor. Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => TextLib.ThaiConv("ตัว|เลือก");

        /// <summary>
        /// In player profile editor. Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => TextLib.ThaiConv("สี|ธง");

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => TextLib.ThaiConv("สี|หลัก");

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => TextLib.ThaiConv("สี|ราย|ละเอียด| 1");

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => TextLib.ThaiConv("สี|ราย|ละเอียด| 2");

        /// <summary>
        /// In player profile editor. Title for selecting you soldiers colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => TextLib.ThaiConv("ผู้|คน");

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => TextLib.ThaiConv("สี|ผิว");

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => TextLib.ThaiConv("สี|ผม");

        /// <summary>
        /// In player profile editor. Open color palette and select color
        /// </summary>
        public override string ProfileEditor_PickColor => TextLib.ThaiConv("เลือก|สี");

        /// <summary>
        /// In player profile editor. Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => TextLib.ThaiConv("ย้าย|รูป|ภาพ");

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => TextLib.ThaiConv("ซ้าย");

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => TextLib.ThaiConv("ขวา");

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => TextLib.ThaiConv("บน");

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => TextLib.ThaiConv("ล่าง");

        /// <summary>
        /// In player profile editor. Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => TextLib.ThaiConv("ยกเลิก|และ|ออก");

        /// <summary>
        /// In player profile editor. Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => TextLib.ThaiConv("ย้อน|การ|ตั้ง|ค่า|ทั้งหมด");

        /// <summary>
        /// In player profile editor. Save changes and close editor
        /// </summary>
        public override string Hud_SaveAndExit => TextLib.ThaiConv("บันทึก|และ|ออก");

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Hue => TextLib.ThaiConv("เฉด|สี");

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Lightness => TextLib.ThaiConv("ความ|สว่าง");

        /// <summary>
        /// In player profile editor. Move between flag and soldier color options.
        /// </summary>
        public override string ProfileEditor_NextColorType => TextLib.ThaiConv("ประเภท|สี|ถัด|ไป");

        /// <summary>
        /// Current running speed of the game, compared to real time
        /// </summary>
        public override string Hud_GameSpeedLabel => TextLib.ThaiConv("ความ|เร็ว|เกม: {0}x");

        public override string Input_GameSpeed => TextLib.ThaiConv("ความ|เร็ว|เกม");

        /// <summary>
        /// Ingame display. Unit gold production
        /// </summary>
        public override string Hud_TotalIncome => TextLib.ThaiConv("ราย|ได้|ทั้งหมด/วินาที: {0}");

        /// <summary>
        /// Unit gold cost.
        /// </summary>
        public override string Hud_Upkeep => TextLib.ThaiConv("ค่า|บำรุง|รักษา: {0}");
        public override string Hud_ArmyUpkeep => TextLib.ThaiConv("ค่า|บำรุง|กอง|ทัพ: {0}");

        /// <summary>
        /// Ingame display. Soldiers protecting a building.
        /// </summary>
        public override string Hud_GuardCount => TextLib.ThaiConv("ทหาร|ยาม");

        public override string Hud_IncreaseMaxGuardCount => TextLib.ThaiConv("จำนวน|ยาม|สูงสุด| {0}");

        public override string Hud_GuardCount_MustExpandCityMessage => TextLib.ThaiConv("คุณ|ต้อง|ขยาย|เมือง|ก่อน");

        public override string Hud_SoldierCount => TextLib.ThaiConv("จำนวน|ทหาร: {0}");

        public override string Hud_SoldierGroupsCount => TextLib.ThaiConv("จำนวน|กลุ่ม: {0}");

        /// <summary>
        /// Ingame display. Unit caculated battle strength.
        /// </summary>
        public override string Hud_StrengthRating => TextLib.ThaiConv("ระดับ|พลัง: {0}");

        /// <summary>
        /// Ingame display. Caculated battle strength for the whole nation.
        /// </summary>
        public override string Hud_TotalStrengthRating => TextLib.ThaiConv("ความ|แข็ง|แกร่ง|ทางการ|ทหาร: {0}");

        /// <summary>
        /// Ingame display. Extra men coming from outside the city state.
        /// </summary>
        public override string Hud_Immigrants => TextLib.ThaiConv("ผู้|อพยพ");


        public override string Hud_CityCount => TextLib.ThaiConv("จำนวน|เมือง: {0}");
        public override string Hud_ArmyCount => TextLib.ThaiConv("จำนวน|กอง|ทัพ: {0}");


        /// <summary>
        /// Mini button to repeat a purchase a number of times. E.G. "x5"
        /// </summary>
        public override string Hud_XTimes => TextLib.ThaiConv("x{0}");

        public override string Hud_PurchaseTitle_Requirement => TextLib.ThaiConv("เงื่อนไข");
        public override string Hud_PurchaseTitle_Cost => TextLib.ThaiConv("ราคา");
        public override string Hud_PurchaseTitle_Gain => TextLib.ThaiConv("สิ่ง|ที่|ได้|รับ");

        /// <summary>
        /// How much of a resource that will be used, "5 gold. (Available: 10)". There will be a "cost" title above the text. 0: Resource, 1: cost, 2: available
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => TextLib.ThaiConv("{1} {0}. (มี|อยู่: {2})");

        public override string Hud_Purchase_CostWillIncreaseByX => TextLib.ThaiConv("ราคา|จะ|เพิ่ม|ขึ้น| {0}");

        public override string Hud_Purchase_MaxCapacity => TextLib.ThaiConv("จำนวน|เต็ม|พิกัด|แล้ว");

        public override string Hud_CompareMilitaryStrength_YourToOther => TextLib.ThaiConv("พลัง: ของ|คุณ| {0} - ของ|ศัตรู| {1}");

        /// <summary>
        /// Display a short string of date as Year, Month, Day
        /// </summary>
        public override string Hud_Date => TextLib.ThaiConv("ปี {0} เดือน {1} วัน {2}");

        /// <summary>
        /// Display a short string of timespan as Hour, Minutes, Seconds
        /// </summary>
        public override string Hud_TimeSpan => TextLib.ThaiConv("{0} ชม. {1} น. {2} วิ.");

        /// <summary>
        /// Battle between two armies, or army and city
        /// </summary>
        public override string Hud_Battle => TextLib.ThaiConv("การ|รบ");

        /// <summary>
        /// Describes button input. Pause.
        /// </summary>
        public override string Input_Pause => TextLib.ThaiConv("พัก|เกม");

        /// <summary>
        /// Describes button input. Resume from paused.
        /// </summary>
        public override string Input_ResumePaused => TextLib.ThaiConv("เล่น|ต่อ");

        /// <summary>
        /// Generic money resource
        /// </summary>
        public override string ResourceType_Gold => TextLib.ThaiConv("ทอง");

        /// <summary>
        /// Working men resource
        /// </summary>
        public override string ResourceType_Workers => TextLib.ThaiConv("คน|งาน");


        public override string ResourceType_Workers_Description => TextLib.ThaiConv("คน|งาน|ช่วย|สร้าง|ราย|ได้ |และ|จะ|ถูก|เกณฑ์|ไป|เป็น|ทหาร|ใน|กอง|ทัพ|ของ|คุณ");

        /// <summary>
        /// The resource used in diplomacy
        /// </summary>
        public override string ResourceType_DiplomacyPoints => TextLib.ThaiConv("แต้ม|การ|ทูต");

        /// <summary>
        /// 0: How many points you got, 1: Soft max value (will increase much slower after this), 2: Hard limit
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => TextLib.ThaiConv("แต้ม|การ|ทูต: {0} / {1} ({2})");

        /// <summary>
        /// City building type. Building for knights and diplomats.
        /// </summary>
        public override string Building_NobleHouse => TextLib.ThaiConv("บ้าน|ขุน|นาง");

        public override string Building_NobleHouse_DiplomacyPointsAdd => TextLib.ThaiConv("รับ| 1 |แต้ม|การ|ทูต|ทุก| {0} |วินาที");
        public override string Building_NobleHouse_DiplomacyPointsLimit => TextLib.ThaiConv("+{0} |ขีด|จำกัด|แต้ม|การ|ทูต|สูงสุด");
        public override string Building_NobleHouse_UnlocksKnight => TextLib.ThaiConv("ปลด|ล็อก|ยูนิต|อัศวิน");

        public override string Building_BuildAction => TextLib.ThaiConv("สร้าง");
        public override string Building_IsBuilt => TextLib.ThaiConv("สร้าง|แล้ว");

        /// <summary>
        /// City building type. Evil mass production.
        /// </summary>
        public override string Building_DarkFactory => TextLib.ThaiConv("โรง|งาน|ทมิฬ");

        /// <summary>
        /// In game settings menu. Sums all difficulty options in percentage.
        /// </summary>
        public override string Settings_TotalDifficulty => TextLib.ThaiConv("ความ|ยาก|โดย|รวม {0}%");

        /// <summary>
        /// In game settings menu. Base difficulty option.
        /// </summary>
        public override string Settings_DifficultyLevel => TextLib.ThaiConv("ระดับ|ความ|ยาก {0}%");


        /// <summary>
        ///  In game settings menu. Option for creating new maps instead of loading one. You can load pre-generated maps or create new ones.
        /// </summary>
        public override string Settings_GenerateMaps => TextLib.ThaiConv("สร้าง|แผนที่|ใหม่");

        /// <summary>
        ///  In game settings menu.Creating new maps has a longer loading time
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => TextLib.ThaiConv("การ|สร้าง|จะ|ช้า|กว่า|การ|โหลด|แผนที่|ที่|มี|อยู่|แล้ว");

        /// <summary>
        ///  In game settings menu.Difficulty option. Block the ability to play the game while paused.
        /// </summary>
        public override string Settings_AllowPause => TextLib.ThaiConv("อนุญาต|ให้|พัก|เกม|และ|สั่ง|การ");

        /// <summary>
        ///  In game settings menu.Difficulty option. Have bosses that enter the game.
        /// </summary>
        public override string Settings_BossEvents => TextLib.ThaiConv("อีเวนต์|บอส");

        /// <summary>
        ///  In game settings menu.Difficulty option. No Boss description.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => TextLib.ThaiConv("หาก|ปิด|อีเวนต์|บอส |เกม|จะ|เข้า|สู่|โหมด|แซนด์|บ็อกซ์|ที่|ไม่|มี|วัน|จบ");


        /// <summary>
        /// Options for automating game mechanics. Menu title.
        /// </summary>
        public override string Automation_Title => TextLib.ThaiConv("ระบบ|อัตโนมัติ");
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => TextLib.ThaiConv("จะ|รอ|จน|กว่า|แรง|งาน|จะ|เต็ม|พิกัด");
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => TextLib.ThaiConv("จะ|หยุด|ชั่ว|คราว|หาก|ราย|ได้|ติด|ลบ");
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_Priority => TextLib.ThaiConv("ให้|ความ|สำคัญ|กับ|เมือง|ใหญ่|ก่อน");
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => TextLib.ThaiConv("ดำเนิน|การ|ซื้อ|สูงสุด|หนึ่ง|ครั้ง|ต่อ|วินาที");


        /// <summary>
        /// Button caption for action. A specialized building for knights and diplomats.
        /// </summary>
        public override string HudAction_BuyItem => TextLib.ThaiConv("ซื้อ| {0}");

        /// <summary>
        /// The state of peace or war between two nations
        /// </summary>
        public override string Diplomacy_RelationType => TextLib.ThaiConv("ความ|สัมพันธ์");

        /// <summary>
        /// Titel for list of relations other factions have with eachother
        /// </summary>
        public override string Diplomacy_RelationToOthers => TextLib.ThaiConv("ความ|สัมพันธ์|ของ|พวกเขา|กับ|ฝ่าย|อื่น");

        /// <summary>
        /// Diplomatic relation. You are in direct control over the nations resources.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => TextLib.ThaiConv("ผู้|ใต้|ปก|ครอง");

        /// <summary>
        /// Diplomatic relation. Full co-operation.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => TextLib.ThaiConv("พันธมิตร");

        /// <summary>
        /// Diplomatic relation. Reduced chance of war.
        /// </summary>
        public override string Diplomacy_RelationType_Good => TextLib.ThaiConv("ดี");

        /// <summary>
        /// Diplomatic relation. Peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => TextLib.ThaiConv("สันติ");

        /// <summary>
        /// Diplomatic relation. Have not yet made any contact.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => TextLib.ThaiConv("เป็น|กลาง");
        /// <summary>
        /// Diplomatic relation. Temporary peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => TextLib.ThaiConv("สงบ|ศึก");
        /// <summary>
        /// Diplomatic relation. War.
        /// </summary>
        public override string Diplomacy_RelationType_War => TextLib.ThaiConv("สงคราม");
        /// <summary>
        /// Diplomatic relation. War with no chance of peace.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => TextLib.ThaiConv("สงคราม|เบ็ด|เสร็จ");

        /// <summary>
        /// Diplomatic communication. How well you can discuss terms. 0: SpeakTerms
        /// </summary>
        public override string Diplomacy_SpeakTermIs => TextLib.ThaiConv("ระดับ|การ|เจรจา: {0}");

        /// <summary>
        /// Diplomatic communication. Better than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => TextLib.ThaiConv("ดี");

        /// <summary>
        /// Diplomatic communication. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => TextLib.ThaiConv("ปกติ");

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => TextLib.ThaiConv("แย่");

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => TextLib.ThaiConv("ไม่|เจรจา");

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => TextLib.ThaiConv("สร้าง|ความ|สัมพันธ์|กับ: {0}");

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferPeace => TextLib.ThaiConv("เสนอ|สันติ|ภาพ");

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferAlliance => TextLib.ThaiConv("เสนอ|การ|เป็น|พันธมิตร");

        /// <summary>
        /// Diplomatic title. Another player Suggested a new diplomatic relation. 0: player name
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => TextLib.ThaiConv("{0} |เสนอ|ความ|สัมพันธ์|ใหม่");

        /// <summary>
        /// Diplomatic action. Accept new diplomatic relation.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => TextLib.ThaiConv("ตอบ|รับ|ข้อ|เสนอ|ความ|สัมพันธ์");

        /// <summary>
        /// Diplomatic description. Another player Suggested a new diplomatic relation. 0: relation type
        /// </summary>
        public override string Diplomacy_NewRelationOffered => TextLib.ThaiConv("ข้อ|เสนอ|ความ|สัมพันธ์|ใหม่: {0}");

        /// <summary>
        /// Diplomatic action. Make another nation to serve you.
        /// </summary>
        public override string Diplomacy_AbsorbServant => TextLib.ThaiConv("ผนวก|เป็น|ผู้|ใต้|ปก|ครอง");

        /// <summary>
        /// Diplomatic description. Is against evil.
        /// </summary>
        public override string Diplomacy_LightSide => TextLib.ThaiConv("อยู่|ฝ่าย|แสง|สว่าง");

        /// <summary>
        /// Diplomatic description. How long the truce will last.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => TextLib.ThaiConv("จะ|สิ้น|สุด|ใน| {0} |วินาที");

        /// <summary>
        /// Diplomatic action. Make the truce last longer.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => TextLib.ThaiConv("ต่อ|เวลา|สงบ|ศึก");

        /// <summary>
        /// Diplomatic description. How long the truce will be extended.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => TextLib.ThaiConv("ต่อ|เวลา|สงบ|ศึก|ออก|ไป|อีก| {0} |วินาที");

        /// <summary>
        /// Diplomatic description. Going against an agreed relation will cost diplomatic points.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => TextLib.ThaiConv("การ|ตัด|ความ|สัมพันธ์|จะ|เสีย| {0} |แต้ม|การ|ทูต");

        /// <summary>
        /// Diplomatic description for allies.
        /// </summary>
        public override string Diplomacy_AllyDescription => TextLib.ThaiConv("พันธมิตร|จะ|ร่วม|ประกาศ|สงคราม|กับ|ศัตรู|ของ|ท่าน");

        /// <summary>
        /// Diplomatic description for good relation.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => TextLib.ThaiConv("จำกัด|ความ|สามารถ|ใน|การ|ประกาศ|สงคราม");

        /// <summary>
        /// Diplomatic description. You must have a larger military force than your servant (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => TextLib.ThaiConv("พลัง|ทหาร|ต้อง|เข้ม|แข็ง|กว่า| {0} |เท่า");

        /// <summary>
        /// Diplomatic description. Servant must be stuck in a hopeless war (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => TextLib.ThaiConv("ผู้|ใต้|ปก|ครอง|ต้อง|อยู่|ใน|สงคราม|ที่|ไร้|ทาง|สู้");

        /// <summary>
        /// Diplomatic description. A servant can't own too many cities (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => TextLib.ThaiConv("ผู้|ใต้|ปก|ครอง|มี|เมือง|ได้|สูงสุด| {0} |แห่ง");

        /// <summary>
        /// Diplomatic description. Const in diplomatic points will increase (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => TextLib.ThaiConv("ราคา|จะ|สูง|ขึ้น|ตาม|จำนวน|ผู้|ใต้|ปก|ครอง");

        /// <summary>
        /// Diplomatic description. The result of servant relation, peaceful take over of another nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => TextLib.ThaiConv("ผนวก|ฝ่าย|นั้น|เข้า|มา|ด้วย");

        /// <summary>
        /// Messaage when you recieve a war declaration
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => TextLib.ThaiConv("ประกาศ|สงคราม!");

        /// <summary>
        /// The truce timer har run out, and you go back to war
        /// </summary>
        public override string Diplomacy_TruceEndTitle => TextLib.ThaiConv("ช่วง|เวลา|สงบ|ศึก|สิ้น|สุด|ลง");

        /// <summary>
        /// Stats that are shown on the end game screen. Display title.
        /// </summary>
        public override string EndGameStatistics_Title => TextLib.ThaiConv("สถิติ");
        /// <summary>
        /// Stats that are shown on the end game screen. Total ingame time passed.
        /// </summary>
        public override string EndGameStatistics_Time => TextLib.ThaiConv("เวลา|ใน|เกม: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. How many soldiers you bought.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => TextLib.ThaiConv("ทหาร|ที่|เกณฑ์|มา: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that died in battle.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => TextLib.ThaiConv("ทหาร|ที่|เสีย|ชีวิต|ใน|การ|รบ: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Count of opponent soldiers you killed in battle.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => TextLib.ThaiConv("ทหาร|ศัตรู|ที่|สังหาร|ได้: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that have left you.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => TextLib.ThaiConv("ทหาร|หนี|ทัพ: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities won in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => TextLib.ThaiConv("เมือง|ที่|ยึด|ได้: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities lost in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => TextLib.ThaiConv("เมือง|ที่|เสีย|ไป: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle win results.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => TextLib.ThaiConv("การ|รบ|ที่|ชนะ: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle lost results.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => TextLib.ThaiConv("การ|รบ|ที่|แพ้: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Diplomacy. War declarations made by you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => TextLib.ThaiConv("การ|ประกาศ|สงคราม|โดย|คุณ: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen.  Diplomacy. War declarations made toward you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => TextLib.ThaiConv("การ|ประกาศ|สงคราม|จาก|ศัตรู: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Allies made through diplomacy.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => TextLib.ThaiConv("พันธมิตร|ทางการ|ทูต: {0}");

        /// <summary>
        /// Stats that are shown on the end game screen. Servants made through diplomacy. Servants cities and armies become yours.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => TextLib.ThaiConv("ผู้|ใต้|ปก|ครอง|ทางการ|ทูต: {0}");

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_Army => TextLib.ThaiConv("กอง|ทัพ");

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_SoldierGroup => TextLib.ThaiConv("กลุ่ม");

        /// <summary>
        /// Collective unit type on the map. Common name for village or city.
        /// </summary>
        public override string UnitType_City => TextLib.ThaiConv("เมือง");

        /// <summary>
        /// A group selection of armies
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => TextLib.ThaiConv("กลุ่ม|กอง|ทัพ, จำนวน: {0}");

        /// <summary>
        /// Name for a specialized type of soldier. Standard front line soldier.
        /// </summary>
        public override string UnitType_Soldier => TextLib.ThaiConv("พล|ทหาร");

        /// <summary>
        /// Name for a specialized type of soldier. Naval battle soldier.
        /// </summary>
        public override string UnitType_Sailor => TextLib.ThaiConv("ทหาร|เรือ");

        /// <summary>
        /// Name for a specialized type of soldier. Drafted peasants.
        /// </summary>
        public override string UnitType_Folkman => TextLib.ThaiConv("ทหาร|ชาว|บ้าน");

        /// <summary>
        /// Name for a specialized type of soldier. Shield and spear unit.
        /// </summary>
        public override string UnitType_Spearman => TextLib.ThaiConv("พล|หอก");

        /// <summary>
        /// Name for a specialized type of soldier. Elite force, part of the Kings guard.
        /// </summary>
        public override string UnitType_HonorGuard => TextLib.ThaiConv("องครักษ์");

        /// <summary>
        /// Name for a specialized type of soldier. Anti cavalry, wears long two-handed spears.
        /// </summary>
        public override string UnitType_Pikeman => TextLib.ThaiConv("พล|หอก|ยาว");

        /// <summary>
        /// Name for a specialized type of soldier. Armored cavalry unit.
        /// </summary>
        public override string UnitType_Knight => TextLib.ThaiConv("อัศวิน");

        /// <summary>
        /// Name for a specialized type of soldier. Bow and arrow.
        /// </summary>
        public override string UnitType_Archer => TextLib.ThaiConv("พล|ธนู");

        /// <summary>
        /// Name for a specialized type of soldier. 
        /// </summary>
        public override string UnitType_Crossbow => TextLib.ThaiConv("พล|หน้า|ไม้");

        /// <summary>
        /// Name for a specialized type of soldier. Warmashine that slings large spears.
        /// </summary>
        public override string UnitType_Ballista => TextLib.ThaiConv("บัล|ลิส|ตา");

        /// <summary>
        /// Name for a specialized type of soldier. A fantasy troll wearing a cannon.
        /// </summary>
        public override string UnitType_Trollcannon => TextLib.ThaiConv("โทรลล์|ปืน|ใหญ่");

        /// <summary>
        /// Name for a specialized type of soldier. Soldier from the forest.
        /// </summary>
        public override string UnitType_GreenSoldier => TextLib.ThaiConv("นัก|รบ|แห่ง|พงไพร");

        /// <summary>
        /// Name for a specialized type of soldier. Naval unit from the north.
        /// </summary>
        public override string UnitType_Viking => TextLib.ThaiConv("ไวกิ้ง");

        /// <summary>
        /// Name for a specialized type of soldier. The evil master boss.
        /// </summary>
        public override string UnitType_DarkLord => TextLib.ThaiConv("จอม|มาร");

        /// <summary>
        /// Name for a specialized type of soldier. Soldier that carries a large flag.
        /// </summary>
        public override string UnitType_Bannerman => TextLib.ThaiConv("คน|ถือ|ธง");

        /// <summary>
        /// Name for a military unit. Soldier carrying ship. 0: unit type it carries
        /// </summary>
        public override string UnitType_WarshipWithUnit => TextLib.ThaiConv("เรือ|รบ| {0}");

        public override string UnitType_Description_Soldier => TextLib.ThaiConv("ยูนิต|สารพัด|ประโยชน์");
        public override string UnitType_Description_Sailor => TextLib.ThaiConv("แข็ง|แกร่ง|ใน|การ|รบ|ทาง|น้ำ");
        public override string UnitType_Description_Folkman => TextLib.ThaiConv("ทหาร|ราคา|ถูก|ที่|ไม่|ผ่าน|การ|ฝึก");
        public override string UnitType_Description_HonorGuard => TextLib.ThaiConv("ทหาร|ฝีมือ|เยี่ยม|ที่|ไม่|มี|ค่า|บำรุง|รักษา");
        public override string UnitType_Description_Knight => TextLib.ThaiConv("ทรง|พลัง|ใน|สมรภูมิ|ที่|โล่ง");
        public override string UnitType_Description_Archer => TextLib.ThaiConv("แข็ง|แกร่ง|เมื่อ|มี|คน|คุ้ม|กัน|เท่านั้น");
        public override string UnitType_Description_Crossbow => TextLib.ThaiConv("พล|ยิง|ระยะ|ไกล|ที่|ทรง|พลัง");
        public override string UnitType_Description_Ballista => TextLib.ThaiConv("ได้|เปรียบ|เมื่อ|โจมตี|เมือง");
        public override string UnitType_Description_GreenSoldier => TextLib.ThaiConv("นัก|รบ|เอลฟ์|ที่|น่า|เกรง|ขาม");

        public override string UnitType_Description_DarkLord => TextLib.ThaiConv("บอส|ตัว|สุดท้าย");

        /// <summary>
        /// Information about a soldier type
        /// </summary>
        public override string SoldierStats_Title => TextLib.ThaiConv("ค่า|พลัง|ต่อ|ยูนิต");

        /// <summary>
        /// How many groups of soldiers
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => TextLib.ThaiConv("{0} |กลุ่ม, รวม|ทั้งหมด| {1} |ยูนิต");

        /// <summary>
        /// Soldiers will have different strengths depending if the attack on open field, from ships or attacking a settlement
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => TextLib.ThaiConv("พลัง|โจมตี: บก| {0} | ทะเล| {1} | เมือง| {2}");

        /// <summary>
        /// How many wounds a soldier can endure
        /// </summary>
        public override string SoldierStats_Health => TextLib.ThaiConv("พลัง|ชีวิต: {0}");

        /// <summary>
        /// Some soldiers will increase the army movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => TextLib.ThaiConv("โบนัส|ความ|เร็ว|เดิน|ทัพ|บน|บก: {0}");

        /// <summary>
        /// Some soldiers will increase the ship movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => TextLib.ThaiConv("โบนัส|ความ|เร็ว|เดิน|ทัพ|ทาง|ทะเล: {0}");

        /// <summary>
        /// Purchased soliders will start as recruits and complete their training after a few minutes.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => TextLib.ThaiConv("เวลา|ฝึก: {0} |นาที. จะ|เร็ว|ขึ้น|สอง|เท่า|หาก|กอง|ทัพ|อยู่|ติด|กับ|เมือง");

        /// <summary>
        /// Menu option to control an army. Make them stop moving.
        /// </summary>
        public override string ArmyOption_Halt => TextLib.ThaiConv("หยุด|ทัพ");

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_Disband => TextLib.ThaiConv("ปลด|ประจำ|การ|ยูนิต");

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_Divide => TextLib.ThaiConv("แบ่ง|กอง|ทัพ");

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_RemoveX => TextLib.ThaiConv("นำ| {0} |ออก");

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_DisbandAll => TextLib.ThaiConv("ปลด|ประจำ|การ|ทั้งหมด");

        /// <summary>
        /// Menu option to control an army. 0: Count, 1: Unit type
        /// </summary>
        public override string ArmyOption_XGroupsOfType => TextLib.ThaiConv("{1} | {0} |กลุ่ม");

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToX => TextLib.ThaiConv("ส่ง|ยูนิต|ไป|ยัง| {0}");

        public override string ArmyOption_MergeAllArmies => TextLib.ThaiConv("รวม|กอง|ทัพ|ทั้งหมด");

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => TextLib.ThaiConv("แบ่ง|ยูนิต|ไป|ตั้ง|กอง|ทัพ|ใหม่");

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendX => TextLib.ThaiConv("ส่ง| {0}");

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendAll => TextLib.ThaiConv("ส่ง|ทั้งหมด");

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_DivideHalf => TextLib.ThaiConv("แบ่ง|ครึ่ง|กอง|ทัพ");

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_MergeArmies => TextLib.ThaiConv("รวม|กอง|ทัพ");



        /// <summary>
        /// Purchase soldiers.
        /// </summary>
        public override string UnitType_Recruit => TextLib.ThaiConv("ทหาร|ใหม่");

        /// <summary>
        /// Purchase soldiers of type. 0:type
        /// </summary>
        public override string CityOption_RecruitType => TextLib.ThaiConv("เกณฑ์| {0}");

        /// <summary>
        /// Number of paid soldiers
        /// </summary>
        public override string CityOption_XMercenaries => TextLib.ThaiConv("ทหาร|รับ|จ้าง: {0}");


        /// <summary>
        /// Indicates the number of mercenaries currently available for hire from the market
        /// </summary>
        public override string Hud_MercenaryMarket => TextLib.ThaiConv("ทหาร|รับ|จ้าง|ใน|ตลาด");

        /// <summary>
        /// Purchase a number of paid soldiers
        /// </summary>
        public override string CityOption_BuyXMercenaries => TextLib.ThaiConv("จ้าง|ทหาร|รับ|จ้าง| {0} |นาย");

        public override string CityOption_Mercenaries_Description => TextLib.ThaiConv("ทหาร|จะ|ถูก|เกณฑ์|จาก|ทหาร|รับ|จ้าง|แทน|การ|ใช้|แรง|งาน|ของ|คุณ");

        /// <summary>
        /// Button caption for action. Create housing for more workers.
        /// </summary>
        public override string CityOption_ExpandWorkForce => TextLib.ThaiConv("ขยาย|เขต|แรง|งาน");
        public override string CityOption_ExpandWorkForce_IncreaseMax => TextLib.ThaiConv("แรง|งาน|สูงสุด| +{0}");
        public override string CityOption_ExpandGuardSize => TextLib.ThaiConv("เพิ่ม|จำนวน|ยาม");

        public override string CityOption_Damages => TextLib.ThaiConv("ความ|เสียหาย: {0}");
        public override string CityOption_Repair => TextLib.ThaiConv("ซ่อม|แซม|เมือง");
        public override string CityOption_RepairGain => TextLib.ThaiConv("ซ่อม|แซม|ความ|เสียหาย| {0} |หน่วย");

        public override string CityOption_Repair_Description => TextLib.ThaiConv("ความ|เสียหาย|จะ|ลด|จำนวน|แรง|งาน|ที่|เมือง|รอง|รับ|ได้");


        public override string CityOption_BurnItDown => TextLib.ThaiConv("เผา|ทำลาย|เมือง");
        public override string CityOption_BurnItDown_Description => TextLib.ThaiConv("กำจัด|แรง|งาน|ทั้งหมด|และ|สร้าง|ความ|เสียหาย|สูงสุด");

        /// <summary>
        /// The main boss. Named after a glowing metal stone stuck in their forehead.
        /// </summary>
        public override string FactionName_DarkLord => TextLib.ThaiConv("เนตร|แห่ง|หายนะ");

        /// <summary>
        /// Orc inspired faction. Works for the dark lord.
        /// </summary>
        public override string FactionName_DarkFollower => TextLib.ThaiConv("ผู้|รับ|ใช้|ทมิฬ");

        /// <summary>
        /// The largest faction, the old but corrupted kingdom.
        /// </summary>
        public override string FactionName_UnitedKingdom => TextLib.ThaiConv("สห|อาณาจักร");

        /// <summary>
        /// Elf inspired faction. Lives in harmony with the forest.
        /// </summary>
        public override string FactionName_Greenwood => TextLib.ThaiConv("พงไพร|สี|เขียว");

        /// <summary>
        /// Asian flavored faction to the east 
        /// </summary>
        public override string FactionName_EasternEmpire => TextLib.ThaiConv("จักรวรรดิ|ตะวัน|ออก");

        /// <summary>
        /// Viking flavored kingdom in the north. The largest one.
        /// </summary>
        public override string FactionName_NordicRealm => TextLib.ThaiConv("ดิน|แดน|นอร์|ดิก");

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a bear claw symbol.
        /// </summary>
        public override string FactionName_BearClaw => TextLib.ThaiConv("กรง|เล็บ|หมี");

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a cock symbol.
        /// </summary>
        public override string FactionName_NordicSpur => TextLib.ThaiConv("เดือย|นอร์|ดิก");

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a black raven symbol.
        /// </summary>
        public override string FactionName_IceRaven => TextLib.ThaiConv("เรเวน|น้ำ|แข็ง");

        /// <summary>
        /// Faction famous for killing dragons with powerful ballistas.
        /// </summary>
        public override string FactionName_Dragonslayer => TextLib.ThaiConv("ผู้|พิชิต|มังกร");

        /// <summary>
        /// A mercenary unit from the south. Arabic flavored.
        /// </summary>
        public override string FactionName_SouthHara => TextLib.ThaiConv("เซาท์|ฮารา");

        /// <summary>
        /// Name for neutral CPU controlled nations
        /// </summary>
        public override string FactionName_GenericAi => TextLib.ThaiConv("AI | {0}");

        /// <summary>
        /// Display name for players and their numbers
        /// </summary>
        public override string FactionName_Player => TextLib.ThaiConv("ผู้|เล่น| {0}");

        /// <summary>
        /// Message for when a miniboss is approaching on ships from the south.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => TextLib.ThaiConv("ศัตรู|กำลัง|ใกล้|เข้ามา!");
        public override string EventMessage_HaraMercenaryText => TextLib.ThaiConv("พบ|ทหาร|รับ|จ้าง|ฮารา|ทาง|ทิศ|ใต้");

        /// <summary>
        /// First warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_ProphesyTitle => TextLib.ThaiConv("คำ|ทำนาย|ทมิฬ");
        public override string EventMessage_ProphesyText => TextLib.ThaiConv("เนตร|แห่ง|หายนะ|กำลัง|จะ|ปรากฏ|ตัว |และ|ศัตรู|ของ|ท่าน|จะ|เข้า|พวก|กับ|มัน!");

        /// <summary>
        /// Second warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => TextLib.ThaiConv("ยุค|มืด");
        public override string EventMessage_FinalBossEnterText => TextLib.ThaiConv("เนตร|แห่ง|หายนะ|เข้า|สู่|สมรภูมิ|แล้ว!");

        /// <summary>
        /// Message when the main boss will meet you on the battlefield.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => TextLib.ThaiConv("การ|บุก|ครั้ง|สุดท้าย");
        public override string EventMessage_FinalBattleText => TextLib.ThaiConv("จอม|มาร|เข้า|สู่|สมรภูมิ|แล้ว |นี่|คือ|โอกาส|ที่|ท่าน|จะ|กำจัด|มัน|ให้|สิ้น|ซาก!");

        /// <summary>
        /// Message when soldiers leave the army when you can't pay thier upkeep
        /// </summary>
        public override string EventMessage_DesertersTitle => TextLib.ThaiConv("ทหาร|หนี|ทัพ!");
        public override string EventMessage_DesertersText_Money => TextLib.ThaiConv("ทหาร|ที่|ไม่|ได้|รับ|ค่า|จ้าง|กำลัง|หนี|ทัพ|ไป|จาก|กอง|ทัพ|ของ|คุณ");

        public override string DifficultyDescription_AiAggression => TextLib.ThaiConv("ความ|ดุดัน|ของ| AI: {0}");
        public override string DifficultyDescription_BossSize => TextLib.ThaiConv("ขนาด|ของ|บอส: {0}");
        public override string DifficultyDescription_BossEnterTime => TextLib.ThaiConv("เวลา|ปรากฏ|ตัว|ของ|บอส: {0}");
        public override string DifficultyDescription_AiEconomy => TextLib.ThaiConv("เศรษฐกิจ|ของ| AI: {0}%");
        public override string DifficultyDescription_AiDelay => TextLib.ThaiConv("ความ|ล่า|ช้า|ของ| AI: {0}");
        public override string DifficultyDescription_DiplomacyDifficulty => TextLib.ThaiConv("ความ|ยาก|การ|ทูต: {0}");
        public override string DifficultyDescription_MercenaryCost => TextLib.ThaiConv("ราคา|จ้าง|ทหาร: {0}");
        public override string DifficultyDescription_HonorGuards => TextLib.ThaiConv("องครักษ์: {0}");


        /// <summary>
        /// Game has ended in success.
        /// </summary>
        public override string EndScreen_VictoryTitle => TextLib.ThaiConv("ชัยชนะ!");

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
        {
            "ใน|ยาม|สงบ |เรา|ต่าง|ร่วม|ไว้อาลัย|แด่|ผู้|ล่วง|ลับ",
            "ทุก|ความ|สำเร็จ |มัก|มี|เงา|ของ|การ|เสีย|สละ|ซ่อน|อยู่|เสมอ",
            "จง|จดจำ|การ|เดินทาง|ที่|นำ|พา|เรา|มา|ถึง|จุด|นี้ |ซึ่ง|เต็ม|ไป|ด้วย|ดวง|วิญญาณ|ของ|เหล่า|ผู้|กล้า",
            "ใจ|เรา|เบา|เพราะ|ชัยชนะ |แต่|หนัก|อึ้ง|ด้วย|ความ|เศร้า|ต่อ|ผู้|ล่วง|ลับ"
        };

        public override string EndScreen_DominationVictoryQuote => TextLib.ThaiConv("ข้า|คือ|ผู้|ที่|พระ|เจ้า|เลือก|ให้|มา|ครอง|โลก|ใบ|นี้!");

        /// <summary>
        /// Game has ended in failure.
        /// </summary>
        public override string EndScreen_FailTitle => TextLib.ThaiConv("พ่ายแพ้!");

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
        {
            "ด้วย|ร่างกาย|ที่|เหนื่อย|ล้า|และ|คืน|วัน|ที่|แสน|กังวล |เรา|ขอน้อม|รับ|จุด|จบ|นี้",
            "ความ|พ่ายแพ้|อาจ|ทำให้|แผ่นดิน|มืด|มิด |แต่|ไม่|อาจ|ดับ|แสง|แห่ง|ปณิธาน|ของ|เรา|ได้",
            "แม้อัคคี|ใน|ใจ|จะ|มอด|ดับ |แต่|บุตร|หลาน|จะ|ใช้|เถ้า|ถ่าน|นี้|สร้าง|วัน|ใหม่|ที่|รุ่ง|โรจน์",
            "ให้|เรื่อง|ราว|ของ|เรา|เป็น|เปลว|ไฟ|ที่|จุด|ประกาย|ชัยชนะ|ใน|วัน|หน้า"
        };

        /// <summary>
        /// A small cutscene at the end of the game
        /// </summary>
        public override string EndScreen_WatchEpilogue => TextLib.ThaiConv("ชม|บท|ส่ง|ท้าย");

        /// <summary>
        /// Cutscene title
        /// </summary>
        public override string EndScreen_Epilogue_Title => TextLib.ThaiConv("บท|ส่ง|ท้าย");

        /// <summary>
        /// Cutscene introduction
        /// </summary>
        public override string EndScreen_Epilogue_Text => TextLib.ThaiConv("160 |ปี|ก่อน");

        /// <summary>
        /// The Prologue is a short poem about the game's stroy
        /// </summary>
        public override string GameMenu_WatchPrologue => TextLib.ThaiConv("ชม|บท|นำ");

        public override string Prologue_Title => TextLib.ThaiConv("บท|นำ");

        /// <summary>
        /// The poem must be three lines, the fourth line will be pulled from the names translations to present the name of the boss
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
        {
            "ฝัน|ร้าย|ตาม|หลอก|หลอน|ใน|ยาม|ค่ำ|คืน",
            "คำ|ทำนาย|ถึง|อนาคต|อัน|มืด|มน",
            "จง|เตรียม|รับ|มือ|กับ|การ|มา|เยือน|ของ|มัน"
        };

        /// <summary>
        /// Ingame menu when pausing
        /// </summary>
        public override string GameMenu_Title => TextLib.ThaiConv("เมนู|เกม");

        /// <summary>
        /// Continue playing the game after end screen
        /// </summary>
        public override string GameMenu_ContinueGame => TextLib.ThaiConv("เล่น|ต่อ");

        /// <summary>
        /// Continue playing the game
        /// </summary>
        public override string GameMenu_Resume => TextLib.ThaiConv("กลับ|เข้า|สู่|เกม");

        /// <summary>
        /// Exit to game lobby
        /// </summary>
        public override string GameMenu_ExitGame => TextLib.ThaiConv("ออก|จาก|เกม");

        public override string Hud_Save => TextLib.ThaiConv("บันทึก");
        public override string GameMenu_SaveStateWarnings => TextLib.ThaiConv("คำ|เตือน! |ไฟล์|บันทึก|จะ|หาย|ไป|หาก|มีการ|อัปเดต|เกม");
        public override string GameMenu_LoadState => TextLib.ThaiConv("โหลด");
        public override string GameMenu_ContinueFromSave => TextLib.ThaiConv("เล่น|ต่อ|จาก|ไฟล์|เซฟ");

        public override string GameMenu_AutoSave => TextLib.ThaiConv("เซฟ|อัตโนมัติ");

        public override string GameMenu_Load_PlayerCountError => TextLib.ThaiConv("คุณ|ต้อง|ตั้ง|จำนวน|ผู้|เล่น|ให้|ตรง|กับ|ไฟล์|เซฟ: {0}");

        public override string Progressbar_MapLoadingState => TextLib.ThaiConv("กำลัง|โหลด|แผนที่: {0}");

        public override string Progressbar_ProgressComplete => TextLib.ThaiConv("เสร็จ|สมบูรณ์");

        /// <summary>
        /// 0: progress in percentage, 1: fail count
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => TextLib.ThaiConv("กำลัง|สร้าง: {0}%. (ล้มเหลว {1})");


        /// <summary>
        /// 0: current part, 1: number of parts
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => TextLib.ThaiConv("ส่วน|ที่ {0}/{1}");

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_SaveProgress => TextLib.ThaiConv("กำลัง|บันทึก: {0}");

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_LoadProgress => TextLib.ThaiConv("กำลัง|โหลด: {0}");

        /// <summary>
        /// Progress done, waiting for player input
        /// </summary>
        public override string Progressbar_PressAnyKey => TextLib.ThaiConv("กด|ปุ่ม|ใด|ก็ได้|เพื่อ|ไป|ต่อ");


        /// <summary>
        /// A short tutorial where you are supposed to buy and move a soldier. All advanced controls are locked away until the tutorial is complete.
        /// </summary>
        public override string Tutorial_MenuOption => TextLib.ThaiConv("เล่น|โหมด|ฝึก|สอน");
        public override string Tutorial_MissionsTitle => TextLib.ThaiConv("ภารกิจ|ฝึก|สอน");
        public override string Tutorial_Mission_BuySoldier => TextLib.ThaiConv("เลือก|เมือง|และ|เกณฑ์|ทหาร");
        public override string Tutorial_Mission_MoveArmy => TextLib.ThaiConv("เลือก|กอง|ทัพ|และ|สั่ง|ให้|เคลื่อนที่");

        public override string Tutorial_CompleteTitle => TextLib.ThaiConv("ฝึก|สอน|เสร็จ|สิ้น!");
        public override string Tutorial_CompleteMessage => TextLib.ThaiConv("ปลด|ล็อก|การ|ซูม|เต็ม|รูปแบบ|และ|ตัว|เลือก|เกม|ขั้น|สูง|แล้ว");

        /// <summary>
        /// Displays the button input
        /// </summary>
        public override string Tutorial_SelectInput => TextLib.ThaiConv("เลือก");
        public override string Tutorial_MoveInput => TextLib.ThaiConv("สั่ง|การ|เคลื่อนที่");

        /// <summary>
        /// Versus. Text describing the two armies that will go into battle
        /// </summary>
        public override string Hud_Versus => TextLib.ThaiConv("VS.");

        public override string Hud_WardeclarationTitle => TextLib.ThaiConv("การ|ประกาศ|สงคราม");

        public override string ArmyOption_Attack => TextLib.ThaiConv("โจมตี");



        //----
        /// <summary>
        /// In game settings menu. Change what keys and buttons do when pressed
        /// </summary>
        public override string Settings_ButtonMapping => TextLib.ThaiConv("ตั้ง|ค่า|ปุ่ม|กด");



        /// <summary>
        /// Input type, standard PC input
        /// </summary>
        public override string Input_Source_Keyboard => TextLib.ThaiConv("คีย์บอร์ด|และ|เมาส์");

        /// <summary>
        /// Input type, handheld controller like the xbox uses
        /// </summary>
        public override string Input_Source_Controller => TextLib.ThaiConv("คอนโทรลเลอร์");


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */
        public override string CityMenu_SalePricesTitle => TextLib.ThaiConv("ราคา|ขาย");
        public override string Blueprint_Title => TextLib.ThaiConv("แบบ|แปลน");
        public override string Resource_Tab_Overview => TextLib.ThaiConv("ภาพ|รวม");
        public override string Resource_Tab_Stockpile => TextLib.ThaiConv("คลัง|เสบียง");

        public override string Resource => TextLib.ThaiConv("ทรัพยากร");
        public override string Resource_StockPile_Info => TextLib.ThaiConv("ตั้ง|เป้า|หมาย|ใน|การ|เก็บ|สะสม|ทรัพยากร |เพื่อ|ให้|คน|งาน|ทราบ|ว่า|ควร|สลับ|ไป|ผลิต|ทรัพยากร|อื่น|เมื่อ|ใด");
        public override string Resource_TypeName_Water => TextLib.ThaiConv("น้ำ");
        public override string Resource_TypeName_Wood => TextLib.ThaiConv("ไม้");
        public override string Resource_TypeName_Fuel => TextLib.ThaiConv("เชื้อเพลิง");
        public override string Resource_TypeName_Stone => TextLib.ThaiConv("หิน");
        public override string Resource_TypeName_RawFood => TextLib.ThaiConv("วัตถุดิบ|อาหาร");
        public override string Resource_TypeName_Food => TextLib.ThaiConv("อาหาร");
        public override string Resource_TypeName_Beer => TextLib.ThaiConv("เบียร์");
        public override string Resource_TypeName_Wheat => TextLib.ThaiConv("ข้าว|สาลี");
        public override string Resource_TypeName_Linen => TextLib.ThaiConv("ผ้า|ลินิน");
        //public override string Resource_TypeName_SkinAndLinen => TextLib.ThaiConv("หนัง|และ|ผ้า|ลินิน");
        public override string Resource_TypeName_IronOre => TextLib.ThaiConv("แร่|เหล็ก");
        public override string Resource_TypeName_GoldOre => TextLib.ThaiConv("แร่|ทอง");
        public override string Resource_TypeName_Iron => TextLib.ThaiConv("เหล็ก|แท่ง");

        public override string Resource_TypeName_SharpStick => TextLib.ThaiConv("ไม้|แหลม");
        public override string Resource_TypeName_Sword => TextLib.ThaiConv("ดาบ");
        public override string Resource_TypeName_KnightsLance => TextLib.ThaiConv("หอก|อัศวิน");
        public override string Resource_TypeName_TwoHandSword => TextLib.ThaiConv("ดาบ|สอง|มือ");
        public override string Resource_TypeName_Bow => TextLib.ThaiConv("ธนู");

        public override string Resource_TypeName_LightArmor => TextLib.ThaiConv("เกราะ|เบา");
        public override string Resource_TypeName_MediumArmor => TextLib.ThaiConv("เกราะ|กลาง");
        public override string Resource_TypeName_HeavyArmor => TextLib.ThaiConv("เกราะ|หนัก");

        public override string ResourceType_Children => TextLib.ThaiConv("เด็ก|ๆ");

        public override string BuildingType_DefaultName => TextLib.ThaiConv("สิ่ง|ก่อ|สร้าง");
        public override string BuildingType_WorkerHut => TextLib.ThaiConv("กระท่อม|คน|งาน");
        public override string BuildingType_Brewery => TextLib.ThaiConv("โรง|บ่ม|เบียร์");
        public override string BuildingType_Postal => TextLib.ThaiConv("สถานี|ขน|ส่ง|ข่าว");
        public override string BuildingType_Recruitment => TextLib.ThaiConv("ศูนย์|เกณฑ์|ทหาร");
        public override string BuildingType_Barracks => TextLib.ThaiConv("โรง|ทหาร");
        public override string BuildingType_PigPen => TextLib.ThaiConv("คอก|หมู");
        public override string BuildingType_HenPen => TextLib.ThaiConv("เล้า|ไก่");
        public override string BuildingType_WorkBench => TextLib.ThaiConv("โต๊ะ|งาน|ช่าง");
        public override string BuildingType_Carpenter => TextLib.ThaiConv("โรง|ช่าง|ไม้");
        public override string BuildingType_CoalPit => TextLib.ThaiConv("หลุม|เผา|ถ่าน");
        public override string DecorType_Statue => TextLib.ThaiConv("รูป|ปั้น");
        public override string DecorType_Pavement => TextLib.ThaiConv("ทาง|เดิน|หิน");
        public override string BuildingType_Smith => TextLib.ThaiConv("โรง|ตี|เหล็ก");
        public override string BuildingType_Cook => TextLib.ThaiConv("โรง|ครัว");
        public override string BuildingType_Storage => TextLib.ThaiConv("โรง|เก็บ|ของ");

        public override string BuildingType_ResourceFarm => TextLib.ThaiConv("ฟาร์ม| {0}");

        public override string BuildingType_WorkerHut_DescriptionLimitX => TextLib.ThaiConv("เพิ่ม|ขีด|จำกัด|คน|งาน| {0} |คน");
        public override string BuildingType_Tavern_Description => TextLib.ThaiConv("คน|งาน|สามารถ|มา|ทาน|อาหาร|ที่|นี่|ได้");
        public override string BuildingType_Tavern_Brewery => TextLib.ThaiConv("แหล่ง|ผลิต|เบียร์");
        public override string BuildingType_Postal_Description => TextLib.ThaiConv("ส่ง|ทรัพยากร|ไป|ยัง|เมือง|อื่น");
        public override string BuildingType_Recruitment_Description => TextLib.ThaiConv("ส่ง|คน|ไป|ยัง|เมือง|อื่น");
        public override string BuildingType_Barracks_Description => TextLib.ThaiConv("ใช้|คน|และ|อุปกรณ์|ใน|การ|ฝึก|ทหาร");
        public override string BuildingType_PigPen_Description => TextLib.ThaiConv("เลี้ยง|หมู|เพื่อ|ผลิต|อาหาร|และ|หนัง");
        public override string BuildingType_HenPen_Description => TextLib.ThaiConv("เลี้ยง|ไก่|และ|เก็บ|ไข่|เพื่อ|เป็น|อาหาร");
        public override string BuildingType_Decor_Description => TextLib.ThaiConv("ของ|ตก|แต่ง");
        public override string BuildingType_Farm_Description => TextLib.ThaiConv("เพาะ|ปลูก|ทรัพยากร");

        public override string BuildingType_Cook_Description => TextLib.ThaiConv("สถานี|ปรุง|อาหาร");
        public override string BuildingType_Bench_Description => TextLib.ThaiConv("สถานี|คราฟต์|ไอเทม");

        public override string BuildingType_Smith_Description => TextLib.ThaiConv("สถานี|งาน|โลหะ");
        public override string BuildingType_Carpenter_Description => TextLib.ThaiConv("สถานี|งาน|ไม้");

        public override string BuildingType_Nobelhouse_Description => TextLib.ThaiConv("ที่|พัก|สำหรับ|อัศวิน|และ|นัก|การ|ทูต");
        public override string BuildingType_CoalPit_Description => TextLib.ThaiConv("ผลิต|เชื้อเพลิง|อย่าง|มี|ประสิทธิภาพ");
        public override string BuildingType_Storage_Description => TextLib.ThaiConv("จุด|รับ|ฝาก|ทรัพยากร");

        public override string MenuTab_Info => TextLib.ThaiConv("ข้อมูล");
        public override string MenuTab_Work => TextLib.ThaiConv("งาน");
        public override string MenuTab_Recruit => TextLib.ThaiConv("เกณฑ์|ทหาร");
        public override string MenuTab_Resources => TextLib.ThaiConv("ทรัพยากร");
        public override string MenuTab_Trade => TextLib.ThaiConv("การ|ค้า");
        public override string MenuTab_Build => TextLib.ThaiConv("ก่อ|สร้าง");
        public override string MenuTab_Economy => TextLib.ThaiConv("เศรษฐกิจ");
        public override string MenuTab_Delivery => TextLib.ThaiConv("ขน|ส่ง");

        public override string MenuTab_Build_Description => TextLib.ThaiConv("สร้าง|อาคาร|ใน|เมือง|ของ|คุณ");
        public override string MenuTab_BlackMarket_Description => TextLib.ThaiConv("สร้าง|อาคาร|ใน|เมือง|ของ|คุณ");
        public override string MenuTab_Resources_Description => TextLib.ThaiConv("สร้าง|อาคาร|ใน|เมือง|ของ|คุณ");
        public override string MenuTab_Work_Description => TextLib.ThaiConv("สร้าง|อาคาร|ใน|เมือง|ของ|คุณ");
        public override string MenuTab_Automation_Description => TextLib.ThaiConv("สร้าง|อาคาร|ใน|เมือง|ของ|คุณ");

        public override string BuildHud_OutsideCity => TextLib.ThaiConv("นอก|เขต|เมือง");
        public override string BuildHud_OutsideFaction => TextLib.ThaiConv("นอก|เขต|พรม|แดน|ของ|คุณ!");

        public override string BuildHud_OccupiedTile => TextLib.ThaiConv("พื้นที่|ไม่|ว่าง");

        public override string Build_PlaceBuilding => TextLib.ThaiConv("ก่อ|สร้าง");
        public override string Build_DestroyBuilding => TextLib.ThaiConv("ทำลาย");
        public override string Build_ClearTerrain => TextLib.ThaiConv("ปรับ|หน้า|ดิน");

        public override string Build_ClearOrders => TextLib.ThaiConv("ยกเลิก|คำ|สั่ง|สร้าง");
        public override string Build_Order => TextLib.ThaiConv("คำ|สั่ง|สร้าง");
        public override string Build_OrderQue => TextLib.ThaiConv("คิว|คำ|สั่ง|สร้าง: {0}");
        public override string Build_AutoPlace => TextLib.ThaiConv("วาง|อัตโนมัติ");

        public override string Work_OrderPrioTitle => TextLib.ThaiConv("ลำดับ|ความ|สำคัญ");
        public override string Work_OrderPrioDescription => TextLib.ThaiConv("ลำดับ|เริ่ม|จาก| 1 (ต่ำ) |ถึง| {0} (สูง)");

        public override string Work_OrderPrio_No => TextLib.ThaiConv("ไม่|มี|ความ|สำคัญ |จะ|ไม่|ถูก|ดำเนิน|การ");
        public override string Work_OrderPrio_Min => TextLib.ThaiConv("ความ|สำคัญ|ต่ำ|สุด");
        public override string Work_OrderPrio_Max => TextLib.ThaiConv("ความ|สำคัญ|สูง|สุด");

        public override string Work_Move => TextLib.ThaiConv("ย้าย|ไอเทม");

        public override string Work_GatherXResource => TextLib.ThaiConv("เก็บ|เกี่ยว| {0}");
        public override string Work_CraftX => TextLib.ThaiConv("คราฟต์| {0}");
        public override string Work_Farming => TextLib.ThaiConv("กสิกรรม");
        public override string Work_Mining => TextLib.ThaiConv("ทำ|เหมือง");
        public override string Work_Trading => TextLib.ThaiConv("การ|ค้า|ขาย");

        public override string Work_AutoBuild => TextLib.ThaiConv("สร้าง|และ|ขยาย|อัตโนมัติ");

        public override string WorkerHud_WorkType => TextLib.ThaiConv("สถานะ|งาน: {0}");
        public override string WorkerHud_Carry => TextLib.ThaiConv("ถือ|ของ: {0} {1}");
        public override string WorkerHud_Energy => TextLib.ThaiConv("พลัง|งาน: {0}");
        public override string WorkerStatus_Exit => TextLib.ThaiConv("ออก|จาก|กลุ่ม|คน|งาน");
        public override string WorkerStatus_Eat => TextLib.ThaiConv("กิน|อาหาร");
        public override string WorkerStatus_Till => TextLib.ThaiConv("พรวน|ดิน");
        public override string WorkerStatus_Plant => TextLib.ThaiConv("ปลูก|ผัก");
        public override string WorkerStatus_Gather => TextLib.ThaiConv("เก็บ|เกี่ยว");
        public override string WorkerStatus_PickUpResource => TextLib.ThaiConv("หยิบ|ทรัพยากร");
        public override string WorkerStatus_DropOff => TextLib.ThaiConv("ส่ง|ของ");
        public override string WorkerStatus_BuildX => TextLib.ThaiConv("กำลัง|สร้าง| {0}");
        public override string WorkerStatus_TrossReturnToArmy => TextLib.ThaiConv("หน่วย|สนับสนุน|กลับ|เข้า|กอง|ทัพ");

        public override string Hud_ToggleFollowFaction => TextLib.ThaiConv("สลับ|การ|ใช้|ค่า|ตาม|ฝ่าย");
        public override string Hud_FollowFaction_Yes => TextLib.ThaiConv("ตั้ง|ค่า|ให้|ใช้|การ|ตั้ง|ค่า|รวม|ของ|ฝ่าย");
        public override string Hud_FollowFaction_No => TextLib.ThaiConv("ตั้ง|ค่า|ให้|ใช้|การ|ตั้ง|ค่า|เฉพาะ|พื้นที่| (ค่า|รวม|คือ| {0})");

        public override string Hud_Idle => TextLib.ThaiConv("ว่าง|งาน");
        public override string Hud_NoLimit => TextLib.ThaiConv("ไม่|จำกัด");

        public override string Hud_None => TextLib.ThaiConv("ไม่|มี");
        public override string Hud_ProductionQueue => TextLib.ThaiConv("คิว|การ|ผลิต");

        public override string Hud_EmptyList => TextLib.ThaiConv("- รายการ|ว่าง -");

        public override string Hud_RequirementOr => TextLib.ThaiConv("- หรือ -");

        public override string Hud_BlackMarket => TextLib.ThaiConv("ตลาด|มืด");

        public override string Language_CollectProgress => TextLib.ThaiConv("{0} / {1}");
        public override string Hud_SelectCity => TextLib.ThaiConv("เลือก|เมือง");
        public override string Conscription_Title => TextLib.ThaiConv("การ|เกณฑ์|ทหาร");
        public override string Conscript_WeaponTitle => TextLib.ThaiConv("อาวุธ");
        public override string Conscript_ArmorTitle => TextLib.ThaiConv("ชุด|เกราะ");
        public override string Conscript_TrainingTitle => TextLib.ThaiConv("การ|ฝึก|ฝน");

        public override string Conscript_SpecializationTitle => TextLib.ThaiConv("ความ|ชำนาญ|พิเศษ");
        public override string Conscript_SpecializationDescription => TextLib.ThaiConv("จะ|เพิ่ม|พลัง|โจมตี|ใน|ด้าน|หนึ่ง|แต่|จะ|ลด|ด้าน|อื่น|ลง|ทั้งหมด| {0}");
        public override string Conscript_SelectBuilding => TextLib.ThaiConv("เลือก|โรง|ทหาร");

        public override string Conscript_WeaponDamage => TextLib.ThaiConv("พลัง|โจมตี|อาวุธ: {0}");
        public override string Conscript_ArmorHealth => TextLib.ThaiConv("พลัง|ป้องกัน|เกราะ: {0}");
        public override string Conscript_TrainingSpeed => TextLib.ThaiConv("ความ|เร็ว|การ|โจมตี: {0}");
        public override string Conscript_TrainingTime => TextLib.ThaiConv("เวลา|ใน|การ|ฝึก: {0}");

        public override string Conscript_Training_Minimal => TextLib.ThaiConv("พื้น|ฐาน|ที่สุด");
        public override string Conscript_Training_Basic => TextLib.ThaiConv("ขั้น|ต้น");
        public override string Conscript_Training_Skillful => TextLib.ThaiConv("เชี่ยวชาญ");
        public override string Conscript_Training_Professional => TextLib.ThaiConv("มือ|อาชีพ");

        public override string Conscript_Specialization_Field => TextLib.ThaiConv("การ|รบ|ที่|ราบ");
        public override string Conscript_Specialization_Sea => TextLib.ThaiConv("การ|รบ|ทาง|เรือ");
        public override string Conscript_Specialization_Siege => TextLib.ThaiConv("การ|ล้อม|เมือง");
        public override string Conscript_Specialization_Traditional => TextLib.ThaiConv("แบบ|ดั้งเดิม");
        public override string Conscript_Specialization_AntiCavalry => TextLib.ThaiConv("ต้าน|ทหาร|ม้า");

        public override string Conscription_Status_CollectingEquipment => TextLib.ThaiConv("กำลัง|รวบรวม|อุปกรณ์: {0}");
        public override string Conscription_Status_CollectingMen => TextLib.ThaiConv("กำลัง|รวบรวม|พล|ทหาร: {0}");
        public override string Conscription_Status_Training => TextLib.ThaiConv("กำลัง|ฝึก|ฝน: {0}");

        public override string ArmyHud_Food_Reserves_X => TextLib.ThaiConv("เสบียง|สะสม: {0}");
        public override string ArmyHud_Food_Upkeep_X => TextLib.ThaiConv("ค่า|บำรุง|เสบียง: {0}");
        public override string ArmyHud_Food_Costs_X => TextLib.ThaiConv("ราคา|เสบียง: {0}");

        public override string Deliver_WillSendXInfo => TextLib.ThaiConv("จะ|ส่ง|ครั้ง|ละ| {0}");
        public override string Delivery_ListTitle => TextLib.ThaiConv("เลือก|บริการ|ขน|ส่ง");
        public override string Delivery_DistanceX => TextLib.ThaiConv("ระยะ|ทาง: {0}");
        public override string Delivery_DeliveryTimeX => TextLib.ThaiConv("เวลา|ขน|ส่ง: {0}");
        public override string Delivery_SenderMinimumCap => TextLib.ThaiConv("ขีด|จำกัด|ขั้น|ต่ำ|ผู้|ส่ง");
        public override string Delivery_RecieverMaximumCap => TextLib.ThaiConv("ขีด|จำกัด|สูงสุด|ผู้|รับ");
        public override string Delivery_ItemsReady => TextLib.ThaiConv("ไอเทม|พร้อม|ส่ง");
        public override string Delivery_RecieverReady => TextLib.ThaiConv("ผู้|รับ|พร้อม|รับ");
        public override string Hud_ThisCity => TextLib.ThaiConv("เมือง|นี้");
        public override string Hud_RecieveingCity => TextLib.ThaiConv("เมือง|ผู้|รับ");

        public override string Info_ButtonIcon => TextLib.ThaiConv("i");

        public override string Info_PerSecond => TextLib.ThaiConv("แสดง|เป็น|ทรัพยากร|ต่อ|วินาที");

        public override string Info_MinuteAverage => TextLib.ThaiConv("ค่า|นี้|คือ|ค่า|เฉลี่ย|จาก| 1 |นาที|ล่าสุด");

        public override string Message_OutOfFood_Title => TextLib.ThaiConv("เสบียง|หมด");
        public override string Message_CityOutOfFood_Text => TextLib.ThaiConv("เสบียง|ราคา|แพง|จะ|ถูก|ซื้อ|จาก|ตลาด|มืด |คน|งาน|จะ|เริ่ม|อด|อยาก|เมื่อ|เงิน|ของ|คุณ|หมด|ลง");

        public override string Hud_EndSessionIcon => TextLib.ThaiConv("X");

        public override string TerrainType => TextLib.ThaiConv("ประเภท|ภูมิประเทศ");

        public override string Hud_EnergyUpkeepX => TextLib.ThaiConv("ค่า|บำรุง|พลัง|งาน|อาหาร| {0}");

        public override string Hud_EnergyAmount => TextLib.ThaiConv("{0} | พลัง|งาน| (วินาที|ใน|การ|ทำ|งาน)");

        public override string Hud_CopySetup => TextLib.ThaiConv("คัด|ลอก|การ|ตั้ง|ค่า");
        public override string Hud_Paste => TextLib.ThaiConv("วาง");

        public override string Hud_Available => TextLib.ThaiConv("พร้อม|ใช้|งาน");

        public override string WorkForce_ChildBirthRequirements => TextLib.ThaiConv("เงื่อนไข|การ|เกิด|ของ|เด็ก");
        public override string WorkForce_AvailableHomes => TextLib.ThaiConv("บ้าน|ที่|ว่าง: {0}");

        /// <summary>
        /// workers require peace to grow(make babies)
        /// </summary>
        public override string WorkForce_Peace => TextLib.ThaiConv("สันติ|ภาพ");
        public override string WorkForce_ChildToManTime => TextLib.ThaiConv("อายุ|เมื่อ|โต|เต็ม|วัย: {0} |นาที");

        public override string Economy_TaxIncome => TextLib.ThaiConv("ราย|ได้|จาก|ภาษี: {0}");
        public override string Economy_ImportCostsForResource => TextLib.ThaiConv("ค่า|นำ|เข้า| {0}: {1}");
        public override string Economy_BlackMarketCostsForResource => TextLib.ThaiConv("ราคา|ตลาด|มืด|สำหรับ| {0}: {1}");
        public override string Economy_GuardUpkeep => TextLib.ThaiConv("ค่า|บำรุง|รักษา|ยาม: {0}");

        public override string Economy_LocalCityTrade_Export => TextLib.ThaiConv("การ|ส่ง|ออก|ของ|เมือง: {0}");
        public override string Economy_LocalCityTrade_Import => TextLib.ThaiConv("การ|นำ|เข้า|ของ|เมือง: {0}");

        public override string Economy_ResourceProduction => TextLib.ThaiConv("การ|ผลิต| {0}: {1}");
        public override string Economy_ResourceSpending => TextLib.ThaiConv("การ|ใช้|จ่าย| {0}: {1}");

        public override string Economy_TaxDescription => TextLib.ThaiConv("ภาษี|คือ| {0} |ทอง|ต่อ|คน|งาน|หนึ่ง|คน");

        public override string Economy_SoldResources => TextLib.ThaiConv("ทรัพยากร|ที่|ขาย| (แร่|ทอง): {0}");

        public override string UnitType_Cities => TextLib.ThaiConv("เมือง");
        public override string UnitType_Armies => TextLib.ThaiConv("กอง|ทัพ");
        public override string UnitType_Worker => TextLib.ThaiConv("คน|งาน");

        public override string UnitType_FootKnight => TextLib.ThaiConv("อัศวิน|ดาบ|ยาว");
        public override string UnitType_CavalryKnight => TextLib.ThaiConv("อัศวิน|ม้า");

        public override string CityCulture_LargeFamilies => TextLib.ThaiConv("ครอบครัว|ใหญ่");
        public override string CityCulture_FertileGround => TextLib.ThaiConv("พื้นดิน|อุดม|สมบูรณ์");
        public override string CityCulture_Archers => TextLib.ThaiConv("พล|ธนู|ฝีมือ|ดี");
        public override string CityCulture_Warriors => TextLib.ThaiConv("นัก|รบ");
        public override string CityCulture_AnimalBreeder => TextLib.ThaiConv("ผู้|เลี้ยง|สัตว์");
        public override string CityCulture_Miners => TextLib.ThaiConv("นัก|ทำ|เหมือง");
        public override string CityCulture_Woodcutters => TextLib.ThaiConv("คน|ตัด|ไม้");
        public override string CityCulture_Builders => TextLib.ThaiConv("ช่าง|ก่อ|สร้าง");

        /// <summary>
        /// Crab mentality: culture where you suppress those who are better at something
        /// </summary>
        public override string CityCulture_CrabMentality => TextLib.ThaiConv("แนว|คิด|แบบ|ปู|ใน|ข้อง");
        public override string CityCulture_DeepWell => TextLib.ThaiConv("บ่อ|น้ำ|ลึก");
        public override string CityCulture_Networker => TextLib.ThaiConv("ผู้|กว้าง|ขวาง");

        /// <summary>
        /// Pit master: someone who is good at burning work (char coal) 
        /// </summary>
        public override string CityCulture_PitMasters => TextLib.ThaiConv("เจ้า|แห่ง|เตา|เผา");

        public override string CityCulture_CultureIsX => TextLib.ThaiConv("วัฒนธรรม: {0}");
        public override string CityCulture_LargeFamilies_Description => TextLib.ThaiConv("เพิ่ม|อัตรา|การ|เกิด|ของ|เด็ก");
        public override string CityCulture_FertileGround_Description => TextLib.ThaiConv("พืช|ผล|ให้|ผล|ผลิต|มาก|ขึ้น");
        public override string CityCulture_Archers_Description => TextLib.ThaiConv("ผลิต|พล|ธนู|ที่|เชี่ยวชาญ");
        public override string CityCulture_Warriors_Description => TextLib.ThaiConv("ผลิต|นัก|รบ|ประชิด|ที่|เชี่ยวชาญ");
        //public override string CityCulture_AnimalBreeder_Description => TextLib.ThaiConv("สัตว์|ให้|ทรัพยากร|มาก|ขึ้น");
        public override string CityCulture_Miners_Description => TextLib.ThaiConv("ขุด|แร่|ได้|มาก|ขึ้น");
        public override string CityCulture_Woodcutters_Description => TextLib.ThaiConv("ต้น|ไม้|ให้|ไม้|มาก|ขึ้น");
        public override string CityCulture_Builders_Description => TextLib.ThaiConv("ก่อ|สร้าง|ได้|รวดเร็ว");
        public override string CityCulture_CrabMentality_Description => TextLib.ThaiConv("การ|ทำ|งาน|ใช้|พลัง|งาน|น้อย|ลง |แต่|ไม่|สามารถ|ฝึก|ทหาร|ฝีมือ|สูง|ได้");
        public override string CityCulture_DeepWell_Description => TextLib.ThaiConv("น้ำ|เติม|เต็ม|ได้|เร็ว|ขึ้น");
        public override string CityCulture_Networker_Description => TextLib.ThaiConv("ระบบ|ขน|ส่ง|ข่าว|มี|ประสิทธิภาพ");
        public override string CityCulture_PitMasters_Description => TextLib.ThaiConv("ผลิต|เชื้อเพลิง|ได้|มาก|ขึ้น");

        public override string CityOption_AutoBuild_Work => TextLib.ThaiConv("ขยาย|เขต|แรง|งาน|อัตโนมัติ");
        public override string CityOption_AutoBuild_Farm => TextLib.ThaiConv("ขยาย|ฟาร์ม|อัตโนมัติ");

        public override string Hud_PurchaseTitle_Resources => TextLib.ThaiConv("ซื้อ|ทรัพยากร");
        public override string Hud_PurchaseTitle_CurrentlyOwn => TextLib.ThaiConv("คุณ|มี|อยู่");

        public override string Tutorial_EndTutorial => TextLib.ThaiConv("จบ|การ|ฝึก|สอน");
        public override string Tutorial_MissionX => TextLib.ThaiConv("ภารกิจ| {0}");
        public override string Tutorial_CollectXAmountOfY => TextLib.ThaiConv("รวบรวม| {1} |จำนวน| {0}");
        public override string Tutorial_SelectTabX => TextLib.ThaiConv("เลือก|แท็บ: {0}");
        public override string Tutorial_IncreasePriorityOnX => TextLib.ThaiConv("เพิ่ม|ลำดับ|ความ|สำคัญ|ของ: {0}");
        public override string Tutorial_PlaceBuildOrder => TextLib.ThaiConv("สั่ง|ก่อ|สร้าง: {0}");
        public override string Tutorial_ZoomInput => TextLib.ThaiConv("ซูม");

        public override string Tutorial_SelectACity => TextLib.ThaiConv("เลือก|เมือง");
        public override string Tutorial_ZoomInWorkers => TextLib.ThaiConv("ซูม|เข้าไป|เพื่อ|ดู|คน|งาน");
        public override string Tutorial_CreateSoldiers => TextLib.ThaiConv("สร้าง|ยูนิต|ทหาร|สอง|กลุ่ม|ด้วย|อุปกรณ์|เหล่านี้: {0}. {1}.");
        public override string Tutorial_ZoomOutOverview => TextLib.ThaiConv("ซูม|ออก|เพื่อ|ดู|ภาพ|รวม|แผนที่");
        public override string Tutorial_ZoomOutDiplomacy => TextLib.ThaiConv("ซูม|ออก|เพื่อ|ดู|หน้า|การ|ทูต");
        public override string Tutorial_ImproveRelations => TextLib.ThaiConv("พัฒนา|ความ|สัมพันธ์|กับ|ฝ่าย|เพื่อน|บ้าน");
        public override string Tutorial_MissionComplete_Title => TextLib.ThaiConv("ภารกิจ|เสร็จ|สิ้น!");
        public override string Tutorial_MissionComplete_Unlocks => TextLib.ThaiConv("ปลด|ล็อก|การ|ควบคุม|ใหม่|แล้ว");

        //patch1
        public override string Resource_ReachedStockpile => TextLib.ThaiConv("ทรัพยากร|ถึง|ขีด|จำกัด|คลัง|สำรอง|แล้ว");

        public override string BuildingType_ResourceMine => TextLib.ThaiConv("เหมือง| {0}");

        public override string Resource_TypeName_BogIron => TextLib.ThaiConv("เหล็ก|เลน");

        public override string Resource_TypeName_Coal => TextLib.ThaiConv("ถ่าน|หิน");

        public override string Language_XUpkeepIsY => TextLib.ThaiConv("ค่า|บำรุง|รักษา| {0}: {1}");
        public override string Language_XCountIsY => TextLib.ThaiConv("จำนวน| {0}: {1}");

        public override string Message_ArmyOutOfFood_Text => TextLib.ThaiConv("จะ|มีการ|ซื้อ|เสบียง|ราคา|แพง|จาก|ตลาด|มืด |ทหาร|ที่|หิว|โซ|จะ|หนี|ทัพ|เมื่อ|เงิน|ของ|คุณ|หมด|ลง");

        public override string Info_ArmyFood => TextLib.ThaiConv("กอง|ทัพ|จะ|เติม|เสบียง|จาก|เมือง|พันธมิตร|ที่|ใกล้|ที่สุด |เสบียง|สามารถ|ซื้อ|ได้|จาก|ฝ่าย|อื่น |แต่|ใน|เขต|ศัตรู|จะ|ซื้อ|เสบียง|ได้|จาก|ตลาด|มืด|เท่านั้น");

        public override string FactionName_Monger => TextLib.ThaiConv("มอง|เกอร์");
        public override string FactionName_Hatu => TextLib.ThaiConv("ฮา|ตู");
        public override string FactionName_Destru => TextLib.ThaiConv("เดส|ทรู");

        //patch2
        public override string Tutorial_BuildSomething => TextLib.ThaiConv("สร้าง|บาง|อย่าง|ที่|สามารถ|ผลิต| {0}");
        public override string Tutorial_BuildCraft => TextLib.ThaiConv("สร้าง|สถานี|คราฟต์|สำหรับ: {0}");
        public override string Tutorial_IncreaseBufferLimit => TextLib.ThaiConv("เพิ่ม|ขีด|จำกัด| Buffer |สำหรับ: {0}");

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => TextLib.ThaiConv("สะสม| {1} |ใน|คลัง|ให้|ถึง| {0}");
        public override string Tutorial_LookAtFoodBlueprint => TextLib.ThaiConv("ดู|ที่| Blueprint |อาหาร");
        public override string Tutorial_CollectFood_Info1 => TextLib.ThaiConv("คน|งาน|จะ|เดิน|ไป|กิน|อาหาร|ที่|ศาลา|กลาง|เมือง");
        public override string Tutorial_CollectFood_Info2 => TextLib.ThaiConv("กอง|ทัพ|จะ|ส่ง|คน|งาน|หน่วย|สนับสนุน| (Tross) |มา|เก็บ|เสบียง");
        public override string Tutorial_CollectFood_Info0 => TextLib.ThaiConv("ต้องการ|ควบคุม|คน|งาน|อย่าง|เต็ม|รูปแบบ|ไหม? |ลอง|ตั้ง|ค่า|ลำดับ|ความ|สำคัญ|งาน|ทั้งหมด|เป็น|ศูนย์ |แล้ว|ค่อย|เปิด|ใช้|งาน|ที|ละ|อย่าง");

        public override string EndGameStatistics_DecorsBuilt => TextLib.ThaiConv("ของ|ตก|แต่ง|ที่|สร้าง: {0}");
        public override string EndGameStatistics_StatuesBuilt => TextLib.ThaiConv("รูป|ปั้น|ที่|สร้าง: {0}");

        public override string Info_FoodAndDeliveryLocation => TextLib.ThaiConv("โดย|ปกติ|แล้ว |คน|งาน|จะ|ไป|ที่|ศาลา|กลาง|เมือง|เพื่อ|กิน|อาหาร|หรือ|ส่ง|ของ");
        public override string GameMenu_UseSpeedX => TextLib.ThaiConv("ตัว|เลือก|ความ|เร็ว| {0}");
        public override string GameMenu_LongerBuildQueue => TextLib.ThaiConv("ขยาย|คิว|การ|สร้าง");

        public override string Diplomacy_RelationWithOthers => TextLib.ThaiConv("ความ|สัมพันธ์|ของ|พวกเขา|กับ|ฝ่าย|อื่น");
        public override string Automation_queue_description => TextLib.ThaiConv("จะ|ทำ|ซ้ำ|ไป|เรื่อย|ๆ |จน|กว่า|คิว|จะ|ว่าง");

        public override string BuildingType_Storehouse_Description => TextLib.ThaiConv("คน|งาน|สามารถ|นำ|ไอเทม|มา|ส่ง|ที่|นี่|ได้");

        public override string Resource_TypeName_Longbow => TextLib.ThaiConv("ธนู|ยาว");
        public override string Resource_TypeName_Rapeseed => TextLib.ThaiConv("เรป|ซีด");
        public override string Resource_TypeName_Hemp => TextLib.ThaiConv("กัญชง");

        public override string Resource_BogIronDescription => TextLib.ThaiConv("การ|ทำ|เหมือง|เหล็ก|มี|ประสิทธิภาพ|กว่า|การ|ใช้|เหล็ก|เลน");


        public override string Resource_FoodSafeGuard_Description => TextLib.ThaiConv("ระบบ|ป้องกัน. จะ|เพิ่ม|ลำดับ|ความ|สำคัญ|ของ|สาย|การ|ผลิต|อาหาร|ให้|สูงสุด |หาก|ลด|ลง|ต่ำ|กว่า| {0}");
        public override string Resource_FoodSafeGuard_Active => TextLib.ThaiConv("ระบบ|ป้องกัน|กำลัง|ทำ|งาน");

        public override string GameMenu_NextSong => TextLib.ThaiConv("เพลง|ถัด|ไป");

        public override string BuildingType_Bank => TextLib.ThaiConv("ธนาคาร");
        public override string BuildingType_GoldDelivery_Description => TextLib.ThaiConv("ส่ง|ทอง|ไป|ยัง|เมือง|อื่น");

        public override string BuildingType_Logistics => TextLib.ThaiConv("ลอจิสติกส์");
        public override string BuildingType_Logistics_Description => TextLib.ThaiConv("อัปเกรด|ความ|สามารถ|ใน|การ|สั่ง|ก่อ|สร้าง|อาคาร");

        public override string BuildingType_Logistics_NationSizeRequirement => TextLib.ThaiConv("จำนวน|แรง|งาน|รวม|ทั้ง|ประเทศ: {0}");
        public override string Requirements_XItemStorageOfY => TextLib.ThaiConv("คลัง|เก็บ| {1} |ของ|เมือง| {0}");


        public override string XP_UnlockBuildQueue => TextLib.ThaiConv("ปลด|ล็อก|คิว|การ|สร้าง|เป็น: {0}");
        public override string XP_UnlockBuilding => TextLib.ThaiConv("ปลด|ล็อก|สิ่ง|ก่อ|สร้าง: ");
        public override string XP_Upgrade => TextLib.ThaiConv("อัปเกรด");

        public override string XP_UpgradeBuildingX => TextLib.ThaiConv("อัปเกรด|สิ่ง|ก่อ|สร้าง: {0}");

        /// <summary>
        /// Title for describing the production cycle of farms
        /// </summary>
        public override string BuildHud_PerCycle => TextLib.ThaiConv("ต่อ|รอบ");
        public override string BuildHud_MayCraft => TextLib.ThaiConv("อาจ|คราฟต์|ได้");
        public override string BuildHud_WorkTime => TextLib.ThaiConv("เวลา|ทำ|งาน: {0}");
        public override string BuildHud_GrowTime => TextLib.ThaiConv("เวลา|เติบ|โต: {0}");
        public override string BuildHud_Produce => TextLib.ThaiConv("ผลิต:");

        public override string BuildHud_Queue => TextLib.ThaiConv("คิว|การ|สร้าง|ที่|อนุญาต: {0}/{1}");

        public override string LandType_Flatland => TextLib.ThaiConv("ที่|ราบ");
        public override string LandType_Water => TextLib.ThaiConv("แหล่ง|น้ำ");
        public override string BuildingType_Wall => TextLib.ThaiConv("กำแพง");
        public override string Delivery_AutoReciever_Description => TextLib.ThaiConv("จะ|ส่ง|ไป|ยัง|เมือง|ที่|มี|ทรัพยากร|น้อย|ที่สุด");

        public override string Hud_On => TextLib.ThaiConv("เปิด");
        public override string Hud_Off => TextLib.ThaiConv("ปิด");

        public override string Hud_Time_Seconds => TextLib.ThaiConv("{0} | วินาที");
        public override string Hud_Time_Minutes => TextLib.ThaiConv("{0} | นาที");
        public override string Hud_Undo => TextLib.ThaiConv("ย้อน|กลับ");
        public override string Hud_Redo => TextLib.ThaiConv("ทำ|ซ้ำ");

        public override string Tag_ViewOnMap => TextLib.ThaiConv("ดู|แท็ก|บน|แผนที่");

        public override string MenuTab_Tag => TextLib.ThaiConv("แท็ก");

        public override string Input_Build => TextLib.ThaiConv("ก่อ|สร้าง");

        public override string FlagEditor_ClearAll => TextLib.ThaiConv("ล้าง|ทั้งหมด");


        public override string CityCulture_Stonemason => TextLib.ThaiConv("ช่าง|สลัก|หิน");
        public override string CityCulture_Stonemason_Description => TextLib.ThaiConv("เก็บ|สะสม|หิน|ได้|ดี|ขึ้น");

        public override string CityCulture_Brewmaster => TextLib.ThaiConv("ผู้|เชี่ยวชาญ|การ|บ่ม");
        public override string CityCulture_Brewmaster_Description => TextLib.ThaiConv("เพิ่ม|ผล|ผลิต|เบียร์");

        public override string CityCulture_Weavers => TextLib.ThaiConv("ช่าง|ทอ");
        public override string CityCulture_Weavers_Description => TextLib.ThaiConv("เพิ่ม|การ|ผลิต|เกราะ|เบา");

        public override string CityCulture_SiegeEngineer => TextLib.ThaiConv("วิศวกร|ล้อม|เมือง");
        public override string CityCulture_SiegeEngineer_Description => TextLib.ThaiConv("เครื่อง|จักร|สงคราม|ทรง|พลัง|ยิ่ง|ขึ้น");

        public override string CityCulture_Armorsmith => TextLib.ThaiConv("ช่าง|ตี|เกราะ");
        public override string CityCulture_Armorsmith_Description => TextLib.ThaiConv("ปรับปรุง|การ|ผลิต|เกราะ|เหล็ก");

        public override string CityCulture_Noblemen => TextLib.ThaiConv("เหล่า|ขุน|นาง");
        public override string CityCulture_Noblemen_Description => TextLib.ThaiConv("อัศวิน|จะ|ทรง|พลัง|ยิ่ง|ขึ้น");

        public override string CityCulture_Seafaring => TextLib.ThaiConv("นัก|เดิน|เรือ");
        public override string CityCulture_Seafaring_Description => TextLib.ThaiConv("ทหาร|มี|ความ|ชำนาญ|ทาง|ทะเล |และ|เรือ|จะ|แข็ง|แกร่ง|ขึ้น");

        public override string CityCulture_Backtrader => TextLib.ThaiConv("พ่อ|ค้า|นอก|รีต");
        public override string CityCulture_Backtrader_Description => TextLib.ThaiConv("ตลาด|มืด|ราคา|ถูก|ลง");

        public override string CityCulture_LawAbiding => TextLib.ThaiConv("ผู้|เคารพ|กฎหมาย");
        public override string CityCulture_LawAbiding_Description => TextLib.ThaiConv("ได้|รับ|ภาษี|เพิ่ม |แต่|ไม่|มี|ตลาด|มืด");

        //##2##

        public override string Hud_Advanced => TextLib.ThaiConv("ขั้น|สูง");
        public override string Hud_Loading => TextLib.ThaiConv("กำลัง|โหลด...");

        public override string CityOption_LowerGuardSize => TextLib.ThaiConv("ปลด|ทหาร|ยาม");
        public override string Hud_Purchase_MinCapacity => TextLib.ThaiConv("ถึง|ขีด|จำกัด|ขั้น|ต่ำ|แล้ว");
        public override string Settings_ResetToDefault => TextLib.ThaiConv("คืน|ค่า|เริ่มต้น");
        public override string Settings_NewGame => TextLib.ThaiConv("เกม|ใหม่");

        public override string Settings_AdvancedGameSettings => TextLib.ThaiConv("ตั้ง|ค่า|เกม|ขั้น|สูง");
        public override string Settings_FoodMultiplier => TextLib.ThaiConv("ตัว|คูณ|อาหาร");
        public override string Settings_FoodMultiplier_Description => TextLib.ThaiConv("ระยะ|เวลา|ที่|คน|งาน|หรือ|ทหาร|จะ|อยู่|ท้อง |ค่า|ที่|สูง|จะ|ทำ|ให้|ประสิทธิภาพ|ของ|เครื่อง|ลด|ลง");

        public override string Settings_GameMode => TextLib.ThaiConv("โหมด|การ|เล่น");

        public override string Settings_Mode_Story => TextLib.ThaiConv("เนื้อ|เรื่อง|เต็ม|รูปแบบ");
        public override string Settings_Mode_IncludeBoss => TextLib.ThaiConv("รวม|อีเวนต์|บอส");
        public override string Settings_Mode_IncludeAttacks => TextLib.ThaiConv("รวม|การ|โจมตี|แบบ|สุ่ม");
        public override string Settings_Mode_Sandbox => TextLib.ThaiConv("แซนด์|บ็อกซ์");
        public override string Settings_Mode_Peaceful => TextLib.ThaiConv("สงบ|สุข");
        public override string Settings_Mode_Peaceful_Description => TextLib.ThaiConv("สงคราม|ทั้งหมด|จะ|เริ่ม|โดย|ผู้|เล่น|เท่านั้น");

        public override string Lobby_ImportSave => TextLib.ThaiConv("นำ|เข้า|ไฟล์|เซฟ");

        public override string Lobby_ExportSave => TextLib.ThaiConv("ส่ง|ออก|ไฟล์|เซฟ");
        public override string Lobby_ExportSave_Description => TextLib.ThaiConv("สร้าง|สำเนา|ไฟล์|และ|เก็บ|ไว้|ใน|โฟลเดอร์|นำ|เข้า: {0}");

        public override string Resource_CurrentAmount => TextLib.ThaiConv("จำนวน|ปัจจุบัน: {0}");
        public override string Resource_MaxAmount_Soft => TextLib.ThaiConv("ขีด|จำกัด|สูงสุด| (แบบ|ยืดหยุ่น): {0}");
        public override string Resource_MaxAmount => TextLib.ThaiConv("ขีด|จำกัด|สูงสุด: {0}");
        public override string Resource_AddPerSec => TextLib.ThaiConv("อัตรา|การ|เพิ่ม: {0} | ต่อ|วินาที");

        public override string Resource_WaterAddLimit => TextLib.ThaiConv("ไม่|สามารถ|แก้ไข|อัตรา|การ|เพิ่ม|ของ|น้ำ|ได้");

        public override string Tutorial_Select_SubTab => TextLib.ThaiConv("และ|เลือก|หมวด|หมู่: {0}");

        public override string Tutorial_OpenGuardSubTab => TextLib.ThaiConv("เปิด|โรง|ทหาร|และ|เลือก|หมวด|หมู่: {0}");
        public override string Tutorial_GuardToWall => TextLib.ThaiConv("ย้าย|ทหาร|ยาม|ไป|ยัง|กำแพง");
        public override string Demo_MissionObjective_Title => TextLib.ThaiConv("วัตถุประสงค์|ภารกิจ");
        public override string Demo_MissionObjective_Description => TextLib.ThaiConv("ป้องกัน|การ|โจมตี|จาก|ทิศ|ใต้");
        public override string Demo_Complete_Title => TextLib.ThaiConv("เดโม|เสร็จ|สิ้น");
        public override string Demo_TimesUp_Title => TextLib.ThaiConv("หมด|เวลา!");
        public override string Demo_EndInOneMinuteDescription => TextLib.ThaiConv("เดโม|จะ|จบ|ลง|ใน|หนึ่ง|นาที");

        public override string ArmyOption_NewArmy => TextLib.ThaiConv("สร้าง|กอง|ทัพ|ใหม่");
        public override string ProfileEditor_AltMain => TextLib.ThaiConv("สี|หลัก|สำรอง");
        public override string Automation_CheckBoxTitle => TextLib.ThaiConv("ใช้|ระบบ|อัตโนมัติ");

        public override string ArmyStructure_ColumnWidth => TextLib.ThaiConv("ความ|กว้าง|แถว|กอง|ทัพ");
        public override string ArmyStructure_ArmyPlacement => TextLib.ThaiConv("การ|จัด|วาง|ใน|กอง|ทัพ");
        public override string ArmyStructure_Row_Front => TextLib.ThaiConv("แนว|หน้า");
        public override string ArmyStructure_Row_Body => TextLib.ThaiConv("ทัพ|หลวง");
        public override string ArmyStructure_Row_Second => TextLib.ThaiConv("แนว|ที่|สอง");
        public override string ArmyStructure_Row_Behind => TextLib.ThaiConv("แนว|หลัง");

        public override string Diplomacy_RelationType_Enemies => TextLib.ThaiConv("ศัตรู");

        public override string EventMessage_EnemyAlliance_Title => TextLib.ThaiConv("ความ|กลัว|ต่อ|การ|ครอบ|งำ");
        public override string EventMessage_EnemyAlliance => TextLib.ThaiConv("นานา|ประเทศ|ที่|หวาด|กลัว|อำนาจ|ที่|เพิ่ม|ขึ้น|ของ|ท่าน |ได้|รวม|ตัว|กัน|เป็น|พันธมิตร|เพื่อ|ต่อ|ต้าน|ท่าน");

        public override string Settings_CentralGold => TextLib.ThaiConv("คลัง|ทอง|ส่วน|กลาง");
        public override string Settings_CentralGold_Description => TextLib.ThaiConv("เปิด: ทอง|ทั้งหมด|จะ|รวม|อยู่|ใน|คลัง|เดียวกัน|เพื่อ|ให้|ใช้|ได้|ทันที. | ปิด: ทอง|จะ|เป็น|ไอเทม|จริง|และ|ต้อง|มีการ|ขน|ส่ง");

        public override string InputActionName_StopStart => TextLib.ThaiConv("หยุด/เริ่ม");
        public override string InputActionName_ToggleHudDetail => TextLib.ThaiConv("สลับ|ราย|ละเอียด| HUD");
        public override string InputActionName_NextCity => TextLib.ThaiConv("เมือง|ถัด|ไป");
        public override string InputActionName_NextArmy => TextLib.ThaiConv("กอง|ทัพ|ถัด|ไป");
        public override string InputActionName_NextBattle => TextLib.ThaiConv("การ|รบ|ถัด|ไป");
        public override string InputActionName_Build => TextLib.ThaiConv("ก่อ|สร้าง");
        public override string InputActionName_Copy => TextLib.ThaiConv("คัด|ลอก");
        public override string InputActionName_Paste => TextLib.ThaiConv("วาง");
        public override string InputActionName_Menu => TextLib.ThaiConv("เมนู");
        public override string InputActionName_FlagDesign_ToggleColor_Prev => TextLib.ThaiConv("สี|ก่อน|หน้า");
        public override string InputActionName_FlagDesign_ToggleColor_Next => TextLib.ThaiConv("สี|ถัด|ไป");
        public override string InputActionName_FlagDesign_PaintBucket => TextLib.ThaiConv("ถัง|สี");
        public override string InputActionName_Controller_FlagDesign_Colorpicker => TextLib.ThaiConv("เครื่อง|มือ|เลือก|สี");
        public override string InputActionName_ControllerFocus => TextLib.ThaiConv("โฟกัส");
        public override string InputActionName_ControllerCancel => TextLib.ThaiConv("ยกเลิก");
        public override string InputActionName_ControllerMessageClick => TextLib.ThaiConv("คลิก|ข้อ|ความ");
        public override string InputActionName_ControllerSelect => TextLib.ThaiConv("เลือก");
        public override string InputActionName_WASD_UP => TextLib.ThaiConv("ขึ้น");
        public override string InputActionName_WASD_DOWN => TextLib.ThaiConv("ลง");
        public override string InputActionName_WASD_LEFT => TextLib.ThaiConv("ซ้าย");
        public override string InputActionName_WASD_RIGHT => TextLib.ThaiConv("ขวา");
        public override string InputActionName_CameraTiltLeft => TextLib.ThaiConv("เอียง|กล้อง|ซ้าย");
        public override string InputActionName_CameraTiltRight => TextLib.ThaiConv("เอียง|กล้อง|ขวา");
        public override string InputActionName_CameraTiltUp => TextLib.ThaiConv("เอียง|กล้อง|ขึ้น");
        public override string InputActionName_ZoomInKey => TextLib.ThaiConv("ซูม|เข้า");
        public override string InputActionName_ZoomOutKey => TextLib.ThaiConv("ซูม|ออก");

        public override string Settings_Title_Monitor => TextLib.ThaiConv("ตัว|เลือก|จอ|ภาพ");
        public override string Settings_Title_Graphics => TextLib.ThaiConv("ตัว|เลือก|กราฟิก");
        public override string Settings_Title_Input => TextLib.ThaiConv("อินพุต");
        public override string Settings_Title_Gameplay => TextLib.ThaiConv("ตัว|เลือก|เกม|เพลย์");
        public override string Settings_PanOnZoom => TextLib.ThaiConv("เลื่อน|กล้อง|ขณะ|ซูม");
        public override string Settings_ScrollSensitivity_Game => TextLib.ThaiConv("ความ|ไว|การ|เลื่อน: ใน|เกม");
        public override string Settings_ScrollSensitivity_Menu => TextLib.ThaiConv("ความ|ไว|การ|เลื่อน: เมนู");
        public override string Settings_Blood => TextLib.ThaiConv("เลือด");

        public override string Settings_MasterVolume => TextLib.ThaiConv("ระดับ|เสียง|รวม");
        public override string Settings_AmbienceVolume => TextLib.ThaiConv("ระดับ|เสียง|บรรยากาศ");
        public override string Settings_BattleMelody => TextLib.ThaiConv("เพลง|ประกอบ|การ|รบ");

        public override string Settings_ModelLight => TextLib.ThaiConv("เอฟเฟกต์|แสง|โมเดล");
        public override string Settings_Particles => TextLib.ThaiConv("เอฟเฟกต์|พาร์|ติ|เคิล");
        public override string Settings_MapLoadSpeed => TextLib.ThaiConv("ความ|เร็ว|การ|โหลด|แผนที่");
        public override string Lobby_Category_Options => TextLib.ThaiConv("ตั้ง|ค่า");
        public override string Lobby_Category_Editor => TextLib.ThaiConv("เครื่อง|มือ|แก้ไข");
        public override string Lobby_Category_ExtraModes => TextLib.ThaiConv("โหมด|พิเศษ");

        public override string Lobby_Editor_MapEditor => TextLib.ThaiConv("เครื่อง|มือ|สร้าง|แผนที่");
        public override string Lobby_Editor_VoxelEditor => TextLib.ThaiConv("เครื่อง|มือ|สร้าง|ว็อก|เซล");

        public override string Lobby_Mode_BattleLab => TextLib.ThaiConv("ห้อง|ทดลอง|การ|รบ");
        public override string Lobby_Mode_BattleLab_Description => TextLib.ThaiConv("นำ|ทหาร|มา|ต่อ|สู้|กัน|เพื่อ|การ|ทดสอบ");
        public override string Lobby_Mode_Commander => TextLib.ThaiConv("เล่น|โหมด|ผู้|บัญชา|การ");
        public override string Lobby_Mode_Commander_Description => TextLib.ThaiConv("บอร์ด|เกม|วาง|แผน|ขนาด|เล็ก");
        public override string Lobby_MusicPlayList => TextLib.ThaiConv("รายการ|เพลง");

        public override string Lobby_GameSetup => TextLib.ThaiConv("ตั้ง|ค่า|เกม");
        public override string Lobby_PlayerSetup => TextLib.ThaiConv("ตั้ง|ค่า|ผู้|เล่น");
        public override string LobbyDemoMode_Demo => TextLib.ThaiConv("เดโม");

        public override string Lobby_Tutorial => TextLib.ThaiConv("ฝึก|สอน");

        public override string LobbyDemoMode_ShortTutorial => TextLib.ThaiConv("ฝึก|สอน|แบบ|ด่วน");
        public override string LobbyDemoMode_LongTutorial => TextLib.ThaiConv("ฝึก|สอน|แบบ|ละเอียด");

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => TextLib.ThaiConv("เพิ่ม|ลง|ใน|สิ่ง|ที่|อยาก|ได้|บน");

        public override string BattleLab_StartHere => TextLib.ThaiConv("เริ่ม|การ|รบ|ที่|นี่");
        public override string BattleLab_Start => TextLib.ThaiConv("เริ่ม|การ|รบ");
        public override string BattleLab_Attacker => TextLib.ThaiConv("ฝ่าย|โจมตี");

        public override string MapGenerator_Name => TextLib.ThaiConv("ระบบ|สร้าง|แผนที่|อัตโนมัติ");

        public override string MapType_CustomMap => TextLib.ThaiConv("แผนที่|ที่|กำหนด|เอง");
        public override string MapType_GenerateNewMap => TextLib.ThaiConv("สร้าง|แผนที่|ใหม่");
        public override string MapGenerator_GenerateAction => TextLib.ThaiConv("เริ่ม|สร้าง");
        public override string MapGenerator_Terrain_CustomSize => TextLib.ThaiConv("กำหนด|ขนาด|เอง");
        public override string MapGenerator_Terrain_StartAs => TextLib.ThaiConv("เริ่มต้น|เป็น");
        public override string MapGenerator_Terrain_ClearPass => TextLib.ThaiConv("เริ่ม|ขั้นตอน|ล้าง|พื้นที่");
        public override string MapGenerator_Terrain_BuildPass => TextLib.ThaiConv("เริ่ม|ขั้นตอน|สร้าง|พื้นที่");
        public override string MapGenerator_Terrain_DigPass => TextLib.ThaiConv("เริ่ม|ขั้นตอน|ขุด|พื้นที่");
        public override string MapGenerator_Terrain_BuildDigLoops => TextLib.ThaiConv("จำนวน|รอบ|การ|สร้าง-ขุด");
        public override string MapGenerator_Terrain_BuildStrokes => TextLib.ThaiConv("จำนวน|การ|วาด|พื้นที่|สร้าง");
        public override string MapGenerator_Terrain_BuildStrokes_Description => TextLib.ThaiConv("วัด|จาก|จำนวน|การ|ลาก|ต่อ| 100 |ช่อง");
        public override string MapGenerator_Terrain_DigStrokes => TextLib.ThaiConv("จำนวน|การ|วาด|พื้นที่|ขุด");
        public override string MapGenerator_Terrain_CleanUp_Option => TextLib.ThaiConv("การ|จัด|การ|พื้นที่|เศษ|เกิน");
        public override string MapGenerator_Terrain_CleanUpPass => TextLib.ThaiConv("เริ่ม|ขั้นตอน|จัด|การ|เศษ|เกิน");



        public override string Economy_ServicemenUpkeep => TextLib.ThaiConv("ค่า|บำรุง|รักษา|พล|ทหาร: {0}");
        public override string Economy_ServicemenUpkeep_Description => TextLib.ThaiConv("ค่า|บำรุง|รักษา|คือ| {0} |ทอง|ต่อ|พล|ทหาร|หนึ่ง|คน");
        public override string Economy_GuardUpkeep_Description => TextLib.ThaiConv("ค่า|บำรุง|รักษา|คือ| {0} |ทอง|ต่อ|ยาม|หนึ่ง|คน");

        public override string EndScreen_TimeHasEndedTitle => TextLib.ThaiConv("หมด|เวลา");

        public override string Hud_AdvancedSettings => TextLib.ThaiConv("ตั้ง|ค่า|ขั้น|สูง");
        public override string Hud_Vector_X => TextLib.ThaiConv("แกน X");
        public override string Hud_Vector_Y => TextLib.ThaiConv("แกน Y");
        public override string Hud_Cancel => TextLib.ThaiConv("ยกเลิก");
        public override string Hud_Delete => TextLib.ThaiConv("ลบ");
        public override string Hud_Next => TextLib.ThaiConv("ถัด|ไป");
        //public override string Hud_None => TextLib.ThaiConv("ไม่|มี");
        public override string Hud_Apply => TextLib.ThaiConv("ตก|ลง|ใช้");
        public override string Hud_AllCities => TextLib.ThaiConv("ทุก|เมือง");
        public override string Hud_Time_Hours => TextLib.ThaiConv("{0} | ชั่วโมง");
        public override string Hud_AddX => TextLib.ThaiConv("เพิ่ม| {0}");
        public override string Hud_Both => TextLib.ThaiConv("ทั้ง|คู่");
        public override string Hud_Direction => TextLib.ThaiConv("ทิศทาง");


        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => TextLib.ThaiConv("{0}, จำนวน: {1}");

        public override string Hud_EffectDoesNotStack => TextLib.ThaiConv("ผล|ของ|สถานะ|นี้|ไม่|ทับ|ซ้อน|กัน");

        public override string Work_SmeltX => TextLib.ThaiConv("หลอม| {0}");

        public override string Info_TotalFoodProduction => TextLib.ThaiConv("การ|ผลิต|อาหาร|รวม");
        public override string Info_TotalFoodSpending => TextLib.ThaiConv("การ|ใช้|อาหาร|รวม");

        public override string Info_FooodAndDeliveryLocation => TextLib.ThaiConv("ตาม|ปกติ|คน|งาน|จะ|ไป|ที่|ศาลา|กลาง|เมือง|เพื่อ|กิน|อาหาร|หรือ|ส่ง|ของ");

        public override string Delivery_SendChunk => TextLib.ThaiConv("จำนวน|ไอเทม|ต่อ|การ|ขน|ส่ง");
        public override string Delivery_SpeedBonus => TextLib.ThaiConv("โบนัส|ความ|เร็ว: {0}%");

        public override string Delivery_AutoResourceDescription => TextLib.ThaiConv("ส่ง|ไอเทม|ที่|ถึง|ขีด|จำกัด|ใน|คลัง|ไป|ยัง|เมือง|ที่|ต้องการ");

        public override string Conscript_Soldiers_ArmyType => TextLib.ThaiConv("พล|ทหาร|กอง|ทัพ");
        public override string Conscript_Soldiers_ArmyType_Description => TextLib.ThaiConv("เกณฑ์|ทหาร|เข้า|สู่|กอง|ทัพ|ที่|อยู่|ติด|กัน");
        public override string Conscript_Soldiers_GuardType => TextLib.ThaiConv("ทหาร|ยาม|ประจำ|เมือง");
        public override string Conscript_Soldiers_GuardType_Description => TextLib.ThaiConv("ทหาร|ยาม|ใช้|สำหรับ|ป้องกัน|กำแพง");
        //-
        public override string Defence_Title => TextLib.ThaiConv("การ|ป้องกัน");
        public override string Defence_GuardPost => TextLib.ThaiConv("จุด|ประจำ|การ|ยาม");

        public override string Defence_WallDescription_Movement => TextLib.ThaiConv("ขัด|ขวาง|การ|เคลื่อนที่|ของ|ศัตรู");
        public override string Defence_WallDescription_GuardPost => TextLib.ThaiConv("สามารถ|ส่ง|ยาม|ไป|ประจำ|การ|ที่|นี่|ได้");
        public override string Defence_AutoAssign => TextLib.ThaiConv("มอบ|หมาย|อัตโนมัติ");
        public override string Defence_AutoAssign_Description => TextLib.ThaiConv("ยาม|ใหม่|จะ|เคลื่อนที่|ไป|ยัง|จุด|นี้|ทันที");
        public override string Conscript_SplashDamage => TextLib.ThaiConv("ความ|เสียหาย|วง|กว้าง");
        public override string Conscript_HighSplashDamage => TextLib.ThaiConv("ความ|เสียหาย|วง|กว้าง|รุนแรง");

        public override string Conscript_Training_Champion => TextLib.ThaiConv("แชมเปี้ยน");
        public override string Conscript_Training_Legendary => TextLib.ThaiConv("ตำนาน");


        public override string Experience_Title => TextLib.ThaiConv("ประสบการณ์");
        public override string Experience_TopExperience => TextLib.ThaiConv("ระดับ|ประสบการณ์|สูงสุด");

        public override string Experience_TimeReductionDescription => TextLib.ThaiConv("เวลา|ทำ|งาน|ลด|ลง| {0}% |ต่อ|ระดับ");

        public override string ExperienceType_Farm => TextLib.ThaiConv("ชาว|นา");
        public override string ExperienceType_AnimalCare => TextLib.ThaiConv("คน|เลี้ยง|สัตว์");
        public override string ExperienceType_HouseBuilding => TextLib.ThaiConv("ช่าง|สร้าง|บ้าน");
        public override string ExperienceType_WoodWork => TextLib.ThaiConv("ช่าง|ไม้");
        public override string ExperienceType_StoneCutter => TextLib.ThaiConv("ช่าง|หิน");
        public override string ExperienceType_Mining => TextLib.ThaiConv("คน|ทำ|เหมือง");
        public override string ExperienceType_Transport => TextLib.ThaiConv("คน|ขน|ส่ง");
        public override string ExperienceType_Cook => TextLib.ThaiConv("พ่อ|ครัว");
        public override string ExperienceType_Fletcher => TextLib.ThaiConv("ช่าง|ทำ|ลูก|ธนู");
        public override string ExperienceType_RefineOre => TextLib.ThaiConv("ช่าง|หลอม|แร่");
        public override string ExperienceType_Casting => TextLib.ThaiConv("ช่าง|หล่อ|โลหะ");
        public override string ExperienceType_CraftMetal => TextLib.ThaiConv("ช่าง|ตี|เหล็ก");
        public override string ExperienceType_CraftArmor => TextLib.ThaiConv("ช่าง|ทำ|ชุด|เกราะ");
        public override string ExperienceType_CraftWeapon => TextLib.ThaiConv("ช่าง|ตี|อาวุธ");
        public override string ExperienceType_CraftFuel => TextLib.ThaiConv("คน|เผา|ถ่าน");
        public override string ExperienceType_Chemist => TextLib.ThaiConv("นัก|เคมี");

        public override string ExperienceLevel_1 => TextLib.ThaiConv("มือ|ใหม่");
        public override string ExperienceLevel_2 => TextLib.ThaiConv("ผู้|ฝึก|ฝน");
        public override string ExperienceLevel_3 => TextLib.ThaiConv("ผู้|เชี่ยวชาญ");
        public override string ExperienceLevel_4 => TextLib.ThaiConv("ปรมาจารย์");
        public override string ExperienceLevel_5 => TextLib.ThaiConv("ตำนาน");

        public override string ExperenceOrDistancePrio_Title => TextLib.ThaiConv("การ|เลือก|คน|งาน");
        public override string ExperenceOrDistancePrio_Description => TextLib.ThaiConv("คน|งาน|ที่|ว่าง|จะ|ถูก|เลือก|มา|ทำ|งาน|โดย|พิจารณา|จาก|ระยะ|ทาง|หรือ|ประสบการณ์");


        public override string Technology_Description => TextLib.ThaiConv("ทุก|เมือง|จะ|มี|ผัง|เทคโนโลยี |แต่ละ|เทคโนโลยี|จะ|ปลด|ล็อก|สิ่ง|ก่อ|สร้าง|และ|ไอเทม|ใหม่|ๆ");
        public override string Experience_Description => TextLib.ThaiConv("คน|งาน|จะ|ได้รับ|ประสบการณ์|และ|เก่ง|ขึ้น|เรื่อย|ๆ");


        public override string Technology_Title => TextLib.ThaiConv("เทคโนโลยี");
        public override string Technology_ShareField => TextLib.ThaiConv("สาย|งาน|เทคโนโลยี|ที่|ใช้|ร่วม|กัน");

        public override string Technology_GainByNeigborRelation => TextLib.ThaiConv("สำหรับ|ทุก|เมือง|ข้าง|เคียง|ที่|มี|เทคโนโลยี|นี้ |และ|ความ|สัมพันธ์|ของ|ท่าน|คือ| {0}: {1}");
        public override string Technology_ForEachMaster => TextLib.ThaiConv("เมื่อ| {0} |ถึง|ระดับ|ประสบการณ์| {1} |ใน|สาย|เทคโนโลยี: {2}");
        public override string Technology_CitySpread => TextLib.ThaiConv("เมือง|ของ|ท่าน|จะ|แบ่ง|ปัน|เทคโนโลยี|เมื่อ|อยู่|ติด|กัน: {0}");
        public override string Technology_CityCapture => TextLib.ThaiConv("เทคโนโลยี|ส่วน|ใหญ่|จะ|ถูก|ทำลาย|เมื่อ|เมือง|ถูก|ยึด|ใน|สงคราม");

        public override string Technology_AdvancedBuildings => TextLib.ThaiConv("สิ่ง|ก่อ|สร้าง|ขั้น|สูง");
        public override string Technology_AdvancedFarming => TextLib.ThaiConv("เกษตรกรรม|ขั้น|สูง");
        public override string Technology_AdvancedCasting => TextLib.ThaiConv("การ|หล่อ|โลหะ|ขั้น|สูง");

        public override string Help_Title => TextLib.ThaiConv("ความ|ช่วยเหลือ");
        public override string Help_Work_Title => TextLib.ThaiConv("ทำไม|งาน|ไม่|เริ่ม");
        public override string Help_Work_Resources => TextLib.ThaiConv("สิ่ง|ก่อ|สร้าง|จำเป็น|ต้อง|มี|ทรัพยากร|พร้อม|ใช้|งาน");
        public override string Help_Work_Skill => TextLib.ThaiConv("คน|งาน|จำเป็น|ต้อง|มี|ระดับ|ทักษะ|ที่|กำหนด| (หรือ|สูง|กว่า)");
        public override string Help_Work_Stockpile => TextLib.ThaiConv("การ|เก็บ|สะสม|ทรัพยากร|จะ|หยุด|ลง|หาก|คลัง|สินค้า|เต็ม");
        public override string Help_Work_Priority => TextLib.ThaiConv("งาน|นั้น|อาจ|มี|ลำดับ|ความ|สำคัญ|ต่ำ|หรือ|เป็น|ศูนย์");


        public override string Help_Soldiers_Title => TextLib.ThaiConv("การ|ผลิต|ทหาร");
        public override string Help_Soldiers_PlaceBuildingX => TextLib.ThaiConv("สร้าง|สิ่ง|ก่อ|สร้าง: {0}");
        public override string Help_Soldiers_Workers => TextLib.ThaiConv("ต้อง|มี|คน|งาน|ที่|ว่าง|เพื่อ|มา|เกณฑ์|ทหาร");
        public override string Help_Soldiers_Weapon => TextLib.ThaiConv("ต้อง|มี|อาวุธ|สำหรับ|ทหาร|แต่ละ|คน");
        public override string Help_Soldiers_StartX => TextLib.ThaiConv("เริ่มต้น: {0}");

        public override string Hud_SelectHistory => TextLib.ThaiConv("เลือก|ประวัติ");

        public override string Hud_PointsPerMinute => TextLib.ThaiConv("{0} | แต้ม|ต่อ|นาที");
        public override string Hud_PercentValueCost => TextLib.ThaiConv("ค่า|บริการ|คิด|เป็น| {0}% |ของ|มูลค่า");

        public override string Hud_Mixed => TextLib.ThaiConv("ผสม");
        public override string Hud_Distance => TextLib.ThaiConv("ระยะ|ทาง");

        public override string Hud_Unlock => TextLib.ThaiConv("ปลด|ล็อก");
        public override string Hud_category => TextLib.ThaiConv("หมวด|หมู่");

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public override string Input_StepOneFrame => TextLib.ThaiConv("ขยับ|ที|ละ| 1 |เฟรม");

        public override string Resource_TypeName_Wagon2Wheel => TextLib.ThaiConv("เกวียน|เล็ก");
        public override string Resource_TypeName_Wagon4Wheel => TextLib.ThaiConv("เกวียน|ใหญ่");
        public override string Resource_TypeName_Tin => TextLib.ThaiConv("ดีบุก");
        public override string Resource_TypeName_TinOre => TextLib.ThaiConv("แร่|ดีบุก");

        public override string Resource_TypeName_Copper => TextLib.ThaiConv("ทองแดง");
        public override string Resource_TypeName_CopperOre => TextLib.ThaiConv("แร่|ทองแดง");
        public override string Resource_TypeName_SilverOre => TextLib.ThaiConv("แร่|เงิน");
        public override string Resource_TypeName_Silver => TextLib.ThaiConv("เงิน");

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => TextLib.ThaiConv("แร่|มิทริล|ดิบ");
        public override string Resource_TypeName_Mithril => TextLib.ThaiConv("มิทริล");

        public override string Resource_TypeName_BronzeSword => TextLib.ThaiConv("ดาบ|ทอง|สัมฤทธิ์");
        public override string Resource_TypeName_ShortSword => TextLib.ThaiConv("ดาบ|สั้น");
        public override string Resource_TypeName_LongSword => TextLib.ThaiConv("ดาบ|ยาว");
        public override string Resource_TypeName_HandSpear => TextLib.ThaiConv("หอก|สั้น");
        public override string Resource_TypeName_Warhammer => TextLib.ThaiConv("ค้อน|ศึก");
        public override string Resource_TypeName_MithrilSword => TextLib.ThaiConv("ดาบ|มิทริล");
        public override string Resource_TypeName_SlingShot => TextLib.ThaiConv("เครื่อง|ยิง|กระสุน|หิน");
        public override string Resource_TypeName_ThrowingSpear => TextLib.ThaiConv("หอก|ซัด");
        public override string Resource_TypeName_Crossbow => TextLib.ThaiConv("หน้า|ไม้");
        public override string Resource_TypeName_MithrilBow => TextLib.ThaiConv("ธนู|มิทริล");

        public override string Resource_TypeName_CoolingFluid => TextLib.ThaiConv("สาร|หล่อ|เย็น");
        public override string Resource_TypeName_Palisade => TextLib.ThaiConv("รั้ว|ไม้|ระเนียด");
        public override string Resource_TypeName_Toolkit => TextLib.ThaiConv("ชุด|เครื่อง|มือ");

        public override string Resource_TypeName_Sulfur => TextLib.ThaiConv("กำมะถัน");
        public override string Resource_TypeName_LeadOre => TextLib.ThaiConv("แร่|ตะกั่ว");
        public override string Resource_TypeName_Lead => TextLib.ThaiConv("ตะกั่ว");
        public override string Resource_TypeName_Bronze => TextLib.ThaiConv("ทอง|สัมฤทธิ์");
        public override string Resource_TypeName_BloomIron => TextLib.ThaiConv("เหล็ก|หลอม|ดิบ");
        public override string Resource_TypeName_Steel => TextLib.ThaiConv("เหล็ก|กล้า");
        public override string Resource_TypeName_CastIron => TextLib.ThaiConv("เหล็ก|หล่อ");

        public override string Resource_TypeName_BlackPowder => TextLib.ThaiConv("ดิน|ดำ");
        public override string Resource_TypeName_GunPowder => TextLib.ThaiConv("ดิน|ปืน");
        public override string Resource_TypeName_LedBullet => TextLib.ThaiConv("กระสุน|ตะกั่ว");

        public override string Resource_TypeName_HandCannon => TextLib.ThaiConv("ปืน|ใหญ่|มือ|ถือ");
        public override string Resource_TypeName_HandCulverin => TextLib.ThaiConv("ปืน|คัล|เวอริน|มือ");
        public override string Resource_TypeName_Rifle => TextLib.ThaiConv("ปืน|คาบ|ศิลา");
        public override string Resource_TypeName_Blunderbuss => TextLib.ThaiConv("ปืน|ปาก|แตร");

        public override string Resource_TypeName_Manuballista => TextLib.ThaiConv("หน้า|ไม้|ยักษ์|มือ|ถือ");
        public override string Resource_TypeName_Catapult => TextLib.ThaiConv("เครื่อง|ยิง|หิน");
        public override string Resource_TypeName_BatteringRam => TextLib.ThaiConv("เครื่อง|กระทุ้ง|ประตู");
        public override string Resource_TypeName_SiegeCannonBronze => TextLib.ThaiConv("ปืน|ใหญ่|บา|ซิ|ลิก");
        public override string Resource_TypeName_ManCannonBronze => TextLib.ThaiConv("ปืน|ใหญ่|บอม|บาร์ด");
        public override string Resource_TypeName_SiegeCannonIron => TextLib.ThaiConv("ปืน|ใหญ่|ฮาว|บิตซ์");
        public override string Resource_TypeName_ManCannonIron => TextLib.ThaiConv("ปืน|ใหญ่");

        public override string Resource_TypeName_PaddedArmor => TextLib.ThaiConv("ชุด|เกราะ|บุ|นวม");
        public override string Resource_TypeName_HeavyPaddedArmor => TextLib.ThaiConv("ชุด|เกราะ|บุ|นวม|หนา");

        public override string Resource_TypeName_IronArmor => TextLib.ThaiConv("เกราะ|โซ่|ถัก");
        public override string Resource_TypeName_HeavyIronArmor => TextLib.ThaiConv("เกราะ|โซ่|ถัก|หนัก");

        public override string Resource_TypeName_BronzeArmor => TextLib.ThaiConv("เกราะ|ทอง|สัมฤทธิ์");

        public override string Resource_TypeName_LightPlateArmor => TextLib.ThaiConv("เกราะ|แผ่น|เหล็ก");
        public override string Resource_TypeName_FullPlateArmor => TextLib.ThaiConv("เกราะ|แผ่น|เหล็ก|เต็ม|ตัว");
        public override string Resource_TypeName_MithrilArmor => TextLib.ThaiConv("เกราะ|มิทริล");
        public override string Resource_TypeName_Coin => TextLib.ThaiConv("เหรียญ");

        public override string UnitType_Warhammer => TextLib.ThaiConv("อัศวิน|ค้อน|ศึก");

        public override string UnitType_SpearAndShield => TextLib.ThaiConv("ทหาร|โล่|หอก");

        public override string UnitType_CollectionOfSoldiers => TextLib.ThaiConv("ชุด|ยูนิต|ทหาร");
        public override string UnitType_CollectionOfArmies => TextLib.ThaiConv("ชุด|กอง|ทัพ");

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => TextLib.ThaiConv("(ไอ|ดี {0})");

        public override string BuildHud_AreaEffectTitle => TextLib.ThaiConv("ผล|กระทบ|พื้นที่");
        public override string BuildHud_BonusRadius => TextLib.ThaiConv("รัศมี|โบนัส: {0}");

        public override string BuildHud_BuildTime => TextLib.ThaiConv("เวลา|ก่อ|สร้าง");
        public override string SchoolHud_ToLevel => TextLib.ThaiConv("ไป|ยัง|เลเวล");
        public override string SchoolHud_TimeDescription => TextLib.ThaiConv("เวลา|นี้|คำนวณ|จาก|ประสบการณ์|ศูนย์ |และ|จะ|ลด|ลง|ตาม|ประสบการณ์|ที่|เพิ่ม|ขึ้น");
        public override string SchoolHud_SelectSchool => TextLib.ThaiConv("เลือก|โรง|เรียน");
        public override string Upgrade_Order => TextLib.ThaiConv("ลำดับ|การ|อัปเกรด");

        public override string Building_ListDescription => TextLib.ThaiConv("รายการ|สิ่ง|ก่อ|สร้าง|ทั้งหมด|ใน|หมวด|นี้");

        public override string BuildingType_IsUpgraded => TextLib.ThaiConv("{0} - อัปเกรด|แล้ว");
        public override string BuildingType_WoodCutter => TextLib.ThaiConv("โรง|เลื่อย|ไม้");
        public override string BuildingType_Workshop_Description => TextLib.ThaiConv("ช่วย|ปรับปรุง|การ|ทำ|งาน|ใน|พื้นที่");

        public override string BuildingType_WoodCutter_AreaAffect => TextLib.ThaiConv("ได้รับ|ไม้|จาก|ต้น|ไม้|เพิ่ม|ขึ้น| {0}%");

        public override string BuildingType_StoneCutter_AreaAffect => TextLib.ThaiConv("ได้รับ|หิน|เพิ่ม|ขึ้น| {0}%");

        public override string BuildingType_StoneCutter => TextLib.ThaiConv("เหมือง|หิน");

        public override string BuildingType_Embassy => TextLib.ThaiConv("สถาน|ทูต");
        public override string BuildingType_Embassy_Description => TextLib.ThaiConv("สำหรับ|การ|เจรจา|ทางการ|ทูต");

        public override string BuildingType_SoldierBarracks => TextLib.ThaiConv("โรง|ทหาร|ราบ");
        public override string BuildingType_ArcherBarracks => TextLib.ThaiConv("โรง|ทหาร|ธนู");
        public override string BuildingType_WarmachineBarracks => TextLib.ThaiConv("โรง|ทหาร|เครื่อง|จักร|สงคราม");
        public override string BuildingType_GunBarracks => TextLib.ThaiConv("โรง|ทหาร|ปืน");
        public override string BuildingType_CannonBarracks => TextLib.ThaiConv("โรง|ทหาร|ปืน|ใหญ่");
        public override string BuildingType_KnightsBarracks => TextLib.ThaiConv("โรง|ทหาร|อัศวิน");

        public override string BuildingType_WaterResovoir => TextLib.ThaiConv("อ่าง|เก็บ|น้ำ");
        public override string BuildingType_WaterResovoir_Description => TextLib.ThaiConv("เพิ่ม|ความ|จุ|ใน|การ|เก็บ|น้ำ");

        public override string BuildingType_SmeltingFurnace => TextLib.ThaiConv("เตา|หลอม|แร่");
        public override string BuildingType_SmeltingFurnace_Description => TextLib.ThaiConv("สกัด|แร่|ให้|เป็น|โลหะ");

        public override string BuildingType_Foundry => TextLib.ThaiConv("โรง|หล่อ");
        public override string BuildingType_Foundry_Description => TextLib.ThaiConv("สถานี|สำหรับ|หล่อ|โลหะ");

        public override string BuildingType_Armory => TextLib.ThaiConv("คลัง|แสง");
        public override string BuildingType_Armory_Description => TextLib.ThaiConv("สถานี|คราฟต์|ชุด|เกราะ");
        public override string BuildingType_Chemist => TextLib.ThaiConv("โรง|เคมี");
        public override string BuildingType_Chemist_Description => TextLib.ThaiConv("สถานี|คราฟต์|สาร|เคมี");
        public override string BuildingType_CoinMaker => TextLib.ThaiConv("โรง|กษาปณ์");
        public override string BuildingType_CoinMaker_Description => TextLib.ThaiConv("เปลี่ยน|โลหะ|ให้|เป็น|เงิน|ตรา");
        public override string BuildingType_Gunmaker => TextLib.ThaiConv("ช่าง|ทำ|ปืน");
        public override string BuildingType_Gunmaker_Description => TextLib.ThaiConv("สถานี|คราฟต์|ปืน|และ|ปืน|ใหญ่");

        public override string BuildingType_School_Tab => TextLib.ThaiConv("โรง|เรียน");
        public override string BuildingType_School => TextLib.ThaiConv("สมาคม|ช่าง|ฝีมือ");
        public override string BuildingType_School_Description => TextLib.ThaiConv("เพิ่ม|ระดับ|ทักษะ|ให้|กับ|คน|งาน");

        public override string BuildingType_GoldDelivery => TextLib.ThaiConv("หน่วย|ขน|ส่ง|ทอง");
        public override string BuildingType_Bank_Description => TextLib.ThaiConv("การ|จัด|การ|คลัง|ทอง");

        public override string DecorType_CobbleStones => TextLib.ThaiConv("พื้น|หิน|กรวด");
        public override string DecorType_Square => TextLib.ThaiConv("ลาน|เมือง");

        public override string DecorType_Garden => TextLib.ThaiConv("สวน");
        public override string DecorType_Flag => TextLib.ThaiConv("ธง");
        public override string DecorType_Banner => TextLib.ThaiConv("ป้าย|แบนเนอร์");

        public override string BuildingType_DirtRoad => TextLib.ThaiConv("ถนน|ดิน");
        public override string BuildingType_Palisade => TextLib.ThaiConv("ป้อม|ปราการ|ไม้|ระเนียด");

        public override string ResourceType_ServiceMen => TextLib.ThaiConv("หน่วย|บริการ");
        public override string BuildingType_ServiceHouse => TextLib.ThaiConv("สถาน|บริการ");
        public override string BuildingType_ServiceHouse_DescriptionAddX => TextLib.ThaiConv("เพิ่ม|หน่วย|บริการ| {0} |คน");

        public override string BuildingType_GuardOffice => TextLib.ThaiConv("สำ|นัก|งาน|ยาม");
        public override string BuildingType_GuardOffice_DescriptionAddX => TextLib.ThaiConv("เพิ่ม|ขีด|จำกัด|ทหาร|ยาม|อีก| {0} |คน");

        public override string BuildingType_DirtWall => TextLib.ThaiConv("กำแพง|ดิน");
        public override string BuildingType_DirtTower => TextLib.ThaiConv("หอ|คอย|ดิน");
        public override string BuildingType_WoodWall => TextLib.ThaiConv("กำแพง|ไม้");
        public override string BuildingType_WoodTower => TextLib.ThaiConv("หอ|คอย|ไม้");
        public override string BuildingType_StoneWall => TextLib.ThaiConv("กำแพง|หิน");
        public override string BuildingType_StoneTower => TextLib.ThaiConv("หอ|คอย|หิน");
        public override string BuildingType_StoneGate => TextLib.ThaiConv("ประตู|หิน");
        public override string BuildingType_StoneHouse => TextLib.ThaiConv("โรง|หิน");

        /// <summary>
        /// เมื่อแสดงรายการที่มีความหลากหลายเล็กน้อย เช่น "โคมไฟ A" และ "โคมไฟ B"
        /// </summary>
        public override string VariantType_A => TextLib.ThaiConv("{0} | แบบ | A");
        public override string VariantType_B => TextLib.ThaiConv("{0} | แบบ | B");
        public override string VariantType_C => TextLib.ThaiConv("{0} | แบบ | C");
        public override string VariantType_D => TextLib.ThaiConv("{0} | แบบ | D");
        public override string VariantType_E => TextLib.ThaiConv("{0} | แบบ | E");
        public override string VariantType_F => TextLib.ThaiConv("{0} | แบบ | F");
        public override string VariantType_G => TextLib.ThaiConv("{0} | แบบ | G");
        public override string VariantType_H => TextLib.ThaiConv("{0} | แบบ | H");

        public override string BuildingToolShape_Free => TextLib.ThaiConv("วาด|อิสระ");
        public override string BuildingToolShape_Area => TextLib.ThaiConv("สี่เหลี่ยม");
        public override string BuildingToolShape_Line => TextLib.ThaiConv("เส้น|ตรง");
        public override string BuildingToolShape_LShape => TextLib.ThaiConv("รูป|ตัว|แอล");


        public override string CityHall_Upgrade => TextLib.ThaiConv("อัปเกรด|ศาลา|กลาง|เมือง");

        /// <summary>
        /// ขีดจำกัดจำนวนคนงานที่เมืองสามารถรองรับได้
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => TextLib.ThaiConv("คน|งาน|สูงสุด|ที่|รองรับ: {0}");

        public override string CityHall_Size_Small => TextLib.ThaiConv("หมู่บ้าน");
        public override string CityHall_Size_Medium => TextLib.ThaiConv("เมือง");
        public override string CityHall_Size_Large => TextLib.ThaiConv("เมือง|หลวง");

        public override string GuardHousingCount => TextLib.ThaiConv("ที่|พัก|หน่วย|ยาม");
        public override string ServicemenCount => TextLib.ThaiConv("หน่วย|บริการ: {0}");


        public override string Work_MiningResource => TextLib.ThaiConv("กำลัง|ทำ|เหมือง | {0}");

        public override string MenuTab_Progress => TextLib.ThaiConv("ความ|คืบหน้า");

        public override string Automation_AutomateCity => TextLib.ThaiConv("จัดการ|เมือง|อัตโนมัติ");
        public override string Automation_AutomationFocus => TextLib.ThaiConv("จุด|เน้น|ระบบ|อัตโนมัติ");
        public override string Automation_AutomationFocus_Grow => TextLib.ThaiConv("เน้น|เติบโต");
        public override string Automation_AutomationFocus_Export => TextLib.ThaiConv("เน้น|ส่ง|ออก");
        public override string Automation_AutomationFocus_War => TextLib.ThaiConv("เน้น|สงคราม");

        public override string CityCulture_Smelters_Description => TextLib.ThaiConv("ปรับปรุง|การ|หลอม|แร่|ให้|ดี|ขึ้น");
        public override string CityCulture_Smelters => TextLib.ThaiConv("ช่าง|หลอม");

        public override string CityCulture_Apprentices_Description => TextLib.ThaiConv("คน|งาน|ใหม่|จะ|ได้รับ|ประสบการณ์|จาก|คน|งาน|ที่|เชี่ยวชาญ");
        public override string CityCulture_Apprentices => TextLib.ThaiConv("ศิษย์|ฝึก|งาน");

        public override string CityCulture_BronzeCasters_Description => TextLib.ThaiConv("ปรับปรุง|การ|ผลิต|ทอง|สัมฤทธิ์|และ|ไอเทม|ทอง|สัมฤทธิ์");
        public override string CityCulture_BronzeCasters => TextLib.ThaiConv("ช่าง|หล่อ|ทอง|สัมฤทธิ์");

        //DEMO PATCH 1

        /// <summary>
        /// ออร์คที่ดุร้ายซึ่งเดินเตร่อยู่ในแผนที่
        /// </summary>
        public override string FactionName_Barbarian => TextLib.ThaiConv("ทัพ|อสูร|ทมิฬ");
        public override string Tutorial_AttackAndDestroyX => TextLib.ThaiConv("โจมตี|และ|ทำลาย: {0}");
        public override string Resource_TypeName_Pike => TextLib.ThaiConv("หอก|ยาว | (Pike)");


        public override string BattleTrials_Title => TextLib.ThaiConv("บท|ทดสอบ|การ|รบ");
        public override string BattleTrials_Description => TextLib.ThaiConv("ทดสอบ|กลยุทธ์|ของ|คุณ|ใน|การ|ประจัญ|บาน|ระหว่าง|กอง|ทัพ");


        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => TextLib.ThaiConv("การ|โจมตี|นี้|จะ|ลด|โอกาส|บล็อก");

        public override string Conscript_BlockPerSecond => TextLib.ThaiConv("อาจ|บล็อก|ได้| {0} |ครั้ง|ต่อ|วินาที");

        public override string Conscript_BlockDescription => TextLib.ThaiConv("ทหาร|จะ|บล็อก|การ|โจมตี|ส่วน|ใหญ่|ที่|เข้า|มา|จาก|ทาง|ด้าน|หน้า");

        public override string Map_CustomSeed => TextLib.ThaiConv("รหัส|แผนที่ | (Seed)");

        public override string Settings_Mode_Spectator => TextLib.ThaiConv("โหมด|ผู้|ชม");

        //public override string Settings_Mode_Spectator_Description => TextLib.ThaiConv("ดู|เพียง|อย่าง|เดียว");

        public override string Automation_AutomationFocus_NoFocus_Description => TextLib.ThaiConv("จะ|สร้าง|ทุก|อย่าง|อย่าง|ละ|นิด|อย่าง|ละ|หน่อย");

        public override string Automation_AutomationFocus_WillProduce => TextLib.ThaiConv("จะ|เน้น|ผลิต:");

        public override string Help_Food_WhoEats => TextLib.ThaiConv("ทหาร|และ|คน|งาน|ทุก|คน|ต้อง|กิน|อาหาร");

        public override string Help_Food_BigArmy => TextLib.ThaiConv("กอง|ทัพ|ขนาด|ใหญ่|อาจ|ทำให้|เมือง|ใน|พื้นที่|ขาด|แคลน|เสบียง|ได้");

        public override string Help_Food_DontBuild => TextLib.ThaiConv("การ|สร้าง|ฟาร์ม|เพิ่ม|ไม่ได้|หมาย|ความ|ว่า|อาหาร|จะ|เพิ่ม|ขึ้น|ทันที |คุณ|จำเป็น|ต้อง|มี|คน|งาน|และ|โรง|ครัว|เพื่อ|เก็บ|เกี่ยว|และ|แปรรูป|มัน");

        public override string Help_Food_UseWater => TextLib.ThaiConv("การ|ผลิต|อาหาร|จำเป็น|ต้อง|ใช้|น้ำ");

        public override string Help_Food_Postal => TextLib.ThaiConv("ตรวจสอบ|ให้|แน่|ใจ|ว่า|เมือง|ของ|คุณ|ช่วย|เหลือ|กัน|โดย|การ|ส่ง|เสบียง|ให้|กัน");

        public override string Message_LostCity => TextLib.ThaiConv("เสีย|เมือง");

        public override string Demo_Description => TextLib.ThaiConv("สถานการณ์|สั้น: ป้องกัน|เมือง|ของ|คุณ|เป็น|เวลา| {0} |นาที");


        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => TextLib.ThaiConv("เดโม|จะ|จบ|ลง|ใน|อีก| {0} |นาที");

        public override string Experience_Required => TextLib.ThaiConv("ประสบการณ์|ที่|ต้องการ");

        public override string InputActionName_ToggleMenu => TextLib.ThaiConv("เปิด/ปิด | เมนู");

        //DEMO PATCH 4
        public override string Work_BadValueDescription => TextLib.ThaiConv("ทรัพยากร|อาจ|ลด|ลง|ต่ำ|กว่า|ศูนย์|หรือ|เกิน|ขีด|จำกัด|คลัง|ได้|เล็ก|น้อย |โดย|จะ|มีการ|บังคับ|ใช้|ขอบเขต|เมื่อ|มีการ|สร้าง|คิว|งาน|เท่านั้น");

        public override string Work_SelectCategory => TextLib.ThaiConv("เลือก|หมวด|หมู่|ไอเทม");
        public override string Hud_RemoveFromList => TextLib.ThaiConv("นำ|ออก|จาก|รายการ");

        public override string Hud_ReturnToPrevious => TextLib.ThaiConv("ย้อน|กลับ");
        public override string Hud_Close => TextLib.ThaiConv("ปิด");

        public override string Hud_Low => TextLib.ThaiConv("ต่ำ");
        public override string Hud_Medium => TextLib.ThaiConv("กลาง");
        public override string Hud_High => TextLib.ThaiConv("สูง");

        public override string Hud_Copy => TextLib.ThaiConv("คัด|ลอก");
        //public override string Hud_Paste => TextLib.ThaiConv("วาง");
        public override string Hud_Cut => TextLib.ThaiConv("ตัด");
        public override string Hud_SaveCompleted => TextLib.ThaiConv("บันทึก|เสร็จ|สมบูรณ์");

        public override string Settings_WaterMultiplier => TextLib.ThaiConv("ตัว|คูณ|ปริมาณ|น้ำ");
        public override string Settings_WaterMultiplier_Description => TextLib.ThaiConv("กำหนด|ปริมาณ|น้ำ|ที่|เมือง|ผลิต|และ|จัด|เก็บ |ค่า|ที่|สูง|เกิน|ไป|อาจ|ส่ง|ผล|ต่อ|ประสิทธิภาพ|ของ|เครื่อง");

        public override string Settings_ChildMultiplier => TextLib.ThaiConv("ตัว|คูณ|อัตรา|การ|เกิด");

        public override string Settings_CraftMultiplier_Description => TextLib.ThaiConv("ค่า|ที่|น้อย|ลง|จะ|ทำให้|ผลิต|ได้|เร็ว|ขึ้น");

        public override string FastProduction => TextLib.ThaiConv("ผลิต|เร็ว");
        public override string SlowProduction => TextLib.ThaiConv("ผลิต|ช้า");

        /// <summary>
        /// ป้ายกำกับสำหรับรายการไอเทมที่ถูกระงับการผลิต
        /// </summary>
        public override string BlocksProduction => TextLib.ThaiConv("จะไม่|ผลิต");

        //public override string CityAutomation_WaitForMaxPopulation => TextLib.ThaiConv("รอ|ให้|ประชากร|เต็ม");
        public override string Automation_AutomationFocus_NoFocus => TextLib.ThaiConv("ทั้งหมด");
        public override string CityAutomation_SoldierQuality => TextLib.ThaiConv("คุณภาพ|ทหาร");
        public override string CityAutomation_SoldierWeaponType => TextLib.ThaiConv("ประเภท|อาวุธ");

        public override string WarsResourceGroup_Resources => TextLib.ThaiConv("ทรัพยากร");
        public override string WarsResourceGroup_Weapons => TextLib.ThaiConv("อาวุธ");

        public override string WarsResourceGroup_AllWeaponTypes => TextLib.ThaiConv("คละ|ประเภท");
        public override string WarsResourceGroup_MeleeHandWeapons => TextLib.ThaiConv("อาวุธ|ประชิด");
        public override string WarsResourceGroup_RangedHandWeapons => TextLib.ThaiConv("อาวุธ|ระยะ|ไกล");
        public override string WarsResourceGroup_Warmachines => TextLib.ThaiConv("เครื่อง|จักร|สงคราม");

        public override string FactionSettings_Titel => TextLib.ThaiConv("การ|ตั้ง|ค่า|ทั้ง|ฝ่าย");
        public override string FactionSettings_Description => TextLib.ThaiConv("มี|ผล|กับ|ทุก|เมือง|ของ|ท่าน");

        public override string Conscript_MaxPopulation => TextLib.ThaiConv("ประชากร|สูงสุด");
        public override string Conscript_MaxPopulation_Description => TextLib.ThaiConv("จะ|เกณฑ์|ทหาร|เมื่อ|ประชากร|เต็ม|เท่านั้น");

        public override string Conscript_FoodAbundance => TextLib.ThaiConv("คลัง|เสบียง|เต็ม");
        public override string Conscript_FoodAbundance_Description => TextLib.ThaiConv("จะ|เกณฑ์|ทหาร|เมื่อ|เสบียง|ถึง|ระดับ|สูงสุด|ของ|คลัง|เท่านั้น");

        /// <summary>
        /// การตั้งค่าทั่วไปจะไล่ดูรายการไอเทมทั้งหมดและใช้กับทุกไอเทม (ในกล่องติ๊กเลือก)
        /// </summary>
        public override string GeneralSetting_On => TextLib.ThaiConv("ตั้ง|ค่า: เปิด");
        public override string GeneralSetting_Off => TextLib.ThaiConv("ตั้ง|ค่า: ปิด");
        public override string GeneralSetting_AllBuildingsDescription => TextLib.ThaiConv("มี|ผล|กับ|สิ่ง|ก่อ|สร้าง|ทั้งหมด");

        public override string GeneralSetting_ApplyMessage => TextLib.ThaiConv("เปลี่ยน|การ|ตั้ง|ค่า|ให้|กับ|สิ่ง|ก่อ|สร้าง | {0} | แห่ง|แล้ว");

        public override string MustTurnOffSteamInput => TextLib.ThaiConv("หาก|ต้องการ|ใช้|คอนโทรลเลอร์ | คุณ|ต้อง|ปิด| Steam | Input | ก่อน");

        public override string Technology_GainTitle => TextLib.ThaiConv("ช่องทาง|รับ|เทคโนโลยี");
        public override string Technology_LevelUp => TextLib.ThaiConv("เลเวล|เพิ่ม");
        public override string Technology_ForEachLevelUp => TextLib.ThaiConv("เมื่อ|คน|งาน|เลเวล|เพิ่ม|ใน|สาย|เทคโนโลยี: {0}");

        public override string VoxelEditor_Description => TextLib.ThaiConv("สร้าง|โมเดล|แบบ|บล็อก");

        public override string Editor_Tool => TextLib.ThaiConv("เครื่อง|มือ");
        public override string Editor_SelectOptionsMenu => TextLib.ThaiConv("ตัว|เลือก|การ|เลือก");
        public override string Editor_Continous => TextLib.ThaiConv("ต่อเนื่อง");
        public override string Editor_Tool_PencilSize => TextLib.ThaiConv("ขนาด|ดินสอ");
        public override string Editor_Tool_SizeTolerance => TextLib.ThaiConv("ความ|ละเอียด|ของ|ขนาด");
        public override string Editor_Tool_RoundPencil => TextLib.ThaiConv("ดินสอ|หัว|กลม");
        public override string Editor_Tool_EdgeSize => TextLib.ThaiConv("ขนาด|ขอบ");
        public override string Editor_Tool_PercentFill => TextLib.ThaiConv("เปอร์เซ็นต์|การ|เติม");
        public override string Editor_Tool_ClearAbove => TextLib.ThaiConv("ลบ|ด้าน|บน");
        public override string Editor_Tool_FillBelow => TextLib.ThaiConv("เติม|ด้าน|ล่าง");
        public override string Editor_UserModels => TextLib.ThaiConv("โมเดล|ของ|ผู้|ใช้");
        public override string Editor_UserModels_Description => TextLib.ThaiConv("เรียก|ดู|โมเดล|ที่|คุณ|บันทึก|ไว้");

        public override string Editor_RetailModels => TextLib.ThaiConv("โมเดล|ใน|เกม");
        public override string Editor_RetailModels_Description => TextLib.ThaiConv("โหลด|โมเดล|จาก|ตัว|เกม");

        public override string Editor_ModTemplates => TextLib.ThaiConv("แม่|แบบ|สำหรับ|ม็อด");
        public override string Editor_ExportAsOBJ => TextLib.ThaiConv("ส่ง|ออก|เป็น | .OBJ");
        public override string Editor_SelectAll => TextLib.ThaiConv("เลือก|ทั้งหมด");

        public override string Editor_Canvas_Title => TextLib.ThaiConv("พื้นที่|งาน");
        public override string Editor_Canvas_Size => TextLib.ThaiConv("ขนาด");
        public override string Editor_Canvas_Dimension_X => TextLib.ThaiConv("แกน X");
        public override string Editor_Canvas_Dimension_Y => TextLib.ThaiConv("แกน Y");
        public override string Editor_Canvas_Dimension_Z => TextLib.ThaiConv("แกน Z");
        public override string Editor_Canvas_SizePresets => TextLib.ThaiConv("ขนาด|ที่|ตั้ง|ไว้");
        public override string Editor_Canvas_Move => TextLib.ThaiConv("ย้าย");
        public override string Editor_Canvas_Move_Up => TextLib.ThaiConv("ขึ้น");
        public override string Editor_Canvas_Move_Down => TextLib.ThaiConv("ลง");
        public override string Editor_Canvas_RotateClockwise => TextLib.ThaiConv("หมุน|ตาม|เข็ม|นาฬิกา");
        public override string Editor_Canvas_RotateCounterClockwise => TextLib.ThaiConv("หมุน|ทวน|เข็ม|นาฬิกา");
        public override string Editor_Canvas_Mirror => TextLib.ThaiConv("สะท้อน|เงา");

        public override string Editor_Canvas_RotateFlip_Title => TextLib.ThaiConv("หมุน/พลิก");
        public override string Editor_Canvas_FlipVertical => TextLib.ThaiConv("พลิก|บน-ล่าง");
        public override string Editor_Canvas_FlipOrientation => TextLib.ThaiConv("พลิก|แนว|ตั้ง-นอน");
        public override string Editor_Canvas_ClearAll_Description => TextLib.ThaiConv("ลบ|บล็อก|และ|เฟรม|ทั้งหมด");

        public override string Editor_Animation => TextLib.ThaiConv("แอนิเมชัน");
        public override string Editor_Animation_RemoveCurrentFrame => TextLib.ThaiConv("ลบ|เฟรม|ปัจจุบัน");
        public override string Editor_Animation_AddFrameCopy => TextLib.ThaiConv("เพิ่ม|เฟรม|โดย|คัด|ลอก");
        public override string Editor_Animation_AddEmptyFrame => TextLib.ThaiConv("เพิ่ม|เฟรม|ว่าง");
        public override string Editor_Animation_MoveDescription => TextLib.ThaiConv("เปลี่ยน|ตำแหน่ง|เฟรม");
        public override string Editor_Animation_AllFrames => TextLib.ThaiConv("ทุก|เฟรม");
        public override string Editor_Animation_AllFrames_ActionDescription => TextLib.ThaiConv("ทำ|สิ่ง|เดียวกัน|นี้|กับ|ทุก|เฟรม");

        public override string Editor_SettingsMenu => TextLib.ThaiConv("ตั้ง|ค่า");
        public override string Hud_Exit => TextLib.ThaiConv("ออก");
        public override string Editor_Canvas_Clear => TextLib.ThaiConv("ล้าง|พื้นที่");

        public override string Editor_Stamp => TextLib.ThaiConv("ประทับ|ตรา");
        public override string Editor_StampOtherFrames => TextLib.ThaiConv("ประทับ|ลง|ใน|เฟรม|อื่น");
        public override string Editor_StampOtherFrames_Description => TextLib.ThaiConv("วาง|บล็อก|ลง|ใน|เฟรม|เหล่านี้");
        public override string Editor_PasteToFrame => TextLib.ThaiConv("วาง|บล็อก|ลง|ใน|เฟรม|นี้");
        public override string Editor_ClearAllFrames => TextLib.ThaiConv("ล้าง|ทุก|เฟรม");
        public override string Editor_ClearOtherFrames => TextLib.ThaiConv("ล้าง|เฟรม|อื่น");

        public override string Editor_Settings_MoveSpeed => TextLib.ThaiConv("ความ|เร็ว|การ|เคลื่อนที่");
        public override string Editor_Settings_BackgroundColor => TextLib.ThaiConv("สี|พื้น|หลัง");
        public override string Editor_Settings_HideHUD => TextLib.ThaiConv("ซ่อน | HUD");

        public override string Editor_Color => TextLib.ThaiConv("สี");
        public override string Editor_ColorsInUseLabel => TextLib.ThaiConv("สี|ที่|ใช้|อยู่");
        public override string Editor_Color_BrighterPlus => TextLib.ThaiConv("สว่าง|ขึ้น | +");
        public override string Editor_Color_Brighter => TextLib.ThaiConv("สว่าง|ขึ้น");
        public override string Editor_Color_Darker => TextLib.ThaiConv("เข้ม|ลง");
        public override string Editor_Color_DarkerPlus => TextLib.ThaiConv("เข้ม|ลง | +");
        public override string Editor_Color_RedTint => TextLib.ThaiConv("ย้อม|แดง");
        public override string Editor_Color_Tint => TextLib.ThaiConv("ย้อม|สี");
        public override string Editor_Color_GreenTint => TextLib.ThaiConv("ย้อม|เขียว");
        public override string Editor_Color_BlueTint => TextLib.ThaiConv("ย้อม|น้ำ|เงิน");
        public override string Editor_Color_YellowTint => TextLib.ThaiConv("ย้อม|เหลือง");
        public override string Editor_Color_PurpleTint => TextLib.ThaiConv("ย้อม|ม่วง");
        public override string Editor_NoColor => TextLib.ThaiConv("ว่าง|เปล่า");

        public override string Editor_Material => TextLib.ThaiConv("วัสดุ");

        /// <summary>
        /// ผู้ใช้อาจเปลี่ยนสีหนึ่งเป็นอีกสีหนึ่งทั่วทั้งโมเดล
        /// </summary>
        public override string Editor_Color_Recolor => TextLib.ThaiConv("เปลี่ยน|สี");
        public override string Editor_Color_RecolorTo => TextLib.ThaiConv("เปลี่ยน|สี|เป็น");

        public override string Editor_Material_Set => TextLib.ThaiConv("กำหนด|วัสดุ");

        public override string Editor_Preview => TextLib.ThaiConv("ตัว|อย่าง");
        public override string Editor_CombineWithCurrent => TextLib.ThaiConv("รวม|กับ|โมเดล|ปัจจุบัน");

        public override string Editor_PickedColor => TextLib.ThaiConv("เลือก|แล้ว");
        public override string Editor_ColorRGBvalues => TextLib.ThaiConv("R:{0} G:{1} B:{2}");

        public override string BuildingType_ImmigrationTent => TextLib.ThaiConv("เต็นท์|ผู้|อพยพ");
        public override string BuildingType_ImmigrationTent_Description => TextLib.ThaiConv("รองรับ|ผู้|อพยพ|ได้ | {0} | คน");
        public override string BuildingType_ReseachCenter => TextLib.ThaiConv("ศูนย์|วิจัย");
        public override string BuildingType_Bookpress => TextLib.ThaiConv("เครื่อง|พิมพ์|หนังสือ");
        public override string BuildingType_Bookpress_Description => TextLib.ThaiConv("ใน|สาย|วิจัย|หนึ่ง |แต้ม|ที่|ได้รับ|จะ|ถูก|แบ่ง|ปัน|ให้|กับ | {0} | ทั้งหมด|ใน|เมือง|อื่น|ของ|ท่าน");

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => TextLib.ThaiConv("ตัวอย่าง: เมื่อ|คน|งาน|ผลิต| {0}, |จะ|เพิ่ม|ทักษะ|ด้าน| {1} |ของ|พวกเขา. |เมื่อ|เลเวล|เพิ่ม|ขึ้น |จะ|ช่วย|เพิ่ม|แต้ม|วิจัย|ให้|กับ|เทคโนโลยี| {2} |เนื่องจาก|ใช้|สาย|งาน| {1} |ร่วม|กัน");

        public override string BuildingType_Research_BaseDescription => TextLib.ThaiConv("เพิ่ม|ความ|เร็ว|ใน|การ|วิจัย|เทคโนโลยี");

        public override string BuildingType_ResearchCenter_Description => TextLib.ThaiConv("เพิ่ม|แต้ม|วิจัย|เทคโนโลยี|พิเศษ| {0} |แต้ม |เมื่อ|คน|งาน|เลเวล|เพิ่ม|ใน|สาย|งาน|เดียวกัน");

        //DEMO PATCH 5

        public override string Editor_CropSelection => TextLib.ThaiConv("ตัด|ตาม|ส่วน|ที่|เลือก");

        public override string Immigrants_DisbandedSoldiers => TextLib.ThaiConv("ทหาร|ที่|ปลด|ประจำ|การ|จะ|กลาย|เป็น|ผู้|อพยพ");
        public override string Immigrants_RefillWorkers => TextLib.ThaiConv("เติม|จำนวน|แรง|งาน|อย่าง|รวดเร็ว");
        public override string Immigrants_UnhousedAreLost => TextLib.ThaiConv("ผู้|อพยพ|ที่|ไม่|มี|บ้าน|พัก|จะ|หาย|ไป|เมื่อ|เวลา|ผ่าน|ไป|สัก|พัก");
        public override string Editor_VoxelCount => TextLib.ThaiConv("{0} | ว็อก|เซล");

        public override string Editor_Layers_Titel => TextLib.ThaiConv("เลเยอร์");
        public override string Editor_Layers_All => TextLib.ThaiConv("ทุก|เลเยอร์");
        public override string Editor_LayerNumber => TextLib.ThaiConv("เลเยอร์ | {0}");

        public override string Editor_Layer_AddEmpty => TextLib.ThaiConv("เพิ่ม|เลเยอร์|ว่าง");
        public override string Editor_Layer_AddCopy => TextLib.ThaiConv("คัด|ลอก|เลเยอร์");
        public override string Editor_Layer_Remove => TextLib.ThaiConv("ลบ|เลเยอร์");
        public override string Editor_Layer_MergeDown => TextLib.ThaiConv("รวม|กับ|เลเยอร์|ด้าน|ล่าง");
        public override string Editor_IsAnimated => TextLib.ThaiConv("มี|ภาพ|เคลื่อนไหว");
        public override string Editor_ToggleVisible => TextLib.ThaiConv("เปิด/ปิด|การ|มอง|เห็น");
        public override string Editor_ToggleAnimatedLayer => TextLib.ThaiConv("เปิด/ปิด|เลเยอร์|เคลื่อนไหว");
        public override string Editor_Projects => TextLib.ThaiConv("ไฟล์|โปรเจกต์");
        public override string ProfileEditor_ReplaceMaterial => TextLib.ThaiConv("สี|โปรไฟล์: {0}");

        public override string ProfileEditor_ProfileColors_Label => TextLib.ThaiConv("สี|โปรไฟล์");
        public override string ProfileEditor_TunicColor => TextLib.ThaiConv("สี|เสื้อ|นอก");
        public override string ProfileEditor_PantsColor => TextLib.ThaiConv("สี|กางเกง");
        public override string ProfileEditor_LeaderColor => TextLib.ThaiConv("สี|ของ|ผู้นำ");

        public override string MapStartAs_Water => TextLib.ThaiConv("น้ำ");
        public override string MapStartAs_Land => TextLib.ThaiConv("พื้นดิน");
        public override string MapStartAs_Circle => TextLib.ThaiConv("วงกลม");

        public override string Hud_NeedToBeAssigned => TextLib.ThaiConv("รอ|การ|มอบ|หมาย");
        public override string Hud_CommitAssignment => TextLib.ThaiConv("มอบ|หมาย");
        public override string Technology_NoAvailableResearch => TextLib.ThaiConv("ไม่|มี|หัวข้อ|วิจัย");

        public override string Research_Tab => TextLib.ThaiConv("วิจัย");

        //5.2
        public override string BuildCategory_General => TextLib.ThaiConv("ทั่วไป");
        public override string BuildCategory_Military => TextLib.ThaiConv("การ|ทหาร");
        public override string BuildCategory_Decoration => TextLib.ThaiConv("ตก|แต่ง");
        public override string BuildCategory_Upgrade => TextLib.ThaiConv("อัปเกรด");
        public override string Work_NoMines => TextLib.ThaiConv("ไม่|มี|เหมือง");

        //NEXT FEST DEMO
        public override string HUD_DisplayName => TextLib.ThaiConv("ชื่อ|ที่|แสดง");
        public override string HUD_Filter => TextLib.ThaiConv("ตัว|กรอง");
        public override string HUD_Scale => TextLib.ThaiConv("ปรับ|ขนาด");
        public override string HUD_Tags => TextLib.ThaiConv("แท็ก");
        public override string HUD_ClickToCancel => TextLib.ThaiConv("คลิก|เพื่อ|ยกเลิก");

        public override string ObjectTag_Description => TextLib.ThaiConv("เพิ่ม|สัญลักษณ์|บน|แผนที่");
        public override string HudPins => TextLib.ThaiConv("หมุด | HUD");
        public override string HudPins_Description => TextLib.ThaiConv("ปัก|ข้อมูล|ไว้|บน|หน้าจอ");

        public override string Lobby_PlayerProfileNumbered => TextLib.ThaiConv("โปรไฟล์ | {0}");
        public override string Lobby_CharacterCreationNumbered => TextLib.ThaiConv("ตัว|ละคร | {0}");
        public override string Lobby_PlayerProfileEdit => TextLib.ThaiConv("แก้ไข|โปรไฟล์|ผู้|เล่น");

        public override string Editor_ConvertAnimationToLayers => TextLib.ThaiConv("เปลี่ยน|แอนิเมชัน|เป็น|เลเยอร์");
        public override string Editor_StampAllFrames => TextLib.ThaiConv("ประทับ|ตรา|ทุก|เฟรม");

        public override string Editor_DisplayOptions => TextLib.ThaiConv("ตัว|เลือก|การ|แสดง|ผล");
        public override string Editor_CharacterCreator => TextLib.ThaiConv("เครื่อง|มือ|สร้าง|ตัว|ละคร");
        public override string Editor_CharacterCreator_Description => TextLib.ThaiConv("แก้ไข|รูปลักษณ์|ยูนิต|ทหาร");
        public override string Editor_HatGenre => TextLib.ThaiConv("โหมด|การ|แสดง|หมวก");
        public override string Editor_HatGenre_FollowWeapon => TextLib.ThaiConv("ตาม|อาวุธ");
        public override string Editor_HatGenre_Uniform => TextLib.ThaiConv("ชุด|แบบ|ฟอร์ม");
        public override string Editor_CopyPasteSelectedColor => TextLib.ThaiConv("คัด|ลอก|จาก|สี|ที่|เลือก");

        public override string Character_Accessories => TextLib.ThaiConv("เครื่อง|ประดับ");
        public override string Character_Hat => TextLib.ThaiConv("หมวก");
        public override string Character_Head => TextLib.ThaiConv("ส่วน|หัว");
        public override string Character_Body => TextLib.ThaiConv("ร่างกาย");
        public override string Character_Arms => TextLib.ThaiConv("แขน");
        public override string Character_Back => TextLib.ThaiConv("หลัง");
        public override string Character_Face => TextLib.ThaiConv("ใบ|หน้า");

        public override string BuildingType_Tavern => TextLib.ThaiConv("โรง|เลี้ยง|ส่วน|กลาง");

        public override string Settings_CraftMultiplier => TextLib.ThaiConv("ตัว|คูณ|เวลา|คราฟต์");
        public override string Settings_ChildMultiplier_Description => TextLib.ThaiConv("เพิ่ม|ความ|เร็ว|ใน|การ|รับ|คน|งาน|ใหม่");

        public override string Settings_CasualControls => TextLib.ThaiConv("การ|ควบคุม|แบบ|เล่น|ง่าย");
        public override string Settings_CasualControls_Description => TextLib.ThaiConv("เน้น|การ|ตัดสิน|ใจ|หลัก |และ|ใช้|เพียง|ทอง|เป็น|ทรัพยากร|เท่านั้น");

        public override string Settings_AdvancedControls => TextLib.ThaiConv("การ|ควบคุม|ขั้น|สูง");
        public override string Settings_AdvancedControls_Description => TextLib.ThaiConv("ระบบ|การ|จัดการ|ทรัพยากร|แบบ|เต็ม|รูปแบบ");

        public override string WarsResourceGroup_Metal => TextLib.ThaiConv("โลหะ");
        public override string Work_Craft => TextLib.ThaiConv("คราฟต์");
        public override string Work_OnlyCraftOnFullStock => TextLib.ThaiConv("คราฟต์|เมื่อ|คลัง|สินค้า|เต็ม|เท่านั้น");

        public override string ExperienceType_Smelting => TextLib.ThaiConv("การ|หลอม|โลหะ");
        public override string Category_Optimize => TextLib.ThaiConv("ปรับ|ประสิทธิภาพ");
        public override string BuildCategory_Road => TextLib.ThaiConv("ถนน");
        public override string XP_UnlockBuildPrio => TextLib.ThaiConv("ปลด|ล็อก|ลำดับ|ความ|สำคัญ|การ|สร้าง: {0}");
        public override string Technology_ModernFarming => TextLib.ThaiConv("เกษตรกรรม|สมัย|ใหม่");

        public override string ExportImportDescription => TextLib.ThaiConv("สำหรับ|แบ่ง|ปัน|ไฟล์|เซฟ|ให้|ผู้|เล่น|คน|อื่น |ไฟล์|ทั้งหมด|อยู่|ใน|โฟลเดอร์|นี้: {0}");

        public override string CityCultureDescription => TextLib.ThaiConv("วัฒนธรรม|จะ|มอบ|โบนัส|พิเศษ|ให้|แก่|เมือง");

        public override string UnitType_CloseRangeRifle => TextLib.ThaiConv("พล|ปืน|คาบ|ศิลา|ระยะ|ใกล้");
        public override string UnitType_LongRangeRifle => TextLib.ThaiConv("พล|ปืน|คาบ|ศิลา|ระยะ|ไกล");
        public override string UnitType_Skirmisher => TextLib.ThaiConv("หน่วย|รบ|กวน");

        //From lumen (light)
        public override string UnitType_MithrilArcher => TextLib.ThaiConv("พล|ธนู|ลูนารี");
        public override string UnitType_MithrilSwordsman => TextLib.ThaiConv("อัศวิน|ลูนารี");

        public override string Defence_AutoAssign_Towers => TextLib.ThaiConv("มอบ|หมาย|ทหาร|ประจำ|หอ|คอย");

        public override string EventMessage_DesertersText_Food => TextLib.ThaiConv("ทหาร|ที่|หิว|โหย|กำลัง|หนี|ทัพ|ไป|จาก|กอง|ทัพ|ของ|ท่าน");

        public override string Tutorial_CasualRecruitSoldiers => TextLib.ThaiConv("ซื้อ|กลุ่ม|ทหาร| 1 |กลุ่ม");


        //Shadow update
        public override string Technology_CannotReassign => TextLib.ThaiConv("ไม่|สามารถ|เปลี่ยน|วิจัย|ได้|จน|กว่า|จะ|เสร็จ|สมบูรณ์");
        public override string Diplomacy_DeclareWarAgainst => TextLib.ThaiConv("ท่าน|กำลัง|จะ|ประกาศ|สงคราม|กับ");
        public override string Diplomacy_AllyCount => TextLib.ThaiConv("จำนวน|พันธมิตร");
        public override string Diplomacy_CostPerAlly => TextLib.ThaiConv("ราคา|จะ|เพิ่ม|ขึ้น| {0} |ต่อ|พันธมิตร|หนึ่ง|ฝ่าย");

        public override string Event_ChanceOfFailure => TextLib.ThaiConv("โอกาส|ล้ม|เหลว | {0}%");
        public override string EventMessage_Event_Title => TextLib.ThaiConv("อีเวนต์");
        public override string EventMessage_TheCohalition => TextLib.ThaiConv("กลุ่ม|พันธมิตร|ร่วม");

        public override string EventMessage_DarkHorde => TextLib.ThaiConv("อสูร|ทมิฬ");
        public override string EventMessage_DarkHordeKiller_Title => TextLib.ThaiConv("ผู้|พิชิต|อสูร|ทมิฬ");
        public override string EventMessage_DarkHordeKiller_Message => TextLib.ThaiConv("อัศวิน|แชมเปี้ยน|ได้|เข้า|ร่วม|กอง|ทัพ|ของ|ท่าน|แล้ว");

        public override string Settings_Mode_Spectator_Description => TextLib.ThaiConv("เฝ้า|มอง|อย่าง|เดียว |หรือ|จะ|แทรก|แซง|ด้วย|พลัง|พระ|เจ้า");
        public override string GodPower => TextLib.ThaiConv("พลัง|พระ|เจ้า");

        public override string Building_TreeSprout_Description => TextLib.ThaiConv("ปลูก|ต้น|ไม้");
        public override string Building_TreeSprout_Soft => TextLib.ThaiConv("ต้น|ไม้|เนื้อ|อ่อน");
        public override string Building_TreeSprout_Hard => TextLib.ThaiConv("ต้น|ไม้|เนื้อ|แข็ง");

        public override string GeneralSetting_SetAll => TextLib.ThaiConv("ใช้|กับ|ทั้งหมด");

        public override string Hud_All => TextLib.ThaiConv("ทั้งหมด");

        public override string Hud_Previous => TextLib.ThaiConv("ก่อน|หน้า");

        public override string Hud_EffectWillStack => TextLib.ThaiConv("ผล|ของ|สถานะ|จะ|ทับ|ซ้อน|กัน");

        public override string Info_WhenFoodRunsOut => TextLib.ThaiConv("เมื่อ|อาหาร|หมด |เมือง|และ|กอง|ทัพ|จะ|ซื้อ|อาหาร|จาก|ตลาด|มืด|โดย|อัตโนมัติ");

        public override string InputActionName_NextWar => TextLib.ThaiConv("ฝ่าย|ที่|ทำ|สงคราม|ถัด|ไป");

        /// <summary>
        /// These symbols are needed to fit large numbers on the HUD, there will be a tooltip to explain what number it represents
        /// </summary>
        public override string EngineHud_SymbolFor100 => TextLib.ThaiConv("c");
        public override string EngineHud_SymbolFor1000 => TextLib.ThaiConv("k");
        public override string EngineHud_SymbolFor10000 => TextLib.ThaiConv("10k");

        /// <summary>
        /// When loading files from other players, you won’t get their achievement progress. Use the word for Steam Achievements.
        /// </summary>
        public override string GameMenu_BlockImportAchievements => TextLib.ThaiConv("ระงับ|การ|เก็บ|ความ|สำเร็จ| (Achievements) |ใน|ไฟล์|ที่|นำ|เข้า");

        public override string EndScreen_PeaceVictoryQuote => TextLib.ThaiConv("จง|วาง|ดาบ|ของ|เรา|ลง |และ|โอบ|รับ|อนาคต|ที่|ดี|กว่า");

        public override string VictoryType_DefeatBoss => TextLib.ThaiConv("พิชิต|บอส");
        public override string VictoryType_Domination => TextLib.ThaiConv("การ|แผ่|อำนาจ|ครอบ|งำ");
        public override string VictoryType_WorldPeace => TextLib.ThaiConv("สันติ|ภาพ|โลก");
    }
}
