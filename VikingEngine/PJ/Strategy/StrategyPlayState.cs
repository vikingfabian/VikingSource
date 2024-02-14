using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Strategy
{
    abstract class AbsStrategyState : AbsPJGameState
    {
        public AbsStrategyState(bool editor)
            :base(!editor)
        {
            StrategyRef.inEditor = editor;
        }

        virtual public void mapLoadingComplete()
        {  }

        protected override void createDrawManager()
        {
            draw = new VikingEngine.PJ.Strategy.Draw();
        }
    }

    class StrategyPlayState : AbsStrategyState
    {
        ListWithSelection<Gamer> gamers;
        GamePhase phase = GamePhase.EndPlayer;
        Time phaseTimer = 0;

        ListWithSelection<MapArea> playerAreas;
        int currentDirection;
        Graphics.Image selectedAreaMarker;
        Graphics.Image[] moveArmyDirections;
        MoveArrow moveArrow;
        
        Battle battle = null;

        GamerDisplay display;
        bool activatedTileEvent = false;
        BuyUnitsDisplay buyUnitsDisplay = null;
        ShippingDisplay shippingDisplay = null;
        UpgradeDisplay upgradeDisplay = null;

        public StrategyPlayState(List<GamerData> joinedGamers)
            : base(false)
        {
            draw.ClrColor = Color.Black;
            new Map();

            gamers = new ListWithSelection<Gamer>(joinedGamers.Count);
            foreach (var m in joinedGamers)
            {
                gamers.Add(new Gamer(m), false);
            }
            gamers.SelectRandom();

            selectedAreaMarker = new Graphics.Image( SpriteName.ClickCirkleEffect, Vector2.Zero,
                new Vector2(Engine.Screen.IconSize * 1.8f), ImageLayers.Foreground7, true);
            

            StrategyRef.map.cameraPos = Vector2.Zero;

            moveArrow = new MoveArrow();

            const int MaxMoveDirs = 6;
            moveArmyDirections = new Graphics.Image[MaxMoveDirs];
            for (int i = 0; i < MaxMoveDirs; ++i)
            {

                moveArmyDirections[i] = moveArrow.createAvailableDirection(i == 0);
            }

            display = new GamerDisplay();
        }

        public override void mapLoadingComplete()
        {
            base.mapLoadingComplete();
            playerSetup();
            mapSetup();
        }

        void playerSetup()
        {
            //List all available start areas
            List<MapArea>[] start_Areas_prio = new List<MapArea>[MapArea.MaxStartPrio + 1];

            for (int i = 0; i < start_Areas_prio.Length; ++i)
            {
                start_Areas_prio[i] = new List<MapArea>();
            }

            foreach (var m in StrategyRef.map.areas)
            {
                if (m.startAreaPrio >= 0)
                {
                    start_Areas_prio[m.startAreaPrio].Add(m);
                }
            }

            int includePrio = -1;
            List<MapArea> startAreas = new List<MapArea>();

            while (startAreas.Count < PjRef.storage.joinedGamersSetup.Count)
            {
                includePrio++;
                if (includePrio > MapArea.MaxStartPrio)
                {
                    throw new Exception("Map does not contain enough start areas");
                }

                startAreas.AddRange(start_Areas_prio[includePrio]);
            }


            //Hand out areas to players
            foreach (var m in gamers.list)
            {
                m.setStartArea(arraylib.RandomListMemberPop<MapArea>(startAreas), gamers.Count);
            }
        }

        void mapSetup()
        {
            //Go through all areas and set their type
            foreach (var area in StrategyRef.map.areas)
            {
                if (area.type == AreaType.Standard) //not set
                {
                    bool gotAdjVp = false;

                    foreach (var adj in area.adjacentAreas)
                    {
                        if (adj.toArea.type == AreaType.VictoryPoint)
                        {
                            gotAdjVp = true;
                        }
                    }

                    if (area.startAreaPrio >= 0 && Ref.rnd.Chance(0.2f))
                    {
                        area.type = AreaType.Castle;
                    }
                    else if (!gotAdjVp && Ref.rnd.Chance(0.2f))
                    {
                        area.type = AreaType.VictoryPoint;
                    }
                }

                area.createResourceIcons();
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (phase == GamePhase.AnnounceWinner)
            {
                if (phaseTimer.CountDown())
                {
                    new FinalScoreState();
                }
                return;
            }

            if (baseClassUpdate()) return;

            if (battle != null)
            {
                updateBattle();
            }
            else
            {
                if (phaseTimer.CountDown())
                {
                    if (moveArrow.moveAmount > 0)
                    {
                        beginMoveAndAttack();
                    }
                    else
                    {
                        nextPhase();
                    }
                }

                updateInput();
            }

            updateCamera();

            if (PlatformSettings.DevBuild && Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                new Editor();
            }
        }

        void updateCamera()
        {
            Vector2 goalCamPos =  -selectedAreaMarker.Position + Engine.Screen.CenterScreen;
            Vector2 diff = goalCamPos - StrategyRef.map.cameraPos;
            if (diff.Length() < 2f)
            {
                StrategyRef.map.cameraPos = goalCamPos;
            }
            else
            {
                StrategyRef.map.cameraPos += diff * 0.08f;
            }
            draw.Set2DTranslation(0, StrategyRef.map.cameraPos);
        }


        void updateBattle()
        {
            if (battle.update())
            {
                battle.fromArea.ArmyCount -= battle.moveAmount;
                battle.fromArea.refreshArmyVisuals();

                if (battle.attackerWon)
                {
                    battle.toArea.placeArmy(battle.fromArea.owner, battle.attackers.survivorCount);
                }
                else
                {
                    battle.toArea.ArmyCount = battle.defenders.survivorCount;
                }
                battle.toArea.refreshArmyVisuals();
                endDirectionsPhase();

                battle.DeleteMe();
                battle = null;
            }
        }

        void beginMoveAndAttack()
        {
            StrategyLib.SetMapLayer();
            moveArrow.removeAmountText();

            //Start move and attack phase
            MapArea fromArea = playerAreas.Selected();
            var connection = playerAreas.Selected().adjacentAreas[currentDirection];
            MapArea toArea = connection.toArea;

            if (connection.waterPassage)
            {
                fromArea.owner.Coins -= StrategyLib.ShippingCost;
                display.refreshGamer(gamers.Selected());
            }

            if (fromArea.owner == toArea.owner)
            {//Just move the army
                fromArea.ArmyCount -= moveArrow.moveAmount;
                toArea.ArmyCount += moveArrow.moveAmount;

                fromArea.refreshArmyVisuals();
                toArea.refreshArmyVisuals();

                endDirectionsPhase();
            }
            else
            {
                battle = new Battle(fromArea, toArea, moveArrow.moveAmount);
            }

            moveArrow.moveAmount = 0;
        }

        void removeAreaActionDisplay()
        {
            if (upgradeDisplay != null)
            {
                upgradeDisplay.DeleteMe();
                upgradeDisplay = null;
            }
            if (buyUnitsDisplay != null)
            {
                buyUnitsDisplay.DeleteMe();
                buyUnitsDisplay = null;
            }
            if (shippingDisplay != null)
            {
                shippingDisplay.DeleteMe();
                shippingDisplay = null;
            }
        }

        void nextPhase()
        {
            removeAreaActionDisplay();

            switch (phase)
            {
                case GamePhase.EndPlayer:
                    phase = GamePhase.NextPlayer;
                    break;
                case GamePhase.NextPlayer:
                    gamers.Next_IsEnd(true);

                    Gamer gamer = gamers.Selected();
                    moveArrow.setGamer(gamer);   
                // moveArmyButtonIcon.SetSpriteName(gamer.data.button.Icon);
                    display.refreshGamer(gamer);
                    playerAreas = new ListWithSelection<MapArea>(new List<MapArea>(gamer.areas));
                    playerAreas.selectedIndex = -1;
                    phase++;
                    break;
                case GamePhase.NextArmy:
                    while (true)
                    {
                        if (playerAreas.Next_IsEnd(false))
                        {
                            //Out of armies
                            phase = GamePhase.EndPlayer;
                            return;
                        }
                        else
                        {
                            var area = playerAreas.Selected();
                            area.gainResource(display, announceWinner);
                            
                            //Mark army
                            var action = area.canAffordAreaAction();
                            switch(action)
                            {
                                case AreaActionType.BuySoldier: buyUnitsDisplay = new BuyUnitsDisplay(gamers.Selected()); break;
                                case AreaActionType.UpgradeIncome: upgradeDisplay = new UpgradeDisplay(true); break;
                                case AreaActionType.UpgradeVp: upgradeDisplay = new UpgradeDisplay(false); break;
                            }


                            bool canMove = area.ArmyCount > 1;

                            if (canMove || action != AreaActionType.NoAction)
                            {
                                selectedAreaMarker.Position = playerAreas.Selected().center;
                                phaseTimer = StrategyLib.nextUnitTime;
                                if (playerAreas.selectedIndex == 0)
                                {
                                    phaseTimer.Seconds += 1f;
                                }
                                phase = GamePhase.Directions;
                                currentDirection = -1;

                                if (canMove)
                                {
                                    var connections = playerAreas.Selected().adjacentAreas;
                                    for (int i = 0; i < connections.Count; ++i)
                                    {
                                        moveArmyDirections[i].Position = connections[i].arrowPos;
                                        moveArmyDirections[i].Rotation = connections[i].arrowRotation.Radians;
                                        moveArmyDirections[i].Visible = true;
                                    }
                                }
                                return;
                            }
                        }
                    }

                case GamePhase.Directions:
                    if (activatedTileEvent)
                    {
                        activatedTileEvent = false;
                        endDirectionsPhase();
                        return;
                    }

                    currentDirection++;
                    if (playerAreas.Selected().ArmyCount > 1 &&
                        currentDirection < playerAreas.Selected().adjacentAreas.Count)
                    {
                        moveArrow.moveAmount = 0;
                        AreaConnection connection = playerAreas.Selected().adjacentAreas[currentDirection];

                        moveArrow.setConnection(connection);
                        moveArmyDirections[currentDirection].Visible = false;

                        if (connection.waterPassage)
                        {
                            shippingDisplay = new ShippingDisplay();
                        }

                        phaseTimer = StrategyLib.nextDirTime;
                       
                    }
                    else
                    {
                        //Has viewed all directions
                        endDirectionsPhase();
                    }
                    break;

                }
        }



        void announceWinner()
        {
            phase = GamePhase.AnnounceWinner;
            phaseTimer.Seconds = 2f;

            foreach (var m in gamers.list)
            {
                m.data.coins = m.Coins;
                m.data.Victories = m.VictoryPoints;
            }

            StrategyLib.SetMapLayer();
            Graphics.Image winnerIcon = new Graphics.Image(SpriteName.BirdThrophy, playerAreas.Selected().center, 
                new Vector2(Engine.Screen.IconSize * 2f), ImageLayers.AbsoluteTopLayer, true);

            const float FadeTime = 200;
            new Graphics.Motion2d(Graphics.MotionType.MOVE, winnerIcon, new Vector2(0, -Joust.Gamer.ImageScale * 0.8f),
                Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
            new Graphics.Motion2d(Graphics.MotionType.OPACITY, winnerIcon, Vector2.One,
                Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
            
        }

        void endDirectionsPhase()
        {
            phase = GamePhase.NextArmy;
            moveArrow.hide();
        }

        void updateInput()
        {
            switch (phase)
            {
                case GamePhase.Directions:
                    if (currentDirection < 0)
                    {
                        if (gamers.Selected().data.button.DownEvent && 
                            playerAreas.Selected().hasAreaAction() != AreaActionType.NoAction)
                        {
                            if (gamers.Selected().buy(playerAreas.Selected().actionCost(), display))
                            {
                                phaseTimer = StrategyLib.nextUnitTime;
                                activatedTileEvent = true;

                                switch (playerAreas.Selected().hasAreaAction())
                                {
                                    case AreaActionType.BuySoldier:
                                        {
                                                //BUY
                                                playerAreas.Selected().ArmyCount++;
                                                playerAreas.Selected().refreshArmyVisuals();
                                        }
                                        break;
                                    case AreaActionType.UpgradeIncome:
                                        {
                                                //BUY UPGRADE
                                                playerAreas.Selected().areaLevel = 2;
                                                playerAreas.Selected().createResourceIcons();
                                            
                                        }
                                        break;
                                    case AreaActionType.UpgradeVp:
                                        {
                                                //BUY UPGRADE
                                                playerAreas.Selected().areaLevel = 2;
                                                playerAreas.Selected().createResourceIcons();
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        moveArrow.moveInput(ref phaseTimer);
                    }

                    break;
            }
        }

        protected override void setMenuLayer()
        {
            StrategyLib.SetHudLayer();
        }

        protected override bool hasMusic
        {
            get
            {
                return false;
            }
        }

        enum GamePhase
        {
            NextPlayer,
            NextArmy,
            Directions,
            EndPlayer,
            AnnounceWinner,
            NUM
        }
    }
}
