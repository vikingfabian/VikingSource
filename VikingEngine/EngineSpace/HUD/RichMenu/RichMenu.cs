using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;

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
        
        NineSplitAreaTexture backgroundTextures;
        public VectorRect backgroundArea, edgeArea, renderArea, richboxArea, mouseScrollArea;
        Vector2 renderEdge;
        public RbInteraction interaction = null;
        
        //Graphics.RectangleLines outLine;
        public RenderTargetDrawContainer renderList = null;//Is a target image, rendering the menu content
        
        RichBoxSettings settings;

        float scrollerWidth;
        RichScrollbar scrollBar;
        ImageLayers layer;
        public PlayerData playerData;
        RichTooltip tooltip = null;
        public string activeDropDown = null;
        public bool needRefresh = false;
        public List<string> menuStack = new List<string>();

        public RichMenu(RichBoxSettings settings, VectorRect edgeArea, Vector2 edgeThickness, Vector2 renderEdge, ImageLayers layer, PlayerData playerData)
        { 
            this.playerData = playerData;
            this.layer = layer;
            this.settings = settings;
            this.edgeArea = edgeArea;
            backgroundArea = edgeArea;
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

        

        public void OnDropDownClick(string name)
        {
            if (activeDropDown == name)
            {
                activeDropDown = null;
            }
            else
            {
                activeDropDown = name;
            }
            needRefresh = true;
        }

        public void Refresh()
        {
            needRefresh = true;
        }

        public void CloseDropDown()
        {
            activeDropDown = null;
            needRefresh = true;
        }

        public void addToolTip(RichBoxContent content, VectorRect buttonArea)
        {
            //Debug.Log("deleteTooltip: add");
            deleteTooltip();
            //Debug.Log("add Tooltip");
            buttonArea.Position += renderList.position;
            tooltip = new RichTooltip(content, HudLib.TooltipSettings, buttonArea, playerData.view.safeScreenArea, layer -5);
        }

        public void deleteTooltip()
        {
            if (Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }
            //Debug.Log("delete Tooltip");
            tooltip?.DeleteMe();
            tooltip = null;
        }

        public void move(Vector2 move)
        {
            backgroundArea.Position  += move;
            renderArea.Position += move;
            renderList.position += move;

            backgroundTextures?.Move(move);
            //backgroundTextures.images
        }

        public void moveToY(float y)
        {
            //backgroundArea.Position += move;
            //renderArea.Position += move;
            //renderList.position += move;
            Vector2 moveDist= VectorExt.V2FromY(y- backgroundArea.Y);
            this.move(moveDist);
        }

        public void updateWidthFromContent(bool resetFirst = true)
        {
            float edgeThickness = edgeArea.Bottom - renderArea.Bottom;

            if (resetFirst)
            {
                backgroundArea = edgeArea;
            }
            backgroundArea.Width = richBox.maxArea.Size.X + edgeThickness * 2;
        }


        public void updateHeightFromContent(bool resetFirst = true)
        {
            float edgeThickness = edgeArea.Bottom - renderArea.Bottom;
            if (resetFirst)
            {
                backgroundArea = edgeArea;
            }
            backgroundArea.Height = richBox.area.Size.Y + edgeThickness * 3;
        }


        public NineSplitAreaTexture addBackground(NineSplitSettings texture, ImageLayers layer)
        {
            backgroundTextures = new NineSplitAreaTexture(texture, backgroundArea, layer + 1);
            return backgroundTextures;
        }

        public void OpenMenu(string menuName, bool stack)
        {
            if (!stack)
            {
                menuStack.Clear();
            }
            menuStack.Add(menuName);
            needRefresh = true;
        }

        public void OpenMenu(RichBoxContent content, string menuName)
        {
            menuStack.Add(menuName);
            Refresh(content);
        }

        public void clearState()
        {
            menuStack.Clear();
            needRefresh = true;
        }
        //public void SetMenuState(string state)
        //{
        //    menuState.Add(state);
        //    menuStateHasChange = true;
        //}

        //public bool HasMenuState(string state)
        //{
        //    return menuState.Contains(state);
        //}
        public void menuBack()
        {
            arraylib.RemoveLast(menuStack);
            needRefresh = true;
        }

        //void menuMoveRefreshOnStateChange()
        //{
        //    if (input.RichboxGuiUseMove && movePos_part >= 0)
        //    {
        //        beginMove(movePos_part);
        //    }
        //}

        public string CurrentMenuState => menuStack.LastOrDefault();

        public void Refresh(RichBoxContent content)
        {
            //Debug.Log("Rich menu REFRESH");
            deleteContent();

            Ref.draw.AddToContainer = renderList;
            {
                richBox = new RichBoxGroup(Vector2.Zero,
                    richboxArea.Width, ImageLayers.Lay0, settings, content, true, true, false);                
                
            } Ref.draw.AddToContainer = null;

            scrollBar.Refresh(richBox.area.Height + renderEdge.Y * 2, renderArea.Height - renderEdge.Y * 2, settings.button.size);
            bool hadSelection = interaction != null && interaction.hover != null;

            //var prevInteract = interaction;
            //updateContentScroll();
            if (interaction == null || interaction.drawContainer != renderList)
            {
                interaction = new RbInteraction(content, layer, new Input.MouseButtonMap(MouseButton.Left));

                interaction.drawContainer = renderList;
            }
            else
            {
                interaction.refresh(content);
            }
            //Debug.Log("Rich menu SCROLL");
            updateContentScroll();

            needRefresh = false;

            if (hadSelection)
            {
                
                //interaction.inherit(prevInteract);
                //deleteTooltip();
                interaction.update(-renderArea.Position, this, false, out _, out _);
                
                tooltip?.view();

                //interaction.update(-renderArea.Position, this, false, out _);
            }
        }

        void deleteContent()
        {
            renderList.renderList.Clear();
        }

        public void DeleteMe()
        {
            renderList.DeleteMe();
            backgroundTextures.DeleteMe();
            //Debug.Log("deleteTooltip: del menu");
            deleteTooltip();
            scrollBar.DeleteMe();
        }

        public void updateMouseInput(ref bool mouseOver)
        {
            if (interaction != null)
            {      
                if (backgroundArea.IntersectPoint(Input.Mouse.Position)
                    ||
                    interaction.interactionStack != null)
                {
                    mouseOver = true;
                    if (interaction.update(-renderArea.Position, this, true, out bool interactRefresh, out _))
                    {
                        needRefresh = true;
                    }
                    needRefresh |= interactRefresh;
                }
                else
                {
                    //Debug.Log("deleteTooltip: outside menu");
                    deleteTooltip();
                    interaction.clearSelection();
                }

                if (scrollBar.IsVisible() && mouseScrollArea.IntersectPoint(Input.Mouse.Position))
                {
                    mouseOver = true;
                    if (interaction.interactionStack == null && scrollBar.updateMouseInput())
                    {
                        updateContentScroll();
                    }
                    if (scrollBar.updateScrollWheel())
                    {
                        updateContentScroll();
                    }
                }
            }
        }

        public bool BlockRefresh()
        {
            return interaction.interactionStack != null;
        }

        void updateContentScroll()
        {

            richBox.SetOffset( new Vector2(renderEdge.X, renderEdge.Y + scrollBar.scrollResult));
        }
    }
}
