using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class MenuSystem : VikingEngine.ToGG.MenuSystem
    {
        ToGG.Data.SaveStateManager saveStateManager;

        public MenuSystem()
            : base(Input.InputSource.DefaultPC)
        {
            if (Ref.netSession.IsHostOrOffline)
            {
                saveStateManager = new ToGG.Data.SaveStateManager();
            }
        }

        protected override void InGameMenu(GuiLayout layout)
        {
            if (PlatformSettings.DevBuild)
            {
                new GuiTextButton("Give awards", null, listGiftedAwards, true, layout);
            }

            if (!PlatformSettings.Demo)
            {
                new GuiTextButton("*DEBUG*", null, debugOptions, true, layout);
            }
        }

        public void debugOptions()
        {
            GuiLayout layout = new GuiLayout("Debug Options", toggRef.menu.menu);
            {
                if (saveStateManager != null)
                {
                    new GuiTextButton("Save state", null,
                        lib.Combine(saveStateManager.savestate, toggRef.menu.CloseMenu), false, layout);
                    new GuiTextButton("Load state", null,
                        lib.Combine(saveStateManager.loadstate, toggRef.menu.CloseMenu), false, layout);
                }
                if (Ref.netSession.IsHostOrOffline)
                {
                    new GuiTextButton("Force end turn", null, debugForceEndTurn, false, layout);
                }
                new GuiTextButton("Set One Hp", null, debugOneHp, false, layout);
                new GuiTextButton("Set Zero Stamina", null, debugZeroStamina, false, layout);
                new GuiTextButton("Set Full Hp", null, debugFullHp, false, layout);
                new GuiTextButton("Set Full Blood rage", null, debugBloodRage, false, layout);
                new GuiTextButton("Full backpack access", null, debugBackpackAccess, false, layout);
                new GuiTextButton("Place gadget", null, debugPlaceGadget, true, layout);
                new GuiTextButton("Reset quickbelt", null, lib.Combine(Player.Backpack().equipment.quickbelt.reset, toggRef.menu.CloseMenu), false, layout);
                new GuiTextButton("Remove all fog", null,
                    lib.Combine(toggRef.board.removeAllFog, toggRef.menu.CloseMenu), false, layout);

                new GuiLabel("Ctrl + Alt + Click: square options", layout);
                new GuiLabel(MapGen.SpawnManager.SpawnResult, layout);
            }
            layout.End();
        }

        void debugPlaceGadget()
        {
            var available = new Gadgets.GadgetsCatalogue().listAll();
            GuiLayout layout = new GuiLayout("Gadgets", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {
                foreach (var m in available)
                {
                    new GuiIcon(m.Icon, m.Name, 
                        new GuiAction1Arg<AbsItem>(placeGadget, m), false, layout);
                }
            }
            layout.End();
        }

        void placeGadget(AbsItem item)
        {
            IntVector2 center = Player.HeroUnit.squarePos;

            foreach (var dir in IntVector2.Dir8Array)
            {
                //var sq = toggRef.Square(center + dir);
                var pos = center + dir;
                if (toggRef.board.IsSpawnAvailableSquare(pos))
                {
                    hqRef.items.moveToGround(item, pos);
                    CloseMenu();
                    return;
                }
            }
        }

        void debugForceEndTurn()
        {
            toggRef.menu.CloseMenu();
            new QueAction.QueActionEndTurn(false);
        }

        void debugBackpackAccess()
        {
            toggRef.menu.CloseMenu();

            Display.BackPackMenu.DebugAccess = true;

            new Players.Phase.Backpack(Player);
            //toggleBackpack();
        }

        void debugOneHp()
        {
            Player.HeroUnit.setHealth(1);
            toggRef.menu.CloseMenu();
        }
        void debugFullHp()
        {
            Player.HeroUnit.health.setMax();
            toggRef.menu.CloseMenu();
        }
        void debugBloodRage()
        {
            Player.HeroUnit.data.hero.bloodrage.setMax();
            toggRef.menu.CloseMenu();
        }

        void debugZeroStamina()
        {
            Player.HeroUnit.data.hero.stamina.setZero();
            toggRef.menu.CloseMenu();
        }

        void listGiftedAwards()
        {
            var list = toggRef.achievements.giftedList();

            GuiLayout layout = new GuiLayout("Gift achievement", toggRef.menu.menu);
            {
                foreach (var m in list)
                {
                    new GuiTextButton(m.name, m.desc, new GuiAction1Arg<int>(
                        giftedAwardsLink, m.id), false, layout);
                }
            }
            layout.End();
        }

        void giftedAwardsLink(int id)
        {
            HeroQuest.hqRef.players.remotePlayersCounter.Reset();
            HeroQuest.hqRef.players.remotePlayersCounter.Next();

            toggRef.achievements.GetGiftedAchievement(id).NetSend(
                HeroQuest.hqRef.players.remotePlayersCounter.GetSelection);

            CloseMenu();
        }

        Players.LocalPlayer Player => hqRef.players.localHost;
    }
}
