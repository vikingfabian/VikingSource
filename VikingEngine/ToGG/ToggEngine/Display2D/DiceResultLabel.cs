using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class DiceResultLabel : AbsUpdateable
    {
        List<QueAttackValue> addQue = new List<QueAttackValue>(2);

        Graphics.Image bgImg;
        Graphics.Image iconImg;

        Graphics.TextG valueText;

        Time nextValueAddTimer = Time.Zero;
        public int value, critiquals;

        public bool dodge = false;
        VectorRect area;

        public DiceResultLabel(AttackRollDiceDisplay dice, bool primaryLabel, int type_0Damage_1Surges_2Shield_3Retreat)
            : base(true)
        {
            ImageLayers Layer = HudLib.AttackWheelLabelLayer;

            SpriteName icon; SpriteName bgSprite;
            Color textCol;
            switch (type_0Damage_1Surges_2Shield_3Retreat)
            {
                default:
                    icon = SpriteName.cmdSlotMashineHit;
                    bgSprite = SpriteName.cmdHudSideLabelRed;
                    textCol = Color.White;
                    break;
                case 1:
                    icon = SpriteName.cmdIconSurge;
                    bgSprite = SpriteName.cmdHudSideLabelBlue;
                    textCol = Color.White;
                    break;
                case 2:
                    icon = SpriteName.cmdArmorResult;
                    bgSprite = SpriteName.cmdHudSideLabelLightBlue;
                    textCol = Color.Black;
                    break;
                case 3:
                    icon = SpriteName.cmdRetreatFlatSymbol;
                    bgSprite = SpriteName.cmdHudSideLabelBlue;
                    textCol = Color.White;
                    break;
            }

            area = new VectorRect(dice.bgArea.LeftBottom,
                 MathExt.Round(Engine.Screen.IconSize * 1.3f) * new Vector2(2, 1));
            area.X += (int)(Engine.Screen.IconSize * 0.05f);
            area.Y -= area.Height + Engine.Screen.BorderWidth;

            if (!primaryLabel)
            {
                area.Y = dice.bgArea.Y + (int)(Engine.Screen.IconSize * 0.2f);
            }
            area.X -= area.Width;


            bgImg = new Graphics.Image(bgSprite, area.Position, area.Size, 
                Layer, false);

            VectorRect contentArea = area;
            contentArea.AddYRadius(-MathExt.Round(Engine.Screen.IconSize * 0.29f));
            contentArea.X += Engine.Screen.IconSize * 1.1f;

            iconImg = new Graphics.Image(icon, contentArea.Position, new Vector2(contentArea.Height), ImageLayers.AbsoluteBottomLayer, false);
            iconImg.LayerAbove(bgImg);

            valueText = new Graphics.TextG(LoadedFont.Regular, iconImg.RightCenter, 
                Vector2.One, Graphics.Align.CenterHeight,
                null, textCol, Layer);
            valueText.Xpos -= MathExt.Round(Engine.Screen.IconSize * 0.07f);
            valueText.LayerAbove(bgImg);


            refresh();
        }

        public void set(int val, int crit = 0)
        {
            int addVal = val - value;
            int addCrit = crit - critiquals;

            this.Add(addVal, addCrit);
            
        }

        public void Add(int addVal, int addCitiqual = 0)
        {
            if (addVal > 0)
            {
                QueAttackValue val = new QueAttackValue(addVal, addCitiqual);
                addQue.Add(val);
            }
        }

        public void SetAsDodge()
        {
            QueAttackValue val = new QueAttackValue(0, 0);
            val.dodge = true;
            addQue.Add(val);

        }

        public override void Time_Update(float time_ms)
        {
            foreach (var m in addQue)
            {
                if (m.addDelay.CountDown())
                {
                    apply(m);

                    refresh();

                    const float BumpUp = 0.2f;
                    const float BumpTime = 50;

                    new Graphics.Motion2d(Graphics.MotionType.SCALE, iconImg, iconImg.Size * BumpUp, Graphics.MotionRepeate.BackNForwardOnce,
                        BumpTime, true);

                    addQue.Remove(m);
                    break;
                }
            }            
        }

        public void applyValues()
        {
            foreach (var m in addQue)
            {
                apply(m);
            }
            addQue.Clear();
            refresh();
        }

        void apply(QueAttackValue val)
        {
            if (val.dodge)
            {
                dodge = true;
                iconImg.SetSpriteName(SpriteName.cmdDodge);

                value = 5; //Just for scale
                valueText.Visible = false;
            }
            else
            {
                value += val.addValue;
                critiquals += val.addCritiqual;
            }
        } 

        void refresh()
        {
            valueText.StringBuilder.Clear();
            valueText.StringBuilder.Append(value.ToString());
            if (critiquals > 0)
            {
                valueText.StringBuilder.Append('!', critiquals);
            }

            float textH = iconImg.Height * (0.85f + value * 0.05f);
            valueText.SetHeight(textH);
            valueText.origo = VectorExt.V2HalfY;
            valueText.updateCenter();
        }

        public Vector2 Center
        {
            get { return bgImg.Position; }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();

            bgImg.DeleteMe();
            iconImg.DeleteMe();
            valueText.DeleteMe();
        }

        class QueAttackValue
        {
            const float TimeDelay = 0;

            public int addValue, addCritiqual;
            public bool dodge;

            public Time addDelay;

            public QueAttackValue(int addValue, int addCritiqual)
            {
                this.addValue = addValue;
                this.addCritiqual = addCritiqual;

                dodge = false;
                addDelay = new Time(TimeDelay);
            }
        }
    }

    
}
