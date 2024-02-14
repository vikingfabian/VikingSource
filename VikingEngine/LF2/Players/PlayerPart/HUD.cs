using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.Players
{
    //HUD
    partial class Player
    {
        ExitModeButton exitModeButton = null;

        void buttonHelp(PlayerMode mode)
        {
            string title = null;
            List<HUD.ButtonDescriptionData> buttons = null;

            switch (mode)
            {
                case PlayerMode.Play:
                    buttons = new List<HUD.ButtonDescriptionData>
                    {
                        new HUD.ButtonDescriptionData("Move", SpriteName.LeftStick),
                        new HUD.ButtonDescriptionData("Camera", SpriteName.RightStick),

                        new HUD.ButtonDescriptionData("Use healing", LootfestLib.InputHealUpImg),

                        new HUD.ButtonDescriptionData("Menu", SpriteName.ButtonSTART),
                       

                        new HUD.ButtonDescriptionData(

                            "Map",

                            SpriteName.DpadDown),
                    };

                    if (LfRef.gamestate.Progress.GeneralProgress >= Data.GeneralProgress.DestroyMonsterSpawn)
                    {
                        buttons.Add(new HUD.ButtonDescriptionData("Equip", SpriteName.ButtonLT, SpriteName.ButtonA));
                        buttons.Add(new HUD.ButtonDescriptionData("Backpack", SpriteName.ButtonLT, SpriteName.ButtonX));
                        buttons.Add(new HUD.ButtonDescriptionData("Camera view", SpriteName.DpadUp));

                    }
                    if (Ref.netSession.InMultiplayer)
                    {
                        buttons.Add(new HUD.ButtonDescriptionData("Multiplayer", SpriteName.ButtonLT, SpriteName.ButtonB));
                    }
                    //if (settings.UnlockedWeaponSetups)
                    //{
                    //    buttons.Add(new HUD.ButtonDescriptionData("Next setup", SpriteName.BoardCtrlRB));
                    //}

                    title = "Character";
                    break;
                case PlayerMode.InMenu:
                    buttons = GameState.VoxelDesignState.ButtonDescription(mode);
                    title = "Menu";
                    break;
                case PlayerMode.Map:
                    buttons = new List<HUD.ButtonDescriptionData>
                    {
                        new HUD.ButtonDescriptionData("Move pointer", SpriteName.LeftStick),
                        new HUD.ButtonDescriptionData("Toggle waypoint", SpriteName.ButtonA),
                        new HUD.ButtonDescriptionData("Zoom", SpriteName.RightStick_UD),
                        new HUD.ButtonDescriptionData("You", SpriteName.BoardPieceCenter),
                        new HUD.ButtonDescriptionData("Waypoint", SpriteName.IconMapFlag),
                        new HUD.ButtonDescriptionData("Village", SpriteName.IconVillage),
                        new HUD.ButtonDescriptionData("City", SpriteName.IconTown),
                        new HUD.ButtonDescriptionData("Castle", SpriteName.IconCastle),

                    };
                    title = "Map";
                    break;
                case PlayerMode.Creation:
                    buttons = GameState.VoxelDesignState.ButtonDescription(mode);
                    //};
                    title = "Build mode";
                    break;
                case PlayerMode.CreationSelection:
                    buttons = GameState.VoxelDesignState.ButtonDescription(mode);
                    title = "Selection";
                    break;

                //case PlayerMode.RCtoy:
                //    buttons = new List<HUD.ButtonDescriptionData>();
                //    rcToy.ViewControls(buttons);
                //    buttons.Add(new HUD.ButtonDescriptionData("Exit mode", SpriteName.ButtonLT, SpriteName.ButtonX));
                //    title = rcToy.ToString();
                //    break;
            }



            string tip = null;
            if (buttons != null)
            {
                buttons.Insert(0, new HUD.ButtonDescriptionData("EXIT HELP", SpriteName.ButtonBACK));
                tip = LootfestLib.RandomTip(progress);
                buttonLayOut = new HUD.ButtonLayout(buttons, screenArea, safeScreenArea, title, tip);

                if (mode == PlayerMode.Play)
                {
                    //view button assignment
                    buttonLayOut.AddOn(new EquipSetupGroup(0, safeScreenArea.Center, progress.Equipped, false,
                        LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather1_PickGifts && progress.GiftLeftFromFather <= 1));
                }

            }
        }

        public void beginSendMessage()
        {
            //CloseMenu();
            if (mode != PlayerMode.InMenu)
                exitCurrentMode();

            Engine.XGuide.BeginKeyBoardInput(new KeyboardInputValues("Send message", "Will message all players in the network, max " +
                ChatMessageMaxChars.ToString() + "chars.", TextLib.EmptyString, Index));
        }

        public QuestDialogue QuestDialogue = null;

        int HUDrow = 0;
        List<AbsHUD> HUDgroup = new List<AbsHUD>();
        ItemHUD[] itemHUDs;

        void addHUD(AbsHUD obj)
        {
            const float Space = 32;
            Vector2 pos = safeScreenArea.Position;
            if (HUDgroup.Count > 0)
            {
                pos.X = HUDgroup[HUDgroup.Count - 1].Right + Space;
                pos.Y = HUDrowY;

                if (pos.X + obj.Width > safeScreenArea.Width)
                {
                    HUDrow++;
                    pos.X = safeScreenArea.X;
                    pos.Y = HUDrowY;
                }
            }
            obj.Position = pos;
            HUDgroup.Add(obj);
        }
        float HUDrowY
        { get { return safeScreenArea.Y + HUDrow * AbsHUD.HUDRowHeight; } }

        void createHUD()
        {
            bool minimalSpace = screenArea.Width < 640;
            Health healthBar = new Health(minimalSpace);
            addHUD(healthBar);

            hero.SetHUD(healthBar, null, null);

            itemHUDs = new ItemHUD[(int)GameObjects.Gadgets.Item.NumTypes];
            for (GameObjects.Gadgets.GoodsType iType = Item.FirstType; iType < GoodsType.END_ITEMS; ++iType)//int i = 0; i < itemHUDs.Length; i++)
            {
                ItemHUD itemHUD = new ItemHUD(iType);
                itemHUDs[(int)iType - Item.FirstEnumIndex] = itemHUD;
                addHUD(itemHUD);
            }

        }

        void updateHUD()
        {

            List<AbsHUD> HUDgroupCopy = new List<AbsHUD>(HUDgroup.Capacity);
            HUDgroupCopy.AddRange(HUDgroup);
            HUDgroup.Clear();
            HUDrow = 0;
            foreach (AbsHUD h in HUDgroupCopy)
            {
                addHUD(h);
            }
            updateMessageHandlerPos();

        }
        void updateMessageHandlerPos()
        {
            if (messageHandler != null)
            {
                Vector2 messageHandlerPos = safeScreenArea.Position;
                messageHandlerPos.Y += 40;
                messageHandler.Position = messageHandlerPos;
            }
        }

        public void UpdateItemAmount(GameObjects.Gadgets.Item item)
        {
            if (itemHUDs != null)
                itemHUDs[(int)item.Type - Item.FirstEnumIndex].BumpVisibility(item.Amount);
        }

        void clearGamerNames()
        {
            foreach (GamerName gn in gamerNames)
            {
                gn.DeleteMe();
            }
            gamerNames.Clear();
        }

        Image pauseBorder;
        TextG pauseText;
        public void Pause(bool isPaused)
        {
            const float TransfereTime = 200;

            bool change = true;

            if (isPaused)
            {
                if (pauseBorder == null)
                {
                    pauseBorder = new Image(SpriteName.LFEdge, ScreenArea.Position, ScreenArea.Size, ImageLayers.Background8);
                    pauseBorder.Color = Color.Black;
                    pauseBorder.Transparentsy = 0;




                    if (menu != null)
                    {
                        pauseText = new TextG(LoadedFont.Lootfest, new Vector2(menu.Right, menu.Center.Y), new Vector2(1.2f, 0.8f), Align.CenterAll, "PAUSE",
                            Color.White, ImageLayers.Background6);
                        //new Graphics.Effects.Motion2d(MotionType.TRANSPARENSY, pauseText, new Vector2(-0.6f), MotionRepeate.BackNForwardLoop, 800, true);
                        pauseText.Rotation = MathHelper.PiOver2;

                    }

                }
                else
                {
                    change = false;
                }
            }
            else
            {
                if (pauseText != null)
                    new Graphics.Motion2d(MotionType.TRANSPARENSY, pauseText, new Vector2NegativeOne, MotionRepeate.NO_REPEATE, TransfereTime, true);

                new Timer.TerminateCollection(TransfereTime * 10, new List<IDeleteable> { pauseBorder, pauseText });
                
            }
            if (change)
            {
                if (pauseBorder != null)
                    new Graphics.Motion2d(MotionType.TRANSPARENSY, pauseBorder, Vector2.One * (0.7f * lib.BoolToDirection(isPaused)), MotionRepeate.NO_REPEATE, TransfereTime, true);
                if (pauseText != null)
                    new Graphics.Motion2d(MotionType.MOVE, pauseText, new Vector2(40 * lib.BoolToDirection(isPaused), 0), MotionRepeate.NO_REPEATE, TransfereTime, true);
            
                if (!isPaused)
                    pauseBorder = null;
            }
        }

        public Vector2 CompassIconLocation { get { return progress.mapFlag.PlanePos; } }
        public SpriteName CompassIcon { get { return SpriteName.IconMapFlag; } }
        public bool CompassIconVisible { get { return progress.mapFlag.Visible; } }
        ///// <summary>
        ///// View a throphy icon in the center of the screen for a moment
        ///// </summary>
        //void throphyMessage()
        //{

        //}
    }
}
