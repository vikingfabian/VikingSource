using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DebugExtensions
{


    class WatchValue : AbsDebugValue
    {
        static List<WatchValue> activeWatch;
        //public static int CurrentTweakValue;
        //public static string CurrentName { get { return activeWatch[CurrentTweakValue].name; } }
        //public static float CurrentValue { get { return activeWatch[CurrentTweakValue].Value; } }

        public WatchValue(string name)
            : base(name)
        {
            if (activeWatch == null)
            {
                activeWatch = new List<WatchValue>();
            }
            activeWatch.Add(this);
        }
        //public static HUD.GuiLayout ListTweaks(int dialogue)
        //{
        //    //HUD.File file = new HUD.File();
        //    //for (int i = 0; i < activeWatch.Count; i++)
        //    //{
        //    //    activeWatch[i].AddToMenu(file, i, dialogue);
        //    //}
        //    //return file;
        //}
        
    }
}
