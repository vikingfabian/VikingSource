using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsModelAttachment_Batched : AbsModelAttachment
    {
        public override void DeleteMe()
        {
            model.preRemoveFromDrawBatch();
        }
    }

    abstract class AbsModelAttachment
    {
        protected Graphics.AbsVoxelObj model;
        protected Vector3 diff;

        virtual public void update(AbsSoldierUnit parent)
        {
            var parentModel = parent.model;
            if (parentModel != null)
            {
                model.Rotation = parentModel.model.Rotation;
                model.position = parentModel.model.Rotation.TranslateAlongAxis(
                    diff, parentModel.model.position);
            }
        }

        public void onNewModel_asynch(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            DSSWars.Faction.SetNewMaster(name, modelName(), model, master);
        }

        virtual public void DeleteMe()
        {
            model.DeleteMe();
        }

        abstract protected LootFest.VoxelModelName modelName();
    }
}
