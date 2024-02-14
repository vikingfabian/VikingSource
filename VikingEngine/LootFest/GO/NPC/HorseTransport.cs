using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.NPC
{
    class HorseTransport : AbsGameObject
    {
        Graphics.AbsVoxelObj driverModel;

        public HorseTransport(GoArgs args)
            : base(args)
        {
            args.startWp.SetAtClosestFreeY(0);
            args.updatePosV3();
            args.startPos.Y += 0.6f;

            WorldPos = args.startWp;
            modelScale = 11f;
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.transport_wagon, modelScale);
            image.position = args.startPos;

            Vector3 boundSz = new Vector3(modelScale * 0.16f, modelScale * 0.15f, modelScale * 0.45f);
            CollisionAndDefaultBound = new Bounds.ObjectBound(Bounds.BoundShape.Box1axisRotation, VectorExt.AddY(image.position, boundSz.Y), boundSz, Vector3.Zero);

            driverModel = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.wagondriver, modelScale * 0.3f);
            driverModel.position = image.position + modelScale * new Vector3(0f, 0.17f, -0.16f);
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact2_SearchPlayer(false);
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
                return SpriteName.LfTransportIcon;
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
            if (start)
            {
                hero.player.MenuSystem().listTransportLocations();
            }
        }

        public override void HandleColl3D(Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //base.HandleColl3D(collData, ObjCollision);
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            driverModel.DeleteMe();
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.HorseTransport; }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return NetworkShare.None;
            }
        }
    }
}
