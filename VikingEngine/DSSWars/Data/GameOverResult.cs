using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DataLib;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameState.VoxelEditor;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.PJ;
using VikingEngine.Voxels;
using static VikingEngine.DataLib.FileIndex;

namespace VikingEngine.DSSWars.Data
{
    class GameOverResultCollection
    {
        public FileIndex allFiles = null;

        public GameOverResultCollection(Action refreshUnderMenu)
        {
            new Timer.Asynch1ArgTrigger<Action>(loadMeta, refreshUnderMenu, true);    
        }

        void loadMeta(Action refreshUnderMenu)
        {
            var path = GameOverResult.BasePath();
            FileIndex fileIndex = new FileIndex(path.CompleteDirectory, true,
                path.searchPattern(), true, new FileSortSettings());

            for (int i = 0; i < fileIndex.Files.Count && i < 50; i++)
            {
                new GameOverResult(fileIndex.Files[i]);
            }

            allFiles = fileIndex;

            Ref.update.AddSyncAction(new SyncAction(refreshUnderMenu));
        }
    }

    class GameOverResult
    {
        int steamVersion;
        DateTime date;
        TimeSpan gameTime;
        public GameEndReason endReason; VictoryType vType;
        public bool matchResults = false;
        Difficulty difficulty;
        List<GameOverResultPlayer> players;
        public GameOverResult(GameEndReason endReason, VictoryType vType, MatchResult matchResult)
        {
            if (!int.TryParse(Engine.LoadContent.EngineVersion, out steamVersion))
            {
                steamVersion = -1;
            }
            date = DateTime.Now;
            gameTime = DssRef.time.TotalIngameTime();

            this.endReason = endReason;
            this.vType = vType;
            
            if (matchResult != null)
            {
                this.matchResults = true;
            }

            difficulty = DssRef.difficulty.Clone();


            players = new List<GameOverResultPlayer>();
            foreach (var lp in DssRef.state.localPlayers)
            {
                players.Add(new GameOverResultPlayer(lp, matchResult));
            }

            Save();
        }

        public GameOverResult(FileEntry fileEntry)
        {
            DataStream.FilePath path = BasePath();
            path.FileName = fileEntry.Name;

            DataStream.BeginReadWrite.BinaryIO(false, path, null, read, null, false);

            fileEntry.Tag = this;
        }

        public RichBoxContent ButtonContent()
        {
            RichBoxContent content = new RichBoxContent();
            resultTitle(out string title, out string type);

            content.Add(new RbText(title));
            //if (type != null)
            //{
            //    HudLib.BulletSeperationPoint(content);
            //    content.Add(new RbText(type, HudLib.TitleColor_TypeName));
            //}
            HudLib.BulletSeperationPoint(content);
            content.Add(new RbText(difficulty.TotalDifficulty() + "%", HudLib.TitleColor_Label));

            return content;
        }

        void resultTitle(out string title, out string type)
        {
            title = null;
            type = null;
            if (matchResults)
            {
                title = DssRef.lang.EndScreen_MatchComplete;
            }
            else
            {
                switch (endReason)
                {
                    case GameEndReason.Victory:
                       title =DssRef.lang.EndScreen_VictoryTitle;

                        switch (vType)
                        {
                            case VictoryType.DefeatBoss:
                                type = DssRef.lang.VictoryType_DefeatBoss;
                                break;
                            case VictoryType.Domination:
                                type = DssRef.lang.VictoryType_Domination;
                                break;
                            case VictoryType.WorldPeace:
                                type = DssRef.lang.VictoryType_WorldPeace;
                                break;
                        }
                        break;

                    case GameEndReason.Defeat:
                        title = DssRef.lang.EndScreen_FailTitle;
                        break;

                    case GameEndReason.TimesUp:
                        title = DssRef.lang.EndScreen_TimeHasEndedTitle;
                        break;
                }
            }
        }

        public void tooltipContent(RichBoxContent content, object tag)
        {
            ToHud(content, true);
        }

        public void ToHud(RichBoxContent content, bool tooltip)
        {
            if (matchResults)
            {
                content.h1(DssRef.lang.EndScreen_MatchComplete, Color.Yellow);
            }
            else
            {
                switch (endReason)
                {
                    case GameEndReason.Victory:
                        content.h1(DssRef.lang.EndScreen_VictoryTitle, Color.Yellow);

                        string typeText = null;
                        switch (vType)
                        {
                            case VictoryType.DefeatBoss:
                                typeText = DssRef.lang.VictoryType_DefeatBoss;
                                break;
                            case VictoryType.Domination:
                                typeText = DssRef.lang.VictoryType_Domination;
                                break;
                            case VictoryType.WorldPeace:
                                typeText = DssRef.lang.VictoryType_WorldPeace;
                                break;
                        }

                        content.h2(typeText, HudLib.TitleColor_TypeName);
                        break;

                    case GameEndReason.Defeat:
                        content.h1(DssRef.lang.EndScreen_FailTitle).overrideColor = Color.Yellow;
                        break;

                    case GameEndReason.TimesUp:
                        content.h1(DssRef.lang.EndScreen_TimeHasEndedTitle).overrideColor = Color.Yellow;
                        break;
                }
            }

            gameSetupToHud(content, tooltip);

            if (!tooltip)
            {
                foreach (var p in players)
                {
                    content.Add(new RbSeperationLine());
                    content.h1(p.playerName, HudLib.TitleColor_Name);
                    if (matchResults)
                    {
                        content.h2(p.matchWinner ? DssRef.lang.EndScreen_VictoryTitle : DssRef.lang.EndScreen_FailTitle, Color.Yellow);
                    }
                    content.icontext(HudLib.CheckImage(p.casual), DssRef.lang.Settings_CasualControls);
                    p.statistics.ToHud(content);
                }
            }
        }

        void gameSetupToHud(RichBoxContent content, bool tooltip)
        {

            

            LangLib.GameModeText(difficulty.setting_gameMode, out string caption, out _);
            content.h1(caption, HudLib.TitleColor_Head);
            content.h2(string.Format(DssRef.lang.Settings_TotalDifficulty, difficulty.TotalDifficulty()), HudLib.TitleColor_Label);

            if (!tooltip)
            {
                content.text(string.Format(DssRef.lang.Settings_DifficultyLevel, difficulty.PercDifficulty));
            }
            content.icontext(SpriteName.WarsMapIcon, DssRef.lang.Lobby_MapSizeTitle + ": " + WorldData.SizeString(DssRef.world.metaData.mapSize));

            if (!tooltip)
            {
                content.icontext(HudLib.CheckImage(DssRef.storage.gameRuleset.centralGold), DssRef.lang.Settings_CentralGold);
                content.icontext(HudLib.CheckImage(difficulty.setting_allowPauseCommand), DssRef.lang.Settings_AllowPause);

                content.icontext(SpriteName.WarsResource_Food, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Settings_FoodMultiplier, TextLib.OneDecimal(difficulty.setting_foodMulti)));
                content.icontext(SpriteName.WarsResource_WaterAdd, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Settings_WaterMultiplier, TextLib.OneDecimal(difficulty.setting_waterMulti)));
                content.icontext(SpriteName.WarsWorker, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Settings_ChildMultiplier, TextLib.OneDecimal(difficulty.setting_childMulti)));
                content.icontext(SpriteName.WarsHammer, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Settings_CraftMultiplier, TextLib.OneDecimal(difficulty.setting_craftMulti)));
                content.icontext(SpriteName.WarsTechnology_Unlocked, string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Settings_TechMultiplier, difficulty.TechMultiProperty(null, false, 0)));

                var time = HudLib.TimeSpan_LongText(gameTime);
                content.text(string.Format(DssRef.lang.EndGameStatistics_Time, time));

                
            }

            content.newParagraph();

            content.text(HudLib.Date(date));
            content.text(string.Format(HudLib.EngineVersionString, steamVersion));
        }

        public static DataStream.FilePath BasePath()
        {
            return new DataStream.FilePath(Ref.steam.UserCloudPath + FilePath.Dir +
               "GameOverResult", null, ".gos", true, false);
        }

        void Save()
        {
            DataStream.FilePath path = BasePath();
            path.FileName= endReason.ToString() + difficulty.TotalDifficulty().ToString() + "_" + date.Ticks.ToString();

            try
            {
                System.IO.Directory.CreateDirectory(path.CompleteDirectory);
            }
            catch (Exception ex)
            {
                //IOLib.fileCheck_gamestorage.createFolderFail = false;
                IOLib.fileCheck_gamestorage.exception = ex;
                return;
            }

            DataStream.BeginReadWrite.BinaryIO(true, path, write, null, null, true);
        }

        public void write(BinaryWriter w)
        {
            w.Write(GameStorage.Version);

            w.Write(steamVersion);
            w.Write(date.ToBinary());
            w.Write(gameTime.Ticks);
            w.Write((byte)endReason);
            w.Write((byte)vType);
            w.Write(matchResults);

            difficulty.write(w);

            Debug.WriteCheck(w);

            w.Write((byte)players.Count);
            foreach (var p in players)
            {
                p.write(w);
            }

            Debug.WriteCheck(w);
        }

        public void read(BinaryReader r)
        {
            int storageVersion = r.ReadInt32();

            steamVersion = r.ReadInt32();
            date = DateTime.FromBinary(r.ReadInt64());
            gameTime = new TimeSpan(r.ReadInt64());
            endReason = (GameEndReason)r.ReadByte();
            vType = (VictoryType)r.ReadByte();
            matchResults = r.ReadBoolean();

            difficulty = new Difficulty();
            difficulty.read(r, storageVersion);

            Debug.ReadCheck(r);

            int playerCount = r.ReadByte();
            players = new List<GameOverResultPlayer>(playerCount);
            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new GameOverResultPlayer(r));
            }

            Debug.ReadCheck(r);
        }

        class GameOverResultPlayer
        {
            public string playerName;
            public bool casual;
            public Statistics statistics;
            public bool matchWinner = false;

            public GameOverResultPlayer(LocalPlayer player, MatchResult matchResult)
            {
                casual = player.profile.casualControls;
                playerName = player.Name;
                statistics = player.statistics;

                if (matchResult != null)
                {
                    foreach (var f in matchResult.winner)
                    {
                        if (f == player.faction)
                        {
                            matchWinner = true;
                        }
                    }
                }
            }

            public GameOverResultPlayer(BinaryReader r)
            {
                read(r);
            }

            public void write(BinaryWriter w)
            {
                w.Write(SaveGamestate.SubVersion);

                StreamLib.WriteString(w, playerName);
                w.Write(casual);
                statistics.writeGameState(w);
                w.Write(matchWinner);
            }

            public void read(BinaryReader r)
            {
                int metaVersion = r.ReadInt32();
#if DEBUG
                //temp fix
                if (metaVersion == 12)
                {
                    metaVersion = 88;
                }
#endif

                playerName = StreamLib.ReadString(r);
                casual = r.ReadBoolean();
                statistics = new Statistics();
                statistics.readGameState(r, metaVersion);
                matchWinner = r.ReadBoolean();
            }
        }
    }
}
