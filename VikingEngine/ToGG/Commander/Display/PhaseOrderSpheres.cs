using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.Commander.Display
{
    /// <summary>
    /// List all phases in a list of round icons
    /// </summary>
    class PhaseOrderSpheres
    {
        const float FocusAnimTime = 200;

        Graphics.ImageGroupParent2D images;
        List<Sphere> spheres;
        Vector2 largeShpereSz, smallShpereSz;
        GamePhaseType prevFocus = GamePhaseType.NUM_NON;
        Graphics.ImageGroup2DMember selectionChild;
        public float width;

        Sphere mouseHover = null;
        Graphics.Image tooltipBg;
        Graphics.TextG tooltipText;

        public PhaseOrderSpheres()
        {
            images = new Graphics.ImageGroupParent2D();

            largeShpereSz = Engine.Screen.IconSizeV2 * 0.9f;
            smallShpereSz = largeShpereSz * 0.9f;

            float spacing = largeShpereSz.X * 1.2f;
            //float hspacing = spacing * 0.5f;
            float endSpacing = spacing * 0.42f;

            Vector2 startPos = VectorExt.V2FromX(endSpacing);
            Vector2 endPos = startPos;
            Vector2 pos = startPos;
            spheres = new List<Sphere>(toggRef.gamestate.gameSetup.activeTurnPhases.Count);

            for (int i = 0; i < toggRef.gamestate.gameSetup.activeTurnPhases.Count; ++i)
            {
                var s = new Sphere(i, pos, smallShpereSz, images, toggRef.gamestate.gameSetup.activeTurnPhases[i]);
                endPos = pos;
                pos.X += spacing;

                spheres.Add(s);
            }

            pos.X = pos.X - spacing + endSpacing;

            width = pos.X;

            Graphics.Line line = new Graphics.Line(Engine.Screen.IconSize * 0.2f, HudLib.BgLayer + 1, Color.White, startPos, endPos);
            images.Add(line);

            Graphics.Image selection = new Graphics.Image(SpriteName.cmdPhaseCirkle, Vector2.Zero, largeShpereSz * 1.2f, HudLib.BgLayer, true);
            selection.Visible = true;

            selectionChild = images.Add(selection);

            float Height; Vector2 cardSize; float non_spacing; float bgWidth;
            ToggEngine.Display2D.StrategyCardsHud.Scale(out Height, out cardSize, out non_spacing, out bgWidth);

            Vector2 spherePos = Engine.Screen.SafeArea.RightBottom;//nextPhaseButton.area.LeftBottom;
            spherePos.X -= bgWidth + HudLib.PhaseButtonsSpacing + spacing * 0.5f;
            spherePos.Y -= HudLib.PrevPhaseButtonsSz.Y * 0.5f;
            images.ParentPosition = VectorExt.AddX(spherePos, -width);
            //leftPos = phaseOrderSpheres.setRightPos(spherePos) - HudLib.PhaseButtonsSpacing;
        }

        //public float setRightPos(Vector2 right)
        //{
        //    images.ParentPosition = VectorExt.AddX(right, -width);
        //    return images.ParentX;
        //}

        public Vector2 GetPosition()
        {
            return images.ParentPosition;
        }

        public void setPhase(GamePhaseType phase)
        {
            if (prevFocus !=  phase)
            {
                Vector2 focusPos = Vector2.Zero;

                foreach (var m in spheres)
                {
                    if (m.phase == prevFocus)
                    {
                        m.setFocus(false, smallShpereSz, largeShpereSz);
                    }
                    else if (m.phase == phase)
                    {
                        focusPos = m.setFocus(true, smallShpereSz, largeShpereSz);
                    }
                }

                prevFocus = phase;

                //if (selection.Visible)
                //{
                //    moveSelection = (focusPos - selection.Position) / FocusAnimTime;
                //    animateTime.MilliSeconds = FocusAnimTime;
                //}
                //else
                //{
                selectionChild.image.Visible = true;
                selectionChild.SetScreenPosition(focusPos, images);
                //}
            }
        }

        public void Update(ref PhaseUpdateArgs args)
        {
            Sphere intersect = null;

            foreach (var m in spheres)
            {
                m.update();

                if (m.intersect(Input.Mouse.Position))
                {
                    intersect = m;
                    args.mouseOverHud = true;
                }
            }

            if (intersect != mouseHover)
            {
                removeTooltip();

                mouseHover = intersect;

                if (mouseHover != null)
                {
                    tooltipText = new Graphics.TextG(LoadedFont.Regular, Vector2.Zero, Engine.Screen.TextSizeV2,
                        Graphics.Align.Zero, 
                        "Phase" + TextLib.IndexToString(mouseHover.index) + ": " + 
                        AbsGamePhase.PhaseNameShort(mouseHover.phase), 
                        Color.White, HudLib.TooltipLayer);
                    VectorRect area = VectorRect.Zero;
                    area.Size = tooltipText.MeasureText() + Engine.Screen.IconSizeV2 * 0.2f;
                    area.RightBottom = mouseHover.toolTipStartPos(largeShpereSz);

                    tooltipBg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.AbsoluteBottomLayer);
                    tooltipBg.LayerBelow(tooltipText);
                    tooltipBg.Color = Color.Black;

                    tooltipText.Position = area.Position + Engine.Screen.IconSizeV2 * 0.1f;
                }
            }
            
            //if (animateTime.HasTime)
            //{
            //    animateTime.CountDown();
            //    selection.Position += moveSelection * Ref.DeltaGameTimeMs;
            //}
        }

        void removeTooltip()
        {
            if (tooltipBg != null)
            {
                tooltipBg.DeleteMe();
                tooltipText.DeleteMe();

                tooltipBg = null;
            }
        }

        public void DeleteMe()
        {
            images.DeleteMe();
            removeTooltip();
        }

        class Sphere
        {
            Time animateTime = Time.Zero;
            Vector2 scaleChange;

            Graphics.Image cirkle, icon;
            const float ToIconScale = 1f;
            public GamePhaseType phase;
            Vector2 goalScale;
            Circle bound = new Circle();
            public int index;

            public Sphere(int index, Vector2 pos, Vector2 sz, Graphics.ImageGroupParent2D images, GamePhaseType phase)
            {
                this.index = index;
                this.phase = phase;
                Color bgCol; SpriteName iconTile; Color iconColor;
                bool use = AbsGamePhase.BorderVisuals(phase, out bgCol, out iconTile, out iconColor);

                cirkle = new Graphics.Image(SpriteName.cmdPhaseCirkle, pos, sz, HudLib.ContentLayer, true);
                cirkle.Color = bgCol;
                icon = new Graphics.Image(iconTile, pos, sz * ToIconScale, ImageLayers.AbsoluteBottomLayer, true);
                icon.Color = iconColor;
                icon.LayerAbove(cirkle);

                images.Add(cirkle);
                images.Add(icon);

                goalScale = sz;
            }

            public Vector2 toolTipStartPos(Vector2 large)
            {
                Vector2 result = cirkle.Position;
                result.X += large.X * 0.5f;
                result.Y -= large.Y * 0.5f + Engine.Screen.IconSize * 0.1f;

                return result;

            }

            public Vector2 setFocus(bool focus, Vector2 small, Vector2 large)
            {
                if (focus)
                {
                    cirkle.Size = small;
                    goalScale = large;
                }
                else
                {
                    cirkle.Size = large;
                    goalScale = small;
                }

                icon.Size = cirkle.Size * ToIconScale;

                scaleChange = (goalScale - icon.Size) / FocusAnimTime;
                animateTime.MilliSeconds = FocusAnimTime;

                return icon.Position;
            }

            public void update()
            {
                if (animateTime.HasTime)
                {
                    animateTime.CountDown();
                    cirkle.Size += scaleChange * Ref.DeltaGameTimeMs;
                    icon.Size = cirkle.Size * ToIconScale;
                }
            }

            public bool intersect(Vector2 point)
            {
                bound.Center = cirkle.Position;
                bound.Radius = cirkle.Size1D * 0.5f;

                return bound.IntersectPoint(point);
            }
        }
    }
}
