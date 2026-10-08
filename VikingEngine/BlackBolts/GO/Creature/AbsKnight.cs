using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    abstract class AbsKnight : AbsCreature
    {
        abstract protected bool IsGoodSide { get; }
        virtual protected DestroyType DestroyType => DestroyType.Default;

        virtual protected int AttackFrame => 1;

        public AbsKnight(PlaceObjectData placementData)
            : base(placementData)
        {
        }
        override public void refreshAnimation()
        {
            animation = new Animation(2, 5, 120);
        }
        public override bool CheckAttackAction(AbsCreature counterAttackFrom)
        {
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
                if (BlackRef.mapData.tileGrid.TryGet(placement.tilePos, out var tile))
                {
                    if (tile.pCreature.hasValue)
                    {
                        var otherCreature = tile.pCreature.GetCreature();
                        if (FactoryObjectLib.Get(otherCreature.FactoryObjectType).IsEnemyTarget(IsGoodSide))
                        {
                            if (otherCreature.pointer.objIndex > this.pointer.objIndex)
                            {
                                //Havent done their attack yet
                                otherCreature.CheckAttackAction(this);
                            }
                            DestructionLaws.Destroy(otherCreature, tile, DestroyType);

                            onAttack(placement);
                            return true;
                        }
                    }

                    if (IsGoodSide && tile.pMachine.hasValue)
                    {
                        var machine = tile.pMachine.GetMachine();
                        if (FactoryObjectLib.Get(machine.FactoryObjectType).IsEnemyTarget(IsGoodSide))
                        {
                            DestructionLaws.Destroy(machine, tile, DestroyType);

                            onAttack(placement);
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
                    if (FactoryObjectLib.Get(otherCreature.FactoryObjectType).IsEnemyTarget(IsGoodSide))
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
            nextPos = currentPos;
            model.Frame = AttackFrame;
            refreshPos();
        }

        public override bool WillMoveItems()
        {
            return false;
        }
    }
}
