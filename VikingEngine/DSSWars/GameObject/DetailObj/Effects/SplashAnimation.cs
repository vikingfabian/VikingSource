using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars
{
    class SplashAnimation : AbsInGameUpdateable
    {
        const float SpeedL = 12f;

        SlashAnimMember[] models;
        float lengthLeft;

        public SplashAnimation(Vector3 position, float radius)
            : base(true)
        {
            lengthLeft = radius;

            position.Y = 0.2f;
            Vector3 scale = new Vector3(0.26f);
            //var tex = new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.OrangeRed);
            models = new SlashAnimMember[32];

            Rotation1D dir = Rotation1D.D0;
            for (int i = 0; i < models.Length; ++i)
            {
                var p = new SlashAnimMember(position, scale);
                p.Rotation = WP.ToQuaterion(dir.Radians);
                models[i] = p;
                p.v.Set(dir, SpeedL);
                dir.Add(MathHelper.TwoPi / models.Length);
            }
        }

        public override void Time_Update(float time_ms)
        {
            lengthLeft -= SpeedL * Ref.DeltaGameTimeSec;

            foreach (var m in models)
            {
                m.Position += m.v.Value * Ref.DeltaGameTimeSec;
            }

            if (lengthLeft <= 0)
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            foreach (var m in models)
            {
                m.DeleteMe();
            }
        }

        class SlashAnimMember : Graphics.Mesh
        {
            public Velocity v;

            public SlashAnimMember(Vector3 pos, Vector3 scale)
                : base(LoadedMesh.cube_repeating, pos, scale,  Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.Orange)
            {

            }
        }
    }
}
