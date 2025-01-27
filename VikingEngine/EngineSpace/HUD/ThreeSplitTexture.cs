﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.ToGG;

namespace VikingEngine.HUD
{
    struct ThreeSplitSettings
    {
        public SpriteName baseTexture;
        public SpriteName disableTexture;
        
        /// <summary>
        /// Edge space on the spritesheet
        /// </summary>
        public int textureEdgeInsert;

        /// <summary>
        /// From left to right on horizontal
        /// </summary>
        public int sideTexSize;

        public ThreeSplitSettings(SpriteName baseTexture, int textureEdgeInsert, int sideTexSize)
        { 
            this.baseTexture = baseTexture;
            disableTexture = baseTexture;
            this.textureEdgeInsert = textureEdgeInsert;
            this.sideTexSize = sideTexSize;
        }

        public float TotalSideLeght()
        { 
            return sideTexSize;
        }

    }

    class ThreeSplitTexture_Hori
    {
        public List<Graphics.Image> images;
        VectorRect area;
        ImageLayers layer;

        public ThreeSplitTexture_Hori(ThreeSplitSettings settings, VectorRect area, ImageLayers layer, bool addToRender = true)
        {
            this.layer = layer;
            area.Round();
            this.area = area;

            Sprite file = DataLib.SpriteCollection.Get(settings.baseTexture);
            file.Source.X += settings.textureEdgeInsert;
            file.Source.Y += settings.textureEdgeInsert;
            file.Source.Width -= settings.textureEdgeInsert * 2;
            file.Source.Height -= settings.textureEdgeInsert * 2;

            int centerTexSize = file.Source.Width - settings.sideTexSize * 2;

            Rectangle left_top = file.Source;
            left_top.Width = settings.sideTexSize;

            Rectangle center = left_top;
            center.X += settings.sideTexSize;
            center.Width = centerTexSize;

            Rectangle right_bottom = left_top;
            right_bottom.X += settings.sideTexSize + centerTexSize;

            Vector2 sideSize = new Vector2(0, area.Size.Y);
            sideSize.X = sideSize.Y / left_top.Size.Y * left_top.Size.X;

            
            Graphics.ImageAdvanced left_topImg = new ImageAdvanced(settings.baseTexture, area.Position, sideSize,
                layer, false, addToRender);
            left_topImg.ImageSource = left_top;

            Vector2 centerSize = area.Size;
            centerSize.X -= sideSize.X * 2;
            Graphics.ImageAdvanced centerImg = new ImageAdvanced(settings.baseTexture, area.Position, centerSize,
                layer -1, false, addToRender);
            centerImg.position.X += sideSize.X;
            centerImg.ImageSource = center;

            Graphics.ImageAdvanced right_bottomImg = new ImageAdvanced(settings.baseTexture, area.Position, sideSize,
                layer, false, addToRender);
            right_bottomImg.position.X += sideSize.X + centerSize.X;
            right_bottomImg.ImageSource = right_bottom;

            images = new List<Image>()
            {
                left_topImg, centerImg, right_bottomImg
            };
        }

        public VectorRect Area()
        {
            area.Position = images[0].position;
            return area;
        }
        //public void refresh()
        //{ 
            
        //}

    }
}
