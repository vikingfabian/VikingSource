using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Map
{
    class Foliage
    {
        Graphics.AbsVoxelObj model;
        LootFest.VoxelModelName modelName;
        Vector3 pos;

        public Foliage(LootFest.VoxelModelName modelName, Vector3 pos)
        {
            this.modelName = modelName;
            this.pos = pos;
        }

        public void addToRender()
        {
            model = DssRef.models.ModelInstance(modelName, 0.12f, false);
            model.AddToRender(DrawGame.UnitDetailLayer);
            model.position = pos;
        }

        public void DeleteMe()
        {
            model?.DeleteMe();
        }
    }
}
