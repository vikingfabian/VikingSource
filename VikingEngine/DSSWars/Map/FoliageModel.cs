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
    //struct FlagModel //: FoliageModel
    //{
    //    FoliageModel modelData;
        
        

        

    //    public override void DeleteMe()
    //    {
    //        model?.preRemoveFromDrawBatch();
    //    }
    //}

    struct FoliageModel
    {
        public Graphics.VoxelModelInstance model; //object type
        public LootFest.VoxelModelName modelName;
        public Vector3 pos;
        public float scale;
        public int setFrame = -1;
        double randomFrame = -1;
        public Faction faction = null;

        /// <summary>
        /// For flags
        /// </summary>
        public FoliageModel(Faction faction, int frame, Vector3 pos, float scale)
        {
            this.faction = faction;
            this.pos = pos;
            this.scale = scale;
            this.setFrame = frame;
        }

        public FoliageModel(LootFest.VoxelModelName modelName, PcgRandom rnd, Vector3 pos, float scale)
        {
            this.modelName = modelName;
            this.pos = pos;
            this.scale = scale;
            this.randomFrame = Math.Min(rnd.Double(), rnd.Double());
        }

        public FoliageModel(LootFest.VoxelModelName modelName, int frame, Vector3 pos, float scale)
        {
            this.modelName = modelName;
            this.pos = pos;
            this.scale = scale;
            this.setFrame = frame;
        }

        public void addToRender()
        {
            if (faction == null)
            {
                model = DssRef.models.ModelInstance_drawbatch(modelName, scale);

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
            else
            {
                addToRender_flag();
            }
        }

        public void addToRender_flag()
        {
            if (faction.flagProfile != null)
            {
                model = faction.AutoLoadModelInstance_batched(
                    LootFest.VoxelModelName.wars_flag, scale);
                model.position = pos;
                model.Frame = setFrame;
            }

        }

        public void DeleteMe()
        {
            if (model != null)
            {
                model.preRemoveFromDrawBatch();
            }
        }
    }
}
