//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////xna

//namespace VikingEngine.ToGG
//{
    //class NetActionAttack_Host : AbsNetActionHost
    //{
    //    public NetActionAttack_Host(System.IO.BinaryWriter movementWriter, Battle.BattleAttackAndDefenceCollection attacks)
    //        :base(movementWriter)
    //    {
    //        attacks.write(this.writer);
    //    }

    //    public void AddAttack(Battle.AttackResult result, bool blockedAttack)
    //    {
    //        writer.Write(true);
    //        writer.Write((byte)result);
    //        writer.Write(blockedAttack);
    //    }

    //    public void AttackComplete()
    //    { 
    //        writer.Write(false);
    //    }

    //    protected override NetActionType Type
    //    {
    //        get { return NetActionType.Attack; }
    //    }
    //}

    //class NetActionAttack_Client : AbsNetActionClient
    //{
    //    Battle.AttackAnimationClient attackAnim;
    //    bool moreAttacks;
    //    public  AbsGenericPlayer player;

    //    public NetActionAttack_Client(System.IO.BinaryReader packet, VectorRect area)
    //        : base(packet)
    //    {
    //        Battle.BattleAttackAndDefenceCollection attacks = new BattleAttackAndDefenceCollection();
    //        attacks.read(reader, out player);

    //        updateHasMoreAttacks();

    //        attackAnim = new AttackAnimationClient(attacks, area, this);
    //    }

    //    public override bool Update()
    //    {
    //        if (attackAnim.Update())
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    void updateHasMoreAttacks()
    //    {
    //        moreAttacks = reader.ReadBoolean();
    //    }

    //    public bool hasMoreAttacks()
    //    {
    //        return moreAttacks;
    //    }

    //    public AttackResult nextAttack(out bool blockedAttack)
    //    {
    //        AttackResult result = (AttackResult)reader.ReadByte();
    //        blockedAttack = reader.ReadBoolean();

    //        updateHasMoreAttacks();

    //        return result;
    //    }
    //}
//}
