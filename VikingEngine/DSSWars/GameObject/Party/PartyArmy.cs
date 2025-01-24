using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.GameObject.Party
{
    class PartyArmy : Army
    {
        /// <summary>
        /// Untranied - veterans - war fatigue
        /// </summary>
        //public BalancedStatsMeter battleExperienceMeter;

        /// <summary>
        /// Restless - warm in the clothes - Worn out
        /// </summary>
        Graphics.VoxelModelInstance restBarModel;
        public Pan restMeter;
        public Pan actionPoints;

        public PartyArmy(Faction faction, IntVector2 startPosition)
            : base(faction, startPosition)
        {
            restMeter = new Pan(-0.9f);
            actionPoints.Value = 0f;            
        }

        public override void update()
        {
            base.update();

            if (DssRef.time.oneSecond)
            {
                actionPoints.Add(PartyLib.WeeklyArmyActionPoints);
            }
        }

        public override void updateModelsPosition()
        {
            base.updateModelsPosition();
            restBarModel.position = overviewBanner.position;
            restBarModel.position.Y += 1;
        }

        protected override void setInRenderState()
        {
            base.setInRenderState();

            if (inRender_detailLayer)
            {
                if (restBarModel == null)
                {
                    //y adj = 2f
                    restBarModel = DssRef.models.ModelInstance( LootFest.VoxelModelName.party_restbar, false,1f, true);
                    //restBarModel.AddToRender(DrawGame.TerrainLayer);
                }
            }
            else
            {
                if (restBarModel != null)
                {
                    //restBarModel.DeleteMe();
                    DssRef.models.recycle(ref restBarModel, false);
                }
            }
        }

        public void RoyalArmyStatus()
        {
            restMeter = new Pan(-0.1f);
            actionPoints.Value = 1f;
            
        }

        //public void Combine(ArmyStatus army2, float percentSz)
        //{
        //    float myPerc = 1f - percentSz;
        //    //battleExperienceMeter.Value.Value = battleExperienceMeter.Value.Value * myPerc + army2.battleExperienceMeter.Value.Value * percentSz;
        //    restMeter.Value.Value = restMeter.Value.Value * myPerc + army2.restMeter.Value.Value * percentSz;
        //    actionPoints.Value = actionPoints.Value * myPerc + army2.actionPoints.Value * percentSz;
        //}

        public void Write(System.IO.BinaryWriter w)
        {
            //w.Write(battleExperienceMeter.Value.ByteValue);
            w.Write(restMeter.ByteValue);
            w.Write(actionPoints.ByteValue);
        }
        public void Read(System.IO.BinaryReader r)
        {
            //battleExperienceMeter.Value.ByteValue = r.ReadByte();
            restMeter.ByteValue = r.ReadByte();
            actionPoints.ByteValue = r.ReadByte();
        }

        public void WriteMoveStatus(System.IO.BinaryWriter w)
        {
            w.Write(restMeter.ByteValue);
            w.Write(actionPoints.ByteValue);
        }
        public void ReadMoveStatus(System.IO.BinaryReader r)
        {
            restMeter.ByteValue = r.ReadByte();
            actionPoints.ByteValue = r.ReadByte();
        }

        public void RemoveNetAsynch()
        {
            //battleExperienceMeter.Value.ByteValue = battleExperienceMeter.Value.ByteValue;
            restMeter.ByteValue = restMeter.ByteValue;
            actionPoints.ByteValue = actionPoints.ByteValue;
        }

        //public override void DeleteMe(bool netShare)
        //{
        //    base.DeleteMe(netShare);
        //    if (restBarModel != null)
        //    {
        //        restBarModel.DeleteMe();
        //    }
        //}

        //public override void DeleteMe()
        //{
        //    base.DeleteMe();
        //    if (restBarModel != null)
        //    {
        //        restBarModel.DeleteMe();
        //    }
        //}
    }
}
