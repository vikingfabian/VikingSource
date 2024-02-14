using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataLib
{
    class ScriptCommands
    {
        public List<string> Commands = new List<string>();
        public List<string> Values = new List<string>();

        //public void Init()
        //{
        //    Commands = new List<string>();
        //    Values = new List<string>();
        //}

        public void LoadLine(string line)
        {
            //Init();
            string val = TextLib.EmptyString;
            string command = TextLib.EmptyString;
            input currentIn = input.Command;
            Commands.Clear();
            Values.Clear();

            foreach (char c in line)
            {
                if (c == ' ')
                { //Ignore if command
                    if (currentIn == input.Value) { val += c; }
                }
                else if (c == '(')
                {
                    currentIn = input.Value;
                }
                else if (c == ')')
                {
                    if (command != TextLib.EmptyString && val != TextLib.EmptyString)
                    {
                        Commands.Add(command);
                        Values.Add(val);
                    }
                    val = TextLib.EmptyString; command = TextLib.EmptyString;
                    currentIn = input.Command;
                }
                else if (currentIn == input.Value)
                { val += c; }
                else
                { command += c; }
            }
        }
        public string GetValue(string command)
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i] == command)
                { return Values[i]; }
            }
            return TextLib.EmptyString;
        }
        public void AddCommand(string command, string value)
        { Commands.Add(command); Values.Add(value); }

        public string GetLine()
        {
            string ret = TextLib.EmptyString;
            for (int i = 0; i < Commands.Count; i++)
            {
                ret += Commands[i] + "(" + Values[i] + ")";
            }
            return ret;
        }
        enum input { Command, Value };
    }
}
