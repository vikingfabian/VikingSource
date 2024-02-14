using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

//namespace VikingEngine.Debug
//{
//    class TweakValue
//    {
//    }
//    abstract class AbsDebugValue
//    {
//        public string name;
//        public float Value;

//        public AbsDebugValue(string name)
//        {
//            this.name = name;
//        }

//        virtual public void AddToMenu(HUD.File file, int index, int dialogue)
//        {
//            file.AddTextLink(ToString(), new HUD.Link(dialogue, index));
//        }
//        public override string ToString()
//        {
//            return name + ":" + Value.ToString();
//        }
//    }


//    abstract class AbsTweakValue //: AbsDebugValue
//    {
//        public string Name { get; protected set; }
//        public AbsTweakValue(string name)
//        {
//            this.Name = name;
//        }


//        abstract public void Change(string input, int index);
//        abstract public string[] EditBoxes();
//    }



//    /// <summary>
//    /// Be able to tweak constants from a debug menu
//    /// </summary>
//    class TweakValueFloat : AbsTweakValue
//    {
//        public float Value;

//        public TweakValueFloat(string name, float startval)
//            : base(name)
//        {
//            Value = startval;
//        }
//        public override void Change(string input, int index)
//        {
//            Value = (float)lib.safeStringValue(input);
//        }
//        public override string ToString()
//        {
//            return Name + " " + Value.ToString();
//        }

//        override public string[] EditBoxes()
//        {
//            return new string[] { Value.ToString() };
//        }
//    }

 
//}
