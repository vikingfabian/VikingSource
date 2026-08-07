using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class UnitMiniModels
    {
        int processFactionCount = 20;
        List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(256);

        bool processing = true;
        SpottedArrayCounter<Faction> factions;

        Graphics.IVerticeData verticeData;
        Graphics.VoxelModel model;

        Timer.Basic updateTimer = new Timer.Basic(500, false);

        public UnitMiniModels()
        {
            Graphics.CustomEffect ModelEffect = new Graphics.CustomEffect("FlatVerticeColor", false);

            factions = DssRef.world.factions.counter();
            model = new Graphics.VoxelModel(false);
            model.Effect = ModelEffect;
            model.AddToRender(DrawGame.MidLayer);
            model.Visible = false;
        }

        public void update()
        {
            if (!processing)
            {
                if (verticeData != null)
                {

                    model.BuildFromVerticeData(verticeData,
                        new List<int> { verticeData.DrawData.numTriangles / 2 }, LoadedTexture.SpriteSheet);
                    model.Visible = true;

                    verticeData = null;
                }
            
                processFactionCount = Bound.Min(DssRef.world.factions.Count / 10, 1);
                factions = DssRef.world.factions.counter();

                processing = true;
            }

            
        }

        public void asynchUpdate()
        {
            if (processing)
            {
                
                for (int i = 0; i < processFactionCount; ++i)
                {
                    if (factions.Next())
                    {
                        var faction_sp = factions.sel;
                        if (faction_sp != null)
                        {
                            faction_sp.Colors(out Color main, out Color second);
                            PolygonColor topPoly = new PolygonColor();
                            topPoly.setSprite(SpriteName.WarsTextureGroupSquare, Dir4.N);
                            topPoly.SetColor(main);

                            var armies = faction_sp.armies.counter();
                            while (armies.Next())
                            {
                                var groups = armies.sel.groups.counter();
                                while (groups.Next())
                                {
                                    Vector3 pos = groups.sel.position;
                                    pos.Y += 0.07f;

                                    if (pos.Y < Tile.UnitQuadMinY)
                                    {
                                        pos.Y = Tile.UnitQuadMinY;
                                    }

                                    //PolygonColor poly = Graphics.PolygonColor.QuadXZ(pos,
                                    //    new Vector2(groups.sel.groupRadius), groups.sel.rotation.radians - MathExt.TauOver4,
                                    //    SpriteName.WarsTextureGroupSquare, Dir4.N,
                                    //    main);
                                    topPoly.quadXZPlacement(pos,
                                        new Vector2(groups.sel.groupRadius), groups.sel.rotation.radians - MathExt.TauOver4);
                                    polygons.Add(topPoly);
                                    
                                    topPoly.quadXZSides(0.08f, SpriteName.WhiteArea_LFtiles, Dir4.N, second, polygons);

                                    PolygonColor typeSymbol = topPoly;
                                    typeSymbol.Move(new Vector3(0, 0.01f, 0));
                                    typeSymbol.setSprite(SpriteName.WarsTextureGroupSquareMelee, Dir4.N);
                                    typeSymbol.SetColor(second);

                                    polygons.Add(typeSymbol);


                                }
                            }
                        }
                    }
                    else
                    { //Complete
                        if (polygons.Count > 0)
                        {
                            model.Visible = true;
                            verticeData = Graphics.PolygonLib.BuildVDFromPolygons(
                                new Graphics.PolygonsAndTrianglesColor(polygons, null));
                            polygons.Clear();
                        }
                        else
                        {
                            model.Visible = false;
                            updateTimer.Reset();
                        }
                        processing = false;
                        return;
                    }
                }
            }
        }
    }
}
