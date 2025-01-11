using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DataLib
{
    class StandardHighScore : IHighScore
    {
        static readonly Color MenuDarkBlueBG = new Color(41, 70, 132);

        public string gamer;
        public int Score = 0;

        public string Name { get { return gamer; } }
        public void ToMenu(HUD.GuiLayout file)
        {
            //file.Add(new HUD.TextBoxData(gamer, new TextFormat(LoadedFont.PhoneText, 0.5f, Color.LightGray), Color.Black));
            //file.Add(new HUD.TextBoxData(Score.ToString(), new TextFormat(LoadedFont.PhoneText, 0.7f, Color.White), MenuDarkBlueBG));
        }

        public void WriteSaveFile(System.IO.BinaryWriter w)
        {
            SaveLib.WriteString(w, gamer);
            w.Write(Score);
        }
        public void ReadSaveFile(System.IO.BinaryReader r, byte version)
        {
            gamer = SaveLib.ReadString_safe(r);
            Score = r.ReadInt32();
        }

        public void WriteNetPacket(System.IO.BinaryWriter w)
        {
            WriteSaveFile(w);
        }
        public void ReadNetPacket(System.IO.BinaryReader r)
        {
            ReadSaveFile(r, byte.MaxValue);
        }

        

        // Returns:
        //     A value that indicates the relative order of the objects being compared.
        //     The return value has these meanings: Value Meaning Less than zero This instance
        //     is less than obj. Zero This instance is equal to obj. Greater than zero This
        //     instance is greater than obj.
        public int CompareTo(object obj)
        {
            StandardHighScore other = obj as StandardHighScore;
            return Score - other.Score;
        }
    }
    class StandardLeaderBoard : AbsLeaderBoard
    {
        const int MaxScoresCount = 250;

        override protected IHighScore readOneScore(System.IO.BinaryReader r)
        {
            StandardHighScore result = new StandardHighScore();
            result.ReadSaveFile(r, byte.MaxValue);
            return result;
        }

        override protected int maxScoresCount { get { return MaxScoresCount; } }
        override protected bool highToLow { get { return true; } }
    }
}
