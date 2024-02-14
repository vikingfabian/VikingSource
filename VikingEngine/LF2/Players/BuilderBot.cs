using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Players
{
    class BuilderBot : AbsUpdateable
    {
        Player p;
        State state = State.Walking;
        Timer.Basic statetimer = new Timer.Basic(4000);
        Rotation1D walkingDir = Rotation1D.Random();
        Vector3 moveDir;

        public BuilderBot(Player p)
            :base(true)
        {
            this.p = p;
        }

        public override void Time_Update(float time)
        {
            switch (state)
            {
                case State.Walking:
                    JoyStickValue j  = new JoyStickValue();
                    j.Direction = walkingDir.Direction(1);
                    p.Pad_Event(j);
                    break;
                case State.InEditmode:
                    
                    break;
                case State.MoveEditor:
                    moveV3();
                    break;
                case State.Drawing:
                    moveV3();
                    break;
            }

            if (statetimer.Update(time))
            {
                state++;
                switch (state)
                {
                    case State.InEditmode:
                        p.BeginCreationMode();
                        p.SetRandomMaterial();
                        statetimer.Set(0);
                        break;
                    case State.MoveEditor:
                        moveDir = lib.RandomV3(Vector3.Zero, 1);
                        statetimer.Set(Ref.rnd.Int(3000));
                        break;
                    case State.Drawing: 
                        moveDir = lib.RandomV3(Vector3.Zero, 1);
                        drawClick(true);
                        statetimer.Set(Ref.rnd.Int(3000));
                        break;
                    case State.NUM:
                        walkingDir = Rotation1D.Random();
                        drawClick(false);
                        p.EndCreationMode();

                        state = State.Walking;
                        statetimer.Set(4000 + Ref.rnd.Int(5000));
                        break;

                }
            }
        }

        void drawClick(bool down)
        {
            ButtonValue e = new ButtonValue();
            e.Button = numBUTTON.RB;
            e.KeyDown = down;
            p.Button_Event(e);
        }

        void moveV3()
        {
            JoyStickValue j2 = new JoyStickValue();
            j2.Direction = new Microsoft.Xna.Framework.Vector2(moveDir.X, moveDir.Z);
            j2.Stick = Stick.Left;
            j2.DirAndTime = j2.Direction * 33;
            p.Pad_Event(j2);

            j2.Direction = new Microsoft.Xna.Framework.Vector2(0, moveDir.Y);
            j2.Stick = Stick.Right;
            j2.DirAndTime = j2.Direction * 33;
            p.Pad_Event(j2);
        }
        enum State
        {
            Walking,
            InEditmode,
            MoveEditor,
            Drawing,
            NUM
        }
    }
}
