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
        float scale;
        double randomFrame;

        public Foliage(LootFest.VoxelModelName modelName, PcgRandom rnd, Vector3 pos, float scale)
        {
            this.modelName = modelName;
            this.pos = pos;
            this.scale = scale;
            this.randomFrame = rnd.Double();
        }

        public void addToRender()
        {
            model = DssRef.models.ModelInstance(modelName, scale, false);
            model.Frame = (int)(randomFrame * model.NumFrames);
            
            model.AddToRender(DrawGame.UnitDetailLayer);
            model.position = pos;
        }

        public void DeleteMe()
        {
            model?.DeleteMe();
        }
    }
}
