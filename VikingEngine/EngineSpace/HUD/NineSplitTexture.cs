using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    struct NineSplitSettings
    {
        public SpriteName baseTexture;
        
        public SpriteName disableTexture;
        public SpriteName notSelectedTexture;//maybe add primary, secondary
        
        /// <summary>
        /// Edge space on the spritesheet
        /// </summary>
        public int textureEdgeInsert;
        
        public int cornerTexSize;
        public float edgeScale; 
        public bool useTextureAsEdgeSz;
        public bool addCenterImage;

        public NineSplitSettings(SpriteName baseTexture, int textureEdgeInsert, int cornerTexSize,
            float edgeScale, bool useTextureAsEdgeSz, bool addCenterImage)
        {
            this.baseTexture = baseTexture;
            this.disableTexture = baseTexture;
            this.notSelectedTexture = baseTexture;

            this.textureEdgeInsert = textureEdgeInsert;
            this.cornerTexSize = cornerTexSize;
            this.edgeScale = edgeScale;
            this.useTextureAsEdgeSz = useTextureAsEdgeSz;
            this.addCenterImage = addCenterImage;
        }

        public NineSplitSettings Enabled(bool enabled)
        {
            var result = this;
            if (!enabled)
            {
                result.baseTexture = disableTexture;
            }

            return result;
        }

        public NineSplitSettings Selected(bool selected)
        {
            var result = this;
            if (!selected)
            {
                result.baseTexture = notSelectedTexture;
            }

            return result;
        }
    }

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
        { }

        public NineSplitAreaTexture(NineSplitSettings settings, VectorRect area, ImageLayers layer, bool addToRender = true)
        {
            area.Round();
            this.area = area;
            this.layer = layer;

            Sprite file = DataLib.SpriteCollection.Get(settings.baseTexture);//DataLib.SpriteCollection.imageFiles[DataLib.SpriteCollection.imagesNames[baseTexture]];
            file.Source.X += settings.textureEdgeInsert;
            file.Source.Y += settings.textureEdgeInsert;
            file.Source.Width -= settings.textureEdgeInsert * 2;
            file.Source.Height -= settings.textureEdgeInsert * 2;
            
            int centerTexSize = file.Source.Width - settings.cornerTexSize * 2;

            Rectangle nw = file.Source;
            nw.Width = settings.cornerTexSize;
            nw.Height = settings.cornerTexSize;

            Rectangle n = nw;
            n.X += nw.Width;
            n.Width = centerTexSize;

            Rectangle ne = nw;
            ne.X = n.Right;

            Rectangle w = nw;
            w.Y += settings.cornerTexSize;
            w.Height = centerTexSize;

            Rectangle center = w;
            center.X += settings.cornerTexSize;
            center.Width = centerTexSize;

            Rectangle e = w;
            e.X = center.Right;

            Rectangle sw = nw;
            sw.Y = w.Bottom;

            Rectangle s = n;
            s.Y = sw.Y;

            Rectangle se = ne;
            se.Y = sw.Y;


            images = new List<Image>(16);
            Vector2 cornerSize;

            if (settings.useTextureAsEdgeSz)
            {
                 cornerSize= new Vector2((int)(settings.cornerTexSize * settings.edgeScale));
            }
            else
            {
                cornerSize = new Vector2(settings.edgeScale);
            }
            Vector2 centerSize = area.Size - cornerSize * 2f;

            Graphics.ImageAdvanced nwImg = new ImageAdvanced(settings.baseTexture, area.Position, cornerSize,
                layer -1, false, addToRender);
            nwImg.ImageSource = nw;

            Graphics.ImageAdvanced nImg = new ImageAdvanced(settings.baseTexture, 
                VectorExt.AddX(area.Position, cornerSize.X), new Vector2(centerSize.X, cornerSize.Y), 
                layer+1, false, addToRender);
            nImg.ImageSource = n;

            Graphics.ImageAdvanced neImg = new ImageAdvanced(settings.baseTexture, new Vector2(nImg.Right, area.Position.Y), 
                cornerSize, layer - 1, false, addToRender);
            neImg.ImageSource = ne;

            images.Add(nwImg); images.Add(nImg); images.Add(neImg);

            Graphics.ImageAdvanced wImg = new ImageAdvanced(settings.baseTexture, VectorExt.AddY(area.Position, cornerSize.Y), 
                new Vector2(cornerSize.X, centerSize.Y), layer, false, addToRender);
            wImg.ImageSource = w;

            if (settings.addCenterImage)
            {
                Graphics.ImageAdvanced cImg = new ImageAdvanced(settings.baseTexture, area.Position + cornerSize, 
                    centerSize, layer, false, addToRender);
                cImg.ChangePaintLayer(+1);
                cImg.ImageSource = center;
                images.Add(cImg);
            }

            Graphics.ImageAdvanced eImg = new ImageAdvanced(settings.baseTexture, new Vector2(area.X + cornerSize.X + centerSize.X,
                area.Y + cornerSize.Y), new Vector2(cornerSize.X, centerSize.Y), layer, false, addToRender);
            eImg.ImageSource = e;

            images.Add(wImg); images.Add(eImg);

            Graphics.ImageAdvanced swImg = new ImageAdvanced(settings.baseTexture, 
                new Vector2(area.X, area.Y + cornerSize.Y + centerSize.Y), cornerSize, layer - 1, false, addToRender);
            swImg.ImageSource = sw;

            Graphics.ImageAdvanced sImg = new ImageAdvanced(settings.baseTexture, new Vector2(nImg.Xpos, swImg.Ypos), 
                nImg.Size, layer + 1, false, addToRender);
            sImg.ImageSource = s;

            Graphics.ImageAdvanced seImg = new ImageAdvanced(settings.baseTexture, new Vector2(neImg.Xpos, swImg.Ypos), cornerSize,
                layer - 1, false, addToRender);
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

        public void SetColor(Color color)
        {
            foreach (var m in images)
            {
                m.Color = color;
            }
        }

        public VectorRect GetAreaAdjusted()
        {
            var result = area;
            area.Position = images[0].position;
            return result;
        }
    }
}
