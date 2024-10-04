using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
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

        public ResourceEffect(ItemResourceType item, int add, Vector3 pos)
            :base(true)
        {
            //const int TexWidth = 32;
            Graphics.TextG value = new Graphics.TextG(LoadedFont.Bold, Vector2.Zero, Vector2.One, Graphics.Align.Zero,
                TextLib.PlusMinus(add), HudLib.AvailableColor, ImageLayers.Lay0, false);
            Vector2 sz = value.MeasureText();

            Graphics.Image img = new Graphics.Image(ResourceLib.Icon(item), new Vector2( sz.X, 0), new Vector2(sz.Y),
                ImageLayers.Lay0, false, false);

            sz.X += sz.Y;

            model = new Graphics.RenderTargetBillboard(pos,
                DssConst.Men_StandardModelScale * 1f, false);
            model.AddToRender(DrawGame.UnitDetailLayer);
            model.FaceCamera = false;
            model.Rotation = toggLib.PlaneTowardsCam;
            model.images = new List<Graphics.AbsDraw> { value, img };

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
}
