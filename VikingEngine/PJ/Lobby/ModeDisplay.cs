using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;

namespace VikingEngine.PJ.Lobby
{
    class ModeDisplay
    {
        public const Buttons NextXInput = Buttons.DPadRight;
        public const Keys NextKeyboardInput = Keys.Tab;

        LobbyState lobby;
        PJ.Display.ImageButton prev, next;
        Graphics.Image mode;
        Graphics.TextG betaModeText;
        public Graphics.Image nextInputIcon;

        Graphics.Image lockIcon, unlockDlcArrow;
        PJ.Display.ImageButton unlockDlc;       


        public ModeDisplay(LobbyState lobby, float topBarH)
        {
            this.lobby = lobby;
            const int ModeOutlineWidth = 2;
            Vector2 buttonSpriteSz = new Vector2(20 + ModeOutlineWidth*2, 64 + ModeOutlineWidth*2);
            
            Vector2 sz = Vector2.Zero;
            sz.Y = topBarH - Engine.Screen.SafeArea.Y - Engine.Screen.IconSize * 0.4f;
            sz.X = sz.Y / buttonSpriteSz.Y * buttonSpriteSz.X;

            Vector2 position = Engine.Screen.SafeArea.Position;
            VectorRect nextArea = new VectorRect(position, sz);
            if (!PjRef.XboxLayout)
            {
                prev = new PJ.Display.ImageButton(SpriteName.birdModeNextButton, nextArea, HudLib.LargeButtonSettings);
                prev.baseImage.spriteEffects = SpriteEffects.FlipHorizontally;
                position = prev.area.RightTop;
            }

            sz.X = sz.Y / SpriteSheet.PjModeSz.Y * SpriteSheet.PjModeSz.X;
            VectorRect modeArea = new VectorRect(position, sz);
            modeArea.X += Engine.Screen.BorderWidth;
            Graphics.Image modeOutline = new Graphics.Image(SpriteName.WhiteArea,
                modeArea.Position, modeArea.Size, ImageLayers.Lay3_Back);
            position = modeArea.RightTop;
            modeOutline.Opacity = 0.6f;

            modeArea.AddRadius(-ModeOutlineWidth);
            mode = new Graphics.Image(SpriteName.birdModeContruction, modeArea.Position, modeArea.Size, ImageLayers.Lay3);
            
            position.X += Engine.Screen.BorderWidth;

            nextArea.Position = position;
            next = new PJ.Display.ImageButton(SpriteName.birdModeNextButton, nextArea, HudLib.LargeButtonSettings);

           
            nextInputIcon = new Graphics.Image(SpriteName.MissingImage,
                nextArea.RightBottom, Engine.Screen.IconSizeV2, HudLib.LayInputDisplay, true);
            nextInputIcon.Xpos += nextInputIcon.Width * 0.5f + Engine.Screen.BorderWidth;
            nextInputIcon.Ypos -= nextInputIcon.Height * 0.5f;
            

            lockIcon = new Graphics.Image(SpriteName.birdLock, mode.Area.PercentToPosition(new Vector2(0.85f, 0.28f)),
                new Vector2(Engine.Screen.IconSize * 3f), ImageLayers.AbsoluteBottomLayer, true);
            lockIcon.LayerAbove(mode);

            unlockDlcArrow = new Graphics.Image(SpriteName.birdUnlockModeDlcArrow, 
                lockIcon.Area.PercentToPosition(new Vector2(0.16f, -0.2f)),
                Engine.Screen.IconSize * 1.0f * new Vector2(2, 1), 
                HudLib.LayButtons -4);
            
            unlockDlc = new PJ.Display.ImageButton(SpriteName.DlcBoxPrimeLarge,
                new VectorRect(unlockDlcArrow.RightTop + new Vector2(-Engine.Screen.IconSize * 0.1f, 0f), 
                new Vector2(Engine.Screen.IconSize * 2.4f)),
                HudLib.LargeButtonSettings); 
            
            betaModeText = new Graphics.TextG(LoadedFont.Regular, mode.Area.PercentToPosition(new Vector2(0.05f, 0.14f)),
                new Vector2(Engine.Screen.TextSize), Graphics.Align.Zero, "BETA", Color.White, ImageLayers.Lay2);
            
            //END
            refreshModeVisuals();
        }

        public void refreshModeVisuals()
        {
            SpriteName modeImageTile = SpriteName.NO_IMAGE;

            switch (PjRef.storage.Mode)
            {
                case PartyGameMode.Jousting:
                    modeImageTile = SpriteName.birdModeJoust;
                    break;
                case PartyGameMode.Bagatelle:
                    modeImageTile = SpriteName.birdModeBagatelle;
                    break;
                case PartyGameMode.Match3:
                    modeImageTile = SpriteName.birdModeMatch3;
                    break;
                case PartyGameMode.MiniGolf:
                    modeImageTile = SpriteName.birdModeGolf;
                    break;
                case PartyGameMode.CarBall:
                    modeImageTile = SpriteName.birdModeCarBall;
                    break;

                case PartyGameMode.MeatPie:
                    modeImageTile = SpriteName.birdModeContruction;
                    break;
            }

            if (modeImageTile == SpriteName.NO_IMAGE)
            {
                modeImageTile = SpriteName.birdModeContruction;
                betaModeText.TextString = PjRef.storage.Mode.ToString() + Environment.NewLine + "BETA";
                betaModeText.Visible = true;
            }
            else
            {
                betaModeText.Visible = false;
            }

            mode.SetSpriteName(modeImageTile);

            bool canPlay;
            lobby.canViewMode(PjRef.storage.Mode, out canPlay);

            bool lockedContent = !canPlay;

            if (PlatformSettings.DebugLevel <= BuildDebugLevel.ShowDemo)
            {
                lockedContent = false;
            }

            bool viewDlc = lockedContent && !PjRef.hasAllContentDLC;

            mode.Opacity = viewDlc? 0.5f : 1f;
            lockIcon.Visible = viewDlc;
            unlockDlcArrow.Visible = viewDlc;
            unlockDlc.Visible = viewDlc;            
        }

        public void update()
        {
            if (!PjRef.XboxLayout)
            {
                if (prev.update())
                {
                    nextMode(-1);
                }
                if (next.update() ||
                    Input.Keyboard.KeyDownEvent(NextKeyboardInput))
                {
                    nextMode(1);
                }
            }

            bool highlightNext = false;
            foreach (var m in Input.XInput.controllers)
            {
                if (m.IsButtonDown(NextXInput))
                {
                    highlightNext = true;

                    if (m.KeyDownEvent(NextXInput))
                    {
                        nextMode(1);
                    }
                }
            }

            next.highlight.Visible = highlightNext || next.mouseOver;
            
            unlockDlc.update();
        }
        
        void nextMode(int dir)
        {
            if (dir != 0)
            {
                lobby.nextMode(dir);
                lobby.checkIfModeConflictsWithMultiplayer();
            }
        }

        void dlcClick()
        {
            PjLib.TryStartDlcPurchase(0);
        }
    }
}
