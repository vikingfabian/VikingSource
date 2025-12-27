using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Communication
{
    struct DiplomaticRelation
    {
        //int faction1, faction2;
        public RelationType Relation;
        public SpeakTerms SpeakTerms;
        public GameTimeStamp RelationEnd_GameTimeSec;
        public bool secret = false;
        public int allyAgainst = -1;

        public DiplomaticRelation()
        { 
            Relation = RelationType.RelationType0_Neutral;
            SpeakTerms = SpeakTerms.SpeakTerms0_Normal;
        }

        //public DiplomaticRelation(int faction1, int faction2, RelationType Relation, SpeakTerms speakterms)
        //{
        //    this.Relation = Relation;
        //    this.SpeakTerms = speakterms;

        //    if (faction1 < faction2)
        //    {
        //        this.faction1 = faction1;
        //        this.faction2 = faction2;
        //    }
        //    else
        //    {
        //        this.faction1 = faction2;
        //        this.faction2 = faction1;
        //    }

        //    addToFactions();
        //}

        //public void addToFactions()
        //{
        //    //if (arraylib.InBound(DssRef.world.factions.Array, faction1, faction2))
        //    ////{
        //    //    if (DssRef.world.factions.Array[faction1] != null &&
        //    //        DssRef.world.factions.Array[faction2] != null)
        //    //    {

        //    var f1 = DssRef.world.faction(faction1);
        //    var f2 = DssRef.world.faction(faction2);

        //    if (f1 != null && f2 != null)
        //    {
        //        f1.diplomaticRelations[faction2] = this;
        //        f2.diplomaticRelations[faction1] = this;
        //    }
        //    //}
        //}

        public void write(System.IO.BinaryWriter w)
        {
            //w.Write((short)faction1);
            //w.Write((short)faction2);


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

            //w.Write((sbyte)Relation);
            //w.Write((sbyte)SpeakTerms);
            //RelationEnd_GameTimeSec.write_ushort(w);
            //w.Write(Convert.ToUInt16(RelationEnd_GameTimeSec));
        }

        public bool read(System.IO.BinaryReader r, int subVersion)
        {
            faction1 = r.ReadInt16();
            if (faction1 >= 0)
            {
                faction2 = r.ReadInt16();
                if (subVersion < 58)
                {
                    Relation = (RelationType)r.ReadSByte();
                    SpeakTerms = (SpeakTerms)r.ReadSByte();
                    RelationEnd_GameTimeSec.read_ushort(r);
                }
                else
                {
                    EightBit bools = EightBit.FromStream(r);
                    bools.Get(out bool hasRelation, out bool hasSpeakTerms, out bool hasEndTime, out bool hasCommonEnemy);
                    if (hasRelation)
                    {
                        Relation = (RelationType)r.ReadSByte();
                    }
                    if (hasSpeakTerms)
                    {
                        SpeakTerms = (SpeakTerms)r.ReadSByte();
                    }
                    if (hasEndTime)
                    {
                        RelationEnd_GameTimeSec.read(r);
                    }

                    if (subVersion >= 72)
                    {
                        if (hasCommonEnemy)
                        {
                            allyAgainst = r.ReadUInt16();
                        }
                        else
                        {
                            allyAgainst = -1;
                        }
                    }
                }
                return true;
            }

            return false;
        }

        public bool opponentIsPlayer(Faction faction)
        {
            return !opponent(faction).player.IsBot();
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

        public Faction opponent(Faction faction)
        {
            if (faction.myIndex == faction1)
            {
                return DssRef.world.faction(faction2);
            }
            else
            {
                return DssRef.world.faction(faction1);
            }
        }

        public void truce_update()
        {
            if (Relation == RelationType.RelationTypeN2_Truce &&
                RelationEnd_GameTimeSec.TimeOut())
            {
                Relation = RelationType.RelationTypeN3_War;
            }
        }

        public bool IsFactionOne(Faction faction)
        {
            return faction.myIndex == faction1;
        }
    }
}
