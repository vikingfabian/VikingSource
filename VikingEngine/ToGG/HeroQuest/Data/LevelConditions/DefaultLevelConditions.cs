using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.HeroQuest.MapGen;
using VikingEngine.ToGG.HeroQuest.Players.Ai;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class DefaultLevelConditions
    {
        protected static readonly Color FlavorTextColor = new Color(205, 193, 175);
        protected List2<AbsUnit> taggedUnits = new List2<AbsUnit>();
        protected List<int> gotEvent = new List<int>();
        public DoomData doom = new DoomData(8);
        public List<IntVector2> objectiveAttackTargets = null;
        
        public void refeshConditions()
        {
            if (taggedUnits.Count > 0)
            {
                taggedUnits.loopBegin(false);
                while (taggedUnits.loopNext())
                {
                    if (taggedUnits.sel.Dead)
                    {
                        taggedUnits.loopRemove();
                    }
                }

                if (taggedUnits.Count == 0)
                {
                    new QueAction.GameOver(true);
                }
            }
        }

        public void addUnit(AbsUnit unit)
        {
            taggedUnits.Add(unit);
        }

        virtual public void eventFlag(IntVector2 position, int id, AbsUnit unit)
        { }

        protected bool newEventFlag(int id)
        {
            if (gotEvent.Contains(id))
            {
                return false;
            }
            else
            {
                gotEvent.Add(id);
                return true;
            }
        }

        virtual public void OnEvent(ToGG.Data.EventType eventType, object tag)
        { }

        virtual public void OnObjective(Unit unit, AttackTargetGroup targetGroup, AiObjectiveType objectiveType, bool local)
        { }
        
        protected void netWriteObjective(Unit unit, AiObjectiveType objectiveType)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqOnObjective, Network.PacketReliability.Reliable);
            unit.netWriteUnitId(w);
            w.Write((byte)objectiveType);
        }

        public void netReadObjective(System.IO.BinaryReader r)
        {
            var u = Unit.NetReadUnitId(r);
            AiObjectiveType objectiveType = (AiObjectiveType)r.ReadByte();

            if (u != null)
            {
                OnObjective(u, null, objectiveType, false);
            }
        }

        public System.IO.BinaryWriter netWriteConditionEvent(byte eventId)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqLevelConditionEvent, Network.PacketReliability.Reliable);
            w.Write(eventId);

            return w;
        }

        virtual public void onConditionEvent(byte eventId, System.IO.BinaryReader r)
        { }


        virtual public List<AbsRichBoxMember> questDescription()
        {
            return new List<AbsRichBoxMember>
            {
                new RichBoxText("Not set"),
            };
        }

        protected void flavorText(List<AbsRichBoxMember> rb, string text)
        {
            rb.Add(new RichBoxText(text, FlavorTextColor));
            rb.Add(new RichBoxNewLine(true));
        }

        protected void missionObjectivesTitle(List<AbsRichBoxMember> rb)
        {
            rb.Add(new RichBoxBeginTitle());
            rb.Add(new RichBoxText("Mission objectives"));
            rb.Add(new RichBoxNewLine());
        }

        protected void specialConditionsTitle(List<AbsRichBoxMember> rb)
        {
            rb.Add(new RichBoxBeginTitle());
            rb.Add(new RichBoxText("Special conditions"));
            rb.Add(new RichBoxNewLine());
        }

        protected void turnLimitText(List<AbsRichBoxMember> rb)
        {
            rb.Add(new RichBoxBeginTitle());
            rb.Add(new RichBoxText("Turn: " + HeroTurnCount.ToString() + "/" + TimeLimit.ToString()));
        }

        virtual public void environmentSpawn(MapGen.SpawnManager spawnManager)
        { }

        virtual public void monsterSpawn(MapGen.SpawnManager spawnManager)
        {
        }

        virtual public List<Unit> monsterRespawnSession(int turn, bool beforeActions)
        {
            return null;
        }

        virtual public bool HasDungeonMaster => true;

        virtual public DoomClockType DoomClock => DoomClockType.FourTurns;

        virtual public int? TimeLimit => null;

        protected int HeroTurnCount => hqRef.players.localHost.TurnsCount;

        virtual public bool EnemyLootDrop => true;
    }      

    

    enum DoomClockType
    {
        FourTurns,
        NoClock,
    }

}
