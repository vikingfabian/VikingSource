using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.PJ.PjEngine;

namespace VikingEngine.PJ
{
    class GameModeSettings
    {
        public SpriteName symbol;
        public Range localPlayerRange;
        public Range remotePlayerRange;
        public bool hasNetwork;
        public GameModeAccessibility access;
        public ModeAvatarType avatarType; 

        public GameModeSettings(PartyGameMode mode)//Interval localPlayerRange, bool hasNetwork, GameModeAccessibility access)
        {
            switch (mode)
            {
                case PartyGameMode.Jousting:
                    localPlayerRange = new Range(2, PjLib.SharedControllerMaxPlayers);
                    hasNetwork = false;
                    access = GameModeAccessibility.Free_5;
                    avatarType = ModeAvatarType.Joust;
                    break;

                case PartyGameMode.Bagatelle:
                    localPlayerRange = new Range(2, PjLib.SharedControllerMaxPlayers);
                    hasNetwork = true;
                    access = GameModeAccessibility.Paid_4;
                    avatarType = ModeAvatarType.Joust;
                    break;

                case PartyGameMode.Strategy:
                    localPlayerRange = new Range(2, PjLib.SharedControllerMaxPlayers);
                    hasNetwork = false;
                    access = GameModeAccessibility.Beta_3;
                    avatarType = ModeAvatarType.Joust;
                    break;

                case PartyGameMode.MiniGolf:
                    localPlayerRange = new Range(2, PjLib.SharedControllerMaxPlayers);
                    hasNetwork = false;
                    access = GameModeAccessibility.Paid_4;
                    avatarType = ModeAvatarType.Joust;
                    break;
                    
                case PartyGameMode.CarBall:
                    localPlayerRange = new Range(2, 8);//12 Begränsat till jag har fler djur bilderf
                    hasNetwork = false;
                    access = GameModeAccessibility.Paid_4;
                    avatarType = ModeAvatarType.Car;
                    break;
                    
                case PartyGameMode.SuperSmashBirds:
                    localPlayerRange = new Range(2, 8);
                    hasNetwork = false;
                    access = GameModeAccessibility.Beta_3;
                    avatarType = ModeAvatarType.Joust;
                    break;

                case PartyGameMode.Tank:
                    localPlayerRange = new Range(2, 8);
                    hasNetwork = false;
                    access = GameModeAccessibility.DevOnly_1;
                    avatarType = ModeAvatarType.Car;
                    break;

                case PartyGameMode.SpacePirate:
                    localPlayerRange = new Range(1, 4);
                    hasNetwork = false;
                    access = GameModeAccessibility.Beta_3;
                    avatarType = ModeAvatarType.Joust;
                    break;

                //case PartyGameMode.SuperSmashBirds:
                //    localPlayerRange = new Interval(2, 8);
                //    hasNetwork = false;
                //    access = GameModeAccessibility.Beta_3;
                //    avatarType = ModeAvatarType.Joust;
                //    break;

                case PartyGameMode.Match3:
                    localPlayerRange = new Range(2, 8);
                    hasNetwork = false;
                    access = GameModeAccessibility.Paid_4;
                    avatarType = ModeAvatarType.Joust;
                    break;

                case PartyGameMode.MoneyRoll:
                    localPlayerRange = new Range(2, PjLib.SharedControllerMaxPlayers);
                    hasNetwork = false;
                    access = GameModeAccessibility.DevOnly_1;
                    avatarType = ModeAvatarType.Joust;
                    break;

                case PartyGameMode.MeatPie:
                    localPlayerRange = new Range(2, PjLib.SharedControllerMaxPlayers);
                    hasNetwork = false;
                    access = GameModeAccessibility.Free_5;
                    avatarType = ModeAvatarType.Joust;
                    break;

                default:
                    throw new NotImplementedException();
            }

            //symbol = SpriteName.NO_IMAGE;
            //this.localPlayerRange = localPlayerRange;
            //if (hasNetwork)
            //{
            //    remotePlayerRange = new Interval(2, PjLib.MaxRemotePlayers);
            //}
            //else
            //{
            //    remotePlayerRange = Interval.Zero;
            //}
            //this.hasNetwork = hasNetwork;
            //this.access = access;
        }
    }
}
