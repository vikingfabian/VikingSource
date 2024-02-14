using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            model.AddToRender(DrawGame.TerrainLayer);
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
                factions.Reset();

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
                            var armies = faction_sp.armies.counter();
                            while (armies.Next())
                            {
                                var groups = armies.sel.groups.counter();
                                while (groups.Next())
                                {
                                    var poly = Graphics.PolygonColor.QuadXZ(VectorExt.AddY(groups.sel.position, 0.02f),
                                        new Vector2(groups.sel.groupRadius), groups.sel.rotation.radians,
                                        SpriteName.WhiteArea_LFtiles, Dir4.N,
                                        faction_sp.Color());

                                    polygons.Add(poly);
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
