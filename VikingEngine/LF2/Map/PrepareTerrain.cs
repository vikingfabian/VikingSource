using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VikingEngine.Graphics;


namespace VikingEngine.LF2.Map
{
    //ska ta ut dom block som ska renderas
    //räkna ut vilka sidor som blocket har
    //beräkna skuggor och ljus

    struct closeScreenData
    {
        public IntVector2 Position;
        public bool Keep;

        public closeScreenData(IntVector2 pos)
        {
            Position = pos;
            Keep = false;
        }
    }
    //class PrepareTerrain
    //{
    //    const int OpenScreenRadius = 2;
    //    public const int DecompressScreenRadius = OpenScreenRadius + 1;
    //    const int OpenScreenWidth = OpenScreenRadius * 2 + 1;
    //    static readonly int OpenScreenListLength = (int)Math.Pow(OpenScreenWidth, 2);

    //    static readonly List<bool> ShadowSearchSideStep = new List<bool> { false, true, false, true };
    //    static readonly IPreparedBlock Empty = new EmptyBlock();
    //   // IntVector3 blockGridArraySize;
    //    //PreparedBlock[, ,] blockGrid;
    //    IntVector2 centerScreen = new IntVector2(-1, -1);
    //    //public List<PreparedScreen> preparedScreens = new List<PreparedScreen>();
    //    public List<DataChunks.Screen> openScreensList = new List<DataChunks.Screen>();
    //    // Unused vars
    //    //int lastCheckedPreparedScreen;
    //    //RangeIntV3 
    //    //RangeIntV3 shadowGridWPRange;

    //    public PrepareTerrain()
    //    {
    //        //shadowGridWPRange = new RangeIntV3();
    //        //shadowGridWPRange.Max.Y = WorldPosition.SquaresPerScreenY - 1;
    //        //shadowGridWPRange.Min.Y = 1;
    //    }
    //    //public List<PreparedBlock> GetPreparedBlocks()
    //    //{
    //    //    List<PreparedBlock> result = new List<PreparedBlock>();
    //    //    //que this
    //    //    foreach (PreparedScreen screen in preparedScreens)
    //    //    {
    //    //        result.AddRange(screen.FaceBlocks);
    //    //    }
    //    //    return result;
    //    //}
    //    public bool OpenScreen(WorldPosition screenPos)
    //    {
    //       //lägg till decompretion av skärmar

    //        List<DataChunks.Screen> newOpenScreensList = new List<DataChunks.Screen>();

    //        for (int i = 0; i < openScreensList.Count; i++)
    //        {
    //            bool keep = false;
    //            //jämför med alla spelare, se om den ligger nära nån av dom
    //            for (int p = 0; p < LfRef.Heroes.Count; p++)
    //            {
    //                //jämför avstånd
    //                IntVector2 diff = LfRef.Heroes[p].ScreenPos;
    //                diff.X -= openScreensList[i].Index.X;
    //                diff.Y -= openScreensList[i].Index.Y;
    //                if (Math.Abs(diff.X) <= OpenScreenRadius &&
    //                    Math.Abs(diff.Y) <= OpenScreenRadius)
    //                {
    //                    keep = true;
    //                    break;
    //                }

    //            }
    //            if (keep)
    //            {
    //                newOpenScreensList.Add(openScreensList[i]);
    //            }
    //            else
    //            {
    //                WorldPosition wp = new WorldPosition();
    //                wp.ChunkGrindex = openScreensList[i].Index;
    //               // LfRef.chunks.GetScreen(wp).OpenScreen(false);
    //            }

    //        }
    //        centerScreen = screenPos.ChunkGrindex;
    //        //decompress all screens in the new area
    //        WorldPosition decompressPos = new WorldPosition();
    //        for (int y = -DecompressScreenRadius; y <= DecompressScreenRadius; y++)
    //        {
    //            decompressPos.ChunkY = y + screenPos.ChunkGrindex.Y;
    //            for (int x = -DecompressScreenRadius; x <= DecompressScreenRadius; x++)
    //            {
    //                decompressPos.ChunkX = x + screenPos.ChunkGrindex.X;
    //                if (decompressPos.CorrectScreenPos)
    //                {
    //                   // LfRef.chunks.GetScreen(decompressPos).PutInCompressQue(false);
    //                }
    //            }
    //        }
    //        //which to open
    //        List<ShortVector2> OpenThese = new List<ShortVector2>();
               
    //        Map.WorldPosition open = screenPos;
    //        for (short y = -OpenScreenRadius; y <= OpenScreenRadius; y++)
    //        {
    //            open.ChunkGrindex.Y = (short)(y + screenPos.ChunkGrindex.Y);
    //            for (short x = -OpenScreenRadius; x <= OpenScreenRadius; x++)
    //            {

    //                open.ChunkGrindex.X = (short)(x + screenPos.ChunkGrindex.X);
    //                if (open.CorrectScreenPos)
    //                {

    //                    OpenThese.Add(open.ChunkGrindex);
    //                }
    //            }
    //        }

    //        //lägg allt i kö

    //        if (OpenThese.Count > 0)
    //        {

    //            foreach (ShortVector2 o in OpenThese)
    //            {
    //                WorldPosition wp = new WorldPosition();
    //                wp.ChunkGrindex = o;
    //                if (wp.CorrectScreenPos)
    //                {

    //                    DataChunks.Screen screen = LfRef.chunks.GetScreen(wp); 
                       
    //                }
    //            }
    //        }

    //        openScreensList = newOpenScreensList;

    //        //new ResetShadowsQue(this);
    //        //new CalculateShadowsQue(this);
    //        return true;
    //    }
        


    //    IntVector2[] openScreenList(IntVector2 center)
    //    {
    //        IntVector2[] result = new IntVector2[OpenScreenListLength];
            
    //        int listIx = 0;
    //        IntVector2 screenIx = IntVector2.Zero;
    //        for (int y = -OpenScreenRadius; y <= OpenScreenRadius; y++)
    //        {
    //            screenIx.Y = y + center.Y;
    //            for (int x = -OpenScreenRadius; x <= OpenScreenRadius; x++)
    //            {
    //                screenIx.X = x + center.X;
    //                result[listIx] = screenIx;
    //                listIx++;
    //            }
    //        }
    //        return result;
    //    }

    //    //public void CalculateShadows(List<PreparedBlock> FaceBlocks, int startIx, int stopBeforeIx)
    //    //{

    //    //    lastCheckedPreparedScreen = 0;
            
            
    //    //    //List<PreparedBlock> FaceBlocks = GetPreparedBlocks();
    //    //    //foreach (PreparedBlock block in FaceBlocks)
    //    //    for (int b = startIx ; b < stopBeforeIx; b++)
    //    //    {
    //    //        if (FaceBlocks[b].ShadowSide)
    //    //        {

    //    //            const int Xdir = 1;
    //    //            const int Ydir = -1;
    //    //            const int Zdir = -1;

    //    //            WorldPosition searchPos = FaceBlocks[b].WorldPos;
    //    //            //always start by take one step down diagonally
    //    //            searchPos.GridPos.X += Xdir;
    //    //            searchPos.GridPos.Y += Ydir;
    //    //            searchPos.GridPos.Z += Zdir;


    //    //            //loop the search until finding a block or being outside the map
    //    //            const int MaxShadowRange = 20;
    //    //            const float ShadowValue = 0.3f;
    //    //            const float MaxLostValue = 0.08f;
    //    //            const float SpreadSideMulti = 0.5f;
    //    //            const float SpreadDiagonallyMulti = 0.35f;
    //    //            const float LostValuePerStep = MaxLostValue / MaxShadowRange;
    //    //            int currentShadowSearchSideStep = 0;

    //    //            for (int i = 0; i < MaxShadowRange; i++)
    //    //            {
    //    //                if (searchPos.GridPos.Y <= 1 || searchPos.GridPos.Z <= 1)
    //    //                { break; }
    //    //                PreparedBlock currentblock = GetBlock(searchPos);

    //    //                if (currentblock != null)
    //    //                {

    //    //                    //found receiving block
    //    //                    float shadow = ShadowValue;// -LostValuePerStep * i;
    //    //                    currentblock.AddShadowValue(shadow);
    //    //                    //check the blocks around it
    //    //                    //check +-X and +-Z
    //    //                    float sideShadow = shadow * SpreadSideMulti;
    //    //                    float diagonalShadow = shadow * SpreadDiagonallyMulti;
    //    //                    WorldPosition wp = searchPos;

    //    //                    //Have now found a block, will search areound it and shadow all sourronding ones
    //    //                    for (int z = -1; z <= 1; z++)
    //    //                    {
    //    //                        wp.GridPos.Z = searchPos.GridPos.Z + z;

    //    //                        for (int x = -1; x <= 1; x++)
    //    //                        {

    //    //                            if (x != 0 || z != 0)
    //    //                            {
    //    //                                wp.GridPos.X = searchPos.GridPos.X + x;
    //    //                                searchShadowRecievingBlock(wp, (x + z == 2 ? diagonalShadow : sideShadow));
    //    //                            }
    //    //                        }
    //    //                    }
    //    //                    break;
    //    //                    //Determens the angle of the shadows
    //    //                    searchPos.GridPos.Y += Ydir;
    //    //                    if (ShadowSearchSideStep[currentShadowSearchSideStep])
    //    //                    {
    //    //                        searchPos.GridPos.X += Xdir;
    //    //                        searchPos.GridPos.Z += Zdir;
    //    //                    }

    //    //                    currentShadowSearchSideStep++;
    //    //                    if (currentShadowSearchSideStep >= ShadowSearchSideStep.Count)
    //    //                        currentShadowSearchSideStep = 0;

    //    //                }
    //    //                else
    //    //                {
    //    //                    break;
    //    //                }

    //    //            }
    //    //        }
    //    //    }
            
    //    //}

    //    //bool searchShadowRecievingBlock(WorldPosition searchPos, float shadow)
    //    //{
    //    //    IPreparedBlock currentblock = GetBlock(searchPos);

    //    //    if (currentblock != null)
    //    //    {
    //    //        currentblock.AddShadowValue(shadow);
    //    //        return true;
    //    //    }
            
    //    //    return false;
    //    //}
    //    //PreparedBlock GetBlock(WorldPosition pos)
    //    //{
            
    //    //    //pos.UpdateScreen();
    //    //    //if (preparedScreens[lastCheckedPreparedScreen].Position.X == pos.ScreenIndex.X &&
    //    //    //    preparedScreens[lastCheckedPreparedScreen].Position.Y == pos.ScreenIndex.Y)
    //    //    //{
    //    //    //    return preparedScreens[lastCheckedPreparedScreen].GetBlock(pos);
    //    //    //}
    //    //    //else
    //    //    //{
    //    //    //    for (int i = 0; i < preparedScreens.Count; i++)
    //    //    //    {
    //    //    //        if (preparedScreens[i].Position.X == pos.ScreenIndex.X &&
    //    //    //            preparedScreens[i].Position.Y == pos.ScreenIndex.Y)
    //    //    //        {
    //    //    //            lastCheckedPreparedScreen = i;
    //    //    //            return preparedScreens[i].GetBlock(pos);
    //    //    //        }
    //    //    //    }
    //    //    //}
    //    //    return null;
    //    //}
    //}


    
    //class NewPreparedScreenQue : Engine.UpdateQue
    //{
    //    List<PreparedScreen> preparedScreens;
    //    WorldPosition wp;
    //    public NewPreparedScreenQue(List<PreparedScreen> preparedScreens, WorldPosition wp)
    //        : base()
    //    {
    //        this.preparedScreens = preparedScreens;
    //        this.wp = wp;
    //    }
    //    public override void Update()
    //    {
    //        preparedScreens.Add(new PreparedScreen(wp));
    //    }
    //}

    //class CalculateShadowsQue : Engine.UpdateQue
    //{
    //    PrepareTerrain terrain;
    //    List<PreparedBlock> pBLocks;
    //    int half;
    //    public CalculateShadowsQue(PrepareTerrain terrain)
    //        :base()
    //    {
    //        this.terrain = terrain;
    //    }
    //    public override void Update()
    //    {
    //        switch (currentUpdatePart)
    //        {
    //            case 0:
    //                //pBLocks = terrain.GetPreparedBlocks();
    //                half = pBLocks.Count / 2;
    //                break;
    //            case 1:
    //                terrain.CalculateShadows(pBLocks, 0, half);
    //                break;
    //            case  2:
    //                terrain.CalculateShadows(pBLocks, half, pBLocks.Count);
    //                break;

    //        }
            
            
    //    }
    //    protected override int NumParts
    //    {
    //        get
    //        {
    //            return 3;
    //        }
    //    }
    //}
    //class ResetShadowsQue : Engine.UpdateQue
    //{
    //    PrepareTerrain screen;
    //    public ResetShadowsQue(PrepareTerrain screen)
    //        :base()
    //    {
    //        this.screen = screen;
    //    }
    //    public override void Update()
    //    {
    //        //foreach (PreparedScreen s in screen.preparedScreens)
    //        //{
    //        //    s.ResetShadows();
    //        //}
            
    //    }
    //}
}
