using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Interface;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class CreatureSpawner : AbsGameObject
    {
        public bool needRespawn = false;

        public CreatureSpawner(PlaceObjectData placementData)
            : base(placementData)
        {
            pointer = new ObjectPointer() { hasValue = true, objIndex = BlackRef.mapData.spawnerList.Add(this) };
            BlackRef.mapData.tileGrid.Get(placementData.mapPlacement.tilePos).tileEffect = Map.TileEffect.Spawner;

            BlackRef.playScene.mapmodel.floorNeedsUpdate = true;
        }

        public override void DeleteMe()
        {
            //base.DeleteMe();
            BlackRef.mapData.tileGrid.Get(placementData.mapPlacement.tilePos).tileEffect = Map.TileEffect.None;
            BlackRef.mapData.spawnerList.RemoveAt(pointer.objIndex);

            BlackRef.playScene.mapmodel.floorNeedsUpdate = true;
        }

        public override bool RefreshUiDisplay(IOdisplay display)
        {
            display.AddSpawn(placementData.mapPlacement);
            return true;
        }

        public void OnRunStart()
        {
            spawn();
        }

        public void OnCycleStart()
        {
            if (needRespawn)
            {
                spawn();                
            }
        }

        void spawn()
        {
            PlaceObjectData place = placementData;
            place.component = new Mission.ToolSetupComponent(placementData.spawn);
            var creature = ObjectBuilder.Create(placementData, false) as AbsCreature;
            if (creature != null)
            {
                creature.spawner = pointer;
            }

            needRespawn = false;
        }

        public override FactoryObjectType FactoryObjectType => FactoryObjectType.CreatureSpawner;
    }
}
