using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander.GO;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG
{
    class GamePhase_SelectArmy : AbsGamePhase
    {
        public const string Name = "Select Army";

        ArmySetup selectedArmy = null;

        ArmyRace[] armyOptions = new ArmyRace[]
        {
            ArmyRace.Human,
            ArmyRace.Undead,
            ArmyRace.Elf,
            ArmyRace.Orc,
        };

        public GamePhase_SelectArmy(Commander.Players.AbsLocalPlayer player)
            : base(player)
        {
            if (player.LocalHumanPlayer)
            {
                selectCommandPromt();

                if (StartUpSett.cmdAutoPickMyArmy != ArmyRace.NUM_NON)
                {
                    selectedArmy = new ArmySetup(StartUpSett.cmdAutoPickMyArmy);
                }
            }
            else
            {//Ai
                if (StartUpSett.cmdAutoPickAiArmy == ArmyRace.NUM_NON)
                {
                    selectedArmy = new ArmySetup(arraylib.RandomListMember(armyOptions));
                }
                else
                {
                    selectedArmy = new ArmySetup(StartUpSett.cmdAutoPickAiArmy);
                }
            }
        }

        //public GamePhase_SelectArmy(Commander.Players.AiPlayer aiplayer)
        //    : base(aiplayer)
        //{
            
        //}

        void selectCommandPromt()
        {
            Commander.cmdRef.hud.viewInputPrompt(Name, toggRef.inputmap.click.Icon);//player.inputMap.ButtonIcon(Input.ButtonActionType.MenuClick));

        }
        public override void Update(ref PhaseUpdateArgs args)
        {
            isNewState = false;

            mapControls.updateMapMovement(true);
            if (toggRef.inputmap.click.DownEvent)//player.inputMap.DownEvent(Input.ButtonActionType.MenuClick))
            {
                menuFile();
            }

            basicUpdate();
        }

        public override bool UpdateAi()
        {
            basicUpdate();
            return false;
        }

        void basicUpdate()
        {
            if (selectedArmy != null)
            {
                absPlayer.settings.Set(selectedArmy);
                absPlayer.StartPhase(GamePhaseType.Deployment);
            }
        }

        public void OnMenuSelect(ArmyRace race)
        {
            selectedArmy = new ArmySetup(race);
            toggRef.menu.CloseMenu();
        }

        public void menuFile()
        {
            toggRef.menu.OpenMenu(true);
            GuiLayout layout = new GuiLayout("Select Army", toggRef.menu.menu);
            {
                foreach (ArmyRace army in armyOptions)
                {
                    new HUD.GuiIconTextButton(UnitLib.ArmyRaceIcon(army), army.ToString(), null,
                        new HUD.GuiAction1Arg<ArmyRace>(OnMenuSelect, army), false, layout);
                }

                new GuiSectionSeparator(layout);
                toggRef.menu.openManualButton(layout);
            }
            layout.End();
        }

        public override void EndTurnNotRecommendedText(out string title, out string message, out string okText)
        {
            throw new NotImplementedException();
        }
        public override EnableType canExitPhase()
        {
            return EnableType.Disabled;
        } 

        public override void OnCancelMenu()
        {
            selectCommandPromt();
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
            get { return Name; }
        }

        public override GamePhaseType Type
        {
            get { return GamePhaseType.SelectArmy; }
        }
    }
}
