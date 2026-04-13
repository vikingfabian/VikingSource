using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Interface
{
    class MessageGroup
    {
        public RichboxGuiSettings settings;
        protected PlayerData playerData;

        protected List<Message> messages = new List<Message>();
        protected float screenAreaBottom;
        protected float viewMessageSeconds;

        public MessageGroup(RichboxGuiSettings settings, PlayerData playerData, float viewMessageSeconds)
        {
            this.settings = settings;
            this.playerData = playerData;
            this.viewMessageSeconds = viewMessageSeconds;
        }

        public static void Title(RichBoxContent content, string title)
        {
            content.Add(new RbBeginTitle(2));
            content.Add(new RbImage(SpriteName.cmdWarningTriangle));
            content.space();
            content.Add(new RbText(title, Color.Yellow));
            content.newLine();
        }


        virtual protected Vector2 position()
        {
            Vector2 result = new Vector2(Engine.Screen.SafeArea.Right - (RichMenu.DefaultRenderEdge.X + HudLib.MessageDisplayWidth),
              Engine.Screen.SafeArea.Y);

            return result;
        }
        void UpdatePositions()
        {
            Vector2 currentPos = position();
            foreach (var message in messages)
            {
                currentPos = message.UpdatePositions(currentPos, screenAreaBottom);

                currentPos.Y += settings.edgeWidth * 2f;
            }
        }

        protected void add(RichBoxContent content)
        {
            messages.Insert(0, new Message(playerData, content, position().X, settings));
            UpdatePositions();
        }

        public bool freeSpace()
        {
            return messages.Count < 3;
        }

        public void Update(ref bool mouseOver)
        {

            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    message.update(ref mouseOver);
                }

                if (messages.Last().time.secPassed(viewMessageSeconds))
                {
                    arraylib.PullLastMember(messages).DeleteMe();
                }
            }
        }
    }

    class MessageGroup_Editor : MessageGroup
    {
        public MessageGroup_Editor()
            : base(HudLib.richboxGui, XGuide.LocalHost, 3)
        {
            
        }

        public void Add(string message)
        {
            RichBoxContent content = new RichBoxContent();
            
            content.text(message);

            Add(content);
        }

        public void Add(RichBoxContent content)
        {
            add(content);
        }
    }

    class MessageGroup_Ingame : MessageGroup
    {        
        LocalPlayer player;

        static readonly TimeLength FoodWarningTimeout = new TimeLength(120);

        TimeInGameCountdown cityLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
        TimeInGameCountdown armyLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
        
        public MessageGroup_Ingame(LocalPlayer player, int numPlayers, RichboxGuiSettings settings)
            :base(settings, player.playerData, 20)
        {
            this.player = player;
        }

        bool highEconomyWarningBlock()
        { 
            return DssRef.storage.gameRuleset.centralGold && player.faction.money.GetGold() > DssConst.Gold_RichStatus;
        }

        public void blockFoodWarning(bool block)
        {
            //Blocked during tutorial
            if (block)
            {
                cityLowFoodMessageCooldown.start(new TimeLength(100000));
                armyLowFoodMessageCooldown.start(new TimeLength(100000));
            }
            else
            {
                cityLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
                armyLowFoodMessageCooldown = new TimeInGameCountdown(FoodWarningTimeout);
            }
        }

        public void onControllerClick()
        {
            foreach (var m in messages)
            {
                if (m.onControllerClick())
                {
                    return;
                }
            }
        }

        public void onGameStart()
        { 
            screenAreaBottom = player.playerData.view.DrawArea.Bottom + Engine.Screen.SmallIconSize;
            //if (player.hud.head.Right > player.playerData.view.DrawArea.Width / 2)
            //{
                
            //}
        }

        

        public static void ControllerInputIcons(LocalPlayer player, List<AbsRichBoxMember> button)
        {
            if (player.gameControls.input.inputSource.HasControllerInput &&
               player.gameControls.input.ControllerMessageClick.IsActive)
            {
                RichBoxContent.ButtonMap(player.gameControls.input.ControllerMessageClick, button);
                button.Add(new RbSpace());
            }
        }

        public void cityLowFoodMessage(City city)
        {   
            if (!highEconomyWarningBlock() &&
                DssRef.storage.runTutorial == false && 
                cityLowFoodMessageCooldown.TimeOut())
            {
                cityLowFoodMessageCooldown.start();

                RichBoxContent content = new RichBoxContent();
                Title(content, DssRef.lang.Message_OutOfFood_Title);
                content.text(DssRef.lang.Message_CityOutOfFood_Text);

                content.newParagraph();

                var gotoButtonContent = new RichBoxContent();
                MessageGroup_Ingame.ControllerInputIcons(player,gotoButtonContent);
                //gotoButtonContent.Add(new RbText(city.TypeName()));
                city.toButtonContent(gotoButtonContent, true);

                content.Add(new ArtButton(RbButtonStyle.Primary, gotoButtonContent,
                    new RbAction1Arg<AbsGameObject>(goToMapObject, city, RbSoundType.Default))
                { fillWidth = true });

                Add(content);
            }
        }

        public void armyLowFoodMessage(Army army)
        {
            if (!highEconomyWarningBlock() &&
                DssRef.storage.runTutorial == false &&
                armyLowFoodMessageCooldown.TimeOut())
            {
                armyLowFoodMessageCooldown.start();

                RichBoxContent content = new RichBoxContent();
                Title(content, DssRef.lang.Message_OutOfFood_Title);
                content.text(DssRef.lang.Message_ArmyOutOfFood_Text);

                content.newParagraph();

                var gotoButtonContent = new RichBoxContent();
                MessageGroup_Ingame.ControllerInputIcons(player, gotoButtonContent);
                //gotoButtonContent.Add(new RbText(city.TypeName()));
                army.toButtonContent(gotoButtonContent, true);

                content.Add(new ArtButton(RbButtonStyle.Primary, gotoButtonContent,
                    new RbAction1Arg<AbsGameObject>(goToMapObject, army, RbSoundType.Default))
                { fillWidth = true });

                Add(content);
            }
        }

        public void changedAllBuildings(bool onOff, int count)
        {
            RichBoxContent content = new RichBoxContent();
            content.h2(string.Format( DssRef.lang.GeneralSetting_ApplyMessage, count), HudLib.TitleColor_Head);
            content.space();
            content.Add(new RbText(onOff ? DssRef.lang.Hud_On : DssRef.lang.Hud_Off, HudLib.InfoYellow_Light));

            Add(content);
        }

        public void Add(string title, string text)
        {
            RichBoxContent content = new RichBoxContent();
            Title(content, title);
            content.text(text);

            Add(content);
        }

        public void Add(RichBoxContent content, bool vibrate = true)
        {
            if (StartupSettings.BlockMessages)
                return;

            SoundLib.message.Play(Pan.Right);
            if (vibrate)
            {
                player.gameControls.input.Vibrate(300, 0, 1);
            }

            if (player.hud.maximizedHud == false)
            {
                RichBoxContent compact = new RichBoxContent();
                foreach (var m in content)
                {
                    if (m.IsNewLine())
                    {
                        break;
                    }
                    else
                    {
                        compact.Add(m);
                    }
                }

                content = compact;
            }

            add(content);
        }

        public void goToMapObject(AbsGameObject city)
        {
            player.gameControls.map.selection.obj = city;
            player.gameControls.map.cameraFocus = city;
            player.hud.needRefresh = true;
        }

        

        override protected Vector2 position()
        {
            Vector2 result = player.hud.MessageStart;

            if (player.tutorial != null)
            {
                result.X -= HudLib.richboxGui.width;
            }

            return result;
        }       

    }

    class Message
    {
        public RichMenu menu;
        protected RichBoxContent content = new RichBoxContent();
        public TimeStamp time;

        public Message(PlayerData player, RichBoxContent content, float startX, RichboxGuiSettings settings)
        {
            menu = new RichMenu(HudLib.RbSettings, new VectorRect(VectorExt.V2FromX(startX), new Vector2(HudLib.MessageDisplayWidth, 500)),
                new Vector2(HudLib.MenuEdgeSize), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player);

            menu.Refresh(content);
            menu.updateHeightFromContent();
            menu.addBackground(HudLib.MessageBackground, HudLib.GUILayer + 2);
            time = TimeStamp.Now();
            
        }

        public bool onControllerClick()
        {
            if (menu.richBox.buttonGrid_Y_X.Count > 0 && menu.richBox.buttonGrid_Y_X[0].Count > 0)
            {
                if (time.msPassed(200))
                {
                    menu.richBox.buttonGrid_Y_X[0][0].onClick(null);
                }
                return true;
            }
            return false;
        }


        public Vector2 UpdatePositions(Vector2 position, float screenAreaBottom)
        {
            menu.moveToY(position.Y);

            return menu.backgroundArea.LeftBottom;
        }

        public void update(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
        }

        public void DeleteMe()
        {
            menu.DeleteMe();
        }
    }
}
