using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2
{
    class MiniHealthBarShard : AbsUpdateable
    {
        Graphics.ImageAdvanced img;
        Graphics.Image bg;
        MiniHealthBar parent;
        Vector2 relPos;
        Vector2 speed = new Vector2(0.002f, -0.004f);
        Time lifeTime = new Time(900);
        const int BorderW = 0;
        Vector2 borderRelPos = new Vector2(-BorderW);

        public MiniHealthBarShard(int texureStart, int textureWidth, Color barCol, MiniHealthBar parent, Vector2 barPos)
            :base(true)
        {
            this.parent = parent;
            this.relPos = barPos - parent.Position;
            relPos.X += texureStart;

            img = new Graphics.ImageAdvanced(SpriteName.TextureHealthbarMedium, Vector2.Zero,
                new Vector2(textureWidth, MiniHealthBar.TextureHeight), ImageLayers.Background2, false);
            
            img.SourceX += texureStart;
            img.SourceWidth = textureWidth;
            img.Color = barCol;
            img.Transparentsy = 0.4f;

            bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, 
                img.Size + new Vector2(BorderW * 2), ImageLayers.Background3);

        }

        public override void Time_Update(float time)
        {
            parent.updatePosition();
            relPos += speed * time;
            img.Position = parent.Position + relPos;
            bg.Position = img.Position + borderRelPos;
            if (lifeTime.CountDown())
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            img.DeleteMe();
            bg.DeleteMe();
        }
    }
}
