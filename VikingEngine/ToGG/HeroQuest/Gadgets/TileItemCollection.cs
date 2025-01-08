using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class TileItemCollection : ToggEngine.GO.AbsTileObject
    {
        public TileItemCollData data;
        public ItemCollection items = new ItemCollection();
        public Graphics.Mesh model, itemModel;
        Graphics.Mesh movestepIcon;

        Vector3 posOffset;

        public TileItemCollection(IntVector2 pos, object args)
            : base(pos)
        {
            if (args == null)
            {
                data = new TileItemCollData(LootLevel.NUM_NONE);
            }
            else
            {
                data = (TileItemCollData)args;
            }

            data.refreshSettings();
            InteractSettings.icon = SpriteName.cmdAddToBackpack;
            InteractSettings.text = "Take items";
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = toggRef.board.toWorldPos_Center(newpos, 0f) + posOffset;

            if (itemModel != null)
            {
                itemModel.Position = model.Position + new Vector3(0.05f, -0.01f, 0.04f);
            }
        }

        public override void onLoadComplete()
        {
            base.onLoadComplete();

            if (data.Chest && 
                items.Count == 0 &&
                !toggRef.InEditor)
            {
                hqRef.loot.chestItems(this);
            }

            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero,
                new Vector3(0.4f), Graphics.TextureEffectType.Flat,
                SpriteName.MissingImage, Color.White);
            refreshVisuals();
            posOffset = new Vector3(-0.25f, model.Scale.Y * 0.3f, -0.17f);

            model.Rotation = toggLib.PlaneTowardsCam;

            newPosition(position);
        }

        public void setChest(LootLevel lvl)
        {
            data.lootLevel = lvl;
            data.discovered = false;

            hqRef.loot.chestItems(this);

            refreshVisuals();
        }

        public void refreshVisuals()
        {
            if (model != null)
            {
                model.Scale1D = data.Chest ? 0.7f : 0.4f;
                model.SetSpriteName(data.texture());

                if (!data.Chest && items.Count == 1)
                {
                    model.Color = Color.DarkGray;

                    itemModel = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero,
                        new Vector3(0.3f), Graphics.TextureEffectType.Flat,
                        items[0].Icon, Color.White);
                    itemModel.Rotation = toggLib.PlaneTowardsCam;
                    newPosition(position);
                }
                else
                {
                    model.Color = Color.White;

                    itemModel?.DeleteMe();
                    itemModel = null;
                }
            }
        }

        public override List<AbsRichBoxMember> interactToolTip()
        {
            var richbox = base.interactToolTip();
            richbox.Add(new RbNewLine(true));
            items.toRichbox(richbox);

            return richbox;
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.itemsCard(ref position, this);
        }

        public static void NetWrite(IntVector2 pos)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqTileItemColl, Network.PacketReliability.Reliable);
            toggRef.board.WritePosition(w, pos);

            TileItemCollection itemCollection = hqRef.items.groundCollection(pos, false);

            bool hasItems = itemCollection != null && itemCollection.items.Count > 0;
            w.Write(hasItems);

            if (hasItems) 
            {
                itemCollection.data.Write(w);
                itemCollection.items.write(w);
            }
        }

        

        public static void NetRead(System.IO.BinaryReader r)
        {
            IntVector2 pos = toggRef.board.ReadPosition(r);
            bool hasItems = r.ReadBoolean();
            TileItemCollection itemCollection = hqRef.items.groundCollection(pos, hasItems);

            if (hasItems)
            {
                itemCollection.data.Read(r);
                itemCollection.refreshVisuals();

                itemCollection.Clear();
                itemCollection.items.read(r, DataStream.FileVersion.Max);

                itemCollection.refreshVisuals();
            }
            else
            {
                itemCollection?.DeleteMe();
            }
        }

        public void Add(AbsItem item)
        {
            items.Add(item);
            if (items.Count <= 2)
            {
                refreshVisuals();
            }
        }

        public void onAccess()
        {
            if (!data.discovered)
            {
                data.discovered = true;
                NetWrite(position);
                refreshVisuals();
            }
        }

        
        public override void createMoveStepIcon(Graphics.ImageGroup icons)
        {
            DefaultMoveStepIcon(SpriteName.cmdAddToBackpack, position, icons, ref movestepIcon);
        }

        public override AbsSquareAction collectSquareEnterAction(IntVector2 pos, ToggEngine.GO.AbsUnit unit, bool local)
        {
            if (unit.hq().data.IsLootCollector())
            {
                TileObjectActivation activate = new TileObjectActivation(pos, true, local, this);
                activate.DelayTime = 0;
                return activate;
            }

            return null;
        }

        public override void onMoveEnter(ToggEngine.GO.AbsUnit unit, bool local)
        {
            base.onMoveEnter(unit, local);
            Loot(unit.hq());
        }

        public override bool IsLootable()
        {
            return true;
        }

        public override void Loot(Unit unit)
        {
            hqRef.items.interaction_takeAllItems(unit.hq(), items);
            DeleteMe();

            NetWrite(position);
        }


        public void Clear()
        {
            foreach (var m in items)
            {
                m.DeleteImage();
            }
            items.Clear();
        }

        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);
            data.Write(w);

            items.write(w);
        }

        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);
            data.Read(r);

            if (version.release >= 5)
            {
                items.read(r, version);
            }

           // onLoadComplete();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model?.DeleteMe();
            itemModel?.DeleteMe();
            Clear();
        }

        public override ToggEngine.TileObjectType Type => ToggEngine.TileObjectType.ItemCollection;
    }

    class TileItemCollTooltip : AbsExtToolTip
    {
        TileItemCollData data;
        public TileItemCollTooltip(TileItemCollData data)
        {
            this.data = data;
        }

        public override SpriteName Icon => data.texture();
        public override string Title => data.name();
        public override string Text
        {
            get
            {
               return "A collection of items. " + LanguageLib.MoveEnterPickup;
            }
        }
            
    }

    struct TileItemCollData
    {
        public const SpriteName LootDropIcon = SpriteName.cmdItemPouch;

        public LootLevel lootLevel;
        public bool discovered;

        public TileItemCollData(LootLevel lootLevel)
        {
            this.lootLevel = lootLevel;
            discovered = false;

            refreshSettings();
        }

        public void refreshSettings()
        {
            if (!Chest)
            {
                discovered = true;
            }
        }

        public SpriteName texture()
        {
            if (Chest)
            {
                switch (lootLevel)
                {
                    case LootLevel.Level1:
                        return discovered? SpriteName.hqTier1TreasureOpen : SpriteName.hqTier1Treasure;

                    case LootLevel.Level2:
                        return discovered ? SpriteName.hqTier2TreasureOpen : SpriteName.hqTier2Treasure;

                    case LootLevel.Level3:
                        return discovered ? SpriteName.hqTier3TreasureOpen : SpriteName.hqTier3Treasure;


                    default: return SpriteName.MissingImage;
                }
            }
            else
            {
                return LootDropIcon;
            }
        }

        public string name()
        {
            return Name(lootLevel);
        }

        public static string Name(LootLevel lootLevel)
        {
            switch (lootLevel)
            {
                case LootLevel.NUM_NONE: return "Items";
                case LootLevel.Level1: return "Tier1 treasure";
                case LootLevel.Level2: return "Tier2 treasure";
                case LootLevel.Level3: return "Tier3 treasure";

                default: return TextLib.Error;
            }
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((byte)lootLevel);
            w.Write(discovered);
        }

        public void Read(System.IO.BinaryReader r)
        {
            lootLevel = (LootLevel)r.ReadByte();
            discovered = r.ReadBoolean();
        }

        public bool Chest => lootLevel != LootLevel.NUM_NONE;
    }
}
