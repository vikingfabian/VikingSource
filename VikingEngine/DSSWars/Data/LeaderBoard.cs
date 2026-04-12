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
            foreach (var entry in values)
            {
                content.newLine();

                leaderBoard.toMenu(content, entry);

                HudLib.BulletSeperationPoint(content);

                content.Add(new RbText(LoadContent.CheckCharsSafety( entry.userName, LoadedFont.Regular), HudLib.TitleColor_Name));
                
                
            }

            if (values.Count == 0)
            {
                content.text(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light);
            }
        }

        public bool LoadComplete => values != null;
    }
    class LeaderboardMenu
    {
        RichMenu menu;
        LeaderBoardType tab = LeaderBoardType.story_difficulty;
        LeaderboardList[] tabs = new LeaderboardList[(int)LeaderBoardType.NUM_NONE];

        public LeaderboardMenu(RichMenu menu)
        { 
            this.menu = menu;
            if (DssRef.LastLeaderBoardUpload != LeaderBoardType.NUM_NONE)
            {
                tab = DssRef.LastLeaderBoardUpload;
                DssRef.LastLeaderBoardUpload = LeaderBoardType.NUM_NONE;
            }
        }


        public void toMenu()
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(DssRef.lang.Leaderboards_title, HudLib.TitleColor_Head);

            content.newLine();
            List<ArtTabMember> tabMembers = new List<ArtTabMember>((int)LeaderBoardType.NUM_NONE);
            for (int i = 0; i < (int)LeaderBoardType.NUM_NONE; ++i)
            {
                LeaderBoardType tabType = (LeaderBoardType)i;
                ArtTabMember tabMember = new ArtTabMember(new List<AbsRichBoxMember> { new RbText(TextLib.IndexToString(i)) }
                    );
                tabMembers.Add(tabMember);
            }

            var tabGroup = new ArtTabgroup(tabMembers, (int)tab,(int tabType) => {
                tab = (LeaderBoardType)tabType;
                toMenu();
            });
            content.Add(tabGroup);

            string title = LeaderBoardCommunityName(tab, false);
           
            content.h1(title, HudLib.TitleColor_Label);
            if (StartupSettings.LeaderboardInBeta)
            {
                content.text("beta mode", HudLib.SecondaryTextColor);
            }

            if (openTab(tab) == false)
            {
                content.newLine();
                content.Add(new RbText(DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light));
                menu.Refresh(content);

                tabs[(int)tab].loadPageId = menu.richBox.PageId;
            }
            else
            {
                tabs[(int)tab].toMenu(content);
                menu.Refresh(content);
            }
        }

        public static string LeaderBoardCommunityName(LeaderBoardType tab, bool beta)
        {
            string title = null;
            switch (tab)
            {
                case LeaderBoardType.story_difficulty:
                    title = DssRef.lang.Leaderboards_victory;
                    break;
                case LeaderBoardType.domination_speed50:
                    title = string.Format(DssRef.lang.Leaderboards_domination, 50);
                    break;
                case LeaderBoardType.domination_speed100:
                    title = string.Format(DssRef.lang.Leaderboards_domination, 100);
                    break;
                case LeaderBoardType.domination_speed150:
                    title = string.Format(DssRef.lang.Leaderboards_domination, 150);
                    break;
                case LeaderBoardType.city_size:
                    title = DssRef.lang.Leaderboards_CitySize;
                    break;
                case LeaderBoardType.survive300_time:
                    title = string.Format(DssRef.lang.Leaderboards_Survival, SurviveLeaderBoard.Difficulty300);
                    break;
                case LeaderBoardType.survive400_time:
                    title = string.Format(DssRef.lang.Leaderboards_Survival, SurviveLeaderBoard.Difficulty400);
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
            if (tab == type && tabs[(int)tab].loadPageId == menu.richBox.PageId)
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
                    case LeaderBoardType.story_difficulty:
                        leaderBoard = new VictoryLeaderBoard(type);
                        break;
                    case LeaderBoardType.city_size:
                        leaderBoard = new CitySizeLeaderBoard();
                        break;
                    case LeaderBoardType.survive300_time:
                    case LeaderBoardType.survive400_time:
                        leaderBoard = new SurviveLeaderBoard(type);
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
        public static void CreateLeaderBoards()
        {
            LeaderBoardType type = LeaderBoardType.survive400_time;
            //for (LeaderBoardType type = 0; type < LeaderBoardType.NUM; type++)
            {
                ELeaderboardSortMethod sort;
                ELeaderboardDisplayType display;
                switch (type)
                {
                    case LeaderBoardType.story_difficulty:
                    case LeaderBoardType.city_size:
                        display = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric;
                        sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending;
                        break;

                    case LeaderBoardType.survive300_time:
                    case LeaderBoardType.survive400_time:
                        display = ELeaderboardDisplayType.k_ELeaderboardDisplayTypeTimeSeconds;
                        sort = ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending;
                        break;

                    case LeaderBoardType.domination_speed50:
                    case LeaderBoardType.domination_speed100:
                    case LeaderBoardType.domination_speed150:
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

        public override void BeginUpload()
        {
            if (DssRef.state.importedWorld && DssRef.storage.blockImportAchievements)
            {
                return;
            }

            DssRef.LastLeaderBoardUpload = type;

            base.BeginUpload();
        }

        abstract public void toMenu(RichBoxContent content, SteamWrapping.SteamLeaderBoardRemote entry);
    }

    class CitySizeLeaderBoard : AbsLeaderBoard
    {
        public static int SizeUploaded = 200;
        public CitySizeLeaderBoard()
        {
            this.type = LeaderBoardType.city_size;
        }

        public CitySizeLeaderBoard(int workerCount)
        {
            var difficulty = DssRef.difficulty.TotalDifficulty();

            setup(LeaderBoardType.city_size, workerCount);
            scoreDetails.Add(difficulty);
            scoreDetails.Add((int)DssRef.time.TotalIngameTime().TotalSeconds);
            BeginUpload();
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry)
        {
            content.Add(new RbText(TextLib.LargeNumber(entry.score)));
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
            setup(difficulty >= Difficulty400? LeaderBoardType.survive400_time : LeaderBoardType.survive300_time, (int)time.TotalSeconds);
            scoreDetails.Add(difficulty);
            BeginUpload();
                        
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry)
        {
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
                var difficulty = DssRef.difficulty.TotalDifficulty();

                switch (vType)
                {
                    case VictoryType.DefeatBoss:
                        setup(LeaderBoardType.story_difficulty, difficulty);
                        scoreDetails.Add((int)DssRef.time.TotalIngameTime().TotalSeconds);
                        BeginUpload();
                        break;

                    case VictoryType.Domination:
                        
                        if (difficulty >= 150)
                        {
                            type = LeaderBoardType.domination_speed150;
                        }
                        else if (difficulty >= 100)
                        {
                            type = LeaderBoardType.domination_speed100;
                        }
                        else
                        {
                            type = LeaderBoardType.domination_speed50;
                        }
                        setup(type, (int)DssRef.time.TotalIngameTime().TotalSeconds);
                        scoreDetails.Add(difficulty);
                        BeginUpload();
                        break;

                }
            }
        }

        public override void toMenu(RichBoxContent content, SteamLeaderBoardRemote entry)
        {
            if (type == LeaderBoardType.story_difficulty)
            {
                content.Add(new RbText( $"{entry.score}%"));
            }
            else
            { 
                content.Add(new RbText(HudLib.TimeSpan_LongText(TimeSpan.FromSeconds(entry.score))));
            }
        }
    }

    enum LeaderBoardType
    {
        story_difficulty,
        domination_speed50,
        domination_speed100,
        domination_speed150,
        city_size,
        survive300_time,
        survive400_time,
        NUM_NONE
    }
}

