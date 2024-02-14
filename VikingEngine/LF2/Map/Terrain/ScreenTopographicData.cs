using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain
{
    class ScreenTopographicData
    {
        static readonly Range CenterDiff = new Range(Terrain.HeightTemplate.PosDiff);
        static readonly Range Heights = new Range(-1, 2);
        static readonly Range DetailHeights = new Range(-1, 1);

        public byte[,] heightMap;
        //public Terrain.HeightTemplate heightTemplate;
        public byte detailMap;
        public byte DotMap;
        public IntVector2 centerDislocation;
        public short Height;
        public short DetailHeight;
        public int TerrainType;

        public ScreenTopographicData(IntVector2 screenIx, AreaType area, int TerrainType)
        {
            //if (area == AreaType.HomeBase)
            //    lib.DoNothing();

            this.TerrainType = TerrainType;
            Data.RandomSeed.Instance.SetSeedPosition(screenIx.X * 3 + screenIx.Y * 7);
            //heightTemplate = LfRef.worldOverView.Topographics.ChunkTemplate();
            centerDislocation = IntVector2.Zero;
            centerDislocation.X += (short)Data.RandomSeed.Instance.Next(CenterDiff);
            centerDislocation.Y += (short)Data.RandomSeed.Instance.Next(CenterDiff);
            Height = (short)Data.RandomSeed.Instance.Next(Heights);
            if (area != AreaType.Empty || area != AreaType.FlatEmptyAndMonsterFree)
            {
                Height = 0;
            }
            if (Height == 2)
            {
                Height = 1;
                heightMap = HandmadeTemplates.Height2Templates[
                    Data.RandomSeed.Instance.Next(HandmadeTemplates.Height2Templates.Count)
                    ];
            }
            else
            {
                heightMap = HandmadeTemplates.Height1Templates[
                    Data.RandomSeed.Instance.Next(HandmadeTemplates.Height1Templates.Count)
                    ];
            }
            DetailHeight = (short)Data.RandomSeed.Instance.Next(DetailHeights);
            detailMap = (byte)Data.RandomSeed.Instance.Next(Terrain.HandmadeTemplates.DetailTemplates.Count());
            DotMap = (byte)Data.RandomSeed.Instance.Next(Terrain.HandmadeTemplates.DotTemplates.Count());
        }
    }
}
