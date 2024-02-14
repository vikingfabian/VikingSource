using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

//namespace VikingEngine.HUD
//{

//    class ExplainControlsGroup : VikingEngine.AbsInput
//    {
//        Vector2 position;
//        Dictionary<numBUTTON, ExplainButtonSlider> buttons = new Dictionary<numBUTTON, ExplainButtonSlider>();
//        List<ExplainStickSlider> sticks = new List<ExplainStickSlider>();
//        public int CurrentPositionIndex = 0;

//        public ExplainControlsGroup(Vector2 pos)
//        {

//            position = pos;
//        }

//        public void Add(numBUTTON button, string description)
//        {
//            buttons.Add(button, new ExplainButtonSlider(this, position, description, button));
//        }
//        public void Add(Stick stickIx, StickDirType dir, string description)
//        {
//            sticks.Add(new ExplainStickSlider(this, position, description, stickIx, dir));
//        }
//        public override void Button_Event(ButtonValue e)
//        {
//            if (buttons.ContainsKey(e.Button))
//            {
//                buttons[e.Button].Click(e.KeyDown, Vector2.Zero);
//            }
//        }
//        public override void Pad_Event(JoyStickValue e)
//        {
//            foreach (ExplainStickSlider obj in sticks)
//            {
//                if (obj.PadIx == e.Stick)
//                {
//                    obj.Click(false, e.Direction);
//                }
//            }
//        }
//        public void Remove(AbsExplainCtlrSlider obj)
//        {
//            foreach (ExplainStickSlider sobj in sticks)
//            {
//                sobj.MoveLeft(obj.PosIndex);
//            }
//            foreach (KeyValuePair<numBUTTON, ExplainButtonSlider> kv in buttons)
//            {
//                kv.Value.MoveLeft(obj.PosIndex);
//            }

//            if (obj.ButtonType)
//            {
//                buttons.Remove(((ExplainButtonSlider)obj).Button);
//            }
//            else
//            {
//                sticks.Remove((ExplainStickSlider)obj);
//            }
//        }
//        public void Clear()
//        {
//            foreach (ExplainStickSlider obj in sticks)
//            {
//                obj.DeleteMe();
//            }
//            sticks.Clear();
//            foreach (KeyValuePair<numBUTTON, ExplainButtonSlider> kv in buttons)
//            {
//                kv.Value.DeleteMe();
//            }
//            buttons.Clear();
//            CurrentPositionIndex = 0;
//        }
//    }

//    abstract class AbsExplainCtlrSlider : Update
//    {
//        const float ColWidth = 160;
//        float moveLeft = 0;   
//        int posIndex;
//        public int PosIndex
//        {
//            get { return posIndex; }
//        }
//        const float Fadetime = 400;
//        const float RemoveTime =  10000;
//        const float AutoRemoveTime = 40000;
//        float currentRemoveTime;// = float.MaxValue;
//        protected Image icon;
//        TextG text;
//        ExplainControlsGroup parent;

//        public AbsExplainCtlrSlider(ExplainControlsGroup parent, Vector2 startPos, string desc)
//            : base(true)
//        {
//            this.parent = parent;
//        }
        
//        static readonly Vector2 TextSize = VectorExt.V2(0.7f);
//        protected void createImage(SpriteName tile, Vector2 startPos, string desc)
//        {
//            posIndex = parent.CurrentPositionIndex;
//            parent.CurrentPositionIndex++;

//            const float IconSize = 64;
            
            
//            const float IconAddX = (ColWidth - IconSize) * PublicConstants.Half;

//            icon = new Image(tile, startPos, VectorExt.V2(IconSize), ImageLayers.Foreground4);
//            icon.Xpos += IconAddX + ColWidth * posIndex;
//            text = new TextG(LoadedFont.Lootfest, new Vector2(icon.Center.X, icon.Bottom + 10),
//               TextSize, Align.CenterAll, desc, Color.White, ImageLayers.Foreground4);

//            currentRemoveTime = AutoRemoveTime + 2000 * posIndex;
//        }
//        virtual public void Click(bool keyDown, Vector2 dir)
//        {
//            if (currentRemoveTime > RemoveTime)
//            {
//                this.AddToUpdateList(true);
//                currentRemoveTime = RemoveTime;
//            }
//        }
//        public override void Time_Update(float time)
//        {
//            currentRemoveTime -= time;
//            if (currentRemoveTime < Fadetime)
//            {
//                if (currentRemoveTime <= 0)
//                {
//                    DeleteMe();
//                    parent.Remove(this);
//                }
//                else
//                    setTransprantsy(currentRemoveTime / Fadetime);

//            }
//            if (moveLeft > 0)
//            {
//                const float MoveSpeed = -0.3f;
//                float move = MoveSpeed * time;
//                moveMe(move);
//                moveLeft += move;
//            }
//        }
//        virtual protected void moveMe(float move)
//        {
//            icon.Xpos += move;
//            text.Xpos += move;
//        }
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            icon.DeleteMe();
//            text.DeleteMe();
//        }
//        protected void setTransprantsy(float transparentsy)
//        {
//            icon.Transparentsy = transparentsy;
//            text.Transparentsy = transparentsy;
//        }
//        public void MoveLeft(int index)
//        {
//            if (this.posIndex > index)
//            {
//                posIndex--;
//                moveLeft += ColWidth;
//            }
//        }
//        abstract public bool ButtonType { get; }
//    }
//    class ExplainButtonSlider : AbsExplainCtlrSlider
//    {
//        numBUTTON button;
//        public numBUTTON Button
//        { get { return button; } }
//        public ExplainButtonSlider(ExplainControlsGroup parent, Vector2 startPos, string desc, numBUTTON button)
//            :base(parent, startPos, desc)
//        {
//            this.button = button;
//            SpriteName tile = SpriteName.NO_IMAGE;
//            switch (button)
//            {
//                case numBUTTON.A:
//                    tile = SpriteName.ControllerA;
//                    break;
//                case numBUTTON.B:
//                    tile = SpriteName.ControllerB;
//                    break;
//                case numBUTTON.X:
//                    tile = SpriteName.ControllerX;
//                    break;
//                case numBUTTON.Y:
//                    tile = SpriteName.ControllerY;
//                    break;
//                case numBUTTON.LB:
//                    tile = SpriteName.ControllerLB;
//                    break;
//                case numBUTTON.RB:
//                    tile = SpriteName.ControllerRB;
//                    break;
//                case numBUTTON.LT:
//                    tile = SpriteName.ControllerLT;
//                    break;
//                case numBUTTON.RT:
//                    tile = SpriteName.ControllerRT;
//                    break;
//                case numBUTTON.Start:
//                    tile = SpriteName.ControllerStart;
//                    break;
//                case numBUTTON.Back:
//                    tile = SpriteName.ControllerBack;
//                    break;
//            }
//            createImage(tile, startPos, desc);
//        }
//        public override void Click(bool keyDown, Vector2 dir)
//        {
//            base.Click(keyDown, dir);
//            icon.Position += VectorExt.V2(lib.BoolToDirection(keyDown) * 2);
//            icon.Color = keyDown ? Color.Gray : Color.White;
//        }
//        override public bool ButtonType { get { return true; } }
//    }
//    class ExplainStickSlider : AbsExplainCtlrSlider
//    {
//        Stick padIx;
//        public Stick PadIx
//        {
//            get { return padIx; }
//        }
//        //StickDirType dirType;
//        Vector2 iconPos;

//        public ExplainStickSlider(ExplainControlsGroup parent, Vector2 startPos, string desc, Stick padIx, StickDirType dirType)
//            : base(parent, startPos, desc)
//        {
//            this.padIx = padIx;
//            SpriteName tile = SpriteName.NO_IMAGE;
//            switch (padIx)
//            {
//                case Stick.Left:
//                    tile = SpriteName.ControllerLeftStick;
//                    break;
//                case Stick.Right:
//                    tile = SpriteName.ControllerRightStick;
//                    break;
//                case Stick.D:
//                    tile = SpriteName.ControllerDpadUp;
//                    break;

//            }
//            createImage(tile, startPos, desc);
//            iconPos = icon.Position;
//        }
//        public override void Click(bool keyDown, Vector2 dir)
//        {
//            base.Click(keyDown, dir);
//            icon.Position = iconPos + dir * 4;
//        }
//        protected override void moveMe(float move)
//        {
//            base.moveMe(move);
//            iconPos.X += move;
//        }
//        override public bool ButtonType { get { return false; } }
                
//    }

//    enum StickDirType
//    {
//        AllDir,
//        UpDown,
//        LeftRight,
//    }
//}
