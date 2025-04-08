using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.NPC;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleLabPlayer : Players.LocalPlayer
    {
        const int BothPlayers = 2;
        public bool StartState = true;
        int selectedPlayer = BothPlayers;
        ItemResourceType selectedWeapon = ItemResourceType.Sword;
        int attackingPlayer = 0;
        Army friendlyArmy, enemyArmy;
        public BattleLabPlayer(Faction faction)
            : base(faction)
        { 
            
        }

        public override bool updateObjectDisplay()
        {
            hud.objMenu.createMenu(this);
            RichBoxContent content = new RichBoxContent();

            content.h1(DssRef.todoLang.Lobby_Mode_BattleLab, HudLib.TitleColor_Head);

            content.newLine();
            if (StartState)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(".Start battle here") },
                    new RbAction(beginBattleSetup)));
            }
            else
            {
                List<ArtTabMember> tabs = new List<ArtTabMember>();
                for (int i = 0; i < 3; i++)
                {
                    tabs.Add(new ArtTabMember(new List<AbsRichBoxMember> { new RbText(PlayerOptionName(i)) }));
                }
                //ArtTabMember playerTab = new ArtTabMember(new List<AbsRichBoxMember> { new RbText(this.Name) });
                //ArtTabMember enemyTab = new ArtTabMember(new List<AbsRichBoxMember> { new RbText(DssRef.lang.FactionName_DarkLord) });
                //ArtTabMember bothTab = new ArtTabMember(new List<AbsRichBoxMember> { new RbText("Both") });

                content.Add(new ArtTabgroup(tabs, selectedPlayer,
                    new Action<int>((int ix) => { selectedPlayer = ix; })));

                var weapons_groups =  ConscriptMenu.AllConstriptWeapons();
                foreach (var group in weapons_groups)
                {
                    content.newLine();
                    foreach (var wep in group)
                    {
                        content.Add(new ArtToggle(wep == selectedWeapon, new List<AbsRichBoxMember> { new RbImage(ResourceLib.Icon(wep)) },
                            new RbAction1Arg<ItemResourceType>(selectWeapon, wep), new RbTooltip_Text(LangLib.Item(wep))));
                    }
                }

                content.newParagraph();

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Add 1") }, new RbAction1Arg<int>(addSoldier, 1)));
                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText("x 5") }, new RbAction1Arg<int>(addSoldier, 5)));
                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText("x 20") }, new RbAction1Arg<int>(addSoldier, 20)));

                content.newParagraph();
                content.Add(new RbSeperationLine());
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(".Start battle") }, new RbAction1Arg<bool>(startBattle, false)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarPauseIcon) }, new RbAction1Arg<bool>(startBattle, true)));

                content.newParagraph();
                DropDownBuilder attackerOptions = new DropDownBuilder("attacker");
                {
                    for (int i = -1; i < 3; i++)
                    {
                        attackerOptions.AddOption(PlayerOptionName(i), i == attackingPlayer, i == 0, new RbAction1Arg<int>((int player) => { attackingPlayer = player; }, i), null);
                    }
                    attackerOptions.Build(content, SpriteName.WarsBattleIcon, "Attacker", hud.objMenu.menu);
                }

                
            }
            hud.objMenu.refresh(this, content);
            
            return true;
        }

        string PlayerOptionName(int player)
        {
                switch (player)
                {
                    case -1:
                        return DssRef.todoLang.Hud_None;

                    case 0:
                        return this.Name;
                    case 1:
                        return DssRef.lang.FactionName_DarkLord;

                    default:
                        return "Both";

                }
        }

        void selectWeapon(ItemResourceType item)
        {
            selectedWeapon = item;
        }

        void addSoldier(int count)
        {
            SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile()
                {
                    weapon = selectedWeapon,
                    armorLevel = Resource.ItemResourceType.PaddedArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.Traditional,
                }
            };

            for (int i = 0; i < count; ++i)
            {
                if (selectedPlayer != 1)
                {
                    new SoldierGroup(friendlyArmy, SoldierProfile, friendlyArmy.position);
                }
                if (selectedPlayer != 0)
                {
                    new SoldierGroup(enemyArmy, SoldierProfile, enemyArmy.position);
                }
            }

            if (selectedPlayer != 1)
            {
                friendlyArmy.setAsStartArmy();
            }
            if (selectedPlayer != 0)
            {
                enemyArmy.setAsStartArmy();
            }
        }

        public void beginBattleSetup()
        {
            Ref.SetPause(true);
            StartState = false;
            //selectedPlayer = 0;

            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            DssRef.diplomacy.declareWar(faction, enemyFac);

            IntVector2 position = WP.ToTilePos( DssRef.state.culling.players[playerData.localPlayerIndex].MapCenter);//mapConttilePosition;

            {
                var army = faction.NewArmy(VectorExt.AddX(position, -2));
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


            if (attackingPlayer == 0 || attackingPlayer == BothPlayers)
            {
                friendlyArmy.Order_Attack(enemyArmy);
            }
            if (attackingPlayer == 1 || attackingPlayer == BothPlayers)
            {
                enemyArmy.Order_Attack(friendlyArmy);
            }
        }
    }
}
