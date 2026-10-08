using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Interface;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    

    class IO_unit : AbsMachine
    {
        List<VoxelModelInstance> resourceIcons = new List<VoxelModelInstance>();
        List<IO_port> ports;
        ResourceType lastFedResource;

        public override void AnimateUpdate()
        {
        }

        public IO_unit(PlaceObjectData placementData)
           : base(placementData)
        {
            this.currentPos = placementData.mapPlacement;
            var template = placementData.machineId.GetTemplate();
            ports = new List<IO_port>(template.ports);           

            tilesize = template.tilesize;
            model = new VoxelModelInstance(template.model, true);
            model.Frame = template.frame;
            model.scale = new Vector3(template.scale * model.SizeToScale);

            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            IntVector2 offset = IntVector2.RotateVector_D4(this.tilesize - 1, (int)placementData.mapPlacement.direction);
            model.position += VectorExt.V2toV3XZ(offset.Vec * 0.5f);

            WP.DirToQuaterion(model, currentPos.direction);

            for (int i = 0; i < ports.Count; ++i)
            {
                var port = ports[i];
                port.refreshPlacement(currentPos);
                ports[i] = port;
            }
        }

        public override bool RefreshUiDisplay(IOdisplay display)
        {
            foreach (var p in ports)
            {
                display.AddInput(p);
            }
            return true;
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
            bool mayProduce = true;
            for (int i = 0; i < ports.Count; ++i)
            {
                var port = ports[i];
                if (!port.input && port.collected > 0)
                {
                    var toPos = port.mapPlacement.ForwardPos();
                    bool canDrop = ResourceLaws.CanDispenceResource(toPos.tilePos);
                    if (canDrop)
                    {
                        ResourceType resourceType = port.resourceType;
                        switch (resourceType)
                        {
                            case ResourceType.Any:
                                resourceType = lastFedResource;
                                break;
                            case ResourceType.Heat:
                                resourceType = ResourceLib.Get( lastFedResource).fireConvert.resource;
                                break;
                        }
                        ResourceLaws.DispenceResource(port.mapPlacement.ForwardPos().tilePos, resourceType);
                        port.collected--;
                        ports[i] = port;
                    }

                    mayProduce = false;
                }

            }

            if (!mayProduce)
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
                    if (port.input)
                    {
                        port.collected = 0;
                    }
                    else
                    {
                        port.collected = port.amount;
                    }
                    ports[i] = port;
                }

                //productionPoints++;
            }

            void feedResource(SolidResource resource, int toPort)
            {
                lastFedResource = resource.placementData.resourceType;
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
