using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.GameObject
{
    class ResourceEffect : AbsUpdateable
    {
        const float MoveTime = 600;
        const float ViewTime = 600;

        //static List<PolygonColor> polygons = new List<PolygonColor>(4);

        float stateTime = MoveTime;
        bool moveState = true;

        Graphics.RenderTargetBillboard model_old;
        //Graphics.VoxelModel model;
        public ResourceEffect(ItemResourceType item, int add, Vector3 pos, ResourceEffectType type)
            :base(true)
        {
            //Vector3 polyPosition = Vector3.Zero;
            //polygons.Clear();

            string text;
            Color textCol;
            //if (add == 0)
            //{ 
            //    lib.DoNothing();
            //}
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

            model_old = new Graphics.RenderTargetBillboard(pos,
                DssConst.Men_StandardModelScale * 1.5f, false);
            model_old.AddToRender(DrawGame.UnitDetailLayer);
            model_old.FaceCamera = false;
            model_old.Rotation = toggLib.PlaneTowardsCam;
            model_old.images = new List<Graphics.AbsDraw> { value, img };

            if (type == ResourceEffectType.Deliver)
            {
                Graphics.Image deliverIcon = new Graphics.Image(SpriteName.WarsDelivery, Vector2.Zero, img.size,
                    ImageLayers.Lay0, false, false);
                value.Xpos += deliverIcon.Width;
                img.Xpos += deliverIcon.Width;
                sz.X +=deliverIcon.Width;

                model_old.images.Add(deliverIcon);
            }
            
            model_old.createTexture(sz, model_old.images, null);
            model_old.setModelSizeFromTexWidth();

            //void addPolygon(SpriteName spriteName, Color color)
            //{
            //    var poly = toggLib.CamFacingPolygon(
            //        polyPosition,
            //        new Vector2(DssConst.Men_StandardModelScale * 1.1f),
            //        DataLib.SpriteCollection.Get(spriteName),
            //        color);
            //    polyPosition.X += DssConst.Men_StandardModelScale * 0.8f;

            //}
            
        }
        public override void Time_Update(float time_ms)
        {
            stateTime -= time_ms;

            if (moveState)
            {
                model_old.Y += time_ms * 0.0001f;
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
            model_old.DeleteMe();
        }
    }

    enum ResourceEffectType
    { 
        Add,
        Deliver,
    }
}
