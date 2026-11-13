using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.Core.Wand.Display.GameDisplay
{
    class MapTooltip : RichGuiTooltip
    {
        public void updateMapTip(IntVector2 position)
        {
            RichBoxContent content = new RichBoxContent();
            if (WandRef.world.map.tileGrid.TryGet(position, out var tile))
            {

                if (tile.creature.HasGameObject() &&
                    tile.creature.creatureType != GO.ObjectPointer.CreatureType.Hero)
                {
                    var creature = tile.creature.Get();
                    content.h2("Creature", HudLib.TitleColor_Label);
                    content.text(creature.pointer.creatureType.ToString());
                    content.text("Attack: " + creature.AttackStrength().ToString("-"));
                }

                if (tile.item.HasGameObject())
                {
                    if (content.Count > 0)
                    {
                        content.Add(new RbSeperationLine());
                        content.newParagraph();
                    }

                    var item = tile.item.Get();
                    content.h2("Item", HudLib.TitleColor_Label);
                    content.text(item.pointer.itemType.ToString() + "(" + item.itemTier.ToString() + ")");
                    string desc = null;
                    switch (item.pointer.itemType)
                    {
                        case GO.ObjectPointer.TileItemType.Altar:
                            desc = "Sacrifice to recharge all wands";
                            break;
                        case GO.ObjectPointer.TileItemType.Book:
                            desc = "Reveal a hidden spell";
                            break;
                    }

                    if (desc != null)
                    {
                        content.text(desc, HudLib.InfoYellow_Light);
                    }
                }
            }
            basecreate(content, ImageLayers.Lay9, false);
        }

        public void update(bool overHud)
        {
            if (overHud)
            {
                clear();
            }
            else
            {
                baseUpdate(Input.Mouse.Position, false, false);
            }
        }

        protected override void createBackground(VectorRect area, ImageLayers layer)
        {
            var backgroundTextures = new NineSplitAreaTexture(HudLib.TooltipSettings.windowBackground, area, layer + 2);
            images.Add(backgroundTextures.images);
        }
    }
}
