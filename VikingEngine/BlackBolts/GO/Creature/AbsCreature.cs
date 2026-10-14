using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    abstract class AbsCreature : AbsGameObject
    {
        public MapPlacement beltPos;
        public Fluid stompSmear = Fluid.Empty;
        protected Animation animation;

        public ObjectPointer spawner = ObjectPointer.Empty;

        public AbsCreature(PlaceObjectData placementData)
            : base(placementData)
        {
            this.currentPos = placementData.mapPlacement;
            nextPos = currentPos;
            
        }

        protected void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.DirToQuaterion(model, currentPos.direction);

            RefreshResourcePos();
        }
        public override void OnResourceChanged()
        {
            base.OnResourceChanged();
            refreshAnimation();
        }

        virtual public void refreshAnimation()
        {
            animation = new Animation(1, 5, 120);
            if (pResource.hasValue)
            {
                animation.Add(5);
            }
        }

        public override void TweenUpdate(bool beltMove, float tween)
        {
            base.TweenUpdate(beltMove, tween);

            if (!beltMove && nextPos != currentPos)
            {
                animation.update(Ref.DeltaGameTimeMs, model, out _);
            }

            RefreshResourcePos();
        }

        virtual public void FinalizeMove()
        {
            bool didMove = currentPos.tilePos != nextPos.tilePos;
            currentPos = nextPos;
            var tile = BlackRef.mapData.tileGrid.Get(currentPos.tilePos);
            tile.pCreature = pointer;
            refreshPos();

            if (didMove)
            {
                //Check stomp
                if (stompSmear.HasValue)
                {
                    if (tile.fluid.resourceType != stompSmear.resourceType ||
                        tile.fluid.amount == 0)
                    {
                        tile.fluid = stompSmear;
                        BlackRef.playScene.mapmodel.decalsNeedsUpdate = true;
                        stompSmear.amount--;
                    }
                }
                else if (tile.fluid.HasValue)
                {
                    stompSmear = tile.fluid.GetOne();
                }

                if (tile.pResource.hasValue)
                {
                    var resource = tile.pResource.GetSolidResource();
                    var resProp = ResourceLib.Get(resource.placementData.resourceType);
                    if (resProp.stompEffect)
                    {                        
                        resource.DeleteMe();
                        tile.pResource.hasValue = false;
                        tile.fluid = new Fluid( ResourceType.FluidPoopStain);
                        stompSmear = tile.fluid;

                        BlackRef.missionSetup.runStatistics.poopStomps++;
                        BlackRef.playScene.mapmodel.decalsNeedsUpdate = true;
                    }
                    else if (resProp.isJob)
                    {
                        onJobItem(tile, resource);
                    }
                }
            }
        }

        virtual protected void onJobItem(Tile tile, SolidResource job)
        { }

        public bool NoMovement()
        {
            return currentPos.tilePos == nextPos.tilePos;
        }

        public override void RefreshResourcePos()
        {
            if (pResource.hasValue)
            {
                var resource = pResource.GetSolidResource();
                resource.model.Rotation = model.Rotation;
                resource.model.position = model.Rotation.TranslateAlongAxis(
                    new Vector3(0, 0.25f, 0.4f), model.position);
            }
        }

        static Vector3 diff = new Vector3(0, 0.4f, 0.4f);
        public override Vector3 ResourceOffset()
        {
            return diff;
        }

        abstract public bool WillMoveItems();

        /// <summary>
        /// The attack will replace the move
        /// </summary>
        virtual public bool CheckAttackAction(AbsCreature counterAttackFrom) { return false; }

        /// <summary>
        /// Bumping into another creature
        /// </summary>
        virtual public void onWalkingIntoSameTile(List<ObjectPointer> collideWidth, List<TwoCreatures> preparedAttackers)
        {
            nextPos = currentPos.TurnAroundPos();
        }

        virtual public void applyAttack(AbsCreature otherCreature)
        {  }

        public override void DeleteMe()
        {
            base.DeleteMe();
            BlackRef.mapData.creatureList.RemoveAt(pointer.objIndex);

            if (spawner.hasValue)
            {
                var spawnerObj = BlackRef.mapData.spawnerList.GetIndex_Safe(spawner.objIndex);
                spawnerObj?.SetRespawn();
                
            }
        }
    }
}
