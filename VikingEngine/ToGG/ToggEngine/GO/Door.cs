using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.ToggEngine.GO
{
    class Door : AbsTileObject
    {
        public bool leftToRightModel;
        Graphics.VoxelModel model, outlineModel;
        Graphics.VoxelModel floorMark, floorMarkDoor;
        public DoorSettings sett;
        Vector3 centerOffset;
        float ModelHeight;

        UpdateableFunction animationUpdate = null;

        public SpriteName sprite = SpriteName.hqRegularDoor;

        public Door(IntVector2 pos, object args)
            : base(pos)
        {
            if (args != null)
            {
                sett = (DoorSettings)args;
            }
            else
            {
                sett = new DoorSettings();
            }
        }
        
        public override void onLoadComplete()
        {
            base.onLoadComplete();

            leftToRightModel = toggRef.board.IsWall(VectorExt.AddY(position, -1)) && toggRef.board.IsWall(VectorExt.AddY(position, 1));

            const float ModelEdge = 0.2f;
            const float ModelWidth = 1f - ModelEdge * 2;
            ModelHeight = ModelWidth * 1.3f;

            if (leftToRightModel)
            {
                ModelHeight *= 0.8f;
            }

            const float Thickness = ModelWidth * 0.2f;

            
                
            Vector3 se = Vector3.Zero, sw, nw, ne;
            Vector3 edgeOffset;

            { //Door model
                List<PolygonColor> polygons = new List<PolygonColor>(6);

                var doorSprite = DataLib.SpriteCollection.Get(sprite);
                var doorSpriteMirror = doorSprite;
                doorSpriteMirror.FlipX();

                if (leftToRightModel)
                {
                    centerOffset.X += Thickness * 0.5f;
                    centerOffset.Z -= (0.5f - ModelEdge);

                    sw = VectorExt.AddZ(se, ModelWidth);
                    edgeOffset = new Vector3(-Thickness, 0, 0);
                }
                else
                {
                    centerOffset.X += ModelWidth * 0.5f;
                    centerOffset.Z += 0.2f;

                    se.Z = Thickness;
                    sw = VectorExt.AddX(se, -ModelWidth);
                    edgeOffset = new Vector3(0, 0, -Thickness);
                }
                nw = VectorExt.AddY(sw, ModelHeight);
                ne = VectorExt.AddY(se, ModelHeight);

                var front = new Graphics.PolygonColor(nw, ne, sw, se,
                    doorSpriteMirror, Color.White);
                polygons.Add(front);

                if (leftToRightModel)
                {
                    var back = new Graphics.PolygonColor(ne + edgeOffset, nw + edgeOffset, se + edgeOffset, sw + edgeOffset,
                        doorSprite, Color.White);
                    polygons.Add(back);
                }

                var topEdge = new Graphics.PolygonColor(nw + edgeOffset, ne + edgeOffset, nw, ne,
                    SpriteName.hqRegularDoorEdge, Dir4.N, Color.White);

                Color sideCol = Color.Gray;
                var leftEdge = new Graphics.PolygonColor(nw + edgeOffset, nw, sw + edgeOffset, sw,
                    SpriteName.hqRegularDoorEdge, Dir4.E, sideCol);

                var rightEdge = new Graphics.PolygonColor(ne, ne + edgeOffset, se, se + edgeOffset,
                    SpriteName.hqRegularDoorEdge, Dir4.E, sideCol);

                polygons.Add(topEdge);
                polygons.Add(leftEdge);
                polygons.Add(rightEdge);

                model = new Graphics.VoxelModel(false);
                model.Effect = toggLib.ModelEffect;
                model.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons, null),
                    new List<int> { polygons.Count }, LoadedTexture.SpriteSheet);

            }
            
            { //Floor model
                
                const float YOffset = 0.02f;

                var poly = new Graphics.PolygonColor(
                    VectorExt.AddY(sw + edgeOffset, YOffset),
                    VectorExt.AddY(se + edgeOffset, YOffset),
                    VectorExt.AddY(sw, YOffset),
                    VectorExt.AddY(se, YOffset),
                    SpriteName.hqDoorHoleOutLine, Dir4.N, Color.White);

                List<PolygonColor> polygons = new List<PolygonColor> { poly };

                floorMark = new Graphics.VoxelModel(true);
                floorMark.Effect = toggLib.ModelEffect;
                floorMark.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons, null),
                    new List<int> { polygons.Count }, LoadedTexture.SpriteSheet);
            }

            { //Floor Door model

                const float YOffset = 0.024f;

                var poly = new Graphics.PolygonColor(
                    VectorExt.AddY(sw + edgeOffset, YOffset),
                    VectorExt.AddY(se + edgeOffset, YOffset),
                    VectorExt.AddY(sw, YOffset),
                    VectorExt.AddY(se, YOffset),
                    SpriteName.hqDoorOutLine, Dir4.S, Color.White);

                List<PolygonColor> polygons = new List<PolygonColor> { poly };

                floorMarkDoor = new Graphics.VoxelModel(true);
                floorMarkDoor.Effect = toggLib.ModelEffect;
                floorMarkDoor.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons, null),
                    new List<int> { polygons.Count }, LoadedTexture.SpriteSheet);
            }


            {//Outline model
                
                const float OutlineWidth = ModelEdge + GenerateBoardModelArgs.WallSideLean * 0.5f;
                const float Depth = OutlineWidth;//Thickness * 1.4f;
                const float DepthOffSet = (Depth - Thickness) * 0.5f;
                float OutLineHeight;// = Height * 0.8f;
                               

                List<PolygonColor> polygons = new List<PolygonColor>(10);

                Vector3 frontnw, frontne, frontsw, frontse;
                Vector3 backOffset;
                Vector3 otherSideOffset;

                if (leftToRightModel)
                {
                    OutLineHeight = ModelHeight * 0.7f;

                    frontsw = VectorExt.AddX(sw, -(Depth - DepthOffSet));
                    frontse = VectorExt.AddZ(frontsw, OutlineWidth);

                    frontnw = VectorExt.AddY(frontsw, OutLineHeight);
                    frontne = VectorExt.AddY(frontse, OutLineHeight);

                    backOffset = VectorExt.V3FromX(Depth);
                    otherSideOffset = VectorExt.V3FromZ(-(ModelWidth + OutlineWidth));
                }
                else
                {
                    OutLineHeight = ModelHeight * 0.8f;

                    frontsw = VectorExt.AddZ(se, DepthOffSet);
                    frontse = VectorExt.AddX(frontsw, OutlineWidth);

                    frontnw = VectorExt.AddY(frontsw, OutLineHeight);
                    frontne = VectorExt.AddY(frontse, OutLineHeight);

                    backOffset = VectorExt.V3FromZ(-Depth);
                    otherSideOffset = VectorExt.V3FromX(-(ModelWidth + OutlineWidth));
                }

                outlineSidePolys(polygons, frontnw, frontne, frontsw, frontse, backOffset);
                outlineSidePolys(polygons, frontnw + otherSideOffset, frontne + otherSideOffset, 
                    frontsw + otherSideOffset, frontse + otherSideOffset, backOffset);

                outlineModel = new Graphics.VoxelModel(true);
                outlineModel.Effect = toggLib.ModelEffect;
                outlineModel.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons, null),
                    new List<int> { polygons.Count }, LoadedTexture.SpriteSheet);
            }
            model.AddToRender();

            newPosition(position);
        }

        void outlineSidePolys(List<PolygonColor> polygons, 
            Vector3 frontnw, Vector3 frontne, Vector3 frontsw, Vector3 frontse,
            Vector3 backOffset)
        {
            var tex = DataLib.SpriteCollection.Get(SpriteName.WhiteArea_LFtiles);
            Color sideGray = ColorExt.GrayScale(0.4f);
            Color sideTintGray = sideGray;
            sideTintGray.B += 20;
            Color topGray = ColorExt.GrayScale(0.3f);

            var front = new Graphics.PolygonColor(frontnw, frontne, frontsw, frontse,
                    tex, sideGray);
            polygons.Add(front);

            var back = new Graphics.PolygonColor(
                    frontne + backOffset, frontnw + backOffset,
                    frontse + backOffset, frontsw + backOffset,
                    tex, sideGray);
            polygons.Add(back);

            var top = new Graphics.PolygonColor(
                frontnw, frontnw + backOffset,
                frontne, frontne + backOffset,
                tex, topGray);
            polygons.Add(top);

            var lSide = new Graphics.PolygonColor(
                frontnw + backOffset, frontnw,
                frontsw + backOffset, frontsw,
                tex, sideTintGray);
            polygons.Add(lSide);

            var rSide = new Graphics.PolygonColor(
                frontne, frontne + backOffset,
                frontse, frontse + backOffset,
                tex, sideTintGray);
            polygons.Add(rSide);
        }

        public override void OnEvent(EventType eventType)
        {
            if (eventType == EventType.LevelProgress)
            {
                if (sett.type == DoorType.Rune &&
                    HeroQuest.hqRef.gamestate.levelProgress.runeKeys.Get(sett.lockId))
                {
                    unlock();
                }
            }

            base.OnEvent(eventType);
        }

        public override bool HasOverridingMoveRestriction(AbsUnit unit,
            out MovementRestrictionType restrictionType)
        {
            if (sett.openStatus == OpenStatus.Open)
            {
                restrictionType = MovementRestrictionType.NoRestrictions;
            }
            else if (canOpenDoor(unit))
            {
                if (isRevealed())
                {
                    restrictionType = MovementRestrictionType.NoRestrictions;
                }
                else
                {
                    restrictionType = MovementRestrictionType.MustStop;
                }
            }
            else
            {
                restrictionType = MovementRestrictionType.Impassable;
            }

            return restrictionType != MovementRestrictionType.NoRestrictions;
        }

        bool isRevealed()
        {
            if (leftToRightModel)
            {
                return revealed(VectorExt.AddX(position, -1)) &&
                    revealed(VectorExt.AddX(position, 1));
            }
            else
            {
                return revealed(VectorExt.AddY(position, -1)) &&
                    revealed(VectorExt.AddY(position, 1));
            }

            bool revealed(IntVector2 pos)
            {
                var sq = toggRef.Square(pos);
                if (sq != null)
                {
                    return !sq.hidden;
                }
                return true;
            }
        }

        public override ToggEngine.QueAction.AbsSquareAction collectSquareEnterAction(IntVector2 pos, AbsUnit unit, bool local)
        {
            if (sett.openStatus != OpenStatus.Open)
            {
                ToggEngine.QueAction.TileObjectActivation activate = new ToggEngine.QueAction.TileObjectActivation(pos, true, local, this);
                return activate;
            }
            else
            {
                return null;
            }
        }

        public override IDeleteable areaHoverGui()
        {
            if (sett.openStatus == OpenStatus.Open)
            {
                return null;
            }

            SpriteName icon = sett.openStatus == OpenStatus.Locked ? 
                DoorSettings.LockIcon(sett.type, sett.lockId) : SpriteName.birdUnLock;

            Graphics.Mesh plane = new Mesh(LoadedMesh.plane, 
                toggRef.board.toModelCenter(position, 0) + 
                ModelHeight * 1.2f * toggLib.TowardCamVector_Yis1,
                new Vector3(0.6f), TextureEffectType.Flat, icon, Color.White);
            plane.Z -= 0.3f;

            plane.Rotation = toggLib.PlaneTowardsCam;

            return plane;
        }

        public override void onMoveEnter(AbsUnit unit, bool local)
        {
            if (sett.openStatus != OpenStatus.Open)
            {
                openDoor();

                ToggEngine.TileObjLib.NetWriteEvent(this, TileObjEventType.MoveEnter);
            }
        }

        public override void netReadEvent(BinaryReader r, TileObjEventType eventType)
        {
            base.netReadEvent(r, eventType);

            openDoor();
        }

        void openDoor()
        {   
            beginOpenAnimation();
            new ToGG.ToggEngine.Map.RevealMap(position, true);
            //toggRef.board.soundFromSquare(position);
            HeroQuest.hqRef.events.SendToPlayers(EventType.DoorOpened, this);
        }

        void unlock()
        {
            if (sett.openStatus == OpenStatus.Locked)
            {
                sett.openStatus = OpenStatus.Closed;
            }
        }

        public override void createMoveStepIcon(ImageGroup icons)
        {
            Graphics.Mesh doorIcon = new Graphics.Mesh(LoadedMesh.plane,
                toggRef.board.toWorldPos_Center(position, 0.0f) + toggLib.TowardCamVector * 1f,
                new Vector3(0.2f),
                Graphics.TextureEffectType.Flat, SpriteName.hqRegularDoor, Color.White);
            doorIcon.Rotation = toggLib.PlaneTowardsCam;

            icons.Add(doorIcon);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);

            Vector3 center = toggRef.board.toWorldPos_Center(position, 0.05f);

            center += centerOffset;
            model.position = center;
            outlineModel.position = center;
            floorMark.position = center;
            floorMarkDoor.position = center;
        }

        float percentAnimated;

        public void beginOpenAnimation()
        {
            percentAnimated = 0;
            animationUpdate?.DeleteMe();
            animationUpdate = new UpdateableFunction(updateOpenClose);

            doorEndPosition();

            model.Visible = true;
            outlineModel.Visible = true;
            floorMarkDoor.Visible = true;
            floorMark.Visible = true;
            if (sett.openStatus == OpenStatus.Open)
            {
                sett.openStatus = OpenStatus.Closed;
            }
            else
            {
                sett.openStatus = OpenStatus.Open;
            }
            
        }

        void doorEndPosition()
        {
            model.Rotation = RotationQuarterion.Identity;
            if (sett.openStatus == OpenStatus.Open)
            {
                model.Rotation.RotateWorldX(openAngle());
                model.Opacity = 0;
                model.Visible = false;
            }
            else
            {
                model.Opacity = 1f;
                model.Visible = true;
            }

            outlineModel.Visible = model.Visible;
            floorMarkDoor.Visible = !model.Visible;
            floorMark.Visible = !model.Visible;

            updateOpacity();
            floorMarkDoor.Rotation = model.Rotation;
        }

        bool updateOpenClose()
        {
            float percAdd = Ref.DeltaTimeSec * 4f;

            percentAnimated += percAdd;
            if (percentAnimated >= 1f)
            {
                doorEndPosition();
                return true;
            }
            
            model.Rotation.RotateWorldX(percAdd * openAngle() * lib.BoolToLeftRight(IsOpen));
            floorMarkDoor.Rotation = model.Rotation;

            if (IsOpen)
            {
                model.Opacity = 1f - percentAnimated;
            }
            else
            {
                model.Opacity = percentAnimated;
            }
            updateOpacity();
            return false;
        }

        public bool IsOpen => sett.openStatus == OpenStatus.Open;

        public bool IsClosed => sett.openStatus != OpenStatus.Open;

        void updateOpacity()
        {
            outlineModel.Opacity = model.Opacity;

            floorMarkDoor.Opacity = (1f - model.Opacity) * 0.4f;
            floorMark.Opacity = floorMarkDoor.Opacity;
        }
        
        float openAngle()
        {
            int rotDir = leftToRightModel ? 1 : -1;
            return MathHelper.ToRadians(110) * rotDir;
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.portrait(ref position, sprite, sett.Name(), true, 0.6f);
            if (IsClosed)
            {
                if (canOpenDoor(null))
                {
                    card.propertyBox(ref position, new DoorClosedProperty());
                }
                else
                {
                    card.propertyBox(ref position, new DoorLockedProperty(sett));
                }
            }
        }

        bool canOpenDoor(AbsUnit unit)
        {
            if (unit == null || unit.hq().data.hero != null)
            {
                switch (sett.type)
                {
                    case DoorType.Regular:
                        return true;                    
                    
                    default:
                        return sett.openStatus != OpenStatus.Locked;
                }
            }
            return false;
        }

        public override void DeleteMe()
        {
            model?.DeleteMe();
            outlineModel?.DeleteMe();

            floorMark?.DeleteMe();
            floorMarkDoor?.DeleteMe();

            base.DeleteMe();
        }

        public override string ToString()
        {
            return sett.type.ToString() + " Door (id" + 
                sett.lockId.ToString() + ", " + sett.openStatus.ToString() + ")";
        }

        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);
            sett.Write(w);
        }
        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);
            sett.Read(r, version);
        }

        public override void interactEvent(AbsUnit unit)
        {
            unlock();
        }

        public override bool IsTileFillingUnit => true;

        override public TileObjectType Type { get { return TileObjectType.Door; } }
    }
    
    
    class DoorSettings
    {
        public OpenStatus openStatus = OpenStatus.Closed;
        public DoorType type = DoorType.Regular;
        public byte lockId = 0;

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((byte)openStatus);
            w.Write(lockId);
            w.Write((byte)type);            
        }

        public void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {   
            openStatus = (OpenStatus)r.ReadByte();
            lockId = r.ReadByte();
            
            type = (DoorType)r.ReadByte();
            
            checkLockStatus();
        }

        public void toEditorSettings(GuiLayout layout)
        {
            var typeOptions = new List<GuiOption<DoorType>>
            {
                new GuiOption<DoorType>(DoorType.Regular),
                new GuiOption<DoorType>(DoorType.Mechanical),
                new GuiOption<DoorType>(DoorType.Rune),
            };

            var openOptions = new List<GuiOption<OpenStatus>>
            {
                new GuiOption<OpenStatus>(OpenStatus.Locked),
                new GuiOption<OpenStatus>(OpenStatus.Closed),
                new GuiOption<OpenStatus>(OpenStatus.Open),
            };

            new GuiOptionsList<DoorType>(SpriteName.NO_IMAGE, "TYPE", typeOptions, typeProperty, layout);
            new GuiOptionsList<OpenStatus>(SpriteName.NO_IMAGE, "Status", openOptions, openStatusProperty, layout);
            new GuiIntSlider(SpriteName.NO_IMAGE, "Lock id", idProperty, new IntervalF(0, 3), false, layout);
        }

        DoorType typeProperty(bool set, DoorType value)
        {
            if (set)
            {
                type = value;

                if (type == DoorType.Regular)
                {
                    openStatus = OpenStatus.Closed;
                }
                else
                {
                    openStatus = OpenStatus.Locked;
                }
            }
            return type;
        }

        OpenStatus openStatusProperty(bool set, OpenStatus value)
        {
            if (set)
            {
                openStatus  = value;
                checkLockStatus();
            }
            return openStatus;
        }

        int idProperty(bool set, int value)
        {
            if (set)
            {
                lockId = (byte)value;
            }
            return lockId;
        }

        public static SpriteName LockIcon(DoorType doorType, int uType)
        {
            switch (doorType)
            {
                case DoorType.Mechanical:
                    return SpriteName.cmdLockMechanical;
                case DoorType.Rune:
                    switch ((HeroQuest.Gadgets.RuneKeyType)uType)
                    {
                        case HeroQuest.Gadgets.RuneKeyType.Hera:
                            return SpriteName.cmdLockRuneH;
                        case HeroQuest.Gadgets.RuneKeyType.Bast:
                            return SpriteName.cmdLockRuneB;
                        case HeroQuest.Gadgets.RuneKeyType.Froe:
                            return SpriteName.cmdLockRuneF;
                        case HeroQuest.Gadgets.RuneKeyType.Ami:
                            return SpriteName.cmdLockRuneA;

                        default:
                            return SpriteName.MissingImage;
                    }

                default:
                    return SpriteName.birdLock;
            }
        }

        public string Name()
        {
            return type.ToString() + " door";
        }

        public DoorSettings Clone()
        {
            DoorSettings clone = new DoorSettings();
            clone.openStatus = this.openStatus;
            clone.type = this.type;
            clone.lockId = this.lockId;

            return clone;
        }

        void checkLockStatus()
        {
            if (type == DoorType.Regular && 
                openStatus == OpenStatus.Locked)
            {
                openStatus = OpenStatus.Closed;
            }
        }

        public HeroQuest.Gadgets.RuneKeyType RuneLock
        {
            get { return (HeroQuest.Gadgets.RuneKeyType)lockId; }
        }
    }

    class DoorClosedProperty : AbsProperty
    {
        public DoorClosedProperty()
        {
        }

        public override string Name => "Closed";

        public override string Desc => "Place a unit on the door to open it";
    }

    class DoorLockedProperty : AbsProperty
    {
        DoorSettings settings;

        public DoorLockedProperty(DoorSettings settings)
        {
            this.settings = settings;
        }

        public override SpriteName Icon => DoorSettings.LockIcon(settings.type, settings.lockId);
       
        public override string Name => "Locked";

        public override string Desc
        {
            get
            {
                switch (settings.type)
                {
                    case DoorType.Rune:
                        return "Is locked with a " + settings.RuneLock.ToString() + " rune key";
                    case DoorType.Mechanical:
                        return "Is locked with some mechanical device";
                    default:
                        return TextLib.Error;
                }
            }
        }
    }

    enum OpenStatus
    {
        Locked,
        Closed,
        Open,
        NUM
    }

    enum DoorType
    {
        Regular,
        Mechanical, //Unlocked w switch
        KeyLock, //Any key
        Rune, //Special rune key 
        NUM,
    }

    

    /*
     * Four demi-godesses of protection
     * Looks like cats
     * Daughters of Proper
     * Hera
     * Bast
     * Froe
     * Ami
     */
}

