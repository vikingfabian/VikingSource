using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;
using VikingEngine.SteamWrapping;

namespace VikingEngine.DSSWars.Data
{
    class LeaderboardList
    {
        LeaderboardMenu leaderboardMenu;
        public int loadPageId = -1;
        public AbsLeaderBoard leaderBoard;
        List<SteamWrapping.SteamLeaderBoardRemote> values;
        public LeaderboardList(LeaderboardMenu leaderboardMenu, AbsLeaderBoard leaderBoard) 
        { 
            this.leaderboardMenu = leaderboardMenu;
            this.leaderBoard = leaderBoard;
            leaderBoard.setName();
            leaderBoard.BeginDownload(onDownload);
        }
        
        public void onDownload(List<SteamWrapping.SteamLeaderBoardRemote> values)
        {
            this.values = values;
            leaderboardMenu.onLoadComplete(leaderBoard.type);
        }
        public void toMenu(RichBoxContent content)
        {
            var id = SteamUser.GetSteamID().m_SteamID;

            foreach (var entry in values)
            {
                content.newLine();

                leaderBoard.toMenu(content, entry, out bool wide);

                if (wide)
                {
                    content.newLine();
                }
                HudLib.BulletSeperationPoint(content);

                Color nameCol = HudLib.TitleColor_Name;
                if (entry.user.m_SteamID == id)
                {
                    nameCol = HudLib.AvailableColor;
                    DssRef.achieve.UnlockAchievement(AchievementIndex.leaderboard_glory);
                }

                if (leaderBoard.casualControls)
                {
                    content.Add(new RbImage(SpriteName.WarsHudCasualMode));
                    content.hspace();
                }

                content.Add(new RbText(LoadContent.CheckCharsSafety(entry.userName, LoadedFont.Regular), nameCol));

            }

            if (values.Count == 0)
            {
                content.text(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light);
            }
        }

        public bool LoadComplete => values != null;
    }

    struct LeaderBoardMainTab
    {
        public SpriteName icon;
        public LeaderBoardType first;
        public bool casualSubTab;
        //public bool difficultySubTab;
        public FlatArray_Eight<DifficultySubTab> difficultyTabs;

        public bool Contains(LeaderBoardType type, out int difficultyTab, out int casualTab)
        {
            difficultyTab = -1;
            casualTab = 0;
            for (int i = 0; i < difficultyTabs.count; ++i)
            {
                if (difficultyTabs[i].type == type)
                {
                    difficultyTab = i;
                    return true;
                }
                else if (casualSubTab && difficultyTabs[i].type + 1 == type)
                {
                    casualTab = 1;
                    difficultyTab = i;
                    return true;
                }
            }
            return false;
        }
    }

    struct DifficultySubTab
    {
        public LeaderBoardType type;
        public int difficulty;

        public DifficultySubTab(LeaderBoardType type, int difficulty)
        {
            this.type = type;
            this.difficulty = difficulty;
        }

        
    }

    class LeaderboardMenu
    {
        static readonly LeaderBoardMainTab[] mainTabs = new LeaderBoardMainTab[]
        {
            new LeaderBoardMainTab { first = LeaderBoardType.story_difficulty, icon = SpriteName.MenuPixelIconManual, casualSubTab = true, },

            new LeaderBoardMainTab { first = LeaderBoardType.domination_speed50, icon = SpriteName.WarsMapFilterStrength, casualSubTab = true, difficultyTabs = new FlatArray_Eight<DifficultySubTab>(
                new DifficultySubTab(LeaderBoardType.domination_speed50, 50),new DifficultySubTab(LeaderBoardType.domination_speed100, 100), new DifficultySubTab(LeaderBoardType.domination_speed150, 150), new DifficultySubTab(LeaderBoardType.domination_speed200, 200)) },

            new LeaderBoardMainTab { first = LeaderBoardType.city_size50, icon = SpriteName.WarsWorker, casualSubTab = true, difficultyTabs = new FlatArray_Eight<DifficultySubTab>(
                    new DifficultySubTab(LeaderBoardType.city_size50, 50),new DifficultySubTab(LeaderBoardType.city_size100, 100), new DifficultySubTab(LeaderBoardType.city_size150, 150), new DifficultySubTab(LeaderBoardType.city_size200, 200)) },

            new LeaderBoardMainTab { first = LeaderBoardType.one_army50_strength, icon = SpriteName.WarsArmy, casualSubTab = true, difficultyTabs = new FlatArray_Eight<DifficultySubTab>(
                    new DifficultySubTab(LeaderBoardType.one_army50_strength, 50),new DifficultySubTab(LeaderBoardType.one_army100_strength, 100), new DifficultySubTab(LeaderBoardType.one_army150_strength, 150), new DifficultySubTab(LeaderBoardType.one_army200_strength, 200)) },

            new LeaderBoardMainTab { first = LeaderBoardType.nation50_strength, icon = SpriteName.WarsStrengthIcon, casualSubTab = true, difficultyTabs = new FlatArray_Eight<DifficultySubTab>(
                    new DifficultySubTab(LeaderBoardType.nation50_strength, 50),new DifficultySubTab(LeaderBoardType.nation100_strength, 100), new DifficultySubTab(LeaderBoardType.nation150_strength, 150), new DifficultySubTab(LeaderBoardType.nation200_strength, 200)) },

            new LeaderBoardMainTab { first = LeaderBoardType.survive300_time, icon = SpriteName.WarsRelationTotalWar,casualSubTab = true, difficultyTabs = new FlatArray_Eight<DifficultySubTab>(
                new DifficultySubTab(LeaderBoardType.survive300_time, 300),new DifficultySubTab(LeaderBoardType.survive400_time, 400)) },

            new LeaderBoardMainTab { first = LeaderBoardType.multiplayer_playercount, icon = SpriteName.WarsHudIconMultiplayer, casualSubTab = false, },
        };


        RichMenu menu;
        //LeaderBoardType tab = LeaderBoardType.story_difficulty;
        int mainTabIndex = 0;
        int difficultyTabIndex = 0;
        int casualTabIndex = 0;
        LeaderboardList[] tabs = new LeaderboardList[(int)LeaderBoardType.NUM_NONE];

        public LeaderboardMenu(RichMenu menu)
        { 
            this.menu = menu;
            if (DssRef.LastLeaderBoardUpload != LeaderBoardType.NUM_NONE)
            {
                //tab = DssRef.LastLeaderBoardUpload;
                for (int main = 0; main < mainTabs.Length; ++main)
                {
                    if (mainTabs[main].Contains(DssRef.LastLeaderBoardUpload, out var difficultyTab, out var casualTab))
                    {
                        mainTabIndex = main;
                        difficultyTabIndex = difficultyTab;
                        casualTabIndex = casualTab;
                        break;
                    }
                }
                DssRef.LastLeaderBoardUpload = LeaderBoardType.NUM_NONE;
            }
        }



        public void toMenu()
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(DssRef.lang.Leaderboards_title, HudLib.TitleColor_Head);

            content.newLine();
            List<ArtTabMember> mainTabMembers = new List<ArtTabMember>(mainTabs.Length);
            for (int i = 0; i < mainTabs.Length; ++i)
            {
                LeaderBoardMainTab tab = mainTabs[i];
                ArtTabMember tabMember = new ArtTabMember(new List<AbsRichBoxMember> {/* new RbText(TextLib.IndexToString(i)), new RbSpace(0.5f),*/ new RbImage(tab.icon) }
                    );
                mainTabMembers.Add(tabMember);
            }

            var tabGroup = new ArtTabgroup(mainTabMembers, mainTabIndex, (int selected) =>
            {
                mainTabIndex = selected;
                toMenu();
            });
            content.Add(tabGroup);
            LeaderBoardType leaderBoard = CurrentLeaderboard();

            string title = LeaderBoardCommunityName(leaderBoard, false);

            content.h1(title, HudLib.TitleColor_Label);
            if (StartupSettings.LeaderboardInBeta)
            {
                content.text("beta mode", HudLib.SecondaryTextColor);
            }

            var currentTab = mainTabs[mainTabIndex];

            if (currentTab.casualSubTab)
            {
                List<ArtTabMember> casualTabMembers = new List<ArtTabMember>(2);
                for (int i = 0; i < 2; ++i)
                {
                    string text = i == 0 ? DssRef.lang.Settings_AdvancedControls : DssRef.lang.Settings_CasualControls;
                    ArtTabMember tabMember = new ArtTabMember(new List<AbsRichBoxMember> { new RbText(text) });
                    casualTabMembers.Add(tabMember);
                }
                var casualTabGroup = new ArtTabgroup(casualTabMembers, casualTabIndex, (int selected) =>
                {
                    casualTabIndex = selected;
                    toMenu();
                });
                content.newLine();
                content.Add(casualTabGroup);
            }

#if DEBUG
            content.text(leaderBoard.ToString(), Color.Gray);
#endif

            if (currentTab.difficultyTabs.count > 0)
            {
                List<ArtTabMember> difficultyTabMembers = new List<ArtTabMember>(currentTab.difficultyTabs.count);
                for (int i = 0; i < currentTab.difficultyTabs.count; ++i)
                {
                    DifficultySubTab tab = currentTab.difficultyTabs[i];
                    ArtTabMember tabMember = new ArtTabMember(new List<AbsRichBoxMember> { new RbText(tab.difficulty.ToString() + "%") });
                    difficultyTabMembers.Add(tabMember);
                }

                var diffTabGroup = new ArtTabgroup(difficultyTabMembers, difficultyTabIndex, (int selected) =>
                {
                    difficultyTabIndex = selected;
                    toMenu();
                });
                content.newLine();
                content.Add(diffTabGroup);
            }

            if (openTab(leaderBoard) == false)
            {
                content.newLine();
                content.Add(new RbText(DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light));
                menu.Refresh(content);

                tabs[(int)leaderBoard].loadPageId = menu.richBox.PageId;
            }
            else
            {
                tabs[(int)leaderBoard].toMenu(content);
                menu.Refresh(content);
            }
        }

        private LeaderBoardType CurrentLeaderboard()
        {
            var currentTab = mainTabs[mainTabIndex];

            LeaderBoardType leaderBoard;
            if (currentTab.difficultyTabs.count > 0)
            {
                if (!currentTab.difficultyTabs.InBounds(difficultyTabIndex))
                {
                    difficultyTabIndex = 0;
                }
                leaderBoard = currentTab.difficultyTabs[difficultyTabIndex].type;
            }
            else
            {
                leaderBoard = currentTab.first;
            }

            if (currentTab.casualSubTab)
            {
                if (casualTabIndex == 1)
                {
                    leaderBoard++;
                }
            }

            return leaderBoard;
        }

        public static string LeaderBoardCommunityName(LeaderBoardType tab, bool beta)
        {
            string title = null;
            switch (tab)
            {
                case LeaderBoardType.story_difficulty:
                case LeaderBoardType.story_difficulty_casual:
                    title = DssRef.lang.Leaderboards_victory;
                    break;

                case LeaderBoardType.domination_speed50:
                case LeaderBoardType.domination_speed50_casual:
                    title = string.Format(DssRef.lang.Leaderboards_domination, 50);
                    break;

                case LeaderBoardType.domination_speed100:
                case LeaderBoardType.domination_speed100_casual:
                    title = string.Format(DssRef.lang.Leaderboards_domination, 100);
                    break;

                case LeaderBoardType.domination_speed150:
                case LeaderBoardType.domination_speed150_casual:
                    title = string.Format(DssRef.lang.Leaderboards_domination, 150);
                    break;

                case LeaderBoardType.domination_speed200:
                case LeaderBoardType.domination_speed200_casual:
                    title = string.Format(DssRef.lang.Leaderboards_domination, 200);
                    break;

                case LeaderBoardType.city_size50:
                case LeaderBoardType.city_size50_casual:
                case LeaderBoardType.city_size100:
                case LeaderBoardType.city_size100_casual:
                case LeaderBoardType.city_size150:
                case LeaderBoardType.city_size150_casual:
                case LeaderBoardType.city_size200:
                case LeaderBoardType.city_size200_casual:
                    title = DssRef.lang.Leaderboards_CitySize;
                    break;

                case LeaderBoardType.one_army50_strength:
                case LeaderBoardType.one_army50_strength_casual:
                case LeaderBoardType.one_army100_strength:
                case LeaderBoardType.one_army100_strength_casual:
                case LeaderBoardType.one_army150_strength:
                case LeaderBoardType.one_army150_strength_casual:
                case LeaderBoardType.one_army200_strength:
                case LeaderBoardType.one_army200_strength_casual:
                    title = DssRef.lang.Leaderboards_ArmySize;
                    break;

                case LeaderBoardType.nation50_strength:
                case LeaderBoardType.nation50_strength_casual:
                case LeaderBoardType.nation100_strength:
                case LeaderBoardType.nation100_strength_casual:
                case LeaderBoardType.nation150_strength:
                case LeaderBoardType.nation150_strength_casual:
                case LeaderBoardType.nation200_strength:
                case LeaderBoardType.nation200_strength_casual:
                    title = DssRef.lang.Leaderboards_NationStrength;
                    break;

                case LeaderBoardType.survive300_time:
                case LeaderBoardType.survive300_time_casual:
                    title = string.Format(DssRef.lang.Leaderboards_Survival, SurviveLeaderBoard.Difficulty300);
                    break;

                case LeaderBoardType.survive400_time:
                case LeaderBoardType.survive400_time_casual:
                    title = string.Format(DssRef.lang.Leaderboards_Survival, SurviveLeaderBoard.Difficulty400);
                    break;

                case LeaderBoardType.multiplayer_playercount:
                    title = DssRef.lang.Leaderboards_MultiplayerPlayerCount;
                    break;
            }

            if (beta)
            {
                title += " (beta)";
            }

            return title;
        }

        public void onLoadComplete(LeaderBoardType type)
        {
            LeaderBoardType leaderBoard = CurrentLeaderboard();
            if (leaderBoard == type && tabs[(int)leaderBoard].loadPageId == menu.richBox.PageId)
            {
                toMenu();
            }
        }

        public bool openTab(LeaderBoardType type)
        {
            int ix = (int)type;
            if (tabs[ix] == null)
            {
                AbsLeaderBoard leaderBoard = null;
                switch (type)
                {
                    case LeaderBoardType.domination_speed50:
                    case LeaderBoardType.domination_speed100:
                    case LeaderBoardType.domination_speed150:
                    case LeaderBoardType.domination_speed200:

                    case LeaderBoardType.domination_speed50_casual:
                    case LeaderBoardType.domination_speed100_casual:
                    case LeaderBoardType.domination_speed150_casual:
                    case LeaderBoardType.domination_speed200_casual:

                    case LeaderBoardType.story_difficulty:
                    case LeaderBoardType.story_difficulty_casual:
                        leaderBoard = new VictoryLeaderBoard(type);
                        break;

                    case LeaderBoardType.city_size50:
                    case LeaderBoardType.city_size50_casual:
                    case LeaderBoardType.city_size100:
                    case LeaderBoardType.city_size100_casual:
                    case LeaderBoardType.city_size150:
                    case LeaderBoardType.city_size150_casual:
                    case LeaderBoardType.city_size200:
                    case LeaderBoardType.city_size200_casual:
                        leaderBoard = new CitySizeLeaderBoard(type);
                        break;

                    case LeaderBoardType.one_army50_strength:
                    case LeaderBoardType.one_army100_strength:
                    case LeaderBoardType.one_army150_strength:
                    case LeaderBoardType.one_army200_strength:
                    case LeaderBoardType.one_army50_strength_casual:
                    case LeaderBoardType.one_army100_strength_casual:
                    case LeaderBoardType.one_army150_strength_casual:
                    case LeaderBoardType.one_army200_strength_casual:
                        leaderBoard = new ArmyStrengthLeaderBoard(type);
                        break;

                    case LeaderBoardType.nation50_strength:
                    case LeaderBoardType.nation50_strength_casual:
                    case LeaderBoardType.nation100_strength:
                    case LeaderBoardType.nation100_strength_casual:
                    case LeaderBoardType.nation150_strength:
                    case LeaderBoardType.nation150_strength_casual:
                    case LeaderBoardType.nation200_strength:
                    case LeaderBoardType.nation200_strength_casual:
                        leaderBoard = new NationStrengthLeaderBoard(type);
                        break;

                    case LeaderBoardType.survive300_time:
                    case LeaderBoardType.survive400_time:
                    case LeaderBoardType.survive300_time_casual:
                    case LeaderBoardType.survive400_time_casual:
                        leaderBoard = new SurviveLeaderBoard(type);
                        break;

                    case LeaderBoardType.multiplayer_playercount:
                        leaderBoard = new MultiplayerCountLeaderBoard();
                        break;
                    default:
                        throw new NotImplementedException($"leaderboard {type}");
                }
                tabs[ix] = new LeaderboardList(this, leaderBoard);
                return false;
            }

            return tabs[ix].LoadComplete;
        }
    }

    abstract class AbsLeaderBoard: SteamLeaderBoardLocal
    {
        public bool casualControls;

        public static void CreateLeaderBoards()
        {
            //LeaderBoardType type = LeaderBoardType.survive400_time;
            for (LeaderBoardType type = 0; type < LeaderBoardType.NUM_NONE; type++)
            {
                ELeaderboardSortMethod sort;
                ELeaderboardDisplayType display;
                switch (type)
                {
                    case LeaderBoardType.story_difficulty:
                    case LeaderBoardType.story_difficulty_casual:
                    case LeaderBoardType.city_size50:
                    case LeaderBoardType.city_size50_casual:
                    case LeaderBoardType.city_size100:
                    case LeaderBoardType.city_size100_casual:
                    case LeaderBoardType.city_size150:
                    case LeaderBoardType.city_size150_casual:
                    case LeaderBoardType.city_size200:
                    case LeaderBoardType.city_size200_casual:

                    case LeaderBoardType.one_army50_strength:
                    case LeaderBoardType.one_army100_strength:
                    case LeaderBoardType.one_army150_strength:
                    case LeaderBoardType.one_army200_strength:
                    case LeaderBoardType.one_army50_strength_casual:
                    case LeaderBoardType.one_army100_strength_casual:
                    case LeaderBoardType.one_army150_strength_casual:
                    case LeaderBoardType.one_army200_strength_casual:

                    case LeaderBoardType.nation50_strength:
                    case LeaderBoardType.nation50_strength_casual:
                    case LeaderBoardType.nation100_strength:
                    case LeaderBoardType.nation100_strength_casual:
                    case LeaderBoardType.nation150_strength:
                    case LeaderBoardType.nation150_strength_casual:
                    case LeaderBoardType.nation200_strength:
                    case LeaderBoardType.nation200_strength_casual:

                    case LeaderBoardType.multiplayer_playercount:
                        display = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric;
                        sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending;
                        break;

                    case LeaderBoardType.survive300_time:
                    case LeaderBoardType.survive400_time:
                    case LeaderBoardType.survive300_time_casual:
                    case LeaderBoardType.survive400_time_casual:
                        display = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeSeconds;
                        sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending;
                        break;

                    case LeaderBoardType.domination_speed50:
                    case LeaderBoardType.domination_speed100:
                    case LeaderBoardType.domination_speed150:
                    case LeaderBoardType.domination_speed200:

                    case LeaderBoardType.domination_speed50_casual:
                    case LeaderBoardType.domination_speed100_casual:
                    case LeaderBoardType.domination_speed150_casual:
                    case LeaderBoardType.domination_speed200_casual:
                        display = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeSeconds;
                        sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                create(false);
                create(true);

                void create(bool beta)
                {
                    //CallResult<LeaderboardFindResult_t> findLeaderboardCallback = new CallResult<LeaderboardFindResult_t>(onFindLeaderboard);
                    var apiCall = SteamUserStats.FindOrCreateLeaderboard(TypeId(type, beta), sort, display);//"Error");
                    //findLeaderboardCallback.Set(apiCall);
                }
            }
        }

        protected void difficultyLevelAndCasual(out int diffLevel, out bool casual)
        {
            casual = DssRef.state?.playstate()?.casualControls ?? false;

            var difficulty = DssRef.difficulty.TotalDifficulty();
            if (difficulty >= 200)
            {
                diffLevel = 200;
            }
            else if (difficulty >= 150)
            {
                diffLevel = 150;
            }
            else if (difficulty >= 100)
            {
                diffLevel = 100;
            }
            else if (difficulty >= 50)
            {
                diffLevel = 50;
            }
            else
            {
                diffLevel = 0;                
            }
        }

        public LeaderBoardType type;
        protected void setup(LeaderBoardType type, int score)
        {
            this.type = type;
            setName();
            this.score = score;

            int.TryParse(Engine.LoadContent.EngineVersion, out int version);
            scoreDetails.Add(version);
        }

        public void setName()
        {
            name = TypeId(type, StartupSettings.LeaderboardInBeta);
        }



        public static string TypeId(LeaderBoardType type, bool beta)
        {
            string name = $"{type}_{StartupSettings.LeaderboardVersion}";
            if (beta)
            {
                name = "beta_" + name;
            }
            return name ;
        }

        //protected void AddCasualControls()
        //{
        //    var state = DssRef.state?.playstate();
        //    if (state != null)
        //    {
        //        casualControls = state.casualControls;
        //    }

        //    scoreDetails.Add(lib.BoolToInt01(casualControls));
        //}

        //protected void GetCasualControls(SteamLeaderBoardRemote entry, int detailIx)
        //{
        //    casualControls  = false;
        //    if (entry.scoreDetails.TryGetIndex(detailIx, out int value))
        //    {
        //        casualControls = lib.ToBool(value);
        //    }
        //}

        public override void BeginUpload()
        {
            if (DssRef.state.importedWorld && DssRef.storage.blockImportAchievements)
            {
                return;
            }

            DssRef.LastLeaderBoardUpload = type;

            base.BeginUpload();
        }

        abstract public void toMenu(RichBoxContent content, SteamWrapping.SteamLeaderBoardRemote entry, out bool wideContent);
    }

    class CitySizeLeaderBoard : AbsLeaderBoard
    {
        public static int SizeUploaded = 200;
        public CitySizeLeaderBoard(LeaderBoardType type)
        {
            this.type = type;
        }

        public CitySizeLeaderBoard(int workerCount)
        {
            difficultyLevelAndCasual(out int difficulty, out bool casual);
            switch (difficulty)
            {
                default:
                    return;

                case 50:
                    type = LeaderBoardType.city_size50;
                    break;
                case 100:
                    type = LeaderBoardType.city_size100;
                    break;
                case 150:
                    type = LeaderBoardType.city_size150;
                    break;
                case 200:
                    type = LeaderBoardType.city_size200;
                    break;
            }
            if (casual)
            {
                type++;
            }

            setup(type, workerCount);
            scoreDetails.Add(difficulty);
            scoreDetails.Add((int)DssRef.time.TotalIngameTime().TotalSeconds);
            //AddCasualControls();

            BeginUpload();
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry, out bool wideContent)
        {
            //GetCasualControls(entry, 3);
            wideContent = false;
            content.Add(new RbText(TextLib.LargeNumber(entry.score)));
        }
    }
    class ArmyStrengthLeaderBoard : AbsLeaderBoard
    {
        public static float SizeUploaded = 25;
        public ArmyStrengthLeaderBoard(LeaderBoardType type)
        {
            this.type = type;
        }

        public ArmyStrengthLeaderBoard(float armyStrength, int soldiersCount)
        {
            difficultyLevelAndCasual(out int difficulty, out bool casual);
            switch (difficulty)
            {
                default:
                    return;

                case 50:
                    type = LeaderBoardType.one_army50_strength;
                    break;
                case 100:
                    type = LeaderBoardType.one_army100_strength;
                    break;
                case 150:
                    type = LeaderBoardType.one_army150_strength;
                    break;
                case 200:
                    type = LeaderBoardType.one_army200_strength;
                    break;
            }
            if (casual)
            {
                type++;
            }

            setup(type, Convert.ToInt32(armyStrength));
            scoreDetails.Add(difficulty);
            scoreDetails.Add(soldiersCount);
            //AddCasualControls();
            BeginUpload();
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry, out bool wideContent)
        {
            //GetCasualControls(entry, 3);
            wideContent = false;
            content.Add(new RbText(TextLib.LargeNumber(entry.score)));
        }
    }

    class NationStrengthLeaderBoard : AbsLeaderBoard
    {
        public static float SizeUploaded = 50;
        public NationStrengthLeaderBoard(LeaderBoardType type)
        {
            this.type = type;
        }

        public NationStrengthLeaderBoard(float nationStrength)
        {
            difficultyLevelAndCasual(out int difficulty, out bool casual);
            switch (difficulty)
            {
                default:
                    return;

                case 50:
                    type = LeaderBoardType.nation50_strength;
                    break;
                case 100:
                    type = LeaderBoardType.nation100_strength;
                    break;
                case 150:
                    type = LeaderBoardType.nation150_strength;
                    break;
                case 200:
                    type = LeaderBoardType.nation200_strength;
                    break;
            }
            if (casual)
            {
                type++;
            }

            setup(type, Convert.ToInt32(nationStrength));
            scoreDetails.Add(difficulty);
            //AddCasualControls();
            BeginUpload();
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry, out bool wideContent)
        {
            //GetCasualControls(entry, 3);
            wideContent = false;
            content.Add(new RbText(TextLib.LargeNumber(entry.score)));
        }
    }


    class MultiplayerCountLeaderBoard : AbsLeaderBoard
    {
        //public static int CountUploaded = 2;
        public MultiplayerCountLeaderBoard()
        {
            this.type = LeaderBoardType.multiplayer_playercount;
        }

        public MultiplayerCountLeaderBoard(int count)
        {
            var difficulty = DssRef.difficulty.TotalDifficulty();

            setup(LeaderBoardType.multiplayer_playercount, count);
            scoreDetails.Add(difficulty);
            //AddCasualControls();
            BeginUpload();
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry, out bool wideContent)
        {
            //GetCasualControls(entry, 2);
            wideContent = false;
            content.Add(new RbText(entry.score.ToString()));
        }
    }

    class SurviveLeaderBoard : AbsLeaderBoard
    {
        public const int Difficulty300 = 350;
        public const int Difficulty400 = 400;
        public SurviveLeaderBoard(LeaderBoardType type)
        {
            this.type = type;
        }

        public SurviveLeaderBoard(TimeSpan time)
        {
            var difficulty = DssRef.difficulty.TotalDifficulty();
            var type = difficulty >= Difficulty400 ? LeaderBoardType.survive400_time : LeaderBoardType.survive300_time;

            if (DssRef.state?.playstate()?.casualControls ?? false)
            {
                type++;
            }

            setup(type, (int)time.TotalSeconds);
            scoreDetails.Add(difficulty);
            //AddCasualControls();
            BeginUpload();
                        
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry, out bool wideContent)
        {
            //GetCasualControls(entry, 2);
            wideContent = true;
            content.Add(new RbText(HudLib.TimeSpan_LongText(TimeSpan.FromSeconds(entry.score))));
        }
    }

    class VictoryLeaderBoard : AbsLeaderBoard
    {

        public VictoryLeaderBoard(LeaderBoardType type)
        {
            this.type = type;
        }

        public VictoryLeaderBoard(GameEndReason endReason, VictoryType vType)
        {            

            if (endReason == GameEndReason.Victory)
            {
                //var difficulty = DssRef.difficulty.TotalDifficulty();
                difficultyLevelAndCasual(out int difficulty, out bool casual);

                switch (vType)
                {
                    case VictoryType.DefeatBoss:
                        if (DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory)
                        {
                            setup(LeaderBoardType.story_difficulty, difficulty);
                            scoreDetails.Add((int)DssRef.time.TotalIngameTime().TotalSeconds);
                            //AddCasualControls();
                            BeginUpload();
                        }
                        break;

                    case VictoryType.Domination:
                        //difficultyLevelAndCasual(out int difficulty, out bool casual);
                        switch (difficulty)
                        {
                            default:
                                return;

                            case 50:
                                type = LeaderBoardType.domination_speed50;
                                break;
                            case 100:
                                type = LeaderBoardType.domination_speed100;
                                break;
                            case 150:
                                type = LeaderBoardType.domination_speed150;
                                break;
                            case 200:
                                type = LeaderBoardType.domination_speed200;
                                break;
                        }
                        if (casual)
                        {
                            type++;
                        }

                        setup(type, (int)DssRef.time.TotalIngameTime().TotalSeconds);
                        scoreDetails.Add(difficulty);
                        //  AddCasualControls();
                        BeginUpload();
                        break;

                }
            }
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry, out bool wideContent)
        {
                //GetCasualControls(entry, 2);
            if (type == LeaderBoardType.story_difficulty)
            {
                wideContent = false;
                content.Add(new RbText( $"{entry.score}%"));
            }
            else
            {
                wideContent = true;
                content.Add(new RbText(HudLib.TimeSpan_LongText(TimeSpan.FromSeconds(entry.score))));
            }
        }
    }

    enum LeaderBoardType
    {
        story_difficulty,
        story_difficulty_casual,

        domination_speed50,
        domination_speed50_casual,
        domination_speed100,
        domination_speed100_casual,
        domination_speed150,
        domination_speed150_casual,
        domination_speed200,
        domination_speed200_casual,

        city_size50,
        city_size50_casual,
        city_size100,
        city_size100_casual,
        city_size150,
        city_size150_casual,
        city_size200,
        city_size200_casual,
        //nation_size,

        one_army50_strength,
        one_army50_strength_casual,
        one_army100_strength,
        one_army100_strength_casual,
        one_army150_strength,
        one_army150_strength_casual,
        one_army200_strength,
        one_army200_strength_casual,

        nation50_strength,
        nation50_strength_casual,
        nation100_strength,
        nation100_strength_casual,
        nation150_strength,
        nation150_strength_casual,
        nation200_strength,
        nation200_strength_casual,

        survive300_time,
        survive300_time_casual,
        survive400_time,        
        survive400_time_casual,

        multiplayer_playercount,
        NUM_NONE
    }
}

