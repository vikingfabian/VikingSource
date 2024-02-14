using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class DoomClock : AbsQueActionDoomSkull
    {
        int animationState = 0;
        Graphics.Image face, arm;

        public DoomClock()
            : base()
        {
        }

        public override void onBegin()
        {
            hqRef.playerHud.createDoomBar();

            Vector2 pos = hqRef.playerHud.doombar.bgArea.CenterBottom;

            Vector2 sz = Engine.Screen.IconSizeV2 * 2f;
            pos.Y += Engine.Screen.IconSize + sz.Y * 0.5f;
            face = new Graphics.Image(SpriteName.DoomClockFace, pos, sz, HudLib.DungeonMasterLayer, true);
            arm = new Graphics.Image(SpriteName.DoomClockArm, pos, sz, ImageLayers.AbsoluteBottomLayer, true);
            arm.Rotation = MathExt.TauOver4 * hqRef.setup.conditions.doom.turn;
            arm.LayerAbove(face);

            viewTime.MilliSeconds = 1000;
            new Graphics.Motion2d(Graphics.MotionType.ROTATE, arm, VectorExt.V2FromX(MathExt.TauOver4), Graphics.MotionRepeate.NO_REPEAT, 400, true);

            hqRef.playerHud.doombar.nextClock.SetSpriteName(SpriteName.DoomClockIcon);
            new Graphics.Motion2d(Graphics.MotionType.SCALE, hqRef.playerHud.doombar.nextClock,
                 hqRef.playerHud.doombar.nextClock.Size * 2f, Graphics.MotionRepeate.BackNForwardOnce, 120, true);
        }
        
        public override bool update()
        {
            if (viewTime.CountDown())
            {
                switch (animationState)
                {
                    case 0:
                        face.DeleteMe();
                        arm.DeleteMe();                        

                        if (hqRef.setup.conditions.doom.OnDungeonMasterTurn())
                        {
                            createSkull(face.Position);
                            animationState++;
                        }
                        else
                        {
                            return true;
                        }
                        break;
                    case 1:
                        if (updateSkull())
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        override public bool NetShared { get { return false; } }
        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.DoomClock; } }
    }
}
