using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class EventFlag : ToggEngine.GO.AbsTileObject
    {
        int id;
        Graphics.Text3DBillboard model;

        public EventFlag(IntVector2 pos, object args)
            : base(pos)
        {
            if (args != null)
            {
                id = (int)args;
            }

            if (toggRef.InEditor)
            {
                model = new Graphics.Text3DBillboard(LoadedFont.Console, "00000000", Color.Black, Color.LightBlue, Vector3.Zero,
                    0.2f, 1f, true);
                model.FaceCamera = false;
                refreshModel();
            }
            newPosition(pos);
        }

        public void refreshModel()
        {
            if (model != null)
            {
                model.TextString = "Event" + id.ToString();
            }
        }
        public override ToggEngine.QueAction.AbsSquareAction collectSquareEnterAction(IntVector2 pos, ToggEngine.GO.AbsUnit unit, bool local)
        {
            ToggEngine.QueAction.TileObjectActivation activate = new ToggEngine.QueAction.TileObjectActivation(pos, true, local, this);
            activate.DelayTime = 0;
            return activate;
        }

        public override void onMoveEnter(ToggEngine.GO.AbsUnit unit, bool local)
        {
            base.onMoveEnter(unit, local);

           HeroQuest.hqRef.setup.conditions.eventFlag(position, id, unit);
        }


        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);
            w.Write((byte)id);
        }

        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);
            id = r.ReadByte();

            refreshModel();
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            if (model != null)
            {
                model.Position = toggRef.board.toWorldPos_Center(position, 0.05f);
                model.Z += 0.2f;
            }
        }

        public override string ToString()
        {
            return "Event flag (" + id.ToString() + ")";
            //model.TextString;
        }

        public override void DeleteMe()
        {
            model?.DeleteMe();
            base.DeleteMe();
        }

        override public ToggEngine.TileObjectType Type { get { return ToggEngine.TileObjectType.EventFlag; } }
    }
}
