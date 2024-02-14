using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    class TankImage
    {
        public Vector2 position;
        
        public Rotation1D rotation;
        public bool turnRight = false;

        public Graphics.Image vehicle, animalBody, animalHead, dirSign;
        Vector2 bodyOffset;
        Vector2 bulletOffset;

        AnimalSetup animalSetup;
        Bullet bullet;

        public TankImage(float visualScale, GamerData gamerData, ImageLayers layer, out Physics.RectangleRotatedBound bound)
        {
            vehicle = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero,
                new Vector2(0.8f, 1f) * visualScale, layer, true);
            vehicle.Color = Color.DarkGray;

            dirSign = new Graphics.Image(SpriteName.cballCarTurnArrow, Vector2.Zero, 
                new Vector2(visualScale * 1.6f), layer, true);
            dirSign.Opacity = 0.8f;
            dirSign.LayerAbove(vehicle);

            animalBody = new Graphics.Image(SpriteName.NO_IMAGE, Vector2.Zero, new Vector2(visualScale * 1f),
                ImageLayers.AbsoluteBottomLayer, true);
            animalBody.LayerAbove(dirSign);

            animalHead = new Graphics.Image(SpriteName.NO_IMAGE, Vector2.Zero, animalBody.size,
                ImageLayers.AbsoluteBottomLayer, true);
            animalHead.LayerAbove(animalBody);

            bodyOffset.Y = visualScale * 0.2f;
            bulletOffset.Y = visualScale * -0.6f;

            refreshVisuals(gamerData);

            bound = new Physics.RectangleRotatedBound(vehicle.HalfSize);
        }

        public void refreshVisuals(GamerData gamerData)
        {
            animalSetup = AnimalSetup.Get(gamerData.carAnimal);
            
            animalBody.SetSpriteName(animalSetup.carBody);
            animalHead.SetSpriteName(animalSetup.carHead);
        }

        public void update()
        {
            vehicle.position = position;
            vehicle.Rotation = rotation.radians;

            dirSign.position = position;
            dirSign.Rotation = rotation.radians;

            animalBody.position = VectorExt.RotateVector(bodyOffset, rotation.radians, position);
            animalBody.Rotation = rotation.radians;

            animalHead.position = animalBody.position;
            animalHead.Rotation = rotation.radians;

            if (bullet != null)
            {
                updateBullet();
            }
        }

        void updateBullet()
        {
            bullet.tankUpdate(VectorExt.RotateVector(bulletOffset, rotation.radians, position));
        }

        public void setBullet(Bullet bullet)
        {
            this.bullet = bullet;
            if (bullet != null)
            {
                updateBullet();
            }
        }

        public void setIsTurning(bool turning)
        {
            if (turning)
            {
                if (turnRight)
                {
                    //car.SetSpriteName(rSprite);
                    animalBody.SetSpriteName(animalSetup.carBodyTurnR);
                }
                else
                {
                    //car.SetSpriteName(lSprite);
                    animalBody.SetSpriteName(animalSetup.carBodyTurnL);
                }
            }
            else
            {
                //car.SetSpriteName(forwardSprite);
                animalBody.SetSpriteName(animalSetup.carBody);
            }

            dirSign.spriteEffects = turnRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        public void setVisible(bool visible)
        {
            vehicle.Visible = visible;
            dirSign.Visible = visible;

            animalBody.Visible = visible;
            animalHead.Visible = visible;
        }

        public void DeleteMe()
        {
            vehicle.DeleteMe();
            dirSign.DeleteMe();

            animalBody.DeleteMe();
            animalHead.DeleteMe();
        }
    }
}
