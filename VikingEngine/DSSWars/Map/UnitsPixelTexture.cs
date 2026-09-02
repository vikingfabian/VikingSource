using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars.Map
{


    class UnitsPixelTexture: AbsMapPixelTexture
    {
        Color colorProfile1, colorProfile2;

        //Faction playerFaction;

        public UnitsPixelTexture(int playerIx) 
            :base(playerIx)
        {
            
            initTexture();
            //mapPlane.Y += 0.06f;
           
        }

        public void asynch_Begin()
        {
            texture.ClearPixelArray(ColorExt.Empty);
        }

        public void complete()
        {
            texture.ApplyPixelsToTexture();
        }

        //public override void Draw(int cameraIndex)
        //{
        //    base.Draw(cameraIndex);

        //}

        public void updateColorProfile(PFaction pfaction)
        {
            var playerPFaction = DssRef.state.localPlayers[playerIx].pfaction;
            
            if (pfaction == playerPFaction)
            {
                colorProfile1 = Color.Green;
                colorProfile2 = Color.LightGreen;
            }
            else
            {
                var relation = DssRef.world.diplomacy.GetRefRelation_Safe(playerPFaction, pfaction).Relation;
                if (relation <= RelationType.RelationTypeN1_Enemies)
                {
                    colorProfile1 = Color.Red;
                    colorProfile2 = Color.DarkRed;
                }
                else if (relation >= RelationType.RelationType3_Ally)
                {
                    colorProfile1 = Color.Blue;
                    colorProfile2 = Color.LightBlue;
                }
                else
                {
                    colorProfile1 = Color.Gray;
                    colorProfile2 = Color.LightGray;
                }
            }
            
        }


        public void asynch_AddArmy(Army army)
        {
            IntVector2 previousPos = IntVector2.NegativeOne;

            var groupsCounter = army.groups.counter();
            while (groupsCounter.Next())
            {
                if (previousPos != groupsCounter.sel.tilePos)
                {
                    previousPos = groupsCounter.sel.tilePos;
                    IntVector2 topleft = new IntVector2(
                        groupsCounter.sel.position.X - 0.5f,
                        groupsCounter.sel.position.Z - 0.5f);

                    if (texture.InBound_TwoPixels(topleft))
                    {
                        if (lib.IsEven(topleft.X + topleft.Y))
                        {
                            texture.SetTwoPixels(topleft, colorProfile1, colorProfile2);
                            topleft.Y++;
                            texture.SetTwoPixels(topleft, colorProfile2, colorProfile1);
                        }
                        else
                        {
                            texture.SetTwoPixels(topleft, colorProfile2, colorProfile1);
                            topleft.Y++;
                            texture.SetTwoPixels(topleft, colorProfile1, colorProfile2);
                        }
                    }
                }
                
            }
        }
    }
}
