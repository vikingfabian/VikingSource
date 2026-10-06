using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    

    class IO_unit : AbsMachine
    {
        int productionPoints = 0;
        List<VoxelModelInstance> resourceIcons = new List<VoxelModelInstance>();
        List<IO_port> ports = new List<IO_port>(4);

        public override void AnimateUpdate()
        {
        }

        public IO_unit(PlaceObjectData placementData)
           : base(placementData)
        {
            this.currentPos = placementData.mapPlacement;
            tilesize = new IntVector2(2, 1);
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_onetile], true);
            model.scale = new Vector3(1.3f * model.SizeToScale)/* * VectorExt.V2toV3XZ(tilesize.Vec)*/;
            model.Frame = 0;
            

            //Hard code ports
            ports.Add(new IO_port(true, new MapPlacement(new IntVector2(0, 0), Dir4.W),
                 ResourceType.Flesh, 1));
            ports.Add(new IO_port(false, new MapPlacement(new IntVector2(1, 0), Dir4.E),
                ResourceType.Grilled_meat, 1));

            refreshPos();

        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);

            for (int i = 0; i < ports.Count; ++i)
            {
                var port = ports[i];
                port.refreshPlacement(currentPos);
                ports[i] = port;
            }


            foreach (var p in ports)
            {
                SolidResource.ResourceModel(p.resourceType, out LootFest.VoxelModelName modelName, out int frame, out float scale);
                VoxelModelInstance resmodel = new VoxelModelInstance(BlackRef.models.voxelModels[modelName], true);
                resmodel.scale = new Vector3(0.4f * scale * resmodel.SizeToScale);
                resmodel.Frame = frame;

                resmodel.position = WP.TileToWp(p.mapPlacement.ForwardPos().tilePos);
                resmodel.position.Y = 0.1f;

                resourceIcons.Add(resmodel);
            }


        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            foreach (var res in resourceIcons)
            {
                res.DeleteMe();
            }
        }

        public override void OnCykleEnd()
        {
            base.OnCykleEnd();

            //OUT
            if (productionPoints > 0)
            {
                bool allFree = true;
                foreach (var port in ports)
                {
                    if (!port.input)
                    {
                        var toPos = port.mapPlacement.ForwardPos();
                        bool canDrop = ResourceManager.CanDispenceResource(toPos.tilePos);
                        if (!canDrop)
                        {
                            allFree = false;
                            break;
                        }
                    }
                }

                if (allFree)
                {
                    for (int i = 0; i < ports.Count; ++i)
                    {
                        var port = ports[i];
                        if (!port.input)
                        {
                            ResourceManager.DispenceResource(port.mapPlacement.ForwardPos().tilePos, port.resourceType);
                        }
                    }

                    productionPoints--;
                }
            }

            //IN

            if (productionPoints > 0)
            {
                return;
            }

            //Check sourronding tiles for input, then output if filled
            for (int i = 0; i < ports.Count; ++i)
            {
                var port = ports[i];
                if (port.input && !port.filled())
                {
                    MapPlacement inputPos = port.mapPlacement.ForwardPos();
                    inputPos.FlipDir();

                    var inTile = BlackRef.mapData.tileGrid.Get(inputPos.tilePos);
                    //Input by belt, floor drop or worker
                    //Todo: feed from io unit to another
                    if (inTile.pResource.hasValue && inTile.pMachine.hasValue)
                    {
                        var resource = inTile.pResource.GetSolidResource();
                        if (port.resourceType == resource.placementData.resourceType ||
                            port.resourceType == ResourceType.Any)
                        {
                            var machine = inTile.pMachine.GetMachine();
                            if ((machine.GameObjectType == FactoryObjectType.Belt ||
                                machine.GameObjectType == FactoryObjectType.Floor_drop)
                                && machine.currentPos.direction == inputPos.direction)
                            {
                                inTile.pResource.hasValue = false;
                                feedResource(resource, i);
                                continue;
                            }
                        }
                    }

                    if (inTile.pCreature.hasValue)
                    {
                        var creature = inTile.pCreature.GetCreature();
                        if (creature.currentPos.direction == inputPos.direction &&
                            creature.pResource.hasValue)
                        {
                            var resource = creature.pResource.GetSolidResource();
                            if (port.resourceType == resource.placementData.resourceType ||
                                port.resourceType == ResourceType.Any)
                            {
                                creature.pResource.hasValue = false;
                                feedResource(resource, i);
                                continue;
                            }
                        }
                    }
                }
            }

            bool allFilled = true;
            foreach (var port in ports)
            {
                if (port.input)
                {
                    if (!port.filled())
                    {
                        allFilled = false;
                        break;
                    }
                }
            }

            if (allFilled)
            {
                for (int i = 0; i < ports.Count; ++i)
                {
                    var port = ports[i];
                    port.collected = 0;
                    ports[i] = port;
                }

                productionPoints++;
            }
        
            void feedResource(SolidResource resource, int toPort)
            {
                var port = ports[toPort];
                port.collected++;
                ports[toPort] = port;
                resource.DeleteMe();
            }
        }

        public override bool WalkableTile()
        {
            return false;
        }

        public override FactoryObjectType GameObjectType => FactoryObjectType.IOunit;
    }
}
