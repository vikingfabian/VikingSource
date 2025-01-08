using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    interface IUnitCardDisplay
    {
        void AddToUnitCard(UnitCardDisplay card, ref Vector2 position);
    }

    struct UnitDisplaySettings
    {
        public bool health, stamina, bloodrage, stats, defence, properties;

        public static readonly UnitDisplaySettings All = new UnitDisplaySettings(true, true, true, true, true, true);
        public static readonly UnitDisplaySettings AiAction = new UnitDisplaySettings(true, true, true, true, true, false);
        public static readonly UnitDisplaySettings Defender = new UnitDisplaySettings(true, false, false, false, true, false);

        public UnitDisplaySettings(bool health, bool stamina, bool bloodrage, bool stats, bool defence, bool properties)
        {
            this.health = health;
            this.stamina = stamina;
            this.bloodrage = bloodrage;
            this.stats = stats;
            this.defence = defence;
            this.properties = properties;
        }
    }

    class UnitCardDisplay : ImageGroup
    {
        public static void CreateCardDisplay(IntVector2 pos, AbsPlayerHUD hud)
        {
            hud.removeInfoCardDisplay();

            var square = toggRef.board.tileGrid.Get(pos);

            if (square.hidden == false ||
                toggRef.mode == GameMode.Commander)
            {
                Vector2 position = Vector2.Zero;
                UnitCardDisplay card = new UnitCardDisplay();
                card.hud = hud;

                //if (Input.Keyboard.Alt)
                //{
                //    lib.DoNothing();
                //}
                
                square.AddToUnitCard(card, ref position);
               
                //Completing task
                if (card.images.Count > 0)
                {
                    card.top = Engine.Screen.SafeArea.Bottom - position.Y;
                    card.Move(new Vector2(Engine.Screen.SafeArea.X, card.top));
                    hud.addInfoCardDisplay(card);
                }
            }
        }

        public List<Vector2> staminaHighlightPositions;
        public Vector2 staminaIconSz;
        public UnitDisplaySettings settings;
        AbsPlayerHUD hud;
        HUD.RichBox.RichBoxSettings richBoxSett;
        float propertyBoxH;
        float propertyBoxEdge;
        float propertyIconH;
        float propertyTextH;
        Vector2 itemsHudSz;
        public float top;

        public UnitCardDisplay()
            : base()
        {
            init();
        }

        public UnitCardDisplay(AbsUnit unit, UnitDisplaySettings settings, AbsPlayerHUD hud)
            : base(32)
        {
            this.settings = settings;
            Vector2 position = Engine.Screen.SafeArea.LeftBottom;
            this.hud = hud;

            init();
        }

        void init()
        {
            propertyBoxH = MathExt.RoundAndEven(StatsBoxHeight() * 0.8f);
            propertyBoxEdge = 12;
            propertyTextH = MathExt.RoundAndEven(propertyBoxH * 0.6f);
            propertyIconH = MathExt.RoundAndEven(propertyBoxH - propertyBoxEdge * 2f);
            itemsHudSz = new Vector2(MathExt.Round(Engine.Screen.IconSize * 0.8f));

            richBoxSett = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, propertyTextH, Color.White, ColorExt.Empty),
                new TextFormat(),
                propertyIconH, 1f);
        }

        public void itemsCard(ref Vector2 position, HeroQuest.Gadgets.TileItemCollection items)
        {
            hud.extendedTooltip.add(new HeroQuest.Gadgets.TileItemCollTooltip(items.data));

            portrait(ref position, items.data.texture(), items.data.name(), false, 0.8f);

            if (items.data.discovered && arraylib.HasMembers(items.items))
            {
                //position.Y += Engine.Screen.BorderWidth;

                List<AbsRichBoxMember> rbMembers = new List<HUD.RichBox.AbsRichBoxMember>();
                
                items.items.loopBegin();
                while (items.items.loopNext())
                {
                    if (rbMembers.Count > 0)
                    {
                        rbMembers.Add(new RbNewLine());
                    }

                    if (items.items.sel.StackLimit > 1)
                    {
                        rbMembers.Add(new RbText(items.items.sel.count.ToString(), 
                            null, LoadedFont.Bold));
                    }

                    rbMembers.Add(new RbImage(items.items.sel.Icon, 1.5f, 0, 0.1f));
                    rbMembers.Add(new RbText(items.items.sel.Name));
                    
                }

                RichBoxGroup rb = new RichBoxGroup(
                    VectorExt.Add(position, propertyBoxEdge),
                    Engine.Screen.Width, HudLib.ContentLayer, richBoxSett,
                    rbMembers, false);
                Add(rb);

                VectorRect area = rb.maxArea;
                area.Width += Engine.Screen.BorderWidth;
                area.AddRadius(propertyBoxEdge);

                var bgTex = new HUD.NineSplitAreaTexture(SpriteName.toggPropertyTex, 1, 5, area, 2, true,
                    HudLib.ContentLayer + 1, true);
                Add(bgTex);

                position.Y = area.Bottom;

            }
            //position.Y += itemsHudSz.Y;
        }

        void itemsPortrait(ref Vector2 position, SpriteName icon)
        {
            Graphics.Image iconBg = new Image(SpriteName.cmdCardPortraitBoxLarge, position, itemsHudSz, HudLib.BgLayer);
            Graphics.Image iconImg = new Image(icon, position, itemsHudSz, ImageLayers.AbsoluteBottomLayer);
            iconImg.LayerAbove(iconBg);

            Add(iconBg); Add(iconImg);

            position.X = iconBg.Right + Engine.Screen.BorderWidth * 2f;
        }

        public void LootCard(ref Vector2 position, int count)
        {
            hud.extendedTooltip.add(new HeroQuest.Gadgets.LootdropTooltip());

            string text = LanguageLib.LootDrop;
            if (count > 1)
            {
                text += " (" + count.ToString() + ")";
            }


            itemsPortrait(ref position, HeroQuest.Gadgets.Lootdrop.Icon);
            lootText(text, position);

            position.Y += itemsHudSz.Y;
        }

        void lootText(string textstring, Vector2 position)
        {
            Graphics.Text2 text = new Text2(textstring, LoadedFont.Regular,
                VectorExt.AddY(position, itemsHudSz.Y * 0.5f), Engine.Screen.TextBreadHeight,
                Color.White, HudLib.ContentLayer);

            text.OrigoAtCenterHeight();
            Add(text);
        }

        public void startSegment(ref Vector2 position)
        {
            if (position.Y > 0)
            {
                position.Y += Engine.Screen.SmallIconSize;
            }
        }

        public void spaceY(ref Vector2 position)
        {
            position.Y += Engine.Screen.BorderWidth;
        }

        public void statBoxesRow(ref Vector2 position, AbsUnit unit)
        {
            var data = unit.Data;

            Vector2 statsPos = position;
            var weapon = unit.Data.WeaponStats;
            if (settings.stats)
            {
                skillValueBox(ref statsPos, data.move, 0, new MovementTooltip());
                skillValueBox(ref statsPos, weapon.meleeStrength, 0, new MeleePowerTooltip());
                if (weapon.HasProjectileAttack)
                {
                    skillValueBox(ref statsPos, weapon.projectileStrength, weapon.projectileRange, new RangedPowerTooltip());
                }
            }

            if (settings.defence && toggRef.mode == GameMode.HeroQuest)
            { armorBox(ref statsPos, unit.hq()); }

            position.Y += StatsBoxHeight();
        }

        public void portrait(ref Vector2 position, SpriteName icon, string name, 
            bool largePortrait = true, float scale = 1f, float iconYAdj = -0.075f)
        {
            float portraitH = MathExt.RoundAndEven(Engine.Screen.IconSize * 
                (largePortrait? 1.6f : 1.2f));            
            
            float nameBgH = MathExt.RoundAndEven(Engine.Screen.IconSize * 
                (largePortrait? 0.72f : 0.60f));

            float nameBgY = position.Y + portraitH - nameBgH - Engine.Screen.BorderWidth * 0.5f;

            Graphics.Image portraitBg = new Image(
                largePortrait? SpriteName.cmdCardPortraitBoxLarge : SpriteName.cmdCardPortraitBoxSmall, 
                position, new Vector2(portraitH), HudLib.BgLayer);

            VectorRect portraitArea = VectorRect.FromCenterSize(portraitBg.Center, portraitBg.Size * 1.1f * scale);
            portraitArea.Y += iconYAdj * portraitBg.Height;//Engine.Screen.BorderWidth;
            Graphics.Image portraitImg = new Image(icon,Vector2.Zero, Vector2.One, HudLib.ContentLayer, true);
            portraitImg.fitInAreaWithoutStreching(portraitArea, true);
            
            Graphics.Image portraitShadow = (Graphics.Image)portraitImg.CloneMe();
            portraitShadow.Color = Color.Black;
            portraitShadow.Opacity = 0.5f;

            portraitShadow.Position += portraitImg.Size * 0.01f;
            portraitShadow.Size *= 1.1f;
            portraitShadow.ChangePaintLayer(1);

            Graphics.TextG nameImg = new TextG(LoadedFont.Regular, Vector2.Zero, Vector2.One, Graphics.Align.CenterHeight,
                name, Color.Black, HudLib.ContentLayer);
            nameImg.SetHeight(nameBgH * 0.8f);

            float w = nameImg.MeasureText().X - nameBgH * 0.5f;

            Graphics.Image nameBg1 = new Image(SpriteName.cmdUnitCardNameBannerLeft,
                new Vector2(portraitBg.Right - Engine.Screen.BorderWidth, nameBgY),
                new Vector2(nameBgH), ImageLayers.AbsoluteBottomLayer);
            nameBg1.LayerBelow(portraitBg);

            nameImg.Position = new Vector2(portraitBg.Right + Engine.Screen.BorderWidth, nameBg1.Center.Y);
            Graphics.Image nameBg2 = new Image(SpriteName.cmdUnitCardNameBannerMid, VectorExt.AddX(nameBg1.RightTop, -1),
                new Vector2(w, nameBgH), ImageLayers.AbsoluteBottomLayer);
            nameBg2.LayerBelow(nameBg1);

            Graphics.Image nameBg3 = new Image(SpriteName.cmdUnitCardNameBannerRight, VectorExt.AddX(nameBg2.RightTop, -1),
                nameBg1.Size, ImageLayers.AbsoluteBottomLayer);
            nameBg3.LayerAbove(nameBg2);

            nameBg1.Height *= 2f;
            nameBg2.Height *= 2f;
            nameBg3.Height *= 2f;


            Add(portraitBg); Add(portraitImg); Add(portraitShadow); Add(nameImg); Add(nameBg1); Add(nameBg2); Add(nameBg3);

            position.Y += portraitH;
        }

        float StatsBoxHeight()
        {
            return (int)(Engine.Screen.IconSize * 0.8f);
        }

        void armorBox(ref Vector2 position, HeroQuest.Unit unit)
        {
            HeroQuest.DefenceData defence = unit.data.defence.collectDefence(null, unit, 
                AttackType.None, false);
            int iconCount = defence.Count;

            if (iconCount > 0)
            {
                float h = StatsBoxHeight();
                Vector2 iconSz = new Vector2(h * 0.82f);
                float iconSpacing = iconSz.X * 0.54f;
                float totalW = iconSz.X + (iconCount - 1) * iconSpacing;

                const float BgSidePercW = 0.25f;
                Graphics.ImageAdvanced bg1 = new ImageAdvanced(SpriteName.cmdUnitCardArmorBg,
                    position, new Vector2(h), HudLib.BgLayer, false);
                bg1.SourceWidth = (int)(bg1.SourceWidth * BgSidePercW);
                bg1.Width = (int)(BgSidePercW * bg1.Width);

                Graphics.ImageAdvanced bg2 = new ImageAdvanced(SpriteName.cmdUnitCardArmorBg,
                    bg1.RightTop, new Vector2(h), HudLib.BgLayer, false);
                bg2.SourceX += bg1.SourceWidth;
                bg2.SourceWidth -= bg1.SourceWidth * 2;

                bg2.Width = (int)(totalW - h * 0.2f);
                bg2.PaintLayer += PublicConstants.LayerMinDiff;

                Graphics.ImageAdvanced bg3 = new ImageAdvanced(SpriteName.cmdUnitCardArmorBg,
                    bg2.RightTop, bg1.Size, HudLib.BgLayer, false);
                bg3.SourceX = bg2.ImageSource.Right;
                bg3.SourceWidth = bg3.SourceWidth - bg1.SourceWidth - bg2.SourceWidth;

                Add(bg1); Add(bg2); Add(bg3);

                Vector2 iconBorder = new Vector2((h - iconSz.X) * 0.5f);
                iconBorder.X *= 1.6f;

                Vector2 shieldPos = position + iconBorder;

                float layer = GraphicsLib.ToPaintLayer(HudLib.ContentLayer - 1);
                float shadowlayer = GraphicsLib.ToPaintLayer(HudLib.ContentLayer);

                foreach (var m in defence.dice)
                {
                    Graphics.Image shieldImg = new Image(m.icon, shieldPos, iconSz, ImageLayers.AbsoluteBottomLayer);
                    shieldImg.PaintLayer = layer;
                    layer += PublicConstants.LayerMinDiff;
                    Add(shieldImg);

                    Graphics.Image shadow = (Graphics.Image)shieldImg.CloneMe();
                    shadow.Color = Color.Black;
                    shadow.Opacity = 0.2f;

                    shadow.Position += shieldImg.Size * 0.006f;
                    shadow.Size *= 1.1f;
                    shadow.PaintLayer = shadowlayer;
                    shadowlayer += PublicConstants.LayerMinDiff;
                    Add(shadow);

                    shieldPos.X += iconSpacing;

                    hud.extendedTooltip.add(new ArmorDicetip(m));
                }
            }
        }


        public List<Vector2> valueBar(ref Vector2 position, SpriteName box, ValueBar value,
            AbsValuebarTooltip tooltip)
        {
            hud.extendedTooltip.add(tooltip);

            List<Vector2> positions = new List<Vector2>(value.Value);

            Vector2 boxSz = new Vector2(MathExt.Round(StatsBoxHeight() * 0.76f));
            Vector2 iconSz = boxSz * 0.76f;
            staminaIconSz = iconSz;

            boxSz.X = boxSz.Y / SpriteSheet.CmdValueBoxTileSz.Y * SpriteSheet.CmdValueBoxTileSz.X;

            Graphics.Image valueBox = new Image(box, position, boxSz, HudLib.BgLayer);

            string valueTextString;
            float valueTextH;
            if (value.IsMax)
            {
                valueTextString = value.Value.ToString();
                valueTextH = boxSz.Y * 0.8f;
            }
            else
            {
                valueTextString = value.BarText();
                valueTextH = boxSz.Y * 0.64f;
            }

            Graphics.TextG valueText = new TextG(LoadedFont.Regular, valueBox.Center,
                Vector2.One, Align.CenterAll, valueTextString, Color.White, HudLib.ContentLayer);
            valueText.SetHeight(valueTextH);

            images.Add(valueBox); images.Add(valueText);

            Vector2 barPos = position;
            barPos.X += boxSz.X;
            barPos.Y += (boxSz.Y - iconSz.Y) * 0.5f;

            for (int i = 0; i < value.maxValue; ++i)
            {
                bool available = i < value.Value;
                SpriteName sprite = available ? tooltip.Icon : tooltip.IconGrayed;

                Graphics.Image iconImg = new Image(sprite, barPos, iconSz, HudLib.ContentLayer);
                barPos.X += iconSz.X * 0.92f;

                if (available)
                {
                    positions.Add(iconImg.Center);
                }

                images.Add(iconImg);
            }


            position.Y += boxSz.Y;

            return positions;
        }

        void skillValueBox(ref Vector2 position, int value, int range, AbsStatsTooltip tooltip)
        {
            hud.extendedTooltip.add(tooltip);

            Graphics.Image box = new Image(SpriteName.cmdUnitCardSkillBg, position, new Vector2(2, 1) * StatsBoxHeight(), HudLib.BgLayer);
            var boxAr = box.Area;
            Graphics.Image skillIcon = new Image(tooltip.Icon,
                boxAr.PercentToPosition(new Vector2(0.25f, 0.5f)),
                new Vector2(box.Height * 0.9f), HudLib.ContentLayer, true);

            Graphics.Image iconImgShadow = (Graphics.Image)skillIcon.CloneMe();
            iconImgShadow.Color = Color.Black;
            iconImgShadow.Opacity = 0.2f;
            iconImgShadow.ChangePaintLayer(1);

            iconImgShadow.Position += skillIcon.Size * 0.06f;
            iconImgShadow.Size *= 1.1f;

            //float valueBgViewWidth = box.Size1D * 0.80f;

            //float valueBgH = box.Size1D * 1f;
            //float valueBgW = valueBgH / SpriteSheet.CmdStatsValueBgSz.Y * SpriteSheet.CmdStatsValueBgSz.X;

            //Graphics.Image valueBg = new Image(SpriteName.cmdStatsValueBg, Vector2.Zero, new Vector2(valueBgW, valueBgH), ImageLayers.AbsoluteBottomLayer);
            //valueBg.LayerBelow(box);
            //valueBg.SetRight( box.Right + valueBgViewWidth, false);
            //valueBg.Ypos = box.Ypos + box.Size1D * 0.06f;

            Vector2 valueCenter = boxAr.PercentToPosition(new Vector2(0.63f, 0.56f));
            float textH = box.Height * 0.8f;
            Graphics.Text2 valueText = new Text2(value.ToString(), LoadedFont.Bold,
                valueCenter, textH,
                Color.White, HudLib.ContentLayer);
            valueText.OrigoAtCenter();

            position.X = boxAr.PercentToPosition(new Vector2(0.9f)).X;

            Add(box); Add(skillIcon); Add(iconImgShadow); Add(valueText);

            if (range > 0)
            {
                hud.extendedTooltip.add(new RangedLengthTooltip());

                Graphics.Image rangeBg = new Image(SpriteName.cmdUnitCardRangeBg,
                    boxAr.PercentToPosition(new Vector2(0.76f, 0.01f)),
                    box.Height * 1f * new Vector2(2, 1), ImageLayers.AbsoluteBottomLayer);

                //rangeBg.Xpos -= Engine.Screen.BorderWidth * 2f;
                //rangeBg.origo = VectorExt.V2HalfY;

                Graphics.Text2 rangeText = new Text2(range.ToString(), LoadedFont.Bold,
                    rangeBg.Area.PercentToPosition(new Vector2(0.25f, 0.5f)),
                    textH * 0.9f,
                    Color.Black, HudLib.ContentLayer);
                rangeText.OrigoAtCenter();

                //rangeText.Ypos -= Engine.Screen.BorderWidth * 0.6f;

                Add(rangeBg);
                Add(rangeText);
                position.X = rangeBg.Xpos + rangeBg.Width * 0.6f;
            }
        }

        public void properties(ref Vector2 position, UnitPropertyColl pList)
        {
            //for (int i = pList.members.Count - 1; i >= 0; --i)
            //{
            //    propertyBox(ref position, pList.members[i]);
            //}

            //List<AbsProperty> properties = new List<AbsProperty>(pList.
            List<AbsProperty> properties = new List<AbsProperty>(pList.members);

            propertyList(ref position, properties, SpriteName.toggPropertyTex);
        }

        public void propertyList(ref Vector2 position, List<AbsProperty> properties, SpriteName texture)
        {
            if (arraylib.HasMembers(properties))
            {
                List<AbsRichBoxMember> rbMembers = new List<HUD.RichBox.AbsRichBoxMember>();
                var last = arraylib.Last(properties);
                
                foreach (var m in properties)
                {
                    hud.extendedTooltip.add(new PropertyTooltip(m));

                    var adv = m.AdvancedCardDisplay();
                    if (adv == null)
                    {
                        if (m.Icon != SpriteName.NO_IMAGE)
                        {
                            rbMembers.Add(new RbImage(m.Icon));
                        }
                        rbMembers.Add(new RbText(m.Name));                        
                    }
                    else
                    {
                        rbMembers.AddRange(adv);
                    }

                    if (m != last)
                    {
                        rbMembers.Add(new RbSeperationLine());
                    }
                }

                RichBoxGroup rb = new RichBoxGroup(
                    VectorExt.Add(position, propertyBoxEdge),
                    Engine.Screen.Width, HudLib.ContentLayer, richBoxSett,
                    rbMembers, false);
                Add(rb);

                VectorRect area = rb.maxArea;
                area.AddRadius(propertyBoxEdge);

                //SpriteName overridingTex 
                var bgTex = new HUD.NineSplitAreaTexture(texture, 1, 5, area, 2, true, 
                    HudLib.ContentLayer +1, true);
                Add(bgTex);

                position.Y = area.Bottom;
            }
        }

        public void propertyBox(ref Vector2 position, AbsProperty property)
        {
            propertyBox(ref position, property.Icon, property.Name, property.AdvancedCardDisplay());

            hud.extendedTooltip.add(new PropertyTooltip(property));
        }

        //public void propertyBox(ref Vector2 position,
        //    List<HUD.RichBox.AbsRichBoxMember> alternativeRichbox)
        //{
        //    propertyBox(ref position,  SpriteName.no
        //}

        public void propertyBox(ref Vector2 position,
            SpriteName iconSprite, string name,
            List<HUD.RichBox.AbsRichBoxMember> alternativeRichbox)
        {
        //toggConditionPositiveTex,
        //toggConditionNegativeTex,
        //toggConditionNeutralTex,

            float width = propertyBoxH * 4f;

            Graphics.Image bg = new Image(SpriteName.cmdUnitCardPropertyBox, position,
                new Vector2(width, propertyBoxH), HudLib.BgLayer);
            Add(bg);
            Vector2 pos = new Vector2(bg.Xpos + propertyBoxEdge, bg.Center.Y);

            //var richbox = property.AdvancedCardDisplay();
            if (alternativeRichbox == null)
            {
                
                if (iconSprite != SpriteName.NO_IMAGE)
                {
                    Graphics.Image icon = new Image(iconSprite, pos,
                        new Vector2(propertyIconH), HudLib.ContentLayer);
                    icon.OrigoAtCenterHeight();
                    Add(icon);

                    pos.X = icon.Right;
                }
                Graphics.Text2 text = new Text2(name, LoadedFont.Regular,
                    pos, propertyTextH,
                    Color.White, HudLib.ContentLayer);
                text.OrigoAtCenterHeight();
                Add(text);
            }
            else
            {
                HUD.RichBox.RichBoxGroup richBoxImages = new HUD.RichBox.RichBoxGroup(
                    VectorExt.Add(bg.position, propertyBoxEdge),
                    Engine.Screen.Width, HudLib.ContentLayer, richBoxSett,
                    alternativeRichbox, false);
                Add(richBoxImages);
            }

            position.Y += propertyBoxH;
        } 

        public void buffBox(ref Vector2 position, HeroQuest.Data.Condition.AbsUnitStatus buff)
        {
            float width = propertyBoxH * 6f;

            Graphics.Image bg = new Image(SpriteName.WhiteArea, position,
                new Vector2(width, propertyBoxH), HudLib.BgLayer);
            bg.Color = buff.StatusIsPositive > 0 ? Color.DarkGreen : Color.DarkSalmon;
            Add(bg);
            Vector2 pos = new Vector2(bg.Xpos + propertyBoxEdge, bg.Center.Y);

            //var richbox = property.AdvancedCardDisplay();
            //if (richbox == null)
            //{
            var iconSprite = buff.Icon;
            if (iconSprite != SpriteName.NO_IMAGE)
            {
                Graphics.Image icon = new Image(iconSprite, pos,
                    new Vector2(propertyIconH), HudLib.ContentLayer);
                icon.OrigoAtCenterHeight();
                Add(icon);

                pos.X = icon.Right;
            }
            Graphics.Text2 text = new Text2(buff.Name, LoadedFont.Regular,
                pos, propertyTextH,
                Color.White, HudLib.ContentLayer);
            text.OrigoAtCenterHeight();
            Add(text);
            //}
            //else
            //{
            //    HUD.RichBox.RichBoxGroup richBoxImages = new HUD.RichBox.RichBoxGroup(
            //        VectorExt.Add(bg.position, propertyBoxEdge),
            //        Engine.Screen.Width, HudLib.ContentLayer, richBoxSett,
            //        richbox, false);
            //    Add(richBoxImages);
            //}

            position.Y += propertyBoxH;

            if (buff is HeroQuest.Data.Condition.AbsCondition)
            {
                hud.extendedTooltip.add(new PropertyTooltip(buff));
            }
        }

        public override void DeleteAll()
        {
            base.DeleteAll();

        }
    }
}
