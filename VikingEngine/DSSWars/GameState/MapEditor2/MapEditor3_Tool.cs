using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{

    //class PaintDot
    //{
    //    public Graphics.Image dot;
    //    public IntVector2 tilePos;

    //}

    class MapEditor3_Tool
    {
        MapEditor2_Scene scene;
        bool bPaintKeyDown = false;
        public IntVector2 prevTilePos;

        ToolAddType addType = ToolAddType.Toggle;

        Dictionary<IntVector2,Graphics.Image> paintDots = new Dictionary<IntVector2, Graphics.Image>(128);

        public MapEditor3_Tool(MapEditor2_Scene scene)
        { 
            this.scene = scene;
        }
        public void paintInput(InputMap input)
        {
           
            bool allowPaintInput = false;
            IntVector2 tileSize = IntVector2.One;
            switch (scene.display.tab)
            {
                case Map2GeneratorTab.Nodes:
                    if (scene.generator.nodeMap == null)
                    {
                        return;
                    }
                    allowPaintInput = scene.generator.currentPass == Map2Pass.NodeGrid;
                    tileSize = scene.generator.nodeMap.nodeGrid.Size;
                    break;
            }

            if (allowPaintInput)
            {
                if (bPaintKeyDown)
                {
                    if (scene.map.pointerToTilePos(Input.Mouse.Position, tileSize, out var tilePos))
                    {
                        paintOnTile(tilePos, tileSize);
                    }
                }
                else if (input.editorInput.draw.DownEvent)
                {
                    if (scene.map.pointerToTilePos(Input.Mouse.Position, tileSize, out var tilePos))
                    {
                        bPaintKeyDown = true;
                        prevTilePos = IntVector2.NegativeOne;
                        paintOnTile(tilePos, tileSize);
                    }
                }

                if (input.editorInput.draw.UpEvent)
                {
                    finalizePaintStroke();
                }
            }
            else
            {
                finalizePaintStroke();
            }
        }

        public void paintOnTile(IntVector2 tilePos, IntVector2 tileSize)
        {
            if (tilePos != prevTilePos)
            {
                if (!paintDots.ContainsKey(tilePos))
                {
                    Vector2 pos = scene.map.TileToScreenPos(tilePos, tileSize);
                    var img = new Graphics.Image(SpriteName.WhiteArea,
                        pos, new Vector2(4), ImageLayers.Top0);
                    img.Color = Color.Purple;
                    paintDots.Add(tilePos, img);
                }

               

                prevTilePos = tilePos;
            }
        }

        public void finalizePaintStroke()
        {
            if (bPaintKeyDown)
            {
                bPaintKeyDown = false;
                switch (scene.display.tab)
                {
                    case Map2GeneratorTab.Nodes:
                        foreach (var kv in paintDots)
                        {
                            ref var tile = ref scene.generator.nodeMap.nodeGrid.GetRef(kv.Key);
                            switch (addType)
                            {
                                case ToolAddType.Toggle:
                                    tile = !tile;
                                    break;
                                case ToolAddType.Add:
                                    tile = true;
                                    break;
                                case ToolAddType.Remove:
                                    tile = false;
                                    break;
                            }

                            scene.generator.nodeMap.refreshPixel(kv.Key.X, kv.Key.Y);
                        }
                        break;
                }

                foreach (var kv in paintDots)
                {
                    kv.Value.DeleteMe();
                }

                paintDots.Clear();
            }
        }
    }
}
