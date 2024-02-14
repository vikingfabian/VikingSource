using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    class JumpTutFather : AbsSpeechNpc
    {
        public JumpTutFather(GoArgs args)
            : base(args)
        {
            //loadImage();
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.father, 0f, 1f);
            postImageSetup();

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        //protected override void loadImage()
        //{
        //    image = new Graphics.VoxelModelInstance(
        //        LfRef.Images.StandardModel_Character);

        //    new Process.LoadImage(this, VoxelModelName.father, BasicPositionAdjust);
        //}

        protected override void startSpeechDialogue(PlayerCharacter.AbsHero hero)
        {
            base.startSpeechDialogue(hero);
            //speechbobble.attackTutorial();
            //speechbobble.specialAttackTutorial();
            speechbobble.jumpTutorial();
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.JumpTutFather; }
        }

        override protected float scale
        {
            get
            {
                return 0.16f;
            }
        }
    }
}
