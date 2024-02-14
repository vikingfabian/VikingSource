using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display3D
{
    class UnitMessageEmote : AbsUnitMessage
    {
        static readonly Color YellowBobbleCol = new Color(239, 243, 90);
        static readonly Color RedBobbleCol = new Color(243, 112, 90);

        const float ModelWidth = 0.28f;
        const float ModelWtoH = 1.4f;

        public UnitMessageEmote(AbsUnit unit, EmoteType emote)
            : base(unit)
        {
            start();

            SpriteName content, bobble;
            Color bgCol;

            EmoteSettings(emote, out content, out bobble, out bgCol);

            const int TexWidth = 32;
            
            Graphics.Image bg = new Graphics.Image(bobble, Vector2.Zero, new Vector2(TexWidth, TexWidth * 2f), 
                ImageLayers.Background0, false, false);
            Graphics.Image img = new Graphics.Image(content, Vector2.Zero, new Vector2(TexWidth),
                ImageLayers.Lay0, false, false);

            model = new Graphics.RenderTargetBillboard(basePos,
                ModelWidth, true);
            model.FaceCamera = false;
            model.Rotation = toggLib.PlaneTowardsCam;
            model.images = new List<Graphics.AbsDraw> { bg, img };

            model.createTexture(new Vector2(TexWidth, TexWidth * ModelWtoH), model.images, null);
            model.setModelSizeFromTexWidth();

            completeInit(unit);
        }

        public static void EmoteSettings(EmoteType emote, out SpriteName content, out SpriteName bobble, out Color bgCol)
        {
            int color_0Y_1R_2DR_3W;

            switch (emote)
            {
                case EmoteType.Smile:
                    content = SpriteName.unitEmoteSmile;
                    color_0Y_1R_2DR_3W = 0;
                    break;
                case EmoteType.Angry:
                    content = SpriteName.unitEmoteAngry;
                    color_0Y_1R_2DR_3W = 1;
                    break;
                case EmoteType.Snore:
                    content = SpriteName.unitEmoteSnore;
                    color_0Y_1R_2DR_3W = 0;
                    break;
                case EmoteType.Cry:
                    content = SpriteName.unitEmoteCry;
                    color_0Y_1R_2DR_3W = 0;
                    break;
                case EmoteType.Laught:
                    content = SpriteName.unitEmoteLaugh;
                    color_0Y_1R_2DR_3W = 0;
                    break;
                case EmoteType.Wink:
                    content = SpriteName.unitEmoteWink;
                    color_0Y_1R_2DR_3W = 0;
                    break;
                case EmoteType.Evil:
                    content = SpriteName.unitEmoteEvil;
                    color_0Y_1R_2DR_3W = 1;
                    break;
                case EmoteType.ThumbUp:
                    content = SpriteName.unitEmoteThumbUp;
                    color_0Y_1R_2DR_3W = 3;
                    break;
                case EmoteType.ThumbDown:
                    content = SpriteName.unitEmoteThumbDown;
                    color_0Y_1R_2DR_3W = 3;
                    break;
                case EmoteType.Love:
                    content = SpriteName.unitEmoteLove;
                    color_0Y_1R_2DR_3W = 3;
                    break;
                case EmoteType.Question:
                    content = SpriteName.unitEmoteQuestion;
                    color_0Y_1R_2DR_3W = 3;
                    break;
                case EmoteType.Alerted:
                    content = SpriteName.unitEmoteAlerted;
                    color_0Y_1R_2DR_3W = 2;
                    break;
                default:
                    throw new NotImplementedExceptionExt("EmoteSettings " + emote.ToString(), (int)emote);
            }

            switch (color_0Y_1R_2DR_3W)
            {
                case 0:
                    bobble = SpriteName.speachBobbleYellow;
                    bgCol = YellowBobbleCol;
                    break;
                case 1:
                    bobble = SpriteName.speachBobbleRed;
                    bgCol = RedBobbleCol;
                    break;
                case 2:
                    bobble = SpriteName.speachBobbleDarkRed;
                    bgCol = RedBobbleCol;
                    break;
                default://case 2:
                    bobble = SpriteName.speachBobbleWhite;
                    bgCol = Color.White;
                    break;                    
            }
        }

        public override float MessageHeight => ModelWidth * ModelWtoH;
    }

    enum EmoteType
    {
        Smile,
        Angry,
        Snore,
        Cry,
        Laught,
        Wink,
        Evil,
        ThumbUp,
        ThumbDown,
        Love,
        Question,
        Alerted,
        NUM,
    }
}
