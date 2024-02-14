using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.LootFest
{
    class BoundManager
    {
        Dictionary<int, GameObjectBoundData> bounds;

        public BoundManager()
        {
            LfRef.bounds = this;
            bounds = new Dictionary<int, GameObjectBoundData>();
            
            save(false);
        }

        public void CollectActive()
        {
            LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.Reset();
            while (LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.Next())
            {
                if (LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.sel.BoundSaveAccess != System.IO.FileShare.None)
                {
                    //if (!LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.Member.CollisionAndDefaultBound.createdFromBoundManager)
                    //{
                    GameObjectBoundData data = new GameObjectBoundData(LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.sel);
                    if (!bounds.ContainsKey(data.Key))
                    {
                        bounds.Add(data.Key, data);
                    }
                    //}
                }
            }
        }

        public void ToMenu(Gui menu)
        {
            CollectActive();

            GuiLayout layout = new GuiLayout("Bounds", menu);
            {
                foreach (var kv in bounds)
                {
                    new GuiTextButton(kv.Value.gameObjectType.ToString() + "(" + kv.Value.level.ToString() + ")",
                        null, new GuiAction2Arg<int, Gui>(boundLink, kv.Key, menu), true, layout);
                }
            }
            layout.End();
        }

        /// <summary>
        /// View all bounds for one object type 
        /// </summary>
        void boundLink(int boundKey, Gui menu)
        {
            var bound = bounds[boundKey];
            GuiLayout layout = new GuiLayout(bound.gameObjectType.ToString() + "(" + bound.level.ToString() + ")", menu);
            {
                bound.ToMenu(layout);
                new GuiTextButton("Copy from...", null, new GuiAction2Arg<int, Gui>(copyBound_ListBounds, bound.Key, menu), true, layout);
                new GuiDialogButton("Erase bound", "Will remove bound id from storage", new GuiActionIndex(removeId, bound.Key), false, layout);
                
            }
            layout.End();

            new RefreshBoundsUpdater(bound, menu);
        }

        void copyBound_ListBounds(int toId, Gui menu)
        {
            GuiLayout layout = new GuiLayout("Bounds", menu);
            {
                foreach (var kv in bounds)
                {
                    new GuiTextButton(kv.Value.gameObjectType.ToString() + "(" + kv.Value.level.ToString() + ")",
                        null, new GuiAction3Arg<int, int, Gui>(copyBound_SelectBound, toId, kv.Key, menu), true, layout);
                }
            }
            layout.End();
        }

        void copyBound_SelectBound(int toId, int fromId, Gui menu)
        {
            bounds[fromId].CopyTo(bounds[toId]);

            boundLink(toId, menu);
        }

        void removeId(int id)
        {
            bounds.Remove(id);
        }

        public void save(bool save)
        {
            var path = new DataStream.FilePath(null, "ObjectBounds", ".set", true, false);
            if (PlatformSettings.RunningXbox ||
                (!save && !PlatformSettings.DevBuild))
            {
                //Load from content
                path.Storage = false;
                path.LocalDirectoryPath = "Lootfest";
            }
            DataStream.BeginReadWrite.BinaryIO(save, path, write, read, null, save);
        }

        void write(System.IO.BinaryWriter w)
        {
            w.Write(bounds.Count);
            foreach (var kv in bounds)
            {
                kv.Value.write(w);
            }
        }

        void read(System.IO.BinaryReader r)
        {
            int length = r.ReadInt32();
            for (int i = 0; i < length; ++i)
            {
                var data = new GameObjectBoundData(r);
                bounds.Add(data.Key, data);
            }
        }

        public void LoadBound(GO.AbsUpdateObj go)
        {
            int key = GameObjectBoundData.GoKey(go.Type, go.BoundLevel);

            GameObjectBoundData bound;
            if (!bounds.TryGetValue(key, out bound))
            {
                bound = new GameObjectBoundData(go);
            }

            bound.applyBounds(go);
        }
    }

    class RefreshBoundsUpdater : AbsUpdateable
    {
        int menuId;
        Gui menu;
        GameObjectBoundData bound;

        public RefreshBoundsUpdater(GameObjectBoundData bound, Gui menu)
            :base(true)
        {
            this.bound = bound;
            this.menu = menu;
            menuId = menu.PageId;
        }

        public override void Time_Update(float time)
        {
            LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.Reset();
            while (LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.Next())
            {
                var go = LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.sel;
                //if (go is GO.PlayerCharacter.WolfHero)
                //{
                //    lib.DoNothing();
                //}
                if (GameObjectBoundData.GoKey(go.Type, go.BoundLevel) == bound.Key)
                {
                    bound.applyBounds(go);
                }
            }

            if (menu.IsDeleted || menu.PageId != menuId)
            {
                DeleteMe();
                LfRef.bounds.save(true);
            }
        }
    }

    class GameObjectBoundData
    {
        public GO.GameObjectType gameObjectType;
        public int level;

        List<BoundSaveData> terrainBounds = null;
        List<BoundSaveData> collisionBounds = null;
        List<BoundSaveData> damageBounds = null;

        public GameObjectBoundData(GO.AbsUpdateObj go)
        {
            gameObjectType = go.Type;
            level = go.BoundLevel;

            if (go.CollisionAndDefaultBound == null)
            {
                go.CollisionAndDefaultBound = ObjSingleBound.QuickBoundingBox(1f);
                Debug.LogError("Missing default bound: " + go.ToString());
            }
            collisionBounds = GetData(go.CollisionAndDefaultBound, go);
            terrainBounds = GetData(go.TerrainInteractBound, go);
            damageBounds = GetData(go.DamageBound, go);
        }

        public GameObjectBoundData(System.IO.BinaryReader r)
        {
            read(r);
        }


        public void CopyTo(GameObjectBoundData toBound)
        {
            if (terrainBounds == null)
            {
                toBound.terrainBounds = null;
            }
            else
            {
                toBound.terrainBounds = new List<BoundSaveData>(terrainBounds.Count);
                foreach (var b in terrainBounds)
                {
                    toBound.terrainBounds.Add(b.Clone());
                }
            }

            if (collisionBounds == null)
            {
                toBound.collisionBounds = null;
            }
            else
            {
                toBound.collisionBounds = new List<BoundSaveData>(collisionBounds.Count);
                foreach (var b in collisionBounds)
                {
                    toBound.collisionBounds.Add(b.Clone());
                }
            }

            if (damageBounds == null)
            {
                toBound.damageBounds = null;
            }
            else
            {
                toBound.damageBounds = new List<BoundSaveData>(damageBounds.Count);
                foreach (var b in damageBounds)
                {
                    toBound.damageBounds.Add(b.Clone());
                }
            }
        }

        public void ToMenu(GuiLayout layout)
        {
            dataListToMenu(GoBoundType.Terrain, terrainBounds, layout);
            new GuiSectionSeparator(layout);
            dataListToMenu(GoBoundType.Collision, collisionBounds, layout);
            new GuiSectionSeparator(layout);
            dataListToMenu(GoBoundType.Damage, damageBounds, layout);
        }

        void dataListToMenu(GoBoundType type, List<BoundSaveData> bounds, GuiLayout layout)
        {
            string name = type.ToString() + " bounds";
            if (bounds == null)
            {
                new GuiLabel(name + " - NULL", layout);
            }
            else
            {
                new GuiLabel(name, layout);
                for (int i = 0; i < bounds.Count; ++i)
                {
                    new GuiTextButton("Delete (" + i.ToString() + ")", null, 
                        new GuiAction2Arg<List<BoundSaveData>, int>(deleteBound, bounds, i), 
                        false, layout);
                    bounds[i].ToMenu(layout);
                }
            }
            new GuiTextButton("Add new", null, new GuiAction1Arg<GoBoundType>(addNewBound, type), false, layout);
        }

        void deleteBound(List<BoundSaveData> bounds, int index)
        {
            if (bounds.Count <= 1)
            {
                bounds = null;
            }
            else
            {
                bounds.RemoveAt(index);
            }
        }

        void addNewBound(GoBoundType type)
        {
            BoundSaveData newBound = new BoundSaveData();

            var bound = getBound(type);
            if (bound == null)
            {
                setBound(type, new List<BoundSaveData>{ newBound });
            }
            else
            {
                bound.Add(newBound);
            }
        }

        List<BoundSaveData> getBound(GoBoundType type)
        {
            switch (type)
            {
                default:
                    return collisionBounds;
                case GoBoundType.Terrain:
                    return terrainBounds;
                case GoBoundType.Damage:
                    return damageBounds;
            }
        }

        void setBound(GoBoundType type, List<BoundSaveData> bound)
        {
            switch (type)
            {
                default:
                    collisionBounds = bound;
                    break;
                case GoBoundType.Terrain:
                    terrainBounds = bound;
                    break;
                case GoBoundType.Damage:
                    damageBounds = bound;
                    break;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((int)gameObjectType);
            w.Write(level);
            writeBoundList(terrainBounds, w);
            writeBoundList(collisionBounds, w);
            writeBoundList(damageBounds, w);
        }
        void read(System.IO.BinaryReader r)
        {
            gameObjectType = (GO.GameObjectType)r.ReadInt32();
            //if (gameObjectType > GO.GameObjectType.BigOrcBoss)
            //{
            //    gameObjectType += 100 - 48;
            //}
            level = r.ReadInt32();
            terrainBounds = readBoundList(r);
            collisionBounds = readBoundList(r);
            damageBounds = readBoundList(r);

            //if (gameObjectType == GO.GameObjectType.ScorpionBot)
            //{
            //    BoundSaveData save = collisionBounds[2];
            //    collisionBounds.RemoveAt(2);
            //    collisionBounds.Insert(0, save);
            //}
        }


        void writeBoundList(List<BoundSaveData> bounds, System.IO.BinaryWriter w)
        {
            if (bounds == null)
            {
                w.Write(0);
            }
            else
            {
                w.Write(bounds.Count);
                foreach (BoundSaveData d in bounds)
                {
                    d.write(w);
                }
            }
        }

        List<BoundSaveData> readBoundList(System.IO.BinaryReader r)
        {
            int length = r.ReadInt32();
            if (length <= 0)
            {
                return null;
            }
            else
            {
                var result = new List<BoundSaveData>(length);
                for (int i = 0; i < length; ++i)
                {
                    result.Add(new BoundSaveData(r));
                }
                return result;
            }
        }

        static List<BoundSaveData> GetData(GO.Bounds.ObjectBound bound, GO.AbsUpdateObj go)
        {
            if (bound == null)
                return null;

            //var bounds = bound.Bounds;
            List<BoundSaveData> result = new List<BoundSaveData>(bound.Bounds.Length);
            foreach (var b in bound.Bounds)
            {
                result.Add(new BoundSaveData(b, go.Scale1D));
            }

            return result;
        }

        public void applyBounds(GO.AbsUpdateObj go)
        {
            refreshObjectBound(ref go.TerrainInteractBound, terrainBounds, go, Color.Yellow);
            refreshObjectBound(ref go.CollisionAndDefaultBound, collisionBounds, go, Color.White);
            refreshObjectBound(ref go.DamageBound, damageBounds, go, Color.Red);
        }

        void refreshObjectBound(ref GO.Bounds.ObjectBound bound, List<BoundSaveData> saveData, GO.AbsUpdateObj go, Color color)
        {
            if (saveData != null)
            {
                if (bound != null)
                { bound.DeleteMe(); }
                bound = GO.Bounds.ObjectBound.FromSaveData(saveData, go);
            }

            if (bound != null)
            {
                bound.UpdatePosition2(go);
                if (PlatformSettings.ViewCollisionBounds)
                    bound.DebugBoundColor(color);
            }
        }
        public int Key
        {
            get { return GoKey(gameObjectType, level); }
        }

        public static int GoKey(GO.GameObjectType type, int level)
        {
            return (int)type + level * 1000;
        }

        public override string ToString()
        {
            return "BoundData (" + gameObjectType.ToString() + level.ToString() + ")";
        }
    }

    class BoundSaveData
    {
        public GO.Bounds.BoundShape type;
        public Vector3 scale;
        public Vector3 offset;

        public BoundSaveData()
        {
            type = GO.Bounds.BoundShape.BoundingBox; //GO.Bounds.BoundShape.BoundingBox;
            scale = Vector3.One;
            offset = Vector3.Zero;
        }

        public BoundSaveData(GO.Bounds.BoundShape type, Vector3 scale, Vector3 offset)
        {
            this.type = type;
            this.scale = scale;
            this.offset = offset;
        }

        public BoundSaveData(GO.Bounds.AbsBound bound, float modelScale)
        {
            type = bound.Type;
            offset = bound.offset / modelScale;
            scale = bound.halfSize / modelScale;
        }

        public BoundSaveData(System.IO.BinaryReader r)
        {
            read(r);
        }

        public BoundSaveData Clone()
        {
            return new BoundSaveData(type, scale, offset);
        }

        static readonly IntervalF OffsetInterval = IntervalF.FromCenter(0, 14);
        static readonly IntervalF ScaleInterval = new IntervalF(0, 10);

        public void ToMenu(GuiLayout layout)
        {
            new GuiFloatSlider(SpriteName.NO_IMAGE, "Offset X", offsetX, OffsetInterval, false, layout);
            new GuiFloatSlider(SpriteName.NO_IMAGE, "Offset Y", offsetY, OffsetInterval, false, layout);
            new GuiFloatSlider(SpriteName.NO_IMAGE, "Offset Z", offsetZ, OffsetInterval, false, layout);

            new GuiFloatSlider(SpriteName.NO_IMAGE, "Scale X", scaleX, ScaleInterval, false, layout);
            new GuiFloatSlider(SpriteName.NO_IMAGE, "Scale Y", scaleY, ScaleInterval, false, layout);
            new GuiFloatSlider(SpriteName.NO_IMAGE, "Scale Z", scaleZ, ScaleInterval, false, layout);
            new GuiFloatSlider(SpriteName.NO_IMAGE, "Uni Scale ", scale1D, ScaleInterval, false, layout);

            new GuiTextButton("scale / 2", "half the scale", halfAllScale, false, layout);

            new GuiOptionsList<GO.Bounds.BoundShape>(SpriteName.NO_IMAGE, "Type", new List<GuiOption<GO.Bounds.BoundShape>>
            {
                new GuiOption<GO.Bounds.BoundShape>("Static Box", GO.Bounds.BoundShape.BoundingBox),
                new GuiOption<GO.Bounds.BoundShape>("Rot 1D Box", GO.Bounds.BoundShape.Box1axisRotation),
                new GuiOption<GO.Bounds.BoundShape>("Cylinder", GO.Bounds.BoundShape.Cylinder),

            }, typeProperty, layout);
        }

        void halfAllScale()
        {
            scale *= 0.5f;
        }

        GO.Bounds.BoundShape typeProperty(bool set, GO.Bounds.BoundShape value)
        {
            if (set)
            {
                type = value;
            }
            return type;
        }

        float offsetX(bool set, float value)
        {
            if (set)
            {
                offset.X = value;
            }
            return offset.X;
        }
        float offsetY(bool set, float value)
        {
            if (set)
            {
                offset.Y = value;
            }
            return offset.Y;
        }
        float offsetZ(bool set, float value)
        {
            if (set)
            {
                offset.Z = value;
            }
            return offset.Z;
        }

        float scaleX(bool set, float value)
        {
            if (set)
            {
                scale.X = value;
            }
            return scale.X;
        }
        float scaleY(bool set, float value)
        {
            if (set)
            {
                scale.Y = value;
            }
            return scale.Y;
        }
        float scaleZ(bool set, float value)
        {
            if (set)
            {
                scale.Z = value;
            }
            return scale.Z;
        }

        float scale1D(bool set, float value)
        {
            if (set)
            {
                scale = new Vector3(value);
            }
            return scale.X;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)type);
            SaveLib.WriteVector(w, scale);
            SaveLib.WriteVector(w, offset);
        }

        void read(System.IO.BinaryReader r)
        {
            type = (GO.Bounds.BoundShape)r.ReadByte();
            scale = SaveLib.ReadVector3(r);
            offset = SaveLib.ReadVector3(r);
        }
    }

    enum GoBoundType
    {
        Collision,
        Terrain,
        Damage,
    }
}
