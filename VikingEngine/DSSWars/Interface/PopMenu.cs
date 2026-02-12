using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Interface
{
    /// <summary>
    /// Some options displayed directly under the mouse
    /// </summary>
    class PopMenu
    {
        List<PopMenuButton> buttons = new List<PopMenuButton>(4);
        PopMenuButton sel = null;
      
        VectorRect nextarea;
        Graphics.RectangleLines selectionOutline = null;
        public PopMenu(Players.LocalPlayer player, RbAction1Arg<AbsMapObject> attackLink)
        {
            setup(player);

            addButton(player, SpriteName.WarsHudIconReturn, DssRef.lang.Hud_Cancel, 
                null);
            addButton(player, SpriteName.WarsRelationWar, DssRef.lang.Hud_WardeclarationTitle,
                attackLink);

            complete(player);
        }
        public PopMenu(Players.LocalPlayer player, DetailObjectCollection collection)
        {
            setup(player);
            
            addButton(player, SpriteName.WarsArmy, DssRef.lang.Conscript_Soldiers_ArmyType, 
                new RbAction1Arg<List<SoldierGroup>>(player.gameControls.map.selectCollection, collection.armyGroups));
            addButton(player, SpriteName.WarsGuard, DssRef.lang.Conscript_Soldiers_GuardType,
                new RbAction1Arg<List<SoldierGroup>>(player.gameControls.map.selectCollection, collection.guardGroups));

            complete(player);
        }

        void setup(Players.LocalPlayer player)
        {
            player.hud.popMenu = this;
            float defaultWidth = Engine.Screen.IconSize * 3f;
            float defaultHeight = Engine.Screen.TextBreadHeight * 1.6f;

            nextarea = new VectorRect(Vector2.Zero, new Vector2(defaultWidth, defaultHeight));
        }

        public bool update(Players.LocalPlayer player, out bool overHud)
        {
            player.hud.tooltip.clear();

            if (player.gameControls.input.inputSource.IsControllerOnly)
            {
                overHud = true;

                if (player.gameControls.input.mouseSelect.DownEvent)
                {
                    buttons[0].link?.actionTrigger();
                    return true;
                }
                else if (player.gameControls.input.CancelKey.DownEvent)
                {
                    buttons[1].link.actionTrigger();
                    return true;
                }

                return false;
            }
            else
            {
                foreach (var b in buttons)
                {
                    if (b.area.IntersectPoint(Input.Mouse.Position))
                    {
                        if (b != sel)
                        {
                            sel = b;
                            refreshSelectOutline();
                        }

                        overHud = true;
                        if (player.gameControls.input.menuInput.click.DownEvent)
                        {
                            b.link?.actionTrigger();
                            return true;
                        }
                        return false;
                    }
                }

                //Auto select if leaving the menu
                    overHud = false;
                    buttons.First().link?.actionTrigger();
                return true;
            }
            
        }

        public void refreshSelectOutline()
        {            
            selectionOutline?.DeleteMe();
            if (sel != null)
            {
                var ar = sel.area;
                selectionOutline = new RectangleLines(ar, 2, 1, HudLib.PopMenuLayer);
            }
        }

        void addButton(Players.LocalPlayer player, SpriteName icon, string textString, AbsRbAction link)
        {
            float w = nextarea.Width;
            PopMenuButton button = new PopMenuButton(nextarea, icon, textString, link, ref w);
            nextarea.Width = w;
            nextarea.Y += nextarea.Height;

            if (player.gameControls.input.inputSource.IsControllerOnly)
            {
                int optionIndex = buttons.Count;
                SpriteName input = SpriteName.NO_IMAGE;
                switch (optionIndex)
                {
                    case 0: input = player.gameControls.input.mouseSelect.Icon; break;
                    case 1: input = player.gameControls.input.CancelKey.Icon; break;
                }

                button.addInput(input);
            }

            buttons.Add(button);
        }

        void complete(LocalPlayer player)
        {
            VectorRect centerArea = buttons.First().area;
            centerArea.Width = nextarea.Width;

            Vector2 target = player.gameControls.map.pointerPos(); //Input.Mouse.Position;
            Vector2 move  = target - centerArea.Center;

            foreach (var m in buttons)
            {
                m.complete(move, nextarea.Width);
            }
        }

        public void DeleteMe()
        {
            foreach (var m in buttons)
            {
                m.DeleteMe();
            }
            selectionOutline?.DeleteMe();
        }
    }

    class PopMenuButton
    {
        public AbsRbAction link;
        public VectorRect area;
        Graphics.Image image = null;
        Graphics.TextG text;
        NineSplitAreaTexture background;
        Graphics.Image input = null;
        public PopMenuButton(VectorRect area, SpriteName icon, string textString, AbsRbAction link, ref float maxWidth)
        {
            this.link = link;
            this.area = area;
            Vector2 textPos = new Vector2(Engine.Screen.BorderWidth, area.Center.Y);
            if (icon != SpriteName.NO_IMAGE)
            {
                image = new Graphics.Image(icon, textPos, new Vector2(area.Height * 0.7f), HudLib.PopMenuLayer, false);
                image.OrigoAtCenterHeight();
                textPos.X += image.Width + Engine.Screen.BorderWidth;
            }

            text = new Graphics.TextG(LoadedFont.Regular, textPos, Engine.Screen.TextBreadScale, Graphics.Align.CenterHeight, textString, Color.White, HudLib.PopMenuLayer);

            maxWidth = lib.LargestValue(maxWidth, text.MeasureText().X);
        }

        public void addInput(SpriteName sprite)
        {
            Vector2 size = Screen.SmallIconSizeV2;
            Vector2 pos = area.LeftCenter;
            pos.X -= size.X * 0.6f;
            input = new Image(sprite, pos, size, HudLib.PopMenuLayer, true);
        }

        public void complete(Vector2 move, float maxWidth)
        {
            area.Width = maxWidth;
            Move(move);
            area.Position += move;
            background = new NineSplitAreaTexture(HudLib.PopMenuButtonTexture, area, HudLib.PopMenuLayer + 2);
        }

        void Move(Vector2 move)
        {
            image?.AddXY(move);
            input?.AddXY(move);
            text.AddXY(move);
            background?.Move(move);
        }

        public void DeleteMe()
        {
            image?.DeleteMe();
            input?.DeleteMe();
            text.DeleteMe();
            background.DeleteMe();
        }
    }


}
