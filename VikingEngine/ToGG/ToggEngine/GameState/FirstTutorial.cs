using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.ToGG.GameState
{
    class FirstTutorial : Engine.GameState
    {
        List<Graphics.ImageGroupParent2D> units;
        List<Graphics.Image> dummies;

        ListWithSelection<AbsTutState> states;
        GameSetup setup;

        public FirstTutorial(GameSetup setup)
            : base(true)
        {
            const int UnitCount = 4;


            draw.ClrColor = Color.DarkGreen;
            this.setup = setup;
            Vector2 unitSize = new Vector2( Engine.Screen.Height * 0.1f);
            const float PosAdj = 0.6f;
            Vector2[] unitPosAdj = new Vector2[]
            {
                new Vector2(-0.5f, -0.5f) * unitSize * PosAdj,
                new Vector2(0.5f, -0.2f) * unitSize * PosAdj,
                new Vector2(-0.5f, 0.3f) * unitSize * PosAdj,
                new Vector2(0.5f, 0.5f) * unitSize * PosAdj,
            };

            Vector2 unitDist = new Vector2(Engine.Screen.Height * 0.4f, unitSize.Y * 2f);
            Vector2 startPos = Engine.Screen.CenterScreen;
            startPos.X -= unitDist.X * 0.5f;
            startPos.Y -= unitDist.Y * 1.5f;

            Vector2 currentPos = startPos;
            units = new List<Graphics.ImageGroupParent2D>(UnitCount);
            for (int u = 0; u < UnitCount; ++u)
            {
                Graphics.ImageGroupParent2D unit = new Graphics.ImageGroupParent2D(4);

                for (int i = 0; i < 4; ++i)
                {
                    Graphics.Image soldier = new Graphics.Image(cmdRef.units.GetUnit(UnitType.Practice_Spearman).modelSettings.image, unitPosAdj[i], unitSize, ImageLayers.Lay1, true);
                    soldier.PaintLayer -= PublicConstants.LayerMinDiff * i;

                    unit.Add(soldier);
                }

                unit.ParentPosition = currentPos;
                currentPos.Y += unitDist.Y;

                units.Add(unit);
            }

            const int DummieCount = 3;
            currentPos = startPos;
            currentPos.X +=  unitDist.X;

            dummies = new List<Graphics.Image>(DummieCount);
            for (int i = 0; i < DummieCount; ++i)
            {
                Graphics.Image dummie = new Graphics.Image(cmdRef.units.GetUnit(UnitType.Practice_Dummy).modelSettings.image, currentPos, unitSize, ImageLayers.Lay1, true);
                currentPos.Y += unitDist.Y;

                dummies.Add(dummie);
            }


            states = new ListWithSelection<AbsTutState>();
            states.Add(new WaitSome(this, 500), false);
            states.Add(new ViewText(this, "1. Order"), false);
            for (int i = 0; i < 3; ++i)
            {
                states.Add(new Order(this, units[i]), false);
            }
            states.Add(new WaitSome(this, 1000), false);

            states.Add(new ViewText(this, "2. Move"), false);
            float movelenght = unitDist.X - unitSize.X;
            for (int i = 0; i < 3; ++i)
            {
                states.Add(new Move(this, units[i], movelenght), false);
            }
            states.Add(new WaitSome(this, 1000), false);

            states.Add(new ViewText(this, "3. Attack"), false);
            for (int i = 0; i < 3; ++i)
            {
                states.Add(new Attack(this, units[i], dummies[i], unitSize), false);
            }
            states.Add(new WaitSome(this, 2000), false);

            cinematicBorderEffect();
        }
        void cinematicBorderEffect()
        {
            float height = Engine.Screen.Height * 0.12f;
            Graphics.Image top = new Graphics.Image(SpriteName.WhiteArea, new Vector2(0, -height),
                new Vector2(Engine.Screen.Width, height), ImageLayers.Top1);
            top.Color = Color.Black;

            Graphics.Image bottom = new Graphics.Image(SpriteName.WhiteArea, new Vector2(0, Engine.Screen.Height),
                new Vector2(Engine.Screen.Width, height), ImageLayers.Top1);
            bottom.Color = Color.Black;

            const float MoveTime = 400;
            new Graphics.Motion2d(Graphics.MotionType.MOVE, top, new Vector2(0, height), Graphics.MotionRepeate.NO_REPEAT, MoveTime, true);
            new Graphics.Motion2d(Graphics.MotionType.MOVE, bottom, new Vector2(0, -height), Graphics.MotionRepeate.NO_REPEAT, MoveTime, true);

        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (states.Selected().Update())
            {
                //next state
                if (states.Next_IsEnd(false))
                {
                    //Close
                    //Engine.StateHandler.PopGamestate();
                    new Commander.CmdPlayState(setup);
                }
                else
                {
                    states.Selected().Begin();
                }
            }
        }

        //public override Engine.GameStateType Type
        //{
        //    get { return Engine.GameStateType.Other; }
        //}

        abstract class AbsTutState
        {
            protected Time timer;
            FirstTutorial gamestate;

            public AbsTutState(FirstTutorial gamestate)
            {
                this.gamestate = gamestate;
            }

            virtual public void Begin() { }
            abstract public bool Update();
        }

        class ViewText : AbsTutState
        {
            Graphics.Image bg;
            Graphics.TextG text;

            public ViewText(FirstTutorial gamestate, string text)
                : base(gamestate)
            {
                bg = new Graphics.Image(SpriteName.WhiteArea, Engine.Screen.CenterScreen, new Vector2(Engine.Screen.Width * 0.8f, Engine.Screen.IconSize * 1.4f), ImageLayers.Foreground2, true);
                bg.Color = Color.Black;
                this.text = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.CenterScreen, new Vector2(Engine.Screen.TextSize * 1.6f), Graphics.Align.CenterAll, text, Color.White, ImageLayers.Foreground1);

                bg.Visible = false;
                this.text.Visible = false;

                timer.MilliSeconds = 1500;
            }

            public override void Begin()
            {
                bg.Visible = true;
                this.text.Visible = true;
            }

            public override bool Update()
            {
                if (timer.CountDown())
                {
                    bg.DeleteMe();
                    text.DeleteMe();
                    return true;
                }
                return false;
            }
        }

        class Order : AbsTutState
        {
            Graphics.ImageGroupParent2D unit;
            public Order(FirstTutorial gamestate, Graphics.ImageGroupParent2D unit)
                : base(gamestate)
            {
                this.unit = unit;
            }

            public override void Begin()
            {
                Graphics.Image check = new Graphics.Image(SpriteName.cmdOrderCheckFlat, Vector2.Zero, unit.GetImage(0).Size * 0.6f, ImageLayers.Foreground7, true);
                new Graphics.Motion2d(Graphics.MotionType.SCALE, check, check.Size * 2f, Graphics.MotionRepeate.BackNForwardOnce, 120, true);

                unit.AddAndUpdate(check);
                timer.MilliSeconds = 800;
            }

            public override bool Update()
            {
                return timer.CountDown();
            }
        }

        class Move : AbsTutState
        {
            Graphics.ImageGroupParent2D unit;
            float moveSpeed;
            
            public Move(FirstTutorial gamestate, Graphics.ImageGroupParent2D unit, float moveLenght)
                : base(gamestate)
            {
                this.unit = unit;

                moveSpeed = Engine.Screen.Height * 0.001f;
                timer.MilliSeconds = moveLenght / moveSpeed;
            }

            public override bool Update()
            {
                unit.ParentX += moveSpeed * Ref.DeltaTimeMs;
                return timer.CountDown();
            }
        }

        class Attack : AbsTutState
        {
            Graphics.ImageGroupParent2D unit;
            Graphics.Image dummie;
            Vector2 unitSize;
            public Attack(FirstTutorial gamestate, Graphics.ImageGroupParent2D unit, Graphics.Image dummie, Vector2 unitSize)
                : base(gamestate)
            {
                this.unit = unit;
                this.dummie = dummie;
                this.unitSize = unitSize;
            }

            public override void Begin()
            {
                const float SwingTime = 200;
                Graphics.Image swing = new Graphics.Image(SpriteName.cmdCCAttack, unit.ParentPosition, unitSize, ImageLayers.Foreground8, true);
                swing.Rotation = MathHelper.PiOver2;
                new Graphics.Motion2d(Graphics.MotionType.SCALE, swing, swing.Size * 1.6f, Graphics.MotionRepeate.NO_REPEAT, SwingTime, true);//
                new Graphics.Motion2d(Graphics.MotionType.MOVE, swing, new Vector2(unitSize.X, 0f), Graphics.MotionRepeate.NO_REPEAT, SwingTime, true);
                new Timer.Terminator(SwingTime, swing);
                timer.MilliSeconds = 600;

                var dummieBounce = new Graphics.Motion2d(Graphics.MotionType.MOVE, dummie, new Vector2(unitSize.X * 0.2f), Graphics.MotionRepeate.BackNForwardOnce, 60, false);
                new Timer.UpdateTrigger(SwingTime * 0.6f, dummieBounce, true);
            }

            public override bool Update()
            {
                return timer.CountDown();
            }
        }

        class WaitSome : AbsTutState
        {
            public WaitSome(FirstTutorial gamestate, float time)
                : base(gamestate)
            {
                timer.MilliSeconds = time;
            }

            public override bool Update()
            {
                return timer.CountDown();
            }
        }
    }
}
