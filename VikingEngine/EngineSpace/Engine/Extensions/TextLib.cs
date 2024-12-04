using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using System.Text;
using System.Reflection;
using System.Globalization;
using VikingEngine.Input;

namespace VikingEngine
{
    static class TextLib
    {
        /// <summary>
        /// "\"
        /// </summary>

        public const string EmptyString = "";
        public const string Error = "ERR";
        const string MaxTwoDecimalsFormat = "0.##";
        public const string TextFileEnding = ".txt";
        public static readonly List<char> BreakPoints = new List<char> { ' ', '+', '-', '*', '/', '^' };
        //const char NewLineChar1 = '\n';
        //const char NewLineChar2 = '\r';
        public static readonly string NewLine = Environment.NewLine;
        public static readonly List<char> Numbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static List<string> Lines = new List<string>();
        static NTStringBuilder word = new NTStringBuilder();
        public static NTStringBuilder TextLine = new NTStringBuilder();
        static NTStringBuilder test = new NTStringBuilder();

        public static string Quote(string text)
        {
            return "\"" + text + "\"";
        }

        public static string ListToString(List<string> list)
        {
            if (list.Count == 0)
            {
                return null;
            }

            StringBuilder text = new StringBuilder();
            for (int i = 0; i < list.Count - 1; ++i)
            {
                text.AppendLine(list[i]);
            }

            text.Append(list[list.Count - 1]);

            return text.ToString();
        }

        static readonly NumberFormatInfo ThounsandSeperatorSpaceFormat = new NumberFormatInfo
        {
            NumberGroupSeparator = " ",
            NumberDecimalDigits = 0
        };
        const string NoDecimals = "N0";

        public static string LargeNumber(int number)
        {
            if (Math.Abs(number) < 1000)
            {
                return number.ToString();
            }
            return number.ToString(NoDecimals, ThounsandSeperatorSpaceFormat).Trim();
        }

        public const string OneDecimalFormat = "{0:0.0}";
        public static string OneDecimal(double value)
        {
            string result = string.Format(OneDecimalFormat, value);

            stringSafeDecimal(ref result);

            return result;
        }

        public const string ThreeDecimalFormat = "{0:0.000}";
        public static string ThreeDecimal(double value)
        {
            string result = string.Format(ThreeDecimalFormat, value);

            stringSafeDecimal(ref result);

            return result;
        }

        static void stringSafeDecimal(ref string result)
        {
            if (result.Length > 2)
            {
                if (result[result.Length - 2] == '٫')
                {
                    result = result.Replace('٫', ',');
                }
            }
        }

        public static string PlusMinusOneDecimal(double value)
        {
            string result = OneDecimal(value);
            if (value > 0)
            {
                result = "+" + result;
            }
            return result;
        }

        public static string PlusMinus(double value)
        {
            string result = value.ToString(MaxTwoDecimalsFormat);
            stringSafeDecimal(ref result);

            if (value > 0)
            {
                result = "+" + result;
            }
            return result;
        }

        public static string FirstLetters(string text, int numLetters)
        {
            if (numLetters >= text.Length)
                return text;
            return text.Remove(numLetters, text.Length - numLetters);
        }
        public static string FirstLettersDotDotDot(string text, int numLetters)
        {
            const string DotDotDot = "...";
            if (text.Length > numLetters + 2)
            {
                //numLetters += 2;
                return text.Remove(numLetters - 2, text.Length - numLetters + 2) + DotDotDot;
            }
            return text;

        }

        public static string RemoveEnding(string text, int removeCount)
        {
            if (removeCount >= text.Length)
                return text;
            return text.Remove(text.Length - removeCount, removeCount);
        }

        public static string PluralEnding(string text, int count)
        {
            if (count > 1)
            {
                return text + "s";
            }
            return text;
        }

        public static string Divition(int value, int dividedBy)
        {
            return value.ToString() + "/" + dividedBy.ToString();
        }

        public static string IndexDivition(int index, int length)
        {
            return (index + 1).ToString() + "/" + length.ToString();
        }

        public static string Divition_Large(int value, int dividedBy)
        {
            return LargeNumber(value) + " / " + LargeNumber(dividedBy);
        }

        public static string LastLetters(string text, int length = 1)
        {
            return text.Substring(Bound.Min(text.Length - length, 1));
        }

        public static string EnumName(string enumName)
        {
            return enumName.Replace('_', ' ');
        }

        public static string LargeFirstLetter(string text)
        {
            if (text.Length <= 1)
                return text;
            return string.Concat(text[0].ToString().ToUpper(), text.AsSpan(1));//(text.Remove(1, text.Length - 1)).ToUpper() + text.Remove(0, 1);
        }

        //public static string FirstCharToUpper(this string input)
        //{
        //    switch (input)
        //    {
        //        null => throw new ArgumentNullException(nameof(input)),
        //        "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
        //        _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        //    }
        //}

        public static string PercentText(float percent)
        {
            return Convert.ToInt32(percent * 100).ToString() + "%";
        }

        public static string PercentAddText(float percent)
        {
            if (percent < 0f)
            {
                return PercentText(percent);
            }
            else
            {
                return "+" + PercentText(percent);
            }
        }

        public static string InsertText(string text, string insert, int position)
        {
            if (position == text.Length)
            {
                TextLine.Clear();
                TextLine.Append(text, insert);
            }
            else
            {
                word.Clear();
                word.Append(text);
                word.RemoveFirstLetters(position);

                TextLine.Clear();
                TextLine.Append(FirstLetters(text, position));
                TextLine.Append(insert);
                TextLine.Append(word.String);
            }
            return TextLine.String;
        }

        public static string IndexToString(int index) { return (index + 1).ToString(); }

        public static string CheckBadLanguage(string textString)
        {
            List<string> badWords = new List<string>
            {
                " ass", " arse",
                "bitch",
                "blowjob",
                "cock",
                "crap", 
                "cunt",
                "dick",
                "faggot",
                "fag ",
                "fuck",
                "homo",
                "hore",
                "horny",
                "nigge", "nigga", "coon", "spook ", "spooks",
                "pussy",
                "shagg",
                "shit",
                "vagina",
                " hore",
                "basterd",
            };

            //string text = TextString;

            string lowCase = textString.ToLower();

            foreach (string w in badWords)
            {
                const string replace = "***";
                if (lowCase.Contains(w))
                {
                    lowCase = lowCase.Replace(w, replace);
                    textString = lowCase;
                }
                
            }
            return textString;
        }

        public static string checkFileName(string name)
        {
            if (name.Length <= 1)
            {
                return "--";
            }
            
            const char ReplaceBadChar = '-';

            StringBuilder safeString = new StringBuilder(name.Length);
            
            foreach (char c in name)
            {
                if ((c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= '0' && c <= '9') ||
                    c == '_' || c == '-')
                {
                    safeString.Append(c);
                }
                else
                {
                    safeString.Append(ReplaceBadChar);
                }
            }

            return safeString.ToString();
        }

        public static bool EqualStrings(StringBuilder stringBuilder, ref string text)
        {
            return stringBuilder.Length == text.Length && stringBuilder.ToString() == text;
        }

        public static string ValueInParentheses(double value, bool startSpace = false)
        {
            return Parentheses(value.ToString(), startSpace);
        }

        public static string Parentheses(string text, bool startSpace = false)
        {
            if (startSpace)
            {
                return " (" + text + ")";
            }
            else
            {
                return "(" + text + ")";
            }
        }

        //    public static string SplitToMultiLine2(string text, LoadedFont font, float fontSize, float lineWidth,
        //float inSpaceFirstRow = 0f, List<string> linesOut = null)
        //    {
        //        if (lineWidth < 1)
        //        {
        //            return "";
        //        }

        //        bool isAsian = IsAsianText(text);
        //        text += " ";

        //        StringBuilder completedText = new StringBuilder();
        //        StringBuilder line = new StringBuilder();
        //        StringBuilder testline = new StringBuilder();

        //        foreach (char c in text)
        //        {
        //            if (c == '\n')
        //            {
        //                splitText();
        //            }
        //            else
        //            {
        //                testline.Clear();
        //                testline.Append(line);
        //                testline.Append(c);

        //                bool tooLong =
        //                    (Engine.LoadContent.MeasureString(testline.ToString(), font).X * fontSize) >=
        //                    (lineWidth - inSpaceFirstRow);
        //                if (tooLong)
        //                {
        //                    splitText();
        //                }

        //                line.Append(c);
        //            }
        //        }

        //        if (line.Length > 0)
        //        {
        //            linesOut?.Add(line.ToString());
        //            completedText.Append(line);
        //        }

        //        return completedText.ToString();

        //        void splitText()
        //        {
        //            if (line.Length > 0)
        //            {
        //                linesOut?.Add(line.ToString());
        //                completedText.Append(line);
        //                completedText.Append(Environment.NewLine);
        //                line.Clear();
        //                inSpaceFirstRow = 0f;
        //            }
        //        }

        //        bool IsAsianText(string inputText)
        //        {
        //            foreach (char ch in inputText)
        //            {
        //                if (IsAsianCharacter(ch))
        //                {
        //                    return true;
        //                }
        //            }
        //            return false;
        //        }

        //        bool IsAsianCharacter(char ch)
        //        {
        //            return (ch >= 0x4E00 && ch <= 0x9FFF) || // CJK Unified Ideographs
        //                   (ch >= 0x3400 && ch <= 0x4DBF) || // CJK Unified Ideographs Extension A
        //                   (ch >= 0x3040 && ch <= 0x309F) || // Hiragana
        //                   (ch >= 0x30A0 && ch <= 0x30FF) || // Katakana
        //                   (ch >= 0xAC00 && ch <= 0xD7AF);   // Hangul Syllables
        //        }
        //    }

        public static string SplitToMultiLine2(string text, LoadedFont font, float fontSize, float lineWidth,
            float inSpaceFirstRow = 0f, List<string> linesOut = null)
        {
            if (lineWidth < 1)
            {
                return "";
            }

            text += " ";

            StringBuilder completedText = new StringBuilder();
            StringBuilder line = new StringBuilder();
            StringBuilder testline = new StringBuilder();
            StringBuilder word = new StringBuilder();

            foreach (char c in text)
            {
                if (c == '\n')
                {
                    lib.DoNothing();
                }

                word.Append(c);
                if (BreakPoints.Contains(c) || IsAsianCharacter(c))
                {
                    splitIfTooLong();
                    addLastWord();
                }
                else if (endsWithNewLine())
                {
                    word.Remove(word.Length - Environment.NewLine.Length, Environment.NewLine.Length);

                    splitIfTooLong();

                    addLastWord();
                    splitText();
                }
            }

            line.Append(word);
            line.Remove(line.Length - 1, 1);//Remove last letter

            linesOut?.Add(line.ToString());
            completedText.Append(line);

            return completedText.ToString();

            void splitIfTooLong()
            {
                testline.Clear();
                testline.Append(line);
                testline.Append(word);

                bool tooLong =
                    (Engine.LoadContent.MeasureString(testline.ToString(), font, out _).X * fontSize) >=
                    (lineWidth - inSpaceFirstRow);
                if (tooLong)
                {
                    splitText();
                }
            }

            bool endsWithNewLine()
            {
                return word.Length > 0 && word[word.Length - 1] == Environment.NewLine[Environment.NewLine.Length - 1];
            }

            void splitText()
            {
                linesOut?.Add(line.ToString());
                completedText.Append(line);

                completedText.Append(Environment.NewLine);

                line.Remove(0, line.Length);
                inSpaceFirstRow = 0f;
            }

            void addLastWord()
            {
                line.Append(word);
                word.Clear();
            }

            bool IsAsianCharacter(char ch)
            {
                return (ch >= 0x4E00 && ch <= 0x9FFF) || // CJK Unified Ideographs
                       (ch >= 0x3400 && ch <= 0x4DBF) || // CJK Unified Ideographs Extension A
                       (ch >= 0x3040 && ch <= 0x309F) || // Hiragana
                       (ch >= 0x30A0 && ch <= 0x30FF) || // Katakana
                       (ch >= 0xAC00 && ch <= 0xD7AF);   // Hangul Syllables
            }
        }

        static void AppendLine(StringBuilder completedText, StringBuilder line, List<string> linesOut)
        {
            linesOut?.Add(line.ToString());
            completedText.Append(line);
        }

        static void clearStringBuilder(StringBuilder sb)
        {
            sb.Remove(0, sb.Length);
        }

        static void addLineWithXCheck(float fontSize, float lineWidth, LoadedFont font, NTStringBuilder text)
        {
            while (ToLongString(text.String, fontSize, font, lineWidth))
            { //one very long word, need to be split up
                string toLongLine = text.String;
                test.Clear();
                for (int wordLength = 0; wordLength < toLongLine.Length; wordLength++)
                {
                    test.AppendChar(toLongLine[wordLength]);
                    if (ToLongString(test.String, fontSize, font, lineWidth))
                    {//found a part of the word that just fits
                        test.RemoveLastLetters(1);
                        Lines.Add(test.String);
                        break;
                    }
                }
                text.RemoveFirstLetters(test.StringBuilder.Length);
            }

            if (text.StringBuilder.Length > 0)
                Lines.Add(text.String);
        }
        
        /// <returns>XXmin XXsec</returns>
        public static string TimeToText(double time, bool ms)
        {
            const double Second = 1000;
            const double Minute = Second * 60;
            const double Hour = Minute * 60;

            int hours = (int)(time / Hour);
            time %= Hour;
            int min = (int)(time / Minute);
            double sec = ((time % Minute) / Second);
            string result;
            const string SecText = "sec";
            const string MinText = "min ";
            const string HourText = "h ";
            if (ms)
            {
                const string Format = "#.##";
                result = sec.ToString(Format);
            }
            else
            {

                result = ((int)sec).ToString();
            }

            if (min > 0)
            {
                result = min.ToString() + MinText + result;
            }
            if (hours > 0)
            {
                result = hours.ToString() + HourText + result;
            }

            return result + SecText;
        }
        public static string SetMaxLenght(string text, int maxLenght, bool dots)
        {
            if (text.Length > maxLenght)
            {
                int toLenght = maxLenght;
                if (dots)
                    toLenght -= 2;
                text = text.Remove(toLenght, text.Length - toLenght);
                if (dots)
                    text += "...";
            }
            return text;
        }

        public static List<string> AlphabeticOrder(List<string> text)
        {
            text.Sort();
            return text;
        }
        public static List<TextAndIndex> AlphabeticOrder(List<TextAndIndex> text)
        {
            text.Sort(delegate(TextAndIndex p1, TextAndIndex p2) { return p1.Text.CompareTo(p2.Text); });

            //text.Sort();
            return text;
        }
        public static bool ToLongString(string text, float sizeX, LoadedFont font, float lineWidth)
        {
            return Engine.LoadContent.MeasureString(text, font, out _).X * sizeX >= lineWidth;
        }

        public static float TextHeight(LoadedFont font)
        {
            const string Test = "MWg";
            return Engine.LoadContent.MeasureString(Test, font, out _).Y;
        }
        public static string FileSizeText(long bytes)
        {
            const int Mega = 1000000;
            const int Kilo = 1000;

            if (bytes >= Mega)
            {
                return (bytes / Mega).ToString() + "MB";
            }
            else if (bytes >= Kilo)
            {
                return (bytes / Kilo).ToString() + "KB";
            }
            else
            {
                return bytes.ToString() + "B";
            }
        }

        public static string OnOff(bool value)
        {
            return value ? "on" : "off";
        }
        public static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }

        public static string ValuePlusMinus(int value)
        {
            if (value >= 0) return "+" + value.ToString();
            
            return value.ToString();
        }

        public static string ValueText(int value)
        {
            switch (value)
            {
                case 0: return "zero";
                case 1: return "one";
                case 2: return "two";
                case 3: return "three";
            }

            return value.ToString();
        }

        public static string ToString_Safe(object value)
        {
            if (value == null)
            {
                return "null";
            }
            else
            {
                return value.ToString();
            }
        }

        public static bool IsEmpty(string text)
        {
            return text == null || text.Length == 0;
        }

        public static bool HasValue(string text)
        {
            return text != null && text.Length > 0;
        }

    }
    public class NTStringBuilder
    {
        /// <summary> 
        /// This is the reflected pointer to the actual string our string builder is modifying, which means that calls to it don't require toString and don't make garbage! 
        /// </summary> 
        public string String
        {
            get
            {
                return stringBuilder.ToString();
                //return _string;
            }
        }
        //private string _string;
        
        /// <summary> 
        /// The actual string builder used by the string. We can get it and append directly if needed, but we can't reassign it or it'll mess up the pointer. 
        /// </summary> 
        public StringBuilder StringBuilder
        {
            get
            {
                return stringBuilder;
            }
        }
        private StringBuilder stringBuilder;
        static char[] numberBuffer = new char[16];

        /// <summary> 
        /// Declares a new No Trash String Builder. 
        /// </summary> 
        /// <param name="capacity">The starting and max capacity that the internal string builder should be set to. Only make it as big as you need.</param> 
        public NTStringBuilder()//int capacity)
        {
            //_stringBuilder = new StringBuilder(capacity, capacity);
            stringBuilder = new StringBuilder();
            //_string = (string)_stringBuilder.GetType().GetField(
            //    "m_StringValue", BindingFlags.NonPublic | BindingFlags.Instance)
            //    .GetValue(_stringBuilder);
        }
        public NTStringBuilder(string startValue)
        {
            stringBuilder = new StringBuilder(startValue);
            //_string = (string)_stringBuilder.GetType().GetField(
            //    "m_StringValue", BindingFlags.NonPublic | BindingFlags.Instance)
            //    .GetValue(_stringBuilder);
        }
        public void RemoveFirstLetters(int numLetters)
        {
            stringBuilder.Remove(0, numLetters);
        }
        public void RemoveLastLetters(int numLetters)
        {
            stringBuilder.Remove(stringBuilder.Length - numLetters, numLetters);
        }
        /// <summary> 
        /// Appends a number to the string without creating garbage. 
        /// </summary> 
        /// <param name="number">The number to append.</param> 
        public void AppendNumber(int number)
        {
            AppendNumber(number, 0);
        }

        /// <summary> 
        /// Appends a number to the string without creating garbage, allows you to specify the minimum digits. 
        /// </summary> 
        /// <param name="number">The number to append.</param> 
        /// <param name="minDigits">The minimum number of digits.</param> 
        public void AppendNumber(int number, int minDigits)
        {
            if (number < 0)
            {
                stringBuilder.Append('-');
                number = -number;
            }
            int index = 0;
            do
            {
                int digit = number % 10;
                numberBuffer[index] = (char)('0' + digit);
                number /= 10;
                ++index;
            } while (number > 0 || index < minDigits);
            for (--index; index >= 0; --index)
            {
                stringBuilder.Append(numberBuffer[index]);
            }
        }
        public void AppendChar(char c)
        {
            stringBuilder.Append(c);
        }
        public void AppendChar(string Text, char c)
        {
            stringBuilder.Append(Text);
            stringBuilder.Append(c);
        }
        /// <summary> 
        /// Exposes the internal string builder's Append to make things a bit easier. 
        /// </summary> 
        /// <param name="Text"></param> 
        public void Append(string Text)
        {
            stringBuilder.Append(Text);
        }

        public void Append(string text1, string text2)
        {
            stringBuilder.Append(text1);
            stringBuilder.Append(text2);
        }

        public void Append(string text1, string text2, string text3)
        {
            stringBuilder.Append(text1);
            stringBuilder.Append(text2);
            stringBuilder.Append(text3);
        }

        public void Append(string text1, string text2, string text3, string text4)
        {
            stringBuilder.Append(text1);
            stringBuilder.Append(text2);
            stringBuilder.Append(text3);
            stringBuilder.Append(text4);
        }

        public void Append(string text1, string text2, string text3, string text4, string text5)
        {
            stringBuilder.Append(text1);
            stringBuilder.Append(text2);
            stringBuilder.Append(text3);
            stringBuilder.Append(text4);
            stringBuilder.Append(text5);
        }

        
        public void Clear()
        {
            stringBuilder.Remove(0, stringBuilder.Length);
        }
        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        
    } 
}
