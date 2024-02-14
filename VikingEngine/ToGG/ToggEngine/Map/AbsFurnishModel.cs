using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    abstract class AbsFurnishModel : ToggEngine.GO.AbsGameObject
    {
        protected const float Gravity = -0.001f;

        protected Vector3 speed;
        
        IntVector2 tile;
        protected Time animationTime;

        public AbsFurnishModel(IntVector2 tile, TileFurnishCollection coll)
            : base(false)
        {
            this.tile = tile;
            coll.models.Add(this);
        }

        virtual public void onStomp(IntVector2 from, bool local)
        {
            if (local)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqTileStomp, Network.PacketReliability.Reliable);
                toggRef.board.WritePosition(w, from);
                toggRef.board.WritePosition(w, tile);
            }
        }
    }

    class BoxModel : AbsFurnishModel
    {
               
        float rotationSpeed;

        public BoxModel(Vector3 pos, IntVector2 tile, TileFurnishCollection coll)
            : base(tile, coll)
        {
            createModel(Ref.rnd.Chance(0.6) ? SpriteName.FurnishBox : SpriteName.FurnishBarrel, pos, new Vector2(0.4f), false);

            //onStomp(IntVector2.Zero);
        }

        public override void onStomp(IntVector2 from, bool local)
        {
            base.onStomp(from, local);

            AddToUpdateList();

            bool moveRight = toggRef.board.toWorldPos_Center(from, 0).X < model.Position.X;

            speed.X = Ref.rnd.Float(0.001f, 0.003f) * lib.BoolToLeftRight(moveRight);
            speed.Y = Ref.rnd.Float(0.002f, 0.008f);

            rotationSpeed = Ref.rnd.Float(0.01f) * -lib.BoolToLeftRight(moveRight);

            animationTime.MilliSeconds = Ref.rnd.Int(120, 160);
        }

        public override void Time_Update(float time_ms)
        {
            if (model != null)
            {
                model.Position += speed * time_ms;
                model.Rotation.RotateAxis(new Vector3(rotationSpeed * time_ms, 0, 0));

                if (Ref.TimePassed16ms)
                {
                    speed.Y += Gravity;
                }

                if (animationTime.CountDown())
                {
                    model.Opacity -= 4f * Ref.DeltaTimeSec;
                    if (model.Opacity <= 0f)
                    {
                        DeleteObject();
                    }
                }
            }
        }
    }

    class CritterRatModel : AbsFurnishModel
    {
        public CritterRatModel(Vector3 pos, IntVector2 tile, TileFurnishCollection coll)
            : base(tile, coll)
        {
            createModel(SpriteName.FurnishRat, pos, new Vector2(0.4f), Ref.rnd.Bool());

            //onStomp(IntVector2.Zero, true);
        }

        public override void onStomp(IntVector2 from, bool local)
        {
            base.onStomp(from, local);

            AddToUpdateList();

            Vector2 diff = VectorExt.V3XZtoV2(model.Position) - toggRef.board.toWorldPosXZ_Center(from);

            Rotation1D runDir = Rotation1D.FromDirection(diff);
            runDir.Add(Ref.rnd.Plus_MinusF(0.1f));

            speed = VectorExt.V3FromXZ(runDir.Direction(Ref.rnd.Plus_MinusPercent(0.006f, 0.05f)), 0f);
            model.SetSpriteName(SpriteName.FurnishRatRunning);
            if (speed.X > 0f)
            {
                model.TextureSource.FlipX();
            }
        }
        public override void Time_Update(float time_ms)
        {
            if (model != null)
            {
                model.Position += speed * time_ms;

                var square = toggRef.board.square(model.Position);

                if (square == null || square.HasProperty(TerrainPropertyType.Impassable))
                {
                    DeleteObject();
                }                
            }
        }
    }

    class CritterSparrowModel : AbsFurnishModel
    {
        public CritterSparrowModel(Vector3 pos, IntVector2 tile, TileFurnishCollection coll)
            : base(tile, coll)
        {
            createModel(SpriteName.FurnishSparrow, pos, new Vector2(0.3f), Ref.rnd.Bool());

            //onStomp(IntVector2.Zero, true);
        }

        public override void onStomp(IntVector2 from, bool local)
        {
            base.onStomp(from, local);

            AddToUpdateList();

            Vector2 diff = VectorExt.V3XZtoV2(model.Position) - toggRef.board.toWorldPosXZ_Center(from);

            Rotation1D runDir = Rotation1D.FromDirection(diff);
            runDir.Add(Ref.rnd.Plus_MinusF(0.1f));

            speed = VectorExt.V3FromXZ(runDir.Direction(Ref.rnd.Plus_MinusPercent(0.006f, 0.05f)), 0f);
            speed.Y = Ref.rnd.Plus_MinusPercent(0.006f, 0.05f);
            model.SetSpriteName(SpriteName.FurnishSparrowFlying);
            if (speed.X < 0f)
            {
                model.TextureSource.FlipX();
            }

            animationTime.MilliSeconds = Ref.rnd.Plus_MinusPercent(600f, 0.05f);
        }
        public override void Time_Update(float time_ms)
        {
            if (model != null)
            {
                model.Position += speed * time_ms;

                if (animationTime.CountDown())
                {
                    model.Opacity -= 6f * Ref.DeltaTimeSec;
                    if (model.Opacity <= 0f)
                    {
                        DeleteObject();
                    }
                }
            }
        }
    }
}
