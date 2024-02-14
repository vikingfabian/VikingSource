//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
////using Microsoft.Xna.Framework.Storage;

//namespace VikingEngine.DataLib
//{
//    class DataManager : SaveLoad
//    {
//        const int CurrentSaveVersion = 3;
//        List<DataManagerObject> dataObjects = new List<DataManagerObject>();
//        int selectedObj = 0;
//        string SaveDir = TextLib.EmptyString;
//        string FileName = TextLib.EmptyString;
//        public int Version = CurrentSaveVersion;

//        public DataManager(string folder, string filename)
//            : this(folder, filename, true)
//        { }
//        public DataManager(string folder, string filename, bool loadfile)
//        {
//            if (loadfile)
//            {
//                List<string> file = this.LoadTextFile(folder, filename);
//                if (file.Count > 1)
//                {
//                    int firstLine = 0;
//                    Version = 1;
//                    //load version control text
//                    if (file[0].Length > SaveData.Version.Length)
//                    {
//                        if (TextLib.FirstLetters(file[0], SaveData.Version.Length) == SaveData.Version)
//                        {
//                            Version = (int)lib.safeStringValue(file[0]);
//                            firstLine = 1;
//                        }
//                    }
//                    for (int i = file.Count - 1; i > firstLine; i--)
//                    {
//                        if (file[i].Length > 2)
//                        {
//                            //if (file[i][0] == SaveData.BREAK_LINE)
//                            //{
//                            //    file[i - 1] += file[i].Remove(0, 1);
//                            //}
//                        }
//                    }
//                }
//                foreach (string line in file)
//                {
//                    if (line.Length > 2)
//                    {
//                        DataManagerObject obj = null;
//                        switch (line[0])
//                        {
//                            case SaveData.HeadName:
//                                obj = new Seperator();
//                                break;
//                            case SaveData.Comment:
//                                obj = new Comment();
//                                break;
//                            //case SaveData.BREAK_LINE:
//                            //    //ignore
//                            //    break;
//                            case SaveData.LineObject:
//                                obj = new TextLineObject();
//                                break;
//                            default:
//                                obj = new DataObject();
//                                break;
//                        }
//                        if (obj != null)
//                        {
//                            obj.ReadLine(line);
//                            dataObjects.Add(obj);
//                        }
//                    }
//                }
//            }
//            SaveDir = folder;
//            FileName = filename;
//        }

//        public void SelectFirstObject()
//        { selectedObj = 0; }

//        public bool NextObj()
//        {
//            selectedObj++;
//            return selectedObj < dataObjects.Count;
//        }
//        public DataManagerObject SelectedManagerObject()
//        { return dataObjects[selectedObj]; }
//        public DataObject SelectedDataObject
//        {
//            get
//            {
//                if (selectedObj >= dataObjects.Count) return null;
//                return (DataObject)dataObjects[selectedObj];
//            }
//        }
//        public bool SelectName(string name)
//        {
//            for (int i = 0; i < dataObjects.Count; i++)
//            {
//                if (dataObjects[i].Name == name)
//                { selectedObj = i; return true; }
//            }
//            return false;
//        }
//        public bool LoadError()
//        { return dataObjects.Count == 0; }
//        public void SaveFile()
//        {
//            List<string> file = new List<string> { SaveData.Version + Version.ToString() };

//            foreach (DataManagerObject obj in dataObjects)
//            {
//                file.Add(obj.WriteLine());
//            }
//            CreateTextFile(FileName, file);
//        }
//        public void AddObject(DataManagerObject obj)
//        { dataObjects.Add(obj); }
//        public void AddDataObject(string name, string type)
//        {
//            selectedObj = dataObjects.Count;
//            dataObjects.Add(new DataObject(name, type));
//        }
//        public List<string> TextLineObjects
//        {
//            get
//            {
//                List<string> ret = new List<string>();
//                foreach (DataManagerObject obj in dataObjects)
//                {
//                    if (obj.ObjectType() == DataObjectType.TextLineObject)
//                    { ret.Add(obj.Name); }
//                }
//                return ret;
//            }
//            set
//            {
//                foreach (string s in value)
//                {
//                    dataObjects.Add(new TextLineObject(s));
//                }
//            }
//        }
//    }

//    abstract class DataManagerObject
//    {
//        protected const string NAME = "nm";
//        protected const string TYPE = "ty";
//        public string Name;
//        public DataManagerObject(string _name)
//        {
//            Name = _name;
//        }
//        public abstract DataObjectType ObjectType();
//        public abstract void ReadLine(string line);
//        public abstract string WriteLine();
//    }
//    class DataObject : DataManagerObject
//    {
//        public string Type;
//        Dictionary<string, string> data = new Dictionary<string, string>();

//        public DataObject()
//            : this(TextLib.EmptyString, TextLib.EmptyString)
//        { }

//        public DataObject(string name, string type)
//            : base(name)
//        {
//            Type = type;
//            SaveVariable(Name, NAME, DataType.String, true);
//            SaveVariable(Type, TYPE, DataType.String, true);

//        }

//        public string GetValue(string key)
//        {
//            if (!data.ContainsKey(key)) { return TextLib.EmptyString; }
//            return data[key];
//        }
//        void safeAddValue(string value, ref string key)
//        {
//            if (data.ContainsKey(key))
//            {
//                data[key] = value;
//            }
//            else
//            { data.Add(key, value); }
//        }
//        public void SaveBoolean(ref bool variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.BoolToScript(variable), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = lib.ScriptToBool(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = false;
//                }
//            }
//        }
//        public void SaveFloat(ref float variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.SafeFloatToString(variable), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = (float)lib.safeStringValue(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = 0;
//                }
//            }
//        }
//        public void SaveInteger(ref int variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(variable.ToString(), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = lib.SafeStringToInt(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = 0;
//                }
//            }
//        }
//        public void SaveString(ref string variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(variable, ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = data[key];
//                }
//                else //Key missing
//                {
//                    variable = TextLib.EmptyString;
//                }
//            }
//        }
//        public void SaveVector2(ref Vector2 variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.Vec2Text(variable), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = lib.StringToV2(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = Vector2.Zero;
//                }
//            }
//        }
//        public void SaveIntVector2(ref IntVector2 variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.IntVec2Text(variable), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = lib.StringToIntV2(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = IntVector2.Zero;
//                }
//            }
//        }
//        public void SaveVector3(ref Vector3 variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.Vec3Text(variable), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = lib.StringToV3(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = Vector3.Zero;
//                }
//            }
//        }
//        public void SaveVector4(ref Vector4 variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.Vec4Text(variable), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = lib.StringToV4(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = Vector4.Zero;
//                }
//            }
//        }
//        public void SaveQuarterion(ref Quaternion variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.QuatText(variable), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable = lib.StringToQuat(data[key]);
//                }
//                else //Key missing
//                {
//                    variable = Quaternion.Identity;
//                }
//            }
//        }
//        public void SaveRotationQuarterion(ref RotationQuarterion variable, string key, bool save)
//        {
//            if (save)
//            {
//                safeAddValue(lib.QuatText(variable.QuadRotation), ref key);
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    variable.QuadRotation = lib.StringToQuat(data[key]);
//                }
//                else //Key missing
//                {
//                    variable.QuadRotation = Quaternion.Identity;
//                }
//            }
//        }
//        /// <summary>
//        /// This old version is replaced with a new
//        /// </summary>
//        public object SaveVariable(object variable, string key, DataType type, bool save)
//        {

//            if (save)
//            {
//                string value = TextLib.EmptyString;
//                switch (type)
//                {
//                    case DataType.Boolean:
//                        value = lib.BoolToScript((bool)variable);
//                        break;
//                    case DataType.Float:
//                        value = ((float)variable).ToString();
//                        break;
//                    case DataType.Integer:
//                        value = ((int)variable).ToString();
//                        break;
//                    case DataType.String:
//                        value = (string)variable;
//                        break;
//                    case DataType.Vector2:
//                        value = lib.Vec2Text((Vector2)variable);
//                        break;
//                    case DataType.Vector3:
//                        value = lib.Vec3Text((Vector3)variable);
//                        break;
//                    case DataType.Vector4:
//                        value = lib.Vec4Text((Vector4)variable);
//                        break;
//                    case DataType.Quaterion:
//                        value = lib.QuatText((Quaternion)variable);
//                        break;
//                }
//                if (data.ContainsKey(key))
//                {
//                    data[key] = value;
//                }
//                else
//                { data.Add(key, value); }
//            }
//            else //Load
//            {
//                if (data.ContainsKey(key))
//                {
//                    //string value = data[key];
//                    switch (type)
//                    {
//                        case DataType.Boolean:
//                            variable = lib.ScriptToBool(data[key]);
//                            break;
//                        case DataType.Float:
//                            variable = (float)lib.safeStringValue(data[key]);
//                            break;
//                        case DataType.Integer:
//                            if (data[key] == TextLib.EmptyString) { variable = 0; }
//                            else
//                            { variable = (int)Convert.ToInt16(data[key]); }
//                            break;
//                        case DataType.String:
//                            variable = data[key];
//                            break;
//                        case DataType.Vector2:
//                            variable = lib.StringToV2(data[key]);
//                            break;
//                        case DataType.Vector3:
//                            variable = lib.StringToV3(data[key]);
//                            break;
//                        case DataType.Vector4:
//                            variable = lib.StringToV4(data[key]);
//                            break;
//                        case DataType.Quaterion:
//                            variable = lib.StringToQuat(data[key]);
//                            break;

//                    }
//                }
//                else //Key missing
//                {
//                    switch (type)
//                    {
//                        case DataType.Boolean:
//                            return false;
//                        case DataType.String:
//                            return TextLib.EmptyString;
//                        case DataType.Vector2:
//                            return Vector2.Zero;
//                        case DataType.Vector3:
//                            return Vector3.Zero;
//                        case DataType.Vector4:
//                            return Vector4.Zero;
//                        case DataType.Quaterion:
//                            return Quaternion.Identity;
//                        default:
//                            return 0;
//                    }
//                }
//            }
//            return variable;
//        }
//        public override void ReadLine(string line)
//        {
//            ScriptCommands2 sc = new ScriptCommands2();
//            sc.LoadLine(line);
//            Name = sc.GetValue(NAME);
//            Type = sc.GetValue(TYPE);
//            data = sc.Data();
//        }


//        public override string WriteLine()
//        {
//            string line = TextLib.EmptyString;
//            foreach (KeyValuePair<string, string> kv in data)
//            {
//                line += kv.Key + SaveData.LParen + kv.Value + SaveData.RParen;
//            }
//            return line;
//        }
//        public override DataObjectType ObjectType()
//        {
//            return DataObjectType.Object;
//        }
//    }
//    class TextLineObject : DataManagerObject
//    {
//        public TextLineObject()
//            : base(TextLib.EmptyString)
//        { }
//        public TextLineObject(string text)
//            : base(text)
//        { }
//        public override void ReadLine(string line)
//        {
//            Name = line.Remove(0, 1);
//        }
//        public override string WriteLine()
//        {
//            return SaveData.LineObject + Name;
//        }
//        public override DataObjectType ObjectType()
//        {
//            return DataObjectType.TextLineObject;
//        }
//    }
//    class Seperator : DataManagerObject
//    {
//        public Seperator()
//            : base(TextLib.EmptyString)
//        { }
//        public Seperator(string text)
//            : base(text)
//        { }
//        public override void ReadLine(string line)
//        {
//            Name = line.Remove(0, 1);
//        }
//        public override string WriteLine()
//        {
//            return SaveData.HeadName + Name;
//        }
//        public override DataObjectType ObjectType()
//        {
//            return DataObjectType.Seperator;
//        }
//    }
//    class Comment : DataManagerObject
//    {
//        public Comment()
//            : base(TextLib.EmptyString)
//        { }
//        public Comment(string text)
//            : base(text)
//        { }
//        public override void ReadLine(string line)
//        {
//            Name = line.Remove(0, 2);
//        }
//        public override string WriteLine()
//        {
//            return SaveData.Comment + Name;
//        }
//        public override DataObjectType ObjectType()
//        {
//            return DataObjectType.Comment;
//        }
//    }
//    enum DataObjectType
//    {
//        None = 0,
//        Object,
//        TextLineObject,
//        Seperator,
//        Comment,
//    }

//}
