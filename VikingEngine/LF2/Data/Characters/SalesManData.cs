using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data.Characters
{
    //abstract class AbsSalesManData : NPCdata
    //{
        


    //    protected GameObjects.Gadgets.ShopCollection shopCollection;
    //    GameObjects.Gadgets.WannaBuyColl sellCollection = null;
    //    public AbsSalesManData(GameObjects.Gadgets.ShopProfile shopProfile, NPCdataArgs args)
    //        :base(args)
    //    {
    //        shopCollection = new GameObjects.Gadgets.ShopCollection(shopProfile);
    //        if (shopProfile == GameObjects.Gadgets.ShopProfile.Salesman)
    //            sellCollection = new GameObjects.Gadgets.WannaBuyColl();
    //    }
    //    public override GameObjects.NPC.AbsNPC2 generate(Map.WorldPosition wp)
    //    {
    //        return new GameObjects.NPC.SalesMan(wp, shopCollection, sellCollection, this);
    //    }
    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Salesman; }
    //    }

    //    override public bool NeedToBeStored { get { return true; } }
        
    //}
    //class SalesManData : AbsSalesManData
    //{
    //    public SalesManData(NPCdataArgs args)
    //        : base(GameObjects.Gadgets.ShopProfile.Salesman, args)
    //    {

    //    }
    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Salesman; }
    //    }
    //    override public Voxels.VoxelObjGridData WorkingStation { get { return salesman_station; } }
    //}
    //class LumberjackData : AbsSalesManData
    //{
    //    public LumberjackData(NPCdataArgs args)
    //        : base(GameObjects.Gadgets.ShopProfile.Lumberjack, args)
    //    {

    //    }

    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Salesman; }
    //    }
    //    override public Voxels.VoxelObjGridData WorkingStation { get { return lumberjack_station; } }
    //}
    //class MinerData : AbsSalesManData
    //{
    //    public MinerData(NPCdataArgs args)
    //        : base(GameObjects.Gadgets.ShopProfile.Miner, args)
    //    {

    //    }

    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Salesman; }
    //    }
    //    override public Voxels.VoxelObjGridData WorkingStation { get { return miner_station; } }
    //}
}
