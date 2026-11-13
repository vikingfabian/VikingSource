using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using VikingEngine.Core.Wand.Display.GameDisplay;
using VikingEngine.Core.Wand.GO.ObjectPointer;
using VikingEngine.Core.Wand.Laws;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Creature
{
    abstract class AbsCreature : AbsGameObject
    {
        StatusDisplay statusDisplay;
        public CreaturePointer pointer;
        public bool summoningSickness = false;

        public AbsCreature(CreatureType creatureType, MapPlacement placementData, int baseHealth)
        {
            this.currentPos = placementData;

            pointer.creatureType = creatureType;
            pointer.index = WandRef.world.newCreature(this);
            WandRef.world.map.tileGrid.Get(currentPos.tilePos).creature = pointer;

            health = new ValueBar(baseHealth);

            statusDisplay = new StatusDisplay();
            statusDisplay.refresh(this);
        }

        public void CloneStatusFrom(AbsCreature original)
        {
            this.currentPos.direction = original.currentPos.direction;
            health.ValueRemoved = original.health.ValueRemoved;
            refreshPos();
        }

        public void SetPos(MapPlacement placementData)
        {
            WandRef.world.map.tileGrid.Get(currentPos.tilePos).creature = CreaturePointer.Empty;
            this.currentPos = placementData;
            WandRef.world.map.tileGrid.Get(currentPos.tilePos).creature = pointer;
            refreshPos();
        }

        public void TurnTowards(IntVector2 toPos)
        {
            this.currentPos.direction = (toPos - this.currentPos.tilePos).ToDir4;
            refreshPos();
        }

        protected void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.DirToQuaterion(model, currentPos.direction);
        }

        public void updateUi()
        {
            statusDisplay.updatePosition(this);
        }

        abstract public Range AttackStrength();

        virtual public bool ChaseHero()
        {
            return true;
        }
        public override bool TakeDamage(int damage)
        {
            bool bDamage = base.TakeDamage(damage);
            if (bDamage)
            {
                PhysicsManager.Bleed(currentPos.tilePos);
                if (health.HasValue)
                {
                    statusDisplay.refresh(this);
                }
            }
            return bDamage;
        }
        public override void Heal(int addHealth)
        {
            base.Heal(addHealth);
            statusDisplay.refresh(this);
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
            statusDisplay.DeleteMe();
            WandRef.world.map.tileGrid.Get(currentPos.tilePos).creature = CreaturePointer.Empty;
            WandRef.world.creatures.RemoveAt(pointer.index);
        }

        public override GameObjectCategory ObjectCategory => GameObjectCategory.Creature;
    }
}
