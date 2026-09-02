using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map.Map2
{
    class NodeMap
    {
        public const int TextureScale = 4;
        public const int NodePixWidth = 8;

        public const int start = NodePixWidth / 2;


        public int nodeCount = 0;
        public Grid2D_L<bool> nodeGrid;
        
        public PixelTexture texture;

        public void GenerateTexture()
        {
            texture = new PixelTexture(nodeGrid.Size * TextureScale);
            //for (int x = 0; x < nodeGrid.Width; x++)

            refreshAllPixels();
        }

        public void refreshAllPixels()
        {
            Parallel.For(0, nodeGrid.Width, x =>
            {
                for (int y = 0; y < nodeGrid.Height; y++)
                {
                    refreshPixel(x, y);
                }
            });

            texture.ApplyPixelsToTexture();
        }

        public void refreshPixel(int x, int y)
        {
            var color = nodeGrid.Get(x, y) ? Color.ForestGreen : Color.DarkBlue;

            if (lib.IsEven(x + y))
            {
                color = ColorExt.MultiplyRGB(color, 0.8f);
            }

            for (int pixY = 0; pixY < TextureScale; pixY++)
            {
                for (int pixX = 0; pixX < TextureScale; pixX++)
                {
                    texture.SetPixel(x * TextureScale + pixX, y * TextureScale + pixY, color);
                }
            }
        }

        public void Generate(IconWorldData iconWorld, Map2GenerateSettings generateSettings)
        {
            IntVector2 gridSz = new IntVector2(iconWorld.iconGrid.Width / NodePixWidth - 1, iconWorld.iconGrid.Height / NodePixWidth - 1);
            float fillPerc = conv.FromPercentage(generateSettings.nodeFillPerc);
            float keepFillingPerc = conv.FromPercentage(generateSettings.nodeConnectPerc);
            int recursiveLayers = Bound.Set(generateSettings.nodeConnectPerc / 10 + 8, 2, 20);
            float connectPerc = Bound.Max( keepFillingPerc + 0.2f, 0.95f);
            nodeGrid = new Grid2D_L<bool>(gridSz);
            int fillCount = (int)(gridSz.Area() * fillPerc);
            nodeCount = fillCount;
            int paralellCount = fillPerc < 0.4f ? 2 : 8;

            Parallel.For(0, paralellCount, i =>
                {

                    while (fillCount > 0)
                    {

                        setPos(iconWorld.rnd.intvector2(gridSz), 0);


                        void setPos(IntVector2 pos, int layer)
                        {
                            if (!nodeGrid.Get(pos) && layer < recursiveLayers)
                            {
                                nodeGrid.Set(pos, true);
                                fillCount--;


                                double rand = iconWorld.rnd.Double();

                                if (rand < keepFillingPerc)
                                {
                                    foreach (var dir in IntVector2.Dir8Array)
                                    {
                                        if (iconWorld.rnd.Chance(0.6))
                                        {
                                            if (nodeGrid.TryGet(pos + dir, out var value) && !value)
                                            {
                                                setPos(pos + dir, layer + 1);
                                            }
                                        }
                                    }
                                }
                                else if (rand < connectPerc)
                                {
                                    //Try connect
                                    for (int i = 0; i < 2; i++)
                                    {
                                        IntVector2 rndDir = arraylib.RandomListMember(IntVector2.Dir8Array, iconWorld.rnd);

                                        IntVector2 check = pos + rndDir;
                                        if (nodeGrid.TryGet(check, out var value) && !value)
                                        {
                                            int neighborCount = 0;
                                            foreach (var dir in IntVector2.Dir8Array)
                                            {
                                                if (nodeGrid.TryGet(pos + dir, out var nvalue) && nvalue)
                                                {
                                                    if (++neighborCount >= 2)
                                                    {
                                                        setPos(pos + dir, layer +1);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
        }
    }
}
