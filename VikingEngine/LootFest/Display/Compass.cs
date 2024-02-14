//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.Graphics;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.LootFest
//{
//    class Compass
//    {
//        const float ScreenToRadiusRatio = 0.09f;
//        float CompassRadius = 4;
//        const float IconRadius = 0.86f;
//        float CompassIconPos = 4;
//        public const float NorthIconSz = 32;
//        //public float CompassIconSz = 5;
//        Image compass;
//        Image compassNsymbol;
//        Image compassPlayerDir;
//        Rotation1D compassRot = Rotation1D.D0;
//        Players.Player player;
//        //CompassGoalLocation questIcon;
//        //CompassGoalLocation flagIcon;
//        //List<AbsCompassIcon> compassIcons;
//        Vector2 previousCompassLocation = Vector2.Zero;

//        public Compass(Players.Player player, PlayState gamestate)
//        {
//            this.player = player;
           
//            compass = new Image(SpriteName.Compass, Vector2.Zero, Vector2.One * CompassRadius * PublicConstants.Twice, ImageLayers.Background6, true);
//            compassNsymbol = new Image(SpriteName.CompassNSymbol, Vector2.Zero, Vector2.One * NorthIconSz, ImageLayers.Background5, true);
//            compassPlayerDir = new Image(SpriteName.BoardPieceCenter, Vector2.Zero, Vector2.One, ImageLayers.Lay8, true);

//            questIcon = new CompassGoalLocation(gamestate.Progress, 0);
//            //flagIcon = new CompassGoalLocation(player, 1);
//            compassIcons = new List<AbsCompassIcon>
//            {
//                questIcon,
//               // flagIcon,
//            };
//        }

//        public void DeleteMe()
//        {
//            compass.DeleteMe();
//            compassNsymbol.DeleteMe();
//            compassPlayerDir.DeleteMe();
//            clearIcons();
//        }

//        public void UpdateCompassPosition()
//        {
//            CompassRadius = ScreenToRadiusRatio * player.ScreenArea.Height;
//            CompassIconPos = CompassRadius * IconRadius;
//            compass.Size = Vector2.One * CompassRadius * PublicConstants.Twice;
//            compass.Position =  player.SafeScreenArea.BottomRight - Vector2.One * CompassRadius * 1.5f;
//            compassPlayerDir.Position = compass.Position;

//            compassPlayerDir.Size = compass.Size * 0.2f;
//        }

//        public void Update(PlayState gamestate)
//        {
//            //return; //TEMP

//            compassRot.Radians = MathHelper.TwoPi - (player.ControllerLink.view.Camera.TiltX - MathHelper.PiOver2);
            
//            compass.Rotation = compassRot.Radians;
//            compassNsymbol.Position = compass.Position + compassRot.Direction(CompassIconPos);
//            compassPlayerDir.Rotation = player.AbsHero.FireDir.Radians + compassRot.Radians;

//            if (Ref.update.LasyUpdatePart == Engine.LasyUpdatePart.Part2)
//            {//find compass icons
//                //foreach (var ic in compassIcons)
//                //{
//                //    ic.removeFlag = true;
//                //}
//                if (lib.VectorSideLength(player.AbsHero.PlanePos - previousCompassLocation) >= 32)
//                {
//                    previousCompassLocation = player.AbsHero.PlanePos;

//                    clearIcons();

//                    //find nearby craftsmen
//                    ISpottedArrayCounter<GO.AbsUpdateObj> gameObjects = gamestate.GameObjCollection.AllMembersUpdateCounter;

//                    while (GO.Next())
//                    {
//                        if (GO.GetMember is GO.NPC.AbsNPC)
//                        {
//                            GO.NPC.AbsNPC npc = GO.GetMember as GO.NPC.AbsNPC;
//                            if (npc.CompassIcon != SpriteName.NO_IMAGE)
//                            {
//                                if (player.AbsHero.distanceToObject(npc) <= CompassIconCraftsMan.MaxDistance)
//                                {
//                                    compassIcons.Add(new CompassIconCraftsMan(npc, compassIcons.Count));
//                                }
//                            }
//                        }
//                    }

//                    //other gamers
//                    List<Players.AbsPlayer> allPlayers = gamestate.AllPlayers();
//                    for (int i = 0; i < allPlayers.Count; i++)
//                    {
//                        if (allPlayers[i] != player)
//                            compassIcons.Add(new CompassIconGamer(allPlayers[i], compassIcons.Count));
//                    }

//                    //map locations
//                    //foreach (IMiniMapLocation l in MiniMapData.Locations)
//                    //{
//                    //    if (l.VisibleOnMiniMap)
//                    //    {
//                    //        compassIcons.Add(new CompassIconMapLocation(l, compassIcons.Count));
//                    //    }
//                    //}
//                }
//            }

//            foreach (AbsCompassIcon icon in compassIcons)
//            {
//                icon.Update(player, CompassRadius, compassRot, compass.Position);
//            }
//        }

//        void clearIcons()
//        {
//            for (int i = compassIcons.Count - 1; i > 2; --i)
//            {
//                compassIcons[i].DeleteMe();
//                compassIcons.RemoveAt(i);
//            }
//        }

//        public Vector2 CompassQuestMarkPos { get { return questIcon.Position; } }
//    }

    
//}
