using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    /// <summary>
    /// Takes an image, splits it in nine pieces, corners, edges, center - to create any area of that style
    /// </summary>
    class NineSplitAreaTexture
    {
        public List<Graphics.Image> images;
        VectorRect area;
        ImageLayers layer;

        public NineSplitAreaTexture(SpriteName baseTexture, int textureEdgeInsert, int cornerTexSize, VectorRect area, 
            float edgeScale, bool useTextureAsEdgeSz, ImageLayers layer, bool addCenterImage, bool addToRender = true)
        {
            this.area = area;
            this.layer = layer;

            Sprite file = DataLib.SpriteCollection.Get(baseTexture);//DataLib.SpriteCollection.imageFiles[DataLib.SpriteCollection.imagesNames[baseTexture]];
            file.Source.X += textureEdgeInsert;
            file.Source.Y += textureEdgeInsert;
            file.Source.Width -= textureEdgeInsert * 2;
            file.Source.Height -= textureEdgeInsert * 2;
            
            int centerTexSize = file.Source.Width - cornerTexSize * 2;

            Rectangle nw = file.Source;
            nw.Width = cornerTexSize;
            nw.Height = cornerTexSize;

            Rectangle n = nw;
            n.X += nw.Width;
            n.Width = centerTexSize;

            Rectangle ne = nw;
            ne.X = n.Right;

            Rectangle w = nw;
            w.Y += cornerTexSize;
            w.Height = centerTexSize;

            Rectangle center = w;
            center.X += cornerTexSize;
            center.Width = centerTexSize;

            Rectangle e = w;
            e.X = center.Right;

            Rectangle sw = nw;
            sw.Y = w.Bottom;

            Rectangle s = n;
            s.Y = sw.Y;

            Rectangle se = ne;
            se.Y = sw.Y;


            images = new List<Image>(32);
            Vector2 cornerSize;

            if (useTextureAsEdgeSz)
            {
                 cornerSize= new Vector2((int)(cornerTexSize * edgeScale));
            }
            else
            {
                cornerSize = new Vector2(edgeScale);
            }
            Vector2 centerSize = area.Size - cornerSize * 2f;

            Graphics.ImageAdvanced nwImg = new ImageAdvanced(baseTexture, area.Position, cornerSize, 
                layer, false, addToRender);
            nwImg.ImageSource = nw;

            Graphics.ImageAdvanced nImg = new ImageAdvanced(baseTexture, 
                VectorExt.AddX(area.Position, cornerSize.X), new Vector2(centerSize.X, cornerSize.Y), 
                layer+1, false, addToRender);
            nImg.ImageSource = n;

            Graphics.ImageAdvanced neImg = new ImageAdvanced(baseTexture, new Vector2(nImg.Right, area.Position.Y), 
                cornerSize, layer, false, addToRender);
            neImg.ImageSource = ne;

            images.Add(nwImg); images.Add(nImg); images.Add(neImg);

            Graphics.ImageAdvanced wImg = new ImageAdvanced(baseTexture, VectorExt.AddY(area.Position, cornerSize.Y), 
                new Vector2(cornerSize.X, centerSize.Y), layer, false, addToRender);
            wImg.ImageSource = w;

            if (addCenterImage)
            {
                Graphics.ImageAdvanced cImg = new ImageAdvanced(baseTexture, area.Position + cornerSize, 
                    centerSize, layer, false, addToRender);
                cImg.ChangePaintLayer(+1);
                cImg.ImageSource = center;
                images.Add(cImg);
            }

            Graphics.ImageAdvanced eImg = new ImageAdvanced(baseTexture, new Vector2(area.X + cornerSize.X + centerSize.X,
                area.Y + cornerSize.Y), new Vector2(cornerSize.X, centerSize.Y), layer, false, addToRender);
            eImg.ImageSource = e;

            images.Add(wImg); images.Add(eImg);

            Graphics.ImageAdvanced swImg = new ImageAdvanced(baseTexture, 
                new Vector2(area.X, area.Y + cornerSize.Y + centerSize.Y), cornerSize, layer, false, addToRender);
            swImg.ImageSource = sw;

            Graphics.ImageAdvanced sImg = new ImageAdvanced(baseTexture, new Vector2(nImg.Xpos, swImg.Ypos), 
                nImg.Size, layer + 1, false, addToRender);
            sImg.ImageSource = s;

            Graphics.ImageAdvanced seImg = new ImageAdvanced(baseTexture, new Vector2(neImg.Xpos, swImg.Ypos), cornerSize, 
                layer, false, addToRender);
            seImg.ImageSource = se;

            images.Add(swImg); images.Add(sImg); images.Add(seImg);
        }

        public void addCenterColor(float distanceFromEdge, Color col)
        {
            VectorRect center = area;
            center.AddRadius(-distanceFromEdge);

            Graphics.Image bg = new Image(SpriteName.WhiteArea, center.Position, center.Size, layer + 1);
            bg.Color = col;
            images.Add(bg);
        }

        public void DeleteMe()
        {
            foreach (var m in images)
            {
                m.DeleteMe();
            }
        }

        public void SetVisible(bool visible)
        {
            foreach (var m in images)
            {
                m.Visible = visible;
            }
        }
    }
}
