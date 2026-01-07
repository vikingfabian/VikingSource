using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class PathVisuals
    {
        List<Mesh_MultiLayer> moveDots = new List<Mesh_MultiLayer>(32);
        int playerIndex;
        bool midLayer = false;
        public PathVisuals(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public void refresh(WalkingPath path, bool attack, bool hover)
        {

            Color color = attack ? Color.Pink : Color.White;
            float opacity = hover ? 0.5f : 1f;

            DeleteMe();

            if (path != null)
            {
                midLayer = true;
                for (int i = path.currentNodeIx; i >= 0; --i)
                {
                    Graphics.Mesh_MultiLayer dot = new Graphics.Mesh_MultiLayer(LoadedMesh.SelectCircleThick,
                        WP.ToMapPos(path.nodes[i].position),
                        new Vector3(0.2f), Graphics.TextureEffectType.Flat,
                        SpriteName.WhiteArea, color);
                    dot.Opacity = opacity;
                    //dot.AddToRender(DrawGame.TerrainLayer);
                    //dot.AddToRender(DrawGame.UnitDetailLayer);
                    dot.AddToLayer1(DrawGame.UnitDetailLayer);
                    dot.AddToLayer2(DrawGame.MidLayer);
                    dot.setVisibleCamera(playerIndex);
                    moveDots.Add(dot);
                }
            }
        }

        public void refresh(DetailWalkingPath path, /*bool attack,*/ bool hover)
        {

            Color color = /*attack ? Color.Pink :*/ Color.White;
            float opacity = hover ? 0.5f : 1f;

            DeleteMe();
            //moveDots.DeleteAll();

            if (path != null)
            {
                midLayer = false;
                for (int i = path.currentNodeIx; i >= 0; --i)
                {
                    Graphics.Mesh_MultiLayer dot = new Graphics.Mesh_MultiLayer(LoadedMesh.SelectCircleThick,
                        WP.SubtileToWorldPosXZgroundY_Centered(path.nodes[i].position),
                        new Vector3(0.02f), Graphics.TextureEffectType.Flat,
                        SpriteName.WhiteArea, color);
                    dot.Opacity = opacity;
                    dot.AddToLayer1(DrawGame.UnitDetailLayer);
                    dot.setVisibleCamera(playerIndex);
                    moveDots.Add(dot);
                }
            }
        }

        public void SetVisible(bool visible)
        {
            foreach (var m in moveDots)
            { 
                m.Visible = visible;
            }
            //moveDots.SetVisible(visible);
        }

        public void DeleteMe()
        {
            //foreach (var dot in moveDots.images)
            //{
            //    Ref.draw.AddToRenderList(dot, false, DrawGame.UnitDetailLayer, );
            //    if (midLayer)
            //    {
            //        Ref.draw.AddToRenderList(dot, false, DrawGame.TerrainLayer);
            //    }
            //}
            foreach (var m in moveDots)
            {
                m.DeleteMe();
            }
            moveDots.Clear();
        }



        public void addTo(Graphics.ImageGroup images)
        {
            foreach (var m in moveDots)
            {
                images.Add(m);
            }
            
        }
    }

    class PathFlashEffect : AbsUpdateable
    {
        int flashCount = 2;
        bool visible = false;
        PathVisuals path;
        Timer.Basic timer = new Timer.Basic(120, true);

        public PathFlashEffect(PathVisuals path)
            :base(true)
        {
            this.path = path;
            path.SetVisible(visible);
        }

        public override void Time_Update(float time_ms)
        {
            if (timer.Update())
            {
                lib.Invert(ref visible);
                path.SetVisible(visible);

                if (!visible)
                {
                    if (--flashCount <= 0)
                    {
                        path.DeleteMe();
                        this.DeleteMe();
                    }
                }
            }
        }
    }
}
