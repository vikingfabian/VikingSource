using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.Commander.CommandCard;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG
{
    abstract class AbsGamePhase
    {
        //protected HUD.ButtonDescriptionData PrevpButton() { return new HUD.ButtonDescriptionData("Prev unit", inputMap.ButtonIcon(Input.ButtonActionType.MenuTabLeftUp)); }
        //protected HUD.ButtonDescriptionData NextButton() { return new HUD.ButtonDescriptionData("Next unit", inputMap.ButtonIcon(Input.ButtonActionType.MenuTabRightDown)); }
     
        public AbsLocalPlayer absPlayer;
        protected LocalPlayer player;
        protected AiPlayer aiPlayer;

        protected InputMap inputMap
        {
            get { return toggRef.inputmap; }
        }
        protected PlayerSettings settings;
        public MapControls mapControls;
        //protected Commander.Battle.AttackAnimation attackAnimation = null;
        
        public bool isNewState = false;
        protected TutVideo.AbsVideo tutVid = null;
        //protected FollowUp followUp = null;
        public bool autoSkippedPhase = false;

        public AbsGamePhase(AbsLocalPlayer player)//LocalPlayer player)
        {
            absPlayer = player;
            this.settings = player.settings;

            if (player.LocalHumanPlayer)
            {
                this.player = (LocalPlayer)player;
                //absPlayer = player;

                //this.settings = player.settings;
                this.mapControls = player.mapControls;
                //this.CommandCard = player.commandCard;

                if (Type == GamePhaseType.Move || Type == GamePhaseType.Attack)
                {
                    new PhaseText(name, Commander.cmdRef.hud.viewArea.Center, player.pData.PublicName(LoadedFont.Regular));
                }
                updatePhaseInfoText();
            }
            else
            {
                this.aiPlayer = (AiPlayer)player;               
            }
        }

        //public AbsGamePhase(AiPlayer aiplayer)
        //{
        //    this.aiPlayer = aiplayer;
        //    absPlayer = aiPlayer;
        //    this.settings = aiplayer.settings;
        //    //this.CommandCard = aiplayer.commandCard;
        //}

        abstract protected string name { get; }
        abstract public GamePhaseType Type { get; }

        protected void updatePhaseInfoText()
        {
            //cmdRef.gamestate.playersStatusHUD.PlayerPhase(PhaseInfoText);
            Commander.cmdRef.hud.setCurrentPhase(Type);
        }
        virtual protected string PhaseInfoText { get { return name; } }

        public override string ToString()
        {
            return "Game Phase - " + name;
        }

        /// <returns>Complete</returns>
        abstract public bool UpdateAi();

        abstract public void Update(ref PhaseUpdateArgs args);

        /// <summary>
        /// Update a animation/motion instead of regular update
        /// </summary>
        /// <returns>Does override</returns>
        virtual public bool OverridingUpdate(ref PhaseUpdateArgs args)
        {
            return false;
        }

        public static AbsGamePhase GetPhase(GamePhaseType type, AbsLocalPlayer player)
        {
            switch (type)
            {
                case GamePhaseType.SelectArmy:
                    return new GamePhase_SelectArmy(player);
                case GamePhaseType.Deployment:
                    return new GamePhase_Deployment(player);
                case GamePhaseType.SelectCommand:
                    return new GamePhase_SelectCommand(player);
                case GamePhaseType.GiveOrder:
                    return new GamePhase_GiveOrder(player);
                case GamePhaseType.Move:
                    return new GamePhase_Movement(player);
                case GamePhaseType.Attack:
                    return new GamePhase_Attack(player);
                default:
                    throw new NotImplementedException("GetPhase " + type.ToString()); 
            }

        }

        //public static AbsGamePhase GetPhase(GamePhaseType type, Commander.Players.AiPlayer aiplayer)
        //{
        //    switch (type)
        //    {
        //        case GamePhaseType.SelectArmy:
        //            return new GamePhase_SelectArmy(aiplayer);
        //        case GamePhaseType.Deployment:
        //            return new GamePhase_Deployment(aiplayer);
        //        case GamePhaseType.SelectCommand:
        //            return new GamePhase_SelectCommand(aiplayer);
        //        case GamePhaseType.GiveOrder:
        //            return new GamePhase_GiveOrder(aiplayer);
        //        case GamePhaseType.Move:
        //            return new GamePhase_Movement(aiplayer);
        //        case GamePhaseType.Attack:
        //            return new GamePhase_Attack(aiplayer);
        //        default:
        //            throw new NotImplementedException("GetPhase " + type.ToString());
        //    }

        //}
        public static string PhaseNameShort(GamePhaseType type)
        {
            switch (type)
            {
                case GamePhaseType.SelectArmy:
                    return GamePhase_SelectArmy.Name;
                case GamePhaseType.Deployment:
                    return "Deployment";
                case GamePhaseType.SelectCommand:
                    return "Strategy";
                case GamePhaseType.GiveOrder:
                    return "Order";
                case GamePhaseType.Move:
                    return "Movement";
                case GamePhaseType.Attack:
                    return "Attack";
                default:
                    throw new NotImplementedException("PhaseNameShort " + type.ToString());
            }

        }

        
        virtual public void OnCancelMenu()
        { }
        virtual public void OnMenuSelect(int buttonIndex)
        { }

        virtual public void OnSelectUnit(bool select)
        { }

        //bool canCounterAttack()
        //{
        //    attackAnimation.AttackType == Battle.AttackType.CloseCombat && 
        //        attackAnimation.defender.
        //}

        /// <returns>End update</returns>
        //protected bool updateAttackAnimation()
        //{
        //    if (attackAnimation.Update())
        //    {
        //        bool defenderLeftHisSquare = true;

        //        //The defending unit might counter attack
        //        if (attackAnimation.attackType.IsMelee)
        //        {
        //            defenderLeftHisSquare = attackAnimation.defender.Dead || attackAnimation.retreats > 0;

        //            if (defenderLeftHisSquare)
        //            {
        //                //viewFollowUpPrompt();
        //                return true;
        //            }
        //        }
                
        //        if (Commander.BattleLib.DefenderCanCounter(defenderLeftHisSquare, 
        //            attackAnimation.attackType, attackAnimation.defender ))
        //        {
        //            startCounterAttack();
        //            return false;
        //        }
        //        else
        //        {
        //            endAttackAnimation();
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        protected bool SquareUpdate
        {
            get { return isNewState || player.mapControls.isOnNewTile; }
        }

        //void startCounterAttack()
        //{
        //    VectorRect viewArea;
        //    if (player == null)
        //    {
        //        viewArea = Engine.Screen.SafeArea;
        //    }
        //    else
        //    {
        //        viewArea = Commander.cmdRef.hud.viewArea;
        //    }

        //    //Counter
        //    attackAnimation = new Commander.Battle.AttackAnimation(attackAnimation.defender,
        //        attackAnimation.attacker, AttackType.CounterAttack,
        //        viewArea, CommandType.NUM_NONE, null, absPlayer);
        //}

        //void viewFollowUpPrompt()
        //{
        //    if (toggRef.board.MovementRestriction(attackAnimation.attackPosition, attackAnimation.attacker) ==
        //                    ToggEngine.Map.MovementRestrictionType.Impassable)
        //    {
        //        endAttackAnimation();
        //    }
        //    else
        //    {
        //        if (player == null)
        //        { //Run ai
        //            int stayValue = attackAnimation.attacker.positionValue(attackAnimation.attacker.squarePos);
        //            int followUpValue = attackAnimation.attacker.positionValue(attackAnimation.attackPosition);

        //            if (followUpValue > stayValue)
        //            {
        //                followUpEvent();
        //            }
        //            else
        //            {
        //                endAttackAnimation();
        //            }
        //        }
        //        else
        //        {
        //            followUp = new FollowUp(attackAnimation, player);
        //        }
        //    }
        //}

        //virtual protected void followUpEvent() {  }

        //public void endAttackAnimation()
        //{
        //    //cmdRef.menu.CloseMenu();
        //    //Attack is complete
        //    removeFollowUp();
        //    attackAnimation = null;
        //    if (absPlayer.updateAttackAbility(true, true))
        //    {
        //        CommandCard.orders.NextEnabled(true);
        //        if (mapControls != null)
        //            mapControls.setSelectionPos(CommandCard.orders.Selected().unit.soldierModel.Position);
        //    }
        //}

        //protected void removeFollowUp()
        //{
        //    if (followUp != null)
        //    {
        //        followUp.DeleteMe();
        //        followUp = null;
        //    }
        //}

        virtual public void onNewTile()
        {
            //updateButtonDesc();
        }
        //virtual public void updateButtonDesc()
        //{ }

        abstract public EnableType canExitPhase();

        virtual public bool canGoToPreviousPhase() { return false; }
        virtual public void returnToThisPhaseBeginning() {}

        virtual public void OnExitPhase(bool forwardToNext) 
        {
            if (tutVid != null) tutVid.DeleteMe();
            //removeFollowUp();
        }
        

        protected const string EndTurnTitle = "End turn?";
        protected const string EndTurnOkText = "End turn";
        abstract public void EndTurnNotRecommendedText(out string title, out string message,out string okText);

        virtual public bool ViewHoverUnitDisplay { get { return true; } }

        protected void cancelButton(List<HUD.ButtonDescriptionData> ButtonDesc)
        {
            ButtonDesc.Add(new HUD.ButtonDescriptionData("Cancel", inputMap.back.Icon));
        }
        protected void actionButton(List<HUD.ButtonDescriptionData> ButtonDesc, string text)
        {
            ButtonDesc.Add(new HUD.ButtonDescriptionData(text, inputMap.click.Icon));
        }

        //protected void viewBoardRoamButtonDesc()
        //{
        //    //List<HUD.ButtonDescriptionData> ButtonDesc_Empty = new List<HUD.ButtonDescriptionData>();
        //    //panMapButton(ButtonDesc_Empty);
        //    //undoCommandButton(ButtonDesc_Empty);
        //    //EndPhaseButton(ButtonDesc_Empty);

        //    //Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_Empty);
        //}

        //protected void EndPhaseButton(List<HUD.ButtonDescriptionData> ButtonDesc) 
        //{ 

        //    ButtonDesc.Add(new HUD.ButtonDescriptionData(
        //        player.nextPhaseIsTurnOver()? "End turn" : "End phase", 
        //        cmdLib.ButtonIcon_NextPhase)); 
        //}

        //protected void undoCommandButton(List<HUD.ButtonDescriptionData> ButtonDesc)
        //{
        //    SpriteName buttonIcon = cmdLib.ButtonIcon_PrevPhase;

        //    switch (this.Type)
        //    {
        //        case GamePhaseType.GiveOrder:
        //            if (cmdRef.gamestate.gameSetup.useStrategyCards)
        //            {
        //                ButtonDesc.Add(new HUD.ButtonDescriptionData("Undo strategy", buttonIcon));
        //            }
        //            break;
        //    }
        //}

        public void PhaseMarkVisible(bool visible)
        {
            foreach (OrderedUnit ord in CommandCard.orders.list)
            {
                ord.PhaseMarkVisible(visible);
            }
        }

        protected void panMapButton(List<HUD.ButtonDescriptionData> ButtonDesc)
        {
            if (toggRef.gamestate.gameSetup.level != LevelEnum.NUM)
            {
                ButtonDesc.Add(new HUD.ButtonDescriptionData("Pan map", SpriteName.MouseAllDir, SpriteName.MouseButtonRight));
            }
        }

        protected bool isNewSquare
        {
            get
            {
                bool result = mapControls.isOnNewTile || isNewState;
                //isNewState = false;
                return result;
            }
        }

        public static bool BorderVisuals(GamePhaseType type, out Color bgColor, out SpriteName iconTile, out Color iconColor)
        {
            iconColor = Color.White;

            switch (type)
            {
                case GamePhaseType.Attack:
                    bgColor = Color.DarkRed;
                    iconTile = SpriteName.cmdStatsMelee;
                    return true;

                case GamePhaseType.GiveOrder:
                    bgColor = OrderedUnit.OrderActionReadyCol;
                    iconTile = SpriteName.cmdOrderCheckFlat;
                    return true;

                case GamePhaseType.Move:
                    bgColor = Color.DarkBlue;
                    iconTile = SpriteName.cmdStatsMove;
                    return true;

                case GamePhaseType.SelectCommand:
                    bgColor = Color.SaddleBrown;
                    iconTile = SpriteName.LfCardItemIcon;
                    return true;

                default:
                    bgColor = Color.Pink;
                    iconTile = SpriteName.NO_IMAGE;
                    return false;
            }
        }

        protected AbsCommandCard CommandCard
        {
            get { return absPlayer.commandCard; }
        }
    }

    struct PhaseUpdateArgs
    {
        public static PhaseUpdateArgs None = new PhaseUpdateArgs();

        public bool mouseOverHud;
        public bool blockTooltip;
        public bool exitUpdate;
        public bool refreshPhaseStatus;

        public bool ViewToolTip
        {
            get { return !mouseOverHud && !blockTooltip; }
        }
    }

    enum GamePhaseType
    {
        Init,
        SelectArmy,
        Deployment,
        SelectCommand,
        GiveOrder,
        Move,
        Attack,
        CollectVP,
        Waiting,
        NUM_NON
    }
}
