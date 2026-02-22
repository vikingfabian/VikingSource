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
                content.Add(new RbText(LoadContent.CheckCharsSafety( entry.userName, LoadedFont.Regular), HudLib.TitleColor_Name));
                HudLib.BulletSeperationPoint(content);
                leaderBoard.toMenu(content, entry);
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
        LeaderboardList[] tabs = new LeaderboardList[(int)LeaderBoardType.NUM];

        public LeaderboardMenu(RichMenu menu)
        { 
            this.menu = menu;
        }


        public void toMenu()
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(".Leaderboards", HudLib.TitleColor_Head);

            content.newLine();
            List<ArtTabMember> tabMembers = new List<ArtTabMember>((int)LeaderBoardType.NUM);
            for (int i = 0; i < (int)LeaderBoardType.NUM; ++i)
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
            //string Leaderboard_DominationSpeed = ".World domination top time, {0}% plus";
            //switch (tab)
            //{
            //    case LeaderBoardType.story_difficulty:
            //        title = ".Story victory, top % difficulty";
            //        break;
            //    case LeaderBoardType.domination_speed50:
            //        title = string.Format(Leaderboard_DominationSpeed, 50);
            //        break;
            //    case LeaderBoardType.domination_speed100:
            //        title = string.Format(Leaderboard_DominationSpeed, 100);
            //        break;
            //    case LeaderBoardType.domination_speed150:
            //        title = string.Format(Leaderboard_DominationSpeed, 150);
            //        break;
            //    case LeaderBoardType.city_size:
            //        title = ".Top city size, in workers";
            //        break;
            //    case LeaderBoardType.survive300_time:
            //        title = ".Survival length at 300% difficulty";
            //        break;

            //}
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
            string Leaderboard_DominationSpeed = ".World domination top time, {0}% plus";
            switch (tab)
            {
                case LeaderBoardType.story_difficulty:
                    title = ".Story victory, top % difficulty";
                    break;
                case LeaderBoardType.domination_speed50:
                    title = string.Format(Leaderboard_DominationSpeed, 50);
                    break;
                case LeaderBoardType.domination_speed100:
                    title = string.Format(Leaderboard_DominationSpeed, 100);
                    break;
                case LeaderBoardType.domination_speed150:
                    title = string.Format(Leaderboard_DominationSpeed, 150);
                    break;
                case LeaderBoardType.city_size:
                    title = ".Top city size, in workers";
                    break;
                case LeaderBoardType.survive300_time:
                    title = ".Survival length at 300% difficulty";
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
                        leaderBoard = new SurviveLeaderBoard();
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
            for (LeaderBoardType type = 0; type < LeaderBoardType.NUM; type++)
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
            //name = $"{type}_{StartupSettings.LeaderboardVersion}";
            //if (StartupSettings.LeaderboardInBeta)
            //{
            //    name = "beta_" + name;
            //}
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
            content.Add(new RbText(HudLib.TimeSpan_LongText(TimeSpan.FromSeconds(entry.score))));
        }
    }
    class SurviveLeaderBoard : AbsLeaderBoard
    {
        public SurviveLeaderBoard()
        {
            this.type = LeaderBoardType.survive300_time;
        }

        public SurviveLeaderBoard(TimeSpan time)
        {
            var difficulty = DssRef.difficulty.TotalDifficulty();
            setup(LeaderBoardType.survive300_time, (int)time.TotalSeconds);
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
        NUM
    }
}

