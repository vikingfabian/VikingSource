using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.NPC
{
    /// <summary>
    /// Click on the character to raise a flag
    /// </summary>
    class CheckPointNpc : AbsGameObject
    {
        Effects.SleepingZZZ sleepingEffect;
        bool isActivated = false;
        Timer.Basic sleepAnimation = new Timer.Basic(800, true);

        public CheckPointNpc(GoArgs args)
            : base(args)
        {
            args.startWp.SetAtClosestFreeY(0);
            args.updatePosV3();
            args.startPos.Y += 0.6f;

            WorldPos = args.startWp;
            modelScale = 4f;
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.checkpoint_npc, modelScale);
            image.position = args.startPos;

            Vector3 boundSz = VectorExt.V3FromWidthAndHeight(modelScale * 0.2f, modelScale * 0.3f);
            CollisionAndDefaultBound = new Bounds.ObjectBound(Bounds.BoundShape.Cylinder, VectorExt.AddY(image.position, boundSz.Y), boundSz, Vector3.Zero);

            sleepingEffect = new Effects.SleepingZZZ(this);
        }

        public override void Time_Update(UpdateArgs args)
        {
            //base.Time_Update(args);

            if (!isActivated)
            {
                if (sleepAnimation.Update())
                {
                    image.Frame = lib.AlternateBetweenTwoValues(image.Frame, 0, 1);
                }

                if (args.halfUpdate == halfUpdateRandomBool)
                {
                    Interact2_SearchPlayer(false);

                    if (checkOutsideUpdateArea_ActiveChunk(WorldPos.ChunkGrindex))
                    {
                        DeleteMe();
                    }
                    
                }
            }

        }

        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return VectorExt.V3FromY(3f);
            }
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
            SolidBodyCheck(args.allMembersCounter);
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LfCheckpointFlag;
            }
        }

        public override bool Interact_Enabled
        {
            get
            {
                return true;
            }
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            //base.InteractVersion2_interactEvent(hero, start);

            if (start)
            {
                isActivated = true;
                image.Frame = 2;

                sleepingEffect.DeleteMe();
                sleepingEffect = null;

                Engine.ParticleHandler.AddExpandingParticleArea(VikingEngine.Graphics.ParticleSystemType.GoldenSparkle, VectorExt.AddY(image.position, 2f), 1.5f, 30, 2f);

                new Sound.SoundSettings(LoadedSound.CraftSuccessful, 1f).PlayFlat();
                hero.checkPoint = WorldPos.ChunkGrindex;
            }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (sleepingEffect != null)
            {
                sleepingEffect.DeleteMe();
            }
        }

        public override void HandleColl3D(Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //base.HandleColl3D(collData, ObjCollision);
        }

        override public VikingEngine.Graphics.LightParticleType LightSourceType { get { return Graphics.LightParticleType.Shadow; } }
        public override float LightSourceRadius { get { return image.scale.X * 11; } }
        public override Vector3 LightSourcePosition
        {
            get
            {
                return image.position;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.CheckPointNpc; }
        }
    }
}
