using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.GO.Creature;
using VikingEngine.Core.BlackBolts.Map;

namespace VikingEngine.Core.BlackBolts.Laws
{
    
    static class DestructionLaws
    {

        public static void ConvertCreature(AbsGameObject target, Tile tile, FactoryObjectType convertTo)
        {
            tile.pCreature.hasValue = false;
            target.DeleteMe();

            ObjectBuilder.Create(
                new PlaceObjectData() 
                { 
                    mapPlacement = target.currentPos, 
                    component = new Mission.ToolSetupComponent(convertTo)
                }, false, true);

            if (tile.pCreature.hasValue)
            { 
                tile.pCreature.GetCreature().refreshAnimation();
            }
        }

        public static void Destroy(AbsGameObject target, Tile tile, DestroyType destroyType)
        {
            var objProp = FactoryObjectLib.Get(target.FactoryObjectType);
            if (objProp.isCreature)
            {
                tile.pCreature.hasValue = false;

                switch (target.FactoryObjectType)
                {
                    case FactoryObjectType.WhiteKnight:
                    case FactoryObjectType.BlackKnight:
                        switch (destroyType)
                        {
                            case DestroyType.Default:
                                Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.DssDamage, VectorExt.AddY(target.model.position, 0.3f), 0.3f, 100, 0.8f);
                                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Flesh);
                                ResourceLaws.DropFluid(target.currentPos.tilePos, ResourceType.FluidBlood);
                                break;

                            case DestroyType.Fire:
                                Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Fire, VectorExt.AddY(target.model.position, 0.3f), 0.3f, 100);
                                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Grilled_meat);
                                break;

                            case DestroyType.Void:
                                new Render.VoidEffect(target.currentPos.tilePos);
                                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Job_knight);
                                break;
                        }
                        break;

                    case FactoryObjectType.GoblinWorker:
                    case FactoryObjectType.Dragon:
                        Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.DssDamage, VectorExt.AddY(target.model.position, 0.3f), 0.3f, 100, 0.8f);
                        ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Flesh);
                        ResourceLaws.DropFluid(target.currentPos.tilePos, ResourceType.FluidBlood);
                        break;

                    case FactoryObjectType.NightDemon:
                        //No drop, turn to black hole
                        new Render.VoidEffect(target.currentPos.tilePos);
                        break;
                }

                
            }
            else
            {
                //tile.pMachine.hasValue = false;
                RemoveMachineMapPointers(target);
                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Rubble);
            }

            target.DeleteMe();
        }

        public static void RemoveMachineMapPointers(AbsGameObject target)
        {
            ForXYLoop loop = new ForXYLoop(MapPlacement.CoverArea(target.placementData.mapPlacement, target.tilesize));
            while (loop.Next())
            {
                if (!BlackRef.mapData.tileGrid.Get(loop.Position).pMachine.hasValue)
                {
                    throw new Exception();
                }
                BlackRef.mapData.tileGrid.Get(loop.Position).pMachine.hasValue = false;
            }
        }

       
    }
}
