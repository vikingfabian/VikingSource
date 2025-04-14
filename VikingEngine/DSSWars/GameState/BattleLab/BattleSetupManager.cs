using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.HUD.RichMenu;
using VikingEngine.DSSWars.Display;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleSetupManager
    {
        public bool StartState = true;
        Army friendlyArmy, enemyArmy;

        BattleSetup Setup => BattleLabStorage.Singleton.setup;

        public void beginBattleSetup()
        {
            LocalPlayer player = DssRef.state.LocalHost();
            Ref.SetPause(true);
            StartState = false;

            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            DssRef.diplomacy.declareWar(player.faction, enemyFac);

            IntVector2 position = WP.ToTilePos(DssRef.state.culling.players[player.playerData.localPlayerIndex].MapCenter);//mapConttilePosition;

            {
                var army = player.faction.NewArmy(VectorExt.AddX(position, -2));
                friendlyArmy = army;
                army.rotation = playerRot;
                army.food = float.MaxValue;
            }
            {
                var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
                enemyArmy = army;
                army.rotation = enemyRot;
                army.food = float.MaxValue;
            }
        }

        public void startBattle(bool paused)
        {
            Ref.SetPause(paused);
            StartState = true;

            if (Setup.attackingPlayer == 0 || Setup.attackingPlayer == BattleSetup.BothPlayers)
            {
                friendlyArmy.Order_Attack(enemyArmy);
            }
            if (Setup.attackingPlayer == 1 || Setup.attackingPlayer == BattleSetup.BothPlayers)
            {
                enemyArmy.Order_Attack(friendlyArmy);
            }
        }

        public bool updateObjectDisplay(RichBoxContent content, RichMenu menu)
        {
            content.h1(DssRef.todoLang.Lobby_Mode_BattleLab, HudLib.TitleColor_Head);

            content.newLine();
            if (StartState)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.BattleLab_StartHere) },
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

                var weapons_groups = ConscriptMenu.AllConstriptWeapons();
                foreach (var group in weapons_groups)
                {
                    content.newLine();
                    foreach (var wep in group)
                    {
                        content.Add(new ArtToggle(wep == Setup.selectedWeapon, new List<AbsRichBoxMember> { new RbImage(ResourceLib.Icon(wep)) },
                            new RbAction1Arg<ItemResourceType>(selectWeapon, wep), new RbTooltip_Text(LangLib.Item(wep))));
                    }
                }

                content.newParagraph();

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.todoLang.Hud_AddX, 1)) }, new RbAction1Arg<int>(addSoldier, 1)));
                {
                    const int AddCount = 5;
                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_XTimes, AddCount)) }, new RbAction1Arg<int>(addSoldier, AddCount)));
                }
                {
                    const int AddCount = 20;
                    content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_XTimes, AddCount)) }, new RbAction1Arg<int>(addSoldier, AddCount)));
                }

                content.newParagraph();
                content.Add(new RbSeperationLine());
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.BattleLab_Start) }, new RbAction1Arg<bool>(startBattle, false)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarPauseIcon) }, new RbAction1Arg<bool>(startBattle, true)));

                content.newParagraph();
                DropDownBuilder attackerOptions = new DropDownBuilder("attacker");
                {
                    for (int i = -1; i < 3; i++)
                    {
                        attackerOptions.AddOption(PlayerOptionName(i), i == Setup.attackingPlayer, i == 0, new RbAction1Arg<int>((int player) => { Setup.attackingPlayer = player; }, i), null);
                    }
                    attackerOptions.Build(content, SpriteName.WarsBattleIcon, DssRef.todoLang.BattleLab_Attacker, menu);
                }
            }
            

            return true;
        }

        string PlayerOptionName(int player)
        {
            switch (player)
            {
                case -1:
                    return DssRef.todoLang.Hud_None;

                case 0:
                    return DssRef.state.LocalHost().Name;
                case 1:
                    return DssRef.lang.FactionName_DarkLord;

                default:
                    return DssRef.todoLang.Hud_Both;

            }
        }

        void selectWeapon(ItemResourceType item)
        {
            Setup.selectedWeapon = item;
        }

        void addSoldier(int count)
        {
            SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile()
                {
                    weapon = Setup.selectedWeapon,
                    armorLevel = Resource.ItemResourceType.PaddedArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.Traditional,
                }
            };

            for (int i = 0; i < count; ++i)
            {
                if (Setup.selectedPlayer != 1)
                {
                    new SoldierGroup(friendlyArmy, SoldierProfile, friendlyArmy.position);
                }
                if (Setup.selectedPlayer != 0)
                {
                    new SoldierGroup(enemyArmy, SoldierProfile, enemyArmy.position);
                }
            }

            if (Setup.selectedPlayer != 1)
            {
                friendlyArmy.setAsStartArmy();
            }
            if (Setup.selectedPlayer != 0)
            {
                enemyArmy.setAsStartArmy();
            }
        }
    }
}
