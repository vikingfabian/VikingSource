using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;

namespace VikingEngine.LootFest
{
    /// <summary>
    /// Lootfest3 player HUD
    /// </summary>
    class PlayerStatusDisplay
    {
        public HealthBar2 healthBar;
        public SuitUseHUD primaryAttackHUD;
        public SuitUseHUD specialAttackHUD;
        public CoinsHUD coinsHud;
        public ItemHUD itemHud;
        public MountHealthbar mountHealthbar = null;

        Graphics.Image editorInput, editorIcon;

        public PlayerStatusDisplay(Players.Player parent)
        {
            AbsHUD2.InitHUD();
            createDisplay(parent);
        }

        void createDisplay(Players.Player parent)
        {
            healthBar = new HealthBar2(parent.localPData.view.safeScreenArea.Position, LfLib.HeroHealth);
            primaryAttackHUD = new SuitUseHUD(parent, healthBar.Area, SpriteName.NO_IMAGE, SpriteName.LFPrimaryAttackHudBG, parent.inputMap.mainAttack, true);
            specialAttackHUD = new SuitUseHUD(parent, primaryAttackHUD.Area, SpriteName.NO_IMAGE, SpriteName.LFSecondaryAttackHudBG, parent.inputMap.altAttack, true);
            itemHud = new ItemHUD(specialAttackHUD.Area, parent);

            Vector2 coinsPos = itemHud.Area.RightTop;
            coinsPos.X += AbsHUD2.HUDStandardHeight;
            coinsHud = new CoinsHUD(coinsPos);

            Vector2 editorPos = coinsHud.Area.RightTop;
            editorPos.X += AbsHUD2.HUDStandardHeight;

            editorIcon = new Graphics.Image(SpriteName.VoxelEditorIcon, editorPos, new Vector2(AbsHUD2.HUDStandardHeight), LfLib.Layer_StatusDisplay);
            editorPos.X += editorIcon.Width;
            editorInput = new Graphics.Image(parent.inputMap.editorInput.OpenClose.Icon, editorPos, Engine.Screen.IconSizeV2, LfLib.Layer_StatusDisplay);
        }

        public void createMountDisplay()
        {
            if (mountHealthbar == null)
            {
                Vector2 pos = healthBar.Area.Position + healthBar.Area.Size * 0.3f;
                mountHealthbar = new MountHealthbar(pos, LfLib.MountHealth);
            }
        }

        public static Vector2 BottomLeftPos(Engine.PlayerData pdata)
        {
            Vector2 result = pdata.view.safeScreenArea.Position;
            result.Y += AbsHUD2.HUDStandardHeight * 1.2f;

            return result;
        }

        public void updateInput()
        {
            primaryAttackHUD.update();
            specialAttackHUD.update();
            itemHud.update();
        }

        public void onNewSuit(GO.AbsSuit suit)
        {
            primaryAttackHUD.clearAmount();
            primaryAttackHUD.useDotsForAmount = !(suit is GO.FutureSuit);

            primaryAttackHUD.setIconTile(suit.PrimaryIcon);
            specialAttackHUD.setIconTile(suit.SpecialAttackIcon);
        }

        public void RefreshAll(Players.Player parent)
        {
            coinsHud.UpdateAmount(parent.Storage.Coins);
            if (parent.hero != null)
            {
                onNewSuit(parent.Gear.suit);
                parent.hero.refreshAllHUD();
            }
        }

        public void DeleteMe()
        {
            healthBar.DeleteMe();
            primaryAttackHUD.DeleteMe();
            specialAttackHUD.DeleteMe();
            coinsHud.DeleteMe();
            itemHud.DeleteMe();

            editorIcon.DeleteMe();
            editorInput.DeleteMe();
        }
    }
}
