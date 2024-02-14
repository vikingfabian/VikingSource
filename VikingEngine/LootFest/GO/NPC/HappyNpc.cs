using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.NPC
{
    class HappyNpc : Characters.AbsCharacter
    {
        //happy_npc_female.vox
        Time jumpTime = new Time(1f, TimeUnit.Seconds);

        public HappyNpc(GoArgs args)
            : base(args)
        {
            WorldPos = args.startWp;
            modelScale = Ref.rnd.Float(3.2f, 3.7f);
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.happy_npc_female,
                modelScale, 0f, false);
            image.position = WorldPos.PositionV3;

            createNpcBounds(modelScale);

            new BabyChild(this, modelScale);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            if (localMember)
            {
                var h = GetClosestHero(false);
                if (h != null)
                {
                    rotateTowardsObject(h, 0.01f, 0f);
                    setImageDirFromRotation();
                    if (jumpTime.CountDown())
                    {
                        physics.Jump(0.8f, image);
                        jumpTime.Seconds = Ref.rnd.Float(1f, 1.2f);
                    }
                }
            }
            UpdateAllChildObjects();
        }

        public bool isFalling
        {
            get
            {
                if (localMember)
                {
                    return physics.PhysicsStatusFalling;
                }
                else
                {
                    return clientGoalPosition.Y < image.position.Y;
                }
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.HappyNpc; }
        }
    }

    /// <summary>
    /// A child thats a child object to a parent parent :)
    /// </summary>
    class BabyChild : VikingEngine.LootFest.GO.AbsChildObject
    {
        public Graphics.AbsVoxelObj model;
        Vector3 posOffset;
        HappyNpc parentParent;

        public BabyChild(HappyNpc parentParent, float parentModelScale)
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.baby, 
                parentModelScale * 0.5f, 0, false);

            posOffset = new Vector3(0, 0.2f, 0.2f) * parentModelScale;
            parentParent.AddChildObject(this);
            this.parentParent = parentParent;
            
        }
        public override bool ChildObject_Update(Characters.AbsCharacter parent)
        {
            var rotation = parent.RotationQuarterion;

            model.position = rotation.TranslateAlongAxis(posOffset, parent.Position);
            model.Rotation = rotation;
            model.Rotation.RotateWorldX(MathHelper.PiOver2);


            model.Frame = parentParent.isFalling ? 1 : 0;


            return false;
        }
        public override void ChildObject_OnParentRemoval(Characters.AbsCharacter parent)
        {
            model.DeleteMe();
        }
    }
}
