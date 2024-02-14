using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.CarBall
{
    class CarImage
    {
        public static readonly SpriteSize CarSprite = new SpriteSize(30, 64);
        public Graphics.Image car, animalBody, animalHead, dirSign;

        public Vector2 position;
        public Vector2 carSize;
        public Rotation1D rotation;
        public bool turnRight = false;

        SpriteName forwardSprite, lSprite, rSprite;
        Vector2 bodyOffset;

        AnimalSetup animalSetup;

        public CarImage(float visualScale, GamerData gamerData, ImageLayers layer)
        {
            carSize = new Vector2(CarSprite.toImageSize(visualScale));
            car = new Graphics.Image(forwardSprite,
                new Vector2(Engine.Screen.SafeArea.X, Engine.Screen.SafeArea.Center.Y),
                carSize, layer, true);

            dirSign = new Graphics.Image(SpriteName.cballCarTurnArrow, Vector2.Zero, car.size, layer, true);
            dirSign.Opacity = 0.8f;
            dirSign.LayerAbove(car);

            animalBody = new Graphics.Image(SpriteName.NO_IMAGE, Vector2.Zero, new Vector2(visualScale * 1f), 
                ImageLayers.AbsoluteBottomLayer, true);
            animalBody.LayerAbove(dirSign);

            animalHead = new Graphics.Image(SpriteName.NO_IMAGE, Vector2.Zero, animalBody.size, 
                ImageLayers.AbsoluteBottomLayer, true);
            animalHead.LayerAbove(animalBody);

            bodyOffset.Y = visualScale * 0.2f;

            refreshVisuals(gamerData);
        }

        public void refreshVisuals(GamerData gamerData)
        {
            animalSetup = AnimalSetup.Get(gamerData.carAnimal);

            if (gamerData.LeftTeam)
            {
                forwardSprite = SpriteName.cballCarBlue;
                lSprite = SpriteName.cballCarBlueTurnL;
                rSprite = SpriteName.cballCarBlueTurnR;
            }
            else
            {
                forwardSprite = SpriteName.cballCarRed;
                lSprite = SpriteName.cballCarRedTurnL;
                rSprite = SpriteName.cballCarRedTurnR;
            }

            car.SetSpriteName(forwardSprite);

            animalBody.SetSpriteName(animalSetup.carBody);
            animalHead.SetSpriteName(animalSetup.carHead);
        }

        public void update()
        {
            car.position = position;
            car.Rotation = rotation.radians;

            dirSign.position = position;
            dirSign.Rotation = rotation.radians;
            

            animalBody.position = VectorExt.RotateVector(bodyOffset, rotation.radians, position);
            animalBody.Rotation = rotation.radians;

            animalHead.position = animalBody.position;
            animalHead.Rotation = rotation.radians;
        }

        public void setIsTurning(bool turning)
        {
            if (turning)
            {
                if (turnRight)
                {
                    car.SetSpriteName(rSprite);
                    animalBody.SetSpriteName(animalSetup.carBodyTurnR);
                }
                else
                {
                    car.SetSpriteName(lSprite);
                    animalBody.SetSpriteName(animalSetup.carBodyTurnL);
                }
            }
            else
            {
                car.SetSpriteName(forwardSprite);
                animalBody.SetSpriteName(animalSetup.carBody);
            }

            dirSign.spriteEffects = turnRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        public void setVisible(bool visible)
        {
            car.Visible = visible;
            dirSign.Visible = visible;

            animalBody.Visible = visible;
            animalHead.Visible = visible;
        }

        public void DeleteMe()
        {
            car.DeleteMe();
            dirSign.DeleteMe();

            animalBody.DeleteMe();
            animalHead.DeleteMe();
        }
    }
}
