using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    abstract class AbsQueActionDoomSkull : ToggEngine.QueAction.AbsQueAction
    {
        Graphics.Image skull;        

        public AbsQueActionDoomSkull()
            : base()
        { }

        public AbsQueActionDoomSkull(System.IO.BinaryReader r)
           : base(r)
        {
        }

        protected void createSkull(Vector2 pos)
        {
            skull = new Graphics.Image(SpriteName.DoomSkull, pos, Engine.Screen.SmallIconSizeV2, HudLib.PopupLayer, true);
            new Graphics.Motion2d(Graphics.MotionType.SCALE, skull, skull.Size * 2f, Graphics.MotionRepeate.BackNForwardOnce, 120, true);

            viewTime.MilliSeconds = toggLib.AnimTime(400);
        }

        protected bool updateSkull()
        {
            if (Ref.TimePassed16ms)
            {
                Vector2 diff = hqRef.playerHud.doombar.nextSkullPos - skull.Position;
                skull.Position += diff * 0.16f;

                if (diff.Length() < 2f)
                {
                    skull.DeleteMe();

                    hqRef.playerHud.removeDoomBar();
                    hqRef.playerHud.createDoomBar();
                    
                    viewTime.MilliSeconds = toggLib.AnimTime(100);
                    return true;
                }
            }
            return false;
        }

        public override void onRemove()
        {
            base.onRemove();

            hqRef.setup.conditions.doom.checkGameOver();
        }
    }

    class HeroDeath : AbsQueActionDoomSkull
    {
        Unit hero;
        int animationState = 0;
        
        public HeroDeath(Unit hero)
            :base()
        {
            this.hero = hero;
        }
        public HeroDeath(System.IO.BinaryReader r)
           : base(r)
        {
        }

        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);
            hero.netWriteUnitId(w);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);
            hero = Unit.NetReadUnitId(r);
        }

        public override void onBegin()
        {
            hqRef.setup.conditions.doom.OnHeroDeath();
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
                            new HeroDeathAnimation(hero);
                            viewTime.Seconds = 2f;
                            animationState++;
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
            camTarget = hero.squarePos;
            inCamCheck = true;

            return true;
        }

        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.HeroDeath; } }

        public override bool IsTopPrio => true;
    }
}
