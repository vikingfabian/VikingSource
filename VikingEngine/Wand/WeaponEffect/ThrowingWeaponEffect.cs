using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.WeaponEffect
{
    class ThrowingWeaponEffect : AbsUpdateable
    {
        Mesh model;
        Vector3 from, to;

        float totalTimeMs;
        float time;

        float tween = 0;

        public ThrowingWeaponEffect(MapPlacement fromPos)
            :base(true)
        {
            from = WP.TileToWp(fromPos.tilePos);

            MapPlacement next = fromPos.ForwardPos();
            for (int i = 0; i < 100; ++i)
            {
                var forwardPos = next.ForwardPos();

                bool end = false;

                if (WandRef.world.map.tileGrid.TryGet(next.tilePos, out var tile))
                {
                    var thinWall = WandRef.world.map.GetWall(next.tilePos, forwardPos.tilePos);
                    if (tile.creature.HasGameObject())
                    {
                        tile.creature.Get().TakeDamage(Ref.rnd.Int(1, 3));
                        end = true;
                    }
                    else if (thinWall.wallType != ThinWall.None || tile.tileType != TileType.Floor)
                    {
                        end = true;
                    }
                }
                else
                {
                    end = true;
                }

                if (end)
                {
                    to = WP.TileToWp(next.tilePos);
                    break;
                }
                else
                {
                    next = forwardPos;
                }
            }

            float l = VectorExt.SideLength_XZ(from, to);

            from.Y = 0.3f;
            to.Y = from.Y;
            model = new Mesh(LoadedMesh.cube_repeating, from, new Vector3(0.1f, 0.02f, 0.5f), TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles,
                Color.Gray);
            WP.DirToQuaterion(model, fromPos.direction);

            totalTimeMs = l * 100;
        }
        public override void Time_Update(float time_ms)
        {
            time += time_ms;

            if (time >= totalTimeMs)
            {
                DeleteMe();
            }

            tween = time / totalTimeMs;
            model.position = MathExt.Lerp(from, to, tween);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
