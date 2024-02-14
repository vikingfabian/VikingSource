using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Engine
{
    //class BasicUpdateQue : UpdateQue
    //{
    //    public const int RunLasyUpdate = 1;
    //    public const int BASIC_UPDATE_PARTS = 3;
    //    int part;

    //    public BasicUpdateQue(int _part)
    //        : base()
    //    {
    //        part = _part;
    //    }
    //    public override string ToString()
    //    {
    //        return "Basic Update(" + part.ToString() + ")";
    //    }
    //    public override void Update()
    //    {
    //        switch (part)
    //        {
    //            case 0:
    //                Ref.draw.UpdateCulling(true);
    //                break;
    //            case RunLasyUpdate:
    //                //Engine.XGuide.CheckPlayerStatus();
    //                Ref.update.Time_UpdateLasyList();
    //                break;
    //            case 2:
    //                Ref.draw.UpdateCulling(false);
    //                break;
    //        }
    //        //Ref.draw.UpdateCulling(part1);
    //        Ref.gamestate.RunBasicUpdate();
    //    }
    //}
}
