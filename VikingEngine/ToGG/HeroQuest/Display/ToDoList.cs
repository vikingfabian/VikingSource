using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class ToDoList
    {
        Graphics.ImageGroupParent2D images = new Graphics.ImageGroupParent2D();
        List<ToDoListUnit> list = new List<ToDoListUnit>();
        float height;
        public Graphics.Image strategyCardBg;

        public ToDoList(LocalPlayer player)
        {
            Vector2 position = new Vector2(Engine.Screen.SafeArea.Right, 0);
            var hero = player.HeroUnit;

            Vector2 cardSize = ToggEngine.Display2D.StrategyCardsHudMember.CardSize(Engine.Screen.SmallIconSize);

            strategyCardBg = new Graphics.Image(SpriteName.cmdStrategyBg, 
                VectorExt.AddX(position, -cardSize.X),
                cardSize, HudLib.BgLayer);
            
            images.Add(strategyCardBg);

            if (hero.data.hero.availableStrategies.active != null)
            {
                images.Add(ToggEngine.Display2D.StrategyCardsHudMember.CardIcon(strategyCardBg, hero.data.hero.availableStrategies.active.CardSprite, true));
            }
            position.Y = strategyCardBg.Bottom + Engine.Screen.BorderWidth;

            player.hqUnits.unitsCounter.Reset();
            while (player.hqUnits.unitsCounter.Next())
            {
                if (player.isInactiveHero(player.hqUnits.unitsCounter.sel) == false)
                {
                    list.Add(new ToDoListUnit(ref position, player.hqUnits.unitsCounter.sel.hq(), images));
                }
            }

            height = position.Y;
        }

        public void refresh(float nextButtonY)
        {
            images.ParentY = nextButtonY - (height + Engine.Screen.BorderWidth);

            foreach (var m in list)
            {
                m.refresh();
            }         
        }

        public void endTurnWarning(Graphics.ImageGroup images)
        {
            foreach (var m in list)
            {
                m.endTurnWarning(images);
            }
        }

        public bool allComplete()
        {
            foreach (var m in list)
            {
                if (!m.allComplete())
                {
                    return false;
                }
            }
            return true;
        }

        public void setVisible(bool visible)
        {
            images.SetVisible_Quick(visible);         
        }

        public void DeleteMe()
        {
            images.DeleteMe();
        }

        //---------------------
        class ToDoListUnit
        {
            Unit unit;
            List<CheckBar> bars = new List<CheckBar>();

            public ToDoListUnit(ref Vector2 position, Unit unit, Graphics.ImageGroupParent2D images)
            {
                this.unit = unit;

                var actions = unit.actionList();

                if (arraylib.HasMembers(actions))
                {
                    foreach (var m in actions)
                    {
                        bars.Add(new CheckBar(ref position, m, images));
                    }
                }
            }

            public void refresh()
            {
                foreach (var m in bars)
                {
                    m.refresh();
                }
            }

            public void endTurnWarning(Graphics.ImageGroup images)
            {
                foreach (var m in bars)
                {
                    m.endTurnWarning(images);
                }
            }

            public bool allComplete()
            {
                foreach (var m in bars)
                {
                    if (!m.isComplete)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        class CheckBar
        {
            public bool isComplete = false;
            Graphics.Image check;
            Graphics.TextG valueText;
            AbsToDoAction action;

            public CheckBar(ref Vector2 position, AbsToDoAction action, 
                Graphics.ImageGroupParent2D images)
            {
                this.action = action;

                float height = Engine.Screen.SmallIconSize;
                float border = (int)(Engine.Screen.BorderWidth / 2);
                float contentHeight = height - border * 2;

                Vector2 pos = position;
                pos.X -= height;

               check = new Graphics.Image(SpriteName.MissingImage,
                    pos, new Vector2(height), HudLib.ContentLayer);

                pos.X -= border;
                pos.Y += border;

                images.Add(check);

                float r = pos.X;

                pos.X -= border + contentHeight;
                Graphics.Image icon = new Graphics.Image(action.Icon,
                    pos, new Vector2(contentHeight), HudLib.ContentLayer);
                images.Add(icon);

                valueText = new Graphics.TextG(LoadedFont.Regular, Vector2.Zero, Vector2.One,
                    Graphics.Align.Zero, action.Count.BarSpentText(), Color.White, HudLib.ContentLayer);
                valueText.SetHeight(contentHeight);

                pos.X -= valueText.MeasureText().X;
                valueText.Position = pos;
                images.Add(valueText);

                pos.X -= contentHeight;

                Graphics.Image unitImg = new Graphics.Image(action.unit.data.modelSettings.image, 
                    pos, new Vector2(contentHeight), HudLib.ContentLayer);
                pos.X -= border;
                images.Add(unitImg);

                VectorRect area = new VectorRect(pos.X, position.Y, r - pos.X, height);
                Graphics.Image bgBar = new Graphics.Image(SpriteName.WhiteArea, 
                    area.Position, area.Size, HudLib.BgLayer);
                bgBar.Color = Color.Black;
                bgBar.Opacity = 0.7f;
                images.Add(bgBar);

                position.Y += height + border;
            }
            
            public void refresh()
            {
                var value = action.Count;

                valueText.TextString = value.BarSpentText();
                isComplete = !value.IsMax;
                check.SetSpriteName(isComplete ? SpriteName.cmdIconButtonReadyCheck : SpriteName.cmdIconButtonReadyCheckGray);
            }

            public void endTurnWarning(Graphics.ImageGroup images)
            {
                if (!isComplete)
                {
                    Graphics.Image warning = new Graphics.Image(SpriteName.cmdWarningTriangle,
                        check.Center, check.Size, ImageLayers.AbsoluteBottomLayer, true);
                    warning.LayerAbove(check);

                    images.Add(warning);
                }
            }            
        }
    }
}
