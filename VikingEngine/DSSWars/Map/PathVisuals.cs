using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.Path;

namespace VikingEngine.DSSWars.Map
{
    class PathVisuals
    {
        Graphics.ImageGroup moveDots = new Graphics.ImageGroup(64);
        int playerIndex;
        public PathVisuals(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public void refresh(WalkingPath path, bool attack, bool hover)
        {

            Color color = attack ? Color.Pink : Color.White;
            float opacity = hover ? 0.5f : 1f;

            moveDots.DeleteAll();

            if (path != null)
            {
                for (int i = path.currentNodeIx; i >= 0; --i)
                {
                    Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.SelectCircleThick,
                        WP.ToMapPos(path.nodes[i].position),
                        new Vector3(0.2f), Graphics.TextureEffectType.Flat,
                        SpriteName.WhiteArea, color, false);
                    dot.Opacity = opacity;
                    dot.AddToRender(DrawGame.TerrainLayer);
                    dot.setVisibleCamera(playerIndex);
                    moveDots.Add(dot);
                }
            }
        }

        public void refresh(DetailWalkingPath path, /*bool attack,*/ bool hover)
        {

            Color color = /*attack ? Color.Pink :*/ Color.White;
            float opacity = hover ? 0.5f : 1f;

            moveDots.DeleteAll();

            if (path != null)
            {
                for (int i = path.currentNodeIx; i >= 0; --i)
                {
                    Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.SelectCircleThick,
                        WP.ToMapPos(path.nodes[i].position),
                        new Vector3(0.02f), Graphics.TextureEffectType.Flat,
                        SpriteName.WhiteArea, color, false);
                    dot.Opacity = opacity;
                    dot.AddToRender(DrawGame.UnitDetailLayer);
                    dot.setVisibleCamera(playerIndex);
                    moveDots.Add(dot);
                }
            }
        }

        public void SetVisible(bool visible)
        {
            moveDots.SetVisible(visible);
        }

        public void DeleteMe()
        {
            moveDots.DeleteAll();
        }

        public void addTo(Graphics.ImageGroup images)
        {
            images.Add(moveDots);
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
