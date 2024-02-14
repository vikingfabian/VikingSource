using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DebugExtensions
{
    class TextOutput
    {
        string name;
        List<string> output;

        public TextOutput(string name)
        {
            this.name = "Output_" + name + ".txt";
            output = new List<string>();
        }

        public void Print(string text)
        {
            output.Add(DateTime.Now.ToString(@"hh\:mm\:ss") + ": " + text);
            DataLib.SaveLoad.CreateTextFile(name, output);
        }
    }
}
