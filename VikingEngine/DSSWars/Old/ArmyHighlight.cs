//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.RTS.GameObject;

//namespace Game1.RTS.Display
//{
//    class ArmyHighlight : AbsUnitHighlight
//    {
//        Graphics.TextBoxSimple text;

//        Army army;
//        HighlightUnitCount[] unitCount;

//        public ArmyHighlight(Army army, Player p)
//        {
//           // Ref.draw.CurrentRenderLayer = RTSlib.MapLayer;
//            p.SetPlayerLayer();
//            this.army = army;
//            text = new Graphics.TextBoxSimple(LoadedFont.Lootfest, Vector2.Zero, new Vector2(0.012f), Graphics.Align.Zero,
//                "", Color.White, RTSlib.LayerMapUnitHighlightInfo, 100);


//            images = new List<IDeleteable> { text };

//            unitCount = new HighlightUnitCount[(int)TroopType.NUM];
//            for (TroopType type = 0; type < TroopType.NUM; ++type)
//            {
//                unitCount[(int)type] = new HighlightUnitCount(army, type);
//                unitCount[(int)type].addToRemoveList(images);
//            }
//        }

//        override public void update()
//        {
//            text.TextString =
//                army.ToString() + TextLib.NewLine +
//                army.StatusToString();

//            text.Position = army.Position2D;
//            text.Ypos += 0.7f;
//            text.Xpos -= 0.5f;

//            Vector2 pos = text.Position;
//            pos.Y += 1.1f;
//            foreach (HighlightUnitCount h in unitCount)
//            {
//                h.updatePos(ref pos);
//            }
//            //text.Centertext(Graphics.Align.CenterWidth);
//        }
//    }

//    class HighlightUnitCount
//    {
//        Graphics.Image typeIcon;
//        Graphics.TextS count;

//        public HighlightUnitCount(Army army, TroopType type)
//        {
//            if (army.SoldiersCount.Get(type) > 0)
//            {
//                typeIcon = new Graphics.Image(SoldierUnits.TypeIcon(type), Vector2.Zero, new Vector2(0.5f), RTSlib.LayerMapUnitHighlightInfo);
//                count = new Graphics.TextS(LoadedFont.Lootfest, Vector2.Zero, new Vector2(0.01f), Graphics.Align.Zero,
//                    army.SoldiersCount.Get(type).ToString(), Color.White, RTSlib.LayerMapUnitHighlightInfo);
//            }
//        }

//        public void updatePos(ref Vector2 pos)
//        {
//            if (typeIcon != null)
//            {
//                typeIcon.Position = pos;
//                count.Position = pos;
//                count.Xpos += typeIcon.Width * 1f;

//                pos.Y += typeIcon.Height * 1.2f;
//            }
//        }

//        public void addToRemoveList(List<IDeleteable> list)
//        {
//            if (typeIcon != null)
//            {
//                list.Add(typeIcon);
//                list.Add(count);
//            }
//        }
//    }
//}
