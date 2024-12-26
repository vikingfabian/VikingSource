using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest
{
    //HERO QUEST
    class PlayerHUD : AbsPlayerHUD
    {
        public PrevPhaseButton prevPhaseButton; 
        public NextPhaseButton nextPhaseButton;
        MenuButton menubutton;
        LineOfSightUi lineOfSightUi;
        CommunicationsUi communicationsUi;

        NextPhaseWarningPopup2 nextPhaseWarningPopup = null;
        EnableType exitEnabled;

        RemoteEndTurnButton[] remoteEndTurnButtons;
        
        public ToDoList todoList = null;

        public Display.DoomTrack doombar = null;
        DoomBanner doomBanner;
        public QuestBanner questBanner;

        public BackpackButton backpackButton;
        public List<Display.AbsActionButton> actionButtons = new List<AbsActionButton>(4);
        public bool actionButtonsChanged = false;
        public VectorRect actionButtonsAreaStart;
        

        Graphics.Text2 coordinates = null;
        public HistoryDisplay historyDisplay = null;

        public PlayerHUD()
            :base()
        {            
            VectorRect menubuttonArea = new VectorRect(Engine.Screen.SafeArea.RightTop, Engine.Screen.IconSizeV2);
            {
                menubuttonArea.X -= menubuttonArea.Width;
                menubutton = new MenuButton(menubuttonArea);

                float nextBannerR = menubutton.area.X - Engine.Screen.IconSize * 2f;

                questBanner = new QuestBanner(nextBannerR);
                nextBannerR = questBanner.area.X - Engine.Screen.IconSize * 0.5f;

                if (hqRef.setup.conditions.HasDungeonMaster)
                {
                    doomBanner = new DoomBanner(nextBannerR);
                }

                menubuttonArea.nextAreaY(1, Engine.Screen.BorderWidth);
                lineOfSightUi = new LineOfSightUi(menubuttonArea);

                //menubuttonArea.nextAreaY(1, Engine.Screen.BorderWidth);
                
                //extendedTooltip.SpyglassUi = new SpyglassUi(menubuttonArea);

                menubuttonArea.nextAreaY(1, Engine.Screen.BorderWidth);
                communicationsUi = new CommunicationsUi(menubuttonArea);
            }

            VectorRect nextButtonArea = new VectorRect(Engine.Screen.SafeArea.RightBottom, HudLib.NextPhaseButtonsSz);
            nextButtonArea.Position -= nextButtonArea.Size;
            nextPhaseButton = new NextPhaseButton(nextButtonArea, hqRef.players.localHost);
            //nextPhaseButton.buttonMap = toggRef.inputmap.nextPhase;
            //nextPhaseButton.tooltip = "End turn";

            VectorRect prevButtonArea = new VectorRect(nextButtonArea.LeftBottom, HudLib.PrevPhaseButtonsSz);
            prevButtonArea.Position -= prevButtonArea.Size;
            prevButtonArea.X -= Engine.Screen.BorderWidth;
            prevPhaseButton = new PrevPhaseButton(prevButtonArea);

            int remoteCount = Ref.netSession.RemoteGamersCount;
            //remoteCount = 2;
            remoteEndTurnButtons = new RemoteEndTurnButton[remoteCount];
            float right = prevPhaseButton.area.X;
            for (int i = 0; i < remoteCount; ++i)
            {
                remoteEndTurnButtons[i] = new RemoteEndTurnButton(ref right);
            }

            createBackpackbutton();

            if (toggLib.ViewDebugInfo)
            {
                coordinates = new Graphics.Text2("X, Y", LoadedFont.Console,
                    Engine.Screen.SafeArea.LeftBottom, Engine.Screen.TextBreadHeight,
                    Color.White, ImageLayers.Background0);

                historyDisplay = new HistoryDisplay();
            }
        }

        void createBackpackbutton()
        {
            var quickbelt = hqRef.players.localHost.heroInstances.First.heroUnit.data.hero.equipment.quickbelt;
            VectorRect backpackArea = new VectorRect(quickbelt.hudArea.Position, Gadgets.QuickBelt.SquareSize());
            backpackArea.nextAreaX(-1, Engine.Screen.BorderWidth * 2f);
            backpackButton = new BackpackButton(backpackArea);

            actionButtonsAreaStart = backpackArea;
            actionButtonsAreaStart.X = quickbelt.hudArea.Right + Engine.Screen.BorderWidth * 2f;
        }

        public void viewGamePlayHud(PlayerDisplay display, LocalPlayer player)
        {
            bool viewPhaseButtons = display.hudUpdate &&
                display.playerState != PlayerState.SelectStrategy;

            if (nextPhaseButton.Visible != viewPhaseButtons)
            {
                refreshPhaseHud(player, viewPhaseButtons);
            }
            
            todoList?.setVisible(display.hudUpdate);
            menubutton.Visible = display.hudUpdate || display.playerState == PlayerState.Menu;
            lineOfSightUi.Visible = display.hudUpdate;
            questBanner.Visible = display.hudUpdate;
            if (doomBanner != null)
            {
                doomBanner.Visible = display.hudUpdate;
            }
            communicationsUi.Visible = display.hudUpdate;
            //extendedTooltip.SpyglassUi.Visible = display.hudUpdate;

            foreach (var m in actionButtons)
            {
                m.Visible = display.hudUpdate;
            }
        }

        public void update(ref PlayerDisplay display, LocalPlayer player, bool bGamePlayHud)
        {
            display.hudUpdate = true;
            
            viewGamePlayHud(display, player);

            if (bGamePlayHud)
            {
                display.attackTerrain = player.canAttack() ? EnableType.Enabled : EnableType.Disabled;
            }

            if (nextPhaseWarningPopup != null)
            {
                updateNextPhaseWarningPopup();
            }
            else
            {
                if (bGamePlayHud)
                {
                    if (nextPhaseButton.update())
                    {
                        if (nextPhaseButton.clickSource == Input.InputSourceType.Keyboard &&
                            exitEnabled == EnableType.Able_NotRecommended)
                        {
                            nextPhaseWarningPopup = new NextPhaseWarningPopup2();

                            Graphics.ImageGroup warningTriangels = new Graphics.ImageGroup();
                            todoList.endTurnWarning(warningTriangels);

                            foreach (var img in warningTriangels.images)
                            {
                                var flash = new Graphics.VisualFlash(img, 2, 200);
                                //new Timer.Terminator(flash.TotalTime, img);
                            }

                            nextPhaseWarningPopup.images.Add(warningTriangels);
                        }
                        else
                        {
                            nextPhaseClick(player);
                        }
                    }

                    if (prevPhaseButton.update())
                    {
                        player.returnToStartegySelect();
                    }

                    display.mouseOverHud |= nextPhaseButton.mouseOver;
                }
            }

            if (menubutton.update())
            {
                toggRef.menu.MainMenu();
            }
            lineOfSightUi.update();
            
            if (communicationsUi.update())
            {
                if (player.EndPhase(Players.Phase.PhaseType.Communications) == false)
                {
                    new Players.Phase.CommunicationPalette(player);
                }
            }

            questBanner.update();

            if (doomBanner != null)
            {
                doomBanner.update();
                display.mouseOverHud |= doomBanner.mouseOver;
            }

            player.Backpack()?.equipment.quickbelt.update(player, ref display);
            if (backpackButton.update())
            {
                if (player.EndPhase(Players.Phase.PhaseType.Backpack)== false)
                {
                    new Players.Phase.Backpack(player);
                }
            }

            actionButtonsChanged = false;
            foreach (var m in actionButtons)
            {
                m.update();
                display.mouseOverHud |= m.mouseOver;

                if (actionButtonsChanged)
                {
                    break;
                }
            }

            display.mouseOverHud |= 
                menubutton.mouseOver || 
                questBanner.mouseOver || 
                backpackButton.mouseOver;
        }

        public void infoUpdate(LocalPlayer player)
        {
            if (toggLib.ViewDebugInfo)
            {
                if (player.mapControls.isOnNewTile)
                {
                    coordinates.TextString = player.mapControls.selectionIntV2.ToXYString();
                }

                historyDisplay.update();
            }
        }

        public void menuUpdate()
        {
            if (menubutton.update())
            {
                toggRef.menu.CloseMenu();
            }
        }

        public void refreshTodo(LocalPlayer player)
        {
            if (todoList == null)
            {
                todoList = new ToDoList(player);
            }
            todoList.refresh(nextPhaseButton.area.Y);
            nextPhaseButton.tooltipFromArea = todoList.strategyCardBg.Area;
            nextPhaseButton.createTooltipAction = todoList.endTurnWarning;
        }

        public void createDoomBar()
        {
            if (doombar == null)
            {
                doombar = new DoomTrack();
            }
        }

        public void removeDoomBar()
        {
            if (doombar != null)
            {
                doombar.DeleteMe();
                doombar = null;
            }
        }

        public void removeToDo()
        {
            if (todoList != null)
            {
                todoList.DeleteMe();
                todoList = null;
            }
        }

        

        public void nextPhaseClick(LocalPlayer player)
        {
            if (player.heroInstances.AllAreActivated())
            {
                player.toggleReadyEndTurn();

                refreshEndTurnButton(player, false);

                hqRef.netManager.requestEndTurn();
            }
            else if (player.heroInstances.EndPhase_AutoSelect())
            {
                player.onPickHero();
            }
            else
            {
                new Players.Phase.PickHero(player);
            }
        }

        public void refreshEndTurnButton(LocalPlayer player, bool turnStart)
        {
            if (turnStart)
            {
                player.readyToEndTurn_Request = false;
                player.readyToEndTurn_Confirmed = false;
                nextPhaseButton.refreshIcon(false, player);
            }
            else
            {
                nextPhaseButton.refreshIcon(player.readyToEndTurn_Request, player);

                prevPhaseButton.Visible = !player.heroInstances.sel.lockedInStrategySelection && !player.readyToEndTurn_Request;
            }

            nextPhaseButton.EnableToolTip(!player.readyToEndTurn_Request);
        }

        public void refreshPhaseHud(LocalPlayer player, bool visible)
        {
            if (visible)
            {
                string message;
                exitEnabled = player.canExitPhase(out message);
                nextPhaseButton.toolwarning = message;

                bool recommendEndTurn = false;
                if (todoList != null)
                {
                    recommendEndTurn = todoList.allComplete();
                }

                nextPhaseButton.viewPulsatingEdge(recommendEndTurn);

                nextPhaseButton.Visible = true;
                nextPhaseButton.refresh();
                nextPhaseButton.viewPressNextKeyTip(recommendEndTurn);

                prevPhaseButton.Visible = !player.heroInstances.sel.lockedInStrategySelection;
            }
            else
            {
                nextPhaseButton.Visible = false;
                prevPhaseButton.Visible = false;
            }
        }

        void updateNextPhaseWarningPopup()
        {
            bool remove = false;

            if (toggRef.inputmap.nextPhase.DownEvent)
            {
                remove = true;
                toggRef.absPlayers.LocalHost().nextPhaseButtonAction(true);
            }
            if (nextPhaseWarningPopup.Update() ||
                Input.Mouse.ButtonDownEvent(MouseButton.Left) ||
                Input.Mouse.ButtonDownEvent(MouseButton.Right))
            {
                remove = true;
            }

            if (remove)
            {
                nextPhaseWarningPopup.DeleteMe(); nextPhaseWarningPopup = null;
            }
        }

        public void refreshRemoteEndTurn()
        {
            int ix = 0;
            var remotes = new SpottedArrayTypeCounter<AbsHQPlayer>(hqRef.players.allPlayers, typeof(RemotePlayer));
            while (remotes.Next())
            {
                if (remotes.sel.readyToEndTurn_Confirmed)
                {
                    remoteEndTurnButtons[ix].refresh(remotes.sel);
                    ix++;
                }
            }

            for (int i = ix; i < remoteEndTurnButtons.Length; ++i)
            {
                remoteEndTurnButtons[i].refresh(null);
            }
        }

        

        class RemoteEndTurnButton
        {
            Graphics.Image button;
            Graphics.ImageAdvanced gamerIcon;

            public RemoteEndTurnButton(ref float right)
            {
                float width = Engine.Screen.IconSize * 0.5f;
                right -= width + Engine.Screen.BorderWidth * 2f;

                Vector2 pos = new Vector2(right, Engine.Screen.SafeArea.Bottom - width);

                button = new Graphics.Image(SpriteName.cmdIconButtonReadyCheckGray, pos, new Vector2(width), ImageLayers.Background6);
                pos.Y -= width + Engine.Screen.BorderWidth;

                gamerIcon = new Graphics.ImageAdvanced(SpriteName.MissingImage, pos, new Vector2(width), ImageLayers.Background6, false);
                gamerIcon.Visible = false;
            }

            public void refresh(AbsHQPlayer p)
            {
                if (p == null)
                {
                    button.SetSpriteName(SpriteName.cmdIconButtonReadyCheckGray);
                    gamerIcon.Visible = false;
                }
                else
                {
                    button.SetSpriteName(SpriteName.cmdIconButtonReadyCheck);
                    gamerIcon.Visible = true;
                    new SteamWrapping.LoadGamerIcon(gamerIcon, p.pData);
                }
            }
        }
    }
}
