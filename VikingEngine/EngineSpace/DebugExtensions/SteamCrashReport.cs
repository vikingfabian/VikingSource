#if PCGAME
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace VikingEngine.DebugExtensions
{
    static class SteamCrashReport
    {
        const bool PrintLineContent = true;
        public const string LeaderboardName = "Error";
        //const int MaxStackTrace = 3;

        static int crashReports = 0;

        public static void uploadException(Exception e, TryMethodType methodType)
        {
            
            if (crashReports++ == 0)
            {
                try
                {
                    if (Ref.steam.isInitialized && Ref.steam.leaderboardsInitialized)
                    {
                        exceptionToLeaderboard(e, methodType).BeginUpload();
                    }
                }
                catch (Exception e2)
                {
                    //var message = e2.Message + " @" + e2.StackTrace;

                    //var result = System.Windows.Forms.MessageBox.Show(message, "Loading content Crash",
                    //    System.Windows.Forms.MessageBoxButtons.OK,
                    //    System.Windows.Forms.MessageBoxIcon.Error,
                    //    System.Windows.Forms.MessageBoxDefaultButton.Button1);
                    Ref.draw.ClrColor = Color.Red;
                }
            }
            
        }


        static SteamWrapping.SteamLeaderBoardLocal exceptionToLeaderboard(Exception e, TryMethodType methodType)
        {
            SteamWrapping.SteamLeaderBoardLocal leaderboard = new SteamWrapping.SteamLeaderBoardLocal(LeaderboardName);
            
            leaderboard.score = SteamWrapping.SteamLeaderBoard.NowToScoreValue();
            leaderboard.scoreDetails.Add(CompressExceptionDetails(e, methodType));
            
            var stacktrace = new System.Diagnostics.StackTrace(e, true);
            if (stacktrace == null)
            {
                throw new NullReferenceException("null stack trace");
            }

            leaderboard.scoreDetails.Add(stacktrace.FrameCount);

            for (int i = 0; i < stacktrace.FrameCount && leaderboard.scoreDetails.UnusedArrayLength > CompressedNameIntLength; ++i)
            {
                var frame = stacktrace.GetFrame(i);
                string frameName = frame.GetFileName(); //frame.GetMethod().ReflectedType.Name;//frame.GetFileName();
                if (frameName != null)
                {
                    var filename = System.IO.Path.GetFileName(frameName);
                    var fileNameHash = filename.GetHashCode();
                    //compressString(frameName, leaderboard.scoreDetails);
                    //frame

                    var line = frame.GetFileLineNumber();

                    //if (i == 0)
                    //{
                    //    var result = MessageBox.Show(frame.GetFileName() + "::" + line.ToString(), "test",
                    //           MessageBoxButtons.OK,
                    //           MessageBoxIcon.Error,
                    //           MessageBoxDefaultButton.Button1);

                    //}

                    leaderboard.scoreDetails.Add(fileNameHash);
                    leaderboard.scoreDetails.Add(line);
                }

                //if (i == 0)
                //{
                //    var result = MessageBox.Show(frameName, "test",
                //           MessageBoxButtons.OK,
                //           MessageBoxIcon.Error,
                //           MessageBoxDefaultButton.Button1);
                //}
            }

            return leaderboard;
        }

       


        public static void printLeaderboardException(SteamWrapping.SteamLeaderBoardRemote leaderboard)
        {
            //DateTime time = Year2000;
            var time = SteamWrapping.SteamLeaderBoard.ScoreToDate(leaderboard.score);//time.AddMinutes(leaderboard.score);
            Debug.Log("EXCEPTION DATE: " + time.ToString());
            Debug.Log("User: " + leaderboard.userName);

            DecompressExceptionDetails(leaderboard.scoreDetails.UseNext());
            //Debug.Log(((ExceptionType)scoreDetails[0]).ToString());

            Debug.Log("--Stack trace--");
            List<string> hashMatch = new List<string>(4);
            int stackCount = leaderboard.scoreDetails.UseNext();

            while (stackCount-- > 0 && leaderboard.scoreDetails.UseNextLeft > CompressedNameIntLength)
            {
                int fileHash = leaderboard.scoreDetails.UseNext();
                //string name = uncompressString(leaderboard.scoreDetails);

                int line = leaderboard.scoreDetails.UseNext();

                //Debug.Log("@ " + name + " ::line " + line.ToString());
                string ProjectRoot = "C:\\lootfest3\\VikingEngine";
                getClassTypeFromHash(fileHash, hashMatch, ProjectRoot);

                if (hashMatch.Count > 0)
                {
                    Debug.Log(hashMatch[0] + " ::line " + line.ToString());
                    //Debug.Log("line " + line.ToString());
                    if (PrintLineContent)
                    {
                        var lines = DataLib.SaveLoad.LoadTextFile(hashMatch[0]);
                        if (lines.Count > line)
                        {
                            Debug.Log(lines[line - 1]);
                        }
                    }
                    hashMatch.Clear();
                }
                else
                {
                    break;
                }
            }

            Debug.Log("----");
        }

        static int CompressExceptionDetails(Exception e, TryMethodType methodType)
        {
            EightBit bools = new EightBit();
            bools.Set(0, Ref.steam.P2PManager.remoteGamers.Count > 0);
            
            FourBytes values = new FourBytes();
            values.Set(0, bools.bitArray);
            values.Set(1, (byte)Network.NetLib.PacketType);
            values.Set(2, GetExceptionType(e));
            values.Set(3, (byte)methodType);

            return values.Value;
        }

        static void DecompressExceptionDetails(int value)
        {
            FourBytes values = new FourBytes(value);
            EightBit bools = new EightBit(values.Get(0));

            Debug.Log("Exception type: " + ((ExceptionType)values.Get(2)).ToString());

            TryMethodType tryType = (TryMethodType)values.Get(3);
            string tryTypeName;
            switch (tryType)
            {
                case TryMethodType.D: tryTypeName = "Draw"; break;
                case TryMethodType.U: tryTypeName = "Update"; break;
                case TryMethodType.A: tryTypeName = "Asynch Update"; break;
                default: tryTypeName = tryType.ToString(); break;
            }
            Debug.Log(tryTypeName);

            Debug.Log("In multiplayer: " + bools.Get(0).ToString());
            Debug.Log("Last packet read: " + ((Network.PacketType)values.Get(1)).ToString());
        }

        static byte GetExceptionType(Exception e)
        {
            ExceptionType type = ExceptionType.Other;

            if (e is AbsVikingException)
                type = ((AbsVikingException)e).Type;
            if (e is NullReferenceException)
                type = ExceptionType.NullRef;
            else if (e is NotImplementedException)
                type = ExceptionType.NotImplemented;
            else if (e is IndexOutOfRangeException)
                type = ExceptionType.IndexOutOfRange;

            return (byte)type;
        }

        static void getClassTypeFromHash(int hash, List<string> result, string directory)
        {
            var classes = System.IO.Directory.GetFiles(directory, "*.cs");
            foreach (var m in classes)
            {
                if (hash == System.IO.Path.GetFileName(m).GetHashCode())
                {
                    result.Add(m);
                }
            }

            var subfolders = System.IO.Directory.GetDirectories(directory);
            foreach (var m in subfolders)
            {
                if (m != "C:\\lootfest3\\VikingEngine\\Content" &&
                    m != "C:\\lootfest3\\VikingEngine\\bin" &&
                    m != "C:\\lootfest3\\VikingEngine\\packages")
                {
                    getClassTypeFromHash(hash, result, m);
                }
            }
        }

        const int CompressedNameIntLength = 4;

        public static void compressString(string name, StaticList<int> toList)
        {
            int currentByteIndex = 0;
            int arrayIx = 0;
            FourBytes[] compressedString = new FourBytes[CompressedNameIntLength];
            FourBytes value;

            value = compressedString[arrayIx];
            value.Set(currentByteIndex++, name.Length);
            compressedString[arrayIx] = value;

            int charLength = CompressedNameIntLength * 4 - 1;

            for (int charIx = 0; charIx < charLength; ++charIx)
            {
                if (charIx >= name.Length)
                {
                    break;
                }
                byte b = (byte)name[charIx];

                value = compressedString[arrayIx];
                value.Set(currentByteIndex++, b);
                compressedString[arrayIx] = value;

                if (currentByteIndex >= 4)
                {
                    currentByteIndex = 0;
                    arrayIx++;
                }
            }

            foreach (var m in compressedString)
            {
                toList.Add(m.Value);
            }
        }

        public static string uncompressString(StaticList<int> fromList)
        {
            int currentByteIndex = 0;
            int arrayIx = 0;
            FourBytes[] compressedString = new FourBytes[CompressedNameIntLength];
            //FourBytes value;

            for (int i = 0; i < CompressedNameIntLength; ++i)
            {
                compressedString[i].Value = fromList.UseNext();
            }

            int length = compressedString[arrayIx].Get(currentByteIndex++);
            StringBuilder result = new StringBuilder(length);

            int charLength = Math.Min(CompressedNameIntLength * 4 - 1, length);
            for (int charIx = 0; charIx < charLength; ++charIx)
            {
                var c = (char)compressedString[arrayIx].Get(currentByteIndex++);
                result.Append(c);

                if (currentByteIndex >= 4)
                {
                    currentByteIndex = 0;
                    arrayIx++;
                }
            }

            if (result.Length < length)
            {
                result.Append('*', length - result.Length);
            }

            return result.ToString();
        }
    }

    class DownloadSteamCrashReports : AbsUpdateable
    {
        public DownloadSteamCrashReports()
            :base(true)
        {
            SteamWrapping.SteamLeaderBoardLocal leaderboard = new SteamWrapping.SteamLeaderBoardLocal(SteamCrashReport.LeaderboardName);
            leaderboard.BeginDownload(onDownload);
        }

        void onDownload(List<SteamWrapping.SteamLeaderBoardRemote> values)
        {
            Debug.CrashIfThreaded();
            foreach (var m in values)
            {
                SteamCrashReport.printLeaderboardException(m);
            }

            Debug.Log("Download crashes COMPLETE...");

            DeleteMe();
        }

        public override void Time_Update(float time_ms)
        {
        }
    }
}
#endif