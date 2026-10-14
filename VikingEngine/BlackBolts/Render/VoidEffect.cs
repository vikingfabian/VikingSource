using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.Render
{
    class VoidEffect :AbsUpdateable
    {
        GameTimeStamp timeStamp;
        Mesh plane;
        public VoidEffect(IntVector2 tilePos)
            :base(true)
        {
            timeStamp.setNow();
            plane = new Mesh(LoadedMesh.plane, WP.TileToWp(tilePos), new Microsoft.Xna.Framework.Vector3(1f),
                 TextureEffectType.Flat, SpriteName.bb_voidWarp, Color.White);
            plane.Y = 0.3f;
        }

        public override void Time_Update(float time_ms)
        {
            plane.Rotation.RotateWorldX(0.005f * Ref.DeltaGameTimeMs);

            if (timeStamp.secPassed(0.8f))
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            plane.DeleteMe();
        }
    }
}
