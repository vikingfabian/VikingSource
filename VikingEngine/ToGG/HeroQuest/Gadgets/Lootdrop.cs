using Microsoft.Xna.Framework;
using System;
using System.IO;
using VikingEngine.DataStream;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class Lootdrop : ToggEngine.GO.AbsTileObject
    {
        public const SpriteName Icon = SpriteName.cmdCoin;
        Graphics.ImageGroupParent3D models;

        public Lootdrop(IntVector2 pos, object args)
            : base(pos)
        {
            models = new Graphics.ImageGroupParent3D(2);

            if (args == null)
            {
                addModel();
            }
            else
            {
                setCount((int)args);                
            }

            newPosition(pos);
        }

        void setCount(int count)
        {
            if (models.Count > count)
            {
                models.DeleteAll();
            }

            for (int i = models.Count; i < count; ++i)
            {
                addModel();
            }
        }

        void addModel()
        {
            var model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, 
                new Vector3(0.3f), Graphics.TextureEffectType.Flat,
                Icon, Color.White);

            float countAdd = models.Count * 0.1f;

            model.Position = new Vector3(
                -0.25f + countAdd,
                ToggEngine.Map.SquareModelLib.TerrainY_Loot + models.Count * 0.001f, 
                -0.2f + countAdd);
            models.AddAndUpdate(model);
        }

        public void StackOne()
        {
            addModel();
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            models.ParentPosition = toggRef.board.toWorldPos_Center(newpos, 0f);
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.LootCard(ref position, Count);
        }

        public override AbsSquareAction collectSquareEnterAction(IntVector2 pos, AbsUnit unit, bool local)
        {
            if (unit.hq().data.IsLootCollector())
            {
                return new PickLootAction(pos, local);
            }
            else
            {
                return null;
            }
        }

        public override bool IsLootable()
        {
            return true;
        }

        public override void Loot(Unit unit)
        {
            int count = Count;

            int min = 1 * count;
            int max = 3 * count;

            int coinCount = Ref.rnd.Int(min, max + 1);
            var item = new Gadgets.Coins(coinCount);

            var lc = unit.data.properties.Get(UnitPropertyType.LootCollector);
            if (lc != null)
            {
                ((Data.LootCollector)lc).add(item);
            }
            else
            {
                unit.PlayerHQ.add(item, true, true);
            }
            DeleteMe();
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            w.Write((byte)Count);
        }

        public override void Read(BinaryReader r, FileVersion version)
        {
            base.Read(r, version);
            int count = r.ReadByte();

            if (count > 1)
            {
                setCount(count);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            models.DeleteAll();
        }

        public int Count => models.Count;

        public override ToggEngine.TileObjectType Type => ToggEngine.TileObjectType.Lootdrop;
        
    }

    class LootdropTooltip : AbsExtToolTip
    {
        public override SpriteName Icon => Lootdrop.Icon;
        public override string Title => LanguageLib.LootDrop;
        public override string Text => LanguageLib.MoveEnterPickup;
    }

    class PickLootAction : AbsSquareAction
    {
        public PickLootAction(IntVector2 pos, bool local)
            :base(pos, true, local)
        {
            DelayTime = 0;
        }

        public override void onCommitMove(AbsUnit unit)
        {
            var loot = toggRef.Square(position).tileObjects.GetObject(ToggEngine.TileObjectType.Lootdrop);
            if (loot != null)
            {
                loot.Loot(unit.hq());
            }
        }

        
    }
}
