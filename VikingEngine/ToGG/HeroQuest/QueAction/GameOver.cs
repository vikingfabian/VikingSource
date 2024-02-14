using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class GameOver: ToggEngine.QueAction.AbsQueAction
    {
        Graphics.Image redBg;
        Graphics.TextG text = null;
        bool victory;

        public GameOver(bool victory)
            : base()
        {
            this.victory = victory;
        }
        public GameOver(System.IO.BinaryReader r)
           : base(r)
        {
        }

        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);
            w.Write(victory);
        }
        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);
            victory = r.ReadBoolean();
        }

        public override void onBegin()
        {
            VectorRect area = Engine.Screen.Area;
            area.AddRadius(2f);
            redBg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Foreground1);
            redBg.Color = victory? Color.Black : Color.DarkRed;
            redBg.Opacity = 0;
        }

        public override bool update()
        {
            if (redBg.Opacity < 0.6f)
            {
                redBg.Opacity += Ref.DeltaTimeSec * 0.2f;
            }
            else if (text == null)
            {
                text = new Graphics.TextG(LoadedFont.Bold, Engine.Screen.CenterScreen, Engine.Screen.TextTitleScale * 4f,
                    Graphics.Align.CenterAll, victory ? "VICTORY" : "GAME OVER", Color.White, ImageLayers.Foreground1_Front);
                text.Opacity = 0;

                Graphics.Motion2d fadein = new Graphics.Motion2d(Graphics.MotionType.OPACITY,
                    text, Vector2.One, Graphics.MotionRepeate.NO_REPEAT, 200, true);

                viewTime.Seconds = 4f;

                if (hqRef.setup.quest != QuestName.Custom)
                {
                    for (DoomChestLevel chestLevel = 0; chestLevel < DoomChestLevel.NUM; ++chestLevel)
                    {
                        if (hqRef.setup.conditions.doom.chestIsOpen(chestLevel))
                        {
                            var achievement = toggRef.achievements.Mission(hqRef.setup.quest, chestLevel);
                            achievement.Unlock();
                        }
                    }
                }
            }
            else 
            {
                if (viewTime.CountDown())
                {
                    hqRef.gamestate.endGame();
                    //new GameState.MainMenuState();
                }
            }

            return false;
        }

        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.GameOver; } }
    }


}
