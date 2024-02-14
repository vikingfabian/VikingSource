//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace VikingEngine.DataLib
//{
//    class Language : AbsQuedTasks
//    {
//        public bool loadComplete = false;
//        DataStream.FilePath filePath;

//        public LanguageGroup constant;
//        LanguageGroup description;

//        public Language(string contentFolder, string languageOption)
//            :base(true, true, true, false, true)
//        {
//            Ref.language = this;
//            filePath = new DataStream.FilePath(contentFolder + languageOption, "", "", false);

//            beginAutoTasksRun();
//        }

//        protected override void runQuedStorageTask()
//        {
//            base.runQuedStorageTask();

//            filePath.FileName = "Constant";
//            filePath.FileEnd = ".txt";
//            constant = new LanguageGroup(filePath);

//            filePath.FileName = "Description";
//            description = new LanguageGroup(filePath);
//        }

//        public string Print(string id, params PrintArg[] printargs)
//        {
//            return print(id, 0, printargs);
//        }

//        public string Print(string id, int lineIx, params PrintArg[] printargs)
//        {
//            return print(id, lineIx, printargs);
//        }

//        public TwoStrings PrintLines(string id, params PrintArg[] printargs)
//        {
//            TwoStrings result = new TwoStrings(
//                print(id, 0, printargs),
//                print(id, 1, printargs));

//            return result;
//        }

//        string print(string id, int lineIx, PrintArg[] printargs)
//        {
//            LanguageString languageString;
//            if (description.strings.TryGetValue(id.GetHashCode(), out languageString))
//            {
//                string text;
//                if (lineIx == 0)
//                {
//                    text = languageString.line1;
//                }
//                else
//                {
//                    text = languageString.line2;
//                }

//                if (arraylib.HasMembers(printargs))
//                {
//                    text = processString(text, printargs);
//                }

//                return text;
//            }
//            else
//            {
//                return "ERR " + id;
//            }
//        }

//        string processString(string text, PrintArg[] printargs)
//        {
//            StringBuilder result = new StringBuilder();
//            StringBuilder id = new StringBuilder();

//            for (int letterIx = 0; letterIx < text.Length; ++letterIx)
//            {
//                if (text[letterIx] == '%')
//                {
//                    ++letterIx;
//                    if (text[letterIx] == '(')
//                    {
//                        readId(text, ')', ref letterIx, ref id);
//                        string variable = id.ToString();

//                        foreach (var m in printargs)
//                        {
//                            if (m.variableId == variable)
//                            {
//                                result.Append(variable);
//                                break;
//                            }
//                        }
//                    }
//                    else if (text[letterIx] == '<')
//                    {
//                        readId(text, '>', ref letterIx, ref id);
//                        string constId = id.ToString();

//                        LanguageString languageString;
//                        if (description.strings.TryGetValue(constId.GetHashCode(), out languageString))
//                        {
//                            result.Append(languageString.line1);
//                        }
//                        else
//                        {
//                            result.Append("ERR CONST ID " + constId);
//                        }
//                    }
//                    else
//                    {
//                        result.Append(text[letterIx]);
//                    }
//                }
//            }

//            return result.ToString();
//        }

//        void readId(string text, char endBracket, ref int letterIx, ref StringBuilder id)
//        {
//            id.Clear();
//            ++letterIx;
//            for (; letterIx < text.Length; ++letterIx)
//            {
//                if (text[letterIx] == endBracket)
//                {
//                    return;
//                }
//                else
//                {
//                    id.Append(text[letterIx]);
//                }
//            }
            
//        }

//        protected override void runQuedAsynchTask()
//        {
//            base.runQuedAsynchTask();

//            constant.processData();
//            description.processData();
//        }

//        protected override void runQuedMainTask()
//        {
//            base.runQuedMainTask();

//            loadComplete = true;
//        }
//    }

//    class LanguageGroup
//    {
//        static string MessageId = "msgid";
//        static string MessageString = "msgstr";
//        const char Comment = '#';

//        List<string> data = null;
//        public Dictionary<int, LanguageString> strings;
//        StringBuilder word = new StringBuilder();              

//        public LanguageGroup(DataStream.FilePath filePath)
//        {
//#if PCGAME
//            string path = Environment.CurrentDirectory + DataStream.FilePath.Dir + filePath.CompletePath(false);
//            data = SaveLoad.LoadTextFile(path);
//#endif
//        }

//        public void processData()
//        {
//            LanguageString buildingString = null;
//            int letterIndex;
//            strings = new Dictionary<int, LanguageString>();
//            WordType prevWordType = WordType.Ignore;

//            for (int lineIndex = 0; lineIndex < data.Count; ++lineIndex)
//            {
//                string line = data[lineIndex];
//                WordType wordType = GetWordType(line, out letterIndex);

//                if (wordType != WordType.Ignore)
//                {
//                    switch (wordType)
//                    {
//                        case WordType.ID:
//                            completeString(buildingString);
//                            buildingString = null;

//                            if (findFirstQuote(line, ref letterIndex))
//                            {
//                                string idString = readToEndQuote(line, letterIndex);
//                                buildingString = new LanguageString(idString);
//                            }
//                            break;
//                        case WordType.String:
//                            if (buildingString != null)
//                            {
//                                if (findFirstQuote(data[lineIndex], ref letterIndex))
//                                {
//                                    string text = readToEndQuote(line, letterIndex);

//                                    if (buildingString.line1 == null)
//                                    {
//                                        buildingString.line1 = text;
//                                    }
//                                    else
//                                    {
//                                        buildingString.line2 = text;
//                                    }
//                                }
//                                else
//                                {
//                                    if (buildingString.line1 == null)
//                                    {
//                                        buildingString.line1 = "";
//                                    }
//                                    else
//                                    {
//                                        buildingString.line2 = "";
//                                    }
//                                }
//                            }
//                            break;
//                        case WordType.StringNewLine:
//                            if (buildingString != null)
//                            {
//                                string text = readToEndQuote(line, letterIndex);

//                                if (buildingString.line2 != null)
//                                {
//                                    buildingString.line2 += text;
//                                }
//                                else if (buildingString.line1 != null)
//                                {
//                                    buildingString.line1 += text;
//                                }
//                                else
//                                {
//                                    buildingString.line1 = text;
//                                }
//                            }
//                            break;
//                    }
//                }

//                prevWordType = wordType;
//            }

//            completeString(buildingString);

//            data = null;
//        }

//        void completeString(LanguageString buildingString)
//        {
//            if (buildingString != null && buildingString.IsComplete())
//            {
//                strings.Add(buildingString.id, buildingString);
//            }
//        }

//        WordType GetWordType(string line, out int letterIndex)
//        {
//            if (line.Length > 5)
//            {
//                word.Clear();

//                for (letterIndex = 0; letterIndex <= line.Length; ++letterIndex)
//                {
//                    char c = line[letterIndex];

//                    switch (c)
//                    {
//                        case Comment:
//                            return WordType.Ignore;

//                        case '"':
//                            return WordType.StringNewLine;

//                        case ' ':                            
//                            if (TextLib.EqualStrings(word, ref MessageId))
//                            {
//                                return WordType.ID;
//                            }
//                            if (TextLib.EqualStrings(word, ref MessageString))
//                            {
//                                return WordType.String;
//                            }
//                            break;

//                        default:
//                            word.Append(c);
//                            break;
//                    }
//                }
//            }

//            letterIndex = 0;
//            return WordType.Ignore;
//        }

//        bool findFirstQuote(string line, ref int letterIndex)
//        {
//            for (int i = letterIndex; i < line.Length; ++i)
//            {
//                if (line[i] == '"')
//                {
//                    letterIndex = i + 1;
//                    return true;
//                }
//            }

//            return false;
//        }

//        string readToEndQuote(string line, int letterIndex)
//        {
//            word.Clear();

//            for (int i = letterIndex; i < line.Length; ++i)
//            {
//                if (line[i] == '"')
//                {
//                    break;
//                }
//                else
//                {
//                    word.Append(line[i]);
//                }
//            }

//            return word.ToString();
//        }

//        enum WordType
//        {
//            Ignore,
//            ID,
//            String,
//            StringNewLine,
//        }
//    }

//    struct PrintArg
//    {
//        public string variableId;
//        public string value;

//        public PrintArg(string variableId, string value)
//        {
//            this.variableId = variableId;
//            this.value = value;
//        }

//        public PrintArg(string variableId, int value)
//        {
//            this.variableId = variableId;
//            this.value = value.ToString(); 
//        }
//    }

//    class LanguageString
//    {
//        public int id;
//        public string line1 = null, line2 = null;

//        public LanguageString(string id)
//        {
//            this.id = id.GetHashCode();
//        }

//        public bool IsComplete()
//        {
//            return line1 != null;
//        }
//    }

//}
