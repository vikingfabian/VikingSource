using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Communication
{
    class RelationArrows
    {
        Vector2 iconScale;
        float radius;
        List<Image> relationArrows = new List<Image>(32);

        public RelationArrows(Vector2 relBgScale) 
        {
            radius = relBgScale.X * 1.6f;
            iconScale = relBgScale * 0.4f;
        }

        public void update(Faction selected, DiplomacyMap map)
        {
            if (selected == null)
            {
                ClearAll();
            }
            else 
            {
                int arrowIndex = 0;

                for (int i = 0; i < selected.diplomaticRelations.Length; i++)
                {
                    if (selected.diplomaticRelations[i] != null)
                    {
                        var relationType = selected.diplomaticRelations[i].Relation;

                        if (relationType <= RelationType.RelationTypeN2_Truce)
                        {

                        }
                        else if (relationType >= RelationType.RelationType3_Ally)
                        {

                        }

                        void addArrow(bool goodRelation)
                        {
                            var otherFaction = DssRef.world.faction(i);
                            if (otherFaction != null)
                            {
                                
                            }
                        }
                    }
                }

                clearFromIndex(arrowIndex);
            }

        }

        void clearFromIndex(int start)
        {
            for (int i = relationArrows.Count -1; i >= start ; i--)
            {
                relationArrows[i].DeleteMe();
                relationArrows.RemoveAt(i);
            }
        }

        public void ClearAll()
        { 
            clearFromIndex(0);
        }
    }
}
