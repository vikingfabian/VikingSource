using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.BlockMap;
using VikingEngine.LootFest.BlockMap.Level;

namespace VikingEngine.LootFest.GO.PickUp
{
    class Key : AbsHeroPickUp
    {
        bool firstArea;
        //AbsLevel level;
        VoxelModelName modelName = VoxelModelName.NUM_NON;

        public Key(GoArgs args, bool firstArea, AbsLevel level, VoxelModelName modelName)
            :base(args)
        {
           // managedGameObject = true;
            SetAsManaged();
            this.modelName = modelName;
            this.firstArea = firstArea;
            this.levelCollider = new LevelPointer(level);
            //levelEnum = level.LevelEnum;
            startSetup(args);
        }

        protected override VoxelModelName imageType
        {
            get { return modelName; }
        }
        //public //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(
        //    new Color(255, 228, 0), new Vector3(1, 1, 0.2f));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}



        public override GameObjectType Type
        {
            get { return GameObjectType.Key; }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        protected override bool heroPickUp(PlayerCharacter.AbsHero hero)
        {
            hero.foundItemAnimation(imageType, Scale, false);
            Level.keyCount.Value++;
            hero.player.refreshKeyCount(new IntVector2(Level.keyCount.Value));
            return true;
        }

        protected override bool giveStartSpeed
        {
            get
            {
                return false;
            }
        }

        protected override bool timedRemoval
        {
            get
            {
                return false;
            }
        }
        protected override bool autoMoveTowardsHero
        {
            get
            {
                return false;
            }
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.NO_PHYSICS;
            }
        }

        const float Scale = 2f;
        override protected float imageScale { get { return Scale; } }
    }
}
