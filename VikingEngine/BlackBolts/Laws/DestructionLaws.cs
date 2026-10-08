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
        
        public static void Destroy(AbsGameObject target, Tile tile, DestroyType destroyType)
        {
            var objProp = FactoryObjectLib.Get(target.GameObjectType);
            if (objProp.isCreature)
            {
                tile.pCreature.hasValue = false;

                switch (target.GameObjectType)
                {
                    case FactoryObjectType.WhiteKnight:
                    case FactoryObjectType.BlackKnight:
                        switch (destroyType)
                        {
                            case DestroyType.Default:
                                Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.DssDamage, VectorExt.AddY(target.model.position, 0.3f), 0.3f, 100, 0.8f);
                                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Flesh);
                                break;
                            case DestroyType.Fire:
                                Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Fire, VectorExt.AddY(target.model.position, 0.3f), 0.3f, 100);
                                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Grilled_meat);
                                break;
                            case DestroyType.Void:
                                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Job_knight);
                                break;
                        }
                        break;

                    case FactoryObjectType.GoblinWorker:
                    case FactoryObjectType.Dragon:
                        Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.DssDamage, VectorExt.AddY(target.model.position, 0.3f), 0.3f, 100, 0.8f);
                        ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Flesh);
                        break;

                    case FactoryObjectType.NightDemon:
                        //No drop, turn to black hole
                        break;
                }

                
            }
            else
            {   
                tile.pMachine.hasValue = false;

                ResourceLaws.DropResource(target.currentPos.tilePos, ResourceType.Rubble);
            }

            target.DeleteMe();
        }

       
    }
}
