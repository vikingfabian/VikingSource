using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG
{
    class VictoryPointEffect : AbsUpdateable
    {
        int state_Fadein_Hold_Fadeout = 0;
        const float FadeTime = 400;
        const float FadeSpeed = 1f / FadeTime;
        Time stateTime = new Time(FadeTime);
        const float MoveSpeed = 0.001f;
        Graphics.Mesh model;

        public VictoryPointEffect(IntVector2 onSquare)
            :base(true)
        {
            model = new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(onSquare), new Vector3(0.5f),
                Graphics.TextureEffectType.Flat, SpriteName.cmd1Honor, Color.White);
                
            model.Y = model.ScaleY * 0.2f;
           // model.Rotation = cmdLib.FirstPlayerView() ? cmdLib.UnitPlaneRotationP1 : cmdLib.UnitPlaneRotationP2;
            model.Opacity = 0f;
        }

        public override void Time_Update(float time)
        {
            switch (state_Fadein_Hold_Fadeout)
            { 
                case 0://fade in
                    model.Opacity += time * FadeSpeed;
                    model.Y += MoveSpeed * time;
                    break;
                case 1:
                    model.Y += MoveSpeed * 0.1f * time;
                    break;
                case 2://fade out
                    model.Opacity -= time * FadeSpeed;
                    model.Y += MoveSpeed * time;
                    break;
                case 3://End
                    DeleteMe();
                    
                    return;

            }

            if (stateTime.CountDown())
            {
                state_Fadein_Hold_Fadeout++;

                switch (state_Fadein_Hold_Fadeout)
                {
                    case 0://fade in
                        stateTime.MilliSeconds = FadeTime;
                        break;
                    case 2://fade out
                        stateTime.MilliSeconds = FadeTime;
                        break;
                    case 1://Hold
                        stateTime.MilliSeconds = 1200f;
                        return;
                }
            }

        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
