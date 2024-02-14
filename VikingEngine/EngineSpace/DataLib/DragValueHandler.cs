//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.DataLib
//{
//    //interface DragValueObj
//    //{
//    //    //To init the object: Get the index from gamestate.AddDragValueObj()
//    //    //int objIndex;
//    //    //int ObjIndex { get; }
//    //    float GetDragValue(string key);
//    //    void SetDragValue(string key, float value);
//    //    bool UseDecimals(string key);
//    //    float BasicStepping(string key);
//    //    IntervalF GetBounds(string key);
//    //    void SetTextObj(string key, Graphics.Text _textObj);
//    //    //void UpdateText(string key);
//    //    string ValueToString(string key);

//    //}
//    abstract class AbsDragValueObj : DragValueObj
//    {
//        protected const string Xpos = "Xpos";
//        protected const string Ypos = "Ypos";
//        protected const string Zpos = "Zpos";

//        protected const string XYZsz = "ScaleXYZ";
//        protected const string Xsz = "Xsz";
//        protected const string Ysz = "Ysz";
//        protected const string Zsz = "Zsz";

//        protected const string Xrot = "Xrot";
//        protected const string Yrot = "Yrot";
//        protected const string Zrot = "Zrot";

//        protected const string Xrot45 = "Xrot45";
//        protected const string Yrot45 = "Yrot45";
//        protected const string Zrot45 = "Zrot45";

//        //int objIndex;
//        //public int ObjIndex { get { return objIndex; } }
//        protected Dictionary<string, Graphics.Text> textLinks;
        

//       public AbsDragValueObj()
//       {
//           textLinks = new Dictionary<string, VikingEngine.Graphics.Text>();
//           //objIndex = Ref.gamestate.AddDragValueObj((DragValueObj)this); 
//       }

//       abstract public float GetDragValue(string key);
//       virtual public void SetDragValue(string key, float value)
//       {
//           UpdateText(key);
//       }
//        virtual public bool UseDecimals(string key)
//        { return true; }
//        virtual public IntervalF GetBounds(string key)
//        { return new IntervalF(); }
//        virtual public void SetTextObj(string key, Graphics.Text _textObj)
//        {
//            if (textLinks.ContainsKey(key))
//            {
//                textLinks[key] = _textObj;
//            }
//            else
//            { textLinks.Add(key, _textObj); }
//        }
//        protected void UpdateText(string key)
//        {
//            if (textLinks.ContainsKey(key))
//                textLinks[key].ChangeTextString(ValueToString(key));
//        }
//        virtual public void GenerateDocument(ref DataLib.DocFile file)
//        { }
//        //protected void addValuesToDoc(ref DataLib.DocFile file, List<string> keys)
//        //{
//        //    DataLib.DragValueObj thisbase = (DataLib.DragValueObj)this;
//        //    foreach (string key in keys)
//        //    {
//        //        SpriteName img = SpriteName.InterfaceDragBox;
//        //        switch (key[0])
//        //        {
//        //            case 'X':
//        //                img = SpriteName.InterfaceDragBoxX;
//        //                break;
//        //            case 'Y':
//        //                img = SpriteName.InterfaceDragBoxY;
//        //                break;
//        //            case 'Z':
//        //                img = SpriteName.InterfaceDragBoxZ;
//        //                break;

//        //        }

//        //        file.addDragValue(img, key, thisbase);
//        //    }
//        //}

//        virtual public float BasicStepping(string key)
//        { return 1; }
//        virtual public string ValueToString(string key)
//        { return key + ":" + GetDragValue(key).ToString(); }
//    }

//    class TestDragValue : AbsDragValueObj
//    {
        
//        public TestDragValue()
//            :base()
//        { }
//        public override void SetDragValue(string key, float value)
//        {
//            throw new NotImplementedException();
//        }
//        public override float GetDragValue(string key)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    class DragValueHandler : VikingEngine.AbsInput
//    {//Add this class to the Gamestate if you want the functions from the drag values
//        static int DragObjIx = 0;
//        public const char Prefix = '<';
//        const char IconLink = '1';
//        const char TextLink = '2';
//        const char Seperator = ':';
//        Dictionary<int, DataLib.DragValueObj> dragValueObjects;
//        DataLib.DragValueObj activeObj;
//        string activeKey;
//        bool haveActiveObj = false;
//        bool waitingForTextInput = false;
//        public bool HaveActiveObj { get { return haveActiveObj; } }
//        float restValue = 0;

//        public int GetNewObjIx(DataLib.DragValueObj obj)
//        {
//            DragObjIx++;
//            dragValueObjects.Add(DragObjIx, obj);
//            return DragObjIx;
//        }
//        public DragValueHandler()
//        {
//            dragValueObjects = new Dictionary<int, DragValueObj>();
//        }

//        public bool LinkEvent2(VikingEngine.HUD.ActiveControlLink linkType, string linkName)
//        {
//            if (linkType == VikingEngine.HUD.ActiveControlLink.DocumentLink)
//            {
//                int objIx = 0;
//                string key = TextLib.EmptyString;
//                bool icon = false;
//                if (ReadDocKey(ref key, ref objIx, ref icon, linkName))
//                {
//                    if (dragValueObjects.ContainsKey(objIx))
//                    {
//                        activeObj = dragValueObjects[objIx];
//                        activeKey = key;
//                        if (icon)
//                        {
//                            haveActiveObj = true;
//                        }
//                        else
//                        {
//                            waitingForTextInput = true;
//                            IntervalF bounds = activeObj.GetBounds(activeKey);
//                            Engine.XGuide.BeginKeyBoardInput(new Engine.KeyboardInputValues(
//                                "Set Vale to " + key, "Min Value(" + bounds.Min.ToString() + "), Max Value(" +
//                                bounds.Max.ToString(), activeObj.GetDragValue(activeKey).ToString(), Microsoft.Xna.Framework.PlayerIndex.One));
                                
//                        }
                        
//                    }
//                    return true;
//                }
//            }
//            return false;
//        }
//        public bool TextInput(string input)
//        {
//            if (waitingForTextInput)
//            {
//                IntervalF bounds = activeObj.GetBounds(activeKey);
//                updateActiveValue(Bound.SetBounds((float)lib.safeStringValue(input), bounds.Min, bounds.Max));
//                waitingForTextInput = false;
//                return true;
//            }
//            return false;
//        }
//        public void MouseMove_Event(Microsoft.Xna.Framework.Vector2 deltaPos, bool ctrl)//kan lägga till stepping
//        {
//            if (haveActiveObj)
//            {
//                float value = activeObj.GetDragValue(activeKey);
//                float basicstepping = activeObj.BasicStepping(activeKey);
//                float stepping = basicstepping;
//                if (ctrl) stepping *= 0.05f;
                
//                value += (deltaPos.X * stepping) - (deltaPos.Y * stepping) + restValue;
//                IntervalF bounds = activeObj.GetBounds(activeKey);
//                value = Bound.SetBounds(value, bounds.Min, bounds.Max);
//                restValue = 0;
//                if (!activeObj.UseDecimals(activeKey))
//                {
//                    float oldValue = value;
//                    value = (int)(value / basicstepping);
//                    value = (int)(value * basicstepping);
//                    restValue = oldValue - value;
                    
//                }
//                //activeObj.SetDragValue(activeKey, value);
//                //activeObj.TextObj.ChangeTextString(activeObj.ValueToString(activeKey));
//                updateActiveValue(value);
//            }
//        }

//        void updateActiveValue(float val)
//        {
//            activeObj.SetDragValue(activeKey, val);
//            //activeObj.TextObj.ChangeTextString(activeObj.ValueToString(activeKey));
//        }
//        public override void MouseClick_Event(Microsoft.Xna.Framework.Vector2 position, MouseButton button, bool keydown)
//        {
//            if (button == MouseButton.Left && !keydown)
//            { haveActiveObj = false; }
//        }
//        public static string CreateDocKey(string basicKey, int ObjIx, bool icon)
//        {
//            string key = Convert.ToString(icon ? IconLink : TextLink);
//            return Prefix + key + ObjIx.ToString() + Seperator + basicKey;
//        }
//        static bool ReadDocKey(ref string basicKey, ref int ObjIx, ref bool icon, string linkName)
//        {
//            if (linkName[0] == Prefix)
//            {

//                icon = linkName[1] == IconLink;

//                linkName = linkName.Remove(0, 2);
//                for (int ix = 0; ix < linkName.Length; ix++)
//                {
//                    if (linkName[ix] == Seperator)
//                    {
//                        ObjIx = Convert.ToInt16(linkName.Remove(ix, linkName.Length - ix));
//                        basicKey = linkName.Remove(0, ix + 1);
//                        break;
//                    }
//                }
//                return true;
//            }
//            return false;
//        }
//    }
    
//}
