using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data
{
   

    class WorldSummary : DataStream.IStreamIOCallback, IBinaryIOobj
    {
        public bool Removed = false;
        public bool NewWorld = true;
        public int SaveIndex;
        public byte key;
        public byte[] seedListIndex;
        public const string WorldFileName = 
#if CMODE
            "World";
#else
            "Game";
#endif
        public DateTime DateCreated;
        public DateTime DateLastPlayed;
        string OriginalHost = null;
        
        public string OverridingName = TextLib.EmptyString;

        //ver2
        //public List<NamedArea> NamedAreas = new List<NamedArea>();

        public bool IsVisitedWorld { get; private set; }
#if !CMODE
        Percent progress;
        
#endif
        int numKilledEnemies = 0;
        long timePlayed = 0;
        int numTotalNetworkVisitors = 0;
        List<string> latestVisitors = new List<string>();
        string lastestHost = TextLib.EmptyString;

        public string SeedName()
        {
            string result = key.ToString();
            for (int i = 0; i < seedListIndex.Length; i++)
            {
                result += "x" + seedListIndex[i].ToString();
            }
            return result;
        }

        public void SetVisitorStatus()
        {
            OriginalHost = Ref.netSession.Host.Gamertag;

            SaveIndex = 1;
            string suggestedName = OriginalHost + SaveIndex.ToString();

            while (WorldsSummaryColl.HasVisitorWorldName(suggestedName))
            {
                SaveIndex++;
                suggestedName = OriginalHost + SaveIndex.ToString();
            }

            
            //#if CMODE
            IsVisitedWorld = true;
            //MainMenuState.AddVisited(suggestedName);
            CreateFolder();
        }



        public WorldSummary()
        {
            DateCreated = DateTime.Now;
        }

        public List<string> LiveButtonInfo()
        {
            string time;
            if (TimeSpan.FromTicks(timePlayed).Hours > 0)
            {
                time = Convert.ToInt32(TimeSpan.FromTicks(timePlayed).TotalHours).ToString() + "hours";
            }
            else
            {
                time = Convert.ToInt32(TimeSpan.FromTicks(timePlayed).TotalMinutes).ToString() + "minutes";
            }


            List<string> result = new List<string>
            {
                //"Last played: ",
                
                (DateLastPlayed.Date == DateTime.Now.Date? "Today" : DateLastPlayed.ToString("dd MMMM yyyy")),
                ProgressText(progress),
                "Played " + time,
                "Hosted by: " + lastestHost,
                "Enemies defeated: " + numKilledEnemies.ToString(),
            };
            if (latestVisitors.Count > 0)
            {
                result.Add("CoOp partners:");
                result.AddRange(latestVisitors);
            }
            return result;
        }

        public static string ProgressText(Percent progress)
        {
            return "Progress: " + progress.ToString();
        }

        public void UpdateData(List<Players.Player> activePlayers, Data.Progress progress)
        {
            DateTime now = DateTime.Now;
            long timePassed = now.Ticks - DateLastPlayed.Ticks;
            if (timePassed > 0)
                timePlayed += timePassed;
            DateLastPlayed = now;

            TimeSpan timespan = TimeSpan.FromTicks(timePlayed);
            List<Microsoft.Xna.Framework.Net.Network.AbsNetworkPeer> remote = Ref.netSession.AllRemoteGamers();
            for (int i = 0; i < remote.Count; i++)
            {
                if (!latestVisitors.Contains(remote[i].Gamertag))
                {
                    numTotalNetworkVisitors++;
                    latestVisitors.Add(remote[i].Gamertag);
                }
            }
            lastestHost = activePlayers[0].Name;
            for (int i = 1; i < activePlayers.Count; i++)
            {
                if (!latestVisitors.Contains(activePlayers[i].Name))
                {
                    latestVisitors.Add(activePlayers[i].Name);
                }
            }

            this.progress = progress.PercentProgress;
        }

        public void StartPlayingThisWorld()
        {
            DateLastPlayed = DateTime.Now;
            latestVisitors.Clear();
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            const byte Version = 5;
            w.Write(Version);
            
            w.Write(DateCreated.Ticks);
//#if CMODE
            w.Write(IsVisitedWorld);
//#endif
            SaveLib.WriteString(w ,OverridingName);
            WriteNetShare(w);

            //new
            w.Write(DateLastPlayed.Ticks);
#if !CMODE
            w.Write(progress.ByteVal); //Percent progress;
#endif
            w.Write((ushort)numKilledEnemies);//int numKilledEnemies = 0;
            w.Write(timePlayed);//long timePlayed = 0;
            w.Write((ushort)numTotalNetworkVisitors);
            w.Write((ushort)latestVisitors.Count);
            for (int i = 0; i < latestVisitors.Count; i++)
            {
                SaveLib.WriteString(w, latestVisitors[i]);
            }
            w.Write((ushort)SaveIndex);
            w.Write(Removed);
            if (IsVisitedWorld)
            {
                SaveLib.WriteString(w, OriginalHost);
            }
            SaveLib.WriteString(w, lastestHost);
        }
        
        public void ReadStream(System.IO.BinaryReader r)
        {
            byte version = r.ReadByte();
            try
            {

                DateCreated = new DateTime(r.ReadInt64());
//#if CMODE
                IsVisitedWorld = r.ReadBoolean();

                OverridingName = SaveLib.ReadString(r);

                ReadNetShare(r, version);

                
                DateLastPlayed = new DateTime(r.ReadInt64());
#if !CMODE
                progress.ByteVal = r.ReadByte();
#endif
                numKilledEnemies = r.ReadUInt16();

                if (version >= 4)
                {
                    timePlayed = r.ReadInt64();
                }
                else
                {
                    timePlayed = r.ReadInt32();
                }
                numTotalNetworkVisitors =  r.ReadUInt16();
                int numLatestVisitors =  r.ReadUInt16();
                latestVisitors = new List<string>(numLatestVisitors);
                latestVisitors.Clear();
                for (int i = 0; i < numLatestVisitors; i++)
                {
                    latestVisitors.Add(SaveLib.ReadString(r));
                }
                SaveIndex = r.ReadUInt16();
                Removed = r.ReadBoolean();
                if (IsVisitedWorld)
                {
                    OriginalHost = SaveLib.ReadString(r);
                }
                lastestHost = SaveLib.ReadString(r);
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERR Loading WorldSummary: " + e.Message);
            }
        }
        public void WriteNetShare(System.IO.BinaryWriter w)
        {
            w.Write(key);
            if (seedListIndex == null)
            {
                System.Diagnostics.Debug.WriteLine("ERR worldsummary empty index");
                seedListIndex = new byte[RandomSeed.NumLoadedFiles];
            }
            w.Write(seedListIndex, 0, RandomSeed.NumLoadedFiles);

           //WriteNamedAreas(w);
        }

        //public void WriteNamedAreas(System.IO.BinaryWriter w)
        //{
        //    w.Write((byte)NamedAreas.Count);
        //    foreach (NamedArea na in NamedAreas)
        //    { na.WriteStream(w); }
        //}
        
        public void ReadNetShare(System.IO.BinaryReader r, byte version)
        {
            key = r.ReadByte();
            seedListIndex = r.ReadBytes(RandomSeed.NumLoadedFiles);
            
            if (version < 5)
                ReadNamedAreas(r);
            //IsVisitedWorld = true;
        }

        public void ReadNamedAreas(System.IO.BinaryReader r)
        {
            //NamedAreas.Clear();
            int numNamedLoc = r.ReadByte();
            //for (int i = 0; i < numNamedLoc; i++)
            //{
            //    string Name = SaveLib.ReadString(r);
            //    IntVector2 Chunk = Map.WorldPosition.ReadChunkGrindex_Static(r);
            //    //NamedArea na = new NamedArea();
            //    //na.ReadStream(r);
            //    //NamedAreas.Add(na);
            //}
        }
        public void KilledEnemy()
        {
            numKilledEnemies++;
        }
        public string WorldName
        {
            get 
            {
                if (OverridingName != TextLib.EmptyString)
                    return OverridingName;
                return fileName;
            }
        }

        string fileName
        { get { return WorldFileName + SaveIndex.ToString(); } }

        public string FolderPath
        {
            get
            {
//#if CMODE
                if (IsVisitedWorld)
                    return Map.Chunk.VisitedFolder + TextLib.Dir + OriginalHost + SaveIndex.ToString();
//#endif
                return fileName;
            }
        }
        //public string BackupPath
        //{
        //    get
        //    {
        //        return FolderPath + TextLib.Dir + "Backup";
        //    }
        //}
        public void CreateFolder()
        {
            DataLib.SaveLoad.CreateFolder(FolderPath);
        }

        DataStream.FilePath path()
        {
            const string FileName = "WorldSum";
            //string result = Path + TextLib.Dir + FileName;
            return new DataStream.FilePath(FolderPath, FileName, "");
        }

        public void Save(bool save, bool threaded)
        {
            DataStream.BeginReadWrite.BinaryIO(save, path(), this, this, threaded);
        }
       
        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        //public void SaveComplete(bool save, int player, DataLib.AbsSaveToStorage obj, bool failed)
        {
            //if (!save)
            //{
            //    try
            //    {
            //        List<string> lines = ((DataLib.TextFileToStorage)obj).lines;
            //        if (lines.Count > 0)
            //        {
            //            int currentLineIx = 0;
            //            int version = lib.SafeStringToInt(lines[currentLineIx]);
            //            currentLineIx++;
            //            key = (byte)lib.SafeStringToInt(lines[currentLineIx]);
            //            currentLineIx++;
            //            //const int SeedStartIx = 2;
            //            seedListIndex = new byte[RandomSeed.NumLoadedFiles];
            //            for (int i = 0; i < RandomSeed.NumLoadedFiles; i++)
            //            {
            //                seedListIndex[i] = (byte)lib.SafeStringToInt(lines[currentLineIx]);
            //                currentLineIx++;
            //            }

            //            Date = new DateTime(Convert.ToInt64(lines[currentLineIx]));

            //            currentLineIx++;
            //            if (lines.Count <= currentLineIx)
            //                return;
            //            IsVisitedWorld = lib.ScriptToBool(lines[currentLineIx]);
            //            currentLineIx++;
            //            if (lines.Count <= currentLineIx)
            //                return;
            //            //SpawnPoint = lib.StringToIntV2(lines[currentLineIx]);

            //        }
            //        NewWorld = false;
            //    }
            //    catch (Exception e)
            //    {
            //        System.Diagnostics.Debug.WriteLine(e.Message);

            //    }
            //}
        }
     
    }

    //struct WorldPlayerSettings //: IBinaryIOobj
    //{
    //    //GameObjects.Characters.Hero parent;
        
    //    public ShortVector2 SpawnPoint;

    //    public void Init()
    //    {
    //       // this.parent = parent;
    //        SpawnPoint = Map.WorldPosition.CenterChunk;
    //    }

    //}
    //struct NamedArea
    //{
    //    public const int MaxAreas = 10;
    //    public const int NameMaxLenght = 14;
    //    public string Name;
    //    public IntVector2 Chunk;

    //    public void WriteStream(System.IO.BinaryWriter w)
    //    {
    //        SaveLib.WriteString(w, Name);
    //        Map.WorldPosition.WriteChunkGrindex_Static(Chunk, w); //Chunk.WriteChunkGrindex(w);
    //    }
    //    public void ReadStream(System.IO.BinaryReader r)
    //    {
    //        Name = SaveLib.ReadString(r);
    //        if (r.BaseStream.Position < r.BaseStream.Length)
    //            Chunk = Map.WorldPosition.ReadChunkGrindex_Static(r);
    //    }
    //}
    
}
