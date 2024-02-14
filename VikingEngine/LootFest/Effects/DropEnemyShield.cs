using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.LootFest.Effects
{
    class DropEnemyShield : AbsInGameUpdateable
    {
        const float Gravity = AbsPhysics.StandardGravity * 2f;

        Graphics.AbsVoxelObj model;
        Velocity v;
        Effects.BouncingBlockColors damageColors;
        Vector3 rotation;

        public DropEnemyShield(Graphics.AbsVoxelObj model, Effects.BouncingBlockColors damageColors)
            :base(true)
        {
            this.model = model;
            this.damageColors = damageColors;
            v.Set(Rotation1D.Random(), Ref.rnd.Float(0.001f, 0.0015f));
            v.Y = 0.02f + Ref.rnd.Plus_MinusF(0.002f);

            const float MaxRotationSpeed = 0.04f;
            rotation = new Vector3(
                0, Ref.rnd.Plus_MinusF(MaxRotationSpeed), 0); 
        }

        public override void Time_Update(float time)
        {
            
            if (Ref.TimePassed16ms)
            {
                v.Y += Gravity;
            }
            v.Update(Ref.DeltaTimeMs, model);
            model.Rotation.RotateAxis(rotation * Ref.DeltaTimeMs);

            Map.WorldPosition wp = new Map.WorldPosition(model.position);
            var c = wp.ScreenUnsafe;
            if (c != null)
            {
                if (wp.BlockHasMaterial())
                {
                    this.DeleteMe();
                    blockEffect();
                }
            }
            else
            {  //outside
                this.DeleteMe();
            }
        }

        private void blockEffect()
        {
            int numDummies = 1;
            int numBlocks = 2;
            if (Engine.Update.IsRunningSlow || Ref.gamesett.DetailLevel == 0)
            {
                numBlocks = 0;
            }
            else if (Ref.gamesett.DetailLevel == 2)
            {
                numBlocks += numDummies;
                numDummies = 0;
            }

            Vector3 pos = model.position;
            pos.Y += model.scale.Y * 8f;

            float scale = lib.SmallestValue(model.scale.X * 1.6f, 0.5f);
            for (int i = 0; i < numBlocks; i++)
            {
                new Effects.BouncingBlock2(pos, damageColors.GetRandom(), scale);
            }
            for (int i = 0; i < numDummies; i++)
            {
                new Effects.BouncingBlock2Dummie(pos, damageColors.GetRandom(), scale);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
