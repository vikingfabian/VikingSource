//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Game1.RTS.GameObject;
//using Microsoft.Xna.Framework.Input;

//namespace Game1.RTS.Display
//{
//    /// <summary>
//    /// Create a bar with info about the army
//    /// </summary>
//    class ArmyDialogue : AbsSelectedObjDialogue
//    {
//        GameObject.Army army;
//        Faction faction;

//        public ArmyDialogue(GameObject.Army army, WorldData world, ButtonsOverview buttonsOverview, Faction faction)
//            : base(world, buttonsOverview)
//        {
//            this.faction = faction;
//            this.army = army;

//            //createBar("");
//            buttonsOverview.Update(new List<ButtonOption>
//            {
//                new ButtonOption(false, TileName.LeftStick, "Move"),
//                new ButtonOption(false, TileName.ButtonA, "Attack"),
//                new ButtonOption(true, TileName.ButtonA, "Ship"),
//                new ButtonOption(false, TileName.ButtonX, "Split"),
//                new ButtonOption(true, TileName.ButtonX, "Combine"),
//                new ButtonOption(false, TileName.ButtonY, "Disband"),
//                new ButtonOption(false, TileName.ButtonB, "Exit"),

//            });
//        }

//        /// <returns>Close dialogue</returns>
//        override public bool UpdateInput(Input.AbsControllerInstance controller)
//        {
//            army.Move(controller.JoyStickValue(Stick.Left));
//            //var targets = world.collectBattleTargets(army);
//            //GameObject.AbsUnit closest = null;
//            //if (targets.Count > 0)
//            //{
//            //    closest = targets[0];//temp
//            //    if (!army.hasBattleTarget(closest))
//            //    {
//            //        new GameObject.BattleGroup(army, closest);
//            //    }
//            //}

//            if (controller.KeyDownEvent(Buttons.X))
//            {
//                if (controller.IsButtonDown(Buttons.LeftTrigger))
//                {//combine
//                    Army army2 = faction.ClosestFriendlyArmy(army, RTSlib.ArmyAttackRadius);
//                    if (army2 != null)
//                    {
//                        army.Combine(army2);
//                    }
//                }
//                else
//                { //split
//                    army.Split();
//                }
//            }
//            else if (controller.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.A))
//            {//build or drop boat
//                if (controller.IsButtonDown(Buttons.LeftTrigger))
//                {
//                    army.BuildOrDropShip();
//                }
//                else
//                {
//                    if (army.InBattleGroup)//closest != null)
//                    {

//                        LootFest.Music.SoundManager.PlayFlatSound(LoadedSound.Sword1);

//                        List<AbsUnit> attacker = new List<AbsUnit> { army };
//                        List<AbsUnit> defender = army.getBattle_factions_targets()[0];

//                        foreach (AbsUnit u in defender)
//                        {
//                            u.AttackerSupport(army, attacker);
//                        }

//                        new Battle(new BattleFaction(attacker, true), new BattleFaction(defender, false));
                     
//                    }    
//                }
//            }
//            else if (controller.KeyDownEvent(Buttons.Y))
//            {//Remove unit
//                army.DeleteMe( true);
//                return true;
//            }
//            if (controller.KeyDownEvent(Buttons.B, Buttons.Back, Buttons.Start) || !army.Alive)
//            {
//                return true;
//            }
//            return false;
//        }


//    }
//}
