using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna

namespace VikingEngine.ToGG
{
    //class NetActionMove_Host : AbsNetActionHost
    //{
    //    public NetActionMove_Host(Unit unit, MoveLinesGroup moveLines)
    //        : base()
    //    {
    //        unit.writeIndex(writer);
    //        moveLines.Write(writer);
    //    }

    //    public void BackStab(IntVector2 pos)
    //    {
    //        writer.Write(true);
    //        pos.WriteByteStream(writer);
    //    }

    //    public override void Send()
    //    {
    //        writer.Write(false);
    //        base.Send();
    //    }

    //    protected override NetActionType Type
    //    {
    //        get { return NetActionType.Movement; }
    //    }
    //}

    //class NetActionMove_Client : AbsNetActionClient
    //{
    //    int state_CheckBackStab_ResolveBackstab_TimeOut = 0;
    //    MoveLinesGroup moveLines;
    //    NetActionAttack_Client backstabAction;
    //    Unit unit;
    //    Time viewMovementTime = new Time(600);
    //    AbsGenericPlayer player;

    //    public NetActionMove_Client(Network.ReceivedPacket packet)
    //        : base(packet.r)
    //    {
    //        unit = cmdRef.gamestate.GetUnit(reader, out player);
    //        moveLines = new MoveLinesGroup(unit, reader);
    //    }

    //    public override bool Update()
    //    {
    //        switch (state_CheckBackStab_ResolveBackstab_TimeOut)
    //        {
    //            case 0:
    //                bool hasBackStab =  reader.ReadBoolean();
    //                if (hasBackStab)
    //                {
    //                    state_CheckBackStab_ResolveBackstab_TimeOut = 1;
    //                    IntVector2 pos = IntVector2.FromByteStream(reader);
    //                    unit.SetVisualPosition(pos);
    //                    backstabAction = new NetActionAttack_Client(reader, Engine.Screen.SafeArea);
    //                }
    //                else
    //                {
    //                    state_CheckBackStab_ResolveBackstab_TimeOut = 2;
    //                    unit.SetPosition(moveLines.CurrentSquarePos());
    //                }
    //                break;
    //            case 1:
    //                if (backstabAction.Update())
    //                {
    //                    state_CheckBackStab_ResolveBackstab_TimeOut = 0;
    //                }
    //                break;
    //            case 2:
    //                if (viewMovementTime.CountDown())
    //                {
    //                    moveLines.DeleteMe();
    //                    return true;
    //                }
    //                break;

    //        }
    //        player.SpectatorTargetPos = cmdLib.ToV2(unit.soldierModel.Position);

    //        return false;
    //    }
    //}
}
