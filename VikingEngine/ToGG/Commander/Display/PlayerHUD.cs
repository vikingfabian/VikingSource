using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.Commander.Display
{
    /// <summary>
    /// A container for the local player hud elements
    /// </summary>
    class PlayerHUD : AbsPlayerHUD
    {
        public VectorRect viewArea;

        Graphics.TextG inputPromptText;
        Graphics.Image inputPromptButton;
        //public Graphics.ImageGroupParent2D tileInfoCard = null;
       
        public HUD.DialogueBox warningMessage = null;
        //public ButtonsOverview buttonsOverview;

        //public ToggleImageButtton spyglass;
        public AbsPhaseButton nextPhaseButton, previousPhaseButton;
        MenuButton menubutton;
        NextPhaseWarningPopup2 nextPhaseWarningPopup = null;
        PhaseOrderSpheres phaseOrderSpheres = null;
        public QuestBanner questBanner;
        public Graphics.Image spyglassTutorialArrow = null;
        LineOfSightUi lineOfSightUi;

        public PlayerHUD()
            :base()
        {
            viewArea = Engine.Screen.SafeArea;
            viewArea.Height -= HudLib.cardSize.Y;

            inputPromptText = new Graphics.TextG(LoadedFont.Regular, viewArea.Center, new Vector2(Engine.Screen.TextSize), Graphics.Align.CenterAll,
                "XXXXXXXXXXXXX", Color.White, ImageLayers.Foreground7);
            inputPromptButton = new Graphics.Image(SpriteName.ButtonSTART, inputPromptText.Position, new Vector2(Engine.Screen.IconSize), ImageLayers.Foreground7, true);
            
            inputPromptButton.Visible = false;
            inputPromptText.Visible = false;

            VectorRect menubuttonArea = new VectorRect(Engine.Screen.SafeArea.RightTop, Engine.Screen.IconSizeV2);
            menubuttonArea.X -= menubuttonArea.Width;
            menubutton = new MenuButton(menubuttonArea);
            float nextBannerR = menubutton.area.X - Engine.Screen.IconSize * 2f;
            questBanner = new QuestBanner(nextBannerR);

            menubuttonArea.nextAreaY(1, Engine.Screen.BorderWidth);
            lineOfSightUi = new LineOfSightUi(menubuttonArea);
            //createSpyGlass();

            //buttonsOverview = new ButtonsOverview(viewArea);
            VectorRect nextButtonArea = new VectorRect(Engine.Screen.SafeArea.RightBottom, HudLib.NextPhaseButtonsSz);
            nextButtonArea.Position -= nextButtonArea.Size;

            nextPhaseButton = new NextPhaseButton(nextButtonArea);
            //nextPhaseButton.buttonMap = toggRef.inputmap.nextPhase;

            VectorRect prevButtonArea = new VectorRect(nextButtonArea.LeftBottom, HudLib.PrevPhaseButtonsSz);
            prevButtonArea.Position -= prevButtonArea.Size;
            prevButtonArea.X -= Engine.Screen.BorderWidth;

            previousPhaseButton = new PrevPhaseButton(prevButtonArea);
            //previousPhaseButton.baseImage.SetSpriteName(SpriteName.cmdIconButtonBack);
            //previousPhaseButton.buttonMap = toggRef.inputmap.prevPhase;

            setNextPhaseStatus(EnableType.Disabled, false, null);
            setPrevPhaseStatus(false);

            //infoToggler = new InfoToggler(viewArea.Bottom - Engine.Screen.IconSize);
        }

        public void Update(ref PhaseUpdateArgs args)
        {
            if (menubutton.update())
            {
                toggRef.menu.MainMenu();
            }

            //if (spyglass.update())
            //{
            //    spyglass.Visible = false;
            //    var player = Commander.cmdRef.players.ActiveLocalPlayer();
            //    new SpyglassGamePhase(player);
            //}

            if (nextPhaseWarningPopup != null)
            {
                updateNextPhaseWarningPopup();
            }
            else
            {
                updatePhaseButtons(ref args);
                if (args.exitUpdate)
                {
                    return;
                }
            }

            //infoToggler.update();
            questBanner.update();
            lineOfSightUi.update();
            args.mouseOverHud |= questBanner.mouseOver;

            if (phaseOrderSpheres != null)
            {
                phaseOrderSpheres.Update(ref args);
            }
        }

        void updateNextPhaseWarningPopup()
        {
            bool remove = false;

            if (toggRef.inputmap.nextPhase.DownEvent)
            {
                remove = true;
                Commander.cmdRef.players.allPlayers.Selected().nextPhaseButtonAction(true);
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

        void updatePhaseButtons(ref PhaseUpdateArgs args)
        {
            if (nextPhaseButton.update())
            {
                EnableType exitEnabled = Commander.cmdRef.players.ActiveLocalPlayer().gamePhase.canExitPhase();

                if (nextPhaseButton.clickSource == Input.InputSourceType.Keyboard &&
                    exitEnabled == EnableType.Able_NotRecommended)
                {
                    nextPhaseWarningPopup = new NextPhaseWarningPopup2();//nextPhaseButton.toolwarning);
                }
                else
                {
                    Commander.cmdRef.players.allPlayers.Selected().nextPhaseButtonAction(true);
                    args.exitUpdate = true;
                }
            }
            if (previousPhaseButton.update())
            {
                Commander.cmdRef.players.allPlayers.Selected().nextPhaseButtonAction(false);
                args.exitUpdate = true;
            }
        }

        public void setNextPhaseStatus(EnableType canExit, bool nextPhaseIsTurnover, Commander.Players.LocalPlayer player)
        {
            if (nextPhaseButton.Visible)
            {
                nextPhaseButton.Enabled = canExit == EnableType.Able_NotRecommended || canExit == EnableType.Enabled;
            }
            //if (canExit == EnableType.Locked)
            //    return;

            //nextPhaseButton.tooltip = null;
            //nextPhaseButton.toolwarning = null;
            //bool nextButtonLargeSz;

            //if (canExit == EnableType.Disabled)
            //{
            //    nextPhaseButton.Enabled = false;

            //    if (nextPhaseIsTurnover)
            //    {
            //        nextPhaseButton.baseImage.SetSpriteName(SpriteName.cmdIconButtonNextPlayerGrayed);
            //    }
            //    else
            //    {
            //        nextPhaseButton.baseImage.SetSpriteName(SpriteName.cmdIconButtonNextPhaseGrayed);
            //    }
            //    nextButtonLargeSz = false;
            //}
            //else
            //{
            //    nextPhaseButton.Enabled = true;

            //    if (nextPhaseIsTurnover)
            //    {
            //        nextPhaseButton.tooltip = "End turn";
            //        nextPhaseButton.baseImage.SetSpriteName(SpriteName.cmdIconButtonNextPlayer);
            //    }
            //    else
            //    {
            //        bool non;
            //        GamePhaseType nextPhase = player.getNextPhase(player.GamePhase, out non);
            //        nextPhaseButton.tooltip =  "Begin " + AbsGamePhase.PhaseNameShort(nextPhase) + " phase";

            //        nextPhaseButton.baseImage.SetSpriteName(SpriteName.cmdIconButtonNextPhase);
            //    }

            //    if (canExit == EnableType.Able_NotRecommended)
            //    {
            //        string title, message, okText;
            //        player.gamePhase.EndTurnNotRecommendedText(out title, out message, out okText);
            //        nextPhaseButton.toolwarning = message;
                    
            //        nextButtonLargeSz = false;
            //    }
            //    else
            //    {
            //        nextButtonLargeSz = true;
            //    }
            //}

            //nextPhaseButton.baseImage.Size = nextButtonLargeSz? HudLib.NextPhaseButtonsSz : HudLib.PrevPhaseButtonsSz;
            //nextPhaseButton.refresh();
            //nextPhaseButton.viewPressNextKeyTip(nextButtonLargeSz);

            //float leftPos;
            //if (phaseOrderSpheres == null)
            //{
            //    leftPos = nextPhaseButton.area.X - HudLib.PhaseButtonsSpacing;
            //}
            //else
            //{
            //    leftPos = phaseOrderSpheres.GetPosition().X - HudLib.PhaseButtonsSpacing;
            //}
            //previousPhaseButton.baseImage.Xpos = leftPos - HudLib.PrevPhaseButtonsSz.X;
            //previousPhaseButton.refresh();
        }

        public void setPrevPhaseStatus(bool canGoToPrePhase)
        {
            previousPhaseButton.Visible = canGoToPrePhase;
        }

        public void viewInputPrompt(string text, SpriteName icon)
        {
            inputPromptText.TextString = text;
            inputPromptButton.SetSpriteName(icon);
            inputPromptButton.Xpos = inputPromptText.Xpos - (inputPromptText.MeasureText().X  + inputPromptButton.Width) * 0.5f ;

            setInputPromptVisible(true);
        }

        public void setCurrentPhase(GamePhaseType type)
        {
            if (phaseOrderSpheres != null)
            {
                phaseOrderSpheres.setPhase(type);
            }
        }

        public void hideInputPrompt()
        {
            setInputPromptVisible(false);
        }

        GamePhaseType onOpenMenuPhase;
        bool onOpenMenuHadPrompt;

        public void onOpenMenu(bool open, GamePhaseType phase)
        {
            if (open)
            {
                this.onOpenMenuHadPrompt = inputPromptText.Visible;
                this.onOpenMenuPhase = phase;

                hideInputPrompt();
            }
            else
            {
                if (onOpenMenuHadPrompt && phase == onOpenMenuPhase)
                {
                    setInputPromptVisible(true);
                }
            }
        }

        public void setInputPromptVisible(bool visible)
        {
            inputPromptText.Visible = visible;
            inputPromptButton.Visible = visible;
        }

        public bool OverridingUpdate()
        {
            if (warningMessage != null)
            {
                if (warningMessage.Update() != DialogueResult.NoResult)
                {
                    warningMessage.DeleteMe();
                    warningMessage = null;
                }
                return true;
            }
            return false;
        }

        //public void RemoveTileInfoCard()
        //{
        //    if (tileInfoCard != null)
        //    {
        //        tileInfoCard.DeleteMe();
        //        tileInfoCard = null;
        //    }
        //}

        //public void createSpyGlass()
        //{
        //    Vector2 spyGlassSz = new Vector2(Engine.Screen.IconSize * 1.2f);
        //    VectorRect area = new VectorRect(
        //        viewArea.LeftBottom + VectorExt.SetY(HudLib.cardSize, 0) + spyGlassSz * new Vector2(-0.4f, 1f),
        //        spyGlassSz);

        //    spyglass = new ToggleImageButtton(toggLib.ButtonIcon_MoreInfo, SpriteName.WhiteArea, HudLib.ContentLayer - 1, area);
           
        //    spyglass.inputIcon = new Graphics.Image(toggRef.inputmap.moreInfo.Icon,
        //        spyglass.area.RightBottom + new Vector2(-Engine.Screen.IconSize * 0.2f, - Engine.Screen.IconSize), 
        //        Engine.Screen.IconSizeV2, ImageLayers.AbsoluteTopLayer, false);
        //    spyglass.inputIcon.LayerBelow(spyglass.baseImage);
            
        //    if (toggRef.gamestate.gameSetup.level == LevelEnum.UseSpyglass &&
        //        spyglassTutorialArrow == null)
        //    {
        //        Rotation1D arrowdir = Rotation1D.FromDegrees(240);

        //        Vector2 arrowSz = Engine.Screen.IconSizeV2 * 2f;
        //        spyglassTutorialArrow = new Graphics.Image(SpriteName.cmdIconFollowUp,
        //            spyglass.area.Position + arrowSz * new Vector2(1.0f, -0.6f),
        //            arrowSz, ImageLayers.AbsoluteTopLayer, true);
        //        spyglassTutorialArrow.LayerAbove(spyglass.baseImage);
        //        spyglassTutorialArrow.Rotation = arrowdir.Radians;

        //        new Graphics.Motion2d(Graphics.MotionType.MOVE,
        //            spyglassTutorialArrow, arrowdir.Direction(spyglassTutorialArrow.Size1D * 0.3f), Graphics.MotionRepeate.BackNForwardLoop, 400, true);
        //    }
        //}

        //public void removeHoverUnitMoveAndAttackDots()
        //{
        //    if (hoverUnitMoveAndAttackDots != null)
        //    {
        //        hoverUnitMoveAndAttackDots.DeleteImages();
        //        hoverUnitMoveAndAttackDots = null;
        //    }
        //}

        public void clearAll()
        {
            hideInputPrompt();
            //RemoveTileInfoCard();
            removeInfoCardDisplay();
            if (phaseOrderSpheres != null)
            {
                phaseOrderSpheres.DeleteMe(); phaseOrderSpheres = null;
            }
            //removeHoverUnitMoveAndAttackDots();
            //buttonsOverview.DeleteAll();

            hidePhaseButtons();
            //infoToggler.Visible = false;
            //spyglass.Visible = false;
            
        }

        public void hidePhaseButtons()
        {
            nextPhaseButton.Visible = false; previousPhaseButton.Visible = false;
        }

        public void viewLocalPlayerHud()
        {
            nextPhaseButton.Visible = true;
            previousPhaseButton.Visible = true;
            //infoToggler.Visible = true;
            //spyglass.Visible = true;

            if (phaseOrderSpheres == null)
            {
                phaseOrderSpheres = new PhaseOrderSpheres();
            }
        }
    }
}
