//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace VikingEngine.DSSWars.Communication
//{
//    class RelationSystem
//    {
        
//        int factionCapacity;

//        public DiplomaticRelation[] diplomaticRelations;
//        int[] indexRegister;

//        public RelationSystem(int factionCapacity = 64) 
//        { 
//            this.factionCapacity = factionCapacity;
//            diplomaticRelations = new DiplomaticRelation[length()];
//            indexRegister = new int[factionCapacity -1];

//            int nextLength = factionCapacity - 1;
//            int currentIndex = 0;

//            for (int i = 0; i < factionCapacity -1; i++)
//            {
//                indexRegister[i] = currentIndex;
//                currentIndex += nextLength;
//                nextLength--;
//            }
//        }

//        int length()
//        {
//            return MathExt.GaussSum(factionCapacity -1);
//        }

//        public int RelationIndex(int faction1, int faction2)
//        {
//            int lowIndex, highIndex;
//            if (faction1 < faction2)
//            {
//                lowIndex = faction1;
//                highIndex = faction2;
//            }
//            else if (faction2 < faction1)
//            {
//                highIndex = faction1;
//                lowIndex = faction2;
//            }
//            else
//            {
//                return -1;
//            }

//            int index = indexRegister[lowIndex] + highIndex - lowIndex;
//            return index;
//        }

//        public DiplomaticRelation GetSafe(int faction1, int faction2)
//        {
//            if (faction1 < 0 || faction2 < 0 || faction1 == faction2)
//            {
//                return DiplomaticRelation.Empty;
//            }
//            return diplomaticRelations[RelationIndex(faction1, faction2)];
//        }

//        public DiplomaticRelation Get(int faction1, int faction2)
//        {
//            return diplomaticRelations[RelationIndex(faction1, faction2)];
//        }

//        public void Set(int faction1, int faction2, DiplomaticRelation relation)
//        {
//            diplomaticRelations[RelationIndex(faction1, faction2)] = relation;
//        }

//        public ref DiplomaticRelation GetRef(int faction1, int faction2)
//        {
//            return ref diplomaticRelations[RelationIndex(faction1, faction2)];
//        }

//        public void async_update(int faction)
//        {
//            for (int otherFaction = 0; otherFaction < DssRef.world.factions.Array.Length; otherFaction++)
//            {
//                if (faction != otherFaction)
//                {
//                    diplomaticRelations[RelationIndex(faction, otherFaction)].truce_update();
//                }
//            }

//            //foreach (var p in DssRef.state.localPlayers)
//            //{
//            //    for (int relIx = 0; relIx < p.faction.diplomaticRelations.Length; ++relIx)
//            //    {
//            //        var rel = p.faction.diplomaticRelations[relIx];
//            //        if (rel != null)
//            //        {
//            //            rel.truce_update();
//            //        }
//            //    }
//            //}
//        }
//    }
//}
