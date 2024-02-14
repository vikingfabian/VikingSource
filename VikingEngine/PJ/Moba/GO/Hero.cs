using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.PJ.Moba.GO
{
    class Hero : AbsUnit
    {
        Graphics.Image horse = null;
        Vector2 horsePos;

        public Hero(LocalGamer gamer)
        {
            health = new ValueBar(100000);
            Vector2 pos = MobaRef.map.line.P1;
            blueTeam = true;
            initUnit(SpriteName.mobaPigBlue, pos, 2.8f, 1.1f, TimeExt.SecondsToMS(2f));
        }

        public void addHorse()
        {
            if (horse == null)
            {
                horse = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(0.8f, 0.2f) * image.Size, ImageLayers.AbsoluteBottomLayer, true);
                horse.Color = Color.RosyBrown;
                horsePos = new Vector2(0f, -image.Height * 0.2f);
                walkingSpeed = 2f;
            }
        }

        public void removeHorse()
        {
            if (horse != null)
            {
                horse.DeleteMe();
                walkingSpeed = 1f;
                horse = null;
            }
        }

        public override void Update()
        {
            base.Update();
            if (horse != null)
            {
                horse.Position = image.Position + horsePos;
            }
        }

        public override void updateDepth()
        {
            base.updateDepth();
            horse?.LayerBelow(image);
        }

        protected override void attack(AbsUnit target)
        {
            base.attack(target);
            removeHorse();
        }

        public override void takeDamage(int damage, AbsUnit fromAttacker)
        {
            base.takeDamage(damage, fromAttacker);
            removeHorse();
        }

        protected override int meleeDamage()
        {
            return 5;
        }
    }
}
