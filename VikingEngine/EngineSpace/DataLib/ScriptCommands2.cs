using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataLib
{
    class ScriptCommands2
    {
        Dictionary<string, string> Script = new Dictionary<string, string>();

        public void Clear()
        {
            Script = new Dictionary<string, string>();
        }
        public Dictionary<string, string> Data()
        {
            return Script;
        }
        public void LoadLine(string line)
        {
            string val = TextLib.EmptyString;
            string command = TextLib.EmptyString;
            input currentIn = input.Command;
            Clear();

            foreach (char c in line)
            {
                if (c == ' ')
                { //Ignore if command
                    if (currentIn == input.Value) { val += c; }
                }
                else if (c == SaveData.LParen)
                {
                    currentIn = input.Value;
                }
                else if (c == SaveData.RParen)
                {
                    if (command != TextLib.EmptyString && val != TextLib.EmptyString)
                    {
                        Script.Add(command, val);
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
        public bool ContainsComand(string command)
        {
            return Script.ContainsKey(command);
        }
        public string GetValue(string command)
        {
            if (Script.ContainsKey(command))
            {
                return Script[command];
            }
            return TextLib.EmptyString;
        }
        public void AddCommand(string command, string value)
        { Script.Add(command, value); } //Commands.Add(command); Values.Add(value); }

        public string GetLine()
        {
            string ret = TextLib.EmptyString;
            foreach (KeyValuePair<string, string> kv in Script)
            {
                ret += kv.Key + SaveData.LParen + kv.Value + SaveData.RParen;
            }
            return ret;
        }
        enum input { Command, Value };
    }
}
