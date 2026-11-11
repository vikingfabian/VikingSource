using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Data;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.Display.GameDisplay
{
    class DamageDisplay : AbsUpdateable
    {
        Time lifeTime = new Time(1.2f, TimeUnit.Seconds);
        Text2 damageText;
        AbsGameObject gameObject;
        Vector2 offset;

        public DamageDisplay(AbsGameObject gameObject, int damage)
            : base(true)
        {
            this.gameObject = gameObject;

            damageText = new Text2(TextLib.PlusMinus(-damage), LoadedFont.Bold,
                Vector2.Zero, Engine.Screen.IconSize * 1.6f * WandSettings.GameInterfaceScale, Color.Red, ImageLayers.Background1_Front);
            damageText.OrigoAtCenter();

            offset.Y = -Engine.Screen.IconSize * 0.5f;
        }
        public override void Time_Update(float time_ms)
        {
            
            offset.Y -= Ref.DeltaTimeSec * Engine.Screen.IconSize * 1.6f;
            damageText.position = Ref.draw.Camera.From3DToScreenPos(gameObject.model.position, Engine.Draw.defaultViewport) + offset;

            if (lifeTime.CountDown())
            {
                DeleteMe();
            }
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
            damageText.DeleteMe();
        }
    }
}
