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

            Parallel.For(0, nodeGrid.Width, x =>
            {
                for (int y = 0; y < nodeGrid.Height; y++)
                {
                    var color = nodeGrid.Get(x, y) ? Color.ForestGreen : Color.DarkBlue;
                    for (int pixY = 0; pixY < TextureScale; pixY++)
                    {
                        for (int pixX = 0; pixX < TextureScale; pixX++)
                        {
                            texture.SetPixel(x * TextureScale + pixX, y * TextureScale + pixY, color);
                        }
                    }

                }
            });

            texture.ApplyPixelsToTexture();
        }

        public void Generate(IconWorldData iconWorld, Map2GenerateSettings generateSettings)
        {
            IntVector2 gridSz = new IntVector2(iconWorld.iconGrid.Width / NodePixWidth - 1, iconWorld.iconGrid.Height / NodePixWidth - 1);
            float fillPerc = 0.15f;
            nodeGrid = new Grid2D_L<bool>(gridSz);
            int fillCount = (int)(gridSz.Area() * fillPerc);
            nodeCount = fillCount;

            Parallel.For(0, 8, i =>
                {

                    while (fillCount > 0)
                    {

                        setPos(iconWorld.rnd.intvector2(gridSz));


                        void setPos(IntVector2 pos)
                        {
                            if (!nodeGrid.Get(pos))
                            {
                                nodeGrid.Set(pos, true);
                                fillCount--;


                                double rand = iconWorld.rnd.Double();

                                if (rand < 0.45)
                                {
                                    foreach (var dir in IntVector2.Dir8Array)
                                    {
                                        if (iconWorld.rnd.Chance(0.6))
                                        {
                                            if (nodeGrid.TryGet(pos + dir, out var value) && !value)
                                            {
                                                setPos(pos + dir);
                                            }
                                        }
                                    }
                                }
                                else if (rand < 0.9)
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
                                                        setPos(pos + dir);
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
