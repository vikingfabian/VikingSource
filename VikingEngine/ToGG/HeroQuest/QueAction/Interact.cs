using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class Interact : ToggEngine.QueAction.AbsQueAction
    {
        Unit unit;
        AbsTileObject interactObj;
        ToggEngine.Map.ActionReciever activationReciever;
        
        public Interact(AbsHQPlayer player, Unit unit, AbsTileObject interactObj)
            : base(player)
        {
            this.unit = unit;
            this.interactObj = interactObj;
        }

        public Interact(System.IO.BinaryReader r)
           : base(r)
        {
        }

        public override void onBegin()
        {
            //todo animate
            interactObj.interactEvent(unit);
            if (interactObj.InteractSettings.willEndMovement)
            {
                unit.hasEndedMovement = true;
            }

            if (interactObj.InteractSettings.interactType == InteractType.SendActivation)
            {
                if (toggRef.board.metaData.actionRecievers.TryGetValue(
                    interactObj.InteractId, out activationReciever))
                {
                    camTarget = activationReciever.recievers[0].position;
                    camTargetInCamCheck = false;
                }
                else
                {
                    camTarget = unit.squarePos;
                    activationReciever = null;
                }
            }
            else
            {
                state = ToggEngine.QueAction.QueState.Completed;
            }
        }

        public override bool update()
        {
            if (timeStamp.event_ms(400))
            {
                if (activationReciever == null)
                {
                    unit.textAnimation(SpriteName.NO_IMAGE, "Nothing happened");
                }
                else
                {
                    activationReciever.interactEvent();
                }
                return true;
            }

            return false;
        }

        public override bool CameraTarget(out IntVector2 camTarget, out bool inCamCheck)
        {
            base.CameraTarget(out camTarget, out inCamCheck);

            return interactObj.InteractSettings.interactType == InteractType.SendActivation;
        }

        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.Interact;

        public override bool IsPlayerQue => true;

        public override bool NetShared => interactObj.InteractSettings.netShared;

        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);

            unit.writeIndex(w);
            ToggEngine.TileObjLib.writeTileObj(w, interactObj);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            unit = Unit.NetReadUnitId(r);
            interactObj = ToggEngine.TileObjLib.readTileObj(r);

            //if (interactObj != null)
            //{

            //}
        }
    }
}
