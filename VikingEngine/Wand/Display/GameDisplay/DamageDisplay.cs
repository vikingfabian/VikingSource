using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Data;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Graphics;
using VikingEngine.ToGG;

namespace VikingEngine.Core.Wand.Display.GameDisplay
{
    abstract class AbsValueChangeDisplay : AbsUpdateable
    {
        Time lifeTime = new Time(1.2f, TimeUnit.Seconds);
        Text2 damageText;
        AbsGameObject gameObject;
        Vector2 offset;

        public AbsValueChangeDisplay(AbsGameObject gameObject)
            : base(true)
        {
            this.gameObject = gameObject;

            offset.Y = -Engine.Screen.IconSize * 0.5f;
        }

        protected void CreateText(string text, Color color)
        {
            damageText = new Text2(text, LoadedFont.Bold,
                Vector2.Zero, Engine.Screen.IconSize * 1.6f * WandSettings.GameInterfaceScale, color, ImageLayers.Background1_Front);
            damageText.OrigoAtCenter();
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

    class DamageDisplay : AbsValueChangeDisplay
    {
        public DamageDisplay(AbsGameObject gameObject, int damage)
            : base(gameObject)
        {
            CreateText(TextLib.PlusMinus(-damage), Color.Red);
        }
    }

    class HealDisplay : AbsValueChangeDisplay
    { 
        public HealDisplay(AbsGameObject gameObject, int add)
            : base(gameObject)
        {
            CreateText(TextLib.PlusMinus(add), Color.LightGreen);
        }
    }

    class LevelUpDisplay : AbsValueChangeDisplay
    {
        public LevelUpDisplay(AbsGameObject gameObject, int add)
            : base(gameObject)
        {
            CreateText("UP" + TextLib.PlusMinus(add), Color.White);
        }
    }
}
