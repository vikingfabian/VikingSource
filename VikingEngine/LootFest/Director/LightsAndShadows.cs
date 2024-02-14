using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.LootFest.Director
{
    class LightsAndShadows
    {
        public bool NewLightSource = false;
        GO.PlayerCharacter.AbsHero[] heroes = new GO.PlayerCharacter.AbsHero[0];
        public static LightsAndShadows Instance;
        SpottedArray<Graphics.ILightSource> sources = new SpottedArray<Graphics.ILightSource>(8);

        public LightsAndShadows()
        {
            toChunkCounter = new SpottedArrayCounter<Graphics.ILightSource>(sources);
            updateDistanceCounter = new SpottedArrayCounter<Graphics.ILightSource>(sources);
        }

        public void AddLight(Graphics.ILightSource l, bool add)
        {
            if (add)
            {
#if PCGAME
                if (sources.Contains(l))
                    throw new Exception("Light source added twice: " + l.ToString());
                if (l.LightSourceType == Graphics.LightParticleType.NUM_NON)
                {
                    throw new Exception(l.ToString() + " has no lightsource");
                }
#endif

                l.LightSourceArrayIndex = sources.Add(l);
                CalcDistanceToGamer(l);
            }
            else
            {
                if (l.LightSourceArrayIndex < 0)
                    throw new Exception("Light source removed before being added, " + l.ToString());
                sources.RemoveAt(l.LightSourceArrayIndex);
            }
            NewLightSource = true;
        }

        public void DebugInfo(HUD.Gui menu)
        {
           // LootFest.File file = new File();
            GuiLayout layout = new GuiLayout("List GameObjects", menu);
            {

                new GuiLabel("LightsAndShadows (" + sources.Count.ToString() + ")", layout);
                SpottedArrayCounter<Graphics.ILightSource> counter = new SpottedArrayCounter<Graphics.ILightSource>(sources);
                while (counter.Next())
                {
                    new GuiLabel("Type:" + counter.sel.LightSourceType.ToString(), true, layout.gui.style.textFormatDebug, layout);
                    new GuiLabel("User:" + counter.sel.ToString(), true, layout.gui.style.textFormatDebug, layout);
                    new GuiLabel("Chunk:" + new Map.WorldPosition(counter.sel.LightSourcePosition).ChunkGrindex.ToString(), true,
                        layout.gui.style.textFormatDebug, layout);
                    new GuiLabel("---", true, layout.gui.style.textFormatDebug, layout);
                }
            }
            layout.End();
            //return file;
        }

        SpottedArrayCounter<Graphics.ILightSource> toChunkCounter;
        float[] distances = new float[Map.Chunk.MaxLightPoints];
        /// <summary>
        /// Calc which lights are closest to each chunk
        /// </summary>
        public void GroupToChunk(VikingEngine.LootFest.Map.Chunk chunk)
        {
            const float ChunkRadius = Map.WorldPosition.ChunkWidth + 2;
            chunk.LightPointsBuffer.Clear();

            toChunkCounter.Reset();
            while(toChunkCounter.Next())
            {
                float l = VectorExt.SideLength(VectorExt.V3XZtoV2(toChunkCounter.sel.LightSourcePosition - chunk.Mesh.Position));
                if (l < toChunkCounter.sel.LightSourceRadius + ChunkRadius)
                {
                    if (PlatformSettings.ViewErrorWarnings)
                    {
                        if (toChunkCounter.sel is GO.AbsUpdateObj && !(toChunkCounter.sel is GO.PlayerCharacter.AbsHero))
                        {
                            GO.AbsUpdateObj obj = (GO.AbsUpdateObj)toChunkCounter.sel;
                            if (!obj.Alive)
                            {
                                Debug.LogWarning("Light source member is dead " + toChunkCounter.sel.ToString());
                            }
                        }
                    }

                    //add if there is a spot available
                    if (chunk.LightPointsBuffer.Count < Map.Chunk.MaxLightPoints)
                    { //not reached max limit yet
                        distances[chunk.LightPointsBuffer.Count] = toComparableLength(toChunkCounter.sel);
                        chunk.LightPointsBuffer.Add(toChunkCounter.sel);
                    }
                    else
                    { //buffer full
                        for (int i = 0; i < chunk.LightPointsBuffer.Count; ++i)
                        {
                            l = toComparableLength(toChunkCounter.sel);
                            if (distances[i] > l)
                            {
                                chunk.LightPointsBuffer.Array[i] = toChunkCounter.sel;
                                distances[i] = i;
                                break;
                            }
                        }
                    }
                }
                
            }

            StaticList<Graphics.ILightSource> buffer = chunk.LightPoints;
            chunk.LightPoints = chunk.LightPointsBuffer;
            chunk.LightPointsBuffer = buffer;

        }

        /// <summary>
        /// When a player join or drop out
        /// </summary>
        public void LocalGamerJoinedEvent()
        {
            
            lock (heroes)
            {
                heroes = LfRef.LocalHeroes.ToArray();
            }
            
        }

        static float toComparableLength(Graphics.ILightSource source)
        {

            return source.LightSourceDistanceToGamer + (int)source.LightSourcePrio * 100;
        }

        SpottedArrayCounter<Graphics.ILightSource> updateDistanceCounter;
        public void UpdateDistanceToGamer()
        {
            
            updateDistanceCounter.Reset();
            while (updateDistanceCounter.Next())
            {
                CalcDistanceToGamer(updateDistanceCounter.sel);
            }
        }

        public void CalcDistanceToGamer(Graphics.ILightSource source)
        {
            lock (heroes)
            {
                FindMinValue dist = new FindMinValue(false);
                for (int i = 0; i < heroes.Length; ++i)
                {
                    dist.Next((heroes[i].Position - source.LightSourcePosition).Length(), i);
                }
                source.LightSourceDistanceToGamer = dist.minValue;
            }
        }
    }
}
