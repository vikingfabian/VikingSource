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
        double randomDouble;

        public Foliage(LootFest.VoxelModelName modelName, double randomDouble, Vector3 pos)
        {
            this.modelName = modelName;
            this.pos = pos;
            this.randomDouble = randomDouble;
        }

        public void addToRender()
        {
            model = DssRef.models.ModelInstance(modelName, 0.12f, false);
            model.Frame = (int)(randomDouble * model.NumFrames);
            
            model.AddToRender(DrawGame.UnitDetailLayer);
            model.position = pos;
        }

        public void DeleteMe()
        {
            model?.DeleteMe();
        }
    }
}
