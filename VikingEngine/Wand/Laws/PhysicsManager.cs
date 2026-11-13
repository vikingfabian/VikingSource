using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.Wand.GO;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.Core.Wand.Laws
{
    static class PhysicsManager
    {
        public static void Explosion(IntVector2 pos, int damage)
        {
            ExplosiveDamage(pos, damage);
            foreach (var dir in IntVector2.Dir4Array)
            {
                IntVector2 npos = pos + dir;
                if (WandRef.world.map.CanMove_WallCheck(pos, npos, false, out _))
                {
                    ExplosiveDamage(npos, damage);
                }
            }
            foreach (var dir in IntVector2.AllDiagonalsArray)
            {
                IntVector2 npos = pos + dir;
                if (WandRef.world.map.CanMove_WallCheck_Diagonal(pos, npos))
                {
                    ExplosiveDamage(npos, damage);
                }
            }
        }

        static void ExplosiveDamage(IntVector2 pos, int damage)
        {
            Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Fire, VectorExt.AddY(WP.TileToWp(pos), 0.3f), 0.3f, 100);

            var creature = WandRef.world.map.tileGrid.Get(pos).creature;
            if (creature.HasGameObject())
            {
                creature.Get()?.TakeDamage(damage);
            }
        }

        public static void Bleed(IntVector2 pos)
        {
            Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.DssDamage, VectorExt.AddY(WP.TileToWp(pos), 0.3f), 0.3f, 100, 0.8f);
            DropFluid(pos, new Fluid(FluidType.Blood));
        }

        public static void DropFluid(IntVector2 pos, Fluid fluid)
        {
            WandRef.world.map.tileGrid.Get(pos).fluid = fluid;
            WandRef.PlayScene.mapmodel.decalsNeedsUpdate = true;
        }
    }
}
