using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.QueAction
{
    abstract class AbsQueAction
    {
        public Time viewTime = new Time(0.8f, TimeUnit.Seconds);
        public QueState state = QueState.QuedUp;
        protected TimeStamp timeStamp;

        public int index;
        public bool isLocalAction = true;
        protected IntVector2 camTarget = IntVector2.NegativeOne;
        protected bool camTargetInCamCheck = true;

        public Que inQue;

        public AbsQueAction(AbsGenericPlayer player, AbsHQPlayer lockOtherPlayer = null)
        {
            if (IsPlayerQue_InMode())
            {
                player.que.Add(this);
                if (lockOtherPlayer != null)
                {
                    lockOtherPlayer.otherPlayerLockQue.Add(this);
                }
            }
            else
            {
                //throw new Exception();
                addToGameQue();
            }
        }
        virtual public void onBegin() { }
        virtual public void onRemove() { }

        protected void lockTargetInAnimation(AbsUnit target)
        {
            target.hq().PlayerHQ.otherPlayerLockQue.Add(this);
        }

        public AbsQueAction()
        {
            if (!IsPlayerQue_InMode())
            {
                addToGameQue();
            }
        }

        bool IsPlayerQue_InMode()
        {
            if (toggRef.mode == GameMode.HeroQuest)
            {
                return IsPlayerQue;
            }
            return false;
        }

        public void startSetup()
        {   
            state = QueState.Started;
            timeStamp = TimeStamp.Now();

            onBegin();

            if (isLocalAction && NetShared)
            {
                BeginNetWrite();
            }
        }

        protected void addToGameQue()
        {
           
            if (toggRef.NetHost)
            {
                index = toggRef.gamestate.nextQueIndex++;
            }
            toggRef.gamestate.que.Add(this);
        }

        public AbsQueAction(System.IO.BinaryReader r)
        {
            isLocalAction = false;

            netRead(r);

            toggRef.gamestate.que.Add(this);
        }

        public void BeginNetWrite()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqQueAction, Network.PacketReliability.Reliable);
            w.Write((byte)Type);
            netWrite(w);
        }

        virtual protected void netWrite(System.IO.BinaryWriter w)
        {            
            w.Write((ushort)index);
        }

        virtual protected void netRead(System.IO.BinaryReader r)
        {
            index = r.ReadUInt16();
        }

        virtual public bool update()
        {
            return viewTime.CountDown();
        }

        virtual public bool readyToStart()
        {
            return true;
        }

        virtual public bool CameraTarget(out IntVector2 camTarget, out bool inCamCheck)
        {
            camTarget = this.camTarget;
            inCamCheck = this.camTargetInCamCheck;

            return false;
        }

        public void setNextInQue()
        {
            inQue.queActions.Remove(this);
            inQue.queActions.Insert(1, this);
        }

        abstract public QueActionType Type { get; }

        virtual public bool NetShared { get { return true; } }

        virtual public bool IsPlayerQue { get { return false; } }

        virtual public bool IsTopPrio { get { return false; } }

        public static AbsQueAction NetReadAction(Network.ReceivedPacket packet)
        {
            QueActionType type = (QueActionType)packet.r.ReadByte();
            switch (type)
            {
                case QueActionType.StartTurn:
                    return new HeroQuest.QueAction.QueActionStartTurn(packet.r);
                case QueActionType.EndTurn:
                    return new HeroQuest.QueAction.QueActionEndTurn(packet.r);
                //case QueActionType.MonsterRespawn:
                //    return new MonsterRespawn(
                case QueActionType.PassiveSkill_DamageUnit:
                    return new HeroQuest.QueAction.PassiveSkillDamageUnit(packet.r);
                case QueActionType.SpringTrapDamage:
                    return new HeroQuest.QueAction.SpringTrapDamage(packet.r);

                case QueActionType.HeroDeath:
                    return new HeroQuest.QueAction.HeroDeath(packet.r);
                case QueActionType.DoomSkullObjective:
                    return new HeroQuest.QueAction.DoomSkullObjective(packet.r);
                case QueActionType.GameOver:
                    return new HeroQuest.QueAction.GameOver(packet.r);
                case QueActionType.Interact:
                    return new HeroQuest.QueAction.Interact(packet.r);

                case QueActionType.PerformGrapple:
                    return new HeroQuest.Data.PerformGrapple(packet.r);
                case QueActionType.PerformHeal:
                    return new HeroQuest.Data.Property.PerformHealAction(packet.r);
                case QueActionType.PerformRetaliate:
                    return new HeroQuest.Data.PerformRetaliate(packet.r);
                case QueActionType.PerformThrow:
                    return new HeroQuest.Data.Property.PerformThrowAction(packet.r);


                default:
                    throw new NotImplementedExceptionExt("NetReadAction " + type.ToString(), (int)type);
            }
        }
    }

    enum QueState
    {
        QuedUp,
        Started,
        Completed,
    }
    enum QueActionType
    {
        StartTurn,
        EndTurn,
        MonsterRespawn,
        MonsterSpawnGroup,
        DoomClock,
        PassiveSkill_DamageUnit,
        SpringTrapDamage,
        HeroDeath,
        DoomSkullObjective,
        GameOver,
        MoveAction,
        Interact,
        UnitPropertyQueAction,

        PerformThrow,
        PerformHeal,
        PerformGrapple,
        PerformRetaliate,

    }
}
