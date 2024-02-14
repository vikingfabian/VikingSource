using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2
{
    class HeightCheck : Timer.AbsTimer
    {
        Graphics.Mesh model;
        public HeightCheck(Map.WorldPosition wp)
            :base(lib.MinutesToMS(0.5f), UpdateType.Lasy)
        {
            Vector3 pos = wp.ToV3();
            pos.Y = wp.GetGroundY();//SetFromGroundY(0);
            model = new Graphics.Mesh(LoadedMesh.sphere, pos, new Graphics.TextureEffect(Graphics.TextureEffectType.LambertFixed, SpriteName.WhiteArea), 1);
        }
        protected override void timeTrigger()
        {
            base.timeTrigger();
            this.DeleteMe();
            model.DeleteMe();
        }
    }
}
