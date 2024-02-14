using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using VikingEngine.DataStream;
using Microsoft.Xna.Framework;
using VikingEngine.HUD;

namespace VikingEngine.LF2.Data
{
#if !CMODE
   
    /// <summary>
    /// General progress for all members in the world
    /// </summary>
    class Progress : IStreamIOCallback, ICompassIconLocation
    {
        public static readonly IntVector2 OldSwineLocationChunkDiff = new IntVector2(0, -3);
        public static readonly IntVector2 MessengerLocationChunkDiff = new IntVector2(-2, -3);
        BossKnowledge[] bossProgress;
        public GameObjects.NPC.i i = null;
        public Map.MiniMapGoal mapGoal = new Map.MiniMapGoal();
        int numHeroDeaths = 0;
        public int NumHeroDeaths { get { return numHeroDeaths; } }
        public void debugResetHeroDeaths() { numHeroDeaths = 0; }

        EightBit[] castleLootCrates;
        bool[,] visitedArea;
        
        public EightBit AliveMonstersSpawns = EightBit.AllTrue;
        public byte DestroyMonsterSpawnIndex = byte.MaxValue;

        public Progress()
        {
            bossProgress = new BossKnowledge[LootfestLib.BossCount];
            castleLootCrates = new EightBit[LootfestLib.BossCount];
            bossProgress[0].Immunity = true;
            bossProgress[0].Location = true;
            bossProgress[0].Weakness = true;

            visitedArea = new bool[MiniMap.MinimapAreas.X, MiniMap.MinimapAreas.Y];
            SetQuestGoal(null);
            progressChanged();
        }

        public void OpenLootCrate(int castleLvl, Corner c)
        {
            EightBit corners = castleLootCrates[castleLvl];
            corners.Set((int)c, true);
            castleLootCrates[castleLvl] = corners;
        }

        public bool HasLootCrate(int castleLvl, Corner c)
        {
            return !castleLootCrates[castleLvl].Get((int)c);
        }

        public void HeroDied()
        {
            numHeroDeaths++;
        }

        public bool ViewMapGoal
        {
            get
            {
                return Map.World.RunningAsHost && generalProgress < GeneralProgress.AskAround;
            }
        }
        
        GeneralProgress generalProgress = (GeneralProgress)0;
        public GeneralProgress GeneralProgress
        {
            get { return generalProgress; }
            set
            {
                generalProgress = value;
                promtMissionTimer.Reset();
                RemoveQuestExpression();
                //check if a character is gonna get !
                
                mapGoal.Visible = generalProgress < GeneralProgress.AskAround;
                if (mapGoal.Visible)
                {
                    mapGoal.Chunk = questPos();
                }
                if (generalProgress < GeneralProgress.GameComplete)
                {
                    LfRef.gamestate.UpdateMissioni();
                }
                progressChanged();

                if (Map.World.RunningAsHost && value == Data.GeneralProgress.AskAround)
                {
                    NetworkWriteProgress();
                }
            }
        }
        public void RemoveQuestExpression()
        {
            if (i != null)
            {
                i.DeleteMe();
            }
        }

        

        

        public void NetworkReadProgress(System.IO.BinaryReader r)
        {
            AliveMonstersSpawns.read(r);
            GeneralProgress = (GeneralProgress)r.ReadByte();
            DestroyMonsterSpawnIndex = r.ReadByte();

            int lenght = DataStream.DataStreamLib.ReadGrowingAddValue(r);
            List<IntVector3> start_length = new List<IntVector3>(lenght);
            for (int i = 0; i < lenght; i++)
            {
                int x = r.ReadByte();
                int y = r.ReadByte();
                int l = DataStream.DataStreamLib.ReadGrowingAddValue(r);


                start_length.Add(new IntVector3(x, y, l));
            }

            foreach (IntVector3 sl in start_length)
            {
                IntVector3 pos_l = sl;
                while (pos_l.Z > 0)
                {
                    visitedArea[pos_l.X, pos_l.Y] = true;
                    pos_l.Z--;

                    pos_l.X++;
                    if (pos_l.X >= MiniMap.MinimapAreas.X)
                    {
                        pos_l.X = 0;
                        pos_l.Y++;
                    }
                }
            }

            readBossProgress(r);
        }

        public void UnlockBossInfo(BossKnowledge info, int lvl)
        {
            BossKnowledge b = bossProgress[lvl];
            b.Location |= info.Location;
            b.Immunity |= info.Immunity;
            b.Weakness |= info.Weakness;
            bossProgress[lvl] = b;
            progressChanged();
        }

        public bool IsBossDefeated(int lvl)
        {
            return bossProgress[lvl].Defeated;
        }

        public bool BossKey(int lvl, GameObjects.Characters.Hero hero, bool local)
        {
            BossKnowledge b = bossProgress[lvl];
            if (!b.UnLocked)
            {
                b.UnLocked = true;
                bossProgress[lvl] = b;

                //message
                if (hero != null)
                    hero.Player.Print("Found boss key", SpriteName.IconBossKey);
                Music.SoundManager.PlayFlatSound(LoadedSound.Dialogue_QuestAccomplished);
                progressChanged();

                if (local)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_BossKey, Network.PacketReliability.Reliable);
                    w.Write((byte)lvl);
                }
                return true;
            }
            return false;
        }

        public bool GotBossKey(int lvl)
        {
            return bossProgress[lvl].UnLocked;
        }

        public void Save(bool save)
        {
            DataStream.BeginReadWrite.BinaryIO(save, new DataStream.FilePath(
                Data.WorldsSummaryColl.CurrentWorld.FolderPath, "Progress", ".LF2"), this.WriteSaveFile, this.ReadSaveFile,  this);
        }
        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (!save && completed)
            {
                progressChanged();
            }
        }

        
        void progressChanged()
        {
            Ref.netSession.setLobbyData(LF2.NetworkLib.ProgressNetProperty, PercentProgress.ByteVal);
        }

        public bool GetVisitedArea(IntVector2 pos)
        {
            return visitedArea[pos.X, pos.Y];
        }
        public void SetVisitedArea(IntVector2 chunkPos, GameObjects.Characters.Hero unlockMessage, bool netShare)
        {
            chunkPos.X /= MiniMap.ChunksDivide;
            chunkPos.Y /= MiniMap.ChunksDivide;

            IntVector2 pos;
            for (pos.Y = chunkPos.Y - 1; pos.Y <= chunkPos.Y + 1; pos.Y++)
            {
                if (pos.Y >= 0 && pos.Y < MiniMap.MinimapAreas.Y)
                {
                    for (pos.X = chunkPos.X - 1; pos.X <= chunkPos.X + 1; pos.X++)
                    {
                        if (pos.X >= 0 && pos.X < MiniMap.MinimapAreas.X)
                        {
                            visitedArea[pos.X, pos.Y] = true;
                        }
                    }
                }
            }

            if (unlockMessage != null)
            {
                unlockMessage.Player.MapLocationMessage();
            }

            if (netShare)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_SetVisitedArea, Network.PacketReliability.ReliableLasy);
                w.Write((byte)chunkPos.X);
                w.Write((byte)chunkPos.Y);

            }
        }

        public void SetClosestMonsterNest(IntVector2 chunk)
        {
            Map.Terrain.AreaInfo[] nests = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.EnemySpawn];
            ClosestV2 closest = new ClosestV2(chunk.Vec);
            for (int i = 0; i < nests.Length; i++)
            {
                closest.Next(nests[i].position.Vec, i);
            }
            LfRef.gamestate.Progress.DestroyMonsterSpawnIndex = (byte)closest.Result;
        }

        public void NetworkReadSetVisitedArea(System.IO.BinaryReader r)
        {
            IntVector2 chunkPos = new IntVector2(r.ReadByte(), r.ReadByte()) * MiniMap.ChunksDivide;
            SetVisitedArea(chunkPos, LfRef.LocalHeroes[0], false);
        }

        /// <returns>level 0-3</returns>
        public int ClientItemLevel()
        {
            int result = 0;

            if (generalProgress >= GeneralProgress.TalkToGranPa1)
            {
                ++result;
                if (generalProgress >= GeneralProgress.AskAround)
                {
                    ++result;
                }
            }

            int numBossDef = 0;
            for (int i = 1; i < bossProgress.Length; i++)
            {
                if (bossProgress[i].Defeated)
                    ++numBossDef;
            }
            if (numBossDef >= 3)
            {
                ++result;
            }
            return result;
        }

        public Percent PercentProgress
        {
            get
            {
                const float GeneralProgressPart = 0.3f;

                if (generalProgress == Data.GeneralProgress.GameComplete)
                    return Percent.Full;

                float result = (float)generalProgress / (float)GeneralProgress.NUM * GeneralProgressPart;

                float bossProgressPercent = 0;
                for (int i = 1; i < bossProgress.Length; i++)
                {
                    float boss = 0;
                    if (bossProgress[i].Immunity)
                    {
                        boss++;
                    }
                    if (bossProgress[i].Location)
                    {
                        boss++;
                    }
                    if (bossProgress[i].UnLocked)
                    {
                        boss++;
                    }
                    if (bossProgress[i].Weakness)
                    {
                        boss++;
                    }
                    if (bossProgress[i].Defeated)
                    {
                        boss+= 3;
                    }

                    bossProgressPercent += (boss / 7f) / (bossProgress.Length - 1);
                }

                result += bossProgressPercent * (1 - GeneralProgressPart);

                return new Percent(result);
            }
        }

        public void DefeatedBoss(int lvl)
        {
            BossKnowledge bl = new BossKnowledge();
            bl.Immunity = true;
            bl.Location = true;
            bl.Weakness = true;
            bl.Defeated = true;
            bl.UnLocked = true;
            bossProgress[lvl] = bl;

            if (lvl == LootfestLib.BossCount - 1)
            {
                //defeated head boss
                new QuestDialogue(
                    new List<IQuestDialoguePart>
                    {
                        new QuestDialogueSpeach("Congratulation! The evil magician is finally defeated", LoadedSound.Dialogue_QuestAccomplished),
                        new QuestDialogueSpeach("Maybe you should go to your father and receive his gratitude!", LoadedSound.Dialogue_DidYouKnow),
                        new QuestDialogueSpeach("See you in Lootfest3!", LoadedSound.Dialogue_Neutral),//"You can keep playing or generate a new world"),

                    }, new StringObject("Announcement"),LfRef.gamestate.LocalHostingPlayer);
                generalProgress = Data.GeneralProgress.GameComplete;
            }

            progressChanged();
        }

        public void DebugUnlockAllBosses()
        {
            for (int i = 0; i < LootfestLib.BossCount; i++)
            {
                BossKnowledge bl = new BossKnowledge();
                bl.Immunity = true;
                bl.Location = true;
                bl.Weakness = true;
                bl.UnLocked = true;
                bossProgress[i] = bl;
            }
        }

        public void NetworkWriteProgress()
        {
            List<IntVector3> start_length = new List<IntVector3>();
            bool isCounting = false;
            IntVector3 current = IntVector3.Zero;
            ForXYLoop loop = new ForXYLoop(MiniMap.MinimapAreas);
            while (!loop.Done)
            {
                IntVector2 pos = loop.Next_Old();
                if (visitedArea[pos.X, pos.Y])
                {
                    if (isCounting)
                    {
                        current.Z++;
                    }
                    else
                    {
                        current = new IntVector3(pos.X, pos.Y, 1);
                        isCounting = true;
                    }
                }
                else if (isCounting)
                {
                    isCounting = false;
                    start_length.Add(current);
                }
            }

            if (isCounting)
            {
                start_length.Add(current);
            }

            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_GameProgress,
                 Network.PacketReliability.ReliableLasy);

            //nests
            AliveMonstersSpawns.write(w);
            //general
            w.Write((byte)generalProgress);
            w.Write(DestroyMonsterSpawnIndex);
            //map
            DataStream.DataStreamLib.WriteGrowingAddValue(w, start_length.Count);
            foreach (IntVector3 pos_l in start_length)
            {
                w.Write((byte)pos_l.X);
                w.Write((byte)pos_l.Y);
                DataStream.DataStreamLib.WriteGrowingAddValue(w, pos_l.Z);
            }

            //boss
            writeBossProgress(w);
        }


        const byte ReleaseVersion = 1;

        public void WriteSaveFile(System.IO.BinaryWriter w)
        {
            const byte Version = 4; //First release is "1"
            w.Write(Version);

            IntVector2 pos = IntVector2.Zero;
            for (pos.Y = 0; pos.Y < MiniMap.MinimapAreas.Y; pos.Y++)
            {
                for (pos.X = 0; pos.X < MiniMap.MinimapAreas.X; pos.X++)
                {
                    w.Write(visitedArea[pos.X, pos.Y]);
                }
            }
            AliveMonstersSpawns.write(w);

            w.Write((byte)GeneralProgress);
            

            writeBossProgress(w);

            w.Write((ushort)numHeroDeaths);

            LfRef.gamestate.WriteMessageHistory(w);
            
            //Update1
            w.Write(DestroyMonsterSpawnIndex);

            //Update2
            for (int i = 0; i < LootfestLib.BossCount; ++i)
            {
                castleLootCrates[i].write(w);
            }
        }
        public void ReadSaveFile(System.IO.BinaryReader r)
        {
           
            try
            {
                byte version = r.ReadByte();

                IntVector2 pos = IntVector2.Zero;
                for (pos.Y = 0; pos.Y < MiniMap.MinimapAreas.Y; pos.Y++)
                {
                    for (pos.X = 0; pos.X < MiniMap.MinimapAreas.X; pos.X++)
                    {
                        visitedArea[pos.X, pos.Y] = r.ReadBoolean();
                    }
                }
                //nests
                AliveMonstersSpawns.read(r);
                //general
                GeneralProgress = (GeneralProgress)r.ReadByte();
               
                //map
                if (GeneralProgress == Data.GeneralProgress.KillOldSwine)
                {
                    GeneralProgress--;
                }
                else if (GeneralProgress == Data.GeneralProgress.HelpMessenger)
                {
                    GeneralProgress = Data.GeneralProgress.TalkToFather_MessengerReset;
                }
                readBossProgress(r);
                numHeroDeaths = r.ReadUInt16();

                if (version < 2)
                {
                    if (generalProgress == Data.GeneralProgress.GameComplete)
                        generalProgress = Data.GeneralProgress.AskAround;
                }

                if (version > ReleaseVersion)
                {
                    LfRef.gamestate.ReadMessageHistory(r);
                }

                //Update1
                DestroyMonsterSpawnIndex = r.ReadByte();

                //Update2
                if (version >= 4)
                {   
                    for (int i = 0; i < LootfestLib.BossCount; ++i)
                    {
                        castleLootCrates[i] = EightBit.FromStream(r);
                    }
                }
            }
            catch (Exception e)
            {
                if (PlatformSettings.DevBuild) throw;
                   
                Debug.LogError( "read progress, " + e.Message);
            }

            if (generalProgress < Data.GeneralProgress.GameComplete)
                SetQuestGoal(null);
        }

        void writeBossProgress(System.IO.BinaryWriter w)
        {
            for (int i = 0; i < bossProgress.Length; i++)
            {
                bossProgress[i].WriteStream(w);
            }
        }
        void readBossProgress(System.IO.BinaryReader r)
        {
            for (int i = 0; i < bossProgress.Length; i++)
            {
                BossKnowledge bn = new BossKnowledge();
                bn.ReadStream(r);
                bossProgress[i] = bn;
            }

            bossProgress[0].Immunity = true;
            bossProgress[0].Location = true;
            bossProgress[0].Weakness = true;

        }

        public void SetQuestGoal(GameObjects.Characters.Hero hero)
        {
            IntVector2 pos = questPos();

            mapGoal.Visible = true;
            mapGoal.Chunk = pos;
            SetVisitedArea(pos, hero, false);
            if (hero != null)
                hero.Player.NewMapLocation(pos);
        }

        public IntVector2 questPos()
        {
            IntVector2 pos = IntVector2.Zero;
            
            if (GeneralProgress == GeneralProgress.TalkToFather_MessengerReset || GeneralProgress == GeneralProgress.TalkToFather1_PickGifts ||
                GeneralProgress == GeneralProgress.TalkToFather2_HogKilled || GeneralProgress == GeneralProgress.TalkToFather3_GotMessage ||
                generalProgress == Data.GeneralProgress.HorseTravel)
            {
                pos = LfRef.worldOverView.GetAreaObject(
                    new Map.ChunkData(Map.Terrain.AreaType.HomeBase, 0)).AreaChunkCenter;
            }
            else if (generalProgress == Data.GeneralProgress.KillOldSwine)
            {
                pos = LfRef.worldOverView.GetAreaObject(
                    new Map.ChunkData(Map.Terrain.AreaType.HomeBase, 0)).AreaChunkCenter;
                pos += OldSwineLocationChunkDiff;
            }
            else if (generalProgress == Data.GeneralProgress.HelpMessenger)
            {
                pos = LfRef.worldOverView.GetAreaObject(
                    new Map.ChunkData(Map.Terrain.AreaType.HomeBase, 0)).AreaChunkCenter;
                pos += MessengerLocationChunkDiff;
            }
            else if (GeneralProgress == GeneralProgress.TalkToGuardCaptain1 || GeneralProgress == GeneralProgress.TalkToGuardCaptain2)
            {
                pos = LfRef.worldOverView.GetAreaObject(
                new Map.ChunkData(Map.Terrain.AreaType.Village, 0)).AreaChunkCenter;
            }
            //else if (GeneralProgress == GeneralProgress.DestroyMonsterSpawn)
            //{
            //    pos = LfRef.worldOverView.GetAreaObject(
            //       new Map.ChunkData(Map.Terrain.AreaType.EnemySpawn, 0)).AreaChunkCenter;
            //}
            else if (GeneralProgress == GeneralProgress.DestroyMonsterSpawn)
            {
                //byte ix = DestroyMonsterSpawnIndex;
                if (DestroyMonsterSpawnIndex == byte.MaxValue)
                {
                    LfRef.gamestate.Progress.SetClosestMonsterNest(LfRef.LocalHeroes[0].ScreenPos);
                }

                pos = LfRef.worldOverView.GetAreaObject(
                    new Map.ChunkData(Map.Terrain.AreaType.EnemySpawn, DestroyMonsterSpawnIndex)).AreaChunkCenter;
            }
            else if (GeneralProgress == GeneralProgress.TalkToGranPa1 || GeneralProgress == GeneralProgress.TalkToGranPa2_BoughtApples ||
                GeneralProgress == GeneralProgress.TalkToGranPa3_HasApplePie || GeneralProgress == GeneralProgress.CraftPie)
            {
                pos = LfRef.worldOverView.GetAreaObject(
                    new Map.ChunkData(Map.Terrain.AreaType.City, 0)).AreaChunkCenter;
            }
            else if (GeneralProgress == GeneralProgress.TalkToWarVeteran)
            {
                pos = LfRef.worldOverView.GetAreaObject(
                   new Map.ChunkData(Map.Terrain.AreaType.City, 1)).AreaChunkCenter;
            }
            else if (GeneralProgress == GeneralProgress.AskAround)
            {
                pos = LfRef.worldOverView.GetAreaObject(
                   new Map.ChunkData(Map.Terrain.AreaType.Castle, 0)).AreaChunkCenter;
            }
           
            else if (PlatformSettings.ViewErrorWarnings)
            {
                throw new Exception("no questPos");
            }
            return pos;
        }

        public void QuestLog(Gui menu)
        {            
            const string UnKnown = "???";

            var titleFormat = menu.style.textFormat;
            titleFormat.Font = LoadedFont.Bold;
            titleFormat.size *= 1.1f;

            GuiLayout layout = new GuiLayout("Quest Log", menu, GuiLayoutMode.MultipleColumns, null);
            {
                layout.scrollOnly = true;

                //File file = new File((int)Players.MenuPageName.MainMenu);
                for (int i = 0; i < LootfestLib.BossCount; i++)
                {
                    BossKnowledge knowledge = bossProgress[i];
                    string title = "Magician" + (i + 1).ToString();
                    if (knowledge.Defeated)
                        title += " - defeated!";
                    else if (knowledge.UnLocked)
                        title += " - unlocked!";


                    new GuiLabel(title, true, titleFormat, layout);

                    //file.TextBoxTitle(title);
                    Data.Characters.MagicianData data = LfRef.worldOverView.Boss(i);

                    string location;
                    if (knowledge.Location)
                    {
                        location = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.Castle][i].Name;
                    }
                    else
                    {
                        location = UnKnown;
                    }
                    new GuiLabel("Location: " + location, layout);
                    new GuiLabel("Weakness: " + (knowledge.Weakness ? data.Weakness.ToString() : UnKnown), layout);
                    new GuiLabel("Immune to: " + (knowledge.Immunity ? data.Immune.ToString() : UnKnown), layout);
                }
            }
            layout.End();
            //return file;
        }


        Timer.Basic promtMissionTimer = new Timer.Basic(TimeExt.MinutesToMS(1));
        //bool gotTipTutorial = false;
        //public void Update(float time, PlayState state)
        //{
        //    //if (!gotTipTutorial)
        //    //{
        //    //    if (promtMissionTimer.Update(time))
        //    //    {
        //    //        gotTipTutorial = true;
        //    //        state.LocalHostingPlayer.startButtonTutorial(new HUD.ButtonTutorial(SpriteName.BoardCtrlBACK, 
        //    //            numBUTTON.Back, "View tip on the current mission", state.LocalHostingPlayer.SafeScreenArea));
        //    //        //promtMissionTimer.Reset();
        //    //        //state.localHostingPlayerPrint(missionHelp(), SpriteName.IconInfo);
        //    //    }
        //    //}
        //}

        bool newMission = true;
        int currentTip = 0;
        public string missionHelp(Players.PlayerProgress player)
        {
            if (newMission)
            {
                currentTip = 0;
                newMission = false;
            }
            const string Abouti = "The '!' symbol shows where to go next";
            const string Compass = "The '!' on the compass will show the direction";
            const string QuickTravel = "You can pay the wagon driver for a quick travel";

            List<string> help = null;

            switch (GeneralProgress)
            {
                default:
                    if (GeneralProgress == GeneralProgress.TalkToGuardCaptain1 || GeneralProgress == GeneralProgress.TalkToGuardCaptain2)
                    {
                        help = new List<string>
                        {
                            "Talk to the guard captain",
                            "The guard captain wears a red suit",
                            "The guard captain is walking around in the village",
                            "You will find the village on the map",
                            Abouti,
                            Compass,
                        };
                        if (GeneralProgress == GeneralProgress.TalkToGuardCaptain1)
                        {
                            help.Add(QuickTravel);
                        }
                    }
                    break;
                case Data.GeneralProgress.TalkToFather1_PickGifts:
                    if (player.TakenGifts)
                    {
                        help = new List<string>
                        {
                            "Talk to your father, just where you started",
                            "Get your first quest from your father",
                            Abouti,
                            Compass,
                        };
                    }
                    else
                    {
                        help = new List<string>
                        {
                            "You need to pick your startup gear",
                            "Take the weapons your father is offering",
                            "Talk to your father, just where you started",
                        };
                       
                    }
                    break;
                case Data.GeneralProgress.KillOldSwine:
                    help = new List<string>
                        {
                            "Kill the old hog",
                            Abouti,
                            Compass,
                        };
                    break;
                case GeneralProgress.TalkToFather2_HogKilled:
                    help = new List<string>
                    {
                        "Your father have more info for you",
                        "Go back to where you started",
                        Abouti,
                        Compass,
                    };
                    break;
                case GeneralProgress.TalkToFather_MessengerReset:
                    help = new List<string>
                    {
                        "Your father has a quest for you",
                        Abouti,
                        Compass,
                    };
                    break;
                case GeneralProgress.HelpMessenger:
                    help = new List<string>
                    {
                        "Help the messenger",
                        "Kill all the attacking monsters",
                        Abouti,
                        Compass,
                    };
                    break;
                case GeneralProgress.TalkToFather3_GotMessage:
                    help = new List<string>
                    {
                        "Bring the message back to your father",
                        "Head back to the starting location",
                        Abouti,
                        Compass,
                    };
                    break;
                case Data.GeneralProgress.HorseTravel:
                    help = new List<string>
                    {
                        "Talk to the wagoner",
                        "You can quick travel by horse"
                    };
                    break;
                
                case GeneralProgress.TalkToGranPa1:
                    help = new List<string>
                        {
                            "Locate the GranPa in the city",
                            Abouti,
                            Compass,
                            QuickTravel,
                        };
                    break;
                case GeneralProgress.TalkToWarVeteran:
                    help = new List<string>
                        {
                            "Talk to the war veteran",
                            Abouti,
                            Compass,
                            QuickTravel,
                        };
                    break;
                case GeneralProgress.TalkToGranPa2_BoughtApples:
                    help = new List<string>
                        {
                            "Bring " + Data.Gadgets.BluePrintLib.ApplePieNumApples.ToString() + "apples to granpa",
                            "You can buy apples from a salesman"
                        };
                    break;
                case GeneralProgress.TalkToGranPa3_HasApplePie:
                    help = new List<string>
                        {
                            "Give granpa an apple pie",
                        };
                    break;
                case GeneralProgress.CraftPie:
                    help = new List<string>
                        {
                            "You will find the cook in the city",
                            "Craft an apple pie from apples and seed",
                        };
                    break;

                case GeneralProgress.DestroyMonsterSpawn:
                    help = new List<string>
                        {
                            "Destroy the egg nest",
                            Abouti,
                            Compass,
                        };
                    break;
                case GeneralProgress.AskAround:
                    help = new List<string>
                        {
                            "Go around and ask people to find more clues",
                            "You have a quest log in the pause menu",
                            "Each magician has a weakness, something to consider when you craft new weapons",
                            "There is a bunch on secrets to find if you walk around",
                            "You need to get rid of the evil magicians",
                        };
                    break;
                case Data.GeneralProgress.GameComplete:
                    help = new List<string>
                        {
                            "The game is completed",
                            "If you unlock all the trophies, you will get a reward",
                            "Each new world you start will be different",
                        };
                    break;
            }

            if (currentTip >= help.Count)
                currentTip = 0;
            string result = help[currentTip];
            currentTip++;
            return result;
        }


        public void ReadQuestDialogue(System.IO.BinaryReader r)
        {
            if (Ref.netSession.IsHost)
            {
                new HostClientMixUpException("QuestDialogue");
            }
            GeneralProgress quest = (Data.GeneralProgress)r.ReadByte();
            BeginQuestDialogue(quest, LfRef.gamestate.LocalHostingPlayer);
        }

        /// <summary>
        /// Dialogue that should be shared over network
        /// </summary>
        public static void BeginQuestDialogue(GeneralProgress quest, Players.Player player)
        {
            List<IQuestDialoguePart> say = null;
            GameObjects.EnvironmentObj.MapChunkObjectType speaker = GameObjects.EnvironmentObj.MapChunkObjectType.NUM_NONE;
            string name = null;
            switch (quest)
            {
                case Data.GeneralProgress.TalkToFather1_PickGifts:
                    say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Take a few practice swings first", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Try to defeat that old and limp hog over there", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Should be a fair challenge for you, he he!", LoadedSound.Dialogue_DidYouKnow),
                        };
                    missionTipTutorial(player.inputMap, say);
                    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Father;
                    break;
                case Data.GeneralProgress.TalkToFather2_HogKilled:
                    say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Congratulations!", LoadedSound.Dialogue_QuestAccomplished),
		                    new QuestDialogueSpeach("You've completed a quest with no challenge whatsoever!", LoadedSound.Dialogue_DidYouKnow),
		                    new QuestDialogueSpeach("About that, there is a messenger over there...", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Give him a hand!", LoadedSound.Dialogue_DidYouKnow),
                            
                        };
                    
                    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Father;
                    break;
                case Data.GeneralProgress.HelpMessenger:
                    say = new List<IQuestDialoguePart>
                        {
		                    new QuestDialogueSpeach("I have an important message:", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueSpeach("The town is plagued by waves of monsters", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueSpeach("We need help to fend them off", LoadedSound.Dialogue_Neutral),
                        };
                    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Messenger;
                    missionTipTutorial(player.inputMap, say);
                    break;
                case Data.GeneralProgress.TalkToFather3_GotMessage:
                    say = new List<IQuestDialoguePart>
                        {
		                    new QuestDialogueSpeach("Monsters.. Hmm...", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueSpeach("As a member of the clan, you are expected to be there and help them out", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Take a ride in and talk to the guard captain", LoadedSound.Dialogue_DidYouKnow),
                        };
                    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Father;
                    break;
                case Data.GeneralProgress.HorseTravel:
                    say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("I'll mark the town on your map", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueQuestMapLocation(quest +1),
                            new QuestDialogueSpeach("Either pay " + LootfestLib.TravelCost.ToString() + LootfestLib.MoneyText+" for a quick travel or take a walk", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Your choice...", LoadedSound.Dialogue_DidYouKnow),
                        };

                    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport;
                    break;
                case Data.GeneralProgress.TalkToGuardCaptain1:
                    LfRef.gamestate.Progress.SetClosestMonsterNest(player.hero.ScreenPos);
                    //Map.Terrain.AreaInfo[] nests = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.EnemySpawn];
                    //ClosestV2 closest = new ClosestV2(player.hero.ScreenPos.Vec);
                    //for (int i = 0; i < nests.Length; i++)
                    //{
                    //    closest.Next(nests[i].position.Vec, i);
                    //}
                    //LfRef.gamestate.Progress.DestroyMonsterSpawnIndex = (byte)closest.Result;

                    say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("We suspect the monster attacks are coming from a nearby nest", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueSpeach("Locate and destroy!", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueQuestMapLocation(quest + 1),
                            new QuestDialogueSpeach("Dismiss!", LoadedSound.Dialogue_DidYouKnow),
                            viewMapTutorial(player.inputMap), 
                        };
                    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain;
                    break;
                case Data.GeneralProgress.TalkToGuardCaptain2:
                    say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Good work!", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueSpeach("However, the monsters keep attacking us.", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("You should talk to the wise man in the " + LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.City][0].Name, LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueSpeach("He might have a clue where they come from", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueQuestMapLocation(quest + 1),
                            new QuestDialogueSpeach("Here is some money for the travel costs", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueItemGift(new GameObjects.Gadgets.Item( GameObjects.Gadgets.GoodsType.Coins,  LootfestLib.TravelCost)),
                        };
                    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain;
                    break;
                 case Data.GeneralProgress.TalkToGranPa1:
                    say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Oh dear! You look skinny!", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueSpeach("We definitely won't solve any monster problems on empty stomachs!", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Let's share an apple pie", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("But first we need " + Data.Gadgets.BluePrintLib.ApplePieNumApples.ToString() + "apples", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueSpeach("You can buy them from a salesman. This should cover it:", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueItemGift(new GameObjects.Gadgets.Item(GameObjects.Gadgets.GoodsType.Coins, 1)),
                         };
                     speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Granpa;
                     break;
                  case Data.GeneralProgress.TalkToGranPa2_BoughtApples:
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("How much did you say the apples cost!?", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueSpeach("The prices must have gone up! I remember when I could get them for just half a copper", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Emm.. where was I?", LoadedSound.Dialogue_Question),
                            new QuestDialogueSpeach("Oh, apple pie! That's right! Take these seeds with the apples to the cook:", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueItemGift(new GameObjects.Gadgets.Goods( GameObjects.Gadgets.GoodsType.Seed, GameObjects.Gadgets.Quality.High, Data.Gadgets.BluePrintLib.ApplePieNumSeed)),
                        };
                        speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Granpa;
                        break;
                  case Data.GeneralProgress.CraftPie:
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("If I've got pie today? Emm.. let me check", LoadedSound.Dialogue_Question),
                            new QuestDialogueSpeach("*nom nom!*", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Yes they are delicous, with a taste of.. wait!", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("*nom nom!*", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("..cinnamon. Just select apple pie in the crafting menu", LoadedSound.Dialogue_DidYouKnow),
                        };
                        speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Cook;//name = Data.Characters.WorkerCook.Name;
                        break;
                  case Data.GeneralProgress.TalkToGranPa3_HasApplePie:
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Mmm! Delicious!", LoadedSound.Dialogue_QuestAccomplished),
                            new QuestDialogueSpeach("Now, there is a rumor about evil magicians having risen from a tomb", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("I suspect that they are the ones invoking the evil monsters", LoadedSound.Dialogue_Neutral), 
                            new QuestDialogueSpeach("There is a war veteran that claims to have seen one of the magicians!", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueQuestMapLocation(),
                            
                        };
                        speaker = GameObjects.EnvironmentObj.MapChunkObjectType.Granpa;
                        break;
                    case GeneralProgress.TalkToWarVeteran:
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Oh, I have seen some evil magic around here!", LoadedSound.Dialogue_QuestAccomplished),
	                        new QuestDialogueSpeach("They are hiding in castles, I've spotted one over here:", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueQuestMapLocation(),
                            viewMapTutorial(player.inputMap),
                        };
                        speaker = GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran;
                        break;
                    case GeneralProgress.WarVeteranInfo_Gossip:
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("I suggest you ask around among the people who work here", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueSpeach("Someone might have heard rumors about the magicians", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Some of that intel could be very useful", LoadedSound.Dialogue_Neutral),
                        };
                        speaker = GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran;
                        break;
                    case GeneralProgress.WarVeteranInfo_Progress:
                        player.SetQuestLogTutorial();
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("Use your Quest Log to keep track of the info you recieved", LoadedSound.Dialogue_DidYouKnow),
                            new QuestDialogueButtonTutorial(player.inputMap.menuInput.OpenCloseController, null, 
                            "View the Quest Log in the pause menu"),
                        };
                        speaker = GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran;
                        break;
                    case GeneralProgress.WarVeteranInfo_Goal:
                        say = new List<IQuestDialoguePart>
                        {
                            new QuestDialogueSpeach("You have to find the evil magicians and bring the fight to them", LoadedSound.Dialogue_Neutral),
                            new QuestDialogueSpeach("Find their weaknesses and destroy them!", LoadedSound.Dialogue_DidYouKnow),
                        };
                        speaker = GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran;
                        break;
                    //case GeneralProgress.WarVeteranInfo_WeaponBelt:
                    //    player.settings.UnlockedWeaponSetups = true;
                    //    say = new List<IQuestDialoguePart>
                    //    {
                    //        new QuestDialogueSpeach("A true warrior needs a good weapon belt", LoadedSound.Dialogue_Neutral),
                    //        new QuestDialogueSpeach("With it, you can quickly swap between different weapon setups", LoadedSound.Dialogue_DidYouKnow),
                    //        new QuestDialogueButtonTutorial(SpriteName.BoardCtrlRB, numBUTTON.RB, null, "Use LB or RB to change weapon setup"),
                    //    };
                    //    speaker = GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran;
                    //    break;

                
            }
            name = TextLib.EnumName(speaker.ToString());

            if (Map.World.RunningAsHost)
            {
                //network share
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_QuestDialogue, Network.PacketReliability.ReliableLasy);
                w.Write((byte)quest);
            }
            else
            {
                name = name + " to " + Ref.netSession.Host().Gamertag;
            }
            new QuestDialogue(say, name, player);
        }

        static void missionTipTutorial(Players.InputMap inputMap, List<IQuestDialoguePart> say)
        {
            if (Map.World.RunningAsHost)
            {
                say.Add(new QuestDialogueButtonTutorial(inputMap.viewHelp, null, LootfestLib.ViewBackText + " to view tip on the current mission (toggle to view more tips)"));
                say.Add(new QuestDialogueLargeCompassPulse());
            }
        }
        static IQuestDialoguePart viewMapTutorial(Players.InputMap inputMap)
        {
            return new QuestDialogueButtonTutorial(inputMap.viewMap, null, "To view the location on the map");
        }
        public Vector2 CompassIconLocation { get { return i == null? mapGoal.PlanePos : i.PlanePos; } }
        public SpriteName CompassIcon { get { return SpriteName.IconMapQuest; } }
        public bool CompassIconVisible { get { return ViewMapGoal; } }
    }

    class StringObject
    {
        string name;
        public StringObject(string name)
        {
            this.name = name;
        }
        public override string ToString()
        {
            return name;
        }
    }

    struct BossKnowledge
    {
        public bool Location;
        public bool Weakness;
        public bool Immunity;
        public bool Defeated;
        public bool UnLocked;

        public void WriteStream(System.IO.BinaryWriter w)
        {
            EightBit value = new EightBit();
            value.Set(0, Location);
            value.Set(1, Weakness);
            value.Set(2, Immunity);
            value.Set(3, Defeated);
            value.Set(4, UnLocked);
            value.write(w);
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            EightBit value = EightBit.FromStream(r);
            Location = value.Get(0);
            Weakness = value.Get(1);
            Immunity = value.Get(2);
            Defeated = value.Get(3);
            UnLocked = value.Get(4);
        }
    }

    enum GeneralProgress
    {
        TalkToFather1_PickGifts,
        KillOldSwine,
        TalkToFather2_HogKilled,
        TalkToFather_MessengerReset,
        HelpMessenger,
        //TalkToMessenger,
        TalkToFather3_GotMessage,
        HorseTravel,
        TalkToGuardCaptain1,
        DestroyMonsterSpawn,
        TalkToGuardCaptain2,
        TalkToGranPa1,
        TalkToGranPa2_BoughtApples,
        CraftPie,
        TalkToGranPa3_HasApplePie,
        TalkToWarVeteran,
        AskAround,
        OpenedEndTomb,
        GameComplete,
        NUM,
        WarVeteranInfo_Gossip,
        WarVeteranInfo_Progress,
        WarVeteranInfo_Goal,
    }
#endif
}
