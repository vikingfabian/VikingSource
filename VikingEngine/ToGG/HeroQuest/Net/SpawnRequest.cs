using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.Net
{
    class SpawnUnitRequest : AbsNetRequest
    {
        Unit unit;
                

        public SpawnUnitRequest()
            :base()
        {
        }

        public SpawnUnitRequest(Unit unit, INetRequestReciever callback)
            :base()
        {
            this.callback = callback;
            this.unit = unit;
            requestPos = unit.squarePos;

            beginRequest();
        }

        public override void requestCallback(bool successful)
        {
            base.requestCallback(successful);

            if (!successful)
            {
                unit.DeleteMe();
            }
        }

        public override void write(BinaryWriter w)
        {
            toggRef.board.WritePosition(w, requestPos);
        }
        public override void read(BinaryReader r)
        {
            requestPos = toggRef.board.ReadPosition(r);
        }

        public override void writeSuccessfulAction(BinaryWriter w)
        {
            //hqRef.players.netWritePlayer(w, unit);
            //unit.writePlayerCollection(w);
            unit.writeCreate(w);
        }

        public override void readSuccessfulAction(BinaryReader r)
        {
            //var player = hqRef.players.netReadPlayer(r);
            //new Unit(r, DataStream.FileVersion.Max, player.hqUnits);
            Unit.ReadCreate(r);
        }

        public override bool isAvailable()
        {
            return toggRef.board.IsEmptyFloorSquare(requestPos, false);
        }

        override protected NetRequestType Type { get { return NetRequestType.SpawnUnit; } }
    }

    class SpawnTileObjRequest : AbsNetRequest
    {
        ToggEngine.TileObjectType type;
        //IntVector2 requestPos = IntVector2.NegativeOne;
        
        public SpawnTileObjRequest()
            : base()
        {
            lib.DoNothing();
        }

        public SpawnTileObjRequest(ToggEngine.TileObjectType type, IntVector2 pos, INetRequestReciever callback)
            : base()
        {
            this.callback = callback;
            //this.unit = unit;
            this.type = type;
            requestPos = pos;

            beginRequest();
        }

        public override void requestCallback(bool successful)
        {
            base.requestCallback(successful);

            if (successful)
            {
                ToggEngine.TileObjLib.CreateObject(type, requestPos, null, false);
            }
        }

        public override void write(BinaryWriter w)
        {
            //toggRef.board.WritePosition(w, requestPos);
        }
        public override void read(BinaryReader r)
        {
            //requestPos = toggRef.board.ReadPosition(r);
        }

        public override void writeSuccessfulAction(BinaryWriter w)
        {
            w.Write((byte)type);

            //hqRef.players.netWritePlayer(w, unit);
            //unit.Write(w);

        }

        public override void readSuccessfulAction(BinaryReader r)
        {
            type = (ToggEngine.TileObjectType)r.ReadByte();

           
            //var player = hqRef.players.netReadPlayer(r);
            //new Unit(r, DataStream.FileVersion.Max, player.hqUnits);
        }

        public override bool isAvailable()
        {
            return toggRef.board.IsEmptyFloorSquare(requestPos, false);
        }

        override protected NetRequestType Type { get { return NetRequestType.SpawnTileObject; } }
    }
}
