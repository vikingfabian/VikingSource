using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{

    struct PaintDot
    {
        public Graphics.Image dot;
        public IntVector2 tilePos;
        public float strength;

        public PaintDot(Graphics.Image dot, IntVector2 tilePos)
        {
            this.dot = dot;
            this.tilePos = tilePos;
            this.strength = 0;
        }

        public void refreshColor()
        {
            const int Scale = 5;
            int r = 110;
            int b = 110;
            if (strength < 0)
            {
                b += (int)(-strength * Scale);
            }
            else if (strength > 0)
            {
                r += (int)(strength * Scale);
            }
        }
    }
    class ToolSettings
    {
        public Map2GeneratorTab tab;

        public ToolAddType addType = ToolAddType.Add;
        
        public PencilShape pencilShape = PencilShape.Round;

        public int penSize;
        public int maxPenSize;
        public bool noise = false;

        public bool advancedStrength;

        public DrawMapOptions draw = new DrawMapOptions()
        {   
            flatness = 0.2f,
            addHeight = Map2Generator.Height_DefaultGround,
            add = true,
            quadChance = 0f,
            noiseStrength = 0.3f,
            radius = 1f,
        };
    }

    class MapEditor3_Tool
    {
        MapEditor2_Scene scene;
        bool bPaintKeyDown = false;
        public IntVector2 prevTilePos;

        ToolSettings settings_nodes = new ToolSettings() { tab = Map2GeneratorTab.Nodes, penSize = 2, maxPenSize = 10, };
        ToolSettings settings_tiles = new ToolSettings() { tab = Map2GeneratorTab.Icon, penSize = 6, maxPenSize = 80, noise = true, advancedStrength = true};
        ToolSettings settings_bioms = new ToolSettings() { tab = Map2GeneratorTab.Bioms, penSize = 10, maxPenSize = 200, noise = true, };

        public ToolSettings toolSettings;

        public BiomType biom = 0;

        public bool setHeightProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                toolSettings.draw.add = !value;
            }
            return !toolSettings.draw.add;
        }
        public float heightProperty(object tag, bool set, float value)
        {
            if (set)
            {
                toolSettings.draw.addHeight = value;
            }
            return toolSettings.draw.addHeight;
        }

        public int flatnessProperty(object tag, bool set, int value)
        {
            if (set)
            {
                toolSettings.draw.flatness = conv.FromPercentage(value);
            }
            return conv.ToPercentage(toolSettings.draw.flatness);
        }

        public int penSizeProperty(object tag, bool set, int value)
        {
            if (set)
            {
                toolSettings.penSize = value;
            }
            return toolSettings.penSize;
        }

        public bool noiseProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                toolSettings.noise = value;
            }
            return toolSettings.noise;
        }

        Dictionary<int, PaintDot> paintDots = new Dictionary<int, PaintDot>(128);
        EngineSpace.Maths.SimplexNoise2D noiseMap;
        NoiseOptions noiseOpt;

        public MapEditor3_Tool(MapEditor2_Scene scene)
        { 
            this.scene = scene;
            noiseMap = new EngineSpace.Maths.SimplexNoise2D(Ref.rnd.Ushort());
            noiseOpt = new NoiseOptions(true, 0.1f, 4, 1f, 10f);
        }

        public void refreshTools(Map2GeneratorTab tab)
        {
            switch (tab)
            {

                case Map2GeneratorTab.Nodes:
                    toolSettings = settings_nodes;
                    break;
                case Map2GeneratorTab.Icon:
                    toolSettings = settings_tiles;
                    break;
                default:
                    toolSettings = settings_bioms;
                    break;

            }
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
                case Map2GeneratorTab.Icon:
                    if (scene.generator.iconWorld == null)
                    {
                        return;
                    }
                    allowPaintInput = scene.generator.currentPass >= Map2Pass.Icon;
                    tileSize = scene.generator.iconWorld.iconGrid.Size;
                    break;
                case Map2GeneratorTab.Bioms:
                    if (scene.generator.iconWorld == null)
                    {
                        return;
                    }
                    allowPaintInput = scene.generator.currentPass > Map2Pass.NodeGrid;
                    tileSize = scene.generator.iconWorld.iconGrid.Size;
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
            int radius = toolSettings.penSize - 1;

            var advDraw = toolSettings.draw;
            advDraw.refreshHeight();
            advDraw.refreshRadius_PaintTool();

            if (tilePos != prevTilePos)
            {
                Rectangle2 bound = new Rectangle2(IntVector2.Zero, tileSize);
                Rectangle2 area = Rectangle2.FromCenterTileAndRadius(tilePos, radius);
                area.SetBounds(bound);
                ForXYLoop loop = new ForXYLoop(area);
                while (loop.Next())
                {
                    float centerDistance = (tilePos - loop.Position).Length() / radius;

                    if (toolSettings.pencilShape == PencilShape.Round)
                    {
                        if (centerDistance >= 1f)
                        {
                            continue;
                        }
                    }

                    if (toolSettings.noise)
                    {
                        float noiseValue = noiseMap.OctaveNoise2D_Normal(noiseOpt, loop.Position.X, loop.Position.Y);
                        if (noiseValue * 0.8f < centerDistance)
                        {
                            continue;
                        }
                    }

                    float strength = 0;
                    if (toolSettings.advancedStrength)
                    {
                        if (centerDistance < advDraw.flatRadius)
                        {
                            strength = advDraw.centerHeight;
                        }
                        else
                        {
                            float percTowardsEdge = (centerDistance - advDraw.flatRadius) / advDraw.hillRadius;
                            strength = advDraw.centerHeight * (1f - percTowardsEdge) + advDraw.edgeHeight * percTowardsEdge;
                        }
                    }

                    if (paintDots.TryGetValue(loop.Position.GetHashCode(), out var dot))
                    {
                        if (toolSettings.advancedStrength)
                        {
                            if (Math.Abs(strength) > Math.Abs(dot.strength))
                            {
                                dot.strength = strength;
                                dot.refreshColor();
                                paintDots[loop.Position.GetHashCode()] = dot;
                            }
                        }
                    }
                    else
                    {
                        Vector2 pos = scene.map.TileToScreenPos(loop.Position, tileSize);
                        var img = new Graphics.Image(SpriteName.WhiteArea,
                            pos, new Vector2(8), ImageLayers.Top0);
                        img.Color = Color.Purple;
                        dot = new PaintDot(img, loop.Position);
                        if (toolSettings.advancedStrength)
                        {
                            dot.strength = strength;
                            dot.refreshColor();
                        }
                        paintDots.Add(loop.Position.GetHashCode(), dot);
                    }
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
                            ref var tile = ref scene.generator.nodeMap.nodeGrid.GetRef(kv.Value.tilePos);
                            switch (toolSettings.addType)
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

                            scene.generator.nodeMap.refreshPixel(kv.Value.tilePos.X, kv.Value.tilePos.Y);
                            scene.generator.nodeMap.texture.ApplyPixelsToTexture();
                        }
                        break;

                    case Map2GeneratorTab.Icon:
                        foreach (var kv in paintDots)
                        {
                            ref var tile = ref scene.generator.iconWorld.iconGrid.GetRef(kv.Value.tilePos);
                            if (toolSettings.draw.add)
                            {
                                tile.groundY += kv.Value.strength;
                            }
                            else
                            {
                                tile.groundY = kv.Value.strength;
                            }
                        }
                        break;
                    case Map2GeneratorTab.Bioms:
                        foreach (var kv in paintDots)
                        {
                            ref var tile = ref scene.generator.iconWorld.iconGrid.GetRef(kv.Value.tilePos);
                            tile.biom1 = biom;
                        }
                        break;
                }

                foreach (var kv in paintDots)
                {
                    kv.Value.dot.DeleteMe();
                }

                if (scene.display.tab != Map2GeneratorTab.Nodes)
                {
                    scene.redrawPixels();
                }
                paintDots.Clear();
            }
        }

        public void fill()
        {
            switch (toolSettings.tab)
            {
                case Map2GeneratorTab.Nodes:
                    setAllNodes(true);
                    break;
                case Map2GeneratorTab.Bioms:
                    for (int i = 0; i < scene.generator.iconWorld.iconGrid.array.Length; i++)
                    {
                        scene.generator.iconWorld.iconGrid.array[i].biom1 = biom;
                    }
                    scene.redrawPixels();
                    break;

            }
        }
        public void clear()
        {
            setAllNodes(false);
        }


        void setAllNodes(bool toValue)
        {
            scene.generator.nodeMap.nodeGrid.SetAll(toValue);
            scene.generator.nodeMap.refreshAllPixels();
        }
    }

    enum PencilShape
    { 
        Round,
        Square,
        NUM,
    }
}
