using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.Communication
{
    struct DiplomaticRelation
    {
        public static readonly DiplomaticRelation Empty = new DiplomaticRelation();

        public RelationType Relation = RelationType.RelationType0_Neutral;
        public SpeakTerms SpeakTerms = SpeakTerms.SpeakTerms0_Normal;
        public GameTimeStamp RelationEnd_GameTimeSec;
        public bool secret = false;
        public int allyAgainst = -1;

        public override string ToString()
        {
            return $"{Relation}, {SpeakTerms}, end sec {RelationEnd_GameTimeSec.Seconds}";
        }
        public DiplomaticRelation()
        {   
        }
        public bool HasValue()
        { 
            return Relation != RelationType.RelationType0_Neutral || SpeakTerms != SpeakTerms.SpeakTerms0_Normal;
        }
        public bool InWar()
        {
            return Diplomacy.IsWar(Relation);
        }

        public bool InAlliance()
        {
            return Relation >= RelationType.RelationType3_Ally;
        }

        public void SetRelation(PFaction faction1, PFaction faction2, RelationType newRelation, PFaction actuator, out RelationType previousRelation, bool fromAllianceTrade, bool localAction)
        {
            previousRelation = Relation;

            if (Relation != newRelation )
                //&&
                //faction1 != null && faction2 != null)
            {
                Relation = newRelation;
                if (Relation == RelationType.RelationTypeN5_TotalWar)
                {
                    SpeakTerms = SpeakTerms.SpeakTermsN2_None;
                }
                faction1.GetPlayer()?.onNewRelation(actuator == faction2,faction2, this, previousRelation, fromAllianceTrade, true);
                faction2.GetPlayer()?.onNewRelation(actuator == faction1,faction1, this, previousRelation, fromAllianceTrade, true);

                if (localAction)
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssDiplomacyRelation, Network.PacketReliability.Reliable, out var packet);
                    {
                        EightBit actuators = new EightBit(actuator == faction1, actuator == faction2);
                        actuators.write(w);

                        faction1.write(w);
                        faction2.write(w);

                        write(w);
                    }
                    packet.EndWrite_Asynch();
                }
            }
        }

        public static void NetReadRelation(System.IO.BinaryReader r)
        {
            //Faction faction1 = Net.ObjectId.ReadFaction(r, out _);
            //Faction faction2 = Net.ObjectId.ReadFaction(r, out _);

            

            //if (faction1 != null && faction2 != null)
            {
                EightBit actuators = new EightBit(r);

                PFaction faction1 = new PFaction(r);
                PFaction faction2 = new PFaction(r);

                ref var rel = ref DssRef.world.diplomacy.GetRefRelation_Safe(faction1, faction2);
                var previousRelation = rel.Relation;
                rel.read(r, int.MaxValue);

                faction1.GetPlayer()?.onNewRelation(actuators.Get(1), faction2, rel, previousRelation, false, false);
                faction2.GetPlayer()?.onNewRelation(actuators.Get(0), faction1, rel, previousRelation, false, false);
            }
        }

        public void OnDeath()
        {
            Relation = RelationType.RelationType0_Neutral;
            SpeakTerms = SpeakTerms.SpeakTermsN2_None;
        }
       
        public void write(System.IO.BinaryWriter w)
        {
            
            bool hasRelation = Relation != RelationType.RelationType0_Neutral;
            bool hasSpeakTerms = SpeakTerms != SpeakTerms.SpeakTerms0_Normal;
            bool hasEndTime = RelationEnd_GameTimeSec.HasTime();
            bool hasCommonEnemy = allyAgainst >= 0;

            EightBit bools = new EightBit(hasRelation, hasSpeakTerms, hasEndTime, hasCommonEnemy);
            bools.write(w);

            if (hasRelation)
            {
                w.Write((sbyte)Relation);
            }
            if (hasSpeakTerms)
            {
                w.Write((sbyte)SpeakTerms);
            }
            if (hasEndTime)
            {
                RelationEnd_GameTimeSec.write(w);
            }
            if (hasCommonEnemy)
            {
                w.Write((ushort)allyAgainst);
            }
        }

        public void read(System.IO.BinaryReader r, int subVersion)
        {
            EightBit bools = EightBit.FromStream(r);
            bools.Get(out bool hasRelation, out bool hasSpeakTerms, out bool hasEndTime, out bool hasCommonEnemy);
            if (hasRelation)
            {
                Relation = (RelationType)r.ReadSByte();
            }
            else
            {
                Relation = RelationType.RelationType0_Neutral;
            }

            if (hasSpeakTerms)
            {
                SpeakTerms = (SpeakTerms)r.ReadSByte();
            }
            else
            {
                SpeakTerms = SpeakTerms.SpeakTerms0_Normal;
            }

            if (hasEndTime)
            {
                RelationEnd_GameTimeSec.read(r);
            }
        
            if (hasCommonEnemy)
            {
                allyAgainst = r.ReadUInt16();
            }
            else
            {
                allyAgainst = -1;
            }
        }

        public void writeRelation(System.IO.BinaryWriter w)
        {
            w.Write((sbyte)Relation);
        }
        public void readRelation(System.IO.BinaryReader r)
        {
            Relation = (RelationType)r.ReadSByte();
        }

        public void SetWorseSpeakTerms(double subOneChance, double subTwoChance)
        {

            if (Ref.rnd.Chance(subTwoChance))
            {
                changeSpeakTerms(-2);
            }
            if (Ref.rnd.Chance(subOneChance))
            {
                changeSpeakTerms(-1);
            }
        }

        void changeSpeakTerms(int change)
        {
            SpeakTerms = (SpeakTerms)Bound.Set((int)SpeakTerms + change, (int)SpeakTerms.SpeakTermsN2_None, (int)SpeakTerms.SpeakTerms1_Good);
        }


        public void truce_update()
        {
            if (Relation == RelationType.RelationTypeN2_Truce ||
                Relation == RelationType.RelationTypeN3_Mobilization)
            {
                if (RelationEnd_GameTimeSec.TimeOut())
                {
                    Relation = RelationType.RelationTypeN4_War;
                }
            }
        }

    }
}
