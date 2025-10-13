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
            radius = relBgScale.X * 1f;
            iconScale = relBgScale * 0.4f;
        }

        public void update(Faction selected, Vector2 flagPos, DiplomacyMap map)
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
                    if (selected.diplomaticRelations[i] != null && i != selected.myIndex)
                    {
                        var relationType = selected.diplomaticRelations[i].Relation;

                        if (relationType <= RelationType.RelationTypeN2_Truce)
                        {
                            addArrow(false);
                        }
                        else if (relationType >= RelationType.RelationType3_Ally)
                        {
                            addArrow(true);
                        }

                        void addArrow(bool goodRelation)
                        {
                            var otherFaction = DssRef.world.faction(i);
                            if (otherFaction != null)
                            {
                                Graphics.Image arrow;

                                if (arrowIndex < relationArrows.Count)
                                {
                                    arrow = relationArrows[arrowIndex];
                                }
                                else
                                {
                                    arrow = new Image(SpriteName.WhiteArea, Vector2.Zero, iconScale, HudLib.DiplomacyDisplayLayer - 1 - arrowIndex, true);
                                    relationArrows.Add(arrow);
                                }

                                arrow.Color = goodRelation? Color.Blue : Color.Orange;

                                Vector2 otherPos = map.flagPosition(otherFaction);
                                Vector2 diff = otherPos - flagPos;
                                diff.Normalize();
                                arrow.position = flagPos + diff * radius;

                                arrowIndex++;
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
