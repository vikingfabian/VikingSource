using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class LaunchCannon
    {
        const float MaxAngle = MathHelper.PiOver4;

        public Graphics.Image image;
        Rotation1D rotationCenter;
        public Rotation1D fireAngle;

        float rotationAdd;
        int rotationDir = 1;

        bool movingTowardsPos1 = false;

        public LaunchCannon()
        {
            Ref.draw.CurrentRenderLayer = Draw.ShadowObjLayer;

            GolfRef.objects.cannon = this;

            image = new Graphics.Image(SpriteName.bagCannon, 
                GolfRef.field.dataCollection.cannonPos1, 
                GolfRef.gamestate.BallScale * 0.6f * new Vector2(2f, 3f),
                ImageLayers.Lay3, true);
            image.origo = new Vector2(0.5f, 0.33f);

            rotationCenter = GolfRef.field.dataCollection.cannonDir;
            fireAngle = rotationCenter;
            image.Rotation = rotationCenter.Radians + MathHelper.Pi;

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        public void update()
        {
            if (GolfRef.field.dataCollection.movingCannon)
            {
                Vector2 goal = movingTowardsPos1 ? GolfRef.field.dataCollection.cannonPos1 : GolfRef.field.dataCollection.cannonPos2;
                Vector2 diff = goal - image.Position;

                float l;
                diff = VectorExt.Normalize(diff, out l);

                image.Position += Ref.DeltaTimeSec * GolfRef.field.squareSize.X * 3f * diff;

                if (l < GolfRef.field.squareSize.X * 0.2f)
                {
                    lib.Invert(ref movingTowardsPos1);
                }
            }
            else
            {
                rotationAdd += Ref.DeltaTimeSec * GolfLib.ClubAngleSpeed * 0.5f * rotationDir;

                if (Math.Abs(rotationAdd) >= MaxAngle)
                {
                    rotationAdd = MaxAngle * rotationDir;
                    lib.Invert(ref rotationDir);
                }

                fireAngle = rotationCenter;
                fireAngle.Add(rotationAdd);

                image.Rotation = fireAngle.Radians + MathHelper.Pi;
            }
        }

        public Vector2 Position
        {
            get { return image.Position + fireAngle.Direction(image.Size.Y * 0.4f); }
        }

        public void DeleteMe()
        {
            image.DeleteMe();
        }
    }
}
