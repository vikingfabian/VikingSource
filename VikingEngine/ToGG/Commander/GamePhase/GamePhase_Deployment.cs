using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.Commander.GO;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG
{
    class GamePhase_Deployment : AbsGamePhase
    {
        static readonly HUD.ButtonDescriptionData PlaceUnitButtonDesc =  new HUD.ButtonDescriptionData("Place unit", SpriteName.ButtonA);

        static readonly List<HUD.ButtonDescriptionData> ButtonDesc_FreeSquare = new List<HUD.ButtonDescriptionData>
        {
            PlaceUnitButtonDesc,    
        };
        static readonly List<HUD.ButtonDescriptionData> ButtonDesc_OccupiedSquare = new List<HUD.ButtonDescriptionData>
        {
            PlaceUnitButtonDesc,
            new HUD.ButtonDescriptionData("Remove unit", SpriteName.ButtonB),
        };

        public GamePhase_Deployment(Commander.Players.AbsLocalPlayer player)
            : base(player)
        {
            if (player.LocalHumanPlayer)
            {
                IntVector2 startPos = IntVector2.Zero;
                startPos.X = toggRef.board.HalfSize.X;
                startPos.Y = player.pData.globalPlayerIndex == Commander.Players.LocalPlayer.PlayerNumberOne ? toggRef.board.Size.Y - 1 : 0;

                player.mapControls.setSelection(startPos);
                player.mapControls.setAvailable(MapSquareAvailableType.None);
                var deploymentTiles = player.listDeploymentTiles(false);
                player.mapControls.availableTiles = new AvailableTilesGUI(deploymentTiles, null);
            }
            autoDeploy();
        }

        //public GamePhase_Deployment(Commander.Players.AiPlayer aiplayer)
        //    :base(aiplayer)
        //{
        //    autoDeploy();
        //}

        //public override void updateButtonDesc()
        //{
 	      //  bool holdingUnit = settings.armySetup.Selected != null && settings.armySetup.Selected.LeftToPlace > 0;
        //    bool availableSquare = mapControls.selectedTile.playerPlacement == player.pData.globalPlayerIndex;
        //    bool freeSquare = mapControls.selectedTile.unit == null;

        //    if (availableSquare)
        //    {
        //        if (holdingUnit)
        //        {
        //            if (freeSquare)
        //            {
        //                Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_FreeSquare);
        //            }
        //            else
        //            {
        //                Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_OccupiedSquare);
        //            }
        //        }
        //        else
        //        {
        //            if (freeSquare)
        //            {
        //                Commander.cmdRef.hud.buttonsOverview.DeleteAll();
        //            }
        //            else
        //            {
        //                Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_OccupiedSquare);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Commander.cmdRef.hud.buttonsOverview.DeleteAll();
        //    }
        //}

        public override void Update(ref PhaseUpdateArgs args)
        {
            isNewState = false;

            if (!toggRef.gamestate.gameSetup.useArmyDeployment)
            {
                player.nextPhase(true);
                return;
            }

            player.mapControls.updateMapMovement(true);
            bool holdingUnit = player.settings.armySetup.Selected != null && player.settings.armySetup.Selected.LeftToPlace > 0;
            bool availableSquare = player.mapControls.selectedTile.playerPlacement == player.pData.globalPlayerIndex;
            if (!holdingUnit)
                availableSquare = availableSquare && player.myUnit();
            player.mapControls.setAvailable(availableSquare);

            if (toggRef.inputmap.click.DownEvent)//inputMap.DownEvent(Input.ButtonActionType.MenuClick))
            {
                //Place unit
                if (holdingUnit)
                {
                    if (availableSquare)
                        placeUnit(player.mapControls.selectionIntV2);

                }
                else if (player.settings.armySetup.Complete())
                {
                    removeUnit();
                }
                else
                {
                    armySetupMenu();
                }
            }
            else if (toggRef.inputmap.back.DownEvent)//inputMap.DownEvent(Input.ButtonActionType.MenuBack))
            {
                removeUnit();
            }

            if (holdingUnit)
            {
                if (toggRef.inputmap.menuInput.tabLeftUp.DownEvent)//inputMap.DownEvent(Input.ButtonActionType.MenuTabLeftUp))//controller.KeyDownEvent(Buttons.LeftShoulder))
                {
                    player.settings.armySetup.PreviousMember();
                    updatePointerUnitIcon();
                }
                else if (toggRef.inputmap.menuInput.tabRightDown.DownEvent)//inputMap.DownEvent(Input.ButtonActionType.MenuTabRightDown))
                {
                    player.settings.armySetup.NextMember();
                    updatePointerUnitIcon();
                }
            }
        }

        public override bool UpdateAi()
        {
            return true;
        }

        void updatePointerUnitIcon()
        {
            //if (settings.armySetup.Selected == null)
            //{

            //    //mapControls.pointerIcon.DeleteMe();
            //    return;
            //}

            //if (mapControls.pointerIcon.Empty)
            //{
            //    Graphics.Image unitIcon = new Graphics.Image(SpriteName.NO_IMAGE, mapControls.pointer.Size * new Vector2(0.2f, 0f), new Vector2(Engine.Screen.IconSize * 2f), ImageLayers.Bottom2);
            //    Graphics.TextG count = new Graphics.TextG(LoadedFont.PhoneText, unitIcon.RightCenter, new Vector2(Engine.Screen.TextSize * 1.2f), Graphics.Align.CenterHeight,
            //        null, Color.Blue, ImageLayers.Bottom1);
            //    count.Xpos -= unitIcon.Width * 0.2f;
            //    mapControls.pointerIcon.Add(unitIcon, count);
            //}

            //UnitData unitData = settings.armySetup.Selected.GetData();
            //mapControls.pointerIcon.GetImage(0).SetSpriteName(unitData.image);
            //mapControls.pointerIcon.GetTextG(1).TextString = settings.armySetup.Selected.LeftToPlace.ToString();
        }

        private void placeUnit(IntVector2 pos)
        {
            AbsUnit previousUnit = mapControls.SelectedUnit;
            var u = new Commander.GO.Unit(pos, (UnitType)settings.armySetup.Selected.type, absPlayer);
            settings.armySetup.OnPlaceUnit(previousUnit);
            updatePointerUnitIcon();
        }

        void autoPlaceUnit(IntVector2 pos, UnitType type)
        {
            var u = new Commander.GO.Unit(pos, type, absPlayer);
            settings.armySetup.OnPlaceUnit(null);
        }

        void removeUnit()
        {
            if (player.myUnit())
            {
                AbsUnit u = mapControls.SelectedUnit;
                mapControls.SelectedUnit = null;
                settings.armySetup.RemoveUnit(u);
                u.DeleteMe();

                updatePointerUnitIcon();
            }
        }

        void armySetupMenu()
        {
            //player.openCardMenu();
            //player.hud.cardMenu.NewPage(player.settings.armySetup.cardMenuFile());
        }

        public void OnMenuSelect(ArmySetupMember armyMember)
        {
            //settings.armySetup.Select(hud.cardMenu.selectedIndex);
            settings.armySetup.Selected = armyMember;

            selectPlacementTile();
            updatePointerUnitIcon();
        }
        void selectPlacementTile()
        {
            List<IntVector2> placementPositions = new List<IntVector2>(16);
            //ForXYLoop loop = cmdRef.board.BoardLoop();
            toggRef.board.tileGrid.LoopBegin();
            while (toggRef.board.tileGrid.LoopNext())
            {
                if (toggRef.board.tileGrid.LoopValueGet().playerPlacement == player.pData.globalPlayerIndex)
                {
                    placementPositions.Add(toggRef.board.tileGrid.LoopPosition);//loop.Position);
                }
            }

            mapControls.SetAvailableTiles(placementPositions);//availableTiles = new AvailableTiles(placementPositions);
        }

        public void autoDeploy()
        {
            var deploymentTiles = absPlayer.listDeploymentTiles(true);
            int backRow = toggRef.board.placementRowToYPosition(ToggEngine.Map.Board.PlacementHeight - 1, absPlayer.BottomPlayer);
            
            for (int unitType = 0; unitType < settings.armySetup.members.Count; ++unitType)
            {
                for (int i = 0; i < settings.armySetup.members[unitType].Count; ++i) //Number of units of this type
                {
                    settings.armySetup.selectedIndex = unitType;
                    UnitType utype = (UnitType)settings.armySetup.Selected.type;
                    if (utype == UnitType.TacticalBase)
                    {

                        int row = toggLib.BottomPlayer(absPlayer.pData.globalPlayerIndex) ? toggRef.board.MaxPos.Y : 0;
                        IntVector2 pos = IntVector2.Zero;
                        var cirkleCounter = new ListCirkleCounter<IntVector2>(deploymentTiles, Ref.rnd.Int(deploymentTiles.Count));                        
                        
                        do
                        {
                            pos = cirkleCounter.CurrentValue;
                        } while (pos.Y != row && cirkleCounter.Next());

                        //placeUnit(pos);
                        autoPlaceUnit(pos, utype);
                    }
                    else
                    {
                        IntVector2 placePos = IntVector2.Zero;
                        for (int loop = 0; loop < 100; ++loop)
                        {
                            int rndIndex = Ref.rnd.Int(deploymentTiles.Count);
                            placePos = deploymentTiles[rndIndex];
                            if (placePos.Y != backRow)
                            {
                                deploymentTiles.RemoveAt(rndIndex);
                                break;
                            }
                        }
                        autoPlaceUnit(placePos, utype);
                    }
                }
            }
        }

        public override EnableType canExitPhase()
        {
            if (settings.armySetup.Complete())
                return EnableType.Enabled;
            else if (player.unitsColl.units.Count > 0)
                return EnableType.Able_NotRecommended;
            else
                return EnableType.Disabled;
        }

        public override void OnExitPhase(bool forwardToNext)
        {
            if (forwardToNext)
            {
                absPlayer.NetShareUnitSetup();
            }
        }

        public override void EndTurnNotRecommendedText(out string title, out string message,out string okText)
        {
            title = EndTurnTitle;
            message = "You can place more units";
            okText = EndTurnOkText;
        }

        public override void OnCancelMenu()
        {
            player.settings.armySetup.Unselect();
        }

        //public override bool borderVisuals(out Color bgColor, out SpriteName iconTile, out Color iconColor)
        //{
        //    bgColor = Color.Black;
        //    iconTile = SpriteName.NO_IMAGE;
        //    iconColor = Color.Black;

        //    return false;
        //}

        protected override string name
        {
            get { return "Deploy units"; }
        }

        public override GamePhaseType Type
        {
            get { return GamePhaseType.Deployment; }
        }
    }
}
