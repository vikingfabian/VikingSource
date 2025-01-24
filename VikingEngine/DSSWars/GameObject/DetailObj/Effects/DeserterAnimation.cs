using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class DeserterAnimation : AbsInGameUpdateable
    {
        Graphics.VoxelModelInstance model;
        WalkingAnimation walkingAnimation;
        Vector3 movedir;
        float lifeTime = 1500;
        public DeserterAnimation(AbsSoldierUnit soldier, Vector3 movedir, RotationQuarterion rotation)
            :base(true)
        {
            lifeTime = Ref.rnd.Float(3000f, 5000f);
            walkingAnimation = WalkingAnimation.Standard;
            walkingAnimation.randomStartFrame();
            this.movedir = movedir;
            model = DssRef.models.ModelInstance(LootFest.VoxelModelName.wars_deserter, true,
                DssConst.Men_StandardModelScale,
                true);

            model.position = soldier.position;
            model.Rotation = rotation;
        }

        public override void Time_Update(float time_ms)
        {
            float speed = DssConst.Men_StandardWalkingSpeed * 1.2f * time_ms;
            model.position += speed * movedir ;

            walkingAnimation.update(speed, model);

            lifeTime -= time_ms;
            if (lifeTime < 0)
            {
                this.DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            DssRef.models.recycle(ref model, true);
            base.DeleteMe();
        }
    }
}
