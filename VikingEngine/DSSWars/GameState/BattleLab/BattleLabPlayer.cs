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
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.NPC;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleLabPlayer : Players.LocalPlayer
    {
        public bool StartState = true;
        int selectedPlayer = 0;
        ItemResourceType selectedWeapon = ItemResourceType.Sword;
        Army friendlyArmy, enemyArmy;
        public BattleLabPlayer(Faction faction)
            : base(faction)
        { 
            
        }

        public override bool updateObjectDisplay()
        {

            RichBoxContent content = new RichBoxContent();

            content.h1("Battle lab", HudLib.TitleColor_Head);

            content.newLine();
            if (StartState)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Start battle here") },
                    new RbAction(beginBattleSetup)));
            }
            else
            {
                ArtTabMember playerTab = new ArtTabMember(new List<AbsRichBoxMember> { new RbText(this.Name) });
                ArtTabMember enemyTab = new ArtTabMember(new List<AbsRichBoxMember> { new RbText(DssRef.lang.FactionName_DarkLord) });
                ArtTabMember bothTab = new ArtTabMember(new List<AbsRichBoxMember> { new RbText("Both") });

                content.Add(new ArtTabgroup(new List<ArtTabMember> { playerTab, enemyTab, bothTab }, selectedPlayer,
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
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Start battle") }, new RbAction(startBattle)));
            }

            hud.objMenu.refresh(this, content);
            return true;
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

            for (int i = 0; i < 4; ++i)
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
        }

        public void beginBattleSetup()
        {
            Ref.SetPause(true);
            StartState = false;
            selectedPlayer = 0;

            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            DssRef.diplomacy.declareWar(faction, enemyFac);

            IntVector2 position = mapControls.tilePosition;

            {
                var army = faction.NewArmy(VectorExt.AddX(position, -2));
                friendlyArmy = army;
                army.rotation = playerRot;
            }
            {
                var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
                enemyArmy = army;
                army.rotation = enemyRot;
            }
        }

        public void startBattle()
        {
            Ref.SetPause(false);
            StartState = true;

            friendlyArmy.setAsStartArmy();
            enemyArmy.setAsStartArmy();

            friendlyArmy.Order_Attack(enemyArmy);
        }
    }
}
