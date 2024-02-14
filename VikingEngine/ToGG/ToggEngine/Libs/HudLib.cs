using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG
{
    static class HudLib
    {
        public const ImageLayers TutorialArrowLayer = ImageLayers.Top0;
        public const ImageLayers TooltipLayer = ImageLayers.Top5;
        public const ImageLayers TooltipBgLayer = TooltipLayer + 1;

        public const ImageLayers PopupLayer =  ImageLayers.Lay1;

        public const ImageLayers OptionsPaletteLayer = ImageLayers.Lay3;

        public const ImageLayers AttackWheelLayer = ImageLayers.Lay5;
        public const ImageLayers AttackWheelLabelLayer = AttackWheelLayer +2;

        public const ImageLayers BackpackLayer = ImageLayers.Lay5;

        public const ImageLayers DungeonMasterLayer = ImageLayers.Lay6;

        public const ImageLayers ContentLayer =  ImageLayers.Lay7;        
        public const ImageLayers BgLayer = ImageLayers.Lay8;

        public const ImageLayers StatusHudLayer = ImageLayers.Lay9;

        public static Vector2 cardSize;
        public static float textHeight;
        public static Vector2 PrevPhaseButtonsSz, NextPhaseButtonsSz;
        public static float PhaseButtonsSpacing;
        public static VectorRect cardContentArea;
        public static HUD.ButtonGuiSettings ButtonGuiSettings_old;
        public static HUD.ButtonGuiSettings ButtonGuiSettings;

        public const float SelectionOutlineThickness = 4f;
        public static readonly Color TitleTextBronze = new Color(200, 198, 171);
        public static readonly Color AvailableGreenCol = Color.LightGreen;
        public static readonly Color UnavailableRedCol = new Color(255, 60, 60);
        public static HUD.RichBox.RichBoxSettings MouseTipRichBoxSett, 
            ButtonTipRichBoxSett, UnitMessageRichBoxSett, PopupRichBoxSett;
        public const float ConvertArrowScale = 0.74f;

        public static HUD.UiStyleGuide style;

        public static void Init()
        {
            cardSize = new Vector2(5, 1) * Engine.Screen.Height * 0.12f;
            textHeight = cardSize.Y * 0.25f;
            float edgeSz = cardSize.Y * 0.04f;
            cardContentArea = new VectorRect(Vector2.Zero, cardSize);
            cardContentArea.AddRadius(-edgeSz);

            style = new HUD.UiStyleGuide();
            style.quickSetup(LoadedFont.Bold, TitleTextBronze, LoadedFont.Regular, Color.White);


            ButtonGuiSettings_old = new HUD.ButtonGuiSettings();
            ButtonGuiSettings_old.bgColor = Color.SaddleBrown;
            ButtonGuiSettings_old.highlightThickness = SelectionOutlineThickness;

            ButtonGuiSettings = new HUD.ButtonGuiSettings(Color.White, SelectionOutlineThickness, Color.White, Color.Red);
            
            NextPhaseButtonsSz = Engine.Screen.IconSizeV2 * 1.5f;
            PrevPhaseButtonsSz = NextPhaseButtonsSz * 0.5f;
            PhaseButtonsSpacing = cardSize.Y * 0.1f;

            const float TextToIconSz = 1.2f;

            MouseTipRichBoxSett = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.White, ColorExt.Empty),
                new TextFormat(),
                Engine.Screen.TextBreadHeight * TextToIconSz, 1.1f);
            MouseTipRichBoxSett.head1.Font = LoadedFont.Bold;
            MouseTipRichBoxSett.head1.Color = HudLib.TitleTextBronze;
            
            ButtonTipRichBoxSett = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.White, ColorExt.Empty),
                new TextFormat(),
                Engine.Screen.TextBreadHeight * TextToIconSz, Engine.Screen.TextTitleHeight / Engine.Screen.TextBreadHeight);
            ButtonTipRichBoxSett.head1.Font = LoadedFont.Bold;
            ButtonTipRichBoxSett.head1.Color = HudLib.TitleTextBronze;

            const int MessageRichBoxIconSz = 64;
            UnitMessageRichBoxSett = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, MessageRichBoxIconSz / TextToIconSz, Color.Black, ColorExt.Empty),
                new TextFormat(),
                MessageRichBoxIconSz, 1.1f);
            UnitMessageRichBoxSett.head1.Font = LoadedFont.Bold;
            UnitMessageRichBoxSett.head1.Color = HudLib.TitleTextBronze;

            PopupRichBoxSett = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.Black, ColorExt.Empty),
                new TextFormat(),
               Engine.Screen.TextBreadHeight * TextToIconSz, 1.1f);
            PopupRichBoxSett.head1.Font = LoadedFont.Bold;
            PopupRichBoxSett.head1.Color = HudLib.TitleTextBronze;
        }

        public static Graphics.ImageGroupParent2D HudCardBasics(Graphics.ImageGroupParent2D images, string name, SpriteName iconTile, float iconScale, out Graphics.Image icon)
        {
            if (images == null)
            {
                images = new Graphics.ImageGroupParent2D();
            }

            Graphics.Image background = new Graphics.Image(SpriteName.cmdCard5by1, Vector2.Zero, cardSize, BgLayer);
            images.Add(background);

            var sz = cardContentArea.Height * iconScale * DataLib.SpriteCollection.RatioFromLargestSide(iconTile);
            icon = new Graphics.Image(iconTile, cardContentArea.Position, sz, ContentLayer);
            icon.Ypos = cardContentArea.Center.Y - icon.Height * 0.5f;
            icon.Xpos = icon.Ypos * 0.2f;
            images.Add(icon);

            Graphics.TextG nameImg = new Graphics.TextG(LoadedFont.Regular, 
                new Vector2(cardContentArea.Height * 0.8f, cardContentArea.Y + 0.1f * Engine.Screen.IconSize),
                Vector2.One, Graphics.Align.Zero, name,
                Color.Black, ContentLayer);
            nameImg.SetHeight(textHeight * 1.2f);
            images.Add(nameImg);

            return images;
        }

        public static void ListSkillsText(List<string> texts, ref  Vector2 skillTextPos, Graphics.ImageGroupParent2D images)
        {
            const int MaxSkillsSpace = 3;

            if (texts.Count <= MaxSkillsSpace)
            {
                foreach (var t in texts)
                {
                    SkillText(t, ref skillTextPos, images);
                }
            }
            else
            {
                for (int i = 0; i < MaxSkillsSpace - 1; ++i)
                {
                    SkillText(texts[i], ref skillTextPos, images);
                }
                SkillText("+" + (texts.Count - (MaxSkillsSpace - 1)).ToString() + "...", ref skillTextPos, images);
            }
        }

        public static void SkillText(string text, ref  Vector2 skillTextPos, Graphics.ImageGroupParent2D images)
        {
            Graphics.TextG skillText = new Graphics.TextG(LoadedFont.Regular, skillTextPos,
                Vector2.One, Graphics.Align.Zero,
                text, Color.Black, HudLib.ContentLayer);
            skillText.SetHeight(textHeight);
            images.Add(skillText);

            skillTextPos.Y += HudLib.cardContentArea.Height * 0.25f;
        }

        public const int ThickBorderEdgeSize = 8;
        public const float BorderScale = 2f;
        const int BorderBgOpacity = 200;

        public static HUD.NineSplitAreaTexture ThickBorder(VectorRect area, ImageLayers layer)
        {
            var result = new HUD.NineSplitAreaTexture(SpriteName.cmdHudBorderThick, 1, ThickBorderEdgeSize, area, BorderScale, true, layer, false);
            result.addCenterColor(ThickBorderEdgeSize, new Color(27, 41, 53, BorderBgOpacity));

            return result;
        }

        public static HUD.NineSplitAreaTexture ThinBorder(VectorRect area, ImageLayers layer)
        {
            var result = new HUD.NineSplitAreaTexture(SpriteName.cmdHudBorderThin, 1, 2, area, BorderScale, true, layer, false);
            result.addCenterColor(2, new Color(55, 64, 73, 230));

            return result;
        }

        public const float TooltipBorderEdgeSize = 12;
        
        public static HUD.NineSplitAreaTexture TooltipBorder(VectorRect area, ImageLayers layer)
        {
            return new HUD.NineSplitAreaTexture(SpriteName.cmdHudBorderTooltip, 1, 5, area, BorderScale, true, layer, true);
        }

        public static Graphics.Image TooltipBorderArrow(VectorRect area, Vector2? pointAtCenter, Dir4 dir, ImageLayers layer)
        {
            Vector2 arrowSz;
            float arrowOffset;
            TooltipBorderArrowSize(out arrowSz, out arrowOffset);

            Vector2 center;
            VectorRect arrowbounds = area;
            arrowbounds.AddRadius(-arrowSz.X);

            if (dir == Dir4.N || dir == Dir4.S)
            {
                center = new Vector2(area.X + area.Width / 3f, area.Side(dir));

                if (pointAtCenter != null)
                {
                    center.X = Bound.Set( pointAtCenter.Value.X, arrowbounds.X, arrowbounds.Right);
                }
            }
            else
            {
                center = new Vector2(area.Side(dir), area.Y + area.Height / 3f);

                if (pointAtCenter != null)
                {
                    center.Y = Bound.Set(pointAtCenter.Value.Y, arrowbounds.Y, arrowbounds.Bottom);
                }
            }
            

            Graphics.Image arrowImage = new Graphics.Image(SpriteName.cmdHudTooltipArrow, center, arrowSz, layer);
            arrowImage.OrigoAtCenterWidth();
            arrowImage.ChangePaintLayer(-1);

            switch (dir)
            {
                case Dir4.N:
                    arrowImage.Ypos -= arrowOffset;
                    break;

                case Dir4.S:
                    arrowImage.OrigoAtBottom();
                    arrowImage.spriteEffects = SpriteEffects.FlipVertically;
                    arrowImage.Ypos += arrowOffset;
                    break;

                case Dir4.E:
                    arrowImage.Rotation = MathExt.TauOver4;
                    arrowImage.Xpos += arrowOffset;
                    break;
            }
            return arrowImage;
        }

        public static void TooltipBorderArrowSize(out Vector2 size, out float yOffset)
        {
            const int TexSz = 15;
            const int ArrowHeight = 7;

            size = new Vector2(TexSz * BorderScale);
            yOffset = ArrowHeight * BorderScale;
        }

        public static float ToolTipWidth => Engine.Screen.Height * 0.25f;

        public static void AddTooltipText(Graphics.ImageGroup tooltip, string title, string text, Dir4 dir, VectorRect area, 
            VectorRect? buttonArea = null)
        {            
            var members = new List<HUD.RichBox.AbsRichBoxMember>(2);
            
            if (title != null)
            {
                members.Add(new HUD.RichBox.RichBoxBeginTitle());
                members.Add(new HUD.RichBox.RichBoxText(title));
                members.Add(new HUD.RichBox.RichBoxNewLine(false));
            }
            members.Add(new HUD.RichBox.RichBoxText(text));

            AddTooltipText(tooltip, members, dir, area, buttonArea);
        }

        public static void AddTooltipText(Graphics.ImageGroup tooltip, 
            List<HUD.RichBox.AbsRichBoxMember> members, 
            Dir4 dir, VectorRect area, VectorRect? buttonArea = null, 
            bool centerToArea = false, bool bArrow = true)
        {
            if (buttonArea == null)
            {
                buttonArea = area;
            }

            HUD.RichBox.RichBoxGroup richBox = new HUD.RichBox.RichBoxGroup(Vector2.Zero, ToolTipWidth, HudLib.TooltipLayer,
                ButtonTipRichBoxSett, members);
            VectorRect tiparea = richBox.maxArea;

            Vector2 arrowSz;
            float arrowOffset;
            HudLib.TooltipBorderArrowSize(out arrowSz, out arrowOffset);

            bool rightSideOrientation = buttonArea.Value.Center.X < Engine.Screen.Width * 0.7f;
            float areaOffset = Engine.Screen.BorderWidth + arrowOffset + HudLib.TooltipBorderEdgeSize;

            switch (dir)
            {
                case Dir4.N:
                    if (rightSideOrientation)
                    {
                        tiparea.Position = VectorExt.AddY(buttonArea.Value.Position, -(areaOffset + tiparea.Size.Y));
                    }
                    else
                    {
                        tiparea.Position = buttonArea.Value.RightTop;
                        tiparea.Position.X -= tiparea.Size.X;
                        tiparea.Position.Y -= areaOffset + tiparea.Size.Y;
                    }
                    break;

                case Dir4.S:
                    if (centerToArea)
                    {
                        tiparea.Position = VectorExt.AddY(buttonArea.Value.CenterBottom, areaOffset);
                        tiparea.Position.X -= tiparea.Size.X * 0.5f;
                    }
                    else
                    {
                        if (rightSideOrientation)
                        {
                            tiparea.Position = VectorExt.AddY(buttonArea.Value.LeftBottom, areaOffset);
                        }
                        else
                        {
                            tiparea.Position = VectorExt.AddY(buttonArea.Value.RightBottom, areaOffset);
                            tiparea.Position.X -= tiparea.Size.X;
                        }
                    }
                    break;

                case Dir4.W:
                    tiparea.Position = VectorExt.AddX(buttonArea.Value.Position, -(areaOffset + tiparea.Size.X));

                    if (tiparea.Position.Y > Engine.Screen.CenterScreen.Y)
                    {
                        tiparea.Position.Y = buttonArea.Value.Bottom - tiparea.Size.Y;
                    }
                    break;

            }

            richBox.Move(tiparea.Position);
            tooltip.Add(richBox);

            tiparea.AddRadius(HudLib.TooltipBorderEdgeSize);
            var bg = HudLib.TooltipBorder(tiparea, HudLib.TooltipBgLayer);
            tooltip.Add(bg);

            if (bArrow)
            {
                var arrow = HudLib.TooltipBorderArrow(tiparea, area.Center, lib.Invert(dir), HudLib.TooltipLayer);
                tooltip.Add(arrow);
            }
        }

    }
    enum MapSquareAvailableType
    {
        None,
        Disabled,
        Enabled,
    }
}
