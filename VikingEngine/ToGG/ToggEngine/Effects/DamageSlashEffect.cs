using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Effects
{
    class DamageSlashEffect: AbsUpdateable
    {
        const float ModelY = 0.5f;
        Graphics.Mesh model;

        Rotation1D angle = Rotation1D.D0;
        int update = 0;

        AbsUnit unit;
        IntVector2 source;
        public Time delay = Time.Zero;

        public DamageSlashEffect(AbsUnit unit, IntVector2 source)
            : base(true)
        {
            this.unit = unit;

            init(VectorExt.AddY(unit.soldierModel.Position, ModelY), source);
        }

        public DamageSlashEffect(AbsTileObject obj, IntVector2 source)
            : base(true)
        {
            init(toggRef.board.toModelCenter(obj.position, ModelY), source);
        }

        public DamageSlashEffect(IntVector2 target, IntVector2 source)
            : base(true)
        {
            init(toggRef.board.toModelCenter(target, ModelY), source);
        }

        void init(Vector3 center, IntVector2 source)
        {
            this.source = source;
            float scale = Ref.rnd.Plus_MinusPercent(0.014f, 0.06f);

            model = new Graphics.Mesh(LoadedMesh.plane, center,
                new Vector3(SpriteSheet.DamageSlashEffectSz.X, 1f, SpriteSheet.DamageSlashEffectSz.Y) * scale,
                Graphics.TextureEffectType.Flat, SpriteName.DamageSlashEffect1, Color.White);

            float tiltAngle = Ref.rnd.Float(0.05f, 1.7f) * Ref.rnd.LeftRight();
            model.Rotation = toggLib.PlaneTowardsCamWithRotation(tiltAngle);
        }

        public override void Time_Update(float time_ms)
        {
            if (delay.CountDown() && Ref.TimePassed16ms)
            {
                update++;

                if (update > 9)
                {
                    model.Opacity -= 0.2f;
                    if (model.Opacity <= 0)
                    {
                        DeleteMe();
                    }
                }
                else
                {
                    switch (update)
                    {
                        case 1:
                            model.SetSpriteName(SpriteName.DamageSlashEffect2);
                            break;
                        case 2:
                            model.SetSpriteName(SpriteName.DamageSlashEffect3);
                            break;
                        case 3:
                            model.SetSpriteName(SpriteName.DamageSlashEffect4);
                            if (unit != null && unit.Alive)
                            {
                                new Effects.BounceUnitAnim(unit, source.mirrorTilePos(unit.visualPos), 0.15f);
                            }
                            break;
                        case 4:
                            model.SetSpriteName(SpriteName.DamageSlashEffect5);
                            
                            break;
                        case 5:
                            model.SetSpriteName(SpriteName.DamageSlashEffect6);
                            
                            break;
                        case 9:
                            model.SetSpriteName(SpriteName.DamageSlashEffect7);
                            break;
                    }
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
