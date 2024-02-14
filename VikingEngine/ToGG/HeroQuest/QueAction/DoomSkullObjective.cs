using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class DoomSkullObjective : AbsQueActionDoomSkull
    {
        Unit unit;
        IntVector2 position;
        bool removeUnit;
        int animationState = 0;

        public DoomSkullObjective(Unit unit, bool removeUnit)
           : base()
        {
            this.unit = unit;
            position = unit.squarePos;
            this.removeUnit = removeUnit;
        }

        public DoomSkullObjective(IntVector2 position)
            : base()
        {
            this.position = position;
        }

        public DoomSkullObjective(BinaryReader r)
           : base(r)
        {
        }

        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);
            toggRef.board.WritePosition(w, position);

            if (unit != null)
            {
                w.Write(true);
                unit.netWriteUnitId(w);
                w.Write(removeUnit);
            }
            else
            {
                w.Write(false);
            }            
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);
            position = toggRef.board.ReadPosition(r);

            if (r.ReadBoolean())
            {
                unit = Unit.NetReadUnitId(r);
                removeUnit = r.ReadBoolean();
            }
            else
            {
                unit = null;
                removeUnit = false;
            }            
        }

        public override void onBegin()
        {
            if (unit != null)
            {
                position = unit.squarePos;
            }
            hqRef.setup.conditions.doom.OnSkullObjective();
            viewTime.Seconds = 0.8f;
        }

        public override bool update()
        {
            if (viewTime.CountDown())
            {
                switch (animationState)
                {
                    case 0:
                        createSkull(Engine.Screen.CenterScreen);
                        animationState++;
                        break;
                    case 1:
                        hqRef.playerHud.createDoomBar();
                        animationState++;
                        break;
                    case 2:
                        if (updateSkull())
                        {
                            if (removeUnit)
                            {
                                unit.DeleteMe();
                                viewTime.Seconds = 0.6f;
                                animationState++;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        break;

                    default:
                        return true;
                }
            }
            return false;
        }

        public override bool CameraTarget(out IntVector2 camTarget, out bool inCamCheck)
        {
            camTarget = position;
            inCamCheck = false;

            return true;
        }

        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.DoomSkullObjective; } }

        //public override bool IsTopPrio => false;
    }
}
