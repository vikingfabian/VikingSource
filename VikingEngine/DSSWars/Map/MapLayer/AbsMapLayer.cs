using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map.MapLayer
{
    abstract class AbsMapLayer : Point3D
    {
        Timer.Basic waterAnimTimer = new Timer.Basic(3000, true);
        int waterFrame = 0;
        //int waterEdgeFrame = 0;
        double waterMoveCurve = 0;

        protected Graphics.Mesh waterSurface, waterBottom;
        protected void WaterModel(bool detailLayer)
        {
            //Graphics.Mesh waterBottom;

            var vol = WaterModelVolume();

            waterBottom = new Mesh(LoadedMesh.plane, vol.Position, new Vector3(1f),
                TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.DarkBlue, false);
            waterBottom.Y -= 0.6f;
            waterBottom.Scale = vol.Scale;

            waterSurface = new Mesh(LoadedMesh.plane, vol.Position, new Vector3(1f),
                TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.White,
                false);

            //if (highDetail)
            //{
            waterSurface.texture = WaterTex()[0];
            int repeatCount = detailLayer ? 2 : 1;
            waterSurface.repeatingTextureSource(WaterTex()[1], DssRef.world.Size * repeatCount);
            //}
            //else
            //{
            //    waterSurface.effectType = TextureEffectType.SeaNoise;

            //    waterSurface.Color = WorldData.WaterCol;//new Color(14, 155, 246);
            //    //new Color(4.3f, 48.6f,77.3f);
            //}
            waterSurface.Scale = vol.Scale;
            const float SurfaceTrans = 0.8f;
            waterSurface.Opacity = SurfaceTrans;

            int drawLayer = detailLayer ? DrawGame.UnitDetailLayer : DrawGame.MidLayer;
            //if (!detailLayer)
            {
                waterSurface.AddToRender(drawLayer);
            }
            waterBottom.AddToRender(drawLayer);
            //waterSurface.Visible = true;
        }

        abstract protected Texture2D[] WaterTex();

        public static VectorVolume WaterModelVolume()
        {
            Vector3 surfacePos = new Vector3(DssRef.world.unitSize.X * 0.5f - 0.5f, MapHeight2.WaterSurfaceY, DssRef.world.unitSize.Y * 0.5f - 0.5f);
            Vector3 waterScale = new Vector3(DssRef.world.unitSize.X, 1f, DssRef.world.unitSize.Y);

            return new VectorVolume(surfacePos, waterScale);
        }

        protected void updateWaterTexture()
        {
            if (waterAnimTimer.Update(Ref.DeltaGameTimeMs))
            {
                if (++waterFrame >= WaterTex().Length)
                {
                    waterFrame = 0;
                }

                //if (++waterEdgeFrame >= DssRef.models.waterEdgeTextures.Length)
                //{ 
                //    waterEdgeFrame = 0;
                //}

                waterSurface.texture = WaterTex()[waterFrame];
            }

            waterMoveCurve += Ref.DeltaGameTimeSec * 0.5f;
            waterSurface.TextureSource.SourceF.X += Ref.DeltaGameTimeSec * -0.05f;
            waterSurface.TextureSource.SourceF.Y = (float)(Math.Sin(waterMoveCurve) * 0.1);
        }

        //public Texture2D waterEdgeTex()
        //{
        //    return DssRef.models.waterEdgeTextures[waterEdgeFrame];
        //}

        #region DRAW

        public override DrawObjType DrawType
        {
            get { return DrawObjType.MeshGenerated; }
        }
        public override void copyAllDataFrom(Graphics.AbsDraw clone)
        {
            throw new NotImplementedException();
        }
        public override Graphics.AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        public override Color Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override float Opacity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void UpdateCulling()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
