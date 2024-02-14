//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
////
//using VikingEngine.DataStream;

//namespace VikingEngine.PJ
//{
//    class Highscore
//    {
//        List<LocalScore> localScores = new List<LocalScore>();
//        LeaderBoard WorldScores = new LeaderBoard();
//        LeaderBoard FriendScores = new LeaderBoard();
//        public LeaderBoard activeFriends = new LeaderBoard();

//        Timer.Basic networkUpdate = new Timer.Basic(lib.SecondsToMS(5), true);
//        bool currentlySendingFriendsList = true;
//        int sendingScoreIndex = 0;

//        public Highscore()
//        { }

//        public void Update()
//        {
            
//            if (Network.Session.Connected)
//            {
//                if (networkUpdate.Update())
//                {
//                    LeaderBoard list = currentlySendingFriendsList ? FriendScores : WorldScores;
//                    if (list.Count == 0)
//                    {
//                        currentlySendingFriendsList = true;
//                    }
//                    else
//                    {
//                        if (list.netWorkShare(ref sendingScoreIndex))
//                        {
//                            currentlySendingFriendsList = !currentlySendingFriendsList;
//                        }
//                    }
//                }

//                if (Ref.netSession.IsHost && Ref.netSession.IsFull)
//                {
//                    List<Network.AbsNetworkPeer> remoteGamers = Ref.netSession.AllRemoteGamers();
//                    for (int i = 0; i < remoteGamers.Count; ++i)
//                    {
//                        if (!Ref.netSession.IsFriend(remoteGamers[i]))
//                        {
//                            Ref.netSession.RemovePlayer(remoteGamers[i]);
//                            return;
//                        }
//                    }
//                }
//            }
//        }
        

//        public void netReadScore(System.IO.BinaryReader r, Network.AbsNetworkPeer sender)
//        {
//            GamerScore score = new GamerScore(r);

//            score.Friend = Ref.netSession.IsSystemLink || Ref.netSession.IsFriend(score.Name);
//            addScore(score);

//            Debug.Log( DebugLogType.Output, "Net recive score: " + score.ToString());
//        }

//        public void addScore(GamerScore gamerScore)
//        {
//            if (gamerScore.Friend)
//            {
//                FriendScores.add(gamerScore);
//            }
//            WorldScores.add(gamerScore);
//        }

//        public void NewScore(int score, Gamer player)
//        {
//            newHighScore = false;

//            string name = player.Name;
//            LocalScore localScore = tryGetLocalScore(name);

//            if (localScore == null)
//            {
//                localScore = new LocalScore(name, player.playerData.OnlineSessionsPrivilege, score);
//                localScores.Add(localScore);
//                newHighScore = true;
//            }
//            else
//            {
//                if (score > localScore.score)
//                {
//                    localScore.score = score;
//                    newHighScore = true;
//                }
//                localScore.deathcount++;
//                localScore.GoldMembership = player.playerData.OnlineSessionsPrivilege;
//            }

//            if (newHighScore)
//            {
//                //Play sound
//                //--
//                WorldScores.AddLocalScore(localScore);
//                FriendScores.AddLocalScore(localScore);
//            }
//        }

//        public LocalScore tryGetLocalScore(string player)
//        {
//            //name = Engine.XGuide.GetPlayer(player).PublicName;

//            for (int index = 0; index < localScores.Count; ++index)
//            {
//                if (localScores[index].name == player)
//                    return localScores[index];
//            }

//            return null;
//        }

//        public void DebugTest()
//        {
//            for (int i = 0; i < 400; ++i)
//            {
//                if (Ref.rnd.RandomChance(0.2f))
//                {
//                    addScore(new GamerScore("Friend" + i.ToString(), Ref.rnd.Int(50), true));
//                }
//                else
//                {
//                    addScore(new GamerScore("Player" + i.ToString(), Ref.rnd.Int(50), false));
//                }
//            }

//            for (int i = 0; i < 10; ++i)
//            {
//                addScore(new GamerScore("Doublett test", 51 + Ref.rnd.Int(10), true));
//            }
//        }

//        /// <summary>
//        /// Dynamically view the most interesting scores for the player
//        /// </summary>
//        public void UpFrontScores(LootFest.File file, List<Gamer> gamers)
//        {
//            ListToMenu(true, file, gamers, false);
//            if (FriendScores.Count <= 5)
//            {
//                ListToMenu(false, file, gamers, false);
//            }
//        }

//        public void ListToMenu(bool friends, LootFest.File file, List<Gamer> gamers, bool emptyText)
//        {
//            if (friends)
//            {
//                List<string> activeGamers = new List<string>(gamers.Count);
//                foreach (Gamer g in gamers)
//                    activeGamers.Add(g.Name);

//                if (activeFriends.Count > 0)
//                {
//                    file.AddDescription("Playing now");
//                    activeFriends.ListToMenu(file, null);
//                    activeFriends.GamertagsTo(activeGamers);

//                }
//                if (FriendScores.Count > activeGamers.Count)
//                {
//                    file.AddDescription("Friends");
//                    FriendScores.ListToMenu(file, activeGamers);
//                }
//            }
//            else
//            {
//                if (WorldScores.Count == 0)// || WorldScores.Count <= FriendScores.Count)
//                {
//                    if (emptyText)
//                        file.AddDescription("Empty");
//                }
//                else
//                {
//                    file.AddDescription("World");
//                    WorldScores.ListToMenu(file, null);
//                }
//            }
//        }
//        public void LocalListToMenu(LootFest.File file)
//        {
//            for (int i = 0; i < localScores.Count; ++i)
//            {
//                BirdLib.playerNameText(localScores[i].name, file);
//                BirdLib.playerScoreText(localScores[i].score.ToString(), file);

//            }
//        }
//        public void ActivePlayersToMenu(LootFest.File file, List<Gamer> gamers)
//        {
//            List<LocalScore> scores = new List<LocalScore>(gamers.Count); 

//            foreach (Gamer g in gamers)
//            {
//                LocalScore score = tryGetLocalScore(g.Name);
//                if (score != null)
//                {
//                    scores.Add(score);
//                    //BirdLib.playerNameText(score.name, file);
//                    //BirdLib.playerScoreText(score.score.ToString(), file);
//                }
//            }

//            LocalScore[] sorted = scores.ToArray();
//            if (scores.Count > 1)
//            {
//                arraylib.Quicksort(sorted, false);
//            }

//            for (int i = 0; i < lib.SmallestValue(sorted.Length, 4); ++i )//foreach (LocalScore s in sorted)
//            {
//                BirdLib.playerNameText(sorted[i].name, file);
//                BirdLib.playerScoreText(sorted[i].score.ToString(), file);
//            }

//        }

//        public static readonly FilePath FilePath = new FilePath("", "highscores", ".dat", true, true);

//        public void BeginSave()
//        {
//            const float ViewMessageTime = 800;
//            const float FadeOutTime = 500;

//            Graphics.TextS saveMessage = new Graphics.TextS(LoadedFont.PhoneText,
//                Engine.Screen.SafeArea.BottomLeft, new Vector2(0.8f), new Graphics.Align(new Vector2(0, 1)), "Saving...", Color.White, ImageLayers.Background8);
//            Graphics.Motion2d fadeout = new Graphics.Motion2d(Graphics.MotionType.TRANSPARENSY, saveMessage, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, FadeOutTime, false);
//            new Timer.UpdateTrigger(ViewMessageTime, fadeout, true);
//            new Timer.Terminator(ViewMessageTime + FadeOutTime + 500, saveMessage);

//            Engine.Storage.AddToSaveQue(saveThread, true);
//        }

//        void saveThread(bool saveThread)
//        {
//            DataStream.MemoryStreamHandler data = new DataStream.MemoryStreamHandler();
//            WriteSaveFile(data.GetWriter());
//            data.WriteChecksum();

//            //public static void Write(FilePath file, byte[] data)
            
//            DataStream.DataStreamHandler.Write(FilePath, data.ByteArray());

//        }

//        public void LoadThread()
//        {
//            DataStream.MemoryStreamHandler data = new DataStream.MemoryStreamHandler();
//            data.SetByteArray(DataStream.DataStreamHandler.Read(FilePath));

//            data.ReadCheckSum();
//            if (data.HasData)
//            {
//                ReadSaveFile(data.GetReader());
//            }
//        }

//        const byte FileVersion = 3;
//        public void WriteSaveFile(System.IO.BinaryWriter w)
//        {
//            //Version 0
//            w.Write(FileVersion);
//            w.Write((ushort)localScores.Count);

//            w.Write(Ref.rnd.Int(int.MaxValue)); w.Write(Ref.rnd.Int(int.MaxValue));
//            for (int i = 0; i < localScores.Count; ++i)
//            {
//                localScores[i].Write(w);
//            }

//            FriendScores.WriteSaveFile(w);
//            WorldScores.WriteSaveFile(w);

//            //Version 1
//            w.Write((byte)SoundManager.SoundLevel);
//            w.Write((byte)SoundManager.MusicLevel);
//            //Version 2
//            w.Write(PlayState.UseVibration);

//        }
//        public void ReadSaveFile(System.IO.BinaryReader r)
//        {
//            byte version = r.ReadByte();

//            int numLocal = r.ReadUInt16();

//            r.ReadInt32(); r.ReadInt32();
//            for (int i = 0; i < numLocal; ++i)
//            {
//                localScores.Add(new LocalScore(r, version));
//            }

//            FriendScores.ReadSaveFile(r);
//            WorldScores.ReadSaveFile(r);

//            if (version > 0)
//            {
//                SoundManager.SoundLevel = (SoundLevel)r.ReadByte();
//                SoundManager.MusicLevel = (SoundLevel)r.ReadByte();
//            }
//            if (version > 1)
//            {
//                PlayState.UseVibration = r.ReadBoolean();
//            }
//        }

//        public bool newHighScore { get; private set; }
//    }

//    class LocalScore : IComparable
//    {
//        public string name;
//        public int score;
//        public int deathcount;
//        public bool GoldMembership;

//        public LocalScore(string name, bool GoldMembership, int score)
//        {
//            this.name = name;
//            this.score = score;
//            deathcount = 1;
//            this.GoldMembership = GoldMembership;
//        }
//        public LocalScore(System.IO.BinaryReader r, byte version)
//        {
//            this.Read(r, version);
//        }

//        public void Write(System.IO.BinaryWriter w)
//        {
//            SaveLib.WriteString(w, name);
//            w.Write((ushort)score);
//            w.Write((ushort)deathcount);
//            w.Write(GoldMembership);
//        }
//        public void Read(System.IO.BinaryReader r, byte version)
//        {
//            name = SaveLib.ReadString(r);
//            score = r.ReadUInt16();
//            deathcount = r.ReadUInt16();
//            if (version < 3)
//            {
//                //Older versions
//                GoldMembership = true;
//            }
//            else
//            {
//                GoldMembership = r.ReadBoolean();
//            }
//        }

//        public int CompareTo(object obj)
//        {
//            return score - ((LocalScore)obj).score;
//        }

//        public override string ToString()
//        {
//            return name + "{" + score.ToString() + "}";
//        }
//    }

    
//}
