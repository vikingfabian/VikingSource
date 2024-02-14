using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.NPC
{
    class EmoSuitSmith : AbsSpeechNpc
    {
        public const int CraftingIngotCount = 5;
        
        PickUp.SuitBox suit = null;

        public EmoSuitSmith(GoArgs args)
            :base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.weaponsmith, 0f, 1f);
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
        //    new Process.LoadImage(this, VoxelModelName.weaponsmith, BasicPositionAdjust);
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.EmoSuitSmith; }
        }

        

        override protected float scale
        {
            get
            {
                return 0.16f;
            }
        }

        

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return (Level == null || Level.collect.collectedCount < CraftingIngotCount)?  
                    base.InteractVersion2_interactIcon : SpriteName.LfCraftItemIcon;
            }
        }

        

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            //start speech dialogue
            if (start)
            {
                if (Level.collect.collectedCount < CraftingIngotCount)
                {
                    startSpeechDialogue(hero);
                    //targetHero = hero;
                    //SoundLib.NpcChatSound.PlayFlat();
                    //speechbobble = new Display.NpcSpeechBobble(hero.Player);
                    //speechbobble.craftEmoSuit();
                }
                else
                {
                    if (hero.player.Gear.suit.Type != SuitType.Emo &&
                        (suit == null || suit.IsDeleted))
                    {
                        SoundLib.LargeSuccessSound.PlayFlat();
                        Vector3 startPos = image.position;
                        startPos.Y += 2;
                        startPos += VectorExt.V2toV3XZ(lib.AngleToV2( AngleDirToObject(hero), 1.6f));
                        suit = new PickUp.SuitBox(new GoArgs(startPos), SuitType.Emo);
                    }
                }
            }
        }

        protected override void startSpeechDialogue(PlayerCharacter.AbsHero hero)
        {
            base.startSpeechDialogue(hero);
            speechbobble.craftEmoSuit();
        }

        protected override void Interact2_PromptHero(PlayerCharacter.AbsHero hero)
        {
            base.Interact2_PromptHero(hero);
            if (hero.Level != null &&
                hero.player.interactDisplay is Display.NpcSpeechBobble == false)
            {
                hero.player.refreshLevelCollectItem();
            }
        }
        
    }
}
