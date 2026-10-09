using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class Dragon : AbsCreature
    {
        const int AttackRange = 3;

        public Dragon(PlaceObjectData placementData)
            : base(placementData)
        {
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_dragon], true);

            model.scale = new Vector3(1.2f * model.SizeToScale);

            refreshPos();
        }
        override public void refreshAnimation()
        {
            animation = new Animation(2, 5, 120);
        }
        public override bool CheckAttackAction(AbsCreature counterAttackFrom)
        {
            //Burger check
            if (checkFood(currentPos))
            {
                return true;
            }
            if (checkFood(currentPos.ForwardPos()))
            {
                return true;
            }
            if (checkFood(currentPos.LeftPos()))
            {
                return true;
            }
            if (checkFood(currentPos.RightPos()))
            {
                return true;
            }

            //Attack check
            if (checkDir(currentPos.ForwardPos()))
            {
                return true;
            }
            if (checkDir(currentPos.LeftPos()))
            {
                return true;
            }
            if (checkDir(currentPos.RightPos()))
            {
                return true;
            }

            return false;

            bool checkDir(MapPlacement placement)
            {
                for (int length = 0; length < AttackRange; ++length)
                {
                    if (BlackRef.mapData.tileGrid.TryGet(placement.tilePos, out var tile))
                    {
                        if (tile.pCreature.hasValue)
                        {
                            var otherCreature = tile.pCreature.GetCreature();
                            if (FactoryObjectLib.Get(otherCreature.FactoryObjectType).IsEnemyTarget(false))
                            {
                                if (otherCreature.pointer.objIndex > this.pointer.objIndex)
                                {
                                    //Havent done their attack yet
                                    otherCreature.CheckAttackAction(this);
                                }
                                DestructionLaws.Destroy(otherCreature, tile, DestroyType.Fire);

                                onAttack(placement);
                                return true;
                            }
                        }
                    }
                    placement = placement.ForwardPos();
                }
                return false;
            }

            bool checkFood(MapPlacement placement)
            {
                if (BlackRef.mapData.tileGrid.TryGet(placement.tilePos, out var tile))
                {
                    if (tile.pResource.hasValue)
                    {
                        var res = tile.pResource.GetSolidResource();
                        if (res.placementData.resourceType == ResourceType.Grilled_meat)
                        {
                            tile.pResource.hasValue = false;
                            res.DeleteMe();
                            BlackRef.mapData.tileGrid.Get(currentPos.tilePos).pCreature.hasValue = false;
                            //this.DeleteMe();
                            BlackRef.mapData.creatureList.RemoveAt(pointer.objIndex);
                            new FlyingDragonEffect(this);
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public override void onWalkingIntoSameTile(List<ObjectPointer> collideWidth, List<TwoCreatures> preparedAttackers)
        {
            foreach (var other in collideWidth)
            {
                if (other != pointer)
                {
                    var otherCreature = other.GetCreature();
                    if (FactoryObjectLib.Get(otherCreature.FactoryObjectType).IsEnemyTarget(false))
                    {
                        preparedAttackers.Add(new TwoCreatures() { creature1 = this, creature2 = otherCreature });
                        return;
                    }
                }
            }

            base.onWalkingIntoSameTile(collideWidth, preparedAttackers);
        }
        public override void applyAttack(AbsCreature otherCreature)
        {
            DestructionLaws.Destroy(otherCreature,
                BlackRef.mapData.tileGrid.Get(otherCreature.currentPos.tilePos), DestroyType.Default);

            onAttack(currentPos);
        }


        void onAttack(MapPlacement attackPos)
        {
            currentPos.direction = attackPos.direction;
            MapPlacement fireEffect = currentPos.ForwardPos();
            for (int i = 0; i < AttackRange; ++i)
            {
                Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Fire, VectorExt.AddY(WP.TileToWp(fireEffect.tilePos), 0.3f), 0.5f, 20);

                if (fireEffect.tilePos != attackPos.tilePos)
                {
                    ResourceLaws.ConvertResource(fireEffect.tilePos, Data.ResourceType.Heat);
                }
                fireEffect = fireEffect.ForwardPos();
            }
            nextPos = currentPos;
            model.Frame = 0;
            refreshPos();
        }

        public override bool WillMoveItems()
        {
            return false;
        }

        public override FactoryObjectType FactoryObjectType =>  FactoryObjectType.Dragon;
    }
}
