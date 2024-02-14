using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna


namespace VikingEngine.DataLib
{
    
    interface IHighScore : IComparable
    {
        string Name { get; }
       // void ToMenu(HUD.File file);

        void WriteSaveFile(System.IO.BinaryWriter w);
        void ReadSaveFile(System.IO.BinaryReader r, byte version);

        void WriteNetPacket(System.IO.BinaryWriter w);
        void ReadNetPacket(System.IO.BinaryReader r);
    }

    /// <summary>
    /// Organize high scores
    /// </summary>
    abstract class AbsLeaderBoard
    {

        LinkedList<IHighScore> scoreList;

        public AbsLeaderBoard()
        {
            scoreList = new LinkedList<IHighScore>();
        }

        //const int ListAmount = 60;
        //public void ListToMenu(LootFest.File file, List<string> dontListTheseGamers, int maxListed)
        //{
        //    LinkedListMember<IHighScore> current = scoreList.First;

        //    while (scoreList.Next(ref current))
        //    {
        //        if (dontListTheseGamers == null || !dontListTheseGamers.Contains(current.Value.Name))
        //        {
        //            current.Value.ToMenu(file);
        //        }

        //        if (--maxListed <= 0)
        //            return;
        //    }
        //}
        

        public void GamertagsTo(List<string> list)
        {
            LinkedListMember<IHighScore> current = scoreList.First;

            while (scoreList.Next(ref current))
            {
                list.Add(current.Value.Name);
            }
        }

        public void add(IHighScore score)
        {
            if (scoreList.Count == 0)
            {
                scoreList.Add(score);
            }
            else
            {
                bool foundSpot = false;

                LinkedListMember<IHighScore> current = scoreList.First;

                while (scoreList.Next(ref current))
                {
                    if (foundSpot)
                    { //remove spots with lower score
                        if (current.Value.Name == score.Name)
                        {
                            scoreList.RemoveMember(current);
                            break;
                        }
                    }
                    else
                    {
                        bool higher;
                        if (highToLow)
                           higher = score.CompareTo(current.Value) > 0;//score.Score > current.Value.Score;
                        else
                           higher = score.CompareTo(current.Value) < 0;
                        
                        if (current.Value.Name == score.Name)
                        {//already have a high score stored
                            if (higher)
                                current.Value = score;
                            return;
                        }
                        if (higher)
                        {
                            foundSpot = true;
                            scoreList.Insert(current, score);
                        }
                    }
                }

                if (foundSpot)
                {
                    if (scoreList.Count > maxScoresCount)
                    {
                        scoreList.RemoveMember(scoreList.Last);
                    }
                }
                else
                {//put in the end
                    if (scoreList.Count < maxScoresCount)
                    {
                        scoreList.Add(score);
                    }
                }
            }
        }

        public void WriteSaveFile(System.IO.BinaryWriter w)
        {
            LinkedListMember<IHighScore> current = scoreList.First;

            while (scoreList.Next(ref current))
            {
                w.Write(true);
                current.Value.WriteSaveFile(w);
            }
            w.Write(false);
        }
        public void ReadSaveFile(System.IO.BinaryReader r)
        {
            scoreList.Clear();
            while (r.ReadBoolean())
            {
                scoreList.Add(readOneScore(r));
            }
        }

        abstract protected IHighScore readOneScore(System.IO.BinaryReader r);

        public void RemoveGamer(string gamer)
        {
             LinkedListMember<IHighScore> current = scoreList.First;

             while (scoreList.Next(ref current))
             {
                 if (current.Value.Name == gamer)
                 {
                     scoreList.RemoveMember(current);
                     return;
                 }
             }
        }

        public void Clear()
        {
            scoreList.Clear();
        }

        public int Count { get { return scoreList.Count; } }


        //const int MaxScoresCount = 250;
        abstract protected int maxScoresCount { get; }
        abstract protected bool highToLow { get; }
    }
    
}
