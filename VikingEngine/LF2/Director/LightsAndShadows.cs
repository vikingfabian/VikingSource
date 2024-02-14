using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Director
{
    class LightsAndShadows
    {
        public bool NewLightSource = false;
        GameObjects.Characters.Hero[] heroes = new GameObjects.Characters.Hero[0];
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
#if WINDOWS
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

        public LF2.File DebugInfo()
        {
            LF2.File file = new File();
            file.TextBoxBread("LightsAndShadows (" + sources.Count.ToString() + ")");
            SpottedArrayCounter<Graphics.ILightSource> counter = new SpottedArrayCounter<Graphics.ILightSource>(sources);
            while (counter.Next())
            {
                file.TextBoxDebug("Type:" + counter.Member.LightSourceType.ToString());
                file.TextBoxDebug("User:" + counter.Member.ToString());
                file.TextBoxDebug("Chunk:" + new Map.WorldPosition(counter.Member.LightSourcePosition).ChunkGrindex.ToString());
                file.TextBoxDebug("---");
            }
            return file;
        }

        SpottedArrayCounter<Graphics.ILightSource> toChunkCounter;
        float[] distances = new float[HeightMapChunk.MaxLightPoints];
        /// <summary>
        /// Calc which lights are closest to each chunk
        /// </summary>
        public void GroupToChunk(HeightMapChunk chunk)
        {
            const float ChunkRadius = 60;
            chunk.LightPointsBuffer.Clear();

            toChunkCounter.Reset();
            while(toChunkCounter.Next())
            {
                float l = (toChunkCounter.Member.LightSourcePosition - chunk.Mesh.Position).Length();
                if (l < toChunkCounter.Member.LightSourceRadius + ChunkRadius)
                {
                    if (PlatformSettings.ViewErrorWarnings)
                    {
                        if (toChunkCounter.Member is GameObjects.AbsUpdateObj && !(toChunkCounter.Member is GameObjects.Characters.Hero))
                        {
                            GameObjects.AbsUpdateObj obj = (GameObjects.AbsUpdateObj)toChunkCounter.Member;
                            if (!obj.Alive)
                            {
                                Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "Light source member is dead " + toChunkCounter.Member.ToString());
                            }
                        }
                    }

                    //add if there is a spot available
                    if (chunk.LightPointsBuffer.Count < HeightMapChunk.MaxLightPoints)
                    { //not reached max limit yet
                        distances[chunk.LightPointsBuffer.Count] = toComparableLength(toChunkCounter.Member);
                        chunk.LightPointsBuffer.Add(toChunkCounter.Member);
                    }
                    else
                    { //buffer full
                        for (int i = 0; i < chunk.LightPointsBuffer.Count; ++i)
                        {
                            l = toComparableLength(toChunkCounter.Member);
                            if (distances[i] > l)
                            {
                                chunk.LightPointsBuffer.Array[i] = toChunkCounter.Member;
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
                CalcDistanceToGamer(updateDistanceCounter.Member);
            }
        }

        public void CalcDistanceToGamer(Graphics.ILightSource source)
        {
            lock (heroes)
            {
                LowestValue dist = new LowestValue(false);
                for (int i = 0; i < heroes.Length; ++i)
                {
                    dist.Next((heroes[i].Position - source.LightSourcePosition).Length(), i);
                }
                source.LightSourceDistanceToGamer = dist.Lowest;
            }
        }
    }
}
