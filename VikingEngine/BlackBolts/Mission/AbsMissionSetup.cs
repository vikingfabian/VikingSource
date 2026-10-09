using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.DSSWars;

namespace VikingEngine.Core.BlackBolts.Mission
{
    

    abstract class AbsMissionSetup
    {
        public IntVector2 mapSize = new IntVector2(25, 20);
        public string missionName;
        public List<ToolSetupComponent> componentList;
        public List<IOTemplate> ioUnits;

        public AbsMissionSetup()
        {
            BlackRef.missionSetup = this;
        }

        public void LoadMissionMap()
        {
            if (!IsSandbox)
            {
                BlackRef.storage.Load(false);
                for(int i = 0; i < BlackRef.storage.restorePoint.Count; ++i)//each (var m in BlackRef.storage.restorePoint)
                {
                    var m = BlackRef.storage.restorePoint[i];
                    m.locked = true;
                    BlackRef.storage.restorePoint[i] = m;
                }
            }
        }

        protected void mission1units()
        {
            {
                IOTemplate template = new IOTemplate() { name = "Heater", tilesize = new IntVector2(1, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.S),
                    ResourceType.Any, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Zero, Dir4.N),
                    ResourceType.Heat, 1));
                template.model = BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_onetile];
                template.frame = 7;
                template.scale = 1.6f;
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Chicken seperator", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Chicken, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.N),
                    ResourceType.Feather, 2));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.S),
                    ResourceType.Flesh, 1));
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Duplicator", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Any, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.N),
                    ResourceType.Any, 1));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.E),
                    ResourceType.Poop, 2));
                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.S),
                    ResourceType.Any, 1));
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Night eggs", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.N),
                    ResourceType.Magic_crystal, 1));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Void_cube, 2));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.S),
                    ResourceType.Feather, 1));

                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.E),
                    ResourceType.Night_egg, 1));
                ioUnits.Add(template);
            }
            {
                IOTemplate template = new IOTemplate() { name = "Dragon eggs", tilesize = new IntVector2(2, 1) };
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.N),
                    ResourceType.Fire_crystal, 2));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                    ResourceType.Grilled_meat, 1));
                template.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.S),
                    ResourceType.Feather, 1));

                template.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.E),
                    ResourceType.Dragon_egg, 1));

                ioUnits.Add(template);
            }

            foreach (var io in ioUnits)
            {
                io.buildModel();
                io.buildId();
            }
        }

        virtual public bool IsSandbox => false;
    }

    class TutorialSetup : AbsMissionSetup
    {
        public TutorialSetup()
             :base()
        {
            missionName = "tutorial";

            componentList = new List<ToolSetupComponent>
            {
                new ToolSetupComponent( FactoryObjectType.Spin_plate),
                new ToolSetupComponent( FactoryObjectType.Belt),
                new ToolSetupComponent( FactoryObjectType.Table),
                new ToolSetupComponent( FactoryObjectType.Floor_drop),
            };

            ioUnits = new List<IOTemplate>(0);
        }

    }
    class WhiteKnightSetup : AbsMissionSetup
    {
        public WhiteKnightSetup()
             : base()
        {
            missionName = "whiteknight";

            componentList = new List<ToolSetupComponent>
            {
                new ToolSetupComponent( FactoryObjectType.Spin_plate),
                new ToolSetupComponent( FactoryObjectType.Belt),
                new ToolSetupComponent( FactoryObjectType.Table),
                new ToolSetupComponent( FactoryObjectType.Floor_drop),
                new ToolSetupComponent( FactoryObjectType.Stone_pillar),
            };

            ioUnits = new List<IOTemplate>(8);
            mission1units();
        }

    }

    class SandboxSetup : AbsMissionSetup
    {
        public SandboxSetup()
            :base()
        {
            missionName = "sandbox";
            mapSize = new IntVector2(25, 20);
            componentList = new List<ToolSetupComponent>((int)FactoryObjectType.NUM_NONE);

            for (FactoryObjectType fobj = 0; fobj < FactoryObjectType.NUM_NONE; fobj++)
            {
                if (FactoryObjectLib.Get(fobj).debugLevel > ObjectDebugLevel.Incomplete)
                {
                    componentList.Add(new ToolSetupComponent(fobj));
                }
            }

            ioUnits = new List<IOTemplate>(8);
            mission1units();

            
        }

       

        public override bool IsSandbox => true;
    }

    
}
