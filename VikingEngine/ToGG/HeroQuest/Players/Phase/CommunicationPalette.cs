using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine.Display3D;

namespace VikingEngine.ToGG.HeroQuest.Players.Phase
{
    class CommunicationPalette : AbsPlayerPhase
    {
        static readonly EmoteType[] EmotesGroup1 = new EmoteType[]
            {
                EmoteType.Smile,
                EmoteType.Wink,
                EmoteType.Laught,
                EmoteType.Angry,
                EmoteType.Cry,
                EmoteType.Evil,
                EmoteType.Snore,                
            };

        static readonly EmoteType[] EmotesGroup2 = new EmoteType[]
            {
                EmoteType.ThumbUp,
                EmoteType.ThumbDown,
                EmoteType.Love,
                EmoteType.Question,
            };

        static readonly string[] Expressions = new string[]
            {
                "Hello!",
                "Thanks",
                "Sorry",
                "Need health",
                "Need stamina",
                "Pushing forward",
                "Lootfest!",
            };

        const float FadeTime = 200;

        Graphics.Image blackFade;
        Graphics.Motion2d fadeIn;
        List<CommunicationButton> emotes, expressions;
        //LocalPlayer player;

        public CommunicationPalette(LocalPlayer player)
            : base(player)
        {
        }

        public override void onBegin()
        {
            
            var screenAr = Engine.Screen.Area;
            screenAr.AddRadius(2);
            blackFade = new Graphics.Image(SpriteName.WhiteArea, screenAr.Position, screenAr.Size,
                HudLib.OptionsPaletteLayer + 3);
            blackFade.ColorAndAlpha(Color.Black, 0.0f);
            fadeIn = new Graphics.Motion2d(Graphics.MotionType.OPACITY, blackFade,
                new Vector2(0.4f), Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);

            VectorRect centerArea = VectorRect.FromCenterSize(Engine.Screen.CenterScreen, Engine.Screen.IconSizeV2 * 3f);
            float areaSpacing = Engine.Screen.BorderWidth * 2;
            float buttonHeight = Engine.Screen.IconSize;
            float buttonSpacing = Engine.Screen.BorderWidth;

            emotes = new List<CommunicationButton>(EmotesGroup1.Length + EmotesGroup2.Length);
            {
                int RowCount = 4;
                Vector2 emotesTopLeft = VectorExt.AddX(centerArea.RightTop, areaSpacing);
                IntVector2 grindex = IntVector2.Zero;

                foreach (var m in EmotesGroup1)
                {
                    emoteButton(m);
                }

                grindex.Grindex_NewRow();

                foreach (var m in EmotesGroup2)
                {
                    emoteButton(m);
                }

                void emoteButton(EmoteType emote)
                {
                    SpriteName content, bobble;
                    Color bgCol;
                    UnitMessageEmote.EmoteSettings(emote, out content, out bobble, out bgCol);

                    CommunicationButton button = new CommunicationButton(
                        new VectorRect(
                         emotesTopLeft + grindex.Vec * (buttonHeight + buttonSpacing),
                        new Vector2(buttonHeight)), content, bgCol, (int)emote);
                    emotes.Add(button);
                    grindex.Grindex_Next(RowCount);
                }
            }

            expressions = new List<CommunicationButton>(Expressions.Length);
            {
                int RowCount = 3;

                float buttonW = buttonHeight * 4f;

                Vector2 topLeft = VectorExt.AddX(centerArea.Position, 
                    -(areaSpacing + Table.TotalWidth(RowCount, buttonW, buttonSpacing)));
                IntVector2 grindex = IntVector2.Zero;

                Vector2 sz = new Vector2(buttonW, buttonHeight);
                Vector2 spacing = VectorExt.Add(sz, buttonSpacing);

                for (int i = 0; i < Expressions.Length; ++i)
                {
                    CommunicationButton button = new CommunicationButton(
                       new VectorRect(topLeft + grindex.Vec * spacing, sz),
                       Expressions[i], Color.White, i);
                    expressions.Add(button);
                    grindex.Grindex_Next(RowCount);
                }
            }
        }

        public override void update(ref PlayerDisplay display)
        {
            foreach (var m in emotes)
            {
                if (m.update())
                {
                    communicationOption(CommunicationType.Emote, m.link);
                    end();
                }
            }

            foreach (var m in expressions)
            {
                if (m.update())
                {
                    communicationOption(CommunicationType.TextExpression, m.link);
                    end();
                }
            }

            if (toggRef.inputmap.back.DownEvent || 
                toggRef.inputmap.communications.DownEvent)
            {
                end();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            fadeIn.DeleteMe();
            new Graphics.Motion2d(Graphics.MotionType.OPACITY, blackFade,
                new Vector2(-blackFade.Opacity), Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
            new Timer.Terminator(FadeTime * 2, blackFade);

            arraylib.DeleteAndClearArray(emotes);
            arraylib.DeleteAndClearArray(expressions);
        }

        void communicationOption(CommunicationType type, int uType)
        {
            Unit unit = player.HeroUnit;

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqCommunicate, 
                Network.PacketReliability.Reliable);
            unit.netWriteUnitId(w);
            w.Write((byte)type);
            w.Write((byte)uType);

            UseCommunication(unit, type, uType);
        }

        public static void NetRead(System.IO.BinaryReader r)
        {
            Unit unit = Unit.NetReadUnitId(r);
            CommunicationType type = (CommunicationType)r.ReadByte();
            int uType = r.ReadByte();

            if (unit != null)
            {
                UseCommunication(unit, type, uType);
            }
        }

        public static void UseCommunication(Unit unit, CommunicationType type, int uType)
        {
            switch (type)
            {
                case CommunicationType.Emote:
                    new UnitMessageEmote(unit, (EmoteType)uType);
                    break;
                case CommunicationType.TextExpression:
                    new UnitMessageRichbox(unit, Expressions[uType]);
                    break;
            }
        }

        public override PhaseType Type => PhaseType.Communications;
    }

    class CommunicationButton : Display.AbsButton
    {
        public int link;

        public CommunicationButton(VectorRect area, SpriteName icon, Color bgCol, int link)
            : base(area, HudLib.OptionsPaletteLayer)
        {
            this.link = link;
            var iconAr = area;
            iconAr.AddRadius(-4);
            var iconImg = new Graphics.Image(icon, iconAr.Position, iconAr.Size, ImageLayers.AbsoluteBottomLayer);
            iconImg.LayerAbove(baseImage);
            imagegroup.Add(iconImg);

            init(bgCol);
        }

        public CommunicationButton(VectorRect area, string text, Color bgCol, int link)
                    : base(area, HudLib.OptionsPaletteLayer)
        {
            this.link = link;

            Graphics.Text2 textImg = new Graphics.Text2(text, LoadedFont.Regular,
                area.Center, Engine.Screen.TextBreadHeight, Color.Black, ImageLayers.AbsoluteBottomLayer);
            textImg.LayerAbove(baseImage);
            textImg.OrigoAtCenter();
            imagegroup.Add(textImg);
            
            init(bgCol);
        }

        void init(Color bgCol)
        {
            baseImage.Color = bgCol;
            var outline = new Graphics.RectangleLines(this.area, 2, 0, HudLib.OptionsPaletteLayer - 2);
            outline.setColor(Color.Black);

            imagegroup.Add(outline);
        }

    }

    enum CommunicationType
    {
        Emote,
        TextExpression,
    }
}
