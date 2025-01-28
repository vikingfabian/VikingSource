using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.HUD.RichMenu
{
    /// <summary>
    /// Creates a scrollable container of richbox content. Will update input, and create tooltips.
    /// </summary>
    class RichMenu
    {
        public static readonly Vector2 DefaultRenderEdge = new Vector2(4);

        public RichBoxGroup richBox;
        protected RichBoxContent content = new RichBoxContent();
        //protected Graphics.Image bg;
        NineSplitAreaTexture backgroundTextures;
        public VectorRect edgeArea, renderArea, richboxArea, mouseScrollArea;
        Vector2 renderEdge;
        public RbInteraction interaction = null;
        //protected RichboxGui gui;
        Graphics.RectangleLines outLine;

        RenderTargetDrawContainer renderList = null;//Is a target image, rendering the menu content
        
        RichBoxSettings settings;

        float scrollerWidth;
        RichScrollbar scrollBar;
        ImageLayers layer;
        public PlayerData playerData;
        RichTooltip tooltip = null;

        public RichMenu(RichBoxSettings settings, VectorRect edgeArea, Vector2 edgeThickness, Vector2 renderEdge, ImageLayers layer, PlayerData playerData)
        { 
            this.playerData = playerData;
            this.layer = layer;
            this.settings = settings;
            this.edgeArea = edgeArea;
            renderArea = edgeArea;
            renderArea.AddXRadius(-edgeThickness.X);
            renderArea.AddYRadius(-edgeThickness.Y);

            this.renderEdge = renderEdge;
            richboxArea = renderArea;
            richboxArea.AddXRadius(-renderEdge.X); 
            richboxArea.AddYRadius(-renderEdge.Y);

            scrollerWidth = Screen.MinClickSize;

            renderList = new RenderTargetDrawContainer(renderArea.Position, renderArea.Size, layer, new List<AbsDraw>());
            
            scrollBar = new RichScrollbar(HudLib.HudMenuScollButton, HudLib.HudMenuScollBackground, edgeArea, scrollerWidth, layer -2);
            mouseScrollArea = scrollBar.IncludeScrollArea(edgeArea);
        }

        public void addToolTip(RichBoxContent content, VectorRect buttonArea)
        {
            deleteTooltip();
            buttonArea.Position += renderList.position;
            tooltip = new RichTooltip(content, HudLib.TooltipSettings, buttonArea, playerData.view.safeScreenArea, layer -5);
        }

        public void deleteTooltip()
        {
            tooltip?.DeleteMe();
            tooltip = null;
        }

        public void addBackground(NineSplitSettings texture, ImageLayers layer)
        {
            backgroundTextures = new NineSplitAreaTexture(texture, edgeArea, layer + 1);
        }

        public void Refresh(RichBoxContent content)
        {
            Ref.draw.AddToContainer = renderList;
            {
                richBox = new RichBoxGroup(Vector2.Zero,
                    richboxArea.Width, ImageLayers.Lay0, settings, content, true, true, false);                
                
            } Ref.draw.AddToContainer = null;

            scrollBar.Refresh(richBox.area.Height + renderEdge.Y * 2, renderArea.Height - renderEdge.Y * 2, settings.button.size);
            interaction = new RbInteraction(content, layer, new Input.MouseButtonMap(MouseButton.Left));
            //interaction.outlineOffset = renderEdge;
            interaction.drawContainer = renderList;

            updateContentScroll();
        }

        public void updateMouseInput()
        {
            if (interaction.interactionStack == null && scrollBar.updateMouseInput())
            {
                updateContentScroll();
            }
            else if (renderArea.IntersectPoint(Input.Mouse.Position) ||
                interaction.interactionStack != null)
            {
                interaction.update(-renderArea.Position, this, out _);
            }
            else
            {
                
                interaction.clearSelection();
            }

            if (mouseScrollArea.IntersectPoint(Input.Mouse.Position))
            {
                if (scrollBar.updateScrollWheel())
                {
                    updateContentScroll();
                }
            }
        }

        void updateContentScroll()
        {
            richBox.SetOffset( new Vector2(renderEdge.X, renderEdge.Y + scrollBar.scrollResult));
        }
    }
}
