using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{

    

    class FactionPixelTexture : AbsMapPixelTexture
    {
        
        public FactionPixelTexture(bool init)
            :base()
        {
            //initTexture();
            

            if (init)
            {
                refreshScale();
                

                if (DssRef.settings.playType == GameState.PlayStateType.Play ||
                    DssRef.settings.playType == GameState.PlayStateType.MapEditor)
                {
                    RefreshWorld_FactionCol();
                }
                else
                {
                    RefreshWorld_TerrainCol();
                }
            }
        }

        

        public void RefreshWorld_FactionCol()
        {
            refreshArea_FactionCol(DssRef.world.tileBounds);
        }
        

        void refreshArea_FactionCol(Rectangle2 area)
        {
            Tile t;

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                t = DssRef.world.tileGrid.Get(loop.Position);
                texture.SetPixel(loop.Position, t.MinimapColor_Faction(loop.Position));
            }

            texture.ApplyPixelsToTexture();
        }

        public void RefreshWorld_TerrainCol()
        {
            refreshArea_TerrainCol(DssRef.world.tileBounds);
        }


        void refreshArea_TerrainCol(Rectangle2 area)
        {
            Tile t;

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                t = DssRef.world.tileGrid.Get(loop.Position);
                texture.SetPixel(loop.Position, t.MinimapColor_Terrain(loop.Position));
            }

            texture.ApplyPixelsToTexture();
        }
        public void SetNewTexture()
        {
            texture.ApplyPixelsToTexture();
        }

        Graphics.Motion3d fadeMotion;
        void fadeIn(Vector3 dir)
        {
            if (fadeMotion != null && !fadeMotion.IsDeleted)
                fadeMotion.DeleteMe();
            fadeMotion = new Graphics.Motion3d(Graphics.MotionType.OPACITY,
                model, dir, Graphics.MotionRepeate.NO_REPEAT, 100, true);
        }


    }
}
