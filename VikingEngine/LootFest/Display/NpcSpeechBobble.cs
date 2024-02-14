using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Display
{
    class NpcSpeechBobble : AbsInteractDisplay
    {
        Graphics.ImageGroup images;
        Graphics.Image npcHead, headShadow, bobbleWhite;

        bool firstFrame = true;
        Timer.Basic animateTimer = new Timer.Basic(150, true);
        
        Vector2 bobblePos, bobbleSz;
        SpriteName head1, head2;

        public NpcSpeechBobble(Players.Player p)
            :base(p)
        {
            inputToRemove_notTimed = true;
            refresh(p, null);
        }

        public override void refresh(Players.Player player, object sender)
        {
            Color BobbleFillCol = new Color(255, 250, 238);

            if (images == null)
            {
                Vector2 bgSz = new Vector2(player.ScreenArea.Width, player.ScreenArea.Height * 0.25f);
                Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, new Vector2(player.ScreenArea.X, player.ScreenArea.Bottom - bgSz.Y),
                    bgSz, LfLib.Layer_GuiMenu);
                bg.Color = ColorExt.GrayScale(48);

                float topEdgeH = (int)(bgSz.Y * 0.035f);
                Graphics.Image topEdge = new Graphics.Image(SpriteName.WhiteArea, bg.Position, new Vector2(bg.Width, topEdgeH), ImageLayers.AbsoluteBottomLayer);
                topEdge.LayerAbove(bg);
                topEdge.Color = ColorExt.GrayScale(128);

                bobbleSz = new Vector2((int)(bgSz.Y * 2.6f), (int)(bgSz.Y * 0.7f));
                bobblePos = new Vector2((bg.Width - bobbleSz.X) * 0.5f, bg.Ypos + (bg.Height - bobbleSz.Y) * 0.45f);

                Vector2 npcSz = new Vector2(bgSz.Y * 0.85f);
                bobblePos.X += npcSz.X * 0.5f;

                bobbleWhite = new Graphics.Image(SpriteName.WhiteArea,
                    bobblePos, bobbleSz, LfLib.Layer_GuiMenu - 1);
                bobbleWhite.Color = BobbleFillCol;

                images = new Graphics.ImageGroup(bg, topEdge, bobbleWhite);

                float bobbleEdgeW = (int)(topEdgeH * 0.8f);
                Vector2 shadowPosDiff = new Vector2(bobbleEdgeW * 1.2f);

                for (Dir4 dir = 0; dir < Dir4.NUM_NON; ++dir)
                {
                    Graphics.Image bobbleEdge = (Graphics.Image)bobbleWhite.CloneMe();
                    images.Add(bobbleEdge);
                    bobbleEdge.LayerBelow(bobbleWhite);
                    bobbleEdge.Color = topEdge.Color;
                    bobbleEdge.Position += IntVector2.FromDir4(dir).Vec * bobbleEdgeW;

                    if (dir == Dir4.E || dir == Dir4.S)
                    {
                        Graphics.Image bobbleShadow = (Graphics.Image)bobbleEdge.CloneMe();
                        images.Add(bobbleShadow);
                        bobbleShadow.LayerBelow(bobbleEdge);
                        bobbleShadow.Color = Color.Black;
                        bobbleShadow.Position += shadowPosDiff;
                    }
                }

                Graphics.Image arrow = new Graphics.Image(SpriteName.LfNpcSpeechArrow,
                    new Vector2(bobbleWhite.Xpos + 2, bobbleWhite.Ypos + bobbleWhite.Height * 0.7f),
                    new Vector2(bobbleWhite.Height * 0.3f), ImageLayers.AbsoluteTopLayer, true);
                arrow.Xpos -= arrow.Width * 0.5f;
                arrow.Color = BobbleFillCol;
                images.Add(arrow);
                arrow.LayerAbove(bobbleWhite);

                Graphics.Image arrowShadow = (Graphics.Image)arrow.CloneMe();
                images.Add(arrowShadow);
                arrowShadow.Size *= new Vector2(1.5f, 1.6f);
                arrowShadow.Color = topEdge.Color;
                arrowShadow.PaintLayer = bobbleWhite.PaintLayer + PublicConstants.LayerMinDiff * 2f;

                npcHead = new Graphics.Image(SpriteName.LfWeaponSmithHead1, 
                    new Vector2(bobblePos.X - npcSz.X * 1.4f, player.ScreenArea.Bottom - npcSz.Y),
                    npcSz, ImageLayers.AbsoluteBottomLayer);
                images.Add(npcHead);
                npcHead.LayerAbove(topEdge);

                headShadow = (Graphics.Image)npcHead.CloneMe();
                images.Add(headShadow);
                headShadow.LayerBelow(npcHead);
                headShadow.Color = Color.Black;
                headShadow.Position += shadowPosDiff;

                Vector2 buttonSz = new Vector2(Engine.Screen.IconSize);
                var inputIcon = new VikingEngine.Graphics.Image(player.inputMap.interact.Icon,//.ButtonIcon( VikingEngine.Input.ButtonActionType.GameInteract),
                    new Vector2(bobbleWhite.Right + buttonSz.X * 0.5f, bobbleWhite.Bottom - buttonSz.Y + topEdgeH),buttonSz, ImageLayers.AbsoluteBottomLayer, 
                    false, true);
                images.Add(inputIcon);
                inputIcon.LayerAbove(bg);

            }
        }

        public void attackTutorial()
        {
            head1 = SpriteName.LfVeteranHead1; head2 = SpriteName.LfVeteranHead2;

            Vector2 iconSz = new Vector2(bobbleSz.Y * 0.45f);

            Vector2 position = bobblePos + new Vector2(bobbleSz.X * 0.3f, bobbleSz.Y * 0.5f);

            Graphics.Image inputIcon = new Graphics.Image(player.inputMap.mainAttack.Icon,//.ButtonIcon(Input.ButtonActionType.GameMainAttack), 
                position, iconSz, ImageLayers.AbsoluteBottomLayer, true);

            images.Add(inputIcon);
            inputIcon.LayerAbove(bobbleWhite);

            Graphics.Motion2d bumpMotion = new Graphics.Motion2d(Graphics.MotionType.SCALE, inputIcon, inputIcon.Size * 0.12f, 
                Graphics.MotionRepeate.BackNForwardLoop, 200, true);

            Vector2 imageSz = new Vector2(bobbleSz.Y * 0.9f);
            position.X += inputIcon.Width * 0.5f + imageSz.X * 0.6f;

            Graphics.Image heroImage = new Graphics.Image(SpriteName.LfTutPrimaryAttack, position, imageSz, ImageLayers.AbsoluteTopLayer, true);
            images.Add(heroImage);
            heroImage.LayerAbove(bobbleWhite);

            position.X += imageSz.X * 0.9f;
            Graphics.Image targetImage = new Graphics.Image(SpriteName.LfTutBrokenTarget, position, imageSz, ImageLayers.AbsoluteTopLayer, true);
            images.Add(targetImage);
            targetImage.LayerAbove(bobbleWhite);

            npcHead.SetSpriteName(head1);
        }

        public void specialAttackTutorial()
        {
            head1 = SpriteName.LfGranpaHead1; head2 = SpriteName.LfGranpaHead2;

            Vector2 iconSz = new Vector2(bobbleSz.Y * 0.45f);

            Vector2 position = bobblePos + new Vector2(bobbleSz.X * 0.15f, bobbleSz.Y * 0.5f);

            Graphics.Image inputIcon = new Graphics.Image(player.inputMap.altAttack.Icon,//.ButtonIcon(Input.ButtonActionType.GameAlternativeAttack),
                position, iconSz, ImageLayers.AbsoluteBottomLayer, true);

            images.Add(inputIcon);
            inputIcon.LayerAbove(bobbleWhite);

            Graphics.Motion2d bumpMotion = new Graphics.Motion2d(Graphics.MotionType.SCALE, inputIcon, inputIcon.Size * 0.12f,
                Graphics.MotionRepeate.BackNForwardLoop, 200, true);

            Vector2 imageSz = new Vector2(bobbleSz.Y * 0.9f);
            position.X += inputIcon.Width * 0.5f + imageSz.X * 1f;

            Graphics.Image heroImage = new Graphics.Image(SpriteName.LfTutSpecialAttack, position, new Vector2(imageSz.Y / 4 * 6, imageSz.Y), ImageLayers.AbsoluteTopLayer, true);
            images.Add(heroImage);
            heroImage.LayerAbove(bobbleWhite);

            position.X += heroImage.Width * 0.9f;
            Graphics.Image targetImage = new Graphics.Image(SpriteName.LfTutBrokenTarget, position, imageSz, ImageLayers.AbsoluteTopLayer, true);
            images.Add(targetImage);
            targetImage.LayerAbove(bobbleWhite);

            npcHead.SetSpriteName(head1);
        }


        public void jumpTutorial()
        {
            head1 = SpriteName.LfFatherHead1; head2 = SpriteName.LfFatherHead2;

            Vector2 iconSz = new Vector2(bobbleSz.Y * 0.45f);

            Vector2 position = bobblePos + new Vector2(bobbleSz.X * 0.3f, bobbleSz.Y * 0.5f);

            Graphics.Image inputIcon = new Graphics.Image(player.inputMap.jump.Icon,//.ButtonIcon(Input.ButtonActionType.GameJump),
                position, iconSz, ImageLayers.AbsoluteBottomLayer, true);

            images.Add(inputIcon);
            inputIcon.LayerAbove(bobbleWhite);

            Graphics.Motion2d bumpMotion = new Graphics.Motion2d(Graphics.MotionType.SCALE, inputIcon, inputIcon.Size * 0.12f,
                Graphics.MotionRepeate.BackNForwardLoop, 200, true);

            Vector2 imageSz = new Vector2(bobbleSz.Y * 0.9f);
            position.X += inputIcon.Width * 0.5f + imageSz.X * 1f;

            Graphics.Image heroImage = new Graphics.Image(SpriteName.LfTutJump, position, imageSz, ImageLayers.AbsoluteTopLayer, true);
            images.Add(heroImage);
            heroImage.LayerAbove(bobbleWhite);

            npcHead.SetSpriteName(head1);
        }

        public void craftEmoSuit()
        {
            head1 = SpriteName.LfWeaponSmithHead1; head2 = SpriteName.LfWeaponSmithHead2;

            Vector2 iconSz = new Vector2(bobbleSz.Y * 0.45f);

            Vector2 position = bobblePos + new Vector2(iconSz.X, bobbleSz.Y * 0.5f);

            for (int i = 0; i < VikingEngine.LootFest.GO.NPC.EmoSuitSmith.CraftingIngotCount; ++i)
            {
                Graphics.Image ingot = new Graphics.Image(SpriteName.LfMithrilIngot, position, iconSz, ImageLayers.AbsoluteBottomLayer, true);
                images.Add(ingot);
                ingot.LayerAbove(bobbleWhite);

                position.X += iconSz.X;
            }

            Graphics.Image craftArrow = new Graphics.Image(SpriteName.LfNpcCraftArrow, position, iconSz, ImageLayers.AbsoluteBottomLayer, true);
            images.Add(craftArrow);
            craftArrow.LayerAbove(bobbleWhite);

            position.X += iconSz.X * 1.2f; 

            Graphics.Image sword = new Graphics.Image(SpriteName.LfEmoIcon1, position, iconSz * 1.5f, ImageLayers.AbsoluteBottomLayer, true);
            images.Add(sword);
            sword.LayerAbove(bobbleWhite);

            npcHead.SetSpriteName(head1);
        }

        public void killTrollTutorial()
        {
            head1 = SpriteName.LfGranpaHead1; head2 = SpriteName.LfGranpaHead2;

            Vector2 iconSz = new Vector2(bobbleSz.Y * 0.6f);

            Vector2 position = bobblePos + new Vector2(bobbleSz.X * 0.5f, bobbleSz.Y * 0.5f);
            position.X -= iconSz.X;

            Graphics.Image sword = new Graphics.Image(SpriteName.LfEmoIcon1, position, iconSz * 1.5f, ImageLayers.AbsoluteBottomLayer, true);
            images.Add(sword);
            sword.LayerAbove(bobbleWhite);
            position.X += iconSz.X;

            Graphics.Image craftArrow = new Graphics.Image(SpriteName.LfNpcCraftArrow, position, iconSz, ImageLayers.AbsoluteBottomLayer, true);
            images.Add(craftArrow);
            craftArrow.LayerAbove(bobbleWhite);
            position.X += iconSz.X * 1.4f;

            Graphics.Image trollHead = new Graphics.Image(SpriteName.LfSadTrollFace, position, iconSz * 1.5f, ImageLayers.AbsoluteBottomLayer, true);
            images.Add(trollHead);
            trollHead.LayerAbove(bobbleWhite);

            npcHead.SetSpriteName(head1);
        }

        public override bool Update()
        {
            if (animateTimer.Update())
            {
                firstFrame = !firstFrame;
                SpriteName tile = firstFrame ? head1 : head2;
                npcHead.SetSpriteName(tile);
                headShadow.SetSpriteName(tile);

            }

            //if (minViewTime.CountDown())
            //{
            //    if ((startPos - player.hero.Position).Length() > 6 ||
            //        player.inputMap.DownEvent(VikingEngine.Input.ButtonActionType.GameInteract) ||
            //        player.inMenu)
            //    {
            //        return true;
            //    }
            //}
            return base.Update();
            //return false;
        }

        public override void DeleteMe()
        {
            images.DeleteAll();
            isDeleted = true;
        }

        public bool isDeleted = false;

        override public bool overrideInteractInput { get { return true; } }
    }
}
