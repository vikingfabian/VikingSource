using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine
{
    abstract class AbsParticleEmitter
    {
        public Vector3 center;
        public AbsParticleEmitter(ToGG.ToggEngine.Map.GenerateBoardModelArgs args)
        {
            args.torches.Add(this);
        }

        public AbsParticleEmitter()
        {
            toggRef.board.model.content.emitters.Add(this);
        }
        abstract public void update();

        public void DeleteMe()
        {
            toggRef.board.model.content.emitters.Remove(this);
        }
    }

    class TorchFire : AbsParticleEmitter
    {
        public TorchFire(ToGG.ToggEngine.Map.GenerateBoardModelArgs args, Vector3 center)
            :base(args)
        {
            this.center = center;
        }

        public override void update()
        {
            Engine.ParticleHandler.AddParticles(ParticleSystemType.TorchFire, new ParticleInitData(Ref.rnd.Vector3_Sq(center, 0.02f)));
        }
    }
}
