using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    class Mother : AbsNPC
    {
        public Mother(GoArgs args)
            :base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.mother, 0f, 1f);
            //loadImage();
            postImageSetup();

            if (args.LocalMember)
            {
                socialLevel = SocialLevel.Follower;
                aggresive = Aggressive.Defending;
                NetworkShareObject();
            }
        }

        //protected override void loadImage()
        //{
        //    new Process.LoadImage(this, VoxelModelName.mother, BasicPositionAdjust);
        //}

        override protected float scale
        {
            get
            {
                return 0.15f;
            }
        }
        protected override float maxWalkingLength
        {
            get
            {
                return 4;
            }
        }

        protected override bool Immortal
        {
            get { return true; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Mother; }
        }
    }
}
