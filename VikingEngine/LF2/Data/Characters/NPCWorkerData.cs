using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data.Characters
{
    //abstract class AbsNPCWorker : NPCdata
    //{
        


    //    protected const string StrongWeaponTip = "To create a strong weapon you need high quality materials";
    //    protected const string LowestQualityTip = "The result will be based on the material of the lowest quality, 'a chain isn't stronger than its weakest link' - you know?!";
    //    protected const string AvoidMixingTip = "Avoid mixing in low quality materials with the high quality ones";
        
    //    public AbsNPCWorker(NPCdataArgs args)
    //        : base(args)
    //    { }
        

    //    public List<Gadgets.BluePrint> BluePrints;
    //    abstract public List<string> Phrases { get; }

    //    public override GameObjects.NPC.AbsNPC2 generate(Map.WorldPosition wp)
    //    {
    //        return new GameObjects.NPC.Worker(wp, this);
    //    }
    //}

    //class WorkerVulcanSmith : AbsNPCWorker
    //{
        
    //    public WorkerVulcanSmith(NPCdataArgs args)
    //        : base(args)
    //    {
    //        BluePrints = new List<Gadgets.BluePrint> { 
    //            Gadgets.BluePrint.EnchantedWoodSword, Gadgets.BluePrint.EnchantedAxe, Gadgets.BluePrint.EnchantedSword, Gadgets.BluePrint.EnchantedLongSword, 
    //            Gadgets.BluePrint.EnchantedLongbow, Gadgets.BluePrint.EnchantedMetalbow, };
    //    }


    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Volcan_smith; }
    //    }

    //    override public List<string> Phrases
    //    {
    //        get
    //        {
    //            return new List<string>
    //            {
    //                "its written in the stars",
    //                "Feel the energy flow",
    //                "Your body should be one with the nature",
                
    //            };
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return "Vulcan Smith";
    //    }

    //    override public Voxels.VoxelObjGridData WorkingStation { get { return vulcan_station; } }
    //}

    //class WorkerWeaponSmith : AbsNPCWorker
    //{
    //    public WorkerWeaponSmith(NPCdataArgs args)
    //        :base(args)
    //    {
    //        BluePrints = new List<Gadgets.BluePrint> { 
    //            Gadgets.BluePrint.WoodSword, Gadgets.BluePrint.Axe, Gadgets.BluePrint.Sword, Gadgets.BluePrint.LongSword, };
    //    }

      
    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith; }
    //    }

    //    override public List<string> Phrases
    //    {
    //        get
    //        {
    //            return new List<string>
    //            {
    //                "What!? Talk louder so I can hear you!",
    //                "You got some steel I can hammer on?",
    //                StrongWeaponTip, LowestQualityTip, AvoidMixingTip
    //            };
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return "Weapon Smith";
    //    }

    //    override public  Voxels.VoxelObjGridData WorkingStation { get { return blacksmith_station; } }
    //}
  
    //class WorkerCook : AbsNPCWorker
    //{
    //    public WorkerCook(NPCdataArgs args)
    //        : base(args)
    //    {

    //        BluePrints = new List<Gadgets.BluePrint> { 
    //            Gadgets.BluePrint.GrilledApple, 
    //            Gadgets.BluePrint.GrilledMeat, 
    //            Gadgets.BluePrint.ApplePie, 
    //            Gadgets.BluePrint.Bread, 
    //            Gadgets.BluePrint.Wine, };
    //    }

      
    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Cook; }
    //    }

    //    override public List<string> Phrases
    //    {
    //        get
    //        {
    //            return new List<string>
    //            {
    //                 "I will make your food taste much better!",
    //                "The key to cooking good food is to taste often",
    //                "Only an animal would eat raw food",
    //                "My apple pie is delicious!",
    //            };
    //        }
    //    }
    //    public const string Name = "Cook";
    //    public override string ToString()
    //    {
    //        return Name;
    //    }

    //    override public  Voxels.VoxelObjGridData WorkingStation { get { return cook_station; } }
    //}
    

    //class WorkerArmour : AbsNPCWorker
    //{
    //    public WorkerArmour(NPCdataArgs args)
    //        : base(args)
    //    {

    //        BluePrints = new List<Gadgets.BluePrint> { 
    //            Gadgets.BluePrint.LeatherHelmet, 
    //            Gadgets.BluePrint.MetalHelmet, 
    //            Gadgets.BluePrint.Gambison, 
    //            Gadgets.BluePrint.BodyArmour, };
    //    }

      
    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Armour_maker; }
    //    }
    //    override public List<string> Phrases
    //    {
    //        get
    //        {
    //            return new List<string>
    //            {
    //                "Armour can be made from both animal skin and metal",
    //                "Gems can give you better protection agains magic",
    //                "A helmet is cheap but useful",
    //                "A full body armour will requier a huge amount of material",
    //            };
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return "Armour Crafter";
    //    }

    //    override public  Voxels.VoxelObjGridData WorkingStation { get { return armour_station; } }
    //}
   
    //class WorkerBowmaker : AbsNPCWorker
    //{
    //    public WorkerBowmaker(NPCdataArgs args)
    //        : base(args)
    //    {
    //        BluePrints = new List<Gadgets.BluePrint> { 
    //            Gadgets.BluePrint.Arrow, 
    //            Gadgets.BluePrint.SlingStone, 
    //            Gadgets.BluePrint.Javelin, 
    //            Gadgets.BluePrint.Sling, 
    //            Gadgets.BluePrint.ShortBow, 
    //            Gadgets.BluePrint.LongBow, 
    //            Gadgets.BluePrint.MetalBow, };
    //    }

      
    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker; }
    //    }
    //    override public List<string> Phrases
    //    {
    //        get
    //        {
    //            return new List<string>
    //            {
    //                            "A long bow is stronger than a short",
    //                            "Slingshots are weak but very cheap",
    //                            "If you use high quality material for your arrows, you might get a bonus",
    //            };
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return "Bow maker";
    //    }

    //    override public  Voxels.VoxelObjGridData WorkingStation { get { return bow_station; } }
    //}

    //class WorkerGoldSmith : AbsNPCWorker
    //{
    //    public WorkerGoldSmith(NPCdataArgs args)
    //        : base(args)
    //    {

    //        BluePrints = new List<Gadgets.BluePrint> { Gadgets.BluePrint.Ring, };
    //    }

    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Gold_smith; }
    //    }
    //    override public List<string> Phrases
    //    {
    //        get
    //        {
    //            return new List<string>
    //            {
    //                            "Magic rings are rare and expensive",
    //                            "My rings will enhance your skills",
    //            };
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return "Gold Smith";
    //    }

    //    override public  Voxels.VoxelObjGridData WorkingStation { get { return goldsmith_station; } }
    //}
    
    //class WorkerShieldMaker : AbsNPCWorker
    //{
    //    public WorkerShieldMaker(NPCdataArgs args)
    //        :base(args)
    //    {

    //        BluePrints = new List<Gadgets.BluePrint> { Gadgets.BluePrint.ShieldBuckle, Gadgets.BluePrint.ShieldRound, 
    //            Gadgets.BluePrint.ShieldSquare };
    //    }

      
    //    public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
    //    {
    //        get { return GameObjects.EnvironmentObj.MapChunkObjectType.Shield_maker; }
    //    }
    //    override public List<string> Phrases
    //    {
    //        get
    //        {
    //            return new List<string>
    //            {
    //                            "A shield will give powerful protection against both arrows and swords",
    //                            "Larger shields are better",
    //                            "Your shield defence will be weakend when you do an attack",
    //            };
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return "Shield maker";
    //    }

    //    override public  Voxels.VoxelObjGridData WorkingStation { get { return sheild_station; } }
    //}
}
 