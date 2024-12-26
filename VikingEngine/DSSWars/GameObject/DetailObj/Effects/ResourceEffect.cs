using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.GameObject
{
    class ResourceEffect : AbsUpdateable
    {
        const float MoveTime = 600;
        const float ViewTime = 600;

        float stateTime = MoveTime;
        bool moveState = true;

        Graphics.RenderTargetBillboard model;

        public ResourceEffect(ItemResourceType item, int add, Vector3 pos, ResourceEffectType type)
            :base(true)
        {
            string text;
            Color textCol;
            if (add == 0)
            { 
            lib.DoNothing();
                }
            if (type == ResourceEffectType.Add)
            {
                text = TextLib.PlusMinus(add);
                textCol = add > 0 ? HudLib.AvailableColor: HudLib.NotAvailableColor;
            }
            else
            {
                text = add.ToString();
                textCol = Color.White;
            }

            Graphics.TextG value = new Graphics.TextG(LoadedFont.Bold, Vector2.Zero, Vector2.One, Graphics.Align.Zero,
                text, textCol, ImageLayers.Lay0, false);
            Vector2 sz = value.MeasureText();

            Graphics.Image img = new Graphics.Image(ResourceLib.Icon(item), new Vector2( sz.X, 0), new Vector2(sz.Y),
                ImageLayers.Lay0, false, false);

            sz.X += sz.Y;

            model = new Graphics.RenderTargetBillboard(pos,
                DssConst.Men_StandardModelScale * 1.5f, false);
            model.AddToRender(DrawGame.UnitDetailLayer);
            model.FaceCamera = false;
            model.Rotation = toggLib.PlaneTowardsCam;
            model.images = new List<Graphics.AbsDraw> { value, img };

            if (type == ResourceEffectType.Deliver)
            {
                Graphics.Image deliverIcon = new Graphics.Image(SpriteName.WarsDelivery, Vector2.Zero, img.size,
                    ImageLayers.Lay0, false, false);
                value.Xpos += deliverIcon.Width;
                img.Xpos += deliverIcon.Width;
                sz.X +=deliverIcon.Width;

                model.images.Add(deliverIcon);
            }
            
            model.createTexture(sz, model.images, null);
            model.setModelSizeFromTexWidth();

            
        }
        public override void Time_Update(float time_ms)
        {
            stateTime -= time_ms;

            if (moveState)
            {
                model.Y += time_ms * 0.0001f;
                if (stateTime <= 0)
                {
                    stateTime = ViewTime;
                    moveState = false;
                }
            }
            else if (stateTime <= 0)
            {
                DeleteMe();
            }

        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }

    enum ResourceEffectType
    { 
        Add,
        Deliver,
    }
}
