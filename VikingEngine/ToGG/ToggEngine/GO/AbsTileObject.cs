using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.ToggEngine.GO
{
    abstract class AbsTileObject : IUnitCardDisplay
    {      
        public IntVector2 position;
        public bool hasLoadComplete = false;
       
        public InteractSettings InteractSettings;

        public AbsTileObject(IntVector2 pos)
        {
            this.position = pos;
            var onSquare = toggRef.board.tileGrid.Get(pos);
            
            onSquare.tileObjects.Add(this);
        }

        virtual public void newPosition(IntVector2 newpos)
        {
            this.position = newpos;
        }

        virtual public void onLoadComplete()
        {
        }

        virtual public void stompOnTile(AbsUnit unit, IntVector2 onSquare, bool soundOnly, bool local)
        { }

        virtual public void onMoveEnter(AbsUnit unit, bool local)
        { }

        virtual public void onMoveLeave(AbsUnit unit, bool local)
        { }
                
        virtual public void OnEvent(ToGG.Data.EventType eventType)
        { }

        virtual public void netReadEvent(System.IO.BinaryReader r, TileObjEventType eventType)
        { }

        virtual public IDeleteable areaHoverGui()
        {
            return null;
        }

        virtual public void createMoveStepIcon(Graphics.ImageGroup icons)
        {
            //Add non
        }

        protected static void DefaultMoveStepIcon(SpriteName sprite, IntVector2 position, ImageGroup icons, ref Graphics.Mesh model)
        {
            if (Graphics.GraphicsLib.InRender(model) == false)
            {
                model = new Graphics.Mesh(LoadedMesh.plane,
                    toggRef.board.toWorldPos_Center(position, 0.0f) + toggLib.TowardCamVector * 0.4f,
                    new Vector3(0.2f),
                    Graphics.TextureEffectType.Flat, sprite, Color.White);
                model.Rotation = toggLib.PlaneTowardsCam;

                icons.Add(model);
            }
        }

        virtual public bool IsLootable()
        { return true; }

        virtual public void Loot(HeroQuest.Unit unit)
        { }

        public void takeDamage()
        {
            new Effects.DamageSlashEffect(this, position);
            DeleteMe();
        }

        virtual public void DeleteMe()
        {
            if (toggRef.InRunningGame)
            {
                ToggEngine.TileObjLib.WriteTileObjRemove(this);
            }

            var tile = toggRef.board.tileGrid.Get(position);
            tile.tileObjects.members.Remove(this);
        }

        virtual public void Write(System.IO.BinaryWriter w)
        {
        }

        virtual public void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
        }

        virtual public bool HasOverridingMoveRestriction(AbsUnit unit, out ToGG.ToggEngine.Map.MovementRestrictionType restrictionType)
        {
            restrictionType = ToGG.ToggEngine.Map.MovementRestrictionType.NoRestrictions;
            return false;
        }

        virtual public ToggEngine.QueAction.AbsSquareAction collectSquareEnterAction(IntVector2 pos, AbsUnit unit, bool local)
        {
            return null;
        }

        virtual public ToggEngine.QueAction.AbsSquareAction collectSquareLeaveAction(IntVector2 pos, AbsUnit unit, bool local)
        {
            return null;
        }

        public override string ToString()
        {
            return "TileObj-" + Type.ToString();
        }
        
        virtual public void AddToUnitCard(ToGG.ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        { }

        virtual public bool canInteractWithObj(HeroQuest.Unit unit)
        {
            return false;
        }
        
        virtual public void interactEvent(AbsUnit unit)
        {
            throw new NotImplementedException();
        }

        virtual public List<AbsRichBoxMember> interactToolTip()
        {
            var members = new List<AbsRichBoxMember>{
                new RbBeginTitle(),
                new RbImage(InteractSettings.icon),
                new RbText(InteractSettings.text),
            };

            if (InteractSettings.addedDesc != null)
            {
                members.Add(new RbNewLine(true));
                members.AddRange(InteractSettings.addedDesc);
            }

            if (InteractSettings.willEndMovement)
            {
                members.Add(new RbNewLine(true));
                members.Add(new RbImage(SpriteName.cmdNoMove));
                members.Add(new RbText("Will end movement"));
            }
            return members;
        }

        virtual public bool IsCategory(TileObjCategory category)
        {
            return false;
        }

        virtual public int InteractId => InteractSettings.interactId;

        virtual public bool SaveToStorage { get { return true; } }

        virtual public bool IsTileFillingUnit => false;

        abstract public TileObjectType Type { get; }

        virtual public int ExpectedMoveDamage(AbsUnit unit)
        {
            return 0;
        }
    }

    struct InteractSettings
    {
        public SpriteName icon;
        public string text;
        public InteractType interactType;
        public bool willEndMovement;
        public int interactId;
        public bool activationState;
        public bool netShared;
        public List<AbsRichBoxMember> addedDesc;

        public InteractSettings(SpriteName icon, string text, 
            InteractType interactType, int interactId,
            bool willEndMovement, bool netShared)
        {
            this.icon = icon;
            this.text = text;
            this.interactType = interactType;
            this.interactId = interactId;
            this.willEndMovement = willEndMovement;
            activationState = false;
            this.netShared = netShared;
            addedDesc = null;
        }

        public void WriteInteractData(System.IO.BinaryWriter w)
        {
            w.Write((byte)interactId);
            w.Write(activationState);
        }

        public void ReadInteractData(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            interactId = r.ReadByte();
            activationState = r.ReadBoolean();
        }

        public string InteractStateString()
        {
            return "(Interact id:" + interactId.ToString() + ", " + TextLib.OnOff(activationState) + ")";
        }
    }

    enum TileObjEventType
    {
        MoveEnter,
        MoveLeave,
        Interact,
    }
}

