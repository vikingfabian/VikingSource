using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    abstract class AbsActionButton : Button
    {
        protected LocalPlayer player;
        protected Graphics.ImageGroup coverImages;
        TimeStamp mouseEnterTime;
        protected List<ActionTargetGui> actionTargets = null;

        public AbsActionButton(VectorRect area, LocalPlayer player)
            : base(area, HudLib.ContentLayer, ButtonTextureStyle.Standard)
        {            
            this.player = player;
        }

        public override bool update()
        {
            if (mouseOver && mouseEnterTime.msPassed(400))
            {
                IntVector2? target = cameraTarget();
                if (target != null)
                {
                    player.UpdateSpectating(target.Value);
                }
            }
            return base.update();
        }

        virtual public IntVector2? cameraTarget()
        {
            return null; 
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            if (enter)
            {
                mouseEnterTime = TimeStamp.Now();
            }
            else
            {
                arraylib.DeleteAndNull(ref actionTargets);
            }
        }

        protected override void onEnableChange()
        {
            base.onEnableChange();

            if (coverImages != null)
            {
                if (enabled)
                {
                    coverImages.ColorAndAlpha(Color.White, 1f);
                }
                else
                {
                    coverImages.ColorAndAlpha(Color.DarkGray, 0.4f);
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            arraylib.DeleteAndNull(ref actionTargets);
        }
    }

    class ActionTargetGui : AbsUpdateable
    {
        Graphics.Mesh model;
        float rot;

        public ActionTargetGui(IntVector2 pos, bool instantAction)
            :base(instantAction)
        {
            Ref.draw.CurrentRenderLayer = Draw.Draw3dOverlay_Layer;

            model = new Graphics.Mesh(LoadedMesh.plane, toggRef.board.toWorldPos_Center(pos, 0),
                new Vector3(0.4f), Graphics.TextureEffectType.Flat, SpriteName.cmdActionTargetSeletion, Color.White);

            var bump = new Graphics.Motion3d(Graphics.MotionType.SCALE, model,
                model.Scale * 0.3f, Graphics.MotionRepeate.BackNForwardOnce, 120, true);

            Ref.draw.CurrentRenderLayer = 0;

            Time_Update(0);
        }


        public override void Time_Update(float time_ms)
        {
            rot += Ref.DeltaTimeSec * 0.5f;

            model.Rotation = toggLib.PlaneTowardsCamWithRotation(rot);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
        //cmdActionTargetSeletion
    }
}
