using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.HUD.RichMenu;
using VikingEngine.DSSWars.Interface;
using VikingEngine.LootFest.Players;
using VikingEngine.Timer;
using VikingEngine.DSSWars.Presentation;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleSetupManager
    {
        public const int NoPlayer = -1;
        public const int HumanPlayer = 0;
        public const int EnemyPlayer = 1;
        public const int BothPlayers = 2;

        public bool StartState = true;
        public Army friendlyArmy, enemyArmy;

        BattleSetup Setup => BattleLabStorage.Singleton.setup;

        public void beginBattleSetup()
        {
            LocalPlayer player = DssRef.state.LocalHost();
            beginBattleSetup(WP.ToTilePos(DssRef.state.culling.players[player.playerData.localPlayerIndex].MapCenter));
        }

        public void beginBattleSetup(IntVector2 center)
        {
            LocalPlayer player = DssRef.state.LocalHost();
            Ref.SetPause(true);
            StartState = false;

            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            DssRef.world.diplomacy.declareWar(player.faction, enemyFac);

            //IntVector2 position = WP.ToTilePos(DssRef.state.culling.players[player.playerData.localPlayerIndex].MapCenter);//mapConttilePosition;

            {
                var army = player.faction.NewArmy(VectorExt.AddX(center, -2));
                friendlyArmy = army;
                army.rotation = playerRot;
                army.food = float.MaxValue / 8;

                army.armyColumnWidth = 6;
            }
            {
                var army = enemyFac.NewArmy(VectorExt.AddX(center, 2));
                enemyArmy = army;
                army.rotation = enemyRot;
                army.food = float.MaxValue / 8;

                army.armyColumnWidth = 6;
            }
        }
        public void startBattle(bool paused)
        {
            startBattle(paused, Setup.attackingPlayer);
        }
        public void startBattle(bool paused, int attacker)
        {
            Ref.SetPause(paused);
            StartState = true;

            if (attacker == 0 || attacker == BattleSetupManager.BothPlayers)
            {
                friendlyArmy.Order_Attack(enemyArmy);
            }
            if (attacker == 1 || attacker == BattleSetupManager.BothPlayers)
            {
                enemyArmy.Order_Attack(friendlyArmy);
            }

            DssRef.stats.battle_lab_newbattle.addOne();
        }

        public bool updateObjectDisplay(RichBoxContent content, RichMenu menu)
        {
            content.h1(DssRef.lang.Lobby_Mode_BattleLab, HudLib.TitleColor_Head);

            content.newLine();
            if (StartState)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.BattleLab_StartHere) },
                    new RbAction(beginBattleSetup)));
            }
            else
            {
                List<ArtTabMember> tabs = new List<ArtTabMember>();
                for (int i = 0; i < 3; i++)
                {
                    tabs.Add(new ArtTabMember(new List<AbsRichBoxMember> { new RbText(PlayerOptionName(i)) }));
                }

                content.Add(new ArtTabgroup(tabs, Setup.selectedPlayer,
                    new Action<int>((int ix) => { Setup.selectedPlayer = ix; })));
                GodConscript.ToHud(content, addSoldier);

                content.newParagraph();
                content.Add(new RbSeperationLine());
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.BattleLab_Start) }, new RbAction1Arg<bool>(startBattle, false)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarPauseIcon) }, new RbAction1Arg<bool>(startBattle, true)));

                content.newParagraph();
                DropDownBuilder attackerOptions = new DropDownBuilder("attacker");
                {
                    for (int i = -1; i < 3; i++)
                    {
                        attackerOptions.AddOption(PlayerOptionName(i), i == Setup.attackingPlayer, i == 0, new RbAction1Arg<int>((int player) => { Setup.attackingPlayer = player; }, i), null);
                    }
                    attackerOptions.Build(content, SpriteName.WarsBattleIcon, DssRef.lang.BattleLab_Attacker, menu);
                }
            }


            return true;
        }

        

        string PlayerOptionName(int player)
        {
            switch (player)
            {
                case NoPlayer:
                    return DssRef.lang.Hud_None;

                case HumanPlayer:
                    return DssRef.state.LocalHost().Name;
                case EnemyPlayer:
                    return DssRef.lang.FactionName_DarkLord;

                default:
                    return DssRef.lang.Hud_Both;

            }
        }

        

        void addSoldier(int count)
        {
            addSoldier(count, Setup.conscript, Setup.selectedPlayer);
        }

        public void addSoldier(int count, ConscriptProfile conscript, int toPlayer)
        {
            //conscript.specialization = SpecializationType.Traditional;
            //conscript.training = TrainingLevel.Basic;

            SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
            {
                conscript = conscript,
                //new ConscriptProfile()
                //{
                //    weapon = weapon,
                //    armorLevel = Resource.ItemResourceType.PaddedArmor,
                //    training = TrainingLevel.Basic,
                //    specialization = SpecializationType.Traditional,
                //}
            };

            for (int i = 0; i < count; ++i)
            {
                if (toPlayer != EnemyPlayer)
                {
                    new SoldierGroup(friendlyArmy, SoldierProfile, friendlyArmy.position);
                }
                if (toPlayer != HumanPlayer)
                {
                    new SoldierGroup(enemyArmy, SoldierProfile, enemyArmy.position);
                }
            }

            if (toPlayer != EnemyPlayer)
            {
                friendlyArmy.setAsStartArmy();
            }
            if (toPlayer != HumanPlayer)
            {
                enemyArmy.setAsStartArmy();
            }
        }

        public void addTimedAttackFromEnemy(float seconds)
        {
            new TimedAction0ArgTrigger(() => 
            {
                enemyArmy.Order_Attack(friendlyArmy);
            }, seconds * TimeExt.SecondToMs); 
        }
    }
}
