using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Graphics;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Map
{
    class FlagModel : FoliageModel
    {
        Faction faction;
        public void init(Faction faction, int frame, Vector3 pos, float scale)
        {
            this.faction = faction;
            this.pos = pos;
            this.scale = scale;
            this.setFrame = frame;
        }

        public override void addToRender()
        {
            model = faction.AutoLoadModelInstance(
                LootFest.VoxelModelName.wars_flag, scale, true);
            model.position = pos;
            model.Frame = setFrame;
        }

        public override void DeleteMe()
        {
            model?.DeleteMe();
        }
    }

    class FoliageModel
    {
        protected Graphics.VoxelModelInstance model;
        LootFest.VoxelModelName modelName;
        protected Vector3 pos;
        protected float scale;
        protected int setFrame = -1;
        double randomFrame = -1;

        public void init(LootFest.VoxelModelName modelName, PcgRandom rnd, Vector3 pos, float scale)
        {
            this.modelName = modelName;
            this.pos = pos;
            this.scale = scale;
            this.randomFrame = rnd.Double();
        }

        public void init(LootFest.VoxelModelName modelName, int frame, Vector3 pos, float scale)
        {
            this.modelName = modelName;
            this.pos = pos;
            this.scale = scale;
            this.setFrame = frame;
        }

        virtual public void addToRender()
        {
            model = DssRef.models.ModelInstance( modelName, true, scale, true);

            if (setFrame < 0)
            {
                model.Frame = (int)(randomFrame * model.NumFrames);
            }
            else
            {
                model.Frame = setFrame;
            }

            //model.AddToRender(DrawGame.UnitDetailLayer);
            model.position = pos;
        }

        virtual public void DeleteMe()
        {
            if (model != null)
            {
                //model.Visible = false;//.DeleteMe();
                DssRef.models.recycle(model, true); 
                model = null;
            }
        }
    }
}
