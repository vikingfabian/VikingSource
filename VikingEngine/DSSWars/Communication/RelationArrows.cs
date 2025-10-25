using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Communication
{
    class RelationArrows
    {
        Vector2 iconScale;
        float radius;
        List<Image> relationArrows = new List<Image>(32);
        Image selectHighlight;
        public RelationArrows(Vector2 relBgScale) 
        {
            radius = relBgScale.X * 1f;
            iconScale = relBgScale * 0.5f;
            selectHighlight = new Image(SpriteName.WhiteCirkle, Vector2.Zero, iconScale, HudLib.DiplomacyDisplayLayer - 5, true);
            selectHighlight.Visible = false;
        }

        public void preUpdate()
        { 
            selectHighlight.Visible = false;
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
                            if (otherFaction != null && !otherFaction.HasZeroUnits())
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

                                arrow.SetSpriteName(goodRelation? SpriteName.WarsRelationArrowAlly :  SpriteName.WarsRelationArrowWar);

                                Vector2 otherPos = map.flagPosition(otherFaction);
                                Vector2 diff = otherPos - flagPos;
                                diff.Normalize();
                                arrow.position = flagPos + diff * radius;
                                arrow.Rotation = lib.V2ToAngle_PreNorm_Unsafe(diff);
                                arrow.idOrIndex = i;
                                arrowIndex++;
                            }
                        }
                    }
                }

                clearFromIndex(arrowIndex);
            }

        }

        public bool factionArrowHover(LocalPlayer player, out int factionIndex)
        {
            Vector2 pointer = player.gameControls.map.pointerPos();
            float pointerRadius = iconScale.X * 0.5f;
            foreach (var img in relationArrows)
            {
                if ((img.position - pointer).Length() <= pointerRadius)
                {
                    factionIndex = img.idOrIndex;

                    selectHighlight.position = img.position;
                    selectHighlight.Visible = true;
                    selectHighlight.LayerBelow(img);
                    return true;
                }
            }

            factionIndex = -1;
            return false;
        } 

        void clearFromIndex(int start)
        {
            for (int i = relationArrows.Count -1; i >= start ; i--)
            {
                relationArrows[i].DeleteMe();
                relationArrows.RemoveAt(i);
            }
        }

        void ClearAll()
        { 
            clearFromIndex(0);
            
        }

        public void DeleteMe()
        {
            ClearAll();
            selectHighlight.DeleteMe();
        }
    }
}
