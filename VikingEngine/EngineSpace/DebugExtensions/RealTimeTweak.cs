using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.DebugExtensions
{
    abstract class AbsDebugValue
    {
        public string name;
        public float Value = 0.0f;

        public AbsDebugValue(string name)
        {
            this.name = name;
        }

        virtual public void AddToMenu(HUD.GuiLayout file, int index, int dialogue)
        {
            //file.AddTextLink(ToString(), new HUD.Link(dialogue, index));
        }
        public override string ToString()
        {
            return name + ":" + Value.ToString();
        }
    }


    abstract class AbsRealTimeTweak //: AbsDebugValue
    {
        public string Name { get; protected set; }
        public AbsRealTimeTweak(string name)
        {
            this.Name = name;
        }
        protected void start()
        {
            //DebugForm.AddRealTimeTweak(this);
        }

        abstract public void Change(string input, int index);
        abstract public string[] EditBoxes();
    }



    /// <summary>
    /// Be able to tweak constants from a debug menu
    /// </summary>
    class RealTimeFloatTweak : AbsRealTimeTweak
    {
        public float Value;

        public RealTimeFloatTweak(string name, float startval)
            :base(name)
        {
            Value = startval;
            start();
        }
        public override void Change(string input, int index)
        {
            Value = (float)lib.safeStringValue(input);
        }
        public override string ToString()
        {
            return Name + " " + Value.ToString();
        }

        override public string[] EditBoxes()
        {
            return new string[] { Value.ToString() };
        }
    }

    class RealTimeV3Tweak : AbsRealTimeTweak
    {
        public Vector3 Value;

        public RealTimeV3Tweak(string name, Vector3 startval)
            : base(name)
        {
            Value = startval;
            start();
        }
        public override void Change(string input, int index)
        {
            Value = VectorExt.SetDim(Value,  (Dimensions)index, (float)lib.safeStringValue(input));
        }
        public override string ToString()
        {
            return Name + " " + Value.ToString();
        }
        override public string[] EditBoxes()
        {
            return new string[] { Value.X.ToString(), Value.Y.ToString(), Value.Z.ToString(), };
        }
    }
}
